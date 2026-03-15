namespace SimpleJSON
{
	public class JSONNode
	{
		public virtual global::SimpleJSON.JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual global::SimpleJSON.JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual string Value
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		public virtual global::System.Collections.Generic.IEnumerable<global::SimpleJSON.JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		public global::System.Collections.Generic.IEnumerable<global::SimpleJSON.JSONNode> DeepChilds
		{
			get
			{
				foreach (global::SimpleJSON.JSONNode C in Childs)
				{
					foreach (global::SimpleJSON.JSONNode deepChild in C.DeepChilds)
					{
						yield return deepChild;
					}
				}
			}
		}

		public virtual int AsInt
		{
			get
			{
				int result = 0;
				if (int.TryParse(Value, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				Value = value.ToString();
			}
		}

		public virtual float AsFloat
		{
			get
			{
				float result = 0f;
				if (float.TryParse(Value, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				Value = value.ToString();
			}
		}

		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(Value, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				Value = value.ToString();
			}
		}

		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(Value);
			}
			set
			{
				Value = ((!value) ? "false" : "true");
			}
		}

		public virtual global::SimpleJSON.JSONArray AsArray
		{
			get
			{
				return this as global::SimpleJSON.JSONArray;
			}
		}

		public virtual global::SimpleJSON.JSONClass AsObject
		{
			get
			{
				return this as global::SimpleJSON.JSONClass;
			}
		}

		public virtual void Add(string aKey, global::SimpleJSON.JSONNode aItem)
		{
		}

		public virtual void Add(global::SimpleJSON.JSONNode aItem)
		{
			Add(string.Empty, aItem);
		}

		public virtual global::SimpleJSON.JSONNode Remove(string aKey)
		{
			return null;
		}

		public virtual global::SimpleJSON.JSONNode Remove(int aIndex)
		{
			return null;
		}

		public virtual global::SimpleJSON.JSONNode Remove(global::SimpleJSON.JSONNode aNode)
		{
			return aNode;
		}

		public override string ToString()
		{
			return "JSONNode";
		}

		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		internal static string Escape(string aText)
		{
			string text = string.Empty;
			foreach (char c in aText)
			{
				switch (c)
				{
				case '\\':
					text += "\\\\";
					break;
				case '"':
					text += "\\\"";
					break;
				case '\n':
					text += "\\n";
					break;
				case '\r':
					text += "\\r";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\b':
					text += "\\b";
					break;
				case '\f':
					text += "\\f";
					break;
				default:
					text += c;
					break;
				}
			}
			return text;
		}

		public static global::SimpleJSON.JSONNode Parse(string aJSON)
		{
			global::System.Collections.Generic.Stack<global::SimpleJSON.JSONNode> stack = new global::System.Collections.Generic.Stack<global::SimpleJSON.JSONNode>();
			global::SimpleJSON.JSONNode jSONNode = null;
			int i = 0;
			string text = string.Empty;
			string text2 = string.Empty;
			bool flag = false;
			for (; i < aJSON.Length; i++)
			{
				switch (aJSON[i])
				{
				case '{':
					if (flag)
					{
						text += aJSON[i];
						break;
					}
					stack.Push(new global::SimpleJSON.JSONClass());
					if (jSONNode != null)
					{
						text2 = text2.Trim();
						if (jSONNode is global::SimpleJSON.JSONArray)
						{
							jSONNode.Add(stack.Peek());
						}
						else if (text2 != string.Empty)
						{
							jSONNode.Add(text2, stack.Peek());
						}
					}
					text2 = string.Empty;
					text = string.Empty;
					jSONNode = stack.Peek();
					break;
				case '[':
					if (flag)
					{
						text += aJSON[i];
						break;
					}
					stack.Push(new global::SimpleJSON.JSONArray());
					if (jSONNode != null)
					{
						text2 = text2.Trim();
						if (jSONNode is global::SimpleJSON.JSONArray)
						{
							jSONNode.Add(stack.Peek());
						}
						else if (text2 != string.Empty)
						{
							jSONNode.Add(text2, stack.Peek());
						}
					}
					text2 = string.Empty;
					text = string.Empty;
					jSONNode = stack.Peek();
					break;
				case ']':
				case '}':
					if (flag)
					{
						text += aJSON[i];
						break;
					}
					if (stack.Count == 0)
					{
						throw new global::System.Exception("JSON Parse: Too many closing brackets");
					}
					stack.Pop();
					if (text != string.Empty)
					{
						text2 = text2.Trim();
						if (jSONNode is global::SimpleJSON.JSONArray)
						{
							jSONNode.Add(text);
						}
						else if (text2 != string.Empty)
						{
							jSONNode.Add(text2, text);
						}
					}
					text2 = string.Empty;
					text = string.Empty;
					if (stack.Count > 0)
					{
						jSONNode = stack.Peek();
					}
					break;
				case ':':
					if (flag)
					{
						text += aJSON[i];
						break;
					}
					text2 = text;
					text = string.Empty;
					break;
				case '"':
					flag = (byte)((flag ? 1u : 0u) ^ 1u) != 0;
					break;
				case ',':
					if (flag)
					{
						text += aJSON[i];
						break;
					}
					if (text != string.Empty)
					{
						if (jSONNode is global::SimpleJSON.JSONArray)
						{
							jSONNode.Add(text);
						}
						else if (text2 != string.Empty)
						{
							jSONNode.Add(text2, text);
						}
					}
					text2 = string.Empty;
					text = string.Empty;
					break;
				case '\t':
				case ' ':
					if (flag)
					{
						text += aJSON[i];
					}
					break;
				case '\\':
					i++;
					if (flag)
					{
						char c = aJSON[i];
						switch (c)
						{
						case 't':
							text += '\t';
							break;
						case 'r':
							text += '\r';
							break;
						case 'n':
							text += '\n';
							break;
						case 'b':
							text += '\b';
							break;
						case 'f':
							text += '\f';
							break;
						case 'u':
						{
							string s = aJSON.Substring(i + 1, 4);
							text += (char)int.Parse(s, global::System.Globalization.NumberStyles.AllowHexSpecifier);
							i += 4;
							break;
						}
						default:
							text += c;
							break;
						}
					}
					break;
				default:
					text += aJSON[i];
					break;
				case '\n':
				case '\r':
					break;
				}
			}
			if (flag)
			{
				throw new global::System.Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jSONNode;
		}

		public virtual void Serialize(global::System.IO.BinaryWriter aWriter)
		{
		}

		public void SaveToStream(global::System.IO.Stream aData)
		{
			global::System.IO.BinaryWriter aWriter = new global::System.IO.BinaryWriter(aData);
			Serialize(aWriter);
		}

		public void SaveToCompressedStream(global::System.IO.Stream aData)
		{
			throw new global::System.Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void SaveToCompressedFile(string aFileName)
		{
			throw new global::System.Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public string SaveToCompressedBase64()
		{
			throw new global::System.Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void SaveToFile(string aFileName)
		{
#if !UNITY_WEBPLAYER
			global::System.IO.Directory.CreateDirectory(new global::System.IO.FileInfo(aFileName).Directory.FullName);
			using (global::System.IO.FileStream aData = global::System.IO.File.OpenWrite(aFileName))
			{
				SaveToStream(aData);
			}
#else
			throw new global::System.Exception("Can't use File IO in WebPlayer");
#endif
		}

		public string SaveToBase64()
		{
			using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream())
			{
				SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				return global::System.Convert.ToBase64String(memoryStream.ToArray());
			}
		}

		public static global::SimpleJSON.JSONNode Deserialize(global::System.IO.BinaryReader aReader)
		{
			global::SimpleJSON.JSONBinaryTag jSONBinaryTag = (global::SimpleJSON.JSONBinaryTag)aReader.ReadByte();
			switch (jSONBinaryTag)
			{
			case global::SimpleJSON.JSONBinaryTag.Array:
			{
				int num2 = aReader.ReadInt32();
				global::SimpleJSON.JSONArray jSONArray = new global::SimpleJSON.JSONArray();
				for (int j = 0; j < num2; j++)
				{
					jSONArray.Add(Deserialize(aReader));
				}
				return jSONArray;
			}
			case global::SimpleJSON.JSONBinaryTag.Class:
			{
				int num = aReader.ReadInt32();
				global::SimpleJSON.JSONClass jSONClass = new global::SimpleJSON.JSONClass();
				for (int i = 0; i < num; i++)
				{
					string aKey = aReader.ReadString();
					global::SimpleJSON.JSONNode aItem = Deserialize(aReader);
					jSONClass.Add(aKey, aItem);
				}
				return jSONClass;
			}
			case global::SimpleJSON.JSONBinaryTag.Value:
				return new global::SimpleJSON.JSONData(aReader.ReadString());
			case global::SimpleJSON.JSONBinaryTag.IntValue:
				return new global::SimpleJSON.JSONData(aReader.ReadInt32());
			case global::SimpleJSON.JSONBinaryTag.DoubleValue:
				return new global::SimpleJSON.JSONData(aReader.ReadDouble());
			case global::SimpleJSON.JSONBinaryTag.BoolValue:
				return new global::SimpleJSON.JSONData(aReader.ReadBoolean());
			case global::SimpleJSON.JSONBinaryTag.FloatValue:
				return new global::SimpleJSON.JSONData(aReader.ReadSingle());
			default:
				throw new global::System.Exception("Error deserializing JSON. Unknown tag: " + jSONBinaryTag);
			}
		}

		public static global::SimpleJSON.JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new global::System.Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static global::SimpleJSON.JSONNode LoadFromCompressedStream(global::System.IO.Stream aData)
		{
			throw new global::System.Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static global::SimpleJSON.JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new global::System.Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static global::SimpleJSON.JSONNode LoadFromStream(global::System.IO.Stream aData)
		{
			using (global::System.IO.BinaryReader aReader = new global::System.IO.BinaryReader(aData))
			{
				return Deserialize(aReader);
			}
		}

		public static global::SimpleJSON.JSONNode LoadFromFile(string aFileName)
		{
			using (global::System.IO.FileStream aData = global::System.IO.File.OpenRead(aFileName))
			{
				return LoadFromStream(aData);
			}
		}

		public static global::SimpleJSON.JSONNode LoadFromBase64(string aBase64)
		{
			byte[] buffer = global::System.Convert.FromBase64String(aBase64);
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(buffer);
			memoryStream.Position = 0L;
			return LoadFromStream(memoryStream);
		}

		public static implicit operator global::SimpleJSON.JSONNode(string s)
		{
			return new global::SimpleJSON.JSONData(s);
		}

		public static implicit operator string(global::SimpleJSON.JSONNode d)
		{
			return (!(d == null)) ? d.Value : null;
		}

		public static bool operator ==(global::SimpleJSON.JSONNode a, object b)
		{
			if (b == null && a is global::SimpleJSON.JSONLazyCreator)
			{
				return true;
			}
			return object.ReferenceEquals(a, b);
		}

		public static bool operator !=(global::SimpleJSON.JSONNode a, object b)
		{
			return !(a == b);
		}
	}
}
