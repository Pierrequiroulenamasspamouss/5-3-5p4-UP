namespace Kampai.Game
{
	public class BigThreeCharacter : global::Kampai.Game.NamedCharacter<global::Kampai.Game.BigThreeCharacterDefinition>
	{
		public BigThreeCharacter(global::Kampai.Game.BigThreeCharacterDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go)
		{
			switch (base.Definition.Type)
			{
			case global::Kampai.Game.NamedCharacterType.KEVIN:
				return go.AddComponent<global::Kampai.Game.View.KevinView>();
			case global::Kampai.Game.NamedCharacterType.STUART:
				return go.AddComponent<global::Kampai.Game.View.StuartView>();
			case global::Kampai.Game.NamedCharacterType.PHIL:
				return go.AddComponent<global::Kampai.Game.View.PhilView>();
			default:
				return null;
			}
		}
	}
}
