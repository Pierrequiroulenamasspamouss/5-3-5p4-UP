namespace Kampai.UI.View
{
	public static class BuddyAvatarBuilder
	{
		public static global::Kampai.UI.View.BuddyAvatarView Build(global::Kampai.Game.Prestige prestige, global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Util.IKampaiLogger logger)
		{
			if (prestige == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.PS_NO_SUCH_PRESTIGE);
				return null;
			}
			global::Kampai.Game.PrestigeDefinition definition = prestige.Definition;
			if (definition == null)
			{
				return null;
			}
			global::UnityEngine.Sprite characterImage;
			global::UnityEngine.Sprite characterMask;
			prestigeService.GetCharacterImageBasedOnMood(definition, global::Kampai.Game.CharacterImageType.SmallAvatarIcon, out characterImage, out characterMask);
			if (characterImage == null)
			{
				return null;
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("cmp_BuddyBarAvatar");
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.UI.View.BuddyAvatarView component = gameObject.GetComponent<global::Kampai.UI.View.BuddyAvatarView>();
			component.AvatarImage.sprite = characterImage;
			component.AvatarImage.maskSprite = characterMask;
			component.AvatarNameText.LocKey = definition.LocalizedKey;
			component.ProgressBarText.text = string.Format("{0}/{1}", prestige.CurrentPrestigePoints, prestige.NeededPrestigePoints);
			int index = 0;
			if (prestige.CurrentPrestigeLevel >= 0 && prestige.CurrentPrestigeLevel < definition.PrestigeLevelSettings.Count)
			{
				index = prestige.CurrentPrestigeLevel;
			}
			global::Kampai.Game.CharacterPrestigeLevelDefinition characterPrestigeLevelDefinition = definition.PrestigeLevelSettings[index];
			float x = (float)prestige.CurrentPrestigePoints / (float)characterPrestigeLevelDefinition.PointsNeeded;
			component.ProgressBarFill.anchorMax = new global::UnityEngine.Vector2(x, 1f);
			return component;
		}
	}
}
