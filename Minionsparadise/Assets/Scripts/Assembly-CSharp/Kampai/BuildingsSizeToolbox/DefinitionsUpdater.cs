namespace Kampai.BuildingsSizeToolbox
{
	internal sealed class DefinitionsUpdater
	{
		private const string BUILDING_DEFINITIONS_SECTION = "buildingDefinitions";

		private global::System.Collections.Generic.List<string> lines = new global::System.Collections.Generic.List<string>(65536);

		private string inputFileName;

		private global::System.Text.RegularExpressions.Regex BRACE_OPEN = new global::System.Text.RegularExpressions.Regex("\\s*{");

		private global::System.Text.RegularExpressions.Regex BRACKET_CLOSE = new global::System.Text.RegularExpressions.Regex("\\s*\\]");

		private global::System.Text.RegularExpressions.Regex READ_KEY_REGEX = new global::System.Text.RegularExpressions.Regex("\\s*\\\"(\\w+)\\\":");

		private global::System.Text.RegularExpressions.Regex READ_ID_VALUE = new global::System.Text.RegularExpressions.Regex("\\s*\\\"\\w+\\\":\\s*(\\d+)");

		private global::System.Text.RegularExpressions.Regex REPLACE_FLOAT_VALUE = new global::System.Text.RegularExpressions.Regex("(\\s*\\\"\\w+\\\":\\s*)([-+]?[0-9]*\\.?[0-9]+(?:[eE][-+]?[0-9]+)?)(.*)");

		public DefinitionsUpdater(string defsPath = "_Kampai_/Resources/dev_definitions.json")
		{
			inputFileName = global::System.IO.Path.Combine(global::UnityEngine.Application.dataPath, defsPath);
		}

		public bool Update(global::System.Collections.Generic.Dictionary<int, global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo> updatedBuildings)
		{
#if !UNITY_WEBPLAYER
			using (global::System.IO.FileStream stream = new global::System.IO.FileStream(inputFileName, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read))
			{
				using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(stream))
				{
					while (!streamReader.EndOfStream)
					{
						string text = streamReader.ReadLine();
						lines.Add(text);
						if (text.Contains("buildingDefinitions"))
						{
							break;
						}
					}
					while (!streamReader.EndOfStream)
					{
						string text = streamReader.ReadLine();
						lines.Add(text);
						if (BRACE_OPEN.IsMatch(text))
						{
							ProcessBuilding(streamReader, updatedBuildings);
						}
						else if (BRACKET_CLOSE.IsMatch(text))
						{
							break;
						}
					}
					while (!streamReader.EndOfStream)
					{
						string text = streamReader.ReadLine();
						lines.Add(text);
					}
				}
			}
			using (global::System.IO.FileStream stream2 = new global::System.IO.FileStream(inputFileName, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Write))
			{
				using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(stream2))
				{
					foreach (string line in lines)
					{
						streamWriter.WriteLine(line);
					}
				}
			}
#endif
			return true;
		}

		private void ProcessBuilding(global::System.IO.StreamReader sr, global::System.Collections.Generic.Dictionary<int, global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo> updatedBuildings)
		{
			int result = -1;
			int num = -1;
			int num2 = -1;
			int num3 = 1;
			while (!sr.EndOfStream)
			{
				string text = sr.ReadLine();
				lines.Add(text);
				if (text.Contains("{"))
				{
					num3++;
				}
				if (text.Contains("}"))
				{
					num3--;
					if (num3 == 0)
					{
						break;
					}
				}
				global::System.Text.RegularExpressions.Match match = READ_KEY_REGEX.Match(text);
				switch (match.Groups[1].Value.ToUpper())
				{
				case "ID":
				{
					global::System.Text.RegularExpressions.Match match2 = READ_ID_VALUE.Match(text);
					if (match2.Success)
					{
						int.TryParse(match2.Groups[1].Value, out result);
					}
					break;
				}
				case "UIPOSITION":
					num = lines.Count - 1;
					break;
				case "UISCALE":
					num2 = lines.Count - 1;
					break;
				}
			}
			global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxManagerView.UIPositionInfo value;
			if (updatedBuildings.TryGetValue(result, out value))
			{
				int index2;
				string text2;
				if (num2 > 0)
				{
					replaceFloatInLine(num2, value.Scale);
				}
				else
				{
					int num4 = lines.Count - 1;
					string item = lines[num4];
					global::System.Collections.Generic.List<string> list2;
					global::System.Collections.Generic.List<string> list = (list2 = lines);
					int index = (index2 = num4 - 1);
					text2 = list2[index2];
					list[index] = text2 + ",";
					lines[num4] = string.Format("            \"uiScale\": {0}", value.Scale);
					lines.Add(item);
				}
				if (num > 0)
				{
					global::UnityEngine.Vector3 position = value.Position;
					replaceFloatInLine(num + 1, position.x);
					replaceFloatInLine(num + 2, position.y);
					replaceFloatInLine(num + 3, position.z);
					return;
				}
				global::UnityEngine.Vector3 position2 = value.Position;
				int num5 = lines.Count - 1;
				string item2 = lines[num5];
				global::System.Collections.Generic.List<string> list4;
				global::System.Collections.Generic.List<string> list3 = (list4 = lines);
				int index3 = (index2 = num5 - 1);
				text2 = list4[index2];
				list3[index3] = text2 + ",";
				lines[num5] = "            \"uiPosition\": {";
				lines.Add(string.Format("                \"x\": {0},", position2.x));
				lines.Add(string.Format("                \"y\": {0},", position2.y));
				lines.Add(string.Format("                \"z\": {0}", position2.z));
				lines.Add("            }");
				lines.Add(item2);
			}
		}

		private void replaceFloatInLine(int index, float value)
		{
			lines[index] = REPLACE_FLOAT_VALUE.Replace(lines[index], (global::System.Text.RegularExpressions.Match m) => m.Groups[1].Value + value + m.Groups[3].Value);
		}
	}
}
