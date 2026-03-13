namespace Kampai.Game
{
	public class PurchasedLandExpansionDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1059;
			}
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.PurchasedLandExpansion(this);
		}
	}
}
