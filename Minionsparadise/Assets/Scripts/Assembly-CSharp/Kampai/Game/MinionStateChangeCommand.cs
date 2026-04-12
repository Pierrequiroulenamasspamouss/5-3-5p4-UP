namespace Kampai.Game
{
	public class MinionStateChangeCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::Kampai.Game.MinionState state { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService player { get; set; }

		[Inject]
		public global::Kampai.Game.PartySignal partySignal { get; set; }

		[Inject]
		public global::Kampai.Game.IdleMinionSignal idleSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Minion byInstanceId = player.GetByInstanceId<global::Kampai.Game.Minion>(minionID);
			if (byInstanceId == null)
			{
				return;
			}
			byInstanceId.State = state;
			byInstanceId.Partying = false;
			byInstanceId.IsInIncidental = false;
			if (state == global::Kampai.Game.MinionState.Idle)
			{
				idleSignal.Dispatch();
			}
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			if (component == null)
			{
				return;
			}
			global::UnityEngine.GameObject gameObject = component.GetGameObject(minionID);
			if (gameObject == null)
			{
				return;
			}
			global::UnityEngine.Collider component2 = gameObject.GetComponent<global::UnityEngine.Collider>();
			if (component2 != null)
			{
				if (state == global::Kampai.Game.MinionState.Tasking || state == global::Kampai.Game.MinionState.PlayingMignette)
				{
					component2.enabled = false;
				}
				else
				{
					component2.enabled = true;
				}
			}
			partySignal.Dispatch();
		}
	}
}
