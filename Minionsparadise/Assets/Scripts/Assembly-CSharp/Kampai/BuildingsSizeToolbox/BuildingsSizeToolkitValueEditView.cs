namespace Kampai.BuildingsSizeToolbox
{
	public class BuildingsSizeToolkitValueEditView : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.UI.InputField TextField;

		public float IncrementStep = 0.1f;

		private float currentValue;

		public global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolkitValueUpdateButton[] ValueUpdateButtons;

		public global::strange.extensions.signal.impl.Signal<float> ValueChangedSignal = new global::strange.extensions.signal.impl.Signal<float>();

		public float CurrentValue
		{
			get
			{
				return currentValue;
			}
			set
			{
				currentValue = value;
				TextField.text = value.ToString();
			}
		}

		public void Start()
		{
			global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolkitValueUpdateButton[] valueUpdateButtons = ValueUpdateButtons;
			foreach (global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolkitValueUpdateButton buildingsSizeToolkitValueUpdateButton in valueUpdateButtons)
			{
				buildingsSizeToolkitValueUpdateButton.UpdateValueSignal.AddListener(UpdateValue);
			}
		}

		private void UpdateValue(float sign)
		{
			CurrentValue += sign * IncrementStep;
			ValueChangedSignal.Dispatch(currentValue);
		}

		public void OnValueChanged(string value)
		{
			float result = 0f;
			float.TryParse(value, out result);
			if (global::UnityEngine.Mathf.Abs(result - currentValue) > global::UnityEngine.Mathf.Epsilon)
			{
				CurrentValue = result;
				ValueChangedSignal.Dispatch(result);
			}
		}
	}
}
