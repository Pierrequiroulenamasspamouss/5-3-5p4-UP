namespace Kampai.Game.View
{
	public class EnvironmentalMignetteMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private const float RANGE_FOR_MINION_REACTION = 10f;

		[Inject]
		public global::Kampai.Common.MinionReactInRadiusSignal minionReactInRadiusSignal { get; set; }

		[Inject]
		public global::Kampai.Game.View.EnvironmentalMignetteView view { get; set; }

		[Inject]
		public global::Kampai.Common.EnvironmentalMignetteTappedSignal environmentalMignetteTappedSignal { get; set; }

		public override void OnRegister()
		{
			environmentalMignetteTappedSignal.AddListener(OnTappedSignal);
		}

		public override void OnRemove()
		{
			environmentalMignetteTappedSignal.RemoveListener(OnTappedSignal);
		}

		public void OnTappedSignal(global::UnityEngine.GameObject tappedGO)
		{
			view.AnimateEnvironmentalMignette(tappedGO);
			minionReactInRadiusSignal.Dispatch(10f, tappedGO.transform.position);
		}
	}
}
