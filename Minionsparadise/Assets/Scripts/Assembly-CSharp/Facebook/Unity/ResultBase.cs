namespace Discord.Unity
{
	internal abstract class ResultBase : global::Discord.Unity.IInternalResult, global::Discord.Unity.IResult
	{
		internal const long CancelDialogCode = 4201L;

		internal const string ErrorCodeKey = "error_code";

		internal const string ErrorMessageKey = "error_message";

		public virtual string Error { get; protected set; }

		public virtual global::System.Collections.Generic.IDictionary<string, object> ResultDictionary { get; protected set; }

		public virtual string RawResult { get; protected set; }

		public virtual bool Cancelled { get; protected set; }

		public virtual string CallbackId { get; protected set; }

		protected long? CanvasErrorCode { get; private set; }

		internal ResultBase(global::Discord.Unity.ResultContainer result)
		{
			string errorValue = GetErrorValue(result.ResultDictionary);
			bool cancelledValue = GetCancelledValue(result.ResultDictionary);
			string callbackId = GetCallbackId(result.ResultDictionary);
			Init(result, errorValue, cancelledValue, callbackId);
		}

		internal ResultBase(global::Discord.Unity.ResultContainer result, string error, bool cancelled)
		{
			Init(result, error, cancelled, null);
		}

		public override string ToString()
		{
			return global::Discord.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string>
			{
				{ "Error", Error },
				{ "RawResult", RawResult },
				{
					"Cancelled",
					Cancelled.ToString()
				}
			});
		}

		protected void Init(global::Discord.Unity.ResultContainer result, string error, bool cancelled, string callbackId)
		{
			RawResult = result.RawResult;
			ResultDictionary = result.ResultDictionary;
			Cancelled = cancelled;
			Error = error;
			CallbackId = callbackId;
			if (ResultDictionary == null)
			{
				return;
			}
			long value;
			if (ResultDictionary.TryGetValue<long>("error_code", out value))
			{
				CanvasErrorCode = value;
				if (value == 4201)
				{
					Cancelled = true;
				}
			}
			string value2;
			if (ResultDictionary.TryGetValue<string>("error_message", out value2))
			{
				Error = value2;
			}
		}

		private static string GetErrorValue(global::System.Collections.Generic.IDictionary<string, object> result)
		{
			if (result == null)
			{
				return null;
			}
			string value;
			if (result.TryGetValue<string>("error", out value))
			{
				return value;
			}
			return null;
		}

		private static bool GetCancelledValue(global::System.Collections.Generic.IDictionary<string, object> result)
		{
			if (result == null)
			{
				return false;
			}
			object value;
			if (result.TryGetValue("cancelled", out value))
			{
				bool? flag = value as bool?;
				if (flag.HasValue)
				{
					return flag.HasValue && flag.Value;
				}
				string text = value as string;
				if (text != null)
				{
					return global::System.Convert.ToBoolean(text);
				}
				int? num = value as int?;
				if (num.HasValue)
				{
					return num.HasValue && num.Value != 0;
				}
			}
			return false;
		}

		private static string GetCallbackId(global::System.Collections.Generic.IDictionary<string, object> result)
		{
			if (result == null)
			{
				return null;
			}
			string value;
			if (result.TryGetValue<string>("callback_id", out value))
			{
				return value;
			}
			return null;
		}
	}
}
