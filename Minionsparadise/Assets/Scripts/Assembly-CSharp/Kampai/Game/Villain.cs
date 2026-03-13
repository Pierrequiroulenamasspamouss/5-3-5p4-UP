namespace Kampai.Game
{
	public class Villain : global::Kampai.Game.NamedCharacter<global::Kampai.Game.VillainDefinition>
	{
		private global::Kampai.Game.CabanaBuilding _deprecatedCabanaBuilding;

		public global::Kampai.Game.CabanaBuilding Cabana
		{
			get
			{
				if (_deprecatedCabanaBuilding != null)
				{
					global::UnityEngine.Debug.LogError("Getter should only be used by PlayerVersion!");
					return _deprecatedCabanaBuilding;
				}
				return null;
			}
			set
			{
				_deprecatedCabanaBuilding = value;
			}
		}

		public int CabanaBuildingId { get; set; }

		public Villain(global::Kampai.Game.VillainDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
                        int num = 1; //FIX USE OF UNASSIGNED VARIABLE
                        if (num == 1)
				{
					reader.Read();
					CabanaBuildingId = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "CABANA":
				reader.Read();
				Cabana = (global::Kampai.Game.CabanaBuilding)converters.instanceConverter.ReadJson(reader, converters);
				break;
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
			if (Cabana != null)
			{
				writer.WritePropertyName("Cabana");
				Cabana.Serialize(writer);
			}
			writer.WritePropertyName("CabanaBuildingId");
			writer.WriteValue(CabanaBuildingId);
		}

		public override global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go)
		{
			return go.AddComponent<global::Kampai.Game.View.VillainView>();
		}

		public override string ToString()
		{
			return string.Format("Villain(ID:{0}, LocalizedKey:{1})", ID, base.Definition.LocalizedKey);
		}
	}
}
