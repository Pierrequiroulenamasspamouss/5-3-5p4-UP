namespace strange.extensions.sequencer.api
{
	public interface ISequenceBinding : global::strange.extensions.command.api.ICommandBinding, global::strange.framework.api.IBinding
	{
		new bool isOneOff { get; set; }

		new global::strange.extensions.sequencer.api.ISequenceBinding Once();

		new global::strange.extensions.sequencer.api.ISequenceBinding Bind<T>();

		new global::strange.extensions.sequencer.api.ISequenceBinding Bind(object key);

		new global::strange.extensions.sequencer.api.ISequenceBinding To<T>();

		new global::strange.extensions.sequencer.api.ISequenceBinding To(object o);

		new global::strange.extensions.sequencer.api.ISequenceBinding ToName<T>();

		new global::strange.extensions.sequencer.api.ISequenceBinding ToName(object o);

		new global::strange.extensions.sequencer.api.ISequenceBinding Named<T>();

		new global::strange.extensions.sequencer.api.ISequenceBinding Named(object o);
	}
}
