namespace Kampai.Game
{
	public class SynergyService : global::Kampai.Game.ISynergyService
	{
		[Inject]
		public global::Kampai.Game.UpdateUserSignal updateUserSignal { get; set; }

		public string userID
		{
			get
			{
				return global::UnityEngine.SystemInfo.deviceUniqueIdentifier;
			}
		}
	}
}
