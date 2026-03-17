namespace Facebook.Unity.Mobile.Android
{
	internal class AndroidFacebookLoader : global::Facebook.Unity.FB.CompiledFacebookLoader
	{
		protected override global::Facebook.Unity.FacebookGameObject FBGameObject
		{
			get
			{
				global::Facebook.Unity.Mobile.Android.AndroidFacebookGameObject component = global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Mobile.Android.AndroidFacebookGameObject>();
				if (component.Facebook == null)
				{
					component.Facebook = new global::Facebook.Unity.Mobile.Android.AndroidFacebook();
				}
				return component;
			}
		}
	}
}
