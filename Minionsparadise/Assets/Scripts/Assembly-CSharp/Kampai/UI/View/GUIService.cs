namespace Kampai.UI.View
{
	public class GUIService : global::Kampai.UI.View.IGUIService
	{
		private sealed class GUICommand : global::Kampai.UI.View.IGUICommand
		{
			public global::Kampai.UI.View.GUIOperation operation { get; set; }

			public global::Kampai.UI.View.GUIPriority priority { get; private set; }

			public string prefab { get; private set; }

			public bool WorldCanvas { get; set; }

			public global::Kampai.UI.View.GUIArguments Args { get; set; }

			public string GUILabel { get; set; }

			public string skrimScreen { get; set; }

			public bool darkSkrim { get; set; }

			public float alphaAmt { get; set; }

			public global::Kampai.UI.View.SkrimBehavior skrimBehavior { get; set; }

			public bool disableSkrimButton { get; set; }

			public bool singleSkrimClose { get; set; }

			public bool genericPopupSkrim { get; set; }

			public global::Kampai.UI.View.ShouldShowPredicateDelegate ShouldShowPredicate { get; set; }

			public GUICommand(global::Kampai.UI.View.GUIOperation operation, string prefab, global::Kampai.Util.IKampaiLogger logger, string guiLabel)
			{
				this.operation = operation;
				priority = global::Kampai.UI.View.GUIPriority.Lowest;
				this.prefab = prefab;
				WorldCanvas = false;
				Args = new global::Kampai.UI.View.GUIArguments(logger);
				GUILabel = guiLabel;
				disableSkrimButton = false;
				singleSkrimClose = false;
				genericPopupSkrim = false;
			}

			public GUICommand(global::Kampai.UI.View.GUIOperation operation, global::Kampai.UI.View.GUIPriority priority, string prefab, global::Kampai.Util.IKampaiLogger logger)
			{
				this.operation = operation;
				this.priority = priority;
				this.prefab = prefab;
				WorldCanvas = false;
				Args = new global::Kampai.UI.View.GUIArguments(logger);
				GUILabel = prefab;
				disableSkrimButton = false;
				singleSkrimClose = false;
				genericPopupSkrim = false;
			}
		}

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GUIService") as global::Kampai.Util.IKampaiLogger;

		private global::UnityEngine.GameObject lastActiveInstance;

		private global::System.Collections.Generic.Dictionary<string, global::UnityEngine.GameObject> instances = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.GameObject>();

		private readonly global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AsyncOperation> asyncOperations = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AsyncOperation>();

		private global::System.Collections.Generic.Queue<global::Kampai.UI.View.IGUICommand> priorityQueue = new global::System.Collections.Generic.Queue<global::Kampai.UI.View.IGUICommand>();

		private global::Kampai.UI.View.GUIArguments overrides;

		[Inject(global::Kampai.Main.MainElement.UI_WORLDCANVAS)]
		public global::UnityEngine.GameObject worldCanvas { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.UI.View.GUIServiceQueueEmptySignal guiServiceQueueEmptySignal { get; set; }

		public global::Kampai.UI.View.IGUICommand BuildCommand(global::Kampai.UI.View.GUIOperation operation, string prefab)
		{
			return new global::Kampai.UI.View.GUIService.GUICommand(operation, prefab, logger, prefab);
		}

		public global::Kampai.UI.View.IGUICommand BuildCommand(global::Kampai.UI.View.GUIOperation operation, string prefab, string guiLabel)
		{
			return new global::Kampai.UI.View.GUIService.GUICommand(operation, prefab, logger, guiLabel);
		}

		public global::Kampai.UI.View.IGUICommand BuildCommand(global::Kampai.UI.View.GUIOperation operation, global::Kampai.UI.View.GUIPriority priority, string prefab)
		{
			return new global::Kampai.UI.View.GUIService.GUICommand(operation, priority, prefab, logger);
		}

		public global::UnityEngine.GameObject Execute(global::Kampai.UI.View.GUIOperation operation, string prefab)
		{
			global::Kampai.UI.View.IGUICommand command = ((global::Kampai.UI.View.IGUIService)this).BuildCommand(operation, prefab);
			return ((global::Kampai.UI.View.IGUIService)this).Execute(command);
		}

		public global::UnityEngine.GameObject Execute(global::Kampai.UI.View.GUIOperation operation, global::Kampai.UI.View.GUIPriority priority, string prefab)
		{
			global::Kampai.UI.View.IGUICommand command = ((global::Kampai.UI.View.IGUIService)this).BuildCommand(operation, priority, prefab);
			return ((global::Kampai.UI.View.IGUIService)this).Execute(command);
		}

		public global::UnityEngine.GameObject Execute(global::Kampai.UI.View.IGUICommand command)
		{
			if (command == null)
			{
				logger.Error("GUICommand is null");
				return null;
			}
			EnsureOverrides();
			global::Kampai.UI.View.GUIArguments args = command.Args;
			if (args != null)
			{
				args.AddArguments(overrides);
			}
			else if (overrides.Count > 0)
			{
				command.Args = overrides;
			}
			global::UnityEngine.GameObject result = null;
			switch (command.operation)
			{
			case global::Kampai.UI.View.GUIOperation.LoadStatic:
				result = LoadStatic(command);
				break;
			case global::Kampai.UI.View.GUIOperation.Load:
				result = Load(command);
				break;
			case global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance:
				result = CreateNewInstance(command);
				break;
			case global::Kampai.UI.View.GUIOperation.Unload:
				Unload(command);
				break;
			case global::Kampai.UI.View.GUIOperation.Queue:
				Queue(command);
				break;
			case global::Kampai.UI.View.GUIOperation.AsyncLoad:
				AsyncLoad(command);
				break;
			default:
				logger.Error("Invalid GUIOperation");
				break;
			}
			return result;
		}

		private void AsyncLoad(global::Kampai.UI.View.IGUICommand command)
		{
			CreateCommandSkrim(command, true);
			string commandPrefab = command.prefab;
			string commandPrefabLabel = command.GUILabel;
			string text = commandPrefab + ((!global::Kampai.Util.DeviceCapabilities.IsTablet()) ? "_Phone" : "_Tablet");
			if (global::Kampai.Util.KampaiResources.FileExists(text))
			{
				commandPrefab = text;
			}
			if (string.IsNullOrEmpty(commandPrefab))
			{
				logger.Error("GUISettings.Path is empty");
			}
			if (asyncOperations.ContainsKey(commandPrefabLabel))
			{
				global::UnityEngine.AsyncOperation asyncOperation = asyncOperations[commandPrefabLabel];
				if (asyncOperation != null && !asyncOperation.isDone)
				{
					return;
				}
				asyncOperations.Remove(commandPrefabLabel);
			}
			asyncOperations.Add(commandPrefabLabel, global::Kampai.Util.KampaiResources.LoadAsync(commandPrefab, routineRunner, delegate(global::UnityEngine.Object prefabObj)
			{
				if (!instances.ContainsKey(commandPrefabLabel))
				{
					global::UnityEngine.GameObject gameObject = InstantiateGameObject(commandPrefab, commandPrefabLabel, prefabObj, command);
					if (gameObject != null)
					{
						instances[commandPrefabLabel] = gameObject;
					}
					if (gameObject.activeInHierarchy)
					{
						lastActiveInstance = gameObject;
					}
				}
			}));
		}

		private global::UnityEngine.GameObject GetCachedObject(global::Kampai.UI.View.IGUICommand command)
		{
			string prefab = command.prefab;
			string gUILabel = command.GUILabel;
			if (instances.ContainsKey(gUILabel))
			{
				global::UnityEngine.GameObject gameObject = instances[gUILabel];
				if (gameObject != null)
				{
					if (gameObject.activeSelf)
					{
						routineRunner.StartCoroutine(Initialize(gameObject, command.Args, prefab, gUILabel));
						return instances[gUILabel];
					}
					instances.Remove(gUILabel);
					global::UnityEngine.Object.Destroy(gameObject);
				}
				else
				{
					instances.Remove(gUILabel);
				}
			}
			return null;
		}

		private global::UnityEngine.GameObject LoadStatic(global::Kampai.UI.View.IGUICommand command)
		{
			string prefab = command.prefab;
			string gUILabel = command.GUILabel;
			if (string.IsNullOrEmpty(prefab))
			{
				logger.Error("GUISettings.Path is empty");
				return null;
			}
			global::UnityEngine.GameObject cachedObject = GetCachedObject(command);
			if (cachedObject != null)
			{
				return cachedObject;
			}
			cachedObject = CreateNewInstance(command);
			if (cachedObject != null)
			{
				instances[gUILabel] = cachedObject;
			}
			return cachedObject;
		}

		private global::UnityEngine.GameObject CreateNewInstance(global::Kampai.UI.View.IGUICommand command)
		{
			string text = command.prefab;
			string gUILabel = command.GUILabel;
			string text2 = text + ((!global::Kampai.Util.DeviceCapabilities.IsTablet()) ? "_Phone" : "_Tablet");
			if (global::Kampai.Util.KampaiResources.FileExists(text2))
			{
				text = text2;
			}
			global::UnityEngine.GameObject prefab = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(text);
			return InstantiateGameObject(text, gUILabel, prefab, command);
		}

		private global::UnityEngine.GameObject InstantiateGameObject(string commandPrefab, string commandPrefabLabel, global::UnityEngine.Object prefab, global::Kampai.UI.View.IGUICommand command)
		{
			if (prefab == null)
			{
				logger.Error("Invalid GUISettings.Path: {0}", commandPrefab);
				return null;
			}
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(prefab) as global::UnityEngine.GameObject;
			if (gameObject == null)
			{
				logger.Error("Unable to create instance of {0}", commandPrefab);
				return null;
			}
			global::Kampai.UI.View.SkrimView component = gameObject.GetComponent<global::Kampai.UI.View.SkrimView>();
			if (component != null)
			{
				if (command.disableSkrimButton)
				{
					component.EnableSkrimButton(false);
				}
				component.singleSkrimClose = command.singleSkrimClose;
				component.genericPopupSkrim = command.genericPopupSkrim;
			}
			else
			{
				CreateCommandSkrim(command);
			}
			if (command.WorldCanvas)
			{
				gameObject.transform.parent = worldCanvas.transform;
			}
			else
			{
				gameObject.transform.SetParent(glassCanvas.transform, false);
			}
			global::UnityEngine.CanvasGroup canvasGroup = gameObject.AddComponent<global::UnityEngine.CanvasGroup>();
			canvasGroup.alpha = 0f;
			routineRunner.StartCoroutine(Initialize(gameObject, command.Args, commandPrefab, commandPrefabLabel));
			return gameObject;
		}

		private void CreateCommandSkrim(global::Kampai.UI.View.IGUICommand command, bool asyncLoad = false)
		{
			if (!string.IsNullOrEmpty(command.skrimScreen))
			{
				global::Kampai.UI.View.IGUICommand iGUICommand = BuildCommand((!asyncLoad) ? global::Kampai.UI.View.GUIOperation.Load : global::Kampai.UI.View.GUIOperation.AsyncLoad, "Skrim", command.skrimScreen);
				iGUICommand.singleSkrimClose = command.singleSkrimClose;
				iGUICommand.disableSkrimButton = command.disableSkrimButton;
				iGUICommand.genericPopupSkrim = command.genericPopupSkrim;
				global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
				args.Add(command.darkSkrim);
				args.Add(command.alphaAmt);
				args.Add(command.skrimBehavior);
				Execute(iGUICommand);
			}
		}

		private global::UnityEngine.GameObject Load(global::Kampai.UI.View.IGUICommand command)
		{
			CreateCommandSkrim(command);
			global::UnityEngine.GameObject gameObject = LoadStatic(command);
			if (gameObject == null)
			{
				return null;
			}
			if (gameObject.activeInHierarchy)
			{
				lastActiveInstance = gameObject;
			}
			return gameObject;
		}

		private void Unload(global::Kampai.UI.View.IGUICommand command)
		{
			string gUILabel = command.GUILabel;
			if (!instances.ContainsKey(gUILabel))
			{
				logger.Error("Unable to unload instance: {0}", gUILabel);
				return;
			}
			global::UnityEngine.GameObject gameObject = instances[gUILabel];
			if (gameObject == lastActiveInstance)
			{
				lastActiveInstance = null;
			}
			global::UnityEngine.Object.Destroy(gameObject);
			instances.Remove(gUILabel);
			gameObject = null;
			Next();
		}

		private void Queue(global::Kampai.UI.View.IGUICommand command)
		{
			if (instances.ContainsKey(command.GUILabel))
			{
				return;
			}
			foreach (global::Kampai.UI.View.IGUICommand item in priorityQueue)
			{
				if (item.GUILabel == command.GUILabel)
				{
					return;
				}
			}
			priorityQueue.Enqueue(command);
			Next();
		}

		private void Next()
		{
			if (lastActiveInstance != null)
			{
				return;
			}
			if (priorityQueue.Count == 0)
			{
				guiServiceQueueEmptySignal.Dispatch();
				return;
			}
			global::Kampai.UI.View.IGUICommand iGUICommand = priorityQueue.Dequeue();
			if (iGUICommand == null)
			{
				return;
			}
			global::Kampai.UI.View.GUIOperation operation = iGUICommand.operation;
			if (operation != global::Kampai.UI.View.GUIOperation.Queue)
			{
				logger.Error("Invalid operation on the queue: {0}", iGUICommand.operation);
				return;
			}
			if (iGUICommand.ShouldShowPredicate == null || iGUICommand.ShouldShowPredicate())
			{
				Load(iGUICommand);
			}
			Next();
		}

		private global::System.Collections.IEnumerator Initialize(global::UnityEngine.GameObject instance, global::Kampai.UI.View.GUIArguments args, string prefabName, string guiLabel)
		{
			for (int i = 0; i < 5; i++)
			{
				yield return null;
				if (tryInitializeMediator(instance, args, prefabName, guiLabel))
				{
					break;
				}
			}
			if (!(instance != null))
			{
				yield break;
			}
			global::UnityEngine.CanvasGroup canvasGroup = instance.GetComponent<global::UnityEngine.CanvasGroup>();
			if (canvasGroup != null)
			{
				if (!canvasGroup.blocksRaycasts)
				{
					canvasGroup.alpha = 1f;
				}
				else
				{
					global::UnityEngine.Object.DestroyImmediate(canvasGroup);
				}
			}
		}

		private bool tryInitializeMediator(global::UnityEngine.GameObject instance, global::Kampai.UI.View.GUIArguments args, string prefabName, string guiLabel)
		{
			if (instance == null)
			{
				return false;
			}
			global::Kampai.UI.View.KampaiMediator component = instance.GetComponent<global::Kampai.UI.View.KampaiMediator>();
			if (component != null)
			{
				component.PrefabName = prefabName;
				component.guiLabel = guiLabel;
				component.Initialize(args);
				return true;
			}
			global::strange.extensions.mediation.impl.EventMediator component2 = instance.GetComponent<global::strange.extensions.mediation.impl.EventMediator>();
			if (component2 != null)
			{
				return true;
			}
			return false;
		}

		private void EnsureOverrides()
		{
			if (overrides == null)
			{
				overrides = new global::Kampai.UI.View.GUIArguments(logger);
			}
		}

		public void AddToArguments(object arg)
		{
			EnsureOverrides();
			overrides.Add(arg);
		}

		public void RemoveFromArguments(global::System.Type arg)
		{
			EnsureOverrides();
			overrides.Remove(arg);
		}
	}
}
