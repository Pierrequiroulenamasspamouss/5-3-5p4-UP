namespace Kampai.Game
{
	public class TelemetryUtil : global::Kampai.Game.ITelemetryUtil
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		public void DetermineTaxonomy(global::Kampai.Game.Transaction.TransactionUpdateData update, bool concatenateType, out string highLevel, out string specific, out string type, out string other)
		{
			highLevel = string.Empty;
			specific = string.Empty;
			type = string.Empty;
			other = string.Empty;
			if (update.Outputs != null)
			{
				foreach (global::Kampai.Util.QuantityItem output in update.Outputs)
				{
					if (output.ID != 0 && output.ID <= 8)
					{
						continue;
					}
					global::Kampai.Game.TaxonomyDefinition definition = null;
					int iD = output.ID;
					if (update.Target == global::Kampai.Game.TransactionTarget.LAND_EXPANSION || update.Target == global::Kampai.Game.TransactionTarget.REPAIR_BRIDGE)
					{
						global::Kampai.Game.LandExpansionConfig definition2 = null;
						if (definitionService.TryGet<global::Kampai.Game.LandExpansionConfig>(output.ID, out definition2))
						{
							global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding> list = landExpansionService.GetBuildingsByExpansionID(definition2.expansionId) as global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>;
							if (list != null && list.Count > 0)
							{
								iD = list[0].Definition.ID;
							}
						}
					}
					if (definitionService.TryGet<global::Kampai.Game.TaxonomyDefinition>(iD, out definition))
					{
						DetermineTaxonomy(definition, concatenateType, ref highLevel, ref specific, ref type, ref other);
					}
				}
			}
			else if (update.Source == "MasterPlanRush")
			{
				global::Kampai.Game.TaxonomyDefinition definition3 = null;
				global::Kampai.Game.MasterPlanDefinition definition4 = null;
				int id = 65000;
				if (definitionService.TryGet<global::Kampai.Game.MasterPlanDefinition>(id, out definition4))
				{
					int buildingDefID = definition4.BuildingDefID;
					if (definitionService.TryGet<global::Kampai.Game.TaxonomyDefinition>(buildingDefID, out definition3))
					{
						DetermineTaxonomy(definition3, concatenateType, ref highLevel, ref specific, ref type, ref other);
					}
				}
			}
			if (update.InstanceId != 0)
			{
				global::Kampai.Game.TaxonomyDefinition definition5 = null;
				global::Kampai.Game.Instance byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Instance>(update.InstanceId);
				if (byInstanceId != null)
				{
					definition5 = byInstanceId.Definition as global::Kampai.Game.TaxonomyDefinition;
				}
				else
				{
					definitionService.TryGet<global::Kampai.Game.TaxonomyDefinition>(update.InstanceId, out definition5);
				}
				if (definition5 != null)
				{
					DetermineTaxonomy(definition5, false, ref highLevel, ref specific, ref type, ref other);
				}
			}
		}

		private void DetermineTaxonomy(global::Kampai.Game.TaxonomyDefinition taxonomyDef, bool concatenateType, ref string highLevel, ref string specific, ref string type, ref string other)
		{
			highLevel = SafeString(taxonomyDef.TaxonomyHighLevel);
			specific = SafeString(taxonomyDef.TaxonomySpecific);
			if (!concatenateType || string.IsNullOrEmpty(type))
			{
				type = SafeString(taxonomyDef.TaxonomyType);
			}
			else
			{
				string taxonomyType = taxonomyDef.TaxonomyType;
				if (!string.IsNullOrEmpty(taxonomyType))
				{
					type = type + ", " + taxonomyType;
				}
			}
			other = SafeString(taxonomyDef.TaxonomyOther);
		}

		private string SafeString(string input)
		{
			return input ?? string.Empty;
		}

		public string GetSourceName(global::Kampai.Game.Transaction.TransactionUpdateData update)
		{
			string text = update.Source;
			int instanceId = update.InstanceId;
			int transactionId = update.TransactionId;
			if (instanceId != 0 && string.IsNullOrEmpty(text))
			{
				global::Kampai.Game.Instance byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Instance>(instanceId);
				if (byInstanceId != null)
				{
					text = byInstanceId.Definition.LocalizedKey;
				}
			}
			else if (text == null && transactionId != 0)
			{
				global::Kampai.Game.Transaction.TransactionDefinition definition = null;
				if (definitionService.TryGet<global::Kampai.Game.Transaction.TransactionDefinition>(transactionId, out definition) && !string.IsNullOrEmpty(definition.LocalizedKey))
				{
					text = definition.LocalizedKey;
				}
			}
			global::Kampai.Game.Minion byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Minion>(instanceId);
			if (byInstanceId2 != null)
			{
				text = "Minion Level Up";
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "unknown";
			}
			return text;
		}
	}
}
