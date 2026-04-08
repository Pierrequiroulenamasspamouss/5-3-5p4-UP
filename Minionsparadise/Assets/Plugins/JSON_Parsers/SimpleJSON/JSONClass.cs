namespace SimpleJSON
{
	public class JSONClass : global::SimpleJSON.JSONNode, global::System.Collections.IEnumerable
	{
		private global::System.Collections.Generic.Dictionary<string, global::SimpleJSON.JSONNode> m_Dict = new global::System.Collections.Generic.Dictionary<string, global::SimpleJSON.JSONNode>();

		public override global::SimpleJSON.JSONNode this[string aKey]
		{
			get
			{
				if (m_Dict.ContainsKey(aKey))
				{
					return m_Dict[aKey];
				}
				return new global::SimpleJSON.JSONLazyCreator(this, aKey);
			}
			set
			{
				if (m_Dict.ContainsKey(aKey))
				{
					m_Dict[aKey] = value;
				}
				else
				{
					m_Dict.Add(aKey, value);
				}
			}
		}

		public override global::SimpleJSON.JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= m_Dict.Count)
				{
					return null;
				}
				global::System.Collections.Generic.Dictionary<string, global::SimpleJSON.JSONNode>.Enumerator enumerator = m_Dict.GetEnumerator();
				enumerator.MoveNext();
				for (int i = 0; i < aIndex; i++)
				{
					enumerator.MoveNext();
				}
				return enumerator.Current.Value;
			}
			set
			{
				if (aIndex >= 0 && aIndex < m_Dict.Count)
				{
					global::System.Collections.Generic.Dictionary<string, global::SimpleJSON.JSONNode>.Enumerator enumerator = m_Dict.GetEnumerator();
					enumerator.MoveNext();
					for (int i = 0; i < aIndex; i++)
					{
						enumerator.MoveNext();
					}
					string key = enumerator.Current.Key;
					m_Dict[key] = value;
				}
			}
		}

		public override int Count
		{
			get
			{
				return m_Dict.Count;
			}
		}

		public override global::System.Collections.Generic.IEnumerable<global::SimpleJSON.JSONNode> Childs
		{
			get
			{
				foreach (global::System.Collections.Generic.KeyValuePair<string, global::SimpleJSON.JSONNode> item in m_Dict)
				{
					yield return item.Value;
				}
			}
		}

		public override void Add(string aKey, global::SimpleJSON.JSONNode aItem)
		{
			if (!string.IsNullOrEmpty(aKey))
			{
				if (m_Dict.ContainsKey(aKey))
				{
					m_Dict[aKey] = aItem;
				}
				else
				{
					m_Dict.Add(aKey, aItem);
				}
			}
			else
			{
				m_Dict.Add(global::System.Guid.NewGuid().ToString(), aItem);
			}
		}

		public override global::SimpleJSON.JSONNode Remove(string aKey)
		{
			if (!m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			global::SimpleJSON.JSONNode result = m_Dict[aKey];
			m_Dict.Remove(aKey);
			return result;
		}

		public override global::SimpleJSON.JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= m_Dict.Count)
			{
				return null;
			}
			global::System.Collections.Generic.Dictionary<string, global::SimpleJSON.JSONNode>.Enumerator enumerator = m_Dict.GetEnumerator();
			enumerator.MoveNext();
			for (int i = 0; i < aIndex; i++)
			{
				enumerator.MoveNext();
			}
			global::System.Collections.Generic.KeyValuePair<string, global::SimpleJSON.JSONNode> current = enumerator.Current;
			m_Dict.Remove(current.Key);
			return current.Value;
		}

		public override global::SimpleJSON.JSONNode Remove(global::SimpleJSON.JSONNode aNode)
		{
			try
			{
				string key = null;
				foreach (global::System.Collections.Generic.KeyValuePair<string, global::SimpleJSON.JSONNode> item in m_Dict)
				{
					if (item.Value == aNode)
					{
						key = item.Key;
						break;
					}
				}
				m_Dict.Remove(key);
				return aNode;
			}
			catch
			{
				return null;
			}
		}

		public global::System.Collections.IEnumerator GetEnumerator()
		{
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::SimpleJSON.JSONNode> N in m_Dict)
			{
				yield return N;
			}
		}

		public override string ToString()
		{
			string text = "{";
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::SimpleJSON.JSONNode> item in m_Dict)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				string text2 = text;
				text = text2 + "\"" + global::SimpleJSON.JSONNode.Escape(item.Key) + "\":" + item.Value.ToString();
			}
			return text + "}";
		}

		public override string ToString(string aPrefix)
		{
			string text = "{ ";
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::SimpleJSON.JSONNode> item in m_Dict)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				string text2 = text;
				text = text2 + "\"" + global::SimpleJSON.JSONNode.Escape(item.Key) + "\" : " + item.Value.ToString(aPrefix + "   ");
			}
			return text + "\n" + aPrefix + "}";
		}

		public override void Serialize(global::System.IO.BinaryWriter aWriter)
		{
			aWriter.Write((byte)2);
			aWriter.Write(m_Dict.Count);
			foreach (string key in m_Dict.Keys)
			{
				aWriter.Write(key);
				m_Dict[key].Serialize(aWriter);
			}
		}
	}
}
