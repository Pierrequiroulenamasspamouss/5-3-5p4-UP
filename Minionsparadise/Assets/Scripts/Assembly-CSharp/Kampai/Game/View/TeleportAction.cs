namespace Kampai.Game.View
{
	public class TeleportAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject obj;

		private global::UnityEngine.Vector3 position;

		private global::UnityEngine.Vector3 eulerAngles;

		public TeleportAction(global::Kampai.Game.View.ActionableObject obj, global::UnityEngine.Vector3 position, global::UnityEngine.Vector3 eulerAngles, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			this.position = position;
			this.eulerAngles = eulerAngles;
		}

		public override void Execute()
		{
			global::UnityEngine.Transform transform = obj.transform;
			transform.position = position;
			transform.eulerAngles = eulerAngles;
			base.Done = true;
		}
	}
}
