namespace Newtonsoft.Json.Utilities
{
	internal static class MiscellaneousUtils
	{
		public static bool ValueEquals(object objA, object objB)
		{
			if (objA == null && objB == null)
			{
				return true;
			}
			if (objA != null && objB == null)
			{
				return false;
			}
			if (objA == null && objB != null)
			{
				return false;
			}
			if (objA.GetType() != objB.GetType())
			{
				if (global::Newtonsoft.Json.Utilities.ConvertUtils.IsInteger(objA) && global::Newtonsoft.Json.Utilities.ConvertUtils.IsInteger(objB))
				{
					return global::System.Convert.ToDecimal(objA, global::System.Globalization.CultureInfo.CurrentCulture).Equals(global::System.Convert.ToDecimal(objB, global::System.Globalization.CultureInfo.CurrentCulture));
				}
				if ((objA is double || objA is float || objA is decimal) && (objB is double || objB is float || objB is decimal))
				{
					return global::Newtonsoft.Json.Utilities.MathUtils.ApproxEquals(global::System.Convert.ToDouble(objA, global::System.Globalization.CultureInfo.CurrentCulture), global::System.Convert.ToDouble(objB, global::System.Globalization.CultureInfo.CurrentCulture));
				}
				return false;
			}
			return objA.Equals(objB);
		}

		public static global::System.ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
		{
			string message2 = message + global::System.Environment.NewLine + "Actual value was {0}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, actualValue);
			return new global::System.ArgumentOutOfRangeException(paramName, message2);
		}

		public static bool TryAction<T>(global::Newtonsoft.Json.Utilities.Creator<T> creator, out T output)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(creator, "creator");
			try
			{
				output = creator();
				return true;
			}
			catch
			{
				output = default(T);
				return false;
			}
		}

		public static string ToString(object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			if (!(value is string))
			{
				return value.ToString();
			}
			return "\"" + value.ToString() + "\"";
		}

		public static byte[] HexToBytes(string hex)
		{
			string text = hex.Replace("-", string.Empty);
			byte[] array = new byte[text.Length / 2];
			int num = 4;
			int num2 = 0;
			string text2 = text;
			foreach (char c in text2)
			{
				int num3 = (c - 48) % 32;
				if (num3 > 9)
				{
					num3 -= 7;
				}
				array[num2] |= (byte)(num3 << num);
				num ^= 4;
				if (num != 0)
				{
					num2++;
				}
			}
			return array;
		}

		public static string BytesToHex(byte[] bytes)
		{
			return BytesToHex(bytes, false);
		}

		public static string BytesToHex(byte[] bytes, bool removeDashes)
		{
			string text = global::System.BitConverter.ToString(bytes);
			if (removeDashes)
			{
				text = text.Replace("-", "");
			}
			return text;
		}

		public static int ByteArrayCompare(byte[] a1, byte[] a2)
		{
			int num = a1.Length.CompareTo(a2.Length);
			if (num != 0)
			{
				return num;
			}
			for (int i = 0; i < a1.Length; i++)
			{
				int num2 = a1[i].CompareTo(a2[i]);
				if (num2 != 0)
				{
					return num2;
				}
			}
			return 0;
		}

		public static string GetPrefix(string qualifiedName)
		{
			string prefix;
			string localName;
			GetQualifiedNameParts(qualifiedName, out prefix, out localName);
			return prefix;
		}

		public static string GetLocalName(string qualifiedName)
		{
			string prefix;
			string localName;
			GetQualifiedNameParts(qualifiedName, out prefix, out localName);
			return localName;
		}

		public static void GetQualifiedNameParts(string qualifiedName, out string prefix, out string localName)
		{
			int num = qualifiedName.IndexOf(':');
			if (num == -1 || num == 0 || qualifiedName.Length - 1 == num)
			{
				prefix = null;
				localName = qualifiedName;
			}
			else
			{
				prefix = qualifiedName.Substring(0, num);
				localName = qualifiedName.Substring(num + 1);
			}
		}
	}
}
