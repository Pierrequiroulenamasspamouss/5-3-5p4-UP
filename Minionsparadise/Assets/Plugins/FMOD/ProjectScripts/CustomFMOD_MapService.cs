public static class CustomFMOD_MapService
{
	private static global::System.Func<global::FMOD.RESULT, bool> ERRCHECK = global::FMOD.Studio.UnityUtil.ERRCHECK;

	public static bool TryGenerateFMODMapFiles(global::FMOD.Studio.System system)
	{
		global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<string, string>> nameIdMaps;
		if (TryGenerateMaps(system, out nameIdMaps))
		{
			string arg = string.Format("{0}/StreamingAssets", global::UnityEngine.Application.dataPath);
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::System.Collections.Generic.Dictionary<string, string>> item in nameIdMaps)
			{
				string path = string.Format("{0}/{1}_map.json", arg, item.Key);
				using (global::System.IO.FileStream stream = global::System.IO.File.Create(path))
				{
					using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(stream))
					{
						string value = SerializeStringDictionary(item.Value);
						streamWriter.Write(value);
					}
				}
			}
			return true;
		}
		return false;
	}

	private static bool TryGenerateMaps(global::FMOD.Studio.System system, out global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<string, string>> nameIdMaps)
	{
		nameIdMaps = new global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<string, string>>();
		if (!system.isValid())
		{
			return false;
		}
		try
		{
			global::FMOD.Studio.Bank[] array;
			system.getBankList(out array);
			global::FMOD.Studio.Bank[] array2 = array;
			foreach (global::FMOD.Studio.Bank bank in array2)
			{
				string path;
				ERRCHECK(bank.getPath(out path));
				global::FMOD.Studio.EventDescription[] array3;
				bank.getEventList(out array3);
				if (array3 == null)
				{
					global::UnityEngine.Debug.LogWarning("Could not generate bank mapping: 'events' was null.");
					continue;
				}
				global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
				global::FMOD.Studio.EventDescription[] array4 = array3;
				foreach (global::FMOD.Studio.EventDescription eventDescription in array4)
				{
					string path2;
					ERRCHECK(eventDescription.getPath(out path2));
					global::FMOD.GUID id;
					ERRCHECK(eventDescription.getID(out id));
					string value = GuidToString(id);
					if (path2 != null && !path2.Contains("snapshot:/"))
					{
						string key = ParseEventNameFromPath(path2);
						dictionary.Add(key, value);
					}
					else
					{
						global::UnityEngine.Debug.LogWarning("Could not add name/id pair to map: 'eventPath' was null.");
					}
				}
				if (path != null)
				{
					string key2 = ParseBankNameFromPath(path);
					nameIdMaps.Add(key2, dictionary);
				}
				else
				{
					global::UnityEngine.Debug.LogWarning("Could not generate bank mapping: 'bankPath' was null.");
				}
			}
			return true;
		}
		catch (global::System.Exception arg)
		{
			global::UnityEngine.Debug.LogError(string.Format("An error occurred while generating FMOD map files.\n{0}", arg));
		}
		return false;
	}

	private static string ParseEventNameFromPath(string eventPath)
	{
		return ParseNameFromPath(eventPath);
	}

	private static string ParseBankNameFromPath(string bankPath)
	{
		return ParseNameFromPath(bankPath);
	}

	private static string ParseNameFromPath(string path)
	{
		string[] array = path.Split(new char[1] { '/' }, global::System.StringSplitOptions.RemoveEmptyEntries);
		return array[array.Length - 1];
	}

	private static string GuidToString(global::FMOD.GUID id)
	{
		return id.ToString();
	}

	private static string SerializeStringDictionary(global::System.Collections.Generic.Dictionary<string, string> values)
	{
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
		stringBuilder.Append('{');
		bool flag = true;
		foreach (global::System.Collections.Generic.KeyValuePair<string, string> value in values)
		{
			if (!flag)
			{
				stringBuilder.Append(',');
			}
			flag = false;
			stringBuilder.Append('"');
			stringBuilder.Append(EscapeJsonString(value.Key));
			stringBuilder.Append("\":\"");
			stringBuilder.Append(EscapeJsonString(value.Value));
			stringBuilder.Append('"');
		}
		stringBuilder.Append('}');
		return stringBuilder.ToString();
	}

	private static string EscapeJsonString(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return string.Empty;
		}
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(value.Length);
		foreach (char c in value)
		{
			switch (c)
			{
			case '"':
				stringBuilder.Append("\\\"");
				break;
			case '\\':
				stringBuilder.Append("\\\\");
				break;
			case '\b':
				stringBuilder.Append("\\b");
				break;
			case '\f':
				stringBuilder.Append("\\f");
				break;
			case '\n':
				stringBuilder.Append("\\n");
				break;
			case '\r':
				stringBuilder.Append("\\r");
				break;
			case '\t':
				stringBuilder.Append("\\t");
				break;
			default:
				if (c < ' ')
				{
					stringBuilder.Append("\\u");
					stringBuilder.Append(((int)c).ToString("x4"));
				}
				else
				{
					stringBuilder.Append(c);
				}
				break;
			}
		}
		return stringBuilder.ToString();
	}
}
