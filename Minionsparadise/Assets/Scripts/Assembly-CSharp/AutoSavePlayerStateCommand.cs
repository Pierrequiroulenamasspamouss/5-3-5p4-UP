public class AutoSavePlayerStateCommand : global::strange.extensions.command.impl.Command
{
	public int autoSaveInterval = 60;

	[Inject]
	public global::Kampai.Game.SavePlayerSignal savePlayerSignal { get; set; }

	[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
	public global::Kampai.Game.ISocialService facebookService { get; set; }

	[Inject(global::Kampai.Game.SocialServices.GOOGLEPLAY)]
	public global::Kampai.Game.ISocialService googlePlayService { get; set; }

	[Inject]
	public global::Kampai.Common.ICoppaService coppaService { get; set; }

	[Inject]
	public global::Kampai.Game.IConfigurationsService configService { get; set; }

	[Inject]
	public global::Kampai.Common.LogClientMetricsSignal clientMetricsSignal { get; set; }

	[Inject]
	public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

	public override void Execute()
	{
		routineRunner.StartCoroutine(PeriodicSavePlayer());
	}

	private global::System.Collections.IEnumerator PeriodicSavePlayer()
	{
		while (true)
		{
			yield return new global::UnityEngine.WaitForSeconds(autoSaveInterval);
			while (global::Kampai.Game.Mignette.View.MignetteManagerView.GetIsPlaying())
			{
				yield return new global::UnityEngine.WaitForSeconds(1f);
			}
			savePlayerSignal.Dispatch(new global::Kampai.Util.Tuple<global::Kampai.Game.SaveLocation, string, bool>(global::Kampai.Game.SaveLocation.REMOTE, string.Empty, false));
			clientMetricsSignal.Dispatch(false);
			SetAutoSaveInterval();
		}
	}

	public void SetAutoSaveInterval()
	{
		global::Kampai.Game.ConfigurationDefinition configurations = configService.GetConfigurations();
		bool isLoggedIn = facebookService.isLoggedIn;
		isLoggedIn |= !coppaService.Restricted() && googlePlayService.isLoggedIn;
		autoSaveInterval = ((!isLoggedIn) ? configurations.autoSaveIntervalUnlinkedAccount : configurations.autoSaveIntervalLinkedAccount);
		if (autoSaveInterval == 0)
		{
			autoSaveInterval = 60;
		}
	}
}
