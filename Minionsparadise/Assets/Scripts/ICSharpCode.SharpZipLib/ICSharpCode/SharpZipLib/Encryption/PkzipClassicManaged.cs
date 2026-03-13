namespace ICSharpCode.SharpZipLib.Encryption
{
	public sealed class PkzipClassicManaged : global::ICSharpCode.SharpZipLib.Encryption.PkzipClassic
	{
		private byte[] key_;

		public override int BlockSize
		{
			get
			{
				return 8;
			}
			set
			{
				if (value != 8)
				{
					throw new global::System.Security.Cryptography.CryptographicException("Block size is invalid");
				}
			}
		}

		public override global::System.Security.Cryptography.KeySizes[] LegalKeySizes
		{
			get
			{
				return new global::System.Security.Cryptography.KeySizes[1]
				{
					new global::System.Security.Cryptography.KeySizes(96, 96, 0)
				};
			}
		}

		public override global::System.Security.Cryptography.KeySizes[] LegalBlockSizes
		{
			get
			{
				return new global::System.Security.Cryptography.KeySizes[1]
				{
					new global::System.Security.Cryptography.KeySizes(8, 8, 0)
				};
			}
		}

		public override byte[] Key
		{
			get
			{
				if (key_ == null)
				{
					GenerateKey();
				}
				return (byte[])key_.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new global::System.ArgumentNullException("value");
				}
				if (value.Length != 12)
				{
					throw new global::System.Security.Cryptography.CryptographicException("Key size is illegal");
				}
				key_ = (byte[])value.Clone();
			}
		}

		public override void GenerateIV()
		{
		}

		public override void GenerateKey()
		{
			key_ = new byte[12];
			global::System.Random random = new global::System.Random();
			random.NextBytes(key_);
		}

		public override global::System.Security.Cryptography.ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
		{
			key_ = rgbKey;
			return new global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicEncryptCryptoTransform(Key);
		}

		public override global::System.Security.Cryptography.ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
		{
			key_ = rgbKey;
			return new global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicDecryptCryptoTransform(Key);
		}
	}
}
