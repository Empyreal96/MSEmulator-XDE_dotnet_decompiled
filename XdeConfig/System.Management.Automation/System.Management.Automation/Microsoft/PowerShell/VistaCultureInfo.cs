using System;
using System.Globalization;

namespace Microsoft.PowerShell
{
	// Token: 0x02000254 RID: 596
	internal class VistaCultureInfo : CultureInfo
	{
		// Token: 0x06001C43 RID: 7235 RVA: 0x000A4B4E File Offset: 0x000A2D4E
		public VistaCultureInfo(string name, string[] fallbacks) : base(name)
		{
			this.m_fallbacks = fallbacks;
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x000A4B6C File Offset: 0x000A2D6C
		public override CultureInfo Parent
		{
			get
			{
				if (base.Parent != null && !string.IsNullOrEmpty(base.Parent.Name))
				{
					return this.ImmediateParent;
				}
				while (this.m_fallbacks != null && this.m_fallbacks.Length > 0)
				{
					string name = this.m_fallbacks[0];
					string[] array = null;
					if (this.m_fallbacks.Length > 1)
					{
						array = new string[this.m_fallbacks.Length - 1];
						Array.Copy(this.m_fallbacks, 1, array, 0, this.m_fallbacks.Length - 1);
					}
					try
					{
						return new VistaCultureInfo(name, array);
					}
					catch (ArgumentException)
					{
						this.m_fallbacks = array;
					}
				}
				return base.Parent;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x000A4C18 File Offset: 0x000A2E18
		private VistaCultureInfo ImmediateParent
		{
			get
			{
				if (this.parentCI == null)
				{
					lock (this.syncObject)
					{
						if (this.parentCI == null)
						{
							string name = base.Parent.Name;
							string[] array = null;
							if (this.m_fallbacks != null)
							{
								array = new string[this.m_fallbacks.Length];
								int num = 0;
								foreach (string text in this.m_fallbacks)
								{
									if (!name.Equals(text, StringComparison.OrdinalIgnoreCase))
									{
										array[num] = text;
										num++;
									}
								}
								if (this.m_fallbacks.Length != num)
								{
									Array.Resize<string>(ref array, num);
								}
							}
							this.parentCI = new VistaCultureInfo(name, array);
						}
					}
				}
				return this.parentCI;
			}
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000A4CE8 File Offset: 0x000A2EE8
		public override object Clone()
		{
			return new VistaCultureInfo(base.Name, this.m_fallbacks);
		}

		// Token: 0x04000BAC RID: 2988
		private string[] m_fallbacks;

		// Token: 0x04000BAD RID: 2989
		private VistaCultureInfo parentCI;

		// Token: 0x04000BAE RID: 2990
		private object syncObject = new object();
	}
}
