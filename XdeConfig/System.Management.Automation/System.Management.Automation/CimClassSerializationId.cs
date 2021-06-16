using System;

namespace System.Management.Automation
{
	// Token: 0x0200044E RID: 1102
	internal class CimClassSerializationId : Tuple<string, string, string, int>
	{
		// Token: 0x06002FEF RID: 12271 RVA: 0x001058A1 File Offset: 0x00103AA1
		public CimClassSerializationId(string className, string namespaceName, string computerName, int hashCode) : base(className, namespaceName, computerName, hashCode)
		{
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06002FF0 RID: 12272 RVA: 0x001058AE File Offset: 0x00103AAE
		public string ClassName
		{
			get
			{
				return base.Item1;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002FF1 RID: 12273 RVA: 0x001058B6 File Offset: 0x00103AB6
		public string NamespaceName
		{
			get
			{
				return base.Item2;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x001058BE File Offset: 0x00103ABE
		public string ComputerName
		{
			get
			{
				return base.Item3;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x001058C6 File Offset: 0x00103AC6
		public int ClassHashCode
		{
			get
			{
				return base.Item4;
			}
		}
	}
}
