using System;
using System.Reflection;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000149 RID: 329
	public class PSEvent : PSMemberInfo
	{
		// Token: 0x0600111D RID: 4381 RVA: 0x0005F174 File Offset: 0x0005D374
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.baseEvent.ToString());
			stringBuilder.Append("(");
			int num = 0;
			foreach (ParameterInfo parameterInfo in this.baseEvent.EventHandlerType.GetMethod("Invoke").GetParameters())
			{
				if (num > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(parameterInfo.ParameterType.ToString());
				num++;
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0005F211 File Offset: 0x0005D411
		internal PSEvent(EventInfo baseEvent)
		{
			this.baseEvent = baseEvent;
			this.name = baseEvent.Name;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0005F22C File Offset: 0x0005D42C
		public override PSMemberInfo Copy()
		{
			PSEvent psevent = new PSEvent(this.baseEvent);
			base.CloneBaseProperties(psevent);
			return psevent;
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x0005F24D File Offset: 0x0005D44D
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.Event;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x0005F254 File Offset: 0x0005D454
		// (set) Token: 0x06001122 RID: 4386 RVA: 0x0005F25C File Offset: 0x0005D45C
		public sealed override object Value
		{
			get
			{
				return this.baseEvent;
			}
			set
			{
				throw new ExtendedTypeSystemException("CannotChangePSEventInfoValue", null, ExtendedTypeSystem.CannotSetValueForMemberType, new object[]
				{
					base.GetType().FullName
				});
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x0005F28F File Offset: 0x0005D48F
		public override string TypeNameOfValue
		{
			get
			{
				return typeof(PSEvent).FullName;
			}
		}

		// Token: 0x0400075D RID: 1885
		internal EventInfo baseEvent;
	}
}
