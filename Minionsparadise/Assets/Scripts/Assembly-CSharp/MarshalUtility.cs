internal class MarshalUtility
{
	public const string CINTERFACE_LIB_NAME = "NimbleCInterface";

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_deleteMap(global::System.IntPtr map);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_deleteData(global::System.IntPtr data);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_deleteStringArray(global::System.IntPtr array);

	internal static global::System.Collections.Generic.Dictionary<string, string> ConvertPtrToDictionary(global::System.IntPtr mapPtr)
	{
		NimbleBridge_Map nimbleBridge_Map = (NimbleBridge_Map)global::System.Runtime.InteropServices.Marshal.PtrToStructure(mapPtr, typeof(NimbleBridge_Map));
		global::System.IntPtr[] array = new global::System.IntPtr[nimbleBridge_Map.length];
		global::System.Runtime.InteropServices.Marshal.Copy(nimbleBridge_Map.keys, array, 0, nimbleBridge_Map.length);
		string[] array2 = new string[nimbleBridge_Map.length];
		for (int i = 0; i < nimbleBridge_Map.length; i++)
		{
			array2[i] = global::System.Runtime.InteropServices.Marshal.PtrToStringAuto(array[i]);
		}
		global::System.IntPtr[] array3 = new global::System.IntPtr[nimbleBridge_Map.length];
		global::System.Runtime.InteropServices.Marshal.Copy(nimbleBridge_Map.values, array3, 0, nimbleBridge_Map.length);
		string[] array4 = new string[nimbleBridge_Map.length];
		for (int j = 0; j < nimbleBridge_Map.length; j++)
		{
			array4[j] = global::System.Runtime.InteropServices.Marshal.PtrToStringAuto(array3[j]);
		}
		global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>(nimbleBridge_Map.length);
		for (int k = 0; k < nimbleBridge_Map.length; k++)
		{
			dictionary[array2[k]] = array4[k];
		}
		NimbleBridge_deleteMap(mapPtr);
		return dictionary;
	}

	internal static global::System.IntPtr ConvertDictionaryToPtr(global::System.Collections.Generic.Dictionary<string, string> dictionary)
	{
		NimbleBridge_Map nimbleBridge_Map = default(NimbleBridge_Map);
		global::System.IntPtr zero = global::System.IntPtr.Zero;
		nimbleBridge_Map.length = dictionary.Count;
		string[] array = new string[nimbleBridge_Map.length];
		string[] array2 = new string[nimbleBridge_Map.length];
		nimbleBridge_Map.keys = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)) * nimbleBridge_Map.length);
		nimbleBridge_Map.values = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)) * nimbleBridge_Map.length);
		zero = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(NimbleBridge_Map)));
		dictionary.Keys.CopyTo(array, 0);
		global::System.IntPtr[] array3 = new global::System.IntPtr[nimbleBridge_Map.length];
		for (int i = 0; i < nimbleBridge_Map.length; i++)
		{
			array3[i] = global::System.Runtime.InteropServices.Marshal.StringToHGlobalAuto(array[i]);
		}
		global::System.Runtime.InteropServices.Marshal.Copy(array3, 0, nimbleBridge_Map.keys, nimbleBridge_Map.length);
		dictionary.Values.CopyTo(array2, 0);
		global::System.IntPtr[] array4 = new global::System.IntPtr[nimbleBridge_Map.length];
		for (int j = 0; j < nimbleBridge_Map.length; j++)
		{
			array4[j] = global::System.Runtime.InteropServices.Marshal.StringToHGlobalAuto(array2[j]);
		}
		global::System.Runtime.InteropServices.Marshal.Copy(array4, 0, nimbleBridge_Map.values, nimbleBridge_Map.length);
		global::System.Runtime.InteropServices.Marshal.StructureToPtr(nimbleBridge_Map, zero, false);
		return zero;
	}

	internal static void DisposeMapPtr(global::System.IntPtr mapPtr)
	{
		if (mapPtr != global::System.IntPtr.Zero)
		{
			NimbleBridge_Map nimbleBridge_Map = (NimbleBridge_Map)global::System.Runtime.InteropServices.Marshal.PtrToStructure(mapPtr, typeof(NimbleBridge_Map));
			for (int i = 0; i < nimbleBridge_Map.length; i++)
			{
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(global::System.Runtime.InteropServices.Marshal.ReadIntPtr(nimbleBridge_Map.keys, i * global::System.IntPtr.Size));
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(global::System.Runtime.InteropServices.Marshal.ReadIntPtr(nimbleBridge_Map.values, i * global::System.IntPtr.Size));
			}
			if (nimbleBridge_Map.keys != global::System.IntPtr.Zero)
			{
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(nimbleBridge_Map.keys);
			}
			if (nimbleBridge_Map.values != global::System.IntPtr.Zero)
			{
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(nimbleBridge_Map.values);
			}
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(mapPtr);
		}
	}

	internal static byte[] ConvertPtrToData(global::System.IntPtr dataPtr)
	{
		NimbleBridge_Data nimbleBridge_Data = (NimbleBridge_Data)global::System.Runtime.InteropServices.Marshal.PtrToStructure(dataPtr, typeof(NimbleBridge_Data));
		byte[] array = new byte[nimbleBridge_Data.length];
		if (nimbleBridge_Data.bytes != global::System.IntPtr.Zero)
		{
			global::System.Runtime.InteropServices.Marshal.Copy(nimbleBridge_Data.bytes, array, 0, nimbleBridge_Data.length);
		}
		else
		{
			array = null;
		}
		NimbleBridge_deleteData(dataPtr);
		return array;
	}

	internal static global::System.IntPtr ConvertDataToPtr(byte[] array)
	{
		NimbleBridge_Data nimbleBridge_Data = default(NimbleBridge_Data);
		if (array != null)
		{
			nimbleBridge_Data.bytes = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(array.Length * 1);
			nimbleBridge_Data.length = array.Length;
			global::System.Runtime.InteropServices.Marshal.Copy(array, 0, nimbleBridge_Data.bytes, array.Length);
		}
		else
		{
			nimbleBridge_Data.bytes = global::System.IntPtr.Zero;
			nimbleBridge_Data.length = 0;
		}
		global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(nimbleBridge_Data));
		global::System.Runtime.InteropServices.Marshal.StructureToPtr(nimbleBridge_Data, intPtr, false);
		return intPtr;
	}

	internal static void DisposeDataPtr(global::System.IntPtr dataPtr)
	{
		NimbleBridge_Data nimbleBridge_Data = (NimbleBridge_Data)global::System.Runtime.InteropServices.Marshal.PtrToStructure(dataPtr, typeof(NimbleBridge_Data));
		if (nimbleBridge_Data.bytes != global::System.IntPtr.Zero)
		{
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(nimbleBridge_Data.bytes);
		}
		global::System.Runtime.InteropServices.Marshal.FreeHGlobal(dataPtr);
	}

	internal static string[] ConvertPtrToArray(global::System.IntPtr ptr)
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		if (ptr == global::System.IntPtr.Zero)
		{
			return list.ToArray();
		}
		global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(ptr);
		int num = 1;
		while (intPtr != global::System.IntPtr.Zero)
		{
			list.Add(global::System.Runtime.InteropServices.Marshal.PtrToStringAuto(intPtr));
			intPtr = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(ptr, num * global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)));
			num++;
		}
		NimbleBridge_deleteStringArray(ptr);
		return list.ToArray();
	}

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
