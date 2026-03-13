namespace Kampai.Game
{
	public class TeleportMinionToBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionID { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			global::Kampai.Game.View.MinionObject minionObject = component.Get(minionID);
			if (minionObject != null && minionObject.currentAction != null && minionObject.currentAction is global::Kampai.Game.View.ConstantSpeedPathAction)
			{
				minionObject.currentAction.Abort();
				minionObject.StopLocalAudio();
			}
		}
	}
}
