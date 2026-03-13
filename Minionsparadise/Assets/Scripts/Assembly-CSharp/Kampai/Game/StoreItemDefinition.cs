namespace Kampai.Game
{
	public class StoreItemDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IUTCRangeable
	{
		public override int TypeCode
		{
			get
			{
				return 1145;
			}
		}

		public global::Kampai.Game.StoreItemType Type { get; set; }

		public int ReferencedDefID { get; set; }

		public int TransactionID { get; set; }

		public bool OnlyShowIfInInventory { get; set; }

		public bool OnlyShowIfOwned { get; set; }

		public bool OnlyShowIfUnlocked { get; set; }

		public bool EnableBadging { get; set; }

		public bool IsFeatured { get; set; }

		public int SpecialEventID { get; set; }

		public int UTCStartDate { get; set; }

		public int UTCEndDate { get; set; }

		public int PercentOff { get; set; }

		public string Platform { get; set; }

		public global::System.Collections.Generic.List<string> Countries { get; set; }

		public int PriorityDefinition { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Type);
			writer.Write(ReferencedDefID);
			writer.Write(TransactionID);
			writer.Write(OnlyShowIfInInventory);
			writer.Write(OnlyShowIfOwned);
			writer.Write(OnlyShowIfUnlocked);
			writer.Write(EnableBadging);
			writer.Write(IsFeatured);
			writer.Write(SpecialEventID);
			writer.Write(UTCStartDate);
			writer.Write(UTCEndDate);
			writer.Write(PercentOff);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Platform);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteString, Countries);
			writer.Write(PriorityDefinition);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.StoreItemType>(reader);
			ReferencedDefID = reader.ReadInt32();
			TransactionID = reader.ReadInt32();
			OnlyShowIfInInventory = reader.ReadBoolean();
			OnlyShowIfOwned = reader.ReadBoolean();
			OnlyShowIfUnlocked = reader.ReadBoolean();
			EnableBadging = reader.ReadBoolean();
			IsFeatured = reader.ReadBoolean();
			SpecialEventID = reader.ReadInt32();
			UTCStartDate = reader.ReadInt32();
			UTCEndDate = reader.ReadInt32();
			PercentOff = reader.ReadInt32();
			Platform = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Countries = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadString, Countries);
			PriorityDefinition = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TYPE":
				reader.Read();
				Type = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.StoreItemType>(reader);
				break;
			case "REFERENCEDDEFID":
				reader.Read();
				ReferencedDefID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TRANSACTIONID":
				reader.Read();
				TransactionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ONLYSHOWIFININVENTORY":
				reader.Read();
				OnlyShowIfInInventory = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "ONLYSHOWIFOWNED":
				reader.Read();
				OnlyShowIfOwned = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "ONLYSHOWIFUNLOCKED":
				reader.Read();
				OnlyShowIfUnlocked = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "ENABLEBADGING":
				reader.Read();
				EnableBadging = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "ISFEATURED":
				reader.Read();
				IsFeatured = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "SPECIALEVENTID":
				reader.Read();
				SpecialEventID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCSTARTDATE":
				reader.Read();
				UTCStartDate = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCENDDATE":
				reader.Read();
				UTCEndDate = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PERCENTOFF":
				reader.Read();
				PercentOff = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PLATFORM":
				reader.Read();
				Platform = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "COUNTRIES":
				reader.Read();
				Countries = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadString, Countries);
				break;
			case "PRIORITYDEFINITION":
				reader.Read();
				PriorityDefinition = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public bool IsOnSale(global::UnityEngine.RuntimePlatform rp, global::Kampai.Game.ITimeService timeService, global::Kampai.Main.ILocalizationService localeService, global::Kampai.Util.IKampaiLogger logger)
		{
			bool flag = false;
			if (timeService.WithinRange(this, true))
			{
				flag = true;
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				if (!string.IsNullOrEmpty(Platform))
				{
					flag2 = true;
					string text = Platform.Trim().ToLower();
					string value = global::Kampai.Util.StringUtil.UnifiedPlatformName(rp);
					if (string.IsNullOrEmpty(value))
					{
						logger.Error("Unknown platform {0}", rp.ToString());
						return false;
					}
					flag3 = text.Equals(global::Kampai.Util.StringUtil.UnifiedPlatformName(rp).ToLower());
				}
				global::System.Collections.Generic.List<string> countries = Countries;
				if (countries != null && countries.Count > 0)
				{
					flag4 = true;
					flag5 = global::Kampai.Util.ListUtil.StringIsInList(localeService.GetCountry(), countries);
				}
				if (flag4 && flag2)
				{
					flag = flag5 && flag3;
				}
				else if (flag4)
				{
					flag = flag5;
				}
				else if (flag2)
				{
					flag = flag3;
				}
			}
			return flag;
		}
	}
}
