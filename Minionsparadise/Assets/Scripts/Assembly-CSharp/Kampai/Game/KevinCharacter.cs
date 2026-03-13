namespace Kampai.Game
{
	public class KevinCharacter : global::Kampai.Game.FrolicCharacter<global::Kampai.Game.KevinCharacterDefinition>
	{
		public KevinCharacter(global::Kampai.Game.KevinCharacterDefinition def)
			: base(def)
		{
			Name = "Kevin";
		}

		public override global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go)
		{
			return go.AddComponent<global::Kampai.Game.View.KevinView>();
		}
	}
}
