using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020008A9 RID: 2217
	[Serializable]
	public class ProviderNameAmbiguousException : ProviderNotFoundException
	{
		// Token: 0x060054B2 RID: 21682 RVA: 0x001BF342 File Offset: 0x001BD542
		internal ProviderNameAmbiguousException(string providerName, string errorIdAndResourceId, string resourceStr, Collection<ProviderInfo> possibleMatches, params object[] messageArgs) : base(providerName, SessionStateCategory.CmdletProvider, errorIdAndResourceId, resourceStr, messageArgs)
		{
			this._possibleMatches = new ReadOnlyCollection<ProviderInfo>(possibleMatches);
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x001BF35D File Offset: 0x001BD55D
		public ProviderNameAmbiguousException()
		{
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x001BF365 File Offset: 0x001BD565
		public ProviderNameAmbiguousException(string message) : base(message)
		{
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x001BF36E File Offset: 0x001BD56E
		public ProviderNameAmbiguousException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x001BF378 File Offset: 0x001BD578
		protected ProviderNameAmbiguousException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x060054B7 RID: 21687 RVA: 0x001BF382 File Offset: 0x001BD582
		public ReadOnlyCollection<ProviderInfo> PossibleMatches
		{
			get
			{
				return this._possibleMatches;
			}
		}

		// Token: 0x04002B7E RID: 11134
		private ReadOnlyCollection<ProviderInfo> _possibleMatches;
	}
}
