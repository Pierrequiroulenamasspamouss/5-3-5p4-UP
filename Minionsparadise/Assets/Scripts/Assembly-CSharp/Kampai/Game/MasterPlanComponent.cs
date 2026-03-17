namespace Kampai.Game
{
	public class MasterPlanComponent : global::Kampai.Game.Instance<global::Kampai.Game.MasterPlanComponentDefinition>
	{
		public global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentTask> tasks = new global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentTask>();

		public global::Kampai.Game.MasterPlanComponentState State { get; set; }

		public int planTrackingInstance { get; set; }

		public int buildingDefID { get; set; }

		public global::Kampai.Game.Location buildingLocation { get; set; }

		public global::Kampai.Game.MasterPlanComponentReward reward { get; set; }

		public MasterPlanComponent(global::Kampai.Game.MasterPlanComponentDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STATE":
				reader.Read();
				State = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MasterPlanComponentState>(reader);
				break;
			case "PLANTRACKINGINSTANCE":
				reader.Read();
				planTrackingInstance = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BUILDINGDEFID":
				reader.Read();
				buildingDefID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BUILDINGLOCATION":
				reader.Read();
				buildingLocation = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "REWARD":
				reader.Read();
				reward = global::Kampai.Util.ReaderUtil.ReadMasterPlanComponentReward(reader, converters);
				break;
			case "TASKS":
				reader.Read();
				tasks = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMasterPlanComponentTask, tasks);
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
			writer.WritePropertyName("State");
			writer.WriteValue((int)State);
			writer.WritePropertyName("planTrackingInstance");
			writer.WriteValue(planTrackingInstance);
			writer.WritePropertyName("buildingDefID");
			writer.WriteValue(buildingDefID);
			if (buildingLocation != null)
			{
				writer.WritePropertyName("buildingLocation");
				writer.WriteStartObject();
				writer.WritePropertyName("x");
				writer.WriteValue(buildingLocation.x);
				writer.WritePropertyName("y");
				writer.WriteValue(buildingLocation.y);
				writer.WriteEndObject();
			}
			if (reward != null)
			{
				writer.WritePropertyName("reward");
				writer.WriteStartObject();
				if (reward.Definition != null)
				{
					writer.WritePropertyName("Definition");
					writer.WriteStartObject();
					writer.WritePropertyName("rewardItemId");
					writer.WriteValue(reward.Definition.rewardItemId);
					writer.WritePropertyName("rewardQuantity");
					writer.WriteValue(reward.Definition.rewardQuantity);
					writer.WritePropertyName("grindReward");
					writer.WriteValue(reward.Definition.grindReward);
					writer.WritePropertyName("premiumReward");
					writer.WriteValue(reward.Definition.premiumReward);
					writer.WriteEndObject();
				}
				writer.WriteEndObject();
			}
			if (tasks == null)
			{
				return;
			}
			writer.WritePropertyName("tasks");
			writer.WriteStartArray();
			global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentTask>.Enumerator enumerator = tasks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::Kampai.Game.MasterPlanComponentTask current = enumerator.Current;
					writer.WriteStartObject();
					writer.WritePropertyName("isComplete");
					writer.WriteValue(current.isComplete);
					writer.WritePropertyName("earnedQuantity");
					writer.WriteValue(current.earnedQuantity);
					if (current.Definition != null)
					{
						writer.WritePropertyName("Definition");
						writer.WriteStartObject();
						writer.WritePropertyName("requiredItemId");
						writer.WriteValue(current.Definition.requiredItemId);
						writer.WritePropertyName("requiredQuantity");
						writer.WriteValue(current.Definition.requiredQuantity);
						writer.WritePropertyName("ShowWayfinder");
						writer.WriteValue(current.Definition.ShowWayfinder);
						writer.WritePropertyName("Type");
						writer.WriteValue((int)current.Definition.Type);
						writer.WriteEndObject();
					}
					writer.WriteEndObject();
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			writer.WriteEndArray();
		}
	}
}
