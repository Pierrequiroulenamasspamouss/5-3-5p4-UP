public interface IGenericPopupView
{
	void Init(global::Kampai.Main.ILocalizationService localizationService);

	void Display(global::UnityEngine.Vector3 itemCenter);

	void Close(bool instant);
}
