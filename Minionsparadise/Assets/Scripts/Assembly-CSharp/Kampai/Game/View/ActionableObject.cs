namespace Kampai.Game.View
{
	using Kampai.Util;
	public abstract class ActionableObject : global::UnityEngine.MonoBehaviour, global::Kampai.Game.IDefinitionsHotSwapHandler, global::Kampai.Game.View.Actionable, global::Kampai.Game.View.Animatable, global::Kampai.Game.View.VisuallyEffective, global::Kampai.Game.View.Audible, global::Kampai.Util.Identifiable
	{
		protected global::Kampai.Util.KampaiQueue<global::Kampai.Game.View.KampaiAction> actionQueue = new global::Kampai.Util.KampaiQueue<global::Kampai.Game.View.KampaiAction>();

		protected global::Kampai.Game.View.KampaiAction shelvedAction;

		protected global::Kampai.Util.KampaiQueue<global::Kampai.Game.View.KampaiAction> shelvedQueue;

		protected global::System.Collections.Generic.List<global::UnityEngine.Animator> animators;

		protected global::UnityEngine.RuntimeAnimatorController defaultController;

		protected global::Kampai.Util.VFXScript vfxScript;

		protected global::Kampai.Game.TriggerVFXOnState vfxTrigger;

		protected global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.View.PropObject> activeProps;

		protected global::Kampai.Util.IKampaiLogger logger;

		[global::UnityEngine.SerializeField]
		protected bool debug;

		[global::UnityEngine.SerializeField]
		protected bool dumpQueue;

		private global::System.Collections.Generic.Dictionary<string, CustomFMOD_StudioEventEmitter> _audioEmitters;

		private bool initialized;

		internal bool gfxFaded;

		private bool sfxFaded;

		private global::Kampai.Game.View.KampaiAction fadeSfxAction;

		private global::UnityEngine.RuntimeAnimatorController currentRuntimeController;

		private global::Kampai.Util.Graphics.MaterialModifier materialModifier;

		[Inject]
		public global::Kampai.Common.Service.Audio.IFMODService fmodService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public int ID { get; set; }

		public int DefinitionID { get; protected set; }

		public global::Kampai.Game.View.KampaiAction currentAction { get; protected set; }

		public bool IsAnimating { get; protected set; }

		public bool IsInteractable { get; protected set; }

		public global::UnityEngine.Renderer[] objectRenderers { get; protected set; }

		protected global::UnityEngine.Collider[] colliders { get; set; }

		public CustomFMOD_StudioEventEmitter localAudioEmitter
		{
			get
			{
				return GetAudioEmitter("LocalAudio");
			}
		}

		public CustomFMOD_StudioEventEmitter GetAudioEmitter(string id)
		{
			if (_audioEmitters == null)
			{
				_audioEmitters = new global::System.Collections.Generic.Dictionary<string, CustomFMOD_StudioEventEmitter>();
			}
			if (!_audioEmitters.ContainsKey(id))
			{
				_audioEmitters.Add(id, global::Kampai.Util.Audio.GetAudioEmitter.Get(base.gameObject, id));
			}
			return _audioEmitters[id];
		}

		public virtual void OnDefinitionsHotSwap(global::Kampai.Game.IDefinitionService definitionService)
		{
			if (DefinitionID == 0)
			{
				return;
			}
			global::Kampai.Game.Definition definition = definitionService.Get(DefinitionID);
			if (definition != null && ID != 0 && playerService != null)
			{
				global::Kampai.Game.Instance byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Instance>(ID);
				if (byInstanceId != null)
				{
					byInstanceId.OnDefinitionHotSwap(definition);
				}
			}
		}

		internal virtual void Init()
		{
			objectRenderers = base.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
			colliders = base.gameObject.GetComponents<global::UnityEngine.Collider>();
			IsInteractable = true;
			vfxScript = GetComponent<global::Kampai.Util.VFXScript>();
			if (vfxScript != null)
			{
				vfxScript.Init();
				vfxScript.TriggerState("OnStop");
			}
			InitProps();
		}

		internal void InitProps()
		{
			activeProps = new global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.View.PropObject>();
		}

		private void DisableWarnings()
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					current.logWarnings = false;
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public virtual void Update()
		{
			if (animators != null && !initialized)
			{
				DisableWarnings();
				initialized = true;
				sfxFaded = false;
				if (animators.Count > 0)
				{
					currentRuntimeController = animators[0].runtimeAnimatorController;
				}
			}
			if (dumpQueue)
			{
				LogActions();
				dumpQueue = false;
			}
			HandleActions();
			UpdateVFX();
		}

		private void HandleActions()
		{
			HandleNextAction();
			while (currentAction != null && currentAction.IsInstant() && currentAction.Done)
			{
				HandleNextAction();
			}
		}

		private void HandleNextAction()
		{
			if (currentAction == null || currentAction.Done)
			{
				currentAction = DequeueNextAction();
				if (currentAction != null)
				{
					ExecuteAction(currentAction);
				}
				else if (shelvedQueue == null)
				{
					if (debug)
					{
						logger.Info("{0} IDLE", base.name);
					}
					Idle();
				}
			}
			if (currentAction != null)
			{
				if (debug)
				{
					logger.Info("{0} - {1}.Update()", base.name, currentAction.ToString());
				}
				currentAction.Update();
			}
		}

		private global::Kampai.Game.View.KampaiAction DequeueNextAction()
		{
			global::Kampai.Game.View.KampaiAction kampaiAction = null;
			while ((kampaiAction == null || kampaiAction.Done) && actionQueue.Count > 0)
			{
				kampaiAction = actionQueue.Dequeue();
			}
			return kampaiAction;
		}

		public virtual void LateUpdate()
		{
			if (currentAction != null && !currentAction.Done)
			{
				if (debug)
				{
					logger.Info("{0} - {1}.LateUpdate()", base.name, currentAction.ToString());
				}
				currentAction.LateUpdate();
			}
		}

		public virtual void Idle()
		{
			currentAction = null;
		}

		public virtual void EnqueueAction(global::Kampai.Game.View.KampaiAction action, bool clear = false)
		{
			if (debug)
			{
				logger.Info("{0} - EnqueueAction({1})", base.name, action.ToString());
			}
			if (clear)
			{
				ClearActionQueue();
			}
			actionQueue.Enqueue(action);
		}

		public void Abort<T>() where T : global::Kampai.Game.View.KampaiAction
		{
			global::System.Collections.Generic.LinkedList<global::Kampai.Game.View.KampaiAction> linkedList = new global::System.Collections.Generic.LinkedList<global::Kampai.Game.View.KampaiAction>();
			global::System.Type typeFromHandle = typeof(T);
			if (currentAction is T)
			{
				linkedList.AddLast(currentAction);
				currentAction = null;
			}
			using (global::System.Collections.Generic.IEnumerator<global::Kampai.Game.View.KampaiAction> enumerator = actionQueue.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetType().IsAssignableFrom(typeFromHandle))
					{
						linkedList.AddLast(enumerator.Current);
					}
				}
			}
			using (global::System.Collections.Generic.LinkedList<global::Kampai.Game.View.KampaiAction>.Enumerator enumerator2 = linkedList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.Abort();
					actionQueue.Remove(enumerator2.Current);
				}
			}
		}

		public virtual void InjectAction(global::Kampai.Game.View.KampaiAction action)
		{
			if (debug)
			{
				logger.Info("InjectAction({0})", action.ToString());
			}
			actionQueue.AddFirst(action);
		}

		public virtual void ReplaceCurrentAction(global::Kampai.Game.View.KampaiAction action)
		{
			if (debug)
			{
				logger.Info("ReplaceCurrentAction: {0} -> {1}", currentAction.GetType(), action.ToString());
			}
			InjectAction(action);
			currentAction.Abort();
		}

		public virtual void ReplaceActionsOfType(global::Kampai.Game.View.KampaiAction action)
		{
			if (debug)
			{
				logger.Info("ReplaceActionsOfType: {0} -> {1}", currentAction.GetType(), action.ToString());
			}
			global::System.Type type = action.GetType();
			foreach (global::Kampai.Game.View.KampaiAction item in actionQueue)
			{
				if (type.IsAssignableFrom(item.GetType()))
				{
					item.Abort();
				}
			}
			if (currentAction != null && type.IsAssignableFrom(currentAction.GetType()))
			{
				currentAction.Abort();
			}
			InjectAction(action);
		}

		public virtual void ExecuteAction(global::Kampai.Game.View.KampaiAction action)
		{
			if (debug)
			{
				logger.Info("{0} - {1}.Execute()", base.name, action.ToString());
			}
			if (action != null)
			{
				action.Execute();
			}
		}

		public virtual int GetActionQueueCount()
		{
			return actionQueue.Count;
		}

		public virtual void ClearActionQueue()
		{
			if (debug)
			{
				logger.Info("{0} - ClearActionQueue {1}", base.name, actionQueue.Count);
			}
			if (currentAction != null)
			{
				currentAction.Abort();
				currentAction = null;
			}
			global::System.Collections.Generic.IEnumerator<global::Kampai.Game.View.KampaiAction> enumerator = actionQueue.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Abort();
				}
				actionQueue.Clear();
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public global::Kampai.Game.View.KampaiAction GetNextAction()
		{
			if (actionQueue.Count > 0)
			{
				return actionQueue.Peek();
			}
			return null;
		}

		public virtual T GetAction<T>() where T : global::Kampai.Game.View.KampaiAction
		{
			global::System.Collections.Generic.IEnumerator<global::Kampai.Game.View.KampaiAction> enumerator = actionQueue.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::Kampai.Game.View.KampaiAction current = enumerator.Current;
					T val = current as T;
					if (val != null)
					{
						return val;
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			return (T)null;
		}

		public virtual void ShelveActionQueue()
		{
			if (debug)
			{
				logger.Info("ShelveActionQueue");
			}
			shelvedQueue = actionQueue;
			shelvedAction = currentAction;
			currentAction = null;
			actionQueue = new global::Kampai.Util.KampaiQueue<global::Kampai.Game.View.KampaiAction>();
		}

		public virtual void UnshelveActionQueue()
		{
			if (debug)
			{
				logger.Info("UnshelveActionQueue");
			}
			if (shelvedQueue != null)
			{
				ClearActionQueue();
				actionQueue = shelvedQueue;
				currentAction = shelvedAction;
				shelvedQueue = null;
				shelvedAction = null;
			}
		}

		public void LogActions()
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Debug, "{0} {1} - currentAction:{2}", ID, base.name, (currentAction != null) ? currentAction.ToString() : "null");
			foreach (global::Kampai.Game.View.KampaiAction item in actionQueue)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Debug, "{0} {1} - nextAction:{2}", ID, base.name, item.ToString());
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Debug, "{0} {1} - shelvedAction:{2}", ID, base.name, (shelvedAction != null) ? shelvedAction.ToString() : "null");
			if (shelvedQueue == null)
			{
				return;
			}
			foreach (global::Kampai.Game.View.KampaiAction item2 in shelvedQueue)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Debug, "{0} {1} - nextShelved:{2}", ID, base.name, item2.GetType());
			}
		}

		public virtual void PlayAnimation(int stateHash, int layer, float time)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>(activeProps.Keys);
					foreach (string item in list)
					{
						RemoveProp(item);
					}
					global::UnityEngine.Animator current2 = enumerator.Current;
					current2.Play(stateHash, layer, time);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public virtual void ResetAnimationParameters()
		{
			global::UnityEngine.Animator animator = animators[0];
			global::UnityEngine.AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			float normalizedTime = currentAnimatorStateInfo.normalizedTime;
			animator.Rebind();
			animator.Play(currentAnimatorStateInfo.fullPathHash, 0, normalizedTime);
		}

		public virtual void AddProp(string propName, global::UnityEngine.GameObject parent)
		{
			if (activeProps.ContainsKey(propName))
			{
				return;
			}
			global::Kampai.Game.View.PropObject propObject = new global::Kampai.Game.View.PropObject();
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(propName);
			if (!(gameObject == null))
			{
				propObject.gameObject = global::UnityEngine.Object.Instantiate(gameObject);
				global::UnityEngine.Transform parent2 = base.transform.Find("minion:ROOT");
				global::UnityEngine.Transform transform = propObject.gameObject.transform.Find("minion:ROOT/minion:R_prop");
				if (transform != null)
				{
					transform.parent = parent2;
					propObject.transforms.Add(transform);
				}
				global::UnityEngine.Transform transform2 = propObject.gameObject.transform.Find("minion:ROOT/minion:L_prop");
				if (transform2 != null)
				{
					transform2.parent = parent2;
					propObject.transforms.Add(transform2);
				}
				propObject.gameObject.transform.parent = base.transform;
				activeProps.Add(propName, propObject);
				ResetAnimationParameters();
			}
		}

		public virtual void RemoveProp(string propName)
		{
			global::Kampai.Game.View.PropObject value = null;
			if (!activeProps.TryGetValue(propName, out value))
			{
				return;
			}
			activeProps.Remove(propName);
			global::UnityEngine.Object.Destroy(value.gameObject);
			foreach (global::UnityEngine.Transform transform in value.transforms)
			{
				transform.parent = value.gameObject.transform;
				global::UnityEngine.Object.Destroy(transform);
			}
			global::UnityEngine.Object.Destroy(value);
			ResetAnimationParameters();
		}

		public virtual global::UnityEngine.AnimatorStateInfo? GetAnimatorStateInfo(int layer)
		{
			if (animators.Count > 0 && !animators[0].IsInTransition(layer))
			{
				return animators[0].GetCurrentAnimatorStateInfo(layer);
			}
			return null;
		}

		public virtual bool IsInAnimatorState(int mecanimStateHash, int layer = 0)
		{
			global::UnityEngine.AnimatorStateInfo? animatorStateInfo = GetAnimatorStateInfo(layer);
			if (animatorStateInfo.HasValue)
			{
				int fullPathHash = animatorStateInfo.Value.fullPathHash;
				if (fullPathHash == mecanimStateHash)
				{
					return true;
				}
			}
			return false;
		}

		public virtual void SetAnimTrigger(string name)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					if (current.IsReady() && current.HasParameter(name))
					{
						current.SetTrigger(name);
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public virtual void ResetAnimTrigger(string name)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					current.ResetTrigger(name);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public virtual void SetAnimBool(string name, bool state)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					if (current.IsReady() && current.HasParameter(name))
					{
						current.SetBool(name, state);
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public virtual void SetAnimFloat(string name, float state)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					if (current.IsReady() && current.HasParameter(name))
					{
						current.SetFloat(name, state);
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public virtual void SetAnimInteger(string name, int state)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					if (current.IsReady() && current.HasParameter(name))
					{
						current.SetInteger(name, state);
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public bool GetAnimBool(string name)
		{
			if (animators.Count > 0)
			{
				return animators[0].GetBool(name);
			}
			logger.Error("Error retrieving bool {0} from {1}", name, base.gameObject.name);
			return false;
		}

		public float GetAnimFloat(string name)
		{
			if (animators.Count > 0)
			{
				return animators[0].GetFloat(name);
			}
			logger.Error("Error retrieving float {0} from {1}", name, base.gameObject.name);
			return 0f;
		}

		public int GetAnimInteger(string name)
		{
			if (animators.Count > 0)
			{
				return animators[0].GetInteger(name);
			}
			logger.Error("Error retrieving integer {0} from {1}", name, base.gameObject.name);
			return 0;
		}

		public virtual void SetDefaultAnimController(global::UnityEngine.RuntimeAnimatorController controller)
		{
			defaultController = controller;
		}

		public string GetDefaultAnimControllerName()
		{
			return defaultController.name;
		}

		public string GetCurrentAnimControllerName()
		{
			if (animators.Count > 0)
			{
				return animators[0].runtimeAnimatorController.name;
			}
			return string.Empty;
		}

		public virtual void SetAnimController(global::UnityEngine.RuntimeAnimatorController controller)
		{
			if (animators.Count == 0 || GetCurrentAnimController() == controller)
			{
				return;
			}
			using (global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator())
			{
				currentRuntimeController = controller;
				global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>(activeProps.Keys);
				foreach (string item in list)
				{
					RemoveProp(item);
				}
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current2 = enumerator.Current;
					current2.runtimeAnimatorController = controller;
				}
			}
			vfxTrigger = null;
		}

		public virtual global::UnityEngine.RuntimeAnimatorController GetCurrentAnimController()
		{
			return currentRuntimeController;
		}

		public virtual void ClearAnimators()
		{
			animators.Clear();
		}

		public virtual void SetAnimatorCullingMode(global::UnityEngine.AnimatorCullingMode mode)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					current.cullingMode = mode;
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public virtual void SetAnimatorsActive(bool active)
		{
			if (animators == null)
			{
				return;
			}
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					current.enabled = active;
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public void ApplyRootMotion(bool enabled)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current = enumerator.Current;
					current.applyRootMotion = enabled;
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public virtual void SetRenderLayer(int layer)
		{
			base.gameObject.SetLayerRecursively(layer);
		}

		public bool getMuteStatus()
		{
			global::Kampai.Game.View.AnimEventHandler[] componentsInChildren = base.transform.gameObject.GetComponentsInChildren<global::Kampai.Game.View.AnimEventHandler>();
			if (componentsInChildren.Length > 0)
			{
				return componentsInChildren[0].mute;
			}
			return false;
		}

		private CustomFMOD_StudioEventEmitter GetEmitter(string emitterName)
		{
			CustomFMOD_StudioEventEmitter[] components = base.transform.gameObject.GetComponents<CustomFMOD_StudioEventEmitter>();
			CustomFMOD_StudioEventEmitter result = null;
			CustomFMOD_StudioEventEmitter[] array = components;
			foreach (CustomFMOD_StudioEventEmitter customFMOD_StudioEventEmitter in array)
			{
				if (customFMOD_StudioEventEmitter.id == emitterName)
				{
					result = customFMOD_StudioEventEmitter;
				}
			}
			return result;
		}

		public void StartLocalAudio(string path)
		{
			if (!getMuteStatus())
			{
				CustomFMOD_StudioEventEmitter customFMOD_StudioEventEmitter = GetEmitter("minionPersistentAudio");
				if (customFMOD_StudioEventEmitter == null)
				{
					customFMOD_StudioEventEmitter = base.transform.gameObject.AddComponent<CustomFMOD_StudioEventEmitter>();
					customFMOD_StudioEventEmitter.id = "minionPersistentAudio";
					customFMOD_StudioEventEmitter.staticSound = false;
				}
				else
				{
					customFMOD_StudioEventEmitter.Stop();
				}
				if (fmodService != null)
				{
					customFMOD_StudioEventEmitter.path = fmodService.GetGuid(path);
					customFMOD_StudioEventEmitter.Play();
				}
			}
		}

		public void StopLocalAudio()
		{
			if (!getMuteStatus())
			{
				CustomFMOD_StudioEventEmitter emitter = GetEmitter("minionPersistentAudio");
				if (!(emitter == null))
				{
					emitter.Stop();
					global::UnityEngine.Object.Destroy(emitter);
				}
			}
		}

		public virtual void TrackVFX(global::Kampai.Util.VFXScript vfxScript)
		{
			this.vfxScript = vfxScript;
		}

		public virtual void UntrackVFX()
		{
			vfxScript = null;
		}

		public virtual void AnimVFX(string eventName)
		{
			if (vfxScript != null)
			{
				vfxScript.AnimVFX(eventName);
			}
		}

		public virtual void SetVFXState(string name, string desiredState = null)
		{
			if (vfxScript != null)
			{
				if (desiredState != null)
				{
					vfxTrigger = new global::Kampai.Game.TriggerVFXOnState();
					vfxTrigger.StateName = name;
					vfxTrigger.MecanimStateHash = global::UnityEngine.Animator.StringToHash(desiredState);
				}
				else
				{
					vfxScript.TriggerState(name);
				}
			}
		}

		public virtual void UpdateVFX()
		{
			if (vfxTrigger != null && IsInAnimatorState(vfxTrigger.MecanimStateHash))
			{
				vfxScript.TriggerState(vfxTrigger.StateName);
				vfxTrigger = null;
			}
		}

		public virtual void SetMaterialColor(global::UnityEngine.Color color)
		{
			materialModifier = materialModifier ?? new global::Kampai.Util.Graphics.MaterialModifier(objectRenderers);
			global::Kampai.Util.Graphics.MaterialModifierExtensions.SetMaterialColor(materialModifier, color);
		}

		public virtual void SetBlendedColor(global::UnityEngine.Color color)
		{
			for (int i = 0; i < objectRenderers.Length; i++)
			{
				global::UnityEngine.Renderer renderer = objectRenderers[i];
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					global::UnityEngine.Material material = renderer.materials[j];
					if (material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.Procedural.BlendedColor) && (!material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.Blend.DstBlend) || material.GetFloat(global::Kampai.Util.GameConstants.ShaderProperties.Blend.DstBlend) != 1f))
					{
						material.SetColor(global::Kampai.Util.GameConstants.ShaderProperties.Procedural.BlendedColor, color);
					}
				}
			}
		}

		public virtual void EnableRenderers(bool enabled)
		{
			if (objectRenderers != null)
			{
				for (int i = 0; i < objectRenderers.Length; i++)
				{
					global::UnityEngine.Renderer renderer = objectRenderers[i];
					renderer.enabled = enabled;
				}
			}
		}

		public virtual void SetMaterialShaderFloat(string name, float value)
		{
			materialModifier = materialModifier ?? new global::Kampai.Util.Graphics.MaterialModifier(objectRenderers);
			materialModifier.SetFloat(name, value);
			materialModifier.Update();
		}

		public virtual void IncrementMaterialRenderQueue(int delta)
		{
			for (int i = 0; i < objectRenderers.Length; i++)
			{
				objectRenderers[i].material.renderQueue += delta;
			}
		}

		public virtual void SetZTestFunction(global::Kampai.Util.Graphics.CompareFunction func)
		{
			for (int i = 0; i < objectRenderers.Length; i++)
			{
				global::UnityEngine.Renderer renderer = objectRenderers[i];
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					global::UnityEngine.Material material = renderer.materials[j];
					if (material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.ZTest))
					{
						material.SetFloat(global::Kampai.Util.GameConstants.ShaderProperties.ZTest, (float)func);
					}
				}
			}
		}

		public virtual void UpdateMinRenderQueue(int minQueue)
		{
			for (int i = 0; i < objectRenderers.Length; i++)
			{
				global::UnityEngine.Renderer renderer = objectRenderers[i];
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					global::UnityEngine.Material material = renderer.materials[j];
					int renderQueue = material.renderQueue;
					int num = minQueue + (renderQueue - 1000) / 1000;
					if (num > renderQueue)
					{
						material.renderQueue = num;
					}
				}
			}
		}

		public virtual void ResetRenderQueue()
		{
			for (int i = 0; i < objectRenderers.Length; i++)
			{
				global::UnityEngine.Renderer renderer = objectRenderers[i];
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					global::UnityEngine.Material material = renderer.materials[j];
					material.renderQueue = global::Kampai.Util.Graphics.ShaderUtils.GetMaterialRenderQueue(material);
				}
			}
		}

		public void UpdateColliders(bool enableState)
		{
			if (colliders != null)
			{
				for (int i = 0; i < colliders.Length && !(colliders[i] == null); i++)
				{
					colliders[i].enabled = IsInteractable && enableState;
				}
			}
		}

		public void DebugActionableObject(bool debug)
		{
			this.debug = debug;
		}

		public void DumpQueue()
		{
			dumpQueue = true;
		}

		public virtual bool CanFadeGFX()
		{
			return false;
		}

		public virtual bool CanFadeSFX()
		{
			return false;
		}

		public virtual void FadeSFX(float duration, bool fadeIn)
		{
			if (fadeSfxAction != null)
			{
				fadeSfxAction.Abort();
			}
			fadeSfxAction = new global::Kampai.Game.View.FadeAudioAction(this, duration, fadeIn, logger);
			if (fadeIn && sfxFaded)
			{
				sfxFaded = false;
				fadeSfxAction.Execute();
			}
			else if (!fadeIn && !sfxFaded)
			{
				sfxFaded = true;
				fadeSfxAction.Execute();
			}
		}

		public bool IsFaded()
		{
			return sfxFaded || gfxFaded;
		}

		public bool IsGFXFaded()
		{
			return gfxFaded;
		}
	}
}
