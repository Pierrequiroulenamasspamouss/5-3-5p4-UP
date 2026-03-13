namespace Kampai.Tools.AnimationToolKit
{
	internal sealed class GachaButtonPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.Tools.AnimationToolKit.GachaButtonPanelView view { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.AnimationToolkitModel model { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.PlayMinionAnimationSignal playGachaSignal { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.EnableInterfaceSignal enableUISignal { get; set; }

		public override void OnRegister()
		{
			global::System.Collections.Generic.ICollection<global::Kampai.Game.MinionAnimationDefinition> all = definitionService.GetAll<global::Kampai.Game.MinionAnimationDefinition>();
			global::System.Collections.Generic.ICollection<global::Kampai.Game.GachaAnimationDefinition> all2 = definitionService.GetAll<global::Kampai.Game.GachaAnimationDefinition>();
			if (model.Mode == global::Kampai.Tools.AnimationToolKit.AnimationToolKitMode.Character)
			{
				all2.Clear();
				float num = (float)global::UnityEngine.Screen.height - 135f;
				global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
				rectTransform.offsetMin = new global::UnityEngine.Vector2(rectTransform.offsetMin.x, 0f - num);
			}
			view.Init(all, all2);
			view.SetButtonCallback(OnGachaButtonPressed);
			enableUISignal.AddListener(EnableUI);
		}

		public override void OnRemove()
		{
			enableUISignal.RemoveListener(EnableUI);
		}

		private void OnGachaButtonPressed(global::Kampai.Game.AnimationDefinition def)
		{
			playGachaSignal.Dispatch(def);
		}

		private void EnableUI(bool enabled)
		{
			base.gameObject.SetActive(enabled);
		}
	}
}
