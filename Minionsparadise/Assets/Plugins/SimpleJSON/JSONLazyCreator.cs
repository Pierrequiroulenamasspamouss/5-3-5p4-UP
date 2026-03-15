namespace SimpleJSON
{
	internal class JSONLazyCreator : global::SimpleJSON.JSONNode
	{
		private global::SimpleJSON.JSONNode m_Node;

		private string m_Key;

		public override global::SimpleJSON.JSONNode this[int aIndex]
		{
			get
			{
				return new global::SimpleJSON.JSONLazyCreator(this);
			}
			set
			{
				global::SimpleJSON.JSONArray jSONArray = new global::SimpleJSON.JSONArray();
				jSONArray.Add(value);
				Set(jSONArray);
			}
		}

		public override global::SimpleJSON.JSONNode this[string aKey]
		{
			get
			{
				return new global::SimpleJSON.JSONLazyCreator(this, aKey);
			}
			set
			{
				global::SimpleJSON.JSONClass jSONClass = new global::SimpleJSON.JSONClass();
				jSONClass.Add(aKey, value);
				Set(jSONClass);
			}
		}

		public override int AsInt
		{
			get
			{
				global::SimpleJSON.JSONData aVal = new global::SimpleJSON.JSONData(0);
				Set(aVal);
				return 0;
			}
			set
			{
				global::SimpleJSON.JSONData aVal = new global::SimpleJSON.JSONData(value);
				Set(aVal);
			}
		}

		public override float AsFloat
		{
			get
			{
				global::SimpleJSON.JSONData aVal = new global::SimpleJSON.JSONData(0f);
				Set(aVal);
				return 0f;
			}
			set
			{
				global::SimpleJSON.JSONData aVal = new global::SimpleJSON.JSONData(value);
				Set(aVal);
			}
		}

		public override double AsDouble
		{
			get
			{
				global::SimpleJSON.JSONData aVal = new global::SimpleJSON.JSONData(0.0);
				Set(aVal);
				return 0.0;
			}
			set
			{
				global::SimpleJSON.JSONData aVal = new global::SimpleJSON.JSONData(value);
				Set(aVal);
			}
		}

		public override bool AsBool
		{
			get
			{
				global::SimpleJSON.JSONData aVal = new global::SimpleJSON.JSONData(false);
				Set(aVal);
				return false;
			}
			set
			{
				global::SimpleJSON.JSONData aVal = new global::SimpleJSON.JSONData(value);
				Set(aVal);
			}
		}

		public override global::SimpleJSON.JSONArray AsArray
		{
			get
			{
				global::SimpleJSON.JSONArray jSONArray = new global::SimpleJSON.JSONArray();
				Set(jSONArray);
				return jSONArray;
			}
		}

		public override global::SimpleJSON.JSONClass AsObject
		{
			get
			{
				global::SimpleJSON.JSONClass jSONClass = new global::SimpleJSON.JSONClass();
				Set(jSONClass);
				return jSONClass;
			}
		}

		public JSONLazyCreator(global::SimpleJSON.JSONNode aNode)
		{
			m_Node = aNode;
			m_Key = null;
		}

		public JSONLazyCreator(global::SimpleJSON.JSONNode aNode, string aKey)
		{
			m_Node = aNode;
			m_Key = aKey;
		}

		private void Set(global::SimpleJSON.JSONNode aVal)
		{
			if (m_Key == null)
			{
				m_Node.Add(aVal);
			}
			else
			{
				m_Node.Add(m_Key, aVal);
			}
			m_Node = null;
		}

		public override void Add(global::SimpleJSON.JSONNode aItem)
		{
			global::SimpleJSON.JSONArray jSONArray = new global::SimpleJSON.JSONArray();
			jSONArray.Add(aItem);
			Set(jSONArray);
		}

		public override void Add(string aKey, global::SimpleJSON.JSONNode aItem)
		{
			global::SimpleJSON.JSONClass jSONClass = new global::SimpleJSON.JSONClass();
			jSONClass.Add(aKey, aItem);
			Set(jSONClass);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return true;
			}
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Empty;
		}

		public override string ToString(string aPrefix)
		{
			return string.Empty;
		}

		public static bool operator ==(global::SimpleJSON.JSONLazyCreator a, object b)
		{
			if (b == null)
			{
				return true;
			}
			return object.ReferenceEquals(a, b);
		}

		public static bool operator !=(global::SimpleJSON.JSONLazyCreator a, object b)
		{
			return !(a == b);
		}
	}
}
