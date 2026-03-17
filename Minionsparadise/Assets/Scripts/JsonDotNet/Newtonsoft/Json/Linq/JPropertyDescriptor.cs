namespace Newtonsoft.Json.Linq
{
	public class JPropertyDescriptor : global::System.ComponentModel.PropertyDescriptor
	{
		private readonly global::System.Type _propertyType;

		public override global::System.Type ComponentType
		{
			get
			{
				return typeof(global::Newtonsoft.Json.Linq.JObject);
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public override global::System.Type PropertyType
		{
			get
			{
				return _propertyType;
			}
		}

		/*
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}
		*/

		public JPropertyDescriptor(string name, global::System.Type propertyType)
			: base(name, null)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(name, "name");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(propertyType, "propertyType");
			_propertyType = propertyType;
		}

		private static global::Newtonsoft.Json.Linq.JObject CastInstance(object instance)
		{
			return (global::Newtonsoft.Json.Linq.JObject)instance;
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override object GetValue(object component)
		{
			return CastInstance(component)[Name];
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
			global::Newtonsoft.Json.Linq.JToken value2 = ((value is global::Newtonsoft.Json.Linq.JToken) ? ((global::Newtonsoft.Json.Linq.JToken)value) : new global::Newtonsoft.Json.Linq.JValue(value));
			CastInstance(component)[Name] = value2;
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}
}
