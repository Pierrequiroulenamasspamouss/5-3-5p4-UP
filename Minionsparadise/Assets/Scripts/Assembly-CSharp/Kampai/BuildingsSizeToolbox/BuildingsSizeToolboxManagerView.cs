namespace Kampai.BuildingsSizeToolbox
{
	public class BuildingsSizeToolboxManagerView : global::Kampai.Util.KampaiView
	{
		public class UIPositionInfo
		{
			public global::UnityEngine.Vector3 Position;

			public float Scale;

			public UIPositionInfo()
			{
			}

			public UIPositionInfo(global::UnityEngine.Vector3 pos, float scale)
			{
				Position = pos;
				Scale = scale;
			}
		}

		public global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolkitValueEditView XInputView;

		public global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolkitValueEditView YInputView;

		public global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolkitValueEditView ZInputView;

		public global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolkitValueEditView SInputView;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("BuildingsSizeToolboxManagerView") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.UI.View.UpSell.UpSellModalView currentView;

		private global::Kampai.UI.View.UpSell.UpSellItemView[] upsellItemViews;

		private global::Kampai.Game.BuildingDefinition currentBuildingDefinition;

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo> resetInfo = new global::System.Collections.Generic.Dictionary<int, global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo>();

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo> buildingsInfo = new global::System.Collections.Generic.Dictionary<int, global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo>();

		[Inject]
		public global::Kampai.BuildingsSizeToolbox.NewUpsellScreenSelectedSignal upsellScreenSelectedSignal { get; set; }

		[Inject]
		public global::Kampai.BuildingsSizeToolbox.BuildingSelectedSignal buildingSelectedSignal { get; set; }

		[Inject]
		public global::Kampai.BuildingsSizeToolbox.BuildingModifiedSignal buildingModifiedSignal { get; set; }

		[Inject]
		public global::Kampai.BuildingsSizeToolbox.BuildingsStateSavedSignal buildingStateAppliedSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal { get; set; }

		protected override void Start()
		{
			base.Start();
			upsellScreenSelectedSignal.AddListener(onUpsellScreenSelectedSignal);
			buildingSelectedSignal.AddListener(onBuildingSelectedSignal);
			XInputView.ValueChangedSignal.AddListener(positionXChanged);
			YInputView.ValueChangedSignal.AddListener(positionYChanged);
			ZInputView.ValueChangedSignal.AddListener(positionZChanged);
			SInputView.ValueChangedSignal.AddListener(scaleChanged);
		}

		private void onUpsellScreenSelectedSignal(global::Kampai.UI.View.UpSell.UpSellModalView view)
		{
			upsellItemViews = null;
			currentView = view;
			StartCoroutine(updateUpsellViewState());
		}

		private void setupValueFields(global::Kampai.Game.BuildingDefinition def)
		{
			global::UnityEngine.Vector3 uiPosition = def.UiPosition;
			XInputView.CurrentValue = uiPosition.x;
			YInputView.CurrentValue = uiPosition.y;
			ZInputView.CurrentValue = uiPosition.z;
			SInputView.CurrentValue = def.UiScale;
		}

		private void onBuildingSelectedSignal(global::Kampai.Game.BuildingDefinition def)
		{
			if (def != currentBuildingDefinition)
			{
				currentBuildingDefinition = def;
				StartCoroutine(updateUpsellViewState());
				setupValueFields(def);
				if (!resetInfo.ContainsKey(def.ID))
				{
					resetInfo.Add(def.ID, new global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo(def.UiPosition, def.UiScale));
				}
			}
		}

		private global::System.Collections.IEnumerator updateUpsellViewState(int framesToWait = 1)
		{
			for (int i = 0; i < framesToWait; i++)
			{
				yield return null;
			}
			if (currentBuildingDefinition != null && !(currentView == null))
			{
				if (upsellItemViews != null)
				{
					resetPreviousState();
				}
				global::UnityEngine.Transform[] itemParents = currentView.itemTransforms;
				global::UnityEngine.GameObject itemViewPrefab = currentView.itemViewPrefab;
				upsellItemViews = new global::Kampai.UI.View.UpSell.UpSellItemView[itemParents.Length];
				for (int j = 0; j < itemParents.Length; j++)
				{
					global::Kampai.UI.View.UpSell.UpSellItemView itemView = createItemView(itemViewPrefab, itemParents[j]);
					global::UnityEngine.Rect firstItemTrasformRect = (itemParents[0].transform as global::UnityEngine.RectTransform).rect;
					global::UnityEngine.Rect parentTransformRect = (itemView.transform.parent.transform as global::UnityEngine.RectTransform).rect;
					itemView.AdditionalUIScale = global::UnityEngine.Mathf.Min(parentTransformRect.width / firstItemTrasformRect.width, parentTransformRect.height / firstItemTrasformRect.height);
					itemView.Item = new global::Kampai.Util.QuantityItem(currentBuildingDefinition.ID, 1u);
					itemView.Init(localizationService, fancyUIService, definitionService, logger, moveAudioListenerSignal);
					upsellItemViews[j] = itemView;
				}
			}
		}

		private void resetPreviousState()
		{
			global::Kampai.UI.View.UpSell.UpSellItemView[] array = upsellItemViews;
			foreach (global::Kampai.UI.View.UpSell.UpSellItemView upSellItemView in array)
			{
				global::UnityEngine.Object.Destroy(upSellItemView.gameObject);
			}
			upsellItemViews = null;
		}

		private global::Kampai.UI.View.UpSell.UpSellItemView createItemView(global::UnityEngine.GameObject prefab, global::UnityEngine.Transform parent)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(prefab, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
			if (gameObject == null)
			{
				logger.Error("Could not create UpSellItemView from prefab");
				return null;
			}
			global::Kampai.UI.View.UpSell.UpSellItemView component = gameObject.GetComponent<global::Kampai.UI.View.UpSell.UpSellItemView>();
			if (component == null)
			{
				logger.Error("Could not get UpSellItemView from prefab");
				return null;
			}
			component.transform.SetParent(parent);
			setUpItemTransform(component.transform as global::UnityEngine.RectTransform);
			return component;
		}

		private void setUpItemTransform(global::UnityEngine.RectTransform rect)
		{
			if (!(rect == null))
			{
				rect.anchorMin = global::UnityEngine.Vector2.zero;
				rect.anchorMax = global::UnityEngine.Vector2.one;
				rect.sizeDelta = global::UnityEngine.Vector2.zero;
				rect.localScale = global::UnityEngine.Vector3.one;
				rect.localPosition = global::UnityEngine.Vector3.zero;
			}
		}

		private void positionXChanged(float v)
		{
			if (currentBuildingDefinition != null)
			{
				global::UnityEngine.Vector3 uiPosition = currentBuildingDefinition.UiPosition;
				uiPosition.x = v;
				currentBuildingDefinition.UiPosition = uiPosition;
				updateBuilding();
			}
		}

		private void positionYChanged(float v)
		{
			if (currentBuildingDefinition != null)
			{
				global::UnityEngine.Vector3 uiPosition = currentBuildingDefinition.UiPosition;
				uiPosition.y = v;
				currentBuildingDefinition.UiPosition = uiPosition;
				updateBuilding();
			}
		}

		private void positionZChanged(float v)
		{
			if (currentBuildingDefinition != null)
			{
				global::UnityEngine.Vector3 uiPosition = currentBuildingDefinition.UiPosition;
				uiPosition.z = v;
				currentBuildingDefinition.UiPosition = uiPosition;
				updateBuilding();
			}
		}

		private void scaleChanged(float v)
		{
			if (currentBuildingDefinition != null)
			{
				currentBuildingDefinition.UiScale = v;
				updateBuilding();
			}
		}

		private void updateBuilding()
		{
			buildingModifiedSignal.Dispatch(currentBuildingDefinition);
			buildingsInfo[currentBuildingDefinition.ID] = new global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo(currentBuildingDefinition.UiPosition, currentBuildingDefinition.UiScale);
			if (upsellItemViews != null)
			{
				global::Kampai.UI.View.UpSell.UpSellItemView[] array = upsellItemViews;
				foreach (global::Kampai.UI.View.UpSell.UpSellItemView upSellItemView in array)
				{
					global::UnityEngine.Transform transform = upSellItemView.buildingSlot.transform;
					float additionalUIScale = upSellItemView.AdditionalUIScale;
					float num = currentBuildingDefinition.UiScale * additionalUIScale;
					transform.localScale = new global::UnityEngine.Vector3(num, num, num);
					transform.localPosition = currentBuildingDefinition.UiPosition * additionalUIScale;
				}
			}
		}

		public void ResetState()
		{
			global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo value;
			if (currentBuildingDefinition != null && resetInfo.TryGetValue(currentBuildingDefinition.ID, out value))
			{
				currentBuildingDefinition.UiPosition = value.Position;
				currentBuildingDefinition.UiScale = value.Scale;
				setupValueFields(currentBuildingDefinition);
				updateBuilding();
			}
		}

		public void SaveAll()
		{
			new global::Kampai.BuildingsSizeToolbox.DefinitionsUpdater().Update(buildingsInfo);
			buildingsInfo.Clear();
			buildingStateAppliedSignal.Dispatch();
		}
	}
}
