using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000142 RID: 322
	public class PSCodeMethod : PSMethodInfo
	{
		// Token: 0x060010D1 RID: 4305 RVA: 0x0005E018 File Offset: 0x0005C218
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in this.OverloadDefinitions)
			{
				stringBuilder.Append(value);
				stringBuilder.Append(", ");
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			return stringBuilder.ToString();
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0005E090 File Offset: 0x0005C290
		internal static bool CheckMethodInfo(MethodInfo method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			return method.IsStatic && method.IsPublic && parameters.Length != 0 && parameters[0].ParameterType == typeof(PSObject);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0005E0D4 File Offset: 0x0005C2D4
		internal void SetCodeReference(Type type, string methodName)
		{
			MethodInfo methodInfo = null;
			try
			{
				methodInfo = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
			}
			catch (AmbiguousMatchException)
			{
			}
			if (methodInfo == null)
			{
				throw new ExtendedTypeSystemException("WrongMethodFormatFromTypeTable", null, ExtendedTypeSystem.CodeMethodMethodFormat, new object[0]);
			}
			this.codeReference = new MethodInfo[]
			{
				methodInfo
			};
			this.codeReferenceDefinition = new string[]
			{
				DotNetAdapter.GetMethodInfoOverloadDefinition(null, this.codeReference[0], 0)
			};
			this.codeReferenceMethodInformation = DotNetAdapter.GetMethodInformationArray(this.codeReference);
			if (!PSCodeMethod.CheckMethodInfo(this.codeReference[0]))
			{
				throw new ExtendedTypeSystemException("WrongMethodFormat", null, ExtendedTypeSystem.CodeMethodMethodFormat, new object[0]);
			}
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0005E18C File Offset: 0x0005C38C
		internal PSCodeMethod(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0005E1B0 File Offset: 0x0005C3B0
		public PSCodeMethod(string name, MethodInfo codeReference)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (codeReference == null)
			{
				throw PSTraceSource.NewArgumentNullException("codeReference");
			}
			if (!PSCodeMethod.CheckMethodInfo(codeReference))
			{
				throw new ExtendedTypeSystemException("WrongMethodFormat", null, ExtendedTypeSystem.CodeMethodMethodFormat, new object[0]);
			}
			this.name = name;
			this.codeReference = new MethodInfo[]
			{
				codeReference
			};
			this.codeReferenceDefinition = new string[]
			{
				DotNetAdapter.GetMethodInfoOverloadDefinition(null, this.codeReference[0], 0)
			};
			this.codeReferenceMethodInformation = DotNetAdapter.GetMethodInformationArray(this.codeReference);
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x0005E253 File Offset: 0x0005C453
		public MethodInfo CodeReference
		{
			get
			{
				return this.codeReference[0];
			}
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0005E260 File Offset: 0x0005C460
		public override PSMemberInfo Copy()
		{
			PSCodeMethod pscodeMethod = new PSCodeMethod(this.name, this.codeReference[0]);
			base.CloneBaseProperties(pscodeMethod);
			return pscodeMethod;
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x0005E289 File Offset: 0x0005C489
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.CodeMethod;
			}
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0005E290 File Offset: 0x0005C490
		public override object Invoke(params object[] arguments)
		{
			if (arguments == null)
			{
				throw PSTraceSource.NewArgumentNullException("arguments");
			}
			object[] array = new object[arguments.Length + 1];
			array[0] = this.instance;
			for (int i = 0; i < arguments.Length; i++)
			{
				array[i + 1] = arguments[i];
			}
			object[] arguments2;
			Adapter.GetBestMethodAndArguments(this.codeReference[0].Name, this.codeReferenceMethodInformation, array, out arguments2);
			return DotNetAdapter.AuxiliaryMethodInvoke(null, arguments2, this.codeReferenceMethodInformation[0], array);
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x0005E304 File Offset: 0x0005C504
		public override Collection<string> OverloadDefinitions
		{
			get
			{
				return new Collection<string>
				{
					this.codeReferenceDefinition[0]
				};
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x0005E326 File Offset: 0x0005C526
		public override string TypeNameOfValue
		{
			get
			{
				return typeof(PSCodeMethod).FullName;
			}
		}

		// Token: 0x04000742 RID: 1858
		private MethodInfo[] codeReference;

		// Token: 0x04000743 RID: 1859
		private string[] codeReferenceDefinition;

		// Token: 0x04000744 RID: 1860
		private MethodInformation[] codeReferenceMethodInformation;
	}
}
