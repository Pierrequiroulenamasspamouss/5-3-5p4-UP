namespace Kampai.Game.View
{
	public class CreateInnerTubeCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int routeIndex { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("Unique_InnerTube_Prefab") as global::UnityEngine.GameObject;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.Game.View.InnerTubeObject component = gameObject.GetComponent<global::Kampai.Game.View.InnerTubeObject>();
			component.SetTubeNumberAndFloatAway(routeIndex, routineRunner, randomService);
		}
	}
}
