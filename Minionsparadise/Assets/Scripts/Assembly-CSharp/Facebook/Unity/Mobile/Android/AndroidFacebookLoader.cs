namespace Discord.Unity.Mobile.Android
{
	internal class AndroidFacebookLoader : global::Discord.Unity.FB.CompiledFacebookLoader
	{
		protected override global::Discord.Unity.FacebookGameObject FBGameObject
		{
			get
			{
				global::Discord.Unity.Mobile.Android.AndroidFacebookGameObject component = global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Mobile.Android.AndroidFacebookGameObject>();
				if (component.Discord == null)
				{
					component.Discord = new global::Discord.Unity.Mobile.Android.AndroidFacebook();
				}
				return component;
			}
		}
	}
}
