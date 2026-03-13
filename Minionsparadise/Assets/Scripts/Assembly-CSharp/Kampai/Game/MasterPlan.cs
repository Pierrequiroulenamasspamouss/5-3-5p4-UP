namespace Kampai.Game
{
	public class MasterPlan : global::Kampai.Game.Instance<global::Kampai.Game.MasterPlanDefinition>, global::Kampai.Game.IGameTimeTracker
	{
		public int cooldownUTCStartTime { get; set; }

		public bool introHasBeenDisplayed { get; set; }

		public bool displayCooldownReward { get; set; }

		public bool displayCooldownAlert { get; set; }

		public int StartGameTime { get; set; }

		public int completionCount { get; set; }

		public MasterPlan(global::Kampai.Game.MasterPlanDefinition definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "COOLDOWNUTCSTARTTIME":
				reader.Read();
				cooldownUTCStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "INTROHASBEENDISPLAYED":
				reader.Read();
				introHasBeenDisplayed = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "DISPLAYCOOLDOWNREWARD":
				reader.Read();
				displayCooldownReward = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "DISPLAYCOOLDOWNALERT":
				reader.Read();
				displayCooldownAlert = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "STARTGAMETIME":
				reader.Read();
				StartGameTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COMPLETIONCOUNT":
				reader.Read();
				completionCount = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("cooldownUTCStartTime");
			writer.WriteValue(cooldownUTCStartTime);
			writer.WritePropertyName("introHasBeenDisplayed");
			writer.WriteValue(introHasBeenDisplayed);
			writer.WritePropertyName("displayCooldownReward");
			writer.WriteValue(displayCooldownReward);
			writer.WritePropertyName("displayCooldownAlert");
			writer.WriteValue(displayCooldownAlert);
			writer.WritePropertyName("StartGameTime");
			writer.WriteValue(StartGameTime);
			writer.WritePropertyName("completionCount");
			writer.WriteValue(completionCount);
		}
	}
}
