namespace Kampai.Game.View
{
	public class FootprintMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.Game.ShowBuildingFootprintSignal showSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateMovementValidity updateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.View.FootprintView view { get; set; }

		private void Init()
		{
			view.Init();
		}

		public override void OnRegister()
		{
			Init();
			showSignal.AddListener(ToggleFootprint);
			updateSignal.AddListener(UpdateFootprint);
		}

		public override void OnRemove()
		{
			showSignal.RemoveListener(ToggleFootprint);
			updateSignal.RemoveListener(UpdateFootprint);
		}

		private void UpdateFootprint(bool valid)
		{
			view.UpdateFootprint(valid);
		}

		private void ToggleFootprint(global::Kampai.Game.View.ActionableObject parent, global::UnityEngine.Transform parentTransform, global::Kampai.Util.Tuple<int, int> size, bool enable)
		{
			if (enable)
			{
				view.ParentFootprint(parent, parentTransform, size.Item1, size.Item2);
			}
			else
			{
				view.Reset();
			}
			view.ToggleFootprint(enable);
		}
	}
}
