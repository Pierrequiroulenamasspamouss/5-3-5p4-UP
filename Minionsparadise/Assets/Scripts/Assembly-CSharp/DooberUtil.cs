public static class DooberUtil
{
	public static void CheckForTween(global::Kampai.Game.Transaction.TransactionDefinition transactionDef, global::System.Collections.Generic.List<global::UnityEngine.GameObject> items, bool allowTweenToStorage, global::UnityEngine.Camera uiCamera, global::Kampai.UI.View.SpawnDooberSignal tweenSignal, global::Kampai.Game.IDefinitionService definitionService)
	{
		int outputCount = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(transactionDef);
		int count = items.Count;
		int num = ((outputCount > count) ? count : outputCount);
		for (int i = 0; i < num; i++)
		{
			global::Kampai.Util.QuantityItem quantityItem = transactionDef.Outputs[i];
			global::UnityEngine.RectTransform transform = items[i].transform as global::UnityEngine.RectTransform;
			DetermineTweenToUse(quantityItem.ID, transform, allowTweenToStorage, uiCamera, tweenSignal, definitionService);
		}
	}

	public static void CheckForTween(global::Kampai.Game.Transaction.TransactionDefinition transactionDef, global::System.Collections.Generic.List<global::Kampai.UI.View.KampaiImage> items, bool allowTweenToStorage, global::UnityEngine.Camera uiCamera, global::Kampai.UI.View.SpawnDooberSignal tweenSignal, global::Kampai.Game.IDefinitionService definitionService)
	{
		global::System.Collections.Generic.List<global::UnityEngine.GameObject> list = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
		for (int i = 0; i < items.Count; i++)
		{
			list.Add(items[i].gameObject);
		}
		CheckForTween(transactionDef, list, allowTweenToStorage, uiCamera, tweenSignal, definitionService);
	}

	public static void CheckForTween(global::Kampai.Game.Transaction.TransactionDefinition transactionDef, global::UnityEngine.RectTransform transform, bool allowTweenToStorage, global::UnityEngine.Camera uiCamera, global::Kampai.UI.View.SpawnDooberSignal tweenSignal, global::Kampai.Game.IDefinitionService definitionService, bool staggerTweens = false, global::Kampai.Util.IRoutineRunner routineRunner = null)
	{
		if (staggerTweens && routineRunner != null)
		{
			routineRunner.StartCoroutine(StaggerSingleTransformTweens(transactionDef, transform, allowTweenToStorage, uiCamera, tweenSignal, definitionService));
			return;
		}
		for (int i = 0; i < transactionDef.Outputs.Count; i++)
		{
			global::Kampai.Util.QuantityItem quantityItem = transactionDef.Outputs[i];
			DetermineTweenToUse(quantityItem.ID, transform, allowTweenToStorage, uiCamera, tweenSignal, definitionService);
		}
	}

	private static global::System.Collections.IEnumerator StaggerSingleTransformTweens(global::Kampai.Game.Transaction.TransactionDefinition transactionDef, global::UnityEngine.RectTransform transform, bool allowTweenToStorage, global::UnityEngine.Camera uiCamera, global::Kampai.UI.View.SpawnDooberSignal tweenSignal, global::Kampai.Game.IDefinitionService definitionService)
	{
		for (int i = 0; i < transactionDef.Outputs.Count; i++)
		{
			if (transform == null)
			{
				break;
			}
			global::Kampai.Util.QuantityItem output = transactionDef.Outputs[i];
			DetermineTweenToUse(output.ID, transform, allowTweenToStorage, uiCamera, tweenSignal, definitionService);
			yield return new global::UnityEngine.WaitForSeconds(0.5f);
		}
	}

	public static void CheckForTween(global::System.Collections.Generic.List<global::Kampai.UI.View.RewardSliderView> views, bool allowTweenToStorage, global::UnityEngine.Camera uiCamera, global::Kampai.UI.View.SpawnDooberSignal tweenSignal, global::Kampai.Game.IDefinitionService definitionService)
	{
		for (int i = 0; i < views.Count; i++)
		{
			global::UnityEngine.RectTransform transform = views[i].icon.transform as global::UnityEngine.RectTransform;
			DetermineTweenToUse(views[i].ID, transform, allowTweenToStorage, uiCamera, tweenSignal, definitionService);
		}
	}

	private static void DetermineTweenToUse(int id, global::UnityEngine.RectTransform transform, bool allowTweenToStorage, global::UnityEngine.Camera uiCamera, global::Kampai.UI.View.SpawnDooberSignal tweenSignal, global::Kampai.Game.IDefinitionService definitionService)
	{
		global::Kampai.UI.View.DestinationType destinationType = GetDestinationType(id, definitionService);
		if (allowTweenToStorage || destinationType != global::Kampai.UI.View.DestinationType.STORAGE)
		{
			tweenSignal.Dispatch(uiCamera.WorldToScreenPoint(transform.position), destinationType, id, false);
		}
	}

	public static global::Kampai.UI.View.DestinationType GetDestinationType(int definitionID, global::Kampai.Game.IDefinitionService definitionService)
	{
		global::Kampai.Game.Definition definition = definitionService.Get<global::Kampai.Game.Definition>(definitionID);
		switch (definitionID)
		{
		case 0:
			return global::Kampai.UI.View.DestinationType.GRIND;
		case 1:
			return global::Kampai.UI.View.DestinationType.PREMIUM;
		case 2:
			return global::Kampai.UI.View.DestinationType.XP;
		case 5:
			return global::Kampai.UI.View.DestinationType.MINIONS;
		case 50:
			return global::Kampai.UI.View.DestinationType.MINION_LEVEL_TOKEN;
		default:
			if (definition is global::Kampai.Game.StickerDefinition)
			{
				return global::Kampai.UI.View.DestinationType.STICKER;
			}
			if (definition is global::Kampai.Game.BuffDefinition)
			{
				return global::Kampai.UI.View.DestinationType.BUFF;
			}
			if (definition is global::Kampai.Game.BuildingDefinition)
			{
				return global::Kampai.UI.View.DestinationType.STORE;
			}
			return global::Kampai.UI.View.DestinationType.STORAGE;
		}
	}
}
