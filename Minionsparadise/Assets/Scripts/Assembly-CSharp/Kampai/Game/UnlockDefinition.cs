namespace Kampai.Game
{
	public class UnlockDefinition : global::Kampai.Game.ItemDefinition
	{
		public int ReferencedDefinitionID;

		public int UnlockedQuantity;

		public bool Delta;

		public override int TypeCode
		{
			get
			{
				return 1148;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ReferencedDefinitionID);
			writer.Write(UnlockedQuantity);
			writer.Write(Delta);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ReferencedDefinitionID = reader.ReadInt32();
			UnlockedQuantity = reader.ReadInt32();
			Delta = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "REFERENCEDDEFINITIONID":
				reader.Read();
				ReferencedDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UNLOCKEDQUANTITY":
				reader.Read();
				UnlockedQuantity = global::System.Convert.ToInt32(reader.Value);
				break;
			case "DELTA":
				reader.Read();
				Delta = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
