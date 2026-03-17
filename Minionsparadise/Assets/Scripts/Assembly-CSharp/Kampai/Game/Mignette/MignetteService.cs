namespace Kampai.Game.Mignette
{
	public class MignetteService : global::Kampai.Common.IPickService, global::Kampai.Game.Mignette.IMignetteService
	{
		private global::UnityEngine.EventSystems.EventSystem eventSystem;

		private global::System.Action<global::UnityEngine.Vector3, int, bool> inputEvent;

		[Inject(global::Kampai.Main.MainElement.UI_EVENTSYSTEM)]
		public global::UnityEngine.GameObject uiEventSystem { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			eventSystem = uiEventSystem.GetComponent<global::UnityEngine.EventSystems.EventSystem>();
		}

		public void RegisterListener(global::System.Action<global::UnityEngine.Vector3, int, bool> obj)
		{
			inputEvent = (global::System.Action<global::UnityEngine.Vector3, int, bool>)global::System.Delegate.Combine(inputEvent, obj);
		}

		public void UnregisterListener(global::System.Action<global::UnityEngine.Vector3, int, bool> obj)
		{
			inputEvent = (global::System.Action<global::UnityEngine.Vector3, int, bool>)global::System.Delegate.Remove(inputEvent, obj);
		}

		public void OnGameInput(global::UnityEngine.Vector3 inputPosition, int input, bool pressed)
		{
			if (inputEvent != null && !eventSystem.IsPointerOverGameObject())
			{
				inputEvent(inputPosition, input, pressed);
			}
		}

		public void SetIgnoreInstanceInput(int instanceId, bool isIgnored)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Common.PickState GetPickState()
		{
			throw new global::System.NotImplementedException();
		}
	}
}
