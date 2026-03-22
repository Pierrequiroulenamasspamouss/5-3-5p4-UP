internal class MarshalUtility
{
	// public const string CINTERFACE_LIB_NAME = "NimbleCInterface";

	// Nimble-related marshaling methods removed.
	// JSON conversion methods kept below.

	internal static object ConvertJsonToObject(global::SimpleJSON.JSONNode jsonNode)
	{
		if (jsonNode == null)
		{
			return null;
		}
		global::SimpleJSON.JSONArray asArray = jsonNode.AsArray;
		if (asArray != null)
		{
			return ConvertJsonToList(asArray);
		}
		global::SimpleJSON.JSONClass asObject = jsonNode.AsObject;
		if (asObject != null)
		{
			return ConvertJsonToDictionary(asObject);
		}
		return jsonNode.Value;
	}

	internal static global::System.Collections.Generic.List<object> ConvertJsonToList(global::SimpleJSON.JSONArray jsonArray)
	{
		global::System.Collections.Generic.List<object> list = new global::System.Collections.Generic.List<object>();
		if (jsonArray != null)
		{
			foreach (global::SimpleJSON.JSONNode item in jsonArray)
			{
				list.Add(ConvertJsonToObject(item));
			}
		}
		return list;
	}

	internal static global::System.Collections.Generic.Dictionary<string, object> ConvertJsonToDictionary(global::SimpleJSON.JSONClass jsonObject)
	{
		global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
		if (jsonObject != null)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::SimpleJSON.JSONNode> item in jsonObject)
			{
				dictionary.Add(item.Key, ConvertJsonToObject(item.Value));
			}
		}
		return dictionary;
	}
}
