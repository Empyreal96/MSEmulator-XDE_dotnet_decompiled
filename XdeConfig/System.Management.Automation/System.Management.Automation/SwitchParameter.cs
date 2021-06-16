using System;

namespace System.Management.Automation
{
	// Token: 0x02000007 RID: 7
	public struct SwitchParameter
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002331 File Offset: 0x00000531
		public bool IsPresent
		{
			get
			{
				return this.isPresent;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002339 File Offset: 0x00000539
		public static implicit operator bool(SwitchParameter switchParameter)
		{
			return switchParameter.IsPresent;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002342 File Offset: 0x00000542
		public static implicit operator SwitchParameter(bool value)
		{
			return new SwitchParameter(value);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000234A File Offset: 0x0000054A
		public bool ToBool()
		{
			return this.isPresent;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002352 File Offset: 0x00000552
		public SwitchParameter(bool isPresent)
		{
			this.isPresent = isPresent;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000235B File Offset: 0x0000055B
		public static SwitchParameter Present
		{
			get
			{
				return new SwitchParameter(true);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002364 File Offset: 0x00000564
		public override bool Equals(object obj)
		{
			if (obj is bool)
			{
				return this.isPresent == (bool)obj;
			}
			return obj is SwitchParameter && this.isPresent == ((SwitchParameter)obj).IsPresent;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023A8 File Offset: 0x000005A8
		public override int GetHashCode()
		{
			return this.isPresent.GetHashCode();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023B5 File Offset: 0x000005B5
		public static bool operator ==(SwitchParameter first, SwitchParameter second)
		{
			return first.Equals(second);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023CA File Offset: 0x000005CA
		public static bool operator !=(SwitchParameter first, SwitchParameter second)
		{
			return !first.Equals(second);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023E2 File Offset: 0x000005E2
		public static bool operator ==(SwitchParameter first, bool second)
		{
			return first.Equals(second);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000023F7 File Offset: 0x000005F7
		public static bool operator !=(SwitchParameter first, bool second)
		{
			return !first.Equals(second);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000240F File Offset: 0x0000060F
		public static bool operator ==(bool first, SwitchParameter second)
		{
			return first.Equals(second);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000241E File Offset: 0x0000061E
		public static bool operator !=(bool first, SwitchParameter second)
		{
			return !first.Equals(second);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002430 File Offset: 0x00000630
		public override string ToString()
		{
			return this.isPresent.ToString();
		}

		// Token: 0x04000011 RID: 17
		private bool isPresent;
	}
}
