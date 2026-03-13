namespace Kampai.UI.View
{
	public class SpawnDooberModel
	{
		public const float TOKEN_DOOBER_WIDTH_OFFSET = 4f;

		public global::UnityEngine.Vector3 TikiBarWorldPosition { get; set; }

		public global::UnityEngine.Vector3 XPGlassPosition { get; set; }

		public global::UnityEngine.Vector3 PremiumGlassPosition { get; set; }

		public global::UnityEngine.Vector3 GrindGlassPosition { get; set; }

		public global::UnityEngine.Vector3 StorageGlassPosition { get; set; }

		public global::UnityEngine.Vector3 TokenInfoHUDPosition { get; set; }

		public global::UnityEngine.Vector3 InspirationGlassPosition { get; set; }

		public global::UnityEngine.Vector3 DiscoBallGlassPosition { get; set; }

		public global::UnityEngine.Vector3 StoreGlassPosition { get; set; }

		public global::UnityEngine.Vector3 MiscGlassPosition { get; set; }

		public int DooberCounter { get; set; }

		public int PendingPopulationDoober { get; set; }

		public bool RewardedAdDooberMode { get; set; }

		public global::UnityEngine.Vector3 expScreenPosition { get; set; }

		public global::UnityEngine.Vector3 premiumScreenPosition { get; set; }

		public global::UnityEngine.Vector3 grindScreenPosition { get; set; }

		public global::UnityEngine.Vector3 itemScreenPosition { get; set; }

		public global::UnityEngine.Vector3 defaultDooberSpawnLocation { get; set; }

		public global::UnityEngine.Vector3 rewardedAdDooberSpawnLocation { get; set; }

		public SpawnDooberModel()
		{
			expScreenPosition = new global::UnityEngine.Vector3(0.4f, 0.6f, 0f);
			premiumScreenPosition = new global::UnityEngine.Vector3(0.6f, 0.6f, 0f);
			grindScreenPosition = new global::UnityEngine.Vector3(0.4f, 0.4f, 0f);
			itemScreenPosition = new global::UnityEngine.Vector3(0.6f, 0.4f, 0f);
			defaultDooberSpawnLocation = new global::UnityEngine.Vector3(0.5f, 0.3f, 0f);
			MiscGlassPosition = (StoreGlassPosition = (DiscoBallGlassPosition = (InspirationGlassPosition = (StorageGlassPosition = (GrindGlassPosition = (PremiumGlassPosition = (XPGlassPosition = (TikiBarWorldPosition = (rewardedAdDooberSpawnLocation = global::UnityEngine.Vector3.zero)))))))));
		}
	}
}
