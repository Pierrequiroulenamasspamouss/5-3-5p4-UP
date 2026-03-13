namespace Kampai.Game
{
	public class CabanaBuilding : global::Kampai.Game.Building<global::Kampai.Game.CabanaBuildingDefinition>
	{
		private bool occupied;

		[global::Newtonsoft.Json.JsonIgnore]
		public bool Occupied
		{
			get
			{
				return occupied;
			}
			set
			{
				occupied = value;
			}
		}

		public global::Kampai.Game.Quest Quest { get; set; }

		public CabanaBuilding(global::Kampai.Game.CabanaBuildingDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "QUEST":
				reader.Read();
				Quest = (global::Kampai.Game.Quest)converters.instanceConverter.ReadJson(reader, converters);
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
			if (Quest != null)
			{
				writer.WritePropertyName("Quest");
				Quest.Serialize(writer);
			}
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.CabanaBuildingObject>();
		}

		public override string GetPrefab(int index = 0)
		{
			if (State == global::Kampai.Game.BuildingState.Inaccessible || State == global::Kampai.Game.BuildingState.Broken)
			{
				return base.Definition.brokenPrefab;
			}
			if (State == global::Kampai.Game.BuildingState.Idle)
			{
				return base.Definition.InactivePrefab;
			}
			return base.Definition.Prefab;
		}

		public override bool IsBuildingRepaired()
		{
			return State != global::Kampai.Game.BuildingState.Broken && State != global::Kampai.Game.BuildingState.Inaccessible;
		}
	}
}
