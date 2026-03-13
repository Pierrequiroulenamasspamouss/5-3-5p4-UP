namespace Kampai.Game
{
	public abstract class ReturnValueContainer
	{
		public enum ValueType
		{
			Void = 0,
			Nil = 1,
			Number = 2,
			Boolean = 3,
			String = 4,
			Dictionary = 5,
			Array = 6
		}

		protected readonly global::Kampai.Util.IKampaiLogger logger;

		private global::Kampai.Game.ReturnValueContainer.ValueType _type;

		protected bool boolValue;

		protected float numberValue;

		protected string stringValue;

		protected global::Kampai.Game.ReturnValueContainer.ValueType type
		{
			get
			{
				return _type;
			}
			set
			{
				if (_type != value)
				{
					if (_type == global::Kampai.Game.ReturnValueContainer.ValueType.Array)
					{
						ClearArray();
					}
					if (_type == global::Kampai.Game.ReturnValueContainer.ValueType.Dictionary)
					{
						ClearKeys();
					}
					_type = value;
				}
			}
		}

		public ReturnValueContainer(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		protected abstract global::Kampai.Game.ReturnValueContainer GetContainerForKey(string key);

		protected abstract global::Kampai.Game.ReturnValueContainer GetContainerForNextIndex();

		protected abstract void ClearKeys();

		protected abstract void ClearArray();

		public virtual void Reset()
		{
			SetVoid();
		}

		public void Set(object value)
		{
			if (value == null)
			{
				SetNil();
				return;
			}
			string name = value.GetType().Name;
			logger.Error("ReturnValueContainer: Setting value of type {0} is not supported!", name);
		}

		public void Set(global::Kampai.Util.IBoxed value)
		{
			global::System.Type type = value.GetType();
			global::System.Reflection.MethodInfo getMethod = type.GetProperty("Value").GetGetMethod();
			object obj = getMethod.Invoke(value, new object[0]);
			global::System.Reflection.MethodInfo method = typeof(global::Kampai.Game.ReturnValueContainer).GetMethod("Set", type.GetGenericArguments());
			method.Invoke(this, new object[1] { obj });
		}

		public void Set(global::Kampai.Util.ITuple value)
		{
			global::System.Type type = value.GetType();
			global::System.Type typeFromHandle = typeof(global::Kampai.Game.ReturnValueContainer);
			global::System.Type[] genericArguments = type.GetGenericArguments();
			int i = 0;
			for (int num = genericArguments.Length; i < num; i++)
			{
				global::Kampai.Game.ReturnValueContainer obj = PushIndex();
				object value2 = type.GetProperty("Item" + (i + 1)).GetValue(value, null);
				typeFromHandle.GetMethod("Set", new global::System.Type[1] { genericArguments[i] }).Invoke(obj, new object[1] { value2 });
			}
		}

		public void Set(global::Kampai.Game.Building building)
		{
			Set(building.ID);
		}

		public void Set(global::Kampai.Game.View.ActionableObject obj)
		{
			Set(obj.ID);
		}

		public void Set(global::Kampai.Game.Transaction.TransactionUpdateData updateData)
		{
			global::Kampai.Game.ReturnValueContainer returnValueContainer = SetKey("Type");
			returnValueContainer.Set((int)updateData.Type);
			global::Kampai.Game.ReturnValueContainer returnValueContainer2 = SetKey("TransactionId");
			returnValueContainer2.Set(updateData.TransactionId);
			global::Kampai.Game.ReturnValueContainer returnValueContainer3 = SetKey("InstanceId");
			returnValueContainer3.Set(updateData.InstanceId);
		}

		public void Set(global::Kampai.Game.Transaction.TransactionDefinition definition)
		{
			Set(definition.ID);
		}

		public void Set(global::Kampai.Game.OrderBoardTicket ticket)
		{
			Set(ticket.CharacterDefinitionId);
		}

		public void Set(global::Kampai.Game.ZoomType args)
		{
			Set((int)args);
		}

		public void Set(global::UnityEngine.Vector3 value)
		{
			SetKey("x").Set(value.x);
			SetKey("y").Set(value.y);
			SetKey("z").Set(value.z);
		}

		public void Set(int value)
		{
			Set((float)value);
		}

		public void Set(long value)
		{
			Set(global::System.Convert.ToInt32(value));
		}

		public void Set(float value)
		{
			type = global::Kampai.Game.ReturnValueContainer.ValueType.Number;
			numberValue = value;
		}

		public void Set(double value)
		{
			Set((float)value);
		}

		public void Set(string value)
		{
			type = global::Kampai.Game.ReturnValueContainer.ValueType.String;
			stringValue = value;
		}

		public void Set(bool value)
		{
			type = global::Kampai.Game.ReturnValueContainer.ValueType.Boolean;
			boolValue = value;
		}

		public void SetNil()
		{
			type = global::Kampai.Game.ReturnValueContainer.ValueType.Nil;
		}

		public void SetVoid()
		{
			type = global::Kampai.Game.ReturnValueContainer.ValueType.Void;
		}

		public global::Kampai.Game.ReturnValueContainer SetKey(string key)
		{
			type = global::Kampai.Game.ReturnValueContainer.ValueType.Dictionary;
			return GetContainerForKey(key);
		}

		public global::Kampai.Game.ReturnValueContainer PushIndex()
		{
			type = global::Kampai.Game.ReturnValueContainer.ValueType.Array;
			return GetContainerForNextIndex();
		}

		public void SetEmptyArray()
		{
			type = global::Kampai.Game.ReturnValueContainer.ValueType.Array;
			ClearArray();
		}
	}
}
