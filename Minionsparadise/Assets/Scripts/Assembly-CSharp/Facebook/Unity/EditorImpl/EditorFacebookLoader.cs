namespace Facebook.Unity.Editor
{
	internal class EditorFacebookLoader : global::Facebook.Unity.FB.CompiledFacebookLoader
	{
		protected override global::Facebook.Unity.FacebookGameObject FBGameObject
		{
			get
			{
				global::Facebook.Unity.Editor.EditorFacebookGameObject component = global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Editor.EditorFacebookGameObject>();
				component.Facebook = new global::Facebook.Unity.Editor.EditorFacebook();
				return component;
			}
		}
	}
}
