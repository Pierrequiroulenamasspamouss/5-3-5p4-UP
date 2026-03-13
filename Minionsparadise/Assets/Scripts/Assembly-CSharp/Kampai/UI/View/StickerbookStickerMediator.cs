namespace Kampai.UI.View
{
	public class StickerbookStickerMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.StickerbookStickerView view { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hideItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(definitionService);
			view.buttonView.pointerDownSignal.AddListener(PointerDown);
			view.buttonView.pointerUpSignal.AddListener(PointerUp);
			pauseSignal.AddListener(OnPause);
		}

		public override void OnRemove()
		{
			view.buttonView.pointerDownSignal.RemoveListener(PointerDown);
			view.buttonView.pointerUpSignal.RemoveListener(PointerUp);
			pauseSignal.RemoveListener(OnPause);
		}

		private void PointerDown()
		{
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[4];
			(view.transform as global::UnityEngine.RectTransform).GetWorldCorners(array);
			global::UnityEngine.Vector3 position = default(global::UnityEngine.Vector3);
			global::UnityEngine.Vector3[] array2 = array;
			foreach (global::UnityEngine.Vector3 vector in array2)
			{
				position += vector;
			}
			position /= 4f;
			soundFXSignal.Dispatch("Play_menu_popUp_02");
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_StickerInfo");
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(view.stickerDefinition);
			args.Add(view.locked);
			args.Add(uiCamera.WorldToViewportPoint(position));
			guiService.Execute(iGUICommand);
		}

		private void PointerUp()
		{
			StartCoroutine(WaitASecond());
		}

		private global::System.Collections.IEnumerator WaitASecond()
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			hideItemPopupSignal.Dispatch();
		}

		private void OnPause()
		{
			hideItemPopupSignal.Dispatch();
		}
	}
}
