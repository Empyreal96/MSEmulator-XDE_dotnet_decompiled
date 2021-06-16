using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000777 RID: 1911
	public class FileSystemContentReaderDynamicParameters : FileSystemContentDynamicParametersBase
	{
		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06004C69 RID: 19561 RVA: 0x00194702 File Offset: 0x00192902
		// (set) Token: 0x06004C6A RID: 19562 RVA: 0x0019470A File Offset: 0x0019290A
		[Parameter]
		public string Delimiter
		{
			get
			{
				return this.delimiter;
			}
			set
			{
				this.delimiterSpecified = true;
				this.delimiter = value;
			}
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06004C6B RID: 19563 RVA: 0x0019471A File Offset: 0x0019291A
		// (set) Token: 0x06004C6C RID: 19564 RVA: 0x00194727 File Offset: 0x00192927
		[Parameter]
		public SwitchParameter Wait
		{
			get
			{
				return this.wait;
			}
			set
			{
				this.wait = value;
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06004C6D RID: 19565 RVA: 0x00194735 File Offset: 0x00192935
		// (set) Token: 0x06004C6E RID: 19566 RVA: 0x00194742 File Offset: 0x00192942
		[Parameter]
		public SwitchParameter Raw
		{
			get
			{
				return this.isRaw;
			}
			set
			{
				this.isRaw = value;
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06004C6F RID: 19567 RVA: 0x00194750 File Offset: 0x00192950
		public bool DelimiterSpecified
		{
			get
			{
				return this.delimiterSpecified;
			}
		}

		// Token: 0x040024D8 RID: 9432
		private string delimiter = "\n";

		// Token: 0x040024D9 RID: 9433
		private bool wait;

		// Token: 0x040024DA RID: 9434
		private bool isRaw;

		// Token: 0x040024DB RID: 9435
		private bool delimiterSpecified;
	}
}
