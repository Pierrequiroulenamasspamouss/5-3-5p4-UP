namespace Kampai.Game
{
	internal sealed class SetupTimeEventServiceCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.GameObject contextView { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("TimeEventService");
			global::Kampai.Game.TimeEventService o = gameObject.AddComponent<global::Kampai.Game.TimeEventService>();
			base.injectionBinder.Bind<global::Kampai.Game.ITimeEventService>().ToValue(o).CrossContext()
				.Weak();
			gameObject.transform.parent = contextView.transform;
		}
	}
}
