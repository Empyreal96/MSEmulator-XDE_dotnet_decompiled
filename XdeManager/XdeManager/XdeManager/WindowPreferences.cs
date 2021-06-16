using System;
using System.Windows;
using XdeManager.Properties;

namespace XdeManager
{
	// Token: 0x02000007 RID: 7
	public class WindowPreferences
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000026A7 File Offset: 0x000008A7
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000026AF File Offset: 0x000008AF
		public double WindowTop { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000026B8 File Offset: 0x000008B8
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000026C0 File Offset: 0x000008C0
		public double WindowLeft { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000026C9 File Offset: 0x000008C9
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000026D1 File Offset: 0x000008D1
		public double WindowHeight { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000026DA File Offset: 0x000008DA
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000026E2 File Offset: 0x000008E2
		public double WindowWidth { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000026EB File Offset: 0x000008EB
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000026F3 File Offset: 0x000008F3
		public WindowState WindowState { get; set; }

		// Token: 0x06000030 RID: 48 RVA: 0x000026FC File Offset: 0x000008FC
		public WindowPreferences()
		{
			this.Load();
			this.SizeToFit();
			this.MoveIntoView();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002716 File Offset: 0x00000916
		public void SizeToFit()
		{
			if (this.WindowHeight > SystemParameters.VirtualScreenHeight)
			{
				this.WindowHeight = SystemParameters.VirtualScreenHeight;
			}
			if (this.WindowWidth > SystemParameters.VirtualScreenWidth)
			{
				this.WindowWidth = SystemParameters.VirtualScreenWidth;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002748 File Offset: 0x00000948
		public void MoveIntoView()
		{
			if (this.WindowTop + this.WindowHeight / 2.0 > SystemParameters.VirtualScreenHeight)
			{
				this.WindowTop = SystemParameters.VirtualScreenHeight - this.WindowHeight;
			}
			if (this.WindowLeft + this.WindowWidth / 2.0 > SystemParameters.VirtualScreenWidth)
			{
				this.WindowLeft = SystemParameters.VirtualScreenWidth - this.WindowWidth;
			}
			if (this.WindowTop < 0.0)
			{
				this.WindowTop = 0.0;
			}
			if (this.WindowLeft < 0.0)
			{
				this.WindowLeft = 0.0;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000027F8 File Offset: 0x000009F8
		private void Load()
		{
			this.WindowTop = Settings.Default.WindowTop;
			this.WindowLeft = Settings.Default.WindowLeft;
			this.WindowHeight = Settings.Default.WindowHeight;
			this.WindowWidth = Settings.Default.WindowWidth;
			this.WindowState = Settings.Default.WindowState;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002858 File Offset: 0x00000A58
		public void Save()
		{
			if (this.WindowState != WindowState.Minimized)
			{
				Settings.Default.WindowTop = this.WindowTop;
				Settings.Default.WindowLeft = this.WindowLeft;
				Settings.Default.WindowHeight = this.WindowHeight;
				Settings.Default.WindowWidth = this.WindowWidth;
				Settings.Default.WindowState = this.WindowState;
				Settings.Default.Save();
			}
		}
	}
}
