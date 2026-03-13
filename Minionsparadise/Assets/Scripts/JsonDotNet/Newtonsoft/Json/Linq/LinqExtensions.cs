namespace Newtonsoft.Json.Linq
{
	public static class LinqExtensions
	{
		public static global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> Ancestors<T>(this global::System.Collections.Generic.IEnumerable<T> source) where T : global::Newtonsoft.Json.Linq.JToken
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(source, "source");
			return global::System.Linq.Enumerable.SelectMany(source, (T j) => j.Ancestors()).AsJEnumerable();
		}

		public static global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> Descendants<T>(this global::System.Collections.Generic.IEnumerable<T> source) where T : global::Newtonsoft.Json.Linq.JContainer
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(source, "source");
			return global::System.Linq.Enumerable.SelectMany(source, (T j) => j.Descendants()).AsJEnumerable();
		}

		public static global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JProperty> Properties(this global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JObject> source)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(source, "source");
			return global::System.Linq.Enumerable.SelectMany(source, (global::Newtonsoft.Json.Linq.JObject d) => d.Properties()).AsJEnumerable();
		}

		public static global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> Values(this global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> source, object key)
		{
			return source.Values<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken>(key).AsJEnumerable();
		}

		public static global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> Values(this global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> source)
		{
			return source.Values(null);
		}

		public static global::System.Collections.Generic.IEnumerable<U> Values<U>(this global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> source, object key)
		{
			return source.Values<global::Newtonsoft.Json.Linq.JToken, U>(key);
		}

		public static global::System.Collections.Generic.IEnumerable<U> Values<U>(this global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> source)
		{
			return source.Values<global::Newtonsoft.Json.Linq.JToken, U>(null);
		}

		public static U Value<U>(this global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> value)
		{
			return value.Value<global::Newtonsoft.Json.Linq.JToken, U>();
		}

		public static U Value<T, U>(this global::System.Collections.Generic.IEnumerable<T> value) where T : global::Newtonsoft.Json.Linq.JToken
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(value, "source");
			global::Newtonsoft.Json.Linq.JToken jToken = value as global::Newtonsoft.Json.Linq.JToken;
			if (jToken == null)
			{
				throw new global::System.ArgumentException("Source value must be a JToken.");
			}
			return jToken.Convert<global::Newtonsoft.Json.Linq.JToken, U>();
		}

		internal static global::System.Collections.Generic.IEnumerable<U> Values<T, U>(this global::System.Collections.Generic.IEnumerable<T> source, object key) where T : global::Newtonsoft.Json.Linq.JToken
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(source, "source");
			foreach (T token in source)
			{
				if (key == null)
				{
					if (token is global::Newtonsoft.Json.Linq.JValue)
					{
						yield return ((global::Newtonsoft.Json.Linq.JValue)(object)token).Convert<global::Newtonsoft.Json.Linq.JValue, U>();
						continue;
					}
					foreach (global::Newtonsoft.Json.Linq.JToken t in token.Children())
					{
						yield return t.Convert<global::Newtonsoft.Json.Linq.JToken, U>();
					}
				}
				else
				{
					global::Newtonsoft.Json.Linq.JToken value = token[key];
					if (value != null)
					{
						yield return value.Convert<global::Newtonsoft.Json.Linq.JToken, U>();
					}
				}
			}
		}

		public static global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> Children<T>(this global::System.Collections.Generic.IEnumerable<T> source) where T : global::Newtonsoft.Json.Linq.JToken
		{
			return source.Children<T, global::Newtonsoft.Json.Linq.JToken>().AsJEnumerable();
		}

		public static global::System.Collections.Generic.IEnumerable<U> Children<T, U>(this global::System.Collections.Generic.IEnumerable<T> source) where T : global::Newtonsoft.Json.Linq.JToken
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(source, "source");
			return global::System.Linq.Enumerable.SelectMany(source, (T c) => c.Children()).Convert<global::Newtonsoft.Json.Linq.JToken, U>();
		}

		internal static global::System.Collections.Generic.IEnumerable<U> Convert<T, U>(this global::System.Collections.Generic.IEnumerable<T> source) where T : global::Newtonsoft.Json.Linq.JToken
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(source, "source");
			foreach (T token in source)
			{
				yield return token.Convert<global::Newtonsoft.Json.Linq.JToken, U>();
			}
		}

		internal static U Convert<T, U>(this T token) where T : global::Newtonsoft.Json.Linq.JToken
		{
			if (token == null)
			{
				return default(U);
			}
			if (token is U && typeof(U) != typeof(global::System.IComparable) && typeof(U) != typeof(global::System.IFormattable))
			{
				return (U)(object)token;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = token as global::Newtonsoft.Json.Linq.JValue;
			if (jValue == null)
			{
				throw new global::System.InvalidCastException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot cast {0} to {1}.", global::System.Globalization.CultureInfo.InvariantCulture, token.GetType(), typeof(T)));
			}
			if (jValue.Value is U)
			{
				return (U)jValue.Value;
			}
			global::System.Type type = typeof(U);
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(type))
			{
				if (jValue.Value == null)
				{
					return default(U);
				}
				type = global::System.Nullable.GetUnderlyingType(type);
			}
			return (U)global::System.Convert.ChangeType(jValue.Value, type, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> AsJEnumerable(this global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> source)
		{
			return source.AsJEnumerable<global::Newtonsoft.Json.Linq.JToken>();
		}

		public static global::Newtonsoft.Json.Linq.IJEnumerable<T> AsJEnumerable<T>(this global::System.Collections.Generic.IEnumerable<T> source) where T : global::Newtonsoft.Json.Linq.JToken
		{
			if (source == null)
			{
				return null;
			}
			if (source is global::Newtonsoft.Json.Linq.IJEnumerable<T>)
			{
				return (global::Newtonsoft.Json.Linq.IJEnumerable<T>)source;
			}
			return new global::Newtonsoft.Json.Linq.JEnumerable<T>(source);
		}
	}
}
