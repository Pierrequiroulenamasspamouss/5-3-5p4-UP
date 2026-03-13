namespace Kampai.Game
{
	public class FBUser : global::Kampai.Game.SocialUser
	{
		public string facebookID { get; set; }

		public FBUser(string name, string id)
		{
			base.name = name;
			facebookID = id;
		}
	}
}
