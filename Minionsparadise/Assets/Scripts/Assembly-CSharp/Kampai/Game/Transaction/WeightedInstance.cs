namespace Kampai.Game.Transaction
{
	public class WeightedInstance : global::Kampai.Game.Instance<global::Kampai.Game.Transaction.WeightedDefinition>
	{
		public int DeckIndex { get; set; }

		public int Seed { get; set; }

		public WeightedInstance(global::Kampai.Game.Transaction.WeightedDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SEED":
				reader.Read();
				Seed = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "DECKINDEX":
				reader.Read();
				DeckIndex = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("DeckIndex");
			writer.WriteValue(DeckIndex);
			writer.WritePropertyName("Seed");
			writer.WriteValue(Seed);
		}

		public virtual global::Kampai.Util.QuantityItem NextPick(global::Kampai.Common.IRandomService gameRandomService)
		{
			if (gameRandomService != null)
			{
				int num = Count();
				int num2 = DeckIndex;
				if (num2 < 1 || num2 > num)
				{
					Seed = gameRandomService.NextInt(int.MaxValue);
					int num3 = (DeckIndex = 1);
					num2 = num3;
				}
				global::Kampai.Common.IRandomService randomService = new global::Kampai.Common.RandomService(Seed);
				global::Kampai.Game.Transaction.WeightedQuantityItem[] array = Shuffle(randomService, num);
				DeckIndex = num2 + 1;
				return array[num2 - 1];
			}
			return null;
		}

		private global::Kampai.Game.Transaction.WeightedQuantityItem[] Shuffle(global::Kampai.Common.IRandomService randomService, int count)
		{
			DeckIndex = 1;
			global::Kampai.Game.Transaction.WeightedQuantityItem[] array = new global::Kampai.Game.Transaction.WeightedQuantityItem[count];
			int num = 0;
			foreach (global::Kampai.Game.Transaction.WeightedQuantityItem entity in base.Definition.Entities)
			{
				uint weight = entity.Weight;
				for (uint num2 = 0u; num2 < weight; num2++)
				{
					array[num++] = entity;
				}
			}
			for (int i = 0; i < array.Length; i++)
			{
				int num3 = randomService.NextInt(i, array.Length);
				global::Kampai.Game.Transaction.WeightedQuantityItem weightedQuantityItem = array[i];
				array[i] = array[num3];
				array[num3] = weightedQuantityItem;
			}
			return array;
		}

		protected int Count()
		{
			int num = 0;
			foreach (global::Kampai.Game.Transaction.WeightedQuantityItem entity in base.Definition.Entities)
			{
				num += (int)entity.Weight;
			}
			return num;
		}
	}
}
