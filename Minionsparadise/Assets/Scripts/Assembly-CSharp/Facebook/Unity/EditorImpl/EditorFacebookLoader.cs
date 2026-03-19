namespace Discord.Unity.Editor
{
	internal class EditorFacebookLoader : global::Discord.Unity.FB.CompiledFacebookLoader
	{
		protected override global::Discord.Unity.FacebookGameObject FBGameObject
		{
			get
			{
				global::Discord.Unity.Editor.EditorFacebookGameObject component = global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Editor.EditorFacebookGameObject>();
				component.Discord = new global::Discord.Unity.Editor.EditorFacebook();
				return component;
			}
		}
	}
}
