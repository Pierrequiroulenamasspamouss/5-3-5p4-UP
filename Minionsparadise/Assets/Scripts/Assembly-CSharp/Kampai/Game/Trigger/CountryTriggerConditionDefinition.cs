namespace Kampai.Game.Trigger
{
	public class CountryTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public global::System.Collections.Generic.List<string> Countries;

		public override int TypeCode
		{
			get
			{
				return 1155;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Country;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteString, Countries);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Countries = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadString, Countries);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "COUNTRIES":
				reader.Read();
				Countries = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadString, Countries);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			string country = gameContext.injectionBinder.GetInstance<global::Kampai.Main.ILocalizationService>().GetCountry();
			return global::Kampai.Util.ListUtil.StringIsInList(country, Countries);
		}
	}
}
