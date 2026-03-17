namespace Kampai.Util.AnimatorStateInfo
{
	public class UnloadAnimatorStateInfoCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject obj = glassCanvas.FindChild("Animator State Views");
			global::UnityEngine.Object.Destroy(obj);
		}
	}
}
