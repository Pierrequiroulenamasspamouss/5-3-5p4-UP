namespace Kampai.Game
{
	public class TaskLevelBandDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1146;
			}
		}

		public int MinLevel { get; set; }

		public float FillOrderTaskMinMultiplier { get; set; }

		public float FillOrderTaskMaxMultiplier { get; set; }

		public float GiveTaskMinMultiplier { get; set; }

		public float GiveTaskMaxMultiplier { get; set; }

		public int GiveTaskMinQuantity { get; set; }

		public int XpReward { get; set; }

		public float MinXpMultiplier { get; set; }

		public float MaxXpMultiplier { get; set; }

		public int GrindReward { get; set; }

		public float MinGrindMultiplier { get; set; }

		public float MaxGrindMultiplier { get; set; }

		public int DropOdds { get; set; }

		public int PickWeightsId { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(MinLevel);
			writer.Write(FillOrderTaskMinMultiplier);
			writer.Write(FillOrderTaskMaxMultiplier);
			writer.Write(GiveTaskMinMultiplier);
			writer.Write(GiveTaskMaxMultiplier);
			writer.Write(GiveTaskMinQuantity);
			writer.Write(XpReward);
			writer.Write(MinXpMultiplier);
			writer.Write(MaxXpMultiplier);
			writer.Write(GrindReward);
			writer.Write(MinGrindMultiplier);
			writer.Write(MaxGrindMultiplier);
			writer.Write(DropOdds);
			writer.Write(PickWeightsId);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MinLevel = reader.ReadInt32();
			FillOrderTaskMinMultiplier = reader.ReadSingle();
			FillOrderTaskMaxMultiplier = reader.ReadSingle();
			GiveTaskMinMultiplier = reader.ReadSingle();
			GiveTaskMaxMultiplier = reader.ReadSingle();
			GiveTaskMinQuantity = reader.ReadInt32();
			XpReward = reader.ReadInt32();
			MinXpMultiplier = reader.ReadSingle();
			MaxXpMultiplier = reader.ReadSingle();
			GrindReward = reader.ReadInt32();
			MinGrindMultiplier = reader.ReadSingle();
			MaxGrindMultiplier = reader.ReadSingle();
			DropOdds = reader.ReadInt32();
			PickWeightsId = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MINLEVEL":
				reader.Read();
				MinLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "FILLORDERTASKMINMULTIPLIER":
				reader.Read();
				FillOrderTaskMinMultiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "FILLORDERTASKMAXMULTIPLIER":
				reader.Read();
				FillOrderTaskMaxMultiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "GIVETASKMINMULTIPLIER":
				reader.Read();
				GiveTaskMinMultiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "GIVETASKMAXMULTIPLIER":
				reader.Read();
				GiveTaskMaxMultiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "GIVETASKMINQUANTITY":
				reader.Read();
				GiveTaskMinQuantity = global::System.Convert.ToInt32(reader.Value);
				break;
			case "XPREWARD":
				reader.Read();
				XpReward = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MINXPMULTIPLIER":
				reader.Read();
				MinXpMultiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "MAXXPMULTIPLIER":
				reader.Read();
				MaxXpMultiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "GRINDREWARD":
				reader.Read();
				GrindReward = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MINGRINDMULTIPLIER":
				reader.Read();
				MinGrindMultiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "MAXGRINDMULTIPLIER":
				reader.Read();
				MaxGrindMultiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "DROPODDS":
				reader.Read();
				DropOdds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PICKWEIGHTSID":
				reader.Read();
				PickWeightsId = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
