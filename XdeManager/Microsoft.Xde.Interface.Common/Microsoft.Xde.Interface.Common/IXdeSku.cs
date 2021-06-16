using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000017 RID: 23
	public interface IXdeSku : INotifyPropertyChanged, IDisposable
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000083 RID: 131
		IEnumerable<IXdeFeature> Features { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000084 RID: 132
		IEnumerable<IXdeConnectionController> ConnectionControllers { get; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000085 RID: 133
		IEnumerable<IXdeTab> Tabs { get; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000086 RID: 134
		IEnumerable<IXdeToolbarItem> ToolbarItems { get; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000087 RID: 135
		string SkuDirectory { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000088 RID: 136
		IXdeSkuBranding Branding { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000089 RID: 137
		IXdeSkuOptions Options { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600008A RID: 138
		SkuRegKey[] Keys { get; }
	}
}
