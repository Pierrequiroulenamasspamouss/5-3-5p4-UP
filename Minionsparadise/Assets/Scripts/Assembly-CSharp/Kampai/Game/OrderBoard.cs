namespace Kampai.Game
{
	public class OrderBoard : global::Kampai.Game.RepairableBuilding<global::Kampai.Game.BlackMarketBoardDefinition>, global::Kampai.Game.Building, global::Kampai.Game.ZoomableBuilding, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		public global::System.Collections.Generic.IList<global::Kampai.Game.OrderBoardTicket> tickets { get; set; }

		public global::System.Collections.Generic.IList<int> PriorityPrestigeDefinitionIDs { get; set; }

		public int HarvestableCharacterDefinitionId { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public global::Kampai.Game.ZoomableBuildingDefinition ZoomableDefinition
		{
			get
			{
				return base.Definition;
			}
		}

		[global::Newtonsoft.Json.JsonIgnore]
		public bool menuEnabled { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool MenuOpened { get; set; }

		public OrderBoard(global::Kampai.Game.BlackMarketBoardDefinition def)
			: base(def)
		{
			menuEnabled = true;
			MenuOpened = false;
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TICKETS":
				reader.Read();
				tickets = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadOrderBoardTicket, tickets);
				break;
			case "PRIORITYPRESTIGEDEFINITIONIDS":
				reader.Read();
				PriorityPrestigeDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, PriorityPrestigeDefinitionIDs);
				break;
			case "HARVESTABLECHARACTERDEFINITIONID":
				reader.Read();
				HarvestableCharacterDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
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
			if (tickets != null)
			{
				writer.WritePropertyName("tickets");
				writer.WriteStartArray();
				global::System.Collections.Generic.IEnumerator<global::Kampai.Game.OrderBoardTicket> enumerator = tickets.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						global::Kampai.Game.OrderBoardTicket current = enumerator.Current;
						writer.WriteStartObject();
						if (current.TransactionInst != null)
						{
							writer.WritePropertyName("TransactionInst");
							writer.WriteStartObject();
							writer.WritePropertyName("ID");
							writer.WriteValue(current.TransactionInst.ID);
							if (current.TransactionInst.Inputs != null)
							{
								writer.WritePropertyName("Inputs");
								writer.WriteStartArray();
								global::System.Collections.Generic.IEnumerator<global::Kampai.Util.QuantityItem> enumerator2 = current.TransactionInst.Inputs.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										global::Kampai.Util.QuantityItem current2 = enumerator2.Current;
										writer.WriteStartObject();
										writer.WritePropertyName("ID");
										writer.WriteValue(current2.ID);
										writer.WritePropertyName("Quantity");
										writer.WriteValue(current2.Quantity);
										writer.WriteEndObject();
									}
								}
								finally
								{
									enumerator2.Dispose();
								}
								writer.WriteEndArray();
							}
							if (current.TransactionInst.Outputs != null)
							{
								writer.WritePropertyName("Outputs");
								writer.WriteStartArray();
								global::System.Collections.Generic.IEnumerator<global::Kampai.Util.QuantityItem> enumerator3 = current.TransactionInst.Outputs.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										global::Kampai.Util.QuantityItem current3 = enumerator3.Current;
										writer.WriteStartObject();
										writer.WritePropertyName("ID");
										writer.WriteValue(current3.ID);
										writer.WritePropertyName("Quantity");
										writer.WriteValue(current3.Quantity);
										writer.WriteEndObject();
									}
								}
								finally
								{
									enumerator3.Dispose();
								}
								writer.WriteEndArray();
							}
							writer.WriteEndObject();
						}
						writer.WritePropertyName("StartGameTime");
						writer.WriteValue(current.StartGameTime);
						writer.WritePropertyName("BoardIndex");
						writer.WriteValue(current.BoardIndex);
						writer.WritePropertyName("OrderNameTableIndex");
						writer.WriteValue(current.OrderNameTableIndex);
						writer.WritePropertyName("StartTime");
						writer.WriteValue(current.StartTime);
						writer.WritePropertyName("CharacterDefinitionId");
						writer.WriteValue(current.CharacterDefinitionId);
						writer.WriteEndObject();
					}
				}
				finally
				{
					enumerator.Dispose();
				}
				writer.WriteEndArray();
			}
			if (PriorityPrestigeDefinitionIDs != null)
			{
				writer.WritePropertyName("PriorityPrestigeDefinitionIDs");
				writer.WriteStartArray();
				global::System.Collections.Generic.IEnumerator<int> enumerator4 = PriorityPrestigeDefinitionIDs.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						int current4 = enumerator4.Current;
						writer.WriteValue(current4);
					}
				}
				finally
				{
					enumerator4.Dispose();
				}
				writer.WriteEndArray();
			}
			writer.WritePropertyName("HarvestableCharacterDefinitionId");
			writer.WriteValue(HarvestableCharacterDefinitionId);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			if (tickets == null || tickets.Count == 0)
			{
				tickets = new global::System.Collections.Generic.List<global::Kampai.Game.OrderBoardTicket>();
				PriorityPrestigeDefinitionIDs = new global::System.Collections.Generic.List<int>();
				PriorityPrestigeDefinitionIDs.Add(40003);
				PriorityPrestigeDefinitionIDs.Add(40003);
				HarvestableCharacterDefinitionId = 0;
			}
			return gameObject.AddComponent<global::Kampai.Game.View.OrderBoardBuildingObjectView>();
		}
	}
}
