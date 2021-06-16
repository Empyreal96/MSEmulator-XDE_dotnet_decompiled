using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000145 RID: 325
	public class PSParameterizedProperty : PSMethodInfo
	{
		// Token: 0x060010F2 RID: 4338 RVA: 0x0005E6F7 File Offset: 0x0005C8F7
		public override string ToString()
		{
			return this.adapter.BaseParameterizedPropertyToString(this);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0005E705 File Offset: 0x0005C905
		internal PSParameterizedProperty(string name, Adapter adapter, object baseObject, object adapterData)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			this.adapter = adapter;
			this.adapterData = adapterData;
			this.baseObject = baseObject;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0005E73D File Offset: 0x0005C93D
		internal PSParameterizedProperty(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0005E75F File Offset: 0x0005C95F
		public bool IsSettable
		{
			get
			{
				return this.adapter.BaseParameterizedPropertyIsSettable(this);
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0005E76D File Offset: 0x0005C96D
		public bool IsGettable
		{
			get
			{
				return this.adapter.BaseParameterizedPropertyIsGettable(this);
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0005E77B File Offset: 0x0005C97B
		public override object Invoke(params object[] arguments)
		{
			if (arguments == null)
			{
				throw PSTraceSource.NewArgumentNullException("arguments");
			}
			return this.adapter.BaseParameterizedPropertyGet(this, arguments);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0005E798 File Offset: 0x0005C998
		public void InvokeSet(object valueToSet, params object[] arguments)
		{
			if (arguments == null)
			{
				throw PSTraceSource.NewArgumentNullException("arguments");
			}
			this.adapter.BaseParameterizedPropertySet(this, valueToSet, arguments);
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0005E7B6 File Offset: 0x0005C9B6
		public override Collection<string> OverloadDefinitions
		{
			get
			{
				return this.adapter.BaseParameterizedPropertyDefinitions(this);
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x0005E7C4 File Offset: 0x0005C9C4
		public override string TypeNameOfValue
		{
			get
			{
				return this.adapter.BaseParameterizedPropertyType(this);
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0005E7D4 File Offset: 0x0005C9D4
		public override PSMemberInfo Copy()
		{
			PSParameterizedProperty psparameterizedProperty = new PSParameterizedProperty(this.name, this.adapter, this.baseObject, this.adapterData);
			base.CloneBaseProperties(psparameterizedProperty);
			return psparameterizedProperty;
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0005E807 File Offset: 0x0005CA07
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.ParameterizedProperty;
			}
		}

		// Token: 0x0400074B RID: 1867
		internal Adapter adapter;

		// Token: 0x0400074C RID: 1868
		internal object adapterData;

		// Token: 0x0400074D RID: 1869
		internal object baseObject;
	}
}
