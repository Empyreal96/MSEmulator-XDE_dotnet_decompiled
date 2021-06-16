using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Microsoft.Xde.Client.Controls
{
	// Token: 0x02000031 RID: 49
	public partial class EulaWindow : Window
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x00010741 File Offset: 0x0000E941
		public EulaWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00010750 File Offset: 0x0000E950
		public EulaWindow(string displayName, string eulaFileName) : this()
		{
			if (!string.IsNullOrEmpty(displayName))
			{
				base.Title = displayName;
			}
			using (FileStream fileStream = File.OpenRead(eulaFileName))
			{
				this.RtfBox.Selection.Load(fileStream, DataFormats.Rtf);
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000107AC File Offset: 0x0000E9AC
		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
			base.Close();
		}
	}
}
