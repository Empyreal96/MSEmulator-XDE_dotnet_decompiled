using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000243 RID: 579
	[Serializable]
	public class InvalidRunspacePoolStateException : SystemException
	{
		// Token: 0x06001B7C RID: 7036 RVA: 0x000A180E File Offset: 0x0009FA0E
		public InvalidRunspacePoolStateException() : base(StringUtil.Format(RunspacePoolStrings.InvalidRunspacePoolStateGeneral, new object[0]))
		{
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x000A1826 File Offset: 0x0009FA26
		public InvalidRunspacePoolStateException(string message) : base(message)
		{
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x000A182F File Offset: 0x0009FA2F
		public InvalidRunspacePoolStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000A1839 File Offset: 0x0009FA39
		internal InvalidRunspacePoolStateException(string message, RunspacePoolState currentState, RunspacePoolState expectedState) : base(message)
		{
			this.expectedState = expectedState;
			this.currentState = currentState;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000A1850 File Offset: 0x0009FA50
		protected InvalidRunspacePoolStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001B81 RID: 7041 RVA: 0x000A185A File Offset: 0x0009FA5A
		public RunspacePoolState CurrentState
		{
			get
			{
				return this.currentState;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x000A1862 File Offset: 0x0009FA62
		public RunspacePoolState ExpectedState
		{
			get
			{
				return this.expectedState;
			}
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000A186C File Offset: 0x0009FA6C
		internal InvalidRunspaceStateException ToInvalidRunspaceStateException()
		{
			return new InvalidRunspaceStateException(RunspaceStrings.InvalidRunspaceStateGeneral, this)
			{
				CurrentState = InvalidRunspacePoolStateException.RunspacePoolStateToRunspaceState(this.CurrentState),
				ExpectedState = InvalidRunspacePoolStateException.RunspacePoolStateToRunspaceState(this.ExpectedState)
			};
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000A18A8 File Offset: 0x0009FAA8
		private static RunspaceState RunspacePoolStateToRunspaceState(RunspacePoolState state)
		{
			switch (state)
			{
			case RunspacePoolState.BeforeOpen:
				return RunspaceState.BeforeOpen;
			case RunspacePoolState.Opening:
				return RunspaceState.Opening;
			case RunspacePoolState.Opened:
				return RunspaceState.Opened;
			case RunspacePoolState.Closed:
				return RunspaceState.Closed;
			case RunspacePoolState.Closing:
				return RunspaceState.Closing;
			case RunspacePoolState.Broken:
				return RunspaceState.Broken;
			case RunspacePoolState.Disconnecting:
				return RunspaceState.Disconnecting;
			case RunspacePoolState.Disconnected:
				return RunspaceState.Disconnected;
			case RunspacePoolState.Connecting:
				return RunspaceState.Connecting;
			default:
				return RunspaceState.BeforeOpen;
			}
		}

		// Token: 0x04000B40 RID: 2880
		[NonSerialized]
		private RunspacePoolState currentState;

		// Token: 0x04000B41 RID: 2881
		[NonSerialized]
		private RunspacePoolState expectedState;
	}
}
