namespace Newtonsoft.Json.ObservableSupport
{
	public class PropertyChangingEventArgs : global::System.EventArgs
	{
		public virtual string PropertyName { get; set; }

		public PropertyChangingEventArgs(string propertyName)
		{
			PropertyName = propertyName;
		}
	}
}
