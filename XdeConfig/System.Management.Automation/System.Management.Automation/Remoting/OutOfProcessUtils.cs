using System;
using System.Globalization;
using System.IO;
using System.Management.Automation.Tracing;
using System.Xml;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000373 RID: 883
	internal static class OutOfProcessUtils
	{
		// Token: 0x06002B65 RID: 11109 RVA: 0x000F0558 File Offset: 0x000EE758
		static OutOfProcessUtils()
		{
			OutOfProcessUtils.XmlReaderSettings.CheckCharacters = false;
			OutOfProcessUtils.XmlReaderSettings.IgnoreComments = true;
			OutOfProcessUtils.XmlReaderSettings.IgnoreProcessingInstructions = true;
			OutOfProcessUtils.XmlReaderSettings.XmlResolver = null;
			OutOfProcessUtils.XmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x000F05A8 File Offset: 0x000EE7A8
		internal static string CreateDataPacket(byte[] data, DataPriorityType streamType, Guid psGuid)
		{
			return string.Format(CultureInfo.InvariantCulture, "<{0} {1}='{2}' {3}='{4}'>{5}</{0}>", new object[]
			{
				"Data",
				"Stream",
				streamType.ToString(),
				"PSGuid",
				psGuid.ToString(),
				Convert.ToBase64String(data)
			});
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000F060D File Offset: 0x000EE80D
		internal static string CreateDataAckPacket(Guid psGuid)
		{
			return OutOfProcessUtils.CreatePSGuidPacket("DataAck", psGuid);
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x000F061A File Offset: 0x000EE81A
		internal static string CreateCommandPacket(Guid psGuid)
		{
			return OutOfProcessUtils.CreatePSGuidPacket("Command", psGuid);
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x000F0627 File Offset: 0x000EE827
		internal static string CreateCommandAckPacket(Guid psGuid)
		{
			return OutOfProcessUtils.CreatePSGuidPacket("CommandAck", psGuid);
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000F0634 File Offset: 0x000EE834
		internal static string CreateClosePacket(Guid psGuid)
		{
			return OutOfProcessUtils.CreatePSGuidPacket("Close", psGuid);
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000F0641 File Offset: 0x000EE841
		internal static string CreateCloseAckPacket(Guid psGuid)
		{
			return OutOfProcessUtils.CreatePSGuidPacket("CloseAck", psGuid);
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000F064E File Offset: 0x000EE84E
		internal static string CreateSignalPacket(Guid psGuid)
		{
			return OutOfProcessUtils.CreatePSGuidPacket("Signal", psGuid);
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000F065B File Offset: 0x000EE85B
		internal static string CreateSignalAckPacket(Guid psGuid)
		{
			return OutOfProcessUtils.CreatePSGuidPacket("SignalAck", psGuid);
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000F0668 File Offset: 0x000EE868
		private static string CreatePSGuidPacket(string element, Guid psGuid)
		{
			return string.Format(CultureInfo.InvariantCulture, "<{0} {1}='{2}' />", new object[]
			{
				element,
				"PSGuid",
				psGuid.ToString()
			});
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000F06AC File Offset: 0x000EE8AC
		internal static void ProcessData(string data, OutOfProcessUtils.DataProcessingDelegates callbacks)
		{
			if (string.IsNullOrEmpty(data))
			{
				return;
			}
			XmlReader xmlReader = XmlReader.Create(new StringReader(data), OutOfProcessUtils.XmlReaderSettings);
			while (xmlReader.Read())
			{
				XmlNodeType nodeType = xmlReader.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.EndElement)
					{
						throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownNodeType, RemotingErrorIdStrings.IPCUnknownNodeType, new object[]
						{
							xmlReader.NodeType.ToString(),
							XmlNodeType.Element.ToString(),
							XmlNodeType.EndElement.ToString()
						});
					}
				}
				else
				{
					OutOfProcessUtils.ProcessElement(xmlReader, callbacks);
				}
			}
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000F0740 File Offset: 0x000EE940
		private static void ProcessElement(XmlReader xmlReader, OutOfProcessUtils.DataProcessingDelegates callbacks)
		{
			PowerShellTraceSource traceSource = PowerShellTraceSourceFactory.GetTraceSource();
			string localName;
			switch (localName = xmlReader.LocalName)
			{
			case "Data":
			{
				if (xmlReader.AttributeCount != 2)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCWrongAttributeCountForDataElement, RemotingErrorIdStrings.IPCWrongAttributeCountForDataElement, new object[]
					{
						"Stream",
						"PSGuid",
						"Data"
					});
				}
				string attribute = xmlReader.GetAttribute("Stream");
				string attribute2 = xmlReader.GetAttribute("PSGuid");
				Guid psGuid = new Guid(attribute2);
				if (!xmlReader.Read())
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCInsufficientDataforElement, RemotingErrorIdStrings.IPCInsufficientDataforElement, new object[]
					{
						"Data"
					});
				}
				if (xmlReader.NodeType != XmlNodeType.Text)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCOnlyTextExpectedInDataElement, RemotingErrorIdStrings.IPCOnlyTextExpectedInDataElement, new object[]
					{
						xmlReader.NodeType,
						"Data",
						XmlNodeType.Text
					});
				}
				string value = xmlReader.Value;
				traceSource.WriteMessage("OutOfProcessUtils.ProcessElement : PS_OUT_OF_PROC_DATA received, psGuid : " + psGuid.ToString());
				byte[] rawData = Convert.FromBase64String(value);
				callbacks.DataPacketReceived(rawData, attribute, psGuid);
				return;
			}
			case "DataAck":
			{
				if (xmlReader.AttributeCount != 1)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCWrongAttributeCountForElement, RemotingErrorIdStrings.IPCWrongAttributeCountForElement, new object[]
					{
						"PSGuid",
						"DataAck"
					});
				}
				string attribute3 = xmlReader.GetAttribute("PSGuid");
				Guid psGuid2 = new Guid(attribute3);
				traceSource.WriteMessage("OutOfProcessUtils.ProcessElement : PS_OUT_OF_PROC_DATA_ACK received, psGuid : " + psGuid2.ToString());
				callbacks.DataAckPacketReceived(psGuid2);
				return;
			}
			case "Command":
			{
				if (xmlReader.AttributeCount != 1)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCWrongAttributeCountForElement, RemotingErrorIdStrings.IPCWrongAttributeCountForElement, new object[]
					{
						"PSGuid",
						"Command"
					});
				}
				string attribute4 = xmlReader.GetAttribute("PSGuid");
				Guid psGuid3 = new Guid(attribute4);
				traceSource.WriteMessage("OutOfProcessUtils.ProcessElement : PS_OUT_OF_PROC_COMMAND received, psGuid : " + psGuid3.ToString());
				callbacks.CommandCreationPacketReceived(psGuid3);
				return;
			}
			case "CommandAck":
			{
				if (xmlReader.AttributeCount != 1)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCWrongAttributeCountForElement, RemotingErrorIdStrings.IPCWrongAttributeCountForElement, new object[]
					{
						"PSGuid",
						"CommandAck"
					});
				}
				string attribute5 = xmlReader.GetAttribute("PSGuid");
				Guid psGuid4 = new Guid(attribute5);
				traceSource.WriteMessage("OutOfProcessUtils.ProcessElement : PS_OUT_OF_PROC_COMMAND_ACK received, psGuid : " + psGuid4.ToString());
				callbacks.CommandCreationAckReceived(psGuid4);
				return;
			}
			case "Close":
			{
				if (xmlReader.AttributeCount != 1)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCWrongAttributeCountForElement, RemotingErrorIdStrings.IPCWrongAttributeCountForElement, new object[]
					{
						"PSGuid",
						"Close"
					});
				}
				string attribute6 = xmlReader.GetAttribute("PSGuid");
				Guid psGuid5 = new Guid(attribute6);
				traceSource.WriteMessage("OutOfProcessUtils.ProcessElement : PS_OUT_OF_PROC_CLOSE received, psGuid : " + psGuid5.ToString());
				callbacks.ClosePacketReceived(psGuid5);
				return;
			}
			case "CloseAck":
			{
				if (xmlReader.AttributeCount != 1)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCWrongAttributeCountForElement, RemotingErrorIdStrings.IPCWrongAttributeCountForElement, new object[]
					{
						"PSGuid",
						"CloseAck"
					});
				}
				string attribute7 = xmlReader.GetAttribute("PSGuid");
				Guid psGuid6 = new Guid(attribute7);
				traceSource.WriteMessage("OutOfProcessUtils.ProcessElement : PS_OUT_OF_PROC_CLOSE_ACK received, psGuid : " + psGuid6.ToString());
				callbacks.CloseAckPacketReceived(psGuid6);
				return;
			}
			case "Signal":
			{
				if (xmlReader.AttributeCount != 1)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCWrongAttributeCountForElement, RemotingErrorIdStrings.IPCWrongAttributeCountForElement, new object[]
					{
						"PSGuid",
						"Signal"
					});
				}
				string attribute8 = xmlReader.GetAttribute("PSGuid");
				Guid psGuid7 = new Guid(attribute8);
				traceSource.WriteMessage("OutOfProcessUtils.ProcessElement : PS_OUT_OF_PROC_SIGNAL received, psGuid : " + psGuid7.ToString());
				callbacks.SignalPacketReceived(psGuid7);
				return;
			}
			case "SignalAck":
			{
				if (xmlReader.AttributeCount != 1)
				{
					throw new PSRemotingTransportException(PSRemotingErrorId.IPCWrongAttributeCountForElement, RemotingErrorIdStrings.IPCWrongAttributeCountForElement, new object[]
					{
						"PSGuid",
						"SignalAck"
					});
				}
				string attribute9 = xmlReader.GetAttribute("PSGuid");
				Guid psGuid8 = new Guid(attribute9);
				traceSource.WriteMessage("OutOfProcessUtils.ProcessElement : PS_OUT_OF_PROC_SIGNAL_ACK received, psGuid : " + psGuid8.ToString());
				callbacks.SignalAckPacketReceived(psGuid8);
				return;
			}
			}
			throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
			{
				xmlReader.LocalName
			});
		}

		// Token: 0x040015D1 RID: 5585
		internal const string PS_OUT_OF_PROC_DATA_TAG = "Data";

		// Token: 0x040015D2 RID: 5586
		internal const string PS_OUT_OF_PROC_DATA_ACK_TAG = "DataAck";

		// Token: 0x040015D3 RID: 5587
		internal const string PS_OUT_OF_PROC_STREAM_ATTRIBUTE = "Stream";

		// Token: 0x040015D4 RID: 5588
		internal const string PS_OUT_OF_PROC_PSGUID_ATTRIBUTE = "PSGuid";

		// Token: 0x040015D5 RID: 5589
		internal const string PS_OUT_OF_PROC_CLOSE_TAG = "Close";

		// Token: 0x040015D6 RID: 5590
		internal const string PS_OUT_OF_PROC_CLOSE_ACK_TAG = "CloseAck";

		// Token: 0x040015D7 RID: 5591
		internal const string PS_OUT_OF_PROC_COMMAND_TAG = "Command";

		// Token: 0x040015D8 RID: 5592
		internal const string PS_OUT_OF_PROC_COMMAND_ACK_TAG = "CommandAck";

		// Token: 0x040015D9 RID: 5593
		internal const string PS_OUT_OF_PROC_SIGNAL_TAG = "Signal";

		// Token: 0x040015DA RID: 5594
		internal const string PS_OUT_OF_PROC_SIGNAL_ACK_TAG = "SignalAck";

		// Token: 0x040015DB RID: 5595
		internal const int EXITCODE_UNHANDLED_EXCEPTION = 4000;

		// Token: 0x040015DC RID: 5596
		internal static XmlReaderSettings XmlReaderSettings = new XmlReaderSettings();

		// Token: 0x02000374 RID: 884
		// (Invoke) Token: 0x06002B72 RID: 11122
		internal delegate void DataPacketReceived(byte[] rawData, string stream, Guid psGuid);

		// Token: 0x02000375 RID: 885
		// (Invoke) Token: 0x06002B76 RID: 11126
		internal delegate void DataAckPacketReceived(Guid psGuid);

		// Token: 0x02000376 RID: 886
		// (Invoke) Token: 0x06002B7A RID: 11130
		internal delegate void CommandCreationPacketReceived(Guid psGuid);

		// Token: 0x02000377 RID: 887
		// (Invoke) Token: 0x06002B7E RID: 11134
		internal delegate void CommandCreationAckReceived(Guid psGuid);

		// Token: 0x02000378 RID: 888
		// (Invoke) Token: 0x06002B82 RID: 11138
		internal delegate void ClosePacketReceived(Guid psGuid);

		// Token: 0x02000379 RID: 889
		// (Invoke) Token: 0x06002B86 RID: 11142
		internal delegate void CloseAckPacketReceived(Guid psGuid);

		// Token: 0x0200037A RID: 890
		// (Invoke) Token: 0x06002B8A RID: 11146
		internal delegate void SignalPacketReceived(Guid psGuid);

		// Token: 0x0200037B RID: 891
		// (Invoke) Token: 0x06002B8E RID: 11150
		internal delegate void SignalAckPacketReceived(Guid psGuid);

		// Token: 0x0200037C RID: 892
		internal struct DataProcessingDelegates
		{
			// Token: 0x040015DD RID: 5597
			internal OutOfProcessUtils.DataPacketReceived DataPacketReceived;

			// Token: 0x040015DE RID: 5598
			internal OutOfProcessUtils.DataAckPacketReceived DataAckPacketReceived;

			// Token: 0x040015DF RID: 5599
			internal OutOfProcessUtils.CommandCreationPacketReceived CommandCreationPacketReceived;

			// Token: 0x040015E0 RID: 5600
			internal OutOfProcessUtils.CommandCreationAckReceived CommandCreationAckReceived;

			// Token: 0x040015E1 RID: 5601
			internal OutOfProcessUtils.SignalPacketReceived SignalPacketReceived;

			// Token: 0x040015E2 RID: 5602
			internal OutOfProcessUtils.SignalAckPacketReceived SignalAckPacketReceived;

			// Token: 0x040015E3 RID: 5603
			internal OutOfProcessUtils.ClosePacketReceived ClosePacketReceived;

			// Token: 0x040015E4 RID: 5604
			internal OutOfProcessUtils.CloseAckPacketReceived CloseAckPacketReceived;
		}
	}
}
