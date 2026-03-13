namespace Kampai.Game
{
	public class CurrencyItemConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.CurrencyItemDefinition>
	{
		private bool salePackType;

		private global::Kampai.Game.SalePackType type;

		private bool platformStoreSkuPropertyExists;

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			global::Newtonsoft.Json.Linq.JProperty jProperty = jObject.Property("type");
			if (jProperty != null)
			{
				salePackType = global::System.Enum.IsDefined(typeof(global::Kampai.Game.SalePackType), jProperty.Value.ToString());
				if (salePackType)
				{
					string value = jProperty.Value.ToString();
					type = (global::Kampai.Game.SalePackType)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.SalePackType), value);
				}
			}
			platformStoreSkuPropertyExists = jObject.Property("platformStoreSku") != null;
			reader = jObject.CreateReader();
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.CurrencyItemDefinition Create(global::System.Type objectType)
		{
			if (salePackType)
			{
				if (type == global::Kampai.Game.SalePackType.Store)
				{
					return new global::Kampai.Game.CurrencyStorePackDefinition();
				}
				return new global::Kampai.Game.SalePackDefinition();
			}
			if (platformStoreSkuPropertyExists)
			{
				return new global::Kampai.Game.PremiumCurrencyItemDefinition();
			}
			return new global::Kampai.Game.CurrencyItemDefinition();
		}
	}
}
