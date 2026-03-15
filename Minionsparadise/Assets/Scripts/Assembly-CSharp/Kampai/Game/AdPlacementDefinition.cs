namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public class AdPlacementDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1019;
			}
		}

		public global::Kampai.Game.AdPlacementName Name { get; set; }

		public int CooldownSeconds { get; set; }

		public int CooldownWatchDeclineSeconds { get; set; }

		public int MaxRewardsPerDay { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.Trigger.TriggerConditionDefinition> Conditions { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Name);
			writer.Write(CooldownSeconds);
			writer.Write(CooldownWatchDeclineSeconds);
			writer.Write(MaxRewardsPerDay);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, Conditions);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Name = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.AdPlacementName>(reader);
			CooldownSeconds = reader.ReadInt32();
			CooldownWatchDeclineSeconds = reader.ReadInt32();
			MaxRewardsPerDay = reader.ReadInt32();
			Conditions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, Conditions);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NAME":
				reader.Read();
				Name = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.AdPlacementName>(reader);
				break;
			case "COOLDOWNSECONDS":
				reader.Read();
				CooldownSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COOLDOWNWATCHDECLINESECONDS":
				reader.Read();
				CooldownWatchDeclineSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MAXREWARDSPERDAY":
				reader.Read();
				MaxRewardsPerDay = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CONDITIONS":
				reader.Read();
				Conditions = global::Kampai.Util.ReaderUtil.PopulateList<global::Kampai.Game.Trigger.TriggerConditionDefinition>(reader, converters, converters.triggerConditionDefinitionConverter, Conditions);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public virtual bool IsAvailable(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			if (Conditions == null)
			{
				return true;
			}
			for (int i = 0; i < Conditions.Count; i++)
			{
				global::Kampai.Game.Trigger.TriggerConditionDefinition triggerConditionDefinition = Conditions[i];
				if (triggerConditionDefinition == null || !triggerConditionDefinition.IsTriggered(gameContext))
				{
					return false;
				}
			}
			return true;
		}

		public virtual global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.AdPlacementInstance(this);
		}
	}
}
