using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020005F4 RID: 1524
	[Serializable]
	internal class ScriptBlockSerializationHelper : ISerializable, IObjectReference
	{
		// Token: 0x060041AA RID: 16810 RVA: 0x0015B574 File Offset: 0x00159774
		private ScriptBlockSerializationHelper(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.scriptText = (info.GetValue("ScriptText", typeof(string)) as string);
			if (this.scriptText == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x0015B5C8 File Offset: 0x001597C8
		public object GetRealObject(StreamingContext context)
		{
			return ScriptBlock.Create(this.scriptText);
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x0015B5D5 File Offset: 0x001597D5
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040020EA RID: 8426
		private readonly string scriptText;
	}
}
