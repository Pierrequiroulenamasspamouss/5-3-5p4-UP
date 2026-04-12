namespace Kampai.UI.View
{
	public abstract class UIStackMediator<T> : global::Kampai.UI.View.KampaiMediator where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
	{
		[Inject]
		public T view { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIAddedSignal uiAddedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIRemovedSignal uiRemovedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllOtherMenuSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			RegisterView();
			closeAllOtherMenuSignal.AddListener(OnCloseAllMenu);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			RemoveView();
			closeAllOtherMenuSignal.RemoveListener(OnCloseAllMenu);
		}

		protected virtual void OnEnable()
		{
			RegisterView();
		}

		protected virtual void OnDisable()
		{
			RemoveView();
		}

		protected virtual global::UnityEngine.GameObject GetViewGameObject()
		{
			T val = view;
			return (val != null) ? val.gameObject : null;
		}

		protected virtual void OnCloseAllMenu(global::UnityEngine.GameObject exception)
		{
			T val = view;
			if (exception != val.gameObject)
			{
				Close();
			}
		}

		protected abstract void Close();

		private void RegisterView()
		{
			if (view != null)
			{
				uiAddedSignal.Dispatch(GetViewGameObject(), Close);
			}
		}

		private void RemoveView()
		{
			global::UnityEngine.GameObject viewGameObject = GetViewGameObject();
			if (viewGameObject != null)
			{
				uiRemovedSignal.Dispatch(viewGameObject);
			}
		}
	}
}
