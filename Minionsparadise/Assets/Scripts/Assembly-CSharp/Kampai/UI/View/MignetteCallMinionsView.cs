namespace Kampai.UI.View
{
	public class MignetteCallMinionsView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.ButtonView leftArrow;

		public global::Kampai.UI.View.ButtonView rightArrow;

		public global::UnityEngine.UI.Text modalName;

		public global::UnityEngine.UI.Text minionsNeeded;

		public global::UnityEngine.UI.Text mignetteDescription;

		public global::Kampai.UI.View.ButtonView callMinionsButton;

		public global::UnityEngine.UI.Text rewardsString;

		public global::UnityEngine.UI.Text availableString;

		public global::UnityEngine.UI.Text minionsAvailable;

		public global::Kampai.UI.View.ProgressBarModal modal;

		public global::UnityEngine.GameObject CallMinionGroup;

		public global::UnityEngine.GameObject RushCooldownGroup;

		public global::UnityEngine.UI.Text XPReward;

		public global::System.Collections.Generic.List<global::Kampai.UI.View.MignetteRuleViewObject> mignetteRulesList;

		private global::Kampai.Game.MignetteBuilding mignetteBuilding;

		private global::Kampai.Main.ILocalizationService localizationService;

		private global::Kampai.Game.IPlayerService playerService;

		private global::UnityEngine.GameObject minionManager;

		private int startTime;

		private int endTime;

		private global::UnityEngine.Vector2 fillPosition;

		private int minionSlots;

		internal void Init(global::Kampai.Game.MignetteBuilding building, global::Kampai.Main.ILocalizationService localizationService, global::Kampai.Game.IPlayerService playerService, global::UnityEngine.GameObject minionManager, global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData)
		{
			InitProgrammatic(buildingPopupPositionData);
			this.localizationService = localizationService;
			this.playerService = playerService;
			this.minionManager = minionManager;
			RecreateModal(building);
			base.Open();
		}

		internal void UpdateTime(int timeRemaining)
		{
			int num = endTime - startTime;
			float num2 = 1f - (float)timeRemaining / (float)num;
			fillPosition.x = num2;
			modal.fillImage.rectTransform.anchorMax = fillPosition;
			modal.percentageText.text = string.Format("{0}%", (int)(num2 * 100f));
			SetTimeRemainingText(timeRemaining);
		}

		private void OrganizeRulesPanel()
		{
			global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = mignetteBuilding.MignetteBuildingDefinition;
			int count = mignetteRulesList.Count;
			int count2 = mignetteBuildingDefinition.MignetteRules.Count;
			for (int i = 0; i < count; i++)
			{
				if (i < count2)
				{
					mignetteRulesList[i].gameObject.SetActive(true);
					mignetteRulesList[i].RenderRule(mignetteBuildingDefinition.MignetteRules[i]);
					mignetteRulesList[i].AmountLabel.text += localizationService.GetString("MignettePoints");
				}
				else
				{
					mignetteRulesList[i].gameObject.SetActive(false);
				}
			}
		}

		internal void SetArrowButtonsState(bool enable)
		{
			leftArrow.GetComponent<global::UnityEngine.UI.Button>().interactable = enable;
			rightArrow.GetComponent<global::UnityEngine.UI.Button>().interactable = enable;
		}

		internal void SetArrowButtonsVisibleAndActive(bool active)
		{
			leftArrow.gameObject.SetActive(active);
			rightArrow.gameObject.SetActive(active);
		}

		private void SetUpView()
		{
			global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = mignetteBuilding.MignetteBuildingDefinition;
			modalName.text = localizationService.GetString(mignetteBuildingDefinition.LocalizedKey);
			mignetteDescription.text = localizationService.GetString(mignetteBuildingDefinition.Description);
			XPReward.text = mignetteBuildingDefinition.XPRewardFactor.ToString();
			bool flag = mignetteBuilding.State == global::Kampai.Game.BuildingState.Cooldown;
			CallMinionGroup.SetActive(!flag);
			RushCooldownGroup.SetActive(flag);
			if (!playerService.HasPurchasedMinigamePack())
			{
				minionSlots = mignetteBuilding.GetMinionSlotsOwned();
				minionsNeeded.text = minionSlots.ToString();
				availableString.text = localizationService.GetString("MignetteMinionsAvailable");
			}
			UpdateView();
			rewardsString.text = localizationService.GetString("MignetteRewards");
			OrganizeRulesPanel();
		}

		internal void RecreateModal(global::Kampai.Game.MignetteBuilding building)
		{
			mignetteBuilding = building;
			SetUpView();
		}

		internal void StartTime(int startTime, int endTime)
		{
			SetTimeRemainingText(endTime - startTime);
			this.startTime = startTime;
			this.endTime = endTime;
			fillPosition = modal.fillImage.rectTransform.anchorMax;
		}

		internal void SetTimeRemainingText(int time)
		{
			int num = time / 3600;
			int num2 = time / 60 % 60;
			int num3 = time % 60;
			modal.timeRemainingText.text = string.Format("{0}:{1}:{2}", num.ToString("00"), num2.ToString("00"), num3.ToString("00"));
		}

		internal void SetRushCost(int rushCost)
		{
			modal.rushText.text = string.Format("{0}", rushCost);
		}

		internal void UpdateView()
		{
			if (minionManager == null)
			{
				return;
			}
			bool flag = playerService.HasStorageBuilding();
			if (!playerService.HasPurchasedMinigamePack())
			{
				int idleMinionCount = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerMediator>().GetIdleMinionCount();
				minionsAvailable.text = idleMinionCount.ToString();
				flag &= idleMinionCount >= minionSlots;
			}
			callMinionsButton.GetComponent<global::UnityEngine.Animator>().enabled = true;
			ScrollableButtonView component = callMinionsButton.GetComponent<ScrollableButtonView>();
			if (!flag)
			{
				component.Disable();
			}
			else
			{
				component.ResetAnim();
				if (playerService.GetHighestFtueCompleted() < 999999)
				{
					StartCoroutine(StartCallButtonPulse());
				}
			}
			callMinionsButton.GetComponent<global::UnityEngine.UI.Button>().interactable = flag;
		}

		private global::System.Collections.IEnumerator StartCallButtonPulse()
		{
			yield return null;
			callMinionsButton.GetComponent<global::UnityEngine.Animator>().enabled = false;
			global::UnityEngine.Vector3 dummyVector = global::UnityEngine.Vector3.one;
			global::Kampai.Util.TweenUtil.Throb(callMinionsButton.transform, 0.85f, 0.5f, out dummyVector);
		}
	}
}
