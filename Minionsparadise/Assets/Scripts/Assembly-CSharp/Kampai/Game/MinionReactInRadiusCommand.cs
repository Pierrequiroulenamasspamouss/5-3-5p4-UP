namespace Kampai.Game
{
	public class MinionReactInRadiusCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public float radius { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 reactionPosition { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionReactSignal reactSignal { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.ICollection<global::Kampai.Util.AI.Agent> collection = global::Kampai.Util.AI.Agent.Agents.WithinRange(reactionPosition, radius);
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			foreach (global::Kampai.Util.AI.Agent item in collection)
			{
				global::Kampai.Game.View.MinionObject component = item.GetComponent<global::Kampai.Game.View.MinionObject>();
				if (component != null)
				{
					global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(component.ID);
					if (byInstanceId.State == global::Kampai.Game.MinionState.Idle)
					{
						list.Add(byInstanceId.ID);
					}
				}
			}
			global::Kampai.Util.Boxed<global::UnityEngine.Vector3> type = new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(new global::UnityEngine.Vector3(reactionPosition.x, 0f, reactionPosition.z));
			reactSignal.Dispatch(list, type);
		}
	}
}
