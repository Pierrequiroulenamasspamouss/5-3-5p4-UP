namespace Kampai.Game
{
	public class QuestScriptKernel
	{
		private readonly global::System.Collections.Generic.Dictionary<string, global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool>> apiFunctions = new global::System.Collections.Generic.Dictionary<string, global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool>>();

		private readonly global::System.Collections.Generic.List<global::Kampai.Game.SignalListener> signalListeners = new global::System.Collections.Generic.List<global::Kampai.Game.SignalListener>();

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("QuestScriptKernel") as global::Kampai.Util.IKampaiLogger;

		public QuestScriptKernel()
		{
			global::System.Type typeFromHandle = typeof(global::Kampai.Game.QuestScriptController);
			global::System.Reflection.MethodInfo[] methods = typeFromHandle.GetMethods(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public);
			int i = 0;
			for (int num = methods.Length; i < num; i++)
			{
				global::System.Reflection.MethodInfo methodInfo = methods[i];
				object[] customAttributes = methodInfo.GetCustomAttributes(typeof(global::Kampai.Game.QuestScriptAPIAttribute), false);
				int j = 0;
				for (int num2 = customAttributes.Length; j < num2; j++)
				{
					global::Kampai.Game.QuestScriptAPIAttribute questScriptAPIAttribute = customAttributes[j] as global::Kampai.Game.QuestScriptAPIAttribute;
					if (questScriptAPIAttribute != null)
					{
						global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool> func = null;
						try
						{
							func = global::System.Delegate.CreateDelegate(typeof(global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool>), methodInfo) as global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool>;
						}
						catch (global::System.ArgumentException ex)
						{
							logger.Error("Failed grabbing {0}: {1}", methodInfo.Name, ex.Message);
							continue;
						}
						if (func == null)
						{
							logger.Error("Cannot use {0} as a Quest Script API command since it doesn't fit the delegate.", methodInfo.Name);
						}
						else
						{
							apiFunctions[questScriptAPIAttribute.Name] = func;
						}
					}
				}
			}
		}

		public global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool> GetApiFunction(string name)
		{
			global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool> value = null;
			apiFunctions.TryGetValue(name, out value);
			return value;
		}

		public bool HasApiFunction(string name)
		{
			return apiFunctions.ContainsKey(name);
		}

		public void SignalDispatched<T1, T2, T3, T4>(string name, int paramCount, T1 p1, T2 p2, T3 p3, T4 p4)
		{
			global::Kampai.Game.SignalListener[] array = signalListeners.ToArray();
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				array[i].SignalDispatched(name, paramCount, p1, p2, p3, p4);
			}
		}

		public void AddSignalListener(global::Kampai.Game.SignalListener listener)
		{
			if (!signalListeners.Contains(listener))
			{
				signalListeners.Add(listener);
			}
		}

		public void RemoveSignalListener(global::Kampai.Game.SignalListener listener)
		{
			int num = signalListeners.IndexOf(listener);
			if (num != -1)
			{
				signalListeners.RemoveAt(num);
			}
		}
	}
}
