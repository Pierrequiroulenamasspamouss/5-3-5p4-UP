namespace Kampai.UI.View
{
	public abstract class AbstractGenericPopupMediator<T> : global::Kampai.UI.View.KampaiMediator where T : global::UnityEngine.MonoBehaviour, IGenericPopupView
	{
		[Inject]
		public T view { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayItemPopupSignal popupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		public override void OnRegister()
		{
			T val = view;
			val.Init(localizationService);
			popupSignal.AddListener(ForceClose);
			closeSignal.AddListener(Close);
		}

		public override void OnRemove()
		{
			popupSignal.RemoveListener(ForceClose);
			closeSignal.RemoveListener(Close);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			soundFXSignal.Dispatch("Play_menu_popUp_02");
			global::Kampai.Game.ItemDefinition itemDef = args.Get<global::Kampai.Game.ItemDefinition>();
			global::UnityEngine.Vector3 itemCenter = args.Get<global::UnityEngine.Vector3>();
			Register(itemDef, itemCenter);
			global::UnityEngine.RectTransform rectTransform = args.Get<global::UnityEngine.RectTransform>();
			T val = view;
			val.gameObject.transform.parent = rectTransform.parent;
			T val2 = view;
			val2.gameObject.transform.SetAsLastSibling();
		}

		public virtual void Register(global::Kampai.Game.ItemDefinition itemDef, global::UnityEngine.Vector3 itemCenter)
		{
			T val = view;
			val.Display(itemCenter);
		}

		public virtual void ForceClose(int ignore, global::UnityEngine.RectTransform these, global::Kampai.UI.View.UIPopupType variables)
		{
			Close();
		}

		public virtual void Close()
		{
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			T val = view;
			val.Close(false);
		}

		public virtual void OnMenuClose()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
