public class EncryptionService : IEncryptionService
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("EncryptionService") as global::Kampai.Util.IKampaiLogger;

	private int Iterations = 2;

	private int KeySize = 256;

	private string Salt = "s499bgcalptrefxe";

	private string Vector = "087gbfgx3278kmnu";

	public string Encrypt(string plainText, string password)
	{
		byte[] bytes = global::System.Text.Encoding.ASCII.GetBytes(Vector);
		byte[] bytes2 = global::System.Text.Encoding.ASCII.GetBytes(Salt);
		byte[] bytes3 = global::System.Text.Encoding.UTF8.GetBytes(plainText);
		byte[] inArray = null;
		using (global::System.Security.Cryptography.SymmetricAlgorithm symmetricAlgorithm = new global::System.Security.Cryptography.RijndaelManaged())
		{
			global::System.Security.Cryptography.Rfc2898DeriveBytes rfc2898DeriveBytes = new global::System.Security.Cryptography.Rfc2898DeriveBytes(password, bytes2, Iterations);
			byte[] bytes4 = rfc2898DeriveBytes.GetBytes(KeySize / 8);
			symmetricAlgorithm.Mode = global::System.Security.Cryptography.CipherMode.CBC;
			using (global::System.Security.Cryptography.ICryptoTransform transform = symmetricAlgorithm.CreateEncryptor(bytes4, bytes))
			{
				using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream())
				{
					using (global::System.Security.Cryptography.CryptoStream cryptoStream = new global::System.Security.Cryptography.CryptoStream(memoryStream, transform, global::System.Security.Cryptography.CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytes3, 0, bytes3.Length);
						cryptoStream.FlushFinalBlock();
						inArray = memoryStream.ToArray();
					}
				}
			}
			symmetricAlgorithm.Clear();
		}
		return global::System.Convert.ToBase64String(inArray);
	}

	public bool TryDecrypt(string cipherText, string password, out string plainText)
	{
		byte[] bytes = global::System.Text.Encoding.ASCII.GetBytes(Vector);
		byte[] bytes2 = global::System.Text.Encoding.ASCII.GetBytes(Salt);
		byte[] array = global::System.Convert.FromBase64String(cipherText);
		int count = 0;
		byte[] array2 = null;
		using (global::System.Security.Cryptography.SymmetricAlgorithm symmetricAlgorithm = new global::System.Security.Cryptography.RijndaelManaged())
		{
			global::System.Security.Cryptography.Rfc2898DeriveBytes rfc2898DeriveBytes = new global::System.Security.Cryptography.Rfc2898DeriveBytes(password, bytes2, Iterations);
			byte[] bytes3 = rfc2898DeriveBytes.GetBytes(KeySize / 8);
			symmetricAlgorithm.Mode = global::System.Security.Cryptography.CipherMode.CBC;
			try
			{
				using (global::System.Security.Cryptography.ICryptoTransform transform = symmetricAlgorithm.CreateDecryptor(bytes3, bytes))
				{
					using (global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream(array))
					{
						using (global::System.Security.Cryptography.CryptoStream cryptoStream = new global::System.Security.Cryptography.CryptoStream(stream, transform, global::System.Security.Cryptography.CryptoStreamMode.Read))
						{
							array2 = new byte[array.Length];
							count = cryptoStream.Read(array2, 0, array2.Length);
						}
					}
				}
			}
			catch (global::System.Security.Cryptography.CryptographicException ex)
			{
				logger.Error("failed to decrypt data " + ex.Message);
				plainText = string.Empty;
				return false;
			}
			symmetricAlgorithm.Clear();
		}
		plainText = global::System.Text.Encoding.UTF8.GetString(array2, 0, count);
		return true;
	}
}
