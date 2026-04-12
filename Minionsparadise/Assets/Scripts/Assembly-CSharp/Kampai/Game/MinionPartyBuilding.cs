namespace Kampai.Game
{
	public interface MinionPartyBuilding : global::Kampai.Game.Building, global::Kampai.Game.IMinionPartyBuilding, global::Kampai.Game.RepairableBuilding, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
	}
	public abstract class MinionPartyBuilding<T> : global::Kampai.Game.RepairableBuilding<T>, global::Kampai.Game.Building, global::Kampai.Game.MinionPartyBuilding, global::Kampai.Game.IMinionPartyBuilding, global::Kampai.Game.RepairableBuilding, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable where T : global::Kampai.Game.MinionPartyBuildingDefinition
	{
		public MinionPartyBuilding(T definition)
			: base(definition)
		{
		}

		public string GetPartyPrefab(global::Kampai.Game.MinionPartyType partyType)
		{
			string result = string.Empty;
			string text = partyType.ToString();
			T definition = base.Definition;
			if (definition.MinionPartyPrefabs != null)
			{
				T definition2 = base.Definition;
				foreach (global::Kampai.Game.MinionPartyPrefabDefinition minionPartyPrefab in definition2.MinionPartyPrefabs)
				{
					if (minionPartyPrefab.EventType == text)
					{
						result = minionPartyPrefab.Prefab;
						break;
					}
				}
			}
			return result;
		}
	}
}
