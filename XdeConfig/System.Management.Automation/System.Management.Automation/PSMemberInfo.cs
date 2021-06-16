using System;

namespace System.Management.Automation
{
	// Token: 0x02000137 RID: 311
	public abstract class PSMemberInfo
	{
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0005C7A5 File Offset: 0x0005A9A5
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x0005C7AD File Offset: 0x0005A9AD
		internal bool ShouldSerialize { get; set; }

		// Token: 0x06001055 RID: 4181 RVA: 0x0005C7B6 File Offset: 0x0005A9B6
		internal virtual void ReplicateInstance(object particularInstance)
		{
			this.instance = particularInstance;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0005C7C0 File Offset: 0x0005A9C0
		internal void SetValueNoConversion(object setValue)
		{
			PSProperty psproperty = this as PSProperty;
			if (psproperty == null)
			{
				this.Value = setValue;
				return;
			}
			psproperty.SetAdaptedValue(setValue, false);
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0005C7E7 File Offset: 0x0005A9E7
		protected PSMemberInfo()
		{
			this.ShouldSerialize = true;
			this.IsInstance = true;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0005C800 File Offset: 0x0005AA00
		internal void CloneBaseProperties(PSMemberInfo destiny)
		{
			destiny.name = this.name;
			destiny.IsHidden = this.IsHidden;
			destiny.IsReservedMember = this.IsReservedMember;
			destiny.IsInstance = this.IsInstance;
			destiny.instance = this.instance;
			destiny.ShouldSerialize = this.ShouldSerialize;
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001059 RID: 4185
		public abstract PSMemberTypes MemberType { get; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x0005C855 File Offset: 0x0005AA55
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0005C85D File Offset: 0x0005AA5D
		protected void SetMemberName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0005C879 File Offset: 0x0005AA79
		// (set) Token: 0x0600105D RID: 4189 RVA: 0x0005C881 File Offset: 0x0005AA81
		internal bool IsReservedMember { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x0005C88A File Offset: 0x0005AA8A
		// (set) Token: 0x0600105F RID: 4191 RVA: 0x0005C892 File Offset: 0x0005AA92
		internal bool IsHidden { get; set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x0005C89B File Offset: 0x0005AA9B
		// (set) Token: 0x06001061 RID: 4193 RVA: 0x0005C8A3 File Offset: 0x0005AAA3
		public bool IsInstance { get; internal set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001062 RID: 4194
		// (set) Token: 0x06001063 RID: 4195
		public abstract object Value { get; set; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001064 RID: 4196
		public abstract string TypeNameOfValue { get; }

		// Token: 0x06001065 RID: 4197
		public abstract PSMemberInfo Copy();

		// Token: 0x06001066 RID: 4198 RVA: 0x0005C8AC File Offset: 0x0005AAAC
		internal bool MatchesOptions(MshMemberMatchOptions options)
		{
			return (!this.IsHidden || (options & MshMemberMatchOptions.IncludeHidden) != MshMemberMatchOptions.None) && (this.ShouldSerialize || (options & MshMemberMatchOptions.OnlySerializable) == MshMemberMatchOptions.None);
		}

		// Token: 0x04000728 RID: 1832
		internal object instance;

		// Token: 0x04000729 RID: 1833
		internal string name;
	}
}
