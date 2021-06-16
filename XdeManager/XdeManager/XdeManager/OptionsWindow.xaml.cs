using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using XdeManager.ViewModel;

namespace XdeManager
{
	// Token: 0x02000006 RID: 6
	public partial class OptionsWindow : Window
	{
		// Token: 0x06000020 RID: 32 RVA: 0x000025A5 File Offset: 0x000007A5
		public OptionsWindow()
		{
			this.Model = new OptionsViewModel();
			base.DataContext = this.Model;
			this.InitializeComponent();
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000025CA File Offset: 0x000007CA
		private OptionsViewModel Model { get; }

		// Token: 0x06000022 RID: 34 RVA: 0x000025D2 File Offset: 0x000007D2
		private bool IsValid(DependencyObject obj)
		{
			return !Validation.GetHasError(obj) && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(new Func<DependencyObject, bool>(this.IsValid));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000025FA File Offset: 0x000007FA
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (!this.IsValid(this))
			{
				return;
			}
			if (this.Model.Save())
			{
				base.DialogResult = new bool?(true);
				base.Close();
			}
		}
	}
}
