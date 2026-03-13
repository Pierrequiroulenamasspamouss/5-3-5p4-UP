namespace Kampai.Util
{
	public interface IMinionBuilder
	{
		global::Kampai.Game.View.MinionObject BuildMinion(global::Kampai.Game.CostumeItemDefinition costume, string animatorStateMachine, global::UnityEngine.GameObject parent = null, bool showShadow = true);

		void SetLOD(global::Kampai.Util.TargetPerformance targetPerformance);

		global::Kampai.Util.TargetPerformance GetLOD();

		void RebuildMinion(global::UnityEngine.GameObject minion);
	}
}
