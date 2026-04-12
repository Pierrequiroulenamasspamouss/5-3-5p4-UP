namespace Kampai.Game.Mignette
{
	public class DestroyMignetteContextCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		public override void Execute()
		{
			global::UnityEngine.Object.Destroy(contextView);
		}
	}
}
