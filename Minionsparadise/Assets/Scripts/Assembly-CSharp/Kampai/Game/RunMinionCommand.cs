namespace Kampai.Game
{
	public class RunMinionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 goalPos { get; set; }

		[Inject]
		public float speed { get; set; }

		[Inject]
		public bool muteStatus { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Game.MinionWalkPathSignal walkPathSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionAppearSignal appearSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			if (!component.HasObject(minionID))
			{
				return;
			}
			global::UnityEngine.Vector3 objectPosition = component.GetObjectPosition(minionID);
			global::System.Collections.Generic.IList<global::UnityEngine.Vector3> list = pathFinder.FindPath(objectPosition, goalPos, 4);
			if (list != null)
			{
				walkPathSignal.Dispatch(minionID, list, speed, muteStatus);
				return;
			}
			global::Kampai.Util.Point point = new global::Kampai.Util.Point(global::UnityEngine.Mathf.RoundToInt(goalPos.x), global::UnityEngine.Mathf.RoundToInt(goalPos.z));
			if (environment.IsWalkable(point.x, point.y))
			{
				appearSignal.Dispatch(minionID, goalPos);
			}
		}
	}
}
