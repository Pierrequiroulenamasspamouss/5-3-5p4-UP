namespace Kampai.Game
{
	public class StartCOPPAFlowCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.Game.CoppaCompletedSignal coppaCompletedSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public override void Execute()
		{
			if (!coppaService.IsBirthdateKnown())
			{
				global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "COPPA_Age_Gate_Panel");
				iGUICommand.skrimScreen = "CoppaAgeGate";
				iGUICommand.disableSkrimButton = true;
				iGUICommand.darkSkrim = false;
				guiService.Execute(iGUICommand);
			}
			else
			{
				routineRunner.StartCoroutine(CompleteCoppa());
			}
		}

		private global::System.Collections.IEnumerator CompleteCoppa()
		{
			yield return new global::UnityEngine.WaitForSeconds(2f);
			coppaCompletedSignal.Dispatch();
		}
	}
}
