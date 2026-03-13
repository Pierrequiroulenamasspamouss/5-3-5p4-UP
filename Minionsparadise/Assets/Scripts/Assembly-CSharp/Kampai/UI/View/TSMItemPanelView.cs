namespace Kampai.UI.View
{
	public class TSMItemPanelView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.UpsellButtonView buttonView;

		public global::UnityEngine.GameObject itemPanel;

		[global::UnityEngine.Header("Prefabs")]
		public global::UnityEngine.GameObject ItemPrefab;

		[global::UnityEngine.SerializeField]
		private float m_layoutSpacing = 25f;

		private readonly global::System.Collections.Generic.IList<global::Kampai.UI.View.TSMQuantityItemView> m_views = new global::System.Collections.Generic.List<global::Kampai.UI.View.TSMQuantityItemView>();

		private global::Kampai.Util.IKampaiLogger m_logger;

		private global::Kampai.Game.Trigger.TriggerRewardDefinition m_reward;

		private global::Kampai.Main.ILocalizationService m_localizationService;

		private global::Kampai.Game.ICurrencyService m_currencyService;

		private global::Kampai.Game.IDefinitionService m_definitionService;

		private global::Kampai.Game.IUpsellService m_upsellService;

		private global::Kampai.Game.IPlayerService m_playerService;

		private global::System.Action<global::Kampai.Game.Trigger.TriggerRewardDefinition> m_onPurchaseCallback;

		public void Init(global::Kampai.Game.Trigger.TriggerRewardDefinition reward, global::UnityEngine.Transform parent, global::Kampai.Game.ICurrencyService currencyService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::System.Action<global::Kampai.Game.Trigger.TriggerRewardDefinition> onPurchaseCallback)
		{
			m_reward = reward;
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			m_logger = global::Elevation.Logging.LogManager.GetClassLogger("TSMItemPanelView") as global::Kampai.Util.IKampaiLogger;
			m_currencyService = currencyService;
			m_definitionService = injectionBinder.GetInstance<global::Kampai.Game.IDefinitionService>();
			m_playerService = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			m_localizationService = injectionBinder.GetInstance<global::Kampai.Main.ILocalizationService>();
			m_onPurchaseCallback = onPurchaseCallback;
			m_upsellService = injectionBinder.GetInstance<global::Kampai.Game.IUpsellService>();
			base.gameObject.transform.SetParent(parent, false);
			SetUpView();
			buttonView.ClickedSignal.AddListener(OnButtonClickCallback);
		}

		private void SetUpView()
		{
			SetUpCollectButton();
			SetItemPanelLayout();
			SetUpItemLayouts(m_reward.layoutElements);
		}

		public void Disable()
		{
			for (int i = 0; i < m_views.Count; i++)
			{
				if (!(m_views[i] == null))
				{
					m_views[i].Disable();
				}
			}
			DisableButton();
		}

		private void OnButtonClickCallback()
		{
			if (buttonView.isDoubleConfirmed())
			{
				Disable();
				m_onPurchaseCallback(m_reward);
			}
		}

		public void TranactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				Disable();
				if (m_onPurchaseCallback != null)
				{
					m_onPurchaseCallback(m_reward);
				}
			}
		}

		public void SetUpCollectButton()
		{
			buttonView.Init(m_upsellService, m_playerService, m_currencyService, m_definitionService, m_localizationService);
			global::Kampai.Game.PremiumCurrencyItemDefinition definition = null;
			string sKU = string.Empty;
			if (m_definitionService.TryGet<global::Kampai.Game.PremiumCurrencyItemDefinition>(m_reward.SKUId, out definition))
			{
				sKU = definition.SKU;
			}
			buttonView.SetupButton(m_reward.transaction, sKU, 0, m_reward.IsFree, m_reward.buttonText);
		}

		internal void DisableButton()
		{
			buttonView.Disable();
		}

		protected override void OnDestroy()
		{
			Release();
			base.OnDestroy();
		}

		public void Release()
		{
			for (int i = 0; i < m_views.Count; i++)
			{
				ReleaseItem(m_views[i]);
			}
			m_views.Clear();
		}

		private static void ReleaseItem(global::UnityEngine.MonoBehaviour behaviour)
		{
			if (!(behaviour == null) && !(behaviour.gameObject == null))
			{
				global::UnityEngine.Object.Destroy(behaviour.gameObject);
			}
		}

		public void SetUpItemLayouts(global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerRewardLayout> layouts)
		{
			if (layouts == null || layouts.Count == 0)
			{
				global::UnityEngine.Transform parent = SetUpLayout(itemPanel, new global::Kampai.Game.Trigger.TriggerRewardLayout(), m_layoutSpacing);
				for (int i = 0; i < global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(m_reward.transaction); i++)
				{
					SetUpItemView(global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputItem(m_reward.transaction, i).ID, parent);
				}
				return;
			}
			for (int j = 0; j < layouts.Count; j++)
			{
				global::Kampai.Game.Trigger.TriggerRewardLayout triggerRewardLayout = layouts[j];
				global::System.Collections.Generic.IList<int> itemIds = triggerRewardLayout.itemIds;
				global::UnityEngine.Transform parent2 = SetUpLayout(itemPanel, triggerRewardLayout, m_layoutSpacing);
				for (int k = 0; k < itemIds.Count; k++)
				{
					SetUpItemView(itemIds[k], parent2);
				}
			}
		}

		private void SetItemPanelLayout()
		{
			global::UnityEngine.UI.HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = null;
			switch (m_reward.rewardLayout)
			{
			case global::Kampai.Game.Trigger.TriggerRewardLayout.Layout.None:
			case global::Kampai.Game.Trigger.TriggerRewardLayout.Layout.Horizontal:
				horizontalOrVerticalLayoutGroup = itemPanel.AddComponent<global::UnityEngine.UI.HorizontalLayoutGroup>();
				break;
			case global::Kampai.Game.Trigger.TriggerRewardLayout.Layout.Vertical:
				horizontalOrVerticalLayoutGroup = itemPanel.AddComponent<global::UnityEngine.UI.VerticalLayoutGroup>();
				break;
			}
			if (!(horizontalOrVerticalLayoutGroup == null))
			{
				horizontalOrVerticalLayoutGroup.childForceExpandHeight = false;
				horizontalOrVerticalLayoutGroup.childForceExpandWidth = false;
			}
		}

		public void SetUpItemView(int itemId, global::UnityEngine.Transform parent)
		{
			global::Kampai.Game.Definition definition;
			bool flag = m_definitionService.TryGet<global::Kampai.Game.Definition>(itemId, out definition);
			if (flag && (!flag || (itemId != 2 && !(definition is global::Kampai.Game.UnlockDefinition))))
			{
				global::Kampai.UI.View.TSMQuantityItemView tSMQuantityItemView = SetItemView(ItemPrefab, parent, itemId);
				if (tSMQuantityItemView == null)
				{
					m_logger.Error("Item view is null for item id {0}", itemId);
				}
				else
				{
					m_views.Add(tSMQuantityItemView);
				}
			}
		}

		private global::Kampai.UI.View.TSMQuantityItemView SetItemView(global::UnityEngine.GameObject prefab, global::UnityEngine.Transform parent, int itemId)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(prefab, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
			if (gameObject == null)
			{
				m_logger.Error("Couldn't create game object for {0} prefab", prefab);
				return null;
			}
			SetUpItemTransform(gameObject.transform, parent);
			global::Kampai.UI.View.TSMQuantityItemView component = gameObject.GetComponent<global::Kampai.UI.View.TSMQuantityItemView>();
			if (component == null)
			{
				m_logger.Error("Couldn't get TSMQuantityItemView from prefab {0}", prefab);
				global::UnityEngine.Object.Destroy(gameObject);
				return null;
			}
			global::Kampai.Util.QuantityItem outputItemId = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputItemId(m_reward.transaction, itemId);
			if (outputItemId == null)
			{
				m_logger.Error("Couldn't find item id {0} in transaction {1}", itemId, m_reward.transaction);
				global::UnityEngine.Object.Destroy(gameObject);
				return null;
			}
			return component.Init(m_definitionService.Get<global::Kampai.Game.DisplayableDefinition>(outputItemId.ID), outputItemId.Quantity, m_logger, m_localizationService);
		}

		private static void SetUpItemTransform(global::UnityEngine.Transform transform, global::UnityEngine.Transform parent)
		{
			if (!(transform == null))
			{
				transform.SetParent(parent, false);
				global::UnityEngine.RectTransform rectTransform = transform as global::UnityEngine.RectTransform;
				if (!(rectTransform == null))
				{
					rectTransform.anchorMin = global::UnityEngine.Vector2.zero;
					rectTransform.anchorMax = global::UnityEngine.Vector2.one;
					rectTransform.sizeDelta = global::UnityEngine.Vector2.zero;
					rectTransform.localScale = global::UnityEngine.Vector3.one;
					rectTransform.localPosition = global::UnityEngine.Vector3.zero;
				}
			}
		}

		private static global::UnityEngine.Transform SetUpLayout(global::UnityEngine.GameObject itemPanel, global::Kampai.Game.Trigger.TriggerRewardLayout layout, float layoutSpacing)
		{
			global::UnityEngine.Transform transform = null;
			global::UnityEngine.UI.HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = null;
			switch (layout.layout)
			{
			case global::Kampai.Game.Trigger.TriggerRewardLayout.Layout.None:
				return itemPanel.transform;
			case global::Kampai.Game.Trigger.TriggerRewardLayout.Layout.Horizontal:
			{
				global::UnityEngine.GameObject gameObject2 = new global::UnityEngine.GameObject("Horizontal Layout Group");
				horizontalOrVerticalLayoutGroup = gameObject2.AddComponent<global::UnityEngine.UI.HorizontalLayoutGroup>();
				transform = gameObject2.transform;
				break;
			}
			case global::Kampai.Game.Trigger.TriggerRewardLayout.Layout.Vertical:
			{
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Vertical Layout Group");
				horizontalOrVerticalLayoutGroup = gameObject.AddComponent<global::UnityEngine.UI.VerticalLayoutGroup>();
				transform = gameObject.transform;
				break;
			}
			}
			if (transform == null)
			{
				return null;
			}
			transform.SetParent(itemPanel.transform, false);
			transform.SetSiblingIndex(layout.index);
			if (horizontalOrVerticalLayoutGroup == null)
			{
				return transform;
			}
			horizontalOrVerticalLayoutGroup.childForceExpandHeight = false;
			horizontalOrVerticalLayoutGroup.childForceExpandWidth = false;
			horizontalOrVerticalLayoutGroup.spacing = layoutSpacing;
			return transform;
		}
	}
}
