namespace Kampai.Game.Trigger
{
	[global::System.Flags]
	public enum TriggerConditionOperator
	{
		Invalid = 1,
		Equal = 2,
		NotEqual = 4,
		Greater = 8,
		Less = 0x10,
		LessEqual = 0x12,
		GreaterEqual = 0xA
	}
}
