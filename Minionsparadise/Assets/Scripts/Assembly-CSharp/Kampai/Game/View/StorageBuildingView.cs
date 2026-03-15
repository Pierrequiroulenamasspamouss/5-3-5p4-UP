namespace Kampai.Game.View
{
	public class StorageBuildingView : global::Kampai.Util.KampaiView
	{
		private global::Kampai.Game.StorageBuilding storageBuilding;

		private global::UnityEngine.Renderer grindRewardRenderer;

		private global::UnityEngine.Renderer pendingSaleRenderer;

		private global::UnityEngine.Renderer repairMarketplaceRenderer;

		private global::UnityEngine.Renderer marketplaceRenderer;

		public void Init(global::Kampai.Game.Building building)
		{
			storageBuilding = building as global::Kampai.Game.StorageBuilding;
			if (storageBuilding != null)
			{
				global::UnityEngine.Transform transform = base.transform.Find("Unique_Storage_LOD0/Unique_Storage:Unique_Storage/Unique_Storage:unique_Storage_GrindReward_Mesh");
				if (transform != null)
				{
					grindRewardRenderer = transform.GetComponent<global::UnityEngine.Renderer>();
				}
				global::UnityEngine.Transform transform2 = base.transform.Find("Unique_Storage_LOD0/Unique_Storage:Unique_Storage/Unique_Storage:unique_Storage_PendingSale_Mesh");
				if (transform2 != null)
				{
					pendingSaleRenderer = transform2.GetComponent<global::UnityEngine.Renderer>();
				}
				global::UnityEngine.Transform transform3 = base.transform.Find("Unique_Storage_LOD0/Unique_Storage:Unique_Storage/Unique_Storage:unique_Storage_RepairState_Mesh");
				if (transform3 != null)
				{
					repairMarketplaceRenderer = transform3.GetComponent<global::UnityEngine.Renderer>();
				}
				global::UnityEngine.Transform transform4 = base.transform.Find("Unique_Storage_LOD0/Unique_Storage:Unique_Storage/Unique_Storage:unique_Storage_MarketPlace_Mesh");
				if (transform4 != null)
				{
					marketplaceRenderer = transform4.GetComponent<global::UnityEngine.Renderer>();
				}
			}
		}

		public void SetMarketplaceEnabled(bool isEnabled)
		{
			if (repairMarketplaceRenderer != null)
			{
				repairMarketplaceRenderer.enabled = !isEnabled;
			}
			if (marketplaceRenderer != null)
			{
				marketplaceRenderer.enabled = isEnabled;
			}
		}

		public bool ToggleGrindReward(bool isEnabled)
		{
			bool result = false;
			if (grindRewardRenderer != null)
			{
				result = grindRewardRenderer.enabled != isEnabled;
				grindRewardRenderer.enabled = isEnabled;
			}
			return result;
		}

		public bool TogglePendingSale(bool isEnabled)
		{
			bool result = false;
			if (pendingSaleRenderer != null)
			{
				result = pendingSaleRenderer.enabled != isEnabled;
				pendingSaleRenderer.enabled = isEnabled;
			}
			return result;
		}
	}
}
