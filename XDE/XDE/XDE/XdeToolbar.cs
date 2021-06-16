using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000015 RID: 21
	public class XdeToolbar : ItemsControl
	{
		// Token: 0x06000106 RID: 262 RVA: 0x00006016 File Offset: 0x00004216
		public XdeToolbar()
		{
			base.Loaded += new RoutedEventHandler(this.XdeToolbar_Loaded);
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000603B File Offset: 0x0000423B
		// (set) Token: 0x06000108 RID: 264 RVA: 0x0000604D File Offset: 0x0000424D
		public int Columns
		{
			get
			{
				return (int)base.GetValue(XdeToolbar.ColumnsProperty);
			}
			set
			{
				base.SetValue(XdeToolbar.ColumnsProperty, value);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006060 File Offset: 0x00004260
		private static void ColumnsCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((XdeToolbar)d).model.Columns = (int)e.NewValue;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006080 File Offset: 0x00004280
		private void XdeToolbar_Loaded(object sender, EventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)base.Template.FindName("ItemsPanel", this);
			if (frameworkElement != null)
			{
				frameworkElement.DataContext = this.model;
				base.Loaded -= new RoutedEventHandler(this.XdeToolbar_Loaded);
			}
		}

		// Token: 0x0400006A RID: 106
		public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(XdeToolbar), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(XdeToolbar.ColumnsCallback)));

		// Token: 0x0400006B RID: 107
		private const double ItemWidth = 30.0;

		// Token: 0x0400006C RID: 108
		private XdeToolbar.XdeToolbarViewModel model = new XdeToolbar.XdeToolbarViewModel();

		// Token: 0x02000035 RID: 53
		private class XdeToolbarViewModel : ViewModelBase
		{
			// Token: 0x1700018E RID: 398
			// (get) Token: 0x0600042F RID: 1071 RVA: 0x000108E4 File Offset: 0x0000EAE4
			// (set) Token: 0x06000430 RID: 1072 RVA: 0x000108EC File Offset: 0x0000EAEC
			public int Columns
			{
				get
				{
					return this.columns;
				}
				set
				{
					this.columns = value;
					this.OnPropertyChanged("Columns");
					this.OnPropertyChanged("ItemPanelWidth");
				}
			}

			// Token: 0x1700018F RID: 399
			// (get) Token: 0x06000431 RID: 1073 RVA: 0x0001090B File Offset: 0x0000EB0B
			public double ItemWidth
			{
				get
				{
					return 30.0;
				}
			}

			// Token: 0x17000190 RID: 400
			// (get) Token: 0x06000432 RID: 1074 RVA: 0x00010916 File Offset: 0x0000EB16
			public double ItemPanelWidth
			{
				get
				{
					return (double)this.columns * this.ItemWidth;
				}
			}

			// Token: 0x04000183 RID: 387
			private int columns = 1;
		}
	}
}
