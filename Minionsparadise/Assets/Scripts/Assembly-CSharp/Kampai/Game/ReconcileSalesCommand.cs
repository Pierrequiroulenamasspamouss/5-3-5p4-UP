namespace Kampai.Game
{
	public class ReconcileSalesCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IUpsellService upsellService { get; set; }

		[Inject]
		public int triggerRewardSaleDefID { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateSaleBadgeSignal updateSaleBadgeSignal { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Sale> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Sale>();
			global::System.Collections.Generic.List<global::Kampai.Game.SalePackDefinition> all = definitionService.GetAll<global::Kampai.Game.SalePackDefinition>();
			global::System.Collections.Generic.List<global::Kampai.Game.Sale> salesToSchedule = new global::System.Collections.Generic.List<global::Kampai.Game.Sale>();
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			if (quantity < 1)
			{
				return;
			}
			foreach (global::Kampai.Game.SalePackDefinition item in all)
			{
				ProcessSale(item, instancesByType, ref salesToSchedule);
			}
			upsellService.ScheduleSales(salesToSchedule);
			updateSaleBadgeSignal.Dispatch();
		}

		private void ProcessSale(global::Kampai.Game.SalePackDefinition def, global::System.Collections.Generic.List<global::Kampai.Game.Sale> playerSales, ref global::System.Collections.Generic.List<global::Kampai.Game.Sale> salesToSchedule)
		{
			global::Kampai.Game.SalePackType type = def.Type;
			if (type == global::Kampai.Game.SalePackType.Redeemable)
			{
				return;
			}
			int iD = def.ID;
			global::Kampai.Game.Sale sale = upsellService.GetSaleInstanceFromID(playerSales, iD);
			if (sale != null && sale.Finished && !sale.Viewed)
			{
				sale.Viewed = true;
			}
			if (type == global::Kampai.Game.SalePackType.Upsell && PackUtil.HasPurchasedEnough(def, playerService))
			{
				if (sale != null)
				{
					playerService.Remove(sale);
				}
				definitionService.Remove(def);
				return;
			}
			if (def.UTCEndDate != 0 && timeService.CurrentTime() > def.UTCEndDate)
			{
				if (sale == null)
				{
					definitionService.Remove(def);
					return;
				}
				if (def.Duration <= 0 || sale.Finished)
				{
					playerService.Remove(sale);
					definitionService.Remove(def);
					return;
				}
			}
			if (sale == null)
			{
				if (def.UnlockByTrigger && triggerRewardSaleDefID != iD)
				{
					return;
				}
				sale = upsellService.AddNewSaleInstance(def);
			}
			else if (sale.Finished && !PackUtil.HasPurchasedEnough(def, playerService))
			{
				if (def.UnlockByTrigger && triggerRewardSaleDefID != iD)
				{
					return;
				}
				sale.Started = false;
				sale.UTCUserStartTime = 0;
				sale.Finished = false;
			}
			if (upsellService.IsSaleUpsellInstance(sale))
			{
				salesToSchedule.Add(sale);
			}
		}
	}
}
