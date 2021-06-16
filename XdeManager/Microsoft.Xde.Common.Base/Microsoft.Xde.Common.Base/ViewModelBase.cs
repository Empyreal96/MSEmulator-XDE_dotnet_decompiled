using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000028 RID: 40
	public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600019C RID: 412 RVA: 0x00004170 File Offset: 0x00002370
		// (remove) Token: 0x0600019D RID: 413 RVA: 0x000041A8 File Offset: 0x000023A8
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000041DD File Offset: 0x000023DD
		// (set) Token: 0x0600019F RID: 415 RVA: 0x000041E5 File Offset: 0x000023E5
		public virtual string DisplayName { get; protected set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000041EE File Offset: 0x000023EE
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x000041F6 File Offset: 0x000023F6
		private protected virtual bool ThrowOnInvalidPropertyName { protected get; private set; }

		// Token: 0x060001A2 RID: 418 RVA: 0x00004200 File Offset: 0x00002400
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public void VerifyPropertyName(string propertyName)
		{
			if (propertyName == null)
			{
				return;
			}
			if (TypeDescriptor.GetProperties(this)[propertyName] == null)
			{
				string message = "Invalid property name: " + propertyName;
				if (this.ThrowOnInvalidPropertyName)
				{
					throw new Exception(message);
				}
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000423A File Offset: 0x0000243A
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.OnDispose();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00004248 File Offset: 0x00002448
		protected virtual void OnDispose()
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000424A File Offset: 0x0000244A
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
