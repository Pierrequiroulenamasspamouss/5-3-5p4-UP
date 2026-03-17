namespace Kampai.Game.View
{
	public class SyncDelegateAction : global::Kampai.Game.View.SyncAction
	{
		private bool Fired;

		private global::System.Action Once;

		public SyncDelegateAction(global::System.Collections.Generic.IList<global::Kampai.Game.View.ActionableObject> syncObjects, global::System.Action once, global::Kampai.Util.IKampaiLogger logger)
			: base(syncObjects, logger)
		{
			Once = once;
		}

		public override void LateUpdate()
		{
			bool flag = false;
			lock (syncObjects)
			{
				base.LateUpdate();
				if (!Fired && base.Done)
				{
					Fired = true;
					flag = true;
				}
			}
			if (flag)
			{
				Once();
			}
		}
	}
}
