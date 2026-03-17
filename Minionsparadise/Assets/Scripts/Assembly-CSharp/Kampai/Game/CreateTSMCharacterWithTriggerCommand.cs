namespace Kampai.Game
{
	public class CreateTSMCharacterWithTriggerCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateNamedCharacterViewSignal createNamedCharacterViewSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TSMStartIntroAnimation tsmStartIntroAnimation { get; set; }

		[Inject]
		public global::Kampai.Game.ITriggerService triggerService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner RoutineRunner { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.TSMCharacter byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TSMCharacter>(301);
			if (byInstanceId != null)
			{
				if (byInstanceId.Created)
				{
					tsmStartIntroAnimation.Dispatch(IsTreasureChestIntro());
				}
				else
				{
					RoutineRunner.StartCoroutine(ShowTSMCharacter(byInstanceId));
				}
			}
		}

		private global::System.Collections.IEnumerator ShowTSMCharacter(global::Kampai.Game.TSMCharacter tsmCharacter)
		{
			yield return null;
			if (tsmCharacter != null && !tsmCharacter.Created)
			{
				createNamedCharacterViewSignal.Dispatch(tsmCharacter);
				createWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(301));
				RoutineRunner.StartCoroutine(StartTSMIntroAnimation());
			}
		}

		private global::System.Collections.IEnumerator StartTSMIntroAnimation()
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			tsmStartIntroAnimation.Dispatch(IsTreasureChestIntro());
		}

		private bool IsTreasureChestIntro()
		{
			global::Kampai.Game.Trigger.TriggerInstance activeTrigger = triggerService.ActiveTrigger;
			if (activeTrigger != null)
			{
				return activeTrigger.Definition.TreasureIntro;
			}
			return false;
		}
	}
}
