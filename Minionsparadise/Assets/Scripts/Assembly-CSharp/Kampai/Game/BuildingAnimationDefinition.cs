namespace Kampai.Game
{
	public class BuildingAnimationDefinition : global::Kampai.Game.AnimationDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1031;
			}
		}

		public int CostumeId { get; set; }

		public float GagWeight { get; set; }

		public string BuildingController { get; set; }

		public string MinionController { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(CostumeId);
			writer.Write(GagWeight);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, BuildingController);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MinionController);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			CostumeId = reader.ReadInt32();
			GagWeight = reader.ReadSingle();
			BuildingController = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MinionController = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "COSTUMEID":
				reader.Read();
				CostumeId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "GAGWEIGHT":
				reader.Read();
				GagWeight = global::System.Convert.ToSingle(reader.Value);
				break;
			case "BUILDINGCONTROLLER":
				reader.Read();
				BuildingController = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MINIONCONTROLLER":
				reader.Read();
				MinionController = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
