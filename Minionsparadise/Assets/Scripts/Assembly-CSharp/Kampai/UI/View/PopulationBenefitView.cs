namespace Kampai.UI.View
{
	public class PopulationBenefitView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.KampaiImage PopulationIcon;

		public global::UnityEngine.UI.Text Goal;

		public global::UnityEngine.UI.Text Benefit;

		public global::UnityEngine.CanvasGroup lockedGroup;

		public float lockedAphaValue = 0.5f;

		private global::Kampai.Game.PopulationBenefitDefinition definition;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Main.ILocalizationService localizationService;

		private global::Kampai.Game.IPlayerService playerService;

		internal global::strange.extensions.signal.impl.Signal updateSignal = new global::strange.extensions.signal.impl.Signal();

		public int benefitDefinitionID { get; set; }

		public void Init(global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Main.ILocalizationService localizationService, global::Kampai.Game.IPlayerService playerService)
		{
			this.definitionService = definitionService;
			this.localizationService = localizationService;
			this.playerService = playerService;
			UpdateView(false);
		}

		public void UpdateView(bool checkDoober)
		{
			if (definitionService != null)
			{
				definition = definitionService.Get<global::Kampai.Game.PopulationBenefitDefinition>(benefitDefinitionID);
				Goal.text = localizationService.GetString("MinionUpgradePopulationGoal", definition.numMinionsRequired, definition.minionLevelRequired + 1);
				Benefit.text = GetPopulationBenfitText();
				SetIcon();
				lockedGroup.alpha = (IsBenefitUnlocked(definition.ID) ? 1f : lockedAphaValue);
				if (checkDoober)
				{
					updateSignal.Dispatch();
				}
			}
		}

		public void SetIcon()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(definition.transactionDefinitionID);
			int count = transactionDefinition.Outputs.Count;
			if (count > 0)
			{
				UIUtils.SetItemIcon(PopulationIcon, definitionService.Get<global::Kampai.Game.DisplayableDefinition>(global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputItem(transactionDefinition, 0).ID));
			}
		}

		public bool IsBenefitUnlocked(int definitionID)
		{
			global::Kampai.Game.MinionUpgradeBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MinionUpgradeBuilding>(375);
			return byInstanceId.processedPopulationBenefitDefinitionIDs.Contains(definitionID);
		}

		private string GetPopulationBenfitText()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(definition.transactionDefinitionID);
			int count = transactionDefinition.Outputs.Count;
			if (count <= 0)
			{
				return localizationService.GetString(definition.benefitDescriptionLocalizedKey);
			}
			object[] array = new object[count];
			for (int i = 0; i < count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = transactionDefinition.Outputs[i];
				if (quantityItem.ID == 335)
				{
					array[i] = UIUtils.FormatTime(quantityItem.Quantity, localizationService);
				}
				else
				{
					array[i] = quantityItem.Quantity;
				}
			}
			return localizationService.GetString(definition.benefitDescriptionLocalizedKey, array);
		}
	}
}
