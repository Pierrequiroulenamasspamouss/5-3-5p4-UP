namespace Discord.Unity
{
	internal abstract class MethodCall<T> where T : global::Discord.Unity.IResult
	{
		public string MethodName { get; private set; }

		public global::Discord.Unity.FacebookDelegate<T> Callback { protected get; set; }

		protected global::Discord.Unity.FacebookBase FacebookImpl { get; set; }

		protected global::Discord.Unity.MethodArguments Parameters { get; set; }

		public MethodCall(global::Discord.Unity.FacebookBase facebookImpl, string methodName)
		{
			Parameters = new global::Discord.Unity.MethodArguments();
			FacebookImpl = facebookImpl;
			MethodName = methodName;
		}

		public abstract void Call(global::Discord.Unity.MethodArguments args = null);
	}
}
