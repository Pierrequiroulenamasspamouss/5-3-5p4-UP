namespace Kampai.Game.View
{
	public class Instantiator : global::strange.extensions.mediation.impl.View
	{
		public enum InstatiationEvent
		{
			OnAwake = 0,
			OnStart = 1
		}

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("Instantiator") as global::Kampai.Util.IKampaiLogger;

		public global::Kampai.Game.View.Instantiator.InstatiationEvent instatiationEvent;

		public string PrefabName = string.Empty;

		private bool isInstantiated;

		private static global::UnityEngine.GameObject gameRoot;

		protected override void Awake()
		{
			RegisterWithContext();
			if (!string.IsNullOrEmpty(PrefabName) && instatiationEvent == global::Kampai.Game.View.Instantiator.InstatiationEvent.OnAwake)
			{
				Instantiate();
			}
		}

		protected override void Start()
		{
			if (!string.IsNullOrEmpty(PrefabName) && instatiationEvent == global::Kampai.Game.View.Instantiator.InstatiationEvent.OnStart)
			{
				Instantiate();
			}
		}

		private void Instantiate()
		{
			if (isInstantiated)
			{
				return;
			}
			isInstantiated = true;
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(PrefabName);
			if (gameObject == null)
			{
				if (logger != null)
				{
					logger.Error("Unable to load: {0}", PrefabName);
				}
				return;
			}
			global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
			if (gameObject2 != null)
			{
				gameObject2.transform.parent = base.transform;
				gameObject2.transform.localPosition = global::UnityEngine.Vector3.zero;
			}
		}

		private void RegisterWithContext()
		{
			if (!registeredWithContext)
			{
				global::strange.extensions.context.impl.ContextView component = GetGameRoot().GetComponent<global::strange.extensions.context.impl.ContextView>();
				if (component == null)
				{
					throw new global::strange.extensions.mediation.impl.MediationException("Game Root missing ContextView", global::strange.extensions.mediation.api.MediationExceptionType.NO_CONTEXT);
				}
				if (component.context != null)
				{
					component.context.AddView(this);
				}
			}
		}

		private static global::UnityEngine.GameObject GetGameRoot()
		{
			if (gameRoot != null)
			{
				return gameRoot;
			}
			gameRoot = global::UnityEngine.GameObject.Find("/Game Root");
			if (gameRoot == null)
			{
				throw new global::strange.extensions.mediation.impl.MediationException("Could not find Game Root", global::strange.extensions.mediation.api.MediationExceptionType.NO_CONTEXT);
			}
			return gameRoot;
		}
	}
}
