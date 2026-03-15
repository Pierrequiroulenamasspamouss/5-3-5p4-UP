namespace Newtonsoft.Json.Serialization
{
	internal class DefaultReferenceResolver : global::Newtonsoft.Json.Serialization.IReferenceResolver
	{
		private int _referenceCount;

		private global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, object> GetMappings(object context)
		{
			global::Newtonsoft.Json.Serialization.JsonSerializerInternalBase jsonSerializerInternalBase;
			if (context is global::Newtonsoft.Json.Serialization.JsonSerializerInternalBase)
			{
				jsonSerializerInternalBase = (global::Newtonsoft.Json.Serialization.JsonSerializerInternalBase)context;
			}
			else
			{
				if (!(context is global::Newtonsoft.Json.Serialization.JsonSerializerProxy))
				{
					throw new global::System.Exception("The DefaultReferenceResolver can only be used internally.");
				}
				jsonSerializerInternalBase = ((global::Newtonsoft.Json.Serialization.JsonSerializerProxy)context).GetInternalSerializer();
			}
			return jsonSerializerInternalBase.DefaultReferenceMappings;
		}

		public object ResolveReference(object context, string reference)
		{
			object second;
			GetMappings(context).TryGetByFirst(reference, out second);
			return second;
		}

		public string GetReference(object context, object value)
		{
			global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, object> mappings = GetMappings(context);
			string first;
			if (!mappings.TryGetBySecond(value, out first))
			{
				_referenceCount++;
				first = _referenceCount.ToString(global::System.Globalization.CultureInfo.InvariantCulture);
				mappings.Add(first, value);
			}
			return first;
		}

		public void AddReference(object context, string reference, object value)
		{
			GetMappings(context).Add(reference, value);
		}

		public bool IsReferenced(object context, object value)
		{
			string first;
			return GetMappings(context).TryGetBySecond(value, out first);
		}
	}
}
