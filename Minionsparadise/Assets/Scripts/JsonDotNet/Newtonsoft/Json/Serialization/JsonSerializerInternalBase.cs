namespace Newtonsoft.Json.Serialization
{
	internal abstract class JsonSerializerInternalBase
	{
		private class ReferenceEqualsEqualityComparer : global::System.Collections.Generic.IEqualityComparer<object>
		{
			bool global::System.Collections.Generic.IEqualityComparer<object>.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			int global::System.Collections.Generic.IEqualityComparer<object>.GetHashCode(object obj)
			{
				return global::System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
			}
		}

		private global::Newtonsoft.Json.Serialization.ErrorContext _currentErrorContext;

		private global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, object> _mappings;

		internal global::Newtonsoft.Json.JsonSerializer Serializer { get; private set; }

		internal global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, object> DefaultReferenceMappings
		{
			get
			{
				if (_mappings == null)
				{
					_mappings = new global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, object>(global::System.Collections.Generic.EqualityComparer<string>.Default, new global::Newtonsoft.Json.Serialization.JsonSerializerInternalBase.ReferenceEqualsEqualityComparer());
				}
				return _mappings;
			}
		}

		protected JsonSerializerInternalBase(global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(serializer, "serializer");
			Serializer = serializer;
		}

		protected global::Newtonsoft.Json.Serialization.ErrorContext GetErrorContext(object currentObject, object member, global::System.Exception error)
		{
			if (_currentErrorContext == null)
			{
				_currentErrorContext = new global::Newtonsoft.Json.Serialization.ErrorContext(currentObject, member, error);
			}
			if (_currentErrorContext.Error != error)
			{
				throw new global::System.InvalidOperationException("Current error context error is different to requested error.");
			}
			return _currentErrorContext;
		}

		protected void ClearErrorContext()
		{
			if (_currentErrorContext == null)
			{
				throw new global::System.InvalidOperationException("Could not clear error context. Error context is already null.");
			}
			_currentErrorContext = null;
		}

		protected bool IsErrorHandled(object currentObject, global::Newtonsoft.Json.Serialization.JsonContract contract, object keyValue, global::System.Exception ex)
		{
			global::Newtonsoft.Json.Serialization.ErrorContext errorContext = GetErrorContext(currentObject, keyValue, ex);
			contract.InvokeOnError(currentObject, Serializer.Context, errorContext);
			if (!errorContext.Handled)
			{
				Serializer.OnError(new global::Newtonsoft.Json.Serialization.ErrorEventArgs(currentObject, errorContext));
			}
			return errorContext.Handled;
		}
	}
}
