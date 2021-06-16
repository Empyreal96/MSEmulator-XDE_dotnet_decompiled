using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.Deployment.Internal;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x0200000C RID: 12
	internal class VisualStudioXdeDevice : XdeDevice
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00003358 File Offset: 0x00001558
		private VisualStudioXdeDevice(string fileName, XmlNamespaceManager nsMgr, XmlNode deviceNode, XmlDocument locDoc)
		{
			this.fileName = fileName;
			this.nsMgr = nsMgr;
			this.deviceNode = deviceNode;
			this.locDoc = locDoc;
			this.propertyContainerNode = this.deviceNode.SelectSingleNode("PROPERTYCONTAINER/PROPERTY[@ID='D7C86969-EB5F-41e2-96CC-290683622203_ALL']/PROPERTYCONTAINER/xsl:element[@name='PROPERTY']/PROPERTYCONTAINER", this.nsMgr);
			string path = Path.Combine(XdeInstallation.DevicesFolder, string.Format("VS-{0}.json", this.ID));
			if (File.Exists(path))
			{
				try
				{
					this.appXdeDevice = AppXdeDevice.LoadFromFile(path);
				}
				catch (Exception)
				{
				}
			}
			if (this.appXdeDevice == null)
			{
				this.appXdeDevice = AppXdeDevice.InitNewDevice(path);
				this.appXdeDevice.ID = this.ID;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003414 File Offset: 0x00001614
		private string GetXdePathProperty(string propName)
		{
			return TokenExpander.ExpandSpecialPaths(this.GetNodeValue(this.propertyContainerNode, "PROPERTY[@ID='" + propName + "']"));
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003437 File Offset: 0x00001637
		private string GetXdeProperty(string propName)
		{
			return this.GetNodeValue(this.propertyContainerNode, "PROPERTY[@ID='" + propName + "']");
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003455 File Offset: 0x00001655
		private void SetXdeProperty(string propName, string value)
		{
			this.SetNodeValue(this.propertyContainerNode, "PROPERTY[@ID='" + propName + "']", value);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003474 File Offset: 0x00001674
		private string GetNodeValue(XmlNode parentNode, string xpath)
		{
			XmlNode xmlNode = parentNode.SelectSingleNode(xpath, this.nsMgr);
			if (xmlNode != null)
			{
				XmlElement xmlElement;
				if ((xmlElement = (xmlNode.FirstChild as XmlElement)) == null || !(xmlElement.Name == "xsl:value-of"))
				{
					return xmlNode.InnerText;
				}
				string attribute = xmlElement.GetAttribute("select");
				if (attribute != null)
				{
					XmlNode xmlNode2 = this.locDoc.SelectSingleNode(attribute, this.nsMgr);
					if (xmlNode2 != null)
					{
						return xmlNode2.InnerText;
					}
				}
			}
			return null;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000034E8 File Offset: 0x000016E8
		private void SetNodeValue(XmlNode parentNode, string xpath, string value)
		{
			XmlElement xmlElement = parentNode.SelectSingleNode(xpath, this.nsMgr) as XmlElement;
			if (xmlElement != null)
			{
				xmlElement.IsEmpty = true;
				XmlText newChild = xmlElement.OwnerDocument.CreateTextNode(value);
				xmlElement.AppendChild(newChild);
				this.dirty = true;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000352E File Offset: 0x0000172E
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00003541 File Offset: 0x00001741
		public override string Name
		{
			get
			{
				return this.GetNodeValue(this.deviceNode, "xsl:attribute[@name = 'Name']");
			}
			set
			{
				this.SetNodeValue(this.deviceNode, "xsl:attribute[@name = 'Name']", value);
				this.SetXdeProperty("name", value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003561 File Offset: 0x00001761
		public override string UapVersion
		{
			get
			{
				return this.GetNodeValue(this.deviceNode, "PROPERTYCONTAINER/PROPERTY[@ID='UapVersion']");
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003574 File Offset: 0x00001774
		public override string MacAddress
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000357B File Offset: 0x0000177B
		private bool IsInVSFolder
		{
			get
			{
				return this.FileName != null && this.FileName.StartsWith(VisualStudioXdeDevice.AddonsDir, StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003598 File Offset: 0x00001798
		public override bool IsDirty
		{
			get
			{
				return this.dirty || this.appXdeDevice.IsDirty;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000035AF File Offset: 0x000017AF
		public override bool CanDelete
		{
			get
			{
				return !this.IsInVSFolder;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000035BA File Offset: 0x000017BA
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000035C7 File Offset: 0x000017C7
		public override string Vhd
		{
			get
			{
				return this.GetXdePathProperty("vhd");
			}
			set
			{
				this.SetXdeProperty("vhd", value);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000035D5 File Offset: 0x000017D5
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000035F1 File Offset: 0x000017F1
		public override bool UseCheckpoint
		{
			get
			{
				return !(this.GetXdeProperty("snapshot") == "$nosnapshot$");
			}
			set
			{
				this.SetXdeProperty("snapshot", value ? string.Empty : "$nosnapshot$");
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000360D File Offset: 0x0000180D
		public override bool CanKernelDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003610 File Offset: 0x00001810
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00003613 File Offset: 0x00001813
		public override bool UseDiffDisk
		{
			get
			{
				return true;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000361C File Offset: 0x0000181C
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00003644 File Offset: 0x00001844
		public override string Sku
		{
			get
			{
				string text = this.GetXdePathProperty("sku");
				if (string.IsNullOrEmpty(text))
				{
					text = "WP";
				}
				return text;
			}
			set
			{
				this.SetXdeProperty("sku", value);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003652 File Offset: 0x00001852
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x0000365F File Offset: 0x0000185F
		public override string Skin
		{
			get
			{
				return this.GetXdeProperty("video");
			}
			set
			{
				this.SetXdeProperty("video", value);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00003670 File Offset: 0x00001870
		public override string XdeLocation
		{
			get
			{
				string xdePathProperty = this.GetXdePathProperty("XdeInstallLocation");
				string xdeProperty = this.GetXdeProperty("XdeExe");
				if (!string.IsNullOrEmpty(xdePathProperty) && !string.IsNullOrEmpty(xdeProperty))
				{
					return Path.Combine(xdePathProperty, xdeProperty);
				}
				return null;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000036AE File Offset: 0x000018AE
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000036C0 File Offset: 0x000018C0
		public override int MemSize
		{
			get
			{
				return int.Parse(this.GetXdeProperty("memsize"));
			}
			set
			{
				this.SetXdeProperty("memsize", value.ToString());
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000036D4 File Offset: 0x000018D4
		public override string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000036DC File Offset: 0x000018DC
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003703 File Offset: 0x00001903
		public override Guid ID
		{
			get
			{
				Guid result;
				Guid.TryParse(this.GetNodeValue(this.deviceNode, "xsl:attribute[@name = 'ID']"), out result);
				return result;
			}
			set
			{
				this.SetNodeValue(this.deviceNode, "xsl:attribute[@name = 'ID']", value.ToString());
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003723 File Offset: 0x00001923
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00003730 File Offset: 0x00001930
		public override bool ShowDisplayName
		{
			get
			{
				return this.appXdeDevice.ShowDisplayName;
			}
			set
			{
				this.appXdeDevice.ShowDisplayName = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000373E File Offset: 0x0000193E
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x0000374B File Offset: 0x0000194B
		public override string DisplayName
		{
			get
			{
				return this.appXdeDevice.DisplayName;
			}
			set
			{
				this.appXdeDevice.DisplayName = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00003759 File Offset: 0x00001959
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00003766 File Offset: 0x00001966
		public override bool NoGpu
		{
			get
			{
				return this.appXdeDevice.NoGpu;
			}
			set
			{
				this.appXdeDevice.NoGpu = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00003774 File Offset: 0x00001974
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00003781 File Offset: 0x00001981
		public override bool UseWmi
		{
			get
			{
				return this.appXdeDevice.UseWmi;
			}
			set
			{
				this.appXdeDevice.UseWmi = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000378F File Offset: 0x0000198F
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00003792 File Offset: 0x00001992
		public override bool DisableStateSep
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00003794 File Offset: 0x00001994
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00003797 File Offset: 0x00001997
		public override bool VisibleToVisualStudio
		{
			get
			{
				return true;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000379E File Offset: 0x0000199E
		protected override bool UsingOldEmulator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000037A1 File Offset: 0x000019A1
		public override bool CanModifyProperty(string propName)
		{
			return propName == "Name" || propName == "NoGpu" || propName == "DisplayName";
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000037CD File Offset: 0x000019CD
		public override Task Delete()
		{
			if (this.CanDelete)
			{
				base.DeleteVirtualMachine();
				if (File.Exists(this.fileName))
				{
					File.Delete(this.fileName);
				}
			}
			return Task.FromResult<int>(0);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000037FC File Offset: 0x000019FC
		public override void Save()
		{
			if (this.appXdeDevice.IsDirty)
			{
				this.appXdeDevice.Save();
			}
			if (this.dirty)
			{
				string directoryName = Path.GetDirectoryName(this.fileName);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				this.deviceNode.OwnerDocument.Save(this.fileName);
				this.dirty = false;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003861 File Offset: 0x00001A61
		private static IEnumerable<VisualStudioXdeDevice> LoadFromFile(string xdeConfigXslPath)
		{
			string path = Path.GetFileNameWithoutExtension(xdeConfigXslPath) + ".xml";
			string text = Path.Combine(VisualStudioXdeDevice.EngDir, path);
			XmlDocument xmlDocument = new XmlDocument();
			XmlDocument xdeConfigXml = null;
			xmlDocument.Load(xdeConfigXslPath);
			if (File.Exists(text))
			{
				xdeConfigXml = new XmlDocument();
				xdeConfigXml.Load(text);
				string name = Path.GetFileNameWithoutExtension(text).ToUpperInvariant().Replace('.', '_');
				List<XmlNode> list = new List<XmlNode>();
				foreach (object obj in xdeConfigXml.DocumentElement.ChildNodes)
				{
					list.Add((XmlNode)obj);
				}
				XmlElement xmlElement = xdeConfigXml.CreateElement(name);
				foreach (XmlNode newChild in list)
				{
					xmlElement.AppendChild(newChild);
				}
				xdeConfigXml.DocumentElement.AppendChild(xmlElement);
			}
			XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDocument.NameTable);
			nsMgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
			XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("//xsl:element[@name='DEVICE']", nsMgr);
			foreach (object obj2 in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj2;
				VisualStudioXdeDevice visualStudioXdeDevice = new VisualStudioXdeDevice(xdeConfigXslPath, nsMgr, xmlNode, xdeConfigXml);
				yield return visualStudioXdeDevice;
			}
			IEnumerator enumerator3 = null;
			yield break;
			yield break;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003871 File Offset: 0x00001A71
		public static IEnumerable<VisualStudioXdeDevice> LoadDevices()
		{
			foreach (string path in new string[]
			{
				VisualStudioXdeDevice.AddonsDir
			})
			{
				if (Directory.Exists(path))
				{
					foreach (string xdeConfigXslPath in Directory.GetFiles(path, "ImageConfig.XDE.*.xsl"))
					{
						foreach (VisualStudioXdeDevice visualStudioXdeDevice in VisualStudioXdeDevice.LoadFromFile(xdeConfigXslPath))
						{
							yield return visualStudioXdeDevice;
						}
						IEnumerator<VisualStudioXdeDevice> enumerator = null;
					}
					string[] array2 = null;
				}
			}
			string[] array = null;
			yield break;
			yield break;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000387A File Offset: 0x00001A7A
		internal static string GetAppMadeDevicePath(Guid id)
		{
			return Path.Combine(VisualStudioXdeDevice.AddonsDir, string.Format("ImageConfig.AppXDE.{0:D}.xsl", id));
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003896 File Offset: 0x00001A96
		internal static bool AppMadeDeviceExists(Guid id)
		{
			return File.Exists(VisualStudioXdeDevice.GetAppMadeDevicePath(id));
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000038A4 File Offset: 0x00001AA4
		internal static VisualStudioXdeDevice InitNewDevice(Guid id, string name)
		{
			string xml;
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Microsoft.Xde.DeviceManagement.ImageConfig.XDE.Template.xsl"))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					xml = streamReader.ReadToEnd();
				}
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			string appMadeDevicePath = VisualStudioXdeDevice.GetAppMadeDevicePath(id);
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
			XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//xsl:element[@name='DEVICE']", xmlNamespaceManager);
			return new VisualStudioXdeDevice(appMadeDevicePath, xmlNamespaceManager, xmlNode, null)
			{
				ID = id,
				Name = name
			};
		}

		// Token: 0x0400002F RID: 47
		private static string Corecon12Dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft\\Phone Tools\\CoreCon\\12.0");

		// Token: 0x04000030 RID: 48
		public static string AddonsDir = Path.Combine(VisualStudioXdeDevice.Corecon12Dir, "addons");

		// Token: 0x04000031 RID: 49
		private static string EngDir = Path.Combine(VisualStudioXdeDevice.Corecon12Dir, "1033");

		// Token: 0x04000032 RID: 50
		private string fileName;

		// Token: 0x04000033 RID: 51
		private XmlNode deviceNode;

		// Token: 0x04000034 RID: 52
		private XmlNamespaceManager nsMgr;

		// Token: 0x04000035 RID: 53
		private XmlDocument locDoc;

		// Token: 0x04000036 RID: 54
		private XmlNode propertyContainerNode;

		// Token: 0x04000037 RID: 55
		private AppXdeDevice appXdeDevice;

		// Token: 0x04000038 RID: 56
		private const string DeviceNameXPath = "xsl:attribute[@name = 'Name']";

		// Token: 0x04000039 RID: 57
		private const string VMNamePropertyName = "name";

		// Token: 0x0400003A RID: 58
		private bool dirty;

		// Token: 0x0400003B RID: 59
		private const string NoSnapshotValue = "$nosnapshot$";
	}
}
