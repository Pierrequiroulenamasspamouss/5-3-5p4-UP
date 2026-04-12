public interface IEncryptionService
{
	string Encrypt(string plainText, string password);

	bool TryDecrypt(string cipherText, string password, out string plainText);
}
