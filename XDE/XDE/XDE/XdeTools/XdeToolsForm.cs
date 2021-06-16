using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Microsoft.Xde.Common;
using Microsoft.Xde.Tools;

namespace Microsoft.Xde.Client.XdeTools
{
	// Token: 0x02000030 RID: 48
	public partial class XdeToolsForm : Form
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x000103AE File Offset: 0x0000E5AE
		public XdeToolsForm()
		{
			this.InitializeComponent();
			base.TopLevel = true;
			XdeToolsForm.CurrentInstance = this;
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x000103C9 File Offset: 0x0000E5C9
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x000103D0 File Offset: 0x0000E5D0
		public static XdeToolsForm CurrentInstance { get; private set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x000103D8 File Offset: 0x0000E5D8
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x000103E0 File Offset: 0x0000E5E0
		public IXdeSku Sku { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000103E9 File Offset: 0x0000E5E9
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x000103F1 File Offset: 0x0000E5F1
		public XdeToolsControl XdeToolsControl { get; private set; }

		// Token: 0x0600041B RID: 1051 RVA: 0x000103FC File Offset: 0x0000E5FC
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			ElementHost elementHost = null;
			XdeToolsControl xdeToolsControl = null;
			try
			{
				elementHost = new ElementHost();
				elementHost.Dock = DockStyle.Fill;
				elementHost.Text = Resources.ToolsWindowTitle;
				xdeToolsControl = new XdeToolsControl();
				this.XdeToolsControl = xdeToolsControl;
				xdeToolsControl.Initialize();
				xdeToolsControl.CloseClicked += this.XdeTools_CloseClicked;
				xdeToolsControl.StartDragMove += this.XdeTools_StartDragMove;
				int num = 0;
				foreach (IXdeTab xdeTab in this.Sku.Tabs)
				{
					System.Windows.Controls.UserControl tab = xdeTab.CreateTab();
					xdeToolsControl.InsertTab(xdeTab.Caption, xdeTab.Name, tab, num++);
				}
				using (Graphics graphics = base.CreateGraphics())
				{
					double num2 = (double)graphics.DpiX / 96.0;
					double num3 = (double)graphics.DpiY / 96.0;
					int width = (int)(xdeToolsControl.Width * num2);
					int height = (int)(xdeToolsControl.Height * num3);
					base.Size = new Size(width, height);
				}
				elementHost.Child = xdeToolsControl;
				xdeToolsControl = null;
				base.Controls.Add(elementHost);
				elementHost = null;
			}
			finally
			{
				if (elementHost != null)
				{
					elementHost.Dispose();
				}
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001058C File Offset: 0x0000E78C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.dragging)
			{
				int num = e.Location.X - this.anchorLocation.X;
				int num2 = e.Location.Y - this.anchorLocation.Y;
				base.Location = new Point(base.Location.X + num, base.Location.Y + num2);
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00010609 File Offset: 0x0000E809
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.dragging = false;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00010619 File Offset: 0x0000E819
		protected override void OnDeactivate(EventArgs e)
		{
			this.dragging = false;
			base.OnDeactivate(e);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00010629 File Offset: 0x0000E829
		protected override void OnSizeChanged(EventArgs e)
		{
			if (base.WindowState == FormWindowState.Maximized)
			{
				base.WindowState = FormWindowState.Normal;
			}
			base.OnSizeChanged(e);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00010642 File Offset: 0x0000E842
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				base.Hide();
			}
			base.OnFormClosing(e);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00010664 File Offset: 0x0000E864
		private void XdeTools_StartDragMove(object sender, EventArgs e)
		{
			Point position = Cursor.Position;
			this.dragging = true;
			base.Capture = true;
			this.anchorLocation = base.PointToClient(position);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00010692 File Offset: 0x0000E892
		private void XdeTools_CloseClicked(object sender, EventArgs e)
		{
			base.Hide();
		}

		// Token: 0x04000174 RID: 372
		private bool dragging;

		// Token: 0x04000175 RID: 373
		private Point anchorLocation;
	}
}
