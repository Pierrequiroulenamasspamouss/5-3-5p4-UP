namespace Kampai.Game
{
	public class GuestOfHonorService : global::Kampai.Game.IGuestOfHonorService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GuestOfHonorService") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.GuestOfHonorDefinition lastGuestOfHonor;

		private global::Kampai.Game.GuestOfHonorDefinition currentGuestOfHonor;

		private int currentBuffDuration;

		private bool haveUnlockedAPrestige;

		private bool firstMasterPlanComplete;

		private global::System.Collections.Generic.List<global::Kampai.Game.SpecialEventItemDefinition> specialEvents;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT, optional = true)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPartyService partyService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.EndPartyBuffTimerSignal endPartyBuffTimerSignal { get; set; }

		public global::Kampai.Game.GuestOfHonorDefinition CurrentGuestOfHonor
		{
			get
			{
				return currentGuestOfHonor;
			}
		}

		public global::System.Collections.Generic.Dictionary<int, bool> GetAllGOHStates()
		{
			global::System.Collections.Generic.Dictionary<int, bool> dictionary = new global::System.Collections.Generic.Dictionary<int, bool>();
			bool flag = false;
			global::System.Collections.Generic.IList<global::Kampai.Game.PrestigeDefinition> all = definitionService.GetAll<global::Kampai.Game.PrestigeDefinition>();
			foreach (global::Kampai.Game.PrestigeDefinition item in all)
			{
				int iD = item.ID;
				if (item.GuestOfHonorDefinitionID == 0 || item.GuestOfHonorDisplayableType == global::Kampai.Game.GOHDisplayableType.Never)
				{
					continue;
				}
				global::Kampai.Game.Prestige firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(iD);
				bool flag2 = CharacterShouldBeDisplayed(item, firstInstanceByDefinitionId);
				if (firstInstanceByDefinitionId == null)
				{
					if (flag2)
					{
						dictionary.Add(iD, flag);
					}
				}
				else if (flag2)
				{
					if (firstInstanceByDefinitionId.CurrentPrestigeLevel > 0 || firstInstanceByDefinitionId.state == global::Kampai.Game.PrestigeState.Questing || firstInstanceByDefinitionId.state == global::Kampai.Game.PrestigeState.Taskable || firstInstanceByDefinitionId.state == global::Kampai.Game.PrestigeState.TaskableWhileQuesting)
					{
						dictionary.Add(iD, !flag);
					}
					else
					{
						dictionary.Add(iD, flag);
					}
				}
			}
			return dictionary;
		}

		private bool CharacterShouldBeDisplayed(global::Kampai.Game.PrestigeDefinition def, global::Kampai.Game.Prestige prestige)
		{
			if (specialEvents == null)
			{
				specialEvents = definitionService.GetAll<global::Kampai.Game.SpecialEventItemDefinition>();
			}
			foreach (global::Kampai.Game.SpecialEventItemDefinition specialEvent in specialEvents)
			{
				if (specialEvent.PrestigeDefinitionID == def.ID)
				{
					global::Kampai.Game.SpecialEventItem firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.SpecialEventItem>(specialEvent.ID);
					return firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.HasEnded;
				}
			}
			if (def.GuestOfHonorDisplayableType != global::Kampai.Game.GOHDisplayableType.SpecialConditionOnly)
			{
				return true;
			}
			if (prestige == null)
			{
				return false;
			}
			if (prestige.state == global::Kampai.Game.PrestigeState.Locked)
			{
				return false;
			}
			if (firstMasterPlanComplete)
			{
				return true;
			}
			global::Kampai.Game.MasterPlanDefinition planDefinition = definitionService.Get<global::Kampai.Game.MasterPlanDefinition>(65000);
			if (masterPlanService.HasReceivedInitialRewardFromPlanDefinition(planDefinition))
			{
				firstMasterPlanComplete = true;
				return true;
			}
			return false;
		}

		public float GetCurrentBuffMultiplierForBuffType(global::Kampai.Game.BuffType buffType)
		{
			if (currentGuestOfHonor == null || !playerService.GetMinionPartyInstance().IsBuffHappening)
			{
				return 1f;
			}
			if (currentGuestOfHonor.buffDefinitionIDs == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.BS_BAD_GUEST_OF_HONOR_DEFINITION, currentGuestOfHonor.ID);
				return 1f;
			}
			bool flag = false;
			int index = 0;
			global::Kampai.Game.BuffDefinition buffDefinition = null;
			for (int i = 0; i < currentGuestOfHonor.buffDefinitionIDs.Count; i++)
			{
				buffDefinition = definitionService.Get<global::Kampai.Game.BuffDefinition>(currentGuestOfHonor.buffDefinitionIDs[i]);
				if (buffDefinition.buffType == buffType)
				{
					index = i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return 1f;
			}
			int value = currentGuestOfHonor.buffStarValues[index];
			value = global::UnityEngine.Mathf.Clamp(value, 0, buffDefinition.starMultiplierValue.Count);
			return buffDefinition.starMultiplierValue[value];
		}

		public int GetPartyCooldownForPrestige(int prestigeInstanceID)
		{
			global::Kampai.Game.Prestige byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Prestige>(prestigeInstanceID);
			if (!byInstanceId.onCooldown)
			{
				return 0;
			}
			global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(byInstanceId.Definition.ID);
			global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(prestigeDefinition.GuestOfHonorDefinitionID);
			return guestOfHonorDefinition.cooldown - byInstanceId.numPartiesThrown;
		}

		public int GetRemainingInvitesForPrestige(int prestigeInstanceID)
		{
			global::Kampai.Game.Prestige byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Prestige>(prestigeInstanceID);
			if (byInstanceId.onCooldown)
			{
				return 0;
			}
			global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(byInstanceId.Definition.ID);
			global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(prestigeDefinition.GuestOfHonorDefinitionID);
			return guestOfHonorDefinition.availableInvites - byInstanceId.numPartiesInvited;
		}

		public int GetCurrentBuffDuration()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			global::Kampai.Game.PartyMeterTierDefinition partyMeterTierDefinition = minionPartyInstance.Definition.partyMeterDefinition.Tiers[minionPartyInstance.PartyStartTier];
			int duration = partyMeterTierDefinition.Duration;
			int num = 0;
			int num2 = 0;
			foreach (int lastGuestsOfHonorPrestigeID in minionPartyInstance.lastGuestsOfHonorPrestigeIDs)
			{
				if (lastGuestsOfHonorPrestigeID != 0)
				{
					global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(lastGuestsOfHonorPrestigeID);
					global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(prestigeDefinition.GuestOfHonorDefinitionID);
					num = duration + guestOfHonorDefinition.partyDurationBoost * 60;
					num2 += (int)((float)num * guestOfHonorDefinition.partyDurationMultipler - (float)duration);
				}
			}
			return RoundToMinute(duration + num2);
		}

		private int RoundToMinute(int seconds)
		{
			int num = seconds % 60;
			return (num >= 30) ? (seconds + 60 - num) : (seconds - num);
		}

		public void SelectGuestOfHonor(int prestigeDefinitionID)
		{
			if (prestigeDefinitionID == 0)
			{
				SelectGuestOfHonor(definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(8200));
				return;
			}
			global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(prestigeDefinitionID);
			SelectGuestOfHonor(prestigeDefinition);
		}

		public void SelectGuestOfHonor(int guest1PrestigeDefinitionID, int guest2PrestigeDefinitionID)
		{
			global::Kampai.Game.PrestigeDefinition guest1PrestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(guest1PrestigeDefinitionID);
			global::Kampai.Game.PrestigeDefinition guest2PrestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(guest2PrestigeDefinitionID);
			SelectGuestOfHonor(guest1PrestigeDefinition, guest2PrestigeDefinition);
		}

		public void SelectGuestOfHonor(global::Kampai.Game.PrestigeDefinition prestigeDefinition)
		{
			currentGuestOfHonor = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(prestigeDefinition.GuestOfHonorDefinitionID);
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			minionPartyInstance.lastGuestsOfHonorPrestigeIDs.Clear();
			minionPartyInstance.lastGuestsOfHonorPrestigeIDs.Add(prestigeDefinition.ID);
		}

		public void SelectGuestOfHonor(global::Kampai.Game.PrestigeDefinition guest1PrestigeDefinition, global::Kampai.Game.PrestigeDefinition guest2PrestigeDefinition)
		{
			currentGuestOfHonor = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(guest1PrestigeDefinition.GuestOfHonorDefinitionID);
			currentGuestOfHonor = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(guest2PrestigeDefinition.GuestOfHonorDefinitionID);
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			minionPartyInstance.lastGuestsOfHonorPrestigeIDs.Clear();
			minionPartyInstance.lastGuestsOfHonorPrestigeIDs.Add(guest1PrestigeDefinition.ID);
			minionPartyInstance.lastGuestsOfHonorPrestigeIDs.Add(guest2PrestigeDefinition.ID);
		}

		private void SelectGuestOfHonor(global::Kampai.Game.GuestOfHonorDefinition guestDefinition)
		{
			currentGuestOfHonor = guestDefinition;
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			minionPartyInstance.lastGuestsOfHonorPrestigeIDs.Clear();
			minionPartyInstance.lastGuestsOfHonorPrestigeIDs.Add(0);
		}

		public int GetBuffRemainingTime(global::Kampai.Game.MinionParty minionParty)
		{
			return minionParty.BuffStartTime + currentBuffDuration - timeService.CurrentTime();
		}

		public void UpdateAndStoreGuestOfHonorCooldowns()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			foreach (global::Kampai.Game.Prestige item in playerService.GetInstancesByType<global::Kampai.Game.Prestige>())
			{
				if (minionPartyInstance.lastGuestsOfHonorPrestigeIDs.Contains(item.Definition.ID))
				{
					global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(item.Definition.GuestOfHonorDefinitionID);
					if (guestOfHonorDefinition.cooldown > 0)
					{
						item.numPartiesInvited++;
						if (item.numPartiesInvited >= guestOfHonorDefinition.availableInvites)
						{
							item.onCooldown = true;
						}
					}
				}
				else if (item.onCooldown)
				{
					item.numPartiesThrown++;
					if (GetPartyCooldownForPrestige(item.ID) <= 0)
					{
						item.onCooldown = false;
						item.numPartiesThrown = 0;
						item.numPartiesInvited = 0;
					}
				}
			}
		}

		public int GetRushCostForPartyCoolDown(int prestigeInstanceID)
		{
			global::Kampai.Game.Prestige byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Prestige>(prestigeInstanceID);
			global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(byInstanceId.Definition.GuestOfHonorDefinitionID);
			return (guestOfHonorDefinition.cooldown - byInstanceId.numPartiesThrown) * guestOfHonorDefinition.rushCostPerParty;
		}

		public void StartBuff(int buffBaseDurationFromMinionParty)
		{
			if (currentGuestOfHonor == null)
			{
				logger.Error("You are trying to start a buff without a guest of honor selected!");
				return;
			}
			int count = currentGuestOfHonor.buffDefinitionIDs.Count;
			int num = buffBaseDurationFromMinionParty + currentGuestOfHonor.partyDurationBoost * 60;
			currentBuffDuration = RoundToMinute((int)((float)num * currentGuestOfHonor.partyDurationMultipler));
			int buffStartTime = playerService.GetMinionPartyInstance().BuffStartTime;
			lastGuestOfHonor = currentGuestOfHonor;
			timeEventService.AddEvent(80000, buffStartTime, currentBuffDuration, endPartyBuffTimerSignal);
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			for (int i = 0; i < count; i++)
			{
				int id = currentGuestOfHonor.buffDefinitionIDs[i];
				global::Kampai.Game.BuffDefinition buffDefinition = definitionService.Get<global::Kampai.Game.BuffDefinition>(id);
				int index = currentGuestOfHonor.buffStarValues[i];
				float type = buffDefinition.starMultiplierValue[index];
				switch (buffDefinition.buffType)
				{
				case global::Kampai.Game.BuffType.CURRENCY:
					injectionBinder.GetInstance<global::Kampai.Game.StartCurrencyBuffSignal>().Dispatch();
					break;
				case global::Kampai.Game.BuffType.PARTY:
					injectionBinder.GetInstance<global::Kampai.Game.StartPartyPointBuffSignal>().Dispatch(type, buffStartTime);
					break;
				case global::Kampai.Game.BuffType.PRODUCTION:
					injectionBinder.GetInstance<global::Kampai.Game.StartProductionBuffSignal>().Dispatch(type, buffStartTime);
					break;
				}
			}
		}

		public void StopBuff(int timePassedSinceBuffStarts, int lastBuffStartTime)
		{
			if (lastGuestOfHonor == null)
			{
				return;
			}
			int count = lastGuestOfHonor.buffDefinitionIDs.Count;
			for (int i = 0; i < count; i++)
			{
				int id = lastGuestOfHonor.buffDefinitionIDs[i];
				global::Kampai.Game.BuffDefinition buffDefinition = definitionService.Get<global::Kampai.Game.BuffDefinition>(id);
				int index = currentGuestOfHonor.buffStarValues[i];
				float third = buffDefinition.starMultiplierValue[index];
				global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
				switch (buffDefinition.buffType)
				{
				case global::Kampai.Game.BuffType.CURRENCY:
					injectionBinder.GetInstance<global::Kampai.Game.StopCurrencyBuffSignal>().Dispatch();
					break;
				case global::Kampai.Game.BuffType.PARTY:
					injectionBinder.GetInstance<global::Kampai.Game.StopPartyPointBuffSignal>().Dispatch(new global::Kampai.Util.Tuple<int, int, float>(lastBuffStartTime, timePassedSinceBuffStarts, third));
					break;
				case global::Kampai.Game.BuffType.PRODUCTION:
					injectionBinder.GetInstance<global::Kampai.Game.StopProductionBuffSignal>().Dispatch(new global::Kampai.Util.Tuple<int, int, float>(lastBuffStartTime, timePassedSinceBuffStarts, third));
					break;
				}
			}
			currentBuffDuration = 0;
			lastGuestOfHonor = null;
		}

		public bool PartyShouldProduceBuff()
		{
			if (haveUnlockedAPrestige)
			{
				return true;
			}
			global::Kampai.Game.StuartCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StuartCharacter>(70001);
			if (firstInstanceByDefinitionId != null)
			{
				haveUnlockedAPrestige = true;
			}
			return haveUnlockedAPrestige;
		}

		public float GetBuffMultiplierForPrestige(int prestigeDefinitionID)
		{
			global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(prestigeDefinitionID);
			global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(prestigeDefinition.GuestOfHonorDefinitionID);
			return GetBuffMultiplerFromGuestOfHonor(guestOfHonorDefinition);
		}

		public int GetBuffDurationForSingleGuestOfHonorOnNextLevel(global::Kampai.Game.GuestOfHonorDefinition gohDefinition)
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			int currentPartyIndex = minionPartyInstance.CurrentPartyIndex;
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			int currentLevel = ((!partyService.IsInspirationParty(quantity, currentPartyIndex)) ? quantity : (quantity + 1));
			int index = minionPartyInstance.DeterminePartyTier((uint)currentLevel);
			global::Kampai.Game.PartyMeterTierDefinition partyMeterTierDefinition = minionPartyInstance.Definition.partyMeterDefinition.Tiers[index];
			int duration = partyMeterTierDefinition.Duration;
			int num = gohDefinition.partyDurationBoost * 60;
			int seconds = (int)((float)duration * gohDefinition.partyDurationMultipler + (float)num);
			return RoundToMinute(seconds);
		}

		public float GetCurrentBuffMultipler()
		{
			return GetBuffMultiplerFromGuestOfHonor(lastGuestOfHonor);
		}

		public global::Kampai.Game.BuffDefinition GetRecentBuffDefinition(bool useCurrent = false)
		{
			global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition = lastGuestOfHonor;
			if (useCurrent)
			{
				guestOfHonorDefinition = currentGuestOfHonor;
			}
			if (guestOfHonorDefinition == null || guestOfHonorDefinition.buffDefinitionIDs == null || guestOfHonorDefinition.buffDefinitionIDs.Count == 0)
			{
				return null;
			}
			return definitionService.Get<global::Kampai.Game.BuffDefinition>(guestOfHonorDefinition.buffDefinitionIDs[0]);
		}

		private float GetBuffMultiplerFromGuestOfHonor(global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition)
		{
			if (guestOfHonorDefinition == null || guestOfHonorDefinition.buffDefinitionIDs == null || guestOfHonorDefinition.buffDefinitionIDs.Count == 0)
			{
				return 1f;
			}
			global::Kampai.Game.BuffDefinition buffDefinition = definitionService.Get<global::Kampai.Game.BuffDefinition>(guestOfHonorDefinition.buffDefinitionIDs[0]);
			int index = guestOfHonorDefinition.buffStarValues[0];
			return buffDefinition.starMultiplierValue[index];
		}

		public void RushPartyCooldownForPrestige(int prestigeInstanceID)
		{
			global::Kampai.Game.Prestige byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Prestige>(prestigeInstanceID);
			byInstanceId.onCooldown = false;
			byInstanceId.numPartiesInvited = 0;
			byInstanceId.numPartiesThrown = 0;
		}

		public bool ShouldDisplayUnlockAtLevelText(uint unlockLevel, uint prestigeDefID)
		{
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			int currentPartyIndex = minionPartyInstance.CurrentPartyIndex;
			int num = ((!partyService.IsInspirationParty(quantity, currentPartyIndex)) ? quantity : (quantity + 1));
			if (num < (int)unlockLevel)
			{
				return true;
			}
			if (prestigeDefID == 40004)
			{
				return true;
			}
			return false;
		}
	}
}
