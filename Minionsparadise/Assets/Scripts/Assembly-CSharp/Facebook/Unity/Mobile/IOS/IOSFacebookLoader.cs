namespace Facebook.Unity.Mobile.IOS
{
	internal class IOSFacebookLoader : global::Facebook.Unity.FB.CompiledFacebookLoader
	{
		protected override global::Facebook.Unity.FacebookGameObject FBGameObject
		{
			get
			{
				global::Facebook.Unity.Mobile.IOS.IOSFacebookGameObject component = global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Mobile.IOS.IOSFacebookGameObject>();
				if (component.Facebook == null)
				{
					component.Facebook = new global::Facebook.Unity.Mobile.IOS.IOSFacebook();
				}
				return component;
			}
		}
	}
}
