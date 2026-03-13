namespace Kampai.Game
{
	public class MIBBuildingResourceIconSelectedCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MIBBuildingResourceIconSelectedCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.RemoveResourceIconSignal removeResourceIconSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMIBService mibService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSoundFXSignal { get; set; }

		public override void Execute()
		{
			if (!mibService.IsUserReturning())
			{
				return;
			}
			logger.Debug("MIB: User selected resource icon, showing reward UI");
			global::Kampai.Game.MIBBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MIBBuilding>(3129);
			removeResourceIconSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(firstInstanceByDefinitionId.ID, -1));
			mibService.ClearReturningKey();
			global::Kampai.Game.UserSegment segmentDefinition = GetSegmentDefinition(firstInstanceByDefinitionId);
			if (segmentDefinition == null)
			{
				logger.Error("MIB: Failed to find a suitable user segment");
				return;
			}
			int num = segmentDefinition.FirstXReturnRewardsWeightedDefinitionId;
			if (firstInstanceByDefinitionId.NumOfRewardsCollectedOnReturn > segmentDefinition.AfterXReturnRewards)
			{
				num = segmentDefinition.SecondXReturnRewardsWeightedDefinitionId;
			}
			logger.Info("MIB: Granting user reward {0} segmentedWeightedDefId:{1}", global::Kampai.Game.MIBRewardType.ON_RETURN, num);
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = mibService.PickWeightedTransaction(num);
			if (transactionDefinition == null)
			{
				logger.Error("MIB: Failed to find weighted definition id {0}, will not grant user rewards", num);
				return;
			}
			global::Kampai.UI.View.MIBRewardView mIBRewardView = CreateUIView();
			if (mIBRewardView == null)
			{
				logger.Error("MIB: Invalid reward prefab, can not grant user rewards");
				return;
			}
			mIBRewardView.Init(transactionDefinition, mibService.GetItemDefinition(transactionDefinition), mibService.GetItemDefinitions(num), playGlobalSoundFXSignal);
			playGlobalSoundFXSignal.Dispatch("Play_UI_levelUp_rewards_01");
		}

		private global::Kampai.Game.UserSegment GetSegmentDefinition(global::Kampai.Game.MIBBuilding building)
		{
			string dataPlayer = localPersistanceService.GetDataPlayer("IsSpender");
			global::System.Collections.Generic.List<global::Kampai.Game.UserSegment> list = null;
			list = ((!(dataPlayer == "true")) ? building.Definition.ReturnNonSpenderLevelSegments : building.Definition.ReturnSpenderLevelSegments);
			if (list == null || list.Count <= 0)
			{
				logger.Error("MIB: Failed to find segment, please check definition id: {0}", building.Definition.ID);
				return null;
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			for (int i = 0; i < list.Count; i++)
			{
				if (quantity >= list[i].LevelGreaterThanOrEqualTo)
				{
					return list[i];
				}
			}
			return null;
		}

		private global::Kampai.UI.View.MIBRewardView CreateUIView()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_MessageInABottle");
			iGUICommand.darkSkrim = true;
			iGUICommand.disableSkrimButton = true;
			iGUICommand.skrimScreen = "MIBRewardScreenSkrim";
			global::UnityEngine.GameObject gameObject = guiService.Execute(iGUICommand);
			return gameObject.GetComponent<global::Kampai.UI.View.MIBRewardView>();
		}
	}
}
