namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaNodeCollection : global::System.Collections.ObjectModel.KeyedCollection<string, global::Newtonsoft.Json.Schema.JsonSchemaNode>
	{
		protected override string GetKeyForItem(global::Newtonsoft.Json.Schema.JsonSchemaNode item)
		{
			return item.Id;
		}
	}
}
