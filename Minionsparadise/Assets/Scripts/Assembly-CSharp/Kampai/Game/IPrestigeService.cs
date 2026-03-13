namespace Kampai.Game
{
	public interface IPrestigeService
	{
		void Initialize();

		void AddPrestige(global::Kampai.Game.Prestige prestige);

		global::Kampai.Game.Prestige CreatePrestige(int prestigeDefinitionId);

		void RemovePrestige(global::Kampai.Game.Prestige prestige);

		int GetIdlePrestigeDuration(int prestigeDefinitionId);

		global::Kampai.Game.Prestige GetPrestige(int prestigeDefinitionId, bool logIfNonexistant = true);

		global::Kampai.Game.Prestige GetPrestigeFromMinionInstance(global::Kampai.Game.Character minionCharacter);

		void ChangeToPrestigeState(global::Kampai.Game.Prestige prestige, global::Kampai.Game.PrestigeState targetState, int targetPrestigeLevel = 0, bool triggerNewQuest = true);

		void GetCharacterImageBasedOnMood(int prestigeDefinitionId, global::Kampai.Game.CharacterImageType type, out global::UnityEngine.Sprite characterImage, out global::UnityEngine.Sprite characterMask);

		void GetCharacterImageBasedOnMood(global::Kampai.Game.PrestigeDefinition prestigeDefinition, global::Kampai.Game.CharacterImageType type, out global::UnityEngine.Sprite characterImage, out global::UnityEngine.Sprite characterMask);

		void GetCharacterImagePathBasedOnMood(int prestigeDefinitionId, global::Kampai.Game.CharacterImageType type, out string characterImagePath, out string characterMaskPath);

		void GetCharacterImagePathBasedOnMood(global::Kampai.Game.PrestigeDefinition prestigeDefinition, global::Kampai.Game.CharacterImageType type, out string characterImagePath, out string characterMaskPath);

		global::Kampai.Game.QuestResourceDefinition DetermineQuestResourceDefinition(int prestigeDefinitionId, global::Kampai.Game.CharacterImageType type);

		void PostOrderCompletion(global::Kampai.Game.Prestige prestige);

		global::System.Collections.Generic.IList<global::Kampai.Game.Prestige> GetBuddyPrestiges();

		global::System.Collections.Generic.Dictionary<int, bool> GetPrestigedCharacterStates(bool includeBob = true);

		int GetPrestigeUnlockedPrestigeLevel(global::Kampai.Game.PrestigeDefinition prestigeDefinition);

		void UpdateEligiblePrestigeList();

		global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.Prestige> GetAllUnlockedPrestiges();

		void AddMinionToTikiBarSlot(global::Kampai.Game.Character targetMinion, int slotIndex, global::Kampai.Game.TikiBarBuilding tikiBar, bool enablePathing = false);

		int ResolveTrackedId(int questTrackedInstanceId);

		bool IsTikiBarFull();

		global::Kampai.Game.CabanaBuilding GetEmptyCabana();

		void CheckCompletedPrestiges();

		bool IsPrestigeFulfilled(global::Kampai.Game.Prestige prestige);
	}
}
