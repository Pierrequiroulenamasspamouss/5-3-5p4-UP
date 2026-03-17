namespace Kampai.Game
{
	public class GachaAnimationDefinition : global::Kampai.Game.AnimationDefinition
	{
		public const int INFINITE_MINIONS = 4;

		public override int TypeCode
		{
			get
			{
				return 1110;
			}
		}

		public int AnimationID { get; set; }

		public int Minions { get; set; }

		public string Prefab { get; set; }

		public global::Kampai.Util.TargetPerformance MinPerformance { get; set; }

		public bool SoloExit { get; set; }

		public global::Kampai.Game.KnuckleheadednessInfo knuckleheadednessInfo { get; set; }

		public global::Kampai.Game.AnimationAlternate AnimationAlternate { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(AnimationID);
			writer.Write(Minions);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Prefab);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, MinPerformance);
			writer.Write(SoloExit);
			global::Kampai.Util.BinarySerializationUtil.WriteKnuckleheadednessInfo(writer, knuckleheadednessInfo);
			global::Kampai.Util.BinarySerializationUtil.WriteAnimationAlternate(writer, AnimationAlternate);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AnimationID = reader.ReadInt32();
			Minions = reader.ReadInt32();
			Prefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MinPerformance = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Util.TargetPerformance>(reader);
			SoloExit = reader.ReadBoolean();
			knuckleheadednessInfo = global::Kampai.Util.BinarySerializationUtil.ReadKnuckleheadednessInfo(reader);
			AnimationAlternate = global::Kampai.Util.BinarySerializationUtil.ReadAnimationAlternate(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ANIMATIONID":
				reader.Read();
				AnimationID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MINIONS":
				reader.Read();
				Minions = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREFAB":
				reader.Read();
				Prefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MINPERFORMANCE":
				reader.Read();
				MinPerformance = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Util.TargetPerformance>(reader);
				break;
			case "SOLOEXIT":
				reader.Read();
				SoloExit = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "KNUCKLEHEADEDNESSINFO":
				reader.Read();
				knuckleheadednessInfo = global::Kampai.Util.ReaderUtil.ReadKnuckleheadednessInfo(reader, converters);
				break;
			case "ANIMATIONALTERNATE":
				reader.Read();
				AnimationAlternate = global::Kampai.Util.ReaderUtil.ReadAnimationAlternate(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
