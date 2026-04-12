namespace strange.framework.impl
{
	public class Binder : global::strange.framework.api.IBinder
	{
		public delegate void BindingResolver(global::strange.framework.api.IBinding binding);

		protected global::System.Collections.Generic.Dictionary<object, global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding>> bindings;

		protected global::System.Collections.Generic.Dictionary<object, global::System.Collections.Generic.Dictionary<global::strange.framework.api.IBinding, object>> conflicts;

		public Binder()
		{
			bindings = new global::System.Collections.Generic.Dictionary<object, global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding>>();
			conflicts = new global::System.Collections.Generic.Dictionary<object, global::System.Collections.Generic.Dictionary<global::strange.framework.api.IBinding, object>>();
		}

		public virtual global::strange.framework.api.IBinding Bind<T>()
		{
			return Bind(typeof(T));
		}

		public virtual global::strange.framework.api.IBinding Bind(object key)
		{
			global::strange.framework.api.IBinding rawBinding = GetRawBinding();
			rawBinding.Bind(key);
			return rawBinding;
		}

		public virtual global::strange.framework.api.IBinding GetBinding<T>()
		{
			return GetBinding(typeof(T), null);
		}

		public virtual global::strange.framework.api.IBinding GetBinding(object key)
		{
			return GetBinding(key, null);
		}

		public virtual global::strange.framework.api.IBinding GetBinding<T>(object name)
		{
			return GetBinding(typeof(T), name);
		}

		public virtual global::strange.framework.api.IBinding GetBinding(object key, object name)
		{
			if (conflicts.Count > 0)
			{
				string text = "";
				global::System.Collections.Generic.Dictionary<object, global::System.Collections.Generic.Dictionary<global::strange.framework.api.IBinding, object>>.KeyCollection keys = conflicts.Keys;
				foreach (object item in keys)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += item.ToString();
				}
				throw new global::strange.framework.impl.BinderException("Binder cannot fetch Bindings when the binder is in a conflicted state.\nConflicts: " + text, global::strange.framework.api.BinderExceptionType.CONFLICT_IN_BINDER);
			}
			if (bindings.ContainsKey(key))
			{
				global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding> dictionary = bindings[key];
				name = ((name == null) ? global::strange.framework.api.BindingConst.NULLOID : name);
				if (dictionary.ContainsKey(name))
				{
					return dictionary[name];
				}
			}
			return null;
		}

		public virtual void Unbind<T>()
		{
			Unbind(typeof(T), null);
		}

		public virtual void Unbind(object key)
		{
			Unbind(key, null);
		}

		public virtual void Unbind<T>(object name)
		{
			Unbind(typeof(T), name);
		}

		public virtual void Unbind(object key, object name)
		{
			if (bindings.ContainsKey(key))
			{
				global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding> dictionary = bindings[key];
				object key2 = ((name == null) ? global::strange.framework.api.BindingConst.NULLOID : name);
				if (dictionary.ContainsKey(key2))
				{
					dictionary.Remove(key2);
				}
			}
		}

		public virtual void Unbind(global::strange.framework.api.IBinding binding)
		{
			if (binding != null)
			{
				Unbind(binding.key, binding.name);
			}
		}

		public virtual void RemoveValue(global::strange.framework.api.IBinding binding, object value)
		{
			if (binding == null || value == null)
			{
				return;
			}
			object key = binding.key;
			if (!bindings.ContainsKey(key))
			{
				return;
			}
			global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding> dictionary = bindings[key];
			if (dictionary.ContainsKey(binding.name))
			{
				global::strange.framework.api.IBinding binding2 = dictionary[binding.name];
				binding2.RemoveValue(value);
				object[] array = binding2.value as object[];
				if (array == null || array.Length == 0)
				{
					dictionary.Remove(binding2.name);
				}
			}
		}

		public virtual void RemoveKey(global::strange.framework.api.IBinding binding, object key)
		{
			if (binding == null || key == null || !bindings.ContainsKey(key))
			{
				return;
			}
			global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding> dictionary = bindings[key];
			if (dictionary.ContainsKey(binding.name))
			{
				global::strange.framework.api.IBinding binding2 = dictionary[binding.name];
				binding2.RemoveKey(key);
				object[] array = binding2.key as object[];
				if (array != null && array.Length == 0)
				{
					dictionary.Remove(binding.name);
				}
			}
		}

		public virtual void RemoveName(global::strange.framework.api.IBinding binding, object name)
		{
			if (binding != null && name != null)
			{
				object key;
				if (binding.keyConstraint.Equals(global::strange.framework.api.BindingConstraintType.ONE))
				{
					key = binding.key;
				}
				else
				{
					object[] array = binding.key as object[];
					key = array[0];
				}
				global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding> dictionary = bindings[key];
				if (dictionary.ContainsKey(name))
				{
					global::strange.framework.api.IBinding binding2 = dictionary[name];
					binding2.RemoveName(name);
				}
			}
		}

		public virtual global::strange.framework.api.IBinding GetRawBinding()
		{
			return new global::strange.framework.impl.Binding(resolver);
		}

		protected virtual void resolver(global::strange.framework.api.IBinding binding)
		{
			object key = binding.key;
			if (binding.keyConstraint.Equals(global::strange.framework.api.BindingConstraintType.ONE))
			{
				ResolveBinding(binding, key);
				return;
			}
			object[] array = key as object[];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				ResolveBinding(binding, array[i]);
			}
		}

		public virtual void ResolveBinding(global::strange.framework.api.IBinding binding, object key)
		{
			if (conflicts.ContainsKey(key))
			{
				global::System.Collections.Generic.Dictionary<global::strange.framework.api.IBinding, object> dictionary = conflicts[key];
				if (dictionary.ContainsKey(binding))
				{
					object name = dictionary[binding];
					if (!isConflictCleared(dictionary, binding))
					{
						return;
					}
					clearConflict(key, name, dictionary);
				}
			}
			object key2 = ((binding.name == null) ? global::strange.framework.api.BindingConst.NULLOID : binding.name);
			global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding> dictionary2;
			if (bindings.ContainsKey(key))
			{
				dictionary2 = bindings[key];
				if (dictionary2.ContainsKey(key2))
				{
					global::strange.framework.api.IBinding binding2 = dictionary2[key2];
					if (binding2 != binding && !binding2.isWeak && !binding.isWeak)
					{
						registerNameConflict(key, binding, dictionary2[key2]);
						return;
					}
					if (binding2.isWeak && binding2 != binding && binding2.isWeak && (!binding.isWeak || binding2.value == null || binding2.value is global::System.Type))
					{
						dictionary2.Remove(key2);
					}
				}
			}
			else
			{
				dictionary2 = new global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding>();
				bindings[key] = dictionary2;
			}
			if (dictionary2.ContainsKey(global::strange.framework.api.BindingConst.NULLOID) && dictionary2[global::strange.framework.api.BindingConst.NULLOID] == binding)
			{
				dictionary2.Remove(global::strange.framework.api.BindingConst.NULLOID);
			}
			if (!dictionary2.ContainsKey(key2))
			{
				dictionary2.Add(key2, binding);
			}
		}

		protected void registerNameConflict(object key, global::strange.framework.api.IBinding newBinding, global::strange.framework.api.IBinding existingBinding)
		{
			global::System.Collections.Generic.Dictionary<global::strange.framework.api.IBinding, object> dictionary;
			if (!conflicts.ContainsKey(key))
			{
				dictionary = new global::System.Collections.Generic.Dictionary<global::strange.framework.api.IBinding, object>();
				conflicts[key] = dictionary;
			}
			else
			{
				dictionary = conflicts[key];
			}
			dictionary[newBinding] = newBinding.name;
			dictionary[existingBinding] = newBinding.name;
		}

		protected bool isConflictCleared(global::System.Collections.Generic.Dictionary<global::strange.framework.api.IBinding, object> dict, global::strange.framework.api.IBinding binding)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<global::strange.framework.api.IBinding, object> item in dict)
			{
				if (item.Key != binding && item.Key.name == binding.name)
				{
					return false;
				}
			}
			return true;
		}

		protected void clearConflict(object key, object name, global::System.Collections.Generic.Dictionary<global::strange.framework.api.IBinding, object> dict)
		{
			global::System.Collections.Generic.List<global::strange.framework.api.IBinding> list = new global::System.Collections.Generic.List<global::strange.framework.api.IBinding>();
			foreach (global::System.Collections.Generic.KeyValuePair<global::strange.framework.api.IBinding, object> item in dict)
			{
				object value = item.Value;
				if (value.Equals(name))
				{
					list.Add(item.Key);
				}
			}
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				dict.Remove(list[i]);
			}
			if (dict.Count == 0)
			{
				conflicts.Remove(key);
			}
		}

		protected T[] spliceValueAt<T>(int splicePos, object[] objectValue)
		{
			T[] array = new T[objectValue.Length - 1];
			int num = 0;
			int num2 = objectValue.Length;
			for (int i = 0; i < num2; i++)
			{
				if (i == splicePos)
				{
					num = -1;
				}
				else
				{
					array[i + num] = (T)objectValue[i];
				}
			}
			return array;
		}

		protected object[] spliceValueAt(int splicePos, object[] objectValue)
		{
			return spliceValueAt<object>(splicePos, objectValue);
		}

		public virtual void OnRemove()
		{
		}
	}
}
