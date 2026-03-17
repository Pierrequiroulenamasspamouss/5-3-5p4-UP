namespace Kampai.Game
{
	public class LandExpansionPickController : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int pickEvent { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.SelectLandExpansionSignal selectLandExpansionSignal { get; set; }

		public override void Execute()
		{
			switch (pickEvent)
			{
			case 1:
				break;
			case 3:
				PickEnd();
				break;
			case 2:
				break;
			}
		}

		private void PickEnd()
		{
			if (!(model.EndHitObject == null) && !(model.StartHitObject != model.EndHitObject) && !model.DetectedMovement)
			{
				global::Kampai.Game.View.LandExpansionBuildingObject component = model.EndHitObject.GetComponent<global::Kampai.Game.View.LandExpansionBuildingObject>();
				selectLandExpansionSignal.Dispatch(component.ID);
			}
		}
	}
}
