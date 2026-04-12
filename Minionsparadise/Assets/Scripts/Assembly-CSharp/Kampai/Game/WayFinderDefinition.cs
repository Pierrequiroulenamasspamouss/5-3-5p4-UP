namespace Kampai.Game
{
	public class WayFinderDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1188;
			}
		}

		public string NewQuestIcon { get; set; }

		public string QuestCompleteIcon { get; set; }

		public string TaskCompleteIcon { get; set; }

		public string SpecialEventNewQuestIcon { get; set; }

		public string SpecialEventQuestCompleteIcon { get; set; }

		public string SpecialEventTaskCompleteIcon { get; set; }

		public string TikibarDefaultIcon { get; set; }

		public float TikibarZoomViewEnabledAt { get; set; }

		public string CabanaDefaultIcon { get; set; }

		public string OrderBoardDefaultIcon { get; set; }

		public string TSMDefaultIcon { get; set; }

		public string StorageBuildingDefaultIcon { get; set; }

		public string BobPointsAtStuffLandExpansionIcon { get; set; }

		public string BobPointsAtStuffDefaultIcon { get; set; }

		public float BobPointsAtStuffYWorldOffset { get; set; }

		public string MarketplaceSoldIcon { get; set; }

		public string StageBuildingIcon { get; set; }

		public string DefaultIcon { get; set; }

		public string KevinLairIcon { get; set; }

		public string MasterPlanComponentCompleteIcon { get; set; }

		public string MasterPlanComponentTaskCompleteIcon { get; set; }

		public string MignetteDefaultIcon { get; set; }

		public string ConnectableIcon { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, NewQuestIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestCompleteIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TaskCompleteIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, SpecialEventNewQuestIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, SpecialEventQuestCompleteIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, SpecialEventTaskCompleteIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TikibarDefaultIcon);
			writer.Write(TikibarZoomViewEnabledAt);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, CabanaDefaultIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, OrderBoardDefaultIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TSMDefaultIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, StorageBuildingDefaultIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, BobPointsAtStuffLandExpansionIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, BobPointsAtStuffDefaultIcon);
			writer.Write(BobPointsAtStuffYWorldOffset);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MarketplaceSoldIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, StageBuildingIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, DefaultIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, KevinLairIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MasterPlanComponentCompleteIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MasterPlanComponentTaskCompleteIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MignetteDefaultIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, ConnectableIcon);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			NewQuestIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestCompleteIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			TaskCompleteIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			SpecialEventNewQuestIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			SpecialEventQuestCompleteIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			SpecialEventTaskCompleteIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			TikibarDefaultIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			TikibarZoomViewEnabledAt = reader.ReadSingle();
			CabanaDefaultIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			OrderBoardDefaultIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			TSMDefaultIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			StorageBuildingDefaultIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			BobPointsAtStuffLandExpansionIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			BobPointsAtStuffDefaultIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			BobPointsAtStuffYWorldOffset = reader.ReadSingle();
			MarketplaceSoldIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			StageBuildingIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			DefaultIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			KevinLairIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MasterPlanComponentCompleteIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MasterPlanComponentTaskCompleteIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MignetteDefaultIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			ConnectableIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NEWQUESTICON":
				reader.Read();
				NewQuestIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTCOMPLETEICON":
				reader.Read();
				QuestCompleteIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TASKCOMPLETEICON":
				reader.Read();
				TaskCompleteIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SPECIALEVENTNEWQUESTICON":
				reader.Read();
				SpecialEventNewQuestIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SPECIALEVENTQUESTCOMPLETEICON":
				reader.Read();
				SpecialEventQuestCompleteIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SPECIALEVENTTASKCOMPLETEICON":
				reader.Read();
				SpecialEventTaskCompleteIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TIKIBARDEFAULTICON":
				reader.Read();
				TikibarDefaultIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TIKIBARZOOMVIEWENABLEDAT":
				reader.Read();
				TikibarZoomViewEnabledAt = global::System.Convert.ToSingle(reader.Value);
				break;
			case "CABANADEFAULTICON":
				reader.Read();
				CabanaDefaultIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ORDERBOARDDEFAULTICON":
				reader.Read();
				OrderBoardDefaultIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TSMDEFAULTICON":
				reader.Read();
				TSMDefaultIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "STORAGEBUILDINGDEFAULTICON":
				reader.Read();
				StorageBuildingDefaultIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "BOBPOINTSATSTUFFLANDEXPANSIONICON":
				reader.Read();
				BobPointsAtStuffLandExpansionIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "BOBPOINTSATSTUFFDEFAULTICON":
				reader.Read();
				BobPointsAtStuffDefaultIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "BOBPOINTSATSTUFFYWORLDOFFSET":
				reader.Read();
				BobPointsAtStuffYWorldOffset = global::System.Convert.ToSingle(reader.Value);
				break;
			case "MARKETPLACESOLDICON":
				reader.Read();
				MarketplaceSoldIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "STAGEBUILDINGICON":
				reader.Read();
				StageBuildingIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "DEFAULTICON":
				reader.Read();
				DefaultIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "KEVINLAIRICON":
				reader.Read();
				KevinLairIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MASTERPLANCOMPONENTCOMPLETEICON":
				reader.Read();
				MasterPlanComponentCompleteIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MASTERPLANCOMPONENTTASKCOMPLETEICON":
				reader.Read();
				MasterPlanComponentTaskCompleteIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MIGNETTEDEFAULTICON":
				reader.Read();
				MignetteDefaultIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "CONNECTABLEICON":
				reader.Read();
				ConnectableIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
