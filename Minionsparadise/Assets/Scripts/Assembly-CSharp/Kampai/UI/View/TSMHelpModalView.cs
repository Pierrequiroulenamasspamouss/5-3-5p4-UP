namespace Kampai.UI.View
{
	public class TSMHelpModalView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text Title;

		public global::UnityEngine.UI.Text Message;

		public global::Kampai.UI.View.KampaiImage Image;

		public global::Kampai.UI.View.ButtonView Button;

		public global::UnityEngine.UI.Text ButtonText;

		public global::Kampai.UI.View.MinionSlotModal MinionSlot;

		private global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject;

		public void InitializeView(global::Kampai.UI.IFancyUIService fancyUIService, global::Kampai.UI.View.TSMHelpModalArguments args, global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal)
		{
			base.Init();
			base.Open();
			Title.gameObject.SetActive(true);
			Title.text = args.Title;
			Message.gameObject.SetActive(true);
			Message.text = args.Message;
			Image.sprite = UIUtils.LoadSpriteFromPath(args.Image);
			Image.maskSprite = UIUtils.LoadSpriteFromPath(args.Image);
			ButtonText.text = args.ButtonText;
			global::Kampai.UI.DummyCharacterType type = global::Kampai.UI.DummyCharacterType.NamedCharacter;
			dummyCharacterObject = fancyUIService.CreateCharacter(type, global::Kampai.UI.DummyCharacterAnimationState.SelectedHappy, MinionSlot.transform, MinionSlot.VillainScale, MinionSlot.VillainPositionOffset, 40014);
			moveAudioListenerSignal.Dispatch(false, dummyCharacterObject.transform);
		}
	}
}
