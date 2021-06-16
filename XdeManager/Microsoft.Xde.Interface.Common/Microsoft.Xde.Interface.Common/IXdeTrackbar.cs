using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000005 RID: 5
	public interface IXdeTrackbar : IXdeToolbarItem, IXdePluginComponent, INotifyPropertyChanged
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000007 RID: 7
		TrackbarSettings Settings { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000008 RID: 8
		int MaxValue { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000009 RID: 9
		int MinValue { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000A RID: 10
		int Length { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000B RID: 11
		// (set) Token: 0x0600000C RID: 12
		int Value { get; set; }
	}
}
