namespace Prime31
{
	public class ThreadingCallbackHelper : global::UnityEngine.MonoBehaviour
	{
		private global::System.Collections.Generic.List<global::System.Action> _actions = new global::System.Collections.Generic.List<global::System.Action>();

		private global::System.Collections.Generic.List<global::System.Action> _currentActions = new global::System.Collections.Generic.List<global::System.Action>();

		public void addActionToQueue(global::System.Action action)
		{
			lock (_actions)
			{
				_actions.Add(action);
			}
		}

		private void Update()
		{
			lock (_actions)
			{
				_currentActions.AddRange(_actions);
				_actions.Clear();
			}
			for (int i = 0; i < _currentActions.Count; i++)
			{
				_currentActions[i]();
			}
			_currentActions.Clear();
		}

		public void disableIfEmpty()
		{
			lock (_actions)
			{
				if (_actions.Count == 0)
				{
					base.enabled = false;
				}
			}
		}
	}
}
