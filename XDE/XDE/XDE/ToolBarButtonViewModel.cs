using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000019 RID: 25
	public class ToolBarButtonViewModel : ToolBarItemViewModel, ICommand
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00007B99 File Offset: 0x00005D99
		public ToolBarButtonViewModel(IXdeButton item, Executed executedCallback) : base(item)
		{
			this.item = item;
			this.item.PropertyChanged += this.Item_PropertyChanged;
			this.executedCallback = executedCallback;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600018B RID: 395 RVA: 0x00007BC8 File Offset: 0x00005DC8
		// (remove) Token: 0x0600018C RID: 396 RVA: 0x00007C00 File Offset: 0x00005E00
		public event EventHandler CanExecuteChanged;

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00007C35 File Offset: 0x00005E35
		public ICommand Command
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00007C38 File Offset: 0x00005E38
		public ImageSource ButtonImage
		{
			get
			{
				return XamlUtils.ConvertBitmapToBitmapImage(this.item.Image);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00007C4A File Offset: 0x00005E4A
		public bool Toggled
		{
			get
			{
				return this.item.Toggled;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00007C57 File Offset: 0x00005E57
		public bool Arrowed
		{
			get
			{
				return this.item.Arrowed;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00007C64 File Offset: 0x00005E64
		public bool Enabled
		{
			get
			{
				return this.item.Enabled;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00007C71 File Offset: 0x00005E71
		public bool Visible
		{
			get
			{
				return this.item.Visible;
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007C7E File Offset: 0x00005E7E
		public bool CanExecute(object parameter)
		{
			return this.item.Enabled;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007C8B File Offset: 0x00005E8B
		public void Execute(object parameter)
		{
			Executed executed = this.executedCallback;
			if (executed != null)
			{
				executed(this.item);
			}
			this.item.OnClicked(this, EventArgs.Empty);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007CB8 File Offset: 0x00005EB8
		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Application application = Application.Current;
			if (application == null)
			{
				return;
			}
			application.Dispatcher.Invoke(delegate()
			{
				string propertyName = e.PropertyName;
				if (!(propertyName == "Enabled"))
				{
					if (propertyName == "Toggled")
					{
						this.OnPropertyChanged("Toggled");
						return;
					}
					if (propertyName == "Arrowed")
					{
						this.OnPropertyChanged("Arrowed");
						return;
					}
					if (!(propertyName == "Image"))
					{
						return;
					}
					this.OnPropertyChanged("ButtonImage");
					return;
				}
				else
				{
					this.OnPropertyChanged("Enabled");
					EventHandler canExecuteChanged = this.CanExecuteChanged;
					if (canExecuteChanged == null)
					{
						return;
					}
					canExecuteChanged(this, EventArgs.Empty);
					return;
				}
			});
		}

		// Token: 0x0400009D RID: 157
		private IXdeButton item;

		// Token: 0x0400009E RID: 158
		private Executed executedCallback;
	}
}
