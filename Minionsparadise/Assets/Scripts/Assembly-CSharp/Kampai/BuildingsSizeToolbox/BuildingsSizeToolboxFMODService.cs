namespace Kampai.BuildingsSizeToolbox
{
	internal sealed class BuildingsSizeToolboxFMODService : global::Kampai.Common.Service.Audio.IFMODService
	{
		public global::System.Collections.IEnumerator InitializeSystem()
		{
			yield break;
		}

		public string GetGuid(string eventName)
		{
			return string.Empty;
		}

		public void StartAsyncBankLoadingProcessing()
		{
		}

		public bool BanksLoadingInProgress()
		{
			return false;
		}
	}
}
