public interface ILocalPersistanceService
{
	void PutData(string name, string data);

	void PutDataPlayer(string name, string data);

	void PutDataInt(string name, int data);

	void PutDataIntPlayer(string name, int data);

	string GetData(string name);

	string GetDataPlayer(string name);

	int GetDataInt(string name);

	int GetDataIntPlayer(string name);

	void DeleteAll();

	bool HasKey(string name);

	bool HasKeyPlayer(string name);

	void DeleteKey(string name);

	void DeleteKeyPlayer(string name);
}
