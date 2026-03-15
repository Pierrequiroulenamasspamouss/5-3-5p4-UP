namespace Kampai.Tools.AnimationToolKit
{
	internal sealed class AnimationToolKitButtonMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonView view { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.AnimationToolkitModel Model { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.AnimationToolKit AnimationToolKit { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.LoadInterfaceSignal loadInterfaceSignal { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.ToggleInterfaceSignal toggleInterfaceSignal { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.ToggleMeshSignal toggleMeshSignal { get; set; }

		public override void OnRegister()
		{
			view.ButtonPressSignal.AddListener(OnButtonPress);
		}

		public override void OnRemove()
		{
			view.ButtonPressSignal.RemoveListener(OnButtonPress);
		}

		private void OnButtonPress(global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType buttonType)
		{
			switch (buttonType)
			{
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.MinionMode:
				Model.Mode = global::Kampai.Tools.AnimationToolKit.AnimationToolKitMode.Minion;
				loadInterfaceSignal.Dispatch();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.VillainMode:
				Model.Mode = global::Kampai.Tools.AnimationToolKit.AnimationToolKitMode.Villain;
				loadInterfaceSignal.Dispatch();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.CharacterMode:
				Model.Mode = global::Kampai.Tools.AnimationToolKit.AnimationToolKitMode.Character;
				loadInterfaceSignal.Dispatch();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.AddMinion:
				AnimationToolKit.AddMinion();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.RemoveMinion:
				AnimationToolKit.RemoveMinion();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.AddVillain:
				AnimationToolKit.AddVillain();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.RemoveVillain:
				AnimationToolKit.RemoveVillain();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.AddCharacter:
				AnimationToolKit.AddCharacter();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.RemoveCharacter:
				AnimationToolKit.RemoveCharacter();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.LoopAnimation:
				AnimationToolKit.LoopAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.GagAnimation:
				AnimationToolKit.GagAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.WaitAnimation:
				AnimationToolKit.WaitAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.VillainIntroAnimation:
				AnimationToolKit.VillainIntroAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.VillainCabanaAnimation:
				AnimationToolKit.VillainCabanaAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.VillainFarewellAnimation:
				AnimationToolKit.VillainFarewellAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.StuartStageIdleAnimation:
				AnimationToolKit.StuartStageIdleAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.StuartPerformAnimation:
				AnimationToolKit.StuartPerformAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.StuartCelebrateAnimation:
				AnimationToolKit.StuartCelebrateAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.StuartAttentionAnimation:
				AnimationToolKit.StuartAttentionAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.TikiBarCelebrateAnimation:
				AnimationToolKit.TikiBarCelebrateAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.TikiBarAttentionAnimation:
				AnimationToolKit.TikiBarAttentionAnimation();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.TikiBarSpyGlassAnimation:
				AnimationToolKit.TikiBarAlternateAnimation(1);
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.TikiBarMixDrinkAnimation:
				AnimationToolKit.TikiBarAlternateAnimation(0);
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.ToggleMesh:
				toggleMeshSignal.Dispatch();
				break;
			case global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.ToggleInterface:
				toggleInterfaceSignal.Dispatch();
				break;
			}
		}
	}
}
