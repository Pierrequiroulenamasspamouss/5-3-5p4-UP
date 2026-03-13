namespace Kampai.UI.View
{
	public class MasterPlanComponentInfoMediator : global::Kampai.UI.View.KampaiMediator
	{
		private global::Kampai.Game.MasterPlanDefinition planDefinition;

		private global::Kampai.Game.MasterPlanComponentDefinition componentDefinition;

		private global::Kampai.Game.MasterPlanComponent componentInstance;

		[Inject]
		public global::Kampai.UI.View.MasterPlanComponentInfoView view { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshFromIndexSignal refreshFromIndexSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			planDefinition = args.Get<global::Kampai.Game.MasterPlanDefinition>();
			view.Init(args.Get<global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType>(), args.Get<int>());
		}

		public override void OnRegister()
		{
			base.OnRegister();
			view.updateItemsSignal.AddListener(UpdateItemCallback);
			view.setTaskIconSignal.AddListener(SetTaskRequiresIcon);
			view.setRewardIconSignal.AddListener(SetRewardIcon);
			refreshFromIndexSignal.AddListener(view.Refresh);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.updateItemsSignal.RemoveListener(UpdateItemCallback);
			view.setTaskIconSignal.RemoveListener(SetTaskRequiresIcon);
			view.setRewardIconSignal.RemoveListener(SetRewardIcon);
			refreshFromIndexSignal.RemoveListener(view.Refresh);
		}

		public void UpdateItemCallback()
		{
			if (planDefinition != null)
			{
				componentDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentDefinition>(planDefinition.ComponentDefinitionIDs[view.index]);
				componentInstance = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(componentDefinition.ID);
				switch (view.itemType)
				{
				case global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType.Requires:
					UpdateRequiresIcons();
					break;
				case global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType.Rewards:
					UpdateRewardsIcons();
					break;
				}
			}
		}

		private void UpdateRewardsIcons()
		{
			global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem> list = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			list.Add(new global::Kampai.Util.QuantityItem(componentInstance.reward.Definition.rewardItemId, componentInstance.reward.Definition.rewardQuantity));
			global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem> list2 = list;
			if (componentInstance.reward.Definition.premiumReward != 0)
			{
				list2.Add(new global::Kampai.Util.QuantityItem(1, componentInstance.reward.Definition.premiumReward));
			}
			if (componentInstance.reward.Definition.grindReward != 0)
			{
				list2.Add(new global::Kampai.Util.QuantityItem(0, componentInstance.reward.Definition.grindReward));
			}
			list2.Sort((global::Kampai.Util.QuantityItem x, global::Kampai.Util.QuantityItem y) => x.ID.CompareTo(y.ID));
			view.SetupRewardIcons(list2);
		}

		private void UpdateRequiresIcons()
		{
			view.SetupTasksIcons(componentInstance.tasks);
		}

		private void SetRewardIcon(global::Kampai.Util.QuantityItem rewardItem, global::Kampai.UI.View.KampaiImage icon)
		{
			if (rewardItem != null)
			{
				global::Kampai.Game.DisplayableDefinition itemDefinitions = definitionService.Get<global::Kampai.Game.DisplayableDefinition>(rewardItem.ID);
				UIUtils.SetItemIcon(icon, itemDefinitions);
			}
		}

		private void SetTaskRequiresIcon(global::Kampai.Game.MasterPlanComponentTask task, global::Kampai.UI.View.KampaiImage icon)
		{
			task.Definition.GetTaskImage(icon, localizationService, definitionService, string.Empty);
		}
	}
}
