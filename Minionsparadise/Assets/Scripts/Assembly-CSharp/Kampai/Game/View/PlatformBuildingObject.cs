namespace Kampai.Game.View
{
	public class PlatformBuildingObject : global::UnityEngine.MonoBehaviour, global::Kampai.Game.View.IScaffoldingPart
	{
		public global::UnityEngine.GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		public void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IDefinitionService definitionService)
		{
		}
	}
}
