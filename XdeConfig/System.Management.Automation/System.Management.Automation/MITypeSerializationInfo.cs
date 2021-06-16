using System;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x02000A46 RID: 2630
	internal class MITypeSerializationInfo
	{
		// Token: 0x06006988 RID: 27016 RVA: 0x00213CBB File Offset: 0x00211EBB
		internal MITypeSerializationInfo(Type type, MITypeSerializerDelegate serializer, string cimClassName)
		{
			this._type = type;
			this._serializer = serializer;
			this._cimType = CimConverter.GetCimType(type);
			this._cimClassName = cimClassName;
		}

		// Token: 0x06006989 RID: 27017 RVA: 0x00213CE4 File Offset: 0x00211EE4
		internal MITypeSerializationInfo(Type type, MITypeSerializerDelegate serializer, CimType cimType, string cimClassName) : this(type, serializer, cimClassName)
		{
			this._cimType = cimType;
		}

		// Token: 0x17001D8F RID: 7567
		// (get) Token: 0x0600698A RID: 27018 RVA: 0x00213CF7 File Offset: 0x00211EF7
		internal Type Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17001D90 RID: 7568
		// (get) Token: 0x0600698B RID: 27019 RVA: 0x00213CFF File Offset: 0x00211EFF
		internal MITypeSerializerDelegate Serializer
		{
			get
			{
				return this._serializer;
			}
		}

		// Token: 0x17001D91 RID: 7569
		// (get) Token: 0x0600698C RID: 27020 RVA: 0x00213D07 File Offset: 0x00211F07
		internal CimType CimType
		{
			get
			{
				return this._cimType;
			}
		}

		// Token: 0x17001D92 RID: 7570
		// (get) Token: 0x0600698D RID: 27021 RVA: 0x00213D0F File Offset: 0x00211F0F
		internal string CimClassName
		{
			get
			{
				return this._cimClassName;
			}
		}

		// Token: 0x04003283 RID: 12931
		private readonly Type _type;

		// Token: 0x04003284 RID: 12932
		private readonly MITypeSerializerDelegate _serializer;

		// Token: 0x04003285 RID: 12933
		private readonly CimType _cimType;

		// Token: 0x04003286 RID: 12934
		private string _cimClassName;
	}
}
