using System;
using System.ComponentModel;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B7 RID: 183
	public class JPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06000A74 RID: 2676 RVA: 0x0002A98B File Offset: 0x00028B8B
		public JPropertyDescriptor(string name) : base(name, null)
		{
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0002A995 File Offset: 0x00028B95
		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002A99D File Offset: 0x00028B9D
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0002A9A0 File Offset: 0x00028BA0
		public override object GetValue(object component)
		{
			JObject jobject = component as JObject;
			if (jobject == null)
			{
				return null;
			}
			return jobject[this.Name];
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0002A9B9 File Offset: 0x00028BB9
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0002A9BC File Offset: 0x00028BBC
		public override void SetValue(object component, object value)
		{
			JObject jobject;
			if ((jobject = (component as JObject)) != null)
			{
				JToken value2 = (value as JToken) ?? new JValue(value);
				jobject[this.Name] = value2;
			}
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0002A9F1 File Offset: 0x00028BF1
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0002A9F4 File Offset: 0x00028BF4
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0002AA00 File Offset: 0x00028C00
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0002AA03 File Offset: 0x00028C03
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0002AA0F File Offset: 0x00028C0F
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}
	}
}
