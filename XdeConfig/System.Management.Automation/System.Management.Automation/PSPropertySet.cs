using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000148 RID: 328
	public class PSPropertySet : PSMemberInfo
	{
		// Token: 0x06001115 RID: 4373 RVA: 0x0005EFB4 File Offset: 0x0005D1B4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Name);
			stringBuilder.Append(" {");
			if (this.referencedPropertyNames.Count != 0)
			{
				foreach (string value in this.referencedPropertyNames)
				{
					stringBuilder.Append(value);
					stringBuilder.Append(", ");
				}
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0005F05C File Offset: 0x0005D25C
		public PSPropertySet(string name, IEnumerable<string> referencedPropertyNames)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (referencedPropertyNames == null)
			{
				throw PSTraceSource.NewArgumentNullException("referencedPropertyNames");
			}
			this.referencedPropertyNames = new Collection<string>();
			foreach (string text in referencedPropertyNames)
			{
				if (string.IsNullOrEmpty(text))
				{
					throw PSTraceSource.NewArgumentException("referencedPropertyNames");
				}
				this.referencedPropertyNames.Add(text);
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x0005F0F8 File Offset: 0x0005D2F8
		public Collection<string> ReferencedPropertyNames
		{
			get
			{
				return this.referencedPropertyNames;
			}
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0005F100 File Offset: 0x0005D300
		public override PSMemberInfo Copy()
		{
			PSPropertySet pspropertySet = new PSPropertySet(this.name, this.referencedPropertyNames);
			base.CloneBaseProperties(pspropertySet);
			return pspropertySet;
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x0005F127 File Offset: 0x0005D327
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.PropertySet;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600111A RID: 4378 RVA: 0x0005F12B File Offset: 0x0005D32B
		// (set) Token: 0x0600111B RID: 4379 RVA: 0x0005F130 File Offset: 0x0005D330
		public override object Value
		{
			get
			{
				return this;
			}
			set
			{
				throw new ExtendedTypeSystemException("CannotChangePSPropertySetValue", null, ExtendedTypeSystem.CannotSetValueForMemberType, new object[]
				{
					base.GetType().FullName
				});
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x0005F163 File Offset: 0x0005D363
		public override string TypeNameOfValue
		{
			get
			{
				return typeof(PSPropertySet).FullName;
			}
		}

		// Token: 0x0400075C RID: 1884
		private Collection<string> referencedPropertyNames;
	}
}
