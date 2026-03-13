namespace Kampai.Game
{
	public class TSMCharacterDefinition : global::Kampai.Game.NamedCharacterDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1083;
			}
		}

		public global::System.Collections.Generic.IList<global::UnityEngine.Vector3> IntroPath { get; set; }

		public float IntroTime { get; set; }

		public int CooldownInSeconds { get; set; }

		public int CooldownMignetteDelayInSeconds { get; set; }

		public int PartyAnimationId { get; set; }

		public string TreasureController { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteVector3, IntroPath);
			writer.Write(IntroTime);
			writer.Write(CooldownInSeconds);
			writer.Write(CooldownMignetteDelayInSeconds);
			writer.Write(PartyAnimationId);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TreasureController);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			IntroPath = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadVector3, IntroPath);
			IntroTime = reader.ReadSingle();
			CooldownInSeconds = reader.ReadInt32();
			CooldownMignetteDelayInSeconds = reader.ReadInt32();
			PartyAnimationId = reader.ReadInt32();
			TreasureController = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "INTROPATH":
				reader.Read();
				IntroPath = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadVector3, IntroPath);
				break;
			case "INTROTIME":
				reader.Read();
				IntroTime = global::System.Convert.ToSingle(reader.Value);
				break;
			case "COOLDOWNINSECONDS":
				reader.Read();
				CooldownInSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COOLDOWNMIGNETTEDELAYINSECONDS":
				reader.Read();
				CooldownMignetteDelayInSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYANIMATIONID":
				reader.Read();
				PartyAnimationId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TREASURECONTROLLER":
				reader.Read();
				TreasureController = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.TSMCharacter(this);
		}
	}
}
