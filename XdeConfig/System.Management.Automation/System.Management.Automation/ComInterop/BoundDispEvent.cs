using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A50 RID: 2640
	internal sealed class BoundDispEvent : DynamicObject
	{
		// Token: 0x0600699C RID: 27036 RVA: 0x00214034 File Offset: 0x00212234
		internal BoundDispEvent(object rcw, Guid sourceIid, int dispid)
		{
			this._rcw = rcw;
			this._sourceIid = sourceIid;
			this._dispid = dispid;
		}

		// Token: 0x0600699D RID: 27037 RVA: 0x00214051 File Offset: 0x00212251
		public override bool TryBinaryOperation(BinaryOperationBinder binder, object handler, out object result)
		{
			if (binder.Operation == ExpressionType.AddAssign)
			{
				result = this.InPlaceAdd(handler);
				return true;
			}
			if (binder.Operation == ExpressionType.SubtractAssign)
			{
				result = this.InPlaceSubtract(handler);
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x00214081 File Offset: 0x00212281
		private static void VerifyHandler(object handler)
		{
			if (handler is Delegate && handler.GetType() != typeof(Delegate))
			{
				return;
			}
			if (handler is IDynamicMetaObjectProvider)
			{
				return;
			}
			if (handler is DispCallable)
			{
				return;
			}
			throw Error.UnsupportedHandlerType();
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x002140BC File Offset: 0x002122BC
		private object InPlaceAdd(object handler)
		{
			BoundDispEvent.VerifyHandler(handler);
			ComEventSink comEventSink = ComEventSink.FromRuntimeCallableWrapper(this._rcw, this._sourceIid, true);
			comEventSink.AddHandler(this._dispid, handler);
			return this;
		}

		// Token: 0x060069A0 RID: 27040 RVA: 0x002140F0 File Offset: 0x002122F0
		private object InPlaceSubtract(object handler)
		{
			BoundDispEvent.VerifyHandler(handler);
			ComEventSink comEventSink = ComEventSink.FromRuntimeCallableWrapper(this._rcw, this._sourceIid, false);
			if (comEventSink != null)
			{
				comEventSink.RemoveHandler(this._dispid, handler);
			}
			return this;
		}

		// Token: 0x04003296 RID: 12950
		private object _rcw;

		// Token: 0x04003297 RID: 12951
		private Guid _sourceIid;

		// Token: 0x04003298 RID: 12952
		private int _dispid;
	}
}
