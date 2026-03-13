namespace Kampai.Game
{
	public class CancelNotificationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.INotificationService service { get; set; }

		[Inject]
		public string type { get; set; }

		public override void Execute()
		{
			service.CancelLocalNotification(type);
		}
	}
}
