using System;
using System.Diagnostics.PerformanceData;

namespace System.Management.Automation.PerformanceData
{
	// Token: 0x02000904 RID: 2308
	public struct CounterInfo
	{
		// Token: 0x060056C2 RID: 22210 RVA: 0x001C66E0 File Offset: 0x001C48E0
		public CounterInfo(int id, CounterType type, string name)
		{
			this._Id = id;
			this._Type = type;
			this._Name = name;
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x001C66F7 File Offset: 0x001C48F7
		public CounterInfo(int id, CounterType type)
		{
			this._Id = id;
			this._Type = type;
			this._Name = null;
		}

		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x060056C4 RID: 22212 RVA: 0x001C670E File Offset: 0x001C490E
		public string Name
		{
			get
			{
				return this._Name;
			}
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x060056C5 RID: 22213 RVA: 0x001C6716 File Offset: 0x001C4916
		public int Id
		{
			get
			{
				return this._Id;
			}
		}

		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x060056C6 RID: 22214 RVA: 0x001C671E File Offset: 0x001C491E
		public CounterType Type
		{
			get
			{
				return this._Type;
			}
		}

		// Token: 0x04002E4C RID: 11852
		private string _Name;

		// Token: 0x04002E4D RID: 11853
		private int _Id;

		// Token: 0x04002E4E RID: 11854
		private CounterType _Type;
	}
}
