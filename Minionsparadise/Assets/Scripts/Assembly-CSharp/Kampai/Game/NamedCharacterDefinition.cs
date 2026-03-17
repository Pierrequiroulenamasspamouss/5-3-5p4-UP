namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public abstract class NamedCharacterDefinition : global::Kampai.Game.TaxonomyDefinition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>, global::Kampai.Game.Locatable
	{
		public override int TypeCode
		{
			get
			{
				return 1073;
			}
		}

		public string Prefab { get; set; }

		public global::Kampai.Game.Location Location { get; set; }

		public global::UnityEngine.Vector3 RotationEulers { get; set; }

		public global::Kampai.Game.NamedCharacterAnimationDefinition CharacterAnimations { get; set; }

		public global::Kampai.Game.NamedCharacterType Type { get; set; }

		public int VFXBuildingID { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Prefab);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, Location);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, RotationEulers);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, CharacterAnimations);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Type);
			writer.Write(VFXBuildingID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Prefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Location = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
			RotationEulers = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			CharacterAnimations = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.NamedCharacterAnimationDefinition>(reader);
			Type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.NamedCharacterType>(reader);
			VFXBuildingID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PREFAB":
				reader.Read();
				Prefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "LOCATION":
				reader.Read();
				Location = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "ROTATIONEULERS":
				reader.Read();
				RotationEulers = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "CHARACTERANIMATIONS":
				reader.Read();
				CharacterAnimations = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.NamedCharacterAnimationDefinition>(reader, converters);
				break;
			case "TYPE":
				reader.Read();
				Type = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.NamedCharacterType>(reader);
				break;
			case "VFXBUILDINGID":
				reader.Read();
				VFXBuildingID = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public abstract global::Kampai.Game.Instance Build();
	}
}
