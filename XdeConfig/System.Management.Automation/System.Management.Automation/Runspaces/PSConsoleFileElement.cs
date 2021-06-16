using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Text;
using System.Xml;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200084E RID: 2126
	internal class PSConsoleFileElement
	{
		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060051DD RID: 20957 RVA: 0x001B4D53 File Offset: 0x001B2F53
		internal string MonadVersion
		{
			get
			{
				return this.monadVersion;
			}
		}

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060051DE RID: 20958 RVA: 0x001B4D5B File Offset: 0x001B2F5B
		internal Collection<string> PSSnapIns
		{
			get
			{
				return this.mshsnapins;
			}
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x001B4D63 File Offset: 0x001B2F63
		private PSConsoleFileElement(string version)
		{
			this.monadVersion = version;
			this.mshsnapins = new Collection<string>();
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x001B4D80 File Offset: 0x001B2F80
		internal static void WriteToFile(string path, Version version, IEnumerable<PSSnapInInfo> snapins)
		{
			PSConsoleFileElement._mshsnapinTracer.WriteLine("Saving console info to file {0}.", new object[]
			{
				path
			});
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.Encoding = Encoding.UTF8;
			using (Stream stream = new FileStream(path, FileMode.OpenOrCreate))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
				{
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement("PSConsoleFile");
					xmlWriter.WriteAttributeString("ConsoleSchemaVersion", "1.0");
					xmlWriter.WriteStartElement("PSVersion");
					xmlWriter.WriteString(version.ToString());
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("PSSnapIns");
					foreach (PSSnapInInfo pssnapInInfo in snapins)
					{
						xmlWriter.WriteStartElement("PSSnapIn");
						xmlWriter.WriteAttributeString("Name", pssnapInInfo.Name);
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndDocument();
				}
			}
			PSConsoleFileElement._mshsnapinTracer.WriteLine("Saving console info succeeded.", new object[0]);
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x001B4ED0 File Offset: 0x001B30D0
		internal static PSConsoleFileElement CreateFromFile(string path)
		{
			PSConsoleFileElement._mshsnapinTracer.WriteLine("Loading console info from file {0}.", new object[]
			{
				path
			});
			XmlDocument xmlDocument = InternalDeserializer.LoadUnsafeXmlDocument(new FileInfo(path), false, null);
			if (xmlDocument["PSConsoleFile"] == null)
			{
				PSConsoleFileElement._mshsnapinTracer.TraceError("Console file {0} doesn't contain tag {1}.", new object[]
				{
					path,
					"PSConsoleFile"
				});
				throw new XmlException(StringUtil.Format(ConsoleInfoErrorStrings.MonadConsoleNotFound, path));
			}
			if (xmlDocument["PSConsoleFile"]["PSVersion"] == null || string.IsNullOrEmpty(xmlDocument["PSConsoleFile"]["PSVersion"].InnerText))
			{
				PSConsoleFileElement._mshsnapinTracer.TraceError("Console file {0} doesn't contain tag {1}.", new object[]
				{
					path,
					"PSVersion"
				});
				throw new XmlException(StringUtil.Format(ConsoleInfoErrorStrings.MonadVersionNotFound, path));
			}
			XmlElement xmlElement = xmlDocument["PSConsoleFile"];
			if (!xmlElement.HasAttribute("ConsoleSchemaVersion"))
			{
				PSConsoleFileElement._mshsnapinTracer.TraceError("Console file {0} doesn't contain tag schema version.", new object[]
				{
					path
				});
				throw new XmlException(StringUtil.Format(ConsoleInfoErrorStrings.BadConsoleVersion, path));
			}
			if (!xmlElement.GetAttribute("ConsoleSchemaVersion").Equals("1.0", StringComparison.OrdinalIgnoreCase))
			{
				string format = StringUtil.Format(ConsoleInfoErrorStrings.BadConsoleVersion, path);
				string text = string.Format(CultureInfo.CurrentCulture, format, new object[]
				{
					"1.0"
				});
				PSConsoleFileElement._mshsnapinTracer.TraceError(text, new object[0]);
				throw new XmlException(text);
			}
			xmlElement = xmlDocument["PSConsoleFile"]["PSVersion"];
			PSConsoleFileElement psconsoleFileElement = new PSConsoleFileElement(xmlElement.InnerText.Trim());
			bool flag = false;
			bool flag2 = false;
			for (XmlNode xmlNode = xmlDocument["PSConsoleFile"].FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType != XmlNodeType.Comment)
				{
					xmlElement = (xmlNode as XmlElement);
					if (xmlElement == null)
					{
						throw new XmlException(ConsoleInfoErrorStrings.BadXMLFormat);
					}
					if (xmlElement.Name == "PSVersion")
					{
						if (flag2)
						{
							PSConsoleFileElement._mshsnapinTracer.TraceError("Console file {0} contains more than one  msh versions", new object[]
							{
								path
							});
							throw new XmlException(StringUtil.Format(ConsoleInfoErrorStrings.MultipleMshSnapinsElementNotSupported, "PSVersion"));
						}
						flag2 = true;
					}
					else
					{
						if (xmlElement.Name != "PSSnapIns")
						{
							PSConsoleFileElement._mshsnapinTracer.TraceError("Tag {0} is not supported in console file", new object[]
							{
								xmlElement.Name
							});
							throw new XmlException(StringUtil.Format(ConsoleInfoErrorStrings.BadXMLElementFound, new object[]
							{
								xmlElement.Name,
								"PSConsoleFile",
								"PSVersion",
								"PSSnapIns"
							}));
						}
						if (flag)
						{
							PSConsoleFileElement._mshsnapinTracer.TraceError("Console file {0} contains more than one mshsnapin lists", new object[]
							{
								path
							});
							throw new XmlException(StringUtil.Format(ConsoleInfoErrorStrings.MultipleMshSnapinsElementNotSupported, "PSSnapIns"));
						}
						flag = true;
						for (XmlNode xmlNode2 = xmlElement.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
						{
							XmlElement xmlElement2 = xmlNode2 as XmlElement;
							if (xmlElement2 == null || xmlElement2.Name != "PSSnapIn")
							{
								throw new XmlException(StringUtil.Format(ConsoleInfoErrorStrings.PSSnapInNotFound, xmlNode2.Name));
							}
							string attribute = xmlElement2.GetAttribute("Name");
							if (string.IsNullOrEmpty(attribute))
							{
								throw new XmlException(ConsoleInfoErrorStrings.IDNotFound);
							}
							psconsoleFileElement.mshsnapins.Add(attribute);
							PSConsoleFileElement._mshsnapinTracer.WriteLine("Found in mshsnapin {0} in console file {1}", new object[]
							{
								attribute,
								path
							});
						}
					}
				}
			}
			return psconsoleFileElement;
		}

		// Token: 0x04002A15 RID: 10773
		private const string MSHCONSOLEFILE = "PSConsoleFile";

		// Token: 0x04002A16 RID: 10774
		private const string CSCHEMAVERSION = "ConsoleSchemaVersion";

		// Token: 0x04002A17 RID: 10775
		private const string CSCHEMAVERSIONNUMBER = "1.0";

		// Token: 0x04002A18 RID: 10776
		private const string PSVERSION = "PSVersion";

		// Token: 0x04002A19 RID: 10777
		private const string SNAPINS = "PSSnapIns";

		// Token: 0x04002A1A RID: 10778
		private const string SNAPIN = "PSSnapIn";

		// Token: 0x04002A1B RID: 10779
		private const string SNAPINNAME = "Name";

		// Token: 0x04002A1C RID: 10780
		private readonly string monadVersion;

		// Token: 0x04002A1D RID: 10781
		private readonly Collection<string> mshsnapins;

		// Token: 0x04002A1E RID: 10782
		private static readonly PSTraceSource _mshsnapinTracer = PSTraceSource.GetTracer("MshSnapinLoadUnload", "Loading and unloading mshsnapins", false);
	}
}
