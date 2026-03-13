namespace Kampai.Game
{
	public interface IGuestOfHonorService
	{
		global::Kampai.Game.GuestOfHonorDefinition CurrentGuestOfHonor { get; }

		global::System.Collections.Generic.Dictionary<int, bool> GetAllGOHStates();

		float GetCurrentBuffMultiplierForBuffType(global::Kampai.Game.BuffType buffType);

		int GetPartyCooldownForPrestige(int prestigeInstanceID);

		int GetRemainingInvitesForPrestige(int prestigeInstanceID);

		void UpdateAndStoreGuestOfHonorCooldowns();

		int GetRushCostForPartyCoolDown(int prestigeInstanceID);

		void SelectGuestOfHonor(global::Kampai.Game.PrestigeDefinition prestigeDefinition);

		void SelectGuestOfHonor(int prestigeDefinitionID);

		void SelectGuestOfHonor(global::Kampai.Game.PrestigeDefinition guest1PrestigeDefinition, global::Kampai.Game.PrestigeDefinition guest2PrestigeDefinition);

		void SelectGuestOfHonor(int guest1PrestigeDefinitionID, int guest2PrestigeDefinitionID);

		int GetBuffRemainingTime(global::Kampai.Game.MinionParty minionParty);

		void StartBuff(int buffBaseDurationFromMinionParty);

		void StopBuff(int timePassedSinceBuffStarts, int lastBuffStartTime);

		int GetCurrentBuffDuration();

		bool PartyShouldProduceBuff();

		float GetBuffMultiplierForPrestige(int prestigeDefinitionID);

		int GetBuffDurationForSingleGuestOfHonorOnNextLevel(global::Kampai.Game.GuestOfHonorDefinition gohDefinition);

		float GetCurrentBuffMultipler();

		global::Kampai.Game.BuffDefinition GetRecentBuffDefinition(bool useCurrent = false);

		void RushPartyCooldownForPrestige(int prestigeInstanceID);

		bool ShouldDisplayUnlockAtLevelText(uint unlockLevel, uint prestigeDefID);
	}
}
