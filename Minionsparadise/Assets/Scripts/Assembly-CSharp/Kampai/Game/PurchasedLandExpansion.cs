namespace Kampai.Game
{
	public class PurchasedLandExpansion : global::Kampai.Game.Instance<global::Kampai.Game.PurchasedLandExpansionDefinition>
	{
		public global::System.Collections.Generic.IList<int> PurchasedExpansions { get; set; }

		public global::System.Collections.Generic.IList<int> AdjacentExpansions { get; set; }

		public PurchasedLandExpansion(global::Kampai.Game.PurchasedLandExpansionDefinition definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
                        int num = 1; //FIX USE OF UNASSIGNED VARIABLE
                        if (num == 1)
				{
					reader.Read();
					AdjacentExpansions = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, AdjacentExpansions);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "PURCHASEDEXPANSIONS":
				reader.Read();
				PurchasedExpansions = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, PurchasedExpansions);
				break;
			}
			return true;
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			if (PurchasedExpansions != null)
			{
				writer.WritePropertyName("PurchasedExpansions");
				writer.WriteStartArray();
				global::System.Collections.Generic.IEnumerator<int> enumerator = PurchasedExpansions.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.Current;
						writer.WriteValue(current);
					}
				}
				finally
				{
					enumerator.Dispose();
				}
				writer.WriteEndArray();
			}
			if (AdjacentExpansions == null)
			{
				return;
			}
			writer.WritePropertyName("AdjacentExpansions");
			writer.WriteStartArray();
			global::System.Collections.Generic.IEnumerator<int> enumerator2 = AdjacentExpansions.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					int current2 = enumerator2.Current;
					writer.WriteValue(current2);
				}
			}
			finally
			{
				enumerator2.Dispose();
			}
			writer.WriteEndArray();
		}

		public bool HasPurchased(int expansionId)
		{
			return PurchasedExpansions.Contains(expansionId);
		}

		public bool IsAdjacentExpansion(int expansionId)
		{
			return AdjacentExpansions.Contains(expansionId);
		}

		public bool IsUnpurchasedAdjacentExpansion(int expansionId)
		{
			return !HasPurchased(expansionId) && IsAdjacentExpansion(expansionId);
		}

		public int PurchasedExpansionsCount()
		{
			return PurchasedExpansions.Count;
		}
	}
}
