namespace Newtonsoft.Json.Serialization
{
	public class ReflectionValueProvider : global::Newtonsoft.Json.Serialization.IValueProvider
	{
		private readonly global::System.Reflection.MemberInfo _memberInfo;

		public ReflectionValueProvider(global::System.Reflection.MemberInfo memberInfo)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			_memberInfo = memberInfo;
		}

		public void SetValue(object target, object value)
		{
			try
			{
				global::Newtonsoft.Json.Utilities.ReflectionUtils.SetMemberValue(_memberInfo, target, value);
			}
			catch (global::System.Exception innerException)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error setting value to '{0}' on '{1}'.", global::System.Globalization.CultureInfo.InvariantCulture, _memberInfo.Name, target.GetType()), innerException);
			}
		}

		public object GetValue(object target)
		{
			try
			{
				return global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberValue(_memberInfo, target);
			}
			catch (global::System.Exception innerException)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error getting value from '{0}' on '{1}'.", global::System.Globalization.CultureInfo.InvariantCulture, _memberInfo.Name, target.GetType()), innerException);
			}
		}
	}
}
