namespace Kampai.UI.View
{
	public class SettingsLearnSystemsCategoryMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.SettingsLearnSystemsCategoryView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.SettingsLearnSystemsCategorySelectedSignal categorySelectedSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public override void OnRegister()
		{
			// global::UnityEngine.Debug.Log(string.Format("ANTIGRAVITY: SettingsLearnSystemsCategoryMediator.OnRegister - View: {0}, ID: {1}", (view != null) ? view.name : "NULL", (view != null) ? view.GetInstanceID().ToString() : "N/A"));
			view.Button.ClickedSignal.AddListener(OnToggleSelected);
			categorySelectedSignal.AddListener(UpdateColor);
		}

		public override void OnRemove()
		{
			view.Button.ClickedSignal.RemoveListener(OnToggleSelected);
			categorySelectedSignal.RemoveListener(UpdateColor);
		}

		private void OnToggleSelected()
		{
			// global::UnityEngine.Debug.Log("ANTIGRAVITY: SettingsLearnSystemsCategoryMediator - Category clicked: " + ((view != null && view.Definition != null) ? view.Definition.categoryTitleLocalizedKey : "NULL"));
			
			if (playSFXSignal != null) playSFXSignal.Dispatch("Play_button_click_01");
			
			if (view != null) 
			{
				if (view.Definition != null && categorySelectedSignal != null)
				{
					categorySelectedSignal.Dispatch(view.Definition.ID);
				}
				else if (view.Definition == null)
				{
					global::UnityEngine.Debug.LogWarning("Help Category clicked but Definition is null on " + view.name);
				}
			}
		}

		private void UpdateColor(int origin)
		{
			if (origin != view.Definition.ID)
			{
				view.toggleImage.gameObject.SetActive(false);
			}
			else
			{
				view.toggleImage.gameObject.SetActive(true);
			}
		}
	}
}
