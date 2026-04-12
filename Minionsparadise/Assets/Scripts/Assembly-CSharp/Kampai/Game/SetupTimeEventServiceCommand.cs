namespace Kampai.Game
{
	internal sealed class SetupTimeEventServiceCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.GameObject contextView { get; set; }

		public override void Execute()
		{
			global::UnityEngine.Debug.Log("[SetupTimeEventServiceCommand] Executing SetupTimeEventServiceCommand.");
			global::strange.extensions.injector.api.IInjectionBinding binding = base.injectionBinder.GetBinding<global::Kampai.Game.ITimeEventService>();
			if (binding != null)
			{
				global::Kampai.Game.ITimeEventService existing = base.injectionBinder.GetInstance<global::Kampai.Game.ITimeEventService>();
				if (existing != null)
				{
					global::UnityEngine.Debug.Log("[SetupTimeEventServiceCommand] Existing ITimeEventService found, reparenting.");
					global::UnityEngine.MonoBehaviour mb = existing as global::UnityEngine.MonoBehaviour;
					if (mb != null)
					{
						mb.gameObject.transform.parent = contextView.transform;
					}
					return;
				}
			}

			global::UnityEngine.Debug.Log("[SetupTimeEventServiceCommand] No existing service found, creating a new TimeEventService.");

			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("TimeEventService");
			global::Kampai.Game.TimeEventService o = gameObject.AddComponent<global::Kampai.Game.TimeEventService>();
			base.injectionBinder.Bind<global::Kampai.Game.ITimeEventService>().ToValue(o).CrossContext()
				.Weak();
			gameObject.transform.parent = contextView.transform;
		}
	}
}
