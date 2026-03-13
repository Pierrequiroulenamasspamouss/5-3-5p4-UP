namespace Kampai.Game.Mignette.AlligatorSkiing
{
	public class AlligatorSkiingMignetteSetupCommand : global::Kampai.Game.Mignette.SetupMignetteManagerViewCommand
	{
		public override void Execute()
		{
			global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMignetteManagerView alligatorSkiingMignetteManagerView = CreateManagerView<global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMignetteManagerView>("AlligatorSkiingMignetteManagerView");
			base.contextView.transform.position = alligatorSkiingMignetteManagerView.MignetteBuildingObject.transform.position;
			InitializeChildObjects(alligatorSkiingMignetteManagerView);
			global::UnityEngine.Shader.Find("Kampai/Standard/Hidden");
		}
	}
}
