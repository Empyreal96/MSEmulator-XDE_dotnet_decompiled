using System;
using System.Reflection;
using Microsoft.PowerShell;

namespace System.Management.Automation.Language
{
	// Token: 0x02000583 RID: 1411
	public sealed class ReflectionTypeName : ITypeName, ISupportsTypeCaching
	{
		// Token: 0x06003A85 RID: 14981 RVA: 0x001347D4 File Offset: 0x001329D4
		public ReflectionTypeName(Type type)
		{
			if (type == null)
			{
				throw PSTraceSource.NewArgumentNullException("type");
			}
			this._type = type;
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06003A86 RID: 14982 RVA: 0x001347F7 File Offset: 0x001329F7
		public string FullName
		{
			get
			{
				return ToStringCodeMethods.Type(this._type, false);
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x00134805 File Offset: 0x00132A05
		public string Name
		{
			get
			{
				return this.FullName;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06003A88 RID: 14984 RVA: 0x0013480D File Offset: 0x00132A0D
		public string AssemblyName
		{
			get
			{
				return this._type.GetTypeInfo().Assembly.FullName;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06003A89 RID: 14985 RVA: 0x00134824 File Offset: 0x00132A24
		public bool IsArray
		{
			get
			{
				return this._type.IsArray;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06003A8A RID: 14986 RVA: 0x00134831 File Offset: 0x00132A31
		public bool IsGeneric
		{
			get
			{
				return this._type.GetTypeInfo().IsGenericType;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06003A8B RID: 14987 RVA: 0x00134843 File Offset: 0x00132A43
		public IScriptExtent Extent
		{
			get
			{
				return PositionUtilities.EmptyExtent;
			}
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x0013484A File Offset: 0x00132A4A
		public Type GetReflectionType()
		{
			return this._type;
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x00134852 File Offset: 0x00132A52
		public Type GetReflectionAttributeType()
		{
			return this._type;
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x0013485A File Offset: 0x00132A5A
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x00134864 File Offset: 0x00132A64
		public override bool Equals(object obj)
		{
			ReflectionTypeName reflectionTypeName = obj as ReflectionTypeName;
			return reflectionTypeName != null && this._type == reflectionTypeName._type;
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x0013488E File Offset: 0x00132A8E
		public override int GetHashCode()
		{
			return this._type.GetHashCode();
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06003A91 RID: 14993 RVA: 0x0013489B File Offset: 0x00132A9B
		// (set) Token: 0x06003A92 RID: 14994 RVA: 0x001348A3 File Offset: 0x00132AA3
		Type ISupportsTypeCaching.CachedType
		{
			get
			{
				return this._type;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04001D68 RID: 7528
		private readonly Type _type;
	}
}
