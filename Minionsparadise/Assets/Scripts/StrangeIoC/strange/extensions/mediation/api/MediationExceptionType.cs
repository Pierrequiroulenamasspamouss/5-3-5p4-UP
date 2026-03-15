namespace strange.extensions.mediation.api
{
	public enum MediationExceptionType
	{
		NO_CONTEXT = 0,
		MEDIATOR_VIEW_STACK_OVERFLOW = 1,
		NULL_MEDIATOR = 2,
		IMPLICIT_BINDING_MEDIATOR_TYPE_IS_NULL = 3,
		IMPLICIT_BINDING_VIEW_TYPE_IS_NULL = 4,
		VIEW_NOT_ASSIGNABLE = 5
	}
}
