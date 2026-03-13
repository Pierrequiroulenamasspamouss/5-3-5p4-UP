namespace Kampai.UI.View
{
	public class RandomDropMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.RandomDropView view { get; set; }

		[Inject]
		public global::Kampai.Common.ZoomPercentageSignal zoomSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal tweenSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Common.RequestZoomPercentageSignal requestSignal { get; set; }

		public override void OnRegister()
		{
			Init();
			soundFXSignal.Dispatch("Play_drop_harvest_01");
			view.timeToTweenSignal.AddListener(TweenTime);
			zoomSignal.AddListener(UpdateScale);
			view.button.ClickedSignal.AddListener(OnClick);
		}

		public override void OnRemove()
		{
			view.timeToTweenSignal.RemoveListener(TweenTime);
			zoomSignal.RemoveListener(UpdateScale);
			view.button.ClickedSignal.RemoveListener(OnClick);
		}

		private void Init()
		{
			view.Init();
			StartCoroutine(WaitAFrame());
		}

		private global::System.Collections.IEnumerator WaitAFrame()
		{
			yield return null;
			requestSignal.Dispatch();
		}

		private void UpdateScale(float percentage)
		{
			view.UpdateScale(percentage);
		}

		private void TweenTime(global::UnityEngine.Vector3 startPosition, int itemDefId)
		{
			tweenSignal.Dispatch(startPosition, global::Kampai.UI.View.DestinationType.STORAGE, itemDefId, true);
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		private void OnClick()
		{
			view.KillTweens();
			TweenTime(view.gameObject.transform.position, view.ItemDefinitionId);
		}
	}
}
