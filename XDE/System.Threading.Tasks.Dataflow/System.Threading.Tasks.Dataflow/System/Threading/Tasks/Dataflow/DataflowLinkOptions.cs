using System;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides options used to configure a link between dataflow blocks.</summary>
	// Token: 0x0200002B RID: 43
	[DebuggerDisplay("PropagateCompletion = {PropagateCompletion}, MaxMessages = {MaxMessages}, Append = {Append}")]
	public class DataflowLinkOptions
	{
		/// <summary>Gets or sets whether the linked target will have completion and faulting notification propagated to it automatically.</summary>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005004 File Offset: 0x00003204
		// (set) Token: 0x06000111 RID: 273 RVA: 0x0000500C File Offset: 0x0000320C
		public bool PropagateCompletion
		{
			get
			{
				return this._propagateCompletion;
			}
			set
			{
				this._propagateCompletion = value;
			}
		}

		/// <summary>Gets or sets the maximum number of messages that may be consumed across the link.</summary>
		/// <returns>Returns <see cref="T:System.Int32" />.</returns>
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005015 File Offset: 0x00003215
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000501D File Offset: 0x0000321D
		public int MaxMessages
		{
			get
			{
				return this._maxNumberOfMessages;
			}
			set
			{
				if (value < 1 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._maxNumberOfMessages = value;
			}
		}

		/// <summary>Gets or sets whether the link should be appended to the source’s list of links, or whether it should be prepended.</summary>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005039 File Offset: 0x00003239
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00005041 File Offset: 0x00003241
		public bool Append
		{
			get
			{
				return this._append;
			}
			set
			{
				this._append = value;
			}
		}

		// Token: 0x04000074 RID: 116
		private bool _propagateCompletion;

		// Token: 0x04000075 RID: 117
		private int _maxNumberOfMessages = -1;

		// Token: 0x04000076 RID: 118
		private bool _append = true;

		// Token: 0x04000077 RID: 119
		internal static readonly DataflowLinkOptions Default = new DataflowLinkOptions();

		// Token: 0x04000078 RID: 120
		internal static readonly DataflowLinkOptions UnlinkAfterOneAndPropagateCompletion = new DataflowLinkOptions
		{
			MaxMessages = 1,
			PropagateCompletion = true
		};
	}
}
