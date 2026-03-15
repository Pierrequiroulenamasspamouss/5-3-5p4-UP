namespace Kampai.Main
{
	public class WhiteboxContext : global::Kampai.Main.MainContext
	{
		public WhiteboxContext()
		{
		}

		public WhiteboxContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override string PlayerDataSource()
		{
			return "whitebox_player";
		}

		protected override void BindPlayerCommand()
		{
			base.commandBinder.Bind<global::Kampai.Game.LoadPlayerSignal>().To<global::Kampai.Game.LoadWhiteboxPlayerCommand>();
			base.commandBinder.Bind<global::Kampai.Game.LoginUserSignal>().To<LoginWhiteboxUserCommand>();
		}
	}
}
