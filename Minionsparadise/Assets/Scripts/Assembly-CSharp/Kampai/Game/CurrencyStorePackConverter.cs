namespace Kampai.Game
{
	public class CurrencyStorePackConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.CurrencyStorePackDefinition>
	{
		public override global::Kampai.Game.CurrencyStorePackDefinition Create()
		{
			return new global::Kampai.Game.CurrencyStorePackDefinition();
		}
	}
}
