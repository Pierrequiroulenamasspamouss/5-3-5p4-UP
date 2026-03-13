namespace Kampai.Util
{
	public interface IRoutineRunner
	{
		global::UnityEngine.Coroutine StartCoroutine(global::System.Collections.IEnumerator method);

		void StopCoroutine(global::System.Collections.IEnumerator method);

		void StartTimer(string timerID, float time, global::System.Action onComplete);

		void StopTimer(string timerID);

		global::Kampai.Util.AsyncRoutineResult StartAsyncTask(global::System.Action task, global::System.Action onComplete = null);

		global::Kampai.Util.AsyncRoutineResult StartAsyncConditionTask(global::Kampai.Util.ContidionTask task, global::System.Action onComplete = null);
	}
}
