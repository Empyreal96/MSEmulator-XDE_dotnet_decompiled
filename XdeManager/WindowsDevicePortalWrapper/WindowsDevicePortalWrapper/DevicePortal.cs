using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000002 RID: 2
	public class DevicePortal
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		// (remove) Token: 0x06000002 RID: 2 RVA: 0x00002080 File Offset: 0x00000280
		public event UnvalidatedCertEventHandler UnvalidatedCert;

		// Token: 0x06000003 RID: 3 RVA: 0x000020B8 File Offset: 0x000002B8
		public async Task<X509Certificate2> GetRootDeviceCertificateAsync()
		{
			X509Certificate2 result = null;
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.RootCertificateEndpoint, null);
			using (Stream stream = await this.GetAsync(uri))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					result = new X509Certificate2(binaryReader.ReadBytes((int)stream.Length));
				}
			}
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020FD File Offset: 0x000002FD
		private void SetManualCertificate(X509Certificate2 cert)
		{
			this.manualCertificate = cert;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002108 File Offset: 0x00000308
		private bool ServerCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (this.manualCertificate != null)
			{
				chain.ChainPolicy.ExtraStore.Add(this.manualCertificate);
			}
			X509Certificate2 certificate2 = new X509Certificate2(certificate);
			bool flag = chain.Build(certificate2);
			if (!flag && this.manualCertificate != null)
			{
				foreach (X509ChainElement x509ChainElement in chain.ChainElements)
				{
					foreach (X509ChainStatus x509ChainStatus in x509ChainElement.ChainElementStatus)
					{
						if (x509ChainStatus.Status != X509ChainStatusFlags.NoError && x509ChainStatus.Status != X509ChainStatusFlags.UntrustedRoot && x509ChainStatus.Status != X509ChainStatusFlags.RevocationStatusUnknown)
						{
							return false;
						}
					}
					if (x509ChainElement.Certificate.Issuer == this.manualCertificate.Issuer && x509ChainElement.Certificate.Thumbprint == this.manualCertificate.Thumbprint)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				UnvalidatedCertEventHandler unvalidatedCert = this.UnvalidatedCert;
				bool? flag2 = (unvalidatedCert != null) ? new bool?(unvalidatedCert(this, certificate, chain, sslPolicyErrors)) : null;
				if (flag2 != null)
				{
					bool? flag3 = flag2;
					bool flag4 = true;
					if (flag3.GetValueOrDefault() == flag4 & flag3 != null)
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002250 File Offset: 0x00000450
		public async Task<ApplicationInstallStatus> GetInstallStatusAsync()
		{
			ApplicationInstallStatus status = ApplicationInstallStatus.None;
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.InstallStateApi, null);
			using (HttpClient client = new HttpClient(new WebRequestHandler
			{
				UseDefaultCredentials = false,
				Credentials = this.deviceConnection.Credentials,
				ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.ServerCertificateValidation)
			}))
			{
				this.ApplyHttpHeaders(client, DevicePortal.HttpMethods.Get);
				HttpResponseMessage httpResponseMessage = await client.GetAsync(uri).ConfigureAwait(false);
				using (HttpResponseMessage response = httpResponseMessage)
				{
					if (!response.IsSuccessStatusCode)
					{
						TaskAwaiter<DevicePortalException> taskAwaiter = DevicePortalException.CreateAsync(response, "", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<DevicePortalException> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<DevicePortalException>);
						}
						throw taskAwaiter.GetResult();
					}
					if (response.StatusCode == HttpStatusCode.OK)
					{
						if (response.Content == null)
						{
							status = ApplicationInstallStatus.Completed;
						}
						else
						{
							Stream dataStream = null;
							using (HttpContent content = response.Content)
							{
								dataStream = new MemoryStream();
								await content.CopyToAsync(dataStream).ConfigureAwait(false);
								dataStream.Position = 0L;
							}
							HttpContent content = null;
							if (dataStream == null)
							{
								throw new DevicePortalException(response.StatusCode, "Failed to deserialize GetInstallStatus response.", null, "", null);
							}
							DevicePortalException.HttpErrorResponse httpErrorResponse = (DevicePortalException.HttpErrorResponse)new DataContractJsonSerializer(typeof(DevicePortalException.HttpErrorResponse)).ReadObject(dataStream);
							if (!httpErrorResponse.Success)
							{
								throw new DevicePortalException(response.StatusCode, httpErrorResponse, uri, "", null);
							}
							status = ApplicationInstallStatus.Completed;
							dataStream = null;
						}
					}
					else if (response.StatusCode == HttpStatusCode.NoContent)
					{
						status = ApplicationInstallStatus.InProgress;
					}
				}
				HttpResponseMessage response = null;
			}
			HttpClient client = null;
			return status;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002298 File Offset: 0x00000498
		private async Task<Stream> PostAsync(Uri uri, Stream requestStream = null, string requestStreamContentType = null)
		{
			StreamContent streamContent = null;
			MemoryStream responseDataStream = null;
			if (requestStream != null)
			{
				streamContent = new StreamContent(requestStream);
				streamContent.Headers.Remove(DevicePortal.ContentTypeHeaderName);
				streamContent.Headers.TryAddWithoutValidation(DevicePortal.ContentTypeHeaderName, requestStreamContentType);
			}
			using (HttpClient client = new HttpClient(new WebRequestHandler
			{
				UseDefaultCredentials = false,
				Credentials = this.deviceConnection.Credentials,
				ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.ServerCertificateValidation)
			}))
			{
				this.ApplyHttpHeaders(client, DevicePortal.HttpMethods.Post);
				HttpResponseMessage httpResponseMessage = await client.PostAsync(uri, streamContent).ConfigureAwait(false);
				using (HttpResponseMessage response = httpResponseMessage)
				{
					if (!response.IsSuccessStatusCode)
					{
						TaskAwaiter<DevicePortalException> taskAwaiter = DevicePortalException.CreateAsync(response, "", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<DevicePortalException> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<DevicePortalException>);
						}
						throw taskAwaiter.GetResult();
					}
					this.RetrieveCsrfToken(response);
					if (response.Content != null)
					{
						using (HttpContent responseContent = response.Content)
						{
							responseDataStream = new MemoryStream();
							await responseContent.CopyToAsync(responseDataStream).ConfigureAwait(false);
							responseDataStream.Position = 0L;
						}
						HttpContent responseContent = null;
					}
				}
				HttpResponseMessage response = null;
			}
			HttpClient client = null;
			return responseDataStream;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022F8 File Offset: 0x000004F8
		private async Task<Stream> PutAsync(Uri uri, HttpContent body = null)
		{
			MemoryStream dataStream = null;
			using (HttpClient client = new HttpClient(new WebRequestHandler
			{
				UseDefaultCredentials = false,
				Credentials = this.deviceConnection.Credentials,
				ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.ServerCertificateValidation)
			}))
			{
				this.ApplyHttpHeaders(client, DevicePortal.HttpMethods.Put);
				HttpResponseMessage httpResponseMessage = await client.PutAsync(uri, body).ConfigureAwait(false);
				using (HttpResponseMessage response = httpResponseMessage)
				{
					if (!response.IsSuccessStatusCode)
					{
						TaskAwaiter<DevicePortalException> taskAwaiter = DevicePortalException.CreateAsync(response, "", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<DevicePortalException> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<DevicePortalException>);
						}
						throw taskAwaiter.GetResult();
					}
					this.RetrieveCsrfToken(response);
					if (response.Content != null)
					{
						using (HttpContent content = response.Content)
						{
							dataStream = new MemoryStream();
							await content.CopyToAsync(dataStream).ConfigureAwait(false);
							dataStream.Position = 0L;
						}
						HttpContent content = null;
					}
				}
				HttpResponseMessage response = null;
			}
			HttpClient client = null;
			return dataStream;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002350 File Offset: 0x00000550
		private async Task<Stream> DeleteAsync(Uri uri)
		{
			MemoryStream dataStream = null;
			using (HttpClient client = new HttpClient(new WebRequestHandler
			{
				UseDefaultCredentials = false,
				Credentials = this.deviceConnection.Credentials,
				ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.ServerCertificateValidation)
			}))
			{
				this.ApplyHttpHeaders(client, DevicePortal.HttpMethods.Delete);
				HttpResponseMessage httpResponseMessage = await client.DeleteAsync(uri).ConfigureAwait(false);
				using (HttpResponseMessage response = httpResponseMessage)
				{
					if (!response.IsSuccessStatusCode)
					{
						TaskAwaiter<DevicePortalException> taskAwaiter = DevicePortalException.CreateAsync(response, "", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<DevicePortalException> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<DevicePortalException>);
						}
						throw taskAwaiter.GetResult();
					}
					this.RetrieveCsrfToken(response);
					if (response.Content != null)
					{
						using (HttpContent content = response.Content)
						{
							dataStream = new MemoryStream();
							await content.CopyToAsync(dataStream).ConfigureAwait(false);
							dataStream.Position = 0L;
						}
						HttpContent content = null;
					}
				}
				HttpResponseMessage response = null;
			}
			HttpClient client = null;
			return dataStream;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023A0 File Offset: 0x000005A0
		private async Task<Stream> GetAsync(Uri uri)
		{
			MemoryStream dataStream = null;
			using (HttpClient client = new HttpClient(new WebRequestHandler
			{
				UseDefaultCredentials = false,
				Credentials = this.deviceConnection.Credentials,
				ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.ServerCertificateValidation)
			}))
			{
				this.ApplyHttpHeaders(client, DevicePortal.HttpMethods.Get);
				HttpResponseMessage httpResponseMessage = await client.GetAsync(uri).ConfigureAwait(false);
				using (HttpResponseMessage response = httpResponseMessage)
				{
					if (!response.IsSuccessStatusCode)
					{
						TaskAwaiter<DevicePortalException> taskAwaiter = DevicePortalException.CreateAsync(response, "", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<DevicePortalException> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<DevicePortalException>);
						}
						throw taskAwaiter.GetResult();
					}
					this.RetrieveCsrfToken(response);
					using (HttpContent content = response.Content)
					{
						dataStream = new MemoryStream();
						await content.CopyToAsync(dataStream).ConfigureAwait(false);
						dataStream.Position = 0L;
					}
					HttpContent content = null;
				}
				HttpResponseMessage response = null;
			}
			HttpClient client = null;
			return dataStream;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023F0 File Offset: 0x000005F0
		public async Task<List<DevicePortal.AppCrashDump>> GetAppCrashDumpListAsync()
		{
			return (await this.GetAsync<DevicePortal.AppCrashDumpList>(DevicePortal.AvailableCrashDumpsApi, null)).CrashDumps;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002438 File Offset: 0x00000638
		public async Task<Stream> GetAppCrashDumpAsync(DevicePortal.AppCrashDump crashdump)
		{
			string path = DevicePortal.CrashDumpFileApi + string.Format("?packageFullName={0}&fileName={1}", crashdump.PackageFullName, crashdump.Filename);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, path, null);
			return await this.GetAsync(uri);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002488 File Offset: 0x00000688
		public async Task DeleteAppCrashDumpAsync(DevicePortal.AppCrashDump crashdump)
		{
			await this.DeleteAsync(DevicePortal.CrashDumpFileApi, string.Format("packageFullName={0}&fileName={1}", crashdump.PackageFullName, crashdump.Filename));
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000024D8 File Offset: 0x000006D8
		public async Task<DevicePortal.AppCrashDumpSettings> GetAppCrashDumpSettingsAsync(DevicePortal.AppPackage app)
		{
			return await this.GetAppCrashDumpSettingsAsync(app.PackageFullName);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002528 File Offset: 0x00000728
		public async Task<DevicePortal.AppCrashDumpSettings> GetAppCrashDumpSettingsAsync(string packageFullname)
		{
			return await this.GetAsync<DevicePortal.AppCrashDumpSettings>(DevicePortal.CrashDumpSettingsApi, string.Format("packageFullname={0}", packageFullname));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002578 File Offset: 0x00000778
		public async Task SetAppCrashDumpSettingsAsync(DevicePortal.AppPackage app, bool enable = true)
		{
			string packageFullName = app.PackageFullName;
			await this.SetAppCrashDumpSettingsAsync(packageFullName, enable);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025D0 File Offset: 0x000007D0
		public async Task SetAppCrashDumpSettingsAsync(string packageFullName, bool enable = true)
		{
			if (enable)
			{
				await this.PostAsync(DevicePortal.CrashDumpSettingsApi, string.Format("packageFullName={0}", packageFullName));
			}
			else
			{
				await this.DeleteAsync(DevicePortal.CrashDumpSettingsApi, string.Format("packageFullName={0}", packageFullName));
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000012 RID: 18 RVA: 0x00002628 File Offset: 0x00000828
		// (remove) Token: 0x06000013 RID: 19 RVA: 0x00002660 File Offset: 0x00000860
		public event ApplicationInstallStatusEventHandler AppInstallStatus;

		// Token: 0x06000014 RID: 20 RVA: 0x00002698 File Offset: 0x00000898
		public async Task<DevicePortal.AppPackages> GetInstalledAppPackagesAsync()
		{
			return await this.GetAsync<DevicePortal.AppPackages>(DevicePortal.InstalledPackagesApi, null);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000026E0 File Offset: 0x000008E0
		public async Task InstallApplicationAsync(string appName, string packageFileName, List<string> dependencyFileNames, string certificateFileName = null, short stateCheckIntervalMs = 500, short timeoutInMinutes = 15, bool uninstallPreviousVersion = true)
		{
			string installPhaseDescription = string.Empty;
			try
			{
				FileInfo packageFile = new FileInfo(packageFileName);
				if (string.IsNullOrEmpty(appName))
				{
					appName = packageFile.Name;
				}
				if (uninstallPreviousVersion)
				{
					installPhaseDescription = string.Format("Uninstalling any previous version of {0}", appName);
					this.SendAppInstallStatus(ApplicationInstallStatus.InProgress, ApplicationInstallPhase.UninstallingPreviousVersion, installPhaseDescription);
					DevicePortal.AppPackages appPackages = await this.GetInstalledAppPackagesAsync();
					foreach (DevicePortal.PackageInfo packageInfo in appPackages.Packages)
					{
						if (packageInfo.Name == appName)
						{
							await this.UninstallApplicationAsync(packageInfo.FullName);
							break;
						}
					}
					List<DevicePortal.PackageInfo>.Enumerator enumerator = default(List<DevicePortal.PackageInfo>.Enumerator);
				}
				Uri uri;
				string arg;
				this.CreateAppInstallEndpointAndBoundaryString(packageFile.Name, out uri, out arg);
				using (MemoryStream dataStream = new MemoryStream())
				{
					installPhaseDescription = string.Format("Copying: {0}", packageFile.Name);
					this.SendAppInstallStatus(ApplicationInstallStatus.InProgress, ApplicationInstallPhase.CopyingFile, installPhaseDescription);
					byte[] bytes = Encoding.ASCII.GetBytes(string.Format("--{0}\r\n", arg));
					dataStream.Write(bytes, 0, bytes.Length);
					DevicePortal.CopyFileToRequestStream(packageFile, dataStream);
					foreach (string fileName in dependencyFileNames)
					{
						FileInfo fileInfo = new FileInfo(fileName);
						installPhaseDescription = string.Format("Copying: {0}", fileInfo.Name);
						this.SendAppInstallStatus(ApplicationInstallStatus.InProgress, ApplicationInstallPhase.CopyingFile, installPhaseDescription);
						bytes = Encoding.ASCII.GetBytes(string.Format("\r\n--{0}\r\n", arg));
						dataStream.Write(bytes, 0, bytes.Length);
						DevicePortal.CopyFileToRequestStream(fileInfo, dataStream);
					}
					if (!string.IsNullOrEmpty(certificateFileName))
					{
						FileInfo fileInfo2 = new FileInfo(certificateFileName);
						installPhaseDescription = string.Format("Copying: {0}", fileInfo2.Name);
						this.SendAppInstallStatus(ApplicationInstallStatus.InProgress, ApplicationInstallPhase.CopyingFile, installPhaseDescription);
						bytes = Encoding.ASCII.GetBytes(string.Format("\r\n--{0}\r\n", arg));
						dataStream.Write(bytes, 0, bytes.Length);
						DevicePortal.CopyFileToRequestStream(fileInfo2, dataStream);
					}
					bytes = Encoding.ASCII.GetBytes(string.Format("\r\n--{0}--\r\n", arg));
					dataStream.Write(bytes, 0, bytes.Length);
					dataStream.Position = 0L;
					string requestStreamContentType = string.Format("multipart/form-data; boundary={0}", arg);
					await this.PostAsync(uri, dataStream, requestStreamContentType);
				}
				MemoryStream dataStream = null;
				ApplicationInstallStatus applicationInstallStatus;
				do
				{
					installPhaseDescription = string.Format("Installing {0}", appName);
					this.SendAppInstallStatus(ApplicationInstallStatus.InProgress, ApplicationInstallPhase.Installing, installPhaseDescription);
					await Task.Delay(TimeSpan.FromMilliseconds((double)stateCheckIntervalMs));
					applicationInstallStatus = await this.GetInstallStatusAsync().ConfigureAwait(false);
				}
				while (applicationInstallStatus == ApplicationInstallStatus.InProgress);
				installPhaseDescription = string.Format("{0} installed successfully", appName);
				this.SendAppInstallStatus(ApplicationInstallStatus.Completed, ApplicationInstallPhase.Idle, installPhaseDescription);
				packageFile = null;
			}
			catch (Exception ex)
			{
				DevicePortalException ex2 = ex as DevicePortalException;
				if (ex2 != null)
				{
					this.SendAppInstallStatus(ApplicationInstallStatus.Failed, ApplicationInstallPhase.Idle, string.Format("Failed to install {0}: {1}", appName, ex2.Reason));
				}
				else
				{
					this.SendAppInstallStatus(ApplicationInstallStatus.Failed, ApplicationInstallPhase.Idle, string.Format("Failed to install {0}: {1}", appName, installPhaseDescription));
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002758 File Offset: 0x00000958
		public async Task UninstallApplicationAsync(string packageName)
		{
			await this.DeleteAsync(DevicePortal.PackageManagerApi, string.Format("package={0}", packageName));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000027A8 File Offset: 0x000009A8
		private void CreateAppInstallEndpointAndBoundaryString(string packageName, out Uri uri, out string boundaryString)
		{
			uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.PackageManagerApi, string.Format("package={0}", packageName));
			boundaryString = Guid.NewGuid().ToString();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000027EC File Offset: 0x000009EC
		private void SendAppInstallStatus(ApplicationInstallStatus status, ApplicationInstallPhase phase, string message = "")
		{
			ApplicationInstallStatusEventHandler appInstallStatus = this.AppInstallStatus;
			if (appInstallStatus == null)
			{
				return;
			}
			appInstallStatus(this, new ApplicationInstallStatusEventArgs(status, phase, message));
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002808 File Offset: 0x00000A08
		public async Task<DevicePortal.KnownFolders> GetKnownFoldersAsync()
		{
			return await this.GetAsync<DevicePortal.KnownFolders>(DevicePortal.KnownFoldersApi, null);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002850 File Offset: 0x00000A50
		public async Task<DevicePortal.FolderContents> GetFolderContentsAsync(string knownFolderId, string subPath = null, string packageFullName = null)
		{
			Dictionary<string, string> payload = this.BuildCommonFilePayload(knownFolderId, subPath, packageFullName);
			return await this.GetAsync<DevicePortal.FolderContents>(DevicePortal.GetFilesApi, Utilities.BuildQueryString(payload));
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000028B0 File Offset: 0x00000AB0
		public async Task<Stream> GetFileAsync(string knownFolderId, string filename, string subPath = null, string packageFullName = null)
		{
			Dictionary<string, string> dictionary = this.BuildCommonFilePayload(knownFolderId, subPath, packageFullName);
			dictionary.Add("filename", filename);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.GetFileApi, Utilities.BuildQueryString(dictionary));
			return await this.GetAsync(uri);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002918 File Offset: 0x00000B18
		public async Task UploadFileAsync(string knownFolderId, string filepath, string subPath = null, string packageFullName = null)
		{
			Dictionary<string, string> payload = this.BuildCommonFilePayload(knownFolderId, subPath, packageFullName);
			List<string> list = new List<string>();
			list.Add(filepath);
			await this.PostAsync(DevicePortal.GetFileApi, list, Utilities.BuildQueryString(payload));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002980 File Offset: 0x00000B80
		public async Task DeleteFileAsync(string knownFolderId, string filename, string subPath = null, string packageFullName = null)
		{
			Dictionary<string, string> dictionary = this.BuildCommonFilePayload(knownFolderId, subPath, packageFullName);
			dictionary.Add("filename", filename);
			await this.DeleteAsync(DevicePortal.GetFileApi, Utilities.BuildQueryString(dictionary));
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000029E8 File Offset: 0x00000BE8
		public async Task RenameFileAsync(string knownFolderId, string filename, string newFilename, string subPath = null, string packageFullName = null)
		{
			Dictionary<string, string> dictionary = this.BuildCommonFilePayload(knownFolderId, subPath, packageFullName);
			dictionary.Add("filename", filename);
			dictionary.Add("newfilename", newFilename);
			await this.PostAsync(DevicePortal.RenameFileApi, Utilities.BuildQueryString(dictionary));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002A58 File Offset: 0x00000C58
		private Dictionary<string, string> BuildCommonFilePayload(string knownFolderId, string subPath, string packageFullName)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("knownfolderid", knownFolderId);
			if (!string.IsNullOrEmpty(subPath))
			{
				if (!subPath.StartsWith("/", StringComparison.OrdinalIgnoreCase))
				{
					subPath = subPath.Insert(0, "/");
				}
				dictionary.Add("path", subPath);
			}
			if (!string.IsNullOrEmpty(packageFullName))
			{
				dictionary.Add("packagefullname", packageFullName);
			}
			else if (string.Equals(knownFolderId, "LocalAppData", StringComparison.OrdinalIgnoreCase))
			{
				throw new Exception("LocalAppData requires a packageFullName be provided.");
			}
			return dictionary;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public async Task<List<DevicePortal.Device>> GetDeviceListAsync()
		{
			return (await this.GetAsync<DevicePortal.DeviceList>(DevicePortal.InstalledDevicesApi, null)).Devices;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B20 File Offset: 0x00000D20
		public async Task<List<string>> GetServiceTagsAsync()
		{
			return (await this.GetAsync<DevicePortal.ServiceTags>(DevicePortal.TagsApi, null)).Tags;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002B68 File Offset: 0x00000D68
		public async Task AddServiceTagAsync(string tagValue)
		{
			await this.PostAsync(DevicePortal.TagApi, string.Format("tagValue={0}", tagValue));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public async Task DeleteAllTagsAsync()
		{
			await this.DeleteAsync(DevicePortal.TagsApi, null);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C00 File Offset: 0x00000E00
		public async Task DeleteTagAsync(string tagValue)
		{
			await this.DeleteAsync(DevicePortal.TagApi, string.Format("tagValue={0}", tagValue));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002C50 File Offset: 0x00000E50
		public async Task<List<DevicePortal.Dumpfile>> GetDumpfileListAsync()
		{
			return (await this.GetAsync<DevicePortal.DumpFileList>(DevicePortal.AvailableBugChecksApi, null)).DumpFiles;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002C98 File Offset: 0x00000E98
		public async Task<Stream> GetDumpFileAsync(DevicePortal.Dumpfile crashdump)
		{
			string path = DevicePortal.BugcheckFileApi + string.Format("?filename={0}", crashdump.Filename);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, path, null);
			return await this.GetAsync(uri);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002CE8 File Offset: 0x00000EE8
		public async Task<Stream> GetLiveKernelDumpAsync()
		{
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.LiveKernelDumpApi, null);
			return await this.GetAsync(uri);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D30 File Offset: 0x00000F30
		public async Task<Stream> GetLiveProcessDumpAsync(int pid)
		{
			string path = DevicePortal.LiveProcessDumpApi + string.Format("?pid={0}", pid);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, path, null);
			return await this.GetAsync(uri);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D80 File Offset: 0x00000F80
		public async Task<DevicePortal.DumpFileSettings> GetDumpFileSettingsAsync()
		{
			return await this.GetAsync<DevicePortal.DumpFileSettings>(DevicePortal.BugcheckSettingsApi, null);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public async Task SetDumpFileSettingsAsync(DevicePortal.DumpFileSettings dfs)
		{
			string payload = string.Format("autoreboot={0}&overwrite={1}&dumptype={2}&maxdumpcount={3}", new object[]
			{
				dfs.AutoReboot ? "1" : "0",
				dfs.Overwrite ? "1" : "0",
				(int)dfs.DumpType,
				dfs.MaxDumpCount
			});
			await this.PostAsync(DevicePortal.BugcheckSettingsApi, payload);
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600002B RID: 43 RVA: 0x00002E18 File Offset: 0x00001018
		// (remove) Token: 0x0600002C RID: 44 RVA: 0x00002E50 File Offset: 0x00001050
		public event WebSocketMessageReceivedEventHandler<DevicePortal.EtwEvents> RealtimeEventsMessageReceived;

		// Token: 0x0600002D RID: 45 RVA: 0x00002E88 File Offset: 0x00001088
		public async Task<DevicePortal.EtwProviders> GetCustomEtwProvidersAsync()
		{
			return await this.GetAsync<DevicePortal.EtwProviders>(DevicePortal.CustomEtwProvidersApi, null);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002ED0 File Offset: 0x000010D0
		public async Task<DevicePortal.EtwProviders> GetEtwProvidersAsync()
		{
			return await this.GetAsync<DevicePortal.EtwProviders>(DevicePortal.EtwProvidersApi, null);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002F18 File Offset: 0x00001118
		public async Task ToggleEtwProviderAsync(DevicePortal.EtwProviderInfo etwProvider, bool isEnabled = true, int level = 5)
		{
			await this.ToggleEtwProviderAsync(etwProvider.GUID, isEnabled, level);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002F78 File Offset: 0x00001178
		public async Task ToggleEtwProviderAsync(Guid etwProvider, bool isEnabled = true, int level = 5)
		{
			string arg = isEnabled ? "enable" : "disable";
			string message = string.Format("provider {0} {1} {2}", etwProvider, arg, level);
			await this.InitializeRealtimeEventsWebSocketAsync();
			await this.realtimeEventsWebSocket.SendMessageAsync(message);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002FD8 File Offset: 0x000011D8
		public async Task StartListeningForEtwEventsAsync()
		{
			await this.InitializeRealtimeEventsWebSocketAsync();
			if (!this.isListeningForRealtimeEvents)
			{
				this.isListeningForRealtimeEvents = true;
				this.realtimeEventsWebSocket.WebSocketMessageReceived += this.EtwEventsReceivedHandler;
			}
			await this.realtimeEventsWebSocket.ReceiveMessagesAsync();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003020 File Offset: 0x00001220
		public async Task StopListeningForEtwEventsAsync()
		{
			if (this.isListeningForRealtimeEvents)
			{
				this.isListeningForRealtimeEvents = false;
				this.realtimeEventsWebSocket.WebSocketMessageReceived -= this.EtwEventsReceivedHandler;
			}
			await this.realtimeEventsWebSocket.CloseAsync();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003068 File Offset: 0x00001268
		private async Task InitializeRealtimeEventsWebSocketAsync()
		{
			if (this.realtimeEventsWebSocket == null)
			{
				this.realtimeEventsWebSocket = new WebSocket<DevicePortal.EtwEvents>(this.deviceConnection, new Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool>(this.ServerCertificateValidation), false);
			}
			string realtimeEtwSessionApi = DevicePortal.RealtimeEtwSessionApi;
			await this.realtimeEventsWebSocket.ConnectAsync(realtimeEtwSessionApi, null);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000030AD File Offset: 0x000012AD
		private void EtwEventsReceivedHandler(WebSocket<DevicePortal.EtwEvents> sender, WebSocketMessageReceivedEventArgs<DevicePortal.EtwEvents> args)
		{
			if (args.Message != null)
			{
				WebSocketMessageReceivedEventHandler<DevicePortal.EtwEvents> realtimeEventsMessageReceived = this.RealtimeEventsMessageReceived;
				if (realtimeEventsMessageReceived == null)
				{
					return;
				}
				realtimeEventsMessageReceived(this, args);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000030CC File Offset: 0x000012CC
		public async Task<DevicePortal.IpConfiguration> GetIpConfigAsync()
		{
			return await this.GetAsync<DevicePortal.IpConfiguration>(DevicePortal.IpConfigApi, null);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003114 File Offset: 0x00001314
		public async Task<string> GetDeviceFamilyAsync()
		{
			return (await this.GetAsync<DevicePortal.DeviceOsFamily>(DevicePortal.DeviceFamilyApi, null).ConfigureAwait(false)).Family;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000315C File Offset: 0x0000135C
		public async Task<string> GetDeviceNameAsync()
		{
			return (await this.GetAsync<DevicePortal.DeviceName>(DevicePortal.MachineNameApi, null)).Name;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000031A1 File Offset: 0x000013A1
		public Task<DevicePortal.OperatingSystemInformation> GetOperatingSystemInformationAsync()
		{
			return this.GetAsync<DevicePortal.OperatingSystemInformation>(DevicePortal.OsInfoApi, null);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000031AF File Offset: 0x000013AF
		public Task SetDeviceNameAsync(string name)
		{
			return this.PostAsync(DevicePortal.MachineNameApi, string.Format("name={0}", Utilities.Hex64Encode(name)));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000031CC File Offset: 0x000013CC
		public async Task<Guid> GetActivePowerSchemeAsync()
		{
			return (await this.GetAsync<DevicePortal.ActivePowerScheme>(DevicePortal.ActivePowerSchemeApi, null)).Id;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003214 File Offset: 0x00001414
		public async Task<DevicePortal.BatteryState> GetBatteryStateAsync()
		{
			return await this.GetAsync<DevicePortal.BatteryState>(DevicePortal.BatteryStateApi, null);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000325C File Offset: 0x0000145C
		public async Task<DevicePortal.PowerState> GetPowerStateAsync()
		{
			return await this.GetAsync<DevicePortal.PowerState>(DevicePortal.PowerStateApi, null);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000032A4 File Offset: 0x000014A4
		public async Task RebootAsync()
		{
			await this.PostAsync(DevicePortal.RebootApi, null);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000032EC File Offset: 0x000014EC
		public async Task ShutdownAsync()
		{
			await this.PostAsync(DevicePortal.ShutdownApi, null);
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600003F RID: 63 RVA: 0x00003334 File Offset: 0x00001534
		// (remove) Token: 0x06000040 RID: 64 RVA: 0x0000336C File Offset: 0x0000156C
		public event WebSocketMessageReceivedEventHandler<DevicePortal.RunningProcesses> RunningProcessesMessageReceived;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000041 RID: 65 RVA: 0x000033A4 File Offset: 0x000015A4
		// (remove) Token: 0x06000042 RID: 66 RVA: 0x000033DC File Offset: 0x000015DC
		public event WebSocketMessageReceivedEventHandler<DevicePortal.SystemPerformanceInformation> SystemPerfMessageReceived;

		// Token: 0x06000043 RID: 67 RVA: 0x00003414 File Offset: 0x00001614
		public async Task<DevicePortal.RunningProcesses> GetRunningProcessesAsync()
		{
			return await this.GetAsync<DevicePortal.RunningProcesses>(DevicePortal.RunningProcessApi, null);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000345C File Offset: 0x0000165C
		public async Task StartListeningForRunningProcessesAsync()
		{
			if (this.deviceProcessesWebSocket == null)
			{
				this.deviceProcessesWebSocket = new WebSocket<DevicePortal.RunningProcesses>(this.deviceConnection, new Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool>(this.ServerCertificateValidation), false);
				this.deviceProcessesWebSocket.WebSocketMessageReceived += this.RunningProcessesReceivedHandler;
			}
			else if (this.deviceProcessesWebSocket.IsListeningForMessages)
			{
				return;
			}
			await this.deviceProcessesWebSocket.ConnectAsync(DevicePortal.RunningProcessApi, null);
			await this.deviceProcessesWebSocket.ReceiveMessagesAsync();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000034A4 File Offset: 0x000016A4
		public async Task StopListeningForRunningProcessesAsync()
		{
			await this.deviceProcessesWebSocket.CloseAsync();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000034EC File Offset: 0x000016EC
		public async Task<DevicePortal.SystemPerformanceInformation> GetSystemPerfAsync()
		{
			return await this.GetAsync<DevicePortal.SystemPerformanceInformation>(DevicePortal.SystemPerfApi, null);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003534 File Offset: 0x00001734
		public async Task StartListeningForSystemPerfAsync()
		{
			if (this.systemPerfWebSocket == null)
			{
				this.systemPerfWebSocket = new WebSocket<DevicePortal.SystemPerformanceInformation>(this.deviceConnection, new Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool>(this.ServerCertificateValidation), false);
				this.systemPerfWebSocket.WebSocketMessageReceived += this.SystemPerfReceivedHandler;
			}
			else if (this.systemPerfWebSocket.IsListeningForMessages)
			{
				return;
			}
			await this.systemPerfWebSocket.ConnectAsync(DevicePortal.SystemPerfApi, null);
			await this.systemPerfWebSocket.ReceiveMessagesAsync();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000357C File Offset: 0x0000177C
		public async Task StopListeningForSystemPerfAsync()
		{
			await this.systemPerfWebSocket.CloseAsync();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000035C1 File Offset: 0x000017C1
		private void RunningProcessesReceivedHandler(WebSocket<DevicePortal.RunningProcesses> sender, WebSocketMessageReceivedEventArgs<DevicePortal.RunningProcesses> args)
		{
			if (args.Message != null)
			{
				WebSocketMessageReceivedEventHandler<DevicePortal.RunningProcesses> runningProcessesMessageReceived = this.RunningProcessesMessageReceived;
				if (runningProcessesMessageReceived == null)
				{
					return;
				}
				runningProcessesMessageReceived(this, args);
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000035DD File Offset: 0x000017DD
		private void SystemPerfReceivedHandler(WebSocket<DevicePortal.SystemPerformanceInformation> sender, WebSocketMessageReceivedEventArgs<DevicePortal.SystemPerformanceInformation> args)
		{
			if (args.Message != null)
			{
				WebSocketMessageReceivedEventHandler<DevicePortal.SystemPerformanceInformation> systemPerfMessageReceived = this.SystemPerfMessageReceived;
				if (systemPerfMessageReceived == null)
				{
					return;
				}
				systemPerfMessageReceived(this, args);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000035FC File Offset: 0x000017FC
		public async Task<uint> LaunchApplicationAsync(string appid, string packageName)
		{
			string payload = string.Format("appid={0}&package={1}", Utilities.Hex64Encode(appid), Utilities.Hex64Encode(packageName));
			await this.PostAsync(DevicePortal.TaskManagerApi, payload);
			DevicePortal.RunningProcesses runningProcesses = await this.GetRunningProcessesAsync();
			uint result = 0U;
			foreach (DevicePortal.DeviceProcessInfo deviceProcessInfo in runningProcesses.Processes)
			{
				if (string.Compare(deviceProcessInfo.PackageFullName, packageName) == 0)
				{
					result = deviceProcessInfo.ProcessId;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003654 File Offset: 0x00001854
		public async Task TerminateApplicationAsync(string packageName)
		{
			await this.DeleteAsync(DevicePortal.TaskManagerApi, string.Format("package={0}", Utilities.Hex64Encode(packageName)));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000036A4 File Offset: 0x000018A4
		public async Task ConnectToWifiNetworkAsync(Guid networkAdapter, string ssid, string networkKey)
		{
			string payload = string.Format("interface={0}&ssid={1}&op=connect&createprofile=yes&key={2}", networkAdapter.ToString(), Utilities.Hex64Encode(ssid), Utilities.Hex64Encode(networkKey));
			await this.PostAsync(DevicePortal.WifiNetworkApi, payload);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003704 File Offset: 0x00001904
		public async Task<DevicePortal.WifiInterfaces> GetWifiInterfacesAsync()
		{
			return await this.GetAsync<DevicePortal.WifiInterfaces>(DevicePortal.WifiInterfacesApi, null);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000374C File Offset: 0x0000194C
		public async Task<DevicePortal.WifiNetworks> GetWifiNetworksAsync(Guid interfaceGuid)
		{
			return await this.GetAsync<DevicePortal.WifiNetworks>(DevicePortal.WifiNetworksApi, string.Format("interface={0}", interfaceGuid.ToString()));
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000379C File Offset: 0x0000199C
		public async Task<DevicePortal.WerDeviceReports> GetWindowsErrorReportsAsync()
		{
			this.CheckPlatformSupport();
			return await this.GetAsync<DevicePortal.WerDeviceReports>(DevicePortal.WindowsErrorReportsApi, null);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000037E4 File Offset: 0x000019E4
		public async Task<DevicePortal.WerFiles> GetWindowsErrorReportingFileListAsync(string user, string type, string name)
		{
			this.CheckPlatformSupport();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("user", user);
			dictionary.Add("type", type);
			dictionary.Add("name", Utilities.Hex64Encode(name));
			return await this.GetAsync<DevicePortal.WerFiles>(DevicePortal.WindowsErrorReportingFilesApi, Utilities.BuildQueryString(dictionary));
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003844 File Offset: 0x00001A44
		public async Task<byte[]> GetWindowsErrorReportingFileAsync(string user, string type, string name, string file)
		{
			this.CheckPlatformSupport();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("user", user);
			dictionary.Add("type", type);
			dictionary.Add("name", Utilities.Hex64Encode(name));
			dictionary.Add("file", Utilities.Hex64Encode(file));
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.WindowsErrorReportingFileApi, Utilities.BuildQueryString(dictionary));
			byte[] array = null;
			using (Stream stream = await this.GetAsync(uri))
			{
				array = new byte[stream.Length];
				stream.Read(array, 0, array.Length);
			}
			return array;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000038AC File Offset: 0x00001AAC
		private void CheckPlatformSupport()
		{
			DevicePortal.DevicePortalPlatforms platform = this.Platform;
			if (platform == DevicePortal.DevicePortalPlatforms.Mobile || platform == DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Windows Desktop, HoloLens and IoT platforms.");
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000038D3 File Offset: 0x00001AD3
		public DevicePortal(IDevicePortalConnection connection)
		{
			this.deviceConnection = connection;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000038ED File Offset: 0x00001AED
		public DevicePortal(string address)
		{
			this.deviceConnection = new DefaultDevicePortalConnection(address, string.Empty, string.Empty);
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000056 RID: 86 RVA: 0x00003918 File Offset: 0x00001B18
		// (remove) Token: 0x06000057 RID: 87 RVA: 0x00003950 File Offset: 0x00001B50
		public event DeviceConnectionStatusEventHandler ConnectionStatus;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003985 File Offset: 0x00001B85
		public string Address
		{
			get
			{
				return this.deviceConnection.Connection.Authority;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003997 File Offset: 0x00001B97
		// (set) Token: 0x0600005A RID: 90 RVA: 0x0000399F File Offset: 0x00001B9F
		public HttpStatusCode ConnectionHttpStatusCode { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000039A8 File Offset: 0x00001BA8
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000039B0 File Offset: 0x00001BB0
		public string ConnectionFailedDescription { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000039B9 File Offset: 0x00001BB9
		public string DeviceFamily
		{
			get
			{
				if (this.deviceConnection.Family == null)
				{
					return string.Empty;
				}
				return this.deviceConnection.Family;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000039D9 File Offset: 0x00001BD9
		public string OperatingSystemVersion
		{
			get
			{
				if (this.deviceConnection.OsInfo == null)
				{
					return string.Empty;
				}
				return this.deviceConnection.OsInfo.OsVersionString;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000039FE File Offset: 0x00001BFE
		public DevicePortal.DevicePortalPlatforms Platform
		{
			get
			{
				if (this.deviceConnection.OsInfo == null)
				{
					return DevicePortal.DevicePortalPlatforms.Unknown;
				}
				return this.deviceConnection.OsInfo.Platform;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003A1F File Offset: 0x00001C1F
		public string PlatformName
		{
			get
			{
				if (this.deviceConnection.OsInfo == null)
				{
					return "Unknown";
				}
				return this.deviceConnection.OsInfo.PlatformName;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003A44 File Offset: 0x00001C44
		public async Task ConnectAsync(string ssid = null, string ssidKey = null, bool updateConnection = false, X509Certificate2 manualCertificate = null)
		{
			this.ConnectionHttpStatusCode = HttpStatusCode.OK;
			string connectionPhaseDescription = string.Empty;
			if (manualCertificate != null)
			{
				this.SetManualCertificate(manualCertificate);
			}
			int num = 0;
			try
			{
				connectionPhaseDescription = "Requesting operating system information";
				this.SendConnectionStatus(DeviceConnectionStatus.Connecting, DeviceConnectionPhase.RequestingOperatingSystemInformation, connectionPhaseDescription);
				IDevicePortalConnection devicePortalConnection = this.deviceConnection;
				string family = await this.GetDeviceFamilyAsync().ConfigureAwait(false);
				devicePortalConnection.Family = family;
				devicePortalConnection = null;
				devicePortalConnection = this.deviceConnection;
				devicePortalConnection.OsInfo = await this.GetOperatingSystemInformationAsync().ConfigureAwait(false);
				devicePortalConnection = null;
				bool requiresHttps = this.IsUsingHttps();
				if (this.deviceConnection.OsInfo.Platform == DevicePortal.DevicePortalPlatforms.HoloLens)
				{
					connectionPhaseDescription = "Checking secure connection requirements";
					this.SendConnectionStatus(DeviceConnectionStatus.Connecting, DeviceConnectionPhase.DeterminingConnectionRequirements, connectionPhaseDescription);
					requiresHttps = await this.GetIsHttpsRequiredAsync().ConfigureAwait(false);
				}
				if (!string.IsNullOrWhiteSpace(ssid))
				{
					connectionPhaseDescription = string.Format("Connecting to {0} network", ssid);
					this.SendConnectionStatus(DeviceConnectionStatus.Connecting, DeviceConnectionPhase.ConnectingToTargetNetwork, connectionPhaseDescription);
					await this.ConnectToWifiNetworkAsync((await this.GetWifiInterfacesAsync().ConfigureAwait(false)).Interfaces[0].Guid, ssid, ssidKey).ConfigureAwait(false);
				}
				if (updateConnection)
				{
					connectionPhaseDescription = "Updating device connection";
					this.SendConnectionStatus(DeviceConnectionStatus.Connecting, DeviceConnectionPhase.UpdatingDeviceAddress, connectionPhaseDescription);
					bool preservePort = true;
					if (this.Platform == DevicePortal.DevicePortalPlatforms.HoloLens || this.Platform == DevicePortal.DevicePortalPlatforms.Mobile)
					{
						preservePort = false;
					}
					devicePortalConnection = this.deviceConnection;
					devicePortalConnection.UpdateConnection(await this.GetIpConfigAsync().ConfigureAwait(false), requiresHttps, preservePort);
					devicePortalConnection = null;
				}
				this.SendConnectionStatus(DeviceConnectionStatus.Connected, DeviceConnectionPhase.Idle, "Device connection established");
			}
			catch (Exception obj)
			{
				num = 1;
			}
			object obj;
			if (num == 1)
			{
				Exception ex = (Exception)obj;
				DevicePortalException ex2 = ex as DevicePortalException;
				if (ex2 != null)
				{
					this.ConnectionHttpStatusCode = ex2.StatusCode;
					this.ConnectionFailedDescription = ex2.Message;
				}
				else
				{
					this.ConnectionHttpStatusCode = HttpStatusCode.Conflict;
					Exception innermostException = ex;
					while (innermostException.InnerException != null)
					{
						innermostException = innermostException.InnerException;
						await Task.Yield();
					}
					this.ConnectionFailedDescription = innermostException.Message;
					innermostException = null;
				}
				this.SendConnectionStatus(DeviceConnectionStatus.Failed, DeviceConnectionPhase.Idle, string.Format("Device connection failed: {0}, {1}", connectionPhaseDescription, this.ConnectionFailedDescription));
			}
			obj = null;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003AAC File Offset: 0x00001CAC
		public async Task SaveEndpointResponseToFileAsync(string endpoint, string directory, DevicePortal.HttpMethods httpMethod, Stream requestBody = null, string requestBodyContentType = null)
		{
			Uri uri = new Uri(this.deviceConnection.Connection, endpoint);
			string text = this.OperatingSystemVersion;
			string[] array = text.Split(new char[]
			{
				'.'
			});
			if ((long)array.Length == (long)((ulong)DevicePortal.ExpectedOSVersionSections))
			{
				text = array[(int)DevicePortal.TargetOSVersionSection];
			}
			string text2 = string.Concat(new string[]
			{
				endpoint,
				"_",
				this.Platform.ToString(),
				"_",
				text
			});
			if (httpMethod != DevicePortal.HttpMethods.Get)
			{
				text2 = httpMethod.ToString() + "_" + text2;
			}
			Utilities.ModifyEndpointForFilename(ref text2);
			text2 += ".dat";
			string filepath = Path.Combine(directory, text2);
			if (DevicePortal.HttpMethods.WebSocket != httpMethod)
			{
				if (DevicePortal.HttpMethods.Put == httpMethod)
				{
					StreamContent streamContent = null;
					if (requestBody != null)
					{
						streamContent = new StreamContent(requestBody);
						streamContent.Headers.ContentType = new MediaTypeHeaderValue(requestBodyContentType);
					}
					using (Stream stream = await this.PutAsync(uri, streamContent))
					{
						using (FileStream fileStream = File.Create(filepath))
						{
							stream.Seek(0L, SeekOrigin.Begin);
							stream.CopyTo(fileStream);
						}
						return;
					}
				}
				if (DevicePortal.HttpMethods.Post == httpMethod)
				{
					using (Stream stream2 = await this.PostAsync(uri, requestBody, requestBodyContentType))
					{
						using (FileStream fileStream2 = File.Create(filepath))
						{
							stream2.Seek(0L, SeekOrigin.Begin);
							stream2.CopyTo(fileStream2);
						}
						return;
					}
				}
				if (DevicePortal.HttpMethods.Delete == httpMethod)
				{
					using (Stream stream3 = await this.DeleteAsync(uri))
					{
						using (FileStream fileStream3 = File.Create(filepath))
						{
							stream3.Seek(0L, SeekOrigin.Begin);
							stream3.CopyTo(fileStream3);
						}
						return;
					}
				}
				if (httpMethod == DevicePortal.HttpMethods.Get)
				{
					using (Stream stream4 = await this.GetAsync(uri))
					{
						using (FileStream fileStream4 = File.Create(filepath))
						{
							stream4.Seek(0L, SeekOrigin.Begin);
							stream4.CopyTo(fileStream4);
						}
						return;
					}
				}
				throw new NotImplementedException(string.Format("Unsupported HttpMethod {0}", httpMethod.ToString()));
			}
			DevicePortal.<>c__DisplayClass218_0 CS$<>8__locals1 = new DevicePortal.<>c__DisplayClass218_0();
			WebSocket<object> websocket = new WebSocket<object>(this.deviceConnection, new Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool>(this.ServerCertificateValidation), true);
			CS$<>8__locals1.streamReceived = new ManualResetEvent(false);
			CS$<>8__locals1.stream = null;
			WebSocketStreamReceivedEventInternalHandler<object> streamReceivedHandler = delegate(WebSocket<object> sender, WebSocketMessageReceivedEventArgs<Stream> args)
			{
				if (args.Message != null)
				{
					CS$<>8__locals1.stream = args.Message;
					CS$<>8__locals1.streamReceived.Set();
				}
			};
			websocket.WebSocketStreamReceived += streamReceivedHandler;
			await websocket.ConnectAsync(endpoint, null);
			await websocket.ReceiveMessagesAsync();
			CS$<>8__locals1.streamReceived.WaitOne();
			await websocket.CloseAsync();
			websocket.WebSocketStreamReceived -= streamReceivedHandler;
			using (CS$<>8__locals1.stream)
			{
				using (FileStream fileStream5 = File.Create(filepath))
				{
					CS$<>8__locals1.stream.Seek(0L, SeekOrigin.Begin);
					CS$<>8__locals1.stream.CopyTo(fileStream5);
				}
			}
			CS$<>8__locals1 = null;
			websocket = null;
			streamReceivedHandler = null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003B1B File Offset: 0x00001D1B
		private void SendConnectionStatus(DeviceConnectionStatus status, DeviceConnectionPhase phase, string message = "")
		{
			DeviceConnectionStatusEventHandler connectionStatus = this.ConnectionStatus;
			if (connectionStatus == null)
			{
				return;
			}
			connectionStatus(this, new DeviceConnectionStatusEventArgs(status, phase, message));
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003B36 File Offset: 0x00001D36
		private bool IsUsingHttps()
		{
			return this.deviceConnection.Connection.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003B54 File Offset: 0x00001D54
		public async Task<DevicePortal.HolographicServices> GetHolographicServiceState()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return await this.GetAsync<DevicePortal.HolographicServices>(DevicePortal.HolographicServicesApi, null);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003B9C File Offset: 0x00001D9C
		public async Task<float> GetInterPupilaryDistanceAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return (await this.GetAsync<DevicePortal.InterPupilaryDistance>(DevicePortal.HolographicIpdApi, null)).Ipd;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public async Task SetIsHttpsRequiredAsync(bool httpsRequired)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			await this.PostAsync(DevicePortal.HolographicWebManagementHttpSettingsApi, string.Format("required={0}", httpsRequired));
			this.deviceConnection.UpdateConnection(httpsRequired);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003C34 File Offset: 0x00001E34
		public async Task SetInterPupilaryDistanceAsync(float ipd)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("ipd={0}", (int)(ipd * 1000f));
			await this.PostAsync(DevicePortal.HolographicIpdApi, payload);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003C84 File Offset: 0x00001E84
		public async Task<bool> GetIsHttpsRequiredAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return (await this.GetAsync<DevicePortal.WebManagementHttpSettings>(DevicePortal.HolographicWebManagementHttpSettingsApi, null)).IsHttpsRequired;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003CCC File Offset: 0x00001ECC
		public async Task<string> CreatePerceptionSimulationControlStreamAsync(DevicePortal.SimulationControlStreamPriority priority)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			TaskAwaiter<bool> taskAwaiter = this.VerifySimulationControlModeAsync(DevicePortal.SimulationControlMode.Simulation).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				throw new InvalidOperationException("The simulation control mode on the target HoloLens must be 'Simulation'.");
			}
			string payload = string.Format("priority={0}", (int)priority);
			return (await this.GetAsync<DevicePortal.PerceptionSimulationControlStreamId>(DevicePortal.HolographicSimulationStreamApi, payload)).StreamId;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003D1C File Offset: 0x00001F1C
		public async Task DeletePerceptionSimulationControlStreamAsync(string streamId)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			TaskAwaiter<bool> taskAwaiter = this.VerifySimulationControlModeAsync(DevicePortal.SimulationControlMode.Simulation).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				throw new InvalidOperationException("The simulation control mode on the target HoloLens must be 'Simulation'.");
			}
			string payload = string.Format("streamId={0}", streamId);
			await this.DeleteAsync(DevicePortal.HolographicSimulationStreamApi, payload);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003D6C File Offset: 0x00001F6C
		public async Task<DevicePortal.SimulationControlMode> GetPerceptionSimulationControlModeAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return (await this.GetAsync<DevicePortal.PerceptionSimulationControlMode>(DevicePortal.HolographicSimulationModeApi, null)).Mode;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public async Task SetPerceptionSimulationControlModeAsync(DevicePortal.SimulationControlMode mode)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("mode={0}", (int)mode);
			await this.PostAsync(DevicePortal.HolographicSimulationModeApi, payload);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003E04 File Offset: 0x00002004
		private async Task<bool> VerifySimulationControlModeAsync(DevicePortal.SimulationControlMode expectedMode)
		{
			TaskAwaiter<DevicePortal.SimulationControlMode> taskAwaiter = this.GetPerceptionSimulationControlModeAsync().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<DevicePortal.SimulationControlMode> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<DevicePortal.SimulationControlMode>);
			}
			return taskAwaiter.GetResult() == expectedMode;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003E54 File Offset: 0x00002054
		public async Task<ThermalStages> GetThermalStageAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return (await this.GetAsync<DevicePortal.ThermalStage>(DevicePortal.ThermalStageApi, null)).Stage;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003E9C File Offset: 0x0000209C
		public async Task DeleteMrcFileAsync(string fileName)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			await this.DeleteAsync(DevicePortal.MrcFileApi, string.Format("filename={0}", Utilities.Hex64Encode(fileName)));
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003EEC File Offset: 0x000020EC
		public Uri GetHighResolutionMrcLiveStreamUri(bool includeHolograms = true, bool includeColorCamera = true, bool includeMicrophone = true, bool includeAudio = true)
		{
			string payload = string.Format("holo={0}&pv={1}&mic={2}&loopback={3}", new object[]
			{
				includeHolograms,
				includeColorCamera,
				includeMicrophone,
				includeAudio
			}).ToLowerInvariant();
			return Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.MrcLiveStreamHighResApi, payload);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003F4C File Offset: 0x0000214C
		public Uri GetLowResolutionMrcLiveStreamUri(bool includeHolograms = true, bool includeColorCamera = true, bool includeMicrophone = true, bool includeAudio = true)
		{
			string payload = string.Format("holo={0}&pv={1}&mic={2}&loopback={3}", new object[]
			{
				includeHolograms,
				includeColorCamera,
				includeMicrophone,
				includeAudio
			}).ToLowerInvariant();
			return Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.MrcLiveStreamLowResApi, payload);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003FAC File Offset: 0x000021AC
		public Uri GetMediumResolutionMrcLiveStreamUri(bool includeHolograms = true, bool includeColorCamera = true, bool includeMicrophone = true, bool includeAudio = true)
		{
			string payload = string.Format("holo={0}&pv={1}&mic={2}&loopback={3}", new object[]
			{
				includeHolograms,
				includeColorCamera,
				includeMicrophone,
				includeAudio
			}).ToLowerInvariant();
			return Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.MrcLiveStreamMediumResApi, payload);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000400C File Offset: 0x0000220C
		public async Task<byte[]> GetMrcFileDataAsync(string fileName, bool isThumbnailRequest = false)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			byte[] array = null;
			string path = isThumbnailRequest ? DevicePortal.MrcThumbnailApi : DevicePortal.MrcFileApi;
			string payload = string.Format("filename={0}", Utilities.Hex64Encode(fileName));
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, path, payload);
			using (Stream stream = await this.GetAsync(uri))
			{
				array = new byte[stream.Length];
				stream.Read(array, 0, array.Length);
			}
			return array;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004064 File Offset: 0x00002264
		public async Task<DevicePortal.MrcFileList> GetMrcFileListAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			DevicePortal.MrcFileList mrcFileList2 = await this.GetAsync<DevicePortal.MrcFileList>(DevicePortal.MrcFileListApi, null);
			DevicePortal.MrcFileList mrcFileList = mrcFileList2;
			foreach (DevicePortal.MrcFileInformation mrcFileInformation in mrcFileList.Files)
			{
				try
				{
					DevicePortal.MrcFileInformation mrcFileInformation2 = mrcFileInformation;
					mrcFileInformation2.Thumbnail = await this.GetMrcThumbnailDataAsync(mrcFileInformation.FileName);
					mrcFileInformation2 = null;
				}
				catch
				{
				}
			}
			List<DevicePortal.MrcFileInformation>.Enumerator enumerator = default(List<DevicePortal.MrcFileInformation>.Enumerator);
			return mrcFileList;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000040AC File Offset: 0x000022AC
		public Uri GetMrcLiveStreamUri(bool includeHolograms = true, bool includeColorCamera = true, bool includeMicrophone = true, bool includeAudio = true)
		{
			string payload = string.Format("holo={0}&pv={1}&mic={2}&loopback={3}", new object[]
			{
				includeHolograms,
				includeColorCamera,
				includeMicrophone,
				includeAudio
			}).ToLowerInvariant();
			return Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.MrcLiveStreamApi, payload);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000410C File Offset: 0x0000230C
		public async Task<DevicePortal.MrcSettings> GetMrcSettingsAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return await this.GetAsync<DevicePortal.MrcSettings>(DevicePortal.MrcSettingsApi, null);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004154 File Offset: 0x00002354
		public async Task<DevicePortal.MrcStatus> GetMrcStatusAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return await this.GetAsync<DevicePortal.MrcStatus>(DevicePortal.MrcStatusApi, null);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000419C File Offset: 0x0000239C
		public async Task<byte[]> GetMrcThumbnailDataAsync(string fileName)
		{
			return await this.GetMrcFileDataAsync(fileName, true);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000041EC File Offset: 0x000023EC
		public async Task SetMrcSettingsAsync(DevicePortal.MrcSettings settings)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("holo={0}&pv={1}&mic={2}&appAudio={3}&vstabbuffer={4}", new object[]
			{
				settings.IncludeHolograms.ToString().ToLower(),
				settings.IncludeColorCamera.ToString().ToLower(),
				settings.IncludeMicrophone.ToString().ToLower(),
				settings.IncludeAudio.ToString().ToLower(),
				settings.VideoStabilizationBuffer
			});
			await this.PostAsync(DevicePortal.MrcSettingsApi, payload);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000423C File Offset: 0x0000243C
		public async Task StartMrcRecordingAsync(bool includeHolograms = true, bool includeColorCamera = true, bool includeMicrophone = true, bool includeAudio = true)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("holo={0}&pv={1}&mic={2}&loopback={3}", new object[]
			{
				includeHolograms,
				includeColorCamera,
				includeMicrophone,
				includeAudio
			}).ToLower();
			await this.PostAsync(DevicePortal.MrcStartRecordingApi, payload);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000042A4 File Offset: 0x000024A4
		public async Task StopMrcRecordingAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			await this.PostAsync(DevicePortal.MrcStopRecordingApi, null);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000042EC File Offset: 0x000024EC
		public async Task TakeMrcPhotoAsync(bool includeHolograms = true, bool includeColorCamera = true)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			await this.PostAsync(DevicePortal.MrcPhotoApi, string.Format("holo={0}&pv={1}", includeHolograms, includeColorCamera).ToLower());
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004344 File Offset: 0x00002544
		public async Task DeleteHolographicSimulationRecordingAsync(string name)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("recording={0}", name);
			await this.DeleteAsync(DevicePortal.HolographicSimulationPlaybackFileApi, payload);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004394 File Offset: 0x00002594
		public async Task<DevicePortal.HolographicSimulationPlaybackFiles> GetHolographicSimulationPlaybackFilesAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return await this.GetHolographicSimulationPlaybackFilesPrivateAsync(false);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000043DC File Offset: 0x000025DC
		public async Task<DevicePortal.HolographicSimulationPlaybackFiles> GetHolographicSimulationPlaybackSessionFilesAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return await this.GetHolographicSimulationPlaybackFilesPrivateAsync(true);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004424 File Offset: 0x00002624
		public async Task<DevicePortal.HolographicSimulationDataTypes> GetHolographicSimulationPlaybackSessionDataTypesAsync(string recordingName)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("recording={0}", recordingName);
			return await this.GetAsync<DevicePortal.HolographicSimulationDataTypes>(DevicePortal.HolographicSimulationPlaybackDataTypesApi, payload);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004474 File Offset: 0x00002674
		public async Task<DevicePortal.HolographicSimulationPlaybackStates> GetHolographicSimulationPlaybackStateAsync(string name)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			DevicePortal.HolographicSimulationPlaybackStates playbackState = DevicePortal.HolographicSimulationPlaybackStates.Unexpected;
			string payload = string.Format("recording={0}", name);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.HolographicSimulationPlaybackStateApi, payload);
			using (Stream stream = await this.GetAsync(uri))
			{
				if (stream != null && stream.Length != 0L)
				{
					try
					{
						playbackState = ((DevicePortal.HolographicSimulationPlaybackSessionState)new DataContractJsonSerializer(typeof(DevicePortal.HolographicSimulationPlaybackSessionState)).ReadObject(stream)).State;
					}
					catch
					{
						stream.Position = 0L;
						throw new InvalidOperationException(((DevicePortal.HolographicSimulationError)new DataContractJsonSerializer(typeof(DevicePortal.HolographicSimulationError)).ReadObject(stream)).Reason);
					}
				}
			}
			return playbackState;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000044C4 File Offset: 0x000026C4
		public async Task LoadHolographicSimulationRecordingAsync(string recordingName)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("recording={0}", recordingName);
			await this.PostAsync(DevicePortal.HolographicSimulationPlaybackSessionFileApi, payload);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004514 File Offset: 0x00002714
		public async Task PauseHolographicSimulationRecordingAsync(string recordingName)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("recording={0}", recordingName);
			await this.PostAsync(DevicePortal.HolographicSimulationPlaybackPauseApi, payload);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004564 File Offset: 0x00002764
		public async Task PlayHolographicSimulationRecordingAsync(string recordingName)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("recording={0}", recordingName);
			await this.PostAsync(DevicePortal.HolographicSimulationPlaybackPlayApi, payload);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000045B4 File Offset: 0x000027B4
		public async Task StopHolographicSimulationRecordingAsync(string recordingName)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("recording={0}", recordingName);
			await this.PostAsync(DevicePortal.HolographicSimulationPlaybackStopApi, payload);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004604 File Offset: 0x00002804
		public async Task UnloadHolographicSimulationRecordingAsync(string recordingName)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("recording={0}", recordingName);
			await this.DeleteAsync(DevicePortal.HolographicSimulationPlaybackSessionFileApi, payload);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004654 File Offset: 0x00002854
		private async Task<DevicePortal.HolographicSimulationPlaybackFiles> GetHolographicSimulationPlaybackFilesPrivateAsync(bool session)
		{
			string apiPath = session ? DevicePortal.HolographicSimulationPlaybackSessionFilesApi : DevicePortal.HolographicSimulationPlaybackFilesApi;
			return await this.GetAsync<DevicePortal.HolographicSimulationPlaybackFiles>(apiPath, null);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000046A4 File Offset: 0x000028A4
		public async Task<bool> GetHolographicSimulationRecordingStatusAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			return (await this.GetAsync<DevicePortal.HolographicSimulationRecordingStatus>(DevicePortal.HolographicSimulationRecordingStatusApi, null)).IsRecording;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000046EC File Offset: 0x000028EC
		public async Task StartHolographicSimulationRecordingAsync(string name, bool recordHead = true, bool recordHands = true, bool recordSpatialMapping = true, bool recordEnvironment = true)
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			string payload = string.Format("head={0}&hands={1}&spatialMapping={2}&environment={3}&name={4}", new object[]
			{
				recordHead ? 1 : 0,
				recordHands ? 1 : 0,
				recordSpatialMapping ? 1 : 0,
				recordEnvironment ? 1 : 0,
				name
			});
			await this.PostAsync(DevicePortal.StartHolographicSimulationRecordingApi, payload);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000475C File Offset: 0x0000295C
		public async Task<byte[]> StopHolographicSimulationRecordingAsync()
		{
			if (!Utilities.IsHoloLens(this.Platform, this.DeviceFamily))
			{
				throw new NotSupportedException("This method is only supported on HoloLens.");
			}
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.StopHolographicSimulationRecordingApi, null);
			byte[] dataBytes = null;
			using (Stream stream = await this.GetAsync(uri))
			{
				if (stream != null && stream.Length != 0L)
				{
					DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(DevicePortal.HolographicSimulationError));
					DevicePortal.HolographicSimulationError holographicSimulationError = null;
					try
					{
						holographicSimulationError = (DevicePortal.HolographicSimulationError)dataContractJsonSerializer.ReadObject(stream);
					}
					catch
					{
					}
					if (holographicSimulationError != null)
					{
						throw new InvalidOperationException(holographicSimulationError.Reason);
					}
					dataBytes = new byte[stream.Length];
					stream.Read(dataBytes, 0, dataBytes.Length);
				}
			}
			return dataBytes;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000047A4 File Offset: 0x000029A4
		private static void CopyFileToRequestStream(FileInfo file, Stream stream)
		{
			string s = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n", file.Name, file.Name);
			string s2 = "Content-Type: application/octet-stream\r\n\r\n";
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			stream.Write(bytes, 0, bytes.Length);
			bytes = Encoding.ASCII.GetBytes(s2);
			stream.Write(bytes, 0, bytes.Length);
			using (FileStream fileStream = File.OpenRead(file.FullName))
			{
				fileStream.CopyTo(stream);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000482C File Offset: 0x00002A2C
		private void ApplyCSRFHeader(HttpClient client, DevicePortal.HttpMethods method)
		{
			string name = "X-" + DevicePortal.CsrfTokenName;
			string text = this.csrfToken;
			if (string.Compare(method.ToString(), "get", StringComparison.OrdinalIgnoreCase) == 0)
			{
				name = DevicePortal.CsrfTokenName;
				text = (string.IsNullOrEmpty(this.csrfToken) ? "Fetch" : text);
			}
			client.DefaultRequestHeaders.Add(name, text);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004893 File Offset: 0x00002A93
		private void ApplyHttpHeaders(HttpClient client, DevicePortal.HttpMethods method)
		{
			this.ApplyUserAgentHeader(client);
			this.ApplyCSRFHeader(client, method);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000048A4 File Offset: 0x00002AA4
		private void ApplyUserAgentHeader(HttpClient client)
		{
			string text = DevicePortal.UserAgentValue;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			text = text + "-v" + executingAssembly.GetName().Version.ToString();
			text += "-dotnet";
			client.DefaultRequestHeaders.Add(DevicePortal.UserAgentName, text);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000048F8 File Offset: 0x00002AF8
		private void RetrieveCsrfToken(HttpResponseMessage response)
		{
			IEnumerable<string> enumerable;
			if (response.Headers.TryGetValues("Set-Cookie", out enumerable))
			{
				foreach (string text in enumerable)
				{
					string text2 = DevicePortal.CsrfTokenName + "=";
					if (text.StartsWith(text2, StringComparison.Ordinal))
					{
						this.csrfToken = text.Substring(text2.Length);
					}
				}
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000497C File Offset: 0x00002B7C
		public async Task DeleteAsync(string apiPath, string payload = null)
		{
			await this.DeleteAsync<DevicePortal.NullResponse>(apiPath, payload);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000049D4 File Offset: 0x00002BD4
		public async Task<T> DeleteAsync<T>(string apiPath, string payload = null) where T : new()
		{
			T data = default(T);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, apiPath, payload);
			DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
			using (Stream stream = await this.DeleteAsync(uri))
			{
				if (stream != null && stream.Length != 0L)
				{
					DevicePortal.JsonFormatCheck<T>(stream);
					data = (T)((object)deserializer.ReadObject(stream));
				}
			}
			return data;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004A2C File Offset: 0x00002C2C
		private static void JsonFormatCheck<T>(Stream jsonStream)
		{
			if (typeof(T) == typeof(DevicePortal.SystemPerformanceInformation))
			{
				string text = new StreamReader(jsonStream).ReadToEnd();
				if (text.StartsWith(DevicePortal.SysPerfInfoErrorPrefix, StringComparison.OrdinalIgnoreCase) && text.EndsWith(DevicePortal.SysPerfInfoErrorPostfix, StringComparison.OrdinalIgnoreCase))
				{
					text = text.Substring(DevicePortal.SysPerfInfoErrorPrefix.Length, text.Length - DevicePortal.SysPerfInfoErrorPrefix.Length - DevicePortal.SysPerfInfoErrorPostfix.Length);
					text = Regex.Replace(text, "\\\\\"", "\"", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
					text = Regex.Replace(text, "\\\\\\\\", "\\", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
					jsonStream.SetLength(0L);
					StreamWriter streamWriter = new StreamWriter(jsonStream);
					streamWriter.Write(text);
					streamWriter.Flush();
				}
				jsonStream.Seek(0L, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004B00 File Offset: 0x00002D00
		public async Task<T> GetAsync<T>(string apiPath, string payload = null) where T : new()
		{
			T data = default(T);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, apiPath, payload);
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
			using (Stream stream = await this.GetAsync(uri).ConfigureAwait(false))
			{
				if (stream != null && stream.Length != 0L)
				{
					DevicePortal.JsonFormatCheck<T>(stream);
					data = (T)((object)serializer.ReadObject(stream));
				}
			}
			return data;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004B58 File Offset: 0x00002D58
		public async Task PostAsync(string apiPath, List<string> files, string payload = null)
		{
			string arg = Guid.NewGuid().ToString();
			using (MemoryStream dataStream = new MemoryStream())
			{
				byte[] bytes;
				foreach (string fileName in files)
				{
					FileInfo file = new FileInfo(fileName);
					bytes = Encoding.ASCII.GetBytes(string.Format("\r\n--{0}\r\n", arg));
					dataStream.Write(bytes, 0, bytes.Length);
					DevicePortal.CopyFileToRequestStream(file, dataStream);
				}
				bytes = Encoding.ASCII.GetBytes(string.Format("\r\n--{0}--\r\n", arg));
				dataStream.Write(bytes, 0, bytes.Length);
				dataStream.Position = 0L;
				string requestStreamContentType = string.Format("multipart/form-data; boundary={0}", arg);
				await this.PostAsync<DevicePortal.NullResponse>(apiPath, payload, dataStream, requestStreamContentType);
			}
			MemoryStream dataStream = null;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004BB8 File Offset: 0x00002DB8
		public async Task PostAsync(string apiPath, string payload = null)
		{
			await this.PostAsync<DevicePortal.NullResponse>(apiPath, payload, null, null);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004C10 File Offset: 0x00002E10
		public async Task<T> PostAsync<T>(string apiPath, string payload = null, Stream requestStream = null, string requestStreamContentType = null) where T : new()
		{
			T data = default(T);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, apiPath, payload);
			DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
			using (Stream stream = await this.PostAsync(uri, requestStream, requestStreamContentType))
			{
				if (stream != null && stream.Length != 0L)
				{
					DevicePortal.JsonFormatCheck<T>(stream);
					data = (T)((object)deserializer.ReadObject(stream));
				}
			}
			return data;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004C78 File Offset: 0x00002E78
		public async Task PutAsync<K>(string apiPath, K bodyData, string payload = null) where K : class
		{
			await this.PutAsync<DevicePortal.NullResponse, K>(apiPath, bodyData, payload);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004CD8 File Offset: 0x00002ED8
		public async Task<T> PutAsync<T, K>(string apiPath, K bodyData = default(K), string payload = null) where T : new() where K : class
		{
			T data = default(T);
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, apiPath, payload);
			StreamContent streamContent = null;
			if (bodyData != null)
			{
				XmlObjectSerializer xmlObjectSerializer = new DataContractJsonSerializer(typeof(K));
				Stream stream = new MemoryStream();
				xmlObjectSerializer.WriteObject(stream, bodyData);
				stream.Seek(0L, SeekOrigin.Begin);
				streamContent = new StreamContent(stream);
				streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			}
			DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
			using (Stream stream2 = await this.PutAsync(uri, streamContent))
			{
				if (stream2 != null && stream2.Length != 0L)
				{
					DevicePortal.JsonFormatCheck<T>(stream2);
					data = (T)((object)deserializer.ReadObject(stream2));
				}
			}
			return data;
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600009A RID: 154 RVA: 0x00004D38 File Offset: 0x00002F38
		// (remove) Token: 0x0600009B RID: 155 RVA: 0x00004D70 File Offset: 0x00002F70
		public event WebSocketMessageReceivedEventHandler<DevicePortal.AvailableBluetoothDevicesInfo> BluetoothDeviceListReceived;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600009C RID: 156 RVA: 0x00004DA8 File Offset: 0x00002FA8
		// (remove) Token: 0x0600009D RID: 157 RVA: 0x00004DE0 File Offset: 0x00002FE0
		public event WebSocketMessageReceivedEventHandler<DevicePortal.PairedBluetoothDevicesInfo> PairedBluetoothDeviceListReceived;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600009E RID: 158 RVA: 0x00004E18 File Offset: 0x00003018
		// (remove) Token: 0x0600009F RID: 159 RVA: 0x00004E50 File Offset: 0x00003050
		public event WebSocketMessageReceivedEventHandler<DevicePortal.PairBluetoothDevicesInfo> PairBluetoothDeviceListReceived;

		// Token: 0x060000A0 RID: 160 RVA: 0x00004E88 File Offset: 0x00003088
		public async Task<DevicePortal.AvailableBluetoothDevicesInfo> GetAvailableBluetoothDevicesInfoAsync()
		{
			DevicePortal.AvailableBluetoothDevicesInfo bluetooth = null;
			ManualResetEvent bluetoothReceived = new ManualResetEvent(false);
			WebSocketMessageReceivedEventHandler<DevicePortal.AvailableBluetoothDevicesInfo> bluetoothReceivedHandler = delegate(DevicePortal sender, WebSocketMessageReceivedEventArgs<DevicePortal.AvailableBluetoothDevicesInfo> bluetoothArgs)
			{
				if (bluetoothArgs.Message != null)
				{
					bluetooth = bluetoothArgs.Message;
					bluetoothReceived.Set();
				}
			};
			this.BluetoothDeviceListReceived += bluetoothReceivedHandler;
			await this.StartListeningForBluetoothAsync(DevicePortal.AvailableBluetoothDevicesApi);
			bluetoothReceived.WaitOne();
			await this.StopListeningForBluetoothAsync();
			this.BluetoothDeviceListReceived -= bluetoothReceivedHandler;
			return bluetooth;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004ED0 File Offset: 0x000030D0
		public async Task<DevicePortal.PairedBluetoothDevicesInfo> GetPairedBluetoothDevicesInfoAsync()
		{
			DevicePortal.PairedBluetoothDevicesInfo bluetooth = null;
			ManualResetEvent pairedBluetoothReceived = new ManualResetEvent(false);
			WebSocketMessageReceivedEventHandler<DevicePortal.PairedBluetoothDevicesInfo> pairedBluetoothReceivedHandler = delegate(DevicePortal sender, WebSocketMessageReceivedEventArgs<DevicePortal.PairedBluetoothDevicesInfo> bluetoothArgs)
			{
				if (bluetoothArgs.Message != null)
				{
					bluetooth = bluetoothArgs.Message;
					pairedBluetoothReceived.Set();
				}
			};
			this.PairedBluetoothDeviceListReceived += pairedBluetoothReceivedHandler;
			await this.StartListeningForPairedBluetoothAsync(DevicePortal.PairedBluetoothDevicesApi);
			pairedBluetoothReceived.WaitOne();
			await this.StopListeningForPairedBluetoothAsync();
			this.PairedBluetoothDeviceListReceived -= pairedBluetoothReceivedHandler;
			return bluetooth;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004F18 File Offset: 0x00003118
		public async Task<DevicePortal.PairBluetoothDevicesInfo> GetPairBluetoothDevicesInfoAsync(string deviceId)
		{
			DevicePortal.PairBluetoothDevicesInfo bluetooth = null;
			ManualResetEvent pairBluetoothReceived = new ManualResetEvent(false);
			WebSocketMessageReceivedEventHandler<DevicePortal.PairBluetoothDevicesInfo> pairBluetoothReceivedHandler = delegate(DevicePortal sender, WebSocketMessageReceivedEventArgs<DevicePortal.PairBluetoothDevicesInfo> bluetoothArgs)
			{
				if (bluetoothArgs.Message != null)
				{
					bluetooth = bluetoothArgs.Message;
					pairBluetoothReceived.Set();
				}
			};
			this.PairBluetoothDeviceListReceived += pairBluetoothReceivedHandler;
			await this.StartListeningForPairBluetoothAsync(DevicePortal.PairBluetoothDevicesApi, string.Format("deviceId={0}", Utilities.Hex64Encode(deviceId)));
			pairBluetoothReceived.WaitOne();
			await this.StopListeningForPairBluetoothAsync();
			this.PairBluetoothDeviceListReceived -= pairBluetoothReceivedHandler;
			return bluetooth;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004F68 File Offset: 0x00003168
		public async Task StartListeningForBluetoothAsync(string bluetoothApi)
		{
			if (this.bluetoothWebSocket == null)
			{
				this.bluetoothWebSocket = new WebSocket<DevicePortal.AvailableBluetoothDevicesInfo>(this.deviceConnection, new Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool>(this.ServerCertificateValidation), false);
				this.bluetoothWebSocket.WebSocketMessageReceived += this.BluetoothReceivedHandler;
			}
			else if (this.bluetoothWebSocket.IsListeningForMessages)
			{
				return;
			}
			await this.bluetoothWebSocket.ConnectAsync(bluetoothApi, null);
			await this.bluetoothWebSocket.ReceiveMessagesAsync();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004FB8 File Offset: 0x000031B8
		public async Task StopListeningForBluetoothAsync()
		{
			await this.bluetoothWebSocket.CloseAsync();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005000 File Offset: 0x00003200
		public async Task StartListeningForPairedBluetoothAsync(string bluetoothApi)
		{
			if (this.pairedBluetoothWebSocket == null)
			{
				this.pairedBluetoothWebSocket = new WebSocket<DevicePortal.PairedBluetoothDevicesInfo>(this.deviceConnection, new Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool>(this.ServerCertificateValidation), false);
				this.pairedBluetoothWebSocket.WebSocketMessageReceived += this.PairedBluetoothReceivedHandler;
			}
			else if (this.pairedBluetoothWebSocket.IsListeningForMessages)
			{
				return;
			}
			await this.pairedBluetoothWebSocket.ConnectAsync(bluetoothApi, null);
			await this.pairedBluetoothWebSocket.ReceiveMessagesAsync();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005050 File Offset: 0x00003250
		public async Task StopListeningForPairedBluetoothAsync()
		{
			if (this.pairedBluetoothWebSocket != null && this.pairedBluetoothWebSocket.IsListeningForMessages)
			{
				await this.pairedBluetoothWebSocket.CloseAsync();
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005098 File Offset: 0x00003298
		public async Task StartListeningForPairBluetoothAsync(string bluetoothApi, string payload)
		{
			if (this.pairBluetoothWebSocket == null)
			{
				this.pairBluetoothWebSocket = new WebSocket<DevicePortal.PairBluetoothDevicesInfo>(this.deviceConnection, new Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool>(this.ServerCertificateValidation), false);
				this.pairBluetoothWebSocket.WebSocketMessageReceived += this.PairBluetoothReceivedHandler;
			}
			else if (this.pairBluetoothWebSocket.IsListeningForMessages)
			{
				return;
			}
			await this.pairBluetoothWebSocket.ConnectAsync(bluetoothApi, payload);
			await this.pairBluetoothWebSocket.ReceiveMessagesAsync();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000050F0 File Offset: 0x000032F0
		public async Task StopListeningForPairBluetoothAsync()
		{
			await this.pairBluetoothWebSocket.CloseAsync();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005138 File Offset: 0x00003338
		public async Task<DevicePortal.ErrorInformation> UnPairBluetoothDeviceAsync(string deviceId)
		{
			return await this.PostAsync<DevicePortal.ErrorInformation>(DevicePortal.UnpairBluetoothDevicesApi, string.Format("deviceId={0}", Utilities.Hex64Encode(deviceId)), null, null);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005185 File Offset: 0x00003385
		private void BluetoothReceivedHandler(WebSocket<DevicePortal.AvailableBluetoothDevicesInfo> sender, WebSocketMessageReceivedEventArgs<DevicePortal.AvailableBluetoothDevicesInfo> args)
		{
			if (args.Message != null)
			{
				WebSocketMessageReceivedEventHandler<DevicePortal.AvailableBluetoothDevicesInfo> bluetoothDeviceListReceived = this.BluetoothDeviceListReceived;
				if (bluetoothDeviceListReceived == null)
				{
					return;
				}
				bluetoothDeviceListReceived(this, args);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000051A1 File Offset: 0x000033A1
		private void PairedBluetoothReceivedHandler(WebSocket<DevicePortal.PairedBluetoothDevicesInfo> sender, WebSocketMessageReceivedEventArgs<DevicePortal.PairedBluetoothDevicesInfo> args)
		{
			if (args.Message != null)
			{
				WebSocketMessageReceivedEventHandler<DevicePortal.PairedBluetoothDevicesInfo> pairedBluetoothDeviceListReceived = this.PairedBluetoothDeviceListReceived;
				if (pairedBluetoothDeviceListReceived == null)
				{
					return;
				}
				pairedBluetoothDeviceListReceived(this, args);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000051BD File Offset: 0x000033BD
		private void PairBluetoothReceivedHandler(WebSocket<DevicePortal.PairBluetoothDevicesInfo> sender, WebSocketMessageReceivedEventArgs<DevicePortal.PairBluetoothDevicesInfo> args)
		{
			if (args.Message != null)
			{
				WebSocketMessageReceivedEventHandler<DevicePortal.PairBluetoothDevicesInfo> pairBluetoothDeviceListReceived = this.PairBluetoothDeviceListReceived;
				if (pairBluetoothDeviceListReceived == null)
				{
					return;
				}
				pairBluetoothDeviceListReceived(this, args);
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000051DC File Offset: 0x000033DC
		public async Task<DevicePortal.TpmSettingsInfo> GetTpmSettingsInfoAsync()
		{
			return await this.GetAsync<DevicePortal.TpmSettingsInfo>(DevicePortal.TpmSettingsApi, null);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005224 File Offset: 0x00003424
		public async Task SetTpmAcpiTablesInfoAsync(string acpiTableIndex)
		{
			await this.PostAsync(DevicePortal.TpmAcpiTablesApi, string.Format("AcpiTableIndex={0}", Utilities.Hex64Encode(acpiTableIndex)));
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005274 File Offset: 0x00003474
		public async Task<DevicePortal.TpmAcpiTablesInfo> GetTpmAcpiTablesInfoAsync()
		{
			return await this.GetAsync<DevicePortal.TpmAcpiTablesInfo>(DevicePortal.TpmAcpiTablesApi, null);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000052BC File Offset: 0x000034BC
		public async Task<DevicePortal.TpmLogicalDeviceSettingsInfo> GetTpmLogicalDeviceSettingsInfoAsync(int logicalDeviceId)
		{
			return await this.GetAsync<DevicePortal.TpmLogicalDeviceSettingsInfo>(string.Format("{0}/{1}", DevicePortal.TpmSettingsApi, logicalDeviceId), null);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000530C File Offset: 0x0000350C
		public async Task SetTpmLogicalDeviceSettingsInfoAsync(int logicalDeviceId, string azureUri, string azureKey)
		{
			await this.PostAsync(string.Format("{0}/{1}", DevicePortal.TpmSettingsApi, logicalDeviceId), string.Format("AzureUri={0}&AzureKey={1}", Utilities.Hex64Encode(azureUri), Utilities.Hex64Encode(azureKey)));
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000536C File Offset: 0x0000356C
		public async Task ResetTpmLogicalDeviceSettingsInfoAsync(int logicalDeviceId)
		{
			await this.DeleteAsync(string.Format("{0}/{1}", DevicePortal.TpmSettingsApi, logicalDeviceId), null);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000053BC File Offset: 0x000035BC
		public async Task<DevicePortal.TpmAzureTokenInfo> GetTpmAzureTokenInfoAsync(int logicalDeviceId, string validity)
		{
			return await this.GetAsync<DevicePortal.TpmAzureTokenInfo>(string.Format("{0}/{1}", DevicePortal.TpmAzureTokenApi, logicalDeviceId), string.Format("validity={0}", Utilities.Hex64Encode(validity)));
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005414 File Offset: 0x00003614
		public async Task<DevicePortal.AppsListInfo> GetAppsListInfoAsync()
		{
			return await this.GetAsync<DevicePortal.AppsListInfo>(DevicePortal.AppsListApi, null);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000545C File Offset: 0x0000365C
		public async Task<DevicePortal.HeadlessAppsListInfo> GetHeadlessAppsListInfoAsync()
		{
			return await this.GetAsync<DevicePortal.HeadlessAppsListInfo>(DevicePortal.HeadlessAppsListApi, null);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000054A4 File Offset: 0x000036A4
		public async Task UpdateStartupAppAsync(string appId)
		{
			await this.PostAsync(DevicePortal.AppsListApi, string.Format("appid={0}", Utilities.Hex64Encode(appId)));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000054F4 File Offset: 0x000036F4
		public async Task UpdateHeadlessStartupAppAsync(string appId)
		{
			await this.PostAsync(DevicePortal.HeadlessStartupAppApi, string.Format("appid={0}", Utilities.Hex64Encode(appId)));
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005544 File Offset: 0x00003744
		public async Task RemoveHeadlessStartupAppAsync(string appId)
		{
			await this.DeleteAsync(DevicePortal.HeadlessStartupAppApi, string.Format("appid={0}", Utilities.Hex64Encode(appId)));
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005594 File Offset: 0x00003794
		public async Task ActivatePackageAsync(string appId)
		{
			await this.PostAsync(DevicePortal.ActivatePackageApi, string.Format("appid={0}", Utilities.Hex64Encode(appId)));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000055E4 File Offset: 0x000037E4
		public async Task<DevicePortal.AudioDeviceListInfo> GetAudioDeviceListInfoAsync()
		{
			return await this.GetAsync<DevicePortal.AudioDeviceListInfo>(DevicePortal.AudioDeviceListApi, null);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000562C File Offset: 0x0000382C
		public async Task SetRenderVolumeAsync(string renderVolume)
		{
			await this.PostAsync(DevicePortal.SetRenderVolumeApi, string.Format("rendervolume={0}", Utilities.Hex64Encode(renderVolume)));
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000567C File Offset: 0x0000387C
		public async Task SetCaptureVolumeAsync(string captureVolume)
		{
			await this.PostAsync(DevicePortal.SetCaptureVolumeApi, string.Format("capturevolume={0}", Utilities.Hex64Encode(captureVolume)));
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000056CC File Offset: 0x000038CC
		public async Task RunCommandAsync(string command, string runAsDefaultAccount)
		{
			await this.PostAsync(DevicePortal.RunCommandApi, string.Format("command={0}&runasdefaultaccount={1}", Utilities.Hex64Encode(command), Utilities.Hex64Encode(runAsDefaultAccount)));
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005724 File Offset: 0x00003924
		public async Task<DevicePortal.RunCommandOutputInfo> RunCommandWithoutOutputAsync(string commandWithoutOutput, string runAsDefaultAccount, string timeout)
		{
			return await this.PostAsync<DevicePortal.RunCommandOutputInfo>(DevicePortal.RunCommandWithoutOutputApi, string.Format("command={0}&runasdefaultaccount={1}&timeout={2}", Utilities.Hex64Encode(commandWithoutOutput), Utilities.Hex64Encode(runAsDefaultAccount), Utilities.Hex64Encode(timeout)), null, null);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005784 File Offset: 0x00003984
		public async Task<DevicePortal.IscInterfacesInfo> GetIcsInterfacesInfoAsync()
		{
			return await this.GetAsync<DevicePortal.IscInterfacesInfo>(DevicePortal.IcsInterfacesApi, null);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000057CC File Offset: 0x000039CC
		public async Task IcSharingStartAsync(string privateInterface, string publicInterface)
		{
			await this.PostAsync(DevicePortal.IcSharingApi, string.Format("PrivateInterface={0}&PublicInterface={1}", Utilities.Hex64Encode(privateInterface), Utilities.Hex64Encode(publicInterface)));
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005824 File Offset: 0x00003A24
		public async Task IcSharingStopAsync(string privateInterface, string publicInterface)
		{
			await this.DeleteAsync(DevicePortal.IcSharingApi, string.Format("PrivateInterface={0}&PublicInterface={1}", Utilities.Hex64Encode(privateInterface), Utilities.Hex64Encode(publicInterface)));
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000587C File Offset: 0x00003A7C
		public async Task<DevicePortal.SoftAPSettingsInfo> GetSoftAPSettingsInfoAsync()
		{
			return await this.GetAsync<DevicePortal.SoftAPSettingsInfo>(DevicePortal.SoftAPSettingsApi, null);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000058C4 File Offset: 0x00003AC4
		public async Task<DevicePortal.AllJoynSettingsInfo> GetAllJoynSettingsInfoAsync()
		{
			return await this.GetAsync<DevicePortal.AllJoynSettingsInfo>(DevicePortal.AllJoynSettingsApi, null);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000590C File Offset: 0x00003B0C
		public async Task SetSoftApSettingsAsync(string softApStatus, string softApSsid, string softApPassword)
		{
			await this.PostAsync(DevicePortal.SoftAPSettingsApi, string.Format("SoftApEnabled={0}&SoftApSsid={1}&SoftApPassword={2}", Utilities.Hex64Encode(softApStatus), Utilities.Hex64Encode(softApSsid), Utilities.Hex64Encode(softApPassword)));
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000596C File Offset: 0x00003B6C
		public async Task SetAllJoynSettingsAsync(string allJoynStatus, string allJoynDescription, string allJoynManufacturer, string allJoynModelNumber)
		{
			await this.PostAsync(DevicePortal.AllJoynSettingsApi, string.Format("AllJoynOnboardingEnabled={0}&AllJoynOnboardingDefaultDescription={1}&AllJoynOnboardingDefaultManufacturer={2}&AllJoynOnboardingModelNumber={3}", new object[]
			{
				Utilities.Hex64Encode(allJoynStatus),
				Utilities.Hex64Encode(allJoynDescription),
				Utilities.Hex64Encode(allJoynManufacturer),
				Utilities.Hex64Encode(allJoynModelNumber)
			}));
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000059D4 File Offset: 0x00003BD4
		public async Task<DevicePortal.RemoteSettingsStatusInfo> GetRemoteSettingsStatusInfoAsync()
		{
			return await this.GetAsync<DevicePortal.RemoteSettingsStatusInfo>(DevicePortal.RemoteSettingsStatusApi, null);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005A1C File Offset: 0x00003C1C
		public async Task<DevicePortal.RemoteSettingsStatusInfo> RemoteSettingsEnableAsync()
		{
			return await this.PostAsync<DevicePortal.RemoteSettingsStatusInfo>(DevicePortal.RemoteSettingsEnableApi, null, null, null);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005A64 File Offset: 0x00003C64
		public async Task<DevicePortal.RemoteSettingsStatusInfo> RemoteSettingsDisableAsync()
		{
			return await this.PostAsync<DevicePortal.RemoteSettingsStatusInfo>(DevicePortal.RemoteSettingsDisableApi, null, null, null);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005AAC File Offset: 0x00003CAC
		public async Task<DevicePortal.IoTOSInfo> GetIoTOSInfoAsync()
		{
			return await this.GetAsync<DevicePortal.IoTOSInfo>(DevicePortal.IoTOsInfoApi, null);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005AF4 File Offset: 0x00003CF4
		public async Task<DevicePortal.TimezoneInfo> GetTimezoneInfoAsync()
		{
			return await this.GetAsync<DevicePortal.TimezoneInfo>(DevicePortal.TimezoneInfoApi, null);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005B3C File Offset: 0x00003D3C
		public async Task<DevicePortal.DateTimeInfo> GetDateTimeInfoAsync()
		{
			return await this.GetAsync<DevicePortal.DateTimeInfo>(DevicePortal.DateTimeInfoApi, null);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005B84 File Offset: 0x00003D84
		public async Task<DevicePortal.ControllerDriverInfo> GetControllerDriverInfoAsync()
		{
			return await this.GetAsync<DevicePortal.ControllerDriverInfo>(DevicePortal.ControllerDriverApi, null);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005BCC File Offset: 0x00003DCC
		public async Task<DevicePortal.DisplayOrientationInfo> GetDisplayOrientationInfoAsync()
		{
			return await this.GetAsync<DevicePortal.DisplayOrientationInfo>(DevicePortal.DisplayOrientationApi, null);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005C14 File Offset: 0x00003E14
		public async Task<DevicePortal.DisplayResolutionInfo> GetDisplayResolutionInfoAsync()
		{
			return await this.GetAsync<DevicePortal.DisplayResolutionInfo>(DevicePortal.DisplayResolutionApi, null);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005C5C File Offset: 0x00003E5C
		public async Task SetIoTDeviceNameAsync(string name)
		{
			await this.PostAsync(DevicePortal.DeviceNameApi, string.Format("newdevicename={0}", Utilities.Hex64Encode(name)));
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005CAC File Offset: 0x00003EAC
		public async Task<DevicePortal.ErrorInformation> SetNewPasswordAsync(string oldPassword, string newPassword)
		{
			return await this.PostAsync<DevicePortal.ErrorInformation>(DevicePortal.ResetPasswordApi, string.Format("oldpassword={0}&newpassword={1}", Utilities.Hex64Encode(oldPassword), Utilities.Hex64Encode(newPassword)), null, null);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005D04 File Offset: 0x00003F04
		public async Task SetNewRemoteDebuggingPinAsync(string newPin)
		{
			await this.PostAsync(DevicePortal.NewRemoteDebuggingPinApi, string.Format("newpin={0}", Utilities.Hex64Encode(newPin)));
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005D54 File Offset: 0x00003F54
		public async Task<DevicePortal.ControllerDriverInfo> SetControllersDriversAsync(string newDriver)
		{
			return await this.PostAsync<DevicePortal.ControllerDriverInfo>(DevicePortal.ControllerDriverApi, string.Format("newdriver={0}", Utilities.Hex64Encode(newDriver)), null, null);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005DA4 File Offset: 0x00003FA4
		public async Task<DevicePortal.ErrorInformation> SetTimeZoneAsync(int index)
		{
			return await this.PostAsync<DevicePortal.ErrorInformation>(DevicePortal.SetTimeZoneApi, string.Format("index={0}", index), null, null);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005DF4 File Offset: 0x00003FF4
		public async Task SetDisplayResolutionAsync(string displayResolution)
		{
			await this.PostAsync(DevicePortal.DisplayResolutionApi, string.Format("newdisplayresolution={0}", Utilities.Hex64Encode(displayResolution)));
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005E44 File Offset: 0x00004044
		public async Task SetDisplayOrientationAsync(string displayOrientation)
		{
			await this.PostAsync(DevicePortal.DisplayOrientationApi, string.Format("newdisplayorientation={0}", Utilities.Hex64Encode(displayOrientation)));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005E94 File Offset: 0x00004094
		public async Task<DevicePortal.StatusInfo> GetStatusInfoAsync()
		{
			return await this.GetAsync<DevicePortal.StatusInfo>(DevicePortal.StatusApi, null);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005EDC File Offset: 0x000040DC
		public async Task<DevicePortal.UpdateInstallTimeInfo> GetUpdateInstallTimeAsync()
		{
			return await this.GetAsync<DevicePortal.UpdateInstallTimeInfo>(DevicePortal.InstallTimeApi, null);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005F24 File Offset: 0x00004124
		public async Task<DevicePortal.Sandbox> GetXboxLiveSandboxAsync()
		{
			return await this.GetAsync<DevicePortal.Sandbox>(DevicePortal.XboxLiveSandboxApi, null);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005F6C File Offset: 0x0000416C
		public async Task<DevicePortal.Sandbox> SetXboxLiveSandboxAsync(string newSandbox)
		{
			DevicePortal.Sandbox sandbox = new DevicePortal.Sandbox();
			sandbox.Value = newSandbox;
			return await this.PutAsync<DevicePortal.Sandbox, DevicePortal.Sandbox>(DevicePortal.XboxLiveSandboxApi, sandbox, null);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005FBC File Offset: 0x000041BC
		public async Task<DevicePortal.SmbInfo> GetSmbShareInfoAsync()
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			return await this.GetAsync<DevicePortal.SmbInfo>(DevicePortal.GetSmbShareInfoApi, null);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006004 File Offset: 0x00004204
		public async Task<DevicePortal.UserList> GetXboxLiveUsersAsync()
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			return await this.GetAsync<DevicePortal.UserList>(DevicePortal.XboxLiveUserApi, null);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000604C File Offset: 0x0000424C
		public async Task UpdateXboxLiveUsersAsync(DevicePortal.UserList users)
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			await this.PutAsync<DevicePortal.UserList>(DevicePortal.XboxLiveUserApi, users, null);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000609C File Offset: 0x0000429C
		public async Task RegisterApplicationAsync(string folderName)
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			await this.PostAsync(DevicePortal.RegisterPackageApi, string.Format("folder={0}", Utilities.Hex64Encode(folderName))).ConfigureAwait(false);
			ApplicationInstallStatus applicationInstallStatus;
			do
			{
				await Task.Delay(TimeSpan.FromMilliseconds(500.0)).ConfigureAwait(false);
				applicationInstallStatus = await this.GetInstallStatusAsync().ConfigureAwait(false);
			}
			while (applicationInstallStatus == ApplicationInstallStatus.InProgress);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000060EC File Offset: 0x000042EC
		public async Task UploadPackageFolderAsync(string sourceFolder, string destinationFolder)
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			List<string> list = new List<string>();
			list.AddRange(Directory.GetFiles(sourceFolder));
			await this.PostAsync(DevicePortal.UploadPackageFolderApi, list, string.Format("destinationFolder={0}", Utilities.Hex64Encode(destinationFolder)));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006144 File Offset: 0x00004344
		public async Task EnableFiddlerTracingAsync(string proxyAddress, string proxyPort, string certFilePath = null)
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("proxyAddress", proxyAddress);
			dictionary.Add("proxyPort", proxyPort);
			if (!string.IsNullOrEmpty(certFilePath))
			{
				List<string> list = new List<string>();
				list.Add(certFilePath);
				dictionary.Add("updateCert", "true");
				await this.PostAsync(DevicePortal.FiddlerSetupApi, list, Utilities.BuildQueryString(dictionary));
			}
			else
			{
				await this.PostAsync(DevicePortal.FiddlerSetupApi, Utilities.BuildQueryString(dictionary));
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000061A4 File Offset: 0x000043A4
		public async Task DisableFiddlerTracingAsync()
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			await this.DeleteAsync(DevicePortal.FiddlerSetupApi, null);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000061EC File Offset: 0x000043EC
		public async Task<Stream> TakeXboxScreenshotAsync()
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			Uri uri = Utilities.BuildEndpoint(this.deviceConnection.Connection, DevicePortal.GetXboxScreenshotApi, null);
			return await this.GetAsync(uri);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006234 File Offset: 0x00004434
		public async Task<DevicePortal.XboxSettingList> GetXboxSettingsAsync()
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			return await this.GetAsync<DevicePortal.XboxSettingList>(DevicePortal.XboxSettingsApi, null);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000627C File Offset: 0x0000447C
		public async Task<DevicePortal.XboxSetting> GetXboxSettingAsync(string settingName)
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			return await this.GetAsync<DevicePortal.XboxSetting>(Path.Combine(DevicePortal.XboxSettingsApi, settingName), null);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000062CC File Offset: 0x000044CC
		public async Task<DevicePortal.XboxSetting> UpdateXboxSettingAsync(DevicePortal.XboxSetting setting)
		{
			if (this.Platform != DevicePortal.DevicePortalPlatforms.XboxOne)
			{
				throw new NotSupportedException("This method is only supported on Xbox One.");
			}
			return await this.PutAsync<DevicePortal.XboxSetting, DevicePortal.XboxSetting>(Path.Combine(DevicePortal.XboxSettingsApi, setting.Name), setting, null);
		}

		// Token: 0x04000001 RID: 1
		private X509Certificate2 manualCertificate;

		// Token: 0x04000003 RID: 3
		public static readonly string AvailableCrashDumpsApi = "api/debug/dump/usermode/dumps";

		// Token: 0x04000004 RID: 4
		public static readonly string CrashDumpFileApi = "api/debug/dump/usermode/crashdump";

		// Token: 0x04000005 RID: 5
		public static readonly string CrashDumpSettingsApi = "api/debug/dump/usermode/crashcontrol";

		// Token: 0x04000006 RID: 6
		public static readonly string InstalledPackagesApi = "api/app/packagemanager/packages";

		// Token: 0x04000007 RID: 7
		public static readonly string InstallStateApi = "api/app/packagemanager/state";

		// Token: 0x04000008 RID: 8
		public static readonly string PackageManagerApi = "api/app/packagemanager/package";

		// Token: 0x0400000A RID: 10
		public static readonly string GetFileApi = "api/filesystem/apps/file";

		// Token: 0x0400000B RID: 11
		public static readonly string RenameFileApi = "api/filesystem/apps/rename";

		// Token: 0x0400000C RID: 12
		public static readonly string GetFilesApi = "api/filesystem/apps/files";

		// Token: 0x0400000D RID: 13
		public static readonly string KnownFoldersApi = "api/filesystem/apps/knownfolders";

		// Token: 0x0400000E RID: 14
		public static readonly string InstalledDevicesApi = "api/devicemanager/devices";

		// Token: 0x0400000F RID: 15
		public static readonly string TagApi = "api/dns-sd/tag";

		// Token: 0x04000010 RID: 16
		public static readonly string TagsApi = "api/dns-sd/tags";

		// Token: 0x04000011 RID: 17
		public static readonly string AvailableBugChecksApi = "api/debug/dump/kernel/dumplist";

		// Token: 0x04000012 RID: 18
		public static readonly string BugcheckFileApi = "api/debug/dump/kernel/dump";

		// Token: 0x04000013 RID: 19
		public static readonly string BugcheckSettingsApi = "api/debug/dump/kernel/crashcontrol";

		// Token: 0x04000014 RID: 20
		public static readonly string LiveKernelDumpApi = "api/debug/dump/livekernel";

		// Token: 0x04000015 RID: 21
		public static readonly string LiveProcessDumpApi = "api/debug/dump/usermode/live";

		// Token: 0x04000016 RID: 22
		public static readonly string RealtimeEtwSessionApi = "api/etw/session/realtime";

		// Token: 0x04000017 RID: 23
		public static readonly string CustomEtwProvidersApi = "api/etw/customproviders";

		// Token: 0x04000018 RID: 24
		public static readonly string EtwProvidersApi = "api/etw/providers";

		// Token: 0x04000019 RID: 25
		private WebSocket<DevicePortal.EtwEvents> realtimeEventsWebSocket;

		// Token: 0x0400001A RID: 26
		private bool isListeningForRealtimeEvents;

		// Token: 0x0400001C RID: 28
		public static readonly string IpConfigApi = "api/networking/ipconfig";

		// Token: 0x0400001D RID: 29
		public static readonly string DeviceFamilyApi = "api/os/devicefamily";

		// Token: 0x0400001E RID: 30
		public static readonly string MachineNameApi = "api/os/machinename";

		// Token: 0x0400001F RID: 31
		public static readonly string OsInfoApi = "api/os/info";

		// Token: 0x04000020 RID: 32
		public static readonly string ActivePowerSchemeApi = "api/power/activecfg";

		// Token: 0x04000021 RID: 33
		public static readonly string BatteryStateApi = "api/power/battery";

		// Token: 0x04000022 RID: 34
		public static readonly string PowerSchemeSubValueApi = "api/power/cfg";

		// Token: 0x04000023 RID: 35
		public static readonly string PowerStateApi = "api/power/state";

		// Token: 0x04000024 RID: 36
		public static readonly string SleepStudyReportApi = "api/power/sleepstudy/report";

		// Token: 0x04000025 RID: 37
		public static readonly string SleepStudyReportsApi = "api/power/sleepstudy/reports";

		// Token: 0x04000026 RID: 38
		public static readonly string SleepStudyTransformApi = "api/power/sleepstudy/transform";

		// Token: 0x04000027 RID: 39
		public static readonly string RebootApi = "api/control/restart";

		// Token: 0x04000028 RID: 40
		public static readonly string ShutdownApi = "api/control/shutdown";

		// Token: 0x04000029 RID: 41
		public static readonly string RunningProcessApi = "api/resourcemanager/processes";

		// Token: 0x0400002A RID: 42
		public static readonly string SystemPerfApi = "api/resourcemanager/systemperf";

		// Token: 0x0400002B RID: 43
		private WebSocket<DevicePortal.RunningProcesses> deviceProcessesWebSocket;

		// Token: 0x0400002C RID: 44
		private WebSocket<DevicePortal.SystemPerformanceInformation> systemPerfWebSocket;

		// Token: 0x0400002F RID: 47
		public static readonly string TaskManagerApi = "api/taskmanager/app";

		// Token: 0x04000030 RID: 48
		public static readonly string WifiInterfacesApi = "api/wifi/interfaces";

		// Token: 0x04000031 RID: 49
		public static readonly string WifiNetworkApi = "api/wifi/network";

		// Token: 0x04000032 RID: 50
		public static readonly string WifiNetworksApi = "api/wifi/networks";

		// Token: 0x04000033 RID: 51
		public static readonly string WindowsErrorReportingFileApi = "api/wer/report/file";

		// Token: 0x04000034 RID: 52
		public static readonly string WindowsErrorReportingFilesApi = "api/wer/report/files";

		// Token: 0x04000035 RID: 53
		public static readonly string WindowsErrorReportsApi = "api/wer/reports";

		// Token: 0x04000036 RID: 54
		public static readonly string WindowsPerformanceBootTraceApi = "api/wpr/boottrace";

		// Token: 0x04000037 RID: 55
		public static readonly string WindowsPerformanceCustomTraceApi = "api/wpr/customtrace";

		// Token: 0x04000038 RID: 56
		public static readonly string WindowsPerformanceTraceApi = "api/wpr/trace";

		// Token: 0x04000039 RID: 57
		public static readonly string WindowsPerformanceTraceStatusApi = "api/wpr/status";

		// Token: 0x0400003A RID: 58
		public static readonly string DevicePortalCertificateIssuer = "Microsoft Windows Web Management";

		// Token: 0x0400003B RID: 59
		public static readonly string RootCertificateEndpoint = "config/rootcertificate";

		// Token: 0x0400003C RID: 60
		private static readonly uint ExpectedOSVersionSections = 5U;

		// Token: 0x0400003D RID: 61
		private static readonly uint TargetOSVersionSection = 3U;

		// Token: 0x0400003E RID: 62
		private IDevicePortalConnection deviceConnection;

		// Token: 0x04000042 RID: 66
		public static readonly string HolographicIpdApi = "api/holographic/os/settings/ipd";

		// Token: 0x04000043 RID: 67
		public static readonly string HolographicServicesApi = "api/holographic/os/services";

		// Token: 0x04000044 RID: 68
		public static readonly string HolographicWebManagementHttpSettingsApi = "api/holographic/os/webmanagement/settings/https";

		// Token: 0x04000045 RID: 69
		public static readonly string HolographicPerceptionClient = "api/holographic/perception/client";

		// Token: 0x04000046 RID: 70
		public static readonly string HolographicSimulationModeApi = "api/holographic/simulation/control/mode";

		// Token: 0x04000047 RID: 71
		public static readonly string HolographicSimulationStreamApi = "api/holographic/simulation/control/stream";

		// Token: 0x04000048 RID: 72
		public static readonly string ThermalStageApi = "api/holographic/thermal/stage";

		// Token: 0x04000049 RID: 73
		public static readonly string MrcFileApi = "api/holographic/mrc/file";

		// Token: 0x0400004A RID: 74
		public static readonly string MrcFileListApi = "api/holographic/mrc/files";

		// Token: 0x0400004B RID: 75
		public static readonly string MrcPhotoApi = "api/holographic/mrc/photo";

		// Token: 0x0400004C RID: 76
		public static readonly string MrcSettingsApi = "api/holographic/mrc/settings";

		// Token: 0x0400004D RID: 77
		public static readonly string MrcStartRecordingApi = "api/holographic/mrc/video/control/start";

		// Token: 0x0400004E RID: 78
		public static readonly string MrcStatusApi = "api/holographic/mrc/status";

		// Token: 0x0400004F RID: 79
		public static readonly string MrcStopRecordingApi = "api/holographic/mrc/video/control/stop";

		// Token: 0x04000050 RID: 80
		public static readonly string MrcLiveStreamApi = "api/holographic/stream/live.mp4";

		// Token: 0x04000051 RID: 81
		public static readonly string MrcLiveStreamHighResApi = "api/holographic/stream/live_high.mp4";

		// Token: 0x04000052 RID: 82
		public static readonly string MrcLiveStreamLowResApi = "api/holographic/stream/live_low.mp4";

		// Token: 0x04000053 RID: 83
		public static readonly string MrcLiveStreamMediumResApi = "api/holographic/stream/live_med.mp4";

		// Token: 0x04000054 RID: 84
		public static readonly string MrcThumbnailApi = "api/holographic/mrc/thumbnail";

		// Token: 0x04000055 RID: 85
		public static readonly string HolographicSimulationPlaybackSessionFileApi = "api/holographic/simulation/playback/session/file";

		// Token: 0x04000056 RID: 86
		public static readonly string HolographicSimulationPlaybackPauseApi = "api/holographic/simulation/playback/session/pause";

		// Token: 0x04000057 RID: 87
		public static readonly string HolographicSimulationPlaybackFileApi = "api/holographic/simulation/playback/file";

		// Token: 0x04000058 RID: 88
		public static readonly string HolographicSimulationPlaybackFilesApi = "api/holographic/simulation/playback/files";

		// Token: 0x04000059 RID: 89
		public static readonly string HolographicSimulationPlaybackPlayApi = "api/holographic/simulation/playback/session/play";

		// Token: 0x0400005A RID: 90
		public static readonly string HolographicSimulationPlaybackSessionFilesApi = "api/holographic/simulation/playback/session/files";

		// Token: 0x0400005B RID: 91
		public static readonly string HolographicSimulationPlaybackStateApi = "api/holographic/simulation/playback/session";

		// Token: 0x0400005C RID: 92
		public static readonly string HolographicSimulationPlaybackStopApi = "api/holographic/simulation/playback/session/stop";

		// Token: 0x0400005D RID: 93
		public static readonly string HolographicSimulationPlaybackDataTypesApi = "api/holographic/simulation/playback/session/types";

		// Token: 0x0400005E RID: 94
		public static readonly string HolographicSimulationRecordingStatusApi = "api/holographic/simulation/recording/status";

		// Token: 0x0400005F RID: 95
		public static readonly string StartHolographicSimulationRecordingApi = "api/holographic/simulation/recording/start";

		// Token: 0x04000060 RID: 96
		public static readonly string StopHolographicSimulationRecordingApi = "api/holographic/simulation/recording/stop";

		// Token: 0x04000061 RID: 97
		private static readonly string ContentTypeHeaderName = "Content-Type";

		// Token: 0x04000062 RID: 98
		private static readonly string CsrfTokenName = "CSRF-Token";

		// Token: 0x04000063 RID: 99
		private static readonly string UserAgentName = "User-Agent";

		// Token: 0x04000064 RID: 100
		private static readonly string UserAgentValue = "WindowsDevicePortalWrapper";

		// Token: 0x04000065 RID: 101
		private string csrfToken = string.Empty;

		// Token: 0x04000066 RID: 102
		private static readonly string SysPerfInfoErrorPrefix = "{\"Reason\" : \"";

		// Token: 0x04000067 RID: 103
		private static readonly string SysPerfInfoErrorPostfix = "\"}";

		// Token: 0x04000068 RID: 104
		public static readonly string AvailableBluetoothDevicesApi = "api/iot/bt/getavailable";

		// Token: 0x04000069 RID: 105
		public static readonly string PairedBluetoothDevicesApi = "api/iot/bt/getpaired";

		// Token: 0x0400006A RID: 106
		public static readonly string PairBluetoothDevicesApi = "api/iot/bt/pair";

		// Token: 0x0400006B RID: 107
		public static readonly string UnpairBluetoothDevicesApi = "api/iot/bt/unpair";

		// Token: 0x0400006C RID: 108
		private WebSocket<DevicePortal.AvailableBluetoothDevicesInfo> bluetoothWebSocket;

		// Token: 0x0400006D RID: 109
		private WebSocket<DevicePortal.PairedBluetoothDevicesInfo> pairedBluetoothWebSocket;

		// Token: 0x0400006E RID: 110
		private WebSocket<DevicePortal.PairBluetoothDevicesInfo> pairBluetoothWebSocket;

		// Token: 0x04000072 RID: 114
		public static readonly string TpmSettingsApi = "api/iot/tpm/settings";

		// Token: 0x04000073 RID: 115
		public static readonly string TpmAcpiTablesApi = "api/iot/tpm/acpitables";

		// Token: 0x04000074 RID: 116
		public static readonly string TpmAzureTokenApi = "api/iot/tpm/azuretoken";

		// Token: 0x04000075 RID: 117
		public static readonly string AppsListApi = "api/iot/appx/default";

		// Token: 0x04000076 RID: 118
		public static readonly string HeadlessAppsListApi = "api/iot/appx/listHeadlessApps";

		// Token: 0x04000077 RID: 119
		public static readonly string HeadlessStartupAppApi = "api/iot/appx/startupHeadlessApp";

		// Token: 0x04000078 RID: 120
		public static readonly string ActivatePackageApi = "api/iot/appx/app";

		// Token: 0x04000079 RID: 121
		public static readonly string AudioDeviceListApi = "api/iot/audio/listdevices";

		// Token: 0x0400007A RID: 122
		public static readonly string SetRenderVolumeApi = "api/iot/audio/setrendervolume";

		// Token: 0x0400007B RID: 123
		public static readonly string SetCaptureVolumeApi = "api/iot/audio/setcapturevolume";

		// Token: 0x0400007C RID: 124
		public static readonly string RunCommandApi = "api/iot/processmanagement/runcommand";

		// Token: 0x0400007D RID: 125
		public static readonly string RunCommandWithoutOutputApi = "api/iot/processmanagement/runcommandwithoutput";

		// Token: 0x0400007E RID: 126
		public static readonly string IcsInterfacesApi = "api/iot/ics/interfaces";

		// Token: 0x0400007F RID: 127
		public static readonly string IcSharingApi = "api/iot/ics/sharing";

		// Token: 0x04000080 RID: 128
		public static readonly string SoftAPSettingsApi = "api/iot/iotonboarding/softapsettings";

		// Token: 0x04000081 RID: 129
		public static readonly string AllJoynSettingsApi = "api/iot/iotonboarding/alljoynsettings";

		// Token: 0x04000082 RID: 130
		public static readonly string RemoteSettingsStatusApi = "api/iot/remote/status";

		// Token: 0x04000083 RID: 131
		public static readonly string RemoteSettingsEnableApi = "api/iot/remote/enable";

		// Token: 0x04000084 RID: 132
		public static readonly string RemoteSettingsDisableApi = "api/iot/remote/disable";

		// Token: 0x04000085 RID: 133
		public static readonly string IoTOsInfoApi = "api/iot/device/information";

		// Token: 0x04000086 RID: 134
		public static readonly string TimezoneInfoApi = "api/iot/device/timezones";

		// Token: 0x04000087 RID: 135
		public static readonly string DateTimeInfoApi = "api/iot/device/datetime";

		// Token: 0x04000088 RID: 136
		public static readonly string ControllerDriverApi = "api/iot/device/controllersdriver";

		// Token: 0x04000089 RID: 137
		public static readonly string DisplayResolutionApi = "api/iot/device/displayresolution";

		// Token: 0x0400008A RID: 138
		public static readonly string DisplayOrientationApi = "api/iot/device/displayorientation";

		// Token: 0x0400008B RID: 139
		public static readonly string DeviceNameApi = "api/iot/device/name";

		// Token: 0x0400008C RID: 140
		public static readonly string ResetPasswordApi = "api/iot/device/password";

		// Token: 0x0400008D RID: 141
		public static readonly string NewRemoteDebuggingPinApi = "api/iot/device/remotedebuggingpin";

		// Token: 0x0400008E RID: 142
		public static readonly string SetTimeZoneApi = "api/iot/device/settimezone";

		// Token: 0x0400008F RID: 143
		public static readonly string InstallTimeApi = "api/iot/windowsupdate/installtime";

		// Token: 0x04000090 RID: 144
		public static readonly string StatusApi = "api/iot/windowsupdate/status";

		// Token: 0x04000091 RID: 145
		public static readonly string XboxLiveSandboxApi = "ext/xboxlive/sandbox";

		// Token: 0x04000092 RID: 146
		private static readonly string GetSmbShareInfoApi = "ext/smb/developerfolder";

		// Token: 0x04000093 RID: 147
		public static readonly string XboxLiveUserApi = "ext/user";

		// Token: 0x04000094 RID: 148
		public static readonly string RegisterPackageApi = "api/app/packagemanager/register";

		// Token: 0x04000095 RID: 149
		public static readonly string UploadPackageFolderApi = "api/app/packagemanager/upload";

		// Token: 0x04000096 RID: 150
		public static readonly string FiddlerSetupApi = "ext/fiddler";

		// Token: 0x04000097 RID: 151
		public static readonly string GetXboxScreenshotApi = "ext/screenshot";

		// Token: 0x04000098 RID: 152
		public static readonly string XboxSettingsApi = "ext/settings";

		// Token: 0x0200001A RID: 26
		[DataContract]
		public class AppCrashDumpSettings
		{
			// Token: 0x1700002E RID: 46
			// (get) Token: 0x0600017D RID: 381 RVA: 0x00007AC1 File Offset: 0x00005CC1
			// (set) Token: 0x0600017E RID: 382 RVA: 0x00007AC9 File Offset: 0x00005CC9
			[DataMember(Name = "CrashDumpEnabled")]
			public bool CrashDumpEnabled { get; private set; }
		}

		// Token: 0x0200001B RID: 27
		[DataContract]
		public class AppCrashDump
		{
			// Token: 0x1700002F RID: 47
			// (get) Token: 0x06000180 RID: 384 RVA: 0x00007ADA File Offset: 0x00005CDA
			// (set) Token: 0x06000181 RID: 385 RVA: 0x00007AE2 File Offset: 0x00005CE2
			[DataMember(Name = "FileDate")]
			public string FileDateAsString { get; private set; }

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x06000182 RID: 386 RVA: 0x00007AEB File Offset: 0x00005CEB
			public DateTime FileDate
			{
				get
				{
					return DateTime.Parse(this.FileDateAsString);
				}
			}

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x06000183 RID: 387 RVA: 0x00007AF8 File Offset: 0x00005CF8
			// (set) Token: 0x06000184 RID: 388 RVA: 0x00007B00 File Offset: 0x00005D00
			[DataMember(Name = "FileName")]
			public string Filename { get; private set; }

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x06000185 RID: 389 RVA: 0x00007B09 File Offset: 0x00005D09
			// (set) Token: 0x06000186 RID: 390 RVA: 0x00007B11 File Offset: 0x00005D11
			[DataMember(Name = "FileSize")]
			public uint FileSizeInBytes { get; private set; }

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x06000187 RID: 391 RVA: 0x00007B1A File Offset: 0x00005D1A
			// (set) Token: 0x06000188 RID: 392 RVA: 0x00007B22 File Offset: 0x00005D22
			[DataMember(Name = "PackageFullName")]
			public string PackageFullName { get; private set; }
		}

		// Token: 0x0200001C RID: 28
		[DataContract]
		private class AppCrashDumpList
		{
			// Token: 0x17000034 RID: 52
			// (get) Token: 0x0600018A RID: 394 RVA: 0x00007B2B File Offset: 0x00005D2B
			// (set) Token: 0x0600018B RID: 395 RVA: 0x00007B33 File Offset: 0x00005D33
			[DataMember(Name = "CrashDumps")]
			public List<DevicePortal.AppCrashDump> CrashDumps { get; private set; }
		}

		// Token: 0x0200001D RID: 29
		[DataContract]
		public class AppPackages
		{
			// Token: 0x17000035 RID: 53
			// (get) Token: 0x0600018D RID: 397 RVA: 0x00007B3C File Offset: 0x00005D3C
			// (set) Token: 0x0600018E RID: 398 RVA: 0x00007B44 File Offset: 0x00005D44
			[DataMember(Name = "InstalledPackages")]
			public List<DevicePortal.PackageInfo> Packages { get; private set; }

			// Token: 0x0600018F RID: 399 RVA: 0x00007B50 File Offset: 0x00005D50
			public override string ToString()
			{
				string text = "Packages:\n";
				foreach (DevicePortal.PackageInfo arg in this.Packages)
				{
					text += arg;
				}
				return text;
			}
		}

		// Token: 0x0200001E RID: 30
		[DataContract]
		public class InstallState
		{
			// Token: 0x17000036 RID: 54
			// (get) Token: 0x06000191 RID: 401 RVA: 0x00007BAC File Offset: 0x00005DAC
			// (set) Token: 0x06000192 RID: 402 RVA: 0x00007BB4 File Offset: 0x00005DB4
			[DataMember(Name = "Code")]
			public int Code { get; private set; }

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x06000193 RID: 403 RVA: 0x00007BBD File Offset: 0x00005DBD
			// (set) Token: 0x06000194 RID: 404 RVA: 0x00007BC5 File Offset: 0x00005DC5
			[DataMember(Name = "CodeText")]
			public string CodeText { get; private set; }

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x06000195 RID: 405 RVA: 0x00007BCE File Offset: 0x00005DCE
			// (set) Token: 0x06000196 RID: 406 RVA: 0x00007BD6 File Offset: 0x00005DD6
			[DataMember(Name = "Reason")]
			public string Reason { get; private set; }

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x06000197 RID: 407 RVA: 0x00007BDF File Offset: 0x00005DDF
			// (set) Token: 0x06000198 RID: 408 RVA: 0x00007BE7 File Offset: 0x00005DE7
			[DataMember(Name = "Success")]
			public bool WasSuccessful { get; private set; }
		}

		// Token: 0x0200001F RID: 31
		[DataContract]
		public class PackageInfo
		{
			// Token: 0x1700003A RID: 58
			// (get) Token: 0x0600019A RID: 410 RVA: 0x00007BF0 File Offset: 0x00005DF0
			// (set) Token: 0x0600019B RID: 411 RVA: 0x00007BF8 File Offset: 0x00005DF8
			[DataMember(Name = "Name")]
			public string Name { get; private set; }

			// Token: 0x1700003B RID: 59
			// (get) Token: 0x0600019C RID: 412 RVA: 0x00007C01 File Offset: 0x00005E01
			// (set) Token: 0x0600019D RID: 413 RVA: 0x00007C09 File Offset: 0x00005E09
			[DataMember(Name = "PackageFamilyName")]
			public string FamilyName { get; private set; }

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x0600019E RID: 414 RVA: 0x00007C12 File Offset: 0x00005E12
			// (set) Token: 0x0600019F RID: 415 RVA: 0x00007C1A File Offset: 0x00005E1A
			[DataMember(Name = "PackageFullName")]
			public string FullName { get; private set; }

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x060001A0 RID: 416 RVA: 0x00007C23 File Offset: 0x00005E23
			// (set) Token: 0x060001A1 RID: 417 RVA: 0x00007C2B File Offset: 0x00005E2B
			[DataMember(Name = "PackageRelativeId")]
			public string AppId { get; private set; }

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007C34 File Offset: 0x00005E34
			// (set) Token: 0x060001A3 RID: 419 RVA: 0x00007C3C File Offset: 0x00005E3C
			[DataMember(Name = "Publisher")]
			public string Publisher { get; private set; }

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007C45 File Offset: 0x00005E45
			// (set) Token: 0x060001A5 RID: 421 RVA: 0x00007C4D File Offset: 0x00005E4D
			[DataMember(Name = "Version")]
			public DevicePortal.PackageVersion Version { get; private set; }

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007C56 File Offset: 0x00005E56
			// (set) Token: 0x060001A7 RID: 423 RVA: 0x00007C5E File Offset: 0x00005E5E
			[DataMember(Name = "PackageOrigin")]
			public int PackageOrigin { get; private set; }

			// Token: 0x060001A8 RID: 424 RVA: 0x00007C67 File Offset: 0x00005E67
			public bool IsSideloaded()
			{
				return this.PackageOrigin == 4 || this.PackageOrigin == 5;
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x00007C7D File Offset: 0x00005E7D
			public override string ToString()
			{
				return string.Format("\t{0}\n\t\t{1}\n", this.FullName, this.AppId);
			}
		}

		// Token: 0x02000020 RID: 32
		[DataContract]
		public class PackageVersion
		{
			// Token: 0x17000041 RID: 65
			// (get) Token: 0x060001AB RID: 427 RVA: 0x00007C95 File Offset: 0x00005E95
			// (set) Token: 0x060001AC RID: 428 RVA: 0x00007C9D File Offset: 0x00005E9D
			[DataMember(Name = "Build")]
			public int Build { get; private set; }

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x060001AD RID: 429 RVA: 0x00007CA6 File Offset: 0x00005EA6
			// (set) Token: 0x060001AE RID: 430 RVA: 0x00007CAE File Offset: 0x00005EAE
			[DataMember(Name = "Major")]
			public int Major { get; private set; }

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x060001AF RID: 431 RVA: 0x00007CB7 File Offset: 0x00005EB7
			// (set) Token: 0x060001B0 RID: 432 RVA: 0x00007CBF File Offset: 0x00005EBF
			[DataMember(Name = "Minor")]
			public int Minor { get; private set; }

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x060001B1 RID: 433 RVA: 0x00007CC8 File Offset: 0x00005EC8
			// (set) Token: 0x060001B2 RID: 434 RVA: 0x00007CD0 File Offset: 0x00005ED0
			[DataMember(Name = "Revision")]
			public int Revision { get; private set; }

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x060001B3 RID: 435 RVA: 0x00007CD9 File Offset: 0x00005ED9
			public Version Version
			{
				get
				{
					return new Version(this.Major, this.Minor, this.Build, this.Revision);
				}
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x00007CF8 File Offset: 0x00005EF8
			public override string ToString()
			{
				return this.Version.ToString();
			}
		}

		// Token: 0x02000021 RID: 33
		[DataContract]
		public class KnownFolders
		{
			// Token: 0x17000046 RID: 70
			// (get) Token: 0x060001B6 RID: 438 RVA: 0x00007D05 File Offset: 0x00005F05
			// (set) Token: 0x060001B7 RID: 439 RVA: 0x00007D0D File Offset: 0x00005F0D
			[DataMember(Name = "KnownFolders")]
			public List<string> Folders { get; private set; }

			// Token: 0x060001B8 RID: 440 RVA: 0x00007D18 File Offset: 0x00005F18
			public override string ToString()
			{
				if (this.Folders == null)
				{
					return string.Empty;
				}
				string text = string.Empty;
				foreach (string str in this.Folders)
				{
					text = text + str + "\n";
				}
				return text;
			}
		}

		// Token: 0x02000022 RID: 34
		[DataContract]
		public class FolderContents
		{
			// Token: 0x17000047 RID: 71
			// (get) Token: 0x060001BA RID: 442 RVA: 0x00007D88 File Offset: 0x00005F88
			// (set) Token: 0x060001BB RID: 443 RVA: 0x00007D90 File Offset: 0x00005F90
			[DataMember(Name = "Items")]
			public List<DevicePortal.FileOrFolderInformation> Contents { get; private set; }

			// Token: 0x060001BC RID: 444 RVA: 0x00007D9C File Offset: 0x00005F9C
			public override string ToString()
			{
				if (this.Contents == null)
				{
					return string.Empty;
				}
				string text = string.Empty;
				foreach (DevicePortal.FileOrFolderInformation fileOrFolderInformation in this.Contents)
				{
					text += fileOrFolderInformation.ToString();
				}
				return text;
			}
		}

		// Token: 0x02000023 RID: 35
		[DataContract]
		public class FileOrFolderInformation
		{
			// Token: 0x17000048 RID: 72
			// (get) Token: 0x060001BE RID: 446 RVA: 0x00007E0C File Offset: 0x0000600C
			// (set) Token: 0x060001BF RID: 447 RVA: 0x00007E14 File Offset: 0x00006014
			[DataMember(Name = "CurrentDir")]
			public string CurrentDir { get; private set; }

			// Token: 0x17000049 RID: 73
			// (get) Token: 0x060001C0 RID: 448 RVA: 0x00007E1D File Offset: 0x0000601D
			// (set) Token: 0x060001C1 RID: 449 RVA: 0x00007E25 File Offset: 0x00006025
			[DataMember(Name = "DateCreated")]
			public long DateCreated { get; private set; }

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x060001C2 RID: 450 RVA: 0x00007E2E File Offset: 0x0000602E
			// (set) Token: 0x060001C3 RID: 451 RVA: 0x00007E36 File Offset: 0x00006036
			[DataMember(Name = "Id")]
			public string Id { get; private set; }

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x060001C4 RID: 452 RVA: 0x00007E3F File Offset: 0x0000603F
			// (set) Token: 0x060001C5 RID: 453 RVA: 0x00007E47 File Offset: 0x00006047
			[DataMember(Name = "Name")]
			public string Name { get; private set; }

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x060001C6 RID: 454 RVA: 0x00007E50 File Offset: 0x00006050
			// (set) Token: 0x060001C7 RID: 455 RVA: 0x00007E58 File Offset: 0x00006058
			[DataMember(Name = "SubPath")]
			public string SubPath { get; private set; }

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x060001C8 RID: 456 RVA: 0x00007E61 File Offset: 0x00006061
			// (set) Token: 0x060001C9 RID: 457 RVA: 0x00007E69 File Offset: 0x00006069
			[DataMember(Name = "Type")]
			public int Type { get; private set; }

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x060001CA RID: 458 RVA: 0x00007E72 File Offset: 0x00006072
			// (set) Token: 0x060001CB RID: 459 RVA: 0x00007E7A File Offset: 0x0000607A
			[DataMember(Name = "FileSize")]
			public long SizeInBytes { get; private set; }

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060001CC RID: 460 RVA: 0x00007E83 File Offset: 0x00006083
			public bool IsFolder
			{
				get
				{
					return (this.Type & 16) == 16;
				}
			}

			// Token: 0x060001CD RID: 461 RVA: 0x00007E94 File Offset: 0x00006094
			public override string ToString()
			{
				DateTime dateTime = DateTime.FromFileTime(this.DateCreated);
				if (this.IsFolder)
				{
					return string.Format("{0,-24:MM/dd/yyyy  HH:mm tt}{1,-14} {2}\n", dateTime, "<DIR>", this.Name);
				}
				return string.Format("{0,-24:MM/dd/yyyy  HH:mm tt}{1,14:n0} {2}\n", dateTime, this.SizeInBytes, this.Name);
			}
		}

		// Token: 0x02000024 RID: 36
		[DataContract]
		public class DeviceList
		{
			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060001CF RID: 463 RVA: 0x00007EF2 File Offset: 0x000060F2
			// (set) Token: 0x060001D0 RID: 464 RVA: 0x00007EFA File Offset: 0x000060FA
			[DataMember(Name = "DeviceList")]
			public List<DevicePortal.Device> Devices { get; private set; }
		}

		// Token: 0x02000025 RID: 37
		[DataContract]
		public class Device
		{
			// Token: 0x17000051 RID: 81
			// (get) Token: 0x060001D2 RID: 466 RVA: 0x00007F03 File Offset: 0x00006103
			// (set) Token: 0x060001D3 RID: 467 RVA: 0x00007F0B File Offset: 0x0000610B
			[DataMember(Name = "Class")]
			public string Class { get; private set; }

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x060001D4 RID: 468 RVA: 0x00007F14 File Offset: 0x00006114
			// (set) Token: 0x060001D5 RID: 469 RVA: 0x00007F1C File Offset: 0x0000611C
			[DataMember(Name = "Description")]
			public string Description { get; private set; }

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x060001D6 RID: 470 RVA: 0x00007F25 File Offset: 0x00006125
			// (set) Token: 0x060001D7 RID: 471 RVA: 0x00007F2D File Offset: 0x0000612D
			[DataMember(Name = "FriendlyName")]
			public string FriendlyName { get; private set; }

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x060001D8 RID: 472 RVA: 0x00007F36 File Offset: 0x00006136
			// (set) Token: 0x060001D9 RID: 473 RVA: 0x00007F3E File Offset: 0x0000613E
			[DataMember(Name = "ID")]
			public string ID { get; private set; }

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x060001DA RID: 474 RVA: 0x00007F47 File Offset: 0x00006147
			// (set) Token: 0x060001DB RID: 475 RVA: 0x00007F4F File Offset: 0x0000614F
			[DataMember(Name = "Manufacturer")]
			public string Manufacturer { get; private set; }

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x060001DC RID: 476 RVA: 0x00007F58 File Offset: 0x00006158
			// (set) Token: 0x060001DD RID: 477 RVA: 0x00007F60 File Offset: 0x00006160
			[DataMember(Name = "ParentID")]
			public string ParentID { get; private set; }

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x060001DE RID: 478 RVA: 0x00007F69 File Offset: 0x00006169
			// (set) Token: 0x060001DF RID: 479 RVA: 0x00007F71 File Offset: 0x00006171
			[DataMember(Name = "ProblemCode")]
			public int ProblemCode { get; private set; }

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x060001E0 RID: 480 RVA: 0x00007F7A File Offset: 0x0000617A
			// (set) Token: 0x060001E1 RID: 481 RVA: 0x00007F82 File Offset: 0x00006182
			[DataMember(Name = "StatusCode")]
			public int StatusCode { get; private set; }
		}

		// Token: 0x02000026 RID: 38
		[DataContract]
		public class ServiceTags
		{
			// Token: 0x17000059 RID: 89
			// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007F8B File Offset: 0x0000618B
			// (set) Token: 0x060001E4 RID: 484 RVA: 0x00007F93 File Offset: 0x00006193
			[DataMember(Name = "tags")]
			public List<string> Tags { get; private set; }
		}

		// Token: 0x02000027 RID: 39
		[DataContract]
		public class DumpFileSettings
		{
			// Token: 0x1700005A RID: 90
			// (get) Token: 0x060001E6 RID: 486 RVA: 0x00007F9C File Offset: 0x0000619C
			// (set) Token: 0x060001E7 RID: 487 RVA: 0x00007FA4 File Offset: 0x000061A4
			[DataMember(Name = "autoreboot")]
			public bool AutoReboot { get; set; }

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x060001E8 RID: 488 RVA: 0x00007FAD File Offset: 0x000061AD
			// (set) Token: 0x060001E9 RID: 489 RVA: 0x00007FB5 File Offset: 0x000061B5
			[DataMember(Name = "dumptype")]
			public DevicePortal.DumpFileSettings.DumpTypes DumpType { get; set; }

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x060001EA RID: 490 RVA: 0x00007FBE File Offset: 0x000061BE
			// (set) Token: 0x060001EB RID: 491 RVA: 0x00007FC6 File Offset: 0x000061C6
			[DataMember(Name = "maxdumpcount")]
			public int MaxDumpCount { get; set; }

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x060001EC RID: 492 RVA: 0x00007FCF File Offset: 0x000061CF
			// (set) Token: 0x060001ED RID: 493 RVA: 0x00007FD7 File Offset: 0x000061D7
			[DataMember(Name = "overwrite")]
			public bool Overwrite { get; set; }

			// Token: 0x02000145 RID: 325
			public enum DumpTypes
			{
				// Token: 0x04000672 RID: 1650
				Disabled,
				// Token: 0x04000673 RID: 1651
				CompleteMemoryDump,
				// Token: 0x04000674 RID: 1652
				KernelDump,
				// Token: 0x04000675 RID: 1653
				Minidump
			}
		}

		// Token: 0x02000028 RID: 40
		[DataContract]
		public class DumpFileList
		{
			// Token: 0x1700005E RID: 94
			// (get) Token: 0x060001EF RID: 495 RVA: 0x00007FE0 File Offset: 0x000061E0
			// (set) Token: 0x060001F0 RID: 496 RVA: 0x00007FE8 File Offset: 0x000061E8
			[DataMember(Name = "DumpFiles")]
			public List<DevicePortal.Dumpfile> DumpFiles { get; private set; }
		}

		// Token: 0x02000029 RID: 41
		[DataContract]
		public class Dumpfile
		{
			// Token: 0x1700005F RID: 95
			// (get) Token: 0x060001F2 RID: 498 RVA: 0x00007FF1 File Offset: 0x000061F1
			// (set) Token: 0x060001F3 RID: 499 RVA: 0x00007FF9 File Offset: 0x000061F9
			[DataMember(Name = "FileDate")]
			public string FileDateAsString { get; private set; }

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x060001F4 RID: 500 RVA: 0x00008002 File Offset: 0x00006202
			public DateTime FileDate
			{
				get
				{
					return DateTime.Parse(this.FileDateAsString);
				}
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000800F File Offset: 0x0000620F
			// (set) Token: 0x060001F6 RID: 502 RVA: 0x00008017 File Offset: 0x00006217
			[DataMember(Name = "FileName")]
			public string Filename { get; private set; }

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x060001F7 RID: 503 RVA: 0x00008020 File Offset: 0x00006220
			// (set) Token: 0x060001F8 RID: 504 RVA: 0x00008028 File Offset: 0x00006228
			[DataMember(Name = "FileSize")]
			public uint FileSizeInBytes { get; private set; }
		}

		// Token: 0x0200002A RID: 42
		[DataContract]
		public class EtwEvents
		{
			// Token: 0x17000063 RID: 99
			// (get) Token: 0x060001FA RID: 506 RVA: 0x00008034 File Offset: 0x00006234
			public List<DevicePortal.EtwEventInfo> Events
			{
				get
				{
					if (this.stashedList != null)
					{
						return this.stashedList;
					}
					List<DevicePortal.EtwEventInfo> list = new List<DevicePortal.EtwEventInfo>();
					foreach (Dictionary<string, string> dictionary in this.RawEvents)
					{
						list.Add(new DevicePortal.EtwEventInfo(dictionary));
					}
					this.stashedList = list;
					return this.stashedList;
				}
			}

			// Token: 0x17000064 RID: 100
			// (get) Token: 0x060001FB RID: 507 RVA: 0x000080B0 File Offset: 0x000062B0
			// (set) Token: 0x060001FC RID: 508 RVA: 0x000080B8 File Offset: 0x000062B8
			[DataMember(Name = "Frequency")]
			public long Frequency { get; private set; }

			// Token: 0x17000065 RID: 101
			// (get) Token: 0x060001FD RID: 509 RVA: 0x000080C1 File Offset: 0x000062C1
			// (set) Token: 0x060001FE RID: 510 RVA: 0x000080C9 File Offset: 0x000062C9
			[DataMember(Name = "Events")]
			private List<Dictionary<string, string>> RawEvents { get; set; }

			// Token: 0x04000157 RID: 343
			private List<DevicePortal.EtwEventInfo> stashedList;
		}

		// Token: 0x0200002B RID: 43
		public class EtwEventInfo : Dictionary<string, string>
		{
			// Token: 0x06000200 RID: 512 RVA: 0x000080D2 File Offset: 0x000062D2
			internal EtwEventInfo(IDictionary<string, string> dictionary) : base(dictionary)
			{
			}

			// Token: 0x17000066 RID: 102
			// (get) Token: 0x06000201 RID: 513 RVA: 0x000080DB File Offset: 0x000062DB
			public ushort ID
			{
				get
				{
					return ushort.Parse(base["ID"]);
				}
			}

			// Token: 0x17000067 RID: 103
			// (get) Token: 0x06000202 RID: 514 RVA: 0x000080ED File Offset: 0x000062ED
			public ulong Keyword
			{
				get
				{
					return ulong.Parse(base["Keyword"]);
				}
			}

			// Token: 0x17000068 RID: 104
			// (get) Token: 0x06000203 RID: 515 RVA: 0x000080FF File Offset: 0x000062FF
			public uint Level
			{
				get
				{
					return uint.Parse(base["Level"]);
				}
			}

			// Token: 0x17000069 RID: 105
			// (get) Token: 0x06000204 RID: 516 RVA: 0x00008111 File Offset: 0x00006311
			public string Provider
			{
				get
				{
					return base["ProviderName"];
				}
			}

			// Token: 0x1700006A RID: 106
			// (get) Token: 0x06000205 RID: 517 RVA: 0x0000811E File Offset: 0x0000631E
			public ulong Timestamp
			{
				get
				{
					return ulong.Parse(base["Timestamp"]);
				}
			}
		}

		// Token: 0x0200002C RID: 44
		[DataContract]
		public class EtwProviders
		{
			// Token: 0x1700006B RID: 107
			// (get) Token: 0x06000206 RID: 518 RVA: 0x00008130 File Offset: 0x00006330
			// (set) Token: 0x06000207 RID: 519 RVA: 0x00008138 File Offset: 0x00006338
			[DataMember(Name = "Providers")]
			public List<DevicePortal.EtwProviderInfo> Providers { get; private set; }
		}

		// Token: 0x0200002D RID: 45
		[DataContract]
		public class EtwProviderInfo
		{
			// Token: 0x1700006C RID: 108
			// (get) Token: 0x06000209 RID: 521 RVA: 0x00008141 File Offset: 0x00006341
			// (set) Token: 0x0600020A RID: 522 RVA: 0x00008149 File Offset: 0x00006349
			[DataMember(Name = "GUID")]
			public Guid GUID { get; private set; }

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x0600020B RID: 523 RVA: 0x00008152 File Offset: 0x00006352
			// (set) Token: 0x0600020C RID: 524 RVA: 0x0000815A File Offset: 0x0000635A
			[DataMember(Name = "Name")]
			public string Name { get; private set; }
		}

		// Token: 0x0200002E RID: 46
		[DataContract]
		public class Dhcp
		{
			// Token: 0x1700006E RID: 110
			// (get) Token: 0x0600020E RID: 526 RVA: 0x00008163 File Offset: 0x00006363
			// (set) Token: 0x0600020F RID: 527 RVA: 0x0000816B File Offset: 0x0000636B
			[DataMember(Name = "LeaseExpires")]
			public long LeaseExpiresRaw { get; private set; }

			// Token: 0x1700006F RID: 111
			// (get) Token: 0x06000210 RID: 528 RVA: 0x00008174 File Offset: 0x00006374
			// (set) Token: 0x06000211 RID: 529 RVA: 0x0000817C File Offset: 0x0000637C
			[DataMember(Name = "LeaseObtained")]
			public long LeaseObtainedRaw { get; private set; }

			// Token: 0x17000070 RID: 112
			// (get) Token: 0x06000212 RID: 530 RVA: 0x00008185 File Offset: 0x00006385
			// (set) Token: 0x06000213 RID: 531 RVA: 0x0000818D File Offset: 0x0000638D
			[DataMember(Name = "Address")]
			public DevicePortal.IpAddressInfo Address { get; private set; }

			// Token: 0x17000071 RID: 113
			// (get) Token: 0x06000214 RID: 532 RVA: 0x00008196 File Offset: 0x00006396
			public DateTimeOffset LeaseExpires
			{
				get
				{
					return new DateTimeOffset(new DateTime(this.LeaseExpiresRaw));
				}
			}

			// Token: 0x17000072 RID: 114
			// (get) Token: 0x06000215 RID: 533 RVA: 0x000081A8 File Offset: 0x000063A8
			public DateTimeOffset LeaseObtained
			{
				get
				{
					return new DateTimeOffset(new DateTime(this.LeaseObtainedRaw));
				}
			}
		}

		// Token: 0x0200002F RID: 47
		[DataContract]
		public class IpAddressInfo
		{
			// Token: 0x17000073 RID: 115
			// (get) Token: 0x06000217 RID: 535 RVA: 0x000081BA File Offset: 0x000063BA
			// (set) Token: 0x06000218 RID: 536 RVA: 0x000081C2 File Offset: 0x000063C2
			[DataMember(Name = "IpAddress")]
			public string Address { get; private set; }

			// Token: 0x17000074 RID: 116
			// (get) Token: 0x06000219 RID: 537 RVA: 0x000081CB File Offset: 0x000063CB
			// (set) Token: 0x0600021A RID: 538 RVA: 0x000081D3 File Offset: 0x000063D3
			[DataMember(Name = "Mask")]
			public string SubnetMask { get; private set; }
		}

		// Token: 0x02000030 RID: 48
		[DataContract]
		public class IpConfiguration
		{
			// Token: 0x17000075 RID: 117
			// (get) Token: 0x0600021C RID: 540 RVA: 0x000081DC File Offset: 0x000063DC
			// (set) Token: 0x0600021D RID: 541 RVA: 0x000081E4 File Offset: 0x000063E4
			[DataMember(Name = "Adapters")]
			public List<DevicePortal.NetworkAdapterInfo> Adapters { get; private set; }
		}

		// Token: 0x02000031 RID: 49
		[DataContract]
		public class NetworkAdapterInfo
		{
			// Token: 0x17000076 RID: 118
			// (get) Token: 0x0600021F RID: 543 RVA: 0x000081ED File Offset: 0x000063ED
			// (set) Token: 0x06000220 RID: 544 RVA: 0x000081F5 File Offset: 0x000063F5
			[DataMember(Name = "Description")]
			public string Description { get; private set; }

			// Token: 0x17000077 RID: 119
			// (get) Token: 0x06000221 RID: 545 RVA: 0x000081FE File Offset: 0x000063FE
			// (set) Token: 0x06000222 RID: 546 RVA: 0x00008206 File Offset: 0x00006406
			[DataMember(Name = "HardwareAddress")]
			public string MacAddress { get; private set; }

			// Token: 0x17000078 RID: 120
			// (get) Token: 0x06000223 RID: 547 RVA: 0x0000820F File Offset: 0x0000640F
			// (set) Token: 0x06000224 RID: 548 RVA: 0x00008217 File Offset: 0x00006417
			[DataMember(Name = "Index")]
			public int Index { get; private set; }

			// Token: 0x17000079 RID: 121
			// (get) Token: 0x06000225 RID: 549 RVA: 0x00008220 File Offset: 0x00006420
			// (set) Token: 0x06000226 RID: 550 RVA: 0x00008228 File Offset: 0x00006428
			[DataMember(Name = "Name")]
			public Guid Id { get; private set; }

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x06000227 RID: 551 RVA: 0x00008231 File Offset: 0x00006431
			// (set) Token: 0x06000228 RID: 552 RVA: 0x00008239 File Offset: 0x00006439
			[DataMember(Name = "Type")]
			public string AdapterType { get; private set; }

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x06000229 RID: 553 RVA: 0x00008242 File Offset: 0x00006442
			// (set) Token: 0x0600022A RID: 554 RVA: 0x0000824A File Offset: 0x0000644A
			[DataMember(Name = "DHCP")]
			public DevicePortal.Dhcp Dhcp { get; private set; }

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x0600022B RID: 555 RVA: 0x00008253 File Offset: 0x00006453
			// (set) Token: 0x0600022C RID: 556 RVA: 0x0000825B File Offset: 0x0000645B
			[DataMember(Name = "Gateways")]
			public List<DevicePortal.IpAddressInfo> Gateways { get; private set; }

			// Token: 0x1700007D RID: 125
			// (get) Token: 0x0600022D RID: 557 RVA: 0x00008264 File Offset: 0x00006464
			// (set) Token: 0x0600022E RID: 558 RVA: 0x0000826C File Offset: 0x0000646C
			[DataMember(Name = "IpAddresses")]
			public List<DevicePortal.IpAddressInfo> IpAddresses { get; private set; }
		}

		// Token: 0x02000032 RID: 50
		public enum DevicePortalPlatforms
		{
			// Token: 0x0400016C RID: 364
			Unknown = -1,
			// Token: 0x0400016D RID: 365
			Windows,
			// Token: 0x0400016E RID: 366
			Mobile,
			// Token: 0x0400016F RID: 367
			HoloLens,
			// Token: 0x04000170 RID: 368
			XboxOne,
			// Token: 0x04000171 RID: 369
			IoTDragonboard410c,
			// Token: 0x04000172 RID: 370
			IoTMinnowboardMax,
			// Token: 0x04000173 RID: 371
			IoTRaspberryPi2,
			// Token: 0x04000174 RID: 372
			IoTRaspberryPi3,
			// Token: 0x04000175 RID: 373
			VirtualMachine
		}

		// Token: 0x02000033 RID: 51
		[DataContract]
		public class DeviceName
		{
			// Token: 0x1700007E RID: 126
			// (get) Token: 0x06000230 RID: 560 RVA: 0x00008275 File Offset: 0x00006475
			// (set) Token: 0x06000231 RID: 561 RVA: 0x0000827D File Offset: 0x0000647D
			[DataMember(Name = "ComputerName")]
			public string Name { get; private set; }
		}

		// Token: 0x02000034 RID: 52
		[DataContract]
		public class DeviceOsFamily
		{
			// Token: 0x1700007F RID: 127
			// (get) Token: 0x06000233 RID: 563 RVA: 0x00008286 File Offset: 0x00006486
			// (set) Token: 0x06000234 RID: 564 RVA: 0x0000828E File Offset: 0x0000648E
			[DataMember(Name = "DeviceType")]
			public string Family { get; private set; }
		}

		// Token: 0x02000035 RID: 53
		[DataContract]
		public class OperatingSystemInformation
		{
			// Token: 0x17000080 RID: 128
			// (get) Token: 0x06000236 RID: 566 RVA: 0x00008297 File Offset: 0x00006497
			// (set) Token: 0x06000237 RID: 567 RVA: 0x0000829F File Offset: 0x0000649F
			[DataMember(Name = "ComputerName")]
			public string Name { get; private set; }

			// Token: 0x17000081 RID: 129
			// (get) Token: 0x06000238 RID: 568 RVA: 0x000082A8 File Offset: 0x000064A8
			// (set) Token: 0x06000239 RID: 569 RVA: 0x000082B0 File Offset: 0x000064B0
			[DataMember(Name = "Language")]
			public string Language { get; private set; }

			// Token: 0x17000082 RID: 130
			// (get) Token: 0x0600023A RID: 570 RVA: 0x000082B9 File Offset: 0x000064B9
			// (set) Token: 0x0600023B RID: 571 RVA: 0x000082C1 File Offset: 0x000064C1
			[DataMember(Name = "OsEdition")]
			public string OsEdition { get; private set; }

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x0600023C RID: 572 RVA: 0x000082CA File Offset: 0x000064CA
			// (set) Token: 0x0600023D RID: 573 RVA: 0x000082D2 File Offset: 0x000064D2
			[DataMember(Name = "OsEditionId")]
			public uint OsEditionId { get; private set; }

			// Token: 0x17000084 RID: 132
			// (get) Token: 0x0600023E RID: 574 RVA: 0x000082DB File Offset: 0x000064DB
			// (set) Token: 0x0600023F RID: 575 RVA: 0x000082E3 File Offset: 0x000064E3
			[DataMember(Name = "OsVersion")]
			public string OsVersionString { get; private set; }

			// Token: 0x17000085 RID: 133
			// (get) Token: 0x06000240 RID: 576 RVA: 0x000082EC File Offset: 0x000064EC
			// (set) Token: 0x06000241 RID: 577 RVA: 0x000082F4 File Offset: 0x000064F4
			[DataMember(Name = "Platform")]
			public string PlatformName { get; private set; }

			// Token: 0x17000086 RID: 134
			// (get) Token: 0x06000242 RID: 578 RVA: 0x00008300 File Offset: 0x00006500
			public DevicePortal.DevicePortalPlatforms Platform
			{
				get
				{
					DevicePortal.DevicePortalPlatforms result = DevicePortal.DevicePortalPlatforms.Unknown;
					try
					{
						if (this.PlatformName.Contains("Minnowboard Max"))
						{
							return DevicePortal.DevicePortalPlatforms.IoTMinnowboardMax;
						}
						string a = this.PlatformName;
						if (!(a == "Xbox One"))
						{
							if (!(a == "SBC"))
							{
								if (!(a == "Raspberry Pi 2"))
								{
									if (!(a == "Raspberry Pi 3"))
									{
										if (!(a == "Virtual Machine"))
										{
											result = (DevicePortal.DevicePortalPlatforms)Enum.Parse(typeof(DevicePortal.DevicePortalPlatforms), this.PlatformName);
										}
										else
										{
											result = DevicePortal.DevicePortalPlatforms.VirtualMachine;
										}
									}
									else
									{
										result = DevicePortal.DevicePortalPlatforms.IoTRaspberryPi3;
									}
								}
								else
								{
									result = DevicePortal.DevicePortalPlatforms.IoTRaspberryPi2;
								}
							}
							else
							{
								result = DevicePortal.DevicePortalPlatforms.IoTDragonboard410c;
							}
						}
						else
						{
							result = DevicePortal.DevicePortalPlatforms.XboxOne;
						}
					}
					catch
					{
						string a = this.OsEdition;
						if (!(a == "Enterprise") && !(a == "Home") && !(a == "Professional"))
						{
							if (!(a == "Mobile"))
							{
								result = DevicePortal.DevicePortalPlatforms.Unknown;
							}
							else
							{
								result = DevicePortal.DevicePortalPlatforms.Mobile;
							}
						}
						else
						{
							result = DevicePortal.DevicePortalPlatforms.Windows;
						}
					}
					return result;
				}
			}
		}

		// Token: 0x02000036 RID: 54
		[DataContract]
		public class ActivePowerScheme
		{
			// Token: 0x17000087 RID: 135
			// (get) Token: 0x06000244 RID: 580 RVA: 0x00008400 File Offset: 0x00006600
			// (set) Token: 0x06000245 RID: 581 RVA: 0x00008408 File Offset: 0x00006608
			[DataMember(Name = "ActivePowerScheme")]
			public Guid Id { get; private set; }
		}

		// Token: 0x02000037 RID: 55
		[DataContract]
		public class BatteryState
		{
			// Token: 0x17000088 RID: 136
			// (get) Token: 0x06000247 RID: 583 RVA: 0x00008411 File Offset: 0x00006611
			// (set) Token: 0x06000248 RID: 584 RVA: 0x00008419 File Offset: 0x00006619
			[DataMember(Name = "AcOnline")]
			public bool IsOnAcPower { get; private set; }

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x06000249 RID: 585 RVA: 0x00008422 File Offset: 0x00006622
			// (set) Token: 0x0600024A RID: 586 RVA: 0x0000842A File Offset: 0x0000662A
			[DataMember(Name = "BatteryPresent")]
			public bool IsBatteryPresent { get; private set; }

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x0600024B RID: 587 RVA: 0x00008433 File Offset: 0x00006633
			// (set) Token: 0x0600024C RID: 588 RVA: 0x0000843B File Offset: 0x0000663B
			[DataMember(Name = "Charging")]
			public bool IsCharging { get; private set; }

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x0600024D RID: 589 RVA: 0x00008444 File Offset: 0x00006644
			// (set) Token: 0x0600024E RID: 590 RVA: 0x0000844C File Offset: 0x0000664C
			[DataMember(Name = "DefaultAlert1")]
			public int DefaultAlert1 { get; private set; }

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x0600024F RID: 591 RVA: 0x00008455 File Offset: 0x00006655
			// (set) Token: 0x06000250 RID: 592 RVA: 0x0000845D File Offset: 0x0000665D
			[DataMember(Name = "DefaultAlert2")]
			public int DefaultAlert2 { get; private set; }

			// Token: 0x1700008D RID: 141
			// (get) Token: 0x06000251 RID: 593 RVA: 0x00008466 File Offset: 0x00006666
			// (set) Token: 0x06000252 RID: 594 RVA: 0x0000846E File Offset: 0x0000666E
			[DataMember(Name = "EstimatedTime")]
			public uint EstimatedTimeRaw { get; private set; }

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x06000253 RID: 595 RVA: 0x00008477 File Offset: 0x00006677
			// (set) Token: 0x06000254 RID: 596 RVA: 0x0000847F File Offset: 0x0000667F
			[DataMember(Name = "MaximumCapacity")]
			public int MaximumCapacity { get; private set; }

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x06000255 RID: 597 RVA: 0x00008488 File Offset: 0x00006688
			// (set) Token: 0x06000256 RID: 598 RVA: 0x00008490 File Offset: 0x00006690
			[DataMember(Name = "RemainingCapacity")]
			public int RemainingCapacity { get; private set; }

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x06000257 RID: 599 RVA: 0x00008499 File Offset: 0x00006699
			public float Level
			{
				get
				{
					if (this.MaximumCapacity == 0)
					{
						return 100f;
					}
					return 100f * ((float)this.RemainingCapacity / (float)this.MaximumCapacity);
				}
			}

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x06000258 RID: 600 RVA: 0x000084BE File Offset: 0x000066BE
			public TimeSpan EstimatedTime
			{
				get
				{
					return new TimeSpan(0, 0, (int)this.EstimatedTimeRaw);
				}
			}
		}

		// Token: 0x02000038 RID: 56
		[DataContract]
		public class PowerState
		{
			// Token: 0x17000092 RID: 146
			// (get) Token: 0x0600025A RID: 602 RVA: 0x000084CD File Offset: 0x000066CD
			// (set) Token: 0x0600025B RID: 603 RVA: 0x000084D5 File Offset: 0x000066D5
			[DataMember(Name = "LowPowerState")]
			public bool InLowPowerState { get; private set; }

			// Token: 0x17000093 RID: 147
			// (get) Token: 0x0600025C RID: 604 RVA: 0x000084DE File Offset: 0x000066DE
			// (set) Token: 0x0600025D RID: 605 RVA: 0x000084E6 File Offset: 0x000066E6
			[DataMember(Name = "LowPowerStateAvailable")]
			public bool IsLowPowerStateAvailable { get; private set; }
		}

		// Token: 0x02000039 RID: 57
		[DataContract]
		public class AppVersion
		{
			// Token: 0x17000094 RID: 148
			// (get) Token: 0x0600025F RID: 607 RVA: 0x000084EF File Offset: 0x000066EF
			// (set) Token: 0x06000260 RID: 608 RVA: 0x000084F7 File Offset: 0x000066F7
			[DataMember(Name = "Major")]
			public uint Major { get; private set; }

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x06000261 RID: 609 RVA: 0x00008500 File Offset: 0x00006700
			// (set) Token: 0x06000262 RID: 610 RVA: 0x00008508 File Offset: 0x00006708
			[DataMember(Name = "Minor")]
			public uint Minor { get; private set; }

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x06000263 RID: 611 RVA: 0x00008511 File Offset: 0x00006711
			// (set) Token: 0x06000264 RID: 612 RVA: 0x00008519 File Offset: 0x00006719
			[DataMember(Name = "Build")]
			public uint Build { get; private set; }

			// Token: 0x17000097 RID: 151
			// (get) Token: 0x06000265 RID: 613 RVA: 0x00008522 File Offset: 0x00006722
			// (set) Token: 0x06000266 RID: 614 RVA: 0x0000852A File Offset: 0x0000672A
			[DataMember(Name = "Revision")]
			public uint Revision { get; private set; }
		}

		// Token: 0x0200003A RID: 58
		[DataContract]
		public class DeviceProcessInfo
		{
			// Token: 0x17000098 RID: 152
			// (get) Token: 0x06000268 RID: 616 RVA: 0x00008533 File Offset: 0x00006733
			// (set) Token: 0x06000269 RID: 617 RVA: 0x0000853B File Offset: 0x0000673B
			[DataMember(Name = "AppName")]
			public string AppName { get; private set; }

			// Token: 0x17000099 RID: 153
			// (get) Token: 0x0600026A RID: 618 RVA: 0x00008544 File Offset: 0x00006744
			// (set) Token: 0x0600026B RID: 619 RVA: 0x0000854C File Offset: 0x0000674C
			[DataMember(Name = "CPUUsage")]
			public float CpuUsage { get; private set; }

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x0600026C RID: 620 RVA: 0x00008555 File Offset: 0x00006755
			// (set) Token: 0x0600026D RID: 621 RVA: 0x0000855D File Offset: 0x0000675D
			[DataMember(Name = "ImageName")]
			public string Name { get; private set; }

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x0600026E RID: 622 RVA: 0x00008566 File Offset: 0x00006766
			// (set) Token: 0x0600026F RID: 623 RVA: 0x0000856E File Offset: 0x0000676E
			[DataMember(Name = "ProcessId")]
			public uint ProcessId { get; private set; }

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x06000270 RID: 624 RVA: 0x00008577 File Offset: 0x00006777
			// (set) Token: 0x06000271 RID: 625 RVA: 0x0000857F File Offset: 0x0000677F
			[DataMember(Name = "UserName")]
			public string UserName { get; private set; }

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x06000272 RID: 626 RVA: 0x00008588 File Offset: 0x00006788
			// (set) Token: 0x06000273 RID: 627 RVA: 0x00008590 File Offset: 0x00006790
			[DataMember(Name = "PackageFullName")]
			public string PackageFullName { get; private set; }

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x06000274 RID: 628 RVA: 0x00008599 File Offset: 0x00006799
			// (set) Token: 0x06000275 RID: 629 RVA: 0x000085A1 File Offset: 0x000067A1
			[DataMember(Name = "PageFileUsage")]
			public ulong PageFile { get; private set; }

			// Token: 0x1700009F RID: 159
			// (get) Token: 0x06000276 RID: 630 RVA: 0x000085AA File Offset: 0x000067AA
			// (set) Token: 0x06000277 RID: 631 RVA: 0x000085B2 File Offset: 0x000067B2
			[DataMember(Name = "WorkingSetSize")]
			public ulong WorkingSet { get; private set; }

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x06000278 RID: 632 RVA: 0x000085BB File Offset: 0x000067BB
			// (set) Token: 0x06000279 RID: 633 RVA: 0x000085C3 File Offset: 0x000067C3
			[DataMember(Name = "PrivateWorkingSet")]
			public ulong PrivateWorkingSet { get; private set; }

			// Token: 0x170000A1 RID: 161
			// (get) Token: 0x0600027A RID: 634 RVA: 0x000085CC File Offset: 0x000067CC
			// (set) Token: 0x0600027B RID: 635 RVA: 0x000085D4 File Offset: 0x000067D4
			[DataMember(Name = "SessionId")]
			public uint SessionId { get; private set; }

			// Token: 0x170000A2 RID: 162
			// (get) Token: 0x0600027C RID: 636 RVA: 0x000085DD File Offset: 0x000067DD
			// (set) Token: 0x0600027D RID: 637 RVA: 0x000085E5 File Offset: 0x000067E5
			[DataMember(Name = "TotalCommit")]
			public ulong TotalCommit { get; private set; }

			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x0600027E RID: 638 RVA: 0x000085EE File Offset: 0x000067EE
			// (set) Token: 0x0600027F RID: 639 RVA: 0x000085F6 File Offset: 0x000067F6
			[DataMember(Name = "VirtualSize")]
			public ulong VirtualSize { get; private set; }

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x06000280 RID: 640 RVA: 0x000085FF File Offset: 0x000067FF
			// (set) Token: 0x06000281 RID: 641 RVA: 0x00008607 File Offset: 0x00006807
			[DataMember(Name = "IsRunning")]
			public bool IsRunning { get; private set; }

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x06000282 RID: 642 RVA: 0x00008610 File Offset: 0x00006810
			// (set) Token: 0x06000283 RID: 643 RVA: 0x00008618 File Offset: 0x00006818
			[DataMember(Name = "Publisher")]
			public string Publisher { get; private set; }

			// Token: 0x170000A6 RID: 166
			// (get) Token: 0x06000284 RID: 644 RVA: 0x00008621 File Offset: 0x00006821
			// (set) Token: 0x06000285 RID: 645 RVA: 0x00008629 File Offset: 0x00006829
			[DataMember(Name = "Version")]
			public DevicePortal.AppVersion Version { get; private set; }

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x06000286 RID: 646 RVA: 0x00008632 File Offset: 0x00006832
			// (set) Token: 0x06000287 RID: 647 RVA: 0x0000863A File Offset: 0x0000683A
			[DataMember(Name = "IsXAP")]
			public bool IsXAP { get; private set; }

			// Token: 0x06000288 RID: 648 RVA: 0x00008643 File Offset: 0x00006843
			public override string ToString()
			{
				return string.Format("{0} ({1})", this.AppName, this.Name);
			}
		}

		// Token: 0x0200003B RID: 59
		[DataContract]
		public class GpuAdapter
		{
			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x0600028A RID: 650 RVA: 0x0000865B File Offset: 0x0000685B
			// (set) Token: 0x0600028B RID: 651 RVA: 0x00008663 File Offset: 0x00006863
			[DataMember(Name = "DedicatedMemory")]
			public uint DedicatedMemory { get; private set; }

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x0600028C RID: 652 RVA: 0x0000866C File Offset: 0x0000686C
			// (set) Token: 0x0600028D RID: 653 RVA: 0x00008674 File Offset: 0x00006874
			[DataMember(Name = "DedicatedMemoryUsed")]
			public uint DedicatedMemoryUsed { get; private set; }

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x0600028E RID: 654 RVA: 0x0000867D File Offset: 0x0000687D
			// (set) Token: 0x0600028F RID: 655 RVA: 0x00008685 File Offset: 0x00006885
			[DataMember(Name = "Description")]
			public string Description { get; private set; }

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x06000290 RID: 656 RVA: 0x0000868E File Offset: 0x0000688E
			// (set) Token: 0x06000291 RID: 657 RVA: 0x00008696 File Offset: 0x00006896
			[DataMember(Name = "SystemMemory")]
			public uint SystemMemory { get; private set; }

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x06000292 RID: 658 RVA: 0x0000869F File Offset: 0x0000689F
			// (set) Token: 0x06000293 RID: 659 RVA: 0x000086A7 File Offset: 0x000068A7
			[DataMember(Name = "SystemMemoryUsed")]
			public uint SystemMemoryUsed { get; private set; }

			// Token: 0x170000AD RID: 173
			// (get) Token: 0x06000294 RID: 660 RVA: 0x000086B0 File Offset: 0x000068B0
			// (set) Token: 0x06000295 RID: 661 RVA: 0x000086B8 File Offset: 0x000068B8
			[DataMember(Name = "EnginesUtilization")]
			public List<float> EnginesUtilization { get; private set; }
		}

		// Token: 0x0200003C RID: 60
		[DataContract]
		public class GpuPerformanceData
		{
			// Token: 0x170000AE RID: 174
			// (get) Token: 0x06000297 RID: 663 RVA: 0x000086C1 File Offset: 0x000068C1
			// (set) Token: 0x06000298 RID: 664 RVA: 0x000086C9 File Offset: 0x000068C9
			[DataMember(Name = "AvailableAdapters")]
			public List<DevicePortal.GpuAdapter> Adapters { get; private set; }
		}

		// Token: 0x0200003D RID: 61
		[DataContract]
		public class NetworkPerformanceData
		{
			// Token: 0x170000AF RID: 175
			// (get) Token: 0x0600029A RID: 666 RVA: 0x000086D2 File Offset: 0x000068D2
			// (set) Token: 0x0600029B RID: 667 RVA: 0x000086DA File Offset: 0x000068DA
			[DataMember(Name = "NetworkInBytes")]
			public int BytesIn { get; private set; }

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x0600029C RID: 668 RVA: 0x000086E3 File Offset: 0x000068E3
			// (set) Token: 0x0600029D RID: 669 RVA: 0x000086EB File Offset: 0x000068EB
			[DataMember(Name = "NetworkOutBytes")]
			public int BytesOut { get; private set; }
		}

		// Token: 0x0200003E RID: 62
		[DataContract]
		public class RunningProcesses
		{
			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x0600029F RID: 671 RVA: 0x000086F4 File Offset: 0x000068F4
			// (set) Token: 0x060002A0 RID: 672 RVA: 0x000086FC File Offset: 0x000068FC
			[DataMember(Name = "Processes")]
			public List<DevicePortal.DeviceProcessInfo> Processes { get; private set; }

			// Token: 0x060002A1 RID: 673 RVA: 0x00008708 File Offset: 0x00006908
			public bool Contains(int processId)
			{
				bool result = false;
				using (List<DevicePortal.DeviceProcessInfo>.Enumerator enumerator = this.Processes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if ((ulong)enumerator.Current.ProcessId == (ulong)((long)processId))
						{
							result = true;
							break;
						}
					}
				}
				return result;
			}

			// Token: 0x060002A2 RID: 674 RVA: 0x00008764 File Offset: 0x00006964
			public bool Contains(string packageName, bool caseSensitive = true)
			{
				bool result = false;
				using (List<DevicePortal.DeviceProcessInfo>.Enumerator enumerator = this.Processes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (string.Compare(enumerator.Current.PackageFullName, packageName, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) == 0)
						{
							result = true;
							break;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x0200003F RID: 63
		[DataContract]
		public class SystemPerformanceInformation
		{
			// Token: 0x170000B2 RID: 178
			// (get) Token: 0x060002A4 RID: 676 RVA: 0x000087CC File Offset: 0x000069CC
			// (set) Token: 0x060002A5 RID: 677 RVA: 0x000087D4 File Offset: 0x000069D4
			[DataMember(Name = "AvailablePages")]
			public int AvailablePages { get; private set; }

			// Token: 0x170000B3 RID: 179
			// (get) Token: 0x060002A6 RID: 678 RVA: 0x000087DD File Offset: 0x000069DD
			// (set) Token: 0x060002A7 RID: 679 RVA: 0x000087E5 File Offset: 0x000069E5
			[DataMember(Name = "CommitLimit")]
			public int CommitLimit { get; private set; }

			// Token: 0x170000B4 RID: 180
			// (get) Token: 0x060002A8 RID: 680 RVA: 0x000087EE File Offset: 0x000069EE
			// (set) Token: 0x060002A9 RID: 681 RVA: 0x000087F6 File Offset: 0x000069F6
			[DataMember(Name = "CommittedPages")]
			public int CommittedPages { get; private set; }

			// Token: 0x170000B5 RID: 181
			// (get) Token: 0x060002AA RID: 682 RVA: 0x000087FF File Offset: 0x000069FF
			// (set) Token: 0x060002AB RID: 683 RVA: 0x00008807 File Offset: 0x00006A07
			[DataMember(Name = "CpuLoad")]
			public int CpuLoad { get; private set; }

			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x060002AC RID: 684 RVA: 0x00008810 File Offset: 0x00006A10
			// (set) Token: 0x060002AD RID: 685 RVA: 0x00008818 File Offset: 0x00006A18
			[DataMember(Name = "IOOtherSpeed")]
			public int IoOtherSpeed { get; private set; }

			// Token: 0x170000B7 RID: 183
			// (get) Token: 0x060002AE RID: 686 RVA: 0x00008821 File Offset: 0x00006A21
			// (set) Token: 0x060002AF RID: 687 RVA: 0x00008829 File Offset: 0x00006A29
			[DataMember(Name = "IOReadSpeed")]
			public int IoReadSpeed { get; private set; }

			// Token: 0x170000B8 RID: 184
			// (get) Token: 0x060002B0 RID: 688 RVA: 0x00008832 File Offset: 0x00006A32
			// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000883A File Offset: 0x00006A3A
			[DataMember(Name = "IOWriteSpeed")]
			public int IoWriteSpeed { get; private set; }

			// Token: 0x170000B9 RID: 185
			// (get) Token: 0x060002B2 RID: 690 RVA: 0x00008843 File Offset: 0x00006A43
			// (set) Token: 0x060002B3 RID: 691 RVA: 0x0000884B File Offset: 0x00006A4B
			[DataMember(Name = "NonPagedPoolPages")]
			public int NonPagedPoolPages { get; private set; }

			// Token: 0x170000BA RID: 186
			// (get) Token: 0x060002B4 RID: 692 RVA: 0x00008854 File Offset: 0x00006A54
			// (set) Token: 0x060002B5 RID: 693 RVA: 0x0000885C File Offset: 0x00006A5C
			[DataMember(Name = "PageSize")]
			public int PageSize { get; private set; }

			// Token: 0x170000BB RID: 187
			// (get) Token: 0x060002B6 RID: 694 RVA: 0x00008865 File Offset: 0x00006A65
			// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000886D File Offset: 0x00006A6D
			[DataMember(Name = "PagedPoolPages")]
			public int PagedPoolPages { get; private set; }

			// Token: 0x170000BC RID: 188
			// (get) Token: 0x060002B8 RID: 696 RVA: 0x00008876 File Offset: 0x00006A76
			// (set) Token: 0x060002B9 RID: 697 RVA: 0x0000887E File Offset: 0x00006A7E
			[DataMember(Name = "TotalInstalledInKb")]
			public int TotalInstalledKb { get; private set; }

			// Token: 0x170000BD RID: 189
			// (get) Token: 0x060002BA RID: 698 RVA: 0x00008887 File Offset: 0x00006A87
			// (set) Token: 0x060002BB RID: 699 RVA: 0x0000888F File Offset: 0x00006A8F
			[DataMember(Name = "TotalPages")]
			public int TotalPages { get; private set; }

			// Token: 0x170000BE RID: 190
			// (get) Token: 0x060002BC RID: 700 RVA: 0x00008898 File Offset: 0x00006A98
			// (set) Token: 0x060002BD RID: 701 RVA: 0x000088A0 File Offset: 0x00006AA0
			[DataMember(Name = "GPUData")]
			public DevicePortal.GpuPerformanceData GpuData { get; private set; }

			// Token: 0x170000BF RID: 191
			// (get) Token: 0x060002BE RID: 702 RVA: 0x000088A9 File Offset: 0x00006AA9
			// (set) Token: 0x060002BF RID: 703 RVA: 0x000088B1 File Offset: 0x00006AB1
			[DataMember(Name = "NetworkingData")]
			public DevicePortal.NetworkPerformanceData NetworkData { get; private set; }
		}

		// Token: 0x02000040 RID: 64
		[DataContract]
		public class WifiInterface
		{
			// Token: 0x170000C0 RID: 192
			// (get) Token: 0x060002C1 RID: 705 RVA: 0x000088BA File Offset: 0x00006ABA
			// (set) Token: 0x060002C2 RID: 706 RVA: 0x000088C2 File Offset: 0x00006AC2
			[DataMember(Name = "Description")]
			public string Description { get; private set; }

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x060002C3 RID: 707 RVA: 0x000088CB File Offset: 0x00006ACB
			// (set) Token: 0x060002C4 RID: 708 RVA: 0x000088D3 File Offset: 0x00006AD3
			[DataMember(Name = "GUID")]
			public Guid Guid { get; private set; }

			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x060002C5 RID: 709 RVA: 0x000088DC File Offset: 0x00006ADC
			// (set) Token: 0x060002C6 RID: 710 RVA: 0x000088E4 File Offset: 0x00006AE4
			[DataMember(Name = "Index")]
			public int Index { get; private set; }

			// Token: 0x170000C3 RID: 195
			// (get) Token: 0x060002C7 RID: 711 RVA: 0x000088ED File Offset: 0x00006AED
			// (set) Token: 0x060002C8 RID: 712 RVA: 0x000088F5 File Offset: 0x00006AF5
			[DataMember(Name = "ProfilesList")]
			public List<DevicePortal.WifiNetworkProfile> Profiles { get; private set; }
		}

		// Token: 0x02000041 RID: 65
		[DataContract]
		public class WifiInterfaces
		{
			// Token: 0x170000C4 RID: 196
			// (get) Token: 0x060002CA RID: 714 RVA: 0x000088FE File Offset: 0x00006AFE
			// (set) Token: 0x060002CB RID: 715 RVA: 0x00008906 File Offset: 0x00006B06
			[DataMember(Name = "Interfaces")]
			public List<DevicePortal.WifiInterface> Interfaces { get; private set; }
		}

		// Token: 0x02000042 RID: 66
		[DataContract]
		public class WifiNetworks
		{
			// Token: 0x170000C5 RID: 197
			// (get) Token: 0x060002CD RID: 717 RVA: 0x0000890F File Offset: 0x00006B0F
			// (set) Token: 0x060002CE RID: 718 RVA: 0x00008917 File Offset: 0x00006B17
			[DataMember(Name = "AvailableNetworks")]
			public List<DevicePortal.WifiNetworkInfo> AvailableNetworks { get; private set; }
		}

		// Token: 0x02000043 RID: 67
		[DataContract]
		public class WifiNetworkInfo
		{
			// Token: 0x170000C6 RID: 198
			// (get) Token: 0x060002D0 RID: 720 RVA: 0x00008920 File Offset: 0x00006B20
			// (set) Token: 0x060002D1 RID: 721 RVA: 0x00008928 File Offset: 0x00006B28
			[DataMember(Name = "AlreadyConnected")]
			public bool IsConnected { get; private set; }

			// Token: 0x170000C7 RID: 199
			// (get) Token: 0x060002D2 RID: 722 RVA: 0x00008931 File Offset: 0x00006B31
			// (set) Token: 0x060002D3 RID: 723 RVA: 0x00008939 File Offset: 0x00006B39
			[DataMember(Name = "AuthenticationAlgorithm")]
			public string AuthenticationAlgorithm { get; private set; }

			// Token: 0x170000C8 RID: 200
			// (get) Token: 0x060002D4 RID: 724 RVA: 0x00008942 File Offset: 0x00006B42
			// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000894A File Offset: 0x00006B4A
			[DataMember(Name = "Channel")]
			public int Channel { get; private set; }

			// Token: 0x170000C9 RID: 201
			// (get) Token: 0x060002D6 RID: 726 RVA: 0x00008953 File Offset: 0x00006B53
			// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000895B File Offset: 0x00006B5B
			[DataMember(Name = "CipherAlgorithm")]
			public string CipherAlgorithm { get; private set; }

			// Token: 0x170000CA RID: 202
			// (get) Token: 0x060002D8 RID: 728 RVA: 0x00008964 File Offset: 0x00006B64
			// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000896C File Offset: 0x00006B6C
			[DataMember(Name = "Connectable")]
			public bool IsConnectable { get; private set; }

			// Token: 0x170000CB RID: 203
			// (get) Token: 0x060002DA RID: 730 RVA: 0x00008975 File Offset: 0x00006B75
			// (set) Token: 0x060002DB RID: 731 RVA: 0x0000897D File Offset: 0x00006B7D
			[DataMember(Name = "InfrastructureType")]
			public string InfrastructureType { get; private set; }

			// Token: 0x170000CC RID: 204
			// (get) Token: 0x060002DC RID: 732 RVA: 0x00008986 File Offset: 0x00006B86
			// (set) Token: 0x060002DD RID: 733 RVA: 0x0000898E File Offset: 0x00006B8E
			[DataMember(Name = "ProfileAvailable")]
			public bool IsProfileAvailable { get; private set; }

			// Token: 0x170000CD RID: 205
			// (get) Token: 0x060002DE RID: 734 RVA: 0x00008997 File Offset: 0x00006B97
			// (set) Token: 0x060002DF RID: 735 RVA: 0x0000899F File Offset: 0x00006B9F
			[DataMember(Name = "ProfileName")]
			public string ProfileName { get; private set; }

			// Token: 0x170000CE RID: 206
			// (get) Token: 0x060002E0 RID: 736 RVA: 0x000089A8 File Offset: 0x00006BA8
			// (set) Token: 0x060002E1 RID: 737 RVA: 0x000089B0 File Offset: 0x00006BB0
			[DataMember(Name = "SSID")]
			public string Ssid { get; private set; }

			// Token: 0x170000CF RID: 207
			// (get) Token: 0x060002E2 RID: 738 RVA: 0x000089B9 File Offset: 0x00006BB9
			// (set) Token: 0x060002E3 RID: 739 RVA: 0x000089C1 File Offset: 0x00006BC1
			[DataMember(Name = "SecurityEnabled")]
			public bool IsSecurityEnabled { get; private set; }

			// Token: 0x170000D0 RID: 208
			// (get) Token: 0x060002E4 RID: 740 RVA: 0x000089CA File Offset: 0x00006BCA
			// (set) Token: 0x060002E5 RID: 741 RVA: 0x000089D2 File Offset: 0x00006BD2
			[DataMember(Name = "SignalQuality")]
			public int SignalQuality { get; private set; }

			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x060002E6 RID: 742 RVA: 0x000089DB File Offset: 0x00006BDB
			// (set) Token: 0x060002E7 RID: 743 RVA: 0x000089E3 File Offset: 0x00006BE3
			[DataMember(Name = "BSSID")]
			public List<int> Bssid { get; private set; }

			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x060002E8 RID: 744 RVA: 0x000089EC File Offset: 0x00006BEC
			// (set) Token: 0x060002E9 RID: 745 RVA: 0x000089F4 File Offset: 0x00006BF4
			[DataMember(Name = "PhysicalTypes")]
			public List<string> NetworkTypes { get; private set; }
		}

		// Token: 0x02000044 RID: 68
		[DataContract]
		public class WifiNetworkProfile
		{
			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x060002EB RID: 747 RVA: 0x000089FD File Offset: 0x00006BFD
			// (set) Token: 0x060002EC RID: 748 RVA: 0x00008A05 File Offset: 0x00006C05
			[DataMember(Name = "GroupPolicyProfile")]
			public bool IsGroupPolicyProfile { get; private set; }

			// Token: 0x170000D4 RID: 212
			// (get) Token: 0x060002ED RID: 749 RVA: 0x00008A0E File Offset: 0x00006C0E
			// (set) Token: 0x060002EE RID: 750 RVA: 0x00008A16 File Offset: 0x00006C16
			[DataMember(Name = "Name")]
			public string Name { get; private set; }

			// Token: 0x170000D5 RID: 213
			// (get) Token: 0x060002EF RID: 751 RVA: 0x00008A1F File Offset: 0x00006C1F
			// (set) Token: 0x060002F0 RID: 752 RVA: 0x00008A27 File Offset: 0x00006C27
			[DataMember(Name = "PerUserProfile")]
			public bool IsPerUserProfile { get; private set; }
		}

		// Token: 0x02000045 RID: 69
		[DataContract]
		public class WerFiles
		{
			// Token: 0x170000D6 RID: 214
			// (get) Token: 0x060002F2 RID: 754 RVA: 0x00008A30 File Offset: 0x00006C30
			// (set) Token: 0x060002F3 RID: 755 RVA: 0x00008A38 File Offset: 0x00006C38
			[DataMember(Name = "Files")]
			public List<DevicePortal.WerFileInformation> Files { get; private set; }
		}

		// Token: 0x02000046 RID: 70
		[DataContract]
		public class WerFileInformation
		{
			// Token: 0x170000D7 RID: 215
			// (get) Token: 0x060002F5 RID: 757 RVA: 0x00008A41 File Offset: 0x00006C41
			// (set) Token: 0x060002F6 RID: 758 RVA: 0x00008A49 File Offset: 0x00006C49
			[DataMember(Name = "Name")]
			public string Name { get; private set; }

			// Token: 0x170000D8 RID: 216
			// (get) Token: 0x060002F7 RID: 759 RVA: 0x00008A52 File Offset: 0x00006C52
			// (set) Token: 0x060002F8 RID: 760 RVA: 0x00008A5A File Offset: 0x00006C5A
			[DataMember(Name = "Size")]
			public int Size { get; private set; }
		}

		// Token: 0x02000047 RID: 71
		[DataContract]
		public class WerDeviceReports
		{
			// Token: 0x170000D9 RID: 217
			// (get) Token: 0x060002FA RID: 762 RVA: 0x00008A63 File Offset: 0x00006C63
			// (set) Token: 0x060002FB RID: 763 RVA: 0x00008A6B File Offset: 0x00006C6B
			[DataMember(Name = "WerReports")]
			public List<DevicePortal.WerUserReports> UserReports { get; private set; }

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x060002FC RID: 764 RVA: 0x00008A74 File Offset: 0x00006C74
			public DevicePortal.WerUserReports SystemErrorReports
			{
				get
				{
					return this.UserReports.First((DevicePortal.WerUserReports x) => x.UserName == "SYSTEM");
				}
			}
		}

		// Token: 0x02000048 RID: 72
		[DataContract]
		public class WerUserReports
		{
			// Token: 0x170000DB RID: 219
			// (get) Token: 0x060002FE RID: 766 RVA: 0x00008AA0 File Offset: 0x00006CA0
			// (set) Token: 0x060002FF RID: 767 RVA: 0x00008AA8 File Offset: 0x00006CA8
			[DataMember(Name = "User")]
			public string UserName { get; private set; }

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x06000300 RID: 768 RVA: 0x00008AB1 File Offset: 0x00006CB1
			// (set) Token: 0x06000301 RID: 769 RVA: 0x00008AB9 File Offset: 0x00006CB9
			[DataMember(Name = "Reports")]
			public List<DevicePortal.WerReportInformation> Reports { get; private set; }
		}

		// Token: 0x02000049 RID: 73
		[DataContract]
		public class WerReportInformation
		{
			// Token: 0x170000DD RID: 221
			// (get) Token: 0x06000303 RID: 771 RVA: 0x00008AC2 File Offset: 0x00006CC2
			// (set) Token: 0x06000304 RID: 772 RVA: 0x00008ACA File Offset: 0x00006CCA
			[DataMember(Name = "CreationTime")]
			public ulong CreationTime { get; private set; }

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x06000305 RID: 773 RVA: 0x00008AD3 File Offset: 0x00006CD3
			// (set) Token: 0x06000306 RID: 774 RVA: 0x00008ADB File Offset: 0x00006CDB
			[DataMember(Name = "Name")]
			public string Name { get; private set; }

			// Token: 0x170000DF RID: 223
			// (get) Token: 0x06000307 RID: 775 RVA: 0x00008AE4 File Offset: 0x00006CE4
			// (set) Token: 0x06000308 RID: 776 RVA: 0x00008AEC File Offset: 0x00006CEC
			[DataMember(Name = "Type")]
			public string Type { get; private set; }
		}

		// Token: 0x0200004A RID: 74
		public enum HttpMethods
		{
			// Token: 0x040001D5 RID: 469
			Get,
			// Token: 0x040001D6 RID: 470
			Put,
			// Token: 0x040001D7 RID: 471
			Post,
			// Token: 0x040001D8 RID: 472
			Delete,
			// Token: 0x040001D9 RID: 473
			WebSocket
		}

		// Token: 0x0200004B RID: 75
		public enum ProcessStatus
		{
			// Token: 0x040001DB RID: 475
			Running,
			// Token: 0x040001DC RID: 476
			Stopped
		}

		// Token: 0x0200004C RID: 76
		[DataContract]
		public class HolographicServices
		{
			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x0600030A RID: 778 RVA: 0x00008AF5 File Offset: 0x00006CF5
			// (set) Token: 0x0600030B RID: 779 RVA: 0x00008AFD File Offset: 0x00006CFD
			[DataMember(Name = "SoftwareStatus")]
			public DevicePortal.HolographicSoftwareStatus Status { get; private set; }
		}

		// Token: 0x0200004D RID: 77
		[DataContract]
		public class HolographicSoftwareStatus
		{
			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x0600030D RID: 781 RVA: 0x00008B06 File Offset: 0x00006D06
			// (set) Token: 0x0600030E RID: 782 RVA: 0x00008B0E File Offset: 0x00006D0E
			[DataMember(Name = "dwm.exe")]
			public DevicePortal.ServiceStatus Dwm { get; private set; }

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x0600030F RID: 783 RVA: 0x00008B17 File Offset: 0x00006D17
			// (set) Token: 0x06000310 RID: 784 RVA: 0x00008B1F File Offset: 0x00006D1F
			[DataMember(Name = "holoshellapp.exe")]
			public DevicePortal.ServiceStatus HoloShellApp { get; private set; }

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x06000311 RID: 785 RVA: 0x00008B28 File Offset: 0x00006D28
			// (set) Token: 0x06000312 RID: 786 RVA: 0x00008B30 File Offset: 0x00006D30
			[DataMember(Name = "holosi.exe")]
			public DevicePortal.ServiceStatus HoloSi { get; private set; }

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x06000313 RID: 787 RVA: 0x00008B39 File Offset: 0x00006D39
			// (set) Token: 0x06000314 RID: 788 RVA: 0x00008B41 File Offset: 0x00006D41
			[DataMember(Name = "mixedrealitycapture.exe")]
			public DevicePortal.ServiceStatus MixedRealitytCapture { get; private set; }

			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x06000315 RID: 789 RVA: 0x00008B4A File Offset: 0x00006D4A
			// (set) Token: 0x06000316 RID: 790 RVA: 0x00008B52 File Offset: 0x00006D52
			[DataMember(Name = "sihost.exe")]
			public DevicePortal.ServiceStatus SiHost { get; private set; }

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x06000317 RID: 791 RVA: 0x00008B5B File Offset: 0x00006D5B
			// (set) Token: 0x06000318 RID: 792 RVA: 0x00008B63 File Offset: 0x00006D63
			[DataMember(Name = "spectrum.exe")]
			public DevicePortal.ServiceStatus Spectrum { get; private set; }
		}

		// Token: 0x0200004E RID: 78
		[DataContract]
		public class InterPupilaryDistance
		{
			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x0600031A RID: 794 RVA: 0x00008B6C File Offset: 0x00006D6C
			// (set) Token: 0x0600031B RID: 795 RVA: 0x00008B74 File Offset: 0x00006D74
			[DataMember(Name = "ipd")]
			public int IpdRaw { get; private set; }

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x0600031C RID: 796 RVA: 0x00008B7D File Offset: 0x00006D7D
			// (set) Token: 0x0600031D RID: 797 RVA: 0x00008B8C File Offset: 0x00006D8C
			public float Ipd
			{
				get
				{
					return (float)this.IpdRaw / 1000f;
				}
				set
				{
					this.IpdRaw = (int)(value * 1000f);
				}
			}
		}

		// Token: 0x0200004F RID: 79
		[DataContract]
		public class ServiceStatus
		{
			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x0600031F RID: 799 RVA: 0x00008B9C File Offset: 0x00006D9C
			// (set) Token: 0x06000320 RID: 800 RVA: 0x00008BA4 File Offset: 0x00006DA4
			[DataMember(Name = "Expected")]
			public string ExpectedRaw { get; private set; }

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x06000321 RID: 801 RVA: 0x00008BAD File Offset: 0x00006DAD
			// (set) Token: 0x06000322 RID: 802 RVA: 0x00008BB5 File Offset: 0x00006DB5
			[DataMember(Name = "Observed")]
			public string ObservedRaw { get; private set; }

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x06000323 RID: 803 RVA: 0x00008BBE File Offset: 0x00006DBE
			public DevicePortal.ProcessStatus Expected
			{
				get
				{
					if (!(this.ExpectedRaw == "Running"))
					{
						return DevicePortal.ProcessStatus.Stopped;
					}
					return DevicePortal.ProcessStatus.Running;
				}
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x06000324 RID: 804 RVA: 0x00008BD5 File Offset: 0x00006DD5
			public DevicePortal.ProcessStatus Observed
			{
				get
				{
					if (!(this.ObservedRaw == "Running"))
					{
						return DevicePortal.ProcessStatus.Stopped;
					}
					return DevicePortal.ProcessStatus.Running;
				}
			}
		}

		// Token: 0x02000050 RID: 80
		[DataContract]
		public class WebManagementHttpSettings
		{
			// Token: 0x170000ED RID: 237
			// (get) Token: 0x06000326 RID: 806 RVA: 0x00008BEC File Offset: 0x00006DEC
			// (set) Token: 0x06000327 RID: 807 RVA: 0x00008BF4 File Offset: 0x00006DF4
			[DataMember(Name = "httpsRequired")]
			public bool IsHttpsRequired { get; private set; }
		}

		// Token: 0x02000051 RID: 81
		public enum SimulationControlMode
		{
			// Token: 0x040001E9 RID: 489
			Default,
			// Token: 0x040001EA RID: 490
			Simulation
		}

		// Token: 0x02000052 RID: 82
		public enum SimulationControlStreamPriority
		{
			// Token: 0x040001EC RID: 492
			Low,
			// Token: 0x040001ED RID: 493
			Normal
		}

		// Token: 0x02000053 RID: 83
		[DataContract]
		public class PerceptionSimulationControlMode
		{
			// Token: 0x170000EE RID: 238
			// (get) Token: 0x06000329 RID: 809 RVA: 0x00008BFD File Offset: 0x00006DFD
			// (set) Token: 0x0600032A RID: 810 RVA: 0x00008C05 File Offset: 0x00006E05
			[DataMember(Name = "mode")]
			public DevicePortal.SimulationControlMode Mode { get; private set; }
		}

		// Token: 0x02000054 RID: 84
		[DataContract]
		public class PerceptionSimulationControlStreamId
		{
			// Token: 0x170000EF RID: 239
			// (get) Token: 0x0600032C RID: 812 RVA: 0x00008C0E File Offset: 0x00006E0E
			// (set) Token: 0x0600032D RID: 813 RVA: 0x00008C16 File Offset: 0x00006E16
			[DataMember(Name = "streamId")]
			public string StreamId { get; private set; }
		}

		// Token: 0x02000055 RID: 85
		[DataContract]
		public class ThermalStage
		{
			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x0600032F RID: 815 RVA: 0x00008C1F File Offset: 0x00006E1F
			// (set) Token: 0x06000330 RID: 816 RVA: 0x00008C27 File Offset: 0x00006E27
			[DataMember(Name = "CurrentStage")]
			public int StageRaw { get; private set; }

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x06000331 RID: 817 RVA: 0x00008C30 File Offset: 0x00006E30
			public ThermalStages Stage
			{
				get
				{
					ThermalStages thermalStages = ThermalStages.Unknown;
					try
					{
						thermalStages = (ThermalStages)Enum.ToObject(typeof(ThermalStages), this.StageRaw);
						if (!Enum.IsDefined(typeof(ThermalStages), thermalStages))
						{
							thermalStages = ThermalStages.Unknown;
						}
					}
					catch
					{
						thermalStages = ThermalStages.Unknown;
					}
					return thermalStages;
				}
			}
		}

		// Token: 0x02000056 RID: 86
		[DataContract]
		public class MrcFileList
		{
			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x06000333 RID: 819 RVA: 0x00008C98 File Offset: 0x00006E98
			// (set) Token: 0x06000334 RID: 820 RVA: 0x00008CA0 File Offset: 0x00006EA0
			[DataMember(Name = "MrcRecordings")]
			public List<DevicePortal.MrcFileInformation> Files { get; private set; }
		}

		// Token: 0x02000057 RID: 87
		[DataContract]
		public class MrcFileInformation
		{
			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x06000336 RID: 822 RVA: 0x00008CA9 File Offset: 0x00006EA9
			// (set) Token: 0x06000337 RID: 823 RVA: 0x00008CB1 File Offset: 0x00006EB1
			[DataMember(Name = "CreationTime")]
			public long CreationTimeRaw { get; private set; }

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x06000338 RID: 824 RVA: 0x00008CBA File Offset: 0x00006EBA
			// (set) Token: 0x06000339 RID: 825 RVA: 0x00008CC2 File Offset: 0x00006EC2
			[DataMember(Name = "FileName")]
			public string FileName { get; private set; }

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x0600033A RID: 826 RVA: 0x00008CCB File Offset: 0x00006ECB
			// (set) Token: 0x0600033B RID: 827 RVA: 0x00008CD3 File Offset: 0x00006ED3
			[DataMember(Name = "FileSize")]
			public uint FileSize { get; private set; }

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x0600033C RID: 828 RVA: 0x00008CDC File Offset: 0x00006EDC
			// (set) Token: 0x0600033D RID: 829 RVA: 0x00008CE4 File Offset: 0x00006EE4
			public byte[] Thumbnail { get; internal set; }

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x0600033E RID: 830 RVA: 0x00008CED File Offset: 0x00006EED
			public DateTime Created
			{
				get
				{
					return new DateTime(this.CreationTimeRaw);
				}
			}
		}

		// Token: 0x02000058 RID: 88
		[DataContract]
		public class MrcStatus
		{
			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x06000340 RID: 832 RVA: 0x00008CFA File Offset: 0x00006EFA
			// (set) Token: 0x06000341 RID: 833 RVA: 0x00008D02 File Offset: 0x00006F02
			[DataMember(Name = "IsRecording")]
			public bool IsRecording { get; private set; }

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x06000342 RID: 834 RVA: 0x00008D0B File Offset: 0x00006F0B
			// (set) Token: 0x06000343 RID: 835 RVA: 0x00008D13 File Offset: 0x00006F13
			[DataMember(Name = "ProcessStatus")]
			public DevicePortal.MrcProcessStatus Status { get; private set; }
		}

		// Token: 0x02000059 RID: 89
		[DataContract]
		public class MrcProcessStatus
		{
			// Token: 0x170000FA RID: 250
			// (get) Token: 0x06000345 RID: 837 RVA: 0x00008D1C File Offset: 0x00006F1C
			// (set) Token: 0x06000346 RID: 838 RVA: 0x00008D24 File Offset: 0x00006F24
			[DataMember(Name = "MrcProcess")]
			public string MrcProcessRaw { get; private set; }

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x06000347 RID: 839 RVA: 0x00008D2D File Offset: 0x00006F2D
			public DevicePortal.ProcessStatus MrcProcess
			{
				get
				{
					if (!(this.MrcProcessRaw == "Running"))
					{
						return DevicePortal.ProcessStatus.Stopped;
					}
					return DevicePortal.ProcessStatus.Running;
				}
			}
		}

		// Token: 0x0200005A RID: 90
		[DataContract]
		public class MrcSetting
		{
			// Token: 0x170000FC RID: 252
			// (get) Token: 0x06000349 RID: 841 RVA: 0x00008D44 File Offset: 0x00006F44
			// (set) Token: 0x0600034A RID: 842 RVA: 0x00008D4C File Offset: 0x00006F4C
			[DataMember(Name = "Setting")]
			public string Setting { get; set; }

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x0600034B RID: 843 RVA: 0x00008D55 File Offset: 0x00006F55
			// (set) Token: 0x0600034C RID: 844 RVA: 0x00008D5D File Offset: 0x00006F5D
			[DataMember(Name = "Value")]
			public object Value { get; set; }
		}

		// Token: 0x0200005B RID: 91
		[DataContract]
		public class MrcSettings
		{
			// Token: 0x170000FE RID: 254
			// (get) Token: 0x0600034E RID: 846 RVA: 0x00008D66 File Offset: 0x00006F66
			// (set) Token: 0x0600034F RID: 847 RVA: 0x00008D6E File Offset: 0x00006F6E
			[DataMember(Name = "MrcSettings")]
			public List<DevicePortal.MrcSetting> Settings { get; private set; }

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x06000350 RID: 848 RVA: 0x00008D78 File Offset: 0x00006F78
			// (set) Token: 0x06000351 RID: 849 RVA: 0x00008D9C File Offset: 0x00006F9C
			public bool IncludeHolograms
			{
				get
				{
					object setting = this.GetSetting("EnableHolograms");
					return setting == null || (bool)setting;
				}
				set
				{
					this.SetSetting("EnableHolograms", value);
				}
			}

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x06000352 RID: 850 RVA: 0x00008DB0 File Offset: 0x00006FB0
			// (set) Token: 0x06000353 RID: 851 RVA: 0x00008DD4 File Offset: 0x00006FD4
			public bool IncludeColorCamera
			{
				get
				{
					object setting = this.GetSetting("EnableCamera");
					return setting == null || (bool)setting;
				}
				set
				{
					this.SetSetting("EnableCamera", value);
				}
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x06000354 RID: 852 RVA: 0x00008DE8 File Offset: 0x00006FE8
			// (set) Token: 0x06000355 RID: 853 RVA: 0x00008E0C File Offset: 0x0000700C
			public bool IncludeMicrophone
			{
				get
				{
					object setting = this.GetSetting("EnableMicrophone");
					return setting == null || (bool)setting;
				}
				set
				{
					this.SetSetting("EnableMicrophone", value);
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x06000356 RID: 854 RVA: 0x00008E20 File Offset: 0x00007020
			// (set) Token: 0x06000357 RID: 855 RVA: 0x00008E44 File Offset: 0x00007044
			public bool IncludeAudio
			{
				get
				{
					object setting = this.GetSetting("EnableSystemAudio");
					return setting == null || (bool)setting;
				}
				set
				{
					this.SetSetting("EnableSystemAudio", value);
				}
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x06000358 RID: 856 RVA: 0x00008E58 File Offset: 0x00007058
			// (set) Token: 0x06000359 RID: 857 RVA: 0x00008E7C File Offset: 0x0000707C
			public int VideoStabilizationBuffer
			{
				get
				{
					object setting = this.GetSetting("VideoStabilizationBuffer");
					if (setting == null)
					{
						return 0;
					}
					return (int)setting;
				}
				set
				{
					if (value < 0)
					{
						throw new ArgumentException("The video stabilization buffer value must be >= 0");
					}
					this.SetSetting("VideoStabilizationBuffer", value);
				}
			}

			// Token: 0x0600035A RID: 858 RVA: 0x00008EA0 File Offset: 0x000070A0
			private object GetSetting(string settingName)
			{
				object result = null;
				foreach (DevicePortal.MrcSetting mrcSetting in this.Settings)
				{
					if (mrcSetting.Setting == settingName)
					{
						result = mrcSetting.Value;
						break;
					}
				}
				return result;
			}

			// Token: 0x0600035B RID: 859 RVA: 0x00008F08 File Offset: 0x00007108
			private void SetSetting(string settingName, object value)
			{
				DevicePortal.MrcSetting mrcSetting = null;
				foreach (DevicePortal.MrcSetting mrcSetting2 in this.Settings)
				{
					if (mrcSetting2.Setting == settingName)
					{
						mrcSetting = mrcSetting2;
						break;
					}
				}
				if (mrcSetting != null)
				{
					mrcSetting.Value = value;
					return;
				}
				mrcSetting = new DevicePortal.MrcSetting();
				mrcSetting.Setting = settingName;
				mrcSetting.Value = value;
				this.Settings.Add(mrcSetting);
			}
		}

		// Token: 0x0200005C RID: 92
		public enum HolographicSimulationPlaybackStates
		{
			// Token: 0x040001FD RID: 509
			Stopped,
			// Token: 0x040001FE RID: 510
			Playing,
			// Token: 0x040001FF RID: 511
			Paused,
			// Token: 0x04000200 RID: 512
			Complete,
			// Token: 0x04000201 RID: 513
			Unexpected = 9999
		}

		// Token: 0x0200005D RID: 93
		[DataContract]
		public class HolographicSimulationDataTypes
		{
			// Token: 0x17000104 RID: 260
			// (get) Token: 0x0600035D RID: 861 RVA: 0x00008F94 File Offset: 0x00007194
			// (set) Token: 0x0600035E RID: 862 RVA: 0x00008F9C File Offset: 0x0000719C
			[DataMember(Name = "hands")]
			public bool IncludesHands { get; private set; }

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x0600035F RID: 863 RVA: 0x00008FA5 File Offset: 0x000071A5
			// (set) Token: 0x06000360 RID: 864 RVA: 0x00008FAD File Offset: 0x000071AD
			[DataMember(Name = "head")]
			public bool IncludesHead { get; private set; }

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x06000361 RID: 865 RVA: 0x00008FB6 File Offset: 0x000071B6
			// (set) Token: 0x06000362 RID: 866 RVA: 0x00008FBE File Offset: 0x000071BE
			[DataMember(Name = "environment")]
			public bool IncludesEnvironment { get; private set; }

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x06000363 RID: 867 RVA: 0x00008FC7 File Offset: 0x000071C7
			// (set) Token: 0x06000364 RID: 868 RVA: 0x00008FCF File Offset: 0x000071CF
			[DataMember(Name = "spatialMapping")]
			public bool IncludesSpatialMapping { get; private set; }
		}

		// Token: 0x0200005E RID: 94
		[DataContract]
		public class HolographicSimulationPlaybackFiles
		{
			// Token: 0x17000108 RID: 264
			// (get) Token: 0x06000366 RID: 870 RVA: 0x00008FD8 File Offset: 0x000071D8
			// (set) Token: 0x06000367 RID: 871 RVA: 0x00008FE0 File Offset: 0x000071E0
			[DataMember(Name = "recordings")]
			public List<string> Files { get; private set; }
		}

		// Token: 0x0200005F RID: 95
		[DataContract]
		public class HolographicSimulationPlaybackSessionState
		{
			// Token: 0x17000109 RID: 265
			// (get) Token: 0x06000369 RID: 873 RVA: 0x00008FE9 File Offset: 0x000071E9
			// (set) Token: 0x0600036A RID: 874 RVA: 0x00008FF1 File Offset: 0x000071F1
			[DataMember(Name = "state")]
			public string StateRaw { get; private set; }

			// Token: 0x1700010A RID: 266
			// (get) Token: 0x0600036B RID: 875 RVA: 0x00008FFC File Offset: 0x000071FC
			public DevicePortal.HolographicSimulationPlaybackStates State
			{
				get
				{
					string stateRaw = this.StateRaw;
					DevicePortal.HolographicSimulationPlaybackStates result;
					if (!(stateRaw == "stopped"))
					{
						if (!(stateRaw == "playing"))
						{
							if (!(stateRaw == "paused"))
							{
								if (!(stateRaw == "end"))
								{
									result = DevicePortal.HolographicSimulationPlaybackStates.Unexpected;
								}
								else
								{
									result = DevicePortal.HolographicSimulationPlaybackStates.Complete;
								}
							}
							else
							{
								result = DevicePortal.HolographicSimulationPlaybackStates.Paused;
							}
						}
						else
						{
							result = DevicePortal.HolographicSimulationPlaybackStates.Playing;
						}
					}
					else
					{
						result = DevicePortal.HolographicSimulationPlaybackStates.Stopped;
					}
					return result;
				}
			}
		}

		// Token: 0x02000060 RID: 96
		[DataContract]
		public class HolographicSimulationError
		{
			// Token: 0x1700010B RID: 267
			// (get) Token: 0x0600036D RID: 877 RVA: 0x00009063 File Offset: 0x00007263
			// (set) Token: 0x0600036E RID: 878 RVA: 0x0000906B File Offset: 0x0000726B
			[DataMember(Name = "Reason")]
			public string Reason { get; private set; }
		}

		// Token: 0x02000061 RID: 97
		[DataContract]
		public class HolographicSimulationRecordingStatus
		{
			// Token: 0x1700010C RID: 268
			// (get) Token: 0x06000370 RID: 880 RVA: 0x00009074 File Offset: 0x00007274
			// (set) Token: 0x06000371 RID: 881 RVA: 0x0000907C File Offset: 0x0000727C
			[DataMember(Name = "recording")]
			public bool IsRecording { get; private set; }
		}

		// Token: 0x02000062 RID: 98
		[DataContract]
		private class NullResponse
		{
		}

		// Token: 0x02000063 RID: 99
		[DataContract]
		public class AvailableBluetoothDevicesInfo
		{
			// Token: 0x1700010D RID: 269
			// (get) Token: 0x06000374 RID: 884 RVA: 0x00009085 File Offset: 0x00007285
			// (set) Token: 0x06000375 RID: 885 RVA: 0x0000908D File Offset: 0x0000728D
			[DataMember(Name = "AvailableDevices")]
			public List<DevicePortal.BluetoothDeviceInfo> AvailableDevices { get; private set; }
		}

		// Token: 0x02000064 RID: 100
		public class BluetoothDeviceInfo
		{
			// Token: 0x1700010E RID: 270
			// (get) Token: 0x06000377 RID: 887 RVA: 0x00009096 File Offset: 0x00007296
			// (set) Token: 0x06000378 RID: 888 RVA: 0x0000909E File Offset: 0x0000729E
			[DataMember(Name = "ID")]
			public string ID { get; private set; }

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x06000379 RID: 889 RVA: 0x000090A7 File Offset: 0x000072A7
			// (set) Token: 0x0600037A RID: 890 RVA: 0x000090AF File Offset: 0x000072AF
			[DataMember(Name = "Name")]
			public string Name { get; private set; }
		}

		// Token: 0x02000065 RID: 101
		[DataContract]
		public class PairedBluetoothDevicesInfo
		{
			// Token: 0x17000110 RID: 272
			// (get) Token: 0x0600037C RID: 892 RVA: 0x000090B8 File Offset: 0x000072B8
			// (set) Token: 0x0600037D RID: 893 RVA: 0x000090C0 File Offset: 0x000072C0
			[DataMember(Name = "PairedDevices")]
			public List<DevicePortal.BluetoothDeviceInfo> PairedDevices { get; private set; }
		}

		// Token: 0x02000066 RID: 102
		[DataContract]
		public class PairBluetoothDevicesInfo
		{
			// Token: 0x17000111 RID: 273
			// (get) Token: 0x0600037F RID: 895 RVA: 0x000090C9 File Offset: 0x000072C9
			// (set) Token: 0x06000380 RID: 896 RVA: 0x000090D1 File Offset: 0x000072D1
			[DataMember(Name = "PairResult")]
			public DevicePortal.PairResult PairResult { get; private set; }
		}

		// Token: 0x02000067 RID: 103
		public class PairResult
		{
			// Token: 0x17000112 RID: 274
			// (get) Token: 0x06000382 RID: 898 RVA: 0x000090DA File Offset: 0x000072DA
			// (set) Token: 0x06000383 RID: 899 RVA: 0x000090E2 File Offset: 0x000072E2
			[DataMember(Name = "Result")]
			public string Result { get; private set; }

			// Token: 0x17000113 RID: 275
			// (get) Token: 0x06000384 RID: 900 RVA: 0x000090EB File Offset: 0x000072EB
			// (set) Token: 0x06000385 RID: 901 RVA: 0x000090F3 File Offset: 0x000072F3
			[DataMember(Name = "deviceId")]
			public string DeviceId { get; private set; }

			// Token: 0x17000114 RID: 276
			// (get) Token: 0x06000386 RID: 902 RVA: 0x000090FC File Offset: 0x000072FC
			// (set) Token: 0x06000387 RID: 903 RVA: 0x00009104 File Offset: 0x00007304
			[DataMember(Name = "Pin")]
			public string Pin { get; private set; }
		}

		// Token: 0x02000068 RID: 104
		[DataContract]
		public class TpmSettingsInfo
		{
			// Token: 0x17000115 RID: 277
			// (get) Token: 0x06000389 RID: 905 RVA: 0x0000910D File Offset: 0x0000730D
			// (set) Token: 0x0600038A RID: 906 RVA: 0x00009115 File Offset: 0x00007315
			[DataMember(Name = "TPMStatus")]
			public string TPMStatus { get; private set; }

			// Token: 0x17000116 RID: 278
			// (get) Token: 0x0600038B RID: 907 RVA: 0x0000911E File Offset: 0x0000731E
			// (set) Token: 0x0600038C RID: 908 RVA: 0x00009126 File Offset: 0x00007326
			[DataMember(Name = "TPMFamily")]
			public string TPMFamily { get; private set; }

			// Token: 0x17000117 RID: 279
			// (get) Token: 0x0600038D RID: 909 RVA: 0x0000912F File Offset: 0x0000732F
			// (set) Token: 0x0600038E RID: 910 RVA: 0x00009137 File Offset: 0x00007337
			[DataMember(Name = "TPMFirmware")]
			public string TPMFirmware { get; private set; }

			// Token: 0x17000118 RID: 280
			// (get) Token: 0x0600038F RID: 911 RVA: 0x00009140 File Offset: 0x00007340
			// (set) Token: 0x06000390 RID: 912 RVA: 0x00009148 File Offset: 0x00007348
			[DataMember(Name = "TPMRevision")]
			public string TPMRevision { get; private set; }

			// Token: 0x17000119 RID: 281
			// (get) Token: 0x06000391 RID: 913 RVA: 0x00009151 File Offset: 0x00007351
			// (set) Token: 0x06000392 RID: 914 RVA: 0x00009159 File Offset: 0x00007359
			[DataMember(Name = "TPMType")]
			public string TPMTypes { get; private set; }

			// Token: 0x1700011A RID: 282
			// (get) Token: 0x06000393 RID: 915 RVA: 0x00009162 File Offset: 0x00007362
			// (set) Token: 0x06000394 RID: 916 RVA: 0x0000916A File Offset: 0x0000736A
			[DataMember(Name = "TPMVendor")]
			public string TPMVendor { get; private set; }

			// Token: 0x1700011B RID: 283
			// (get) Token: 0x06000395 RID: 917 RVA: 0x00009173 File Offset: 0x00007373
			// (set) Token: 0x06000396 RID: 918 RVA: 0x0000917B File Offset: 0x0000737B
			[DataMember(Name = "TPMManufacturer")]
			public string TPMManufacturer { get; private set; }
		}

		// Token: 0x02000069 RID: 105
		[DataContract]
		public class TpmAcpiTablesInfo
		{
			// Token: 0x1700011C RID: 284
			// (get) Token: 0x06000398 RID: 920 RVA: 0x00009184 File Offset: 0x00007384
			// (set) Token: 0x06000399 RID: 921 RVA: 0x0000918C File Offset: 0x0000738C
			[DataMember(Name = "AcpiTables")]
			public List<string> AcpiTables { get; private set; }
		}

		// Token: 0x0200006A RID: 106
		[DataContract]
		public class TpmLogicalDeviceSettingsInfo
		{
			// Token: 0x1700011D RID: 285
			// (get) Token: 0x0600039B RID: 923 RVA: 0x00009195 File Offset: 0x00007395
			// (set) Token: 0x0600039C RID: 924 RVA: 0x0000919D File Offset: 0x0000739D
			[DataMember(Name = "AzureUri")]
			public string AzureUri { get; private set; }

			// Token: 0x1700011E RID: 286
			// (get) Token: 0x0600039D RID: 925 RVA: 0x000091A6 File Offset: 0x000073A6
			// (set) Token: 0x0600039E RID: 926 RVA: 0x000091AE File Offset: 0x000073AE
			[DataMember(Name = "DeviceId")]
			public string DeviceId { get; private set; }
		}

		// Token: 0x0200006B RID: 107
		[DataContract]
		public class TpmAzureTokenInfo
		{
			// Token: 0x1700011F RID: 287
			// (get) Token: 0x060003A0 RID: 928 RVA: 0x000091B7 File Offset: 0x000073B7
			// (set) Token: 0x060003A1 RID: 929 RVA: 0x000091BF File Offset: 0x000073BF
			[DataMember(Name = "AzureToken")]
			public string AzureToken { get; private set; }
		}

		// Token: 0x0200006C RID: 108
		[DataContract]
		public class AppsListInfo
		{
			// Token: 0x17000120 RID: 288
			// (get) Token: 0x060003A3 RID: 931 RVA: 0x000091C8 File Offset: 0x000073C8
			// (set) Token: 0x060003A4 RID: 932 RVA: 0x000091D0 File Offset: 0x000073D0
			[DataMember(Name = "DefaultApp")]
			public string DefaultApp { get; private set; }

			// Token: 0x17000121 RID: 289
			// (get) Token: 0x060003A5 RID: 933 RVA: 0x000091D9 File Offset: 0x000073D9
			// (set) Token: 0x060003A6 RID: 934 RVA: 0x000091E1 File Offset: 0x000073E1
			[DataMember(Name = "AppPackages")]
			public List<DevicePortal.AppPackage> AppPackages { get; private set; }
		}

		// Token: 0x0200006D RID: 109
		[DataContract]
		public class AppPackage
		{
			// Token: 0x17000122 RID: 290
			// (get) Token: 0x060003A8 RID: 936 RVA: 0x000091EA File Offset: 0x000073EA
			// (set) Token: 0x060003A9 RID: 937 RVA: 0x000091F2 File Offset: 0x000073F2
			[DataMember(Name = "IsStartup")]
			public bool IsStartup { get; private set; }

			// Token: 0x17000123 RID: 291
			// (get) Token: 0x060003AA RID: 938 RVA: 0x000091FB File Offset: 0x000073FB
			// (set) Token: 0x060003AB RID: 939 RVA: 0x00009203 File Offset: 0x00007403
			[DataMember(Name = "PackageFullName")]
			public string PackageFullName { get; private set; }
		}

		// Token: 0x0200006E RID: 110
		[DataContract]
		public class HeadlessAppsListInfo
		{
			// Token: 0x17000124 RID: 292
			// (get) Token: 0x060003AD RID: 941 RVA: 0x0000920C File Offset: 0x0000740C
			// (set) Token: 0x060003AE RID: 942 RVA: 0x00009214 File Offset: 0x00007414
			[DataMember(Name = "AppPackages")]
			public List<DevicePortal.AppPackage> AppPackages { get; private set; }
		}

		// Token: 0x0200006F RID: 111
		[DataContract]
		public class AudioDeviceListInfo
		{
			// Token: 0x17000125 RID: 293
			// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000921D File Offset: 0x0000741D
			// (set) Token: 0x060003B1 RID: 945 RVA: 0x00009225 File Offset: 0x00007425
			[DataMember(Name = "RenderName")]
			public string RenderName { get; private set; }

			// Token: 0x17000126 RID: 294
			// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000922E File Offset: 0x0000742E
			// (set) Token: 0x060003B3 RID: 947 RVA: 0x00009236 File Offset: 0x00007436
			[DataMember(Name = "RenderVolume")]
			public string RenderVolume { get; private set; }

			// Token: 0x17000127 RID: 295
			// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000923F File Offset: 0x0000743F
			// (set) Token: 0x060003B5 RID: 949 RVA: 0x00009247 File Offset: 0x00007447
			[DataMember(Name = "CaptureName")]
			public string CaptureName { get; private set; }

			// Token: 0x17000128 RID: 296
			// (get) Token: 0x060003B6 RID: 950 RVA: 0x00009250 File Offset: 0x00007450
			// (set) Token: 0x060003B7 RID: 951 RVA: 0x00009258 File Offset: 0x00007458
			[DataMember(Name = "CaptureVolume")]
			public string CaptureVolume { get; private set; }

			// Token: 0x17000129 RID: 297
			// (get) Token: 0x060003B8 RID: 952 RVA: 0x00009261 File Offset: 0x00007461
			// (set) Token: 0x060003B9 RID: 953 RVA: 0x00009269 File Offset: 0x00007469
			[DataMember(Name = "LabelStatus")]
			public string LabelStatus { get; private set; }

			// Token: 0x1700012A RID: 298
			// (get) Token: 0x060003BA RID: 954 RVA: 0x00009272 File Offset: 0x00007472
			// (set) Token: 0x060003BB RID: 955 RVA: 0x0000927A File Offset: 0x0000747A
			[DataMember(Name = "LabelErrorCode")]
			public string LabelErrorCode { get; private set; }
		}

		// Token: 0x02000070 RID: 112
		[DataContract]
		public class RunCommandOutputInfo
		{
			// Token: 0x1700012B RID: 299
			// (get) Token: 0x060003BD RID: 957 RVA: 0x00009283 File Offset: 0x00007483
			// (set) Token: 0x060003BE RID: 958 RVA: 0x0000928B File Offset: 0x0000748B
			[DataMember(Name = "output")]
			public string Output { get; private set; }
		}

		// Token: 0x02000071 RID: 113
		[DataContract]
		public class IscInterfacesInfo
		{
			// Token: 0x1700012C RID: 300
			// (get) Token: 0x060003C0 RID: 960 RVA: 0x00009294 File Offset: 0x00007494
			// (set) Token: 0x060003C1 RID: 961 RVA: 0x0000929C File Offset: 0x0000749C
			[DataMember(Name = "PrivateInterfaces")]
			public List<string> PrivateInterfaces { get; private set; }

			// Token: 0x1700012D RID: 301
			// (get) Token: 0x060003C2 RID: 962 RVA: 0x000092A5 File Offset: 0x000074A5
			// (set) Token: 0x060003C3 RID: 963 RVA: 0x000092AD File Offset: 0x000074AD
			[DataMember(Name = "PublicInterfaces")]
			public List<string> PublicInterfaces { get; private set; }
		}

		// Token: 0x02000072 RID: 114
		[DataContract]
		public class SoftAPSettingsInfo
		{
			// Token: 0x1700012E RID: 302
			// (get) Token: 0x060003C5 RID: 965 RVA: 0x000092B6 File Offset: 0x000074B6
			// (set) Token: 0x060003C6 RID: 966 RVA: 0x000092BE File Offset: 0x000074BE
			[DataMember(Name = "SoftAPEnabled")]
			public string SoftAPEnabled { get; private set; }

			// Token: 0x1700012F RID: 303
			// (get) Token: 0x060003C7 RID: 967 RVA: 0x000092C7 File Offset: 0x000074C7
			// (set) Token: 0x060003C8 RID: 968 RVA: 0x000092CF File Offset: 0x000074CF
			[DataMember(Name = "SoftApPassword")]
			public string SoftApPassword { get; private set; }

			// Token: 0x17000130 RID: 304
			// (get) Token: 0x060003C9 RID: 969 RVA: 0x000092D8 File Offset: 0x000074D8
			// (set) Token: 0x060003CA RID: 970 RVA: 0x000092E0 File Offset: 0x000074E0
			[DataMember(Name = "SoftApSsid")]
			public string SoftApSsid { get; private set; }
		}

		// Token: 0x02000073 RID: 115
		[DataContract]
		public class AllJoynSettingsInfo
		{
			// Token: 0x17000131 RID: 305
			// (get) Token: 0x060003CC RID: 972 RVA: 0x000092E9 File Offset: 0x000074E9
			// (set) Token: 0x060003CD RID: 973 RVA: 0x000092F1 File Offset: 0x000074F1
			[DataMember(Name = "AllJoynOnboardingDefaultDescription")]
			public string AllJoynOnboardingDefaultDescription { get; private set; }

			// Token: 0x17000132 RID: 306
			// (get) Token: 0x060003CE RID: 974 RVA: 0x000092FA File Offset: 0x000074FA
			// (set) Token: 0x060003CF RID: 975 RVA: 0x00009302 File Offset: 0x00007502
			[DataMember(Name = "AllJoynOnboardingDefaultManufacturer")]
			public string AllJoynOnboardingDefaultManufacturer { get; private set; }

			// Token: 0x17000133 RID: 307
			// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000930B File Offset: 0x0000750B
			// (set) Token: 0x060003D1 RID: 977 RVA: 0x00009313 File Offset: 0x00007513
			[DataMember(Name = "AllJoynOnboardingEnabled")]
			public string AllJoynOnboardingEnabled { get; private set; }

			// Token: 0x17000134 RID: 308
			// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000931C File Offset: 0x0000751C
			// (set) Token: 0x060003D3 RID: 979 RVA: 0x00009324 File Offset: 0x00007524
			[DataMember(Name = "AllJoynOnboardingModelNumber")]
			public string AllJoynOnboardingModelNumber { get; private set; }
		}

		// Token: 0x02000074 RID: 116
		[DataContract]
		public class RemoteSettingsStatusInfo
		{
			// Token: 0x17000135 RID: 309
			// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000932D File Offset: 0x0000752D
			// (set) Token: 0x060003D6 RID: 982 RVA: 0x00009335 File Offset: 0x00007535
			[DataMember(Name = "IsRunning")]
			public bool IsRunning { get; private set; }

			// Token: 0x17000136 RID: 310
			// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000933E File Offset: 0x0000753E
			// (set) Token: 0x060003D8 RID: 984 RVA: 0x00009346 File Offset: 0x00007546
			[DataMember(Name = "IsScheduled")]
			public bool IsScheduled { get; private set; }
		}

		// Token: 0x02000075 RID: 117
		[DataContract]
		public class IoTOSInfo
		{
			// Token: 0x17000137 RID: 311
			// (get) Token: 0x060003DA RID: 986 RVA: 0x0000934F File Offset: 0x0000754F
			// (set) Token: 0x060003DB RID: 987 RVA: 0x00009357 File Offset: 0x00007557
			[DataMember(Name = "DeviceModel")]
			public string Model { get; private set; }

			// Token: 0x17000138 RID: 312
			// (get) Token: 0x060003DC RID: 988 RVA: 0x00009360 File Offset: 0x00007560
			// (set) Token: 0x060003DD RID: 989 RVA: 0x00009368 File Offset: 0x00007568
			[DataMember(Name = "DeviceName")]
			public string Name { get; private set; }

			// Token: 0x17000139 RID: 313
			// (get) Token: 0x060003DE RID: 990 RVA: 0x00009371 File Offset: 0x00007571
			// (set) Token: 0x060003DF RID: 991 RVA: 0x00009379 File Offset: 0x00007579
			[DataMember(Name = "OSVersion")]
			public string OSVersion { get; private set; }
		}

		// Token: 0x02000076 RID: 118
		[DataContract]
		public class TimezoneInfo
		{
			// Token: 0x1700013A RID: 314
			// (get) Token: 0x060003E1 RID: 993 RVA: 0x00009382 File Offset: 0x00007582
			// (set) Token: 0x060003E2 RID: 994 RVA: 0x0000938A File Offset: 0x0000758A
			[DataMember(Name = "Current")]
			public DevicePortal.Timezone CurrentTimeZone { get; private set; }

			// Token: 0x1700013B RID: 315
			// (get) Token: 0x060003E3 RID: 995 RVA: 0x00009393 File Offset: 0x00007593
			// (set) Token: 0x060003E4 RID: 996 RVA: 0x0000939B File Offset: 0x0000759B
			[DataMember(Name = "Timezones")]
			public List<DevicePortal.Timezone> Timezones { get; private set; }
		}

		// Token: 0x02000077 RID: 119
		[DataContract]
		public class Timezone
		{
			// Token: 0x1700013C RID: 316
			// (get) Token: 0x060003E6 RID: 998 RVA: 0x000093A4 File Offset: 0x000075A4
			// (set) Token: 0x060003E7 RID: 999 RVA: 0x000093AC File Offset: 0x000075AC
			[DataMember(Name = "Description")]
			public string Description { get; private set; }

			// Token: 0x1700013D RID: 317
			// (get) Token: 0x060003E8 RID: 1000 RVA: 0x000093B5 File Offset: 0x000075B5
			// (set) Token: 0x060003E9 RID: 1001 RVA: 0x000093BD File Offset: 0x000075BD
			[DataMember(Name = "Index")]
			public int Index { get; private set; }

			// Token: 0x1700013E RID: 318
			// (get) Token: 0x060003EA RID: 1002 RVA: 0x000093C6 File Offset: 0x000075C6
			// (set) Token: 0x060003EB RID: 1003 RVA: 0x000093CE File Offset: 0x000075CE
			[DataMember(Name = "Name")]
			public string Name { get; private set; }
		}

		// Token: 0x02000078 RID: 120
		[DataContract]
		public class DateTimeInfo
		{
			// Token: 0x1700013F RID: 319
			// (get) Token: 0x060003ED RID: 1005 RVA: 0x000093D7 File Offset: 0x000075D7
			// (set) Token: 0x060003EE RID: 1006 RVA: 0x000093DF File Offset: 0x000075DF
			[DataMember(Name = "Current")]
			public DevicePortal.DateTimeDescription CurrentDateTime { get; private set; }
		}

		// Token: 0x02000079 RID: 121
		[DataContract]
		public class DateTimeDescription
		{
			// Token: 0x17000140 RID: 320
			// (get) Token: 0x060003F0 RID: 1008 RVA: 0x000093E8 File Offset: 0x000075E8
			// (set) Token: 0x060003F1 RID: 1009 RVA: 0x000093F0 File Offset: 0x000075F0
			[DataMember(Name = "Day")]
			public int Day { get; private set; }

			// Token: 0x17000141 RID: 321
			// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000093F9 File Offset: 0x000075F9
			// (set) Token: 0x060003F3 RID: 1011 RVA: 0x00009401 File Offset: 0x00007601
			[DataMember(Name = "Hour")]
			public int Hour { get; private set; }

			// Token: 0x17000142 RID: 322
			// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000940A File Offset: 0x0000760A
			// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00009412 File Offset: 0x00007612
			[DataMember(Name = "Minute")]
			public int Min { get; private set; }

			// Token: 0x17000143 RID: 323
			// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000941B File Offset: 0x0000761B
			// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00009423 File Offset: 0x00007623
			[DataMember(Name = "Month")]
			public int Month { get; private set; }

			// Token: 0x17000144 RID: 324
			// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000942C File Offset: 0x0000762C
			// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00009434 File Offset: 0x00007634
			[DataMember(Name = "Second")]
			public int Sec { get; private set; }

			// Token: 0x17000145 RID: 325
			// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000943D File Offset: 0x0000763D
			// (set) Token: 0x060003FB RID: 1019 RVA: 0x00009445 File Offset: 0x00007645
			[DataMember(Name = "Year")]
			public int Year { get; private set; }
		}

		// Token: 0x0200007A RID: 122
		[DataContract]
		public class ControllerDriverInfo
		{
			// Token: 0x17000146 RID: 326
			// (get) Token: 0x060003FD RID: 1021 RVA: 0x0000944E File Offset: 0x0000764E
			// (set) Token: 0x060003FE RID: 1022 RVA: 0x00009456 File Offset: 0x00007656
			[DataMember(Name = "CurrentDriver")]
			public string CurrentDriver { get; private set; }

			// Token: 0x17000147 RID: 327
			// (get) Token: 0x060003FF RID: 1023 RVA: 0x0000945F File Offset: 0x0000765F
			// (set) Token: 0x06000400 RID: 1024 RVA: 0x00009467 File Offset: 0x00007667
			[DataMember(Name = "ControllersDrivers")]
			public List<string> ControllersDrivers { get; private set; }

			// Token: 0x17000148 RID: 328
			// (get) Token: 0x06000401 RID: 1025 RVA: 0x00009470 File Offset: 0x00007670
			// (set) Token: 0x06000402 RID: 1026 RVA: 0x00009478 File Offset: 0x00007678
			[DataMember(Name = "RequestReboot")]
			public string RequestReboot { get; private set; }
		}

		// Token: 0x0200007B RID: 123
		[DataContract]
		public class DisplayOrientationInfo
		{
			// Token: 0x17000149 RID: 329
			// (get) Token: 0x06000404 RID: 1028 RVA: 0x00009481 File Offset: 0x00007681
			// (set) Token: 0x06000405 RID: 1029 RVA: 0x00009489 File Offset: 0x00007689
			[DataMember(Name = "Orientation")]
			public int Orientation { get; private set; }
		}

		// Token: 0x0200007C RID: 124
		[DataContract]
		public class DisplayResolutionInfo
		{
			// Token: 0x1700014A RID: 330
			// (get) Token: 0x06000407 RID: 1031 RVA: 0x00009492 File Offset: 0x00007692
			// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000949A File Offset: 0x0000769A
			[DataMember(Name = "Current")]
			public DevicePortal.Resolution CurrentResolution { get; private set; }

			// Token: 0x1700014B RID: 331
			// (get) Token: 0x06000409 RID: 1033 RVA: 0x000094A3 File Offset: 0x000076A3
			// (set) Token: 0x0600040A RID: 1034 RVA: 0x000094AB File Offset: 0x000076AB
			[DataMember(Name = "Resolutions")]
			public List<DevicePortal.Resolution> Resolutions { get; private set; }
		}

		// Token: 0x0200007D RID: 125
		[DataContract]
		public class Resolution
		{
			// Token: 0x1700014C RID: 332
			// (get) Token: 0x0600040C RID: 1036 RVA: 0x000094B4 File Offset: 0x000076B4
			// (set) Token: 0x0600040D RID: 1037 RVA: 0x000094BC File Offset: 0x000076BC
			[DataMember(Name = "Resolution")]
			public string ResolutionDetail { get; private set; }

			// Token: 0x1700014D RID: 333
			// (get) Token: 0x0600040E RID: 1038 RVA: 0x000094C5 File Offset: 0x000076C5
			// (set) Token: 0x0600040F RID: 1039 RVA: 0x000094CD File Offset: 0x000076CD
			[DataMember(Name = "Index")]
			public int Index { get; private set; }
		}

		// Token: 0x0200007E RID: 126
		[DataContract]
		public class ErrorInformation
		{
			// Token: 0x1700014E RID: 334
			// (get) Token: 0x06000411 RID: 1041 RVA: 0x000094D6 File Offset: 0x000076D6
			// (set) Token: 0x06000412 RID: 1042 RVA: 0x000094DE File Offset: 0x000076DE
			[DataMember(Name = "ErrorCode")]
			public int ErrorCode { get; private set; }

			// Token: 0x1700014F RID: 335
			// (get) Token: 0x06000413 RID: 1043 RVA: 0x000094E7 File Offset: 0x000076E7
			// (set) Token: 0x06000414 RID: 1044 RVA: 0x000094EF File Offset: 0x000076EF
			[DataMember(Name = "Status")]
			public string Status { get; private set; }
		}

		// Token: 0x0200007F RID: 127
		[DataContract]
		public class StatusInfo
		{
			// Token: 0x17000150 RID: 336
			// (get) Token: 0x06000416 RID: 1046 RVA: 0x000094F8 File Offset: 0x000076F8
			// (set) Token: 0x06000417 RID: 1047 RVA: 0x00009500 File Offset: 0x00007700
			[DataMember(Name = "lastCheckTime")]
			public string LastCheckTime { get; private set; }

			// Token: 0x17000151 RID: 337
			// (get) Token: 0x06000418 RID: 1048 RVA: 0x00009509 File Offset: 0x00007709
			// (set) Token: 0x06000419 RID: 1049 RVA: 0x00009511 File Offset: 0x00007711
			[DataMember(Name = "stagingProgress")]
			public string StagingProgress { get; private set; }

			// Token: 0x17000152 RID: 338
			// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000951A File Offset: 0x0000771A
			// (set) Token: 0x0600041B RID: 1051 RVA: 0x00009522 File Offset: 0x00007722
			[DataMember(Name = "lastUpdateTime")]
			public string LastUpdateTime { get; private set; }

			// Token: 0x17000153 RID: 339
			// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000952B File Offset: 0x0000772B
			// (set) Token: 0x0600041D RID: 1053 RVA: 0x00009533 File Offset: 0x00007733
			[DataMember(Name = "lastFailTime")]
			public string LastFailTime { get; private set; }

			// Token: 0x17000154 RID: 340
			// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000953C File Offset: 0x0000773C
			// (set) Token: 0x0600041F RID: 1055 RVA: 0x00009544 File Offset: 0x00007744
			[DataMember(Name = "updateState")]
			public int UpdateState { get; private set; }

			// Token: 0x17000155 RID: 341
			// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000954D File Offset: 0x0000774D
			// (set) Token: 0x06000421 RID: 1057 RVA: 0x00009555 File Offset: 0x00007755
			[DataMember(Name = "updateStatusMessage")]
			public string UpdateStatusMessage { get; private set; }
		}

		// Token: 0x02000080 RID: 128
		public class UpdateInstallTimeInfo
		{
			// Token: 0x17000156 RID: 342
			// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000955E File Offset: 0x0000775E
			// (set) Token: 0x06000424 RID: 1060 RVA: 0x00009566 File Offset: 0x00007766
			[DataMember(Name = "rebootscheduled")]
			public int RebootScheduled { get; private set; }

			// Token: 0x17000157 RID: 343
			// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000956F File Offset: 0x0000776F
			// (set) Token: 0x06000426 RID: 1062 RVA: 0x00009577 File Offset: 0x00007777
			[DataMember(Name = "rebootscheduledtime")]
			public string RebootScheduledTimeAsString { get; private set; }

			// Token: 0x17000158 RID: 344
			// (get) Token: 0x06000427 RID: 1063 RVA: 0x00009580 File Offset: 0x00007780
			public DateTime RebootScheduledTime
			{
				get
				{
					return DateTime.Parse(this.RebootScheduledTimeAsString);
				}
			}
		}

		// Token: 0x02000081 RID: 129
		[DataContract]
		public class Sandbox
		{
			// Token: 0x17000159 RID: 345
			// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000958D File Offset: 0x0000778D
			// (set) Token: 0x0600042A RID: 1066 RVA: 0x00009595 File Offset: 0x00007795
			[DataMember(Name = "Sandbox")]
			public string Value { get; set; }

			// Token: 0x0600042B RID: 1067 RVA: 0x0000959E File Offset: 0x0000779E
			public override string ToString()
			{
				return this.Value;
			}
		}

		// Token: 0x02000082 RID: 130
		[DataContract]
		public class SmbInfo
		{
			// Token: 0x1700015A RID: 346
			// (get) Token: 0x0600042D RID: 1069 RVA: 0x000095A6 File Offset: 0x000077A6
			// (set) Token: 0x0600042E RID: 1070 RVA: 0x000095AE File Offset: 0x000077AE
			[DataMember(Name = "Path")]
			public string Path { get; private set; }

			// Token: 0x1700015B RID: 347
			// (get) Token: 0x0600042F RID: 1071 RVA: 0x000095B7 File Offset: 0x000077B7
			// (set) Token: 0x06000430 RID: 1072 RVA: 0x000095BF File Offset: 0x000077BF
			[DataMember(Name = "Username")]
			public string Username { get; private set; }

			// Token: 0x1700015C RID: 348
			// (get) Token: 0x06000431 RID: 1073 RVA: 0x000095C8 File Offset: 0x000077C8
			// (set) Token: 0x06000432 RID: 1074 RVA: 0x000095D0 File Offset: 0x000077D0
			[DataMember(Name = "Password")]
			public string Password { get; private set; }

			// Token: 0x06000433 RID: 1075 RVA: 0x000095D9 File Offset: 0x000077D9
			public override string ToString()
			{
				return this.Path;
			}
		}

		// Token: 0x02000083 RID: 131
		[DataContract]
		public class UserList
		{
			// Token: 0x06000435 RID: 1077 RVA: 0x000095E1 File Offset: 0x000077E1
			public UserList()
			{
				this.Users = new List<DevicePortal.UserInfo>();
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x06000436 RID: 1078 RVA: 0x000095F4 File Offset: 0x000077F4
			// (set) Token: 0x06000437 RID: 1079 RVA: 0x000095FC File Offset: 0x000077FC
			[DataMember(Name = "Users")]
			public List<DevicePortal.UserInfo> Users { get; private set; }

			// Token: 0x06000438 RID: 1080 RVA: 0x00009608 File Offset: 0x00007808
			public override string ToString()
			{
				string text = string.Empty;
				foreach (DevicePortal.UserInfo userInfo in this.Users)
				{
					text = string.Concat(new object[]
					{
						text,
						"User:\n",
						userInfo,
						"\n"
					});
				}
				return text;
			}

			// Token: 0x06000439 RID: 1081 RVA: 0x00009680 File Offset: 0x00007880
			public void Add(DevicePortal.UserInfo newUser)
			{
				this.Users.Add(newUser);
			}
		}

		// Token: 0x02000084 RID: 132
		[DataContract]
		public class UserInfo
		{
			// Token: 0x1700015E RID: 350
			// (get) Token: 0x0600043A RID: 1082 RVA: 0x0000968E File Offset: 0x0000788E
			// (set) Token: 0x0600043B RID: 1083 RVA: 0x00009696 File Offset: 0x00007896
			[DataMember(Name = "UserId", EmitDefaultValue = false)]
			public uint? UserId { get; set; }

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000969F File Offset: 0x0000789F
			// (set) Token: 0x0600043D RID: 1085 RVA: 0x000096A7 File Offset: 0x000078A7
			[DataMember(Name = "EmailAddress", EmitDefaultValue = false)]
			public string EmailAddress { get; set; }

			// Token: 0x17000160 RID: 352
			// (get) Token: 0x0600043E RID: 1086 RVA: 0x000096B0 File Offset: 0x000078B0
			// (set) Token: 0x0600043F RID: 1087 RVA: 0x000096B8 File Offset: 0x000078B8
			[DataMember(Name = "Password", EmitDefaultValue = false)]
			public string Password { get; set; }

			// Token: 0x17000161 RID: 353
			// (get) Token: 0x06000440 RID: 1088 RVA: 0x000096C1 File Offset: 0x000078C1
			// (set) Token: 0x06000441 RID: 1089 RVA: 0x000096C9 File Offset: 0x000078C9
			[DataMember(Name = "AutoSignIn", EmitDefaultValue = false)]
			public bool? AutoSignIn { get; set; }

			// Token: 0x17000162 RID: 354
			// (get) Token: 0x06000442 RID: 1090 RVA: 0x000096D2 File Offset: 0x000078D2
			// (set) Token: 0x06000443 RID: 1091 RVA: 0x000096DA File Offset: 0x000078DA
			[DataMember(Name = "Gamertag", EmitDefaultValue = false)]
			public string Gamertag { get; private set; }

			// Token: 0x17000163 RID: 355
			// (get) Token: 0x06000444 RID: 1092 RVA: 0x000096E3 File Offset: 0x000078E3
			// (set) Token: 0x06000445 RID: 1093 RVA: 0x000096EB File Offset: 0x000078EB
			[DataMember(Name = "SignedIn", EmitDefaultValue = false)]
			public bool? SignedIn { get; set; }

			// Token: 0x17000164 RID: 356
			// (get) Token: 0x06000446 RID: 1094 RVA: 0x000096F4 File Offset: 0x000078F4
			// (set) Token: 0x06000447 RID: 1095 RVA: 0x000096FC File Offset: 0x000078FC
			[DataMember(Name = "Delete", EmitDefaultValue = false)]
			public bool? Delete { get; set; }

			// Token: 0x17000165 RID: 357
			// (get) Token: 0x06000448 RID: 1096 RVA: 0x00009705 File Offset: 0x00007905
			// (set) Token: 0x06000449 RID: 1097 RVA: 0x0000970D File Offset: 0x0000790D
			[DataMember(Name = "SponsoredUser", EmitDefaultValue = false)]
			public bool? SponsoredUser { get; set; }

			// Token: 0x17000166 RID: 358
			// (get) Token: 0x0600044A RID: 1098 RVA: 0x00009716 File Offset: 0x00007916
			// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000971E File Offset: 0x0000791E
			[DataMember(Name = "XboxUserId", EmitDefaultValue = false)]
			public string XboxUserId { get; private set; }

			// Token: 0x0600044C RID: 1100 RVA: 0x00009728 File Offset: 0x00007928
			public override string ToString()
			{
				object[] array = new object[12];
				array[0] = "    Id: ";
				array[1] = this.UserId;
				array[2] = "\n";
				int num = 3;
				bool? flag = this.SponsoredUser;
				bool flag2 = true;
				array[num] = ((!(flag.GetValueOrDefault() == flag2 & flag != null)) ? ("    Email: " + this.EmailAddress + "\n") : "    Sponsored User\n");
				array[4] = "    Gamertag: ";
				array[5] = this.Gamertag;
				array[6] = "\n    XboxUserId: ";
				array[7] = this.XboxUserId;
				array[8] = "\n    SignedIn: ";
				int num2 = 9;
				flag = this.SignedIn;
				flag2 = true;
				array[num2] = ((flag.GetValueOrDefault() == flag2 & flag != null) ? "yes" : "no");
				array[10] = "\n";
				int num3 = 11;
				flag = this.SponsoredUser;
				flag2 = true;
				object obj;
				if (flag.GetValueOrDefault() == flag2 & flag != null)
				{
					obj = string.Empty;
				}
				else
				{
					string str = "    AutoSignIn: ";
					flag = this.AutoSignIn;
					flag2 = true;
					obj = str + ((flag.GetValueOrDefault() == flag2 & flag != null) ? "yes" : "no") + "\n";
				}
				array[num3] = obj;
				return string.Concat(array);
			}
		}

		// Token: 0x02000085 RID: 133
		[DataContract]
		public class XboxSettingList
		{
			// Token: 0x0600044E RID: 1102 RVA: 0x0000985B File Offset: 0x00007A5B
			public XboxSettingList()
			{
				this.Settings = new List<DevicePortal.XboxSetting>();
			}

			// Token: 0x17000167 RID: 359
			// (get) Token: 0x0600044F RID: 1103 RVA: 0x0000986E File Offset: 0x00007A6E
			// (set) Token: 0x06000450 RID: 1104 RVA: 0x00009876 File Offset: 0x00007A76
			[DataMember(Name = "Settings")]
			public List<DevicePortal.XboxSetting> Settings { get; private set; }

			// Token: 0x06000451 RID: 1105 RVA: 0x00009880 File Offset: 0x00007A80
			public override string ToString()
			{
				string text = string.Empty;
				foreach (DevicePortal.XboxSetting arg in this.Settings)
				{
					text = text + arg + "\n";
				}
				return text;
			}
		}

		// Token: 0x02000086 RID: 134
		[DataContract]
		public class XboxSetting
		{
			// Token: 0x17000168 RID: 360
			// (get) Token: 0x06000452 RID: 1106 RVA: 0x000098E0 File Offset: 0x00007AE0
			// (set) Token: 0x06000453 RID: 1107 RVA: 0x000098E8 File Offset: 0x00007AE8
			[DataMember(Name = "Name", EmitDefaultValue = false)]
			public string Name { get; set; }

			// Token: 0x17000169 RID: 361
			// (get) Token: 0x06000454 RID: 1108 RVA: 0x000098F1 File Offset: 0x00007AF1
			// (set) Token: 0x06000455 RID: 1109 RVA: 0x000098F9 File Offset: 0x00007AF9
			[DataMember(Name = "Value", EmitDefaultValue = false)]
			public string Value { get; set; }

			// Token: 0x1700016A RID: 362
			// (get) Token: 0x06000456 RID: 1110 RVA: 0x00009902 File Offset: 0x00007B02
			// (set) Token: 0x06000457 RID: 1111 RVA: 0x0000990A File Offset: 0x00007B0A
			[DataMember(Name = "Category", EmitDefaultValue = false)]
			public string Category { get; private set; }

			// Token: 0x1700016B RID: 363
			// (get) Token: 0x06000458 RID: 1112 RVA: 0x00009913 File Offset: 0x00007B13
			// (set) Token: 0x06000459 RID: 1113 RVA: 0x0000991B File Offset: 0x00007B1B
			[DataMember(Name = "RequiresReboot", EmitDefaultValue = false)]
			public string RequiresReboot { get; private set; }

			// Token: 0x0600045A RID: 1114 RVA: 0x00009924 File Offset: 0x00007B24
			public override string ToString()
			{
				return string.Format("{0}: {1}", this.Name, this.Value);
			}
		}
	}
}
