namespace Kampai.Splash
{
	public class LoadInTipMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private float waitSeconds;

		private global::System.Collections.IEnumerator cycleRoutine;

		[Inject]
		public global::Kampai.Splash.ILoadInService loadInService { get; set; }

		[Inject]
		public global::Kampai.Main.LocalizationServiceInitializedSignal localizationServiceInitializedSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.LoadInTipView view { get; set; }

		public override void OnRegister()
		{
			localizationServiceInitializedSignal.AddListener(Init);
		}

		public override void OnRemove()
		{
			localizationServiceInitializedSignal.RemoveListener(Init);
			if (cycleRoutine != null)
			{
				StopCoroutine(cycleRoutine);
			}
		}

		private void Init()
		{
			UpdateTip();
		}

		private void UpdateTip()
		{
			global::Kampai.Splash.TipToShow nextTip = loadInService.GetNextTip();
			if (nextTip.Text.Length == 0)
			{
				view.gameObject.SetActive(false);
				return;
			}
			waitSeconds = nextTip.Time;
			view.SetTip(nextTip.Text);
			cycleRoutine = CycleTip();
			StartCoroutine(cycleRoutine);
		}

		private global::System.Collections.IEnumerator CycleTip()
		{
			yield return new global::UnityEngine.WaitForSeconds(waitSeconds);
			UpdateTip();
		}
	}
}
