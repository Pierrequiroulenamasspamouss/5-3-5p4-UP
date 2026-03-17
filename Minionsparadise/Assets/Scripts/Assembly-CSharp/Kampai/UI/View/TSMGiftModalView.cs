namespace Kampai.UI.View
{
	public class TSMGiftModalView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.LocalizeView Title;

		public global::Kampai.UI.View.LocalizeView CaptainMessage;

		public global::Kampai.UI.View.MinionSlotModal MinionSlot;

		public global::UnityEngine.RectTransform itemPanel;

		public global::strange.extensions.signal.impl.Signal<global::Kampai.Game.Trigger.TriggerRewardDefinition> OnRewardCollected = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.Trigger.TriggerRewardDefinition>();

		private global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject;

		private global::Kampai.UI.View.IGUIService guiService;

		public void InitializeView(global::Kampai.Game.Trigger.TriggerInstance instance, global::Kampai.UI.IFancyUIService fancyUIService, global::Kampai.UI.View.IGUIService guiService, global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal)
		{
			Init();
			this.guiService = guiService;
			global::Kampai.Game.Trigger.TriggerDefinition definition = instance.Definition;
			Title.gameObject.SetActive(true);
			Title.LocKey = definition.Title;
			CaptainMessage.gameObject.SetActive(true);
			CaptainMessage.LocKey = definition.Description;
			if (dummyCharacterObject == null)
			{
				global::Kampai.UI.DummyCharacterType type = global::Kampai.UI.DummyCharacterType.NamedCharacter;
				dummyCharacterObject = fancyUIService.CreateCharacter(type, global::Kampai.UI.DummyCharacterAnimationState.SelectedHappy, MinionSlot.transform, MinionSlot.VillainScale, MinionSlot.VillainPositionOffset, 40014);
				dummyCharacterObject.gameObject.SetActive(true);
				moveAudioListenerSignal.Dispatch(false, dummyCharacterObject.transform);
			}
			for (int i = 0; i < definition.rewards.Count; i++)
			{
				SetupItemPanels(instance, definition.rewards[i]);
			}
			Open();
		}

		public void Release()
		{
			if (dummyCharacterObject != null && dummyCharacterObject.gameObject != null)
			{
				global::UnityEngine.Object.Destroy(dummyCharacterObject.gameObject);
				dummyCharacterObject = null;
			}
			global::Kampai.UI.View.TSMItemPanelView[] componentsInChildren = itemPanel.GetComponentsInChildren<global::Kampai.UI.View.TSMItemPanelView>();
			if (componentsInChildren == null)
			{
				return;
			}
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren[i] == null) && !(componentsInChildren[i].gameObject == null))
				{
					global::UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
				}
			}
		}

		public void SetupItemPanels(global::Kampai.Game.Trigger.TriggerInstance instance, global::Kampai.Game.Trigger.TriggerRewardDefinition reward)
		{
			if (reward != null && reward.type != global::Kampai.Game.Trigger.TriggerRewardType.Identifier.Upsell)
			{
				global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_TSM_SellingItems");
				global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
				args.Add(typeof(global::Kampai.Game.Trigger.TriggerRewardDefinition), reward);
				args.Add(typeof(global::UnityEngine.Transform), itemPanel);
				args.Add(typeof(global::System.Action<global::Kampai.Game.Trigger.TriggerRewardDefinition>), new global::System.Action<global::Kampai.Game.Trigger.TriggerRewardDefinition>(OnPurchasedCallback));
				args.Add(typeof(bool), instance.RecievedRewardIds.Contains(reward.ID));
				guiService.Execute(iGUICommand);
			}
		}

		private void OnPurchasedCallback(global::Kampai.Game.Trigger.TriggerRewardDefinition rewardDefinition)
		{
			OnRewardCollected.Dispatch(rewardDefinition);
		}
	}
}
