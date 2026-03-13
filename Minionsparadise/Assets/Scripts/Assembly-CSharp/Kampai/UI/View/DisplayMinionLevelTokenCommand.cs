namespace Kampai.UI.View
{
	public class DisplayMinionLevelTokenCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.VillainLairEntranceBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(374);
			uint quantity = playerService.GetQuantity(global::Kampai.Game.StaticItem.PENDING_MINION_LEVEL_TOKEN);
			if (byInstanceId != null && quantity != 0)
			{
				global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "cmp_MinionLevelToken");
				guiService.Execute(command);
			}
		}
	}
}
