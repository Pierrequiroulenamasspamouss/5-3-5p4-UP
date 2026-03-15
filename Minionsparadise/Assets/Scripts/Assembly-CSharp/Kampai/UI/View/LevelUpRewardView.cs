namespace Kampai.UI.View
{
	public class LevelUpRewardView : global::Kampai.Util.KampaiView
	{
		public float closeDelay = 1.4f;

		public float waitForDooberTimer = 2f;

		public float openForTimer = 3f;

		public global::UnityEngine.UI.Text levelText;

		public global::UnityEngine.Animator animator;

		public global::UnityEngine.RectTransform scrollView;

		public global::UnityEngine.UI.ScrollRect scrollRect;

		public global::Kampai.UI.View.MinionSlotModal minionSlot;

		public global::Kampai.UI.View.ButtonView skrimButton;

		public global::Kampai.UI.View.ButtonView skipButton;

		[global::UnityEngine.Header("Reward Slider Attributes")]
		public float rewardSliderWidth = 80f;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.View.DummyCharacterObject dummyPhil;

		private global::Kampai.UI.IFancyUIService fancyService;

		private global::Kampai.Main.PlayGlobalSoundFXSignal audioSignal;

		internal global::strange.extensions.signal.impl.Signal closeSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::strange.extensions.signal.impl.Signal beginUnlockSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::strange.extensions.signal.impl.Signal closeBuffInfoSignal = new global::strange.extensions.signal.impl.Signal();

		internal int unlockCount;

		private bool partyUnlocked;

		internal global::UnityEngine.Coroutine coroutine;

		internal global::UnityEngine.Coroutine limitator;

		internal float timeTillForceClose;

		private global::System.Collections.Generic.List<global::Kampai.UI.View.RewardSliderView> sliderViews = new global::System.Collections.Generic.List<global::Kampai.UI.View.RewardSliderView>();

		internal void Init(global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Main.ILocalizationService localization, global::Kampai.UI.IFancyUIService fancyUIService, global::Kampai.Main.PlayGlobalSoundFXSignal audioSignal, global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity> rewards, global::Kampai.Game.IGuestOfHonorService guestService)
		{
			this.audioSignal = audioSignal;
			this.definitionService = definitionService;
			fancyService = fancyUIService;
			levelText.text = string.Format(localization.GetString("LevelupLevel"), playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID));
			partyUnlocked = playerService.IsMinionPartyUnlocked();
			Display(rewards);
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			global::Kampai.Game.MinionPartyDefinition definition = minionPartyInstance.Definition;
			timeTillForceClose = (float)definition.GetPartyDuration(guestService.PartyShouldProduceBuff()) - openForTimer;
			if (timeTillForceClose < 0f)
			{
				timeTillForceClose = 0f;
			}
		}

		internal void StartAnimation()
		{
			if (unlockCount <= 0)
			{
				closeSignal.Dispatch();
				return;
			}
			foreach (global::UnityEngine.Transform item in base.transform)
			{
				item.gameObject.SetActive(true);
			}
			coroutine = StartCoroutine(PlayAnimationSequence());
			if (timeTillForceClose > 0f)
			{
				limitator = StartCoroutine(Limitator());
			}
		}

		private void Display(global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity> quantityChange)
		{
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_InspirationSlider") as global::UnityEngine.GameObject;
			foreach (global::Kampai.Game.View.RewardQuantity item in quantityChange)
			{
				if ((item.ID != 21 || partyUnlocked) && !item.IsReward)
				{
					global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
					global::Kampai.UI.View.RewardSliderView component = gameObject.GetComponent<global::Kampai.UI.View.RewardSliderView>();
					global::UnityEngine.UI.LayoutElement layoutElement = gameObject.AddComponent<global::UnityEngine.UI.LayoutElement>();
					layoutElement.preferredWidth = rewardSliderWidth;
					gameObject.transform.SetParent(scrollView, false);
					global::Kampai.Game.UnlockDefinition unlockDefinition = definitionService.Get<global::Kampai.Game.UnlockDefinition>(item.ID);
					global::Kampai.Game.DisplayableDefinition displayableDefinition = definitionService.Get<global::Kampai.Game.DisplayableDefinition>(unlockDefinition.ReferencedDefinitionID);
					unlockCount++;
					component.icon.sprite = UIUtils.LoadSpriteFromPath(displayableDefinition.Image);
					component.icon.maskSprite = UIUtils.LoadSpriteFromPath(displayableDefinition.Mask);
					component.scrollRect = scrollRect;
					component.pointerDownSignal.AddListener(PointerDown);
					component.pointerUpSignal.AddListener(PointerUp);
					sliderViews.Add(component);
				}
			}
		}

		private void PointerDown()
		{
			if (limitator != null && coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
		}

		private void PointerUp()
		{
			if (limitator != null)
			{
				StartClosing();
			}
		}

		private void StartClosing()
		{
			if (coroutine == null)
			{
				coroutine = StartCoroutine(CloseDelay());
			}
		}

		internal void CleanupListeners()
		{
			foreach (global::Kampai.UI.View.RewardSliderView sliderView in sliderViews)
			{
				sliderView.pointerDownSignal.RemoveListener(PointerDown);
				sliderView.pointerUpSignal.RemoveListener(PointerUp);
			}
		}

		public void SetupMinionSlot()
		{
			int prestigeDefinitionID = 40000;
			global::Kampai.UI.DummyCharacterType characterType = fancyService.GetCharacterType(prestigeDefinitionID);
			dummyPhil = fancyService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Idle, minionSlot.transform, minionSlot.VillainScale, minionSlot.VillainPositionOffset, prestigeDefinitionID);
			dummyPhil.MakePhilDance();
		}

		public void CleanupCoroutine()
		{
			if (dummyPhil != null)
			{
				dummyPhil.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(dummyPhil.gameObject);
			}
		}

		private global::System.Collections.IEnumerator PlayAnimationSequence()
		{
			yield return new global::UnityEngine.WaitForSeconds(waitForDooberTimer);
			audioSignal.Dispatch("Play_UI_levelUp_first_01");
			closeBuffInfoSignal.Dispatch();
			animator.Play("Open");
			coroutine = StartCoroutine(CloseDelay());
		}

		private global::System.Collections.IEnumerator Limitator()
		{
			yield return new global::UnityEngine.WaitForSeconds(timeTillForceClose);
			StartClosing();
			limitator = null;
		}

		internal global::System.Collections.IEnumerator CloseDelay()
		{
			yield return new global::UnityEngine.WaitForSeconds(openForTimer);
			yield return StartCoroutine(CloseDown());
		}

		internal global::System.Collections.IEnumerator CloseDown()
		{
			if (limitator != null)
			{
				StopCoroutine(limitator);
				limitator = null;
			}
			audioSignal.Dispatch("Play_UI_levelUp_last_01");
			animator.Play("Close");
			beginUnlockSignal.Dispatch();
			yield return new global::UnityEngine.WaitForSeconds(closeDelay);
			closeSignal.Dispatch();
		}
	}
}
