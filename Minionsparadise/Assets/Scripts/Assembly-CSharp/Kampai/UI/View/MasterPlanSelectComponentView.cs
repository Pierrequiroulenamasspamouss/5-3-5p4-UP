namespace Kampai.UI.View
{
	public class MasterPlanSelectComponentView : global::Kampai.UI.View.PopupMenuView
	{
		[global::UnityEngine.Header("Header")]
		public global::Kampai.UI.View.LocalizeView titleText;

		[global::UnityEngine.Header("Selection Buttons")]
		public global::Kampai.UI.View.ButtonView previousButtonView;

		public global::Kampai.UI.View.ButtonView nextButtonView;

		[global::UnityEngine.Header("Content")]
		public global::UnityEngine.Transform componentInfoPanelTransform;

		public global::UnityEngine.Transform componentCompletePanelTransform;

		[global::UnityEngine.Header("Action Button")]
		public global::Kampai.UI.View.ButtonView actionButtonView;

		public global::Kampai.UI.View.LocalizeView actionLocalizeView;

		[global::UnityEngine.Header("Prefabs")]
		public global::UnityEngine.GameObject componentInfoPrefab;

		[global::UnityEngine.Header("Building")]
		public global::UnityEngine.RectTransform BuildingSlot;

		internal global::strange.extensions.signal.impl.Signal<int, global::Kampai.Util.Boxed<global::System.Action>> PanWithinLairSignal = new global::strange.extensions.signal.impl.Signal<int, global::Kampai.Util.Boxed<global::System.Action>>();

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.UI.IGhostComponentService ghostComponentService;

		internal readonly global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponentDefinition> componentDefinitions = new global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentDefinition>();

		internal global::System.Collections.Generic.List<int> componentCameraPositions = new global::System.Collections.Generic.List<int>();

		private global::Kampai.UI.View.IGUIService guiService;

		internal readonly global::strange.extensions.signal.impl.Signal<global::System.Type, int> updateSubViewSignal = new global::strange.extensions.signal.impl.Signal<global::System.Type, int>();

		private global::Kampai.Game.IMasterPlanService masterPlanService;

		internal global::Kampai.Game.MasterPlanDefinition planDefinition { get; private set; }

		internal bool willRelease { get; private set; }

		internal int selectedIndex { get; private set; }

		internal void Init(int definitionID, int componentIDfromPlatform, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService defService, global::Kampai.UI.View.IGUIService guiService, global::Kampai.UI.IGhostComponentService ghostService, global::Kampai.Game.IMasterPlanService masterPlanService)
		{
			Init();
			this.playerService = playerService;
			this.masterPlanService = masterPlanService;
			definitionService = defService;
			ghostComponentService = ghostService;
			ghostComponentService.ClearGhostComponentBuildings();
			this.guiService = guiService;
			planDefinition = defService.Get<global::Kampai.Game.MasterPlanDefinition>(definitionID);
			SetupComponentDefinitions();
			SetComponentIndexFromId(componentIDfromPlatform);
			Open();
		}

		private void SetupComponentDefinitions()
		{
			global::Kampai.Game.VillainLairDefinition villainLairDefinition = definitionService.Get<global::Kampai.Game.VillainLairDefinition>(global::Kampai.Game.StaticItem.VILLAIN_LAIR_DEFINITION_ID);
			componentDefinitions.Clear();
			for (int i = 0; i < planDefinition.ComponentDefinitionIDs.Count; i++)
			{
				int id = planDefinition.ComponentDefinitionIDs[i];
				global::Kampai.Game.MasterPlanComponentDefinition item = definitionService.Get<global::Kampai.Game.MasterPlanComponentDefinition>(id);
				componentDefinitions.Add(item);
				componentCameraPositions.Add(villainLairDefinition.Platforms[i].customCameraPosID);
			}
		}

		internal void SetComponentIndexFromId(int id)
		{
			int componentIndexFromId = GetComponentIndexFromId(id);
			SelectComponent((componentIndexFromId < componentDefinitions.Count) ? componentIndexFromId : 0);
		}

		private int GetComponentIndexFromId(int id)
		{
			int i;
			for (i = 0; i < componentDefinitions.Count && id != GetMasterplanComponentDefId(i); i++)
			{
			}
			return i;
		}

		internal void SelectComponent(int index)
		{
			selectedIndex = WrapIndex(index, 0, componentDefinitions.Count - 1);
			global::Kampai.Game.MasterPlanComponent masterPlanComponent = GetMasterPlanComponent(selectedIndex);
			bool flag = masterPlanComponent != null && (masterPlanComponent.State == global::Kampai.Game.MasterPlanComponentState.Complete || masterPlanComponent.State == global::Kampai.Game.MasterPlanComponentState.Scaffolding || masterPlanComponent.State == global::Kampai.Game.MasterPlanComponentState.Built);
			titleText.LocKey = GetMasterplanComponentBuildingDefLocKey(selectedIndex);
			ToggleComponentInfo(!flag);
			ToggleCompletePanel(flag);
			DisplayBuilding(true);
		}

		internal void NextComponent()
		{
			ghostComponentService.ClearGhostComponentBuildings();
			SelectComponent(++selectedIndex);
		}

		internal void PreviousComponent()
		{
			ghostComponentService.ClearGhostComponentBuildings();
			SelectComponent(--selectedIndex);
		}

		private int WrapIndex(int index, int min, int max)
		{
			return (index < min) ? max : ((index <= max) ? index : min);
		}

		private void ToggleComponentInfo(bool show)
		{
			componentInfoPanelTransform.gameObject.SetActive(show);
			actionButtonView.gameObject.SetActive(show);
			if (!show)
			{
				return;
			}
			global::Kampai.Game.MasterPlanComponent activeComponentFromPlanDefinition = masterPlanService.GetActiveComponentFromPlanDefinition(planDefinition.ID);
			if (activeComponentFromPlanDefinition != null)
			{
				actionLocalizeView.LocKey = "MasterPlanTaskSelection";
				actionButtonView.gameObject.SetActive(selectedIndex == GetComponentIndexFromId(activeComponentFromPlanDefinition.Definition.ID));
			}
			else
			{
				actionLocalizeView.LocKey = "MasterPlanSelect";
			}
			global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType[] array = new global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType[2]
			{
				global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType.Requires,
				global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType.Rewards
			};
			if (componentInfoPanelTransform.childCount != array.Length)
			{
				for (int i = 0; i < array.Length; i++)
				{
					global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_masterplanComponentInfo");
					global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
					args.Add(typeof(global::Kampai.Game.MasterPlanDefinition), planDefinition);
					args.Add(typeof(global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType), array[i]);
					args.Add(typeof(int), selectedIndex);
					global::UnityEngine.GameObject gameObject = guiService.Execute(iGUICommand);
					if (!(gameObject == null))
					{
						gameObject.transform.SetParent(componentInfoPanelTransform, false);
					}
				}
			}
			else
			{
				updateSubViewSignal.Dispatch(typeof(global::Kampai.UI.View.MasterPlanComponentInfoView), selectedIndex);
			}
		}

		internal void ReleaseViews()
		{
			ghostComponentService.ClearGhostComponentBuildings();
		}

		private void ToggleCompletePanel(bool show)
		{
			componentCompletePanelTransform.gameObject.SetActive(show);
		}

		private void CreateBuilding(bool isRegularComponent)
		{
			int componentID = ((!isRegularComponent) ? planDefinition.BuildingDefID : componentDefinitions[selectedIndex].ID);
			ghostComponentService.DisplayZoomedInComponent(componentID, isRegularComponent);
		}

		public override void Close(bool instant = false)
		{
			willRelease = true;
			base.Close(instant);
		}

		private void DisplayBuilding(bool isRegularComponent)
		{
			if (isRegularComponent)
			{
				PanToComponentLocation();
			}
			else
			{
				PanToMasterPlanLocation();
			}
		}

		private void PanToComponentLocation()
		{
			PanWithinLairSignal.Dispatch(componentCameraPositions[selectedIndex], new global::Kampai.Util.Boxed<global::System.Action>(PanToComponentComplete));
		}

		private void PanToMasterPlanLocation()
		{
			PanWithinLairSignal.Dispatch(planDefinition.BuildingCustomCameraPosID, new global::Kampai.Util.Boxed<global::System.Action>(PanToMasterPlanBuildingComplete));
		}

		internal void PanToMainLairView()
		{
			PanWithinLairSignal.Dispatch(60017, new global::Kampai.Util.Boxed<global::System.Action>(null));
		}

		private void PanToComponentComplete()
		{
			if (selectedIndex >= 0 && selectedIndex < componentDefinitions.Count && !willRelease)
			{
				CreateBuilding(true);
			}
		}

		private void PanToMasterPlanBuildingComplete()
		{
			if (!willRelease)
			{
				CreateBuilding(false);
			}
		}

		internal global::Kampai.Game.MasterPlanComponentDefinition GetMasterplanComponentDef(int componentIndex)
		{
			return (componentIndex >= 0 && componentIndex < componentDefinitions.Count) ? componentDefinitions[componentIndex] : null;
		}

		internal int GetMasterplanComponentDefId(int componentIndex)
		{
			global::Kampai.Game.MasterPlanComponentDefinition masterplanComponentDef = GetMasterplanComponentDef(componentIndex);
			return (masterplanComponentDef != null) ? masterplanComponentDef.ID : (-1);
		}

		internal string GetMasterplanComponentBuildingDefLocKey(int componentIndex)
		{
			global::Kampai.Game.MasterPlanComponent masterPlanComponent = GetMasterPlanComponent(componentIndex);
			if (masterPlanComponent != null)
			{
				global::Kampai.Game.MasterPlanComponentBuildingDefinition definition = null;
				if (definitionService.TryGet<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(masterPlanComponent.buildingDefID, out definition))
				{
					return definition.LocalizedKey;
				}
			}
			return (masterPlanComponent != null) ? componentDefinitions[componentIndex].LocalizedKey : "MISSING LOC KEY";
		}

		internal global::Kampai.Game.MasterPlanComponent GetMasterPlanComponent(int index)
		{
			global::Kampai.Game.MasterPlanComponentDefinition masterplanComponentDef = GetMasterplanComponentDef(index);
			return (masterplanComponentDef != null) ? playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(masterplanComponentDef.ID) : null;
		}
	}
}
