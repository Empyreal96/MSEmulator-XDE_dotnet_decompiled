using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000899 RID: 2201
	[Serializable]
	public class IncompleteParseException : ParseException
	{
		// Token: 0x06005459 RID: 21593 RVA: 0x001BD701 File Offset: 0x001BB901
		protected IncompleteParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x001BD70B File Offset: 0x001BB90B
		public IncompleteParseException()
		{
			base.SetErrorId("IncompleteParse");
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x001BD71E File Offset: 0x001BB91E
		public IncompleteParseException(string message) : base(message)
		{
			base.SetErrorId("IncompleteParse");
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x001BD732 File Offset: 0x001BB932
		internal IncompleteParseException(string message, string errorId) : base(message, errorId)
		{
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x001BD73C File Offset: 0x001BB93C
		internal IncompleteParseException(string message, string errorId, Exception innerException) : base(message, errorId, innerException)
		{
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x001BD747 File Offset: 0x001BB947
		public IncompleteParseException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorId("IncompleteParse");
		}

		// Token: 0x04002B36 RID: 11062
		private const string errorIdString = "IncompleteParse";
	}
}
