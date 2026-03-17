namespace Kampai.Game
{
	public class MIBBuildingDefinition : global::Kampai.Game.BuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1053;
			}
		}

		public int ExpiryInSeconds { get; set; }

		public int CooldownInSeconds { get; set; }

		public bool DisableTapRewards { get; set; }

		public bool DisableReturnRewards { get; set; }

		public int AfterXTapRewards { get; set; }

		public int FirstXTapsWeightedDefinitionId { get; set; }

		public int SecondXTapsWeightedDefinitionId { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.UserSegment> ReturnSpenderLevelSegments { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.UserSegment> ReturnNonSpenderLevelSegments { get; set; }

		public global::UnityEngine.Vector3 HarvestableIconOffset { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ExpiryInSeconds);
			writer.Write(CooldownInSeconds);
			writer.Write(DisableTapRewards);
			writer.Write(DisableReturnRewards);
			writer.Write(AfterXTapRewards);
			writer.Write(FirstXTapsWeightedDefinitionId);
			writer.Write(SecondXTapsWeightedDefinitionId);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteUserSegment, ReturnSpenderLevelSegments);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteUserSegment, ReturnNonSpenderLevelSegments);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, HarvestableIconOffset);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ExpiryInSeconds = reader.ReadInt32();
			CooldownInSeconds = reader.ReadInt32();
			DisableTapRewards = reader.ReadBoolean();
			DisableReturnRewards = reader.ReadBoolean();
			AfterXTapRewards = reader.ReadInt32();
			FirstXTapsWeightedDefinitionId = reader.ReadInt32();
			SecondXTapsWeightedDefinitionId = reader.ReadInt32();
			ReturnSpenderLevelSegments = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadUserSegment, ReturnSpenderLevelSegments);
			ReturnNonSpenderLevelSegments = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadUserSegment, ReturnNonSpenderLevelSegments);
			HarvestableIconOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "EXPIRYINSECONDS":
				reader.Read();
				ExpiryInSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COOLDOWNINSECONDS":
				reader.Read();
				CooldownInSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "DISABLETAPREWARDS":
				reader.Read();
				DisableTapRewards = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "DISABLERETURNREWARDS":
				reader.Read();
				DisableReturnRewards = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "AFTERXTAPREWARDS":
				reader.Read();
				AfterXTapRewards = global::System.Convert.ToInt32(reader.Value);
				break;
			case "FIRSTXTAPSWEIGHTEDDEFINITIONID":
				reader.Read();
				FirstXTapsWeightedDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SECONDXTAPSWEIGHTEDDEFINITIONID":
				reader.Read();
				SecondXTapsWeightedDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "RETURNSPENDERLEVELSEGMENTS":
				reader.Read();
				ReturnSpenderLevelSegments = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadUserSegment, ReturnSpenderLevelSegments);
				break;
			case "RETURNNONSPENDERLEVELSEGMENTS":
				reader.Read();
				ReturnNonSpenderLevelSegments = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadUserSegment, ReturnNonSpenderLevelSegments);
				break;
			case "HARVESTABLEICONOFFSET":
				reader.Read();
				HarvestableIconOffset = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.MIBBuilding(this);
		}
	}
}
