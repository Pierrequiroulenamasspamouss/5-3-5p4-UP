namespace Facebook.Unity
{
	internal abstract class MethodCall<T> where T : global::Facebook.Unity.IResult
	{
		public string MethodName { get; private set; }

		public global::Facebook.Unity.FacebookDelegate<T> Callback { protected get; set; }

		protected global::Facebook.Unity.FacebookBase FacebookImpl { get; set; }

		protected global::Facebook.Unity.MethodArguments Parameters { get; set; }

		public MethodCall(global::Facebook.Unity.FacebookBase facebookImpl, string methodName)
		{
			Parameters = new global::Facebook.Unity.MethodArguments();
			FacebookImpl = facebookImpl;
			MethodName = methodName;
		}

		public abstract void Call(global::Facebook.Unity.MethodArguments args = null);
	}
}
