using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Communication
{
	// Token: 0x0200000E RID: 14
	public class XdeSDCardPipe : XdePipe, IXdeSDCardPipe, IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable, IXdeAutomationSDCardPipe
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00003D1B File Offset: 0x00001F1B
		protected XdeSDCardPipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeSDCardPipe.SDCardGuid)
		{
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000AA RID: 170 RVA: 0x00003D38 File Offset: 0x00001F38
		// (remove) Token: 0x060000AB RID: 171 RVA: 0x00003D70 File Offset: 0x00001F70
		public event EventHandler<InsertEjectCompletedEventArgs> InsertCompleted;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060000AC RID: 172 RVA: 0x00003DA8 File Offset: 0x00001FA8
		// (remove) Token: 0x060000AD RID: 173 RVA: 0x00003DE0 File Offset: 0x00001FE0
		public event EventHandler<InsertEjectCompletedEventArgs> EjectCompleted;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060000AE RID: 174 RVA: 0x00003E18 File Offset: 0x00002018
		// (remove) Token: 0x060000AF RID: 175 RVA: 0x00003E50 File Offset: 0x00002050
		public event EventHandler<UpdateSyncProgressEventArgs> ProgressBarUpdated;

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003E85 File Offset: 0x00002085
		public bool IsSDCardInserted
		{
			get
			{
				return this.DoGetSDCardMountStatusState() == XdeSDCardPipe.SDCardMountStatusState.SDCARD_MOUNTED_VISIBLE;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003E93 File Offset: 0x00002093
		public static IXdeSDCardPipe XdeSDCardPipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeSDCardPipe(addressInfo);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003E9B File Offset: 0x0000209B
		public void CancelSync()
		{
			this.cancelSyncEvent.Set();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003EAC File Offset: 0x000020AC
		public bool InsertSDCard(string folderPath)
		{
			XdeSDCardPipe.SyncThreadClass @object = new XdeSDCardPipe.SyncThreadClass(folderPath, true, this);
			this.syncThread = new Thread(new ThreadStart(@object.InsertSDCardThreadProc));
			this.syncThread.Start();
			return true;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003EE8 File Offset: 0x000020E8
		public bool EjectSDCard(string folderPath, bool shouldSyncOnEject)
		{
			XdeSDCardPipe.SyncThreadClass @object = new XdeSDCardPipe.SyncThreadClass(folderPath, shouldSyncOnEject, this);
			this.syncThread = new Thread(new ThreadStart(@object.EjectSDCardThreadProc));
			this.syncThread.Start();
			return true;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003F24 File Offset: 0x00002124
		private static string GetStringFromErrorCode(int errorCode)
		{
			string result;
			if (errorCode != 65536)
			{
				if (errorCode != 65537)
				{
					result = new Win32Exception(errorCode).Message;
				}
				else
				{
					result = PipeExceptions.SDCardRootNotSet;
				}
			}
			else
			{
				result = PipeExceptions.SDCardLabelNotSet;
			}
			return result;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003F60 File Offset: 0x00002160
		private static bool CheckSyncNeeded(string absolutePath, DateTime syncTime)
		{
			bool result;
			if (File.Exists(absolutePath) || Directory.Exists(absolutePath))
			{
				DateTime lastWriteTime;
				if ((File.GetAttributes(absolutePath) & FileAttributes.Directory) == FileAttributes.Directory)
				{
					lastWriteTime = new DirectoryInfo(absolutePath).LastWriteTime;
				}
				else
				{
					lastWriteTime = new FileInfo(absolutePath).LastWriteTime;
				}
				result = (Math.Abs((syncTime - lastWriteTime).Seconds) > 2);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003FC8 File Offset: 0x000021C8
		private static uint GetWin32FileAttributesFromFile(string filePath)
		{
			uint num = 0U;
			FileInfo fileInfo = new FileInfo(filePath);
			if ((fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
			{
				num |= 2U;
			}
			if ((fileInfo.Attributes & FileAttributes.System) == FileAttributes.System)
			{
				num |= 4U;
			}
			if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				num |= 1U;
			}
			return num;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000400C File Offset: 0x0000220C
		private static void SetWin32FileAttributesToFile(string filePath, uint win32FileAttribs)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if ((win32FileAttribs & 2U) != 0U)
			{
				fileInfo.Attributes |= FileAttributes.Hidden;
			}
			if ((win32FileAttribs & 4U) != 0U)
			{
				fileInfo.Attributes |= FileAttributes.System;
			}
			if ((win32FileAttribs & 1U) != 0U)
			{
				fileInfo.Attributes |= FileAttributes.ReadOnly;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000405C File Offset: 0x0000225C
		private static ulong CombineHighAndLowDWords(uint highWord, uint lowWord)
		{
			return (ulong)highWord << 32 | (ulong)lowWord;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004073 File Offset: 0x00002273
		private bool IsCancelSyncEventSet()
		{
			return this.cancelSyncEvent.WaitOne(0);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004081 File Offset: 0x00002281
		private void CheckForCancellationAndExit()
		{
			if (this.IsCancelSyncEventSet())
			{
				Thread.CurrentThread.Abort();
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004098 File Offset: 0x00002298
		private void OnProgressBarUpdated()
		{
			if (this.ProgressBarUpdated != null)
			{
				UpdateSyncProgressEventArgs updateSyncProgressEventArgs = new UpdateSyncProgressEventArgs();
				if (this.totalBytesToSync != 0UL)
				{
					updateSyncProgressEventArgs.CurrentProgressValue = this.totalBytesSyncedSoFar * 100.0 / this.totalBytesToSync;
				}
				else
				{
					updateSyncProgressEventArgs.CurrentProgressValue = 0.0;
				}
				this.ProgressBarUpdated(this, updateSyncProgressEventArgs);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000040FC File Offset: 0x000022FC
		private ulong GetFolderSizeOnHostWithFAT32Check(string hostFolderPath)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ulong folderSizeOnHost = this.GetFolderSizeOnHost(hostFolderPath, stringBuilder);
			if (stringBuilder.Length == 0)
			{
				return folderSizeOnHost;
			}
			string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardHostFileMaxSizeExceeded, new object[]
			{
				stringBuilder
			});
			base.ThrowXdePipeException(message);
			return 0UL;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004140 File Offset: 0x00002340
		private ulong GetFolderSizeOnHost(string hostFolderPath, StringBuilder filesExceedingSize)
		{
			ulong num = 0UL;
			DirectoryInfo directoryInfo = new DirectoryInfo(hostFolderPath);
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.EnumerateDirectories())
			{
				num += this.GetFolderSizeOnHost(directoryInfo2.FullName, filesExceedingSize);
			}
			foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles())
			{
				if (fileInfo.Length >= 4294967296L)
				{
					filesExceedingSize.AppendLine(fileInfo.FullName);
				}
				if (filesExceedingSize.Length == 0)
				{
					num += (ulong)fileInfo.Length;
				}
				this.CheckForCancellationAndExit();
			}
			return num;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004214 File Offset: 0x00002414
		private ulong GetFolderSizeOnGuest(string guestFolderPath)
		{
			ulong num = 0UL;
			string filePath = Path.Combine(guestFolderPath, "*");
			XdeSDCardPipe.SDCardFindFirstFileResponse sdcardFindFirstFileResponse = this.DoFindFirstFile(filePath);
			try
			{
				if (sdcardFindFirstFileResponse.FileHandle != -1 || sdcardFindFirstFileResponse.ResponseHeader.LastError != 2)
				{
					XdeSDCardPipe.WIN32_FIND_DATA findFileData = sdcardFindFirstFileResponse.FindFileData;
					XdeSDCardPipe.SDCardFindNextFileResponse sdcardFindNextFileResponse;
					do
					{
						string text = Path.Combine(guestFolderPath, findFileData.FileName);
						if ((findFileData.FileAttributes & 16U) != 0U)
						{
							if (findFileData.FileName != "." && findFileData.FileName != "..")
							{
								num += this.GetFolderSizeOnGuest(text);
							}
						}
						else
						{
							num += XdeSDCardPipe.CombineHighAndLowDWords(findFileData.FileSizeHigh, findFileData.FileSizeLow);
							this.CheckForCancellationAndExit();
						}
						sdcardFindNextFileResponse = this.DoFindNextFile(text, sdcardFindFirstFileResponse.FileHandle);
						findFileData = sdcardFindNextFileResponse.FindFileData;
					}
					while (sdcardFindNextFileResponse.ResponseHeader.ReturnCode != 0U);
				}
			}
			finally
			{
				this.DoFindCloseFileHandle(guestFolderPath, sdcardFindFirstFileResponse.FileHandle);
			}
			return num;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004310 File Offset: 0x00002510
		private void SyncDeleteFolderOnGuest(string hostRoot, string sdcardMountRoot, string relativeFolderPath)
		{
			string path = Path.Combine(hostRoot, relativeFolderPath);
			string text = Path.Combine(sdcardMountRoot, relativeFolderPath);
			string filePath = Path.Combine(sdcardMountRoot, relativeFolderPath, "*");
			XdeSDCardPipe.SDCardFindFirstFileResponse sdcardFindFirstFileResponse = this.DoFindFirstFile(filePath);
			try
			{
				if (sdcardFindFirstFileResponse.FileHandle != -1 || sdcardFindFirstFileResponse.ResponseHeader.LastError != 2)
				{
					XdeSDCardPipe.WIN32_FIND_DATA findFileData = sdcardFindFirstFileResponse.FindFileData;
					XdeSDCardPipe.SDCardFindNextFileResponse sdcardFindNextFileResponse;
					do
					{
						string text2 = Path.Combine(path, findFileData.FileName);
						string text3 = Path.Combine(text, findFileData.FileName);
						string relativeFolderPath2 = Path.Combine(relativeFolderPath, findFileData.FileName);
						if ((findFileData.FileAttributes & 16U) != 0U)
						{
							if (findFileData.FileName != "." && findFileData.FileName != "..")
							{
								if (Directory.Exists(text2))
								{
									this.SyncDeleteFolderOnGuest(hostRoot, sdcardMountRoot, relativeFolderPath2);
									this.CopyFileTimeAndAttributesToGuest(text2, text3);
								}
								else
								{
									this.DoDeleteDirectoryRecursive(text3);
								}
							}
						}
						else if (!File.Exists(text2))
						{
							this.DoDeleteFile(text3);
						}
						sdcardFindNextFileResponse = this.DoFindNextFile(text3, sdcardFindFirstFileResponse.FileHandle);
						findFileData = sdcardFindNextFileResponse.FindFileData;
					}
					while (sdcardFindNextFileResponse.ResponseHeader.ReturnCode != 0U);
				}
			}
			finally
			{
				this.DoFindCloseFileHandle(text, sdcardFindFirstFileResponse.FileHandle);
				this.CheckForCancellationAndExit();
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004450 File Offset: 0x00002650
		private void SyncDeleteFolderOnHost(string hostRoot, string sdcardMountRoot, string relativeFolderPath)
		{
			string text = Path.Combine(hostRoot, relativeFolderPath);
			string path = Path.Combine(sdcardMountRoot, relativeFolderPath);
			Path.Combine(sdcardMountRoot, relativeFolderPath, "*");
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.EnumerateDirectories())
			{
				string text2 = Path.Combine(text, directoryInfo2.Name);
				string text3 = Path.Combine(path, directoryInfo2.Name);
				string relativeFolderPath2 = Path.Combine(relativeFolderPath, directoryInfo2.Name);
				if (this.DoGetFileOrDirectoryExists(text3))
				{
					this.SyncDeleteFolderOnHost(hostRoot, sdcardMountRoot, relativeFolderPath2);
					this.CopyFileTimeAndAttributesFromGuest(text2, text3);
				}
				else
				{
					Directory.Delete(text2, true);
				}
			}
			foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles())
			{
				string path2 = Path.Combine(text, fileInfo.Name);
				string pathOnGuest = Path.Combine(path, fileInfo.Name);
				if (!this.DoGetFileOrDirectoryExists(pathOnGuest))
				{
					File.Delete(path2);
				}
			}
			this.CheckForCancellationAndExit();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004580 File Offset: 0x00002780
		private void CopyFileTimeToGuest(string hostPath, string guestPath)
		{
			FileInfo fileInfo = new FileInfo(hostPath);
			XdeSDCardPipe.SDCardCreateFileResponse sdcardCreateFileResponse = default(XdeSDCardPipe.SDCardCreateFileResponse);
			sdcardCreateFileResponse.FileHandle = -1;
			try
			{
				if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
				{
					sdcardCreateFileResponse = this.DoOpenDirectoryForWrite(guestPath);
				}
				else
				{
					sdcardCreateFileResponse = this.DoOpenFileForWrite(guestPath);
				}
				this.DoSetFileTime(guestPath, sdcardCreateFileResponse.FileHandle, fileInfo.CreationTime, fileInfo.LastAccessTime, fileInfo.LastWriteTime);
			}
			finally
			{
				this.DoCloseFileHandle(guestPath, sdcardCreateFileResponse.FileHandle);
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004604 File Offset: 0x00002804
		private void CopyFileTimeAndAttributesToGuest(string hostPath, string guestPath)
		{
			this.CopyFileTimeToGuest(hostPath, guestPath);
			uint win32FileAttributesFromFile = XdeSDCardPipe.GetWin32FileAttributesFromFile(hostPath);
			this.DoSetFileAttributes(guestPath, win32FileAttributesFromFile);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004628 File Offset: 0x00002828
		private void CopyFileTimeFromGuest(string hostPath, string guestPath)
		{
			XdeSDCardPipe.SDCardFileTimeAttributes sdcardFileTimeAttributes = this.DoGetFileTime(guestPath);
			FileSystemInfo fileSystemInfo = new FileInfo(hostPath);
			if ((fileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
			{
				fileSystemInfo = new DirectoryInfo(hostPath);
			}
			DateTime dateTime = DateTime.FromFileTime(sdcardFileTimeAttributes.CreationTime);
			if (fileSystemInfo.CreationTime != dateTime)
			{
				fileSystemInfo.CreationTime = dateTime;
			}
			dateTime = DateTime.FromFileTime(sdcardFileTimeAttributes.LastAccessTime);
			if (fileSystemInfo.LastAccessTime != dateTime)
			{
				fileSystemInfo.LastAccessTime = dateTime;
			}
			dateTime = DateTime.FromFileTime(sdcardFileTimeAttributes.LastWriteTime);
			if (fileSystemInfo.LastWriteTime != dateTime)
			{
				fileSystemInfo.LastWriteTime = dateTime;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000046BC File Offset: 0x000028BC
		private void CopyFileTimeAndAttributesFromGuest(string hostPath, string guestPath)
		{
			this.CopyFileTimeFromGuest(hostPath, guestPath);
			uint win32FileAttribs = this.DoGetFileAttributes(guestPath);
			XdeSDCardPipe.SetWin32FileAttributesToFile(hostPath, win32FileAttribs);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000046E0 File Offset: 0x000028E0
		private void SendFolderToGuest(string hostRoot, string sdcardMountRoot, string relativeFolderPath)
		{
			string text = Path.Combine(hostRoot, relativeFolderPath);
			string path = Path.Combine(sdcardMountRoot, relativeFolderPath);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.EnumerateDirectories())
			{
				string hostPath = Path.Combine(text, directoryInfo2.Name);
				string text2 = Path.Combine(path, directoryInfo2.Name);
				string relativeFolderPath2 = Path.Combine(relativeFolderPath, directoryInfo2.Name);
				this.DoCreateDirectory(text2);
				this.SendFolderToGuest(hostRoot, sdcardMountRoot, relativeFolderPath2);
				this.CopyFileTimeAndAttributesToGuest(hostPath, text2);
			}
			foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles())
			{
				string text3 = Path.Combine(text, fileInfo.Name);
				string text4 = Path.Combine(path, fileInfo.Name);
				if (this.DoGetFileOrDirectoryExists(text4))
				{
					DateTime syncTime = DateTime.FromFileTime(this.DoGetFileTime(text4).LastWriteTime);
					if (XdeSDCardPipe.CheckSyncNeeded(text3, syncTime))
					{
						this.DoDeleteFile(text4);
						this.DoCopyFileToGuest(text3, text4);
						this.CopyFileTimeAndAttributesToGuest(text3, text4);
					}
					else
					{
						FileInfo fileInfo2 = new FileInfo(text3);
						this.totalBytesSyncedSoFar += (ulong)fileInfo2.Length;
						this.OnProgressBarUpdated();
					}
				}
				else
				{
					this.DoCopyFileToGuest(text3, text4);
					this.CopyFileTimeAndAttributesToGuest(text3, text4);
				}
			}
			this.CheckForCancellationAndExit();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000486C File Offset: 0x00002A6C
		private void ReceiveFolderFromGuest(string hostRoot, string sdcardMountRoot, string relativeFolderPath)
		{
			string path = Path.Combine(hostRoot, relativeFolderPath);
			string text = Path.Combine(sdcardMountRoot, relativeFolderPath);
			string filePath = Path.Combine(sdcardMountRoot, relativeFolderPath, "*");
			XdeSDCardPipe.SDCardFindFirstFileResponse sdcardFindFirstFileResponse = default(XdeSDCardPipe.SDCardFindFirstFileResponse);
			try
			{
				sdcardFindFirstFileResponse = this.DoFindFirstFile(filePath);
				if (sdcardFindFirstFileResponse.FileHandle != -1 || sdcardFindFirstFileResponse.ResponseHeader.LastError != 2)
				{
					XdeSDCardPipe.WIN32_FIND_DATA findFileData = sdcardFindFirstFileResponse.FindFileData;
					XdeSDCardPipe.SDCardFindNextFileResponse sdcardFindNextFileResponse;
					do
					{
						string text2 = Path.Combine(path, findFileData.FileName);
						string text3 = Path.Combine(text, findFileData.FileName);
						string relativeFolderPath2 = Path.Combine(relativeFolderPath, findFileData.FileName);
						if ((findFileData.FileAttributes & 16U) != 0U)
						{
							if (findFileData.FileName != "." && findFileData.FileName != "..")
							{
								if (!Directory.Exists(text2))
								{
									Directory.CreateDirectory(text2);
								}
								this.ReceiveFolderFromGuest(hostRoot, sdcardMountRoot, relativeFolderPath2);
								this.CopyFileTimeAndAttributesFromGuest(text2, text3);
							}
						}
						else if (File.Exists(text2))
						{
							DateTime syncTime = DateTime.FromFileTime(this.DoGetFileTime(text3).LastWriteTime);
							if (XdeSDCardPipe.CheckSyncNeeded(text2, syncTime))
							{
								File.SetAttributes(text2, FileAttributes.Normal);
								File.Delete(text2);
								this.DoCopyFileFromGuest(text3, text2);
								this.CopyFileTimeAndAttributesFromGuest(text2, text3);
							}
							else
							{
								ulong num = XdeSDCardPipe.CombineHighAndLowDWords(findFileData.FileSizeHigh, findFileData.FileSizeLow);
								this.totalBytesSyncedSoFar += num;
								this.OnProgressBarUpdated();
							}
						}
						else
						{
							this.DoCopyFileFromGuest(text3, text2);
							this.CopyFileTimeAndAttributesFromGuest(text2, text3);
						}
						sdcardFindNextFileResponse = this.DoFindNextFile(text3, sdcardFindFirstFileResponse.FileHandle);
						findFileData = sdcardFindNextFileResponse.FindFileData;
					}
					while (sdcardFindNextFileResponse.ResponseHeader.ReturnCode != 0U);
				}
			}
			finally
			{
				this.DoFindCloseFileHandle(text, sdcardFindFirstFileResponse.FileHandle);
				this.CheckForCancellationAndExit();
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004A48 File Offset: 0x00002C48
		private bool DoGetFileOrDirectoryExists(string pathOnGuest)
		{
			XdeSDCardPipe.SDCardFindFirstFileResponse sdcardFindFirstFileResponse = this.DoFindFirstFile(pathOnGuest);
			if (sdcardFindFirstFileResponse.FileHandle == -1 && sdcardFindFirstFileResponse.ResponseHeader.LastError == 2)
			{
				return false;
			}
			this.DoFindCloseFileHandle(pathOnGuest, sdcardFindFirstFileResponse.FileHandle);
			return true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004A84 File Offset: 0x00002C84
		private XdeSDCardPipe.SDCardFileTimeAttributes DoGetFileTime(string pathOnGuest)
		{
			XdeSDCardPipe.SDCardFindFirstFileResponse sdcardFindFirstFileResponse = this.DoFindFirstFile(pathOnGuest);
			XdeSDCardPipe.SDCardFileTimeAttributes result = default(XdeSDCardPipe.SDCardFileTimeAttributes);
			if (sdcardFindFirstFileResponse.FileHandle == -1 && sdcardFindFirstFileResponse.ResponseHeader.LastError == 2)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestFindFileError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardFindFirstFileResponse.ResponseHeader.LastError),
					pathOnGuest
				});
				base.ThrowXdePipeException(message);
			}
			else
			{
				this.DoFindCloseFileHandle(pathOnGuest, sdcardFindFirstFileResponse.FileHandle);
				result.CreationTime = sdcardFindFirstFileResponse.FindFileData.CreationTime;
				result.LastAccessTime = sdcardFindFirstFileResponse.FindFileData.LastAccessTime;
				result.LastWriteTime = sdcardFindFirstFileResponse.FindFileData.LastWriteTime;
			}
			return result;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004B30 File Offset: 0x00002D30
		private void SyncToGuest(string hostFolderPath)
		{
			string sdcardMountRoot = this.DoGetSDCardMountRoot();
			this.totalBytesSyncedSoFar = 0UL;
			this.OnProgressBarUpdated();
			this.totalBytesToSync = this.GetFolderSizeOnHostWithFAT32Check(hostFolderPath);
			this.CheckForCancellationAndExit();
			this.SyncDeleteFolderOnGuest(hostFolderPath, sdcardMountRoot, string.Empty);
			this.CheckForCancellationAndExit();
			this.SendFolderToGuest(hostFolderPath, sdcardMountRoot, string.Empty);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004B88 File Offset: 0x00002D88
		private void SyncFromGuest(string hostFolderPath)
		{
			string text = this.DoGetSDCardMountRoot();
			this.totalBytesSyncedSoFar = 0UL;
			this.OnProgressBarUpdated();
			this.totalBytesToSync = this.GetFolderSizeOnGuest(text);
			this.CheckForCancellationAndExit();
			this.SyncDeleteFolderOnHost(hostFolderPath, text, string.Empty);
			this.CheckForCancellationAndExit();
			this.ReceiveFolderFromGuest(hostFolderPath, text, string.Empty);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004BE0 File Offset: 0x00002DE0
		private void DoWriteFilePart(string filePath, int fileHandle, byte[] bytesToWrite)
		{
			this.SendCommandAndVerifyResponse(31, (uint)(Marshal.SizeOf(typeof(int)) + bytesToWrite.Length));
			base.SendToGuest(fileHandle);
			base.SendToGuest(bytesToWrite);
			XdeSDCardPipe.SDCardReadWriteFileResponse sdcardReadWriteFileResponse = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardReadWriteFileResponse>();
			if (sdcardReadWriteFileResponse.ResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestWriteFileError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardReadWriteFileResponse.ResponseHeader.LastError),
					filePath
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004C5C File Offset: 0x00002E5C
		private void DoCopyFileToGuest(string filePathToRead, string filePathToWrite)
		{
			uint fileAttribs = 128U;
			int fileHandle = this.DoCreateFile(filePathToWrite, fileAttribs).FileHandle;
			bool flag = false;
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(File.Open(filePathToRead, FileMode.Open, FileAccess.Read, FileShare.Read)))
				{
					ulong num = 0UL;
					byte[] array = binaryReader.ReadBytes(102400);
					while (array.Length != 0 && !this.IsCancelSyncEventSet())
					{
						if (num % 100UL == 0UL)
						{
							this.OnProgressBarUpdated();
						}
						this.DoWriteFilePart(filePathToWrite, fileHandle, array);
						this.totalBytesSyncedSoFar += (ulong)((long)array.Length);
						array = binaryReader.ReadBytes(102400);
						num += 1UL;
					}
					if (array.Length == 0)
					{
						flag = true;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.DoCloseFileHandle(filePathToWrite, fileHandle);
				if (!flag)
				{
					this.DoDeleteFile(filePathToWrite);
				}
				this.OnProgressBarUpdated();
				this.CheckForCancellationAndExit();
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004D4C File Offset: 0x00002F4C
		private void SendCommandAndVerifyResponse(int commandID, uint dataSize)
		{
			XdeSDCardPipe.SDCardCommandHeader sdcardCommandHeader;
			sdcardCommandHeader.DataSize = dataSize;
			sdcardCommandHeader.CommandID = commandID;
			base.SendStructToGuest(sdcardCommandHeader);
			if (base.ReceiveIntFromGuest() == 0)
			{
				base.ThrowXdePipeException(PipeExceptions.SDCardProtocolBreached);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004D88 File Offset: 0x00002F88
		private byte[] DoReadFilePart(string filePath, int fileHandle)
		{
			this.SendCommandAndVerifyResponse(30, (uint)Marshal.SizeOf(typeof(int)));
			base.SendToGuest(fileHandle);
			XdeSDCardPipe.SDCardReadWriteFileResponse sdcardReadWriteFileResponse = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardReadWriteFileResponse>();
			if (sdcardReadWriteFileResponse.ResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestReadFileError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardReadWriteFileResponse.ResponseHeader.LastError),
					filePath
				});
				base.ThrowXdePipeException(message);
				return new byte[0];
			}
			if (sdcardReadWriteFileResponse.DataSize > 0U)
			{
				byte[] array = new byte[sdcardReadWriteFileResponse.DataSize];
				base.ReceiveFromGuest(array, (int)sdcardReadWriteFileResponse.DataSize);
				return array;
			}
			return null;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004E24 File Offset: 0x00003024
		private void DoCopyFileFromGuest(string filePathToRead, string filePathToWrite)
		{
			int fileHandle = this.DoOpenFileForRead(filePathToRead).FileHandle;
			bool flag = false;
			try
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(filePathToWrite, FileMode.Create, FileAccess.ReadWrite)))
				{
					ulong num = 0UL;
					while (!this.IsCancelSyncEventSet())
					{
						if (num % 100UL == 0UL)
						{
							this.OnProgressBarUpdated();
						}
						byte[] array = this.DoReadFilePart(filePathToRead, fileHandle);
						if (array == null)
						{
							flag = true;
							break;
						}
						binaryWriter.Write(array);
						this.totalBytesSyncedSoFar += (ulong)((long)array.Length);
						num += 1UL;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.DoCloseFileHandle(filePathToRead, fileHandle);
				if (!flag)
				{
					File.Delete(filePathToWrite);
				}
				this.OnProgressBarUpdated();
				this.CheckForCancellationAndExit();
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004EF0 File Offset: 0x000030F0
		private void DoSetFileTime(string filePath, int fileHandle, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
		{
			this.SendCommandAndVerifyResponse(29, (uint)Marshal.SizeOf(typeof(XdeSDCardPipe.SDCardSetFileTimeRequest)));
			XdeSDCardPipe.SDCardSetFileTimeRequest sdcardSetFileTimeRequest = default(XdeSDCardPipe.SDCardSetFileTimeRequest);
			sdcardSetFileTimeRequest.FileHandle = fileHandle;
			sdcardSetFileTimeRequest.FileTimeAttribs.CreationTime = creationTime.ToFileTime();
			sdcardSetFileTimeRequest.FileTimeAttribs.LastAccessTime = lastAccessTime.ToFileTime();
			sdcardSetFileTimeRequest.FileTimeAttribs.LastWriteTime = lastWriteTime.ToFileTime();
			base.SendStructToGuest(sdcardSetFileTimeRequest);
			XdeSDCardPipe.SDCardResponseHeader sdcardResponseHeader = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardResponseHeader>();
			if (sdcardResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestSetFileTimeError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardResponseHeader.LastError),
					filePath
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004FA4 File Offset: 0x000031A4
		private uint DoGetFileAttributes(string path)
		{
			this.SendCommandAndVerifyResponse(28, (uint)Encoding.Unicode.GetByteCount(path));
			base.SendToGuest(Encoding.Unicode.GetBytes(path));
			XdeSDCardPipe.SDCardGetFileAttributesResponse sdcardGetFileAttributesResponse = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardGetFileAttributesResponse>();
			if (sdcardGetFileAttributesResponse.ResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestGetFileAttribError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardGetFileAttributesResponse.ResponseHeader.LastError),
					path
				});
				base.ThrowXdePipeException(message);
			}
			return sdcardGetFileAttributesResponse.FileAttributes;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005020 File Offset: 0x00003220
		private void DoSetFileAttributes(string path, uint fileAttribs)
		{
			this.SendCommandAndVerifyResponse(27, (uint)(Encoding.Unicode.GetByteCount(path) + Marshal.SizeOf(typeof(uint))));
			base.SendToGuest(BitConverter.GetBytes(fileAttribs));
			base.SendToGuest(Encoding.Unicode.GetBytes(path));
			XdeSDCardPipe.SDCardResponseHeader sdcardResponseHeader = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardResponseHeader>();
			if (sdcardResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestSetFileAttribError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardResponseHeader.LastError),
					path
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000050A8 File Offset: 0x000032A8
		private void DoDeleteFileOrDirectory(string path, XdeSDCardPipe.SDCardCommandID commandID)
		{
			this.SendCommandAndVerifyResponse((int)commandID, (uint)Encoding.Unicode.GetByteCount(path));
			base.SendToGuest(Encoding.Unicode.GetBytes(path));
			XdeSDCardPipe.SDCardResponseHeader sdcardResponseHeader = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardResponseHeader>();
			if (sdcardResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestDeleteFileError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardResponseHeader.LastError),
					path
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005111 File Offset: 0x00003311
		private void DoDeleteDirectoryRecursive(string directoryPath)
		{
			this.DoDeleteFileOrDirectory(directoryPath, XdeSDCardPipe.SDCardCommandID.CMD_DeleteDirectory);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000511C File Offset: 0x0000331C
		private void DoDeleteFile(string filePath)
		{
			this.DoDeleteFileOrDirectory(filePath, XdeSDCardPipe.SDCardCommandID.CMD_DeleteFile);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005128 File Offset: 0x00003328
		private void DoCreateDirectory(string directoryPath)
		{
			this.SendCommandAndVerifyResponse(23, (uint)Encoding.Unicode.GetByteCount(directoryPath));
			base.SendToGuest(Encoding.Unicode.GetBytes(directoryPath));
			XdeSDCardPipe.SDCardResponseHeader sdcardResponseHeader = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardResponseHeader>();
			if (sdcardResponseHeader.ReturnCode == 0U && sdcardResponseHeader.LastError != 183)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestCreateDirectoryError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardResponseHeader.LastError),
					directoryPath
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000519F File Offset: 0x0000339F
		private XdeSDCardPipe.SDCardCreateFileResponse DoCreateFile(string filePath, uint fileAttribs)
		{
			return this.DoCreateOrOpenFile(filePath, fileAttribs, XdeSDCardPipe.SDCardCommandID.CMD_CreateFile);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000051AB File Offset: 0x000033AB
		private XdeSDCardPipe.SDCardCreateFileResponse DoOpenFileForRead(string filePath)
		{
			return this.DoCreateOrOpenFile(filePath, 0U, XdeSDCardPipe.SDCardCommandID.CMD_OpenReadFileHandle);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000051B7 File Offset: 0x000033B7
		private XdeSDCardPipe.SDCardCreateFileResponse DoOpenFileForWrite(string filePath)
		{
			return this.DoCreateOrOpenFile(filePath, 0U, XdeSDCardPipe.SDCardCommandID.CMD_OpenWriteFileHandle);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000051C3 File Offset: 0x000033C3
		private XdeSDCardPipe.SDCardCreateFileResponse DoOpenDirectoryForRead(string filePath)
		{
			return this.DoCreateOrOpenFile(filePath, 0U, XdeSDCardPipe.SDCardCommandID.CMD_OpenReadDirectoryHandle);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000051CF File Offset: 0x000033CF
		private XdeSDCardPipe.SDCardCreateFileResponse DoOpenDirectoryForWrite(string filePath)
		{
			return this.DoCreateOrOpenFile(filePath, 0U, XdeSDCardPipe.SDCardCommandID.CMD_OpenWriteDirectoryHandle);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000051DC File Offset: 0x000033DC
		private XdeSDCardPipe.SDCardCreateFileResponse DoCreateOrOpenFile(string filePath, uint fileAttribs, XdeSDCardPipe.SDCardCommandID commandID)
		{
			this.SendCommandAndVerifyResponse((int)commandID, (uint)(Encoding.Unicode.GetByteCount(filePath) + Marshal.SizeOf(typeof(uint))));
			base.SendToGuest(BitConverter.GetBytes(fileAttribs));
			base.SendToGuest(Encoding.Unicode.GetBytes(filePath));
			XdeSDCardPipe.SDCardCreateFileResponse sdcardCreateFileResponse = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardCreateFileResponse>();
			if (sdcardCreateFileResponse.ResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestCreateOrOpenFileError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardCreateFileResponse.ResponseHeader.LastError),
					filePath
				});
				base.ThrowXdePipeException(message);
			}
			return sdcardCreateFileResponse;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000526C File Offset: 0x0000346C
		private void DoCloseFileHandleInternal(string filePath, int fileHandle, XdeSDCardPipe.SDCardCommandID commandID)
		{
			if (fileHandle == -1)
			{
				return;
			}
			this.SendCommandAndVerifyResponse((int)commandID, (uint)Marshal.SizeOf(typeof(int)));
			base.SendToGuest(fileHandle);
			XdeSDCardPipe.SDCardResponseHeader sdcardResponseHeader = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardResponseHeader>();
			if (sdcardResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestCloseFileHandleError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardResponseHeader.LastError),
					filePath
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000052D4 File Offset: 0x000034D4
		private void DoFindCloseFileHandle(string filePath, int fileHandle)
		{
			this.DoCloseFileHandleInternal(filePath, fileHandle, XdeSDCardPipe.SDCardCommandID.CMD_FindCloseHandle);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000052E0 File Offset: 0x000034E0
		private void DoCloseFileHandle(string filePath, int fileHandle)
		{
			this.DoCloseFileHandleInternal(filePath, fileHandle, XdeSDCardPipe.SDCardCommandID.CMD_CloseFileHandle);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000052EC File Offset: 0x000034EC
		private XdeSDCardPipe.SDCardFindNextFileResponse DoFindNextFile(string previousFilePath, int fileHandle)
		{
			this.SendCommandAndVerifyResponse(20, (uint)Marshal.SizeOf(typeof(int)));
			base.SendToGuest(fileHandle);
			XdeSDCardPipe.SDCardFindNextFileResponse sdcardFindNextFileResponse = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardFindNextFileResponse>();
			if (sdcardFindNextFileResponse.ResponseHeader.ReturnCode == 0U && sdcardFindNextFileResponse.ResponseHeader.LastError != 18)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestFindNextFileError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardFindNextFileResponse.ResponseHeader.LastError),
					previousFilePath
				});
				base.ThrowXdePipeException(message);
			}
			return sdcardFindNextFileResponse;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000536C File Offset: 0x0000356C
		private XdeSDCardPipe.SDCardFindFirstFileResponse DoFindFirstFile(string filePath)
		{
			this.SendCommandAndVerifyResponse(19, (uint)Encoding.Unicode.GetByteCount(filePath));
			base.SendToGuest(Encoding.Unicode.GetBytes(filePath));
			XdeSDCardPipe.SDCardFindFirstFileResponse sdcardFindFirstFileResponse = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardFindFirstFileResponse>();
			if (sdcardFindFirstFileResponse.ResponseHeader.ReturnCode == 0U && (sdcardFindFirstFileResponse.FileHandle != -1 || sdcardFindFirstFileResponse.ResponseHeader.LastError != 2))
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestFindFileError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardFindFirstFileResponse.ResponseHeader.LastError),
					filePath
				});
				base.ThrowXdePipeException(message);
			}
			return sdcardFindFirstFileResponse;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000053F8 File Offset: 0x000035F8
		private string DoGetSDCardMountRoot()
		{
			this.SendCommandAndVerifyResponse(18, 0U);
			XdeSDCardPipe.SDCardGetMountRootResponse sdcardGetMountRootResponse = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardGetMountRootResponse>();
			if (sdcardGetMountRootResponse.ResponseHeader.ReturnCode == 0U || sdcardGetMountRootResponse.DataSize == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestGetMountRootError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardGetMountRootResponse.ResponseHeader.LastError)
				});
				base.ThrowXdePipeException(message);
			}
			byte[] array = new byte[sdcardGetMountRootResponse.DataSize];
			base.ReceiveFromGuest(array, (int)sdcardGetMountRootResponse.DataSize);
			return Encoding.Unicode.GetString(array);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000547C File Offset: 0x0000367C
		private void DoEnsureSDCardVolumeMounted()
		{
			this.SendCommandAndVerifyResponse(36, 0U);
			XdeSDCardPipe.SDCardResponseHeader sdcardResponseHeader = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardResponseHeader>();
			if (sdcardResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestGetMountRootError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardResponseHeader.LastError)
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000054C8 File Offset: 0x000036C8
		private XdeSDCardPipe.SDCardMountStatusState DoGetSDCardMountStatusState()
		{
			this.SendCommandAndVerifyResponse(37, 0U);
			XdeSDCardPipe.SDCardGetMountStatusResponse sdcardGetMountStatusResponse = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardGetMountStatusResponse>();
			if (sdcardGetMountStatusResponse.ResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestGetMountStatusError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardGetMountStatusResponse.ResponseHeader.LastError)
				});
				base.ThrowXdePipeException(message);
				return XdeSDCardPipe.SDCardMountStatusState.SDCARD_MOUNTSTATUS_UNKNOWN;
			}
			return (XdeSDCardPipe.SDCardMountStatusState)sdcardGetMountStatusResponse.MountStatus;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005528 File Offset: 0x00003728
		private void InsertSDCardInternal()
		{
			this.SendCommandAndVerifyResponse(16, 0U);
			XdeSDCardPipe.SDCardResponseHeader sdcardResponseHeader = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardResponseHeader>();
			if (sdcardResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestInsertSDError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardResponseHeader.LastError)
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005574 File Offset: 0x00003774
		private bool EjectSDCardInternal()
		{
			this.SendCommandAndVerifyResponse(17, 0U);
			XdeSDCardPipe.SDCardResponseHeader sdcardResponseHeader = base.ReceiveStructFromGuest<XdeSDCardPipe.SDCardResponseHeader>();
			if (sdcardResponseHeader.ReturnCode == 0U)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardGuestEjectSDError, new object[]
				{
					XdeSDCardPipe.GetStringFromErrorCode(sdcardResponseHeader.LastError)
				});
				base.ThrowXdePipeException(message);
			}
			return true;
		}

		// Token: 0x04000030 RID: 48
		private const int XDE_SDCARD_SUCCESS = 1;

		// Token: 0x04000031 RID: 49
		private const int XDE_SDCARD_INVALID_COMMAND = 0;

		// Token: 0x04000032 RID: 50
		private const int TransferBufferSize = 102400;

		// Token: 0x04000033 RID: 51
		private const int ProgressBarUpdateFrequency = 100;

		// Token: 0x04000034 RID: 52
		private const int LastWriteTimeResolution = 2;

		// Token: 0x04000035 RID: 53
		private const ulong MaxFileSize = 4294967296UL;

		// Token: 0x04000036 RID: 54
		private const int MaxLocalFolderPathLimit = 220;

		// Token: 0x04000037 RID: 55
		private static readonly Guid SDCardGuid = new Guid("{19f761af-d744-4896-a722-01e2feb75bdf}");

		// Token: 0x04000038 RID: 56
		private ManualResetEvent cancelSyncEvent = new ManualResetEvent(false);

		// Token: 0x04000039 RID: 57
		private Thread syncThread;

		// Token: 0x0400003A RID: 58
		private ulong totalBytesToSync;

		// Token: 0x0400003B RID: 59
		private ulong totalBytesSyncedSoFar;

		// Token: 0x02000015 RID: 21
		private enum SDCardCustomErrorCodes
		{
			// Token: 0x04000054 RID: 84
			SDCARD_LABEL_NOTSET = 65536,
			// Token: 0x04000055 RID: 85
			SDCARD_ROOT_NOT_SET
		}

		// Token: 0x02000016 RID: 22
		private enum SDCardMountStatusState
		{
			// Token: 0x04000057 RID: 87
			SDCARD_MOUNTSTATUS_UNKNOWN,
			// Token: 0x04000058 RID: 88
			SDCARD_DISMOUNTED,
			// Token: 0x04000059 RID: 89
			SDCARD_MOUNTED_HIDDEN,
			// Token: 0x0400005A RID: 90
			SDCARD_MOUNTED_VISIBLE
		}

		// Token: 0x02000017 RID: 23
		private enum SDCardCommandID
		{
			// Token: 0x0400005C RID: 92
			CMD_InsertSDCard = 16,
			// Token: 0x0400005D RID: 93
			CMD_EjectSDCard,
			// Token: 0x0400005E RID: 94
			CMD_GetSDMountRoot,
			// Token: 0x0400005F RID: 95
			CMD_FindFirstFile,
			// Token: 0x04000060 RID: 96
			CMD_FindNextFile,
			// Token: 0x04000061 RID: 97
			CMD_FindCloseHandle,
			// Token: 0x04000062 RID: 98
			CMD_CreateFile,
			// Token: 0x04000063 RID: 99
			CMD_CreateDirectory,
			// Token: 0x04000064 RID: 100
			CMD_DeleteFile,
			// Token: 0x04000065 RID: 101
			CMD_DeleteDirectory,
			// Token: 0x04000066 RID: 102
			CMD_CloseFileHandle,
			// Token: 0x04000067 RID: 103
			CMD_SetFileAttributes,
			// Token: 0x04000068 RID: 104
			CMD_GetFileAttributes,
			// Token: 0x04000069 RID: 105
			CMD_SetFileTime,
			// Token: 0x0400006A RID: 106
			CMD_ReadFilePart,
			// Token: 0x0400006B RID: 107
			CMD_WriteFilePart,
			// Token: 0x0400006C RID: 108
			CMD_OpenReadFileHandle,
			// Token: 0x0400006D RID: 109
			CMD_OpenWriteFileHandle,
			// Token: 0x0400006E RID: 110
			CMD_OpenReadDirectoryHandle,
			// Token: 0x0400006F RID: 111
			CMD_OpenWriteDirectoryHandle,
			// Token: 0x04000070 RID: 112
			CMD_EnsureSDCardVolumeMounted,
			// Token: 0x04000071 RID: 113
			CMD_GetSDCardMountStatusState
		}

		// Token: 0x02000018 RID: 24
		private struct WIN32_ERROR_CODES
		{
			// Token: 0x04000072 RID: 114
			public const int INVALID_HANDLE_VALUE = -1;

			// Token: 0x04000073 RID: 115
			public const int ERROR_NO_MORE_FILES = 18;

			// Token: 0x04000074 RID: 116
			public const int ERROR_FILE_NOT_FOUND = 2;

			// Token: 0x04000075 RID: 117
			public const int ERROR_ALREADY_EXISTS = 183;
		}

		// Token: 0x02000019 RID: 25
		private struct FILE_ATTRIBUTE_CONSTANTS
		{
			// Token: 0x04000076 RID: 118
			public const int FILE_ATTRIBUTE_READONLY = 1;

			// Token: 0x04000077 RID: 119
			public const int FILE_ATTRIBUTE_HIDDEN = 2;

			// Token: 0x04000078 RID: 120
			public const int FILE_ATTRIBUTE_SYSTEM = 4;

			// Token: 0x04000079 RID: 121
			public const int FILE_ATTRIBUTE_DIRECTORY = 16;

			// Token: 0x0400007A RID: 122
			public const int FILE_ATTRIBUTE_NORMAL = 128;
		}

		// Token: 0x0200001A RID: 26
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		private struct WIN32_FIND_DATA
		{
			// Token: 0x0400007B RID: 123
			public uint FileAttributes;

			// Token: 0x0400007C RID: 124
			public long CreationTime;

			// Token: 0x0400007D RID: 125
			public long LastAccessTime;

			// Token: 0x0400007E RID: 126
			public long LastWriteTime;

			// Token: 0x0400007F RID: 127
			public uint FileSizeHigh;

			// Token: 0x04000080 RID: 128
			public uint FileSizeLow;

			// Token: 0x04000081 RID: 129
			public uint Reserved0;

			// Token: 0x04000082 RID: 130
			public uint Reserved1;

			// Token: 0x04000083 RID: 131
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string FileName;

			// Token: 0x04000084 RID: 132
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string AlternateFileName;
		}

		// Token: 0x0200001B RID: 27
		private struct SDCardCommandHeader
		{
			// Token: 0x04000085 RID: 133
			public int CommandID;

			// Token: 0x04000086 RID: 134
			public uint DataSize;
		}

		// Token: 0x0200001C RID: 28
		private struct SDCardResponseHeader
		{
			// Token: 0x04000087 RID: 135
			public uint ReturnCode;

			// Token: 0x04000088 RID: 136
			public int LastError;
		}

		// Token: 0x0200001D RID: 29
		private struct SDCardGetMountStatusResponse
		{
			// Token: 0x04000089 RID: 137
			public XdeSDCardPipe.SDCardResponseHeader ResponseHeader;

			// Token: 0x0400008A RID: 138
			public int MountStatus;
		}

		// Token: 0x0200001E RID: 30
		private struct SDCardGetMountRootResponse
		{
			// Token: 0x0400008B RID: 139
			public XdeSDCardPipe.SDCardResponseHeader ResponseHeader;

			// Token: 0x0400008C RID: 140
			public uint DataSize;
		}

		// Token: 0x0200001F RID: 31
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct SDCardFindFirstFileResponse
		{
			// Token: 0x0400008D RID: 141
			public XdeSDCardPipe.SDCardResponseHeader ResponseHeader;

			// Token: 0x0400008E RID: 142
			public int FileHandle;

			// Token: 0x0400008F RID: 143
			public XdeSDCardPipe.WIN32_FIND_DATA FindFileData;
		}

		// Token: 0x02000020 RID: 32
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct SDCardFindNextFileResponse
		{
			// Token: 0x04000090 RID: 144
			public XdeSDCardPipe.SDCardResponseHeader ResponseHeader;

			// Token: 0x04000091 RID: 145
			public XdeSDCardPipe.WIN32_FIND_DATA FindFileData;
		}

		// Token: 0x02000021 RID: 33
		private struct SDCardCreateFileResponse
		{
			// Token: 0x04000092 RID: 146
			public XdeSDCardPipe.SDCardResponseHeader ResponseHeader;

			// Token: 0x04000093 RID: 147
			public int FileHandle;
		}

		// Token: 0x02000022 RID: 34
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct SDCardFileTimeAttributes
		{
			// Token: 0x04000094 RID: 148
			public long CreationTime;

			// Token: 0x04000095 RID: 149
			public long LastAccessTime;

			// Token: 0x04000096 RID: 150
			public long LastWriteTime;
		}

		// Token: 0x02000023 RID: 35
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct SDCardSetFileTimeRequest
		{
			// Token: 0x04000097 RID: 151
			public int FileHandle;

			// Token: 0x04000098 RID: 152
			public XdeSDCardPipe.SDCardFileTimeAttributes FileTimeAttribs;
		}

		// Token: 0x02000024 RID: 36
		private struct SDCardReadWriteFileResponse
		{
			// Token: 0x04000099 RID: 153
			public XdeSDCardPipe.SDCardResponseHeader ResponseHeader;

			// Token: 0x0400009A RID: 154
			public uint DataSize;
		}

		// Token: 0x02000025 RID: 37
		private struct SDCardGetFileAttributesResponse
		{
			// Token: 0x0400009B RID: 155
			public XdeSDCardPipe.SDCardResponseHeader ResponseHeader;

			// Token: 0x0400009C RID: 156
			public uint FileAttributes;
		}

		// Token: 0x02000026 RID: 38
		private class SyncThreadClass
		{
			// Token: 0x06000115 RID: 277 RVA: 0x00006309 File Offset: 0x00004509
			public SyncThreadClass(string folderPath, bool shouldSyncOnEject, XdeSDCardPipe sdcardPipe)
			{
				this.folderPath = folderPath;
				this.shouldSyncOnEject = shouldSyncOnEject;
				this.sdcardPipe = sdcardPipe;
			}

			// Token: 0x06000116 RID: 278 RVA: 0x00006326 File Offset: 0x00004526
			public void OnInsertCompleted(InsertEjectCompletedEventArgs e)
			{
				if (this.sdcardPipe.IsCancelSyncEventSet())
				{
					this.sdcardPipe.cancelSyncEvent.Reset();
				}
				if (this.sdcardPipe.InsertCompleted != null)
				{
					this.sdcardPipe.InsertCompleted(this, e);
				}
			}

			// Token: 0x06000117 RID: 279 RVA: 0x00006365 File Offset: 0x00004565
			public void OnEjectCompleted(InsertEjectCompletedEventArgs e)
			{
				if (this.sdcardPipe.IsCancelSyncEventSet())
				{
					this.sdcardPipe.cancelSyncEvent.Reset();
				}
				if (this.sdcardPipe.EjectCompleted != null)
				{
					this.sdcardPipe.EjectCompleted(this, e);
				}
			}

			// Token: 0x06000118 RID: 280 RVA: 0x000063A4 File Offset: 0x000045A4
			public void InsertSDCardThreadProc()
			{
				try
				{
					this.sdcardPipe.DoEnsureSDCardVolumeMounted();
					this.sdcardPipe.SyncToGuest(this.folderPath);
					this.sdcardPipe.InsertSDCardInternal();
					this.ProcessInsertCompleted(null, InsertEjectCompletedEventArgs.EventCompletionStatus.CompletedSuccessfully, true, this.folderPath, this.shouldSyncOnEject);
				}
				catch (ThreadAbortException)
				{
					this.ProcessInsertCompleted(null, InsertEjectCompletedEventArgs.EventCompletionStatus.Cancelled, false, this.folderPath, this.shouldSyncOnEject);
				}
				catch (Exception ex)
				{
					this.ProcessInsertCompleted(ex, InsertEjectCompletedEventArgs.EventCompletionStatus.CompletedWithException, false, this.folderPath, this.shouldSyncOnEject);
				}
			}

			// Token: 0x06000119 RID: 281 RVA: 0x0000643C File Offset: 0x0000463C
			public void EjectSDCardThreadProc()
			{
				bool flag = false;
				try
				{
					this.sdcardPipe.DoEnsureSDCardVolumeMounted();
					if (this.shouldSyncOnEject)
					{
						this.sdcardPipe.SyncFromGuest(this.folderPath);
					}
					flag = this.sdcardPipe.EjectSDCardInternal();
					this.ProcessEjectCompleted(null, InsertEjectCompletedEventArgs.EventCompletionStatus.CompletedSuccessfully, !flag, this.folderPath, this.shouldSyncOnEject);
				}
				catch (ThreadAbortException)
				{
					this.ProcessEjectCompleted(null, InsertEjectCompletedEventArgs.EventCompletionStatus.Cancelled, !flag, this.folderPath, this.shouldSyncOnEject);
				}
				catch (Exception ex)
				{
					this.ProcessEjectCompleted(ex, InsertEjectCompletedEventArgs.EventCompletionStatus.CompletedWithException, !flag, this.folderPath, this.shouldSyncOnEject);
				}
			}

			// Token: 0x0600011A RID: 282 RVA: 0x000064E8 File Offset: 0x000046E8
			private void ProcessInsertCompleted(Exception ex, InsertEjectCompletedEventArgs.EventCompletionStatus completionStatus, bool isSDCardInserted, string folderPath, bool shouldSyncOnEject)
			{
				InsertEjectCompletedEventArgs e = this.ProcessOperationCompleted(ex, completionStatus, isSDCardInserted, folderPath, shouldSyncOnEject);
				this.OnInsertCompleted(e);
			}

			// Token: 0x0600011B RID: 283 RVA: 0x0000650C File Offset: 0x0000470C
			private void ProcessEjectCompleted(Exception ex, InsertEjectCompletedEventArgs.EventCompletionStatus completionStatus, bool isSDCardInserted, string folderPath, bool shouldSyncOnEject)
			{
				InsertEjectCompletedEventArgs e = this.ProcessOperationCompleted(ex, completionStatus, isSDCardInserted, folderPath, shouldSyncOnEject);
				this.OnEjectCompleted(e);
			}

			// Token: 0x0600011C RID: 284 RVA: 0x00006530 File Offset: 0x00004730
			private InsertEjectCompletedEventArgs ProcessOperationCompleted(Exception ex, InsertEjectCompletedEventArgs.EventCompletionStatus completionStatus, bool isSDCardInserted, string folderPath, bool shouldSyncOnEject)
			{
				InsertEjectCompletedEventArgs insertEjectCompletedEventArgs = new InsertEjectCompletedEventArgs();
				insertEjectCompletedEventArgs.CompletionStatus = completionStatus;
				insertEjectCompletedEventArgs.IsSDCardInserted = isSDCardInserted;
				insertEjectCompletedEventArgs.SDCardException = string.Empty;
				insertEjectCompletedEventArgs.SDCardExceptionStackTrace = string.Empty;
				if (ex is PathTooLongException)
				{
					if (folderPath.Length >= 220)
					{
						string sdcardException = StringUtilities.CurrentCultureFormat(PipeExceptions.SDCardLocalFolderPathTooLong, new object[]
						{
							ex.Message
						});
						insertEjectCompletedEventArgs.SDCardException = sdcardException;
						insertEjectCompletedEventArgs.SDCardExceptionStackTrace = ex.StackTrace;
					}
				}
				else if (ex != null)
				{
					insertEjectCompletedEventArgs.SDCardException = ex.Message;
					insertEjectCompletedEventArgs.SDCardExceptionStackTrace = ex.StackTrace;
				}
				insertEjectCompletedEventArgs.HostFolderPath = folderPath;
				insertEjectCompletedEventArgs.ShouldSyncOnEject = shouldSyncOnEject;
				return insertEjectCompletedEventArgs;
			}

			// Token: 0x0400009D RID: 157
			private string folderPath;

			// Token: 0x0400009E RID: 158
			private bool shouldSyncOnEject;

			// Token: 0x0400009F RID: 159
			private XdeSDCardPipe sdcardPipe;
		}
	}
}
