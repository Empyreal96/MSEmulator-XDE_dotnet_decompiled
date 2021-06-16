using System;
using System.Globalization;
using System.IO;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E7 RID: 743
	internal class RemoteDataObject<T>
	{
		// Token: 0x06002373 RID: 9075 RVA: 0x000C75A7 File Offset: 0x000C57A7
		protected RemoteDataObject(RemotingDestination destination, RemotingDataType dataType, Guid runspacePoolId, Guid powerShellId, T data)
		{
			this.destination = destination;
			this.dataType = dataType;
			this.runspacePoolId = runspacePoolId;
			this.powerShellId = powerShellId;
			this.data = data;
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002374 RID: 9076 RVA: 0x000C75D4 File Offset: 0x000C57D4
		internal RemotingDestination Destination
		{
			get
			{
				return this.destination;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002375 RID: 9077 RVA: 0x000C75DC File Offset: 0x000C57DC
		internal RemotingTargetInterface TargetInterface
		{
			get
			{
				int num = (int)this.dataType;
				if ((num & 266240) == 266240)
				{
					return RemotingTargetInterface.PowerShell;
				}
				if ((num & 135168) == 135168)
				{
					return RemotingTargetInterface.RunspacePool;
				}
				if ((num & 65536) == 65536)
				{
					return RemotingTargetInterface.Session;
				}
				return RemotingTargetInterface.InvalidTargetInterface;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002376 RID: 9078 RVA: 0x000C7621 File Offset: 0x000C5821
		internal RemotingDataType DataType
		{
			get
			{
				return this.dataType;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002377 RID: 9079 RVA: 0x000C7629 File Offset: 0x000C5829
		internal Guid RunspacePoolId
		{
			get
			{
				return this.runspacePoolId;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002378 RID: 9080 RVA: 0x000C7631 File Offset: 0x000C5831
		internal Guid PowerShellId
		{
			get
			{
				return this.powerShellId;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002379 RID: 9081 RVA: 0x000C7639 File Offset: 0x000C5839
		internal T Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000C7641 File Offset: 0x000C5841
		internal static RemoteDataObject<T> CreateFrom(RemotingDestination destination, RemotingDataType dataType, Guid runspacePoolId, Guid powerShellId, T data)
		{
			return new RemoteDataObject<T>(destination, dataType, runspacePoolId, powerShellId, data);
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000C7650 File Offset: 0x000C5850
		internal static RemoteDataObject<T> CreateFrom(Stream serializedDataStream, Fragmentor defragmentor)
		{
			if (serializedDataStream.Length - serializedDataStream.Position < 40L)
			{
				PSRemotingTransportException ex = new PSRemotingTransportException(PSRemotingErrorId.NotEnoughHeaderForRemoteDataObject, RemotingErrorIdStrings.NotEnoughHeaderForRemoteDataObject, new object[]
				{
					61
				});
				throw ex;
			}
			RemotingDestination remotingDestination = (RemotingDestination)RemoteDataObject<T>.DeserializeUInt(serializedDataStream);
			RemotingDataType remotingDataType = (RemotingDataType)RemoteDataObject<T>.DeserializeUInt(serializedDataStream);
			Guid guid = RemoteDataObject<T>.DeserializeGuid(serializedDataStream);
			Guid guid2 = RemoteDataObject<T>.DeserializeGuid(serializedDataStream);
			object valueToConvert = null;
			if (serializedDataStream.Length - 40L > 0L)
			{
				valueToConvert = defragmentor.DeserializeToPSObject(serializedDataStream);
			}
			T t = (T)((object)LanguagePrimitives.ConvertTo(valueToConvert, typeof(T), CultureInfo.CurrentCulture));
			return new RemoteDataObject<T>(remotingDestination, remotingDataType, guid, guid2, t);
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x000C76F6 File Offset: 0x000C58F6
		internal virtual void Serialize(Stream streamToWriteTo, Fragmentor fragmentor)
		{
			this.SerializeHeader(streamToWriteTo);
			if (this.data != null)
			{
				fragmentor.SerializeToBytes(this.data, streamToWriteTo);
			}
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000C771E File Offset: 0x000C591E
		private void SerializeHeader(Stream streamToWriteTo)
		{
			this.SerializeUInt((uint)this.Destination, streamToWriteTo);
			this.SerializeUInt((uint)this.DataType, streamToWriteTo);
			this.SerializeGuid(this.runspacePoolId, streamToWriteTo);
			this.SerializeGuid(this.powerShellId, streamToWriteTo);
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x000C7754 File Offset: 0x000C5954
		private void SerializeUInt(uint data, Stream streamToWriteTo)
		{
			byte[] array = new byte[4];
			int num = 0;
			array[num++] = (byte)(data & 255U);
			array[num++] = (byte)(data >> 8 & 255U);
			array[num++] = (byte)(data >> 16 & 255U);
			array[num++] = (byte)(data >> 24 & 255U);
			streamToWriteTo.Write(array, 0, 4);
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x000C77B8 File Offset: 0x000C59B8
		private static uint DeserializeUInt(Stream serializedDataStream)
		{
			uint num = 0U;
			num |= (uint)(serializedDataStream.ReadByte() & 255);
			num |= (uint)(serializedDataStream.ReadByte() << 8 & 65280);
			num |= (uint)(serializedDataStream.ReadByte() << 16 & 16711680);
			return num | (uint)(serializedDataStream.ReadByte() << 24 & -16777216);
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x000C780C File Offset: 0x000C5A0C
		private void SerializeGuid(Guid guid, Stream streamToWriteTo)
		{
			byte[] array = guid.ToByteArray();
			streamToWriteTo.Write(array, 0, array.Length);
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000C782C File Offset: 0x000C5A2C
		private static Guid DeserializeGuid(Stream serializedDataStream)
		{
			byte[] array = new byte[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = (byte)serializedDataStream.ReadByte();
			}
			return new Guid(array);
		}

		// Token: 0x0400113C RID: 4412
		private const int destinationOffset = 0;

		// Token: 0x0400113D RID: 4413
		private const int dataTypeOffset = 4;

		// Token: 0x0400113E RID: 4414
		private const int rsPoolIdOffset = 8;

		// Token: 0x0400113F RID: 4415
		private const int psIdOffset = 24;

		// Token: 0x04001140 RID: 4416
		private const int headerLength = 40;

		// Token: 0x04001141 RID: 4417
		private const int SessionMask = 65536;

		// Token: 0x04001142 RID: 4418
		private const int RunspacePoolMask = 135168;

		// Token: 0x04001143 RID: 4419
		private const int PowerShellMask = 266240;

		// Token: 0x04001144 RID: 4420
		private RemotingDestination destination;

		// Token: 0x04001145 RID: 4421
		private RemotingDataType dataType;

		// Token: 0x04001146 RID: 4422
		private Guid runspacePoolId;

		// Token: 0x04001147 RID: 4423
		private Guid powerShellId;

		// Token: 0x04001148 RID: 4424
		private T data;
	}
}
