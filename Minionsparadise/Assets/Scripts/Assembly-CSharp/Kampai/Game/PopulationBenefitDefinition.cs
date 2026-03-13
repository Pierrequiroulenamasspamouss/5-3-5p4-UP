namespace Kampai.Game
{
	public class PopulationBenefitDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1122;
			}
		}

		public int numMinionsRequired { get; set; }

		public int minionLevelRequired { get; set; }

		public int transactionDefinitionID { get; set; }

		public string benefitDescriptionLocalizedKey { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(numMinionsRequired);
			writer.Write(minionLevelRequired);
			writer.Write(transactionDefinitionID);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, benefitDescriptionLocalizedKey);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			numMinionsRequired = reader.ReadInt32();
			minionLevelRequired = reader.ReadInt32();
			transactionDefinitionID = reader.ReadInt32();
			benefitDescriptionLocalizedKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NUMMINIONSREQUIRED":
				reader.Read();
				numMinionsRequired = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MINIONLEVELREQUIRED":
				reader.Read();
				minionLevelRequired = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TRANSACTIONDEFINITIONID":
				reader.Read();
				transactionDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BENEFITDESCRIPTIONLOCALIZEDKEY":
				reader.Read();
				benefitDescriptionLocalizedKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
