namespace Newtonsoft.Json.ObservableSupport
{
	public class NotifyCollectionChangedEventArgs
	{
		internal global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction Action { get; set; }

		internal global::System.Collections.IList NewItems { get; set; }

		internal int NewStartingIndex { get; set; }

		internal global::System.Collections.IList OldItems { get; set; }

		internal int OldStartingIndex { get; set; }

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action)
		{
			Action = action;
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, global::System.Collections.IList changedItems)
			: this(action)
		{
			NewItems = changedItems;
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, object changedItem)
			: this(action)
		{
			NewItems = new global::System.Collections.Generic.List<object> { changedItem };
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, global::System.Collections.IList newItems, global::System.Collections.IList oldItems)
			: this(action, newItems)
		{
			OldItems = oldItems;
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, global::System.Collections.IList changedItems, int startingIndex)
			: this(action, changedItems)
		{
			NewStartingIndex = startingIndex;
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, object changedItem, int index)
			: this(action, changedItem)
		{
			NewStartingIndex = index;
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, object newItem, object oldItem)
			: this(action, newItem)
		{
			OldItems = new global::System.Collections.Generic.List<object> { oldItem };
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, global::System.Collections.IList newItems, global::System.Collections.IList oldItems, int startingIndex)
			: this(action, newItems, oldItems)
		{
			NewStartingIndex = startingIndex;
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, global::System.Collections.IList changedItems, int index, int oldIndex)
			: this(action, changedItems, index)
		{
			OldStartingIndex = oldIndex;
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
			: this(action, changedItem, index)
		{
			OldStartingIndex = oldIndex;
		}

		internal NotifyCollectionChangedEventArgs(global::Newtonsoft.Json.ObservableSupport.NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
			: this(action, newItem, oldItem)
		{
			NewStartingIndex = index;
		}
	}
}
