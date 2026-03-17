namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaModelBuilder
	{
		private global::Newtonsoft.Json.Schema.JsonSchemaNodeCollection _nodes = new global::Newtonsoft.Json.Schema.JsonSchemaNodeCollection();

		private global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Schema.JsonSchemaNode, global::Newtonsoft.Json.Schema.JsonSchemaModel> _nodeModels = new global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Schema.JsonSchemaNode, global::Newtonsoft.Json.Schema.JsonSchemaModel>();

		private global::Newtonsoft.Json.Schema.JsonSchemaNode _node;

		public global::Newtonsoft.Json.Schema.JsonSchemaModel Build(global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			_nodes = new global::Newtonsoft.Json.Schema.JsonSchemaNodeCollection();
			_node = AddSchema(null, schema);
			_nodeModels = new global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Schema.JsonSchemaNode, global::Newtonsoft.Json.Schema.JsonSchemaModel>();
			return BuildNodeModel(_node);
		}

		public global::Newtonsoft.Json.Schema.JsonSchemaNode AddSchema(global::Newtonsoft.Json.Schema.JsonSchemaNode existingNode, global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			string id;
			if (existingNode != null)
			{
				if (existingNode.Schemas.Contains(schema))
				{
					return existingNode;
				}
				id = global::Newtonsoft.Json.Schema.JsonSchemaNode.GetId(global::System.Linq.Enumerable.Union(existingNode.Schemas, new global::Newtonsoft.Json.Schema.JsonSchema[1] { schema }));
			}
			else
			{
				id = global::Newtonsoft.Json.Schema.JsonSchemaNode.GetId(new global::Newtonsoft.Json.Schema.JsonSchema[1] { schema });
			}
			if (_nodes.Contains(id))
			{
				return _nodes[id];
			}
			global::Newtonsoft.Json.Schema.JsonSchemaNode jsonSchemaNode = ((existingNode != null) ? existingNode.Combine(schema) : new global::Newtonsoft.Json.Schema.JsonSchemaNode(schema));
			_nodes.Add(jsonSchemaNode);
			AddProperties(schema.Properties, jsonSchemaNode.Properties);
			AddProperties(schema.PatternProperties, jsonSchemaNode.PatternProperties);
			if (schema.Items != null)
			{
				for (int i = 0; i < schema.Items.Count; i++)
				{
					AddItem(jsonSchemaNode, i, schema.Items[i]);
				}
			}
			if (schema.AdditionalProperties != null)
			{
				AddAdditionalProperties(jsonSchemaNode, schema.AdditionalProperties);
			}
			if (schema.Extends != null)
			{
				jsonSchemaNode = AddSchema(jsonSchemaNode, schema.Extends);
			}
			return jsonSchemaNode;
		}

		public void AddProperties(global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchema> source, global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaNode> target)
		{
			if (source == null)
			{
				return;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Newtonsoft.Json.Schema.JsonSchema> item in source)
			{
				AddProperty(target, item.Key, item.Value);
			}
		}

		public void AddProperty(global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaNode> target, string propertyName, global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			global::Newtonsoft.Json.Schema.JsonSchemaNode value;
			target.TryGetValue(propertyName, out value);
			target[propertyName] = AddSchema(value, schema);
		}

		public void AddItem(global::Newtonsoft.Json.Schema.JsonSchemaNode parentNode, int index, global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			global::Newtonsoft.Json.Schema.JsonSchemaNode existingNode = ((parentNode.Items.Count > index) ? parentNode.Items[index] : null);
			global::Newtonsoft.Json.Schema.JsonSchemaNode jsonSchemaNode = AddSchema(existingNode, schema);
			if (parentNode.Items.Count <= index)
			{
				parentNode.Items.Add(jsonSchemaNode);
			}
			else
			{
				parentNode.Items[index] = jsonSchemaNode;
			}
		}

		public void AddAdditionalProperties(global::Newtonsoft.Json.Schema.JsonSchemaNode parentNode, global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			parentNode.AdditionalProperties = AddSchema(parentNode.AdditionalProperties, schema);
		}

		private global::Newtonsoft.Json.Schema.JsonSchemaModel BuildNodeModel(global::Newtonsoft.Json.Schema.JsonSchemaNode node)
		{
			global::Newtonsoft.Json.Schema.JsonSchemaModel value;
			if (_nodeModels.TryGetValue(node, out value))
			{
				return value;
			}
			value = global::Newtonsoft.Json.Schema.JsonSchemaModel.Create(node.Schemas);
			_nodeModels[node] = value;
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Newtonsoft.Json.Schema.JsonSchemaNode> property in node.Properties)
			{
				if (value.Properties == null)
				{
					value.Properties = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaModel>();
				}
				value.Properties[property.Key] = BuildNodeModel(property.Value);
			}
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Newtonsoft.Json.Schema.JsonSchemaNode> patternProperty in node.PatternProperties)
			{
				if (value.PatternProperties == null)
				{
					value.PatternProperties = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaModel>();
				}
				value.PatternProperties[patternProperty.Key] = BuildNodeModel(patternProperty.Value);
			}
			for (int i = 0; i < node.Items.Count; i++)
			{
				if (value.Items == null)
				{
					value.Items = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaModel>();
				}
				value.Items.Add(BuildNodeModel(node.Items[i]));
			}
			if (node.AdditionalProperties != null)
			{
				value.AdditionalProperties = BuildNodeModel(node.AdditionalProperties);
			}
			return value;
		}
	}
}
