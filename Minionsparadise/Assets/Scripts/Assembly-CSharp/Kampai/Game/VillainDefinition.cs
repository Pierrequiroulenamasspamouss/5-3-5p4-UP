namespace Kampai.Game
{
	public class VillainDefinition : global::Kampai.Game.NamedCharacterDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1187;
			}
		}

		public int LoopCountMin { get; set; }

		public int LoopCountMax { get; set; }

		public string AsmCabana { get; set; }

		public string AsmFarewell { get; set; }

		public string AsmBoat { get; set; }

		public string WelcomeDialogKey { get; set; }

		public string FarewellDialogKey { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(LoopCountMin);
			writer.Write(LoopCountMax);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AsmCabana);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AsmFarewell);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AsmBoat);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, WelcomeDialogKey);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, FarewellDialogKey);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			LoopCountMin = reader.ReadInt32();
			LoopCountMax = reader.ReadInt32();
			AsmCabana = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			AsmFarewell = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			AsmBoat = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			WelcomeDialogKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			FarewellDialogKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "LOOPCOUNTMIN":
				reader.Read();
				LoopCountMin = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LOOPCOUNTMAX":
				reader.Read();
				LoopCountMax = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ASMCABANA":
				reader.Read();
				AsmCabana = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ASMFAREWELL":
				reader.Read();
				AsmFarewell = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ASMBOAT":
				reader.Read();
				AsmBoat = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "WELCOMEDIALOGKEY":
				reader.Read();
				WelcomeDialogKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "FAREWELLDIALOGKEY":
				reader.Read();
				FarewellDialogKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Villain(this);
		}
	}
}
