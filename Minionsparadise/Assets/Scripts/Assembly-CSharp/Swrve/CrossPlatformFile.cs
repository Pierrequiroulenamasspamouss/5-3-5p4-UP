namespace Swrve
{
	public static class CrossPlatformFile
	{
		public static void Delete(string path)
		{
#if !UNITY_WEBPLAYER
			global::System.IO.File.Delete(path);
#endif
		}

		public static bool Exists(string path)
		{
#if !UNITY_WEBPLAYER
			return global::System.IO.File.Exists(path);
#else
			return false;
#endif
		}

		public static byte[] ReadAllBytes(string path)
		{
			byte[] array = null;
#if !UNITY_WEBPLAYER
			using (global::System.IO.FileStream fileStream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open))
			{
				array = new byte[fileStream.Length];
				using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(fileStream))
				{
					binaryReader.Read(array, 0, (int)fileStream.Length);
					return array;
				}
			}
#else
			return array;
#endif
		}

		public static string LoadText(string path)
		{
			string text = null;
#if !UNITY_WEBPLAYER
			using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open))
			{
				using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(stream))
				{
					return streamReader.ReadToEnd();
				}
			}
#else
			return text;
#endif
		}

		public static void SaveBytes(string path, byte[] bytes)
		{
#if !UNITY_WEBPLAYER
			using (global::System.IO.FileStream output = global::System.IO.File.Open(path, global::System.IO.FileMode.Create))
			{
				using (global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(output))
				{
					binaryWriter.Write(bytes);
				}
			}
#endif
		}

		public static void SaveText(string path, string data)
		{
#if !UNITY_WEBPLAYER
			using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Create))
			{
				using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(stream))
				{
					streamWriter.Write(data);
				}
			}
#endif
		}
	}
}
