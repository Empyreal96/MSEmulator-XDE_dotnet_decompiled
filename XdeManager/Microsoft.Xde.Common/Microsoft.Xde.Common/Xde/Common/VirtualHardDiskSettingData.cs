using System;
using System.Globalization;
using System.Management;
using System.Xml;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000072 RID: 114
	public class VirtualHardDiskSettingData
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x000072D0 File Offset: 0x000054D0
		public VirtualHardDiskSettingData(VirtualHardDiskType diskType, VirtualHardDiskFormat diskFormat, string path, string parentPath, long maxInternalSize, int blockSize, int logicalSectorSize, int physicalSectorSize)
		{
			this.DiskType = diskType;
			this.DiskFormat = diskFormat;
			this.Path = path;
			this.ParentPath = parentPath;
			this.MaxInternalSize = maxInternalSize;
			this.BlockSize = (long)blockSize;
			this.LogicalSectorSize = (long)logicalSectorSize;
			this.PhysicalSectorSize = (long)physicalSectorSize;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00007323 File Offset: 0x00005523
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000732B File Offset: 0x0000552B
		public VirtualHardDiskType DiskType { get; private set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00007334 File Offset: 0x00005534
		// (set) Token: 0x060002AD RID: 685 RVA: 0x0000733C File Offset: 0x0000553C
		public VirtualHardDiskFormat DiskFormat { get; private set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00007345 File Offset: 0x00005545
		// (set) Token: 0x060002AF RID: 687 RVA: 0x0000734D File Offset: 0x0000554D
		public string Path { get; private set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00007356 File Offset: 0x00005556
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000735E File Offset: 0x0000555E
		public string ParentPath { get; private set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x00007367 File Offset: 0x00005567
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0000736F File Offset: 0x0000556F
		public long MaxInternalSize { get; private set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00007378 File Offset: 0x00005578
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x00007380 File Offset: 0x00005580
		public long BlockSize { get; private set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00007389 File Offset: 0x00005589
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x00007391 File Offset: 0x00005591
		public long LogicalSectorSize { get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000739A File Offset: 0x0000559A
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x000073A2 File Offset: 0x000055A2
		public long PhysicalSectorSize { get; private set; }

		// Token: 0x060002BA RID: 698 RVA: 0x000073AC File Offset: 0x000055AC
		public static VirtualHardDiskSettingData Parse(string embeddedInstance)
		{
			string text = null;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(embeddedInstance);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/INSTANCE/@CLASSNAME");
			if (xmlNodeList.Count != 1)
			{
				throw new FormatException();
			}
			if (xmlNodeList[0].Value != "Msvm_VirtualHardDiskSettingData")
			{
				throw new FormatException();
			}
			xmlNodeList = xmlDocument.SelectNodes("//PROPERTY[@NAME = 'Type']/VALUE/child::text()");
			if (xmlNodeList.Count != 1)
			{
				throw new FormatException();
			}
			VirtualHardDiskType virtualHardDiskType = (VirtualHardDiskType)int.Parse(xmlNodeList[0].Value, NumberStyles.None, CultureInfo.InvariantCulture);
			if (virtualHardDiskType != VirtualHardDiskType.Differencing && virtualHardDiskType != VirtualHardDiskType.DynamicallyExpanding && virtualHardDiskType != VirtualHardDiskType.FixedSize)
			{
				throw new FormatException();
			}
			xmlNodeList = xmlDocument.SelectNodes("//PROPERTY[@NAME = 'Format']/VALUE/child::text()");
			if (xmlNodeList.Count != 1)
			{
				throw new FormatException();
			}
			VirtualHardDiskFormat virtualHardDiskFormat = (VirtualHardDiskFormat)int.Parse(xmlNodeList[0].Value, NumberStyles.None, CultureInfo.InvariantCulture);
			if (virtualHardDiskFormat != VirtualHardDiskFormat.Vhd && virtualHardDiskFormat != VirtualHardDiskFormat.Vhdx)
			{
				throw new FormatException();
			}
			xmlNodeList = xmlDocument.SelectNodes("//PROPERTY[@NAME = 'Path']/VALUE/child::text()");
			if (xmlNodeList.Count != 1)
			{
				throw new FormatException();
			}
			string value = xmlNodeList[0].Value;
			xmlNodeList = xmlDocument.SelectNodes("//PROPERTY[@NAME = 'ParentPath']/VALUE/child::text()");
			if (xmlNodeList.Count == 1)
			{
				text = xmlNodeList[0].Value;
			}
			else if (xmlNodeList.Count != 0)
			{
				throw new FormatException();
			}
			if (virtualHardDiskType == VirtualHardDiskType.Differencing && string.IsNullOrEmpty(text))
			{
				throw new FormatException();
			}
			xmlNodeList = xmlDocument.SelectNodes("//PROPERTY[@NAME = 'MaxInternalSize']/VALUE/child::text()");
			if (xmlNodeList.Count != 1)
			{
				throw new FormatException();
			}
			long maxInternalSize = long.Parse(xmlNodeList[0].Value, CultureInfo.InvariantCulture);
			xmlNodeList = xmlDocument.SelectNodes("//PROPERTY[@NAME = 'BlockSize']/VALUE/child::text()");
			if (xmlNodeList.Count != 1)
			{
				throw new FormatException();
			}
			int blockSize = int.Parse(xmlNodeList[0].Value, CultureInfo.InvariantCulture);
			xmlNodeList = xmlDocument.SelectNodes("//PROPERTY[@NAME = 'LogicalSectorSize']/VALUE/child::text()");
			if (xmlNodeList.Count != 1)
			{
				throw new FormatException();
			}
			int logicalSectorSize = int.Parse(xmlNodeList[0].Value, CultureInfo.InvariantCulture);
			xmlNodeList = xmlDocument.SelectNodes("//PROPERTY[@NAME = 'PhysicalSectorSize']/VALUE/child::text()");
			if (xmlNodeList.Count != 1)
			{
				throw new FormatException();
			}
			int physicalSectorSize = int.Parse(xmlNodeList[0].Value, CultureInfo.InvariantCulture);
			return new VirtualHardDiskSettingData(virtualHardDiskType, virtualHardDiskFormat, value, text, maxInternalSize, blockSize, logicalSectorSize, physicalSectorSize);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00007600 File Offset: 0x00005800
		public string GetVirtualHardDiskSettingDataEmbeddedInstance(string serverName, string namespacePath)
		{
			string text;
			using (ManagementClass managementClass = new ManagementClass(new ManagementPath
			{
				Server = serverName,
				NamespacePath = namespacePath,
				ClassName = "Msvm_VirtualHardDiskSettingData"
			}))
			{
				using (ManagementObject managementObject = managementClass.CreateInstance())
				{
					managementObject["Type"] = this.DiskType;
					managementObject["Format"] = this.DiskFormat;
					managementObject["Path"] = this.Path;
					managementObject["ParentPath"] = this.ParentPath;
					managementObject["MaxInternalSize"] = this.MaxInternalSize;
					managementObject["BlockSize"] = this.BlockSize;
					managementObject["LogicalSectorSize"] = this.LogicalSectorSize;
					managementObject["PhysicalSectorSize"] = this.PhysicalSectorSize;
					text = managementObject.GetText(TextFormat.WmiDtd20);
				}
			}
			return text;
		}
	}
}
