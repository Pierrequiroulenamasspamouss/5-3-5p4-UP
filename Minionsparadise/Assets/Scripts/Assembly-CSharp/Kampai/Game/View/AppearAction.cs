namespace Kampai.Game.View
{
	public class AppearAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject obj;

		private global::UnityEngine.Vector3 position;

		public AppearAction(global::Kampai.Game.View.ActionableObject obj, global::UnityEngine.Vector3 position, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			this.position = position;
		}

		public override void Execute()
		{
			obj.transform.position = position;
			base.Done = true;
		}
	}
}
