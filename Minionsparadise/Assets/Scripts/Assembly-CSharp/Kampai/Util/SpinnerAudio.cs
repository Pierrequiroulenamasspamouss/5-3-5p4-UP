namespace Kampai.Util
{
	public class SpinnerAudio : global::UnityEngine.MonoBehaviour
	{
		public global::strange.extensions.signal.impl.Signal<string> PlaySFXSignal { get; set; }

		private bool playTickingSound { get; set; }

		public void StartSpinningSound()
		{
			playTickingSound = true;
			StartCoroutine(PlaySpinningSoundCoroutine());
		}

		public void StopSpinningSound()
		{
			playTickingSound = false;
		}

		private global::System.Collections.IEnumerator PlaySpinningSoundCoroutine()
		{
			yield return new global::UnityEngine.WaitForSeconds(0.5f);
			while (playTickingSound)
			{
				PlaySFXSignal.Dispatch("Play_marketplace_slotTick_01");
				yield return new global::UnityEngine.WaitForSeconds(0.12f);
			}
			PlaySFXSignal.Dispatch("Play_marketplace_slotEnd_01");
		}
	}
}
