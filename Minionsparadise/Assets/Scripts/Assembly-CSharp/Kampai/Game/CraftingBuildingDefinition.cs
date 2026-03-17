namespace Kampai.Game
{
	public class CraftingBuildingDefinition : global::Kampai.Game.AnimatingBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1045;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.RecipeDefinition> RecipeDefinitions { get; set; }

		public int InitialSlots { get; set; }

		public int MaxQueueSlots { get; set; }

		public int SlotCost { get; set; }

		public int SlotIncrementalCost { get; set; }

		public global::UnityEngine.Vector3 HarvestableIconOffset { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, RecipeDefinitions);
			writer.Write(InitialSlots);
			writer.Write(MaxQueueSlots);
			writer.Write(SlotCost);
			writer.Write(SlotIncrementalCost);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, HarvestableIconOffset);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			RecipeDefinitions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, RecipeDefinitions);
			InitialSlots = reader.ReadInt32();
			MaxQueueSlots = reader.ReadInt32();
			SlotCost = reader.ReadInt32();
			SlotIncrementalCost = reader.ReadInt32();
			HarvestableIconOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "RECIPEDEFINITIONS":
				reader.Read();
				RecipeDefinitions = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, RecipeDefinitions);
				break;
			case "INITIALSLOTS":
				reader.Read();
				InitialSlots = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MAXQUEUESLOTS":
				reader.Read();
				MaxQueueSlots = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SLOTCOST":
				reader.Read();
				SlotCost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SLOTINCREMENTALCOST":
				reader.Read();
				SlotIncrementalCost = global::System.Convert.ToInt32(reader.Value);
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
			return new global::Kampai.Game.CraftingBuilding(this);
		}
	}
}
