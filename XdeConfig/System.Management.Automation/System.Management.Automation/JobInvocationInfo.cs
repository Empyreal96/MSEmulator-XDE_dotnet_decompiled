using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000282 RID: 642
	[Serializable]
	public class JobInvocationInfo : ISerializable
	{
		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x000B09EE File Offset: 0x000AEBEE
		// (set) Token: 0x06001E79 RID: 7801 RVA: 0x000B09F6 File Offset: 0x000AEBF6
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value == null)
				{
					throw new PSArgumentNullException("value");
				}
				this._name = value;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001E7A RID: 7802 RVA: 0x000B0A0D File Offset: 0x000AEC0D
		// (set) Token: 0x06001E7B RID: 7803 RVA: 0x000B0A24 File Offset: 0x000AEC24
		public string Command
		{
			get
			{
				return this._command ?? this._definition.Command;
			}
			set
			{
				this._command = value;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001E7C RID: 7804 RVA: 0x000B0A2D File Offset: 0x000AEC2D
		// (set) Token: 0x06001E7D RID: 7805 RVA: 0x000B0A35 File Offset: 0x000AEC35
		public JobDefinition Definition
		{
			get
			{
				return this._definition;
			}
			set
			{
				this._definition = value;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x000B0A40 File Offset: 0x000AEC40
		public List<CommandParameterCollection> Parameters
		{
			get
			{
				List<CommandParameterCollection> result;
				if ((result = this._parameters) == null)
				{
					result = (this._parameters = new List<CommandParameterCollection>());
				}
				return result;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x000B0A65 File Offset: 0x000AEC65
		public Guid InstanceId
		{
			get
			{
				return this._instanceId;
			}
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000B0A6D File Offset: 0x000AEC6D
		public virtual void Save(Stream stream)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000B0A74 File Offset: 0x000AEC74
		public virtual void Load(Stream stream)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000B0A7B File Offset: 0x000AEC7B
		protected JobInvocationInfo()
		{
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000B0A9C File Offset: 0x000AEC9C
		public JobInvocationInfo(JobDefinition definition, Dictionary<string, object> parameters)
		{
			this._definition = definition;
			CommandParameterCollection commandParameterCollection = JobInvocationInfo.ConvertDictionaryToParameterCollection(parameters);
			if (commandParameterCollection != null)
			{
				this.Parameters.Add(commandParameterCollection);
			}
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x000B0AE4 File Offset: 0x000AECE4
		public JobInvocationInfo(JobDefinition definition, IEnumerable<Dictionary<string, object>> parameterCollectionList)
		{
			this._definition = definition;
			if (parameterCollectionList == null)
			{
				return;
			}
			foreach (Dictionary<string, object> dictionary in parameterCollectionList)
			{
				if (dictionary != null)
				{
					CommandParameterCollection commandParameterCollection = JobInvocationInfo.ConvertDictionaryToParameterCollection(dictionary);
					if (commandParameterCollection != null)
					{
						this.Parameters.Add(commandParameterCollection);
					}
				}
			}
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x000B0B68 File Offset: 0x000AED68
		public JobInvocationInfo(JobDefinition definition, CommandParameterCollection parameters)
		{
			this._definition = definition;
			this.Parameters.Add(parameters ?? new CommandParameterCollection());
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x000B0BA4 File Offset: 0x000AEDA4
		public JobInvocationInfo(JobDefinition definition, IEnumerable<CommandParameterCollection> parameters)
		{
			this._definition = definition;
			if (parameters == null)
			{
				return;
			}
			foreach (CommandParameterCollection item in parameters)
			{
				this.Parameters.Add(item);
			}
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x000B0C18 File Offset: 0x000AEE18
		protected JobInvocationInfo(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000B0C3B File Offset: 0x000AEE3B
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x000B0C58 File Offset: 0x000AEE58
		private static CommandParameterCollection ConvertDictionaryToParameterCollection(IEnumerable<KeyValuePair<string, object>> parameters)
		{
			if (parameters == null)
			{
				return null;
			}
			CommandParameterCollection commandParameterCollection = new CommandParameterCollection();
			foreach (CommandParameter item in from param in parameters
			select new CommandParameter(param.Key, param.Value))
			{
				commandParameterCollection.Add(item);
			}
			return commandParameterCollection;
		}

		// Token: 0x04000D70 RID: 3440
		private string _name = string.Empty;

		// Token: 0x04000D71 RID: 3441
		private string _command;

		// Token: 0x04000D72 RID: 3442
		private JobDefinition _definition;

		// Token: 0x04000D73 RID: 3443
		private List<CommandParameterCollection> _parameters;

		// Token: 0x04000D74 RID: 3444
		private readonly Guid _instanceId = Guid.NewGuid();
	}
}
