namespace Kampai.Game
{
	public class SignalListener
	{
		private sealed class Data
		{
			public global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::Kampai.Game.CompleteSignal, global::Kampai.Game.ReturnValueContainer>> callbacks = new global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::Kampai.Game.CompleteSignal, global::Kampai.Game.ReturnValueContainer>>();
		}

		private global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.SignalListener.Data> watchedSignals = new global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.SignalListener.Data>();

		private void SetKey<T>(global::Kampai.Game.ReturnValueContainer parent, int index, int max, T value)
		{
			if (index < max)
			{
				global::Kampai.Game.ReturnValueContainer obj = parent.PushIndex();
				global::System.Type type = value.GetType();
				if (type.IsEnum)
				{
					type = typeof(long);
				}
				global::System.Reflection.MethodInfo method = typeof(global::Kampai.Game.ReturnValueContainer).GetMethod("Set", new global::System.Type[1] { type });
				method.Invoke(obj, new object[1] { value });
			}
		}

		public void SignalDispatched<T1, T2, T3, T4>(string name, int paramCount, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			global::Kampai.Game.SignalListener.Data value;
			if (!watchedSignals.TryGetValue(name, out value))
			{
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.CompleteSignal> list = new global::System.Collections.Generic.List<global::Kampai.Game.CompleteSignal>();
			int i = 0;
			for (int count = value.callbacks.Count; i < count; i++)
			{
				global::Kampai.Util.Tuple<global::Kampai.Game.CompleteSignal, global::Kampai.Game.ReturnValueContainer> tuple = value.callbacks[i];
				global::Kampai.Game.ReturnValueContainer item = tuple.Item2;
				if (paramCount < 1)
				{
					item.SetEmptyArray();
				}
				else
				{
					SetKey(item, 0, paramCount, param1);
					SetKey(item, 1, paramCount, param2);
					SetKey(item, 2, paramCount, param3);
					SetKey(item, 3, paramCount, param4);
				}
				list.Add(tuple.Item1);
			}
			value.callbacks.Clear();
			int j = 0;
			for (int count2 = list.Count; j < count2; j++)
			{
				list[j].Dispatch(name);
			}
		}

		public void ListenForSignal(string name, global::Kampai.Game.CompleteSignal callback, global::Kampai.Game.ReturnValueContainer ret)
		{
			global::Kampai.Game.SignalListener.Data value;
			if (!watchedSignals.TryGetValue(name, out value))
			{
				value = new global::Kampai.Game.SignalListener.Data();
				watchedSignals.Add(name, value);
			}
			value.callbacks.Add(new global::Kampai.Util.Tuple<global::Kampai.Game.CompleteSignal, global::Kampai.Game.ReturnValueContainer>(callback, ret));
		}

		public void StopListeningForSignal(string name)
		{
			watchedSignals.Remove(name);
		}

		public void Clear()
		{
			watchedSignals.Clear();
		}
	}
}
