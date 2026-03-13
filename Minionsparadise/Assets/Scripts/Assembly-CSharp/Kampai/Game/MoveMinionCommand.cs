namespace Kampai.Game
{
	public class MoveMinionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 goalPos { get; set; }

		[Inject]
		public bool muteMinion { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Game.MinionWalkPathSignal walkPathSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionRunPathSignal runPathSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionAppearSignal appearSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

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
				float num = global::UnityEngine.Random.Range(-0.5f, 0.5f);
				float num2 = global::UnityEngine.Random.Range(-0.5f, 0.5f);
				float num3 = 0.5f;
				for (int i = 1; i < list.Count - 1; i++)
				{
					list[i] = new global::UnityEngine.Vector3(list[i].x + num, list[i].y, list[i].z + num2);
					num = global::UnityEngine.Mathf.Clamp(num + global::UnityEngine.Random.Range(0f - num3, num3), -0.5f, 0.5f);
					num2 = global::UnityEngine.Mathf.Clamp(num2 + global::UnityEngine.Random.Range(0f - num3, num3), -0.5f, 0.5f);
				}
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionID);
				if (byInstanceId.State == global::Kampai.Game.MinionState.Selected)
				{
					runPathSignal.Dispatch(minionID, list, 3.7f, muteMinion);
					return;
				}
				float type = ((byInstanceId.State == global::Kampai.Game.MinionState.Idle) ? 2f : 4.5f);
				walkPathSignal.Dispatch(minionID, list, type, muteMinion);
			}
			else
			{
				global::Kampai.Util.Point point = new global::Kampai.Util.Point(global::UnityEngine.Mathf.RoundToInt(goalPos.x), global::UnityEngine.Mathf.RoundToInt(goalPos.z));
				if (environment.IsWalkable(point.x, point.y))
				{
					appearSignal.Dispatch(minionID, goalPos);
				}
			}
		}
	}
}
