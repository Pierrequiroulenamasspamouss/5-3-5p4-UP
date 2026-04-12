namespace Kampai.Game.View
{
	public interface Animatable
	{
		bool IsAnimating { get; }

		void PlayAnimation(int stateHash, int layer, float time);

		global::UnityEngine.AnimatorStateInfo? GetAnimatorStateInfo(int layer);

		bool IsInAnimatorState(int mecanimStateHash, int layer = 0);

		void SetAnimBool(string name, bool state);

		void SetAnimFloat(string name, float state);

		void SetAnimInteger(string name, int state);

		bool GetAnimBool(string name);

		float GetAnimFloat(string name);

		int GetAnimInteger(string name);

		void SetDefaultAnimController(global::UnityEngine.RuntimeAnimatorController controller);

		void SetAnimController(global::UnityEngine.RuntimeAnimatorController controller);

		global::UnityEngine.RuntimeAnimatorController GetCurrentAnimController();

		void ClearAnimators();

		void SetAnimatorCullingMode(global::UnityEngine.AnimatorCullingMode mode);

		void ApplyRootMotion(bool enabled);

		void SetRenderLayer(int layer);

		bool getMuteStatus();
	}
}
