namespace Kampai.Game
{
	public class ConnectableBuildingDefinition : global::Kampai.Game.DecorationBuildingDefinition
	{
		private const int NumPrefabs = 7;

		public override int TypeCode
		{
			get
			{
				return 1043;
			}
		}

		public int connectableType { get; set; }

		public global::Kampai.Game.ConnectablePiecePrefabDefinition piecePrefabs { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(connectableType);
			global::Kampai.Util.BinarySerializationUtil.WriteConnectablePiecePrefabDefinition(writer, piecePrefabs);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			connectableType = reader.ReadInt32();
			piecePrefabs = global::Kampai.Util.BinarySerializationUtil.ReadConnectablePiecePrefabDefinition(reader);
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
					piecePrefabs = global::Kampai.Util.ReaderUtil.ReadConnectablePiecePrefabDefinition(reader, converters);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "CONNECTABLETYPE":
				reader.Read();
				connectableType = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.ConnectableBuilding(this);
		}

		public int GetNumPrefabs()
		{
			return 7;
		}

		public override string GetPrefab(int index = 0)
		{
			switch (index)
			{
			case 0:
				return piecePrefabs.straight;
			case 1:
				return piecePrefabs.cross;
			case 2:
				return piecePrefabs.post;
			case 3:
				return piecePrefabs.tshape;
			case 4:
				return piecePrefabs.endcap;
			case 5:
				return piecePrefabs.corner;
			default:
				return piecePrefabs.straight;
			}
		}

		public int GetDefaultPrefabIndex()
		{
			return 2;
		}
	}
}
