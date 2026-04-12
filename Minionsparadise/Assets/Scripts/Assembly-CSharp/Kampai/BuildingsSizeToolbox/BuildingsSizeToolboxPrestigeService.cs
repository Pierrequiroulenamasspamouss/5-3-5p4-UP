namespace Kampai.BuildingsSizeToolbox
{
	internal sealed class BuildingsSizeToolboxPrestigeService : global::Kampai.Game.IPrestigeService
	{
		public void Initialize()
		{
			throw new global::System.NotImplementedException();
		}

		public void AddPrestige(global::Kampai.Game.Prestige prestige)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Prestige CreatePrestige(int prestigeDefinitionId)
		{
			throw new global::System.NotImplementedException();
		}

		public void RemovePrestige(global::Kampai.Game.Prestige prestige)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetIdlePrestigeDuration(int prestigeDefinitionId)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Prestige GetPrestige(int prestigeDefinitionId, bool logIfNonexistant = true)
		{
			return null;
		}

		public global::Kampai.Game.Prestige GetPrestigeFromMinionInstance(global::Kampai.Game.Character minionCharacter)
		{
			throw new global::System.NotImplementedException();
		}

		public void ChangeToPrestigeState(global::Kampai.Game.Prestige prestige, global::Kampai.Game.PrestigeState targetState, int targetPrestigeLevel = 0, bool triggerNewQuest = true)
		{
			throw new global::System.NotImplementedException();
		}

		public void GetCharacterImageBasedOnMood(int prestigeDefinitionId, global::Kampai.Game.CharacterImageType type, out global::UnityEngine.Sprite characterImage, out global::UnityEngine.Sprite characterMask)
		{
			throw new global::System.NotImplementedException();
		}

		public void GetCharacterImageBasedOnMood(global::Kampai.Game.PrestigeDefinition prestigeDefinition, global::Kampai.Game.CharacterImageType type, out global::UnityEngine.Sprite characterImage, out global::UnityEngine.Sprite characterMask)
		{
			throw new global::System.NotImplementedException();
		}

		public void GetCharacterImagePathBasedOnMood(int prestigeDefinitionId, global::Kampai.Game.CharacterImageType type, out string characterImagePath, out string characterMaskPath)
		{
			throw new global::System.NotImplementedException();
		}

		public void GetCharacterImagePathBasedOnMood(global::Kampai.Game.PrestigeDefinition prestigeDefinition, global::Kampai.Game.CharacterImageType type, out string characterImagePath, out string characterMaskPath)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.QuestResourceDefinition DetermineQuestResourceDefinition(int prestigeDefinitionId, global::Kampai.Game.CharacterImageType type)
		{
			throw new global::System.NotImplementedException();
		}

		public void PostOrderCompletion(global::Kampai.Game.Prestige prestige)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Prestige> GetBuddyPrestiges()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.Dictionary<int, bool> GetPrestigedCharacterStates(bool includeBob = true)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetPrestigeUnlockedPrestigeLevel(global::Kampai.Game.PrestigeDefinition prestigeDefinition)
		{
			throw new global::System.NotImplementedException();
		}

		public void UpdateEligiblePrestigeList()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.Prestige> GetAllUnlockedPrestiges()
		{
			throw new global::System.NotImplementedException();
		}

		public void AddMinionToTikiBarSlot(global::Kampai.Game.Character targetMinion, int slotIndex, global::Kampai.Game.TikiBarBuilding tikiBar, bool enablePathing = false)
		{
			throw new global::System.NotImplementedException();
		}

		public int ResolveTrackedId(int questTrackedInstanceId)
		{
			throw new global::System.NotImplementedException();
		}

		public bool IsTikiBarFull()
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.CabanaBuilding GetEmptyCabana()
		{
			throw new global::System.NotImplementedException();
		}

		public void CheckCompletedPrestiges()
		{
			throw new global::System.NotImplementedException();
		}

		public bool IsPrestigeFulfilled(global::Kampai.Game.Prestige prestige)
		{
			throw new global::System.NotImplementedException();
		}
	}
}
