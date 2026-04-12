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
			view.Button.ClickedSignal.AddListener(OnToggleSelected);
			categorySelectedSignal.AddListener(UpdateColor);
		}

		public override void OnRemove()
		{
			if (view != null && view.Button != null && view.Button.ClickedSignal != null)
			{
				view.Button.ClickedSignal.RemoveListener(OnToggleSelected);
			}
			if (categorySelectedSignal != null)
			{
				categorySelectedSignal.RemoveListener(UpdateColor);
			}
		}

		private void OnToggleSelected()
		{
			
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
			if (view == null || view.Definition == null || view.toggleImage == null)
			{
				return;
			}
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
