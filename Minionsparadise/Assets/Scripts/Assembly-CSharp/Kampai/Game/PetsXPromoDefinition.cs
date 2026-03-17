namespace Kampai.Game
{
	public class PetsXPromoDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1189;
			}
		}

		public bool PetsXPromoEnabled { get; set; }

		public int PetsXPromoSurfaceLevel { get; set; }

		public string IOSPetsSmartURL { get; set; }

		public string AndroidPetsSmartURL { get; set; }

		public string IOSInstallURL { get; set; }

		public string AndroidInstallURL { get; set; }

		public string PetsImageEN_US { get; set; }

		public string PetsImageFR_FR { get; set; }

		public string PetsImageDE_DE { get; set; }

		public string PetsImageES_ES { get; set; }

		public string PetsImageES_PR { get; set; }

		public string PetsImageID { get; set; }

		public string PetsImagePT_BR { get; set; }

		public string PetsImageNL_NL { get; set; }

		public string PetsImageRU_RU { get; set; }

		public string PetsImageIT_IT { get; set; }

		public string PetsImageJA { get; set; }

		public string PetsImageKO_KR { get; set; }

		public string PetsImageTR { get; set; }

		public string PetsImageZH_CN { get; set; }

		public string PetsImageZH_TW { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(PetsXPromoEnabled);
			writer.Write(PetsXPromoSurfaceLevel);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, IOSPetsSmartURL);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AndroidPetsSmartURL);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, IOSInstallURL);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AndroidInstallURL);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageEN_US);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageFR_FR);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageDE_DE);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageES_ES);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageES_PR);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageID);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImagePT_BR);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageNL_NL);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageRU_RU);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageIT_IT);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageJA);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageKO_KR);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageTR);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageZH_CN);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, PetsImageZH_TW);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			PetsXPromoEnabled = reader.ReadBoolean();
			PetsXPromoSurfaceLevel = reader.ReadInt32();
			IOSPetsSmartURL = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			AndroidPetsSmartURL = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			IOSInstallURL = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			AndroidInstallURL = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageEN_US = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageFR_FR = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageDE_DE = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageES_ES = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageES_PR = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageID = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImagePT_BR = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageNL_NL = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageRU_RU = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageIT_IT = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageJA = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageKO_KR = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageTR = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageZH_CN = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PetsImageZH_TW = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PETSXPROMOENABLED":
				reader.Read();
				PetsXPromoEnabled = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "PETSXPROMOSURFACELEVEL":
				reader.Read();
				PetsXPromoSurfaceLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "IOSPETSSMARTURL":
				reader.Read();
				IOSPetsSmartURL = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ANDROIDPETSSMARTURL":
				reader.Read();
				AndroidPetsSmartURL = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "IOSINSTALLURL":
				reader.Read();
				IOSInstallURL = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ANDROIDINSTALLURL":
				reader.Read();
				AndroidInstallURL = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEEN_US":
				reader.Read();
				PetsImageEN_US = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEFR_FR":
				reader.Read();
				PetsImageFR_FR = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEDE_DE":
				reader.Read();
				PetsImageDE_DE = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEES_ES":
				reader.Read();
				PetsImageES_ES = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEES_PR":
				reader.Read();
				PetsImageES_PR = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEID":
				reader.Read();
				PetsImageID = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEPT_BR":
				reader.Read();
				PetsImagePT_BR = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGENL_NL":
				reader.Read();
				PetsImageNL_NL = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGERU_RU":
				reader.Read();
				PetsImageRU_RU = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEIT_IT":
				reader.Read();
				PetsImageIT_IT = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEJA":
				reader.Read();
				PetsImageJA = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEKO_KR":
				reader.Read();
				PetsImageKO_KR = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGETR":
				reader.Read();
				PetsImageTR = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEZH_CN":
				reader.Read();
				PetsImageZH_CN = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PETSIMAGEZH_TW":
				reader.Read();
				PetsImageZH_TW = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
