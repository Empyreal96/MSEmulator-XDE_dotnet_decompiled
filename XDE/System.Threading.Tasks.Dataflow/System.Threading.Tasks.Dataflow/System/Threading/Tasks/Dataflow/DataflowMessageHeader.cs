using System;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a container of data attributes for passing between dataflow blocks.</summary>
	// Token: 0x0200002C RID: 44
	[DebuggerDisplay("Id = {Id}")]
	public readonly struct DataflowMessageHeader : IEquatable<DataflowMessageHeader>
	{
		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> with the specified attributes.</summary>
		/// <param name="id">The ID of the message. Must be unique within the originating source block. It does not need to be globally unique.</param>
		// Token: 0x06000117 RID: 279 RVA: 0x0000506E File Offset: 0x0000326E
		public DataflowMessageHeader(long id)
		{
			if (id == 0L)
			{
				throw new ArgumentException(SR.Argument_InvalidMessageId, "id");
			}
			this._id = id;
		}

		/// <summary>Gets the validity of the message.</summary>
		/// <returns>true if the ID of the message is different from 0. false if the ID of the message is 0.</returns>
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000508A File Offset: 0x0000328A
		public bool IsValid
		{
			get
			{
				return this._id != 0L;
			}
		}

		/// <summary>Gets the ID of the message within the source.</summary>
		/// <returns>The ID contained in the <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance.</returns>
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00005096 File Offset: 0x00003296
		public long Id
		{
			get
			{
				return this._id;
			}
		}

		/// <summary>Checks two <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instances for equality by ID without boxing.</summary>
		/// <returns>true if the instances are equal; otherwise, false.</returns>
		/// <param name="other">Another <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance.</param>
		// Token: 0x0600011A RID: 282 RVA: 0x0000509E File Offset: 0x0000329E
		public bool Equals(DataflowMessageHeader other)
		{
			return this == other;
		}

		/// <summary>Checks boxed <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instances for equality by ID.</summary>
		/// <returns>true if the instances are equal; otherwise, false.</returns>
		/// <param name="obj">A boxed <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance.</param>
		// Token: 0x0600011B RID: 283 RVA: 0x000050AC File Offset: 0x000032AC
		public override bool Equals(object obj)
		{
			return obj is DataflowMessageHeader && this == (DataflowMessageHeader)obj;
		}

		/// <summary>Generates a hash code for the <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance.</summary>
		/// <returns>The hash code.</returns>
		// Token: 0x0600011C RID: 284 RVA: 0x000050C9 File Offset: 0x000032C9
		public override int GetHashCode()
		{
			return (int)this.Id;
		}

		/// <summary>Checks two <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instances for equality by ID.</summary>
		/// <returns>true if the instances are equal; otherwise, false.</returns>
		/// <param name="left">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance.</param>
		/// <param name="right">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance.</param>
		// Token: 0x0600011D RID: 285 RVA: 0x000050D2 File Offset: 0x000032D2
		public static bool operator ==(DataflowMessageHeader left, DataflowMessageHeader right)
		{
			return left.Id == right.Id;
		}

		/// <summary>Checks two <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instances for non-equality by ID.</summary>
		/// <returns>true if the instances are not equal; otherwise, false.</returns>
		/// <param name="left">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance.</param>
		/// <param name="right">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> instance.</param>
		// Token: 0x0600011E RID: 286 RVA: 0x000050E4 File Offset: 0x000032E4
		public static bool operator !=(DataflowMessageHeader left, DataflowMessageHeader right)
		{
			return left.Id != right.Id;
		}

		// Token: 0x04000079 RID: 121
		private readonly long _id;
	}
}
