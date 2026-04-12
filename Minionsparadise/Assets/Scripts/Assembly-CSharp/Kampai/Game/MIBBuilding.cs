namespace Kampai.Game
{
	public class MIBBuilding : global::Kampai.Game.Building<global::Kampai.Game.MIBBuildingDefinition>
	{
		public int UTCExpiryTime { get; set; }

		public int UTCCooldownTime { get; set; }

		public global::Kampai.Game.MIBBuildingState MIBState { get; set; }

		public int NumOfRewardsCollectedOnTap { get; set; }

		public int NumOfRewardsCollectedOnReturn { get; set; }

		public MIBBuilding(global::Kampai.Game.MIBBuildingDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "UTCEXPIRYTIME":
				reader.Read();
				UTCExpiryTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCCOOLDOWNTIME":
				reader.Read();
				UTCCooldownTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MIBSTATE":
				reader.Read();
				MIBState = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MIBBuildingState>(reader);
				break;
			case "NUMOFREWARDSCOLLECTEDONTAP":
				reader.Read();
				NumOfRewardsCollectedOnTap = global::System.Convert.ToInt32(reader.Value);
				break;
			case "NUMOFREWARDSCOLLECTEDONRETURN":
				reader.Read();
				NumOfRewardsCollectedOnReturn = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			writer.WritePropertyName("UTCExpiryTime");
			writer.WriteValue(UTCExpiryTime);
			writer.WritePropertyName("UTCCooldownTime");
			writer.WriteValue(UTCCooldownTime);
			writer.WritePropertyName("MIBState");
			writer.WriteValue((int)MIBState);
			writer.WritePropertyName("NumOfRewardsCollectedOnTap");
			writer.WriteValue(NumOfRewardsCollectedOnTap);
			writer.WritePropertyName("NumOfRewardsCollectedOnReturn");
			writer.WriteValue(NumOfRewardsCollectedOnReturn);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.MIBBuildingObjectView>();
		}
	}
}
