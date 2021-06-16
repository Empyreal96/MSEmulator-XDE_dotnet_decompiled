using System;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001B RID: 27
	public class BrowserHyperlink : Hyperlink
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00004A74 File Offset: 0x00002C74
		public BrowserHyperlink()
		{
			base.RequestNavigate += this.OnRequestNavigate;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004A8E File Offset: 0x00002C8E
		private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}
	}
}
