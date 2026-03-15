namespace Kampai.UI
{
	public interface IFancyUIService
	{
		global::Kampai.Game.View.DummyCharacterObject CreateCharacter(global::Kampai.UI.DummyCharacterType type, global::Kampai.UI.DummyCharacterAnimationState startingState, global::UnityEngine.Transform parent, global::UnityEngine.Vector3 villainScale, global::UnityEngine.Vector3 villainPositionOffset, int prestigeDefinitionID = 0, bool isHighLOD = true, bool isAudible = true, bool adjustMaterial = false);

		void SetKampaiImage(global::Kampai.UI.View.KampaiImage image, string iconPath, string maskPath);

		global::Kampai.Game.View.DummyCharacterObject BuildMinion(int minionId, global::Kampai.UI.DummyCharacterAnimationState startingState, global::UnityEngine.Transform parent, bool isHighLOD = true, bool isAudible = true, int minionLevel = 0);

		global::Kampai.Game.View.BuildingObject CreateDummyBuildingObject(global::Kampai.Game.BuildingDefinition buildingDefinition, global::UnityEngine.GameObject parent, out global::Kampai.Game.Building building, global::System.Collections.Generic.IList<global::Kampai.Game.View.MinionObject> minionsList = null, bool isAudible = true);

		void ReleaseBuildingObject(global::Kampai.Game.View.BuildingObject buildingObject, global::Kampai.Game.Building building, global::System.Collections.Generic.IList<global::Kampai.Game.View.MinionObject> minionsList = null);

		global::Kampai.UI.DummyCharacterType GetCharacterType(int prestigeDefinitionID);

		void SetStenciledShaderOnBuilding(global::UnityEngine.GameObject buildingObject);
	}
}
