namespace Kampai.Util
{
	public class PetsLogoLocalizer : global::Kampai.Util.KampaiView
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		protected override void Start()
		{
			base.Start();
			string textureName = GetTextureName(global::Kampai.Util.Native.GetDeviceLanguage().ToLower());
			global::UnityEngine.Texture texture = global::Kampai.Util.KampaiResources.Load(textureName) as global::UnityEngine.Texture;
			global::UnityEngine.UI.RawImage component = GetComponent<global::UnityEngine.UI.RawImage>();
			if (component != null && texture != null)
			{
				component.texture = texture;
			}
		}

		private string GetTextureName(string languageCode)
		{
			global::Kampai.Game.PetsXPromoDefinition petsXPromoDefinition = definitionService.Get<global::Kampai.Game.PetsXPromoDefinition>(95000);
			string text = languageCode;
			if (languageCode.Contains("_"))
			{
				text = languageCode.Split('_')[0];
			}
			else if (languageCode.Contains("-"))
			{
				text = languageCode.Split('-')[0];
			}
			if (text.Equals("en"))
			{
				return petsXPromoDefinition.PetsImageEN_US;
			}
			if (text.Equals("fr"))
			{
				return petsXPromoDefinition.PetsImageFR_FR;
			}
			if (text.Equals("de"))
			{
				return petsXPromoDefinition.PetsImageDE_DE;
			}
			if (text.Equals("es"))
			{
				return petsXPromoDefinition.PetsImageES_ES;
			}
			if (text.Equals("it"))
			{
				return petsXPromoDefinition.PetsImageIT_IT;
			}
			if (text.Equals("pt"))
			{
				return petsXPromoDefinition.PetsImagePT_BR;
			}
			if (text.Equals("nl"))
			{
				return petsXPromoDefinition.PetsImageNL_NL;
			}
			if (text.Equals("ko"))
			{
				return petsXPromoDefinition.PetsImageKO_KR;
			}
			if (text.Equals("ru"))
			{
				return petsXPromoDefinition.PetsImageRU_RU;
			}
			if (text.Equals("ja"))
			{
				return petsXPromoDefinition.PetsImageJA;
			}
			if (languageCode.Equals("zh-hans") || languageCode.Equals("zh_hans") || languageCode.Equals("zh_cn") || languageCode.Equals("zh-cn"))
			{
				return petsXPromoDefinition.PetsImageZH_CN;
			}
			if (languageCode.Equals("zh-hant") || languageCode.Equals("zh_hant") || languageCode.Equals("zh_tw") || languageCode.Equals("zh-tw") || languageCode.Equals("zh_hk") || languageCode.Equals("zh-hk"))
			{
				return petsXPromoDefinition.PetsImageZH_TW;
			}
			if (text.Equals("zh"))
			{
				return petsXPromoDefinition.PetsImageZH_CN;
			}
			if (text.Equals("tr"))
			{
				return petsXPromoDefinition.PetsImageTR;
			}
			if (text.Equals("id") || languageCode.Equals("in_id"))
			{
				return petsXPromoDefinition.PetsImageID;
			}
			return petsXPromoDefinition.PetsImageEN_US;
		}
	}
}
