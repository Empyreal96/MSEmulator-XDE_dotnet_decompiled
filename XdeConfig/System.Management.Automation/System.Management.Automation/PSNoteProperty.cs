using System;
using System.Management.Automation.Runspaces;
using System.Text;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x0200013D RID: 317
	public class PSNoteProperty : PSPropertyInfo
	{
		// Token: 0x0600109F RID: 4255 RVA: 0x0005D50C File Offset: 0x0005B70C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(PSNoteProperty.GetDisplayTypeNameOfValue(this.Value));
			stringBuilder.Append(" ");
			stringBuilder.Append(base.Name);
			stringBuilder.Append("=");
			stringBuilder.Append((this.noteValue == null) ? "null" : this.noteValue.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0005D57D File Offset: 0x0005B77D
		public PSNoteProperty(string name, object value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			this.noteValue = value;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0005D5A8 File Offset: 0x0005B7A8
		public override PSMemberInfo Copy()
		{
			PSNoteProperty psnoteProperty = new PSNoteProperty(this.name, this.noteValue);
			base.CloneBaseProperties(psnoteProperty);
			return psnoteProperty;
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x0005D5CF File Offset: 0x0005B7CF
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.NoteProperty;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060010A3 RID: 4259 RVA: 0x0005D5D2 File Offset: 0x0005B7D2
		public override bool IsSettable
		{
			get
			{
				return base.IsInstance;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0005D5DA File Offset: 0x0005B7DA
		public override bool IsGettable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x0005D5DD File Offset: 0x0005B7DD
		// (set) Token: 0x060010A6 RID: 4262 RVA: 0x0005D5E8 File Offset: 0x0005B7E8
		public override object Value
		{
			get
			{
				return this.noteValue;
			}
			set
			{
				if (!base.IsInstance)
				{
					throw new SetValueException("ChangeValueOfStaticNote", null, ExtendedTypeSystem.ChangeStaticMember, new object[]
					{
						base.Name
					});
				}
				this.noteValue = value;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x0005D628 File Offset: 0x0005B828
		public override string TypeNameOfValue
		{
			get
			{
				object value = this.Value;
				if (value == null)
				{
					return typeof(object).FullName;
				}
				PSObject psobject = value as PSObject;
				if (psobject != null)
				{
					ConsolidatedString internalTypeNames = psobject.InternalTypeNames;
					if (internalTypeNames != null && internalTypeNames.Count >= 1)
					{
						return internalTypeNames[0];
					}
				}
				return value.GetType().FullName;
			}
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0005D680 File Offset: 0x0005B880
		internal static string GetDisplayTypeNameOfValue(object val)
		{
			string text = null;
			PSObject psobject = val as PSObject;
			if (psobject != null)
			{
				ConsolidatedString internalTypeNames = psobject.InternalTypeNames;
				if (internalTypeNames != null && internalTypeNames.Count >= 1)
				{
					text = internalTypeNames[0];
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = ((val == null) ? "object" : ToStringCodeMethods.Type(val.GetType(), true));
			}
			return text;
		}

		// Token: 0x04000738 RID: 1848
		internal object noteValue;
	}
}
