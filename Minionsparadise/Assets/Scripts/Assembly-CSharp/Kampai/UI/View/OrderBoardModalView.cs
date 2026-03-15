namespace Kampai.UI.View
{
	public class OrderBoardModalView : global::Kampai.UI.View.PopupMenuView
	{
		private const int TICKET_NUMBER = 9;

		public global::Kampai.UI.View.ButtonView CloseButton;

		public FillOrderButtonView FillOrderButton;

		public global::Kampai.UI.View.ButtonView DeleteButton;

		public global::Kampai.UI.View.ButtonView AdVideoButton;

		internal global::System.Collections.Generic.List<global::Kampai.UI.View.OrderBoardTicketView> TicketSlots = new global::System.Collections.Generic.List<global::Kampai.UI.View.OrderBoardTicketView>(9);

		internal global::Kampai.UI.View.ModalSettings modalSettings;

		private bool enabledDeleteButton = true;

		private bool isClosing;

		private float ticketRepopTime;

		private global::System.Collections.IEnumerator changeTicketCoRoutine;

		private bool adVideoButton;

		public void Init(OrderBoardBuildingTicketsView ticketsView, global::Kampai.UI.IPositionService positionService, global::Kampai.UI.View.IGUIService guiService, float ticketRepopTime, global::Kampai.Main.ILocalizationService localService, bool adVideoButton)
		{
			base.Init();
			this.ticketRepopTime = ticketRepopTime;
			for (int i = 0; i < 9; i++)
			{
				global::Kampai.UI.PositionData positionData = positionService.GetPositionData(ticketsView.GetTicketPosition(i));
				CreateTicket(i, modalSettings, positionData.WorldPositionInUI, guiService, localService);
			}
			foreach (global::Kampai.UI.View.OrderBoardTicketView ticketSlot in TicketSlots)
			{
				ticketSlot.gameObject.SetActive(false);
			}
			this.adVideoButton = adVideoButton;
			FillOrderButton.Init();
			base.Open();
		}

		public void EnableRewardedAdRushButton(bool enable)
		{
			adVideoButton = enable;
			if (FillOrderButton.previousState == global::Kampai.UI.View.OrderBoardButtonState.Rush)
			{
				AdVideoButton.gameObject.SetActive(enable);
			}
		}

		public void DestoryTickets(bool destory = true)
		{
			foreach (global::Kampai.UI.View.OrderBoardTicketView ticketSlot in TicketSlots)
			{
				if (destory)
				{
					ticketSlot.OnRemove();
					global::UnityEngine.Object.Destroy(ticketSlot.gameObject);
				}
				else
				{
					ticketSlot.gameObject.SetActive(false);
				}
			}
			if (destory)
			{
				if (changeTicketCoRoutine != null)
				{
					StopCoroutine(changeTicketCoRoutine);
				}
				TicketSlots.Clear();
			}
		}

		internal void SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState state, int rushCost = -1)
		{
			FillOrderButton.SetFillOrderButtonState(state, rushCost);
			if (adVideoButton)
			{
				bool active = false;
				if (state == global::Kampai.UI.View.OrderBoardButtonState.Rush)
				{
					active = true;
				}
				AdVideoButton.gameObject.SetActive(active);
			}
		}

		public new void Close(bool IsInstant = false)
		{
			isClosing = true;
			DestoryTickets(false);
			base.Close(IsInstant);
		}

		private void CreateTicket(int index, global::Kampai.UI.View.ModalSettings modalSettings, global::UnityEngine.Vector3 position, global::Kampai.UI.View.IGUIService guiService, global::Kampai.Main.ILocalizationService localService)
		{
			global::UnityEngine.GameObject gameObject = guiService.Execute(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_TicketPrefab");
			global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
			rectTransform.position = position;
			global::Kampai.UI.View.OrderBoardTicketView component = gameObject.GetComponent<global::Kampai.UI.View.OrderBoardTicketView>();
			component.Index = index;
			component.Init(localService);
			if (modalSettings.enableTicketThrob)
			{
				component.HighlightTicket(true);
			}
			TicketSlots.Add(component);
			rectTransform.parent = base.transform;
		}

		internal void SetupDeleteOrderButton(bool active)
		{
			global::UnityEngine.UI.Button component = DeleteButton.GetComponent<global::UnityEngine.UI.Button>();
			component.interactable = enabledDeleteButton && active;
		}

		internal void AddTicket(global::Kampai.Game.OrderBoardTicket ticket, bool isInProgress, int duration, string locText, global::Kampai.Game.IPrestigeService prestigeService, bool isInit, global::Kampai.Game.GetBuffStateSignal getBuffStateSignal, global::Kampai.UI.View.OrderBoardTicketClickedSignal ticketClicked, global::Kampai.Game.IPlayerService playerService)
		{
			if (!isClosing)
			{
				global::Kampai.UI.View.OrderBoardTicketView orderBoardTicketView = TicketSlots[ticket.BoardIndex];
				orderBoardTicketView.gameObject.SetActive(true);
				if (isInit)
				{
					SetTicketInfo(orderBoardTicketView, ticket, isInProgress, duration, locText, prestigeService, getBuffStateSignal, null, playerService);
					return;
				}
				changeTicketCoRoutine = ChangeTicket(orderBoardTicketView, ticket, isInProgress, duration, locText, prestigeService, getBuffStateSignal, ticketClicked, playerService);
				StartCoroutine(changeTicketCoRoutine);
			}
		}

		private global::System.Collections.IEnumerator ChangeTicket(global::Kampai.UI.View.OrderBoardTicketView view, global::Kampai.Game.OrderBoardTicket ticket, bool isInProgress, int duration, string locText, global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Game.GetBuffStateSignal getBuffStateSignal, global::Kampai.UI.View.OrderBoardTicketClickedSignal ticketClicked, global::Kampai.Game.IPlayerService playerService)
		{
			view.SetRootAnimation(false);
			yield return new global::UnityEngine.WaitForSeconds(ticketRepopTime);
			SetTicketInfo(view, ticket, isInProgress, duration, locText, prestigeService, getBuffStateSignal, ticketClicked, playerService);
			view.SetRootAnimation(true);
			changeTicketCoRoutine = null;
		}

		private void SetTicketInfo(global::Kampai.UI.View.OrderBoardTicketView view, global::Kampai.Game.OrderBoardTicket ticket, bool isInProgress, int duration, string locText, global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Game.GetBuffStateSignal getBuffStateSignal, global::Kampai.UI.View.OrderBoardTicketClickedSignal ticketClicked, global::Kampai.Game.IPlayerService playerService)
		{
			view.Title = locText;
			view.getBuffStateSignal = getBuffStateSignal;
			view.SetTicketInstance(ticket);
			view.NormalPanel.SetActive(ticket.CharacterDefinitionId == 0);
			view.PrestigePanel.SetActive(ticket.CharacterDefinitionId != 0);
			if (playerService.IsMinionPartyUnlocked())
			{
				view.FunPointIcon.SetActive(true);
				view.XpIcon.SetActive(false);
			}
			else
			{
				view.FunPointIcon.SetActive(false);
				view.XpIcon.SetActive(true);
			}
			if (isInProgress)
			{
				view.StartTimer(ticket.BoardIndex, duration);
			}
			else
			{
				view.SetTicketState(true);
				if (ticket.CharacterDefinitionId != 0)
				{
					int characterDefinitionId = ticket.CharacterDefinitionId;
					global::UnityEngine.Sprite characterImage;
					global::UnityEngine.Sprite characterMask;
					prestigeService.GetCharacterImageBasedOnMood(characterDefinitionId, global::Kampai.Game.CharacterImageType.SmallAvatarIcon, out characterImage, out characterMask);
					global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(characterDefinitionId);
					view.SetCharacterImage(characterImage, characterMask, prestige.CurrentPrestigeLevel == 0);
				}
			}
			if (ticketClicked != null)
			{
				ticketClicked.Dispatch(ticket, locText, false);
			}
		}

		internal void SetTicketClicks(bool enabled)
		{
			foreach (global::Kampai.UI.View.OrderBoardTicketView ticketSlot in TicketSlots)
			{
				ticketSlot.SetTicketClick(enabled);
			}
		}

		internal global::Kampai.UI.View.OrderBoardTicketView GetFirstClickableTicketIndex()
		{
			for (int i = 0; i < TicketSlots.Count; i++)
			{
				if (TicketSlots[i].gameObject.activeSelf && !TicketSlots[i].IsCounting())
				{
					return TicketSlots[i];
				}
			}
			return TicketSlots[0];
		}

		internal void SetDeleteButtonEnabled(bool isEnabled)
		{
			enabledDeleteButton = isEnabled;
			DeleteButton.GetComponent<global::UnityEngine.UI.Button>().interactable = isEnabled;
		}

		internal void ResetDoubleTap(int viewId)
		{
			foreach (global::Kampai.UI.View.OrderBoardTicketView ticketSlot in TicketSlots)
			{
				if (ticketSlot.Index != viewId)
				{
					ticketSlot.TicketMeter.RushButton.ResetTapState();
					ticketSlot.TicketMeter.RushButton.ResetAnim();
				}
			}
		}
	}
}
