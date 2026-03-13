namespace strange.framework.impl
{
	public class SemiBinding : global::strange.framework.api.ISemiBinding, global::strange.framework.api.IManagedList
	{
		protected object[] objectValue;

		public global::strange.framework.api.BindingConstraintType constraint { get; set; }

		public bool uniqueValues { get; set; }

		public virtual object value
		{
			get
			{
				if (constraint == global::strange.framework.api.BindingConstraintType.ONE)
				{
					if (objectValue != null)
					{
						return objectValue[0];
					}
					return null;
				}
				return objectValue;
			}
		}

		public SemiBinding()
		{
			constraint = global::strange.framework.api.BindingConstraintType.ONE;
			uniqueValues = true;
		}

		public global::strange.framework.api.IManagedList Add(object o)
		{
			if (objectValue == null || constraint == global::strange.framework.api.BindingConstraintType.ONE)
			{
				objectValue = new object[1];
			}
			else
			{
				if (uniqueValues)
				{
					int num = objectValue.Length;
					for (int i = 0; i < num; i++)
					{
						object obj = objectValue[i];
						if (obj.Equals(o))
						{
							return this;
						}
					}
				}
				object[] array = objectValue;
				int num2 = array.Length;
				objectValue = new object[num2 + 1];
				array.CopyTo(objectValue, 0);
			}
			objectValue[objectValue.Length - 1] = o;
			return this;
		}

		public global::strange.framework.api.IManagedList Add(object[] list)
		{
			foreach (object o in list)
			{
				Add(o);
			}
			return this;
		}

		public global::strange.framework.api.IManagedList Remove(object o)
		{
			if (o.Equals(objectValue) || objectValue == null)
			{
				objectValue = null;
				return this;
			}
			int num = objectValue.Length;
			for (int i = 0; i < num; i++)
			{
				object obj = objectValue[i];
				if (o.Equals(obj))
				{
					spliceValueAt(i);
					return this;
				}
			}
			return this;
		}

		public global::strange.framework.api.IManagedList Remove(object[] list)
		{
			foreach (object o in list)
			{
				Remove(o);
			}
			return this;
		}

		protected void spliceValueAt(int splicePos)
		{
			object[] array = new object[objectValue.Length - 1];
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
					array[i + num] = objectValue[i];
				}
			}
			objectValue = ((array.Length == 0) ? null : array);
		}
	}
}
