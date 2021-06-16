using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A6A RID: 2666
	internal class ComParamDesc
	{
		// Token: 0x06006A33 RID: 27187 RVA: 0x0021604C File Offset: 0x0021424C
		internal ComParamDesc(ref System.Runtime.InteropServices.ComTypes.ELEMDESC elemDesc, string name)
		{
			this._defaultValue = DBNull.Value;
			if (!string.IsNullOrEmpty(name))
			{
				this._isOut = ((short)(elemDesc.desc.paramdesc.wParamFlags & System.Runtime.InteropServices.ComTypes.PARAMFLAG.PARAMFLAG_FOUT) != 0);
				this._isOpt = ((short)(elemDesc.desc.paramdesc.wParamFlags & System.Runtime.InteropServices.ComTypes.PARAMFLAG.PARAMFLAG_FOPT) != 0);
			}
			this._name = name;
			this._vt = (VarEnum)elemDesc.tdesc.vt;
			System.Runtime.InteropServices.ComTypes.TYPEDESC typedesc = elemDesc.tdesc;
			for (;;)
			{
				if (this._vt == VarEnum.VT_PTR)
				{
					this._byRef = true;
				}
				else
				{
					if (this._vt != VarEnum.VT_ARRAY)
					{
						break;
					}
					this._isArray = true;
				}
				System.Runtime.InteropServices.ComTypes.TYPEDESC typedesc2 = (System.Runtime.InteropServices.ComTypes.TYPEDESC)Marshal.PtrToStructure(typedesc.lpValue, typeof(System.Runtime.InteropServices.ComTypes.TYPEDESC));
				this._vt = (VarEnum)typedesc2.vt;
				typedesc = typedesc2;
			}
			VarEnum vt = this._vt;
			if ((this._vt & VarEnum.VT_BYREF) != VarEnum.VT_EMPTY)
			{
				vt = (this._vt & (VarEnum)(-16385));
				this._byRef = true;
			}
			this._type = VarEnumSelector.GetTypeForVarEnum(vt);
		}

		// Token: 0x06006A34 RID: 27188 RVA: 0x00216159 File Offset: 0x00214359
		internal ComParamDesc(ref System.Runtime.InteropServices.ComTypes.ELEMDESC elemDesc) : this(ref elemDesc, string.Empty)
		{
		}

		// Token: 0x06006A35 RID: 27189 RVA: 0x00216168 File Offset: 0x00214368
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._isOpt)
			{
				stringBuilder.Append("[Optional] ");
			}
			if (this._isOut)
			{
				stringBuilder.Append("[out]");
			}
			stringBuilder.Append(this._type.Name);
			if (this._isArray)
			{
				stringBuilder.Append("[]");
			}
			if (this._byRef)
			{
				stringBuilder.Append("&");
			}
			stringBuilder.Append(" ");
			stringBuilder.Append(this._name);
			if (this._defaultValue != DBNull.Value)
			{
				stringBuilder.Append("=");
				stringBuilder.Append(this._defaultValue.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17001DA9 RID: 7593
		// (get) Token: 0x06006A36 RID: 27190 RVA: 0x00216227 File Offset: 0x00214427
		public bool IsOut
		{
			get
			{
				return this._isOut;
			}
		}

		// Token: 0x17001DAA RID: 7594
		// (get) Token: 0x06006A37 RID: 27191 RVA: 0x0021622F File Offset: 0x0021442F
		public bool IsOptional
		{
			get
			{
				return this._isOpt;
			}
		}

		// Token: 0x17001DAB RID: 7595
		// (get) Token: 0x06006A38 RID: 27192 RVA: 0x00216237 File Offset: 0x00214437
		public bool ByReference
		{
			get
			{
				return this._byRef;
			}
		}

		// Token: 0x17001DAC RID: 7596
		// (get) Token: 0x06006A39 RID: 27193 RVA: 0x0021623F File Offset: 0x0021443F
		public bool IsArray
		{
			get
			{
				return this._isArray;
			}
		}

		// Token: 0x17001DAD RID: 7597
		// (get) Token: 0x06006A3A RID: 27194 RVA: 0x00216247 File Offset: 0x00214447
		public Type ParameterType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17001DAE RID: 7598
		// (get) Token: 0x06006A3B RID: 27195 RVA: 0x0021624F File Offset: 0x0021444F
		internal object DefaultValue
		{
			get
			{
				return this._defaultValue;
			}
		}

		// Token: 0x040032E9 RID: 13033
		private readonly bool _isOut;

		// Token: 0x040032EA RID: 13034
		private readonly bool _isOpt;

		// Token: 0x040032EB RID: 13035
		private readonly bool _byRef;

		// Token: 0x040032EC RID: 13036
		private readonly bool _isArray;

		// Token: 0x040032ED RID: 13037
		private readonly VarEnum _vt;

		// Token: 0x040032EE RID: 13038
		private readonly string _name;

		// Token: 0x040032EF RID: 13039
		private readonly Type _type;

		// Token: 0x040032F0 RID: 13040
		private readonly object _defaultValue;
	}
}
