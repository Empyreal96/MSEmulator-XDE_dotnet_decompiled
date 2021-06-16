using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E8 RID: 744
	internal class RemoteDataObject : RemoteDataObject<object>
	{
		// Token: 0x06002382 RID: 9090 RVA: 0x000C785E File Offset: 0x000C5A5E
		private RemoteDataObject(RemotingDestination destination, RemotingDataType dataType, Guid runspacePoolId, Guid powerShellId, object data) : base(destination, dataType, runspacePoolId, powerShellId, data)
		{
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x000C786D File Offset: 0x000C5A6D
		internal new static RemoteDataObject CreateFrom(RemotingDestination destination, RemotingDataType dataType, Guid runspacePoolId, Guid powerShellId, object data)
		{
			return new RemoteDataObject(destination, dataType, runspacePoolId, powerShellId, data);
		}
	}
}
