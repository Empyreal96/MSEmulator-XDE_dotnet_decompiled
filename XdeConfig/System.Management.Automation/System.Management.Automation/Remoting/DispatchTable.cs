using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002D5 RID: 725
	internal class DispatchTable<T> where T : class
	{
		// Token: 0x060022B9 RID: 8889 RVA: 0x000C38F4 File Offset: 0x000C1AF4
		internal long CreateNewCallId()
		{
			long num = Interlocked.Increment(ref this._nextCallId);
			AsyncObject<T> value = new AsyncObject<T>();
			lock (this._responseAsyncObjects)
			{
				this._responseAsyncObjects[num] = value;
			}
			return num;
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000C3950 File Offset: 0x000C1B50
		private AsyncObject<T> GetResponseAsyncObject(long callId)
		{
			return this._responseAsyncObjects[callId];
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000C3970 File Offset: 0x000C1B70
		internal T GetResponse(long callId, T defaultValue)
		{
			AsyncObject<T> asyncObject = null;
			lock (this._responseAsyncObjects)
			{
				asyncObject = this.GetResponseAsyncObject(callId);
			}
			T value = asyncObject.Value;
			lock (this._responseAsyncObjects)
			{
				this._responseAsyncObjects.Remove(callId);
			}
			if (value == null)
			{
				return defaultValue;
			}
			return value;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000C3A00 File Offset: 0x000C1C00
		internal void SetResponse(long callId, T remoteHostResponse)
		{
			lock (this._responseAsyncObjects)
			{
				if (this._responseAsyncObjects.ContainsKey(callId))
				{
					AsyncObject<T> responseAsyncObject = this.GetResponseAsyncObject(callId);
					responseAsyncObject.Value = remoteHostResponse;
				}
			}
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000C3A5C File Offset: 0x000C1C5C
		private void AbortCall(long callId)
		{
			if (!this._responseAsyncObjects.ContainsKey(callId))
			{
				return;
			}
			AsyncObject<T> responseAsyncObject = this.GetResponseAsyncObject(callId);
			responseAsyncObject.Value = default(T);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000C3A90 File Offset: 0x000C1C90
		private void AbortCalls(List<long> callIds)
		{
			foreach (long callId in callIds)
			{
				this.AbortCall(callId);
			}
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000C3AE0 File Offset: 0x000C1CE0
		private List<long> GetAllCalls()
		{
			List<long> list = new List<long>();
			foreach (KeyValuePair<long, AsyncObject<T>> keyValuePair in this._responseAsyncObjects)
			{
				list.Add(keyValuePair.Key);
			}
			return list;
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000C3B40 File Offset: 0x000C1D40
		internal void AbortAllCalls()
		{
			lock (this._responseAsyncObjects)
			{
				List<long> allCalls = this.GetAllCalls();
				this.AbortCalls(allCalls);
			}
		}

		// Token: 0x04001083 RID: 4227
		internal const long VoidCallId = -100L;

		// Token: 0x04001084 RID: 4228
		private Dictionary<long, AsyncObject<T>> _responseAsyncObjects = new Dictionary<long, AsyncObject<T>>();

		// Token: 0x04001085 RID: 4229
		private long _nextCallId;
	}
}
