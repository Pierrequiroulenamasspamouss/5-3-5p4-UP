namespace Kampai.Game.View
{
	public class VillainLairLocationView : global::Kampai.Util.KampaiView
	{
		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> colliders;

		public global::UnityEngine.GameObject masterPlanPlatformCollider;

		internal global::System.Collections.Generic.Dictionary<int, int> colliderInstanceKeysToComponentIDs = new global::System.Collections.Generic.Dictionary<int, int>();

		internal global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject> componentIDKeysToColliders = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject>();

		public void SetUpInstanceIDs(global::Kampai.Game.MasterPlanDefinition masterPlanDef, global::Kampai.Game.IPlayerService playerService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::System.Collections.Generic.List<int> componentDefinitionIDs = masterPlanDef.ComponentDefinitionIDs;
			int count = componentDefinitionIDs.Count;
			if (count != colliders.Count)
			{
				logger.Error(string.Format("Count mismatch: we have {0} colliders and {1} components", colliders.Count, count));
			}
			for (int i = 0; i < colliders.Count; i++)
			{
				if (i < count)
				{
					int instanceID = colliders[i].GetInstanceID();
					int num = componentDefinitionIDs[i];
					colliderInstanceKeysToComponentIDs[instanceID] = num;
					componentIDKeysToColliders[num] = colliders[i];
					global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(num);
					if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.State >= global::Kampai.Game.MasterPlanComponentState.Scaffolding)
					{
						EnableCollider(num, false);
					}
				}
			}
			colliderInstanceKeysToComponentIDs[masterPlanPlatformCollider.GetInstanceID()] = masterPlanDef.BuildingDefID;
			componentIDKeysToColliders[masterPlanDef.BuildingDefID] = masterPlanPlatformCollider;
		}

		internal void EnableCollider(int componentID, bool enable)
		{
			if (componentIDKeysToColliders.ContainsKey(componentID))
			{
				componentIDKeysToColliders[componentID].SetActive(enable);
			}
		}

		internal void EnableAllColliders(bool enable)
		{
			foreach (global::UnityEngine.GameObject collider in colliders)
			{
				collider.SetActive(enable);
			}
		}
	}
}
