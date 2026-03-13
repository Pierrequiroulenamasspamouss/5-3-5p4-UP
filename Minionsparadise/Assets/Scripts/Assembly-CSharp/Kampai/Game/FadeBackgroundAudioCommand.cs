namespace Kampai.Game
{
	public class FadeBackgroundAudioCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.Service.Audio.IFMODService fmodService { get; set; }

		[Inject]
		public bool fadeIn { get; set; }

		[Inject]
		public string snapshotName { get; set; }

		public override void Execute()
		{
			global::strange.extensions.injector.api.IInjectionBinding binding = gameContext.injectionBinder.GetBinding<CustomFMOD_StudioEventEmitter>(global::Kampai.Game.GameElement.FADE_AUDIO_EMITTER);
			if (binding == null)
			{
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("AudioFader");
				gameObject.transform.parent = contextView.transform;
				CustomFMOD_StudioEventEmitter customFMOD_StudioEventEmitter = gameObject.AddComponent<CustomFMOD_StudioEventEmitter>();
				gameContext.injectionBinder.Bind<CustomFMOD_StudioEventEmitter>().ToValue(customFMOD_StudioEventEmitter).ToName(global::Kampai.Game.GameElement.FADE_AUDIO_EMITTER);
				BeginFade(customFMOD_StudioEventEmitter);
			}
			else if (binding.value != null)
			{
				BeginFade((CustomFMOD_StudioEventEmitter)binding.value);
			}
		}

		private void BeginFade(CustomFMOD_StudioEventEmitter emitter)
		{
			if (fadeIn)
			{
				BeginFadeIn(emitter);
			}
			else
			{
				BeginFadeOut(emitter);
			}
		}

		private void BeginFadeOut(CustomFMOD_StudioEventEmitter emitter)
		{
			emitter.startEventOnAwake = false;
			emitter.SetEventParameters(new global::System.Collections.Generic.Dictionary<string, float>());
			emitter.path = fmodService.GetGuid(snapshotName);
			routineRunner.StartCoroutine(FadeOut(emitter));
		}

		private void BeginFadeIn(CustomFMOD_StudioEventEmitter emitter)
		{
			if (emitter != null && emitter.path != null)
			{
				emitter.Stop();
			}
		}

		private global::System.Collections.IEnumerator FadeOut(CustomFMOD_StudioEventEmitter emitter)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			if (emitter != null && emitter.path != null)
			{
				emitter.StartEvent();
			}
		}
	}
}
