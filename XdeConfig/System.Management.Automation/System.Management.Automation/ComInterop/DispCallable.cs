using System;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A7F RID: 2687
	internal sealed class DispCallable : IPseudoComObject
	{
		// Token: 0x06006ACC RID: 27340 RVA: 0x0021831D File Offset: 0x0021651D
		internal DispCallable(IDispatchComObject dispatch, string memberName, int dispId)
		{
			this._dispatch = dispatch;
			this._memberName = memberName;
			this._dispId = dispId;
		}

		// Token: 0x06006ACD RID: 27341 RVA: 0x0021833C File Offset: 0x0021653C
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "<bound dispmethod {0}>", new object[]
			{
				this._memberName
			});
		}

		// Token: 0x17001DC7 RID: 7623
		// (get) Token: 0x06006ACE RID: 27342 RVA: 0x00218369 File Offset: 0x00216569
		public IDispatchComObject DispatchComObject
		{
			get
			{
				return this._dispatch;
			}
		}

		// Token: 0x17001DC8 RID: 7624
		// (get) Token: 0x06006ACF RID: 27343 RVA: 0x00218371 File Offset: 0x00216571
		public IDispatch DispatchObject
		{
			get
			{
				return this._dispatch.DispatchObject;
			}
		}

		// Token: 0x17001DC9 RID: 7625
		// (get) Token: 0x06006AD0 RID: 27344 RVA: 0x0021837E File Offset: 0x0021657E
		public string MemberName
		{
			get
			{
				return this._memberName;
			}
		}

		// Token: 0x17001DCA RID: 7626
		// (get) Token: 0x06006AD1 RID: 27345 RVA: 0x00218386 File Offset: 0x00216586
		public int DispId
		{
			get
			{
				return this._dispId;
			}
		}

		// Token: 0x06006AD2 RID: 27346 RVA: 0x0021838E File Offset: 0x0021658E
		public DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DispCallableMetaObject(parameter, this);
		}

		// Token: 0x06006AD3 RID: 27347 RVA: 0x00218398 File Offset: 0x00216598
		public override bool Equals(object obj)
		{
			DispCallable dispCallable = obj as DispCallable;
			return dispCallable != null && dispCallable._dispatch == this._dispatch && dispCallable._dispId == this._dispId;
		}

		// Token: 0x06006AD4 RID: 27348 RVA: 0x002183CD File Offset: 0x002165CD
		public override int GetHashCode()
		{
			return this._dispatch.GetHashCode() ^ this._dispId;
		}

		// Token: 0x04003326 RID: 13094
		private readonly IDispatchComObject _dispatch;

		// Token: 0x04003327 RID: 13095
		private readonly string _memberName;

		// Token: 0x04003328 RID: 13096
		private readonly int _dispId;
	}
}
