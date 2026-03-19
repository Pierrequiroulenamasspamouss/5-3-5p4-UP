namespace Discord.Unity
{
	internal class PayResult : global::Discord.Unity.ResultBase, global::Discord.Unity.IPayResult, global::Discord.Unity.IResult
	{
		internal const long CancelPaymentFlowCode = 1383010L;

		public long ErrorCode
		{
			get
			{
				return base.CanvasErrorCode.GetValueOrDefault();
			}
		}

		internal PayResult(global::Discord.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (base.CanvasErrorCode.HasValue && base.CanvasErrorCode.Value == 1383010)
			{
				Cancelled = true;
			}
		}

		public override string ToString()
		{
			return global::Discord.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string> { 
			{
				"ErrorCode",
				ErrorCode.ToString()
			} });
		}
	}
}
