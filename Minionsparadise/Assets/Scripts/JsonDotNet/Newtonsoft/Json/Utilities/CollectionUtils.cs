namespace Newtonsoft.Json.Utilities
{
	internal static class CollectionUtils
	{
		public static global::System.Collections.Generic.IEnumerable<T> CastValid<T>(this global::System.Collections.IEnumerable enumerable)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			return global::System.Linq.Enumerable.Cast<T>(global::System.Linq.Enumerable.Where(global::System.Linq.Enumerable.Cast<object>(enumerable), (object o) => o is T));
		}

		public static global::System.Collections.Generic.List<T> CreateList<T>(params T[] values)
		{
			return new global::System.Collections.Generic.List<T>(values);
		}

		public static bool IsNullOrEmpty(global::System.Collections.ICollection collection)
		{
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return true;
		}

		public static bool IsNullOrEmpty<T>(global::System.Collections.Generic.ICollection<T> collection)
		{
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return true;
		}

		public static bool IsNullOrEmptyOrDefault<T>(global::System.Collections.Generic.IList<T> list)
		{
			if (IsNullOrEmpty(list))
			{
				return true;
			}
			return global::Newtonsoft.Json.Utilities.ReflectionUtils.ItemsUnitializedValue(list);
		}

		public static global::System.Collections.Generic.IList<T> Slice<T>(global::System.Collections.Generic.IList<T> list, int? start, int? end)
		{
			return Slice(list, start, end, null);
		}

		public static global::System.Collections.Generic.IList<T> Slice<T>(global::System.Collections.Generic.IList<T> list, int? start, int? end, int? step)
		{
			if (list == null)
			{
				throw new global::System.ArgumentNullException("list");
			}
			int? num = step;
			if (num.GetValueOrDefault() == 0 && num.HasValue)
			{
				throw new global::System.ArgumentException("Step cannot be zero.", "step");
			}
			global::System.Collections.Generic.List<T> list2 = new global::System.Collections.Generic.List<T>();
			if (list.Count == 0)
			{
				return list2;
			}
			int num2 = step ?? 1;
			int num3 = start ?? 0;
			int num4 = end ?? list.Count;
			num3 = ((num3 < 0) ? (list.Count + num3) : num3);
			num4 = ((num4 < 0) ? (list.Count + num4) : num4);
			num3 = global::System.Math.Max(num3, 0);
			num4 = global::System.Math.Min(num4, list.Count - 1);
			for (int i = num3; i < num4; i += num2)
			{
				list2.Add(list[i]);
			}
			return list2;
		}

		public static global::System.Collections.Generic.Dictionary<K, global::System.Collections.Generic.List<V>> GroupBy<K, V>(global::System.Collections.Generic.ICollection<V> source, global::System.Func<V, K> keySelector)
		{
			if (keySelector == null)
			{
				throw new global::System.ArgumentNullException("keySelector");
			}
			global::System.Collections.Generic.Dictionary<K, global::System.Collections.Generic.List<V>> dictionary = new global::System.Collections.Generic.Dictionary<K, global::System.Collections.Generic.List<V>>();
			V[] array = global::System.Linq.Enumerable.ToArray(source);
			foreach (V val in array)
			{
				K key = keySelector(val);
				global::System.Collections.Generic.List<V> value;
				if (!dictionary.TryGetValue(key, out value))
				{
					value = new global::System.Collections.Generic.List<V>();
					dictionary.Add(key, value);
				}
				value.Add(val);
			}
			return dictionary;
		}

		public static void AddRange<T>(this global::System.Collections.Generic.IList<T> initial, global::System.Collections.Generic.IEnumerable<T> collection)
		{
			if (initial == null)
			{
				throw new global::System.ArgumentNullException("initial");
			}
			if (collection != null)
			{
				T[] array = global::System.Linq.Enumerable.ToArray(collection);
				for (int i = 0; i < array.Length; i++)
				{
					initial.Add(array[i]);
				}
			}
		}

		public static void AddRange(this global::System.Collections.IList initial, global::System.Collections.IEnumerable collection)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(initial, "initial");
			global::Newtonsoft.Json.Utilities.ListWrapper<object> initial2 = new global::Newtonsoft.Json.Utilities.ListWrapper<object>(initial);
			initial2.AddRange(global::System.Linq.Enumerable.Cast<object>(collection));
		}

		public static global::System.Collections.Generic.List<T> Distinct<T>(global::System.Collections.Generic.List<T> collection)
		{
			global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
			T[] array = collection.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (!list.Contains(array[i]))
				{
					list.Add(array[i]);
				}
			}
			return list;
		}

		public static global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> Flatten<T>(params global::System.Collections.Generic.IList<T>[] lists)
		{
			global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> list = new global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>>();
			global::System.Collections.Generic.Dictionary<int, T> currentSet = new global::System.Collections.Generic.Dictionary<int, T>();
			Recurse(new global::System.Collections.Generic.List<global::System.Collections.Generic.IList<T>>(lists), 0, currentSet, list);
			return list;
		}

		private static void Recurse<T>(global::System.Collections.Generic.IList<global::System.Collections.Generic.IList<T>> global, int current, global::System.Collections.Generic.Dictionary<int, T> currentSet, global::System.Collections.Generic.List<global::System.Collections.Generic.List<T>> flattenedResult)
		{
			global::System.Collections.Generic.IList<T> list = global[current];
			for (int i = 0; i < list.Count; i++)
			{
				currentSet[current] = list[i];
				if (current == global.Count - 1)
				{
					global::System.Collections.Generic.List<T> list2 = new global::System.Collections.Generic.List<T>();
					for (int j = 0; j < currentSet.Count; j++)
					{
						list2.Add(currentSet[j]);
					}
					flattenedResult.Add(list2);
				}
				else
				{
					Recurse(global, current + 1, currentSet, flattenedResult);
				}
			}
		}

		public static global::System.Collections.Generic.List<T> CreateList<T>(global::System.Collections.ICollection collection)
		{
			if (collection == null)
			{
				throw new global::System.ArgumentNullException("collection");
			}
			T[] array = new T[collection.Count];
			collection.CopyTo(array, 0);
			return new global::System.Collections.Generic.List<T>(array);
		}

		public static bool ListEquals<T>(global::System.Collections.Generic.IList<T> a, global::System.Collections.Generic.IList<T> b)
		{
			if (a == null || b == null)
			{
				if (a == null)
				{
					return b == null;
				}
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			global::System.Collections.Generic.EqualityComparer<T> equalityComparer = global::System.Collections.Generic.EqualityComparer<T>.Default;
			for (int i = 0; i < a.Count; i++)
			{
				if (!equalityComparer.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool TryGetSingleItem<T>(global::System.Collections.Generic.IList<T> list, out T value)
		{
			return TryGetSingleItem(list, false, out value);
		}

		public static bool TryGetSingleItem<T>(global::System.Collections.Generic.IList<T> list, bool returnDefaultIfEmpty, out T value)
		{
			return global::Newtonsoft.Json.Utilities.MiscellaneousUtils.TryAction(() => GetSingleItem(list, returnDefaultIfEmpty), out value);
		}

		public static T GetSingleItem<T>(global::System.Collections.Generic.IList<T> list)
		{
			return GetSingleItem(list, false);
		}

		public static T GetSingleItem<T>(global::System.Collections.Generic.IList<T> list, bool returnDefaultIfEmpty)
		{
			if (list.Count == 1)
			{
				return list[0];
			}
			if (returnDefaultIfEmpty && list.Count == 0)
			{
				return default(T);
			}
			throw new global::System.Exception("Expected single {0} in list but got {1}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, typeof(T), list.Count));
		}

		public static global::System.Collections.Generic.IList<T> Minus<T>(global::System.Collections.Generic.IList<T> list, global::System.Collections.Generic.IList<T> minus)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(list, "list");
			global::System.Collections.Generic.List<T> list2 = new global::System.Collections.Generic.List<T>(list.Count);
			T[] array = global::System.Linq.Enumerable.ToArray(list);
			for (int i = 0; i < array.Length; i++)
			{
				if (minus == null || !minus.Contains(array[i]))
				{
					list2.Add(array[i]);
				}
			}
			return list2;
		}

		public static global::System.Collections.IList CreateGenericList(global::System.Type listType)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(listType, "listType");
			return (global::System.Collections.IList)global::Newtonsoft.Json.Utilities.ReflectionUtils.CreateGeneric(typeof(global::System.Collections.Generic.List<>), listType);
		}

		public static global::System.Collections.IDictionary CreateGenericDictionary(global::System.Type keyType, global::System.Type valueType)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(keyType, "keyType");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(valueType, "valueType");
			return (global::System.Collections.IDictionary)global::Newtonsoft.Json.Utilities.ReflectionUtils.CreateGeneric(typeof(global::System.Collections.Generic.Dictionary<, >), keyType, valueType);
		}

		public static bool IsListType(global::System.Type type)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return true;
			}
			if (typeof(global::System.Collections.IList).IsAssignableFrom(type))
			{
				return true;
			}
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(type, typeof(global::System.Collections.Generic.IList<>)))
			{
				return true;
			}
			return false;
		}

		public static bool IsCollectionType(global::System.Type type)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return true;
			}
			if (typeof(global::System.Collections.ICollection).IsAssignableFrom(type))
			{
				return true;
			}
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(type, typeof(global::System.Collections.Generic.ICollection<>)))
			{
				return true;
			}
			return false;
		}

		public static bool IsDictionaryType(global::System.Type type)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			if (typeof(global::System.Collections.IDictionary).IsAssignableFrom(type))
			{
				return true;
			}
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(type, typeof(global::System.Collections.Generic.IDictionary<, >)))
			{
				return true;
			}
			return false;
		}

		public static global::Newtonsoft.Json.Utilities.IWrappedCollection CreateCollectionWrapper(object list)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(list, "list");
			global::System.Type collectionDefinition;
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof(global::System.Collections.Generic.ICollection<>), out collectionDefinition))
			{
				global::System.Type collectionItemType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetCollectionItemType(collectionDefinition);
				global::System.Func<global::System.Type, global::System.Collections.Generic.IList<object>, object> instanceCreator = delegate(global::System.Type t, global::System.Collections.Generic.IList<object> a)
				{
					global::System.Reflection.ConstructorInfo constructor = t.GetConstructor(new global::System.Type[1] { collectionDefinition });
					return constructor.Invoke(new object[1] { list });
				};
				return (global::Newtonsoft.Json.Utilities.IWrappedCollection)global::Newtonsoft.Json.Utilities.ReflectionUtils.CreateGeneric(typeof(global::Newtonsoft.Json.Utilities.CollectionWrapper<>), new global::System.Type[1] { collectionItemType }, instanceCreator, list);
			}
			if (list is global::System.Collections.IList)
			{
				return new global::Newtonsoft.Json.Utilities.CollectionWrapper<object>((global::System.Collections.IList)list);
			}
			throw new global::System.Exception("Can not create ListWrapper for type {0}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, list.GetType()));
		}

		public static global::Newtonsoft.Json.Utilities.IWrappedList CreateListWrapper(object list)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(list, "list");
			global::System.Type listDefinition;
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof(global::System.Collections.Generic.IList<>), out listDefinition))
			{
				global::System.Type collectionItemType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetCollectionItemType(listDefinition);
				global::System.Func<global::System.Type, global::System.Collections.Generic.IList<object>, object> instanceCreator = delegate(global::System.Type t, global::System.Collections.Generic.IList<object> a)
				{
					global::System.Reflection.ConstructorInfo constructor = t.GetConstructor(new global::System.Type[1] { listDefinition });
					return constructor.Invoke(new object[1] { list });
				};
				return (global::Newtonsoft.Json.Utilities.IWrappedList)global::Newtonsoft.Json.Utilities.ReflectionUtils.CreateGeneric(typeof(global::Newtonsoft.Json.Utilities.ListWrapper<>), new global::System.Type[1] { collectionItemType }, instanceCreator, list);
			}
			if (list is global::System.Collections.IList)
			{
				return new global::Newtonsoft.Json.Utilities.ListWrapper<object>((global::System.Collections.IList)list);
			}
			throw new global::System.Exception("Can not create ListWrapper for type {0}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, list.GetType()));
		}

		public static global::Newtonsoft.Json.Utilities.IWrappedDictionary CreateDictionaryWrapper(object dictionary)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			global::System.Type dictionaryDefinition;
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(dictionary.GetType(), typeof(global::System.Collections.Generic.IDictionary<, >), out dictionaryDefinition))
			{
				global::System.Type dictionaryKeyType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetDictionaryKeyType(dictionaryDefinition);
				global::System.Type dictionaryValueType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetDictionaryValueType(dictionaryDefinition);
				global::System.Func<global::System.Type, global::System.Collections.Generic.IList<object>, object> instanceCreator = delegate(global::System.Type t, global::System.Collections.Generic.IList<object> a)
				{
					global::System.Reflection.ConstructorInfo constructor = t.GetConstructor(new global::System.Type[1] { dictionaryDefinition });
					return constructor.Invoke(new object[1] { dictionary });
				};
				return (global::Newtonsoft.Json.Utilities.IWrappedDictionary)global::Newtonsoft.Json.Utilities.ReflectionUtils.CreateGeneric(typeof(global::Newtonsoft.Json.Utilities.DictionaryWrapper<, >), new global::System.Type[2] { dictionaryKeyType, dictionaryValueType }, instanceCreator, dictionary);
			}
			if (dictionary is global::System.Collections.IDictionary)
			{
				return new global::Newtonsoft.Json.Utilities.DictionaryWrapper<object, object>((global::System.Collections.IDictionary)dictionary);
			}
			throw new global::System.Exception("Can not create DictionaryWrapper for type {0}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, dictionary.GetType()));
		}

		public static object CreateAndPopulateList(global::System.Type listType, global::System.Action<global::System.Collections.IList, bool> populateList)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(listType, "listType");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(populateList, "populateList");
			bool flag = false;
			global::System.Collections.IList list;
			global::System.Type implementingType;
			if (listType.IsArray)
			{
				list = new global::System.Collections.Generic.List<object>();
				flag = true;
			}
			else if (!global::Newtonsoft.Json.Utilities.ReflectionUtils.InheritsGenericDefinition(listType, typeof(global::System.Collections.ObjectModel.ReadOnlyCollection<>), out implementingType))
			{
				list = (typeof(global::System.Collections.IList).IsAssignableFrom(listType) ? (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsInstantiatableType(listType) ? ((global::System.Collections.IList)global::System.Activator.CreateInstance(listType)) : ((listType != typeof(global::System.Collections.IList)) ? null : new global::System.Collections.Generic.List<object>())) : ((!global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(listType, typeof(global::System.Collections.Generic.ICollection<>))) ? null : ((!global::Newtonsoft.Json.Utilities.ReflectionUtils.IsInstantiatableType(listType)) ? null : CreateCollectionWrapper(global::System.Activator.CreateInstance(listType)))));
			}
			else
			{
				global::System.Type type = implementingType.GetGenericArguments()[0];
				global::System.Type type2 = global::Newtonsoft.Json.Utilities.ReflectionUtils.MakeGenericType(typeof(global::System.Collections.Generic.IEnumerable<>), type);
				bool flag2 = false;
				global::System.Reflection.ConstructorInfo[] constructors = listType.GetConstructors();
				foreach (global::System.Reflection.ConstructorInfo constructorInfo in constructors)
				{
					global::System.Collections.Generic.IList<global::System.Reflection.ParameterInfo> parameters = constructorInfo.GetParameters();
					if (parameters.Count == 1 && type2.IsAssignableFrom(parameters[0].ParameterType))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					throw new global::System.Exception("Read-only type {0} does not have a public constructor that takes a type that implements {1}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, listType, type2));
				}
				list = CreateGenericList(type);
				flag = true;
			}
			if (list == null)
			{
				throw new global::System.Exception("Cannot create and populate list type {0}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, listType));
			}
			populateList(list, flag);
			if (flag)
			{
				if (listType.IsArray)
				{
					list = ToArray(((global::System.Collections.Generic.List<object>)list).ToArray(), global::Newtonsoft.Json.Utilities.ReflectionUtils.GetCollectionItemType(listType));
				}
				else if (global::Newtonsoft.Json.Utilities.ReflectionUtils.InheritsGenericDefinition(listType, typeof(global::System.Collections.ObjectModel.ReadOnlyCollection<>)))
				{
					list = (global::System.Collections.IList)global::Newtonsoft.Json.Utilities.ReflectionUtils.CreateInstance(listType, list);
				}
			}
			else if (list is global::Newtonsoft.Json.Utilities.IWrappedCollection)
			{
				return ((global::Newtonsoft.Json.Utilities.IWrappedCollection)list).UnderlyingCollection;
			}
			return list;
		}

		public static global::System.Array ToArray(global::System.Array initial, global::System.Type type)
		{
			if (type == null)
			{
				throw new global::System.ArgumentNullException("type");
			}
			global::System.Array array = global::System.Array.CreateInstance(type, initial.Length);
			global::System.Array.Copy(initial, 0, array, 0, initial.Length);
			return array;
		}

		public static bool AddDistinct<T>(this global::System.Collections.Generic.IList<T> list, T value)
		{
			return list.AddDistinct(value, global::System.Collections.Generic.EqualityComparer<T>.Default);
		}

		public static bool AddDistinct<T>(this global::System.Collections.Generic.IList<T> list, T value, global::System.Collections.Generic.IEqualityComparer<T> comparer)
		{
			if (list.ContainsValue(value, comparer))
			{
				return false;
			}
			list.Add(value);
			return true;
		}

		public static bool ContainsValue<TSource>(this global::System.Collections.Generic.IEnumerable<TSource> source, TSource value, global::System.Collections.Generic.IEqualityComparer<TSource> comparer)
		{
			if (comparer == null)
			{
				comparer = global::System.Collections.Generic.EqualityComparer<TSource>.Default;
			}
			if (source == null)
			{
				throw new global::System.ArgumentNullException("source");
			}
			TSource[] array = global::System.Linq.Enumerable.ToArray(source);
			for (int i = 0; i < array.Length; i++)
			{
				if (comparer.Equals(array[i], value))
				{
					return true;
				}
			}
			return false;
		}

		public static bool AddRangeDistinct<T>(this global::System.Collections.Generic.IList<T> list, global::System.Collections.Generic.IEnumerable<T> values)
		{
			return list.AddRangeDistinct(values, global::System.Collections.Generic.EqualityComparer<T>.Default);
		}

		public static bool AddRangeDistinct<T>(this global::System.Collections.Generic.IList<T> list, global::System.Collections.Generic.IEnumerable<T> values, global::System.Collections.Generic.IEqualityComparer<T> comparer)
		{
			bool result = true;
			T[] array = global::System.Linq.Enumerable.ToArray(values);
			for (int i = 0; i < array.Length; i++)
			{
				if (!list.AddDistinct(array[i], comparer))
				{
					result = false;
				}
			}
			return result;
		}

		public static int IndexOf<T>(this global::System.Collections.Generic.IEnumerable<T> collection, global::System.Func<T, bool> predicate)
		{
			T[] array = global::System.Linq.Enumerable.ToArray(collection);
			for (int i = 0; i < array.Length; i++)
			{
				if (predicate(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public static int IndexOf<TSource>(this global::System.Collections.Generic.IEnumerable<TSource> list, TSource value) where TSource : global::System.IEquatable<TSource>
		{
			return list.IndexOf(value, global::System.Collections.Generic.EqualityComparer<TSource>.Default);
		}

		public static int IndexOf<TSource>(this global::System.Collections.Generic.IEnumerable<TSource> list, TSource value, global::System.Collections.Generic.IEqualityComparer<TSource> comparer)
		{
			TSource[] array = global::System.Linq.Enumerable.ToArray(list);
			for (int i = 0; i < array.Length; i++)
			{
				if (comparer.Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
