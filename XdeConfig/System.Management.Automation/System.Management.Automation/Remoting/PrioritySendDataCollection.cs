using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000389 RID: 905
	internal class PrioritySendDataCollection
	{
		// Token: 0x06002BE7 RID: 11239 RVA: 0x000F2BEC File Offset: 0x000F0DEC
		internal PrioritySendDataCollection()
		{
			this.onSendCollectionDataAvailable = new SerializedDataStream.OnDataAvailableCallback(this.OnDataAvailable);
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x000F2C11 File Offset: 0x000F0E11
		// (set) Token: 0x06002BE9 RID: 11241 RVA: 0x000F2C1C File Offset: 0x000F0E1C
		internal Fragmentor Fragmentor
		{
			get
			{
				return this.fragmentor;
			}
			set
			{
				this.fragmentor = value;
				string[] names = Enum.GetNames(typeof(DataPriorityType));
				this.dataToBeSent = new SerializedDataStream[names.Length];
				this.syncObjects = new object[names.Length];
				for (int i = 0; i < names.Length; i++)
				{
					this.dataToBeSent[i] = new SerializedDataStream(this.fragmentor.FragmentSize);
					this.syncObjects[i] = new object();
				}
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000F2C90 File Offset: 0x000F0E90
		internal void Add<T>(RemoteDataObject<T> data, DataPriorityType priority)
		{
			lock (this.syncObjects[(int)priority])
			{
				this.fragmentor.Fragment<T>(data, this.dataToBeSent[(int)priority]);
			}
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000F2CE0 File Offset: 0x000F0EE0
		internal void Add<T>(RemoteDataObject<T> data)
		{
			this.Add<T>(data, DataPriorityType.Default);
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000F2CEC File Offset: 0x000F0EEC
		internal void Clear()
		{
			lock (this.syncObjects[1])
			{
				this.dataToBeSent[1].Dispose();
			}
			lock (this.syncObjects[0])
			{
				this.dataToBeSent[0].Dispose();
			}
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000F2D70 File Offset: 0x000F0F70
		internal byte[] ReadOrRegisterCallback(PrioritySendDataCollection.OnDataAvailableCallback callback, out DataPriorityType priorityType)
		{
			byte[] result;
			lock (this.readSyncObject)
			{
				priorityType = DataPriorityType.Default;
				byte[] array = this.dataToBeSent[1].ReadOrRegisterCallback(this.onSendCollectionDataAvailable);
				priorityType = DataPriorityType.PromptResponse;
				if (array == null)
				{
					array = this.dataToBeSent[0].ReadOrRegisterCallback(this.onSendCollectionDataAvailable);
					priorityType = DataPriorityType.Default;
				}
				if (array == null)
				{
					this.onDataAvailableCallback = callback;
				}
				result = array;
			}
			return result;
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000F2DF0 File Offset: 0x000F0FF0
		private void OnDataAvailable(byte[] data, bool isEndFragment)
		{
			lock (this.readSyncObject)
			{
				if (this.isHandlingCallback)
				{
					return;
				}
				this.isHandlingCallback = true;
			}
			if (this.onDataAvailableCallback != null)
			{
				DataPriorityType priorityType;
				byte[] array = this.ReadOrRegisterCallback(this.onDataAvailableCallback, out priorityType);
				if (array != null)
				{
					PrioritySendDataCollection.OnDataAvailableCallback onDataAvailableCallback = this.onDataAvailableCallback;
					this.onDataAvailableCallback = null;
					onDataAvailableCallback(array, priorityType);
				}
			}
			this.isHandlingCallback = false;
		}

		// Token: 0x0400160B RID: 5643
		private SerializedDataStream[] dataToBeSent;

		// Token: 0x0400160C RID: 5644
		private Fragmentor fragmentor;

		// Token: 0x0400160D RID: 5645
		private object[] syncObjects;

		// Token: 0x0400160E RID: 5646
		private PrioritySendDataCollection.OnDataAvailableCallback onDataAvailableCallback;

		// Token: 0x0400160F RID: 5647
		private SerializedDataStream.OnDataAvailableCallback onSendCollectionDataAvailable;

		// Token: 0x04001610 RID: 5648
		private bool isHandlingCallback;

		// Token: 0x04001611 RID: 5649
		private object readSyncObject = new object();

		// Token: 0x0200038A RID: 906
		// (Invoke) Token: 0x06002BF0 RID: 11248
		internal delegate void OnDataAvailableCallback(byte[] data, DataPriorityType priorityType);
	}
}
