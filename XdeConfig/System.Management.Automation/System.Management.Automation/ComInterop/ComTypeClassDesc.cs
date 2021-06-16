using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A74 RID: 2676
	internal class ComTypeClassDesc : ComTypeDesc, IDynamicMetaObjectProvider
	{
		// Token: 0x06006A94 RID: 27284 RVA: 0x0021783D File Offset: 0x00215A3D
		public object CreateInstance()
		{
			if (this._typeObj == null)
			{
				this._typeObj = Type.GetTypeFromCLSID(base.Guid);
			}
			return Activator.CreateInstance(Type.GetTypeFromCLSID(base.Guid));
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x00217870 File Offset: 0x00215A70
		internal ComTypeClassDesc(ITypeInfo typeInfo, ComTypeLibDesc typeLibDesc) : base(typeInfo, ComType.Class, typeLibDesc)
		{
			TYPEATTR typeAttrForTypeInfo = ComRuntimeHelpers.GetTypeAttrForTypeInfo(typeInfo);
			base.Guid = typeAttrForTypeInfo.guid;
			for (int i = 0; i < (int)typeAttrForTypeInfo.cImplTypes; i++)
			{
				int hRef;
				typeInfo.GetRefTypeOfImplType(i, out hRef);
				ITypeInfo itfTypeInfo;
				typeInfo.GetRefTypeInfo(hRef, out itfTypeInfo);
				IMPLTYPEFLAGS impltypeflags;
				typeInfo.GetImplTypeFlags(i, out impltypeflags);
				bool isSourceItf = (impltypeflags & IMPLTYPEFLAGS.IMPLTYPEFLAG_FSOURCE) != (IMPLTYPEFLAGS)0;
				this.AddInterface(itfTypeInfo, isSourceItf);
			}
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x002178DC File Offset: 0x00215ADC
		private void AddInterface(ITypeInfo itfTypeInfo, bool isSourceItf)
		{
			string nameOfType = ComRuntimeHelpers.GetNameOfType(itfTypeInfo);
			if (isSourceItf)
			{
				if (this._sourceItfs == null)
				{
					this._sourceItfs = new LinkedList<string>();
				}
				this._sourceItfs.AddLast(nameOfType);
				return;
			}
			if (this._itfs == null)
			{
				this._itfs = new LinkedList<string>();
			}
			this._itfs.AddLast(nameOfType);
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x00217934 File Offset: 0x00215B34
		internal bool Implements(string itfName, bool isSourceItf)
		{
			if (isSourceItf)
			{
				return this._sourceItfs.Contains(itfName);
			}
			return this._itfs.Contains(itfName);
		}

		// Token: 0x06006A98 RID: 27288 RVA: 0x00217952 File Offset: 0x00215B52
		public DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new ComClassMetaObject(parameter, this);
		}

		// Token: 0x04003317 RID: 13079
		private LinkedList<string> _itfs;

		// Token: 0x04003318 RID: 13080
		private LinkedList<string> _sourceItfs;

		// Token: 0x04003319 RID: 13081
		private Type _typeObj;
	}
}
