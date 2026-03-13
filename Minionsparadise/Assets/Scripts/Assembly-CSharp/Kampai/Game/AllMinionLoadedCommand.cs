namespace Kampai.Game
{
	public class AllMinionLoadedCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.ITriggerService triggerService { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardUpdateTicketOnBoardSignal updateTicketOnBoardSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.SetPartyStatesSignal setPartyStatesSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.PartySignal partySignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdatePrestigeListSignal updatePrestigeList { get; set; }

		[Inject]
		public global::Kampai.Util.ICoroutineProgressMonitor coroutineProgressMonitor { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveInvalidOneOffCraftableSignal removeInvalidOneOffCraftableSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetupSpecialEventCharacterSignal setupSpecialEventCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreMasterPlanSignal restoreMasterPlanSignal { get; set; }

		public override void Execute()
		{
			routineRunner.StartCoroutine(FinishStartup());
		}

		private global::System.Collections.IEnumerator FinishStartup()
		{
			while (coroutineProgressMonitor.HasRunningTasks())
			{
				yield return null;
			}
			global::Kampai.Util.TimeProfiler.StartSection("AllMinionLoadedCommand");
			global::Kampai.Util.TimeProfiler.StartSection("Quests");
			questService.Initialize();
			global::Kampai.Util.TimeProfiler.EndSection("Quests");
			global::Kampai.Util.TimeProfiler.StartSection("Triggers");
			triggerService.Initialize();
			global::Kampai.Util.TimeProfiler.EndSection("Triggers");
			yield return null;
			global::Kampai.Util.TimeProfiler.StartSection("Set up special event characters");
			setupSpecialEventCharacterSignal.Dispatch(-1);
			global::Kampai.Util.TimeProfiler.EndSection("update prestige");
			global::Kampai.Util.TimeProfiler.StartSection("update prestige");
			updatePrestigeList.Dispatch();
			global::Kampai.Util.TimeProfiler.EndSection("update prestige");
			global::Kampai.Util.TimeProfiler.StartSection("select level band");
			updateTicketOnBoardSignal.Dispatch();
			global::Kampai.Util.TimeProfiler.EndSection("select level band");
			global::Kampai.Util.TimeProfiler.StartSection("time service");
			telemetryService.GameStarted();
			global::Kampai.Util.TimeProfiler.EndSection("time service");
			global::Kampai.Util.TimeProfiler.StartSection("party states");
			setPartyStatesSignal.Dispatch(true);
			global::Kampai.Util.TimeProfiler.EndSection("party states");
			global::Kampai.Util.TimeProfiler.StartSection("remove one off craftable");
			removeInvalidOneOffCraftableSignal.Dispatch();
			global::Kampai.Util.TimeProfiler.EndSection("remove one off craftable");
			global::Kampai.Util.TimeProfiler.StartSection("Restore Master Plans");
			restoreMasterPlanSignal.Dispatch();
			global::Kampai.Util.TimeProfiler.EndSection("Restore Master Plans");
			global::Kampai.Util.TimeProfiler.EndSection("AllMinionLoadedCommand");
			routineRunner.StartTimer("StartingPartyOver", (float)definitionService.Get<global::Kampai.Game.MinionPartyDefinition>(80000).GetPartyDuration(true) + 1f, delegate
			{
				partySignal.Dispatch();
			});
		}
	}
}
