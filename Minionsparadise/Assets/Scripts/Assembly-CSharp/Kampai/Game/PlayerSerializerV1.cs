namespace Kampai.Game
{
	internal class PlayerSerializerV1 : global::Kampai.Game.DefaultPlayerSerializer
	{
		public override int Version
		{
			get
			{
				return 1;
			}
		}
	}
}
