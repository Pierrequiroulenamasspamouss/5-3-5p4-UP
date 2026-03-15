namespace Kampai.Game.View
{
	public class KevinMediator : global::Kampai.Game.View.FrolicCharacterMediator
	{
		[Inject]
		public global::Kampai.Game.View.KevinView kevinView { get; set; }

		[Inject]
		public global::Kampai.Game.KevinGreetVillainSignal greetVillainSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AnimateKevinSignal animateKevinSignal { get; set; }

		[Inject]
		public global::strange.extensions.injector.api.IInjectionBinder injectionBinder { get; set; }

		[Inject]
		public global::Kampai.Game.ReleaseMinionFromTikiBarSignal releaseMinionFromTikiBarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetKevinAnimatorCullingModeSignal setKevinAnimatorCullingModeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetVillainLairAnimationTriggerSignal setAnimTriggerSignal { get; set; }

		[Inject]
		public global::Kampai.Game.KevinGoToWelcomeHutSignal gotoWelcomeHutSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			greetVillainSignal.AddListener(HandleGreeting);
			animateKevinSignal.AddListener(AnimateKevin);
			setKevinAnimatorCullingModeSignal.AddListener(kevinView.SetAnimatorCullingMode);
			setAnimTriggerSignal.AddListener(kevinView.SetAnimTrigger);
			gotoWelcomeHutSignal.AddListener(GotoWelcomeHut);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			greetVillainSignal.RemoveListener(HandleGreeting);
			animateKevinSignal.RemoveListener(AnimateKevin);
			setKevinAnimatorCullingModeSignal.RemoveListener(kevinView.SetAnimatorCullingMode);
			setAnimTriggerSignal.RemoveListener(kevinView.SetAnimTrigger);
			gotoWelcomeHutSignal.RemoveListener(GotoWelcomeHut);
		}

		private void AnimateKevin(string animation)
		{
			switch (animation)
			{
			case "walk":
				kevinView.Walk(true);
				break;
			case "idle":
				kevinView.Walk(false);
				break;
			}
		}

		private void HandleGreeting(bool shouldGreet)
		{
			kevinView.GreetVillain(shouldGreet);
			releaseMinionFromTikiBarSignal.Dispatch(base.playerService.GetByInstanceId<global::Kampai.Game.Character>(base.view.ID), true);
		}

		private void GotoWelcomeHut(bool pop)
		{
			kevinView.GotoWelcomeHut(injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.StaticItem.WELCOME_BOOTH_BUILDING_ID_DEF), pop);
		}
	}
}
