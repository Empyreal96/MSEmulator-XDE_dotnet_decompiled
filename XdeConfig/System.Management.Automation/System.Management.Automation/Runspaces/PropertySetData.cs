using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000164 RID: 356
	[DebuggerDisplay("PropertySet: {Name,nq}")]
	public sealed class PropertySetData : TypeMemberData
	{
		// Token: 0x06001220 RID: 4640 RVA: 0x000642BC File Offset: 0x000624BC
		public PropertySetData(IEnumerable<string> referencedProperties)
		{
			if (referencedProperties == null)
			{
				throw PSTraceSource.NewArgumentNullException("referencedProperties");
			}
			this.ReferencedProperties = new Collection<string>();
			foreach (string item in referencedProperties)
			{
				this.ReferencedProperties.Add(item);
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00064328 File Offset: 0x00062528
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x00064330 File Offset: 0x00062530
		public Collection<string> ReferencedProperties { get; private set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x00064339 File Offset: 0x00062539
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x00064341 File Offset: 0x00062541
		internal new string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x0006434A File Offset: 0x0006254A
		// (set) Token: 0x06001226 RID: 4646 RVA: 0x00064352 File Offset: 0x00062552
		public bool IsHidden { get; set; }

		// Token: 0x06001227 RID: 4647 RVA: 0x0006435C File Offset: 0x0006255C
		internal override TypeMemberData Copy()
		{
			return new PropertySetData(this.ReferencedProperties)
			{
				Name = this.Name,
				IsHidden = this.IsHidden
			};
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00064390 File Offset: 0x00062590
		internal override void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			TypeTable.ProcessPropertySetData(errors, typeName, this, membersCollection, isOverride);
		}
	}
}
