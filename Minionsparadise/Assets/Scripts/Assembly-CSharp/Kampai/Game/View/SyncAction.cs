namespace Kampai.Game.View
{
	public class SyncAction : global::Kampai.Game.View.KampaiAction
	{
		protected readonly global::System.Collections.Generic.ICollection<global::Kampai.Game.View.ActionableObject> syncObjects;

		public SyncAction(global::System.Collections.Generic.ICollection<global::Kampai.Game.View.ActionableObject> syncObjects, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.syncObjects = syncObjects;
		}

		public override void LateUpdate()
		{
			foreach (global::Kampai.Game.View.ActionableObject syncObject in syncObjects)
			{
				if (syncObject.currentAction != this)
				{
					return;
				}
			}
			base.Done = true;
		}
	}
}
