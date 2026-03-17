namespace Kampai.Game
{
	public class BigThreeCharacterDefinition : global::Kampai.Game.NamedCharacterDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1072;
			}
		}

		public override global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.BigThreeCharacter(this);
		}
	}
}
