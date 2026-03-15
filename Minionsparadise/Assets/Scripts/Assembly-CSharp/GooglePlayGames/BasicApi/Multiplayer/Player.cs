namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Player : global::GooglePlayGames.PlayGamesUserProfile
	{
		internal Player(string displayName, string playerId, string avatarUrl)
			: base(displayName, playerId, avatarUrl)
		{
		}
	}
}
