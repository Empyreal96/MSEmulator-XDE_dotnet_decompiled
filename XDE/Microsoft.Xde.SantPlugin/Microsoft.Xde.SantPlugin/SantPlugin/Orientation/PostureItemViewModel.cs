using System;
using System.Windows.Media.Imaging;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x0200002B RID: 43
	public class PostureItemViewModel
	{
		// Token: 0x0600017D RID: 381 RVA: 0x00006C0A File Offset: 0x00004E0A
		public PostureItemViewModel(OrientationModeInformation info)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.info = info;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00006C27 File Offset: 0x00004E27
		public OrientationModeInformation Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00006C2F File Offset: 0x00004E2F
		public string DisplayName
		{
			get
			{
				return this.info.DisplayName;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00006C3C File Offset: 0x00004E3C
		public BitmapSource ImageSource
		{
			get
			{
				return this.info.Source;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006C49 File Offset: 0x00004E49
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x040000E9 RID: 233
		private OrientationModeInformation info;
	}
}
