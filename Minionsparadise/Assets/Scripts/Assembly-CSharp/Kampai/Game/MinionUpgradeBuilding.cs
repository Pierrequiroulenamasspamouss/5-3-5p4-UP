namespace Kampai.Game
{
	public class MinionUpgradeBuilding : global::Kampai.Game.RepairableBuilding<global::Kampai.Game.MinionUpgradeBuildingDefinition>
	{
		public global::System.Collections.Generic.List<int> processedPopulationBenefitDefinitionIDs = new global::System.Collections.Generic.List<int>();

		public MinionUpgradeBuilding(global::Kampai.Game.MinionUpgradeBuildingDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PROCESSEDPOPULATIONBENEFITDEFINITIONIDS":
				reader.Read();
				processedPopulationBenefitDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, processedPopulationBenefitDefinitionIDs);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			if (processedPopulationBenefitDefinitionIDs == null)
			{
				return;
			}
			writer.WritePropertyName("processedPopulationBenefitDefinitionIDs");
			writer.WriteStartArray();
			global::System.Collections.Generic.List<int>.Enumerator enumerator = processedPopulationBenefitDefinitionIDs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					writer.WriteValue(current);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			writer.WriteEndArray();
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.MinionUpgradeBuildingObject>();
		}
	}
}
