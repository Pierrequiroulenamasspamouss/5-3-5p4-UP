namespace Newtonsoft.Json.Serialization
{
	public abstract class JsonContract
	{
		public global::System.Type UnderlyingType { get; private set; }

		public global::System.Type CreatedType { get; set; }

		public bool? IsReference { get; set; }

		public global::Newtonsoft.Json.JsonConverter Converter { get; set; }

		internal global::Newtonsoft.Json.JsonConverter InternalConverter { get; set; }

		public global::System.Reflection.MethodInfo OnDeserialized { get; set; }

		public global::System.Reflection.MethodInfo OnDeserializing { get; set; }

		public global::System.Reflection.MethodInfo OnSerialized { get; set; }

		public global::System.Reflection.MethodInfo OnSerializing { get; set; }

		public global::System.Func<object> DefaultCreator { get; set; }

		public bool DefaultCreatorNonPublic { get; set; }

		public global::System.Reflection.MethodInfo OnError { get; set; }

		internal void InvokeOnSerializing(object o, global::System.Runtime.Serialization.StreamingContext context)
		{
			if (OnSerializing != null)
			{
				OnSerializing.Invoke(o, new object[1] { context });
			}
		}

		internal void InvokeOnSerialized(object o, global::System.Runtime.Serialization.StreamingContext context)
		{
			if (OnSerialized != null)
			{
				OnSerialized.Invoke(o, new object[1] { context });
			}
		}

		internal void InvokeOnDeserializing(object o, global::System.Runtime.Serialization.StreamingContext context)
		{
			if (OnDeserializing != null)
			{
				OnDeserializing.Invoke(o, new object[1] { context });
			}
		}

		internal void InvokeOnDeserialized(object o, global::System.Runtime.Serialization.StreamingContext context)
		{
			if (OnDeserialized != null)
			{
				OnDeserialized.Invoke(o, new object[1] { context });
			}
		}

		internal void InvokeOnError(object o, global::System.Runtime.Serialization.StreamingContext context, global::Newtonsoft.Json.Serialization.ErrorContext errorContext)
		{
			if (OnError != null)
			{
				OnError.Invoke(o, new object[2] { context, errorContext });
			}
		}

		internal JsonContract(global::System.Type underlyingType)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			UnderlyingType = underlyingType;
			CreatedType = underlyingType;
		}
	}
}
