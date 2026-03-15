namespace Kampai.Game
{
	public class SalePackConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.SalePackDefinition>
	{
		public override global::Kampai.Game.SalePackDefinition Create()
		{
			return new global::Kampai.Game.SalePackDefinition();
		}
	}
}
