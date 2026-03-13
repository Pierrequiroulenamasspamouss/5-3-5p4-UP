namespace Facebook.Unity
{
	internal class PayResult : global::Facebook.Unity.ResultBase, global::Facebook.Unity.IPayResult, global::Facebook.Unity.IResult
	{
		internal const long CancelPaymentFlowCode = 1383010L;

		public long ErrorCode
		{
			get
			{
				return base.CanvasErrorCode.GetValueOrDefault();
			}
		}

		internal PayResult(global::Facebook.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (base.CanvasErrorCode.HasValue && base.CanvasErrorCode.Value == 1383010)
			{
				Cancelled = true;
			}
		}

		public override string ToString()
		{
			return global::Facebook.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string> { 
			{
				"ErrorCode",
				ErrorCode.ToString()
			} });
		}
	}
}
