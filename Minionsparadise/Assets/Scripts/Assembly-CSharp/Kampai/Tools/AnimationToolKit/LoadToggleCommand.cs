namespace Kampai.Tools.AnimationToolKit
{
	public class LoadToggleCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Util.IRoutineRunner RoutineRunner { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.InitToggleSignal InitToggleSignal { get; set; }

		[Inject]
		public bool isToggleActive { get; set; }

		[Inject]
		public int InstanceId { get; set; }

		[Inject]
		public string LabelText { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject original = global::UnityEngine.Resources.Load<global::UnityEngine.GameObject>("Toggle");
			global::UnityEngine.GameObject toggleInstance = global::UnityEngine.Object.Instantiate(original);
			RoutineRunner.StartCoroutine(InitializeToggle(toggleInstance));
		}

		private global::System.Collections.IEnumerator InitializeToggle(global::UnityEngine.GameObject toggleInstance)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			global::Kampai.Tools.AnimationToolKit.ToggleMediator mediator = toggleInstance.GetComponent<global::Kampai.Tools.AnimationToolKit.ToggleMediator>();
			mediator.InitializeToggle(InstanceId, LabelText);
			toggleInstance.GetComponent<global::UnityEngine.UI.Toggle>().isOn = isToggleActive;
		}
	}
}
