namespace Newtonsoft.Json.Utilities
{
	internal static class ReflectionUtils
	{
		public static bool IsVirtual(this global::System.Reflection.PropertyInfo propertyInfo)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			global::System.Reflection.MethodInfo getMethod = propertyInfo.GetGetMethod();
			if (getMethod != null && getMethod.IsVirtual)
			{
				return true;
			}
			getMethod = propertyInfo.GetSetMethod();
			if (getMethod != null && getMethod.IsVirtual)
			{
				return true;
			}
			return false;
		}

		public static global::System.Type GetObjectType(object v)
		{
			if (v == null)
			{
				return null;
			}
			return v.GetType();
		}

		public static string GetTypeName(global::System.Type t, global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle assemblyFormat)
		{
			return GetTypeName(t, assemblyFormat, null);
		}

		public static string GetTypeName(global::System.Type t, global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle assemblyFormat, global::System.Runtime.Serialization.SerializationBinder binder)
		{
			string assemblyQualifiedName = t.AssemblyQualifiedName;
			switch (assemblyFormat)
			{
			case global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple:
				return RemoveAssemblyDetails(assemblyQualifiedName);
			case global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full:
				return t.AssemblyQualifiedName;
			default:
				throw new global::System.ArgumentOutOfRangeException();
			}
		}

		private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			bool flag = false;
			bool flag2 = false;
			foreach (char c in fullyQualifiedTypeName)
			{
				switch (c)
				{
				case '[':
					flag = false;
					flag2 = false;
					stringBuilder.Append(c);
					break;
				case ']':
					flag = false;
					flag2 = false;
					stringBuilder.Append(c);
					break;
				case ',':
					if (!flag)
					{
						flag = true;
						stringBuilder.Append(c);
					}
					else
					{
						flag2 = true;
					}
					break;
				default:
					if (!flag2)
					{
						stringBuilder.Append(c);
					}
					break;
				}
			}
			return stringBuilder.ToString();
		}

		public static bool IsInstantiatableType(global::System.Type t)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(t, "t");
			if (t.IsAbstract || t.IsInterface || t.IsArray || t.IsGenericTypeDefinition || t == typeof(void))
			{
				return false;
			}
			if (!HasDefaultConstructor(t))
			{
				return false;
			}
			return true;
		}

		public static bool HasDefaultConstructor(global::System.Type t)
		{
			return HasDefaultConstructor(t, false);
		}

		public static bool HasDefaultConstructor(global::System.Type t, bool nonPublic)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(t, "t");
			if (t.IsValueType)
			{
				return true;
			}
			return GetDefaultConstructor(t, nonPublic) != null;
		}

		public static global::System.Reflection.ConstructorInfo GetDefaultConstructor(global::System.Type t)
		{
			return GetDefaultConstructor(t, false);
		}

		public static global::System.Reflection.ConstructorInfo GetDefaultConstructor(global::System.Type t, bool nonPublic)
		{
			global::System.Reflection.BindingFlags bindingFlags = global::System.Reflection.BindingFlags.Public;
			if (nonPublic)
			{
				bindingFlags |= global::System.Reflection.BindingFlags.NonPublic;
			}
			return t.GetConstructor(bindingFlags | global::System.Reflection.BindingFlags.Instance, null, new global::System.Type[0], null);
		}

		public static bool IsNullable(global::System.Type t)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(t, "t");
			if (t.IsValueType)
			{
				return IsNullableType(t);
			}
			return true;
		}

		public static bool IsNullableType(global::System.Type t)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(t, "t");
			if (t.IsGenericType)
			{
				return t.GetGenericTypeDefinition() == typeof(global::System.Nullable<>);
			}
			return false;
		}

		public static global::System.Type EnsureNotNullableType(global::System.Type t)
		{
			if (!IsNullableType(t))
			{
				return t;
			}
			return global::System.Nullable.GetUnderlyingType(t);
		}

		public static bool IsUnitializedValue(object value)
		{
			if (value == null)
			{
				return true;
			}
			object obj = CreateUnitializedValue(value.GetType());
			return value.Equals(obj);
		}

		public static object CreateUnitializedValue(global::System.Type type)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsGenericTypeDefinition)
			{
				throw new global::System.ArgumentException("Type {0} is a generic type definition and cannot be instantiated.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, type), "type");
			}
			if (type.IsClass || type.IsInterface || type == typeof(void))
			{
				return null;
			}
			if (type.IsValueType)
			{
				return global::System.Activator.CreateInstance(type);
			}
			throw new global::System.ArgumentException("Type {0} cannot be instantiated.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, type), "type");
		}

		public static bool IsPropertyIndexed(global::System.Reflection.PropertyInfo property)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(property, "property");
			return !global::Newtonsoft.Json.Utilities.CollectionUtils.IsNullOrEmpty((global::System.Collections.Generic.ICollection<global::System.Reflection.ParameterInfo>)property.GetIndexParameters());
		}

		public static bool ImplementsGenericDefinition(global::System.Type type, global::System.Type genericInterfaceDefinition)
		{
			global::System.Type implementingType;
			return ImplementsGenericDefinition(type, genericInterfaceDefinition, out implementingType);
		}

		public static bool ImplementsGenericDefinition(global::System.Type type, global::System.Type genericInterfaceDefinition, out global::System.Type implementingType)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
			if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
			{
				throw new global::System.ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, genericInterfaceDefinition));
			}
			if (type.IsInterface && type.IsGenericType)
			{
				global::System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericInterfaceDefinition == genericTypeDefinition)
				{
					implementingType = type;
					return true;
				}
			}
			global::System.Type[] interfaces = type.GetInterfaces();
			foreach (global::System.Type type2 in interfaces)
			{
				if (type2.IsGenericType)
				{
					global::System.Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
					if (genericInterfaceDefinition == genericTypeDefinition2)
					{
						implementingType = type2;
						return true;
					}
				}
			}
			implementingType = null;
			return false;
		}

		public static bool AssignableToTypeName(this global::System.Type type, string fullTypeName, out global::System.Type match)
		{
			for (global::System.Type type2 = type; type2 != null; type2 = type2.BaseType)
			{
				if (string.Equals(type2.FullName, fullTypeName, global::System.StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
			}
			global::System.Type[] interfaces = type.GetInterfaces();
			foreach (global::System.Type type3 in interfaces)
			{
				if (string.Equals(type3.Name, fullTypeName, global::System.StringComparison.Ordinal))
				{
					match = type;
					return true;
				}
			}
			match = null;
			return false;
		}

		public static bool AssignableToTypeName(this global::System.Type type, string fullTypeName)
		{
			global::System.Type match;
			return type.AssignableToTypeName(fullTypeName, out match);
		}

		public static bool InheritsGenericDefinition(global::System.Type type, global::System.Type genericClassDefinition)
		{
			global::System.Type implementingType;
			return InheritsGenericDefinition(type, genericClassDefinition, out implementingType);
		}

		public static bool InheritsGenericDefinition(global::System.Type type, global::System.Type genericClassDefinition, out global::System.Type implementingType)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
			if (!genericClassDefinition.IsClass || !genericClassDefinition.IsGenericTypeDefinition)
			{
				throw new global::System.ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, genericClassDefinition));
			}
			return InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
		}

		private static bool InheritsGenericDefinitionInternal(global::System.Type currentType, global::System.Type genericClassDefinition, out global::System.Type implementingType)
		{
			if (currentType.IsGenericType)
			{
				global::System.Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
				if (genericClassDefinition == genericTypeDefinition)
				{
					implementingType = currentType;
					return true;
				}
			}
			if (currentType.BaseType == null)
			{
				implementingType = null;
				return false;
			}
			return InheritsGenericDefinitionInternal(currentType.BaseType, genericClassDefinition, out implementingType);
		}

		public static global::System.Type GetCollectionItemType(global::System.Type type)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			global::System.Type implementingType;
			if (ImplementsGenericDefinition(type, typeof(global::System.Collections.Generic.IEnumerable<>), out implementingType))
			{
				if (implementingType.IsGenericTypeDefinition)
				{
					throw new global::System.Exception("Type {0} is not a collection.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, type));
				}
				return implementingType.GetGenericArguments()[0];
			}
			if (typeof(global::System.Collections.IEnumerable).IsAssignableFrom(type))
			{
				return null;
			}
			throw new global::System.Exception("Type {0} is not a collection.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, type));
		}

		public static void GetDictionaryKeyValueTypes(global::System.Type dictionaryType, out global::System.Type keyType, out global::System.Type valueType)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(dictionaryType, "type");
			global::System.Type implementingType;
			if (ImplementsGenericDefinition(dictionaryType, typeof(global::System.Collections.Generic.IDictionary<, >), out implementingType))
			{
				if (implementingType.IsGenericTypeDefinition)
				{
					throw new global::System.Exception("Type {0} is not a dictionary.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, dictionaryType));
				}
				global::System.Type[] genericArguments = implementingType.GetGenericArguments();
				keyType = genericArguments[0];
				valueType = genericArguments[1];
			}
			else
			{
				if (!typeof(global::System.Collections.IDictionary).IsAssignableFrom(dictionaryType))
				{
					throw new global::System.Exception("Type {0} is not a dictionary.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, dictionaryType));
				}
				keyType = null;
				valueType = null;
			}
		}

		public static global::System.Type GetDictionaryValueType(global::System.Type dictionaryType)
		{
			global::System.Type keyType;
			global::System.Type valueType;
			GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);
			return valueType;
		}

		public static global::System.Type GetDictionaryKeyType(global::System.Type dictionaryType)
		{
			global::System.Type keyType;
			global::System.Type valueType;
			GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);
			return keyType;
		}

		public static bool ItemsUnitializedValue<T>(global::System.Collections.Generic.IList<T> list)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(list, "list");
			global::System.Type collectionItemType = GetCollectionItemType(list.GetType());
			if (collectionItemType.IsValueType)
			{
				object obj = CreateUnitializedValue(collectionItemType);
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].Equals(obj))
					{
						return false;
					}
				}
			}
			else
			{
				if (!collectionItemType.IsClass)
				{
					throw new global::System.Exception("Type {0} is neither a ValueType or a Class.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, collectionItemType));
				}
				for (int j = 0; j < list.Count; j++)
				{
					object obj2 = list[j];
					if (obj2 != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static global::System.Type GetMemberUnderlyingType(global::System.Reflection.MemberInfo member)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(member, "member");
			switch (member.MemberType)
			{
			case global::System.Reflection.MemberTypes.Field:
				return ((global::System.Reflection.FieldInfo)member).FieldType;
			case global::System.Reflection.MemberTypes.Property:
				return ((global::System.Reflection.PropertyInfo)member).PropertyType;
			case global::System.Reflection.MemberTypes.Event:
				return ((global::System.Reflection.EventInfo)member).EventHandlerType;
			default:
				throw new global::System.ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or EventInfo", "member");
			}
		}

		public static bool IsIndexedProperty(global::System.Reflection.MemberInfo member)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(member, "member");
			global::System.Reflection.PropertyInfo propertyInfo = member as global::System.Reflection.PropertyInfo;
			if (propertyInfo != null)
			{
				return IsIndexedProperty(propertyInfo);
			}
			return false;
		}

		public static bool IsIndexedProperty(global::System.Reflection.PropertyInfo property)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(property, "property");
			return property.GetIndexParameters().Length > 0;
		}

		public static object GetMemberValue(global::System.Reflection.MemberInfo member, object target)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(member, "member");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(target, "target");
			switch (member.MemberType)
			{
			case global::System.Reflection.MemberTypes.Field:
				return ((global::System.Reflection.FieldInfo)member).GetValue(target);
			case global::System.Reflection.MemberTypes.Property:
				try
				{
					global::System.Reflection.MethodInfo getMethod = ((global::System.Reflection.PropertyInfo)member).GetGetMethod(true);
					return getMethod.Invoke(target, null);
				}
				catch (global::System.Reflection.TargetParameterCountException innerException)
				{
					throw new global::System.ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, member.Name), innerException);
				}
			default:
				throw new global::System.ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, global::System.Globalization.CultureInfo.InvariantCulture, member.Name), "member");
			}
		}

		public static void SetMemberValue(global::System.Reflection.MemberInfo member, object target, object value)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(member, "member");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(target, "target");
			switch (member.MemberType)
			{
			case global::System.Reflection.MemberTypes.Field:
				((global::System.Reflection.FieldInfo)member).SetValue(target, value);
				break;
			case global::System.Reflection.MemberTypes.Property:
				((global::System.Reflection.PropertyInfo)member).SetValue(target, value, null);
				break;
			default:
				throw new global::System.ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, member.Name), "member");
			}
		}

		public static bool CanReadMemberValue(global::System.Reflection.MemberInfo member, bool nonPublic)
		{
			switch (member.MemberType)
			{
			case global::System.Reflection.MemberTypes.Field:
			{
				global::System.Reflection.FieldInfo fieldInfo = (global::System.Reflection.FieldInfo)member;
				if (nonPublic)
				{
					return true;
				}
				if (fieldInfo.IsPublic)
				{
					return true;
				}
				return false;
			}
			case global::System.Reflection.MemberTypes.Property:
			{
				global::System.Reflection.PropertyInfo propertyInfo = (global::System.Reflection.PropertyInfo)member;
				if (!propertyInfo.CanRead)
				{
					return false;
				}
				if (nonPublic)
				{
					return true;
				}
				return propertyInfo.GetGetMethod(nonPublic) != null;
			}
			default:
				return false;
			}
		}

		public static bool CanSetMemberValue(global::System.Reflection.MemberInfo member, bool nonPublic, bool canSetReadOnly)
		{
			switch (member.MemberType)
			{
			case global::System.Reflection.MemberTypes.Field:
			{
				global::System.Reflection.FieldInfo fieldInfo = (global::System.Reflection.FieldInfo)member;
				if (fieldInfo.IsInitOnly && !canSetReadOnly)
				{
					return false;
				}
				if (nonPublic)
				{
					return true;
				}
				if (fieldInfo.IsPublic)
				{
					return true;
				}
				return false;
			}
			case global::System.Reflection.MemberTypes.Property:
			{
				global::System.Reflection.PropertyInfo propertyInfo = (global::System.Reflection.PropertyInfo)member;
				if (!propertyInfo.CanWrite)
				{
					return false;
				}
				if (nonPublic)
				{
					return true;
				}
				return propertyInfo.GetSetMethod(nonPublic) != null;
			}
			default:
				return false;
			}
		}

		public static global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> GetFieldsAndProperties<T>(global::System.Reflection.BindingFlags bindingAttr)
		{
			return GetFieldsAndProperties(typeof(T), bindingAttr);
		}

		public static global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> GetFieldsAndProperties(global::System.Type type, global::System.Reflection.BindingFlags bindingAttr)
		{
			global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> list = new global::System.Collections.Generic.List<global::System.Reflection.MemberInfo>();
			list.AddRange(GetFields(type, bindingAttr));
			list.AddRange(GetProperties(type, bindingAttr));
			global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> list2 = new global::System.Collections.Generic.List<global::System.Reflection.MemberInfo>(list.Count);
			var enumerable = global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.GroupBy(list, (global::System.Reflection.MemberInfo m) => m.Name), (global::System.Linq.IGrouping<string, global::System.Reflection.MemberInfo> g) => new
			{
				Count = global::System.Linq.Enumerable.Count(g),
				Members = global::System.Linq.Enumerable.Cast<global::System.Reflection.MemberInfo>(g)
			});
			foreach (var item in enumerable)
			{
				if (item.Count == 1)
				{
					list2.Add(global::System.Linq.Enumerable.First(item.Members));
					continue;
				}
				global::System.Collections.Generic.IEnumerable<global::System.Reflection.MemberInfo> collection = global::System.Linq.Enumerable.Where(item.Members, (global::System.Reflection.MemberInfo m) => !IsOverridenGenericMember(m, bindingAttr) || m.Name == "Item");
				list2.AddRange(collection);
			}
			return list2;
		}

		private static bool IsOverridenGenericMember(global::System.Reflection.MemberInfo memberInfo, global::System.Reflection.BindingFlags bindingAttr)
		{
			if (memberInfo.MemberType != global::System.Reflection.MemberTypes.Field && memberInfo.MemberType != global::System.Reflection.MemberTypes.Property)
			{
				throw new global::System.ArgumentException("Member must be a field or property.");
			}
			global::System.Type declaringType = memberInfo.DeclaringType;
			if (!declaringType.IsGenericType)
			{
				return false;
			}
			global::System.Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
			if (genericTypeDefinition == null)
			{
				return false;
			}
			global::System.Reflection.MemberInfo[] member = genericTypeDefinition.GetMember(memberInfo.Name, bindingAttr);
			if (member.Length == 0)
			{
				return false;
			}
			global::System.Type memberUnderlyingType = GetMemberUnderlyingType(member[0]);
			if (!memberUnderlyingType.IsGenericParameter)
			{
				return false;
			}
			return true;
		}

		public static T GetAttribute<T>(global::System.Reflection.ICustomAttributeProvider attributeProvider) where T : global::System.Attribute
		{
			return GetAttribute<T>(attributeProvider, true);
		}

		public static T GetAttribute<T>(global::System.Reflection.ICustomAttributeProvider attributeProvider, bool inherit) where T : global::System.Attribute
		{
			T[] attributes = GetAttributes<T>(attributeProvider, inherit);
			return global::Newtonsoft.Json.Utilities.CollectionUtils.GetSingleItem(attributes, true);
		}

		public static T[] GetAttributes<T>(global::System.Reflection.ICustomAttributeProvider attributeProvider, bool inherit) where T : global::System.Attribute
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			if (attributeProvider is global::System.Type)
			{
				return (T[])((global::System.Type)attributeProvider).GetCustomAttributes(typeof(T), inherit);
			}
			if (attributeProvider is global::System.Reflection.Assembly)
			{
				return (T[])global::System.Attribute.GetCustomAttributes((global::System.Reflection.Assembly)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is global::System.Reflection.MemberInfo)
			{
				return (T[])global::System.Attribute.GetCustomAttributes((global::System.Reflection.MemberInfo)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is global::System.Reflection.Module)
			{
				return (T[])global::System.Attribute.GetCustomAttributes((global::System.Reflection.Module)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is global::System.Reflection.ParameterInfo)
			{
				return (T[])global::System.Attribute.GetCustomAttributes((global::System.Reflection.ParameterInfo)attributeProvider, typeof(T), inherit);
			}
			return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
		}

		public static string GetNameAndAssessmblyName(global::System.Type t)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(t, "t");
			return t.FullName + ", " + t.Assembly.GetName().Name;
		}

		public static global::System.Type MakeGenericType(global::System.Type genericTypeDefinition, params global::System.Type[] innerTypes)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNullOrEmpty((global::System.Collections.Generic.ICollection<global::System.Type>)innerTypes, "innerTypes");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentConditionTrue(genericTypeDefinition.IsGenericTypeDefinition, "genericTypeDefinition", "Type {0} is not a generic type definition.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, genericTypeDefinition));
			return genericTypeDefinition.MakeGenericType(innerTypes);
		}

		public static object CreateGeneric(global::System.Type genericTypeDefinition, global::System.Type innerType, params object[] args)
		{
			return CreateGeneric(genericTypeDefinition, new global::System.Type[1] { innerType }, args);
		}

		public static object CreateGeneric(global::System.Type genericTypeDefinition, global::System.Collections.Generic.IList<global::System.Type> innerTypes, params object[] args)
		{
			return CreateGeneric(genericTypeDefinition, innerTypes, (global::System.Type t, global::System.Collections.Generic.IList<object> a) => CreateInstance(t, global::System.Linq.Enumerable.ToArray(a)), args);
		}

		public static object CreateGeneric(global::System.Type genericTypeDefinition, global::System.Collections.Generic.IList<global::System.Type> innerTypes, global::System.Func<global::System.Type, global::System.Collections.Generic.IList<object>, object> instanceCreator, params object[] args)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNullOrEmpty(innerTypes, "innerTypes");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(instanceCreator, "createInstance");
			global::System.Type arg = MakeGenericType(genericTypeDefinition, global::System.Linq.Enumerable.ToArray(innerTypes));
			return instanceCreator(arg, args);
		}

		public static bool IsCompatibleValue(object value, global::System.Type type)
		{
			if (value == null)
			{
				return IsNullable(type);
			}
			if (type.IsAssignableFrom(value.GetType()))
			{
				return true;
			}
			return false;
		}

		public static object CreateInstance(global::System.Type type, params object[] args)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			return global::System.Activator.CreateInstance(type, args);
		}

		public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
		{
			int? assemblyDelimiterIndex = GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
			if (assemblyDelimiterIndex.HasValue)
			{
				typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
				assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
			}
			else
			{
				typeName = fullyQualifiedTypeName;
				assemblyName = null;
			}
		}

		private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
		{
			int num = 0;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				switch (fullyQualifiedTypeName[i])
				{
				case '[':
					num++;
					break;
				case ']':
					num--;
					break;
				case ',':
					if (num == 0)
					{
						return i;
					}
					break;
				}
			}
			return null;
		}

		public static global::System.Reflection.MemberInfo GetMemberInfoFromType(global::System.Type targetType, global::System.Reflection.MemberInfo memberInfo)
		{
			global::System.Reflection.BindingFlags bindingAttr = global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic;
			global::System.Reflection.MemberTypes memberType = memberInfo.MemberType;
			if (memberType == global::System.Reflection.MemberTypes.Property)
			{
				global::System.Reflection.PropertyInfo propertyInfo = (global::System.Reflection.PropertyInfo)memberInfo;
				global::System.Type[] types = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(propertyInfo.GetIndexParameters(), (global::System.Reflection.ParameterInfo p) => p.ParameterType));
				return targetType.GetProperty(propertyInfo.Name, bindingAttr, null, propertyInfo.PropertyType, types, null);
			}
			return global::System.Linq.Enumerable.SingleOrDefault(targetType.GetMember(memberInfo.Name, memberInfo.MemberType, bindingAttr));
		}

		public static global::System.Collections.Generic.IEnumerable<global::System.Reflection.FieldInfo> GetFields(global::System.Type targetType, global::System.Reflection.BindingFlags bindingAttr)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(targetType, "targetType");
			global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> list = new global::System.Collections.Generic.List<global::System.Reflection.MemberInfo>(targetType.GetFields(bindingAttr));
			GetChildPrivateFields(list, targetType, bindingAttr);
			return global::System.Linq.Enumerable.Cast<global::System.Reflection.FieldInfo>(list);
		}

		private static void GetChildPrivateFields(global::System.Collections.Generic.IList<global::System.Reflection.MemberInfo> initialFields, global::System.Type targetType, global::System.Reflection.BindingFlags bindingAttr)
		{
			if ((bindingAttr & global::System.Reflection.BindingFlags.NonPublic) == 0)
			{
				return;
			}
			global::System.Reflection.BindingFlags bindingAttr2 = bindingAttr.RemoveFlag(global::System.Reflection.BindingFlags.Public);
			while ((targetType = targetType.BaseType) != null)
			{
				global::System.Collections.Generic.IEnumerable<global::System.Reflection.MemberInfo> collection = global::System.Linq.Enumerable.Cast<global::System.Reflection.MemberInfo>(global::System.Linq.Enumerable.Where(targetType.GetFields(bindingAttr2), (global::System.Reflection.FieldInfo f) => f.IsPrivate));
				initialFields.AddRange(collection);
			}
		}

		public static global::System.Collections.Generic.IEnumerable<global::System.Reflection.PropertyInfo> GetProperties(global::System.Type targetType, global::System.Reflection.BindingFlags bindingAttr)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(targetType, "targetType");
			global::System.Collections.Generic.List<global::System.Reflection.PropertyInfo> list = new global::System.Collections.Generic.List<global::System.Reflection.PropertyInfo>(targetType.GetProperties(bindingAttr));
			GetChildPrivateProperties(list, targetType, bindingAttr);
			for (int i = 0; i < list.Count; i++)
			{
				global::System.Reflection.PropertyInfo propertyInfo = list[i];
				if (propertyInfo.DeclaringType != targetType)
				{
					global::System.Reflection.PropertyInfo value = (global::System.Reflection.PropertyInfo)GetMemberInfoFromType(propertyInfo.DeclaringType, propertyInfo);
					list[i] = value;
				}
			}
			return list;
		}

		public static global::System.Reflection.BindingFlags RemoveFlag(this global::System.Reflection.BindingFlags bindingAttr, global::System.Reflection.BindingFlags flag)
		{
			if ((bindingAttr & flag) != flag)
			{
				return bindingAttr;
			}
			return bindingAttr ^ flag;
		}

		private static void GetChildPrivateProperties(global::System.Collections.Generic.IList<global::System.Reflection.PropertyInfo> initialProperties, global::System.Type targetType, global::System.Reflection.BindingFlags bindingAttr)
		{
			if ((bindingAttr & global::System.Reflection.BindingFlags.NonPublic) == 0)
			{
				return;
			}
			global::System.Reflection.BindingFlags bindingAttr2 = bindingAttr.RemoveFlag(global::System.Reflection.BindingFlags.Public);
			while ((targetType = targetType.BaseType) != null)
			{
				global::System.Reflection.PropertyInfo[] properties = targetType.GetProperties(bindingAttr2);
				foreach (global::System.Reflection.PropertyInfo propertyInfo in properties)
				{
					global::System.Reflection.PropertyInfo nonPublicProperty = propertyInfo;
					int num = initialProperties.IndexOf((global::System.Reflection.PropertyInfo p) => p.Name == nonPublicProperty.Name);
					if (num == -1)
					{
						initialProperties.Add(nonPublicProperty);
					}
					else
					{
						initialProperties[num] = nonPublicProperty;
					}
				}
			}
		}
	}
}
