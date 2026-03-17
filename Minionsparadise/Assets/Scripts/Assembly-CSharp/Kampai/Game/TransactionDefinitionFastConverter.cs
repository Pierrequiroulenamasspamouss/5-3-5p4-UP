namespace Kampai.Game
{
	public class TransactionDefinitionFastConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.Transaction.TransactionDefinition>
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		public TransactionDefinitionFastConverter(global::Kampai.Game.IDefinitionService definitionService)
		{
			this.definitionService = definitionService;
		}

		public override global::Kampai.Game.Transaction.TransactionDefinition ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			return definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(global::System.Convert.ToInt32(reader.Value));
		}

		public override global::Kampai.Game.Transaction.TransactionDefinition Create()
		{
			return null;
		}
	}
}
