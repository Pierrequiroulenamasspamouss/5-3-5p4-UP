namespace Kampai.UI.View
{
	public class StickerbookDescriptionMediator : global::Kampai.UI.View.KampaiMediator
	{
		[Inject]
		public global::Kampai.UI.View.StickerbookDescriptionView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hideItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		public override void OnRegister()
		{
			hideItemPopupSignal.AddListener(Close);
			view.OnMenuClose.AddListener(OnMenuClose);
		}

		public override void OnRemove()
		{
			hideItemPopupSignal.RemoveListener(Close);
			view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Game.StickerDefinition stickerDef = args.Get<global::Kampai.Game.StickerDefinition>();
			bool isLocked = args.Get<bool>();
			global::UnityEngine.Vector3 stickerCenter = args.Get<global::UnityEngine.Vector3>();
			Register(stickerDef, isLocked, stickerCenter);
		}

		private void Register(global::Kampai.Game.StickerDefinition stickerDef, bool isLocked, global::UnityEngine.Vector3 stickerCenter)
		{
			bool levelTooLow = false;
			global::Kampai.Game.Sticker sticker = null;
			if (!isLocked)
			{
				sticker = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Sticker>(stickerDef.ID);
				view.SetTitle(localizationService.GetString(stickerDef.LocalizedKey));
			}
			else if (stickerDef.UnlockLevel > playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID))
			{
				levelTooLow = true;
				view.SetTitle(localizationService.GetString("StickerbookStickerLockedPart1"));
			}
			else
			{
				view.SetTitle(localizationService.GetString(stickerDef.LocalizedKey));
			}
			SetViewDescription(levelTooLow, isLocked, sticker, stickerDef);
			view.Display(stickerCenter);
		}

		private void SetViewDescription(bool levelTooLow, bool isLocked, global::Kampai.Game.Sticker sticker, global::Kampai.Game.StickerDefinition stickerDef)
		{
			bool flag = true;
			foreach (global::System.Collections.Generic.KeyValuePair<int, bool> prestigedCharacterState in prestigeService.GetPrestigedCharacterStates(false))
			{
				if (prestigedCharacterState.Key == stickerDef.CharacterID && !prestigedCharacterState.Value)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				string text = localizationService.GetString(definitionService.Get<global::Kampai.Game.PrestigeDefinition>(stickerDef.CharacterID).LocalizedKey);
				view.SetDescription(isLocked, sticker, localizationService.GetString("StickerbookCharLocked", text), localizationService);
			}
			else if (levelTooLow)
			{
				view.SetDescription(isLocked, sticker, localizationService.GetString("StickerbookStickerLockedPart2", stickerDef.UnlockLevel), localizationService);
			}
			else
			{
				view.SetDescription(isLocked, sticker, localizationService.GetString(stickerDef.Description), localizationService);
			}
		}

		private void Close()
		{
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			view.Close();
		}

		private void OnMenuClose()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
