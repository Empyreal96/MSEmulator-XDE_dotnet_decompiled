using System;
using System.ComponentModel;
using System.Diagnostics;

namespace XdeManager.ViewModel
{
	// Token: 0x02000013 RID: 19
	public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000EB RID: 235 RVA: 0x000042E8 File Offset: 0x000024E8
		// (remove) Token: 0x060000EC RID: 236 RVA: 0x00004320 File Offset: 0x00002520
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004355 File Offset: 0x00002555
		// (set) Token: 0x060000EE RID: 238 RVA: 0x0000435D File Offset: 0x0000255D
		public virtual string DisplayName { get; protected set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004366 File Offset: 0x00002566
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x0000436E File Offset: 0x0000256E
		private protected virtual bool ThrowOnInvalidPropertyName { protected get; private set; }

		// Token: 0x060000F1 RID: 241 RVA: 0x00004378 File Offset: 0x00002578
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

		// Token: 0x060000F2 RID: 242 RVA: 0x000043B2 File Offset: 0x000025B2
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.OnDispose();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000043C0 File Offset: 0x000025C0
		protected virtual void OnDispose()
		{
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000043C2 File Offset: 0x000025C2
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
