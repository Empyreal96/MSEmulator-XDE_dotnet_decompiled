using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x0200023F RID: 575
	[Serializable]
	public class PSDataCollection<T> : IList<T>, ICollection<T>, IEnumerable<!0>, IList, ICollection, IEnumerable, IDisposable, ISerializable
	{
		// Token: 0x06001B08 RID: 6920 RVA: 0x0009FFBA File Offset: 0x0009E1BA
		public PSDataCollection() : this(new List<T>())
		{
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0009FFC7 File Offset: 0x0009E1C7
		public PSDataCollection(IEnumerable<T> items) : this(new List<T>(items))
		{
			this.Complete();
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0009FFDB File Offset: 0x0009E1DB
		public PSDataCollection(int capacity) : this(new List<T>(capacity))
		{
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0009FFE9 File Offset: 0x0009E1E9
		public static implicit operator PSDataCollection<T>(bool valueToConvert)
		{
			return PSDataCollection<T>.CreateAndInitializeFromExplicitValue(valueToConvert);
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0009FFF6 File Offset: 0x0009E1F6
		public static implicit operator PSDataCollection<T>(string valueToConvert)
		{
			return PSDataCollection<T>.CreateAndInitializeFromExplicitValue(valueToConvert);
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0009FFFE File Offset: 0x0009E1FE
		public static implicit operator PSDataCollection<T>(int valueToConvert)
		{
			return PSDataCollection<T>.CreateAndInitializeFromExplicitValue(valueToConvert);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000A000B File Offset: 0x0009E20B
		public static implicit operator PSDataCollection<T>(byte valueToConvert)
		{
			return PSDataCollection<T>.CreateAndInitializeFromExplicitValue(valueToConvert);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x000A0018 File Offset: 0x0009E218
		private static PSDataCollection<T> CreateAndInitializeFromExplicitValue(object valueToConvert)
		{
			PSDataCollection<T> psdataCollection = new PSDataCollection<T>();
			psdataCollection.Add(LanguagePrimitives.ConvertTo<T>(valueToConvert));
			psdataCollection.Complete();
			return psdataCollection;
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000A0040 File Offset: 0x0009E240
		public static implicit operator PSDataCollection<T>(Hashtable valueToConvert)
		{
			PSDataCollection<T> psdataCollection = new PSDataCollection<T>();
			psdataCollection.Add(LanguagePrimitives.ConvertTo<T>(valueToConvert));
			psdataCollection.Complete();
			return psdataCollection;
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000A0068 File Offset: 0x0009E268
		public static implicit operator PSDataCollection<T>(T valueToConvert)
		{
			PSDataCollection<T> psdataCollection = new PSDataCollection<T>();
			psdataCollection.Add(LanguagePrimitives.ConvertTo<T>(valueToConvert));
			psdataCollection.Complete();
			return psdataCollection;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x000A0094 File Offset: 0x0009E294
		public static implicit operator PSDataCollection<T>(object[] arrayToConvert)
		{
			PSDataCollection<T> psdataCollection = new PSDataCollection<T>();
			if (arrayToConvert != null)
			{
				for (int i = 0; i < arrayToConvert.Length; i++)
				{
					object valueToConvert = arrayToConvert[i];
					psdataCollection.Add(LanguagePrimitives.ConvertTo<T>(valueToConvert));
				}
			}
			psdataCollection.Complete();
			return psdataCollection;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000A00D1 File Offset: 0x0009E2D1
		internal PSDataCollection(IList<T> listToUse)
		{
			this.isOpen = true;
			this.syncObject = new object();
			this._dataAddedFrequency = 1;
			this._sourceGuid = Guid.Empty;
			base..ctor();
			this.data = listToUse;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x000A0104 File Offset: 0x0009E304
		protected PSDataCollection(SerializationInfo info, StreamingContext context)
		{
			this.isOpen = true;
			this.syncObject = new object();
			this._dataAddedFrequency = 1;
			this._sourceGuid = Guid.Empty;
			base..ctor();
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			IList<T> list = info.GetValue("Data", typeof(IList<T>)) as IList<T>;
			if (list == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			this.data = list;
			this._blockingEnumerator = info.GetBoolean("BlockingEnumerator");
			this._dataAddedFrequency = info.GetInt32("DataAddedCount");
			this.EnumeratorNeverBlocks = info.GetBoolean("EnumeratorNeverBlocks");
			this.isOpen = info.GetBoolean("IsOpen");
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06001B15 RID: 6933 RVA: 0x000A01C0 File Offset: 0x0009E3C0
		// (remove) Token: 0x06001B16 RID: 6934 RVA: 0x000A01F8 File Offset: 0x0009E3F8
		public event EventHandler<DataAddingEventArgs> DataAdding;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001B17 RID: 6935 RVA: 0x000A0230 File Offset: 0x0009E430
		// (remove) Token: 0x06001B18 RID: 6936 RVA: 0x000A0268 File Offset: 0x0009E468
		public event EventHandler<DataAddedEventArgs> DataAdded;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06001B19 RID: 6937 RVA: 0x000A02A0 File Offset: 0x0009E4A0
		// (remove) Token: 0x06001B1A RID: 6938 RVA: 0x000A02D8 File Offset: 0x0009E4D8
		public event EventHandler Completed;

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x000A0310 File Offset: 0x0009E510
		public bool IsOpen
		{
			get
			{
				bool result;
				lock (this.syncObject)
				{
					result = this.isOpen;
				}
				return result;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06001B1C RID: 6940 RVA: 0x000A0354 File Offset: 0x0009E554
		// (set) Token: 0x06001B1D RID: 6941 RVA: 0x000A035C File Offset: 0x0009E55C
		public int DataAddedCount
		{
			get
			{
				return this._dataAddedFrequency;
			}
			set
			{
				bool flag = false;
				lock (this.syncObject)
				{
					this._dataAddedFrequency = value;
					if (this._countNewData >= this._dataAddedFrequency)
					{
						flag = true;
						this._countNewData = 0;
					}
				}
				if (flag)
				{
					this.RaiseDataAddedEvent(this._lastPsInstanceId, this._lastIndex);
				}
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06001B1E RID: 6942 RVA: 0x000A03CC File Offset: 0x0009E5CC
		// (set) Token: 0x06001B1F RID: 6943 RVA: 0x000A03D4 File Offset: 0x0009E5D4
		public bool SerializeInput
		{
			get
			{
				return this.serializeInput;
			}
			set
			{
				if (typeof(T) != typeof(PSObject))
				{
					throw new NotSupportedException(PSDataBufferStrings.SerializationNotSupported);
				}
				this.serializeInput = value;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x000A0403 File Offset: 0x0009E603
		// (set) Token: 0x06001B21 RID: 6945 RVA: 0x000A040B File Offset: 0x0009E60B
		public bool IsAutoGenerated { get; set; }

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x000A0414 File Offset: 0x0009E614
		// (set) Token: 0x06001B23 RID: 6947 RVA: 0x000A0458 File Offset: 0x0009E658
		internal Guid SourceId
		{
			get
			{
				Guid sourceGuid;
				lock (this.syncObject)
				{
					sourceGuid = this._sourceGuid;
				}
				return sourceGuid;
			}
			set
			{
				lock (this.syncObject)
				{
					this._sourceGuid = value;
				}
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x000A049C File Offset: 0x0009E69C
		// (set) Token: 0x06001B25 RID: 6949 RVA: 0x000A04E0 File Offset: 0x0009E6E0
		internal bool ReleaseOnEnumeration
		{
			get
			{
				bool result;
				lock (this.syncObject)
				{
					result = this.releaseOnEnumeration;
				}
				return result;
			}
			set
			{
				lock (this.syncObject)
				{
					this.releaseOnEnumeration = value;
				}
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x000A0524 File Offset: 0x0009E724
		// (set) Token: 0x06001B27 RID: 6951 RVA: 0x000A0568 File Offset: 0x0009E768
		internal bool IsEnumerated
		{
			get
			{
				bool result;
				lock (this.syncObject)
				{
					result = this.isEnumerated;
				}
				return result;
			}
			set
			{
				lock (this.syncObject)
				{
					this.isEnumerated = value;
				}
			}
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x000A05AC File Offset: 0x0009E7AC
		public void Complete()
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				lock (this.syncObject)
				{
					if (this.isOpen)
					{
						this.isOpen = false;
						flag = true;
						Monitor.PulseAll(this.syncObject);
						if (this._countNewData > 0)
						{
							flag2 = true;
							this._countNewData = 0;
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					if (this.readWaitHandle != null)
					{
						this.readWaitHandle.Set();
					}
					EventHandler completed = this.Completed;
					if (completed != null)
					{
						completed(this, EventArgs.Empty);
					}
				}
				if (flag2)
				{
					this.RaiseDataAddedEvent(this._lastPsInstanceId, this._lastIndex);
				}
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x000A066C File Offset: 0x0009E86C
		// (set) Token: 0x06001B2A RID: 6954 RVA: 0x000A06B0 File Offset: 0x0009E8B0
		public bool BlockingEnumerator
		{
			get
			{
				bool blockingEnumerator;
				lock (this.syncObject)
				{
					blockingEnumerator = this._blockingEnumerator;
				}
				return blockingEnumerator;
			}
			set
			{
				lock (this.syncObject)
				{
					this._blockingEnumerator = value;
					if (this._blockingEnumerator)
					{
						if (!this._refCountIncrementedForBlockingEnumerator)
						{
							this._refCountIncrementedForBlockingEnumerator = true;
							this.AddRef();
						}
					}
					else if (this._refCountIncrementedForBlockingEnumerator)
					{
						this._refCountIncrementedForBlockingEnumerator = false;
						this.DecrementRef();
					}
				}
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x000A0728 File Offset: 0x0009E928
		// (set) Token: 0x06001B2C RID: 6956 RVA: 0x000A0730 File Offset: 0x0009E930
		public bool EnumeratorNeverBlocks { get; set; }

		// Token: 0x170006B4 RID: 1716
		public T this[int index]
		{
			get
			{
				T result;
				lock (this.syncObject)
				{
					result = this.data[index];
				}
				return result;
			}
			set
			{
				lock (this.syncObject)
				{
					if (index < 0 || index >= this.data.Count)
					{
						throw PSTraceSource.NewArgumentOutOfRangeException("index", index, PSDataBufferStrings.IndexOutOfRange, new object[]
						{
							0,
							this.data.Count - 1
						});
					}
					if (this.serializeInput)
					{
						value = (T)((object)this.GetSerializedObject(value));
					}
					this.data[index] = value;
				}
			}
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000A0834 File Offset: 0x0009EA34
		public int IndexOf(T item)
		{
			int result;
			lock (this.syncObject)
			{
				result = this.InternalIndexOf(item);
			}
			return result;
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000A0878 File Offset: 0x0009EA78
		public void Insert(int index, T item)
		{
			lock (this.syncObject)
			{
				this.InternalInsertItem(Guid.Empty, index, item);
			}
			this.RaiseEvents(Guid.Empty, index);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000A08CC File Offset: 0x0009EACC
		public void RemoveAt(int index)
		{
			lock (this.syncObject)
			{
				if (index < 0 || index >= this.data.Count)
				{
					throw PSTraceSource.NewArgumentOutOfRangeException("index", index, PSDataBufferStrings.IndexOutOfRange, new object[]
					{
						0,
						this.data.Count - 1
					});
				}
				this.RemoveItem(index);
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x000A095C File Offset: 0x0009EB5C
		public int Count
		{
			get
			{
				int result;
				lock (this.syncObject)
				{
					if (this.data == null)
					{
						result = 0;
					}
					else
					{
						result = this.data.Count;
					}
				}
				return result;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x000A09B0 File Offset: 0x0009EBB0
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000A09B3 File Offset: 0x0009EBB3
		public void Add(T item)
		{
			this.InternalAdd(Guid.Empty, item);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000A09C4 File Offset: 0x0009EBC4
		public void Clear()
		{
			lock (this.syncObject)
			{
				if (this.data != null)
				{
					this.data.Clear();
				}
			}
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000A0A14 File Offset: 0x0009EC14
		public bool Contains(T item)
		{
			bool result;
			lock (this.syncObject)
			{
				if (this.serializeInput)
				{
					item = (T)((object)this.GetSerializedObject(item));
				}
				result = this.data.Contains(item);
			}
			return result;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000A0A78 File Offset: 0x0009EC78
		public void CopyTo(T[] array, int arrayIndex)
		{
			lock (this.syncObject)
			{
				this.data.CopyTo(array, arrayIndex);
			}
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000A0AC0 File Offset: 0x0009ECC0
		public bool Remove(T item)
		{
			bool result;
			lock (this.syncObject)
			{
				int num = this.InternalIndexOf(item);
				if (num < 0)
				{
					result = false;
				}
				else
				{
					this.RemoveItem(num);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x000A0B14 File Offset: 0x0009ED14
		public IEnumerator<T> GetEnumerator()
		{
			return new PSDataCollectionEnumerator<T>(this, this.EnumeratorNeverBlocks);
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x000A0B24 File Offset: 0x0009ED24
		int IList.Add(object value)
		{
			PSDataCollection<T>.VerifyValueType(value);
			int count = this.data.Count;
			this.InternalAdd(Guid.Empty, (T)((object)value));
			this.RaiseEvents(Guid.Empty, count);
			return count;
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000A0B61 File Offset: 0x0009ED61
		bool IList.Contains(object value)
		{
			PSDataCollection<T>.VerifyValueType(value);
			return this.Contains((T)((object)value));
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000A0B75 File Offset: 0x0009ED75
		int IList.IndexOf(object value)
		{
			PSDataCollection<T>.VerifyValueType(value);
			return this.IndexOf((T)((object)value));
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x000A0B89 File Offset: 0x0009ED89
		void IList.Insert(int index, object value)
		{
			PSDataCollection<T>.VerifyValueType(value);
			this.Insert(index, (T)((object)value));
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x000A0B9E File Offset: 0x0009ED9E
		void IList.Remove(object value)
		{
			PSDataCollection<T>.VerifyValueType(value);
			this.Remove((T)((object)value));
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001B3F RID: 6975 RVA: 0x000A0BB3 File Offset: 0x0009EDB3
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001B40 RID: 6976 RVA: 0x000A0BB6 File Offset: 0x0009EDB6
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006B9 RID: 1721
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				PSDataCollection<T>.VerifyValueType(value);
				this[index] = (T)((object)value);
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x000A0BDC File Offset: 0x0009EDDC
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x000A0BDF File Offset: 0x0009EDDF
		object ICollection.SyncRoot
		{
			get
			{
				return this.syncObject;
			}
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x000A0BE8 File Offset: 0x0009EDE8
		void ICollection.CopyTo(Array array, int index)
		{
			lock (this.syncObject)
			{
				this.data.CopyTo((T[])array, index);
			}
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x000A0C34 File Offset: 0x0009EE34
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new PSDataCollectionEnumerator<T>(this, this.EnumeratorNeverBlocks);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000A0C42 File Offset: 0x0009EE42
		public Collection<T> ReadAll()
		{
			return this.ReadAndRemove(0);
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x000A0C4C File Offset: 0x0009EE4C
		internal Collection<T> ReadAndRemove(int readCount)
		{
			int num = (readCount > 0) ? readCount : int.MaxValue;
			Collection<T> result;
			lock (this.syncObject)
			{
				Collection<T> collection = new Collection<T>();
				int num2 = 0;
				while (num2 < num && this.data.Count > 0)
				{
					collection.Add(this.data[0]);
					this.data.RemoveAt(0);
					num2++;
				}
				if (this.readWaitHandle != null)
				{
					if (this.data.Count > 0 || !this.isOpen)
					{
						this.readWaitHandle.Set();
					}
					else
					{
						this.readWaitHandle.Reset();
					}
				}
				result = collection;
			}
			return result;
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x000A0D10 File Offset: 0x0009EF10
		internal T ReadAndRemoveAt0()
		{
			T result = default(T);
			lock (this.syncObject)
			{
				if (this.data != null && this.data.Count > 0)
				{
					result = this.data[0];
					this.data.RemoveAt(0);
				}
			}
			return result;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000A0D84 File Offset: 0x0009EF84
		protected virtual void InsertItem(Guid psInstanceId, int index, T item)
		{
			this.RaiseDataAddingEvent(psInstanceId, item);
			if (this.serializeInput)
			{
				item = (T)((object)this.GetSerializedObject(item));
			}
			this.data.Insert(index, item);
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000A0DBB File Offset: 0x0009EFBB
		protected virtual void RemoveItem(int index)
		{
			this.data.RemoveAt(index);
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000A0DCC File Offset: 0x0009EFCC
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			info.AddValue("Data", this.data);
			info.AddValue("BlockingEnumerator", this._blockingEnumerator);
			info.AddValue("DataAddedCount", this._dataAddedFrequency);
			info.AddValue("EnumeratorNeverBlocks", this.EnumeratorNeverBlocks);
			info.AddValue("IsOpen", this.isOpen);
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001B4D RID: 6989 RVA: 0x000A0E3C File Offset: 0x0009F03C
		internal WaitHandle WaitHandle
		{
			get
			{
				if (this.readWaitHandle == null)
				{
					lock (this.syncObject)
					{
						if (this.readWaitHandle == null)
						{
							this.readWaitHandle = new ManualResetEvent(this.data.Count > 0 || !this.isOpen);
						}
					}
				}
				return this.readWaitHandle;
			}
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000A0EB4 File Offset: 0x0009F0B4
		private void RaiseEvents(Guid psInstanceId, int index)
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.readWaitHandle != null)
				{
					if (this.data.Count > 0 || !this.isOpen)
					{
						this.readWaitHandle.Set();
					}
					else
					{
						this.readWaitHandle.Reset();
					}
				}
				Monitor.PulseAll(this.syncObject);
				this._countNewData++;
				if (this._countNewData >= this._dataAddedFrequency || (this._countNewData > 0 && !this.isOpen))
				{
					flag = true;
					this._countNewData = 0;
				}
				else
				{
					this._lastPsInstanceId = psInstanceId;
					this._lastIndex = index;
				}
			}
			if (flag)
			{
				this.RaiseDataAddedEvent(psInstanceId, index);
			}
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x000A0F84 File Offset: 0x0009F184
		private void RaiseDataAddingEvent(Guid psInstanceId, object itemAdded)
		{
			EventHandler<DataAddingEventArgs> dataAdding = this.DataAdding;
			if (dataAdding != null)
			{
				dataAdding(this, new DataAddingEventArgs(psInstanceId, itemAdded));
			}
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x000A0FAC File Offset: 0x0009F1AC
		private void RaiseDataAddedEvent(Guid psInstanceId, int index)
		{
			EventHandler<DataAddedEventArgs> dataAdded = this.DataAdded;
			if (dataAdded != null)
			{
				dataAdded(this, new DataAddedEventArgs(psInstanceId, index));
			}
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000A0FD1 File Offset: 0x0009F1D1
		private void InternalInsertItem(Guid psInstanceId, int index, T item)
		{
			if (!this.isOpen)
			{
				throw PSTraceSource.NewInvalidOperationException(PSDataBufferStrings.WriteToClosedBuffer, new object[0]);
			}
			this.InsertItem(psInstanceId, index, item);
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x000A0FF8 File Offset: 0x0009F1F8
		internal void InternalAdd(Guid psInstanceId, T item)
		{
			int num = -1;
			lock (this.syncObject)
			{
				num = this.data.Count;
				this.InternalInsertItem(psInstanceId, num, item);
			}
			if (num > -1)
			{
				this.RaiseEvents(psInstanceId, num);
			}
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x000A1058 File Offset: 0x0009F258
		internal void InternalAddRange(Guid psInstanceId, ICollection collection)
		{
			if (collection == null)
			{
				throw PSTraceSource.NewArgumentNullException("collection");
			}
			int index = -1;
			bool flag = false;
			lock (this.syncObject)
			{
				if (!this.isOpen)
				{
					throw PSTraceSource.NewInvalidOperationException(PSDataBufferStrings.WriteToClosedBuffer, new object[0]);
				}
				index = this.data.Count;
				foreach (object obj2 in collection)
				{
					this.InsertItem(psInstanceId, this.data.Count, (T)((object)obj2));
					flag = true;
				}
			}
			if (flag)
			{
				this.RaiseEvents(psInstanceId, index);
			}
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x000A1130 File Offset: 0x0009F330
		internal void AddRef()
		{
			lock (this.syncObject)
			{
				this.refCount++;
			}
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000A1178 File Offset: 0x0009F378
		internal void DecrementRef()
		{
			lock (this.syncObject)
			{
				this.refCount--;
				if (this.refCount == 0 || (this._blockingEnumerator && this.refCount == 1))
				{
					if (this.readWaitHandle != null)
					{
						this.readWaitHandle.Set();
					}
					Monitor.PulseAll(this.syncObject);
				}
			}
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x000A11FC File Offset: 0x0009F3FC
		private int InternalIndexOf(T item)
		{
			if (this.serializeInput)
			{
				item = (T)((object)this.GetSerializedObject(item));
			}
			int count = this.data.Count;
			for (int i = 0; i < count; i++)
			{
				if (object.Equals(this.data[i], item))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x000A1260 File Offset: 0x0009F460
		private static void VerifyValueType(object value)
		{
			if (value == null)
			{
				if (typeof(T).GetTypeInfo().IsValueType)
				{
					throw PSTraceSource.NewArgumentNullException("value", PSDataBufferStrings.ValueNullReference, new object[0]);
				}
			}
			else if (!(value is T))
			{
				throw PSTraceSource.NewArgumentException("value", PSDataBufferStrings.CannotConvertToGenericType, new object[]
				{
					value.GetType().FullName,
					typeof(T).FullName
				});
			}
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000A12DC File Offset: 0x0009F4DC
		private PSObject GetSerializedObject(object value)
		{
			PSObject result = value as PSObject;
			if (this.SerializationWouldHaveNoEffect(result))
			{
				return result;
			}
			object obj = PSSerializer.Deserialize(PSSerializer.Serialize(value));
			if (obj == null)
			{
				return (PSObject)obj;
			}
			return PSObject.AsPSObject(obj);
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000A1318 File Offset: 0x0009F518
		private bool SerializationWouldHaveNoEffect(PSObject result)
		{
			if (result == null)
			{
				return true;
			}
			object obj = PSObject.Base(result);
			return obj == null || InternalSerializer.IsPrimitiveKnownType(obj.GetType()) || obj is CimInstance || result.TypeNames[0].StartsWith("Deserialized", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001B5A RID: 7002 RVA: 0x000A136B File Offset: 0x0009F56B
		internal object SyncObject
		{
			get
			{
				return this.syncObject;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001B5B RID: 7003 RVA: 0x000A1373 File Offset: 0x0009F573
		// (set) Token: 0x06001B5C RID: 7004 RVA: 0x000A137C File Offset: 0x0009F57C
		internal int RefCount
		{
			get
			{
				return this.refCount;
			}
			set
			{
				lock (this.syncObject)
				{
					this.refCount = value;
				}
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001B5D RID: 7005 RVA: 0x000A13C0 File Offset: 0x0009F5C0
		internal bool PulseIdleEvent
		{
			get
			{
				return this.IdleEvent != null;
			}
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06001B5E RID: 7006 RVA: 0x000A13D0 File Offset: 0x0009F5D0
		// (remove) Token: 0x06001B5F RID: 7007 RVA: 0x000A1408 File Offset: 0x0009F608
		internal event EventHandler<EventArgs> IdleEvent;

		// Token: 0x06001B60 RID: 7008 RVA: 0x000A143D File Offset: 0x0009F63D
		internal void FireIdleEvent()
		{
			this.IdleEvent.SafeInvoke(this, null);
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000A144C File Offset: 0x0009F64C
		internal void Pulse()
		{
			lock (this.syncObject)
			{
				Monitor.PulseAll(this.syncObject);
			}
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000A1494 File Offset: 0x0009F694
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000A14A4 File Offset: 0x0009F6A4
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.isDisposed)
				{
					return;
				}
				lock (this.syncObject)
				{
					if (this.isDisposed)
					{
						return;
					}
					this.isDisposed = true;
				}
				this.Complete();
				lock (this.syncObject)
				{
					if (this.readWaitHandle != null)
					{
						this.readWaitHandle.Dispose();
						this.readWaitHandle = null;
					}
					if (this.data != null)
					{
						this.data.Clear();
					}
				}
			}
		}

		// Token: 0x04000B20 RID: 2848
		private IList<T> data;

		// Token: 0x04000B21 RID: 2849
		private ManualResetEvent readWaitHandle;

		// Token: 0x04000B22 RID: 2850
		private bool isOpen;

		// Token: 0x04000B23 RID: 2851
		private bool releaseOnEnumeration;

		// Token: 0x04000B24 RID: 2852
		private bool isEnumerated;

		// Token: 0x04000B25 RID: 2853
		private int refCount;

		// Token: 0x04000B26 RID: 2854
		private object syncObject;

		// Token: 0x04000B27 RID: 2855
		private bool isDisposed;

		// Token: 0x04000B28 RID: 2856
		private bool _blockingEnumerator;

		// Token: 0x04000B29 RID: 2857
		private bool _refCountIncrementedForBlockingEnumerator;

		// Token: 0x04000B2A RID: 2858
		private int _countNewData;

		// Token: 0x04000B2B RID: 2859
		private int _dataAddedFrequency;

		// Token: 0x04000B2C RID: 2860
		private Guid _sourceGuid;

		// Token: 0x04000B30 RID: 2864
		private bool serializeInput;

		// Token: 0x04000B31 RID: 2865
		private Guid _lastPsInstanceId;

		// Token: 0x04000B32 RID: 2866
		private int _lastIndex;
	}
}
