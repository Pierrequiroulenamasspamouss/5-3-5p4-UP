namespace Kampai.Game
{
	public class HelpTipTrackingService : global::Kampai.Game.IHelpTipTrackingService
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		public void TrackHelpTipShown(int tipDefinitionId)
		{
			playerService.TrackHelpTipShown(tipDefinitionId, timeService.CurrentTime());
		}

		public bool GetHelpTipWasShown(int tipDefinitionId)
		{
			return GetHelpTipShowCount(tipDefinitionId) > 0;
		}

		public int GetHelpTipShowCount(int tipDefinitionId)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Player.HelpTipTrackingItem> helpTipsTrackingData = playerService.helpTipsTrackingData;
			if (helpTipsTrackingData == null)
			{
				return 0;
			}
			for (int i = 0; i < helpTipsTrackingData.Count; i++)
			{
				if (helpTipsTrackingData[i].tipDifinitionId == tipDefinitionId)
				{
					return helpTipsTrackingData[i].showsCount;
				}
			}
			return 0;
		}

		public int GetSecondsSinceHelpTipShown(int tipDefinitionId)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Player.HelpTipTrackingItem> helpTipsTrackingData = playerService.helpTipsTrackingData;
			if (helpTipsTrackingData == null)
			{
				return int.MaxValue;
			}
			for (int i = 0; i < helpTipsTrackingData.Count; i++)
			{
				if (helpTipsTrackingData[i].tipDifinitionId == tipDefinitionId)
				{
					return helpTipsTrackingData[i].lastShownTime - timeService.CurrentTime();
				}
			}
			return int.MaxValue;
		}
	}
}
