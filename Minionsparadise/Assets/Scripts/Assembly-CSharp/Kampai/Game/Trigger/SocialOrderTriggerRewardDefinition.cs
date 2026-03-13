namespace Kampai.Game.Trigger
{
	public class SocialOrderTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1184;
			}
		}

		public int OrderId { get; set; }

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.SocialOrder;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(OrderId);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			OrderId = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ORDERID":
				reader.Read();
				OrderId = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = context.injectionBinder;
			global::Kampai.Game.ITimedSocialEventService instance = injectionBinder.GetInstance<global::Kampai.Game.ITimedSocialEventService>();
			global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("SocialOrderTriggerRewardDefinition") as global::Kampai.Util.IKampaiLogger;
			global::Kampai.Game.TimedSocialEventDefinition currentSocialEvent = instance.GetCurrentSocialEvent();
			if (currentSocialEvent == null)
			{
				kampaiLogger.Error("No social order available.");
				return;
			}
			global::Kampai.Game.SocialTeamResponse socialEventStateCached = instance.GetSocialEventStateCached(currentSocialEvent.ID);
			if (socialEventStateCached == null || socialEventStateCached.Team == null)
			{
				kampaiLogger.Error("No social team available.");
			}
			else
			{
				foreach (global::Kampai.Game.SocialEventOrderDefinition order in currentSocialEvent.Orders)
				{
					string value = null;
					foreach (global::Kampai.Game.SocialOrderProgress item in socialEventStateCached.Team.OrderProgress)
					{
						if (item.OrderId == order.OrderID)
						{
							value = item.CompletedByUserId;
							break;
						}
					}
					if (string.IsNullOrEmpty(value) && (order.OrderID == OrderId || OrderId < 1))
					{
						global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = injectionBinder.GetInstance<global::Kampai.Game.IDefinitionService>().Get<global::Kampai.Game.Transaction.TransactionDefinition>(order.Transaction);
						injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>().GrantInputs(transactionDefinition);
						injectionBinder.GetInstance<global::Kampai.UI.View.ShowSocialPartyFillOrderSignal>().Dispatch(order.OrderID);
						return;
					}
				}
			}
			kampaiLogger.Error("No such order: {0}", OrderId);
		}
	}
}
