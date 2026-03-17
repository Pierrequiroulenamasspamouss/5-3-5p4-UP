namespace Kampai.Util
{
	public class RoutineRunnerBehaviour : global::UnityEngine.MonoBehaviour
	{
		private global::System.Collections.Generic.Dictionary<string, global::Kampai.Util.TimerUnit> timerDictionary = new global::System.Collections.Generic.Dictionary<string, global::Kampai.Util.TimerUnit>();

		private global::System.Collections.Generic.List<string> removeList = new global::System.Collections.Generic.List<string>();

		public void StartTimer(string timerID, float time, global::System.Action onComplete)
		{
			if (timerDictionary.ContainsKey(timerID))
			{
				timerDictionary[timerID].TimeLeft = time;
				return;
			}
			global::Kampai.Util.TimerUnit value = new global::Kampai.Util.TimerUnit(time, onComplete);
			timerDictionary.Add(timerID, value);
		}

		public void StopTimer(string timerID)
		{
			if (timerDictionary.ContainsKey(timerID))
			{
				timerDictionary.Remove(timerID);
			}
		}

		private void Update()
		{
			global::System.Collections.Generic.Dictionary<string, global::Kampai.Util.TimerUnit>.Enumerator enumerator = timerDictionary.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::System.Collections.Generic.KeyValuePair<string, global::Kampai.Util.TimerUnit> current = enumerator.Current;
					current.Value.TimeLeft -= global::UnityEngine.Time.deltaTime;
					if (current.Value.TimeLeft < 0f)
					{
						if (current.Value.OnComplete != null)
						{
							current.Value.OnComplete();
						}
						removeList.Add(current.Key);
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			if (removeList.Count == 0)
			{
				return;
			}
			global::System.Collections.Generic.List<string>.Enumerator enumerator2 = removeList.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					timerDictionary.Remove(enumerator2.Current);
				}
			}
			finally
			{
				enumerator2.Dispose();
			}
			removeList.Clear();
		}
	}
}
