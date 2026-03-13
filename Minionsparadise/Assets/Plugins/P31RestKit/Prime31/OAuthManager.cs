namespace Prime31
{
	public class OAuthManager
	{
		private static readonly global::System.DateTime _epoch = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0);

		private global::System.Collections.Generic.SortedDictionary<string, string> _params;

		private global::System.Random _random;

		private static string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

		public string this[string ix]
		{
			get
			{
				if (_params.ContainsKey(ix))
				{
					return _params[ix];
				}
				throw new global::System.ArgumentException(ix);
			}
			set
			{
				if (!_params.ContainsKey(ix))
				{
					throw new global::System.ArgumentException(ix);
				}
				_params[ix] = value;
			}
		}

		public OAuthManager()
		{
			_random = new global::System.Random();
			_params = new global::System.Collections.Generic.SortedDictionary<string, string>();
			_params["consumer_key"] = "";
			_params["consumer_secret"] = "";
			_params["timestamp"] = generateTimeStamp();
			_params["nonce"] = generateNonce();
			_params["signature_method"] = "HMAC-SHA1";
			_params["signature"] = "";
			_params["token"] = "";
			_params["token_secret"] = "";
			_params["version"] = "1.0";
		}

		public OAuthManager(string consumerKey, string consumerSecret, string token, string tokenSecret)
			: this()
		{
			_params["consumer_key"] = consumerKey;
			_params["consumer_secret"] = consumerSecret;
			_params["token"] = token;
			_params["token_secret"] = tokenSecret;
		}

		private string generateTimeStamp()
		{
			return global::System.Convert.ToInt64((global::System.DateTime.UtcNow - _epoch).TotalSeconds).ToString();
		}

		private void prepareNewRequest()
		{
			_params["nonce"] = generateNonce();
			_params["timestamp"] = generateTimeStamp();
		}

		private string generateNonce()
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			for (int i = 0; i < 8; i++)
			{
				if (_random.Next(3) == 0)
				{
					stringBuilder.Append((char)(_random.Next(26) + 97), 1);
				}
				else
				{
					stringBuilder.Append((char)(_random.Next(10) + 48), 1);
				}
			}
			return stringBuilder.ToString();
		}

		private global::System.Collections.Generic.SortedDictionary<string, string> extractQueryParameters(string queryString)
		{
			if (queryString.StartsWith("?"))
			{
				queryString = queryString.Remove(0, 1);
			}
			global::System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new global::System.Collections.Generic.SortedDictionary<string, string>();
			if (string.IsNullOrEmpty(queryString))
			{
				return sortedDictionary;
			}
			string[] array = queryString.Split('&');
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("oauth_"))
				{
					if (text.IndexOf('=') > -1)
					{
						string[] array2 = text.Split('=');
						sortedDictionary.Add(array2[0], array2[1]);
					}
					else
					{
						sortedDictionary.Add(text, string.Empty);
					}
				}
			}
			return sortedDictionary;
		}

		public static string urlEncode(string value)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			foreach (char c in value)
			{
				if (unreservedChars.IndexOf(c) != -1)
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append('%' + string.Format("{0:X2}", (int)c));
				}
			}
			return stringBuilder.ToString();
		}

		private static global::System.Collections.Generic.SortedDictionary<string, string> mergePostParamsWithOauthParams(global::System.Collections.Generic.SortedDictionary<string, string> postParams, global::System.Collections.Generic.SortedDictionary<string, string> oAuthParams)
		{
			global::System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new global::System.Collections.Generic.SortedDictionary<string, string>();
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> postParam in postParams)
			{
				sortedDictionary.Add(postParam.Key, postParam.Value);
			}
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> oAuthParam in oAuthParams)
			{
				if (!string.IsNullOrEmpty(oAuthParam.Value) && !oAuthParam.Key.EndsWith("secret"))
				{
					sortedDictionary.Add("oauth_" + oAuthParam.Key, oAuthParam.Value);
				}
			}
			return sortedDictionary;
		}

		private static string encodeRequestParameters(global::System.Collections.Generic.SortedDictionary<string, string> p)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in p)
			{
				if (!string.IsNullOrEmpty(item.Value) && !item.Key.EndsWith("secret"))
				{
					stringBuilder.AppendFormat("oauth_{0}=\"{1}\", ", item.Key, urlEncode(item.Value));
				}
			}
			return stringBuilder.ToString().TrimEnd(' ').TrimEnd(',');
		}

		public static byte[] encodePostParameters(global::System.Collections.Generic.SortedDictionary<string, string> p)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in p)
			{
				if (!string.IsNullOrEmpty(item.Value))
				{
					stringBuilder.AppendFormat("{0}={1}, ", urlEncode(item.Key), urlEncode(item.Value));
				}
			}
			return global::System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString().TrimEnd(' ').TrimEnd(','));
		}

		public global::Prime31.OAuthResponse acquireRequestToken(string uri, string method)
		{
			prepareNewRequest();
			string authorizationHeader = getAuthorizationHeader(uri, method);
			global::System.Net.HttpWebRequest httpWebRequest = (global::System.Net.HttpWebRequest)global::System.Net.WebRequest.Create(uri);
			httpWebRequest.Headers.Add("Authorization", authorizationHeader);
			httpWebRequest.Method = method;
			using (global::System.Net.HttpWebResponse httpWebResponse = (global::System.Net.HttpWebResponse)httpWebRequest.GetResponse())
			{
				using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(httpWebResponse.GetResponseStream()))
				{
					global::Prime31.OAuthResponse oAuthResponse = new global::Prime31.OAuthResponse(streamReader.ReadToEnd());
					this["token"] = oAuthResponse["oauth_token"];
					try
					{
						if (oAuthResponse["oauth_token_secret"] != null)
						{
							this["token_secret"] = oAuthResponse["oauth_token_secret"];
						}
					}
					catch
					{
					}
					return oAuthResponse;
				}
			}
		}

		public global::Prime31.OAuthResponse acquireAccessToken(string uri, string method, string verifier)
		{
			prepareNewRequest();
			_params["verifier"] = verifier;
			string authorizationHeader = getAuthorizationHeader(uri, method);
			global::System.Net.HttpWebRequest httpWebRequest = (global::System.Net.HttpWebRequest)global::System.Net.WebRequest.Create(uri);
			httpWebRequest.Headers.Add("Authorization", authorizationHeader);
			httpWebRequest.Method = method;
			using (global::System.Net.HttpWebResponse httpWebResponse = (global::System.Net.HttpWebResponse)httpWebRequest.GetResponse())
			{
				using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(httpWebResponse.GetResponseStream()))
				{
					global::Prime31.OAuthResponse oAuthResponse = new global::Prime31.OAuthResponse(streamReader.ReadToEnd());
					this["token"] = oAuthResponse["oauth_token"];
					this["token_secret"] = oAuthResponse["oauth_token_secret"];
					return oAuthResponse;
				}
			}
		}

		public string generateCredsHeader(string uri, string method, string realm)
		{
			prepareNewRequest();
			return getAuthorizationHeader(uri, method, realm);
		}

		public string generateAuthzHeader(string uri, string method)
		{
			prepareNewRequest();
			return getAuthorizationHeader(uri, method, null);
		}

		private string getAuthorizationHeader(string uri, string method)
		{
			return getAuthorizationHeader(uri, method, null);
		}

		private string getAuthorizationHeader(string uri, string method, string realm)
		{
			if (string.IsNullOrEmpty(_params["consumer_key"]))
			{
				throw new global::System.ArgumentNullException("consumer_key");
			}
			if (string.IsNullOrEmpty(_params["signature_method"]))
			{
				throw new global::System.ArgumentNullException("signature_method");
			}
			sign(uri, method);
			string text = encodeRequestParameters(_params);
			return (!string.IsNullOrEmpty(realm)) ? (string.Format("OAuth realm=\"{0}\", ", realm) + text) : ("OAuth " + text);
		}

		private void sign(string uri, string method)
		{
			string signatureBase = getSignatureBase(uri, method);
			global::System.Security.Cryptography.HashAlgorithm hash = getHash();
			byte[] bytes = global::System.Text.Encoding.ASCII.GetBytes(signatureBase);
			byte[] inArray = hash.ComputeHash(bytes);
			this["signature"] = global::System.Convert.ToBase64String(inArray);
		}

		private string getSignatureBase(string url, string method)
		{
			global::System.Uri uri = new global::System.Uri(url);
			string text = string.Format("{0}://{1}", uri.Scheme, uri.Host);
			if ((!(uri.Scheme == "http") || uri.Port != 80) && (!(uri.Scheme == "https") || uri.Port != 443))
			{
				text = text + ":" + uri.Port;
			}
			text += uri.AbsolutePath;
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			stringBuilder.Append(method).Append('&').Append(urlEncode(text))
				.Append('&');
			global::System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = extractQueryParameters(uri.Query);
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in _params)
			{
				if (!string.IsNullOrEmpty(_params[item.Key]) && !item.Key.EndsWith("_secret") && !item.Key.EndsWith("signature"))
				{
					sortedDictionary.Add("oauth_" + item.Key, item.Value);
				}
			}
			global::System.Text.StringBuilder stringBuilder2 = new global::System.Text.StringBuilder();
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> item2 in sortedDictionary)
			{
				stringBuilder2.AppendFormat("{0}={1}&", item2.Key, item2.Value);
			}
			stringBuilder.Append(urlEncode(stringBuilder2.ToString().TrimEnd('&')));
			return stringBuilder.ToString();
		}

		private global::System.Security.Cryptography.HashAlgorithm getHash()
		{
			if (this["signature_method"] != "HMAC-SHA1")
			{
				throw new global::System.NotImplementedException();
			}
			string s = string.Format("{0}&{1}", urlEncode(this["consumer_secret"]), urlEncode(this["token_secret"]));
			global::System.Security.Cryptography.HMACSHA1 hMACSHA = new global::System.Security.Cryptography.HMACSHA1();
			hMACSHA.Key = global::System.Text.Encoding.ASCII.GetBytes(s);
			return hMACSHA;
		}
	}
}
