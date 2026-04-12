namespace Kampai.Game.View
{
	public interface IScaffoldingPart
	{
		global::UnityEngine.GameObject GameObject { get; }

		void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IDefinitionService definitionService);
	}
}
