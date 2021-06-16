using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A77 RID: 2679
	internal sealed class ComTypeLibInfo : IDynamicMetaObjectProvider
	{
		// Token: 0x06006AAF RID: 27311 RVA: 0x00217FA0 File Offset: 0x002161A0
		internal ComTypeLibInfo(ComTypeLibDesc typeLibDesc)
		{
			this._typeLibDesc = typeLibDesc;
		}

		// Token: 0x17001DC2 RID: 7618
		// (get) Token: 0x06006AB0 RID: 27312 RVA: 0x00217FAF File Offset: 0x002161AF
		public string Name
		{
			get
			{
				return this._typeLibDesc.Name;
			}
		}

		// Token: 0x17001DC3 RID: 7619
		// (get) Token: 0x06006AB1 RID: 27313 RVA: 0x00217FBC File Offset: 0x002161BC
		public Guid Guid
		{
			get
			{
				return this._typeLibDesc.Guid;
			}
		}

		// Token: 0x17001DC4 RID: 7620
		// (get) Token: 0x06006AB2 RID: 27314 RVA: 0x00217FC9 File Offset: 0x002161C9
		public short VersionMajor
		{
			get
			{
				return this._typeLibDesc.VersionMajor;
			}
		}

		// Token: 0x17001DC5 RID: 7621
		// (get) Token: 0x06006AB3 RID: 27315 RVA: 0x00217FD6 File Offset: 0x002161D6
		public short VersionMinor
		{
			get
			{
				return this._typeLibDesc.VersionMinor;
			}
		}

		// Token: 0x17001DC6 RID: 7622
		// (get) Token: 0x06006AB4 RID: 27316 RVA: 0x00217FE3 File Offset: 0x002161E3
		public ComTypeLibDesc TypeLibDesc
		{
			get
			{
				return this._typeLibDesc;
			}
		}

		// Token: 0x06006AB5 RID: 27317 RVA: 0x00217FEC File Offset: 0x002161EC
		public string[] GetMemberNames()
		{
			return new string[]
			{
				this.Name,
				"Guid",
				"Name",
				"VersionMajor",
				"VersionMinor"
			};
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x0021802A File Offset: 0x0021622A
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return new TypeLibInfoMetaObject(parameter, this);
		}

		// Token: 0x04003321 RID: 13089
		private readonly ComTypeLibDesc _typeLibDesc;
	}
}
