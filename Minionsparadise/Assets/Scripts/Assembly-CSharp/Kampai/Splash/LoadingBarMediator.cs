namespace Kampai.Splash
{
	public class LoadingBarMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private float start;

		private float target;

		private float current;

		private float timeTarget;

		private float timeRemaining;

		[Inject]
		public global::Kampai.Splash.LoadingBarView view { get; set; }

		[Inject]
		public global::Kampai.Splash.SplashProgressUpdateSignal splashProgressUpdateSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.SetSplashProgressSignal setSplashProgressSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCLoadScreenModel model { get; set; }

		public override void OnRegister()
		{
			splashProgressUpdateSignal.AddListener(OnSplashProgressUpdate);
			setSplashProgressSignal.AddListener(OnSetSplashProgress);
			view.Init();
			current = model.CurrentLoadProgress;
			UpdateView();
		}

		public override void OnRemove()
		{
			splashProgressUpdateSignal.RemoveListener(OnSplashProgressUpdate);
			setSplashProgressSignal.RemoveListener(OnSetSplashProgress);
			model.CurrentLoadProgress = current;
		}

		private void OnSplashProgressUpdate(int target, float time)
		{
			this.target += target;
			start = current;
			if (this.target > 100f)
			{
				this.target = 100f;
			}
			timeRemaining = time;
			timeTarget = time;
			if (time <= 0f)
			{
				current = this.target;
			}
		}

		private void OnSetSplashProgress(float progress)
		{
			start = (current = (target = global::UnityEngine.Mathf.Min(progress, 100f)));
			timeRemaining = (timeTarget = 0f);
		}

		private void Update()
		{
			if (timeRemaining > 0f && timeTarget > 0f)
			{
				float deltaTime = global::UnityEngine.Time.deltaTime;
				timeRemaining -= deltaTime;
				float num = (target - start) * (deltaTime / timeTarget);
				if (num > 0f)
				{
					current = global::UnityEngine.Mathf.Min(current + num, 100f);
				}
			}
			UpdateView();
		}

		private void UpdateView()
		{
			view.SetText(string.Format("{0:0}%", current));
			view.SetMeterFill(current);
		}
	}
}
