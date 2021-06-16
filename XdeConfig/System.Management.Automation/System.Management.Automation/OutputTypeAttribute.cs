using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000417 RID: 1047
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class OutputTypeAttribute : CmdletMetadataAttribute
	{
		// Token: 0x06002EB3 RID: 11955 RVA: 0x00100304 File Offset: 0x000FE504
		public OutputTypeAttribute(params Type[] type)
		{
			List<PSTypeName> list = new List<PSTypeName>();
			if (type != null)
			{
				foreach (Type type2 in type)
				{
					list.Add(new PSTypeName(type2));
				}
			}
			this._type = list.ToArray();
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x00100368 File Offset: 0x000FE568
		public OutputTypeAttribute(params string[] type)
		{
			List<PSTypeName> list = new List<PSTypeName>();
			if (type != null)
			{
				foreach (string name in type)
				{
					list.Add(new PSTypeName(name));
				}
			}
			this._type = list.ToArray();
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x001003CB File Offset: 0x000FE5CB
		public PSTypeName[] Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x001003D3 File Offset: 0x000FE5D3
		// (set) Token: 0x06002EB7 RID: 11959 RVA: 0x001003DB File Offset: 0x000FE5DB
		public string ProviderCmdlet
		{
			get
			{
				return this._providerCmdlet;
			}
			set
			{
				this._providerCmdlet = value;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x001003E4 File Offset: 0x000FE5E4
		// (set) Token: 0x06002EB9 RID: 11961 RVA: 0x001003EC File Offset: 0x000FE5EC
		public string[] ParameterSetName
		{
			get
			{
				return this._parameterSetName;
			}
			set
			{
				this._parameterSetName = value;
			}
		}

		// Token: 0x04001891 RID: 6289
		private PSTypeName[] _type;

		// Token: 0x04001892 RID: 6290
		private string _providerCmdlet;

		// Token: 0x04001893 RID: 6291
		private string[] _parameterSetName = new string[]
		{
			"__AllParameterSets"
		};
	}
}
