namespace Kampai.Common.Service.Audio
{
	public interface IFMODService
	{
		global::System.Collections.IEnumerator InitializeSystem();

		string GetGuid(string eventName);

		void StartAsyncBankLoadingProcessing();

		bool BanksLoadingInProgress();
	}
}
