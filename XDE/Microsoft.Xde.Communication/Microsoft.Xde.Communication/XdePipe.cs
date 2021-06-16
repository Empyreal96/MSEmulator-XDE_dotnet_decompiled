using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xde.Common;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Communication
{
	// Token: 0x0200000B RID: 11
	public class XdePipe : IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00003143 File Offset: 0x00001343
		public XdePipe(IXdeConnectionAddressInfo addressInfo, Guid pipeGuid) : this(addressInfo, pipeGuid, null)
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003150 File Offset: 0x00001350
		public XdePipe(IXdeConnectionAddressInfo addressInfo, Guid pipeGuid, string name)
		{
			if (name == null)
			{
				name = base.GetType().Name;
			}
			this.addressInfo = addressInfo;
			this.PipeGuid = pipeGuid;
			this.Name = name;
			this.cancelAsyncIOWait = new ManualResetEvent(false);
			this.IsConnected = false;
			this.cultureInfo = Thread.CurrentThread.CurrentUICulture;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000071 RID: 113 RVA: 0x000031AC File Offset: 0x000013AC
		// (remove) Token: 0x06000072 RID: 114 RVA: 0x000031E4 File Offset: 0x000013E4
		public event EventHandler ConnectionSucceeded;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000073 RID: 115 RVA: 0x0000321C File Offset: 0x0000141C
		// (remove) Token: 0x06000074 RID: 116 RVA: 0x00003254 File Offset: 0x00001454
		public event EventHandler<ExEventArgs> ConnectionFailed;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000075 RID: 117 RVA: 0x0000328C File Offset: 0x0000148C
		// (remove) Token: 0x06000076 RID: 118 RVA: 0x000032C4 File Offset: 0x000014C4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000032F9 File Offset: 0x000014F9
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003301 File Offset: 0x00001501
		public Guid PipeGuid { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000079 RID: 121 RVA: 0x0000330A File Offset: 0x0000150A
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003312 File Offset: 0x00001512
		public string Name { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000331B File Offset: 0x0000151B
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003323 File Offset: 0x00001523
		public bool IsConnected
		{
			get
			{
				return this.connected;
			}
			private set
			{
				if (this.connected != value)
				{
					this.connected = value;
					this.OnPropertyChanged("IsConnected");
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003340 File Offset: 0x00001540
		public static byte[] GetStructBytes(object obj)
		{
			byte[] array = new byte[Marshal.SizeOf(obj)];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				Marshal.StructureToPtr(obj, gchandle.AddrOfPinnedObject(), false);
			}
			finally
			{
				gchandle.Free();
			}
			return array;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000338C File Offset: 0x0000158C
		public void SetMinimumReceiveBufferSize(int bufferSize)
		{
			if (this.hostSocket.ReceiveBufferSize < bufferSize)
			{
				this.hostSocket.ReceiveBufferSize = bufferSize;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000033A8 File Offset: 0x000015A8
		public void InitiateConnection()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("XdePipe");
			}
			this.connectionThread = new Thread(new ThreadStart(this.ConnectSocket));
			this.connectionThread.CurrentUICulture = this.cultureInfo;
			this.connectionThread.Start();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000033FB File Offset: 0x000015FB
		public void DisconnectFromGuest()
		{
			this.IsConnected = false;
			this.cancelAsyncIOWait.Set();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003410 File Offset: 0x00001610
		public void SendToGuest(byte[] data)
		{
			this.SendToGuest(data, data.Length);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000341C File Offset: 0x0000161C
		public void SendToGuest(byte[] data, int size)
		{
			this.CheckForConnection();
			this.SendToGuestPrivate(data, size);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000342C File Offset: 0x0000162C
		public void SendToGuest(int[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				this.SendToGuest(data[i]);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003450 File Offset: 0x00001650
		public void SendToGuest(int data)
		{
			byte[] bytes = BitConverter.GetBytes(data);
			this.SendToGuest(bytes);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000346C File Offset: 0x0000166C
		public void SendToGuest(string stringValue)
		{
			this.SendToGuest(stringValue.Length);
			for (int i = 0; i < stringValue.Length; i++)
			{
				byte[] bytes = BitConverter.GetBytes(stringValue[i]);
				this.SendToGuest(bytes);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000034AC File Offset: 0x000016AC
		public void SendToGuest(Guid guidValue)
		{
			byte[] data = guidValue.ToByteArray();
			this.SendToGuest(data);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000034C8 File Offset: 0x000016C8
		public void SendStructToGuest(object obj)
		{
			this.SendToGuest(XdePipe.GetStructBytes(obj));
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000034D8 File Offset: 0x000016D8
		public void SendToGuest(float floatData)
		{
			byte[] bytes = BitConverter.GetBytes(floatData);
			this.SendToGuest(bytes);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000034F3 File Offset: 0x000016F3
		public void ReceiveFromGuest(byte[] data, int offset, int receiveSize)
		{
			this.CheckForConnection();
			this.ReceiveWithSizePrivate(data, offset, receiveSize);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003504 File Offset: 0x00001704
		public void ReceiveFromGuest(byte[] data, int receiveSize)
		{
			this.ReceiveFromGuest(data, 0, receiveSize);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000350F File Offset: 0x0000170F
		public void ReceiveFromGuest(byte[] data)
		{
			this.ReceiveFromGuest(data, data.Length);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000351C File Offset: 0x0000171C
		public int ReceiveFromGuestWithSpin(byte[] data, int minimumSize, int maximumSize, int spinMilliseconds)
		{
			int num = 0;
			for (int i = 0; i < spinMilliseconds; i++)
			{
				num = this.hostSocket.Available;
				if (num >= minimumSize || !this.IsConnected)
				{
					break;
				}
				Thread.Sleep(1);
			}
			int num2;
			if (num > maximumSize)
			{
				num2 = maximumSize;
			}
			else if (num < minimumSize)
			{
				num2 = minimumSize;
			}
			else
			{
				num2 = num;
				num2 &= -4;
			}
			this.ReceiveFromGuest(data, num2);
			return num2;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003578 File Offset: 0x00001778
		public void ReceiveFromGuest(int[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = this.ReceiveIntFromGuest();
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000359C File Offset: 0x0000179C
		public int ReceiveIntFromGuest()
		{
			byte[] array = new byte[4];
			this.ReceiveFromGuest(array);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000035C0 File Offset: 0x000017C0
		public Guid ReceiveGuidFromGuest()
		{
			byte[] array = new byte[16];
			this.ReceiveFromGuest(array);
			return new Guid(array);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000035E4 File Offset: 0x000017E4
		public T ReceiveStructFromGuest<T>()
		{
			byte[] array = new byte[Marshal.SizeOf(typeof(T))];
			this.ReceiveFromGuest(array);
			return StructUtils.GetStructFromBytes<T>(array);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003613 File Offset: 0x00001813
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003622 File Offset: 0x00001822
		protected void ThrowXdePipeException(XdePipeError error)
		{
			throw XdePipeException.GetExceptionFromError(this.Name, error);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003630 File Offset: 0x00001830
		protected void ThrowXdePipeException(string message)
		{
			throw new XdePipeException(this.Name, message);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000363E File Offset: 0x0000183E
		protected void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000365C File Offset: 0x0000185C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.disposed)
				{
					return;
				}
				this.disposed = true;
				this.IsConnected = false;
				if (this.cancelAsyncIOWait != null)
				{
					this.cancelAsyncIOWait.Set();
				}
				if (this.hostSocket != null)
				{
					if (this.hostSocket.Connected)
					{
						try
						{
							this.hostSocket.Shutdown(SocketShutdown.Both);
						}
						catch (SocketException)
						{
						}
					}
					this.hostSocket.Close();
					this.hostSocket.Dispose();
				}
				if (this.recvAsyncEventArgs != null)
				{
					this.recvAsyncEventArgs.Completed -= this.RecvCompletedCallback;
					this.recvAsyncEventArgs.Dispose();
				}
				if (this.sendAsyncEventArgs != null)
				{
					this.sendAsyncEventArgs.Completed -= this.SendCompletedCallback;
					this.sendAsyncEventArgs.Dispose();
				}
				if (this.recvCompleted != null)
				{
					this.recvCompleted.Dispose();
				}
				if (this.sendCompleted != null)
				{
					this.sendCompleted.Dispose();
				}
				if (this.cancelAsyncIOWait != null)
				{
					this.cancelAsyncIOWait.Dispose();
				}
				if (this.connectionThread != null)
				{
					this.connectionThread.Join();
				}
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003788 File Offset: 0x00001988
		private void GenerateConnectionErrorEncountered(Exception exception)
		{
			if (!Logger.Instance.IsXdeShuttingDown)
			{
				string message = Logger.Instance.ReplaceSensitiveStrings(exception.Message);
				string innerMessage = Logger.Instance.ReplaceSensitiveStrings((exception.InnerException == null) ? null : exception.InnerException.Message);
				Logger.Instance.LogError("PipeConnectionFailed", new
				{
					PartA_iKey = "A-MSTelDefault",
					pipe = this.Name,
					message = message,
					innerMessage = innerMessage
				});
			}
			EventHandler<ExEventArgs> connectionFailed = this.ConnectionFailed;
			if (connectionFailed == null)
			{
				return;
			}
			connectionFailed(this, new ExEventArgs(exception));
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000380B File Offset: 0x00001A0B
		private void RecvCompletedCallback(object sender, SocketAsyncEventArgs e)
		{
			this.recvCompleted.Set();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003819 File Offset: 0x00001A19
		private void SendCompletedCallback(object sender, SocketAsyncEventArgs e)
		{
			this.sendCompleted.Set();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003828 File Offset: 0x00001A28
		private void SendToGuestPrivate(byte[] data, int size)
		{
			int num = 0;
			do
			{
				if (this.disposed)
				{
					this.ThrowXdePipeException(PipeExceptions.PipeShutdown);
				}
				try
				{
					this.sendAsyncEventArgs.SetBuffer(data, num, size - num);
					if (this.hostSocket.SendAsync(this.sendAsyncEventArgs))
					{
						WaitHandle.WaitAny(this.sendAsyncWaitEvents);
					}
					if (this.cancelAsyncIOWait.WaitOne(0))
					{
						this.ThrowXdePipeException(PipeExceptions.PipeShutdown);
					}
					int bytesTransferred = this.sendAsyncEventArgs.BytesTransferred;
					if (bytesTransferred == 0)
					{
						this.DisconnectFromGuest();
						this.ThrowXdePipeException(XdePipeError.ConnectionClosedByServer);
					}
					num += bytesTransferred;
				}
				catch (ObjectDisposedException)
				{
					this.ThrowXdePipeException(PipeExceptions.PipeShutdown);
				}
				catch (XdePipeException)
				{
					throw;
				}
				catch (Exception ex)
				{
					this.ThrowXdePipeException(ex.Message);
				}
			}
			while (num != size);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000390C File Offset: 0x00001B0C
		private void SendToGuestPrivate(Guid guidValue)
		{
			byte[] array = guidValue.ToByteArray();
			this.SendToGuestPrivate(array, array.Length);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000392C File Offset: 0x00001B2C
		private void ReceiveWithSizePrivate(byte[] data, int offset, int receiveSize)
		{
			int num = 0;
			do
			{
				if (this.disposed)
				{
					this.ThrowXdePipeException(PipeExceptions.PipeShutdown);
				}
				try
				{
					this.recvAsyncEventArgs.SetBuffer(data, offset + num, receiveSize - num);
					if (this.hostSocket.ReceiveAsync(this.recvAsyncEventArgs))
					{
						WaitHandle.WaitAny(this.recvAsyncWaitEvents);
					}
					if (this.cancelAsyncIOWait.WaitOne(0))
					{
						this.ThrowXdePipeException(PipeExceptions.PipeShutdown);
					}
					int bytesTransferred = this.recvAsyncEventArgs.BytesTransferred;
					if (bytesTransferred == 0)
					{
						this.DisconnectFromGuest();
						this.ThrowXdePipeException(XdePipeError.ConnectionClosedByServer);
					}
					num += bytesTransferred;
				}
				catch (ObjectDisposedException)
				{
					this.ThrowXdePipeException(PipeExceptions.PipeShutdown);
				}
				catch (XdePipeException)
				{
					throw;
				}
				catch (Exception ex)
				{
					this.ThrowXdePipeException(ex.Message);
				}
			}
			while (num != receiveSize);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003A14 File Offset: 0x00001C14
		private int ReceiveIntFromGuestPrivate()
		{
			byte[] array = new byte[4];
			this.ReceiveWithSizePrivate(array, 0, array.Length);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003A3C File Offset: 0x00001C3C
		private void ConnectSocket()
		{
			DateTime now = DateTime.Now;
			for (;;)
			{
				Socket socket = this.hostSocket;
				if (socket != null)
				{
					socket.Dispose();
				}
				try
				{
					this.hostSocket = this.addressInfo.CreateSocket();
					EndPoint endPoint = this.addressInfo.GetEndPoint();
					this.hostSocket.Connect(endPoint);
				}
				catch (SocketException exception)
				{
					if (this.disposed || this.cancelAsyncIOWait.WaitOne(0))
					{
						return;
					}
					DateTime now2 = DateTime.Now;
					if (now2 < now || now2 - now > XdePipe.connectTimeout)
					{
						this.GenerateConnectionErrorEncountered(exception);
						return;
					}
					continue;
				}
				catch (Exception exception2)
				{
					this.GenerateConnectionErrorEncountered(exception2);
					return;
				}
				break;
			}
			this.recvAsyncEventArgs = new SocketAsyncEventArgs();
			this.recvAsyncEventArgs.Completed += this.RecvCompletedCallback;
			this.sendAsyncEventArgs = new SocketAsyncEventArgs();
			this.sendAsyncEventArgs.Completed += this.SendCompletedCallback;
			this.recvCompleted = new AutoResetEvent(false);
			this.recvAsyncWaitEvents = new WaitHandle[2];
			this.recvAsyncWaitEvents[0] = this.cancelAsyncIOWait;
			this.recvAsyncWaitEvents[1] = this.recvCompleted;
			this.sendCompleted = new AutoResetEvent(false);
			this.sendAsyncWaitEvents = new WaitHandle[2];
			this.sendAsyncWaitEvents[0] = this.cancelAsyncIOWait;
			this.sendAsyncWaitEvents[1] = this.sendCompleted;
			try
			{
				this.SendToGuestPrivate(this.PipeGuid);
				if (this.ReceiveIntFromGuestPrivate() != 57004)
				{
					this.ThrowXdePipeException(PipeExceptions.ServerReturnedIncorrectAck);
				}
			}
			catch (Exception exception3)
			{
				this.GenerateConnectionErrorEncountered(exception3);
				return;
			}
			this.IsConnected = true;
			Logger.Instance.Log("PipeConnected", Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				pipe = base.GetType().ToString()
			});
			EventHandler connectionSucceeded = this.ConnectionSucceeded;
			if (connectionSucceeded == null)
			{
				return;
			}
			connectionSucceeded(this, EventArgs.Empty);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003C34 File Offset: 0x00001E34
		private void CheckForConnection()
		{
			if (!this.IsConnected)
			{
				if (this.cancelAsyncIOWait.WaitOne(0))
				{
					this.ThrowXdePipeException(PipeExceptions.PipeShutdown);
					return;
				}
				this.ThrowXdePipeException(PipeExceptions.PipeNotConnected);
			}
		}

		// Token: 0x04000019 RID: 25
		private static TimeSpan connectTimeout = new TimeSpan(0, 15, 0);

		// Token: 0x0400001A RID: 26
		private Socket hostSocket;

		// Token: 0x0400001B RID: 27
		private Thread connectionThread;

		// Token: 0x0400001C RID: 28
		private AutoResetEvent recvCompleted;

		// Token: 0x0400001D RID: 29
		private AutoResetEvent sendCompleted;

		// Token: 0x0400001E RID: 30
		private ManualResetEvent cancelAsyncIOWait;

		// Token: 0x0400001F RID: 31
		private SocketAsyncEventArgs recvAsyncEventArgs;

		// Token: 0x04000020 RID: 32
		private SocketAsyncEventArgs sendAsyncEventArgs;

		// Token: 0x04000021 RID: 33
		private WaitHandle[] recvAsyncWaitEvents;

		// Token: 0x04000022 RID: 34
		private WaitHandle[] sendAsyncWaitEvents;

		// Token: 0x04000023 RID: 35
		private bool disposed;

		// Token: 0x04000024 RID: 36
		private CultureInfo cultureInfo;

		// Token: 0x04000025 RID: 37
		private bool connected;

		// Token: 0x04000026 RID: 38
		private IXdeConnectionAddressInfo addressInfo;
	}
}
