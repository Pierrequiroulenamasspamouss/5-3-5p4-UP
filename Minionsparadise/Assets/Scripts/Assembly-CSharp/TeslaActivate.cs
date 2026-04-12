public static class TeslaActivate
{
	public class PKCSKeyGenerator
	{
		private byte[] key = new byte[8];

		private byte[] iv = new byte[8];

		public PKCSKeyGenerator()
		{
		}

		public PKCSKeyGenerator(string keystring, byte[] salt, int md5iterations, int segments, global::System.Security.Cryptography.DESCryptoServiceProvider provider)
		{
			using (Generate(keystring, salt, md5iterations, segments, provider))
			{
			}
		}

		public byte[] GetKey()
		{
			return (byte[])key.Clone();
		}

		public byte[] GetIV()
		{
			return (byte[])iv.Clone();
		}

		public global::System.Security.Cryptography.ICryptoTransform Encryptor(global::System.Security.Cryptography.DESCryptoServiceProvider provider)
		{
			return provider.CreateEncryptor(key, iv);
		}

		public global::System.Security.Cryptography.ICryptoTransform Generate(string keystring, byte[] salt, int md5iterations, int segments, global::System.Security.Cryptography.DESCryptoServiceProvider provider)
		{
			int num = 16;
			byte[] array = new byte[num * segments];
			byte[] bytes = global::System.Text.Encoding.UTF8.GetBytes(keystring);
			byte[] array2 = new byte[bytes.Length + salt.Length];
			global::System.Array.Copy(bytes, array2, bytes.Length);
			global::System.Array.Copy(salt, 0, array2, bytes.Length, salt.Length);
			global::System.Security.Cryptography.MD5 mD = new global::System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] array3 = null;
			byte[] array4 = new byte[num + array2.Length];
			for (int i = 0; i < segments; i++)
			{
				if (i == 0)
				{
					array3 = array2;
				}
				else
				{
					global::System.Array.Copy(array3, array4, array3.Length);
					global::System.Array.Copy(array2, 0, array4, array3.Length, array2.Length);
					array3 = array4;
				}
				for (int j = 0; j < md5iterations; j++)
				{
					array3 = mD.ComputeHash(array3);
				}
				global::System.Array.Copy(array3, 0, array, i * num, array3.Length);
			}
			global::System.Array.Copy(array, 0, key, 0, 8);
			global::System.Array.Copy(array, 8, iv, 0, 8);
			return Encryptor(provider);
		}
	}

	public static string Encrypt(string json, string tokenSecret)
	{
		using (global::System.Security.Cryptography.DESCryptoServiceProvider provider = new global::System.Security.Cryptography.DESCryptoServiceProvider())
		{
			TeslaActivate.PKCSKeyGenerator pKCSKeyGenerator = new TeslaActivate.PKCSKeyGenerator(tokenSecret, new byte[8] { 162, 21, 55, 8, 202, 98, 193, 210 }, 20, 1, provider);
			using (global::System.Security.Cryptography.ICryptoTransform cryptoTransform = pKCSKeyGenerator.Encryptor(provider))
			{
				byte[] inArray = cryptoTransform.TransformFinalBlock(global::System.Text.Encoding.UTF8.GetBytes(json), 0, json.Length);
				return global::System.Convert.ToBase64String(inArray);
			}
		}
	}
}
