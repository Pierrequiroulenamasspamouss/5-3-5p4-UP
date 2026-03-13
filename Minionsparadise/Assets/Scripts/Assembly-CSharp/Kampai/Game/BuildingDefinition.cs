namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public abstract class BuildingDefinition : global::Kampai.Game.TaxonomyDefinition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1034;
			}
		}

		public BuildingType.BuildingTypeIdentifier Type { get; set; }

		public int FootprintID { get; set; }

		public int PlatformFootprintID { get; set; }

		public global::Kampai.Game.ScreenPosition ScreenPosition { get; set; }

		public int ConstructionTime { get; set; }

		public bool Movable { get; set; }

		public int RewardTransactionId { get; set; }

		public virtual string Prefab { get; set; }

		public virtual string Paintover { get; set; }

		public string RevealVFX { get; set; }

		public string ScaffoldingPrefab { get; set; }

		public string RibbonPrefab { get; set; }

		public string PlatformPrefab { get; set; }

		public bool Storable { get; set; }

		public global::UnityEngine.Vector3 QuestIconOffset { get; set; }

		public string MenuPrefab { get; set; }

		public int IncrementalCost { get; set; }

		public int IncrementalConstructionTime { get; set; }

		public bool RouteToSlot { get; set; }

		public int WorkStations { get; set; }

		public string PartyPointsLocalizedKey { get; set; }

		public int PlayerTrainingDefinitionID { get; set; }

		public float UiScale { get; set; }

		public global::UnityEngine.Vector3 UiPosition { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Type);
			writer.Write(FootprintID);
			writer.Write(PlatformFootprintID);
			global::Kampai.Util.BinarySerializationUtil.WriteScreenPosition(writer, ScreenPosition);
			writer.Write(ConstructionTime);
			writer.Write(Movable);
			writer.Write(RewardTransactionId);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Prefab);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Paintover);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, RevealVFX);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, ScaffoldingPrefab);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, RibbonPrefab);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PlatformPrefab);
			writer.Write(Storable);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, QuestIconOffset);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MenuPrefab);
			writer.Write(IncrementalCost);
			writer.Write(IncrementalConstructionTime);
			writer.Write(RouteToSlot);
			writer.Write(WorkStations);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PartyPointsLocalizedKey);
			writer.Write(PlayerTrainingDefinitionID);
			writer.Write(UiScale);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, UiPosition);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<BuildingType.BuildingTypeIdentifier>(reader);
			FootprintID = reader.ReadInt32();
			PlatformFootprintID = reader.ReadInt32();
			ScreenPosition = global::Kampai.Util.BinarySerializationUtil.ReadScreenPosition(reader);
			ConstructionTime = reader.ReadInt32();
			Movable = reader.ReadBoolean();
			RewardTransactionId = reader.ReadInt32();
			Prefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Paintover = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			RevealVFX = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			ScaffoldingPrefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			RibbonPrefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PlatformPrefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Storable = reader.ReadBoolean();
			QuestIconOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			MenuPrefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			IncrementalCost = reader.ReadInt32();
			IncrementalConstructionTime = reader.ReadInt32();
			RouteToSlot = reader.ReadBoolean();
			WorkStations = reader.ReadInt32();
			PartyPointsLocalizedKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PlayerTrainingDefinitionID = reader.ReadInt32();
			UiScale = reader.ReadSingle();
			UiPosition = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TYPE":
				reader.Read();
				Type = global::Kampai.Util.ReaderUtil.ReadEnum<BuildingType.BuildingTypeIdentifier>(reader);
				break;
			case "FOOTPRINTID":
				reader.Read();
				FootprintID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PLATFORMFOOTPRINTID":
				reader.Read();
				PlatformFootprintID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SCREENPOSITION":
				reader.Read();
				ScreenPosition = global::Kampai.Util.ReaderUtil.ReadScreenPosition(reader, converters);
				break;
			case "CONSTRUCTIONTIME":
				reader.Read();
				ConstructionTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MOVABLE":
				reader.Read();
				Movable = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "REWARDTRANSACTIONID":
				reader.Read();
				RewardTransactionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREFAB":
				reader.Read();
				Prefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PAINTOVER":
				reader.Read();
				Paintover = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "REVEALVFX":
				reader.Read();
				RevealVFX = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SCAFFOLDINGPREFAB":
				reader.Read();
				ScaffoldingPrefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "RIBBONPREFAB":
				reader.Read();
				RibbonPrefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PLATFORMPREFAB":
				reader.Read();
				PlatformPrefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "STORABLE":
				reader.Read();
				Storable = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "QUESTICONOFFSET":
				reader.Read();
				QuestIconOffset = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "MENUPREFAB":
				reader.Read();
				MenuPrefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "INCREMENTALCOST":
				reader.Read();
				IncrementalCost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "INCREMENTALCONSTRUCTIONTIME":
				reader.Read();
				IncrementalConstructionTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ROUTETOSLOT":
				reader.Read();
				RouteToSlot = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "WORKSTATIONS":
				reader.Read();
				WorkStations = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYPOINTSLOCALIZEDKEY":
				reader.Read();
				PartyPointsLocalizedKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PLAYERTRAININGDEFINITIONID":
				reader.Read();
				PlayerTrainingDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UISCALE":
				reader.Read();
				UiScale = global::System.Convert.ToSingle(reader.Value);
				break;
			case "UIPOSITION":
				reader.Read();
				UiPosition = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public abstract global::Kampai.Game.Building BuildBuilding();

		public virtual string GetPrefab(int index = 0)
		{
			return Prefab;
		}

		public global::Kampai.Game.Instance Build()
		{
			return BuildBuilding();
		}
	}
}
