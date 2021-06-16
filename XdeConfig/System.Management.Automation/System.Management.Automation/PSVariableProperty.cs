using System;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200013E RID: 318
	public class PSVariableProperty : PSNoteProperty
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x0005D6D4 File Offset: 0x0005B8D4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(PSNoteProperty.GetDisplayTypeNameOfValue(this._variable.Value));
			stringBuilder.Append(" ");
			stringBuilder.Append(this._variable.Name);
			stringBuilder.Append("=");
			stringBuilder.Append(this._variable.Value ?? "null");
			return stringBuilder.ToString();
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0005D749 File Offset: 0x0005B949
		public PSVariableProperty(PSVariable variable) : base((variable != null) ? variable.Name : null, null)
		{
			if (variable == null)
			{
				throw PSTraceSource.NewArgumentException("variable");
			}
			this._variable = variable;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0005D774 File Offset: 0x0005B974
		public override PSMemberInfo Copy()
		{
			PSNoteProperty psnoteProperty = new PSVariableProperty(this._variable);
			base.CloneBaseProperties(psnoteProperty);
			return psnoteProperty;
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x0005D795 File Offset: 0x0005B995
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.NoteProperty;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x0005D798 File Offset: 0x0005B998
		public override bool IsSettable
		{
			get
			{
				return (this._variable.Options & (ScopedItemOptions.ReadOnly | ScopedItemOptions.Constant)) == ScopedItemOptions.None;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x0005D7AA File Offset: 0x0005B9AA
		public override bool IsGettable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x0005D7AD File Offset: 0x0005B9AD
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x0005D7BC File Offset: 0x0005B9BC
		public override object Value
		{
			get
			{
				return this._variable.Value;
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
				this._variable.Value = value;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x0005D800 File Offset: 0x0005BA00
		public override string TypeNameOfValue
		{
			get
			{
				object value = this._variable.Value;
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

		// Token: 0x04000739 RID: 1849
		internal PSVariable _variable;
	}
}
