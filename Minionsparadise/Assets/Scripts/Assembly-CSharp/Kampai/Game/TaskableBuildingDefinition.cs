namespace Kampai.Game
{
	public abstract class TaskableBuildingDefinition : global::Kampai.Game.AnimatingBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1049;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.SlotUnlock> SlotUnlocks { get; set; }

		public int DefaultSlots { get; set; }

		public int RushCost { get; set; }

		public string ModalDescription { get; set; }

		public int GagID { get; set; }

		public global::UnityEngine.Vector3 HarvestableIconOffset { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteSlotUnlock, SlotUnlocks);
			writer.Write(DefaultSlots);
			writer.Write(RushCost);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, ModalDescription);
			writer.Write(GagID);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, HarvestableIconOffset);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			SlotUnlocks = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadSlotUnlock, SlotUnlocks);
			DefaultSlots = reader.ReadInt32();
			RushCost = reader.ReadInt32();
			ModalDescription = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			GagID = reader.ReadInt32();
			HarvestableIconOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SLOTUNLOCKS":
				reader.Read();
				SlotUnlocks = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadSlotUnlock, SlotUnlocks);
				break;
			case "DEFAULTSLOTS":
				reader.Read();
				DefaultSlots = global::System.Convert.ToInt32(reader.Value);
				break;
			case "RUSHCOST":
				reader.Read();
				RushCost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MODALDESCRIPTION":
				reader.Read();
				ModalDescription = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "GAGID":
				reader.Read();
				GagID = global::System.Convert.ToInt32(reader.Value);
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
	}
}
