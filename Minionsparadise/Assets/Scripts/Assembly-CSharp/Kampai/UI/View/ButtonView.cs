namespace Kampai.UI.View
{
	public class ButtonView : global::Kampai.Util.KampaiView
	{
		public bool PlaySoundOnClick = true;

		public string AudioButtonClick = "Play_button_click_01";

		public global::strange.extensions.signal.impl.Signal ClickedSignal = new global::strange.extensions.signal.impl.Signal();

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public virtual void OnClickEvent()
		{
			global::UnityEngine.Debug.Log("ANTIGRAVITY: ButtonView.OnClickEvent triggered on " + base.gameObject.name);
			if (PlaySoundOnClick)
			{
				playSFXSignal.Dispatch(AudioButtonClick);
			}
			ClickedSignal.Dispatch();
		}

		public void StartButtonPulse(bool isTrue)
		{
			global::UnityEngine.Animator component = base.gameObject.GetComponent<global::UnityEngine.Animator>();
			if (!(component == null))
			{
				component.SetBool("Pulse", isTrue);
			}
		}
	}
}
