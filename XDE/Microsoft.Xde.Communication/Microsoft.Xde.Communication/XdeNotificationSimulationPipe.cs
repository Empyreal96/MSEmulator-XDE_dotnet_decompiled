using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Communication
{
	// Token: 0x0200000A RID: 10
	public class XdeNotificationSimulationPipe : XdePipe, IXdeNotificationSimulationPipe, IXdeAutomationNotificationSimulationPipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdePipe, IXdeConnectionController, IDisposable
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00002DB9 File Offset: 0x00000FB9
		protected XdeNotificationSimulationPipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeNotificationSimulationPipe.notificationGuid)
		{
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002DC7 File Offset: 0x00000FC7
		public static IXdeNotificationSimulationPipe XdeNotificationSimulationPipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeNotificationSimulationPipe(addressInfo);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public int SendNotificationSimulationPayload(string uri, int type, string payload, string tagId, string groupId)
		{
			base.SendToGuest(3);
			if (base.ReceiveIntFromGuest() != 0)
			{
				base.ThrowXdePipeException(PipeExceptions.NotificationSendPayloadError);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("URI:" + this.GenerateFourCharacterHex(uri.Length) + ":" + uri);
			stringBuilder.Append("TYPE:0001:" + type);
			stringBuilder.Append("PAYLOAD:" + this.GenerateFourCharacterHex(payload.Length) + ":" + payload);
			if (tagId != null)
			{
				stringBuilder.Append("TAG:" + this.GenerateFourCharacterHex(tagId.Length) + ":" + tagId);
			}
			if (groupId != null)
			{
				stringBuilder.Append("GROUP:" + this.GenerateFourCharacterHex(groupId.Length) + ":" + groupId);
			}
			if (this.SendStringToGuest(stringBuilder.ToString()) != 0)
			{
				base.ThrowXdePipeException(PipeExceptions.NotificationSendPayloadError);
			}
			return base.ReceiveIntFromGuest();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002ECC File Offset: 0x000010CC
		public void SetNotificationSimulationEnabled(bool enabled)
		{
			base.SendToGuest(1);
			if (base.ReceiveIntFromGuest() != 0)
			{
				base.ThrowXdePipeException(PipeExceptions.NotificationSendPayloadError);
			}
			int data = enabled ? 1 : 0;
			base.SendToGuest(data);
			if (base.ReceiveIntFromGuest() != 0)
			{
				if (enabled)
				{
					base.ThrowXdePipeException(PipeExceptions.NotificationEnableSimulationError);
					return;
				}
				base.ThrowXdePipeException(PipeExceptions.NotificationDisableSimulationError);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002F24 File Offset: 0x00001124
		public string[] GetNotificationSimulationAppList()
		{
			List<string> list = new List<string>();
			base.SendToGuest(4);
			if (base.ReceiveIntFromGuest() != 0)
			{
				base.ThrowXdePipeException(PipeExceptions.NotificationSendPayloadError);
			}
			int num = base.ReceiveIntFromGuest();
			base.SendToGuest(0);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = base.ReceiveIntFromGuest();
					if (num2 <= 0)
					{
						string message = StringUtilities.CurrentCultureFormat(PipeExceptions.NotificationUriRetrievalError, Array.Empty<object>());
						base.ThrowXdePipeException(message);
					}
					base.SendToGuest(0);
					byte[] array = new byte[num2];
					base.ReceiveFromGuest(array);
					string @string = new UnicodeEncoding().GetString(array);
					list.Add(@string);
					base.SendToGuest(0);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002FC8 File Offset: 0x000011C8
		public string[] GetNotificationUriListForApp(string appId)
		{
			base.SendToGuest(2);
			if (base.ReceiveIntFromGuest() != 0)
			{
				base.ThrowXdePipeException(PipeExceptions.NotificationSendPayloadError);
			}
			string payload = "APPID:" + this.GenerateFourCharacterHex(appId.Length) + ":" + appId;
			if (this.SendStringToGuest(payload) != 0)
			{
				base.ThrowXdePipeException(PipeExceptions.NotificationSendPayloadError);
			}
			int num = base.ReceiveIntFromGuest();
			if (num == -1)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.NotificationUriRetrievalError, new object[]
				{
					appId
				});
				base.ThrowXdePipeException(message);
			}
			List<string> list = new List<string>();
			base.SendToGuest(0);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = base.ReceiveIntFromGuest();
					if (num2 <= 0)
					{
						string message2 = StringUtilities.CurrentCultureFormat(PipeExceptions.NotificationUriRetrievalError, new object[]
						{
							appId
						});
						base.ThrowXdePipeException(message2);
					}
					base.SendToGuest(0);
					byte[] array = new byte[num2];
					base.ReceiveFromGuest(array);
					string @string = new UnicodeEncoding().GetString(array);
					list.Add(@string);
					base.SendToGuest(0);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000030D0 File Offset: 0x000012D0
		private int SendStringToGuest(string payload)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(payload);
			base.SendToGuest(bytes.Length);
			int num = base.ReceiveIntFromGuest();
			if (num != 0)
			{
				return num;
			}
			base.SendToGuest(bytes);
			return base.ReceiveIntFromGuest();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000310D File Offset: 0x0000130D
		private string GenerateFourCharacterHex(int input)
		{
			if (input >= 0 && input <= 65535)
			{
				return string.Format("{0:X4}", input).ToLowerInvariant();
			}
			return null;
		}

		// Token: 0x04000017 RID: 23
		private static readonly Guid notificationGuid = new Guid("{d1b1bf56-429a-4f77-912a-c564d4803fb8}");

		// Token: 0x04000018 RID: 24
		private const int XDE_DATA_ACK = 0;

		// Token: 0x02000013 RID: 19
		private enum SimulationCommandId
		{
			// Token: 0x0400004C RID: 76
			SetNotificationSimulationState = 1,
			// Token: 0x0400004D RID: 77
			GetUriListForApp,
			// Token: 0x0400004E RID: 78
			SendNotificationPayload,
			// Token: 0x0400004F RID: 79
			GetAppList
		}

		// Token: 0x02000014 RID: 20
		private enum SimulationState
		{
			// Token: 0x04000051 RID: 81
			Disabled,
			// Token: 0x04000052 RID: 82
			Enabled
		}
	}
}
