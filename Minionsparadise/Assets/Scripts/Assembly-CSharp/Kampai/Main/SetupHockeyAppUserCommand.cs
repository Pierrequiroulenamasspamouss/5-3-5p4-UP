namespace Kampai.Main
{
	public class SetupHockeyAppUserCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public string UserId { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("HockeyApp");
			if (!(gameObject == null))
			{
				HockeyAppAndroid component = gameObject.GetComponent<HockeyAppAndroid>();
				component.userId = UserId;
			}
		}
	}
}
