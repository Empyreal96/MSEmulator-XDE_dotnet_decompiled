using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Host
{
	// Token: 0x020001FD RID: 509
	public class FieldDescription
	{
		// Token: 0x06001794 RID: 6036 RVA: 0x000923B0 File Offset: 0x000905B0
		public FieldDescription(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name", DescriptionsStrings.NullOrEmptyErrorTemplate, new object[]
				{
					"name"
				});
			}
			this.name = name;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0009241A File Offset: 0x0009061A
		internal FieldDescription()
		{
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x0009244A File Offset: 0x0009064A
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x00092452 File Offset: 0x00090652
		public void SetParameterType(Type parameterType)
		{
			if (parameterType == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterType");
			}
			this.SetParameterTypeName(parameterType.Name);
			this.SetParameterTypeFullName(parameterType.FullName);
			this.SetParameterAssemblyFullName(parameterType.AssemblyQualifiedName);
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x0009248C File Offset: 0x0009068C
		public string ParameterTypeName
		{
			get
			{
				if (string.IsNullOrEmpty(this.parameterTypeName))
				{
					this.SetParameterType(typeof(string));
				}
				return this.parameterTypeName;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x000924B1 File Offset: 0x000906B1
		public string ParameterTypeFullName
		{
			get
			{
				if (string.IsNullOrEmpty(this.parameterTypeFullName))
				{
					this.SetParameterType(typeof(string));
				}
				return this.parameterTypeFullName;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x000924D6 File Offset: 0x000906D6
		public string ParameterAssemblyFullName
		{
			get
			{
				if (string.IsNullOrEmpty(this.parameterAssemblyFullName))
				{
					this.SetParameterType(typeof(string));
				}
				return this.parameterAssemblyFullName;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x000924FB File Offset: 0x000906FB
		// (set) Token: 0x0600179C RID: 6044 RVA: 0x00092503 File Offset: 0x00090703
		public string Label
		{
			get
			{
				return this.label;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				this.label = value;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x0009251A File Offset: 0x0009071A
		// (set) Token: 0x0600179E RID: 6046 RVA: 0x00092522 File Offset: 0x00090722
		public string HelpMessage
		{
			get
			{
				return this.helpMessage;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				this.helpMessage = value;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x00092539 File Offset: 0x00090739
		// (set) Token: 0x060017A0 RID: 6048 RVA: 0x00092541 File Offset: 0x00090741
		public bool IsMandatory
		{
			get
			{
				return this.isMandatory;
			}
			set
			{
				this.isMandatory = value;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x0009254A File Offset: 0x0009074A
		// (set) Token: 0x060017A2 RID: 6050 RVA: 0x00092552 File Offset: 0x00090752
		public PSObject DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				this.defaultValue = value;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x0009255B File Offset: 0x0009075B
		public Collection<Attribute> Attributes
		{
			get
			{
				if (this.metadata == null)
				{
					this.metadata = new Collection<Attribute>();
				}
				return this.metadata;
			}
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00092578 File Offset: 0x00090778
		internal void SetParameterTypeName(string nameOfType)
		{
			if (string.IsNullOrEmpty(nameOfType))
			{
				throw PSTraceSource.NewArgumentException("nameOfType", DescriptionsStrings.NullOrEmptyErrorTemplate, new object[]
				{
					"nameOfType"
				});
			}
			this.parameterTypeName = nameOfType;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000925B4 File Offset: 0x000907B4
		internal void SetParameterTypeFullName(string fullNameOfType)
		{
			if (string.IsNullOrEmpty(fullNameOfType))
			{
				throw PSTraceSource.NewArgumentException("fullNameOfType", DescriptionsStrings.NullOrEmptyErrorTemplate, new object[]
				{
					"fullNameOfType"
				});
			}
			this.parameterTypeFullName = fullNameOfType;
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000925F0 File Offset: 0x000907F0
		internal void SetParameterAssemblyFullName(string fullNameOfAssembly)
		{
			if (string.IsNullOrEmpty(fullNameOfAssembly))
			{
				throw PSTraceSource.NewArgumentException("fullNameOfAssembly", DescriptionsStrings.NullOrEmptyErrorTemplate, new object[]
				{
					"fullNameOfAssembly"
				});
			}
			this.parameterAssemblyFullName = fullNameOfAssembly;
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060017A7 RID: 6055 RVA: 0x0009262C File Offset: 0x0009082C
		// (set) Token: 0x060017A8 RID: 6056 RVA: 0x00092634 File Offset: 0x00090834
		internal bool ModifiedByRemotingProtocol
		{
			get
			{
				return this.modifiedByRemotingProtocol;
			}
			set
			{
				this.modifiedByRemotingProtocol = value;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x0009263D File Offset: 0x0009083D
		// (set) Token: 0x060017AA RID: 6058 RVA: 0x00092645 File Offset: 0x00090845
		internal bool IsFromRemoteHost
		{
			get
			{
				return this.isFromRemoteHost;
			}
			set
			{
				this.isFromRemoteHost = value;
			}
		}

		// Token: 0x040009F3 RID: 2547
		private readonly string name;

		// Token: 0x040009F4 RID: 2548
		private string label = "";

		// Token: 0x040009F5 RID: 2549
		private string parameterTypeName;

		// Token: 0x040009F6 RID: 2550
		private string parameterTypeFullName;

		// Token: 0x040009F7 RID: 2551
		private string parameterAssemblyFullName;

		// Token: 0x040009F8 RID: 2552
		private string helpMessage = "";

		// Token: 0x040009F9 RID: 2553
		private bool isMandatory = true;

		// Token: 0x040009FA RID: 2554
		private PSObject defaultValue;

		// Token: 0x040009FB RID: 2555
		private Collection<Attribute> metadata = new Collection<Attribute>();

		// Token: 0x040009FC RID: 2556
		private bool modifiedByRemotingProtocol;

		// Token: 0x040009FD RID: 2557
		private bool isFromRemoteHost;
	}
}
