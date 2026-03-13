namespace Kampai.Util
{
	public interface IDummyCharacterBuilder
	{
		global::Kampai.Game.View.DummyCharacterObject BuildMinion(global::Kampai.Game.Minion minion, global::Kampai.Game.CostumeItemDefinition costume, global::UnityEngine.Transform parent, bool isHigh, global::UnityEngine.Vector3 villainScale, global::UnityEngine.Vector3 villainPositionOffset);

		global::Kampai.Game.View.DummyCharacterObject BuildNamedChacter(global::Kampai.Game.NamedCharacter namedCharacter, global::UnityEngine.Transform parent, bool isHigh, global::UnityEngine.Vector3 villainScale, global::UnityEngine.Vector3 villainPositionOffset);
	}
}
