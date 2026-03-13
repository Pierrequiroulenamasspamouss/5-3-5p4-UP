namespace Kampai.Game
{
	public class ItemDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.ItemDefinition>
	{
		private ItemType.ItemTypeIdentifier itemType;

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			if (jObject.Property("type") != null)
			{
				string value = jObject.Property("type").Value.ToString();
				itemType = (ItemType.ItemTypeIdentifier)(int)global::System.Enum.Parse(typeof(ItemType.ItemTypeIdentifier), value);
			}
			else
			{
				itemType = ItemType.ItemTypeIdentifier.DEFAULT;
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.ItemDefinition Create(global::System.Type objectType)
		{
			switch (itemType)
			{
			case ItemType.ItemTypeIdentifier.DEFAULT:
				return new global::Kampai.Game.ItemDefinition();
			case ItemType.ItemTypeIdentifier.INGREDIENTS:
				return new global::Kampai.Game.IngredientsItemDefinition();
			case ItemType.ItemTypeIdentifier.DYNAMIC_INGREDIENTS:
				return new global::Kampai.Game.DynamicIngredientsDefinition();
			case ItemType.ItemTypeIdentifier.COSTUME:
				return new global::Kampai.Game.CostumeItemDefinition();
			case ItemType.ItemTypeIdentifier.UNLOCK:
				return new global::Kampai.Game.UnlockDefinition();
			case ItemType.ItemTypeIdentifier.BRIDGE:
				return new global::Kampai.Game.BridgeDefinition();
			case ItemType.ItemTypeIdentifier.DROP:
				return new global::Kampai.Game.DropItemDefinition();
			case ItemType.ItemTypeIdentifier.PARTY_ANIMATION_ITEM:
				return new global::Kampai.Game.PartyFavorAnimationItemDefinition();
			case ItemType.ItemTypeIdentifier.SPECIAL_EVENT:
				return new global::Kampai.Game.SpecialEventItemDefinition();
			default:
				return null;
			}
		}
	}
}
