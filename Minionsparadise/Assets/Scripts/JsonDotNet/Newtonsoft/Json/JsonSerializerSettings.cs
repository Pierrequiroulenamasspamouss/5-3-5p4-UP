namespace Newtonsoft.Json
{
	public class JsonSerializerSettings
	{
		internal const global::Newtonsoft.Json.ReferenceLoopHandling DefaultReferenceLoopHandling = global::Newtonsoft.Json.ReferenceLoopHandling.Error;

		internal const global::Newtonsoft.Json.MissingMemberHandling DefaultMissingMemberHandling = global::Newtonsoft.Json.MissingMemberHandling.Ignore;

		internal const global::Newtonsoft.Json.NullValueHandling DefaultNullValueHandling = global::Newtonsoft.Json.NullValueHandling.Include;

		internal const global::Newtonsoft.Json.DefaultValueHandling DefaultDefaultValueHandling = global::Newtonsoft.Json.DefaultValueHandling.Include;

		internal const global::Newtonsoft.Json.ObjectCreationHandling DefaultObjectCreationHandling = global::Newtonsoft.Json.ObjectCreationHandling.Auto;

		internal const global::Newtonsoft.Json.PreserveReferencesHandling DefaultPreserveReferencesHandling = global::Newtonsoft.Json.PreserveReferencesHandling.None;

		internal const global::Newtonsoft.Json.ConstructorHandling DefaultConstructorHandling = global::Newtonsoft.Json.ConstructorHandling.Default;

		internal const global::Newtonsoft.Json.TypeNameHandling DefaultTypeNameHandling = global::Newtonsoft.Json.TypeNameHandling.None;

		internal const global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle DefaultTypeNameAssemblyFormat = global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;

		internal static readonly global::System.Runtime.Serialization.StreamingContext DefaultContext = default(global::System.Runtime.Serialization.StreamingContext);

		public global::Newtonsoft.Json.ReferenceLoopHandling ReferenceLoopHandling { get; set; }

		public global::Newtonsoft.Json.MissingMemberHandling MissingMemberHandling { get; set; }

		public global::Newtonsoft.Json.ObjectCreationHandling ObjectCreationHandling { get; set; }

		public global::Newtonsoft.Json.NullValueHandling NullValueHandling { get; set; }

		public global::Newtonsoft.Json.DefaultValueHandling DefaultValueHandling { get; set; }

		public global::System.Collections.Generic.IList<global::Newtonsoft.Json.JsonConverter> Converters { get; set; }

		public global::Newtonsoft.Json.PreserveReferencesHandling PreserveReferencesHandling { get; set; }

		public global::Newtonsoft.Json.TypeNameHandling TypeNameHandling { get; set; }

		public global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle TypeNameAssemblyFormat { get; set; }

		public global::Newtonsoft.Json.ConstructorHandling ConstructorHandling { get; set; }

		public global::Newtonsoft.Json.Serialization.IContractResolver ContractResolver { get; set; }

		public global::Newtonsoft.Json.Serialization.IReferenceResolver ReferenceResolver { get; set; }

		public global::System.Runtime.Serialization.SerializationBinder Binder { get; set; }

		public global::System.EventHandler<global::Newtonsoft.Json.Serialization.ErrorEventArgs> Error { get; set; }

		public global::System.Runtime.Serialization.StreamingContext Context { get; set; }

		public JsonSerializerSettings()
		{
			ReferenceLoopHandling = global::Newtonsoft.Json.ReferenceLoopHandling.Error;
			MissingMemberHandling = global::Newtonsoft.Json.MissingMemberHandling.Ignore;
			ObjectCreationHandling = global::Newtonsoft.Json.ObjectCreationHandling.Auto;
			NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Include;
			DefaultValueHandling = global::Newtonsoft.Json.DefaultValueHandling.Include;
			PreserveReferencesHandling = global::Newtonsoft.Json.PreserveReferencesHandling.None;
			TypeNameHandling = global::Newtonsoft.Json.TypeNameHandling.None;
			TypeNameAssemblyFormat = global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
			Context = DefaultContext;
			Converters = new global::System.Collections.Generic.List<global::Newtonsoft.Json.JsonConverter>();
		}
	}
}
