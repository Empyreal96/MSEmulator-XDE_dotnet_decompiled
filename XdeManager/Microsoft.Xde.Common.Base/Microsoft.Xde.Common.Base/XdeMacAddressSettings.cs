using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200002C RID: 44
	public static class XdeMacAddressSettings
	{
		// Token: 0x060001BF RID: 447 RVA: 0x0000496C File Offset: 0x00002B6C
		public static string GetMacAddressForVmName(string name)
		{
			return XdeMacAddressSettings.GetMacAddressForVmName(name, false);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00004978 File Offset: 0x00002B78
		public static string GetMacAddressForVmName(string name, bool updateMruPosition)
		{
			if (!XdeAppUtils.IsPackagedEmulatorInstalled)
			{
				return string.Empty;
			}
			if (!updateMruPosition)
			{
				Dictionary<string, string> vmNameToMacAddr = XdeMacAddressSettings.VmNameToMacAddr;
				lock (vmNameToMacAddr)
				{
					string result;
					if (XdeMacAddressSettings.VmNameToMacAddr.TryGetValue(name, out result))
					{
						return result;
					}
				}
			}
			try
			{
				XdeMacAddressSettings.MacMutex.WaitOne();
			}
			catch (AbandonedMutexException)
			{
			}
			string macAddress;
			try
			{
				string text = Path.Combine(XdeAppUtils.AppDataFolder, "Settings");
				string path = Path.Combine(text, "MACAddrData.json");
				List<XdeMacAddressSettings.MacAddressData> list = new List<XdeMacAddressSettings.MacAddressData>();
				if (File.Exists(path))
				{
					using (StreamReader streamReader = File.OpenText(path))
					{
						JsonSerializer jsonSerializer = new JsonSerializer();
						try
						{
							list = (List<XdeMacAddressSettings.MacAddressData>)jsonSerializer.Deserialize(streamReader, typeof(List<XdeMacAddressSettings.MacAddressData>));
						}
						catch (Exception)
						{
						}
					}
					Dictionary<string, string> vmNameToMacAddr = XdeMacAddressSettings.VmNameToMacAddr;
					lock (vmNameToMacAddr)
					{
						XdeMacAddressSettings.VmNameToMacAddr.Clear();
						foreach (XdeMacAddressSettings.MacAddressData macAddressData in list)
						{
							XdeMacAddressSettings.VmNameToMacAddr[macAddressData.VmName] = macAddressData.MacAddress;
						}
						string result2;
						if (!updateMruPosition && XdeMacAddressSettings.VmNameToMacAddr.TryGetValue(name, out result2))
						{
							return result2;
						}
					}
				}
				int num = list.FindIndex((XdeMacAddressSettings.MacAddressData i) => StringComparer.OrdinalIgnoreCase.Equals(name, i.VmName));
				XdeMacAddressSettings.MacAddressData macAddressData2 = null;
				bool flag2 = false;
				if (num == -1)
				{
					flag2 = true;
					macAddressData2 = new XdeMacAddressSettings.MacAddressData
					{
						VmName = name
					};
					HashSet<string> hashSet = new HashSet<string>();
					foreach (XdeMacAddressSettings.MacAddressData macAddressData3 in list)
					{
						hashSet.Add(macAddressData3.MacAddress);
					}
					for (int j = 0; j < 255; j++)
					{
						string text2 = StringUtilities.InvariantCultureFormat("02-DF-DF-DF-DE-{0:X2}", new object[]
						{
							j
						});
						if (!hashSet.Contains(text2))
						{
							macAddressData2.MacAddress = text2;
							break;
						}
					}
					Dictionary<string, string> vmNameToMacAddr;
					if (macAddressData2.MacAddress == null)
					{
						XdeMacAddressSettings.MacAddressData macAddressData4 = list.Last<XdeMacAddressSettings.MacAddressData>();
						macAddressData2.MacAddress = macAddressData4.MacAddress;
						list.RemoveAt(list.Count - 1);
						vmNameToMacAddr = XdeMacAddressSettings.VmNameToMacAddr;
						lock (vmNameToMacAddr)
						{
							XdeMacAddressSettings.VmNameToMacAddr.Remove(macAddressData4.VmName);
						}
					}
					list.Insert(0, macAddressData2);
					vmNameToMacAddr = XdeMacAddressSettings.VmNameToMacAddr;
					lock (vmNameToMacAddr)
					{
						XdeMacAddressSettings.VmNameToMacAddr[name] = macAddressData2.MacAddress;
						goto IL_2E0;
					}
				}
				macAddressData2 = list[num];
				if (updateMruPosition)
				{
					flag2 = true;
					list.RemoveAt(num);
					list.Insert(0, macAddressData2);
				}
				IL_2E0:
				if (flag2)
				{
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					using (StreamWriter streamWriter = File.CreateText(path))
					{
						new JsonSerializer().Serialize(streamWriter, list);
					}
				}
				macAddress = macAddressData2.MacAddress;
			}
			finally
			{
				XdeMacAddressSettings.MacMutex.ReleaseMutex();
			}
			return macAddress;
		}

		// Token: 0x04000109 RID: 265
		private static Mutex MacMutex = new Mutex(false, "Microsoft.XDE.MACCreator");

		// Token: 0x0400010A RID: 266
		private static Dictionary<string, string> VmNameToMacAddr = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x02000075 RID: 117
		private class MacAddressData
		{
			// Token: 0x1700008E RID: 142
			// (get) Token: 0x06000218 RID: 536 RVA: 0x00005170 File Offset: 0x00003370
			// (set) Token: 0x06000219 RID: 537 RVA: 0x00005178 File Offset: 0x00003378
			public string VmName { get; set; }

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x0600021A RID: 538 RVA: 0x00005181 File Offset: 0x00003381
			// (set) Token: 0x0600021B RID: 539 RVA: 0x00005189 File Offset: 0x00003389
			public string MacAddress { get; set; }
		}
	}
}
