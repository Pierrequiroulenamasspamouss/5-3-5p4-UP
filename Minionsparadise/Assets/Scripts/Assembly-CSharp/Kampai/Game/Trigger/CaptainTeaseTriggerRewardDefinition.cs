namespace Kampai.Game.Trigger
{
	public class CaptainTeaseTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1177;
			}
		}

		public int PendingRewardDefinitionID { get; set; }

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.CaptainTease;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(PendingRewardDefinitionID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			PendingRewardDefinitionID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PENDINGREWARDDEFINITIONID":
				reader.Read();
				PendingRewardDefinitionID = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
		}
	}
}
