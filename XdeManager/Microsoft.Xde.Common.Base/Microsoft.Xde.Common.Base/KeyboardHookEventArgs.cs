using System;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000008 RID: 8
	public class KeyboardHookEventArgs : EventArgs
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002B28 File Offset: 0x00000D28
		public string KeyName
		{
			get
			{
				return ((Keys)this.KeyCode).ToString();
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002B49 File Offset: 0x00000D49
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002B51 File Offset: 0x00000D51
		public int KeyCode { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002B5A File Offset: 0x00000D5A
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002B62 File Offset: 0x00000D62
		public int ScanCode { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002B6B File Offset: 0x00000D6B
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002B73 File Offset: 0x00000D73
		public bool Down { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002B7C File Offset: 0x00000D7C
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002B84 File Offset: 0x00000D84
		public bool Extended { get; private set; }

		// Token: 0x0600003A RID: 58 RVA: 0x00002B8D File Offset: 0x00000D8D
		public KeyboardHookEventArgs(int keyCode, int scanCode, bool down, bool extended)
		{
			this.KeyCode = keyCode;
			this.ScanCode = scanCode;
			this.Down = down;
			this.Extended = extended;
		}
	}
}
