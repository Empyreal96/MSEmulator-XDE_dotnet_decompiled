using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000060 RID: 96
	[DataContract]
	public class SkinButtonInfo : ISkinButtonInfo
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00005142 File Offset: 0x00003342
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000514A File Offset: 0x0000334A
		[DataMember]
		public Rectangle Bounds { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00005153 File Offset: 0x00003353
		// (set) Token: 0x06000220 RID: 544 RVA: 0x0000515B File Offset: 0x0000335B
		[DataMember]
		public SkinButtonType ButtonType { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00005164 File Offset: 0x00003364
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000516C File Offset: 0x0000336C
		[DataMember]
		public Keys[] KeyCode { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00005175 File Offset: 0x00003375
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000517D File Offset: 0x0000337D
		[DataMember]
		public bool IsEnabled { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00005186 File Offset: 0x00003386
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000518E File Offset: 0x0000338E
		[DataMember]
		public bool IsVisible { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00005197 File Offset: 0x00003397
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000519F File Offset: 0x0000339F
		[DataMember]
		public SkinButtonAnchor Anchor { get; set; }
	}
}
