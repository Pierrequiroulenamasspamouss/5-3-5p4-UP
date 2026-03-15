namespace Kampai.Game
{
	public class PartyFavorAnimationItemDefinition : global::Kampai.Game.ItemDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1094;
			}
		}

		public int ReferencedDefinitionID { get; set; }

		public PartyFavorAnimationItemDefinition()
		{
			base.Storable = false;
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ReferencedDefinitionID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ReferencedDefinitionID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "REFERENCEDDEFINITIONID":
				reader.Read();
				ReferencedDefinitionID = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
