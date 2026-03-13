namespace Kampai.Game.Trigger
{
	public class UpsellTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1186;
			}
		}

		public int upsellId { get; set; }

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.Upsell;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(upsellId);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			upsellId = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "UPSELLID":
				reader.Read();
				upsellId = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = context.injectionBinder;
			if (injectionBinder != null)
			{
				global::Kampai.Game.ReconcileSalesSignal instance = injectionBinder.GetInstance<global::Kampai.Game.ReconcileSalesSignal>();
				if (instance != null)
				{
					instance.Dispatch(upsellId);
				}
			}
		}
	}
}
