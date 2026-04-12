namespace Kampai.Game.View
{
	public class ControllerBuildingAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.AnimatingBuildingObject target;

		private global::UnityEngine.RuntimeAnimatorController controller;

		public ControllerBuildingAction(global::Kampai.Game.View.AnimatingBuildingObject target, global::UnityEngine.RuntimeAnimatorController controller, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.controller = controller;
			this.target = target;
		}

		public override void Execute()
		{
			target.SetAnimController(controller);
			global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObject = target as global::Kampai.Game.View.TaskableBuildingObject;
			if (taskableBuildingObject != null)
			{
				taskableBuildingObject.SetupLayers();
			}
			global::Kampai.Game.View.TikiBarBuildingObjectView tikiBarBuildingObjectView = target as global::Kampai.Game.View.TikiBarBuildingObjectView;
			if (tikiBarBuildingObjectView != null)
			{
				tikiBarBuildingObjectView.SetupLayers();
			}
			base.Done = true;
		}
	}
}
