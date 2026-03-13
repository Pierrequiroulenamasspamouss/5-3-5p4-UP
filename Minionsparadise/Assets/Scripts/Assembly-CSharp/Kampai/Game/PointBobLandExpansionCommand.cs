namespace Kampai.Game
{
	public class PointBobLandExpansionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PointBobLandExpansionCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.BobPointsAtSignSignal pointAtSignSignal { get; set; }

		[Inject]
		public global::Kampai.Game.FrolicSignal frolicSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BobReturnToTownSignal bobReturnToTown { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionConfigService landExpansionConfigService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.PurchasedLandExpansion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			global::Kampai.Game.BobCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.BobCharacter>(70002);
			bool flag = false;
			if (firstInstanceByDefinitionId == null)
			{
				return;
			}
			global::Kampai.Game.Prestige prestigeFromMinionInstance = prestigeService.GetPrestigeFromMinionInstance(firstInstanceByDefinitionId);
			if (prestigeFromMinionInstance == null)
			{
				return;
			}
			switch (prestigeFromMinionInstance.state)
			{
			case global::Kampai.Game.PrestigeState.Prestige:
			case global::Kampai.Game.PrestigeState.InQueue:
				if (prestigeFromMinionInstance.CurrentPrestigeLevel == 0)
				{
					return;
				}
				break;
			case global::Kampai.Game.PrestigeState.Locked:
			case global::Kampai.Game.PrestigeState.PreUnlocked:
			case global::Kampai.Game.PrestigeState.Questing:
				return;
			}
			if (playerService.HasTargetExpansion())
			{
				int targetExpansion = playerService.GetTargetExpansion();
				if (!byInstanceId.HasPurchased(targetExpansion))
				{
					logger.Info("Bob is already pointing at expansion: {0}", targetExpansion);
					PointAtExpansion(firstInstanceByDefinitionId.ID, targetExpansion);
					return;
				}
				logger.Info("Bob needs a new place to point");
				flag = true;
				playerService.ClearTargetExpansion();
			}
			global::System.Collections.Generic.IList<int> expansionIds = landExpansionConfigService.GetExpansionIds();
			global::System.Collections.Generic.List<int> list = FindAspirationalExpansions(expansionIds, byInstanceId);
			if (list.Count > 0)
			{
				int index = randomService.NextInt(list.Count);
				int num = list[index];
				playerService.SetTargetExpansion(num);
				logger.Info("Setting Bob to point at expansion: {0}", num);
			}
			if (playerService.HasTargetExpansion())
			{
				int targetExpansion2 = playerService.GetTargetExpansion();
				PointAtExpansion(firstInstanceByDefinitionId.ID, targetExpansion2);
				return;
			}
			logger.Info("No aspirational expansions for Bob to point at :-( ");
			if (flag)
			{
				bobReturnToTown.Dispatch();
			}
			frolicSignal.Dispatch(firstInstanceByDefinitionId.ID);
			removeWayFinderSignal.Dispatch(firstInstanceByDefinitionId.ID);
		}

		private global::System.Collections.Generic.List<int> FindAspirationalExpansions(global::System.Collections.Generic.IList<int> configIds, global::Kampai.Game.PurchasedLandExpansion purchasedLandExpansions)
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>(8);
			foreach (int configId in configIds)
			{
				global::Kampai.Game.LandExpansionConfig expansionConfig = landExpansionConfigService.GetExpansionConfig(configId);
				if (!purchasedLandExpansions.IsUnpurchasedAdjacentExpansion(expansionConfig.expansionId))
				{
					continue;
				}
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(expansionConfig.transactionId);
				foreach (global::Kampai.Util.QuantityItem output in transactionDefinition.Outputs)
				{
					global::Kampai.Game.LandExpansionConfig definition;
					if (definitionService.TryGet<global::Kampai.Game.LandExpansionConfig>(output.ID, out definition) && definition.containedAspirationalBuildings.Count > 0)
					{
						logger.Debug("Config: {0} Expansion: {1} Aspirational: {2}", configId, definition.expansionId, definition.containedAspirationalBuildings.Count);
						list.Add(configId);
					}
				}
			}
			return list;
		}

		private void PointAtExpansion(int bobID, int targetExpansionId)
		{
			global::Kampai.Game.LandExpansionConfig expansionConfig = landExpansionConfigService.GetExpansionConfig(targetExpansionId);
			global::UnityEngine.Vector3 type = new global::UnityEngine.Vector3(expansionConfig.routingSlot.x, 0f, expansionConfig.routingSlot.y);
			pointAtSignSignal.Dispatch(type);
			createWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(bobID));
		}
	}
}
