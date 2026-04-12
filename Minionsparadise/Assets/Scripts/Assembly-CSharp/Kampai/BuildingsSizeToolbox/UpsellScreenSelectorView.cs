namespace Kampai.BuildingsSizeToolbox
{
	public class UpsellScreenSelectorView : global::Kampai.Util.KampaiView
	{
		private static readonly string[] KnownScreens = new string[4] { "screen_Upsell1Pack", "screen_Upsell2Pack", "screen_Upsell3Pack", "screen_Upsell4Pack" };

		public global::UnityEngine.GameObject ScreenParent;

		public global::UnityEngine.GameObject ScrollContent;

		public global::Kampai.BuildingsSizeToolbox.UpsellScreenSelectorListItemView ListItemViewBase;

		private global::UnityEngine.GameObject currentScreen;

		[Inject]
		public global::Kampai.BuildingsSizeToolbox.NewUpsellScreenSelectedSignal newUpsellScreenSelected { get; set; }

		protected override void Start()
		{
			base.Start();
			string[] knownScreens = KnownScreens;
			foreach (string text in knownScreens)
			{
				global::Kampai.BuildingsSizeToolbox.UpsellScreenSelectorListItemView upsellScreenSelectorListItemView = global::UnityEngine.Object.Instantiate(ListItemViewBase);
				upsellScreenSelectorListItemView.ScreenName.text = text;
				upsellScreenSelectorListItemView.gameObject.SetActive(true);
				upsellScreenSelectorListItemView.transform.SetParent(ScrollContent.transform, false);
				upsellScreenSelectorListItemView.ClickedSignal.AddListener(LoadScreen);
			}
			StartCoroutine(LoadFirstScreen());
		}

		private global::System.Collections.IEnumerator LoadFirstScreen()
		{
			yield return null;
			LoadScreen(KnownScreens[0]);
		}

		private void LoadScreen(string name)
		{
			if (currentScreen != null)
			{
				global::UnityEngine.Object.Destroy(currentScreen);
			}
			global::UnityEngine.Object original = global::UnityEngine.Resources.Load("UI/UI_Prefabs/UI_Common/Features/" + name);
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original) as global::UnityEngine.GameObject;
			global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
			rectTransform.SetParent(ScreenParent.transform, false);
			rectTransform.localPosition = global::UnityEngine.Vector2.zero;
			rectTransform.sizeDelta = global::UnityEngine.Vector2.zero;
			global::Kampai.UI.View.UpSell.UpSellModalView component = gameObject.GetComponent<global::Kampai.UI.View.UpSell.UpSellModalView>();
			component.Init();
			component.Open();
			currentScreen = gameObject;
			newUpsellScreenSelected.Dispatch(component);
		}
	}
}
