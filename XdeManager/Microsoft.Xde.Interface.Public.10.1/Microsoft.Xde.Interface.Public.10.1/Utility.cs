using System;
using System.ServiceModel;
using System.Threading;
using Microsoft.Win32;

namespace Microsoft.Xde.Interface
{
	// Token: 0x02000009 RID: 9
	public static class Utility
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002073 File Offset: 0x00000273
		public static EventWaitHandle CreateInitializeEvent(string virtualMachineName)
		{
			return new EventWaitHandle(false, EventResetMode.ManualReset, "XdeOnServerInitialize" + virtualMachineName);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002087 File Offset: 0x00000287
		public static string GetXdeOwnershipMutexName(string virtualMachineName)
		{
			return "Microsoft.XDE.Ownership." + virtualMachineName;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002094 File Offset: 0x00000294
		public static T Connect<T>(string virtualMachineName, object implementation, EndPointType endPointType, TimeSpan timeout, out ChannelFactory<T> channelFactory)
		{
			bool flag = false;
			do
			{
				using (EventWaitHandle eventWaitHandle = Utility.CreateInitializeEvent(virtualMachineName))
				{
					if (eventWaitHandle.WaitOne(timeout))
					{
						flag = true;
					}
					else if (!Utility.DoesXdeMutexExist(virtualMachineName))
					{
						throw new Exception("Failed to connect to Xde server for virtual machine \"" + virtualMachineName + "\".");
					}
				}
			}
			while (!flag);
			channelFactory = new DuplexChannelFactory<T>(new InstanceContext(implementation), Utility.CreateNamedPipeBinding(), new EndpointAddress(CommonNames.GetEndPointName(virtualMachineName, endPointType)));
			return channelFactory.CreateChannel();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000211C File Offset: 0x0000031C
		public static void TryCloseOrAbort(ICommunicationObject communicationObj)
		{
			if (communicationObj.State != CommunicationState.Closed)
			{
				try
				{
					communicationObj.Close();
				}
				catch
				{
					communicationObj.Abort();
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002154 File Offset: 0x00000354
		internal static NetNamedPipeBinding CreateNamedPipeBinding()
		{
			NetNamedPipeBinding result;
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
				using (RegistryKey registryKey2 = registryKey.OpenSubKey("Software\\Microsoft\\Xde"))
				{
					NetNamedPipeSecurityMode securityMode = NetNamedPipeSecurityMode.Transport;
					if (registryKey2 != null)
					{
						object value = registryKey2.GetValue("PipeTransport", 1, RegistryValueOptions.None);
						if (value is int && (int)value == 0)
						{
							securityMode = NetNamedPipeSecurityMode.None;
						}
					}
					result = new NetNamedPipeBinding(securityMode);
				}
			}
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000021E4 File Offset: 0x000003E4
		private static bool DoesXdeMutexExist(string virtualMachineName)
		{
			bool result;
			try
			{
				using (Mutex.OpenExisting(Utility.GetXdeOwnershipMutexName(virtualMachineName)))
				{
					result = true;
				}
			}
			catch (WaitHandleCannotBeOpenedException)
			{
				result = false;
			}
			return result;
		}
	}
}
