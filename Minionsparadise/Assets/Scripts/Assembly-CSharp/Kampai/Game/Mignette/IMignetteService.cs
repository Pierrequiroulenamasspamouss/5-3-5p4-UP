namespace Kampai.Game.Mignette
{
	public interface IMignetteService : global::Kampai.Common.IPickService
	{
		void RegisterListener(global::System.Action<global::UnityEngine.Vector3, int, bool> obj);

		void UnregisterListener(global::System.Action<global::UnityEngine.Vector3, int, bool> obj);
	}
}
