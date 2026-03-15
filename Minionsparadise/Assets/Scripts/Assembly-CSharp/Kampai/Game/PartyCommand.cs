namespace Kampai.Game
{
	public class PartyCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Game.MinionPartyDefinition partyDefinition;

		private bool delay = true;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.SetPartyStatesSignal setPartyStatesSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public PartyCommand()
		{
		}

		public PartyCommand(bool delay)
		{
			this.delay = delay;
		}

		public override void Execute()
		{
			if (delay)
			{
				routineRunner.StartCoroutine(WaitSomeTime());
			}
			else
			{
				ResolveParty();
			}
		}

		private global::System.Collections.IEnumerator WaitSomeTime()
		{
			yield return new global::UnityEngine.WaitForSeconds(0.2f);
			ResolveParty();
		}

		private void ResolveParty()
		{
			partyDefinition = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>(80000);
			int num = timeService.AppTime();
			if (num > partyDefinition.GetPartyDuration(true))
			{
				ManagePartySize();
			}
		}

		private void ManagePartySize()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Minion> idleMinions = playerService.GetIdleMinions();
			float num = partyDefinition.Percent - PartyPercent(idleMinions);
			int num2 = (int)((float)idleMinions.Count * num / 100f);
			if (num2 != 0)
			{
				ChangePartyGoers(idleMinions, num > 0f, global::System.Math.Abs(num2));
				setPartyStatesSignal.Dispatch(false);
			}
		}

		private void ChangePartyGoers(global::System.Collections.Generic.List<global::Kampai.Game.Minion> minions, bool newState, int count)
		{
			for (int i = 0; i < minions.Count; i++)
			{
				if (minions[i].Partying != newState)
				{
					minions[i].Partying = newState;
					if (--count < 1)
					{
						break;
					}
				}
			}
		}

		private float PartyPercent(global::System.Collections.Generic.List<global::Kampai.Game.Minion> minions)
		{
			if (minions.Count == 0)
			{
				return 0f;
			}
			int num = 0;
			for (int i = 0; i < minions.Count; i++)
			{
				if (minions[i].Partying)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return 0f;
			}
			return (float)num / (float)minions.Count * 100f;
		}
	}
}
