namespace Newtonsoft.Json.Linq
{
	public class JTokenEqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Newtonsoft.Json.Linq.JToken>
	{
		public bool Equals(global::Newtonsoft.Json.Linq.JToken x, global::Newtonsoft.Json.Linq.JToken y)
		{
			return global::Newtonsoft.Json.Linq.JToken.DeepEquals(x, y);
		}

		public int GetHashCode(global::Newtonsoft.Json.Linq.JToken obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetDeepHashCode();
		}
	}
}
