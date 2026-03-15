namespace ICSharpCode.SharpZipLib.Zip
{
	public sealed class ZipConstants
	{
		public const int VersionMadeBy = 51;

		[global::System.Obsolete("Use VersionMadeBy instead")]
		public const int VERSION_MADE_BY = 51;

		public const int VersionStrongEncryption = 50;

		[global::System.Obsolete("Use VersionStrongEncryption instead")]
		public const int VERSION_STRONG_ENCRYPTION = 50;

		public const int VERSION_AES = 51;

		public const int VersionZip64 = 45;

		public const int LocalHeaderBaseSize = 30;

		[global::System.Obsolete("Use LocalHeaderBaseSize instead")]
		public const int LOCHDR = 30;

		public const int Zip64DataDescriptorSize = 24;

		public const int DataDescriptorSize = 16;

		[global::System.Obsolete("Use DataDescriptorSize instead")]
		public const int EXTHDR = 16;

		public const int CentralHeaderBaseSize = 46;

		[global::System.Obsolete("Use CentralHeaderBaseSize instead")]
		public const int CENHDR = 46;

		public const int EndOfCentralRecordBaseSize = 22;

		[global::System.Obsolete("Use EndOfCentralRecordBaseSize instead")]
		public const int ENDHDR = 22;

		public const int CryptoHeaderSize = 12;

		[global::System.Obsolete("Use CryptoHeaderSize instead")]
		public const int CRYPTO_HEADER_SIZE = 12;

		public const int LocalHeaderSignature = 67324752;

		[global::System.Obsolete("Use LocalHeaderSignature instead")]
		public const int LOCSIG = 67324752;

		public const int SpanningSignature = 134695760;

		[global::System.Obsolete("Use SpanningSignature instead")]
		public const int SPANNINGSIG = 134695760;

		public const int SpanningTempSignature = 808471376;

		[global::System.Obsolete("Use SpanningTempSignature instead")]
		public const int SPANTEMPSIG = 808471376;

		public const int DataDescriptorSignature = 134695760;

		[global::System.Obsolete("Use DataDescriptorSignature instead")]
		public const int EXTSIG = 134695760;

		[global::System.Obsolete("Use CentralHeaderSignature instead")]
		public const int CENSIG = 33639248;

		public const int CentralHeaderSignature = 33639248;

		public const int Zip64CentralFileHeaderSignature = 101075792;

		[global::System.Obsolete("Use Zip64CentralFileHeaderSignature instead")]
		public const int CENSIG64 = 101075792;

		public const int Zip64CentralDirLocatorSignature = 117853008;

		public const int ArchiveExtraDataSignature = 117853008;

		public const int CentralHeaderDigitalSignature = 84233040;

		[global::System.Obsolete("Use CentralHeaderDigitalSignaure instead")]
		public const int CENDIGITALSIG = 84233040;

		public const int EndOfCentralDirectorySignature = 101010256;

		[global::System.Obsolete("Use EndOfCentralDirectorySignature instead")]
		public const int ENDSIG = 101010256;

		private static int defaultCodePage = ((global::System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage == 1 || global::System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage == 2 || global::System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage == 3 || global::System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage == 42) ? 437 : global::System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage);

		public static int DefaultCodePage
		{
			get
			{
				return defaultCodePage;
			}
			set
			{
				if (value < 0 || value > 65535 || value == 1 || value == 2 || value == 3 || value == 42)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				defaultCodePage = value;
			}
		}

		public static string ConvertToString(byte[] data, int count)
		{
			if (data == null)
			{
				return string.Empty;
			}
			return global::System.Text.Encoding.GetEncoding(DefaultCodePage).GetString(data, 0, count);
		}

		public static string ConvertToString(byte[] data)
		{
			if (data == null)
			{
				return string.Empty;
			}
			return ConvertToString(data, data.Length);
		}

		public static string ConvertToStringExt(int flags, byte[] data, int count)
		{
			if (data == null)
			{
				return string.Empty;
			}
			if ((flags & 0x800) != 0)
			{
				return global::System.Text.Encoding.UTF8.GetString(data, 0, count);
			}
			return ConvertToString(data, count);
		}

		public static string ConvertToStringExt(int flags, byte[] data)
		{
			if (data == null)
			{
				return string.Empty;
			}
			if ((flags & 0x800) != 0)
			{
				return global::System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
			}
			return ConvertToString(data, data.Length);
		}

		public static byte[] ConvertToArray(string str)
		{
			if (str == null)
			{
				return new byte[0];
			}
			return global::System.Text.Encoding.GetEncoding(DefaultCodePage).GetBytes(str);
		}

		public static byte[] ConvertToArray(int flags, string str)
		{
			if (str == null)
			{
				return new byte[0];
			}
			if ((flags & 0x800) != 0)
			{
				return global::System.Text.Encoding.UTF8.GetBytes(str);
			}
			return ConvertToArray(str);
		}

		private ZipConstants()
		{
		}
	}
}
