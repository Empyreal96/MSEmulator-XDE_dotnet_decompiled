using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x0200010D RID: 269
	internal class DotNetAdapterWithComTypeName : DotNetAdapter
	{
		// Token: 0x06000EA6 RID: 3750 RVA: 0x00050DEE File Offset: 0x0004EFEE
		internal DotNetAdapterWithComTypeName(ComTypeInfo comTypeInfo)
		{
			this.comTypeInfo = comTypeInfo;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00050F5C File Offset: 0x0004F15C
		protected override IEnumerable<string> GetTypeNameHierarchy(object obj)
		{
			Type type = obj.GetType();
			while (type != null)
			{
				if (type.FullName.Equals("System.__ComObject"))
				{
					yield return ComAdapter.GetComTypeName(this.comTypeInfo.Clsid);
				}
				yield return type.FullName;
				type = type.GetTypeInfo().BaseType;
			}
			yield break;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00050F80 File Offset: 0x0004F180
		protected override ConsolidatedString GetInternedTypeNameHierarchy(object obj)
		{
			return new ConsolidatedString(this.GetTypeNameHierarchy(obj), true);
		}

		// Token: 0x04000679 RID: 1657
		private ComTypeInfo comTypeInfo;
	}
}
