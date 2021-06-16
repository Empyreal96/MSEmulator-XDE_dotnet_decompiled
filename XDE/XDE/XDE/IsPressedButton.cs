using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200001E RID: 30
	public class IsPressedButton : Button
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00008073 File Offset: 0x00006273
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00008085 File Offset: 0x00006285
		public new bool IsPressed
		{
			get
			{
				return (bool)base.GetValue(IsPressedButton.IsPressedProperty);
			}
			set
			{
				base.SetValue(IsPressedButton.IsPressedProperty, value);
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008098 File Offset: 0x00006298
		protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsPressedChanged(e);
			this.IsPressed = (bool)e.NewValue;
		}

		// Token: 0x040000AC RID: 172
		public new static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(IsPressedButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
	}
}
