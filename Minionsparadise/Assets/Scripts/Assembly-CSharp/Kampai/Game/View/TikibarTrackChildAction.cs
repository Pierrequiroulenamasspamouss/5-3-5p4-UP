namespace Kampai.Game.View
{
	public class TikibarTrackChildAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.TikiBarBuildingObjectView target;

		private global::UnityEngine.RuntimeAnimatorController controller;

		private global::Kampai.Game.View.CharacterObject child;

		private int routeIndex;

		private global::Kampai.Game.GetNewQuestSignal getNewQuestSignal;

		public TikibarTrackChildAction(global::Kampai.Game.View.TikiBarBuildingObjectView target, global::Kampai.Game.View.CharacterObject child, int routeIndex, global::UnityEngine.RuntimeAnimatorController controller, global::Kampai.Game.GetNewQuestSignal getNewQuestSignal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.controller = controller;
			this.target = target;
			this.child = child;
			this.routeIndex = routeIndex;
			this.getNewQuestSignal = getNewQuestSignal;
		}

		public override void Execute()
		{
			target.TrackChild(child, controller, routeIndex);
			getNewQuestSignal.Dispatch();
			base.Done = true;
		}
	}
}
