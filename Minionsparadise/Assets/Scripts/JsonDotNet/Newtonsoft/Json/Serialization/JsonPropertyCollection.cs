namespace Newtonsoft.Json.Serialization
{
	public class JsonPropertyCollection : global::System.Collections.ObjectModel.KeyedCollection<string, global::Newtonsoft.Json.Serialization.JsonProperty>
	{
		private readonly global::System.Type _type;

		public JsonPropertyCollection(global::System.Type type)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			_type = type;
		}

		protected override string GetKeyForItem(global::Newtonsoft.Json.Serialization.JsonProperty item)
		{
			return item.PropertyName;
		}

		public void AddProperty(global::Newtonsoft.Json.Serialization.JsonProperty property)
		{
			if (Contains(property.PropertyName))
			{
				if (property.Ignored)
				{
					return;
				}
				global::Newtonsoft.Json.Serialization.JsonProperty jsonProperty = base[property.PropertyName];
				if (!jsonProperty.Ignored)
				{
					throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.", global::System.Globalization.CultureInfo.InvariantCulture, property.PropertyName, _type));
				}
				Remove(jsonProperty);
			}
			Add(property);
		}

		public global::Newtonsoft.Json.Serialization.JsonProperty GetClosestMatchProperty(string propertyName)
		{
			global::Newtonsoft.Json.Serialization.JsonProperty property = GetProperty(propertyName, global::System.StringComparison.Ordinal);
			if (property == null)
			{
				property = GetProperty(propertyName, global::System.StringComparison.OrdinalIgnoreCase);
			}
			return property;
		}

		public global::Newtonsoft.Json.Serialization.JsonProperty GetProperty(string propertyName, global::System.StringComparison comparisonType)
		{
			using (global::System.Collections.Generic.IEnumerator<global::Newtonsoft.Json.Serialization.JsonProperty> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					global::Newtonsoft.Json.Serialization.JsonProperty current = enumerator.Current;
					if (string.Equals(propertyName, current.PropertyName, comparisonType))
					{
						return current;
					}
				}
			}
			return null;
		}
	}
}
