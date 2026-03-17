namespace Kampai.Game.View
{
	public class EnvironmentAudioEmitterMediator : global::strange.extensions.mediation.impl.Mediator
	{
		internal CustomFMOD_StudioEventEmitter emitter;

		[Inject]
		public global::Kampai.Game.View.EnvironmentAudioEmitterView View { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera MainCamera { get; set; }

		[Inject]
		public global::Kampai.Common.Service.Audio.IFMODService FMODService { get; set; }

		[Inject]
		public global::Kampai.Main.GameLoadedModel gameLoadedModel { get; set; }

		public override void OnRegister()
		{
			View.OnTargetVisible.AddListener(OnTargetVisible);
			View.Init(MainCamera);
			StartCoroutine(WaitAFrame());
		}

		private global::System.Collections.IEnumerator WaitAFrame()
		{
			yield return null;
			while (!gameLoadedModel.gameLoaded)
			{
				yield return null;
			}
			emitter = base.gameObject.AddComponent<CustomFMOD_StudioEventEmitter>();
			emitter.shiftPosition = false;
			emitter.staticSound = false;
			emitter.path = FMODService.GetGuid(View.AudioName);
			emitter.Play();
		}

		public override void OnRemove()
		{
			View.OnTargetVisible.RemoveListener(OnTargetVisible);
		}

		internal virtual void OnTargetVisible(bool visible)
		{
			if (!(emitter == null))
			{
				if (visible)
				{
					emitter.Fade(0f, 1f, View.FadeDuration);
				}
				else
				{
					emitter.Fade(1f, 0f, View.FadeDuration);
				}
			}
		}
	}
}
