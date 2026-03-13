namespace Kampai.UI.View
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
	public class MinionUpgradeView : global::Kampai.UI.View.PopupMenuView
	{
		private const string LevelupAnimationKey = "Levelup";

		private const string LevelupValueAnimationKey = "LevelupValue";

		private const string WaitAnimAbilitiesAnimationKey = "WaitAnimAbilities";

		public global::Kampai.UI.View.ButtonView Skrim;

		[global::UnityEngine.Header("Level Selection")]
		public global::UnityEngine.UI.ScrollRect MinionLevelsScrollView;

		public global::UnityEngine.GameObject levelSelectButtonOverlay;

		[global::UnityEngine.Header("Minion Slot")]
		public global::UnityEngine.Transform MinionSlot;

		public global::Kampai.UI.View.KampaiImage SilhouetteImage;

		public global::Kampai.UI.View.ButtonView LeftArrow;

		public global::Kampai.UI.View.ButtonView RightArrow;

		public global::UnityEngine.UI.Text CurrentMinionLevelDisplay;

		public global::UnityEngine.ParticleSystem LevelUpParticleSystem;

		[global::UnityEngine.Header("Abilities")]
		public global::UnityEngine.UI.ScrollRect BenefitScrollView;

		[global::UnityEngine.Header("Goals")]
		public global::UnityEngine.UI.ScrollRect PopulationScrollView;

		public global::Kampai.UI.View.LocalizeView GoalDescriptionText;

		[global::UnityEngine.Header("Levelup Token")]
		public global::UnityEngine.GameObject LevelUpButtonPanel;

		public global::Kampai.UI.View.DoubleConfirmButtonView RushButton;

		public global::Kampai.UI.View.DoubleConfirmButtonView UpgradeButton;

		public global::UnityEngine.UI.Text UpgradeTokenCount;

		public global::UnityEngine.UI.Text RequiredTokenCountToLevel;

		public global::UnityEngine.UI.Text TokenRushCost;

		[global::UnityEngine.Header("Controls")]
		public global::Kampai.UI.View.ButtonView CloseButton;

		internal int currentMinionLevelSelected;

		internal global::System.Collections.Generic.List<int> selectedMinionIndex;

		private readonly global::System.Collections.Generic.IList<global::Kampai.UI.View.MinionBenefitView> minionBenefitViews = new global::System.Collections.Generic.List<global::Kampai.UI.View.MinionBenefitView>();

		private readonly global::System.Collections.Generic.IList<global::Kampai.UI.View.MinionLevelSelectorView> minionLevelSelectorViews = new global::System.Collections.Generic.List<global::Kampai.UI.View.MinionLevelSelectorView>();

		private readonly global::System.Collections.Generic.IList<global::Kampai.UI.View.PopulationBenefitView> populationBenefitViews = new global::System.Collections.Generic.List<global::Kampai.UI.View.PopulationBenefitView>();

		private global::System.Collections.Generic.List<global::Kampai.Game.Minion> currentMinionList;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.View.DummyCharacterObject displayedMinion;

		private global::Kampai.UI.IFancyUIService fancyUIService;

		private global::Kampai.Game.MinionBenefitLevelBandDefintion levelBandDef;

		private global::Kampai.Main.ILocalizationService localizationService;

		private global::Kampai.Game.IPlayerService playerService;

		private global::System.Collections.Generic.List<global::Kampai.Game.PopulationBenefitDefinition> populationDefinitions;

		private global::Kampai.Main.PlayGlobalSoundFXSignal soundFxSignal;

		private bool isAnimating;

		private bool isWaiting;

		private float waitAnimTime;

		private global::UnityEngine.Coroutine waitForAnimAbilitiesFinishCoroutine;

		public global::Kampai.UI.View.RefreshAllOfTypeArgsSignal refreshAllOfTypeArgsSignal { get; set; }

		public global::Kampai.UI.View.RefreshFromIndexArgsSignal refreshFromIndexArgsSignal { get; set; }

		public int rushCost { get; set; }

		public int tokensToLevel { get; set; }

		public int GetCurrentMinionDefinitionID()
		{
			global::Kampai.Game.Minion minion = currentMinionList[selectedMinionIndex[currentMinionLevelSelected]];
			return minion.Definition.ID;
		}

		public void Init(global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService defService, global::Kampai.UI.IFancyUIService fancyService, global::Kampai.Main.ILocalizationService localService, global::Kampai.Main.PlayGlobalSoundFXSignal soundFxSignal)
		{
			base.Init();
			this.playerService = playerService;
			definitionService = defService;
			fancyUIService = fancyService;
			localizationService = localService;
			this.soundFxSignal = soundFxSignal;
			levelBandDef = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(89898);
			populationDefinitions = definitionService.GetAll<global::Kampai.Game.PopulationBenefitDefinition>();
			currentMinionList = playerService.GetMinionsByLevel(currentMinionLevelSelected);
			SetupSelectionIndexing();
			SetUpgradeTokenAmount();
			PopulateMinionLevelButtons();
			UpdateUpgradeButton();
			levelSelectButtonOverlay.SetActive(false);
			Open();
		}

		internal void DecrementIndex()
		{
			global::System.Collections.Generic.List<int> list2;
			global::System.Collections.Generic.List<int> list = (list2 = selectedMinionIndex);
			int index2;
			int index = (index2 = currentMinionLevelSelected);
			index2 = list2[index2];
			list[index] = index2 - 1;
			if (selectedMinionIndex[currentMinionLevelSelected] < 0)
			{
				selectedMinionIndex[currentMinionLevelSelected] = currentMinionList.Count - 1;
			}
			UpdateVisuals();
		}

		internal int GetCurrentMinionID()
		{
			return currentMinionList[selectedMinionIndex[currentMinionLevelSelected]].ID;
		}

		internal void IncrementIndex()
		{
			global::System.Collections.Generic.List<int> list2;
			global::System.Collections.Generic.List<int> list = (list2 = selectedMinionIndex);
			int index2;
			int index = (index2 = currentMinionLevelSelected);
			index2 = list2[index2];
			list[index] = index2 + 1;
			if (selectedMinionIndex[currentMinionLevelSelected] >= currentMinionList.Count)
			{
				selectedMinionIndex[currentMinionLevelSelected] = 0;
			}
			UpdateVisuals();
		}

		internal void LevelSelected(int indexSelected)
		{
			if (base.isOpened && !isAnimating)
			{
				int lowestLevel = global::Kampai.UI.View.MinionLevelSelectorView.GetLowestLevel(playerService, definitionService);
				currentMinionLevelSelected = ((indexSelected < lowestLevel) ? lowestLevel : indexSelected);
				if (currentMinionLevelSelected != indexSelected)
				{
					UpdateMinionLevelButton();
				}
				SetMinionDisplay(false);
				PopulatePopulationBenefits(false);
				UpdateMinionAbilities();
				UpdateUpgradeButton();
			}
		}

		private void UpdateMinionLevelButton()
		{
			refreshFromIndexArgsSignal.Dispatch(typeof(global::Kampai.UI.View.MinionLevelSelectorView), currentMinionLevelSelected, new global::Kampai.UI.View.GUIArguments(logger)
			{
				arguments = { 
				{
					typeof(int),
					(object)currentMinionLevelSelected
				} }
			});
		}

		internal void Release()
		{
			CleanupMinion();
			UIUtils.SafeDestoryViews(minionLevelSelectorViews);
			UIUtils.SafeDestoryViews(populationBenefitViews);
			UIUtils.SafeDestoryViews(minionBenefitViews);
		}

		internal void SetUpgradeTokenAmount()
		{
			UpgradeTokenCount.text = localizationService.GetString("QuantityItemFormat", playerService.GetQuantity(global::Kampai.Game.StaticItem.MINION_LEVEL_TOKEN));
		}

		internal void CleanupMinion()
		{
			if (!(displayedMinion == null))
			{
				displayedMinion.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(displayedMinion.gameObject);
			}
		}

		private void DisplayMinion(bool leveledUp)
		{
			selectedMinionIndex[currentMinionLevelSelected] = global::UnityEngine.Mathf.Min(selectedMinionIndex[currentMinionLevelSelected], currentMinionList.Count - 1);
			global::Kampai.Game.Minion minion = currentMinionList[selectedMinionIndex[currentMinionLevelSelected]];
			int iD = minion.Definition.ID;
			CleanupMinion();
			displayedMinion = fancyUIService.BuildMinion(iD, leveledUp ? global::Kampai.UI.DummyCharacterAnimationState.Happy : global::Kampai.UI.DummyCharacterAnimationState.Idle, MinionSlot, true, true, minion.Level);
		}

		private void SetMinionDisplay(bool leveledUp)
		{
			currentMinionList = playerService.GetMinionsByLevel(currentMinionLevelSelected);
			LeftArrow.gameObject.SetActive(currentMinionList.Count > 1);
			RightArrow.gameObject.SetActive(currentMinionList.Count > 1);
			if (currentMinionList.Count >= 1)
			{
				DisplayMinion(leveledUp);
				SetMinionSelectionText();
				SilhouetteImage.gameObject.SetActive(false);
				return;
			}
			CleanupMinion();
			CurrentMinionLevelDisplay.gameObject.SetActive(false);
			if (currentMinionLevelSelected < levelBandDef.minionBenefitLevelBands.Count)
			{
				global::Kampai.Game.MinionBenefitLevel minionBenefitLevel = levelBandDef.minionBenefitLevelBands[currentMinionLevelSelected];
				string text = minionBenefitLevel.image;
				if (string.IsNullOrEmpty(text))
				{
					text = "btn_Main01_fill";
				}
				SilhouetteImage.sprite = UIUtils.LoadSpriteFromPath(text);
				string text2 = minionBenefitLevel.mask;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "btn_Main01_mask";
				}
				SilhouetteImage.maskSprite = UIUtils.LoadSpriteFromPath(text2);
			}
			SilhouetteImage.gameObject.SetActive(true);
		}

		private void SetMinionSelectionText()
		{
			CurrentMinionLevelDisplay.gameObject.SetActive(true);
			CurrentMinionLevelDisplay.text = string.Format("{0} / {1}", selectedMinionIndex[currentMinionLevelSelected] + 1, currentMinionList.Count);
		}

		private void SetScrollViewContentTransform(global::UnityEngine.RectTransform rect)
		{
			if (!(rect == null))
			{
				rect.anchorMin = global::UnityEngine.Vector2.zero;
				rect.anchorMax = global::UnityEngine.Vector2.one;
				rect.localScale = global::UnityEngine.Vector3.one;
				rect.localPosition = global::UnityEngine.Vector3.zero;
			}
		}

		private void SetupSelectionIndexing()
		{
			selectedMinionIndex = new global::System.Collections.Generic.List<int>();
			for (int i = 0; i < levelBandDef.minionBenefitLevelBands.Count; i++)
			{
				selectedMinionIndex.Add(0);
			}
		}

		private void UpdateUpgradeButton()
		{
			if (currentMinionLevelSelected == levelBandDef.minionBenefitLevelBands.Count - 1 || currentMinionList.Count == 0)
			{
				LevelUpButtonPanel.gameObject.SetActive(false);
				return;
			}
			UpgradeButton.ResetTapState();
			RushButton.ResetTapState();
			LevelUpButtonPanel.gameObject.SetActive(true);
			tokensToLevel = levelBandDef.minionBenefitLevelBands[currentMinionLevelSelected].tokensToLevel;
			RequiredTokenCountToLevel.text = string.Format("x{0}", tokensToLevel);
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.MINION_LEVEL_TOKEN);
			if (quantity < tokensToLevel)
			{
				UpgradeButton.gameObject.SetActive(false);
				RushButton.gameObject.SetActive(true);
				rushCost = (tokensToLevel - quantity) * (int)definitionService.Get<global::Kampai.Game.ItemDefinition>(50).BasePremiumCost;
				TokenRushCost.text = rushCost.ToString();
				RushButton.ResetTapState();
			}
			else
			{
				UpgradeButton.gameObject.SetActive(true);
				RushButton.gameObject.SetActive(false);
				UpgradeButton.StartButtonPulse(true);
			}
		}

		private void UpdateVisuals()
		{
			SetMinionSelectionText();
			DisplayMinion(false);
		}

		private global::Kampai.UI.View.PopulationBenefitView CreatePopulationBenefit(int i)
		{
			if (i < populationBenefitViews.Count)
			{
				global::Kampai.UI.View.PopulationBenefitView populationBenefitView = populationBenefitViews[i];
				populationBenefitView.gameObject.SetActive(currentMinionLevelSelected != 0);
				return populationBenefitView;
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_PopulationBenefits") as global::UnityEngine.GameObject;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			if (gameObject == null)
			{
				return null;
			}
			global::Kampai.UI.View.PopulationBenefitView component = gameObject.GetComponent<global::Kampai.UI.View.PopulationBenefitView>();
			component.transform.SetParent(PopulationScrollView.content, false);
			populationBenefitViews.Add(component);
			return component;
		}

		private void PopulatePopulationBenefits(bool checkDoober)
		{
			int count = populationDefinitions.Count;
			int num = 0;
			if (currentMinionLevelSelected == 0)
			{
				for (int i = 0; i < populationBenefitViews.Count; i++)
				{
					global::Kampai.UI.View.PopulationBenefitView populationBenefitView = populationBenefitViews[i];
					populationBenefitView.gameObject.SetActive(false);
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					global::Kampai.Game.PopulationBenefitDefinition populationBenefitDefinition = populationDefinitions[j];
					if (currentMinionLevelSelected == populationBenefitDefinition.minionLevelRequired)
					{
						global::Kampai.UI.View.PopulationBenefitView populationBenefitView2 = CreatePopulationBenefit(num++);
						populationBenefitView2.benefitDefinitionID = populationBenefitDefinition.ID;
						populationBenefitView2.UpdateView(checkDoober);
					}
				}
			}
			SetScrollViewContentTransform(PopulationScrollView.content);
			PopulationScrollView.vertical = populationBenefitViews.Count > 3;
			GoalDescriptionText.gameObject.SetActive(currentMinionLevelSelected == 0);
		}

		private void PopulateMinionLevelButtons()
		{
			int count = levelBandDef.minionBenefitLevelBands.Count;
			if (minionLevelSelectorViews.Count >= count)
			{
				return;
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_MinionLevels") as global::UnityEngine.GameObject;
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
				if (!(gameObject == null))
				{
					global::Kampai.UI.View.MinionLevelSelectorView component = gameObject.GetComponent<global::Kampai.UI.View.MinionLevelSelectorView>();
					component.index = i;
					component.SetLevelText(localizationService.GetString("MinionUpgradeLevel", i + 1));
					component.transform.SetParent(MinionLevelsScrollView.content, false);
					minionLevelSelectorViews.Add(component);
				}
			}
			SetScrollViewContentTransform(MinionLevelsScrollView.content);
			MinionLevelsScrollView.horizontal = count > 4;
		}

		private void PopulateMinionBenefits()
		{
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_MinionBenefit") as global::UnityEngine.GameObject;
			int count = levelBandDef.benefitDescriptions.Count;
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
				if (!(gameObject == null))
				{
					global::Kampai.UI.View.MinionBenefitView component = gameObject.GetComponent<global::Kampai.UI.View.MinionBenefitView>();
					component.category = levelBandDef.benefitDescriptions[i].type;
					component.transform.SetParent(BenefitScrollView.content, false);
					minionBenefitViews.Add(component);
				}
			}
			SetScrollViewContentTransform(BenefitScrollView.content);
			BenefitScrollView.vertical = count > 3;
		}

		public void UpdateMinionAbilities()
		{
			refreshAllOfTypeArgsSignal.Dispatch(typeof(global::Kampai.UI.View.MinionBenefitView), new global::Kampai.UI.View.GUIArguments(logger)
			{
				arguments = { 
				{
					typeof(int),
					(object)currentMinionLevelSelected
				} }
			});
		}

		public void AnimAbilities()
		{
			waitAnimTime = 0f;
			isWaiting = false;
			global::strange.extensions.signal.impl.Signal<float, global::Kampai.Util.Tuple<int, int>> signal = new global::strange.extensions.signal.impl.Signal<float, global::Kampai.Util.Tuple<int, int>>();
			signal.AddListener(WaitForAnimAbilities);
			refreshAllOfTypeArgsSignal.Dispatch(typeof(global::Kampai.UI.View.MinionBenefitView), new global::Kampai.UI.View.GUIArguments(logger)
			{
				arguments = 
				{
					{
						typeof(int),
						(object)currentMinionLevelSelected
					},
					{
						typeof(bool),
						(object)true
					},
					{
						typeof(global::strange.extensions.signal.impl.Signal<float, global::Kampai.Util.Tuple<int, int>>),
						(object)signal
					}
				}
			});
			signal.RemoveListener(WaitForAnimAbilities);
			base.animator.SetBool("WaitAnimAbilities", isWaiting);
			if (isWaiting)
			{
				if (waitForAnimAbilitiesFinishCoroutine != null)
				{
					StopCoroutine(waitForAnimAbilitiesFinishCoroutine);
					waitForAnimAbilitiesFinishCoroutine = null;
				}
				waitForAnimAbilitiesFinishCoroutine = StartCoroutine(WaitForAnimAbilitiesFinish(waitAnimTime));
			}
			else
			{
				AnimPopulationGoals();
			}
		}

		public void AnimLevelUpButton()
		{
			refreshFromIndexArgsSignal.Dispatch(typeof(global::Kampai.UI.View.MinionLevelSelectorView), currentMinionLevelSelected, new global::Kampai.UI.View.GUIArguments(logger)
			{
				arguments = 
				{
					{
						typeof(int),
						(object)currentMinionLevelSelected
					},
					{
						typeof(bool),
						(object)true
					}
				}
			});
			if (currentMinionLevelSelected >= 1)
			{
				GoalDescriptionText.gameObject.SetActive(false);
			}
		}

		public void StartVFX()
		{
			LevelUpParticleSystem.Play();
		}

		public void LevelMinion(int levelTo)
		{
			currentMinionLevelSelected = levelTo;
			selectedMinionIndex[currentMinionLevelSelected] = playerService.GetMinionsByLevel(currentMinionLevelSelected).Count - 1;
			LevelUpButtonPanel.gameObject.SetActive(false);
			isAnimating = true;
			base.animator.Play("Levelup");
			base.animator.SetInteger("LevelupValue", levelTo);
			levelSelectButtonOverlay.SetActive(true);
			if (currentMinionLevelSelected != 1)
			{
				PopulatePopulationBenefits(false);
			}
		}

		public void LoadMinionOnAnimationOpen()
		{
			SetMinionDisplay(false);
			PopulateMinionBenefits();
			PopulatePopulationBenefits(true);
			LevelSelected(global::Kampai.UI.View.MinionLevelSelectorView.GetLowestLevel(playerService, definitionService));
			soundFxSignal.Dispatch("Play_main_menu_open_01");
		}

		public void OnLevelupAnimationFinished()
		{
			isAnimating = false;
			isWaiting = false;
			PopulationScrollView.content.gameObject.SetActive(true);
			UpdateUpgradeButton();
			PopulatePopulationBenefits(true);
			LevelUpParticleSystem.Stop();
			levelSelectButtonOverlay.SetActive(false);
		}

		public void UpdateMinionCustom()
		{
			SetMinionDisplay(true);
		}

		private global::Kampai.UI.View.MinionBenefitView GetAbilitiesView(int index)
		{
			return minionBenefitViews[index];
		}

		public void WaitForAnimAbilities(float animTime, global::Kampai.Util.Tuple<int, int> index)
		{
			if (!(animTime < 0.01f))
			{
				isWaiting = true;
				StartCoroutine(WaitForAnimAbilitiesFinish(waitAnimTime, index));
				waitAnimTime += animTime;
			}
		}

		private global::System.Collections.IEnumerator WaitForAnimAbilitiesFinish(float delayTime, global::Kampai.Util.Tuple<int, int> index)
		{
			yield return new global::UnityEngine.WaitForSeconds(delayTime);
			global::Kampai.UI.View.MinionBenefitView abilityView = GetAbilitiesView(index.Item1);
			if (!(abilityView == null))
			{
				abilityView.AnimateLevelBar(index.Item2);
			}
		}

		private global::System.Collections.IEnumerator WaitForAnimAbilitiesFinish(float time)
		{
			if (currentMinionLevelSelected == 1)
			{
				PopulationScrollView.content.gameObject.SetActive(false);
			}
			yield return new global::UnityEngine.WaitForSeconds(time);
			AnimPopulationGoals();
			waitForAnimAbilitiesFinishCoroutine = null;
		}

		private void AnimPopulationGoals()
		{
			if (currentMinionLevelSelected == 1)
			{
				PopulationScrollView.content.gameObject.SetActive(false);
				base.animator.Play("Show Goals");
				PopulatePopulationBenefits(false);
			}
			else
			{
				OnLevelupAnimationFinished();
			}
		}

		internal uint GetTokensForCurrentMinion()
		{
			return (uint)levelBandDef.minionBenefitLevelBands[currentMinionLevelSelected].tokensToLevel;
		}
	}
}
