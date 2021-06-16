using System;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A75 RID: 2677
	internal sealed class ComTypeEnumDesc : ComTypeDesc, IDynamicMetaObjectProvider
	{
		// Token: 0x06006A99 RID: 27289 RVA: 0x0021795C File Offset: 0x00215B5C
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "<enum '{0}'>", new object[]
			{
				base.TypeName
			});
		}

		// Token: 0x06006A9A RID: 27290 RVA: 0x0021798C File Offset: 0x00215B8C
		internal ComTypeEnumDesc(ITypeInfo typeInfo, ComTypeLibDesc typeLibDesc) : base(typeInfo, ComType.Enum, typeLibDesc)
		{
			System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttrForTypeInfo = ComRuntimeHelpers.GetTypeAttrForTypeInfo(typeInfo);
			string[] array = new string[(int)typeAttrForTypeInfo.cVars];
			object[] array2 = new object[(int)typeAttrForTypeInfo.cVars];
			IntPtr zero = IntPtr.Zero;
			for (int i = 0; i < (int)typeAttrForTypeInfo.cVars; i++)
			{
				typeInfo.GetVarDesc(i, out zero);
				System.Runtime.InteropServices.ComTypes.VARDESC vardesc;
				try
				{
					vardesc = (System.Runtime.InteropServices.ComTypes.VARDESC)Marshal.PtrToStructure(zero, typeof(System.Runtime.InteropServices.ComTypes.VARDESC));
					if (vardesc.varkind == VARKIND.VAR_CONST)
					{
						array2[i] = Marshal.GetObjectForNativeVariant(vardesc.desc.lpvarValue);
					}
				}
				finally
				{
					typeInfo.ReleaseVarDesc(zero);
				}
				array[i] = ComRuntimeHelpers.GetNameOfMethod(typeInfo, vardesc.memid);
			}
			this._memberNames = array;
			this._memberValues = array2;
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x00217A58 File Offset: 0x00215C58
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return new TypeEnumMetaObject(this, parameter);
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x00217A64 File Offset: 0x00215C64
		public object GetValue(string enumValueName)
		{
			for (int i = 0; i < this._memberNames.Length; i++)
			{
				if (this._memberNames[i] == enumValueName)
				{
					return this._memberValues[i];
				}
			}
			throw new MissingMemberException(enumValueName);
		}

		// Token: 0x06006A9D RID: 27293 RVA: 0x00217AA4 File Offset: 0x00215CA4
		internal bool HasMember(string name)
		{
			for (int i = 0; i < this._memberNames.Length; i++)
			{
				if (this._memberNames[i] == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006A9E RID: 27294 RVA: 0x00217AD7 File Offset: 0x00215CD7
		public string[] GetMemberNames()
		{
			return (string[])this._memberNames.Clone();
		}

		// Token: 0x0400331A RID: 13082
		private readonly string[] _memberNames;

		// Token: 0x0400331B RID: 13083
		private readonly object[] _memberValues;
	}
}
