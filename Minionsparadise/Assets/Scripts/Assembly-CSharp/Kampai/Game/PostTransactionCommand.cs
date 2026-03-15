namespace Kampai.Game
{
	public class PostTransactionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PostTransactionCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject namedCharacterManager { get; set; }

		[Inject]
		public global::Kampai.Game.Transaction.TransactionUpdateData update { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal tweenSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.ITelemetryUtil telemetryUtil { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockCharacterModel unlockCharacterModel { get; set; }

		[Inject]
		public global::Kampai.UI.IBuildMenuService buildMenuService { get; set; }

		[Inject]
		public global::Kampai.Game.PlayerTrainingTransactionOutputExaminationSignal playerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.InitializeMarketplaceSlotsSignal initializeSlotsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ProcessSpecialSaleItemSignal processSpecialSaleItemSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public override void Execute()
		{
			if (update.Target != global::Kampai.Game.TransactionTarget.NO_VISUAL && update.Target != global::Kampai.Game.TransactionTarget.REWARD_BUILDING)
			{
				RunScreenTween();
			}
			sendTelemetry();
			questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Delivery);
			masterPlanService.ProcessTransactionData(update);
			playerTrainingSignal.Dispatch(update);
			processSpecialSaleItemSignal.Dispatch(update);
			CreateNewMinions();
			AddBuildMenuBadge();
			UpdateMarketplaceSlots();
		}

		private void RunScreenTween()
		{
			if (update.InstanceId == 0 && update.Outputs == null)
			{
				return;
			}
			global::UnityEngine.Vector3 startLocation = GetStartLocation(update);
			bool type = !update.fromGlass;
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs = update.Outputs;
			foreach (global::Kampai.Util.QuantityItem item in outputs)
			{
				if (item.Quantity != 0)
				{
					if (item.ID == 0)
					{
						tweenSignal.Dispatch(startLocation, global::Kampai.UI.View.DestinationType.GRIND, -1, type);
					}
					if (item.ID == 2)
					{
						tweenSignal.Dispatch(startLocation, global::Kampai.UI.View.DestinationType.XP, -1, type);
					}
					if (item.ID == 1)
					{
						tweenSignal.Dispatch(startLocation, global::Kampai.UI.View.DestinationType.PREMIUM, -1, type);
					}
				}
			}
		}

		private global::UnityEngine.Vector3 GetStartLocation(global::Kampai.Game.Transaction.TransactionUpdateData update)
		{
			global::UnityEngine.Vector3 result = global::UnityEngine.Vector3.zero;
			int instanceId = update.InstanceId;
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(instanceId);
			if (byInstanceId != null)
			{
				global::Kampai.Game.Location location = byInstanceId.Location;
				if (location != null)
				{
					result = new global::UnityEngine.Vector3(location.x, 0f, location.y);
				}
			}
			else
			{
				if (instanceId != 301)
				{
					return update.startPosition;
				}
				global::Kampai.Game.View.NamedCharacterManagerView component = namedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
				global::Kampai.Game.View.CharacterObject characterObject = component.Get(instanceId);
				if (characterObject != null)
				{
					result = new global::UnityEngine.Vector3(characterObject.transform.position.x, 0f, characterObject.transform.position.z);
				}
			}
			return result;
		}

		private void sendTelemetry()
		{
			string sourceName = telemetryUtil.GetSourceName(update);
			if (update.Inputs != null)
			{
				sendSpentTelemetry(sourceName);
			}
			if (update.Outputs != null)
			{
				sendEarnedTelemetry(sourceName);
			}
			SendBuildingAcquiredTelemetry(sourceName);
		}

		private void SendBuildingAcquiredTelemetry(string source)
		{
			if (update == null || update.Outputs == null)
			{
				return;
			}
			int sourceDefId = 0;
			global::Kampai.Game.PackDefinition packDefinitionFromTransactionId = definitionService.GetPackDefinitionFromTransactionId(update.TransactionId);
			if (packDefinitionFromTransactionId != null)
			{
				sourceDefId = packDefinitionFromTransactionId.ID;
			}
			for (int i = 0; i < update.Outputs.Count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = update.Outputs[i];
				global::Kampai.Game.BuildingDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(quantityItem.ID, out definition) && definition != null)
				{
					telemetryService.Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(source, definition, sourceDefId);
				}
			}
		}

		private void sendSpentTelemetry(string sourceName)
		{
			if (update.Inputs.Count == 0)
			{
				return;
			}
			string highLevel = string.Empty;
			string specific = string.Empty;
			string type = string.Empty;
			string other = string.Empty;
			telemetryUtil.DetermineTaxonomy(update, false, out highLevel, out specific, out type, out other);
			foreach (global::Kampai.Util.QuantityItem input in update.Inputs)
			{
				if (input.ID == 0)
				{
					uint quantity = input.Quantity;
					telemetryService.Send_Telemetry_EVT_IGE_FREE_CREDITS_PURCHASE_REVENUE((int)quantity, sourceName, global::Kampai.Game.PurchaseAwarePlayerService.PurchasedCurrencyUsed(logger, playerService, false, quantity), highLevel, specific, type);
					continue;
				}
				if (input.ID == 1)
				{
					uint quantity2 = input.Quantity;
					telemetryService.Send_Telemetry_EVT_IGE_PAID_CREDITS_PURCHASE_REVENUE((int)quantity2, sourceName, global::Kampai.Game.PurchaseAwarePlayerService.PurchasedCurrencyUsed(logger, playerService, true, quantity2), highLevel, specific, type);
					continue;
				}
				global::Kampai.Game.ItemDefinition definition = null;
				if (definitionService.TryGet<global::Kampai.Game.ItemDefinition>(input.ID, out definition) && !global::Kampai.Game.TransactionTarget.BLACKMARKETBOARD.Equals(update.Target))
				{
					SendCraftableEarnedSpentTelemetry(sourceName, (int)input.Quantity, definition, false);
				}
			}
		}

		private void sendEarnedTelemetry(string sourceName)
		{
			foreach (global::Kampai.Util.QuantityItem output in update.Outputs)
			{
				if (output.ID == 0)
				{
					string eventName = ((!sourceName.Equals("MasterPlan") && !sourceName.Equals("Quest")) ? questService.GetEventName(sourceName) : sourceName);
					telemetryService.Send_Telemetry_EVT_IGE_FREE_CREDITS_EARNED((int)output.Quantity, eventName, update.IsFromPremiumSource);
					continue;
				}
				if (output.ID == 1)
				{
					telemetryService.Send_Telemetry_EVT_IGE_PAID_CREDITS_EARNED((int)output.Quantity, sourceName, update.IsFromPremiumSource);
					continue;
				}
				if (output.ID == 2)
				{
					if (update.Target == global::Kampai.Game.TransactionTarget.REWARD_BUILDING)
					{
						telemetryService.Send_Telemetry_EVT_PARTY_POINTS_EARNED((int)output.Quantity, sourceName);
					}
					continue;
				}
				if (output.ID == 50)
				{
					telemetryService.Send_Telemetry_EVT_IGE_RESOURCE_CRAFTABLE_EARNED((int)output.Quantity, "Minion Level Up Token", "Minion Token", sourceName, string.Empty, string.Empty);
					continue;
				}
				global::Kampai.Game.IngredientsItemDefinition definition = null;
				if (definitionService.TryGet<global::Kampai.Game.IngredientsItemDefinition>(output.ID, out definition))
				{
					SendCraftableEarnedSpentTelemetry(sourceName, (int)output.Quantity, definition, true);
				}
			}
			if (update.TransactionId != 5037 || update.NewItems == null)
			{
				return;
			}
			foreach (global::Kampai.Game.Instance newItem in update.NewItems)
			{
				SendCraftableEarnedSpentTelemetry(sourceName, 1, newItem.Definition, true);
			}
		}

		private void SendCraftableEarnedSpentTelemetry(string sourceName, int quantity, global::Kampai.Game.Definition def, bool earned)
		{
			string localizedKey = def.LocalizedKey;
			string highLevel = string.Empty;
			string specific = highLevel;
			string type = highLevel;
			string other = highLevel;
			global::Kampai.Game.TaxonomyDefinition taxonomyDefinition = def as global::Kampai.Game.TaxonomyDefinition;
			if (earned)
			{
				string text = ((taxonomyDefinition == null) ? string.Empty : SafeString(taxonomyDefinition.TaxonomySpecific));
				telemetryUtil.DetermineTaxonomy(update, false, out highLevel, out specific, out type, out other);
				if (string.IsNullOrEmpty(highLevel) || specific == text)
				{
					highLevel = sourceName;
					specific = (type = (other = string.Empty));
				}
				telemetryService.Send_Telemetry_EVT_IGE_RESOURCE_CRAFTABLE_EARNED(quantity, localizedKey, text, highLevel, specific, type);
			}
			else
			{
				if (taxonomyDefinition != null)
				{
					highLevel = SafeString(taxonomyDefinition.TaxonomyHighLevel);
					specific = SafeString(taxonomyDefinition.TaxonomySpecific);
					type = SafeString(taxonomyDefinition.TaxonomyType);
					other = SafeString(taxonomyDefinition.TaxonomyOther);
				}
				telemetryService.Send_Telemetry_EVT_IGE_RESOURCE_CRAFTABLE_SPENT(quantity, sourceName, localizedKey, highLevel, specific, type);
			}
		}

		private string SafeString(string input)
		{
			return input ?? string.Empty;
		}

		private void CreateNewMinions()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> newItems = update.NewItems;
			if (newItems == null)
			{
				return;
			}
			global::Kampai.Game.Minion minion = null;
			int i = 0;
			for (int count = newItems.Count; i < count; i++)
			{
				global::Kampai.Game.QuantityInstance quantityInstance = newItems[i] as global::Kampai.Game.QuantityInstance;
				if (quantityInstance != null && quantityInstance.ID == 5)
				{
					global::Kampai.Game.Transaction.WeightedInstance weightedInstance = playerService.GetWeightedInstance(4007);
					for (int j = 0; j < quantityInstance.Quantity; j++)
					{
						global::Kampai.Util.QuantityItem quantityItem = weightedInstance.NextPick(randomService);
						global::Kampai.Game.MinionDefinition def = definitionService.Get<global::Kampai.Game.MinionDefinition>(quantityItem.ID);
						minion = new global::Kampai.Game.Minion(def);
						playerService.Add(minion);
						unlockCharacterModel.minionUnlocks.Add(minion);
					}
				}
				else
				{
					minion = newItems[i] as global::Kampai.Game.Minion;
					if (minion != null)
					{
						unlockCharacterModel.minionUnlocks.Add(minion);
					}
				}
			}
		}

		private void AddBuildMenuBadge()
		{
			global::Kampai.Game.PackDefinition packDefinition = null;
			if (update.Target != global::Kampai.Game.TransactionTarget.AUTOMATIC)
			{
				if (definitionService.GetPackTransaction(update.TransactionId) == null)
				{
					return;
				}
				packDefinition = definitionService.GetPackDefinitionFromTransactionId(update.TransactionId);
				if (packDefinition == null)
				{
					return;
				}
			}
			if (update.Outputs == null)
			{
				return;
			}
			for (int i = 0; i < update.Outputs.Count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = update.Outputs[i];
				global::Kampai.Game.BuildingDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(quantityItem.ID, out definition))
				{
					buildMenuService.CompleteBuildMenuUpdate(definition.Type, definition.ID);
				}
			}
		}

		private void UpdateMarketplaceSlots()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> newItems = update.NewItems;
			if (newItems == null)
			{
				return;
			}
			foreach (global::Kampai.Game.Instance item in newItems)
			{
				if (item.Definition != null && item.Definition.ID == 316)
				{
					initializeSlotsSignal.Dispatch();
					break;
				}
			}
		}
	}
}
