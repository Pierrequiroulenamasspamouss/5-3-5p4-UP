namespace Kampai.Game
{
	public class PhilCharacter : global::Kampai.Game.NamedCharacter<global::Kampai.Game.PhilCharacterDefinition>
	{
		public PhilCharacter(global::Kampai.Game.PhilCharacterDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go)
		{
			return go.AddComponent<global::Kampai.Game.View.PhilView>();
		}
	}
}
