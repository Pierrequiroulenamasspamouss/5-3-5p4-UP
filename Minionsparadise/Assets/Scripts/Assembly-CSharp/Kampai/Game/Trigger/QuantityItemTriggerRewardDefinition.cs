namespace Kampai.Game.Trigger
{
	public class QuantityItemTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1181;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.QuantityItem;
			}
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
			if (base.transaction == null || context == null)
			{
				return;
			}
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = context.injectionBinder;
			bool flag = global::Kampai.Util.BuildingPacksHelper.UpdateTransactionUnlocksList(base.transaction, injectionBinder);
			global::Kampai.Game.Transaction.TransactionDefinition def = base.transaction.ToDefinition();
			bool flag2 = HandleLandExpansionUI(def, injectionBinder);
			global::Kampai.Game.IPlayerService instance = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			uint storageCount = instance.GetStorageCount();
			instance.RunEntireTransaction(def, global::Kampai.Game.TransactionTarget.AUTOMATIC, null, new global::Kampai.Game.TransactionArg
			{
				InstanceId = 301,
				Source = "TSMTrigger"
			});
			if (storageCount != instance.GetStorageCount())
			{
				injectionBinder.GetInstance<global::Kampai.UI.View.SetStorageCapacitySignal>().Dispatch();
			}
			if (flag)
			{
				injectionBinder.GetInstance<global::Kampai.UI.View.UpdateUIButtonsSignal>().Dispatch(false);
				if (!flag2)
				{
					OpenBuildingStoreUI(base.transaction, injectionBinder);
				}
			}
		}

		private void OpenBuildingStoreUI(global::Kampai.Game.Transaction.TransactionInstance transaction, global::strange.extensions.injector.api.ICrossContextInjectionBinder binder)
		{
			global::Kampai.Game.IDefinitionService instance = binder.GetInstance<global::Kampai.Game.IDefinitionService>();
			foreach (global::Kampai.Util.QuantityItem output in transaction.Outputs)
			{
				global::Kampai.Game.BuildingDefinition definition;
				if (instance.TryGet<global::Kampai.Game.BuildingDefinition>(output.ID, out definition))
				{
					binder.GetInstance<global::Kampai.UI.View.UpdateUIButtonsSignal>().Dispatch(false);
					binder.GetInstance<global::Kampai.UI.View.OpenStoreHighlightItemSignal>().Dispatch(definition.ID, true);
					break;
				}
			}
		}

		private bool HandleLandExpansionUI(global::Kampai.Game.Transaction.TransactionDefinition def, global::strange.extensions.injector.api.ICrossContextInjectionBinder binder)
		{
			global::Kampai.Game.IDefinitionService instance = binder.GetInstance<global::Kampai.Game.IDefinitionService>();
			global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("QuantityItemTriggerRewardDefinition") as global::Kampai.Util.IKampaiLogger;
			global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionConfig> list = new global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionConfig>();
			global::Kampai.Game.PurchasedLandExpansion byInstanceId = binder.GetInstance<global::Kampai.Game.IPlayerService>().GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			global::Kampai.Game.LandExpansionConfig landExpansionConfig = null;
			foreach (global::Kampai.Util.QuantityItem output in def.Outputs)
			{
				if (output.Quantity < 1)
				{
					continue;
				}
				int iD = output.ID;
				global::Kampai.Game.LandExpansionConfig definition;
				if (!instance.TryGet<global::Kampai.Game.LandExpansionConfig>(iD, out definition))
				{
					continue;
				}
				if (byInstanceId.PurchasedExpansions.Contains(definition.expansionId))
				{
					kampaiLogger.Info("Already owned: {0}\n", iD);
					continue;
				}
				if (landExpansionConfig == null)
				{
					landExpansionConfig = definition;
				}
				list.Add(definition);
			}
			if (list.Count > 0)
			{
				RunExpansionUI(list, landExpansionConfig, binder, instance);
			}
			return list.Count > 0;
		}

		private void RunExpansionUI(global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionConfig> expansions, global::Kampai.Game.LandExpansionConfig focusConfig, global::strange.extensions.injector.api.ICrossContextInjectionBinder binder, global::Kampai.Game.IDefinitionService definitionService)
		{
			if (focusConfig != null)
			{
				global::Kampai.Game.ILandExpansionService instance = binder.GetInstance<global::Kampai.Game.ILandExpansionService>();
				global::UnityEngine.Vector3 type = default(global::UnityEngine.Vector3);
				if (instance.HasForSaleSign(focusConfig.expansionId))
				{
					global::UnityEngine.GameObject forSaleSign = instance.GetForSaleSign(focusConfig.expansionId);
					type = forSaleSign.transform.position;
				}
				else
				{
					foreach (global::Kampai.Game.LandExpansionDefinition item in definitionService.GetAll<global::Kampai.Game.LandExpansionDefinition>())
					{
						if (item.ExpansionID == focusConfig.expansionId)
						{
							type = item.Location.ToVector3();
						}
					}
				}
				binder.GetInstance<global::Kampai.Game.HighlightLandExpansionSignal>().Dispatch(focusConfig.expansionId, true);
				binder.GetInstance<global::Kampai.Game.CameraAutoMoveSignal>().Dispatch(type, null, new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null), false);
			}
			foreach (global::Kampai.Game.LandExpansionConfig expansion in expansions)
			{
				binder.GetInstance<global::Kampai.Game.PurchaseLandExpansionSignal>().Dispatch(expansion.expansionId, focusConfig == expansion);
			}
		}
	}
}
