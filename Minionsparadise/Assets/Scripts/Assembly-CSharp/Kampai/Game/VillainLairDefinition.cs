namespace Kampai.Game
{
	public class VillainLairDefinition : global::Kampai.Game.Definition, global::Kampai.Game.Locatable
	{
		public override int TypeCode
		{
			get
			{
				return 1066;
			}
		}

		public int CustomCameraPositionDefinitionId { get; set; }

		public global::UnityEngine.Vector3 KevinOffset { get; set; }

		public float KevinRotation { get; set; }

		public global::UnityEngine.Vector3 VillainOffset { get; set; }

		public float VillainRotation { get; set; }

		public string IntroAnimController { get; set; }

		public global::Kampai.Game.Location Location { get; set; }

		public string Prefab { get; set; }

		public global::Kampai.Game.Location MinionArrivalOffset { get; set; }

		public int ResourceBuildingDefID { get; set; }

		public int ResourceItemID { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.PlatformDefinition> Platforms { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.ResourcePlotDefinition> ResourcePlots { get; set; }

		public int SecondsToHarvest { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(CustomCameraPositionDefinitionId);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, KevinOffset);
			writer.Write(KevinRotation);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, VillainOffset);
			writer.Write(VillainRotation);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, IntroAnimController);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, Location);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Prefab);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, MinionArrivalOffset);
			writer.Write(ResourceBuildingDefID);
			writer.Write(ResourceItemID);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WritePlatformDefinition, Platforms);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteResourcePlotDefinition, ResourcePlots);
			writer.Write(SecondsToHarvest);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			CustomCameraPositionDefinitionId = reader.ReadInt32();
			KevinOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			KevinRotation = reader.ReadSingle();
			VillainOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			VillainRotation = reader.ReadSingle();
			IntroAnimController = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Location = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
			Prefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MinionArrivalOffset = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
			ResourceBuildingDefID = reader.ReadInt32();
			ResourceItemID = reader.ReadInt32();
			Platforms = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadPlatformDefinition, Platforms);
			ResourcePlots = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadResourcePlotDefinition, ResourcePlots);
			SecondsToHarvest = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "CUSTOMCAMERAPOSITIONDEFINITIONID":
				reader.Read();
				CustomCameraPositionDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "KEVINOFFSET":
				reader.Read();
				KevinOffset = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "KEVINROTATION":
				reader.Read();
				KevinRotation = global::System.Convert.ToSingle(reader.Value);
				break;
			case "VILLAINOFFSET":
				reader.Read();
				VillainOffset = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "VILLAINROTATION":
				reader.Read();
				VillainRotation = global::System.Convert.ToSingle(reader.Value);
				break;
			case "INTROANIMCONTROLLER":
				reader.Read();
				IntroAnimController = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "LOCATION":
				reader.Read();
				Location = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "PREFAB":
				reader.Read();
				Prefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MINIONARRIVALOFFSET":
				reader.Read();
				MinionArrivalOffset = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "RESOURCEBUILDINGDEFID":
				reader.Read();
				ResourceBuildingDefID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "RESOURCEITEMID":
				reader.Read();
				ResourceItemID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PLATFORMS":
				reader.Read();
				Platforms = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadPlatformDefinition, Platforms);
				break;
			case "RESOURCEPLOTS":
				reader.Read();
				ResourcePlots = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadResourcePlotDefinition, ResourcePlots);
				break;
			case "SECONDSTOHARVEST":
				reader.Read();
				SecondsToHarvest = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.VillainLair(this);
		}
	}
}
