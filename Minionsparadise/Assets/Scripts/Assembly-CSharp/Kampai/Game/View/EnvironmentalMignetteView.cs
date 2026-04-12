namespace Kampai.Game.View
{
	public class EnvironmentalMignetteView : global::strange.extensions.mediation.impl.View
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("EnvironmentalMignetteView") as global::Kampai.Util.IKampaiLogger;

		public global::UnityEngine.GameObject VfxPrefab;

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		private global::strange.extensions.pool.api.IPool<global::Kampai.Game.View.PoolableVFX> VfxPool { get; set; }

		protected override void Start()
		{
			base.Start();
			VfxPool = new global::strange.extensions.pool.impl.Pool<global::Kampai.Game.View.PoolableVFX>();
			VfxPool.instanceProvider = new global::Kampai.Game.View.PoolableVfxProvider(VfxPrefab);
			VfxPool.size = 4;
			VfxPool.overflowBehavior = global::strange.extensions.pool.api.PoolOverflowBehavior.IGNORE;
		}

		public void AnimateEnvironmentalMignette(global::UnityEngine.GameObject emoGO)
		{
			if (emoGO == null)
			{
				logger.Warning("Can't animate a null GO");
				return;
			}
			global::Kampai.Game.View.EnvironmentalMignetteObject component = emoGO.GetComponent<global::Kampai.Game.View.EnvironmentalMignetteObject>();
			if (component != null)
			{
				PlayEffect(component);
			}
			else
			{
				logger.Warning("GO " + emoGO.name + " doesn't have an EMO");
			}
		}

		private void PlayEffect(global::Kampai.Game.View.EnvironmentalMignetteObject emo)
		{
			emo.PlayEnvironmentalMignetteEffect(VfxPool);
			string audioStringConst = emo.GetAudioStringConst();
			if (!string.IsNullOrEmpty(audioStringConst))
			{
				globalAudioSignal.Dispatch(audioStringConst);
			}
		}
	}
}
