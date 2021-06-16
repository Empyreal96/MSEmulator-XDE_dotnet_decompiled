using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000003 RID: 3
	public interface IXdeToolbarItem : IXdePluginComponent, INotifyPropertyChanged
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2
		string Tooltip { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3
		// (set) Token: 0x06000004 RID: 4
		bool Visible { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000005 RID: 5
		// (set) Token: 0x06000006 RID: 6
		bool Enabled { get; set; }
	}
}
