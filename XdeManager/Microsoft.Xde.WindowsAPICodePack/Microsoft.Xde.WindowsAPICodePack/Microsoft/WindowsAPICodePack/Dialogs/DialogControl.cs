using System;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x0200001C RID: 28
	public abstract class DialogControl
	{
		// Token: 0x0600010A RID: 266 RVA: 0x0000385E File Offset: 0x00001A5E
		protected DialogControl()
		{
			this.Id = DialogControl.nextId;
			if (DialogControl.nextId == 2147483647)
			{
				DialogControl.nextId = 9;
				return;
			}
			DialogControl.nextId++;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003891 File Offset: 0x00001A91
		protected DialogControl(string name) : this()
		{
			this.Name = name;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000038A0 File Offset: 0x00001AA0
		// (set) Token: 0x0600010D RID: 269 RVA: 0x000038A8 File Offset: 0x00001AA8
		public IDialogControlHost HostingDialog { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000038B1 File Offset: 0x00001AB1
		// (set) Token: 0x0600010F RID: 271 RVA: 0x000038B9 File Offset: 0x00001AB9
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException(LocalizedMessages.DialogControlNameCannotBeEmpty);
				}
				if (!string.IsNullOrEmpty(this.name))
				{
					throw new InvalidOperationException(LocalizedMessages.DialogControlsCannotBeRenamed);
				}
				this.name = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000038ED File Offset: 0x00001AED
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000038F5 File Offset: 0x00001AF5
		public int Id { get; private set; }

		// Token: 0x06000112 RID: 274 RVA: 0x000038FE File Offset: 0x00001AFE
		protected void CheckPropertyChangeAllowed(string propName)
		{
			if (this.HostingDialog != null)
			{
				this.HostingDialog.IsControlPropertyChangeAllowed(propName, this);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00003916 File Offset: 0x00001B16
		protected void ApplyPropertyChange(string propName)
		{
			if (this.HostingDialog != null)
			{
				this.HostingDialog.ApplyControlPropertyChange(propName, this);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00003930 File Offset: 0x00001B30
		public override bool Equals(object obj)
		{
			DialogControl dialogControl = obj as DialogControl;
			return dialogControl != null && this.Id == dialogControl.Id;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003957 File Offset: 0x00001B57
		public override int GetHashCode()
		{
			if (this.Name == null)
			{
				return this.ToString().GetHashCode();
			}
			return this.Name.GetHashCode();
		}

		// Token: 0x0400011F RID: 287
		private static int nextId = 9;

		// Token: 0x04000121 RID: 289
		private string name;
	}
}
