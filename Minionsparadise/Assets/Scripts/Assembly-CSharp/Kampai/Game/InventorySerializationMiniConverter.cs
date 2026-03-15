namespace Kampai.Game
{
	public class InventorySerializationMiniConverter : global::Kampai.Util.MiniJSON.MiniJSONSerializeConverter
	{
		public object Convert(object value)
		{
			global::System.Type type = value.GetType();
			if (typeof(global::Kampai.Game.Definition).IsAssignableFrom(type) && (!type.Equals(typeof(global::Kampai.Game.SalePackDefinition)) || ((global::Kampai.Game.SalePackDefinition)value).Type != global::Kampai.Game.SalePackType.Upsell) && !typeof(global::Kampai.Game.DynamicQuestDefinition).IsAssignableFrom(type))
			{
				return ((global::Kampai.Game.Definition)value).ID;
			}
			return value;
		}
	}
}
