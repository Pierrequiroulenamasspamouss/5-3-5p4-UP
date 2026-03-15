namespace Kampai.UI.View
{
	public class QuestRewardView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.KampaiImage questGiverIcon;

		public global::Kampai.UI.View.MinionSlotModal MinionSlot;

		public global::System.Collections.Generic.List<global::UnityEngine.Animator> objectiveAnimators = new global::System.Collections.Generic.List<global::UnityEngine.Animator>();

		public global::System.Collections.Generic.List<global::UnityEngine.UI.Text> objectiveTexts = new global::System.Collections.Generic.List<global::UnityEngine.UI.Text>();

		public global::UnityEngine.UI.Text rewardDescription;

		public global::Kampai.UI.View.ButtonView collectButton;

		public global::Kampai.UI.View.ButtonView adVideoButton;

		public global::UnityEngine.RectTransform scrollViewTransform;

		public global::UnityEngine.GameObject collectButtonPanel;

		public float padding = 10f;

		public float animataionDelay = 1f;

		internal global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Main.ILocalizationService localizedService;

		private global::UnityEngine.Vector3 originalScale;

		private global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject;

		internal global::UnityEngine.Coroutine rewardDisplay;

		internal global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition;

		internal global::System.Collections.Generic.List<global::UnityEngine.GameObject> viewList = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		internal void Init(global::Kampai.Game.Quest quest, global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService, global::Kampai.Game.IPlayerService playerService, global::Kampai.UI.View.ModalSettings modalSettings, global::Kampai.UI.IFancyUIService fancyUIService, global::Kampai.Game.IQuestService questService)
		{
			base.Init();
			definitionService = defService;
			localizedService = localService;
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.offsetMin = global::UnityEngine.Vector2.zero;
			rectTransform.offsetMax = global::UnityEngine.Vector2.zero;
			SetupCharImage(quest, fancyUIService);
			global::Kampai.Game.IQuestController questController = questService.GetQuestControllerByDefinitionID(quest.GetActiveDefinition().ID) ?? questService.GetQuestControllerByInstanceID(quest.ID);
			SetupAndAnimateObjectives(questController);
			SetQuestDescription(quest, questService);
			transactionDefinition = quest.GetActiveDefinition().GetReward(definitionService);
			if (transactionDefinition != null)
			{
				SetupScrollView(quest, RewardUtil.GetRewardQuantityFromTransaction(transactionDefinition, definitionService, playerService));
			}
			originalScale = collectButton.transform.localScale;
			base.Open();
			if (modalSettings.enableCollectThrob)
			{
				HighlightCollect(true);
			}
		}

		public void EnableAdButton(bool adEnabled)
		{
			adVideoButton.gameObject.SetActive(adEnabled);
		}

		internal void CloseView()
		{
			if (dummyCharacterObject != null)
			{
				dummyCharacterObject.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(dummyCharacterObject.gameObject);
			}
			base.Close();
		}

		private void SetQuestDescription(global::Kampai.Game.Quest quest, global::Kampai.Game.IQuestService questService)
		{
			int questLineID = quest.GetActiveDefinition().QuestLineID;
			int completeCount = 0;
			if (questService.GetQuestLines().ContainsKey(questLineID))
			{
				CheckQuestProgress(questService.GetQuestLines()[questLineID], quest.GetActiveDefinition().ID, out completeCount);
			}
			string format = localizedService.GetString("questRewardDescription");
			rewardDescription.text = string.Format(format, questService.GetQuestName(quest.GetActiveDefinition().LocalizedKey), completeCount);
		}

		private void CheckQuestProgress(global::Kampai.Game.QuestLine questLine, int questDefinitionID, out int completeCount)
		{
			completeCount = 0;
			for (int num = questLine.Quests.Count - 1; num >= 0; num--)
			{
				global::Kampai.Game.QuestDefinition questDefinition = questLine.Quests[num];
				if (questDefinition.ID == questDefinitionID)
				{
					completeCount++;
					break;
				}
				if (questDefinition.QuestVersion != -1 && questDefinition.SurfaceType != global::Kampai.Game.QuestSurfaceType.Automatic)
				{
					completeCount++;
				}
			}
		}

		private void SetupAndAnimateObjectives(global::Kampai.Game.IQuestController questController)
		{
			int stepCount = questController.StepCount;
			for (int i = 0; i < stepCount; i++)
			{
				global::Kampai.Game.IQuestStepController stepController = questController.GetStepController(i);
				objectiveTexts[i].text = stepController.GetStepAction(localizedService) + " " + stepController.GetStepDescription(localizedService, definitionService);
			}
			rewardDisplay = StartCoroutine(DisplayRewards(stepCount));
		}

		private global::System.Collections.IEnumerator DisplayRewards(int count)
		{
			yield return null;
			if (dummyCharacterObject != null)
			{
				dummyCharacterObject.gameObject.SetActive(true);
				dummyCharacterObject.transform.localPosition = global::UnityEngine.Vector3.zero;
			}
			yield return new global::UnityEngine.WaitForSeconds(animataionDelay);
			for (int i = 0; i < count; i++)
			{
				soundFXSignal.Dispatch("Play_quest_checkMark_01");
				objectiveAnimators[i].Play("Open");
				yield return new global::UnityEngine.WaitForSeconds(animataionDelay);
			}
		}

		public void SetupVillainAtlas()
		{
			questGiverIcon.maskSprite = UIUtils.LoadSpriteFromPath("icn_nav_villians_mask");
		}

		private void SetupCharImage(global::Kampai.Game.Quest quest, global::Kampai.UI.IFancyUIService fancyUIService)
		{
			int surfaceID = quest.GetActiveDefinition().SurfaceID;
			global::Kampai.UI.DummyCharacterType characterType = fancyUIService.GetCharacterType(surfaceID);
			dummyCharacterObject = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.SelectedHappy, MinionSlot.transform, MinionSlot.VillainScale, MinionSlot.VillainPositionOffset, surfaceID);
			dummyCharacterObject.gameObject.SetActive(false);
		}

		internal void SetupScrollView(global::Kampai.Game.Quest quest, global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity> quantityChange)
		{
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load("cmp_QuestRewardSlider") as global::UnityEngine.GameObject;
			float x = (gameObject.transform as global::UnityEngine.RectTransform).sizeDelta.x;
			int count = quantityChange.Count;
			int rewardDisplayCount = quest.GetActiveDefinition().RewardDisplayCount;
			count = ((count <= rewardDisplayCount) ? count : rewardDisplayCount);
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
				gameObject2.transform.SetParent(scrollViewTransform, false);
				global::UnityEngine.RectTransform rectTransform = gameObject2.transform as global::UnityEngine.RectTransform;
				rectTransform.localPosition = global::UnityEngine.Vector3.zero;
				rectTransform.localScale = global::UnityEngine.Vector3.one;
				rectTransform.offsetMin = new global::UnityEngine.Vector2(x * (float)i + padding * (float)i, 0f);
				rectTransform.offsetMax = new global::UnityEngine.Vector2(x * (float)(i + 1) + padding * (float)i, 0f);
				global::Kampai.UI.View.QuestRewardSliderView component = gameObject2.GetComponent<global::Kampai.UI.View.QuestRewardSliderView>();
				global::Kampai.Game.UnlockDefinition definition;
				global::Kampai.Game.DisplayableDefinition displayableDefinition = ((quantityChange[i].ID == 0 || quantityChange[i].ID == 1 || quantityChange[i].ID == 5 || !definitionService.TryGet<global::Kampai.Game.UnlockDefinition>(quantityChange[i].ID, out definition)) ? definitionService.Get<global::Kampai.Game.DisplayableDefinition>(quantityChange[i].ID) : definitionService.Get<global::Kampai.Game.DisplayableDefinition>(definition.ReferencedDefinitionID));
				component.description.text = localizedService.GetString(displayableDefinition.LocalizedKey);
				component.icon.sprite = UIUtils.LoadSpriteFromPath(displayableDefinition.Image);
				component.icon.maskSprite = UIUtils.LoadSpriteFromPath(displayableDefinition.Mask);
				component.itemQuantity.text = UIUtils.FormatLargeNumber(quantityChange[i].Quantity);
				viewList.Add(component.icon.gameObject);
			}
			int num = 3 * (int)x;
			int num2 = count * ((int)x + (int)padding);
			num2 -= (int)padding;
			int num3 = 0;
			if (num2 > num)
			{
				num3 = (num2 - num) / 2;
			}
			scrollViewTransform.sizeDelta = new global::UnityEngine.Vector2(num2, 0f);
			scrollViewTransform.localPosition = new global::UnityEngine.Vector2((float)num3 / 2f, scrollViewTransform.localPosition.y);
		}

		internal void HighlightCollect(bool isHighlighted)
		{
			global::UnityEngine.Animator[] componentsInChildren = collectButton.GetComponentsInChildren<global::UnityEngine.Animator>();
			if (isHighlighted)
			{
				global::UnityEngine.Animator[] array = componentsInChildren;
				foreach (global::UnityEngine.Animator animator in array)
				{
					animator.enabled = false;
				}
				global::Kampai.Util.TweenUtil.Throb(collectButton.transform, 0.85f, 0.5f, out originalScale);
				return;
			}
			Go.killAllTweensWithTarget(collectButton.transform);
			collectButton.transform.localScale = originalScale;
			global::UnityEngine.Animator[] array2 = componentsInChildren;
			foreach (global::UnityEngine.Animator animator2 in array2)
			{
				animator2.enabled = true;
			}
		}
	}
}
