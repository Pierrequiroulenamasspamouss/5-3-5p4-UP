namespace Kampai.Util
{
	public static class ResourceQualityHelper
	{
		public enum LODQuality
		{
			LOD0 = 0,
			LOD1 = 1,
			LOD2 = 2,
			LOD3 = 3
		}

		public const string LODDirectoryPattern = "\\bLOD[0-3]\\b";

		public const string LODPatternInFullPath = "/LOD[0-3]";

		public const global::Kampai.Util.ResourceQualityHelper.LODQuality defaultLODQuality = global::Kampai.Util.ResourceQualityHelper.LODQuality.LOD0;

		public static string ConvertLODToQualityString(global::Kampai.Util.ResourceQualityHelper.LODQuality quality)
		{
			switch (quality)
			{
			case global::Kampai.Util.ResourceQualityHelper.LODQuality.LOD0:
				return "HIGH";
			case global::Kampai.Util.ResourceQualityHelper.LODQuality.LOD1:
				return "MED";
			case global::Kampai.Util.ResourceQualityHelper.LODQuality.LOD2:
				return "LOW";
			case global::Kampai.Util.ResourceQualityHelper.LODQuality.LOD3:
				return "VERYLOW";
			default:
				global::UnityEngine.Debug.LogError("error in LODQuality argument: returning empty string");
				return string.Empty;
			}
		}

		public static int ConvertQualityStringToLODlevel(string quality)
		{
			if (!string.IsNullOrEmpty(quality))
			{
				switch (quality.ToUpper())
				{
				case "HIGH":
					return 0;
				case "MED":
					return 1;
				case "LOW":
					return 2;
				case "VERYLOW":
					return 3;
				}
			}
			global::UnityEngine.Debug.LogError("Invalid string argument: does not match an LOD quality.  Returning -1.");
			return -1;
		}

		public static int GetIntFromLODString(string lodString, bool fullPathSearch)
		{
			string empty = string.Empty;
			int result = -1;
			empty = ((!fullPathSearch) ? "\\bLOD[0-3]\\b" : "/LOD[0-3]");
			if (!string.IsNullOrEmpty(lodString))
			{
				global::System.Text.RegularExpressions.Match match = global::System.Text.RegularExpressions.Regex.Match(lodString, empty);
				if (match.Success && !int.TryParse(global::System.Text.RegularExpressions.Regex.Match(match.Value, "\\d").Value, out result))
				{
					result = -1;
				}
			}
			return result;
		}

		public static global::System.Collections.Generic.HashSet<int> GetIntHashSetOfLevels()
		{
			global::System.Collections.Generic.HashSet<int> hashSet = new global::System.Collections.Generic.HashSet<int>();
			foreach (int value in global::System.Enum.GetValues(typeof(global::Kampai.Util.ResourceQualityHelper.LODQuality)))
			{
				hashSet.Add(value);
			}
			return hashSet;
		}

		public static int GetLODQualityEnumCount()
		{
			return global::System.Enum.GetValues(typeof(global::Kampai.Util.ResourceQualityHelper.LODQuality)).Length;
		}
	}
}
