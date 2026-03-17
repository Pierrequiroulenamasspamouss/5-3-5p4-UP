namespace Kampai.Game
{
	public class MinionUpgradeCommand : global::strange.extensions.command.impl.Command
	{
		private int tokensToLevel;

		[Inject]
		public int minionInstanceID { get; set; }

		[Inject]
		public uint tokenQuantityPreLevel { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.PostMinionUpgradeSignal postUpgradeSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ChangeMinionCostumeSignal changeMinionCostumeSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Game.CheckPopulationBenefitSignal populationBenefitSignal { get; set; }

		public override void Execute()
		{
			playerService.LevelupMinion(minionInstanceID);
			questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.MinionUpgrade, global::Kampai.Game.QuestTaskTransition.Complete);
			questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.HaveUpgradedMinions);
			global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionInstanceID);
			if (byInstanceId != null)
			{
				global::Kampai.Game.MinionBenefitLevelBandDefintion minionBenefitLevelBandDefintion = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(global::Kampai.Game.StaticItem.MINION_BENEFITS_DEF_ID);
				tokensToLevel = minionBenefitLevelBandDefintion.minionBenefitLevelBands[byInstanceId.Level - 1].tokensToLevel;
				global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
				global::Kampai.Game.View.MinionObject minionObject = component.Get(minionInstanceID);
				if (minionObject != null)
				{
					int costumeId = minionBenefitLevelBandDefintion.GetMinionBenefit(byInstanceId.Level).costumeId;
					global::Kampai.Game.CostumeItemDefinition costumeItemDefinition = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(costumeId);
					if (costumeItemDefinition != null)
					{
						changeMinionCostumeSignal.Dispatch(minionObject, costumeItemDefinition);
					}
				}
			}
			SendTelemetry();
			populationBenefitSignal.Dispatch(byInstanceId.Level);
			postUpgradeSignal.Dispatch();
		}

		private void SendTelemetry()
		{
			int newLevel = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionInstanceID).Level + 1;
			telemetryService.Send_Telemetry_EVT_MINION_UPGRADE(newLevel, tokensToLevel, tokenQuantityPreLevel);
		}
	}
}
