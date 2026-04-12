namespace strange.extensions.reflector.impl
{
	internal class PriorityComparer : global::System.Collections.Generic.IComparer<global::System.Reflection.MethodInfo>
	{
		private int getPriority(global::System.Reflection.MethodInfo methodInfo)
		{
			PostConstruct postConstruct = methodInfo.GetCustomAttributes(true)[0] as PostConstruct;
			return postConstruct.priority;
		}

		int global::System.Collections.Generic.IComparer<global::System.Reflection.MethodInfo>.Compare(global::System.Reflection.MethodInfo x, global::System.Reflection.MethodInfo y)
		{
			int priority = getPriority(x);
			int priority2 = getPriority(y);
			if (priority >= priority2)
			{
				return 1;
			}
			return -1;
		}
	}
}
