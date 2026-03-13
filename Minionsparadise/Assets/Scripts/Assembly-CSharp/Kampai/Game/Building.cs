namespace Kampai.Game
{
	public abstract class Building<T> : global::Kampai.Game.Instance<T>, global::Kampai.Game.Building, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable where T : global::Kampai.Game.BuildingDefinition
	{
		global::Kampai.Game.BuildingDefinition global::Kampai.Game.Building.Definition
		{
			get
			{
				return base.Definition;
			}
		}

		public global::Kampai.Game.BuildingState State { get; set; }

		public global::Kampai.Game.Location Location { get; set; }

		public int BuildingNumber { get; set; }

		public bool IsFootprintable
		{
			get
			{
				return State != global::Kampai.Game.BuildingState.Disabled;
			}
		}

		public int StateStartTime { get; set; }

		protected Building(T definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STATE":
				reader.Read();
				State = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.BuildingState>(reader);
				break;
			case "LOCATION":
				reader.Read();
				Location = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "BUILDINGNUMBER":
				reader.Read();
				BuildingNumber = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STATESTARTTIME":
				reader.Read();
				StateStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
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
			writer.WritePropertyName("State");
			writer.WriteValue((int)State);
			if (Location != null)
			{
				writer.WritePropertyName("Location");
				writer.WriteStartObject();
				writer.WritePropertyName("x");
				writer.WriteValue(Location.x);
				writer.WritePropertyName("y");
				writer.WriteValue(Location.y);
				writer.WriteEndObject();
			}
			writer.WritePropertyName("BuildingNumber");
			writer.WriteValue(BuildingNumber);
			writer.WritePropertyName("StateStartTime");
			writer.WriteValue(StateStartTime);
		}

		public virtual bool HasDetailMenuToShow()
		{
			T definition = base.Definition;
			return !string.IsNullOrEmpty(definition.MenuPrefab);
		}

		public abstract global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject);

		public void SetState(global::Kampai.Game.BuildingState buildingState)
		{
			State = buildingState;
		}

		public virtual string GetPrefab(int index = 0)
		{
			T definition = base.Definition;
			return definition.GetPrefab(index);
		}

		public virtual string GetPaintover()
		{
			T definition = base.Definition;
			return definition.Paintover;
		}

		public virtual bool IsBuildingRepaired()
		{
			return true;
		}

		public virtual bool IsTikiSignRepaired()
		{
			return false;
		}
	}
	public interface Building : global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		global::Kampai.Game.BuildingState State { get; }

		new global::Kampai.Game.BuildingDefinition Definition { get; }

		int BuildingNumber { get; set; }

		bool IsFootprintable { get; }

		int StateStartTime { get; set; }

		bool HasDetailMenuToShow();

		global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject);

		void SetState(global::Kampai.Game.BuildingState buildingState);

		string GetPrefab(int index);

		string GetPaintover();

		bool IsBuildingRepaired();
	}
}
