using System;
using System.Management.Automation.Runspaces;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x02000449 RID: 1097
	internal class Serializer
	{
		// Token: 0x06002FDA RID: 12250 RVA: 0x00105675 File Offset: 0x00103875
		internal Serializer(XmlWriter writer) : this(writer, new SerializationContext())
		{
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x00105683 File Offset: 0x00103883
		internal Serializer(XmlWriter writer, int depth, bool useDepthFromTypes) : this(writer, new SerializationContext(depth, useDepthFromTypes))
		{
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x00105693 File Offset: 0x00103893
		internal Serializer(XmlWriter writer, SerializationContext context)
		{
			if (writer == null)
			{
				throw PSTraceSource.NewArgumentException("writer");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentException("context");
			}
			this._serializer = new InternalSerializer(writer, context);
			this._serializer.Start();
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06002FDD RID: 12253 RVA: 0x001056CF File Offset: 0x001038CF
		// (set) Token: 0x06002FDE RID: 12254 RVA: 0x001056DC File Offset: 0x001038DC
		internal TypeTable TypeTable
		{
			get
			{
				return this._serializer.TypeTable;
			}
			set
			{
				this._serializer.TypeTable = value;
			}
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x001056EA File Offset: 0x001038EA
		internal void Serialize(object source)
		{
			this.Serialize(source, null);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x001056F4 File Offset: 0x001038F4
		internal void Serialize(object source, string streamName)
		{
			this._serializer.WriteOneTopLevelObject(source, streamName);
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x00105703 File Offset: 0x00103903
		internal void Done()
		{
			this._serializer.End();
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x00105710 File Offset: 0x00103910
		internal void Stop()
		{
			this._serializer.Stop();
		}

		// Token: 0x040019E6 RID: 6630
		private readonly InternalSerializer _serializer;
	}
}
