namespace Newtonsoft.Json.Serialization
{
	internal struct ResolverContractKey : global::System.IEquatable<global::Newtonsoft.Json.Serialization.ResolverContractKey>
	{
		private readonly global::System.Type _resolverType;

		private readonly global::System.Type _contractType;

		public ResolverContractKey(global::System.Type resolverType, global::System.Type contractType)
		{
			_resolverType = resolverType;
			_contractType = contractType;
		}

		public override int GetHashCode()
		{
			return _resolverType.GetHashCode() ^ _contractType.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is global::Newtonsoft.Json.Serialization.ResolverContractKey))
			{
				return false;
			}
			return Equals((global::Newtonsoft.Json.Serialization.ResolverContractKey)obj);
		}

		public bool Equals(global::Newtonsoft.Json.Serialization.ResolverContractKey other)
		{
			if (_resolverType == other._resolverType)
			{
				return _contractType == other._contractType;
			}
			return false;
		}
	}
}
