using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E3 RID: 739
	internal class RemoteSessionCapability
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x000C713B File Offset: 0x000C533B
		// (set) Token: 0x06002358 RID: 9048 RVA: 0x000C7143 File Offset: 0x000C5343
		internal Version ProtocolVersion
		{
			get
			{
				return this._protocolVersion;
			}
			set
			{
				this._protocolVersion = value;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x000C714C File Offset: 0x000C534C
		internal Version PSVersion
		{
			get
			{
				return this._psversion;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x0600235A RID: 9050 RVA: 0x000C7154 File Offset: 0x000C5354
		internal Version SerializationVersion
		{
			get
			{
				return this._serversion;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x000C715C File Offset: 0x000C535C
		internal RemotingDestination RemotingDestination
		{
			get
			{
				return this._remotingDestination;
			}
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x000C7164 File Offset: 0x000C5364
		internal RemoteSessionCapability(RemotingDestination remotingDestination)
		{
			this._protocolVersion = RemotingConstants.ProtocolVersion;
			this._psversion = new Version(2, 0);
			this._serversion = PSVersionInfo.SerializationVersion;
			this._remotingDestination = remotingDestination;
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x000C7196 File Offset: 0x000C5396
		internal RemoteSessionCapability(RemotingDestination remotingDestination, Version protocolVersion, Version psVersion, Version serVersion)
		{
			this._protocolVersion = protocolVersion;
			this._psversion = psVersion;
			this._serversion = serVersion;
			this._remotingDestination = remotingDestination;
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000C71BB File Offset: 0x000C53BB
		internal RemoteSessionCapability()
		{
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x000C71C3 File Offset: 0x000C53C3
		internal static RemoteSessionCapability CreateClientCapability()
		{
			return new RemoteSessionCapability(RemotingDestination.Server);
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x000C71CB File Offset: 0x000C53CB
		internal static RemoteSessionCapability CreateServerCapability()
		{
			return new RemoteSessionCapability(RemotingDestination.Client);
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x000C71D4 File Offset: 0x000C53D4
		internal static byte[] GetCurrentTimeZoneInByteFormat()
		{
			if (RemoteSessionCapability._timeZoneInByteFormat == null)
			{
				Exception ex = null;
				try
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					using (MemoryStream memoryStream = new MemoryStream())
					{
						binaryFormatter.Serialize(memoryStream, TimeZone.CurrentTimeZone);
						memoryStream.Seek(0L, SeekOrigin.Begin);
						byte[] array = new byte[memoryStream.Length];
						memoryStream.Read(array, 0, (int)memoryStream.Length);
						RemoteSessionCapability._timeZoneInByteFormat = array;
					}
				}
				catch (ArgumentNullException ex2)
				{
					ex = ex2;
				}
				catch (SerializationException ex3)
				{
					ex = ex3;
				}
				catch (SecurityException ex4)
				{
					ex = ex4;
				}
				if (ex != null)
				{
					RemoteSessionCapability._timeZoneInByteFormat = new byte[0];
				}
			}
			return RemoteSessionCapability._timeZoneInByteFormat;
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x000C729C File Offset: 0x000C549C
		// (set) Token: 0x06002363 RID: 9059 RVA: 0x000C72A4 File Offset: 0x000C54A4
		internal TimeZone TimeZone
		{
			get
			{
				return this._timeZone;
			}
			set
			{
				this._timeZone = value;
			}
		}

		// Token: 0x04001125 RID: 4389
		private Version _psversion;

		// Token: 0x04001126 RID: 4390
		private Version _serversion;

		// Token: 0x04001127 RID: 4391
		private Version _protocolVersion;

		// Token: 0x04001128 RID: 4392
		private RemotingDestination _remotingDestination;

		// Token: 0x04001129 RID: 4393
		private TimeZone _timeZone;

		// Token: 0x0400112A RID: 4394
		private static byte[] _timeZoneInByteFormat;
	}
}
