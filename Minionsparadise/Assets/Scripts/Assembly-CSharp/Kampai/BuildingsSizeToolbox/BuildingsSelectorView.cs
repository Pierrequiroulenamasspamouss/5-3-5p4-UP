namespace Kampai.BuildingsSizeToolbox
{
	public class BuildingsSelectorView : global::Kampai.Util.KampaiView
	{
		private static readonly global::System.Collections.Generic.HashSet<BuildingType.BuildingTypeIdentifier> allowedBuildingTypes = new global::System.Collections.Generic.HashSet<BuildingType.BuildingTypeIdentifier>
		{
			BuildingType.BuildingTypeIdentifier.CRAFTING,
			BuildingType.BuildingTypeIdentifier.DECORATION,
			BuildingType.BuildingTypeIdentifier.LEISURE,
			BuildingType.BuildingTypeIdentifier.RESOURCE,
			BuildingType.BuildingTypeIdentifier.SPECIAL,
			BuildingType.BuildingTypeIdentifier.MASTER_COMPONENT,
			BuildingType.BuildingTypeIdentifier.MASTER_LEFTOVER
		};

		public global::UnityEngine.GameObject ScrollContent;

		public global::Kampai.BuildingsSizeToolbox.BuildingsSelectorListItemView ListItemViewBase;

		public global::UnityEngine.UI.Text Title;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.BuildingsSizeToolbox.BuildingSelectedSignal buildingSelectedSignal { get; set; }

		protected override void Start()
		{
			base.Start();
			global::System.Collections.Generic.List<global::Kampai.Game.BuildingDefinition> all = definitionService.GetAll<global::Kampai.Game.BuildingDefinition>();
			foreach (global::Kampai.Game.BuildingDefinition item in all)
			{
				if (allowedBuildingTypes.Contains(item.Type))
				{
					global::Kampai.BuildingsSizeToolbox.BuildingsSelectorListItemView buildingsSelectorListItemView = global::UnityEngine.Object.Instantiate(ListItemViewBase);
					buildingsSelectorListItemView.Setup(item);
					buildingsSelectorListItemView.gameObject.SetActive(true);
					buildingsSelectorListItemView.transform.SetParent(ScrollContent.transform, false);
					buildingsSelectorListItemView.ClickedSignal.AddListener(buildingSelected);
				}
			}
			StartCoroutine(loadFirstBuilding(all[0]));
		}

		private global::System.Collections.IEnumerator loadFirstBuilding(global::Kampai.Game.BuildingDefinition def)
		{
			yield return null;
			buildingSelected(def);
		}

		private void buildingSelected(global::Kampai.Game.BuildingDefinition def)
		{
			Title.text = string.Format("{0}: {1}", def.ID, def.LocalizedKey);
			buildingSelectedSignal.Dispatch(def);
		}
	}
}
