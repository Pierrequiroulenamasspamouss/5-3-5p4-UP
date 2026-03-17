namespace Kampai.Main
{
	public static class LoadState
	{
		private static global::Kampai.Main.LoadStateType type;

		public static global::Kampai.Main.LoadStateType Get()
		{
			return type;
		}

		public static void Set(global::Kampai.Main.LoadStateType newState)
		{
			type = newState;
		}
	}
}
