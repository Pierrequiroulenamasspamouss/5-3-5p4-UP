namespace Kampai.Game.Trigger
{
	public class SocialOrderTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1174;
			}
		}

		public int PercentComplete { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.SocialOrder;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(PercentComplete);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			PercentComplete = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PERCENTCOMPLETE":
				reader.Read();
				PercentComplete = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.ITimedSocialEventService instance = injectionBinder.GetInstance<global::Kampai.Game.ITimedSocialEventService>();
			global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("SocialOrderTriggerConditionDefinition") as global::Kampai.Util.IKampaiLogger;
			global::Kampai.Game.TimedSocialEventDefinition currentSocialEvent = instance.GetCurrentSocialEvent();
			if (currentSocialEvent == null || currentSocialEvent.Orders == null || currentSocialEvent.Orders.Count == 0)
			{
				kampaiLogger.Info("No social order available.");
				return false;
			}
			global::Kampai.Game.SocialTeamResponse socialEventStateCached = instance.GetSocialEventStateCached(currentSocialEvent.ID);
			if (socialEventStateCached == null || socialEventStateCached.Team == null)
			{
				kampaiLogger.Info("No social team available.");
				return false;
			}
			return TestOperator(PercentComplete, CalculateCompletion(currentSocialEvent, socialEventStateCached));
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, PercentComplete: {3}", GetType(), base.conditionOp, type, PercentComplete);
		}

		private int CalculateCompletion(global::Kampai.Game.TimedSocialEventDefinition current, global::Kampai.Game.SocialTeamResponse team)
		{
			float num = 0f;
			foreach (global::Kampai.Game.SocialEventOrderDefinition order in current.Orders)
			{
				string value = null;
				foreach (global::Kampai.Game.SocialOrderProgress item in team.Team.OrderProgress)
				{
					if (item.OrderId == order.OrderID)
					{
						value = item.CompletedByUserId;
						break;
					}
				}
				if (!string.IsNullOrEmpty(value))
				{
					num += 1f;
				}
			}
			return (int)(num / (float)current.Orders.Count * 100f);
		}
	}
}
