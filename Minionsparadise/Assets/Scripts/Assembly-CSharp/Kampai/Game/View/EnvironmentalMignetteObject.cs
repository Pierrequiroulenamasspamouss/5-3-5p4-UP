namespace Kampai.Game.View
{
	public class EnvironmentalMignetteObject : global::UnityEngine.MonoBehaviour
	{
		public enum EnvironmentalMignetteAudioTypes
		{
			Tree = 0
		}

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("EnvironmentalMignetteObject") as global::Kampai.Util.IKampaiLogger;

		public global::Kampai.Game.View.EnvironmentalMignetteObject.EnvironmentalMignetteAudioTypes AudioEffectType;

		public global::UnityEngine.Animator Animator;

		public global::UnityEngine.Transform VfxSpawnPoint;

		private bool IsPlaying;

		private void Start()
		{
			Animator = GetComponentInChildren<global::UnityEngine.Animator>();
		}

		public void PlayEnvironmentalMignetteEffect(global::strange.extensions.pool.api.IPool<global::Kampai.Game.View.PoolableVFX> vfxPool = null)
		{
			if (Animator != null)
			{
				if (IsPlaying)
				{
					return;
				}
				if (vfxPool != null)
				{
					global::Kampai.Game.View.PoolableVFX instance = vfxPool.GetInstance();
					if (instance != null)
					{
						instance.vfxGO.transform.position = VfxSpawnPoint.position;
						instance.vfxGO.SetActive(true);
						StartCoroutine(instance.CleanupCoroutine(vfxPool));
					}
				}
				StartCoroutine(PlayAnimation());
			}
			else
			{
				logger.Warning("EnvironmentalMignetteObject is missing an animator.");
			}
		}

		private global::System.Collections.IEnumerator PlayAnimation()
		{
			IsPlaying = true;
			Animator.enabled = true;
			yield return null;
			Animator.Play("Base Layer.Wiggle");
			yield return null;
			float delay = Animator.GetCurrentAnimatorStateInfo(0).length;
			yield return new global::UnityEngine.WaitForSeconds(delay);
			Animator.enabled = false;
			IsPlaying = false;
		}

		public string GetAudioStringConst()
		{
			if (AudioEffectType == global::Kampai.Game.View.EnvironmentalMignetteObject.EnvironmentalMignetteAudioTypes.Tree)
			{
				return "Play_tree_shake_01";
			}
			return string.Empty;
		}
	}
}
