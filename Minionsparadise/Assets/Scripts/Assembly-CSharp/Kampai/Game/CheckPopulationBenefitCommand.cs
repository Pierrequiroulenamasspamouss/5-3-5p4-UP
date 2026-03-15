namespace Kampai.Game
{
	public class CheckPopulationBenefitCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int currentLevel { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberModel dooberModel { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal playerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.PopulationBenefitDefinition> all = definitionService.GetAll<global::Kampai.Game.PopulationBenefitDefinition>();
			global::Kampai.Game.MinionUpgradeBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MinionUpgradeBuilding>(375);
			for (int i = 0; i < all.Count; i++)
			{
				global::Kampai.Game.PopulationBenefitDefinition populationBenefitDefinition = all[i];
				if (populationBenefitDefinition.minionLevelRequired == currentLevel)
				{
					int iD = populationBenefitDefinition.ID;
					if (!byInstanceId.processedPopulationBenefitDefinitionIDs.Contains(iD) && playerService.GetMinionCountAtOrAboveLevel(currentLevel) >= populationBenefitDefinition.numMinionsRequired)
					{
						playerService.RunEntireTransaction(populationBenefitDefinition.transactionDefinitionID, global::Kampai.Game.TransactionTarget.NO_VISUAL, null);
						byInstanceId.processedPopulationBenefitDefinitionIDs.Add(iD);
						dooberModel.PendingPopulationDoober = iD;
						playerTrainingSignal.Dispatch(19000029, false, new global::strange.extensions.signal.impl.Signal<bool>());
						telemetryService.Send_Telemetry_EVT_MINION_POPULATION_BENEFIT(populationBenefitDefinition.LocalizedKey);
					}
				}
			}
		}
	}
}
