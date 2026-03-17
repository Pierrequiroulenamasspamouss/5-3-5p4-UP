namespace Kampai.UI.View
{
	public class BuddyWelcomePanelMediator : global::Kampai.UI.View.KampaiMediator
	{
		private global::Kampai.UI.View.CharacterWelcomeState currentState;

		private global::Kampai.Game.Prestige prestige;

		[Inject]
		public global::Kampai.UI.View.BuddyWelcomePanelView view { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUECloseBuddySignal closeBuddySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToPositionSignal cameraAutoMoveToPositionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.OnMenuClose.AddListener(OnMenuClose);
			view.SetUpInjections(positionService);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			prestige = args.Get<global::Kampai.Game.Prestige>();
			global::Kampai.UI.View.CharacterWelcomeState state = args.Get<global::Kampai.UI.View.CharacterWelcomeState>();
			Load(state);
			if (prestige.CurrentPrestigeLevel >= 1)
			{
				hideAllWayFindersSignal.Dispatch();
			}
			StartCoroutine(AnimSequence(state));
		}

		private global::System.Collections.IEnumerator AnimSequence(global::Kampai.UI.View.CharacterWelcomeState state)
		{
			yield return new global::UnityEngine.WaitForSeconds(0.2f);
			global::Kampai.Game.PrestigeType prestigeType = prestige.Definition.Type;
			if ((prestigeType == global::Kampai.Game.PrestigeType.Minion && state == global::Kampai.UI.View.CharacterWelcomeState.Welcome && (prestige.Definition.PrestigeLevelSettings == null || prestige.CurrentPrestigeLevel >= 0)) || (prestigeType == global::Kampai.Game.PrestigeType.Minion && state == global::Kampai.UI.View.CharacterWelcomeState.Farewell) || (prestigeType == global::Kampai.Game.PrestigeType.Villain && state == global::Kampai.UI.View.CharacterWelcomeState.Welcome))
			{
				if (prestige.CurrentPrestigeLevel <= 0 && state == global::Kampai.UI.View.CharacterWelcomeState.Welcome)
				{
					if (prestigeType == global::Kampai.Game.PrestigeType.Villain)
					{
						cameraAutoMoveToPositionSignal.Dispatch(global::Kampai.Util.GameConstants.Building.WELCOME_VILLAIN_LOCATION, 0.97f, false);
					}
					else
					{
						cameraAutoMoveToPositionSignal.Dispatch(global::Kampai.Util.GameConstants.Building.WELCOME_NAMED_CHARACTER_LOCATION, 0.97f, false);
					}
				}
				else
				{
					cameraAutoMoveToPositionSignal.Dispatch(global::Kampai.Util.GameConstants.Building.TIKI_BAR_PAN_LOCATION, 0.97f, false);
				}
			}
			global::Kampai.Game.View.ActionableObject actionableObject = global::Kampai.Game.View.ActionableObjectManagerView.GetFromAllObjects(prestige.trackedInstanceId);
			view.SetUpCharacterObject(actionableObject as global::Kampai.Game.View.CharacterObject);
			if (currentState == global::Kampai.UI.View.CharacterWelcomeState.Welcome)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingReactionSignal>().Dispatch(new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(actionableObject.transform.position), false);
			}
			yield return new global::UnityEngine.WaitForSeconds(0.8f);
			view.Initialized = true;
			view.Open();
			yield return new global::UnityEngine.WaitForSeconds(view.FadeOutTime);
			Close();
		}

		private void OnMenuClose()
		{
			closeBuddySignal.Dispatch();
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_CharacterState");
			uiModel.WelcomeBuddyOpen = false;
		}

		private void Close()
		{
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			view.Close();
			if (prestige.CurrentPrestigeLevel >= 1)
			{
				showAllWayFindersSignal.Dispatch();
			}
			pickControllerModel.SetIgnoreInstance(313, false);
		}

		public void Load(global::Kampai.UI.View.CharacterWelcomeState state)
		{
			currentState = state;
			PlayAudio();
			global::Kampai.Game.PrestigeDefinition prestigeDefinition = ((prestige != null) ? prestige.Definition : null);
			switch (state)
			{
			case global::Kampai.UI.View.CharacterWelcomeState.Farewell:
				view.Init("FarewellTitle", prestigeDefinition.LocalizedKey);
				break;
			case global::Kampai.UI.View.CharacterWelcomeState.Welcome:
				view.Init((prestige.CurrentPrestigeLevel <= 0) ? "WelcomeTitle" : "RePrestigeTitle", prestigeDefinition.LocalizedKey);
				break;
			}
		}

		private void PlayAudio()
		{
			global::Kampai.Game.PrestigeType type = prestige.Definition.Type;
			if (currentState == global::Kampai.UI.View.CharacterWelcomeState.Welcome && type == global::Kampai.Game.PrestigeType.Minion)
			{
				playSFXSignal.Dispatch("Play_minionArrives_01");
			}
			else if (currentState == global::Kampai.UI.View.CharacterWelcomeState.Farewell && type == global::Kampai.Game.PrestigeType.Minion)
			{
				playSFXSignal.Dispatch("Play_minionUnlock_01");
			}
			else if (currentState == global::Kampai.UI.View.CharacterWelcomeState.Welcome && type == global::Kampai.Game.PrestigeType.Villain)
			{
				playSFXSignal.Dispatch("Play_villainArrives_01");
			}
			else if (currentState == global::Kampai.UI.View.CharacterWelcomeState.Farewell && type == global::Kampai.Game.PrestigeType.Villain)
			{
				playSFXSignal.Dispatch("Play_villainLeaves_01");
			}
		}
	}
}
