using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200001D RID: 29
	public class ToolBarItemSelector : DataTemplateSelector
	{
		// Token: 0x060001AB RID: 427 RVA: 0x00007FF4 File Offset: 0x000061F4
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement frameworkElement = container as FrameworkElement;
			if (item is ToolBarButtonViewModel)
			{
				return frameworkElement.FindResource("ToolBarButtonTemplate") as DataTemplate;
			}
			if (item is ToolBarSliderViewModel)
			{
				return frameworkElement.FindResource("ToolBarSliderTemplate") as DataTemplate;
			}
			if (item is ToolBarItemViewModel && (item as ToolBarItemViewModel).Name == "StockPlugin.Separator")
			{
				return frameworkElement.FindResource("ToolBarSeparatorTemplate") as DataTemplate;
			}
			return null;
		}
	}
}
