namespace strange.extensions.injector.impl
{
	public class Injector : global::strange.extensions.injector.api.IInjector
	{
		private const int INFINITY_LIMIT = 10;

		private global::System.Collections.Generic.Dictionary<global::strange.extensions.injector.api.IInjectionBinding, int> infinityLock;

		public global::strange.extensions.injector.api.IInjectorFactory factory { get; set; }

		public global::strange.extensions.injector.api.IInjectionBinder binder { get; set; }

		public global::strange.extensions.reflector.api.IReflectionBinder reflector { get; set; }

		public Injector()
		{
			factory = new global::strange.extensions.injector.impl.InjectorFactory();
		}

		public object Instantiate(global::strange.extensions.injector.api.IInjectionBinding binding)
		{
			failIf(binder == null, "Attempt to instantiate from Injector without a Binder", global::strange.extensions.injector.api.InjectionExceptionType.NO_BINDER);
			failIf(factory == null, "Attempt to inject into Injector without a Factory", global::strange.extensions.injector.api.InjectionExceptionType.NO_FACTORY);
			armorAgainstInfiniteLoops(binding);
			object obj = null;
			global::System.Type type = null;
			if (binding.value is global::System.Type)
			{
				type = binding.value as global::System.Type;
			}
			else if (binding.value == null)
			{
				object[] array = binding.key as object[];
				type = array[0] as global::System.Type;
				if (type.IsPrimitive || type == typeof(decimal) || type == typeof(string))
				{
					obj = binding.value;
				}
			}
			else
			{
				obj = binding.value;
			}
			if (obj == null)
			{
				obj = factory.Get(this, binding);
				if (obj != null)
				{
					if (binding.toInject)
					{
						obj = Inject(obj, false);
					}
					if (binding.type == global::strange.extensions.injector.api.InjectionBindingType.SINGLETON || binding.type == global::strange.extensions.injector.api.InjectionBindingType.VALUE)
					{
						binding.ToInject(false);
					}
				}
			}
			infinityLock.Clear();
			return obj;
		}

		public object Inject(object target)
		{
			return Inject(target, true);
		}

		public object Inject(object target, bool attemptConstructorInjection)
		{
			failIf(binder == null, "Attempt to inject into Injector without a Binder", global::strange.extensions.injector.api.InjectionExceptionType.NO_BINDER);
			failIf(reflector == null, "Attempt to inject without a reflector", global::strange.extensions.injector.api.InjectionExceptionType.NO_REFLECTOR);
			failIf(target == null, "Attempt to inject into null instance", global::strange.extensions.injector.api.InjectionExceptionType.NULL_TARGET);
			global::System.Type type = target.GetType();
			if (type.IsPrimitive || type == typeof(decimal) || type == typeof(string))
			{
				return target;
			}
			global::strange.extensions.reflector.api.IReflectedClass reflection = reflector.Get(type);
			if (attemptConstructorInjection)
			{
				target = performConstructorInjection(target, reflection);
			}
			performSetterInjection(target, reflection);
			postInject(target, reflection);
			return target;
		}

		public void Uninject(object target)
		{
			failIf(binder == null, "Attempt to inject into Injector without a Binder", global::strange.extensions.injector.api.InjectionExceptionType.NO_BINDER);
			failIf(reflector == null, "Attempt to inject without a reflector", global::strange.extensions.injector.api.InjectionExceptionType.NO_REFLECTOR);
			failIf(target == null, "Attempt to inject into null instance", global::strange.extensions.injector.api.InjectionExceptionType.NULL_TARGET);
			global::System.Type type = target.GetType();
			if (!type.IsPrimitive && type != typeof(decimal) && type != typeof(string))
			{
				global::strange.extensions.reflector.api.IReflectedClass reflection = reflector.Get(type);
				performUninjection(target, reflection);
			}
		}

		private object performConstructorInjection(object target, global::strange.extensions.reflector.api.IReflectedClass reflection)
		{
			failIf(target == null, "Attempt to perform constructor injection into a null object", global::strange.extensions.injector.api.InjectionExceptionType.NULL_TARGET);
			failIf(reflection == null, "Attempt to perform constructor injection without a reflection", global::strange.extensions.injector.api.InjectionExceptionType.NULL_REFLECTION);
			if (!reflection.HasNonTrivialConstructor())
			{
				return target;
			}
			object obj = reflection.CreateInstance(this);
			if (obj != null)
			{
				return obj;
			}
			return target;
		}

		private void performSetterInjection(object target, global::strange.extensions.reflector.api.IReflectedClass reflection)
		{
			failIf(target == null, "Attempt to inject into a null object", global::strange.extensions.injector.api.InjectionExceptionType.NULL_TARGET);
			failIf(reflection == null, "Attempt to inject without a reflection", global::strange.extensions.injector.api.InjectionExceptionType.NULL_REFLECTION);
			reflection.PerformInjection(target, this);
		}

		public object GetValueInjection(global::System.Type t, object name, object target, bool optional)
		{
			global::strange.extensions.injector.api.IInjectionBinding binding = binder.GetBinding(t, name);
			if (binding == null)
			{
				if (optional)
				{
					return null;
				}
				failIf(true, "Attempt to Instantiate a null binding.", global::strange.extensions.injector.api.InjectionExceptionType.NULL_BINDING, t, name, target);
			}
			if (binding.type == global::strange.extensions.injector.api.InjectionBindingType.VALUE)
			{
				if (!binding.toInject)
				{
					return binding.value;
				}
				object result = Inject(binding.value, false);
				binding.ToInject(false);
				return result;
			}
			if (binding.type == global::strange.extensions.injector.api.InjectionBindingType.SINGLETON)
			{
				if (binding.value is global::System.Type || binding.value == null)
				{
					Instantiate(binding);
				}
				return binding.value;
			}
			return Instantiate(binding);
		}

		private void postInject(object target, global::strange.extensions.reflector.api.IReflectedClass reflection)
		{
			failIf(target == null, "Attempt to PostConstruct a null target", global::strange.extensions.injector.api.InjectionExceptionType.NULL_TARGET);
			failIf(reflection == null, "Attempt to PostConstruct without a reflection", global::strange.extensions.injector.api.InjectionExceptionType.NULL_REFLECTION);
			reflection.CallPostConstructors(target);
		}

		private void performUninjection(object target, global::strange.extensions.reflector.api.IReflectedClass reflection)
		{
			reflection.PerformUninjection(target, this);
		}

		private void failIf(bool condition, string message, global::strange.extensions.injector.api.InjectionExceptionType type)
		{
			failIf(condition, message, type, null, null, null);
		}

		private void failIf(bool condition, string message, global::strange.extensions.injector.api.InjectionExceptionType type, global::System.Type t, object name)
		{
			failIf(condition, message, type, t, name, null);
		}

		private void failIf(bool condition, string message, global::strange.extensions.injector.api.InjectionExceptionType type, global::System.Type t, object name, object target)
		{
			if (condition)
			{
				message = message + "\n\t\ttarget: " + target;
				message = message + "\n\t\ttype: " + t;
				message = message + "\n\t\tname: " + name;
				throw new global::strange.extensions.injector.impl.InjectionException(message, type);
			}
		}

		private void armorAgainstInfiniteLoops(global::strange.extensions.injector.api.IInjectionBinding binding)
		{
			if (binding != null)
			{
				if (infinityLock == null)
				{
					infinityLock = new global::System.Collections.Generic.Dictionary<global::strange.extensions.injector.api.IInjectionBinding, int>();
				}
				if (!infinityLock.ContainsKey(binding))
				{
					infinityLock.Add(binding, 0);
				}
				infinityLock[binding] += 1;
				if (infinityLock[binding] > 10)
				{
					throw new global::strange.extensions.injector.impl.InjectionException("There appears to be a circular dependency. Terminating loop.", global::strange.extensions.injector.api.InjectionExceptionType.CIRCULAR_DEPENDENCY);
				}
			}
		}
	}
}
