namespace Kampai.UI.View
{
	public class FloatingTextView : global::Kampai.UI.View.WorldToGlassView
	{
		internal global::strange.extensions.signal.impl.Signal OnRemoveSignal = new global::strange.extensions.signal.impl.Signal();

		private global::UnityEngine.UI.Text descriptionText;

		protected override string UIName
		{
			get
			{
				return "FloatingText";
			}
		}

		protected override void LoadModalData(global::Kampai.UI.View.WorldToGlassUIModal modal)
		{
			global::Kampai.UI.View.FloatingTextModal floatingTextModal = modal as global::Kampai.UI.View.FloatingTextModal;
			if (floatingTextModal == null)
			{
				logger.Error("Text WayFinder modal doesn't exist!");
				return;
			}
			descriptionText = floatingTextModal.DescriptionText;
			global::Kampai.UI.View.FloatingTextSettings floatingTextSettings = floatingTextModal.Settings as global::Kampai.UI.View.FloatingTextSettings;
			descriptionText.text = floatingTextSettings.descriptionText;
		}

		public override global::UnityEngine.Vector3 GetIndicatorPosition()
		{
			global::Kampai.Game.View.BuildingObject buildingObject = targetObject as global::Kampai.Game.View.BuildingObject;
			if (buildingObject != null)
			{
				return buildingObject.IndicatorPosition;
			}
			return global::UnityEngine.Vector3.zero;
		}

		public void SetHeight(float height)
		{
			global::UnityEngine.RectTransform component = GetComponent<global::UnityEngine.RectTransform>();
			component.sizeDelta = new global::UnityEngine.Vector2(component.sizeDelta.x, height);
		}

		protected override void OnHide()
		{
			base.OnHide();
			HideText();
		}

		protected override void OnShow()
		{
			base.OnShow();
			ShowText();
		}

		private void ShowText()
		{
			descriptionText.enabled = true;
		}

		private void HideText()
		{
			descriptionText.enabled = false;
		}

		internal override void TargetObjectNullResponse()
		{
			logger.Warning("Removing FloatingText with id: {0} since the target object does not exist", m_trackedId);
			OnRemoveSignal.Dispatch();
		}
	}
}
