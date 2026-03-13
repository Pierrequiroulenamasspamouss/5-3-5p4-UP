namespace Kampai.Game
{
	public class DebrisBuildingDefinition : global::Kampai.Game.TaskableBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1048;
			}
		}

		public int TransactionID { get; set; }

		public global::System.Collections.Generic.IList<string> VFXPrefabs { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(TransactionID);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteString, VFXPrefabs);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			TransactionID = reader.ReadInt32();
			VFXPrefabs = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadString, VFXPrefabs);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
                        int num = 1; //FIX USE OF UNASSIGNED VARIABLE
				if (num == 1)
				{
					reader.Read();
					VFXPrefabs = global::Kampai.Util.ReaderUtil.PopulateList<string>(reader, converters, global::Kampai.Util.ReaderUtil.ReadString, VFXPrefabs);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "TRANSACTIONID":
				reader.Read();
				TransactionID = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.DebrisBuilding(this);
		}
	}
}
