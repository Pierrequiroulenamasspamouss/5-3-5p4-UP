namespace Kampai.Game
{
	public class QuestScriptController
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("QuestScriptController") as global::Kampai.Util.IKampaiLogger;

		private readonly global::strange.extensions.signal.impl.Signal<int> timerDoneSignal = new global::strange.extensions.signal.impl.Signal<int>();

		private global::Kampai.Game.QuestScriptInstance questScriptInstance;

		public readonly global::strange.extensions.signal.impl.Signal ContinueSignal = new global::strange.extensions.signal.impl.Signal();

		private readonly global::Kampai.Game.QuestDialogSetting currentDialogSetting = new global::Kampai.Game.QuestDialogSetting();

		private readonly global::Kampai.Game.SignalListener signalListener = new global::Kampai.Game.SignalListener();

		[Inject]
		public global::Kampai.Game.QuestScriptKernel kernel { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Common.IVideoService videoService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.ITikiBarService tikiBarService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IOrderBoardService orderBoardService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectAllMinionsSignal deselectAllMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSoundSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MoveBuildMenuSignal moveBuildMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenBuildingMenuSignal openBuildingMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowTipsSignal showTipsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableCameraBehaviourSignal enableCameraBehaviourSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisableCameraBehaviourSignal disableCameraBehaviourSignal { get; set; }

		[Inject]
		public global::Kampai.Common.SelectMinionSignal selectMinionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetLimitTikiBarWayFindersSignal setLimitTikiBarWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Common.IPickService pickService { get; set; }

		[Inject]
		public global::Kampai.UI.IPlayerTrainingService playerTrainingService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displayPlayerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockMinionsSignal unlockMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockVillainLairSignal unlockVillainLairSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		public void Setup(global::Kampai.Game.QuestScriptInstance questScriptInstance)
		{
			this.questScriptInstance = questScriptInstance;
			kernel.AddSignalListener(signalListener);
		}

		public void Stop()
		{
			if (questScriptInstance == null)
			{
				logger.Info("QuestScriptController::Stop: Stopping even though it appears setup was never called. (null questScriptInstance)");
				return;
			}
			logger.Info("QuestScriptController::Stop: Stopping quest id {0}", questScriptInstance.QuestID);
			signalListener.Clear();
			kernel.RemoveSignalListener(signalListener);
		}

		private void nextInstruction()
		{
			ContinueSignal.Dispatch();
		}

		[global::Kampai.Game.QuestScriptAPI("wait")]
		public bool wait(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			float seconds = args.GetFloat(1);
			routineRunner.StartCoroutine(waitFinish(seconds, nextInstruction));
			return false;
		}

		private global::System.Collections.IEnumerator waitFinish(float seconds, global::System.Action onFinish)
		{
			yield return new global::UnityEngine.WaitForSeconds(seconds);
			onFinish();
		}

		[global::Kampai.Game.QuestScriptAPI("waitTimer")]
		public bool waitTimer(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int eventTime = args.GetInt(1);
			int value = timeService.CurrentTime();
			timeEventService.AddEvent(questScriptInstance.QuestID, global::System.Convert.ToInt32(value), eventTime, timerDoneSignal);
			timerDoneSignal.AddListener(receive_waitTimerDone);
			return false;
		}

		private void receive_waitTimerDone(int questId)
		{
			timerDoneSignal.RemoveListener(receive_waitTimerDone);
			nextInstruction();
		}

		[global::Kampai.Game.QuestScriptAPI("seedPremiumCurrency")]
		public bool seedPremiumCurrency(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int amount = args.GetInt(1);
			playerService.AlterQuantity(global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID, amount);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SpawnDooberSignal>().Dispatch(global::UnityEngine.Vector3.zero, global::Kampai.UI.View.DestinationType.PREMIUM, -1, false);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("moveMenu")]
		public bool moveMenu(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			moveBuildMenuSignal.Dispatch(args.GetBoolean(1));
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("playVideo")]
		public bool playVideo(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string urlOrFilename = args.GetString(1);
			videoService.playVideo(urlOrFilename, true, false);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("deselectAllMinions")]
		public bool deselectAllMinions(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			deselectAllMinionsSignal.Dispatch();
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getInstanceId")]
		public bool getInstanceId(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int value = 0;
			int defId = args.GetInt(1);
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instancesByDefinitionID = playerService.GetInstancesByDefinitionID(defId);
			if (instancesByDefinitionID != null && instancesByDefinitionID.Count > 0)
			{
				value = instancesByDefinitionID[0].ID;
			}
			ret.Set(value);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getDefinitionId")]
		public bool getDefinitionId(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int value = 0;
			int id = args.GetInt(1);
			global::Kampai.Game.Instance byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Instance>(id);
			if (byInstanceId != null)
			{
				value = byInstanceId.Definition.ID;
			}
			ret.Set(value);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("playSound")]
		public bool playSound(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			internalPlaySound(args.GetString(1));
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("hasSeenPlayerTraining")]
		public bool hasSeenPlayerTraining(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int playerTrainingDefinitionId = args.GetInt(1);
			ret.Set(playerTrainingService.HasSeen(playerTrainingDefinitionId, global::Kampai.UI.PlayerTrainingVisiblityType.GAME));
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("sendTelemetry")]
		public bool sendTelemetry(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string text = args.GetString(1);
			switch (text)
			{
			case "achievement":
				{
					string text2 = args.GetString(2);
					telemetryService.Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL(questScriptInstance.QuestLocalizedKey + "." + text2, global::Kampai.Common.Service.Telemetry.TelemetryAchievementType.QuestStep, 0, string.Empty);
					break;
				}
			default:
				ERROR(string.Format("Unknown telemetry event type at line {0} ({1})", "undefined", text));
				break;
			case "tutorial":
			{
				string step = args.GetString(2);
				telemetryService.Send_Telemetry_EVT_USER_TUTORIAL_FUNNEL_EAL(questScriptInstance.QuestLocalizedKey, step);
				break;
			}
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setDialogType")]
		public bool setDialogType(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			currentDialogSetting.type = global::Kampai.UI.View.QuestDialogType.NORMAL;
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setCharacterImage")]
		public bool setCharacterImage(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int definitionID = args.GetInt(1);
			currentDialogSetting.definitionID = definitionID;
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("sitPhilAtBar")]
		public bool sitPhilAtBar(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.PhilSitAtBarSignal>().Dispatch(true);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setDialogSound")]
		public bool setDialogSound(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string text = args.GetString(1);
			if (text.CompareTo("None") == 0)
			{
				currentDialogSetting.dialogSound = string.Empty;
			}
			else
			{
				currentDialogSetting.dialogSound = text;
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("closeAllDialogs")]
		public bool closeAllDialogs(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.CloseAllOtherMenuSignal>().Dispatch(null);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("showDialog")]
		public bool showDialog(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.CloseAllOtherMenuSignal>().Dispatch(null);
			string type = args.GetString(1);
			global::Kampai.Util.Tuple<int, int> type2 = new global::Kampai.Util.Tuple<int, int>(questScriptInstance.QuestID, questScriptInstance.QuestStepID);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.ShowDialogSignal>().Dispatch(type, currentDialogSetting, type2);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("openGameDialog")]
		public bool openGameDialog(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string prefab = args.GetString(1);
			string text = args.GetString(2);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, prefab);
			if (!string.IsNullOrEmpty(text))
			{
				iGUICommand.skrimScreen = text;
				iGUICommand.darkSkrim = false;
			}
			guiService.Execute(iGUICommand);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("awardBuilding")]
		public bool awardBuilding(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int type = args.GetInt(1);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CreateBuildingInInventorySignal>().Dispatch(type);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setBuildingState")]
		public bool setBuildingState(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int param = args.GetInt(1);
			global::Kampai.Game.BuildingState param2 = (global::Kampai.Game.BuildingState)args.GetInt(2);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingChangeStateSignal>().Dispatch(param, param2);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getBuildingState")]
		public bool getBuildingState(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int id = args.GetInt(1);
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(id);
			if (byInstanceId != null)
			{
				ret.Set((int)byInstanceId.State);
			}
			else
			{
				ret.Set(-1);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("showWayFinder")]
		public bool showWayFinder(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int trackedId = args.GetInt(1);
			bool boolean = args.GetBoolean(2);
			global::Kampai.UI.View.WayFinderSettings wayFinderSettings = new global::Kampai.UI.View.WayFinderSettings(trackedId);
			if (boolean)
			{
				createWayFinderSignal.Dispatch(wayFinderSettings);
			}
			else
			{
				removeWayFinderSignal.Dispatch(wayFinderSettings.TrackedId);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setLimitTikiBarWayFinders")]
		public bool setLimitTikiBarWayFinders(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			bool boolean = args.GetBoolean(1);
			setLimitTikiBarWayFindersSignal.Dispatch(boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("showSettingsMenuButton")]
		public bool showSettingsMenuButton(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			bool boolean = args.GetBoolean(1);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowSettingsButtonSignal>().Dispatch(boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("showTraining")]
		public bool showTraining(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int type = args.GetInt(1);
			displayPlayerTrainingSignal.Dispatch(type, false, new global::strange.extensions.signal.impl.Signal<bool>());
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("debug")]
		public bool debugFunction(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			Debug(args.GetString(1));
			return true;
		}

		private void Debug(string message)
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, true, string.Format("Script Message: {0}", message));
		}

		private void SubStep()
		{
			nextInstruction();
		}

		[global::Kampai.Game.QuestScriptAPI("defaultTimeout")]
		public bool GetDefaultTimeout(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(33301);
			int result = -1;
			string description = itemDefinition.Description;
			int.TryParse(description, out result);
			if (result < 0)
			{
				logger.Error("Failed to get default timeout from definitions!");
				result = 5;
			}
			ret.Set(result);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("cameraDefault")]
		public bool cameraDefault(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoZoomSignal>().Dispatch(0.3f);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("minionsSelected")]
		public bool minionsSelected(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			global::Kampai.Common.PickState pickState = pickService.GetPickState();
			global::System.Collections.Generic.IList<int> list = pickState.MinionsSelected;
			if (list.Count > 0)
			{
				foreach (int item in list)
				{
					ret.SetKey(item.ToString()).Set(true);
				}
			}
			else
			{
				ret.SetNil();
			}
			return true;
		}

		private void internalPlaySound(string soundEvent)
		{
			globalSoundSignal.Dispatch(soundEvent);
		}

		[global::Kampai.Game.QuestScriptAPI("waitSignal")]
		public bool waitSignal(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string signalName = args.GetString(1);
			float num = args.GetFloat(2);
			global::Kampai.Game.CompleteSignal completeSignal = new global::Kampai.Game.CompleteSignal();
			global::strange.extensions.signal.impl.Signal timeoutSignal = new global::strange.extensions.signal.impl.Signal();
			global::System.Action<string, bool> handleCallback = null;
			object @lock = new object();
			bool callbackExecuted = false;
			global::System.Action<string> onComplete = delegate(string name)
			{
				handleCallback(name, true);
			};
			global::System.Action onTimeout = delegate
			{
				handleCallback(signalName, false);
			};
			handleCallback = delegate(string name, bool retVal)
			{
				lock (@lock)
				{
					if (!callbackExecuted)
					{
						callbackExecuted = true;
						if (!retVal)
						{
							ret.SetNil();
						}
						completeSignal.RemoveListener(onComplete);
						timeoutSignal.RemoveListener(onTimeout);
						nextInstruction();
					}
				}
			};
			completeSignal.AddListener(onComplete);
			signalListener.ListenForSignal(signalName, completeSignal, ret);
			if (num > 0f)
			{
				timeoutSignal.AddListener(onTimeout);
				global::System.Action onFinish = delegate
				{
					timeoutSignal.Dispatch();
				};
				routineRunner.StartCoroutine(waitFinish(num, onFinish));
			}
			return false;
		}

		[global::Kampai.Game.QuestScriptAPI("waitAnySignal")]
		public bool waitAnySignal(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int count = args.Length;
			global::System.Collections.Generic.List<string> signals = new global::System.Collections.Generic.List<string>();
			global::Kampai.Game.ReturnValueContainer nameRet = ret.PushIndex();
			global::Kampai.Game.ReturnValueContainer ret2 = ret.PushIndex();
			global::Kampai.Game.CompleteSignal completeSignal = new global::Kampai.Game.CompleteSignal();
			global::System.Action<string, bool> handleCallback = null;
			object @lock = new object();
			bool callbackExecuted = false;
			global::System.Action<string> onComplete = delegate(string name)
			{
				handleCallback(name, true);
			};
			handleCallback = delegate(string name, bool retVal)
			{
				lock (@lock)
				{
					if (!callbackExecuted)
					{
						callbackExecuted = true;
						if (!retVal)
						{
							ret.SetNil();
						}
						nameRet.Set(name);
						completeSignal.RemoveListener(onComplete);
						for (int i = 0; i < count; i++)
						{
							string name2 = signals[i];
							signalListener.StopListeningForSignal(name2);
						}
						nextInstruction();
					}
				}
			};
			completeSignal.AddListener(onComplete);
			for (int num = 1; num <= count; num++)
			{
				string text = args.GetString(num);
				signals.Add(text);
				signalListener.ListenForSignal(text, completeSignal, ret2);
			}
			return false;
		}

		[global::Kampai.Game.QuestScriptAPI("checkCoppa")]
		public bool checkCoppa(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			ret.Set(coppaService.IsBirthdateKnown());
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getPlayerLevel")]
		public bool getPlayerLevel(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			ret.Set(quantity);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getPlayerItemQuantity")]
		public bool getPlayerItemQuantity(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int defId = args.GetInt(1);
			int quantityByDefinitionId = (int)playerService.GetQuantityByDefinitionId(defId);
			ret.Set(quantityByDefinitionId);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getMinionCount")]
		public bool getMinionCount(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int minionCount = playerService.GetMinionCount();
			ret.Set(minionCount);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getPlayerQuantity")]
		public bool getPlayerQuantity(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int defId = args.GetInt(1);
			int countByDefinitionId = playerService.GetCountByDefinitionId(defId);
			ret.Set(countByDefinitionId);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("alterPlayerQuantity")]
		public bool alterPlayerQuantity(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			int amount = args.GetInt(2);
			global::Kampai.Game.StaticItem def = (global::Kampai.Game.StaticItem)num;
			playerService.AlterQuantity(def, amount);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("displayMinionLevelTokenDoober")]
		public bool displayMinionLevelTokenDoober(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			global::Kampai.Game.VillainLairEntranceBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(374);
			gameContext.injectionBinder.GetInstance<global::Kampai.UI.View.SpawnDooberSignal>().Dispatch(new global::UnityEngine.Vector3(byInstanceId.Location.x, 0f, byInstanceId.Location.y), global::Kampai.UI.View.DestinationType.MINION_LEVEL_TOKEN, 31, true);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("gotoInGame")]
		public bool gotoInGame(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int buildingId = args.GetInt(1);
			int buildingDefId = args.GetInt(2);
			int itemId = args.GetInt(3);
			bool boolean = args.GetBoolean(4);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.GotoSignal>().Dispatch(new global::Kampai.Game.GotoArgument(buildingId, buildingDefId, itemId, boolean));
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getBuildingsOfType")]
		public bool getBuildingsOfType(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			BuildingType.BuildingTypeIdentifier buildingTypeIdentifier = (BuildingType.BuildingTypeIdentifier)args.GetInt(1);
			global::System.Collections.Generic.IList<global::Kampai.Game.Building> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Building>();
			ret.SetEmptyArray();
			int i = 0;
			for (int count = instancesByType.Count; i < count; i++)
			{
				global::Kampai.Game.Building building = instancesByType[i];
				if (building.Definition.Type == buildingTypeIdentifier)
				{
					ret.PushIndex().Set(building.ID);
				}
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("isQuestComplete")]
		public bool isQuestComplete(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int questDefinitionID = args.GetInt(1);
			ret.PushIndex().Set(questService.IsQuestCompleted(questDefinitionID) ? 1 : 0);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("isStuartQuesting")]
		public bool isStuartQuesting(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			global::Kampai.Game.StuartCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StuartCharacter>(70001);
			if (firstInstanceByDefinitionId == null)
			{
				ret.Set(0);
				return true;
			}
			if (firstInstanceByDefinitionId == null)
			{
				ret.Set(0);
				return true;
			}
			global::Kampai.Game.Prestige prestigeForSeatableCharacter = tikiBarService.GetPrestigeForSeatableCharacter(firstInstanceByDefinitionId);
			if (prestigeForSeatableCharacter.state != global::Kampai.Game.PrestigeState.InQueue)
			{
				ret.Set(1);
			}
			else
			{
				ret.Set(0);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("isLeisureMenuOpen")]
		public bool isLeisureMenuOpen(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (uiModel.LeisureMenuOpen)
			{
				ret.Set(1);
			}
			else
			{
				ret.Set(0);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("markVillainIslandAsUnlocked")]
		public bool markVillainIslandasUnlocked(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.EnableVillainIslandCollidersSignal>().Dispatch(false);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("highlightStoreItem")]
		public bool highlightStoreItem(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int type = args.GetInt(1);
			bool boolean = args.GetBoolean(2);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.OpenStoreHighlightItemSignal>().Dispatch(type, boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("throbGoToButton")]
		public bool throbGoToButton(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobGotoButton());
			}
			else
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobGotoButton));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("throbPlacementButton")]
		public bool throbPlacementButton(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			bool boolean = args.GetBoolean(1);
			questService.SetPulseMoveBuildingAccept(boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("throbDeliverButton")]
		public bool throbDeliverButton(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobDeliverButton());
			}
			else
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobDeliverButton));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("enablePurchaseButton")]
		public bool enablePurchaseButton(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.DisablePurchaseButton));
			}
			else
			{
				guiService.AddToArguments(new global::Kampai.UI.DisablePurchaseButton());
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("highlightCrafting")]
		public bool highlightCrafting(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobCraftingButton());
			}
			else
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobCraftingButton));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("highlightHarvest")]
		public bool highlightHarvest(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			bool boolean = args.GetBoolean(1);
			routineRunner.StartCoroutine(HarvestHighlight(boolean));
			return true;
		}

		private global::System.Collections.IEnumerator HarvestHighlight(bool highlight)
		{
			yield return null;
			yield return null;
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.HighlightHarvestButtonSignal>().Dispatch(highlight);
		}

		[global::Kampai.Game.QuestScriptAPI("highlightFillOrder")]
		public bool highlightFillOrder(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.OrderBoardHighLightFillOrderSignal>().Dispatch();
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("highlightTicket")]
		public bool highlightTicket(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobTicketButton());
			}
			else
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobTicketButton));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setOrderBoardText")]
		public bool setOrderBoardText(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string title = args.GetString(1);
			routineRunner.StartCoroutine(TicketFTUEText(title));
			return true;
		}

		private global::System.Collections.IEnumerator TicketFTUEText(string title)
		{
			yield return null;
			yield return null;
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SetFTUETextSignal>().Dispatch(title);
		}

		[global::Kampai.Game.QuestScriptAPI("showXPHighlight")]
		public bool showXPHighlight(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			bool boolean = args.GetBoolean(1);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.XPFTUEHighlightSignal>().Dispatch(boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getMinions")]
		public bool getMinions(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Minion> idleMinions = playerService.GetIdleMinions();
			for (int i = 0; i < idleMinions.Count; i++)
			{
				global::Kampai.Game.ReturnValueContainer returnValueContainer = ret.PushIndex();
				returnValueContainer.Set(idleMinions[i].ID);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("rewardPlayer")]
		public bool rewardPlayer(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int xp = args.GetInt(1);
			int grind = args.GetInt(2);
			runDynamicTransaction(grind, xp);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("addMinions")]
		public bool addMinions(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			if (num > 0)
			{
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
				transactionDefinition.ID = int.MaxValue;
				transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(5, (uint)num));
				playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.QUEST_REWARD, null);
				unlockMinionsSignal.Dispatch();
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("runTransaction")]
		public bool runTransaction(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int id = args.GetInt(1);
			global::Kampai.Game.Transaction.TransactionDefinition def = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(id);
			playerService.RunEntireTransaction(def, global::Kampai.Game.TransactionTarget.NO_VISUAL, null);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.UpdateUIButtonsSignal>().Dispatch(false);
			return true;
		}

		private void runDynamicTransaction(int grind = 0, int xp = 0)
		{
			if (xp > 0 || grind > 0)
			{
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
				transactionDefinition.ID = int.MaxValue;
				transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				if (grind > 0)
				{
					transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(0, (uint)grind));
				}
				if (xp > 0)
				{
					transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(2, (uint)xp));
				}
				playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.NO_VISUAL, null);
				global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instancesByDefinitionID = playerService.GetInstancesByDefinitionID(3041);
				global::Kampai.Game.TikiBarBuilding tikiBarBuilding = instancesByDefinitionID[0] as global::Kampai.Game.TikiBarBuilding;
				global::UnityEngine.Vector3 type = new global::UnityEngine.Vector3(tikiBarBuilding.Location.x, 0f, tikiBarBuilding.Location.y);
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SpawnDooberSignal>().Dispatch(type, global::Kampai.UI.View.DestinationType.XP, -1, true);
			}
		}

		[global::Kampai.Game.QuestScriptAPI("seedVillainLairRepairGoods")]
		public bool seedVillainLairRepairGoods(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int id = args.GetInt(1);
			global::Kampai.Game.TransactionTarget target = global::Kampai.Game.TransactionTarget.HARVEST;
			global::Kampai.UI.View.DestinationType type = global::Kampai.UI.View.DestinationType.STORAGE;
			global::Kampai.Game.KevinCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.KevinCharacter>(70003);
			global::UnityEngine.GameObject instance = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER);
			global::Kampai.Game.View.NamedCharacterManagerView component = instance.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
			global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = component.Get(firstInstanceByDefinitionId.ID);
			global::UnityEngine.Vector3 position = namedCharacterObject.gameObject.transform.position;
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(id);
			playerService.RunEntireTransaction(transactionDefinition, target, null);
			for (int i = 0; i < transactionDefinition.Outputs.Count; i++)
			{
				int iD = transactionDefinition.Outputs[i].ID;
				if (iD != 0 && iD != 2 && iD != 1)
				{
					uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SpawnDooberSignal>().Dispatch(position, type, transactionDefinition.Outputs[i].ID, true);
				}
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("unlockVillainLair")]
		public bool unlockVillainLair(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			global::Kampai.Game.VillainLairEntranceBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLairEntranceBuilding>(3132);
			unlockVillainLairSignal.Dispatch(firstInstanceByDefinitionId, 3137);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("localPersistanceHasKey")]
		public bool localPersistanceHasKey(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string name = args.GetString(1);
			ret.Set(localPersistService.HasKey(name));
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("moveMinion")]
		public bool moveMinion(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int param = args.GetInt(1);
			float x = args.GetFloat(2);
			float z = args.GetFloat(3);
			global::UnityEngine.Vector3 value = default(global::UnityEngine.Vector3);
			value.x = x;
			value.y = 0f;
			value.z = z;
			global::Kampai.Util.Boxed<global::UnityEngine.Vector3> param2 = new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(value);
			selectMinionSignal.Dispatch(param, param2, false);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("openTikiHut")]
		public bool openTikiHut(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.IN, global::Kampai.Game.BuildingZoomType.TIKIBAR));
			}
			else
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, global::Kampai.Game.BuildingZoomType.TIKIBAR));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("taskMinion")]
		public bool taskMinion(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int objectId = args.GetInt(1);
			int first = args.GetInt(2);
			global::UnityEngine.GameObject instance = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.MINION_MANAGER);
			global::Kampai.Game.View.MinionManagerView component = instance.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			global::Kampai.Game.View.MinionObject second = component.Get(objectId);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.StartMinionTaskSignal>().Dispatch(new global::Kampai.Util.Tuple<int, global::Kampai.Game.View.MinionObject, int>(first, second, timeService.CurrentTime()));
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getTaskedMinionCount")]
		public bool getTaskedMinionCount(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int id = args.GetInt(1);
			global::Kampai.Game.TaskableBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TaskableBuilding>(id);
			int minionsInBuilding = byInstanceId.GetMinionsInBuilding();
			ret.Set(minionsInBuilding);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("showMessageBox")]
		public bool showMessageBox(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string key = args.GetString(1);
			string type = localService.GetString(key);
			popupMessageSignal.Dispatch(type, global::Kampai.UI.View.PopupMessageType.NORMAL);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("showTips")]
		public bool showTips(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string type = args.GetString(1);
			int num = args.GetInt(2);
			global::System.Collections.Generic.IList<global::Kampai.Game.Minion> idleMinions = playerService.GetIdleMinions();
			int count = idleMinions.Count;
			if (count < num)
			{
				return true;
			}
			showTipsSignal.Dispatch(type);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getStoreItemIdFromDefinition")]
		public bool getStoreItemIdFromDefinition(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			global::Kampai.Game.StoreItemType value = global::Kampai.Game.StoreItemType.BaseResource;
			global::System.Collections.Generic.IList<global::Kampai.Game.StoreItemDefinition> all = definitionService.GetAll<global::Kampai.Game.StoreItemDefinition>();
			foreach (global::Kampai.Game.StoreItemDefinition item in all)
			{
				if (item.ReferencedDefID == num)
				{
					value = item.Type;
				}
			}
			ret.Set((int)value);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("moveStoreToMainMenu")]
		public bool moveStoreToMainMenu(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.MoveTabMenuSignal>().Dispatch(true);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("enableCameraControls")]
		public bool enableCameraControls(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			enableCameraBehaviourSignal.Dispatch(7);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("disableCameraControls")]
		public bool disableCameraControls(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			disableCameraBehaviourSignal.Dispatch(7);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("panToBuildingAndOpen")]
		public bool panToBuildingAndOpen(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			if (num > 0)
			{
				global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(num);
				if (byInstanceId != null)
				{
					global::Kampai.Game.View.BuildingManagerView component = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER).GetComponent<global::Kampai.Game.View.BuildingManagerView>();
					global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(num);
					if (buildingObject != null)
					{
						openBuildingMenuSignal.Dispatch(buildingObject, byInstanceId);
					}
				}
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("panCameraToInstance")]
		public bool panCameraToInstance(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			if (num > 0)
			{
				float num2 = args.GetFloat(2);
				global::Kampai.Game.PanInstructions panInstructions = new global::Kampai.Game.PanInstructions(num);
				if (num2 > 0f)
				{
					panInstructions.ZoomDistance = new global::Kampai.Util.Boxed<float>(num2);
				}
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveToInstanceSignal>().Dispatch(panInstructions, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(new global::Kampai.Game.ScreenPosition()));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("panCameraToPosition")]
		public bool panCameraToPosition(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			float num = args.GetInt(1);
			float y = args.GetInt(2);
			float num2 = args.GetInt(3);
			if (num != 0f && num2 != 0f)
			{
				float type = args.GetFloat(4);
				bool boolean = args.GetBoolean(5);
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveToPositionSignal>().Dispatch(new global::UnityEngine.Vector3(num, y, num2), type, boolean);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("cinematicCameraToBuilding")]
		public bool cinematicCameraToBuilding(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			if (num != 0)
			{
				float type = args.GetFloat(2);
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraCinematicMoveToBuildingSignal>().Dispatch(num, type);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("holdStill")]
		public bool holdStill(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			if (num > 0)
			{
				global::Kampai.Game.SelectMinionState selectMinionState = new global::Kampai.Game.SelectMinionState();
				selectMinionState.runLocation = null;
				selectMinionState.triggerIncidentalAnimation = false;
				selectMinionState.muteStatus = false;
				selectMinionState.minionID = num;
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.AnimateSelectedMinionSignal>().Dispatch(selectMinionState);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("startMinionAnimation")]
		public bool startMinionAnimation(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			int num2 = args.GetInt(2);
			if (num > 0 && num2 > 0)
			{
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(num);
				global::Kampai.Game.MinionAnimationDefinition minionAnimationDefinition = definitionService.Get<global::Kampai.Game.MinionAnimationDefinition>(num2);
				if (byInstanceId != null && minionAnimationDefinition != null)
				{
					gameContext.injectionBinder.GetInstance<global::Kampai.Game.StartIncidentalAnimationSignal>().Dispatch(num, num2);
				}
				else
				{
					logger.Error("Unable to start animation {0} {1}", num, num2);
				}
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("startFunUnlock")]
		public bool startFunUnlock(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.StartMinionPartyUnlockSequenceSignal>().Dispatch();
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("philStart")]
		public bool philStart(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.PhilGoToStartLocationSignal>().Dispatch();
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("philFixedSign")]
		public bool philFixedSign(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.PhilSignFixedSignal>().Dispatch();
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("stopMinionCamping")]
		public bool stopMinionCamping(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.StopMinionCampingSignal>().Dispatch();
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("displayCaptainTeaseModal")]
		public bool displayCaptainTeaseModal(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_MysteryMinionTeaserSelectionModal");
			iGUICommand.skrimScreen = "TSMTeaseSkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.disableSkrimButton = true;
			guiService.Execute(iGUICommand);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setCharacterAnimation")]
		public bool setCharacterAnimation(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int id = args.GetInt(1);
			string type = args.GetString(2);
			global::Kampai.Game.NamedCharacter byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.NamedCharacter>(id);
			if (byInstanceId is global::Kampai.Game.PhilCharacter)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.AnimatePhilSignal>().Dispatch(type);
			}
			else if (byInstanceId is global::Kampai.Game.KevinCharacter)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.KevinGoToWelcomeHutSignal>().Dispatch(true);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("stageRepairCelebrate")]
		public bool stageRepairCelebrate(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.StuartTunesGuitarSignal>().Dispatch();
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("placeCharacterInBuilding")]
		public bool placeCharacterInBuilding(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int id = args.GetInt(1);
			global::Kampai.Game.NamedCharacter byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.NamedCharacter>(id);
			if (byInstanceId is global::Kampai.Game.PhilCharacter)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.PhilGoToTikiBarSignal>().Dispatch(true);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setBuildMenuEnabled")]
		public bool setBuildMenuEnabled(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			bool boolean = args.GetBoolean(1);
			if (boolean)
			{
				uiModel.UIState &= ~global::Kampai.UI.View.UIModel.UIStateFlags.StoreButtonHiddenFromQuest;
			}
			else
			{
				uiModel.UIState |= global::Kampai.UI.View.UIModel.UIStateFlags.StoreButtonHiddenFromQuest;
			}
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SetBuildMenuEnabledSignal>().Dispatch(boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setStorageMenuEnabled")]
		public bool setStorageMenuEnabled(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			bool boolean = args.GetBoolean(1);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SetStorageMenuEnabledSignal>().Dispatch(boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setOrderBoardMenuEnabled")]
		public bool setOrderBoardMenuEnabled(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			bool boolean = args.GetBoolean(1);
			orderBoardService.SetEnabled(boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setIgnoreInstance")]
		public bool setIgnoreInstance(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int instanceId = args.GetInt(1);
			bool boolean = args.GetBoolean(2);
			pickService.SetIgnoreInstanceInput(instanceId, boolean);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("dispatchSignal")]
		public bool DispatchSignal(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			string text = args.GetString(1);
			global::System.Type type = global::System.Reflection.Assembly.GetExecutingAssembly().GetType("Kampai.Game." + text, false);
			if (type == null)
			{
				logger.Error("qs.dispatchSignal: Cannot dispatch signal {0}, cannot find type.", text);
				return true;
			}
			object instance;
			try
			{
				instance = gameContext.injectionBinder.GetInstance(type);
			}
			catch (global::strange.extensions.injector.impl.InjectionException)
			{
				logger.Error("qs.dispatchSignal: Cannot dispatch signal {0}, failed to get instance.", text);
				return true;
			}
			global::System.Type baseType = type.BaseType;
			global::System.Type[] genericArguments = baseType.GetGenericArguments();
			if (genericArguments.Length > args.Length - 1)
			{
				logger.Error("qs.dispatchSignal: Cannot dispatch signal {0}, not enough args", text);
				return true;
			}
			global::System.Reflection.MethodInfo method = baseType.GetMethod("Dispatch", genericArguments);
			object[] array = new object[genericArguments.Length];
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				array[i] = args.Get(i + 2, genericArguments[i]);
			}
			method.Invoke(instance, array);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("throbLockedButtons")]
		public bool throbLockedButtons(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobLockedButtons());
			}
			else
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobLockedButtons));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("throbRushButtons")]
		public bool throbRushButtons(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobRushButtons());
			}
			else
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobRushButtons));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("throbCallButtons")]
		public bool throbCallButtons(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobCallButtons());
			}
			else
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobCallButtons));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("throbCollectButton")]
		public bool throbCollectButton(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobCollectButton());
			}
			else
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobCollectButton));
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("enableCallButtons")]
		public bool enableCallButtons(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.DisableCallButtons));
			}
			else
			{
				guiService.AddToArguments(new global::Kampai.UI.DisableCallButtons());
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("enableRushButtons")]
		public bool enableRushButtons(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.DisableRushButtons));
			}
			else
			{
				guiService.AddToArguments(new global::Kampai.UI.DisableRushButtons());
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("enableDeleteButton")]
		public bool enableDeleteButton(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.DisableDeleteOrderButton));
			}
			else
			{
				guiService.AddToArguments(new global::Kampai.UI.DisableDeleteOrderButton());
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("enableLockedButton")]
		public bool enableLockedButton(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			if (args.GetBoolean(1))
			{
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.DisableLockedButton));
			}
			else
			{
				guiService.AddToArguments(new global::Kampai.UI.DisableLockedButton());
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("enableCabanas")]
		public bool enableCabanas(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			foreach (global::Kampai.Game.Instance item in playerService.GetInstancesByDefinition<global::Kampai.Game.CabanaBuildingDefinition>())
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingChangeStateSignal>().Dispatch(item.ID, global::Kampai.Game.BuildingState.Broken);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("cameraZoomToBeach")]
		public bool cameraZoomToBeach(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraZoomBeachSignal>().Dispatch();
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("setFtueLevelCompleted")]
		public bool setFtueLevelCompleted(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int num = args.GetInt(1);
			if (num > 0)
			{
				playerService.SetHighestFtueCompleted(num);
			}
			if (num == 999999)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.CheckTriggersSignal>().Dispatch(301);
			}
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getFtueLevelCompleted")]
		public bool getFtueLevelCompleted(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			ret.Set(playerService.GetHighestFtueCompleted());
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("getFTUECompleteValue")]
		public bool getFTUECompleteValue(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			ret.Set(999999);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("changeToPrestigeState")]
		public bool changeToPrestigeState(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int definitionId = args.GetInt(1);
			global::Kampai.Game.PrestigeState targetState = (global::Kampai.Game.PrestigeState)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.PrestigeState), args.GetString(2));
			global::Kampai.Game.IPrestigeService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPrestigeService>();
			global::Kampai.Game.Prestige firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(definitionId);
			instance.ChangeToPrestigeState(firstInstanceByDefinitionId, targetState);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("liberateLeveledUpMinions")]
		public bool liberateLeveledUpMinions(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			int type = 1;
			if (args.Length > 1)
			{
				type = args.GetInt(1);
			}
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.LiberateLeveledUpMinionsSignal>().Dispatch(type);
			return true;
		}

		[global::Kampai.Game.QuestScriptAPI("inEditor")]
		public bool inEditor(global::Kampai.Game.IArgRetriever args, global::Kampai.Game.ReturnValueContainer ret)
		{
			ret.Set(false);
			return true;
		}

		private void ERROR(string message)
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, true, string.Format("SCRIPT ERROR[{0}] {1}", "undefined", message));
		}
	}
}
