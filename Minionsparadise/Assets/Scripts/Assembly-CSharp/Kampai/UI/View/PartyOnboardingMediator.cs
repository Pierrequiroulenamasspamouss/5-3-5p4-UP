namespace Kampai.UI.View
{
	public class PartyOnboardingMediator : global::Kampai.UI.View.KampaiMediator
	{
		private global::System.Collections.Generic.Dictionary<string, float> loopingEventParameters = new global::System.Collections.Generic.Dictionary<string, float>(1);

		private CustomFMOD_StudioEventEmitter emitter;

		private bool isClicked;

		private GoTween throbTween;

		private global::Kampai.UI.View.KampaiRaycaster tempRaycaster;

		private global::UnityEngine.Coroutine autoCloseCoroutine;

		[Inject]
		public global::Kampai.UI.View.PartyOnboardingView view { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.HUD)]
		public global::UnityEngine.GameObject hud { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_DOOBER_CANVAS)]
		public global::UnityEngine.GameObject dooberCanvas { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.StartLeisurePartyPointsFinishedSignal unlockSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StartLoopingAudioSignal startLoopingAudioSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			isClicked = false;
			global::UnityEngine.Vector3 originalScale;
			throbTween = global::Kampai.Util.TweenUtil.Throb(view.transform, 0.8f, 0.7f, out originalScale);
			autoCloseCoroutine = StartCoroutine(autoClick());
		}

		public override void OnRegister()
		{
			base.OnRegister();
			view.transform.SetParent(dooberCanvas.transform, false);
			tempRaycaster = dooberCanvas.gameObject.AddComponent<global::Kampai.UI.View.KampaiRaycaster>();
			view.button.ClickedSignal.AddListener(onClicked);
			emitter = global::Kampai.Util.Audio.GetAudioEmitter.Get(view.gameObject, "LocalAudio");
			SetAudioLoop(true);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.button.ClickedSignal.RemoveListener(onClicked);
		}

		public void onClicked()
		{
			global::UnityEngine.Object.Destroy(tempRaycaster);
			if (autoCloseCoroutine != null)
			{
				StopCoroutine(autoCloseCoroutine);
			}
			isClicked = true;
			throbTween.pause();
			view.trailParticles.gameObject.SetActive(true);
			global::UnityEngine.Transform transform = hud.transform.Find("PointsPanel");
			global::UnityEngine.Vector3 destination = new global::UnityEngine.Vector3(transform.position.x, transform.position.y, transform.position.z);
			hideSkrimSignal.Dispatch("PartyOnboardSkrim");
			tweenToDestination(view.gameObject, destination);
			SetAudioLoop(false);
		}

		private void SetAudioLoop(bool isLooping)
		{
			loopingEventParameters["endLoop"] = ((!isLooping) ? 1 : 0);
			startLoopingAudioSignal.Dispatch(emitter, "Play_partyMeter_icon_01", loopingEventParameters);
		}

		private global::System.Collections.IEnumerator autoClick()
		{
			yield return new global::UnityEngine.WaitForSeconds(4f);
			if (!isClicked)
			{
				onClicked();
			}
		}

		private void tweenToDestination(global::UnityEngine.GameObject go, global::UnityEngine.Vector3 destination)
		{
			Go.to(go.transform, 2f, new GoTweenConfig().setEaseType(GoEaseType.QuartIn).scale(0.2f).position(destination)
				.onComplete(delegate
				{
					guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "PartyOnboarding");
					unlockSignal.Dispatch();
				}));
		}
	}
}
