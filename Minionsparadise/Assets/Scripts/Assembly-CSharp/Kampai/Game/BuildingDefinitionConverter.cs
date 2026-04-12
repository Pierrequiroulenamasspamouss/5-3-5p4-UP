namespace Kampai.Game
{
	public class BuildingDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.BuildingDefinition>
	{
		private BuildingType.BuildingTypeIdentifier buildingType;

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			if (jObject.Property("type") != null)
			{
				string value = jObject.Property("type").Value.ToString();
				buildingType = (BuildingType.BuildingTypeIdentifier)(int)global::System.Enum.Parse(typeof(BuildingType.BuildingTypeIdentifier), value);
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.BuildingDefinition Create(global::System.Type objectType)
		{
			switch (buildingType)
			{
			case BuildingType.BuildingTypeIdentifier.CRAFTING:
				return new global::Kampai.Game.CraftingBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.DECORATION:
				return new global::Kampai.Game.DecorationBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.RESOURCE:
				return new global::Kampai.Game.ResourceBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.LEISURE:
				return new global::Kampai.Game.LeisureBuildingDefintiion();
			case BuildingType.BuildingTypeIdentifier.SPECIAL:
				return new global::Kampai.Game.SpecialBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.BLACKMARKETBOARD:
				return new global::Kampai.Game.BlackMarketBoardDefinition();
			case BuildingType.BuildingTypeIdentifier.STORAGE:
				return new global::Kampai.Game.StorageBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.LANDEXPANSION:
				return new global::Kampai.Game.LandExpansionBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.DEBRIS:
				return new global::Kampai.Game.DebrisBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.MIGNETTE:
				return new global::Kampai.Game.MignetteBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.TIKIBAR:
				return new global::Kampai.Game.TikiBarBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.CABANA:
				return new global::Kampai.Game.CabanaBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.BRIDGE:
				return new global::Kampai.Game.BridgeBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.COMPOSITE:
				return new global::Kampai.Game.CompositeBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.STAGE:
				return new global::Kampai.Game.StageBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.FOUNTAIN:
				return new global::Kampai.Game.FountainBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.WELCOMEHUT:
				return new global::Kampai.Game.WelcomeHutBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.DCN:
				return new global::Kampai.Game.DCNBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.MESSAGEINABOTTLE:
				return new global::Kampai.Game.MIBBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.LAIR_ENTRANCE:
				return new global::Kampai.Game.VillainLairEntranceBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.LAIR_RESOURCEPLOT:
				return new global::Kampai.Game.VillainLairResourcePlotDefinition();
			case BuildingType.BuildingTypeIdentifier.MINION_UPGRADE:
				return new global::Kampai.Game.MinionUpgradeBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.MASTER_COMPONENT:
				return new global::Kampai.Game.MasterPlanComponentBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.MASTER_LEFTOVER:
				return new global::Kampai.Game.MasterPlanLeftOverBuildingDefinition();
			case BuildingType.BuildingTypeIdentifier.CONNECTABLE:
				return new global::Kampai.Game.ConnectableBuildingDefinition();
			default:
				return null;
			}
		}
	}
}
