namespace Kampai.Game
{
	public class CurrencyStoreCategoryDefinition : global::Kampai.Game.Definition, global::Kampai.Game.IDisplayableDefinition, global::System.IComparable<global::Kampai.Game.CurrencyStoreCategoryDefinition>, global::System.IEquatable<global::Kampai.Game.CurrencyStoreCategoryDefinition>, global::System.Collections.Generic.IComparer<global::Kampai.Game.CurrencyStoreCategoryDefinition>
	{
		public override int TypeCode
		{
			get
			{
				return 1142;
			}
		}

		public global::Kampai.Game.StoreCategoryType StoreCategoryType { get; set; }

		public string Image { get; set; }

		public string Mask { get; set; }

		public string Description { get; set; }

		public global::System.Collections.Generic.IList<int> StoreItemDefinitionIDs { get; set; }

		public int Priority { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, StoreCategoryType);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Image);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Mask);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Description);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, StoreItemDefinitionIDs);
			writer.Write(Priority);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			StoreCategoryType = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.StoreCategoryType>(reader);
			Image = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Mask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Description = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			StoreItemDefinitionIDs = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, StoreItemDefinitionIDs);
			Priority = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STORECATEGORYTYPE":
				reader.Read();
				StoreCategoryType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.StoreCategoryType>(reader);
				break;
			case "IMAGE":
				reader.Read();
				Image = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MASK":
				reader.Read();
				Mask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "DESCRIPTION":
				reader.Read();
				Description = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "STOREITEMDEFINITIONIDS":
				reader.Read();
				StoreItemDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, StoreItemDefinitionIDs);
				break;
			case "PRIORITY":
				reader.Read();
				Priority = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public virtual int CompareTo(global::Kampai.Game.CurrencyStoreCategoryDefinition rhs)
		{
			if (rhs == null)
			{
				return 1;
			}
			int num = rhs.Priority.CompareTo(Priority);
			if (num != 0)
			{
				return num;
			}
			return ID.CompareTo(rhs.ID);
		}

		public int Compare(global::Kampai.Game.CurrencyStoreCategoryDefinition x, global::Kampai.Game.CurrencyStoreCategoryDefinition y)
		{
			if (x == null)
			{
				return -1;
			}
			return x.CompareTo(y);
		}

		public bool Equals(global::Kampai.Game.CurrencyStoreCategoryDefinition obj)
		{
			return obj != null && Equals((object)obj);
		}

		public override string ToString()
		{
			return string.Format("{0}, PRIORITY: {1}, CATEGORY: {2}", base.ToString(), Priority, StoreCategoryType);
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			global::Kampai.Game.CurrencyStoreCategoryDefinition currencyStoreCategoryDefinition = obj as global::Kampai.Game.CurrencyStoreCategoryDefinition;
			return !object.ReferenceEquals(null, currencyStoreCategoryDefinition) && CompareTo(currencyStoreCategoryDefinition) == 0;
		}

		public override int GetHashCode()
		{
			return new { Priority, ID }.GetHashCode();
		}
	}
}
