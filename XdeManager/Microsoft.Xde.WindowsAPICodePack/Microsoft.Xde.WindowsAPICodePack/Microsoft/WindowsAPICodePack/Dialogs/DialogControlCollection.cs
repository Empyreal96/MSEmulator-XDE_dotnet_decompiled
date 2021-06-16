using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x0200001D RID: 29
	public sealed class DialogControlCollection<T> : Collection<T> where T : DialogControl
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00003981 File Offset: 0x00001B81
		internal DialogControlCollection(IDialogControlHost host)
		{
			this.hostingDialog = host;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00003990 File Offset: 0x00001B90
		protected override void InsertItem(int index, T control)
		{
			if (base.Items.Contains(control))
			{
				throw new InvalidOperationException(LocalizedMessages.DialogCollectionCannotHaveDuplicateNames);
			}
			if (control.HostingDialog != null)
			{
				throw new InvalidOperationException(LocalizedMessages.DialogCollectionControlAlreadyHosted);
			}
			if (!this.hostingDialog.IsCollectionChangeAllowed())
			{
				throw new InvalidOperationException(LocalizedMessages.DialogCollectionModifyShowingDialog);
			}
			control.HostingDialog = this.hostingDialog;
			base.InsertItem(index, control);
			this.hostingDialog.ApplyCollectionChanged();
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00003A0C File Offset: 0x00001C0C
		protected override void RemoveItem(int index)
		{
			if (!this.hostingDialog.IsCollectionChangeAllowed())
			{
				throw new InvalidOperationException(LocalizedMessages.DialogCollectionModifyShowingDialog);
			}
			base.Items[index].HostingDialog = null;
			base.RemoveItem(index);
			this.hostingDialog.ApplyCollectionChanged();
		}

		// Token: 0x17000070 RID: 112
		public T this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					throw new ArgumentException(LocalizedMessages.DialogCollectionControlNameNull, "name");
				}
				return base.Items.FirstOrDefault((T x) => x.Name == name);
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00003AAC File Offset: 0x00001CAC
		internal DialogControl GetControlbyId(int id)
		{
			return base.Items.FirstOrDefault((T x) => x.Id == id);
		}

		// Token: 0x04000123 RID: 291
		private IDialogControlHost hostingDialog;
	}
}
