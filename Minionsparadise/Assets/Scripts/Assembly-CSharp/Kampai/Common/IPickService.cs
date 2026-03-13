namespace Kampai.Common
{
	public interface IPickService
	{
		void OnGameInput(global::UnityEngine.Vector3 inputPosition, int input, bool pressed);

		void SetIgnoreInstanceInput(int instanceId, bool isIgnored);

		global::Kampai.Common.PickState GetPickState();
	}
}
