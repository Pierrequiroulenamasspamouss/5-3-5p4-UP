namespace Kampai.Game.View
{
	public class VillainMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.Game.View.VillainView view { get; set; }

		[Inject]
		public global::Kampai.Game.InitializeVillainSignal initVillainSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayVillainSignal displaySignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetVillainLairAnimationTriggerSignal beginAnimationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetMasterPlanRewardAnimatorSignal setAnimatorSingal { get; set; }

		public override void OnRegister()
		{
			initVillainSignal.AddListener(view.InitializeVillain);
			displaySignal.AddListener(view.DisplayVillain);
			beginAnimationSignal.AddListener(view.SetAnimTrigger);
			setAnimatorSingal.AddListener(view.SetMasterPlanRewardAnimation);
		}

		public override void OnRemove()
		{
			initVillainSignal.RemoveListener(view.InitializeVillain);
			displaySignal.RemoveListener(view.DisplayVillain);
			beginAnimationSignal.RemoveListener(view.SetAnimTrigger);
			setAnimatorSingal.RemoveListener(view.SetMasterPlanRewardAnimation);
		}
	}
}
