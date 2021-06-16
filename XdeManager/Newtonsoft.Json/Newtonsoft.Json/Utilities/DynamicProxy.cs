using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000048 RID: 72
	internal class DynamicProxy<T>
	{
		// Token: 0x060004BF RID: 1215 RVA: 0x000143AE File Offset: 0x000125AE
		public virtual IEnumerable<string> GetDynamicMemberNames(T instance)
		{
			return CollectionUtils.ArrayEmpty<string>();
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000143B5 File Offset: 0x000125B5
		public virtual bool TryBinaryOperation(T instance, BinaryOperationBinder binder, object arg, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000143BC File Offset: 0x000125BC
		public virtual bool TryConvert(T instance, ConvertBinder binder, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000143C2 File Offset: 0x000125C2
		public virtual bool TryCreateInstance(T instance, CreateInstanceBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000143C9 File Offset: 0x000125C9
		public virtual bool TryDeleteIndex(T instance, DeleteIndexBinder binder, object[] indexes)
		{
			return false;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000143CC File Offset: 0x000125CC
		public virtual bool TryDeleteMember(T instance, DeleteMemberBinder binder)
		{
			return false;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000143CF File Offset: 0x000125CF
		public virtual bool TryGetIndex(T instance, GetIndexBinder binder, object[] indexes, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000143D6 File Offset: 0x000125D6
		public virtual bool TryGetMember(T instance, GetMemberBinder binder, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000143DC File Offset: 0x000125DC
		public virtual bool TryInvoke(T instance, InvokeBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000143E3 File Offset: 0x000125E3
		public virtual bool TryInvokeMember(T instance, InvokeMemberBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000143EA File Offset: 0x000125EA
		public virtual bool TrySetIndex(T instance, SetIndexBinder binder, object[] indexes, object value)
		{
			return false;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000143ED File Offset: 0x000125ED
		public virtual bool TrySetMember(T instance, SetMemberBinder binder, object value)
		{
			return false;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000143F0 File Offset: 0x000125F0
		public virtual bool TryUnaryOperation(T instance, UnaryOperationBinder binder, out object result)
		{
			result = null;
			return false;
		}
	}
}
