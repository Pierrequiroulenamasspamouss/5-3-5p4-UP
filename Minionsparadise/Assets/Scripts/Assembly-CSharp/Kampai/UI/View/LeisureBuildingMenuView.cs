namespace Kampai.UI.View
{
	public class LeisureBuildingMenuView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text Title;

		public global::UnityEngine.UI.Text Production;

		public global::UnityEngine.UI.Text ClockTime;

		public global::UnityEngine.UI.Text MinionsNeeded;

		public global::UnityEngine.UI.Text RushCost;

		public global::UnityEngine.UI.Text IdleMinionCount;

		public global::UnityEngine.UI.Text MinionLevelText;

		public global::UnityEngine.GameObject PartyBuffPanel;

		public global::UnityEngine.UI.Text PartyBuffAmt;

		public global::Kampai.UI.View.KampaiImage PartyPointsIcon;

		public global::Kampai.UI.View.KampaiImage LevelArrow;

		public global::Kampai.UI.View.ButtonView PrevBuilding;

		public global::Kampai.UI.View.ButtonView NextBuilding;

		public ScrollableButtonView CallMinions;

		public global::Kampai.UI.View.ButtonView CollectPoints;

		public ScrollableButtonView RushMinions;

		public global::UnityEngine.GameObject ClockPanel;

		public global::UnityEngine.GameObject ClockIcon;

		public global::UnityEngine.GameObject PartyIcon;

		public global::UnityEngine.Transform PartyIconTransform;

		public global::UnityEngine.GameObject AvailableMinionsPanel;

		public global::UnityEngine.GameObject CollectPanel;

		internal int rushCost;

		private int minionsNeeded;

		private int timeRemaining;

		private global::UnityEngine.Vector3 originalScale;

		private global::Kampai.Main.ILocalizationService localService;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.ITimeEventService timeEventService;

		private global::UnityEngine.GameObject minionManager;

		private global::Kampai.Game.MinionParty minionParty;

		public int TimeRemaining
		{
			get
			{
				return timeRemaining;
			}
		}

		internal void Init(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.ITimeEventService timeEventService, global::UnityEngine.GameObject minionManager, global::Kampai.UI.BuildingPopupPositionData positionData)
		{
			InitProgrammatic(positionData);
			this.localService = localService;
			this.definitionService = definitionService;
			this.playerService = playerService;
			this.timeEventService = timeEventService;
			this.minionManager = minionManager;
			minionParty = playerService.GetMinionPartyInstance();
			base.Open();
		}

		internal void SetTitle(string titleKey)
		{
			Title.text = localService.GetString(titleKey);
		}

		internal void SetProduction(string productionKey, int partyPoints, string time)
		{
			Production.text = localService.GetString(productionKey, partyPoints, time);
		}

		internal void SetClockTIme(global::Kampai.Game.LeisureBuilding building)
		{
			int leisureTimeDuration = building.Definition.LeisureTimeDuration;
			if (timeEventService.HasEventID(building.ID))
			{
				timeRemaining = timeEventService.GetTimeRemaining(building.ID);
			}
			else
			{
				timeRemaining = leisureTimeDuration;
			}
			timeRemaining = global::UnityEngine.Mathf.Min(timeRemaining, leisureTimeDuration);
			ClockTime.text = UIUtils.FormatTime(timeRemaining, localService);
		}

		internal void SetRushCost(global::Kampai.Game.LeisureBuilding building)
		{
			if (building != null)
			{
				global::Kampai.Game.RushTimeBandDefinition rushTimeBandForTime = definitionService.GetRushTimeBandForTime(timeRemaining);
				rushCost = rushTimeBandForTime.GetCostForRushActionType(global::Kampai.Game.RushActionType.LEISURE);
				RushCost.text = rushCost.ToString();
			}
		}

		internal void SetPartyInfo(float boost, string boostString, bool isOn = true)
		{
			PartyBuffAmt.text = boostString;
			bool flag = isOn && (int)(boost * 100f) != 100;
			PartyBuffPanel.SetActive(flag);
			ClockIcon.SetActive(!flag);
			PartyIcon.SetActive(flag);
			if (flag)
			{
				global::UnityEngine.Vector3 vector;
				global::Kampai.Util.TweenUtil.Throb(PartyIconTransform, 1.1f, 0.2f, out vector);
				UIUtils.FlashingColor(ClockTime, 0);
				return;
			}
			Go.killAllTweensWithTarget(PartyIconTransform);
			Go.killAllTweensWithTarget(ClockTime);
			ClockTime.color = global::UnityEngine.Color.white;
			PartyIconTransform.localScale = global::UnityEngine.Vector3.one;
		}

		internal void SetMinionsNeeded(int minionsNeeded)
		{
			this.minionsNeeded = minionsNeeded;
			MinionsNeeded.text = localService.GetString("RequiresXMinions*", minionsNeeded);
		}

		public bool IsCallButtonEnabled()
		{
			if (playerService.HasStorageBuilding())
			{
				return true;
			}
			return playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) == 0 && minionParty.CurrentPartyPoints == 0;
		}

		public void DisableRushButton()
		{
			RushMinions.Disable();
			RushMinions.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
		}

		internal void SetIdleMinionCount()
		{
			if (minionManager == null)
			{
				return;
			}
			global::Kampai.Game.View.MinionManagerMediator component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerMediator>();
			if (!(component == null))
			{
				int idleMinionCount = component.GetIdleMinionCount();
				IdleMinionCount.text = idleMinionCount.ToString();
				bool flag = idleMinionCount >= minionsNeeded && IsCallButtonEnabled();
				if (!flag)
				{
					CallMinions.Disable();
				}
				else
				{
					CallMinions.ResetAnim();
				}
				CallMinions.GetComponent<global::UnityEngine.UI.Button>().interactable = flag;
			}
		}

		internal void EnablePartyPoints(bool isEnabled)
		{
			Production.gameObject.SetActive(true);
			PartyPointsIcon.gameObject.SetActive(isEnabled);
		}

		internal void EnableCallMinion()
		{
			CallMinions.gameObject.SetActive(true);
			AvailableMinionsPanel.SetActive(true);
			RushMinions.gameObject.SetActive(false);
			ClockPanel.gameObject.SetActive(false);
			CollectPoints.gameObject.SetActive(false);
			CollectPanel.gameObject.SetActive(false);
		}

		internal void SetCallButtonInfo(int highestLevel)
		{
			LevelArrow.gameObject.SetActive(highestLevel != 0);
			MinionLevelText.text = (highestLevel + 1).ToString();
		}

		internal void EnableRush()
		{
			RushMinions.EnableDoubleConfirm();
			RushMinions.ResetAnim();
			RushMinions.gameObject.SetActive(true);
			ClockPanel.gameObject.SetActive(true);
			CallMinions.gameObject.SetActive(false);
			AvailableMinionsPanel.SetActive(false);
			CollectPoints.gameObject.SetActive(false);
			CollectPanel.gameObject.SetActive(false);
		}

		internal void EnableCollect()
		{
			CollectPoints.gameObject.SetActive(true);
			CollectPanel.gameObject.SetActive(true);
			RushMinions.gameObject.SetActive(false);
			ClockPanel.gameObject.SetActive(false);
			CallMinions.gameObject.SetActive(false);
			AvailableMinionsPanel.SetActive(false);
		}

		internal void SetArrowsInteractable(bool isInteractable)
		{
			PrevBuilding.GetComponent<global::UnityEngine.UI.Button>().interactable = isInteractable;
			NextBuilding.GetComponent<global::UnityEngine.UI.Button>().interactable = isInteractable;
		}

		internal void SetArrowsActive(bool isActive)
		{
			PrevBuilding.gameObject.SetActive(isActive);
			NextBuilding.gameObject.SetActive(isActive);
		}

		internal void Throb(ScrollableButtonView button, bool throb)
		{
			if (!button.enabled || minionManager == null)
			{
				return;
			}
			int idleMinionCount = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerMediator>().GetIdleMinionCount();
			if (idleMinionCount < minionsNeeded)
			{
				return;
			}
			global::UnityEngine.Animator[] componentsInChildren = button.GetComponentsInChildren<global::UnityEngine.Animator>();
			if (throb)
			{
				global::UnityEngine.Animator[] array = componentsInChildren;
				foreach (global::UnityEngine.Animator animator in array)
				{
					animator.enabled = false;
				}
				global::Kampai.Util.TweenUtil.Throb(button.transform, 0.85f, 0.5f, out originalScale);
			}
			else if (originalScale != global::UnityEngine.Vector3.zero)
			{
				Go.killAllTweensWithTarget(button.transform);
				button.transform.localScale = originalScale;
				global::UnityEngine.Animator[] array2 = componentsInChildren;
				foreach (global::UnityEngine.Animator animator2 in array2)
				{
					animator2.enabled = true;
				}
			}
		}

		internal void Cleanup()
		{
			Go.killAllTweensWithTarget(PartyIconTransform);
			Go.killAllTweensWithTarget(ClockTime.transform);
			Go.killAllTweensWithTarget(CallMinions.transform);
			Go.killAllTweensWithTarget(RushMinions.transform);
		}
	}
}
