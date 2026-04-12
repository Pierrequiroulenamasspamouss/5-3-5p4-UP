namespace Kampai.Game.Trigger
{
	public interface IIsTriggerable
	{
		bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext);
	}
}
