namespace Kampai.UI.View
{
	public class OrderBoardTicketDetailMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private const float waitInBetween = 0.05f;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("OrderBoardTicketDetailMediator") as global::Kampai.Util.IKampaiLogger;

		private int currentPrestigePoints;

		private int neededPrestigePoints;

		private int currentPrestigeLevel;

		private int updateTimes;

		private global::System.Collections.IEnumerator fillBarRoutine;

		private bool completeFinished;

		private bool closingModal;

		private global::System.Action fillOrderCallBack;

		private global::System.Collections.IEnumerator PointerDownWait;

		private bool showGoto;

		private global::Kampai.Game.Transaction.TransactionInstance ti;

		[Inject]
		public global::Kampai.UI.View.OrderBoardTicketDetailView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardTicketClickedSignal ticketClickedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardTicketDeletedSignal ticketDeletedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardPrestigeSlotFullSignal slotFullSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardStartFillingPrestigeBarSignal startFillingPrestigeBarSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService characterService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetFTUETextSignal setFTUETextSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hideItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayItemPopupSignal displayItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListener { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSoundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GetBuffStateSignal getBuffStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartCurrencyBuffSignal startCurrencyBuffSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StopCurrencyBuffSignal stopCurrencyBuffSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IPartyFavorAnimationService partyFavorAnimationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowBuffInfoPopupSignal showBuffPopupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.BuffInfoPopupClosedSignal buffPopupClosedSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(localService);
			updateTimes = 15;
			ticketClickedSignal.AddListener(GetTicketClicked);
			ticketDeletedSignal.AddListener(ClearTicket);
			setFTUETextSignal.AddListener(SetFTUEText);
			slotFullSignal.AddListener(SetSlotFullText);
			startFillingPrestigeBarSignal.AddListener(StartFillingPrestige);
			pauseSignal.AddListener(OnPause);
			showBuffPopupSignal.AddListener(showBuffPopup);
			buffPopupClosedSignal.AddListener(hideBuffPopup);
			startCurrencyBuffSignal.AddListener(UpdateReward);
			stopCurrencyBuffSignal.AddListener(BuffEnded);
		}

		public override void OnRemove()
		{
			hideItemPopupSignal.Dispatch();
			view.ClearDummyObject();
			ticketClickedSignal.RemoveListener(GetTicketClicked);
			ticketDeletedSignal.RemoveListener(ClearTicket);
			setFTUETextSignal.RemoveListener(SetFTUEText);
			slotFullSignal.RemoveListener(SetSlotFullText);
			startFillingPrestigeBarSignal.RemoveListener(StartFillingPrestige);
			pauseSignal.RemoveListener(OnPause);
			startCurrencyBuffSignal.RemoveListener(UpdateReward);
			stopCurrencyBuffSignal.RemoveListener(BuffEnded);
			showBuffPopupSignal.RemoveListener(showBuffPopup);
			buffPopupClosedSignal.RemoveListener(hideBuffPopup);
			global::System.Collections.Generic.List<global::Kampai.UI.View.OrderBoardRequiredItemView> itemList = view.GetItemList();
			if (itemList != null)
			{
				foreach (global::Kampai.UI.View.OrderBoardRequiredItemView item in itemList)
				{
					if (item != null)
					{
						item.pointerUpSignal.RemoveListener(PointerUp);
						item.pointerDownSignal.RemoveListener(PointerDown);
					}
				}
			}
			if (fillBarRoutine != null && !completeFinished)
			{
				completeFinished = true;
				StopCoroutine(fillBarRoutine);
				fillOrderCallBack();
			}
		}

		private void ClearTicket()
		{
			view.TicketName.gameObject.SetActive(false);
			view.SetSlotFullText("NoTicketSelected");
			view.SetupItemCount(0);
			view.PrestigePanel.SetActive(false);
			view.OrderPanel.SetActive(true);
		}

		private void StartFillingPrestige(int targetBarValue, global::System.Action FillOrderCallback)
		{
			playSoundFXSignal.Dispatch("Play_prestige_bar_scale_01");
			fillOrderCallBack = FillOrderCallback;
			fillBarRoutine = FillProgreeBarThenCall(targetBarValue - currentPrestigePoints, FillOrderCallback);
			if (targetBarValue >= neededPrestigePoints)
			{
				closingModal = true;
			}
			StartCoroutine(fillBarRoutine);
		}

		private global::System.Collections.IEnumerator FillProgreeBarThenCall(int valueOffset, global::System.Action completeCallback)
		{
			float increment = (float)valueOffset / (float)updateTimes;
			float myPrestigePoints = 0f;
			view.GlowAnimation.SetActive(true);
			for (int i = 1; i <= updateTimes; i++)
			{
				if (!completeFinished)
				{
					myPrestigePoints = (float)currentPrestigePoints + (float)i * increment;
					view.SetPrestigeProgress(myPrestigePoints, neededPrestigePoints);
					yield return new global::UnityEngine.WaitForSeconds(0.05f);
				}
			}
			yield return new global::UnityEngine.WaitForSeconds(0.25f);
			completeFinished = true;
			completeCallback();
			fillBarRoutine = null;
		}

		private void GetTicketClicked(global::Kampai.Game.OrderBoardTicket ticket, string title, bool mute)
		{
			ti = ticket.TransactionInst;
			int count = ti.Inputs.Count;
			if (!closingModal)
			{
				completeFinished = false;
			}
			view.SetupItemCount(count);
			UpdateReward();
			if (ticket.CharacterDefinitionId != 0)
			{
				SetupCharacterDetail(ticket.CharacterDefinitionId);
			}
			else
			{
				view.ClearDummyObject();
				view.SetPanelState(false);
				view.SetTitle(title);
				currentPrestigePoints = 0;
			}
			for (int i = 0; i < count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = ti.Inputs[i];
				global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(quantityItem.ID);
				uint quantity = quantityItem.Quantity;
				global::UnityEngine.Sprite icon = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
				if (string.IsNullOrEmpty(itemDefinition.Mask))
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Your Item Definition: {0} doesn' have a mask image defined for the item icon: {1}", itemDefinition.ID, itemDefinition.Image);
					itemDefinition.Mask = "btn_Circle01_mask";
				}
				global::UnityEngine.Sprite mask = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
				uint quantityByDefinitionId = playerService.GetQuantityByDefinitionId(quantityItem.ID);
				global::Kampai.UI.View.OrderBoardRequiredItemView orderBoardRequiredItemView = view.CreateRequiredItem(i, quantity, quantityByDefinitionId, icon, mask);
				orderBoardRequiredItemView.ItemDefinitionID = quantityItem.ID;
				orderBoardRequiredItemView.pointerUpSignal.AddListener(PointerUp);
				orderBoardRequiredItemView.pointerDownSignal.AddListener(PointerDown);
				if (partyFavorAnimationService.GetAllPartyFavorItems().Contains(itemDefinition.ID))
				{
					orderBoardRequiredItemView.IconAnimator.SetTrigger("IsPartyFavor");
				}
			}
		}

		private void BuffEnded()
		{
			view.DeactivateAllBuffVisuals();
		}

		private void UpdateReward()
		{
			getBuffStateSignal.Dispatch(global::Kampai.Game.BuffType.CURRENCY, SetReward);
		}

		private void SetReward(float modifier)
		{
			int xp = global::Kampai.Game.Transaction.TransactionUtil.ExtractQuantityFromTransaction(ti, 2);
			int num = global::Kampai.Game.Transaction.TransactionUtil.ExtractQuantityFromTransaction(ti, 0);
			int additionalBuffGrind = 0;
			bool flag = (int)(modifier * 100f) != 100;
			view.ActivateBuffIcons(flag, modifier);
			if (flag)
			{
				additionalBuffGrind = global::UnityEngine.Mathf.CeilToInt((float)num * modifier - (float)num);
			}
			view.SetReward(num, xp, additionalBuffGrind);
		}

		private void SetupCharacterDetail(int characterDefID)
		{
			global::Kampai.Game.Prestige prestige = characterService.GetPrestige(characterDefID);
			currentPrestigePoints = prestige.CurrentPrestigePoints;
			neededPrestigePoints = prestige.NeededPrestigePoints;
			currentPrestigeLevel = prestige.CurrentPrestigeLevel;
			bool orderInstructionEnabled = false;
			global::Kampai.Game.PrestigeType type = prestige.Definition.Type;
			if ((type == global::Kampai.Game.PrestigeType.Minion && characterService.IsTikiBarFull()) || (type == global::Kampai.Game.PrestigeType.Villain && characterService.GetEmptyCabana() == null))
			{
				orderInstructionEnabled = true;
			}
			view.SetPanelState(true, currentPrestigeLevel, prestige, orderInstructionEnabled);
			view.SetPrestigeProgress(currentPrestigePoints, neededPrestigePoints);
			if (currentPrestigePoints > 0)
			{
				view.GlowAnimation.SetActive(true);
			}
			global::Kampai.UI.DummyCharacterType characterType = fancyUIService.GetCharacterType(characterDefID);
			global::Kampai.Game.View.DummyCharacterObject character = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Happy, view.MinionSlot.transform, view.MinionSlot.VillainScale, view.MinionSlot.VillainPositionOffset, characterDefID);
			view.SetCharacter(character);
			moveAudioListener.Dispatch(false, view.MinionSlot.transform);
		}

		private void SetSlotFullText(string locKey)
		{
			view.SetSlotFullText(locKey);
			view.SetBuffRewardsPanelGlow(false);
		}

		private void SetFTUEText(string title)
		{
			string fTUEText = localService.GetString(title);
			view.SetFTUEText(fTUEText);
		}

		private void PointerDown(global::Kampai.UI.View.OrderBoardRequiredItemView itemView, global::UnityEngine.RectTransform rectTransform)
		{
			int itemDefinitionID = itemView.ItemDefinitionID;
			showGoto = !itemView.playerHasEnoughItems;
			if (PointerDownWait != null)
			{
				StopCoroutine(PointerDownWait);
				PointerDownWait = null;
			}
			displayItemPopupSignal.Dispatch(itemDefinitionID, rectTransform, showGoto ? global::Kampai.UI.View.UIPopupType.GENERICGOTO : global::Kampai.UI.View.UIPopupType.GENERIC);
		}

		private void PointerUp()
		{
			if (PointerDownWait == null)
			{
				PointerDownWait = WaitASecond();
				StartCoroutine(PointerDownWait);
			}
		}

		private global::System.Collections.IEnumerator WaitASecond()
		{
			yield return new global::UnityEngine.WaitForSeconds((!showGoto) ? 0.5f : 1f);
			hideItemPopupSignal.Dispatch();
		}

		private void OnPause()
		{
			hideItemPopupSignal.Dispatch();
		}

		private void showBuffPopup(global::UnityEngine.Vector3 vector, float offset)
		{
			view.toggleMinionSlot(false);
		}

		private void hideBuffPopup()
		{
			StartCoroutine(WaitForPopupClose());
		}

		private global::System.Collections.IEnumerator WaitForPopupClose()
		{
			yield return new global::UnityEngine.WaitForSeconds(0.23f);
			if (view != null)
			{
				view.toggleMinionSlot(true);
			}
		}
	}
}
