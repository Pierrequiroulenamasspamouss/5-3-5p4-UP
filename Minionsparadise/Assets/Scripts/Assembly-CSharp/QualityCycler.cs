public class QualityCycler : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.UI.Text ButtonText;

	private int qualityLevel;

	private void Start()
	{
		qualityLevel = global::UnityEngine.QualitySettings.GetQualityLevel();
		ButtonText.text = global::UnityEngine.QualitySettings.names[qualityLevel];
	}

	public void Cycle()
	{
		qualityLevel++;
		if (qualityLevel >= global::UnityEngine.QualitySettings.names.Length)
		{
			qualityLevel = 0;
		}
		global::UnityEngine.QualitySettings.SetQualityLevel(qualityLevel, true);
		ButtonText.text = global::UnityEngine.QualitySettings.names[qualityLevel];
	}
}
