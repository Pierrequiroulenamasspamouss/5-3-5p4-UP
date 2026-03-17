namespace Kampai.Game.Trigger
{
	public class TriggerInstanceFastConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.Trigger.TriggerInstance>
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.Definition def;

		public TriggerInstanceFastConverter(global::Kampai.Game.IDefinitionService definitionService)
		{
			this.definitionService = definitionService;
		}

		public override global::Kampai.Game.Trigger.TriggerInstance ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			global::Newtonsoft.Json.Linq.JProperty jProperty = ((jObject.Property("def") != null) ? jObject.Property("def") : jObject.Property("Definition"));
			if (jProperty == null)
			{
				return null;
			}
			int id = global::Newtonsoft.Json.Linq.LinqExtensions.Value<int>(jProperty.Value);
			def = null;
			if (!definitionService.TryGet<global::Kampai.Game.Definition>(id, out def))
			{
				return null;
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, converters);
		}

		public override global::Kampai.Game.Trigger.TriggerInstance Create()
		{
			if (def == null)
			{
				return null;
			}
			global::Kampai.Util.IBuilder<global::Kampai.Game.Trigger.TriggerInstance> builder = def as global::Kampai.Util.IBuilder<global::Kampai.Game.Trigger.TriggerInstance>;
			object result;
			if (builder != null)
			{
				global::Kampai.Game.Trigger.TriggerInstance triggerInstance = builder.Build();
				result = triggerInstance;
			}
			else
			{
				result = null;
			}
			return (global::Kampai.Game.Trigger.TriggerInstance)result;
		}
	}
}
