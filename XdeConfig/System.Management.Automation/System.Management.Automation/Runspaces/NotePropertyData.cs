using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200015E RID: 350
	[DebuggerDisplay("NoteProperty: {Name,nq} = {Value,nq}")]
	public sealed class NotePropertyData : TypeMemberData
	{
		// Token: 0x060011F1 RID: 4593 RVA: 0x00063FD3 File Offset: 0x000621D3
		public NotePropertyData(string name, object value) : base(name)
		{
			this.Value = value;
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x00063FE3 File Offset: 0x000621E3
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x00063FEB File Offset: 0x000621EB
		public object Value { get; set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x00063FF4 File Offset: 0x000621F4
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x00063FFC File Offset: 0x000621FC
		public bool IsHidden { get; set; }

		// Token: 0x060011F6 RID: 4598 RVA: 0x00064008 File Offset: 0x00062208
		internal override TypeMemberData Copy()
		{
			return new NotePropertyData(base.Name, this.Value)
			{
				IsHidden = this.IsHidden
			};
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00064036 File Offset: 0x00062236
		internal override void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			TypeTable.ProcessNoteData(errors, typeName, this, membersCollection, isOverride);
		}
	}
}
