using System;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A68 RID: 2664
	internal class ComMethodDesc
	{
		// Token: 0x06006A17 RID: 27159 RVA: 0x00215D5A File Offset: 0x00213F5A
		private ComMethodDesc(int dispId)
		{
			this._memid = dispId;
		}

		// Token: 0x06006A18 RID: 27160 RVA: 0x00215D69 File Offset: 0x00213F69
		internal ComMethodDesc(string name, int dispId) : this(dispId)
		{
			this._name = name;
		}

		// Token: 0x06006A19 RID: 27161 RVA: 0x00215D79 File Offset: 0x00213F79
		internal ComMethodDesc(string name, int dispId, INVOKEKIND invkind) : this(name, dispId)
		{
			this.InvokeKind = invkind;
		}

		// Token: 0x06006A1A RID: 27162 RVA: 0x00215D8C File Offset: 0x00213F8C
		internal ComMethodDesc(ITypeInfo typeInfo, FUNCDESC funcDesc) : this(funcDesc.memid)
		{
			this.InvokeKind = funcDesc.invkind;
			string[] array = new string[(int)(1 + funcDesc.cParams)];
			int num;
			typeInfo.GetNames(this._memid, array, array.Length, out num);
			bool skipLastParameter = false;
			if (this.IsPropertyPut && array[array.Length - 1] == null)
			{
				array[array.Length - 1] = "value";
				num++;
				skipLastParameter = true;
			}
			this._name = array[0];
			this._paramCnt = (int)funcDesc.cParams;
			this.ReturnType = ComUtil.GetTypeFromTypeDesc(funcDesc.elemdescFunc.tdesc);
			this.ParameterInformation = ComUtil.GetParameterInformation(funcDesc, skipLastParameter);
		}

		// Token: 0x17001D9E RID: 7582
		// (get) Token: 0x06006A1B RID: 27163 RVA: 0x00215E33 File Offset: 0x00214033
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17001D9F RID: 7583
		// (get) Token: 0x06006A1C RID: 27164 RVA: 0x00215E3B File Offset: 0x0021403B
		public int DispId
		{
			get
			{
				return this._memid;
			}
		}

		// Token: 0x17001DA0 RID: 7584
		// (get) Token: 0x06006A1D RID: 27165 RVA: 0x00215E43 File Offset: 0x00214043
		public bool IsPropertyGet
		{
			get
			{
				return (this.InvokeKind & INVOKEKIND.INVOKE_PROPERTYGET) != (INVOKEKIND)0;
			}
		}

		// Token: 0x17001DA1 RID: 7585
		// (get) Token: 0x06006A1E RID: 27166 RVA: 0x00215E53 File Offset: 0x00214053
		public bool IsDataMember
		{
			get
			{
				return this.IsPropertyGet && this.DispId != -4 && this._paramCnt == 0;
			}
		}

		// Token: 0x17001DA2 RID: 7586
		// (get) Token: 0x06006A1F RID: 27167 RVA: 0x00215E72 File Offset: 0x00214072
		public bool IsPropertyPut
		{
			get
			{
				return (this.InvokeKind & (INVOKEKIND.INVOKE_PROPERTYPUT | INVOKEKIND.INVOKE_PROPERTYPUTREF)) != (INVOKEKIND)0;
			}
		}

		// Token: 0x17001DA3 RID: 7587
		// (get) Token: 0x06006A20 RID: 27168 RVA: 0x00215E83 File Offset: 0x00214083
		public bool IsPropertyPutRef
		{
			get
			{
				return (this.InvokeKind & INVOKEKIND.INVOKE_PROPERTYPUTREF) != (INVOKEKIND)0;
			}
		}

		// Token: 0x17001DA4 RID: 7588
		// (get) Token: 0x06006A21 RID: 27169 RVA: 0x00215E93 File Offset: 0x00214093
		internal int ParamCount
		{
			get
			{
				return this._paramCnt;
			}
		}

		// Token: 0x17001DA5 RID: 7589
		// (get) Token: 0x06006A22 RID: 27170 RVA: 0x00215E9B File Offset: 0x0021409B
		// (set) Token: 0x06006A23 RID: 27171 RVA: 0x00215EA3 File Offset: 0x002140A3
		public Type ReturnType { get; set; }

		// Token: 0x17001DA6 RID: 7590
		// (get) Token: 0x06006A24 RID: 27172 RVA: 0x00215EAC File Offset: 0x002140AC
		// (set) Token: 0x06006A25 RID: 27173 RVA: 0x00215EB4 File Offset: 0x002140B4
		public Type InputType { get; set; }

		// Token: 0x17001DA7 RID: 7591
		// (get) Token: 0x06006A26 RID: 27174 RVA: 0x00215EBD File Offset: 0x002140BD
		// (set) Token: 0x06006A27 RID: 27175 RVA: 0x00215EC5 File Offset: 0x002140C5
		public ParameterInformation[] ParameterInformation { get; set; }

		// Token: 0x040032DF RID: 13023
		private readonly int _memid;

		// Token: 0x040032E0 RID: 13024
		private readonly string _name;

		// Token: 0x040032E1 RID: 13025
		internal readonly INVOKEKIND InvokeKind;

		// Token: 0x040032E2 RID: 13026
		private readonly int _paramCnt;
	}
}
