using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A69 RID: 2665
	internal class ComObject : IDynamicMetaObjectProvider
	{
		// Token: 0x06006A28 RID: 27176 RVA: 0x00215ECE File Offset: 0x002140CE
		internal ComObject(object rcw)
		{
			this._rcw = rcw;
		}

		// Token: 0x17001DA8 RID: 7592
		// (get) Token: 0x06006A29 RID: 27177 RVA: 0x00215EDD File Offset: 0x002140DD
		internal object RuntimeCallableWrapper
		{
			get
			{
				return this._rcw;
			}
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x00215EE8 File Offset: 0x002140E8
		public static ComObject ObjectToComObject(object rcw)
		{
			object comObjectData = Marshal.GetComObjectData(rcw, ComObject._ComObjectInfoKey);
			if (comObjectData != null)
			{
				return (ComObject)comObjectData;
			}
			ComObject result;
			lock (ComObject._ComObjectInfoKey)
			{
				comObjectData = Marshal.GetComObjectData(rcw, ComObject._ComObjectInfoKey);
				if (comObjectData != null)
				{
					result = (ComObject)comObjectData;
				}
				else
				{
					ComObject comObject = ComObject.CreateComObject(rcw);
					if (!Marshal.SetComObjectData(rcw, ComObject._ComObjectInfoKey, comObject))
					{
						throw Error.SetComObjectDataFailed();
					}
					result = comObject;
				}
			}
			return result;
		}

		// Token: 0x06006A2B RID: 27179 RVA: 0x00215F70 File Offset: 0x00214170
		internal static MemberExpression RcwFromComObject(Expression comObject)
		{
			return Expression.Property(Helpers.Convert(comObject, typeof(ComObject)), typeof(ComObject).GetProperty("RuntimeCallableWrapper", BindingFlags.Instance | BindingFlags.NonPublic));
		}

		// Token: 0x06006A2C RID: 27180 RVA: 0x00215F9D File Offset: 0x0021419D
		internal static MethodCallExpression RcwToComObject(Expression rcw)
		{
			return Expression.Call(typeof(ComObject).GetMethod("ObjectToComObject"), Helpers.Convert(rcw, typeof(object)));
		}

		// Token: 0x06006A2D RID: 27181 RVA: 0x00215FC8 File Offset: 0x002141C8
		private static ComObject CreateComObject(object rcw)
		{
			IDispatch dispatch = rcw as IDispatch;
			if (dispatch != null)
			{
				return new IDispatchComObject(dispatch);
			}
			return new ComObject(rcw);
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x00215FEC File Offset: 0x002141EC
		internal virtual IList<string> GetMemberNames(bool dataOnly)
		{
			return new string[0];
		}

		// Token: 0x06006A2F RID: 27183 RVA: 0x00215FF4 File Offset: 0x002141F4
		internal virtual IList<KeyValuePair<string, object>> GetMembers(IEnumerable<string> names)
		{
			return new KeyValuePair<string, object>[0];
		}

		// Token: 0x06006A30 RID: 27184 RVA: 0x00215FFC File Offset: 0x002141FC
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return new ComFallbackMetaObject(parameter, BindingRestrictions.Empty, this);
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x0021600A File Offset: 0x0021420A
		internal static bool IsComObject(object obj)
		{
			return obj != null && ComObject.ComObjectType.IsAssignableFrom(obj.GetType());
		}

		// Token: 0x040032E6 RID: 13030
		private readonly object _rcw;

		// Token: 0x040032E7 RID: 13031
		private static readonly object _ComObjectInfoKey = new object();

		// Token: 0x040032E8 RID: 13032
		private static readonly Type ComObjectType = typeof(object).Assembly.GetType("System.__ComObject");
	}
}
