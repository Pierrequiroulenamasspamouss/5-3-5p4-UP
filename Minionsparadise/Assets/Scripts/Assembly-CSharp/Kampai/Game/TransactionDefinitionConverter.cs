namespace Kampai.Game
{
	public class TransactionDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.Transaction.TransactionDefinition>
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		public TransactionDefinitionConverter(global::Kampai.Game.IDefinitionService definitionService)
		{
			this.definitionService = definitionService;
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			return definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(global::System.Convert.ToInt32(reader.Value));
		}

		public override global::Kampai.Game.Transaction.TransactionDefinition Create(global::System.Type objectType)
		{
			return null;
		}
	}
}
