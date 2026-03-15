namespace Kampai.Game
{
	public class CostumeItemDefinition : global::Kampai.Game.ItemDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1090;
			}
		}

		public string Skeleton { get; set; }

		public global::System.Collections.Generic.IList<string> MeshList { get; set; }

		public global::Kampai.Game.CharacterUIAnimationDefinition characterUIAnimationDefinition { get; set; }

		public int PartyAnimations { get; set; }

		public CostumeItemDefinition()
		{
			base.Storable = false;
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Skeleton);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteString, MeshList);
			global::Kampai.Util.BinarySerializationUtil.WriteCharacterUIAnimationDefinition(writer, characterUIAnimationDefinition);
			writer.Write(PartyAnimations);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Skeleton = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MeshList = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadString, MeshList);
			characterUIAnimationDefinition = global::Kampai.Util.BinarySerializationUtil.ReadCharacterUIAnimationDefinition(reader);
			PartyAnimations = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SKELETON":
				reader.Read();
				Skeleton = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MESHLIST":
				reader.Read();
				MeshList = global::Kampai.Util.ReaderUtil.PopulateList<string>(reader, converters, global::Kampai.Util.ReaderUtil.ReadString, MeshList);
				break;
			case "CHARACTERUIANIMATIONDEFINITION":
				reader.Read();
				characterUIAnimationDefinition = global::Kampai.Util.ReaderUtil.ReadCharacterUIAnimationDefinition(reader, converters);
				break;
			case "PARTYANIMATIONS":
				reader.Read();
				PartyAnimations = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
