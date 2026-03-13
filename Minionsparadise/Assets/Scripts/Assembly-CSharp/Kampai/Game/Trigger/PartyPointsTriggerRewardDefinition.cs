namespace Kampai.Game.Trigger
{
	public class PartyPointsTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1180;
			}
		}

		public uint Points { get; set; }

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.PartyPoints;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Points);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Points = reader.ReadUInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "POINTS":
				reader.Read();
				Points = global::System.Convert.ToUInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = context.injectionBinder;
			if (Points != 0)
			{
				AddPoints(injectionBinder, Points);
				return;
			}
			global::Kampai.Game.MinionParty minionPartyInstance = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>().GetMinionPartyInstance();
			if (minionPartyInstance != null && minionPartyInstance.CurrentPartyPointsRequired != 0)
			{
				AddPoints(injectionBinder, minionPartyInstance.CurrentPartyPointsRequired);
				return;
			}
			global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("PartyPointsTriggerRewardDefinition") as global::Kampai.Util.IKampaiLogger;
			kampaiLogger.Error("No party points found");
		}

		private void AddPoints(global::strange.extensions.injector.api.ICrossContextInjectionBinder binder, uint points)
		{
			binder.GetInstance<global::Kampai.Game.IPlayerService>().CreateAndRunCustomTransaction(2, (int)points, global::Kampai.Game.TransactionTarget.NO_VISUAL);
			binder.GetInstance<global::Kampai.UI.View.SetXPSignal>().Dispatch();
		}
	}
}
