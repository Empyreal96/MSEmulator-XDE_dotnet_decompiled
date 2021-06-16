using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security;
using System.Threading.Tasks;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public class DevicePortalException : Exception
	{
		// Token: 0x0600015D RID: 349 RVA: 0x00007848 File Offset: 0x00005A48
		public DevicePortalException(HttpStatusCode statusCode, DevicePortalException.HttpErrorResponse errorResponse, Uri requestUri = null, string message = "", Exception innerException = null) : this(statusCode, errorResponse.Reason, requestUri, message, innerException)
		{
			base.HResult = errorResponse.ErrorCode;
			this.Reason = errorResponse.ErrorMessage;
			if (base.HResult == 0)
			{
				base.HResult = errorResponse.Code;
			}
			if (string.IsNullOrEmpty(this.Reason))
			{
				this.Reason = errorResponse.Reason;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000078AC File Offset: 0x00005AAC
		public DevicePortalException(HttpStatusCode statusCode, string reason, Uri requestUri = null, string message = "", Exception innerException = null) : base(message, innerException)
		{
			this.StatusCode = statusCode;
			this.Reason = reason;
			this.RequestUri = requestUri;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000078CD File Offset: 0x00005ACD
		// (set) Token: 0x06000160 RID: 352 RVA: 0x000078D5 File Offset: 0x00005AD5
		public HttpStatusCode StatusCode { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000078DE File Offset: 0x00005ADE
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000078E6 File Offset: 0x00005AE6
		public string Reason { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000078EF File Offset: 0x00005AEF
		// (set) Token: 0x06000164 RID: 356 RVA: 0x000078F7 File Offset: 0x00005AF7
		public Uri RequestUri { get; private set; }

		// Token: 0x06000165 RID: 357 RVA: 0x00007900 File Offset: 0x00005B00
		public static async Task<DevicePortalException> CreateAsync(HttpResponseMessage responseMessage, string message = "", Exception innerException = null)
		{
			DevicePortalException error = new DevicePortalException(responseMessage.StatusCode, responseMessage.ReasonPhrase, (responseMessage.RequestMessage != null) ? responseMessage.RequestMessage.RequestUri : null, message, innerException);
			try
			{
				if (responseMessage.Content != null)
				{
					Stream dataStream = null;
					using (HttpContent content = responseMessage.Content)
					{
						dataStream = new MemoryStream();
						await content.CopyToAsync(dataStream).ConfigureAwait(false);
						dataStream.Position = 0L;
					}
					HttpContent content = null;
					if (dataStream != null)
					{
						DevicePortalException.HttpErrorResponse httpErrorResponse = (DevicePortalException.HttpErrorResponse)new DataContractJsonSerializer(typeof(DevicePortalException.HttpErrorResponse)).ReadObject(dataStream);
						error.HResult = httpErrorResponse.ErrorCode;
						error.Reason = httpErrorResponse.ErrorMessage;
						if (error.HResult == 0)
						{
							error.HResult = httpErrorResponse.Code;
						}
						if (string.IsNullOrEmpty(error.Reason))
						{
							error.Reason = httpErrorResponse.Reason;
						}
						dataStream.Dispose();
					}
					dataStream = null;
				}
			}
			catch (Exception)
			{
			}
			return error;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007955 File Offset: 0x00005B55
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x02000143 RID: 323
		[DataContract]
		public class HttpErrorResponse
		{
			// Token: 0x1700016C RID: 364
			// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001754A File Offset: 0x0001574A
			// (set) Token: 0x060005D5 RID: 1493 RVA: 0x00017552 File Offset: 0x00015752
			[DataMember(Name = "ErrorCode")]
			public int ErrorCode { get; private set; }

			// Token: 0x1700016D RID: 365
			// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001755B File Offset: 0x0001575B
			// (set) Token: 0x060005D7 RID: 1495 RVA: 0x00017563 File Offset: 0x00015763
			[DataMember(Name = "Code")]
			public int Code { get; private set; }

			// Token: 0x1700016E RID: 366
			// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001756C File Offset: 0x0001576C
			// (set) Token: 0x060005D9 RID: 1497 RVA: 0x00017574 File Offset: 0x00015774
			[DataMember(Name = "ErrorMessage")]
			public string ErrorMessage { get; private set; }

			// Token: 0x1700016F RID: 367
			// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001757D File Offset: 0x0001577D
			// (set) Token: 0x060005DB RID: 1499 RVA: 0x00017585 File Offset: 0x00015785
			[DataMember(Name = "Reason")]
			public string Reason { get; private set; }

			// Token: 0x17000170 RID: 368
			// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001758E File Offset: 0x0001578E
			// (set) Token: 0x060005DD RID: 1501 RVA: 0x00017596 File Offset: 0x00015796
			[DataMember(Name = "Success")]
			public bool Success { get; private set; }
		}
	}
}
