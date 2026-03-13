namespace Kampai.Util
{
	public static class DownloadUtil
	{
		public const int BUF_SIZE = 4096;

		public static string CreateBundleURL(string baseUrl, string name)
		{
			return string.Format("{0}/{1}.unity3d", baseUrl, name);
		}

		public static string CreateBundlePath(string baseDLCPath, string name)
		{
			return global::System.IO.Path.Combine(baseDLCPath, string.Format("{0}.unity3d", name));
		}

		public static string GetBundleNameFromUrl(string url)
		{
			if (!url.EndsWith(".unity3d"))
			{
				return string.Empty;
			}
			int num = url.LastIndexOf('/') + 1;
			return url.Substring(num, url.Length - num - ".unity3d".Length);
		}

		public static bool IsGZipped(string filePath)
		{
			bool result = false;
#if !UNITY_WEBPLAYER
#if !UNITY_WEBPLAYER
			using (global::System.IO.Stream stream = global::System.IO.File.OpenRead(filePath))
#else
			using (global::System.IO.Stream stream = null)
#endif
			{
				byte[] array = new byte[2];
				if (stream.Length >= array.Length && stream.Read(array, 0, array.Length) != 0)
				{
					if (global::System.BitConverter.IsLittleEndian)
					{
						global::System.Array.Reverse(array);
					}
					result = global::System.BitConverter.ToInt16(array, 0) == 8075;
				}
			}
#endif
			return result;
		}

		public static string UnpackFile(string srcPath, string dstPath, string md5Sum = null, bool avoidBackup = false)
		{
			string text = null;
			try
			{
				bool flag = IsGZipped(srcPath);
				bool flag2 = string.IsNullOrEmpty(md5Sum);
				if (!flag2)
				{
					string text2 = null;
#if !UNITY_WEBPLAYER
					using (global::System.Security.Cryptography.MD5 mD = global::System.Security.Cryptography.MD5.Create())
					{
#if !UNITY_WEBPLAYER
						using (global::System.IO.Stream stream = global::System.IO.File.OpenRead(srcPath))
#else
						using (global::System.IO.Stream stream = null)
#endif
						{
							using (global::System.IO.Stream stream2 = ((!flag) ? null : new global::ICSharpCode.SharpZipLib.GZip.GZipInputStream(stream)))
							{
								text2 = global::System.BitConverter.ToString(mD.ComputeHash(stream2 ?? stream)).Replace("-", string.Empty);
							}
						}
					}
#endif
					flag2 = md5Sum.Equals(text2, global::System.StringComparison.InvariantCultureIgnoreCase);
					if (!flag2)
					{
						text = string.Format("Invalid MD5SUM {0} != {1}", md5Sum, text2.ToLower());
					}
				}
				if (flag2)
				{
					string directoryName = global::System.IO.Path.GetDirectoryName(dstPath);
#if !UNITY_WEBPLAYER
					if (!global::System.IO.Directory.Exists(directoryName))
					{
						global::System.IO.Directory.CreateDirectory(directoryName);
					}
#if !UNITY_WEBPLAYER
					if (global::System.IO.File.Exists(dstPath))
#else
					if (false)
#endif
					{
						global::System.IO.File.Delete(dstPath);
					}
#endif
#if !UNITY_WEBPLAYER
					if (flag)
					{
#if !UNITY_WEBPLAYER
						using (global::System.IO.Stream baseInputStream = global::System.IO.File.OpenRead(srcPath))
#else
						using (global::System.IO.Stream baseInputStream = null)
#endif
						{
							using (global::System.IO.Stream source = new global::ICSharpCode.SharpZipLib.GZip.GZipInputStream(baseInputStream))
							{
								using (global::System.IO.Stream destination = global::System.IO.File.Create(dstPath))
								{
									global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(source, destination, new byte[4096]);
								}
							}
						}
					}
					else
					{
						global::System.IO.File.Move(srcPath, dstPath);
					}
#endif
				}
				else if (string.IsNullOrEmpty(text))
				{
					text = "An unknown error occurred.";
				}
			}
			catch (global::ICSharpCode.SharpZipLib.SharpZipBaseException ex)
			{
				global::Kampai.Util.Native.LogError(string.Format("SharpZipBaseException Unpacking File {0}: {1}", srcPath, ex.Message));
				text = ex.Message;
			}
			return text;
		}
	}
}
