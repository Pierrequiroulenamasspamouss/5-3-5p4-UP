namespace SimpleJSON
{
	public class JSONArray : global::SimpleJSON.JSONNode, global::System.Collections.IEnumerable
	{
		private global::System.Collections.Generic.List<global::SimpleJSON.JSONNode> m_List = new global::System.Collections.Generic.List<global::SimpleJSON.JSONNode>();

		public override global::SimpleJSON.JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= m_List.Count)
				{
					return new global::SimpleJSON.JSONLazyCreator(this);
				}
				return m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= m_List.Count)
				{
					m_List.Add(value);
				}
				else
				{
					m_List[aIndex] = value;
				}
			}
		}

		public override global::SimpleJSON.JSONNode this[string aKey]
		{
			get
			{
				return new global::SimpleJSON.JSONLazyCreator(this);
			}
			set
			{
				m_List.Add(value);
			}
		}

		public override int Count
		{
			get
			{
				return m_List.Count;
			}
		}

		public override global::System.Collections.Generic.IEnumerable<global::SimpleJSON.JSONNode> Childs
		{
			get
			{
				foreach (global::SimpleJSON.JSONNode item in m_List)
				{
					yield return item;
				}
			}
		}

		public override void Add(string aKey, global::SimpleJSON.JSONNode aItem)
		{
			m_List.Add(aItem);
		}

		public override global::SimpleJSON.JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= m_List.Count)
			{
				return null;
			}
			global::SimpleJSON.JSONNode result = m_List[aIndex];
			m_List.RemoveAt(aIndex);
			return result;
		}

		public override global::SimpleJSON.JSONNode Remove(global::SimpleJSON.JSONNode aNode)
		{
			m_List.Remove(aNode);
			return aNode;
		}

		public global::System.Collections.IEnumerator GetEnumerator()
		{
			foreach (global::SimpleJSON.JSONNode item in m_List)
			{
				yield return item;
			}
		}

		public override string ToString()
		{
			string text = "[ ";
			foreach (global::SimpleJSON.JSONNode item in m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += item.ToString();
			}
			return text + " ]";
		}

		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (global::SimpleJSON.JSONNode item in m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += item.ToString(aPrefix + "   ");
			}
			return text + "\n" + aPrefix + "]";
		}

		public override void Serialize(global::System.IO.BinaryWriter aWriter)
		{
			aWriter.Write((byte)1);
			aWriter.Write(m_List.Count);
			for (int i = 0; i < m_List.Count; i++)
			{
				m_List[i].Serialize(aWriter);
			}
		}
	}
}
