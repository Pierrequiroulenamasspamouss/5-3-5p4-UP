namespace Kampai.Game
{
	public class CancelAllNotificationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.INotificationService service { get; set; }

		public override void Execute()
		{
			service.CancelAllNotifications();
		}
	}
}
