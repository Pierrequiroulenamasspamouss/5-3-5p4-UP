namespace Kampai.Game
{
	public class CheckResourceBuildingSlotsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CheckResourceBuildingSlotsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			foreach (global::Kampai.Game.ResourceBuilding item in playerService.GetInstancesByType<global::Kampai.Game.ResourceBuilding>())
			{
				if (item.MinionSlotsOwned == 3)
				{
					continue;
				}
				int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
				int num = item.BuildingNumber - 1;
				if (num < 0)
				{
					num = playerService.GetInstancesByDefinitionID(item.Definition.ID).Count;
				}
				global::System.Collections.Generic.IList<global::Kampai.Game.SlotUnlock> slotUnlocks = item.Definition.SlotUnlocks;
				if (num >= slotUnlocks.Count)
				{
					logger.Error("CheckResourceBuildingsSlotsCommand: Data issue: Building number {0} is outside of slot unlocks range.", num);
					num = slotUnlocks.Count - 1;
				}
				global::System.Collections.Generic.IList<int> slotUnlockLevels = slotUnlocks[num].SlotUnlockLevels;
				if (slotUnlockLevels[slotUnlockLevels.Count - 1] < quantity)
				{
					continue;
				}
				foreach (int item2 in slotUnlockLevels)
				{
					if (item2 == quantity)
					{
						playerService.PurchaseSlotForBuilding(item.ID, item2);
					}
				}
			}
		}
	}
}
