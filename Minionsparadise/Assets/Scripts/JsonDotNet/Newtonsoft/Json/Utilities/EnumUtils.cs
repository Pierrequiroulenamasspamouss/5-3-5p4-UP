namespace Newtonsoft.Json.Utilities
{
	internal static class EnumUtils
	{
		public static T Parse<T>(string enumMemberName) where T : struct
		{
			return Parse<T>(enumMemberName, false);
		}

		public static T Parse<T>(string enumMemberName, bool ignoreCase) where T : struct
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentTypeIsEnum(typeof(T), "T");
			return (T)global::System.Enum.Parse(typeof(T), enumMemberName, ignoreCase);
		}

		public static bool TryParse<T>(string enumMemberName, bool ignoreCase, out T value) where T : struct
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentTypeIsEnum(typeof(T), "T");
			return global::Newtonsoft.Json.Utilities.MiscellaneousUtils.TryAction(() => Parse<T>(enumMemberName, ignoreCase), out value);
		}

		public static global::System.Collections.Generic.IList<T> GetFlagsValues<T>(T value) where T : struct
		{
			global::System.Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsDefined(typeof(global::System.FlagsAttribute), false))
			{
				throw new global::System.Exception("Enum type {0} is not a set of flags.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, typeFromHandle));
			}
			global::System.Type underlyingType = global::System.Enum.GetUnderlyingType(value.GetType());
			ulong num = global::System.Convert.ToUInt64(value, global::System.Globalization.CultureInfo.InvariantCulture);
			global::Newtonsoft.Json.Utilities.EnumValues<ulong> namesAndValues = GetNamesAndValues<T>();
			global::System.Collections.Generic.IList<T> list = new global::System.Collections.Generic.List<T>();
			foreach (global::Newtonsoft.Json.Utilities.EnumValue<ulong> item in namesAndValues)
			{
				if ((num & item.Value) == item.Value && item.Value != 0)
				{
					list.Add((T)global::System.Convert.ChangeType(item.Value, underlyingType, global::System.Globalization.CultureInfo.CurrentCulture));
				}
			}
			if (list.Count == 0 && global::System.Linq.Enumerable.SingleOrDefault(namesAndValues, (global::Newtonsoft.Json.Utilities.EnumValue<ulong> v) => v.Value == 0) != null)
			{
				list.Add(default(T));
			}
			return list;
		}

		public static global::Newtonsoft.Json.Utilities.EnumValues<ulong> GetNamesAndValues<T>() where T : struct
		{
			return GetNamesAndValues<ulong>(typeof(T));
		}

		public static global::Newtonsoft.Json.Utilities.EnumValues<TUnderlyingType> GetNamesAndValues<TEnum, TUnderlyingType>() where TEnum : struct where TUnderlyingType : struct
		{
			return GetNamesAndValues<TUnderlyingType>(typeof(TEnum));
		}

		public static global::Newtonsoft.Json.Utilities.EnumValues<TUnderlyingType> GetNamesAndValues<TUnderlyingType>(global::System.Type enumType) where TUnderlyingType : struct
		{
			if (enumType == null)
			{
				throw new global::System.ArgumentNullException("enumType");
			}
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentTypeIsEnum(enumType, "enumType");
			global::System.Collections.Generic.IList<object> values = GetValues(enumType);
			global::System.Collections.Generic.IList<string> names = GetNames(enumType);
			global::Newtonsoft.Json.Utilities.EnumValues<TUnderlyingType> enumValues = new global::Newtonsoft.Json.Utilities.EnumValues<TUnderlyingType>();
			for (int i = 0; i < values.Count; i++)
			{
				try
				{
					enumValues.Add(new global::Newtonsoft.Json.Utilities.EnumValue<TUnderlyingType>(names[i], (TUnderlyingType)global::System.Convert.ChangeType(values[i], typeof(TUnderlyingType), global::System.Globalization.CultureInfo.CurrentCulture)));
				}
				catch (global::System.OverflowException innerException)
				{
					throw new global::System.Exception(string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", global::System.Enum.GetUnderlyingType(enumType), typeof(TUnderlyingType), global::System.Convert.ToUInt64(values[i], global::System.Globalization.CultureInfo.InvariantCulture)), innerException);
				}
			}
			return enumValues;
		}

		public static global::System.Collections.Generic.IList<T> GetValues<T>()
		{
			return global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Cast<T>(GetValues(typeof(T))));
		}

		public static global::System.Collections.Generic.IList<object> GetValues(global::System.Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new global::System.ArgumentException("Type '" + enumType.Name + "' is not an enum.");
			}
			global::System.Collections.Generic.List<object> list = new global::System.Collections.Generic.List<object>();
			global::System.Collections.Generic.IEnumerable<global::System.Reflection.FieldInfo> enumerable = global::System.Linq.Enumerable.Where(enumType.GetFields(), (global::System.Reflection.FieldInfo field) => field.IsLiteral);
			foreach (global::System.Reflection.FieldInfo item in enumerable)
			{
				object value = item.GetValue(enumType);
				list.Add(value);
			}
			return list;
		}

		public static global::System.Collections.Generic.IList<string> GetNames<T>()
		{
			return GetNames(typeof(T));
		}

		public static global::System.Collections.Generic.IList<string> GetNames(global::System.Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new global::System.ArgumentException("Type '" + enumType.Name + "' is not an enum.");
			}
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			global::System.Collections.Generic.IEnumerable<global::System.Reflection.FieldInfo> enumerable = global::System.Linq.Enumerable.Where(enumType.GetFields(), (global::System.Reflection.FieldInfo field) => field.IsLiteral);
			foreach (global::System.Reflection.FieldInfo item in enumerable)
			{
				list.Add(item.Name);
			}
			return list;
		}

		public static TEnumType GetMaximumValue<TEnumType>(global::System.Type enumType) where TEnumType : global::System.IConvertible, global::System.IComparable<TEnumType>
		{
			if (enumType == null)
			{
				throw new global::System.ArgumentNullException("enumType");
			}
			global::System.Type underlyingType = global::System.Enum.GetUnderlyingType(enumType);
			if (!typeof(TEnumType).IsAssignableFrom(underlyingType))
			{
				throw new global::System.ArgumentException(string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "TEnumType is not assignable from the enum's underlying type of {0}.", underlyingType.Name));
			}
			ulong num = 0uL;
			global::System.Collections.Generic.IList<object> values = GetValues(enumType);
			if (enumType.IsDefined(typeof(global::System.FlagsAttribute), false))
			{
				foreach (TEnumType item in values)
				{
					num |= item.ToUInt64(global::System.Globalization.CultureInfo.InvariantCulture);
				}
			}
			else
			{
				foreach (TEnumType item2 in values)
				{
					ulong num2 = item2.ToUInt64(global::System.Globalization.CultureInfo.InvariantCulture);
					if (num.CompareTo(num2) == -1)
					{
						num = num2;
					}
				}
			}
			return (TEnumType)global::System.Convert.ChangeType(num, typeof(TEnumType), global::System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
