namespace Kampai.Game
{
	public class BlackMarketBoardDefinition : global::Kampai.Game.AnimatingBuildingDefinition, global::Kampai.Game.ZoomableBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1035;
			}
		}

		public global::UnityEngine.Vector3 zoomOffset { get; set; }

		public global::UnityEngine.Vector3 zoomEulers { get; set; }

		public float zoomFOV { get; set; }

		public float TicketRepopTime { get; set; }

		public int RefillTime { get; set; }

		public global::System.Collections.Generic.IList<string> OrderNames { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.BlackMarketBoardUnlockedOrderSlotDefinition> UnlockTicketSlots { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.BlackMarketBoardSlotDefinition> MinMaxIngredients { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.BlackMarketBoardMultiplierDefinition> LevelBandXP { get; set; }

		public int CharacterOrderChance { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, zoomOffset);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, zoomEulers);
			writer.Write(zoomFOV);
			writer.Write(TicketRepopTime);
			writer.Write(RefillTime);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteString, OrderNames);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, UnlockTicketSlots);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, MinMaxIngredients);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, LevelBandXP);
			writer.Write(CharacterOrderChance);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			zoomOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			zoomEulers = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			zoomFOV = reader.ReadSingle();
			TicketRepopTime = reader.ReadSingle();
			RefillTime = reader.ReadInt32();
			OrderNames = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadString, OrderNames);
			UnlockTicketSlots = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, UnlockTicketSlots);
			MinMaxIngredients = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, MinMaxIngredients);
			LevelBandXP = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, LevelBandXP);
			CharacterOrderChance = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ZOOMOFFSET":
				reader.Read();
				zoomOffset = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "ZOOMEULERS":
				reader.Read();
				zoomEulers = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "ZOOMFOV":
				reader.Read();
				zoomFOV = global::System.Convert.ToSingle(reader.Value);
				break;
			case "TICKETREPOPTIME":
				reader.Read();
				TicketRepopTime = global::System.Convert.ToSingle(reader.Value);
				break;
			case "REFILLTIME":
				reader.Read();
				RefillTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ORDERNAMES":
				reader.Read();
				OrderNames = global::Kampai.Util.ReaderUtil.PopulateList<string>(reader, converters, global::Kampai.Util.ReaderUtil.ReadString, OrderNames);
				break;
			case "UNLOCKTICKETSLOTS":
				reader.Read();
				UnlockTicketSlots = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, UnlockTicketSlots);
				break;
			case "MINMAXINGREDIENTS":
				reader.Read();
				MinMaxIngredients = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, MinMaxIngredients);
				break;
			case "LEVELBANDXP":
				reader.Read();
				LevelBandXP = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, LevelBandXP);
				break;
			case "CHARACTERORDERCHANCE":
				reader.Read();
				CharacterOrderChance = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.OrderBoard(this);
		}
	}
}
