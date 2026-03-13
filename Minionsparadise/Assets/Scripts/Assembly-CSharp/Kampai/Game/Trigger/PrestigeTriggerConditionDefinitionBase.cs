namespace Kampai.Game.Trigger
{
	public abstract class PrestigeTriggerConditionDefinitionBase : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1164;
			}
		}

		public int prestigeDefinitionID { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(prestigeDefinitionID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			prestigeDefinitionID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PRESTIGEDEFINITIONID":
				reader.Read();
				prestigeDefinitionID = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IPrestigeService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPrestigeService>();
			if (instance == null)
			{
				return false;
			}
			global::Kampai.Game.Prestige prestige = instance.GetPrestige(prestigeDefinitionID, false);
			return IsTriggered(instance, prestige);
		}

		protected abstract bool IsTriggered(global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Game.Prestige prestigeCharacter);
	}
}
