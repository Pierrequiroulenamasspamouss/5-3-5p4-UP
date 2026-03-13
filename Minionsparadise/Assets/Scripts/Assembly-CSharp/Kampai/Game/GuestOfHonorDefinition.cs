namespace Kampai.Game
{
	public class GuestOfHonorDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1115;
			}
		}

		public global::System.Collections.Generic.List<int> buffDefinitionIDs { get; set; }

		public global::System.Collections.Generic.List<int> buffStarValues { get; set; }

		public int partyDurationBoost { get; set; }

		public float partyDurationMultipler { get; set; }

		public int availableInvites { get; set; }

		public int cooldown { get; set; }

		public int rushCostPerParty { get; set; }

		public int gohAnimationID { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, buffDefinitionIDs);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, buffStarValues);
			writer.Write(partyDurationBoost);
			writer.Write(partyDurationMultipler);
			writer.Write(availableInvites);
			writer.Write(cooldown);
			writer.Write(rushCostPerParty);
			writer.Write(gohAnimationID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			buffDefinitionIDs = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, buffDefinitionIDs);
			buffStarValues = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, buffStarValues);
			partyDurationBoost = reader.ReadInt32();
			partyDurationMultipler = reader.ReadSingle();
			availableInvites = reader.ReadInt32();
			cooldown = reader.ReadInt32();
			rushCostPerParty = reader.ReadInt32();
			gohAnimationID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BUFFDEFINITIONIDS":
				reader.Read();
				buffDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, buffDefinitionIDs);
				break;
			case "BUFFSTARVALUES":
				reader.Read();
				buffStarValues = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, buffStarValues);
				break;
			case "PARTYDURATIONBOOST":
				reader.Read();
				partyDurationBoost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYDURATIONMULTIPLER":
				reader.Read();
				partyDurationMultipler = global::System.Convert.ToSingle(reader.Value);
				break;
			case "AVAILABLEINVITES":
				reader.Read();
				availableInvites = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COOLDOWN":
				reader.Read();
				cooldown = global::System.Convert.ToInt32(reader.Value);
				break;
			case "RUSHCOSTPERPARTY":
				reader.Read();
				rushCostPerParty = global::System.Convert.ToInt32(reader.Value);
				break;
			case "GOHANIMATIONID":
				reader.Read();
				gohAnimationID = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
