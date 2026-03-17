namespace Newtonsoft.Json.Utilities
{
	internal static class ConvertUtils
	{
		internal struct TypeConvertKey : global::System.IEquatable<global::Newtonsoft.Json.Utilities.ConvertUtils.TypeConvertKey>
		{
			private readonly global::System.Type _initialType;

			private readonly global::System.Type _targetType;

			public global::System.Type InitialType
			{
				get
				{
					return _initialType;
				}
			}

			public global::System.Type TargetType
			{
				get
				{
					return _targetType;
				}
			}

			public TypeConvertKey(global::System.Type initialType, global::System.Type targetType)
			{
				_initialType = initialType;
				_targetType = targetType;
			}

			public override int GetHashCode()
			{
				return _initialType.GetHashCode() ^ _targetType.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (!(obj is global::Newtonsoft.Json.Utilities.ConvertUtils.TypeConvertKey))
				{
					return false;
				}
				return Equals((global::Newtonsoft.Json.Utilities.ConvertUtils.TypeConvertKey)obj);
			}

			public bool Equals(global::Newtonsoft.Json.Utilities.ConvertUtils.TypeConvertKey other)
			{
				if (_initialType == other._initialType)
				{
					return _targetType == other._targetType;
				}
				return false;
			}
		}

		private static readonly global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::Newtonsoft.Json.Utilities.ConvertUtils.TypeConvertKey, global::System.Func<object, object>> CastConverters = new global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::Newtonsoft.Json.Utilities.ConvertUtils.TypeConvertKey, global::System.Func<object, object>>(CreateCastConverter);

		private static global::System.Func<object, object> CreateCastConverter(global::Newtonsoft.Json.Utilities.ConvertUtils.TypeConvertKey t)
		{
			global::System.Reflection.MethodInfo method = t.TargetType.GetMethod("op_Implicit", new global::System.Type[1] { t.InitialType });
			if (method == null)
			{
				method = t.TargetType.GetMethod("op_Explicit", new global::System.Type[1] { t.InitialType });
			}
			if (method == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Utilities.MethodCall<object, object> call = global::Newtonsoft.Json.Serialization.JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => call(null, o);
		}

		public static bool CanConvertType(global::System.Type initialType, global::System.Type targetType, bool allowTypeNameToString)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(initialType, "initialType");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(targetType, "targetType");
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(targetType))
			{
				targetType = global::System.Nullable.GetUnderlyingType(targetType);
			}
			if (targetType == initialType)
			{
				return true;
			}
			if (typeof(global::System.IConvertible).IsAssignableFrom(initialType) && typeof(global::System.IConvertible).IsAssignableFrom(targetType))
			{
				return true;
			}
			if (initialType == typeof(global::System.DateTime) && targetType == typeof(global::System.DateTimeOffset))
			{
				return true;
			}
			if (initialType == typeof(global::System.Guid) && (targetType == typeof(global::System.Guid) || targetType == typeof(string)))
			{
				return true;
			}
			if (initialType == typeof(global::System.Type) && targetType == typeof(string))
			{
				return true;
			}
			global::System.ComponentModel.TypeConverter converter = GetConverter(initialType);
			if (converter != null && !IsComponentConverter(converter) && converter.CanConvertTo(targetType) && (allowTypeNameToString || converter.GetType() != typeof(global::System.ComponentModel.TypeConverter)))
			{
				return true;
			}
			global::System.ComponentModel.TypeConverter converter2 = GetConverter(targetType);
			if (converter2 != null && !IsComponentConverter(converter2) && converter2.CanConvertFrom(initialType))
			{
				return true;
			}
			if (initialType == typeof(global::System.DBNull) && global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullable(targetType))
			{
				return true;
			}
			return false;
		}

		private static bool IsComponentConverter(global::System.ComponentModel.TypeConverter converter)
		{
			return converter is global::System.ComponentModel.ComponentConverter;
		}

		public static T Convert<T>(object initialValue)
		{
			return Convert<T>(initialValue, global::System.Globalization.CultureInfo.CurrentCulture);
		}

		public static T Convert<T>(object initialValue, global::System.Globalization.CultureInfo culture)
		{
			return (T)Convert(initialValue, culture, typeof(T));
		}

		public static object Convert(object initialValue, global::System.Globalization.CultureInfo culture, global::System.Type targetType)
		{
			if (initialValue == null)
			{
				throw new global::System.ArgumentNullException("initialValue");
			}
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(targetType))
			{
				targetType = global::System.Nullable.GetUnderlyingType(targetType);
			}
			global::System.Type type = initialValue.GetType();
			if (targetType == type)
			{
				return initialValue;
			}
			if (initialValue is string && typeof(global::System.Type).IsAssignableFrom(targetType))
			{
				return global::System.Type.GetType((string)initialValue, true);
			}
			if (targetType.IsInterface || targetType.IsGenericTypeDefinition || targetType.IsAbstract)
			{
				throw new global::System.ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, targetType), "targetType");
			}
			if (initialValue is global::System.IConvertible && typeof(global::System.IConvertible).IsAssignableFrom(targetType))
			{
				if (targetType.IsEnum)
				{
					if (initialValue is string)
					{
						return global::System.Enum.Parse(targetType, initialValue.ToString(), true);
					}
					if (IsInteger(initialValue))
					{
						return global::System.Enum.ToObject(targetType, initialValue);
					}
				}
				return global::System.Convert.ChangeType(initialValue, targetType, culture);
			}
			if (initialValue is global::System.DateTime && targetType == typeof(global::System.DateTimeOffset))
			{
				return new global::System.DateTimeOffset((global::System.DateTime)initialValue);
			}
			if (initialValue is string)
			{
				if (targetType == typeof(global::System.Guid))
				{
					return new global::System.Guid((string)initialValue);
				}
				if (targetType == typeof(global::System.Uri))
				{
					return new global::System.Uri((string)initialValue);
				}
				if (targetType == typeof(global::System.TimeSpan))
				{
					return global::System.TimeSpan.Parse((string)initialValue);
				}
			}
			global::System.ComponentModel.TypeConverter converter = GetConverter(type);
			if (converter != null && converter.CanConvertTo(targetType))
			{
				return converter.ConvertTo(null, culture, initialValue, targetType);
			}
			global::System.ComponentModel.TypeConverter converter2 = GetConverter(targetType);
			if (converter2 != null && converter2.CanConvertFrom(type))
			{
				return converter2.ConvertFrom(null, culture, initialValue);
			}
			if (initialValue == global::System.DBNull.Value)
			{
				if (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullable(targetType))
				{
					return EnsureTypeAssignable(null, type, targetType);
				}
				throw new global::System.Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, type, targetType));
			}
			throw new global::System.Exception("Can not convert from {0} to {1}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, type, targetType));
		}

		public static bool TryConvert<T>(object initialValue, out T convertedValue)
		{
			return TryConvert<T>(initialValue, global::System.Globalization.CultureInfo.CurrentCulture, out convertedValue);
		}

		public static bool TryConvert<T>(object initialValue, global::System.Globalization.CultureInfo culture, out T convertedValue)
		{
			return global::Newtonsoft.Json.Utilities.MiscellaneousUtils.TryAction(delegate
			{
				object convertedValue2;
				TryConvert(initialValue, global::System.Globalization.CultureInfo.CurrentCulture, typeof(T), out convertedValue2);
				return (T)convertedValue2;
			}, out convertedValue);
		}

		public static bool TryConvert(object initialValue, global::System.Globalization.CultureInfo culture, global::System.Type targetType, out object convertedValue)
		{
			return global::Newtonsoft.Json.Utilities.MiscellaneousUtils.TryAction(() => Convert(initialValue, culture, targetType), out convertedValue);
		}

		public static T ConvertOrCast<T>(object initialValue)
		{
			return ConvertOrCast<T>(initialValue, global::System.Globalization.CultureInfo.CurrentCulture);
		}

		public static T ConvertOrCast<T>(object initialValue, global::System.Globalization.CultureInfo culture)
		{
			return (T)ConvertOrCast(initialValue, culture, typeof(T));
		}

		public static object ConvertOrCast(object initialValue, global::System.Globalization.CultureInfo culture, global::System.Type targetType)
		{
			if (targetType == typeof(object))
			{
				return initialValue;
			}
			if (initialValue == null && global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			object convertedValue;
			if (TryConvert(initialValue, culture, targetType, out convertedValue))
			{
				return convertedValue;
			}
			return EnsureTypeAssignable(initialValue, global::Newtonsoft.Json.Utilities.ReflectionUtils.GetObjectType(initialValue), targetType);
		}

		public static bool TryConvertOrCast<T>(object initialValue, out T convertedValue)
		{
			return TryConvertOrCast<T>(initialValue, global::System.Globalization.CultureInfo.CurrentCulture, out convertedValue);
		}

		public static bool TryConvertOrCast<T>(object initialValue, global::System.Globalization.CultureInfo culture, out T convertedValue)
		{
			return global::Newtonsoft.Json.Utilities.MiscellaneousUtils.TryAction(delegate
			{
				object convertedValue2;
				TryConvertOrCast(initialValue, global::System.Globalization.CultureInfo.CurrentCulture, typeof(T), out convertedValue2);
				return (T)convertedValue2;
			}, out convertedValue);
		}

		public static bool TryConvertOrCast(object initialValue, global::System.Globalization.CultureInfo culture, global::System.Type targetType, out object convertedValue)
		{
			return global::Newtonsoft.Json.Utilities.MiscellaneousUtils.TryAction(() => ConvertOrCast(initialValue, culture, targetType), out convertedValue);
		}

		private static object EnsureTypeAssignable(object value, global::System.Type initialType, global::System.Type targetType)
		{
			global::System.Type type = ((value != null) ? value.GetType() : null);
			if (value != null)
			{
				if (targetType.IsAssignableFrom(type))
				{
					return value;
				}
				global::System.Func<object, object> func = CastConverters.Get(new global::Newtonsoft.Json.Utilities.ConvertUtils.TypeConvertKey(type, targetType));
				if (func != null)
				{
					return func(value);
				}
			}
			else if (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			throw new global::System.Exception("Could not cast or convert from {0} to {1}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, (initialType != null) ? initialType.ToString() : "{null}", targetType));
		}

		internal static global::System.ComponentModel.TypeConverter GetConverter(global::System.Type t)
		{
			return global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetTypeConverter(t);
		}

		public static bool IsInteger(object value)
		{
			switch (global::System.Convert.GetTypeCode(value))
			{
			case global::System.TypeCode.SByte:
			case global::System.TypeCode.Byte:
			case global::System.TypeCode.Int16:
			case global::System.TypeCode.UInt16:
			case global::System.TypeCode.Int32:
			case global::System.TypeCode.UInt32:
			case global::System.TypeCode.Int64:
			case global::System.TypeCode.UInt64:
				return true;
			default:
				return false;
			}
		}
	}
}
