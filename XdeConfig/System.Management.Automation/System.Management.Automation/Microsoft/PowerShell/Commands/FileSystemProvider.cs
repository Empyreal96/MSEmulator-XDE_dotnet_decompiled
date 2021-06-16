using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Provider;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200076A RID: 1898
	[OutputType(new Type[]
	{
		typeof(FileSecurity)
	}, ProviderCmdlet = "Set-Acl")]
	[OutputType(new Type[]
	{
		typeof(FileInfo)
	}, ProviderCmdlet = "Get-Item")]
	[OutputType(new Type[]
	{
		typeof(byte),
		typeof(string)
	}, ProviderCmdlet = "Get-Content")]
	[CmdletProvider("FileSystem", ProviderCapabilities.Filter | ProviderCapabilities.ShouldProcess | ProviderCapabilities.Credentials)]
	[OutputType(new Type[]
	{
		typeof(string),
		typeof(PathInfo)
	}, ProviderCmdlet = "Resolve-Path")]
	[OutputType(new Type[]
	{
		typeof(PathInfo)
	}, ProviderCmdlet = "Push-Location")]
	[OutputType(new Type[]
	{
		typeof(FileSecurity),
		typeof(DirectorySecurity)
	}, ProviderCmdlet = "Get-Acl")]
	[OutputType(new Type[]
	{
		typeof(FileInfo),
		typeof(DirectoryInfo)
	}, ProviderCmdlet = "Get-ChildItem")]
	[OutputType(new Type[]
	{
		typeof(bool),
		typeof(string),
		typeof(FileInfo),
		typeof(DirectoryInfo)
	}, ProviderCmdlet = "Get-Item")]
	[OutputType(new Type[]
	{
		typeof(bool),
		typeof(string),
		typeof(DateTime),
		typeof(FileInfo),
		typeof(DirectoryInfo)
	}, ProviderCmdlet = "Get-ItemProperty")]
	[OutputType(new Type[]
	{
		typeof(string),
		typeof(FileInfo)
	}, ProviderCmdlet = "New-Item")]
	public sealed class FileSystemProvider : NavigationCmdletProvider, IContentCmdletProvider, IPropertyCmdletProvider, ISecurityDescriptorCmdletProvider, ICmdletProviderSupportsHelp
	{
		// Token: 0x06004BCF RID: 19407 RVA: 0x0018D5F0 File Offset: 0x0018B7F0
		private static string NormalizePath(string path)
		{
			string text = path.Replace('/', '\\');
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x0018D626 File Offset: 0x0018B826
		private static FileSystemInfo GetFileSystemInfo(string path, ref bool isContainer)
		{
			isContainer = false;
			if (Utils.NativeFileExists(path))
			{
				return new FileInfo(path);
			}
			if (Utils.NativeDirectoryExists(path))
			{
				isContainer = true;
				return new DirectoryInfo(path);
			}
			return null;
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x0018D650 File Offset: 0x0018B850
		internal override bool IsFilterSet()
		{
			bool flag = false;
			GetChildDynamicParameters getChildDynamicParameters = base.DynamicParameters as GetChildDynamicParameters;
			if (getChildDynamicParameters != null)
			{
				flag = (getChildDynamicParameters.Attributes != null || getChildDynamicParameters.Directory || getChildDynamicParameters.File || getChildDynamicParameters.Hidden || getChildDynamicParameters.ReadOnly || getChildDynamicParameters.System);
			}
			return flag || base.IsFilterSet();
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x0018D6C4 File Offset: 0x0018B8C4
		protected override object GetChildNamesDynamicParameters(string path)
		{
			return new GetChildDynamicParameters();
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x0018D6CB File Offset: 0x0018B8CB
		protected override object GetChildItemsDynamicParameters(string path, bool recurse)
		{
			return new GetChildDynamicParameters();
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x0018D6D2 File Offset: 0x0018B8D2
		protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
		{
			return new CopyItemDynamicParameters();
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x0018D6DC File Offset: 0x0018B8DC
		public string GetHelpMaml(string helpItemName, string path)
		{
			string text = null;
			string text2 = null;
			XmlReader xmlReader = null;
			try
			{
				if (string.IsNullOrEmpty(helpItemName))
				{
					return string.Empty;
				}
				CmdletInfo.SplitCmdletName(helpItemName, out text, out text2);
				if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
				{
					return string.Empty;
				}
				XmlDocument xmlDocument = new XmlDocument();
				CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
				string inputUri = Path.Combine(string.IsNullOrEmpty(base.ProviderInfo.ApplicationBase) ? "" : base.ProviderInfo.ApplicationBase, currentUICulture.ToString(), string.IsNullOrEmpty(base.ProviderInfo.HelpFile) ? "" : base.ProviderInfo.HelpFile);
				xmlReader = XmlReader.Create(inputUri, new XmlReaderSettings
				{
					XmlResolver = null
				});
				xmlDocument.Load(xmlReader);
				XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
				xmlNamespaceManager.AddNamespace("command", HelpCommentsParser.commandURI);
				string xpath = string.Format(CultureInfo.InvariantCulture, HelpCommentsParser.ProviderHelpCommandXPath, new object[]
				{
					"[@id='FileSystem']",
					text,
					text2
				});
				XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath, xmlNamespaceManager);
				if (xmlNode != null)
				{
					return xmlNode.OuterXml;
				}
			}
			catch (XmlException)
			{
				return string.Empty;
			}
			catch (PathTooLongException)
			{
				return string.Empty;
			}
			catch (IOException)
			{
				return string.Empty;
			}
			catch (UnauthorizedAccessException)
			{
				return string.Empty;
			}
			catch (NotSupportedException)
			{
				return string.Empty;
			}
			catch (SecurityException)
			{
				return string.Empty;
			}
			catch (XPathException)
			{
				return string.Empty;
			}
			finally
			{
				if (xmlReader != null)
				{
					((IDisposable)xmlReader).Dispose();
				}
			}
			return string.Empty;
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x0018D938 File Offset: 0x0018BB38
		protected override ProviderInfo Start(ProviderInfo providerInfo)
		{
			if (providerInfo != null && string.IsNullOrEmpty(providerInfo.Home))
			{
				string environmentVariable = Environment.GetEnvironmentVariable("USERPROFILE");
				if (!string.IsNullOrEmpty(environmentVariable))
				{
					if (Directory.Exists(environmentVariable))
					{
						FileSystemProvider.tracer.WriteLine("Home = {0}", new object[]
						{
							environmentVariable
						});
						providerInfo.Home = environmentVariable;
					}
					else
					{
						FileSystemProvider.tracer.WriteLine("Not setting home directory {0} - does not exist", new object[]
						{
							environmentVariable
						});
					}
				}
			}
			return providerInfo;
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x0018D9B0 File Offset: 0x0018BBB0
		protected override PSDriveInfo NewDrive(PSDriveInfo drive)
		{
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			if (string.IsNullOrEmpty(drive.Root))
			{
				throw PSTraceSource.NewArgumentException("drive.Root");
			}
			if (drive.Persist && !FileSystemProvider.NativeMethods.PathIsNetworkPath(drive.Root))
			{
				ErrorRecord errorRecord = new ErrorRecord(new NotSupportedException(FileSystemProviderStrings.PersistNotSupported), "DriveRootNotNetworkPath", ErrorCategory.InvalidArgument, drive);
				base.ThrowTerminatingError(errorRecord);
			}
			if (this.IsNetworkMappedDrive(drive))
			{
				this.MapNetworkDrive(drive);
			}
			bool flag = true;
			PSDriveInfo result = null;
			try
			{
				string pathRoot = Path.GetPathRoot(drive.Root);
				DriveInfo driveInfo = new DriveInfo(pathRoot);
				if (driveInfo.DriveType != DriveType.Fixed)
				{
					flag = false;
				}
				if (driveInfo.DriveType == DriveType.Network)
				{
					drive.IsNetworkDrive = true;
				}
			}
			catch (ArgumentException)
			{
			}
			bool flag2 = true;
			if (flag)
			{
				try
				{
					flag2 = Utils.NativeDirectoryExists(drive.Root);
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			if (flag2)
			{
				result = drive;
			}
			else
			{
				string message = StringUtil.Format(FileSystemProviderStrings.DriveRootError, drive.Root);
				Exception exception = new IOException(message);
				base.WriteError(new ErrorRecord(exception, "DriveRootError", ErrorCategory.ReadError, drive));
			}
			drive.Trace();
			return result;
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x0018DAE8 File Offset: 0x0018BCE8
		private void MapNetworkDrive(PSDriveInfo drive)
		{
			if (drive != null && !string.IsNullOrEmpty(drive.Root))
			{
				int num = 0;
				string text = null;
				byte[] array = null;
				string username = null;
				if (drive.Persist)
				{
					if (this.IsSupportedDriveForPersistence(drive))
					{
						num = 1;
						text = drive.Name + ":";
						drive.DisplayRoot = drive.Root;
					}
					else
					{
						ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(FileSystemProviderStrings.InvalidDriveName), "DriveNameNotSupportedForPersistence", ErrorCategory.InvalidOperation, drive);
						base.ThrowTerminatingError(errorRecord);
					}
				}
				if (drive.Credential != null && !drive.Credential.Equals(PSCredential.Empty))
				{
					username = drive.Credential.UserName;
					array = SecureStringHelper.GetData(drive.Credential.Password);
				}
				try
				{
					FileSystemProvider.NetResource netResource = default(FileSystemProvider.NetResource);
					netResource.Comment = null;
					netResource.DisplayType = 0;
					netResource.LocalName = text;
					netResource.Provider = null;
					netResource.RemoteName = drive.Root;
					netResource.Scope = 2;
					netResource.Type = 0;
					netResource.Usage = 1;
					int num2 = FileSystemProvider.NativeMethods.WNetAddConnection2(ref netResource, array, username, num);
					if (num2 != 0)
					{
						ErrorRecord errorRecord2 = new ErrorRecord(new Win32Exception(num2), "CouldNotMapNetworkDrive", ErrorCategory.InvalidOperation, drive);
						base.ThrowTerminatingError(errorRecord2);
					}
					if (num == 1)
					{
						drive.IsNetworkDrive = true;
						drive.Root = text + "\\";
					}
				}
				finally
				{
					if (array != null)
					{
						Array.Clear(array, 0, array.Length - 1);
					}
				}
			}
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x0018DC5C File Offset: 0x0018BE5C
		private bool IsNetworkMappedDrive(PSDriveInfo drive)
		{
			return drive != null && !string.IsNullOrEmpty(drive.Root) && FileSystemProvider.NativeMethods.PathIsNetworkPath(drive.Root) && (drive.Persist || (drive.Credential != null && !drive.Credential.Equals(PSCredential.Empty)));
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x0018DCBC File Offset: 0x0018BEBC
		protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
		{
			if (this.IsNetworkMappedDrive(drive))
			{
				int flags = 0;
				string driveName;
				if (drive.IsNetworkDrive)
				{
					flags = 1;
					driveName = drive.Name + ":";
				}
				else
				{
					driveName = drive.Root;
				}
				int num = FileSystemProvider.NativeMethods.WNetCancelConnection2(driveName, flags, true);
				if (num != 0)
				{
					ErrorRecord errorRecord = new ErrorRecord(new Win32Exception(num), "CouldRemoveNetworkDrive", ErrorCategory.InvalidOperation, drive);
					base.ThrowTerminatingError(errorRecord);
				}
			}
			return drive;
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x0018DD20 File Offset: 0x0018BF20
		private bool IsSupportedDriveForPersistence(PSDriveInfo drive)
		{
			bool result = false;
			if (drive != null && !string.IsNullOrEmpty(drive.Name) && drive.Name.Length == 1)
			{
				char c = Convert.ToChar(drive.Name, CultureInfo.InvariantCulture);
				if (char.ToUpperInvariant(c) >= 'A' && char.ToUpperInvariant(c) <= 'Z')
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x0018DD7C File Offset: 0x0018BF7C
		internal static string GetUNCForNetworkDrive(string driveName)
		{
			string result = null;
			if (!string.IsNullOrEmpty(driveName) && driveName.Length == 1)
			{
				int capacity = 300;
				StringBuilder stringBuilder = new StringBuilder(capacity);
				driveName += ':';
				int num = FileSystemProvider.NativeMethods.WNetGetConnection(driveName, stringBuilder, ref capacity);
				if (num == 234)
				{
					stringBuilder = new StringBuilder(capacity);
					num = FileSystemProvider.NativeMethods.WNetGetConnection(driveName, stringBuilder, ref capacity);
				}
				if (num != 0)
				{
					throw new Win32Exception(num);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x0018DDF0 File Offset: 0x0018BFF0
		internal static string GetSubstitutedPathForNetworkDosDevice(string driveName)
		{
			string text = null;
			if (!string.IsNullOrEmpty(driveName) && driveName.Length == 1)
			{
				int num = 300;
				StringBuilder stringBuilder = new StringBuilder(num);
				driveName += ':';
				int lastWin32Error;
				for (;;)
				{
					stringBuilder.EnsureCapacity(num);
					int num2 = FileSystemProvider.NativeMethods.QueryDosDevice(driveName, stringBuilder, num);
					if (num2 > 0)
					{
						break;
					}
					lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error != 122)
					{
						goto Block_7;
					}
					if (num >= 32767)
					{
						goto Block_8;
					}
					num *= 10;
					if (num > 32767)
					{
						num = 32767;
					}
				}
				text = stringBuilder.ToString();
				if (!text.StartsWith("\\??\\", StringComparison.OrdinalIgnoreCase))
				{
					return driveName + "\\";
				}
				text = text.Remove(0, 4);
				if (text.StartsWith("UNC", StringComparison.OrdinalIgnoreCase))
				{
					text = text.Remove(0, 3);
					return "\\" + text;
				}
				if (text.EndsWith(":", StringComparison.OrdinalIgnoreCase))
				{
					return text + Path.DirectorySeparatorChar;
				}
				return text;
				Block_7:
				throw new Win32Exception(lastWin32Error);
				Block_8:
				string message = StringUtil.Format(FileSystemProviderStrings.SubstitutePathTooLong, driveName);
				throw new InvalidOperationException(message);
			}
			return text;
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x0018DF08 File Offset: 0x0018C108
		internal static string GetRootPathForNetworkDriveOrDosDevice(DriveInfo driveInfo)
		{
			string driveName = driveInfo.Name.Substring(0, 1);
			string result = null;
			try
			{
				result = FileSystemProvider.GetUNCForNetworkDrive(driveName);
			}
			catch (Win32Exception)
			{
				if (!driveInfo.IsReady)
				{
					throw;
				}
				result = FileSystemProvider.GetSubstitutedPathForNetworkDosDevice(driveName);
			}
			return result;
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x0018DF58 File Offset: 0x0018C158
		protected override Collection<PSDriveInfo> InitializeDefaultDrives()
		{
			Collection<PSDriveInfo> collection = new Collection<PSDriveInfo>();
			DriveInfo[] drives = DriveInfo.GetDrives();
			if (drives != null)
			{
				foreach (DriveInfo driveInfo in drives)
				{
					if (base.Stopping)
					{
						collection.Clear();
						break;
					}
					string name = driveInfo.Name.Substring(0, 1);
					string description = string.Empty;
					string root = driveInfo.Name;
					string displayRoot = null;
					if (driveInfo.DriveType == DriveType.Fixed)
					{
						try
						{
							description = driveInfo.VolumeLabel;
						}
						catch (IOException)
						{
						}
						catch (SecurityException)
						{
						}
						catch (UnauthorizedAccessException)
						{
						}
					}
					if (driveInfo.DriveType == DriveType.Network)
					{
						displayRoot = FileSystemProvider.GetRootPathForNetworkDriveOrDosDevice(driveInfo);
					}
					try
					{
						if (driveInfo.DriveType == DriveType.Fixed)
						{
							if (!driveInfo.RootDirectory.Exists)
							{
								goto IL_F2;
							}
							root = driveInfo.RootDirectory.FullName;
						}
						PSDriveInfo psdriveInfo = new PSDriveInfo(name, base.ProviderInfo, root, description, null, displayRoot);
						if (driveInfo.DriveType == DriveType.Network)
						{
							psdriveInfo.IsNetworkDrive = true;
						}
						if (driveInfo.DriveType != DriveType.Fixed)
						{
							psdriveInfo.IsAutoMounted = true;
						}
						collection.Add(psdriveInfo);
					}
					catch (IOException)
					{
					}
					catch (SecurityException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
					IL_F2:;
				}
			}
			return collection;
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0018E0B8 File Offset: 0x0018C2B8
		protected override object GetItemDynamicParameters(string path)
		{
			return new FileSystemProviderGetItemDynamicParameters();
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x0018E0C0 File Offset: 0x0018C2C0
		protected override bool IsValidPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			path = FileSystemProvider.NormalizePath(path);
			path = FileSystemProvider.EnsureDriveIsRooted(path);
			int num = path.IndexOf(':');
			int num2 = path.IndexOf(':', num + 1);
			if (num2 > 0)
			{
				path = path.Substring(0, num2);
			}
			if (!FileSystemProvider.IsAbsolutePath(path) && !FileSystemProvider.IsUNCPath(path))
			{
				return false;
			}
			try
			{
				new FileInfo(path);
			}
			catch (Exception ex)
			{
				if (ex is ArgumentNullException || ex is ArgumentException || ex is SecurityException || ex is UnauthorizedAccessException || ex is PathTooLongException || ex is NotSupportedException)
				{
					return false;
				}
				throw;
			}
			return true;
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x0018E170 File Offset: 0x0018C370
		protected override void GetItem(string path)
		{
			bool flag = false;
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			try
			{
				bool flag2 = false;
				FileSystemProviderGetItemDynamicParameters fileSystemProviderGetItemDynamicParameters = null;
				if (base.DynamicParameters != null)
				{
					fileSystemProviderGetItemDynamicParameters = (base.DynamicParameters as FileSystemProviderGetItemDynamicParameters);
					if (fileSystemProviderGetItemDynamicParameters != null)
					{
						if (fileSystemProviderGetItemDynamicParameters.Stream != null && fileSystemProviderGetItemDynamicParameters.Stream.Length > 0)
						{
							flag2 = true;
						}
						else
						{
							int num = path.IndexOf(':');
							int num2 = path.IndexOf(':', num + 1);
							if (num2 > 0)
							{
								string text = path.Substring(num2 + 1);
								path = path.Remove(num2);
								flag2 = true;
								fileSystemProviderGetItemDynamicParameters = new FileSystemProviderGetItemDynamicParameters();
								fileSystemProviderGetItemDynamicParameters.Stream = new string[]
								{
									text
								};
							}
						}
					}
				}
				FileSystemInfo fileSystemItem = this.GetFileSystemItem(path, ref flag, false);
				if (fileSystemItem != null)
				{
					if (flag2)
					{
						if (!flag)
						{
							foreach (string text2 in fileSystemProviderGetItemDynamicParameters.Stream)
							{
								WildcardPattern wildcardPattern = new WildcardPattern(text2, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
								bool flag3 = false;
								foreach (AlternateStreamData alternateStreamData in AlternateDataStreamUtilities.GetStreams(fileSystemItem.FullName))
								{
									if (wildcardPattern.IsMatch(alternateStreamData.Stream))
									{
										string path2 = fileSystemItem.FullName + ":" + alternateStreamData.Stream;
										base.WriteItemObject(alternateStreamData, path2, flag);
										flag3 = true;
									}
								}
								if (!WildcardPattern.ContainsWildcardCharacters(text2) && !flag3)
								{
									string message = StringUtil.Format(FileSystemProviderStrings.AlternateDataStreamNotFound, text2, fileSystemItem.FullName);
									Exception exception = new FileNotFoundException(message, fileSystemItem.FullName);
									base.WriteError(new ErrorRecord(exception, "AlternateDataStreamNotFound", ErrorCategory.ObjectNotFound, path));
								}
							}
						}
					}
					else
					{
						base.WriteItemObject(fileSystemItem, fileSystemItem.FullName, flag);
					}
				}
				else
				{
					string message2 = StringUtil.Format(FileSystemProviderStrings.ItemNotFound, path);
					Exception exception2 = new IOException(message2);
					base.WriteError(new ErrorRecord(exception2, "ItemNotFound", ErrorCategory.ObjectNotFound, path));
				}
			}
			catch (IOException exception3)
			{
				ErrorRecord errorRecord = new ErrorRecord(exception3, "GetItemIOError", ErrorCategory.ReadError, path);
				base.WriteError(errorRecord);
			}
			catch (UnauthorizedAccessException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "GetItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x0018E3E0 File Offset: 0x0018C5E0
		private FileSystemInfo GetFileSystemItem(string path, ref bool isContainer, bool showHidden)
		{
			path = FileSystemProvider.NormalizePath(path);
			FileSystemInfo result = null;
			int num = FileSystemProvider.SafeGetFileAttributes(path);
			bool flag = num != -1;
			bool flag2 = (num & 16) == 16;
			bool flag3 = (num & 2) == 2;
			FlagsExpression<FileAttributes> flagsExpression = null;
			FlagsExpression<FileAttributes> flagsExpression2 = null;
			GetChildDynamicParameters getChildDynamicParameters = base.DynamicParameters as GetChildDynamicParameters;
			if (getChildDynamicParameters != null)
			{
				flagsExpression = getChildDynamicParameters.Attributes;
				flagsExpression2 = this.FormatAttributeSwitchParamters();
			}
			bool flag4 = false;
			bool flag5 = false;
			if (flagsExpression != null)
			{
				flag4 = flagsExpression.ExistsInExpression(FileAttributes.Hidden);
			}
			if (flagsExpression2 != null)
			{
				flag5 = flagsExpression2.ExistsInExpression(FileAttributes.Hidden);
			}
			if (flag && !flag2 && (!flag3 || base.Force || showHidden || flag4 || flag5))
			{
				FileInfo fileInfo = new FileInfo(path);
				result = fileInfo;
				FileSystemProvider.tracer.WriteLine("Got FileInfo: {0}", new object[]
				{
					fileInfo
				});
			}
			else
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				bool flag6 = string.Compare(Path.GetPathRoot(path), directoryInfo.FullName, StringComparison.OrdinalIgnoreCase) == 0;
				if (flag && (flag6 || !flag3 || base.Force || showHidden || flag4 || flag5))
				{
					result = directoryInfo;
					isContainer = true;
					FileSystemProvider.tracer.WriteLine("Got DirectoryInfo: {0}", new object[]
					{
						directoryInfo
					});
				}
			}
			return result;
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x0018E518 File Offset: 0x0018C718
		protected override void InvokeDefaultAction(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			string invokeItemAction = FileSystemProviderStrings.InvokeItemAction;
			string target = StringUtil.Format(FileSystemProviderStrings.InvokeItemResourceFileTemplate, path);
			if (base.ShouldProcess(target, invokeItemAction))
			{
				Process.Start(path);
			}
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x0018E563 File Offset: 0x0018C763
		protected override void GetChildItems(string path, bool recurse, uint depth)
		{
			this.GetPathItems(path, recurse, depth, false, ReturnContainers.ReturnMatchingContainers);
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x0018E570 File Offset: 0x0018C770
		protected override void GetChildNames(string path, ReturnContainers returnContainers)
		{
			this.GetPathItems(path, false, uint.MaxValue, true, returnContainers);
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x0018E580 File Offset: 0x0018C780
		protected override bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
		{
			if (!string.IsNullOrEmpty(filter) || path.Contains('\\'.ToString()) || path.Contains('/'.ToString()) || path.Contains("`"))
			{
				return false;
			}
			updatedPath = path;
			updatedFilter = Regex.Replace(path, "\\[.*?\\]", "?");
			return true;
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x0018E5E0 File Offset: 0x0018C7E0
		private void GetPathItems(string path, bool recurse, uint depth, bool nameOnly, ReturnContainers returnContainers)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			bool flag2;
			Exception ex;
			bool flag = Utils.NativeItemExists(path, out flag2, out ex);
			if (ex != null)
			{
				throw ex;
			}
			if (flag)
			{
				if (flag2)
				{
					DirectoryInfo directory = new DirectoryInfo(path);
					this.Dir(directory, recurse, depth, nameOnly, returnContainers);
					return;
				}
				FileInfo fileInfo = new FileInfo(path);
				FlagsExpression<FileAttributes> flagsExpression = null;
				FlagsExpression<FileAttributes> flagsExpression2 = null;
				GetChildDynamicParameters getChildDynamicParameters = base.DynamicParameters as GetChildDynamicParameters;
				if (getChildDynamicParameters != null)
				{
					flagsExpression = getChildDynamicParameters.Attributes;
					flagsExpression2 = this.FormatAttributeSwitchParamters();
				}
				bool flag3 = true;
				bool flag4 = true;
				bool flag5 = false;
				bool flag6 = false;
				if (flagsExpression != null)
				{
					flag3 = flagsExpression.Evaluate(fileInfo.Attributes);
					flag5 = flagsExpression.ExistsInExpression(FileAttributes.Hidden);
				}
				if (flagsExpression2 != null)
				{
					flag4 = flagsExpression2.Evaluate(fileInfo.Attributes);
					flag6 = flagsExpression2.ExistsInExpression(FileAttributes.Hidden);
				}
				bool flag7 = (fileInfo.Attributes & FileAttributes.Hidden) != (FileAttributes)0;
				if (flag3 && flag4 && (flag5 || flag6 || base.Force || !flag7))
				{
					if (nameOnly)
					{
						base.WriteItemObject(fileInfo.Name, fileInfo.FullName, false);
						return;
					}
					base.WriteItemObject(fileInfo, path, false);
					return;
				}
			}
			else
			{
				string message = StringUtil.Format(FileSystemProviderStrings.ItemDoesNotExist, path);
				Exception exception = new IOException(message);
				base.WriteError(new ErrorRecord(exception, "ItemDoesNotExist", ErrorCategory.ObjectNotFound, path));
			}
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x0018E730 File Offset: 0x0018C930
		private void Dir(DirectoryInfo directory, bool recurse, uint depth, bool nameOnly, ReturnContainers returnContainers)
		{
			List<IEnumerable<FileSystemInfo>> list = new List<IEnumerable<FileSystemInfo>>();
			try
			{
				if (base.Filter != null && base.Filter.Length > 0)
				{
					if (returnContainers == ReturnContainers.ReturnAllContainers)
					{
						list.Add(directory.EnumerateDirectories());
					}
					else
					{
						list.Add(directory.EnumerateDirectories(base.Filter));
					}
					if (base.Stopping)
					{
						return;
					}
					list.Add(directory.EnumerateFiles(base.Filter));
				}
				else
				{
					list.Add(directory.EnumerateDirectories());
					if (base.Stopping)
					{
						return;
					}
					list.Add(directory.EnumerateFiles());
				}
				FlagsExpression<FileAttributes> flagsExpression = null;
				FlagsExpression<FileAttributes> flagsExpression2 = null;
				GetChildDynamicParameters getChildDynamicParameters = base.DynamicParameters as GetChildDynamicParameters;
				if (getChildDynamicParameters != null)
				{
					flagsExpression = getChildDynamicParameters.Attributes;
					flagsExpression2 = this.FormatAttributeSwitchParamters();
				}
				foreach (IEnumerable<FileSystemInfo> enumerable in list)
				{
					foreach (FileSystemInfo fileSystemInfo in enumerable)
					{
						if (base.Stopping)
						{
							return;
						}
						bool flag = true;
						bool flag2 = true;
						bool flag3 = false;
						bool flag4 = false;
						if (flagsExpression != null)
						{
							flag = flagsExpression.Evaluate(fileSystemInfo.Attributes);
							flag3 = flagsExpression.ExistsInExpression(FileAttributes.Hidden);
						}
						if (flagsExpression2 != null)
						{
							flag2 = flagsExpression2.Evaluate(fileSystemInfo.Attributes);
							flag4 = flagsExpression2.ExistsInExpression(FileAttributes.Hidden);
						}
						bool flag5 = false;
						if (!base.Force)
						{
							flag5 = ((fileSystemInfo.Attributes & FileAttributes.Hidden) != (FileAttributes)0);
						}
						bool flag6 = (flag && flag2) || (returnContainers == ReturnContainers.ReturnAllContainers && (fileSystemInfo.Attributes & FileAttributes.Directory) != (FileAttributes)0);
						if (flag6 && (flag3 || flag4 || base.Force || !flag5))
						{
							if (nameOnly)
							{
								base.WriteItemObject(fileSystemInfo.Name, fileSystemInfo.FullName, false);
							}
							else if (fileSystemInfo is FileInfo)
							{
								base.WriteItemObject(fileSystemInfo, fileSystemInfo.FullName, false);
							}
							else
							{
								base.WriteItemObject(fileSystemInfo, fileSystemInfo.FullName, true);
							}
						}
					}
				}
				bool flag7 = false;
				bool flag8 = false;
				if (flagsExpression != null)
				{
					flag7 = flagsExpression.ExistsInExpression(FileAttributes.Hidden);
				}
				if (flagsExpression2 != null)
				{
					flag8 = flagsExpression2.ExistsInExpression(FileAttributes.Hidden);
				}
				if (recurse && depth > 0U)
				{
					foreach (DirectoryInfo directoryInfo in directory.EnumerateDirectories())
					{
						if (base.Stopping)
						{
							break;
						}
						bool flag9 = false;
						if (!base.Force)
						{
							flag9 = ((directoryInfo.Attributes & FileAttributes.Hidden) != (FileAttributes)0);
						}
						if (base.Force || !flag9 || flag7 || flag8)
						{
							this.Dir(directoryInfo, recurse, depth - 1U, nameOnly, returnContainers);
						}
					}
				}
			}
			catch (ArgumentException exception)
			{
				base.WriteError(new ErrorRecord(exception, "DirArgumentError", ErrorCategory.InvalidArgument, directory.FullName));
			}
			catch (IOException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "DirIOError", ErrorCategory.ReadError, directory.FullName));
			}
			catch (UnauthorizedAccessException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "DirUnauthorizedAccessError", ErrorCategory.PermissionDenied, directory.FullName));
			}
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x0018EAE0 File Offset: 0x0018CCE0
		private FlagsExpression<FileAttributes> FormatAttributeSwitchParamters()
		{
			FlagsExpression<FileAttributes> result = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (((GetChildDynamicParameters)base.DynamicParameters).Directory)
			{
				stringBuilder.Append("+Directory");
			}
			if (((GetChildDynamicParameters)base.DynamicParameters).File)
			{
				stringBuilder.Append("+!Directory");
			}
			if (((GetChildDynamicParameters)base.DynamicParameters).System)
			{
				stringBuilder.Append("+System");
			}
			if (((GetChildDynamicParameters)base.DynamicParameters).ReadOnly)
			{
				stringBuilder.Append("+ReadOnly");
			}
			if (((GetChildDynamicParameters)base.DynamicParameters).Hidden)
			{
				stringBuilder.Append("+Hidden");
			}
			string text = stringBuilder.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				result = new FlagsExpression<FileAttributes>(text.Substring(1));
			}
			return result;
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x0018EBC4 File Offset: 0x0018CDC4
		public static string Mode(PSObject instance)
		{
			if (instance == null)
			{
				return string.Empty;
			}
			FileSystemInfo fileSystemInfo = (FileSystemInfo)instance.BaseObject;
			if (fileSystemInfo == null)
			{
				return string.Empty;
			}
			string text = "";
			if ((fileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
			{
				text += "d";
			}
			else
			{
				text += "-";
			}
			if ((fileSystemInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
			{
				text += "a";
			}
			else
			{
				text += "-";
			}
			if ((fileSystemInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				text += "r";
			}
			else
			{
				text += "-";
			}
			if ((fileSystemInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
			{
				text += "h";
			}
			else
			{
				text += "-";
			}
			if ((fileSystemInfo.Attributes & FileAttributes.System) == FileAttributes.System)
			{
				text += "s";
			}
			else
			{
				text += "-";
			}
			bool flag = false;
			if ((fileSystemInfo.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
			{
				IntPtr preexistingHandle = InternalSymbolicLinkLinkCodeMethods.CreateFile(fileSystemInfo.FullName, (InternalSymbolicLinkLinkCodeMethods.FileDesiredAccess)2147483648U, InternalSymbolicLinkLinkCodeMethods.FileShareMode.Read, IntPtr.Zero, InternalSymbolicLinkLinkCodeMethods.FileCreationDisposition.OpenExisting, InternalSymbolicLinkLinkCodeMethods.FileAttributes.Normal, IntPtr.Zero);
				using (SafeFileHandle safeFileHandle = new SafeFileHandle(preexistingHandle, true))
				{
					bool flag2 = false;
					try
					{
						safeFileHandle.DangerousAddRef(ref flag2);
						IntPtr intPtr = safeFileHandle.DangerousGetHandle();
						flag = InternalSymbolicLinkLinkCodeMethods.IsHardLink(ref intPtr);
					}
					finally
					{
						if (flag2)
						{
							safeFileHandle.DangerousRelease();
						}
					}
				}
			}
			if ((fileSystemInfo.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint || flag)
			{
				text += "l";
			}
			else
			{
				text += "-";
			}
			return text;
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x0018ED74 File Offset: 0x0018CF74
		protected override void RenameItem(string path, string newName)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			if (string.IsNullOrEmpty(newName))
			{
				throw PSTraceSource.NewArgumentException("newName");
			}
			if (newName.StartsWith(".\\", StringComparison.OrdinalIgnoreCase) || newName.StartsWith("./", StringComparison.OrdinalIgnoreCase))
			{
				newName = newName.Remove(0, 2);
			}
			else if (string.Equals(Path.GetDirectoryName(path), Path.GetDirectoryName(newName), StringComparison.OrdinalIgnoreCase))
			{
				newName = Path.GetFileName(newName);
			}
			if (string.Compare(Path.GetFileName(newName), newName, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw PSTraceSource.NewArgumentException("newName", FileSystemProviderStrings.RenameError, new object[0]);
			}
			if (Utils.IsReservedDeviceName(newName))
			{
				string message = StringUtil.Format(FileSystemProviderStrings.TargetCannotContainDeviceName, newName);
				Exception exception = new IOException(message);
				base.WriteError(new ErrorRecord(exception, "RenameError", ErrorCategory.WriteError, newName));
				return;
			}
			try
			{
				bool flag = this.IsItemContainer(path);
				if (flag)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					string fullName = directoryInfo.Parent.FullName;
					string text = this.MakePath(fullName, newName);
					string renameItemActionDirectory = FileSystemProviderStrings.RenameItemActionDirectory;
					string target = StringUtil.Format(FileSystemProviderStrings.RenameItemResourceFileTemplate, directoryInfo.FullName, text);
					if (base.ShouldProcess(target, renameItemActionDirectory))
					{
						directoryInfo.MoveTo(text);
						FileSystemInfo fileSystemInfo = directoryInfo;
						base.WriteItemObject(fileSystemInfo, fileSystemInfo.FullName, flag);
					}
				}
				else
				{
					FileInfo fileInfo = new FileInfo(path);
					string directoryName = fileInfo.DirectoryName;
					string text2 = this.MakePath(directoryName, newName);
					string renameItemActionFile = FileSystemProviderStrings.RenameItemActionFile;
					string target2 = StringUtil.Format(FileSystemProviderStrings.RenameItemResourceFileTemplate, fileInfo.FullName, text2);
					if (base.ShouldProcess(target2, renameItemActionFile))
					{
						fileInfo.MoveTo(text2);
						FileSystemInfo fileSystemInfo = fileInfo;
						base.WriteItemObject(fileSystemInfo, fileSystemInfo.FullName, flag);
					}
				}
			}
			catch (ArgumentException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "RenameItemArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "RenameItemIOError", ErrorCategory.WriteError, path));
			}
			catch (UnauthorizedAccessException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "RenameItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x0018EF8C File Offset: 0x0018D18C
		protected override void NewItem(string path, string type, object value)
		{
			FileSystemProvider.ItemType itemType = FileSystemProvider.ItemType.Unknown;
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (string.IsNullOrEmpty(type))
			{
				type = "file";
			}
			path = FileSystemProvider.NormalizePath(path);
			if (base.Force && !this.CreateIntermediateDirectories(path))
			{
				return;
			}
			itemType = FileSystemProvider.GetItemType(type);
			if (itemType == FileSystemProvider.ItemType.Directory)
			{
				this.CreateDirectory(path, true);
				return;
			}
			if (itemType == FileSystemProvider.ItemType.File)
			{
				try
				{
					FileMode mode = FileMode.CreateNew;
					if (base.Force)
					{
						mode = FileMode.Create;
					}
					string newItemActionFile = FileSystemProviderStrings.NewItemActionFile;
					string target = StringUtil.Format(FileSystemProviderStrings.NewItemActionTemplate, path);
					if (base.ShouldProcess(target, newItemActionFile))
					{
						using (FileStream fileStream = new FileStream(path, mode, FileAccess.Write, FileShare.None))
						{
							if (value != null)
							{
								StreamWriter streamWriter = new StreamWriter(fileStream);
								streamWriter.Write(value.ToString());
								streamWriter.Flush();
								streamWriter.Dispose();
							}
						}
						FileInfo item = new FileInfo(path);
						base.WriteItemObject(item, path, false);
					}
					return;
				}
				catch (IOException exception)
				{
					base.WriteError(new ErrorRecord(exception, "NewItemIOError", ErrorCategory.WriteError, path));
					return;
				}
				catch (UnauthorizedAccessException exception2)
				{
					base.WriteError(new ErrorRecord(exception2, "NewItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
					return;
				}
			}
			if (itemType != FileSystemProvider.ItemType.SymbolicLink && itemType != FileSystemProvider.ItemType.HardLink)
			{
				if (itemType == FileSystemProvider.ItemType.Junction)
				{
					string newItemActionJunction = FileSystemProviderStrings.NewItemActionJunction;
					string target2 = StringUtil.Format(FileSystemProviderStrings.NewItemActionTemplate, path);
					if (!base.ShouldProcess(target2, newItemActionJunction))
					{
						return;
					}
					bool flag = false;
					string text = value.ToString();
					bool flag2 = false;
					try
					{
						flag2 = FileSystemProvider.CheckItemExists(text, out flag);
					}
					catch (Exception exception3)
					{
						base.WriteError(new ErrorRecord(exception3, "AccessException", ErrorCategory.PermissionDenied, text));
						return;
					}
					if (!flag2)
					{
						base.WriteError(new ErrorRecord(new InvalidOperationException(FileSystemProviderStrings.ItemNotFound), "ItemNotFound", ErrorCategory.ObjectNotFound, value));
						return;
					}
					if (!flag)
					{
						string message = StringUtil.Format(FileSystemProviderStrings.ItemNotDirectory, value);
						base.WriteError(new ErrorRecord(new InvalidOperationException(message), "ItemNotDirectory", ErrorCategory.InvalidOperation, value));
						return;
					}
					bool flag3 = false;
					bool flag4 = false;
					try
					{
						flag4 = FileSystemProvider.CheckItemExists(path, out flag3);
					}
					catch (Exception exception4)
					{
						base.WriteError(new ErrorRecord(exception4, "AccessException", ErrorCategory.PermissionDenied, text));
						return;
					}
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					if (flag4)
					{
						if (!flag3)
						{
							string message2 = StringUtil.Format(FileSystemProviderStrings.ItemNotDirectory, path);
							base.WriteError(new ErrorRecord(new InvalidOperationException(message2), "ItemNotDirectory", ErrorCategory.InvalidOperation, path));
							return;
						}
						if (FileSystemProvider.DirectoryInfoHasChildItems(directoryInfo))
						{
							string message3 = StringUtil.Format(FileSystemProviderStrings.DirectoryNotEmpty, path);
							base.WriteError(new ErrorRecord(new IOException(message3), "DirectoryNotEmpty", ErrorCategory.WriteError, path));
							return;
						}
						if (!base.Force)
						{
							goto IL_586;
						}
						try
						{
							directoryInfo.Delete();
							goto IL_586;
						}
						catch (Exception ex)
						{
							if (ex is DirectoryNotFoundException || ex is UnauthorizedAccessException || ex is SecurityException || ex is IOException)
							{
								base.WriteError(new ErrorRecord(ex, "NewItemDeleteIOError", ErrorCategory.WriteError, path));
								goto IL_586;
							}
							throw;
						}
					}
					this.CreateDirectory(path, false);
					try
					{
						IL_586:
						bool flag5 = InternalSymbolicLinkLinkCodeMethods.CreateJunction(path, text);
						if (flag5)
						{
							base.WriteItemObject(directoryInfo, path, true);
						}
						else if (!flag4)
						{
							directoryInfo.Delete();
						}
						return;
					}
					catch (Exception ex2)
					{
						if (!flag4)
						{
							directoryInfo.Delete();
						}
						if (ex2 is FileNotFoundException || ex2 is DirectoryNotFoundException || ex2 is UnauthorizedAccessException || ex2 is SecurityException || ex2 is ArgumentException || ex2 is PathTooLongException || ex2 is NotSupportedException || ex2 is ArgumentNullException || ex2 is Win32Exception || ex2 is IOException)
						{
							base.WriteError(new ErrorRecord(ex2, "NewItemCreateIOError", ErrorCategory.WriteError, path));
							return;
						}
						throw;
					}
				}
				throw PSTraceSource.NewArgumentException("type", FileSystemProviderStrings.UnknownType, new object[0]);
			}
			string action = null;
			if (itemType == FileSystemProvider.ItemType.SymbolicLink)
			{
				action = FileSystemProviderStrings.NewItemActionSymbolicLink;
			}
			else if (itemType == FileSystemProvider.ItemType.HardLink)
			{
				action = FileSystemProviderStrings.NewItemActionHardLink;
			}
			string target3 = StringUtil.Format(FileSystemProviderStrings.NewItemActionTemplate, path);
			if (!base.ShouldProcess(target3, action))
			{
				return;
			}
			bool flag6 = false;
			string text2 = value.ToString();
			if (string.IsNullOrEmpty(text2))
			{
				throw PSTraceSource.NewArgumentNullException("value");
			}
			bool flag7 = false;
			try
			{
				flag7 = FileSystemProvider.CheckItemExists(text2, out flag6);
			}
			catch (Exception exception5)
			{
				base.WriteError(new ErrorRecord(exception5, "AccessException", ErrorCategory.PermissionDenied, text2));
				return;
			}
			if (!flag7)
			{
				string message4 = StringUtil.Format(FileSystemProviderStrings.ItemNotFound, text2);
				base.WriteError(new ErrorRecord(new ItemNotFoundException(message4), "ItemNotFound", ErrorCategory.ObjectNotFound, text2));
			}
			else
			{
				if (itemType == FileSystemProvider.ItemType.HardLink && flag6)
				{
					string message5 = StringUtil.Format(FileSystemProviderStrings.ItemNotFile, text2);
					base.WriteError(new ErrorRecord(new InvalidOperationException(message5), "ItemNotFile", ErrorCategory.InvalidOperation, text2));
					return;
				}
				bool flag8 = false;
				bool flag9 = false;
				try
				{
					flag9 = FileSystemProvider.CheckItemExists(path, out flag8);
				}
				catch (Exception exception6)
				{
					base.WriteError(new ErrorRecord(exception6, "AccessException", ErrorCategory.PermissionDenied, path));
					return;
				}
				if (base.Force)
				{
					try
					{
						if (!flag8 && flag9)
						{
							File.Delete(path);
						}
						else if (flag8 && flag9)
						{
							Directory.Delete(path);
						}
						goto IL_2F2;
					}
					catch (Exception ex3)
					{
						if (ex3 is FileNotFoundException || ex3 is DirectoryNotFoundException || ex3 is UnauthorizedAccessException || ex3 is SecurityException || ex3 is ArgumentException || ex3 is PathTooLongException || ex3 is NotSupportedException || ex3 is ArgumentNullException || ex3 is IOException)
						{
							base.WriteError(new ErrorRecord(ex3, "NewItemDeleteIOError", ErrorCategory.WriteError, path));
							goto IL_2F2;
						}
						throw;
					}
				}
				if (flag9)
				{
					base.WriteError(new ErrorRecord(new IOException("NewItemIOError"), "NewItemIOError", ErrorCategory.ResourceExists, path));
					return;
				}
				IL_2F2:
				bool flag10 = false;
				if (itemType == FileSystemProvider.ItemType.SymbolicLink)
				{
					int num = FileSystemProvider.NativeMethods.CreateSymbolicLink(path, text2, flag6 ? 1 : 0);
					flag10 = (num == 1);
				}
				else if (itemType == FileSystemProvider.ItemType.HardLink)
				{
					flag10 = FileSystemProvider.NativeMethods.CreateHardLink(path, text2, IntPtr.Zero);
				}
				if (!flag10)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					Win32Exception ex4 = new Win32Exception(lastWin32Error);
					if (lastWin32Error == 1314)
					{
						string elevationRequired = FileSystemProviderStrings.ElevationRequired;
						base.WriteError(new ErrorRecord(new UnauthorizedAccessException(elevationRequired, ex4), "NewItemSymbolicLinkElevationRequired", ErrorCategory.PermissionDenied, value.ToString()));
						return;
					}
					if (lastWin32Error == 1)
					{
						string message6;
						if (itemType == FileSystemProvider.ItemType.SymbolicLink)
						{
							message6 = FileSystemProviderStrings.SymbolicLinkNotSupported;
						}
						else
						{
							message6 = FileSystemProviderStrings.HardLinkNotSupported;
						}
						base.WriteError(new ErrorRecord(new InvalidOperationException(message6, ex4), "NewItemInvalidOperation", ErrorCategory.InvalidOperation, value.ToString()));
						return;
					}
					throw ex4;
				}
				else
				{
					if (flag6)
					{
						DirectoryInfo item2 = new DirectoryInfo(path);
						base.WriteItemObject(item2, path, true);
						return;
					}
					FileInfo item3 = new FileInfo(path);
					base.WriteItemObject(item3, path, false);
					return;
				}
			}
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x0018F660 File Offset: 0x0018D860
		private static bool CheckItemExists(string strTargetPath, out bool isDirectory)
		{
			Exception ex;
			bool result = Utils.NativeItemExists(strTargetPath, out isDirectory, out ex);
			if (ex != null)
			{
				throw ex;
			}
			return result;
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x0018F680 File Offset: 0x0018D880
		private static FileSystemProvider.ItemType GetItemType(string input)
		{
			FileSystemProvider.ItemType result = FileSystemProvider.ItemType.Unknown;
			WildcardPattern wildcardPattern = new WildcardPattern(input + "*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
			if (wildcardPattern.IsMatch("directory") || wildcardPattern.IsMatch("container"))
			{
				result = FileSystemProvider.ItemType.Directory;
			}
			else if (wildcardPattern.IsMatch("file"))
			{
				result = FileSystemProvider.ItemType.File;
			}
			else if (wildcardPattern.IsMatch("symboliclink"))
			{
				result = FileSystemProvider.ItemType.SymbolicLink;
			}
			else if (wildcardPattern.IsMatch("junction"))
			{
				result = FileSystemProvider.ItemType.Junction;
			}
			else if (wildcardPattern.IsMatch("hardlink"))
			{
				result = FileSystemProvider.ItemType.HardLink;
			}
			return result;
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x0018F704 File Offset: 0x0018D904
		private void CreateDirectory(string path, bool streamOutput)
		{
			string parentPath = this.GetParentPath(path, null);
			string childName = this.GetChildName(path);
			ErrorRecord errorRecord = null;
			if (!base.Force && this.ItemExists(path, out errorRecord))
			{
				string message = StringUtil.Format(FileSystemProviderStrings.DirectoryExist, path);
				Exception exception = new IOException(message);
				base.WriteError(new ErrorRecord(exception, "DirectoryExist", ErrorCategory.ResourceExists, path));
				return;
			}
			if (errorRecord != null)
			{
				base.WriteError(errorRecord);
				return;
			}
			try
			{
				string newItemActionDirectory = FileSystemProviderStrings.NewItemActionDirectory;
				string target = StringUtil.Format(FileSystemProviderStrings.NewItemActionTemplate, path);
				if (base.ShouldProcess(target, newItemActionDirectory))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(parentPath);
					DirectoryInfo item = directoryInfo.CreateSubdirectory(childName);
					if (streamOutput)
					{
						base.WriteItemObject(item, path, true);
					}
				}
			}
			catch (ArgumentException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "CreateDirectoryArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception3)
			{
				if (!base.Force)
				{
					base.WriteError(new ErrorRecord(exception3, "CreateDirectoryIOError", ErrorCategory.WriteError, path));
				}
			}
			catch (UnauthorizedAccessException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "CreateDirectoryUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x0018F834 File Offset: 0x0018DA34
		private bool CreateIntermediateDirectories(string path)
		{
			bool flag = false;
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			try
			{
				Stack<string> stack = new Stack<string>();
				string text = path;
				do
				{
					string root = string.Empty;
					if (base.PSDriveInfo != null)
					{
						root = base.PSDriveInfo.Root;
					}
					string parentPath = this.GetParentPath(path, root);
					if (string.IsNullOrEmpty(parentPath) || string.Compare(parentPath, text, StringComparison.OrdinalIgnoreCase) == 0 || this.ItemExists(parentPath))
					{
						break;
					}
					stack.Push(parentPath);
					text = parentPath;
				}
				while (!string.IsNullOrEmpty(text));
				foreach (string path2 in stack)
				{
					this.CreateDirectory(path2, false);
				}
				flag = true;
			}
			catch (ArgumentException exception)
			{
				base.WriteError(new ErrorRecord(exception, "CreateIntermediateDirectoriesArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "CreateIntermediateDirectoriesIOError", ErrorCategory.WriteError, path));
			}
			catch (UnauthorizedAccessException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "CreateIntermediateDirectoriesUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x0018F994 File Offset: 0x0018DB94
		protected override void RemoveItem(string path, bool recurse)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			try
			{
				path = FileSystemProvider.NormalizePath(path);
				bool flag = false;
				FileSystemProviderRemoveItemDynamicParameters fileSystemProviderRemoveItemDynamicParameters = null;
				if (base.DynamicParameters != null)
				{
					fileSystemProviderRemoveItemDynamicParameters = (base.DynamicParameters as FileSystemProviderRemoveItemDynamicParameters);
					if (fileSystemProviderRemoveItemDynamicParameters != null)
					{
						if (fileSystemProviderRemoveItemDynamicParameters.Stream != null && fileSystemProviderRemoveItemDynamicParameters.Stream.Length > 0)
						{
							flag = true;
						}
						else
						{
							int num = path.IndexOf(':');
							int num2 = path.IndexOf(':', num + 1);
							if (num2 > 0)
							{
								string text = path.Substring(num2 + 1);
								path = path.Remove(num2);
								flag = true;
								fileSystemProviderRemoveItemDynamicParameters = new FileSystemProviderRemoveItemDynamicParameters();
								fileSystemProviderRemoveItemDynamicParameters.Stream = new string[]
								{
									text
								};
							}
						}
					}
				}
				bool flag2 = false;
				FileSystemInfo fileSystemInfo = FileSystemProvider.GetFileSystemInfo(path, ref flag2);
				if (fileSystemInfo == null)
				{
					string message = StringUtil.Format(FileSystemProviderStrings.ItemDoesNotExist, path);
					Exception exception = new IOException(message);
					base.WriteError(new ErrorRecord(exception, "ItemDoesNotExist", ErrorCategory.ObjectNotFound, path));
				}
				else if (!flag && flag2)
				{
					this.RemoveDirectoryInfoItem((DirectoryInfo)fileSystemInfo, recurse, base.Force, true);
				}
				else if (flag)
				{
					foreach (string text2 in fileSystemProviderRemoveItemDynamicParameters.Stream)
					{
						WildcardPattern wildcardPattern = new WildcardPattern(text2, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
						bool flag3 = false;
						foreach (AlternateStreamData alternateStreamData in AlternateDataStreamUtilities.GetStreams(fileSystemInfo.FullName))
						{
							if (wildcardPattern.IsMatch(alternateStreamData.Stream))
							{
								flag3 = true;
								string target = string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.StreamAction, new object[]
								{
									alternateStreamData.Stream,
									fileSystemInfo.FullName
								});
								if (base.ShouldProcess(target))
								{
									AlternateDataStreamUtilities.DeleteFileStream(fileSystemInfo.FullName, alternateStreamData.Stream);
								}
							}
						}
						if (!WildcardPattern.ContainsWildcardCharacters(text2) && !flag3)
						{
							string message2 = StringUtil.Format(FileSystemProviderStrings.AlternateDataStreamNotFound, text2, fileSystemInfo.FullName);
							Exception exception2 = new FileNotFoundException(message2, fileSystemInfo.FullName);
							base.WriteError(new ErrorRecord(exception2, "AlternateDataStreamNotFound", ErrorCategory.ObjectNotFound, path));
						}
					}
				}
				else
				{
					this.RemoveFileInfoItem((FileInfo)fileSystemInfo, base.Force);
				}
			}
			catch (IOException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "RemoveItemIOError", ErrorCategory.WriteError, path));
			}
			catch (UnauthorizedAccessException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "RemoveItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x0018FC54 File Offset: 0x0018DE54
		protected override object RemoveItemDynamicParameters(string path, bool recurse)
		{
			if (!recurse)
			{
				return new FileSystemProviderRemoveItemDynamicParameters();
			}
			return null;
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x0018FC60 File Offset: 0x0018DE60
		private void RemoveDirectoryInfoItem(DirectoryInfo directory, bool recurse, bool force, bool rootOfRemoval)
		{
			bool flag = true;
			if (rootOfRemoval || recurse)
			{
				string removeItemActionDirectory = FileSystemProviderStrings.RemoveItemActionDirectory;
				flag = base.ShouldProcess(directory.FullName, removeItemActionDirectory);
			}
			if ((directory.Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0 && !base.Force)
			{
				string message = StringUtil.Format(FileSystemProviderStrings.DirectoryReparsePoint, directory.FullName);
				Exception exception = new IOException(message);
				base.WriteError(new ErrorRecord(exception, "DirectoryNotEmpty", ErrorCategory.WriteError, directory));
				return;
			}
			if ((directory.Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
			{
				if (!InternalSymbolicLinkLinkCodeMethods.DeleteJunction(directory.FullName))
				{
					string message2 = StringUtil.Format(FileSystemProviderStrings.CannotRemoveItem, directory.FullName);
					Exception exception2 = new IOException(message2);
					base.WriteError(new ErrorRecord(exception2, "DeleteJunctionFailed", ErrorCategory.WriteError, directory));
					return;
				}
				bool flag2;
				Exception ex;
				if (!Utils.NativeItemExists(directory.FullName, out flag2, out ex))
				{
					return;
				}
				if (ex != null)
				{
					ErrorRecord errorRecord = new ErrorRecord(ex, "RemoveFileSystemItemUnAuthorizedAccess", ErrorCategory.PermissionDenied, directory);
					ErrorDetails errorDetails = new ErrorDetails(this, "FileSystemProviderStrings", "CannotRemoveItem", new object[]
					{
						directory.FullName,
						ex.Message
					});
					errorRecord.ErrorDetails = errorDetails;
					base.WriteError(errorRecord);
					return;
				}
			}
			if (flag)
			{
				foreach (DirectoryInfo directoryInfo in directory.EnumerateDirectories())
				{
					if (base.Stopping)
					{
						return;
					}
					if (directoryInfo != null)
					{
						this.RemoveDirectoryInfoItem(directoryInfo, recurse, force, false);
					}
				}
				IEnumerable<FileInfo> enumerable;
				if (!string.IsNullOrEmpty(base.Filter))
				{
					enumerable = directory.EnumerateFiles(base.Filter);
				}
				else
				{
					enumerable = directory.EnumerateFiles();
				}
				foreach (FileInfo fileInfo in enumerable)
				{
					if (base.Stopping)
					{
						return;
					}
					if (fileInfo != null)
					{
						if (recurse)
						{
							this.RemoveFileInfoItem(fileInfo, force);
						}
						else
						{
							this.RemoveFileSystemItem(fileInfo, force);
						}
					}
				}
				bool flag3 = FileSystemProvider.DirectoryInfoHasChildItems(directory);
				if (flag3 && !force)
				{
					string message3 = StringUtil.Format(FileSystemProviderStrings.DirectoryNotEmpty, directory.FullName);
					Exception exception3 = new IOException(message3);
					base.WriteError(new ErrorRecord(exception3, "DirectoryNotEmpty", ErrorCategory.WriteError, directory));
					return;
				}
				this.RemoveFileSystemItem(directory, force);
			}
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x0018FEBC File Offset: 0x0018E0BC
		private void RemoveFileInfoItem(FileInfo file, bool force)
		{
			string removeItemActionFile = FileSystemProviderStrings.RemoveItemActionFile;
			if (base.ShouldProcess(file.FullName, removeItemActionFile))
			{
				this.RemoveFileSystemItem(file, force);
			}
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x0018FEE8 File Offset: 0x0018E0E8
		private void RemoveFileSystemItem(FileSystemInfo fileSystemInfo, bool force)
		{
			if (!base.Force && (fileSystemInfo.Attributes & (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System)) != (FileAttributes)0)
			{
				string message = StringUtil.Format(FileSystemProviderStrings.PermissionError, new object[0]);
				Exception ex = new IOException(message);
				ErrorDetails errorDetails = new ErrorDetails(this, "FileSystemProviderStrings", "CannotRemoveItem", new object[]
				{
					fileSystemInfo.FullName,
					ex.Message
				});
				base.WriteError(new ErrorRecord(ex, "RemoveFileSystemItemUnAuthorizedAccess", ErrorCategory.PermissionDenied, fileSystemInfo)
				{
					ErrorDetails = errorDetails
				});
				return;
			}
			FileAttributes attributes = fileSystemInfo.Attributes;
			bool flag = false;
			try
			{
				if (force)
				{
					fileSystemInfo.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System);
					flag = true;
				}
				fileSystemInfo.Delete();
				if (force)
				{
					flag = false;
				}
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				ErrorDetails errorDetails2 = new ErrorDetails(this, "FileSystemProviderStrings", "CannotRemoveItem", new object[]
				{
					fileSystemInfo.FullName,
					ex2.Message
				});
				if (ex2 is SecurityException || ex2 is UnauthorizedAccessException)
				{
					base.WriteError(new ErrorRecord(ex2, "RemoveFileSystemItemUnAuthorizedAccess", ErrorCategory.PermissionDenied, fileSystemInfo)
					{
						ErrorDetails = errorDetails2
					});
				}
				else if (ex2 is ArgumentException)
				{
					base.WriteError(new ErrorRecord(ex2, "RemoveFileSystemItemArgumentError", ErrorCategory.InvalidArgument, fileSystemInfo)
					{
						ErrorDetails = errorDetails2
					});
				}
				else
				{
					if (!(ex2 is IOException) && !(ex2 is FileNotFoundException) && !(ex2 is DirectoryNotFoundException))
					{
						throw;
					}
					base.WriteError(new ErrorRecord(ex2, "RemoveFileSystemItemIOError", ErrorCategory.WriteError, fileSystemInfo)
					{
						ErrorDetails = errorDetails2
					});
				}
			}
			finally
			{
				if (flag)
				{
					try
					{
						if (fileSystemInfo.Exists)
						{
							fileSystemInfo.Attributes = attributes;
						}
					}
					catch (Exception ex3)
					{
						CommandProcessorBase.CheckForSevereException(ex3);
						if (!(ex3 is DirectoryNotFoundException) && !(ex3 is SecurityException) && !(ex3 is ArgumentException) && !(ex3 is FileNotFoundException) && !(ex3 is IOException))
						{
							throw;
						}
						ErrorDetails errorDetails3 = new ErrorDetails(this, "FileSystemProviderStrings", "CannotRestoreAttributes", new object[]
						{
							fileSystemInfo.FullName,
							ex3.Message
						});
						base.WriteError(new ErrorRecord(ex3, "RemoveFileSystemItemCannotRestoreAttributes", ErrorCategory.PermissionDenied, fileSystemInfo)
						{
							ErrorDetails = errorDetails3
						});
					}
				}
			}
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x00190180 File Offset: 0x0018E380
		protected override bool ItemExists(string path)
		{
			ErrorRecord errorRecord = null;
			bool result = this.ItemExists(path, out errorRecord);
			if (errorRecord != null)
			{
				base.WriteError(errorRecord);
			}
			return result;
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x001901A4 File Offset: 0x0018E3A4
		private bool ItemExists(string path, out ErrorRecord error)
		{
			error = null;
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			bool flag = false;
			path = FileSystemProvider.NormalizePath(path);
			try
			{
				bool flag2;
				Exception ex;
				if (Utils.NativeItemExists(path, out flag2, out ex))
				{
					flag = true;
				}
				if (ex != null)
				{
					throw ex;
				}
				FileSystemItemProviderDynamicParameters fileSystemItemProviderDynamicParameters = base.DynamicParameters as FileSystemItemProviderDynamicParameters;
				if (flag && fileSystemItemProviderDynamicParameters != null)
				{
					DateTime lastWriteTime = File.GetLastWriteTime(path);
					if (fileSystemItemProviderDynamicParameters.OlderThan != null)
					{
						flag = (lastWriteTime < fileSystemItemProviderDynamicParameters.OlderThan.Value);
					}
					if (fileSystemItemProviderDynamicParameters.NewerThan != null)
					{
						flag = (lastWriteTime > fileSystemItemProviderDynamicParameters.NewerThan.Value);
					}
				}
			}
			catch (SecurityException exception)
			{
				error = new ErrorRecord(exception, "ItemExistsSecurityError", ErrorCategory.PermissionDenied, path);
			}
			catch (ArgumentException exception2)
			{
				error = new ErrorRecord(exception2, "ItemExistsArgumentError", ErrorCategory.InvalidArgument, path);
			}
			catch (UnauthorizedAccessException exception3)
			{
				error = new ErrorRecord(exception3, "ItemExistsUnauthorizedAccessError", ErrorCategory.PermissionDenied, path);
			}
			catch (PathTooLongException exception4)
			{
				error = new ErrorRecord(exception4, "ItemExistsPathTooLongError", ErrorCategory.InvalidArgument, path);
			}
			catch (NotSupportedException exception5)
			{
				error = new ErrorRecord(exception5, "ItemExistsNotSupportedError", ErrorCategory.InvalidOperation, path);
			}
			return flag;
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x001902F8 File Offset: 0x0018E4F8
		protected override object ItemExistsDynamicParameters(string path)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = new FileSystemItemProviderDynamicParameters();
			}
			return result;
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x00190330 File Offset: 0x0018E530
		protected override bool HasChildItems(string path)
		{
			bool result = false;
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			try
			{
				DirectoryInfo directory = new DirectoryInfo(path);
				result = FileSystemProvider.DirectoryInfoHasChildItems(directory);
			}
			catch (ArgumentNullException)
			{
				result = false;
			}
			catch (ArgumentException)
			{
				result = false;
			}
			catch (UnauthorizedAccessException)
			{
				result = false;
			}
			catch (IOException)
			{
				result = false;
			}
			catch (NotSupportedException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x001903C4 File Offset: 0x0018E5C4
		private static bool DirectoryInfoHasChildItems(DirectoryInfo directory)
		{
			bool flag = false;
			IEnumerable<FileSystemInfo> source = directory.EnumerateFileSystemInfos();
			if (source.Any<FileSystemInfo>())
			{
				flag = true;
			}
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00190408 File Offset: 0x0018E608
		protected override void CopyItem(string path, string destinationPath, bool recurse)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (string.IsNullOrEmpty(destinationPath))
			{
				throw PSTraceSource.NewArgumentException("destinationPath");
			}
			path = FileSystemProvider.NormalizePath(path);
			destinationPath = FileSystemProvider.NormalizePath(destinationPath);
			PSSession pssession = null;
			PSSession pssession2 = null;
			CopyItemDynamicParameters copyItemDynamicParameters = base.DynamicParameters as CopyItemDynamicParameters;
			if (copyItemDynamicParameters != null)
			{
				if (copyItemDynamicParameters.FromSession != null)
				{
					pssession = copyItemDynamicParameters.FromSession;
				}
				else
				{
					pssession2 = copyItemDynamicParameters.ToSession;
				}
			}
			if (pssession2 == null && pssession == null && path.Equals(destinationPath, StringComparison.OrdinalIgnoreCase))
			{
				string message = StringUtil.Format(FileSystemProviderStrings.CopyError, path);
				Exception exception = new IOException(message);
				base.WriteError(new ErrorRecord(exception, "CopyError", ErrorCategory.WriteError, path));
				return;
			}
			if (pssession != null)
			{
				this.CopyItemFromRemoteSession(path, destinationPath, recurse, base.Force, pssession);
				return;
			}
			if (pssession2 != null)
			{
				using (PowerShell powerShell = PowerShell.Create())
				{
					powerShell.Runspace = pssession2.Runspace;
					this.CopyItemLocalOrToSession(path, destinationPath, recurse, base.Force, powerShell);
					return;
				}
			}
			this.CopyItemLocalOrToSession(path, destinationPath, recurse, base.Force, null);
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x0019052C File Offset: 0x0018E72C
		private void CopyItemFromRemoteSession(string path, string destinationPath, bool recurse, bool force, PSSession fromSession)
		{
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.Runspace = fromSession.Runspace;
				string script = "\r\n                    ## A hashtable is returned in the following format: \r\n                    ##  Exists - Boolean to keep track if the given path exists.\r\n                    ##  Items  - The items that Get-Item -Path $path resolves to.\r\n                    param ([string] $path)\r\n\r\n                    $op = @{\r\n                        Exists = $null\r\n                        Items = $null\r\n                    }\r\n\r\n                    try\r\n                    {\r\n                        if (-not (Test-Path $path))\r\n                        {\r\n                            $op['Exists'] = $false\r\n                            return $op\r\n                        }\r\n\r\n                        $items = @(Get-Item -Path $path | ForEach-Object {@{FullName = $_.FullName; Name = $_.Name; FileSize = $_.Length; IsDirectory = $_ -is [System.IO.DirectoryInfo]}})\r\n                        $op['Exists'] = $true\r\n                        $op['Items'] = $items\r\n\r\n                        return $op\r\n                    }\r\n                    catch\r\n                    {\r\n                        if ($_.Exception.InnerException)\r\n                        {\r\n                            Write-Error -Exception $_.Exception.InnerException\r\n                        }\r\n                        else\r\n                        {\r\n                            Write-Error -Exception $_.Exception\r\n                        }\r\n                    }  \r\n\r\n                    return $op\r\n                ";
				powerShell.AddScript(script);
				powerShell.AddParameter("path", path);
				Hashtable hashtable = SafeInvokeCommand.Invoke(powerShell, this, null);
				if (hashtable == null)
				{
					Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyFailedToReadFile, new object[]
					{
						path
					}));
					base.WriteError(new ErrorRecord(exception, "CopyItemRemotelyFailedToReadFile", ErrorCategory.WriteError, path));
				}
				else
				{
					if (!(bool)hashtable["Exists"])
					{
						throw PSTraceSource.NewArgumentNullException(SessionStateStrings.PathNotFound, path, new object[0]);
					}
					if (hashtable["Items"] != null)
					{
						bool flag = Utils.NativeFileExists(destinationPath);
						this.InitilizeFunctionPSCopyFileFromRemoteSession(powerShell);
						PSObject psobject = (PSObject)hashtable["Items"];
						ArrayList arrayList = (ArrayList)psobject.BaseObject;
						foreach (object obj in arrayList)
						{
							PSObject psobject2 = (PSObject)obj;
							Hashtable hashtable2 = (Hashtable)psobject2.BaseObject;
							string text = (string)hashtable2["Name"];
							string text2 = (string)hashtable2["FullName"];
							bool flag2 = (bool)hashtable2["IsDirectory"];
							if (flag2)
							{
								if (flag)
								{
									Exception exception2 = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyDestinationIsFile, new object[]
									{
										path,
										destinationPath
									}));
									base.WriteError(new ErrorRecord(exception2, "CopyError", ErrorCategory.WriteError, destinationPath));
									return;
								}
								DirectoryInfo sourceDirectory = new DirectoryInfo(text2);
								this.CopyDirectoryFromRemoteSession(sourceDirectory, destinationPath, force, recurse, powerShell);
							}
							else
							{
								long fileSize = (long)hashtable2["FileSize"];
								FileInfo sourceFile = new FileInfo(text2);
								this.CopyFileFromRemoteSession(sourceFile, destinationPath, force, powerShell, fileSize);
							}
						}
						this.RemoveFunctionsPSCopyFileFromRemoteSession(powerShell);
					}
				}
			}
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00190770 File Offset: 0x0018E970
		private void CopyItemLocalOrToSession(string path, string destinationPath, bool recurse, bool Force, PowerShell ps)
		{
			bool flag = this.IsItemContainer(path);
			if (flag)
			{
				DirectoryInfo directory = new DirectoryInfo(path);
				this.CopyDirectoryInfoItem(directory, destinationPath, recurse, Force, ps);
				return;
			}
			FileInfo file = new FileInfo(path);
			this.CopyFileInfoItem(file, destinationPath, Force, ps);
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x001907B0 File Offset: 0x0018E9B0
		private void CopyDirectoryInfoItem(DirectoryInfo directory, string destination, bool recurse, bool force, PowerShell ps)
		{
			if (ps == null)
			{
				if (this.IsItemContainer(destination))
				{
					destination = this.MakePath(destination, directory.Name);
				}
			}
			else if (this.RemoteDirectoryExist(ps, destination))
			{
				destination = Path.Combine(destination, directory.Name);
			}
			FileSystemProvider.tracer.WriteLine("destination = {0}", new object[]
			{
				destination
			});
			string copyItemActionDirectory = FileSystemProviderStrings.CopyItemActionDirectory;
			string target = StringUtil.Format(FileSystemProviderStrings.CopyItemResourceFileTemplate, directory.FullName, destination);
			if (base.ShouldProcess(target, copyItemActionDirectory))
			{
				if (ps == null)
				{
					this.CreateDirectory(destination, true);
				}
				else
				{
					if (this.RemoteDestinationPathIsFile(destination, ps))
					{
						Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemoteDestinationIsFile, new object[]
						{
							destination
						}));
						base.WriteError(new ErrorRecord(exception, "CopyError", ErrorCategory.WriteError, destination));
						return;
					}
					destination = this.CreateDirectoryOnRemoteSession(destination, force, ps);
					if (destination == null)
					{
						return;
					}
				}
				if (recurse)
				{
					IEnumerable<FileInfo> enumerable;
					if (string.IsNullOrEmpty(base.Filter))
					{
						enumerable = directory.EnumerateFiles();
					}
					else
					{
						enumerable = directory.EnumerateFiles(base.Filter);
					}
					foreach (FileInfo fileInfo in enumerable)
					{
						if (base.Stopping)
						{
							return;
						}
						if (fileInfo != null)
						{
							try
							{
								this.CopyFileInfoItem(fileInfo, destination, force, ps);
							}
							catch (ArgumentException exception2)
							{
								base.WriteError(new ErrorRecord(exception2, "CopyDirectoryInfoItemArgumentError", ErrorCategory.InvalidArgument, fileInfo));
							}
							catch (IOException exception3)
							{
								base.WriteError(new ErrorRecord(exception3, "CopyDirectoryInfoItemIOError", ErrorCategory.WriteError, fileInfo));
							}
							catch (UnauthorizedAccessException exception4)
							{
								base.WriteError(new ErrorRecord(exception4, "CopyDirectoryInfoItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, fileInfo));
							}
						}
					}
					foreach (DirectoryInfo directoryInfo in directory.EnumerateDirectories())
					{
						if (base.Stopping)
						{
							break;
						}
						if (directoryInfo != null)
						{
							try
							{
								this.CopyDirectoryInfoItem(directoryInfo, destination, recurse, force, ps);
							}
							catch (ArgumentException exception5)
							{
								base.WriteError(new ErrorRecord(exception5, "CopyDirectoryInfoItemArgumentError", ErrorCategory.InvalidArgument, directoryInfo));
							}
							catch (IOException exception6)
							{
								base.WriteError(new ErrorRecord(exception6, "CopyDirectoryInfoItemIOError", ErrorCategory.WriteError, directoryInfo));
							}
							catch (UnauthorizedAccessException exception7)
							{
								base.WriteError(new ErrorRecord(exception7, "CopyDirectoryInfoItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, directoryInfo));
							}
						}
					}
				}
			}
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x00190A5C File Offset: 0x0018EC5C
		private void CopyFileInfoItem(FileInfo file, string destinationPath, bool force, PowerShell ps)
		{
			if (ps == null)
			{
				if (this.IsItemContainer(destinationPath))
				{
					destinationPath = this.MakePath(destinationPath, file.Name);
				}
				if (destinationPath.Equals(file.FullName, StringComparison.OrdinalIgnoreCase))
				{
					string message = StringUtil.Format(FileSystemProviderStrings.CopyError, destinationPath);
					Exception exception = new IOException(message);
					base.WriteError(new ErrorRecord(exception, "CopyError", ErrorCategory.WriteError, destinationPath));
					return;
				}
				if (Utils.IsReservedDeviceName(destinationPath))
				{
					string message2 = StringUtil.Format(FileSystemProviderStrings.TargetCannotContainDeviceName, destinationPath);
					Exception exception2 = new IOException(message2);
					base.WriteError(new ErrorRecord(exception2, "CopyError", ErrorCategory.WriteError, destinationPath));
					return;
				}
			}
			string copyItemActionFile = FileSystemProviderStrings.CopyItemActionFile;
			string target = StringUtil.Format(FileSystemProviderStrings.CopyItemResourceFileTemplate, file.FullName, destinationPath);
			if (base.ShouldProcess(target, copyItemActionFile))
			{
				try
				{
					if (ps == null)
					{
						file.CopyTo(destinationPath, true);
						FileInfo item = new FileInfo(destinationPath);
						base.WriteItemObject(item, destinationPath, false);
					}
					else
					{
						this.PerformCopyFileToRemoteSession(file, destinationPath, ps);
					}
				}
				catch (UnauthorizedAccessException exception3)
				{
					if (force)
					{
						try
						{
							if (ps == null)
							{
								FileInfo fileInfo = new FileInfo(destinationPath);
								fileInfo.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden);
							}
							else
							{
								this.PerformCopyFileToRemoteSession(file, destinationPath, ps);
							}
						}
						catch (Exception ex)
						{
							if (!(ex is FileNotFoundException) && !(ex is DirectoryNotFoundException) && !(ex is SecurityException) && !(ex is ArgumentException) && !(ex is IOException))
							{
								throw;
							}
							base.WriteError(new ErrorRecord(exception3, "CopyFileInfoItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, file));
						}
						file.CopyTo(destinationPath, true);
						FileInfo item2 = new FileInfo(destinationPath);
						base.WriteItemObject(item2, destinationPath, false);
					}
					else
					{
						base.WriteError(new ErrorRecord(exception3, "CopyFileInfoItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, file));
					}
				}
			}
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x00190C14 File Offset: 0x0018EE14
		private void CopyDirectoryFromRemoteSession(DirectoryInfo sourceDirectory, string destination, bool force, bool recurse, PowerShell ps)
		{
			if (this.IsItemContainer(destination))
			{
				destination = this.MakePath(destination, sourceDirectory.Name);
			}
			FileSystemProvider.tracer.WriteLine("destination = {0}", new object[]
			{
				destination
			});
			string copyItemActionDirectory = FileSystemProviderStrings.CopyItemActionDirectory;
			string target = StringUtil.Format(FileSystemProviderStrings.CopyItemResourceFileTemplate, sourceDirectory.FullName, destination);
			if (base.ShouldProcess(target, copyItemActionDirectory))
			{
				if (!Utils.NativeDirectoryExists(destination))
				{
					this.CreateDirectory(destination, false);
					if (!Utils.NativeDirectoryExists(destination))
					{
						return;
					}
				}
				if (recurse)
				{
					string script = "\r\n                        # Return a hashtable with the following members:\r\n                        # Files - Array with file fullnames, and their sizes\r\n                        # Directories - Array of child directory fullnames\r\n\r\n                        param ( [string] $path)\r\n\r\n                        $result = @()\r\n\r\n                        $op = @{\r\n                            Files = $null\r\n                            Directories = $null\r\n                        }\r\n\r\n                        try\r\n                        {\r\n                            $item = Get-Item $path\r\n\r\n                            if ($item -isnot [System.IO.DirectoryInfo])\r\n                            {\r\n                                return $op\r\n                            }\r\n\r\n                            $files = @(Get-ChildItem -Path $path -File | ForEach-Object {@{FilePath = $_.FullName; FileSize = $_.Length}})\r\n\r\n                            $directories = @(Get-ChildItem -Path $path -Directory | ForEach-Object {$_.FullName})\r\n\r\n                            if ($files.count -gt 0)\r\n                            {\r\n                                $op['Files'] = $files\r\n                            }\r\n\r\n                            if ($directories.count -gt 0)\r\n                            {\r\n                                $op['Directories'] = $directories\r\n                            }\r\n                        }\r\n                        catch\r\n                        {\r\n                            if ($_.Exception.InnerException)\r\n                            {\r\n                                Write-Error -Exception $_.Exception.InnerException\r\n                            }\r\n                            else\r\n                            {\r\n                                Write-Error -Exception $_.Exception\r\n                            }\r\n                        }\r\n\r\n                        return $op\r\n                    ";
					ps.AddScript(script);
					ps.AddParameter("path", sourceDirectory.FullName);
					Hashtable hashtable = SafeInvokeCommand.Invoke(ps, this, null);
					if (hashtable == null)
					{
						Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyFailedToGetDirectoryChildItems, new object[]
						{
							sourceDirectory.FullName
						}));
						base.WriteError(new ErrorRecord(exception, "CopyError", ErrorCategory.WriteError, sourceDirectory.FullName));
						return;
					}
					if (hashtable["Files"] != null)
					{
						PSObject psobject = (PSObject)hashtable["Files"];
						ArrayList arrayList = (ArrayList)psobject.BaseObject;
						foreach (object obj in arrayList)
						{
							PSObject psobject2 = (PSObject)obj;
							Hashtable hashtable2 = (Hashtable)psobject2.BaseObject;
							string fileName = (string)hashtable2["FilePath"];
							long fileSize = (long)hashtable2["FileSize"];
							FileInfo sourceFile = new FileInfo(fileName);
							if (base.Stopping)
							{
								return;
							}
							this.CopyFileFromRemoteSession(sourceFile, destination, force, ps, fileSize);
						}
					}
					if (hashtable["Directories"] != null)
					{
						PSObject psobject3 = (PSObject)hashtable["Directories"];
						ArrayList arrayList2 = (ArrayList)psobject3.BaseObject;
						foreach (object obj2 in arrayList2)
						{
							string path = (string)obj2;
							if (base.Stopping)
							{
								break;
							}
							DirectoryInfo directoryInfo = new DirectoryInfo(path);
							this.CopyDirectoryFromRemoteSession(directoryInfo, Path.Combine(destination, directoryInfo.Name), force, recurse, ps);
						}
					}
				}
			}
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x00190E84 File Offset: 0x0018F084
		private void RemoveFunctionsPSCopyFileFromRemoteSession(PowerShell ps)
		{
			string script = "\r\n                Remove-Item function:PSCopyFileFromRemoteSession -ea SilentlyContinue -Force\r\n                Remove-Item function:PSSourceSupportsAlternateStreams -ea SilentlyContinue -Force\r\n                Remove-Item function:PSGetFileMetadata -ea SilentlyContinue -Force\r\n            ";
			ps.AddScript(script);
			SafeInvokeCommand.Invoke(ps, this, null, false);
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x00190EAC File Offset: 0x0018F0AC
		private ArrayList GetRemoteSourceAlternateStreams(PowerShell ps, string path)
		{
			ArrayList result = null;
			bool flag = false;
			ps.AddCommand("PSSourceSupportsAlternateStreams");
			ps.AddParameter("path", path);
			Hashtable hashtable = SafeInvokeCommand.Invoke(ps, this, null);
			if (hashtable != null && hashtable["SourceSupportsAlternateStreams"] != null)
			{
				flag = (bool)hashtable["SourceSupportsAlternateStreams"];
			}
			if (flag)
			{
				PSObject psobject = (PSObject)hashtable["Streams"];
				result = (ArrayList)psobject.BaseObject;
			}
			return result;
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x00190F24 File Offset: 0x0018F124
		private void InitilizeFunctionPSCopyFileFromRemoteSession(PowerShell ps)
		{
			string script = "\r\n                function PSCopyFileFromRemoteSession\r\n                {\r\n                    # A hash table with the following members is returned:\r\n                    #   - moreAvailable bool to keep track of whether there is more data available\r\n                    #   - b64Fragment to track of the number of bytes.\r\n                    #   - ExceptionThrown bool to keep track if an exception was thrown\r\n                    param([string]$file, [long]$start, [long]$numbytes, [switch]$force = $false, [switch]$isAlternateStream = $false, [string]$streamName)\r\n\r\n                    $finalResult = @{\r\n                        b64Fragment = $null\r\n                        moreAvailable = $null\r\n                        ExceptionThrown = $false\r\n                    }\r\n\r\n                    function PerformCopyFileFromRemoteSession\r\n                    {\r\n                        param([string]$file, [long]$start, [long]$numbytes, [switch]$isAlternateStream, [string]$streamName)\r\n\r\n                        $op = @{\r\n                            b64Fragment = $null\r\n                            moreAvailable = $false\r\n                        }\r\n\r\n                        $rstream = $null\r\n                        try\r\n                        {\r\n                            if ($isAlternateStream)\r\n                            {\r\n                                $content = Get-Content $file -stream $streamName -Encoding Byte -Raw\r\n                                $rstream = [System.IO.MemoryStream]::new($content)\r\n                            }\r\n                            else\r\n                            {\r\n                                $rstream = [System.IO.File]::OpenRead($file)\r\n                            }\r\n\r\n                            # Create a new array to hold the file content\r\n                            if ($start -lt $rstream.Length)\r\n                            {\r\n                                $o = $rstream.Seek($start, 0)\r\n                                $toRead = [Math]::Min($numbytes, $rstream.Length - $start)\r\n                                $fragment = New-Object byte[] $toRead\r\n                                $readsoFar = 0\r\n                                while ($readsoFar -lt $toRead)\r\n                                {\r\n                                    $read = $rstream.Read($fragment, $readSoFar, $toRead - $readsoFar)\r\n                                    $readsoFar += $read\r\n                                }\r\n\r\n                                $op['b64Fragment'] = [System.Convert]::ToBase64String($fragment)\r\n                                if (($start + $readsoFar) -lt $rstream.Length)\r\n                                {\r\n                                    $op['moreAvailable'] = $true\r\n                                }\r\n                            }\r\n                            $op\r\n                        }\r\n\r\n                        finally\r\n                        {\r\n                            if ($rstream -ne $null)\r\n                            {\r\n                                $rstream.Dispose()\r\n                            }\r\n                        }\r\n                    }\r\n\r\n                    function WriteException\r\n                    {\r\n                        param ($ex)\r\n                        \r\n                        if ($ex.Exception.InnerException)\r\n                        {\r\n                            Write-Error -Exception $ex.Exception.InnerException\r\n                        }\r\n                        else\r\n                        {\r\n                            Write-Error -Exception $ex.Exception\r\n                        }\r\n\r\n                        $finalResult.ExceptionThrown = $true\r\n                    }\r\n\r\n                    $unAuthorizedAccessException = $null\r\n                    $result = $null\r\n\r\n                    $isReadOnly = $false\r\n                    $isHidden = $false\r\n                    try\r\n                    {\r\n                        $result = PerformCopyFileFromRemoteSession -file $file -start $start -numbytes $numbytes -isAlternateStream:$isAlternateStream -streamName $streamName\r\n                        $finalResult.b64Fragment =  $result.b64Fragment\r\n                        $finalResult.moreAvailable =  $result.moreAvailable\r\n                    }\r\n                    catch [System.UnauthorizedAccessException]\r\n                    {\r\n                        $unAuthorizedAccessException = $_\r\n                        if ($force)\r\n                        {\r\n                            $exception = $null\r\n                            try\r\n                            {\r\n                                # Disable the readonly and hidden attributes and try again\r\n                                $item = Get-Item $file  \r\n\r\n                                if ($item.Attributes.HasFlag([System.IO.FileAttributes]::Hidden))\r\n                                {\r\n                                    $isHidden = $true\r\n                                    $item.Attributes = $item.Attributes -band (-bnot ([System.IO.FileAttributes]::Hidden))\r\n                                }\r\n                                \r\n                                if ($item.Attributes.HasFlag([System.IO.FileAttributes]::ReadOnly))\r\n                                {\r\n                                    $isReadOnly = $true\r\n                                    $item.Attributes = $item.Attributes -band (-bnot ([System.IO.FileAttributes]::ReadOnly))\r\n                                }\r\n\r\n                                $result = PerformCopyFileFromRemoteSession -file $file -start $start -numbytes $numbytes \r\n                                $finalResult.b64Fragment =  $result.b64Fragment\r\n                                $finalResult.moreAvailable =  $result.moreAvailable\r\n                            }\r\n                            catch\r\n                            {\r\n                                $e = $_\r\n                                if (($e.Exception.InnerException -is [System.IO.FileNotFoundException]) -or\r\n                                    ($e.Exception.InnerException -is [System.IO.DirectoryNotFoundException]) -or\r\n                                    ($e.Exception.InnerException -is [System.Security.SecurityException] ) -or \r\n                                    ($e.Exception.InnerException -is [System.ArgumentException]) -or \r\n                                    ($e.Exception.InnerException -is [System.IO.IOException]))\r\n                                {\r\n                                    # Write out the original error since we failed to force the copy\r\n                                    WriteException $unAuthorizedAccessException\r\n                                }\r\n                                else\r\n                                {\r\n                                    WriteException $e\r\n                                }\r\n                                $finalResult.ExceptionThrown = $true\r\n                            }\r\n                        }\r\n                        else\r\n                        {\r\n                            $finalResult.ExceptionThrown = $true\r\n                            WriteException $unAuthorizedAccessException\r\n                        }\r\n                    }\r\n                    catch\r\n                    {\r\n                        WriteException $_\r\n                    }\r\n\r\n                    finally\r\n                    {\r\n                        if ($isReadOnly)\r\n                        {\r\n                            $item.Attributes = $item.Attributes -bor [System.IO.FileAttributes]::ReadOnly\r\n                        }\r\n\r\n                        if ($isHidden)\r\n                        {\r\n                            $item.Attributes = $item.Attributes -bor [System.IO.FileAttributes]::Hidden\r\n                        }\r\n                    }\r\n\r\n                    return $finalResult\r\n                }\r\n\r\n                # Returns a hashtable with the following members:\r\n                #    SourceSupportsAlternateStreams - boolean to keep track of whether the source supports Alternate data streams.\r\n                #    Streams - the list of alternate streams \r\n                #\r\n                function PSSourceSupportsAlternateStreams\r\n                {\r\n                    param ([string]$path)\r\n\r\n                    $result = @{\r\n                        SourceSupportsAlternateStreams = $false\r\n                        Streams = @()\r\n                    }\r\n\r\n                    # Check if the source supports 'Get-Content -Stream'. This functionality was introduced in version 3.0.\r\n                    $getContentCmdlet = Get-Command Microsoft.PowerShell.Management\\Get-Content -ErrorAction SilentlyContinue\r\n                    if ($getContentCmdlet.Parameters.Keys -notcontains 'Stream')\r\n                    {\r\n                        return $result\r\n                    }\r\n\r\n                    $result['SourceSupportsAlternateStreams'] = $true\r\n\r\n                    # Check if the file has any alternate data streams.\r\n                    $item = Get-Item -Path $path -Stream * -ea SilentlyContinue\r\n                    if (-not $item)\r\n                    {\r\n                        return $result\r\n                    }\r\n\r\n                    foreach ($streamName in $item.Stream)\r\n                    {\r\n                        if ($streamName -ne ':$DATA')\r\n                        {\r\n                            $result['Streams'] += $streamName\r\n                        }\r\n                    }\r\n\r\n                    return $result\r\n                }\r\n\r\n                # Returns a hash table with metadata info about the file for the given path.\r\n                #\r\n                function PSGetFileMetadata\r\n                {\r\n                    param ($filePath)\r\n\r\n                    if (-not (Test-Path $filePath))\r\n                    {\r\n                        return\r\n                    }\r\n\r\n                    $item = Get-Item $filePath -Force -ea SilentlyContinue\r\n                    if ($item)\r\n                    {\r\n                        $metadata = @{}\r\n\r\n                        # Attributes\r\n                        $attributes = @($item.Attributes.ToString().Split(',').Trim())\r\n                        if ($attributes.Count -gt 0)\r\n                        {\r\n                            $metadata.Add('Attributes', $attributes)\r\n                        }\r\n                        \r\n                        # LastWriteTime\r\n                        $metadata.Add('LastWriteTime', $item.LastWriteTime)\r\n                        $metadata.Add('LastWriteTimeUtc', $item.LastWriteTimeUtc)\r\n\r\n                        return $metadata\r\n                    }\r\n                }\r\n            ";
			ps.AddScript(script);
			SafeInvokeCommand.Invoke(ps, this, null, false);
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x00190F4C File Offset: 0x0018F14C
		private Hashtable GetRemoteFileMetadata(string filePath, PowerShell ps)
		{
			ps.AddCommand("PSGetFileMetadata");
			ps.AddParameter("filePath", filePath);
			return SafeInvokeCommand.Invoke(ps, this, null);
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x00190F7C File Offset: 0x0018F17C
		private void SetFileMetadata(FileInfo sourceFile, FileInfo destinationFile, PowerShell ps)
		{
			Hashtable remoteFileMetadata = this.GetRemoteFileMetadata(sourceFile.FullName, ps);
			if (remoteFileMetadata != null)
			{
				if (remoteFileMetadata["LastWriteTimeUtc"] != null)
				{
					destinationFile.LastWriteTimeUtc = (DateTime)remoteFileMetadata["LastWriteTimeUtc"];
				}
				if (remoteFileMetadata["LastWriteTime"] != null)
				{
					destinationFile.LastWriteTime = (DateTime)remoteFileMetadata["LastWriteTime"];
				}
				if (remoteFileMetadata["Attributes"] != null)
				{
					PSObject psobject = (PSObject)remoteFileMetadata["Attributes"];
					foreach (object obj in ((ArrayList)psobject.BaseObject))
					{
						string a = (string)obj;
						if (string.Equals(a, "ReadOnly", StringComparison.OrdinalIgnoreCase))
						{
							destinationFile.Attributes |= FileAttributes.ReadOnly;
						}
						else if (string.Equals(a, "Hidden", StringComparison.OrdinalIgnoreCase))
						{
							destinationFile.Attributes |= FileAttributes.Hidden;
						}
						else if (string.Equals(a, "Archive", StringComparison.OrdinalIgnoreCase))
						{
							destinationFile.Attributes |= FileAttributes.Archive;
						}
						else if (string.Equals(a, "System", StringComparison.OrdinalIgnoreCase))
						{
							destinationFile.Attributes |= FileAttributes.System;
						}
					}
				}
			}
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x001910D0 File Offset: 0x0018F2D0
		private void CopyFileFromRemoteSession(FileInfo sourceFile, string destinationPath, bool force, PowerShell ps, long fileSize = 0L)
		{
			if (!Utils.NativeDirectoryExists(destinationPath))
			{
				Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemDirectoryNotFound, new object[]
				{
					destinationPath
				}));
				base.WriteError(new ErrorRecord(exception, "DirectoryNotFound", ErrorCategory.WriteError, destinationPath));
				return;
			}
			FileInfo destinationFile = new FileInfo(Path.Combine(destinationPath, sourceFile.Name));
			string copyItemActionFile = FileSystemProviderStrings.CopyItemActionFile;
			string target = StringUtil.Format(FileSystemProviderStrings.CopyItemResourceFileTemplate, sourceFile.FullName, destinationPath);
			if (base.ShouldProcess(target, copyItemActionFile))
			{
				bool flag = this.PerformCopyFileFromRemoteSession(sourceFile, destinationFile, destinationPath, force, ps, fileSize, false, null);
				if (flag)
				{
					ArrayList remoteSourceAlternateStreams = this.GetRemoteSourceAlternateStreams(ps, sourceFile.FullName);
					if (remoteSourceAlternateStreams.Count > 0 && remoteSourceAlternateStreams != null)
					{
						foreach (object obj in remoteSourceAlternateStreams)
						{
							string streamName = (string)obj;
							flag = this.PerformCopyFileFromRemoteSession(sourceFile, destinationFile, destinationPath, force, ps, fileSize, true, streamName);
							if (!flag)
							{
								break;
							}
						}
					}
				}
				if (flag)
				{
					this.SetFileMetadata(sourceFile, destinationFile, ps);
				}
			}
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x001911F8 File Offset: 0x0018F3F8
		private bool PerformCopyFileFromRemoteSession(FileInfo sourceFile, FileInfo destinationFile, string destinationPath, bool force, PowerShell ps, long fileSize, bool isAlternateDataStream, string streamName)
		{
			bool result = false;
			string activity = string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyProgressActivity, new object[]
			{
				sourceFile.FullName,
				destinationFile.FullName
			});
			string statusDescription = string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyStatusDescription, new object[]
			{
				ps.Runspace.ConnectionInfo.ComputerName,
				"localhost"
			});
			ProgressRecord progressRecord = new ProgressRecord(0, activity, statusDescription);
			progressRecord.PercentComplete = 0;
			progressRecord.RecordType = ProgressRecordType.Processing;
			base.WriteProgress(progressRecord);
			FileStream fileStream = null;
			bool flag = false;
			try
			{
				if (!isAlternateDataStream)
				{
					if (force && File.Exists(destinationFile.FullName))
					{
						destinationFile.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System);
					}
					fileStream = new FileStream(destinationFile.FullName, FileMode.Create);
				}
				else
				{
					fileStream = AlternateDataStreamUtilities.CreateFileStream(destinationFile.FullName, streamName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
				}
				long num = 4194304L;
				long num2 = 0L;
				long num3 = 0L;
				for (;;)
				{
					ps.AddCommand("PSCopyFileFromRemoteSession");
					ps.AddParameter("file", sourceFile.FullName);
					ps.AddParameter("start", num3);
					ps.AddParameter("numbytes", num);
					if (force)
					{
						ps.AddParameter("force", true);
					}
					if (isAlternateDataStream)
					{
						ps.AddParameter("isAlternateStream", true);
						ps.AddParameter("streamName", streamName);
					}
					Hashtable hashtable = SafeInvokeCommand.Invoke(ps, this, null);
					if (hashtable == null)
					{
						break;
					}
					if (hashtable["ExceptionThrown"] != null)
					{
						bool flag2 = (bool)hashtable["ExceptionThrown"];
						if (flag2)
						{
							goto Block_11;
						}
					}
					string s = "";
					if (hashtable["b64Fragment"] != null)
					{
						s = (string)hashtable["b64Fragment"];
					}
					bool flag3 = (bool)hashtable["moreAvailable"];
					num3 += num;
					byte[] array = Convert.FromBase64String(s);
					fileStream.Write(array, 0, array.Length);
					num2 += (long)array.Length;
					if (fileStream.Length > 0L)
					{
						int percentComplete = (int)(num2 * 100L / fileStream.Length);
						progressRecord.PercentComplete = percentComplete;
						base.WriteProgress(progressRecord);
					}
					if (!flag3)
					{
						goto Block_14;
					}
				}
				flag = true;
				Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyFailedToReadFile, new object[]
				{
					sourceFile.FullName
				}));
				base.WriteError(new ErrorRecord(exception, "FailedToCopyFileFromRemoteSession", ErrorCategory.WriteError, sourceFile.FullName));
				goto IL_286;
				Block_11:
				flag = true;
				goto IL_286;
				Block_14:
				result = true;
				IL_286:
				progressRecord.PercentComplete = 100;
				progressRecord.RecordType = ProgressRecordType.Completed;
				base.WriteProgress(progressRecord);
			}
			catch (IOException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "CopyItemRemotelyIOError", ErrorCategory.WriteError, sourceFile.FullName));
			}
			catch (ArgumentException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "CopyItemRemotelyArgumentError", ErrorCategory.WriteError, sourceFile.FullName));
			}
			catch (NotSupportedException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "CopyFileInfoRemotelyPathRefersToANonFileDevice", ErrorCategory.InvalidArgument, sourceFile.FullName));
			}
			catch (SecurityException exception5)
			{
				base.WriteError(new ErrorRecord(exception5, "CopyFileInfoRemotelyUnauthorizedAccessError", ErrorCategory.PermissionDenied, sourceFile.FullName));
			}
			catch (UnauthorizedAccessException exception6)
			{
				base.WriteError(new ErrorRecord(exception6, "CopyFileInfoItemRemotelyUnauthorizedAccessError", ErrorCategory.PermissionDenied, sourceFile.FullName));
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
				if (flag && File.Exists(destinationFile.FullName) && !destinationFile.Attributes.HasFlag(FileAttributes.ReadOnly) && !destinationFile.Attributes.HasFlag(FileAttributes.Hidden) && !destinationFile.Attributes.HasFlag(FileAttributes.System))
				{
					this.RemoveFileSystemItem(destinationFile, true);
				}
			}
			return result;
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x00191640 File Offset: 0x0018F840
		private void InitilizeFunctionsPSCopyFileToRemoteSession(PowerShell ps)
		{
			string script = "\r\n            # Return a hashtable with the following members:\r\n            #    BytesWritten - the number of bytes written to a file\r\n            #\r\n            function PSCopyFileToRemoteSession\r\n            {\r\n                param([string]$file, [string]$destination, [string]$b64Fragment, [switch]$create = $false)\r\n                    \r\n                $op = @{\r\n                    BytesWritten = $null\r\n                }\r\n                $path = $destination\r\n                $wstream = $null\r\n\r\n                if (Test-Path $destination -PathType Container)\r\n                {\r\n                    $path = Join-Path $destination $file\r\n                }\r\n\r\n                try\r\n                {\r\n                    ## decode \r\n                    $fragment = [System.Convert]::FromBase64String($b64Fragment)\r\n                    if ($create)\r\n                    {\r\n                        # If the file already exists, try to delete it.\r\n                        if (Test-Path $path)\r\n                        {\r\n                            Remove-Item $path -Force -ea SilentlyContinue\r\n                        }\r\n\r\n                        # Create the new file.\r\n                        [IO.File]::WriteAllBytes($path, $fragment)\r\n                    }\r\n                    else\r\n                    {\r\n                        $wstream = New-Object -TypeName IO.FileStream -ArgumentList $path, ([System.IO.FileMode]::Append)\r\n                        $wstream.Write($fragment,0,$fragment.Length)\r\n                    }\r\n                    $op['BytesWritten'] = $fragment.Length\r\n                }\r\n                catch\r\n                {\r\n                    Write-Error -Exception $_.Exception.InnerException\r\n                }\r\n\r\n                finally\r\n                {\r\n                    if ($wstream -ne $null)\r\n                    {\r\n                        $wstream.Dispose()\r\n                    }\r\n                }\r\n                return $op\r\n            }\r\n\r\n            # Retuns a hashtable with the following members:\r\n            #    BytesWritten - number of bytes written to an alternate file stream\r\n            #\r\n            function PSCopyFileAlternateStreamToRemoteSession\r\n            {\r\n                param ([string]$file, [string]$destination, [string]$b64Fragment, [string]$streamName)\r\n                \r\n                $op = @{ \r\n                    BytesWritten = $null\r\n                }\r\n\r\n                $path = Join-Path $destination $file\r\n\r\n                try\r\n                {\r\n                    ## decode and write the stream\r\n                    $fragment = [System.Convert]::FromBase64String($b64Fragment)\r\n                    Add-Content -Path $path -Value $fragment -Encoding Byte -Stream $streamName -ErrorAction Stop\r\n                    $op['BytesWritten'] = $fragment.Length\r\n                }\r\n                catch\r\n                {\r\n                    Write-Error -Exception $_.Exception\r\n                }\r\n                return $op\r\n            }\r\n\r\n            # Returns a hashtable with the following member:\r\n            #    TargetSupportsAlternateStreams - boolean to keep track of whether the target supports Alternate data streams.\r\n            #\r\n            function PSTargetSupportsAlternateStreams\r\n            {\r\n                param ([string]$path)\r\n\r\n                $result = @{\r\n                    TargetSupportsAlternateStreams = $false\r\n                }\r\n\r\n                $targetDrive = [IO.Path]::GetPathRoot($path)\r\n                if (-not $targetDrive)\r\n                {\r\n                    return $result\r\n                }\r\n\r\n                # Check if the target drive is NTFS\r\n                $driveFormat = 'NTFS'\r\n                foreach ($drive in [System.IO.DriveInfo]::GetDrives())\r\n                {\r\n                    if (($drive.Name -eq $targetDrive) -and ($drive.DriveFormat -eq $driveFormat))\r\n                    {\r\n                        # Now, check if the target supports Add-Command -Stream. This functionality was introduced in version 3.0.\r\n                        $addContentCmdlet = Get-Command Microsoft.PowerShell.Management\\Add-Content -ErrorAction SilentlyContinue\r\n                        if ($addContentCmdlet.Parameters.Keys -contains 'Stream')\r\n                        {\r\n                            $result['TargetSupportsAlternateStreams'] = $true\r\n                            break\r\n                        }\r\n                    }\r\n                }\r\n                return $result\r\n            }\r\n\r\n            # If a copy-item to session operation failed, try to delete the created file.\r\n            #\r\n            function PSRemoveRemoteItem\r\n            {\r\n                param ([string]$path)\r\n\r\n                if (Test-Path $path)\r\n                {\r\n                    Remove-Item -Path $path -Force -ea SilentlyContinue\r\n                }\r\n            }\r\n\r\n            # Sets the metadata for the given file.\r\n            #\r\n            function PSSetFileMetadata\r\n            {\r\n                param ([string]$filePath, [hashtable]$metadata)\r\n\r\n                $item = get-item $filePath -ea SilentlyContinue -Force\r\n\r\n                if ($item)\r\n                {\r\n                    # LastWriteTime\r\n                    if ($metadata['LastWriteTimeUtc'])\r\n                    {\r\n                        $item.LastWriteTimeUtc = $metadata['LastWriteTimeUtc']\r\n                    }\r\n                    if ($metadata['LastWriteTime'])\r\n                    {\r\n                        $item.LastWriteTime = $metadata['LastWriteTime']\r\n                    }\r\n\r\n                    # Attributes\r\n                    if ($metadata['Attributes'])\r\n                    {\r\n                        $item.Attributes = $metadata['Attributes']\r\n                    }\r\n                }\r\n            }\r\n            ";
			ps.AddScript(script);
			SafeInvokeCommand.Invoke(ps, this, null, false);
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x00191668 File Offset: 0x0018F868
		private void RemoveFunctionPSCopyFileToRemoteSession(PowerShell ps)
		{
			string script = "\r\n                Remove-Item function:PSCopyFileToRemoteSession -ea SilentlyContinue -Force\r\n                Remove-Item function:PSCopyFileAlternateStreamToRemoteSession -ea SilentlyContinue -Force\r\n                Remove-Item function:PSTargetSupportsAlternateStreams -ea SilentlyContinue -Force\r\n                Remove-Item function:PSRemoveRemoteItem -ea SilentlyContinue -Force\r\n                Remove-Item function:PSSetFileMetadata -ea SilentlyContinue -Force\r\n            ";
			ps.AddScript(script);
			SafeInvokeCommand.Invoke(ps, this, null, false);
		}

		// Token: 0x06004C0B RID: 19467 RVA: 0x0019168D File Offset: 0x0018F88D
		private void RemoveRemoteItem(PowerShell ps, string path)
		{
			ps.AddCommand("PSRemoveRemoteItem");
			ps.AddParameter("path", path);
			SafeInvokeCommand.Invoke(ps, this, null, false);
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x001916B4 File Offset: 0x0018F8B4
		private bool RemoteTargetSupportsAlternateStreams(PowerShell ps, string path)
		{
			bool result = false;
			ps.AddCommand("PSTargetSupportsAlternateStreams");
			ps.AddParameter("path", path);
			Hashtable hashtable = SafeInvokeCommand.Invoke(ps, this, null);
			if (hashtable != null && hashtable["TargetSupportsAlternateStreams"] != null)
			{
				result = (bool)hashtable["TargetSupportsAlternateStreams"];
			}
			return result;
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x00191708 File Offset: 0x0018F908
		private bool RemoteDirectoryExist(PowerShell ps, string path)
		{
			bool result = false;
			string script = "\r\n            # Returns a hashtable with the following member:\r\n            #    Exists - boolean to keep track of whether the given path exist for a remote directory.\r\n            #\r\n            param ([string]$path)\r\n            \r\n            $result = @{ Exists = (Test-Path $path -PathType Container) }\r\n            return $result\r\n            ";
			ps.AddScript(script);
			ps.AddParameter("path", path);
			Hashtable hashtable = SafeInvokeCommand.Invoke(ps, this, null);
			if (hashtable != null && hashtable["Exists"] != null)
			{
				result = (bool)hashtable["Exists"];
			}
			return result;
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x00191760 File Offset: 0x0018F960
		private bool CopyFileStreamToRemoteSession(FileInfo file, string destinationPath, PowerShell ps, bool isAlternateStream, string streamName)
		{
			string activity = string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyProgressActivity, new object[]
			{
				file.FullName,
				destinationPath
			});
			string statusDescription = string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyStatusDescription, new object[]
			{
				"localhost",
				ps.Runspace.ConnectionInfo.ComputerName
			});
			ProgressRecord progressRecord = new ProgressRecord(0, activity, statusDescription);
			progressRecord.PercentComplete = 0;
			progressRecord.RecordType = ProgressRecordType.Processing;
			base.WriteProgress(progressRecord);
			int num = 4194304;
			byte[] array = null;
			int num2 = 0;
			bool result = false;
			bool flag = false;
			FileStream fileStream = null;
			try
			{
				try
				{
					if (!isAlternateStream)
					{
						fileStream = File.OpenRead(file.FullName);
					}
					else
					{
						fileStream = AlternateDataStreamUtilities.CreateFileStream(file.FullName, streamName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					}
					long num3 = fileStream.Length;
					while (!base.Stopping)
					{
						num2++;
						int num4 = num;
						if ((long)num4 > num3)
						{
							num4 = (int)num3;
						}
						if (array == null)
						{
							array = new byte[num4];
						}
						else if (num4 < num)
						{
							array = new byte[num4];
						}
						int i;
						for (i = 0; i < num4; i += fileStream.Read(array, 0, num4))
						{
						}
						num3 -= (long)i;
						string value = Convert.ToBase64String(array);
						if (!isAlternateStream)
						{
							ps.AddCommand("PSCopyFileToRemoteSession");
							ps.AddParameter("file", file.Name);
							ps.AddParameter("destination", destinationPath);
							ps.AddParameter("b64Fragment", value);
							ps.AddParameter("create", num2 == 1);
						}
						else
						{
							ps.AddCommand("PSCopyFileAlternateStreamToRemoteSession");
							ps.AddParameter("file", file.Name);
							ps.AddParameter("destination", destinationPath);
							ps.AddParameter("b64Fragment", value);
							ps.AddParameter("streamName", streamName);
						}
						Hashtable hashtable = SafeInvokeCommand.Invoke(ps, this, null);
						if (hashtable == null || hashtable["BytesWritten"] == null)
						{
							flag = true;
							Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyFailed, new object[]
							{
								file
							}));
							base.WriteError(new ErrorRecord(exception, "CopyError", ErrorCategory.WriteError, file.FullName));
							return false;
						}
						if ((int)hashtable["BytesWritten"] != num4)
						{
							flag = true;
							Exception exception2 = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyFailed, new object[]
							{
								file
							}));
							base.WriteError(new ErrorRecord(exception2, "CopyError", ErrorCategory.WriteError, file.FullName));
							return false;
						}
						if (fileStream.Length > 0L)
						{
							int percentComplete = (int)((fileStream.Length - num3) * 100L / fileStream.Length);
							progressRecord.PercentComplete = percentComplete;
							base.WriteProgress(progressRecord);
						}
						if (num3 <= 0L)
						{
							progressRecord.PercentComplete = 100;
							progressRecord.RecordType = ProgressRecordType.Completed;
							base.WriteProgress(progressRecord);
							result = true;
							goto IL_363;
						}
					}
					return false;
				}
				catch (IOException exception3)
				{
					base.WriteError(new ErrorRecord(exception3, "CopyItemRemotelyIOError", ErrorCategory.WriteError, file.FullName));
				}
				catch (ArgumentException exception4)
				{
					base.WriteError(new ErrorRecord(exception4, "CopyItemRemotelyArgumentError", ErrorCategory.WriteError, file.FullName));
				}
				catch (NotSupportedException exception5)
				{
					base.WriteError(new ErrorRecord(exception5, "CopyFileInfoRemotelyPathRefersToANonFileDevice", ErrorCategory.InvalidArgument, file.FullName));
				}
				catch (SecurityException exception6)
				{
					base.WriteError(new ErrorRecord(exception6, "CopyFileInfoRemotelyUnauthorizedAccessError", ErrorCategory.PermissionDenied, file.FullName));
				}
				IL_363:;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
				if (flag)
				{
					this.RemoveRemoteItem(ps, Path.Combine(destinationPath, file.Name));
				}
			}
			return result;
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x00191B78 File Offset: 0x0018FD78
		private Hashtable GetFileMetadata(FileInfo file)
		{
			return new Hashtable
			{
				{
					"LastWriteTime",
					file.LastWriteTime
				},
				{
					"LastWriteTimeUtc",
					file.LastWriteTimeUtc
				},
				{
					"Attributes",
					file.Attributes
				}
			};
		}

		// Token: 0x06004C10 RID: 19472 RVA: 0x00191BD0 File Offset: 0x0018FDD0
		private void SetRemoteFileMetadata(FileInfo file, string remoteFilePath, PowerShell ps)
		{
			Hashtable fileMetadata = this.GetFileMetadata(file);
			if (fileMetadata != null)
			{
				ps.AddCommand("PSSetFileMetadata");
				ps.AddParameter("filePath", remoteFilePath);
				ps.AddParameter("metadata", fileMetadata);
				SafeInvokeCommand.Invoke(ps, this, null, false);
			}
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x00191C18 File Offset: 0x0018FE18
		private bool PerformCopyFileToRemoteSession(FileInfo file, string destinationPath, PowerShell ps)
		{
			if (!this.RemoteDirectoryExist(ps, destinationPath))
			{
				Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemDirectoryNotFound, new object[]
				{
					destinationPath
				}));
				base.WriteError(new ErrorRecord(exception, "RemoteDirectoryNotFound", ErrorCategory.WriteError, destinationPath));
				return false;
			}
			this.InitilizeFunctionsPSCopyFileToRemoteSession(ps);
			bool flag = this.CopyFileStreamToRemoteSession(file, destinationPath, ps, false, null);
			bool flag2 = this.RemoteTargetSupportsAlternateStreams(ps, Path.Combine(destinationPath, file.Name));
			if (flag && flag2)
			{
				foreach (AlternateStreamData alternateStreamData in AlternateDataStreamUtilities.GetStreams(file.FullName))
				{
					if (!string.Equals(":$DATA", alternateStreamData.Stream, StringComparison.OrdinalIgnoreCase))
					{
						flag = this.CopyFileStreamToRemoteSession(file, destinationPath, ps, true, alternateStreamData.Stream);
						if (!flag)
						{
							break;
						}
					}
				}
			}
			if (flag)
			{
				this.SetRemoteFileMetadata(file, Path.Combine(destinationPath, file.Name), ps);
			}
			this.RemoveFunctionPSCopyFileToRemoteSession(ps);
			return flag;
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x00191D24 File Offset: 0x0018FF24
		private bool RemoteDestinationPathIsFile(string destination, PowerShell ps)
		{
			string script = "\r\n                # Returns a hashtable with the following information:\r\n                #  - IsFileInfotrue bool to keep track if the given destination is a FileInfo type.\r\n                param ( [string] $destinationPath)\r\n    \r\n                $op = @{\r\n                    IsFileInfo = $null\r\n                }\r\n\r\n                try\r\n                {\r\n                    $op['IsFileInfo'] = (Test-Path $destinationPath -PathType Leaf)\r\n                }\r\n                catch\r\n                {\r\n                    if ($_.Exception.InnerException)\r\n                    {\r\n                        Write-Error -Exception $_.Exception.InnerException\r\n                    }\r\n                    else\r\n                    {\r\n                        Write-Error -Exception $_.Exception\r\n                    }\r\n                }\r\n\r\n                return $op\r\n            ";
			ps.AddScript(script);
			ps.AddParameter("destinationPath", destination);
			Hashtable hashtable = SafeInvokeCommand.Invoke(ps, this, null);
			if (hashtable == null || hashtable["IsFileInfo"] == null)
			{
				Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyFailedToValidateIfDestinationIsFile, new object[]
				{
					destination
				}));
				base.WriteError(new ErrorRecord(exception, "CopyError", ErrorCategory.WriteError, destination));
				return false;
			}
			return (bool)hashtable["IsFileInfo"];
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x00191DAC File Offset: 0x0018FFAC
		private string CreateDirectoryOnRemoteSession(string destination, bool force, PowerShell ps)
		{
			string script = "\r\n            # Return a hash table in the following format:\r\n            #      DirectoryPath is the directory to be created.\r\n            #      PathExists is a bool to to keep track of whether the directory already exist.\r\n            #\r\n            # 1 ) If DirectoryPath already exists:\r\n            #     a) If -Force is specified, force create the directory. Set DirectoryPath to the created directory path.     \r\n            #     b) If not -Force is specified, then set PathExists to $true.  \r\n            # 2) If DirectoryPath does not exist, create it. Set DirectoryPath to the created directory path.\r\n            param ( [string] $path, [switch]$force = $false)\r\n\r\n            $op = @{\r\n                DirectoryPath = $null\r\n                PathExists = $false\r\n            }\r\n\r\n            try\r\n            {\r\n                if (Test-Path $path)\r\n                {\r\n                    # -Force is specified, then force create the directory.\r\n                    if ($force)\r\n                    {\r\n                        New-Item $path -ItemType Directory -Force | Out-Null\r\n                        $op['DirectoryPath'] = $path\r\n                    }\r\n                    else\r\n                    {\r\n                        $op['PathExists'] = $true\r\n                    }\r\n                }\r\n                else\r\n                {\r\n                    New-Item $path -ItemType Directory | Out-Null\r\n                    $op['DirectoryPath'] = $path\r\n                }\r\n            }\r\n            catch\r\n            {\r\n                Write-Error -Exception $_.Exception.InnerException\r\n            }\r\n\r\n            return $op\r\n            ";
			ps.AddScript(script);
			ps.AddParameter("path", destination);
			if (force)
			{
				ps.AddParameter("force", true);
			}
			Hashtable hashtable = SafeInvokeCommand.Invoke(ps, this, null);
			if (hashtable["ExceptionThrown"] != null && (bool)hashtable["ExceptionThrown"])
			{
				return null;
			}
			if (force && hashtable["DirectoryPath"] == null)
			{
				Exception exception = new IOException(string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.CopyItemRemotelyFailedToCreateDirectory, new object[]
				{
					destination
				}));
				base.WriteError(new ErrorRecord(exception, "FailedToCreateDirectory", ErrorCategory.WriteError, destination));
				return null;
			}
			string text = (string)hashtable["DirectoryPath"];
			if (!force && (bool)hashtable["PathExists"])
			{
				Exception exception2 = new IOException(StringUtil.Format(FileSystemProviderStrings.DirectoryExist, text));
				base.WriteError(new ErrorRecord(exception2, "DirectoryExist", ErrorCategory.ResourceExists, text));
				return null;
			}
			return text;
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x00191EB0 File Offset: 0x001900B0
		protected override string GetParentPath(string path, string root)
		{
			string text = base.GetParentPath(path, root);
			if (FileSystemProvider.IsUNCPath(path))
			{
				int num = text.LastIndexOf('\\');
				if (num < 3)
				{
					text = string.Empty;
				}
			}
			else
			{
				text = FileSystemProvider.EnsureDriveIsRooted(text);
			}
			return text;
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x00191EEC File Offset: 0x001900EC
		private static bool IsAbsolutePath(string path)
		{
			bool flag = false;
			int num = path.IndexOf(':');
			if (num != -1)
			{
				flag = true;
			}
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x00191F2C File Offset: 0x0019012C
		private static bool IsUNCPath(string path)
		{
			bool flag = path.StartsWith("\\\\", StringComparison.Ordinal);
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x00191F68 File Offset: 0x00190168
		private static bool IsUNCRoot(string path)
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(path) && FileSystemProvider.IsUNCPath(path))
			{
				int num = path.Length - 1;
				if (path[path.Length - 1] == '\\')
				{
					num--;
				}
				int num2 = 0;
				do
				{
					num = path.LastIndexOf('\\', num);
					if (num == -1)
					{
						break;
					}
					num--;
					if (num < 3)
					{
						break;
					}
					num2++;
				}
				while (num > 3);
				if (num2 == 1)
				{
					flag = true;
				}
			}
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x00191FF0 File Offset: 0x001901F0
		private static bool IsPathRoot(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			bool flag = string.Equals(path, Path.GetPathRoot(path), StringComparison.OrdinalIgnoreCase);
			bool flag2 = FileSystemProvider.IsUNCRoot(path);
			bool flag3 = flag || flag2;
			FileSystemProvider.tracer.WriteLine("result = {0}; isDriveRoot = {1}; isUNCRoot = {2}", new object[]
			{
				flag3,
				flag,
				flag2
			});
			return flag3;
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x00192058 File Offset: 0x00190258
		protected override string NormalizeRelativePath(string path, string basePath)
		{
			if (string.IsNullOrEmpty(path) || !this.IsValidPath(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (basePath == null)
			{
				basePath = string.Empty;
			}
			FileSystemProvider.tracer.WriteLine("basePath = {0}", new object[]
			{
				basePath
			});
			string text = path;
			path = FileSystemProvider.NormalizePath(path);
			path = FileSystemProvider.EnsureDriveIsRooted(path);
			path = this.NormalizeRelativePathHelper(path, basePath);
			basePath = FileSystemProvider.NormalizePath(basePath);
			basePath = FileSystemProvider.EnsureDriveIsRooted(basePath);
			text = path;
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					string text2 = path;
					if (!text2.EndsWith(string.Concat('\\'), StringComparison.OrdinalIgnoreCase))
					{
						text2 += '\\';
					}
					string text3 = basePath;
					if (!text3.EndsWith(string.Concat('\\'), StringComparison.OrdinalIgnoreCase))
					{
						text3 += '\\';
					}
					if (text2.StartsWith(text3, StringComparison.OrdinalIgnoreCase))
					{
						if (!FileSystemProvider.IsUNCPath(text) && !text.StartsWith(basePath, StringComparison.CurrentCulture))
						{
							text = this.MakePath(basePath, text);
						}
						if (FileSystemProvider.IsPathRoot(text))
						{
							text = FileSystemProvider.EnsureDriveIsRooted(text);
						}
						else
						{
							string parentPath = this.GetParentPath(text, string.Empty);
							if (string.IsNullOrEmpty(parentPath))
							{
								return string.Empty;
							}
							string childName = this.GetChildName(text);
							IEnumerable<string> enumerable = Directory.EnumerateFiles(parentPath, childName);
							if (enumerable == null || !enumerable.Any<string>())
							{
								enumerable = Directory.EnumerateDirectories(parentPath, childName);
							}
							if (enumerable == null || !enumerable.Any<string>())
							{
								string message = StringUtil.Format(FileSystemProviderStrings.ItemDoesNotExist, path);
								Exception exception = new IOException(message);
								base.WriteError(new ErrorRecord(exception, "ItemDoesNotExist", ErrorCategory.ObjectNotFound, path));
							}
							else
							{
								text = enumerable.First<string>();
								if (text.StartsWith(basePath, StringComparison.CurrentCulture))
								{
									text = text.Substring(basePath.Length);
								}
								else
								{
									string message2 = StringUtil.Format(FileSystemProviderStrings.PathOutSideBasePath, path);
									Exception exception2 = new ArgumentException(message2);
									base.WriteError(new ErrorRecord(exception2, "PathOutSideBasePath", ErrorCategory.InvalidArgument, null));
								}
							}
						}
					}
				}
				catch (ArgumentException exception3)
				{
					base.WriteError(new ErrorRecord(exception3, "NormalizeRelativePathArgumentError", ErrorCategory.InvalidArgument, path));
				}
				catch (DirectoryNotFoundException exception4)
				{
					base.WriteError(new ErrorRecord(exception4, "NormalizeRelativePathDirectoryNotFoundError", ErrorCategory.ObjectNotFound, path));
				}
				catch (IOException exception5)
				{
					base.WriteError(new ErrorRecord(exception5, "NormalizeRelativePathIOError", ErrorCategory.ReadError, path));
				}
				catch (UnauthorizedAccessException exception6)
				{
					base.WriteError(new ErrorRecord(exception6, "NormalizeRelativePathUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
				}
			}
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x00192330 File Offset: 0x00190530
		private string NormalizeRelativePathHelper(string path, string basePath)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				return string.Empty;
			}
			if (basePath == null)
			{
				basePath = string.Empty;
			}
			FileSystemProvider.tracer.WriteLine("basePath = {0}", new object[]
			{
				basePath
			});
			string text = string.Empty;
			int num = path.IndexOf(':');
			int num2 = path.IndexOf(':', num + 1);
			if (num2 > 0)
			{
				string text2 = path.Substring(0, num2);
				text = path.Replace(text2, "");
				path = text2;
			}
			string text3 = path;
			path = path.Replace('/', '\\');
			string text4 = path;
			path = path.TrimEnd(new char[]
			{
				'\\'
			});
			basePath = basePath.Replace('/', '\\');
			basePath = basePath.TrimEnd(new char[]
			{
				'\\'
			});
			path = this.RemoveRelativeTokens(path);
			if (string.Equals(path, basePath, StringComparison.OrdinalIgnoreCase) && !text4.EndsWith(string.Concat('\\'), StringComparison.OrdinalIgnoreCase))
			{
				string childName = this.GetChildName(path);
				text3 = this.MakePath("..", childName);
			}
			else if (!(path + '\\').StartsWith(basePath + '\\', StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(basePath))
			{
				text3 = string.Empty;
				string commonBase = this.GetCommonBase(path, basePath);
				Stack<string> stack = this.TokenizePathToStack(basePath, commonBase);
				int num3 = stack.Count;
				if (string.IsNullOrEmpty(commonBase))
				{
					num3--;
				}
				for (int i = 0; i < num3; i++)
				{
					text3 = this.MakePath("..", text3);
				}
				if (!string.IsNullOrEmpty(commonBase))
				{
					if (string.Equals(path, commonBase, StringComparison.OrdinalIgnoreCase) && !path.EndsWith(string.Concat('\\'), StringComparison.OrdinalIgnoreCase))
					{
						string childName2 = this.GetChildName(path);
						text3 = this.MakePath("..", text3);
						text3 = this.MakePath(text3, childName2);
					}
					else
					{
						string[] array = this.TokenizePathToStack(path, commonBase).ToArray();
						for (int j = 0; j < array.Length; j++)
						{
							text3 = this.MakePath(text3, array[j]);
						}
					}
				}
			}
			else if (FileSystemProvider.IsPathRoot(path))
			{
				if (string.IsNullOrEmpty(basePath))
				{
					text3 = path;
				}
				else
				{
					text3 = string.Empty;
				}
			}
			else
			{
				Stack<string> tokenizedPathStack = this.TokenizePathToStack(path, basePath);
				Stack<string> normalizedPathStack = new Stack<string>();
				try
				{
					normalizedPathStack = this.NormalizeThePath(basePath, tokenizedPathStack);
				}
				catch (ArgumentException exception)
				{
					base.WriteError(new ErrorRecord(exception, "NormalizeRelativePathHelperArgumentError", ErrorCategory.InvalidArgument, null));
					text3 = null;
					goto IL_281;
				}
				text3 = this.CreateNormalizedRelativePathFromStack(normalizedPathStack);
			}
			IL_281:
			if (!string.IsNullOrEmpty(text))
			{
				text3 += text;
			}
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				text3
			});
			return text3;
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x00192604 File Offset: 0x00190804
		private string RemoveRelativeTokens(string path)
		{
			string text = path.Replace('/', '\\');
			if (text.IndexOf("\\", StringComparison.OrdinalIgnoreCase) >= 0 && !text.StartsWith(".\\", StringComparison.OrdinalIgnoreCase) && !text.StartsWith("..\\", StringComparison.OrdinalIgnoreCase) && !text.EndsWith("\\.", StringComparison.OrdinalIgnoreCase) && !text.EndsWith("\\..", StringComparison.OrdinalIgnoreCase) && text.IndexOf("\\.\\", StringComparison.OrdinalIgnoreCase) <= 0)
			{
				if (text.IndexOf("\\..\\", StringComparison.OrdinalIgnoreCase) <= 0)
				{
					return path;
				}
			}
			try
			{
				Stack<string> tokenizedPathStack = this.TokenizePathToStack(path, "");
				Stack<string> normalizedPathStack = this.NormalizeThePath("", tokenizedPathStack);
				return this.CreateNormalizedRelativePathFromStack(normalizedPathStack);
			}
			catch (UnauthorizedAccessException)
			{
			}
			return path;
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x001926BC File Offset: 0x001908BC
		private string GetCommonBase(string path1, string path2)
		{
			while (!string.Equals(path1, path2, StringComparison.OrdinalIgnoreCase))
			{
				if (path2.Length > path1.Length)
				{
					path2 = this.GetParentPath(path2, null);
				}
				else
				{
					path1 = this.GetParentPath(path1, null);
				}
			}
			return path1;
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x001926F0 File Offset: 0x001908F0
		private Stack<string> TokenizePathToStack(string path, string basePath)
		{
			Stack<string> stack = new Stack<string>();
			string text = path;
			string text2 = path;
			while (text.Length > basePath.Length)
			{
				string childName = this.GetChildName(text);
				if (string.IsNullOrEmpty(childName))
				{
					FileSystemProvider.tracer.WriteLine("tokenizedPathStack.Push({0})", new object[]
					{
						text
					});
					stack.Push(text);
					break;
				}
				FileSystemProvider.tracer.WriteLine("tokenizedPathStack.Push({0})", new object[]
				{
					childName
				});
				stack.Push(childName);
				text = this.GetParentPath(text, basePath);
				if (text.Length >= text2.Length || FileSystemProvider.IsPathRoot(text))
				{
					if (string.IsNullOrEmpty(basePath))
					{
						FileSystemProvider.tracer.WriteLine("tokenizedPathStack.Push({0})", new object[]
						{
							text
						});
						stack.Push(text);
						break;
					}
					break;
				}
				else
				{
					text2 = text;
				}
			}
			return stack;
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x001927D0 File Offset: 0x001909D0
		private Stack<string> NormalizeThePath(string basepath, Stack<string> tokenizedPathStack)
		{
			Stack<string> stack = new Stack<string>();
			string text = basepath;
			while (tokenizedPathStack.Count > 0)
			{
				string text2 = tokenizedPathStack.Pop();
				FileSystemProvider.tracer.WriteLine("childName = {0}", new object[]
				{
					text2
				});
				if (!text2.Equals(".", StringComparison.OrdinalIgnoreCase))
				{
					if (text2.Equals("..", StringComparison.OrdinalIgnoreCase))
					{
						if (stack.Count <= 0)
						{
							throw PSTraceSource.NewArgumentException("path", FileSystemProviderStrings.PathOutSideBasePath, new object[0]);
						}
						string text3 = stack.Pop();
						if (text.Length > text3.Length)
						{
							text = text.Substring(0, text.Length - text3.Length - 1);
						}
						else
						{
							text = "";
						}
						FileSystemProvider.tracer.WriteLine("normalizedPathStack.Pop() : {0}", new object[]
						{
							text3
						});
					}
					else
					{
						text = this.MakePath(text, text2);
						bool flag = false;
						FileSystemInfo fileSystemInfo = FileSystemProvider.GetFileSystemInfo(text, ref flag);
						if (fileSystemInfo != null)
						{
							if (fileSystemInfo.FullName.Length < text.Length)
							{
								throw PSTraceSource.NewArgumentException("path", FileSystemProviderStrings.ItemDoesNotExist, new object[]
								{
									text
								});
							}
							if (fileSystemInfo.Name.Length >= text2.Length)
							{
								text2 = fileSystemInfo.Name;
							}
						}
						else if (!flag && tokenizedPathStack.Count == 0)
						{
							throw PSTraceSource.NewArgumentException("path", FileSystemProviderStrings.ItemDoesNotExist, new object[]
							{
								text
							});
						}
						FileSystemProvider.tracer.WriteLine("normalizedPathStack.Push({0})", new object[]
						{
							text2
						});
						stack.Push(text2);
					}
				}
			}
			return stack;
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x0019296C File Offset: 0x00190B6C
		private string CreateNormalizedRelativePathFromStack(Stack<string> normalizedPathStack)
		{
			string text = string.Empty;
			while (normalizedPathStack.Count > 0)
			{
				if (string.IsNullOrEmpty(text))
				{
					text = normalizedPathStack.Pop();
				}
				else
				{
					string parent = normalizedPathStack.Pop();
					text = this.MakePath(parent, text);
				}
			}
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x001929C8 File Offset: 0x00190BC8
		protected override string GetChildName(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = path.Replace('/', '\\');
			path = path.TrimEnd(new char[]
			{
				'\\'
			});
			int num = path.LastIndexOf('\\');
			string text;
			if (num == -1)
			{
				text = FileSystemProvider.EnsureDriveIsRooted(path);
			}
			else if (FileSystemProvider.IsUNCPath(path))
			{
				if (FileSystemProvider.IsUNCRoot(path))
				{
					text = string.Empty;
				}
				else
				{
					text = path.Substring(num + 1);
				}
			}
			else
			{
				text = path.Substring(num + 1);
			}
			FileSystemProvider.tracer.WriteLine("Result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x00192A6C File Offset: 0x00190C6C
		private static string EnsureDriveIsRooted(string path)
		{
			string text = path;
			int num = path.IndexOf(':');
			if (num != -1 && num + 1 == path.Length)
			{
				text = path + '\\';
			}
			FileSystemProvider.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x00192ABD File Offset: 0x00190CBD
		protected override bool IsItemContainer(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			return Utils.NativeDirectoryExists(path);
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x00192AE0 File Offset: 0x00190CE0
		protected override void MoveItem(string path, string destination)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (string.IsNullOrEmpty(destination))
			{
				throw PSTraceSource.NewArgumentException("destination");
			}
			path = FileSystemProvider.NormalizePath(path);
			destination = FileSystemProvider.NormalizePath(destination);
			if (Utils.IsReservedDeviceName(destination))
			{
				string message = StringUtil.Format(FileSystemProviderStrings.TargetCannotContainDeviceName, destination);
				Exception exception = new IOException(message);
				base.WriteError(new ErrorRecord(exception, "MoveError", ErrorCategory.WriteError, destination));
				return;
			}
			try
			{
				bool flag = this.IsItemContainer(path);
				FileSystemProvider.tracer.WriteLine("Moving {0} to {1}", new object[]
				{
					path,
					destination
				});
				if (flag)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					if (this.ItemExists(destination) && this.IsItemContainer(destination))
					{
						destination = this.MakePath(destination, directoryInfo.Name);
					}
					string moveItemActionDirectory = FileSystemProviderStrings.MoveItemActionDirectory;
					string target = StringUtil.Format(FileSystemProviderStrings.MoveItemResourceFileTemplate, directoryInfo.FullName, destination);
					if (base.ShouldProcess(target, moveItemActionDirectory))
					{
						this.MoveDirectoryInfoItem(directoryInfo, destination, base.Force);
					}
				}
				else
				{
					FileInfo fileInfo = new FileInfo(path);
					if (this.IsItemContainer(destination))
					{
						destination = this.MakePath(destination, fileInfo.Name);
					}
					string moveItemActionFile = FileSystemProviderStrings.MoveItemActionFile;
					string target2 = StringUtil.Format(FileSystemProviderStrings.MoveItemResourceFileTemplate, fileInfo.FullName, destination);
					if (base.ShouldProcess(target2, moveItemActionFile))
					{
						this.MoveFileInfoItem(fileInfo, destination, base.Force, true);
					}
				}
			}
			catch (ArgumentException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "MoveItemArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "MoveItemIOError", ErrorCategory.WriteError, path));
			}
			catch (UnauthorizedAccessException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "MoveItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x00192CB8 File Offset: 0x00190EB8
		private void MoveFileInfoItem(FileInfo file, string destination, bool force, bool output)
		{
			try
			{
				file.MoveTo(destination);
				if (output)
				{
					base.WriteItemObject(file, file.FullName, false);
				}
			}
			catch (UnauthorizedAccessException exception)
			{
				if (force)
				{
					try
					{
						file.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden);
						file.MoveTo(destination);
						if (output)
						{
							base.WriteItemObject(file, file.FullName, false);
						}
						goto IL_C6;
					}
					catch (Exception ex)
					{
						if (ex is IOException || ex is ArgumentNullException || ex is ArgumentException || ex is SecurityException || ex is UnauthorizedAccessException || ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is PathTooLongException || ex is NotSupportedException)
						{
							base.WriteError(new ErrorRecord(exception, "MoveFileInfoItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, file));
							goto IL_C6;
						}
						throw;
					}
				}
				base.WriteError(new ErrorRecord(exception, "MoveFileInfoItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, file));
				IL_C6:;
			}
			catch (ArgumentException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "MoveFileInfoItemArgumentError", ErrorCategory.InvalidArgument, file));
			}
			catch (IOException exception3)
			{
				if (force && File.Exists(destination))
				{
					FileInfo fileInfo = new FileInfo(destination);
					if (fileInfo != null)
					{
						try
						{
							fileInfo.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden);
							fileInfo.Delete();
							file.MoveTo(destination);
							if (output)
							{
								base.WriteItemObject(file, file.FullName, false);
							}
							goto IL_1D3;
						}
						catch (Exception ex2)
						{
							if (ex2 is FileNotFoundException || ex2 is DirectoryNotFoundException || ex2 is UnauthorizedAccessException || ex2 is SecurityException || ex2 is ArgumentException || ex2 is PathTooLongException || ex2 is NotSupportedException || ex2 is ArgumentNullException || ex2 is IOException)
							{
								base.WriteError(new ErrorRecord(exception3, "MoveFileInfoItemIOError", ErrorCategory.WriteError, fileInfo));
								goto IL_1D3;
							}
							throw;
						}
					}
					base.WriteError(new ErrorRecord(exception3, "MoveFileInfoItemIOError", ErrorCategory.WriteError, file));
				}
				else
				{
					base.WriteError(new ErrorRecord(exception3, "MoveFileInfoItemIOError", ErrorCategory.WriteError, file));
				}
				IL_1D3:;
			}
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x00192EDC File Offset: 0x001910DC
		private void MoveDirectoryInfoItem(DirectoryInfo directory, string destination, bool force)
		{
			try
			{
				if (!this.IsSameVolume(directory.FullName, destination))
				{
					this.CopyAndDelete(directory, destination, force);
				}
				else
				{
					directory.MoveTo(destination);
				}
				base.WriteItemObject(directory, directory.FullName, true);
			}
			catch (UnauthorizedAccessException exception)
			{
				if (force)
				{
					try
					{
						directory.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden);
						if (!this.IsSameVolume(directory.FullName, destination))
						{
							this.CopyAndDelete(directory, destination, force);
						}
						else
						{
							directory.MoveTo(destination);
						}
						base.WriteItemObject(directory, directory.FullName, true);
						goto IL_E9;
					}
					catch (IOException)
					{
						base.WriteError(new ErrorRecord(exception, "MoveDirectoryItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, directory));
						goto IL_E9;
					}
					catch (Exception ex)
					{
						if (ex is FileNotFoundException || ex is ArgumentNullException || ex is DirectoryNotFoundException || ex is SecurityException || ex is ArgumentException)
						{
							base.WriteError(new ErrorRecord(exception, "MoveDirectoryItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, directory));
							goto IL_E9;
						}
						throw;
					}
				}
				base.WriteError(new ErrorRecord(exception, "MoveDirectoryItemUnauthorizedAccessError", ErrorCategory.PermissionDenied, directory));
				IL_E9:;
			}
			catch (ArgumentException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "MoveDirectoryItemArgumentError", ErrorCategory.InvalidArgument, directory));
			}
			catch (IOException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "MoveDirectoryItemIOError", ErrorCategory.WriteError, directory));
			}
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x00193044 File Offset: 0x00191244
		private void CopyAndDelete(DirectoryInfo directory, string destination, bool force)
		{
			if (!this.ItemExists(destination))
			{
				this.CreateDirectory(destination, false);
			}
			else if (this.ItemExists(destination) && !this.IsItemContainer(destination))
			{
				string message = StringUtil.Format(FileSystemProviderStrings.DirectoryExist, destination);
				Exception exception = new IOException(message);
				base.WriteError(new ErrorRecord(exception, "DirectoryExist", ErrorCategory.ResourceExists, destination));
				return;
			}
			foreach (FileInfo fileInfo in directory.EnumerateFiles())
			{
				this.MoveFileInfoItem(fileInfo, Path.Combine(destination, fileInfo.Name), force, false);
			}
			foreach (DirectoryInfo directoryInfo in directory.EnumerateDirectories())
			{
				this.CopyAndDelete(directoryInfo, Path.Combine(destination, directoryInfo.Name), force);
			}
			if (!directory.EnumerateDirectories().Any<DirectoryInfo>() && !directory.EnumerateFiles().Any<FileInfo>())
			{
				this.RemoveItem(directory.FullName, false);
			}
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x00193168 File Offset: 0x00191368
		private bool IsSameVolume(string source, string destination)
		{
			FileInfo fileInfo = new FileInfo(source);
			FileInfo fileInfo2 = new FileInfo(destination);
			return fileInfo.Directory.Root.Name == fileInfo2.Directory.Root.Name;
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x001931A8 File Offset: 0x001913A8
		public void GetProperty(string path, Collection<string> providerSpecificPickList)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			PSObject psobject = null;
			try
			{
				FileSystemInfo fileSystemInfo = null;
				bool flag2;
				Exception ex;
				bool flag = Utils.NativeItemExists(path, out flag2, out ex);
				if (ex != null)
				{
					throw ex;
				}
				if (flag)
				{
					if (flag2)
					{
						fileSystemInfo = new DirectoryInfo(path);
					}
					else
					{
						fileSystemInfo = new FileInfo(path);
					}
				}
				if (fileSystemInfo == null)
				{
					string message = StringUtil.Format(FileSystemProviderStrings.ItemDoesNotExist, path);
					Exception exception = new IOException(message);
					base.WriteError(new ErrorRecord(exception, "ItemDoesNotExist", ErrorCategory.ObjectNotFound, path));
				}
				else if (providerSpecificPickList == null || providerSpecificPickList.Count == 0)
				{
					psobject = PSObject.AsPSObject(fileSystemInfo);
				}
				else
				{
					foreach (string text in providerSpecificPickList)
					{
						if (text != null && text.Length > 0)
						{
							try
							{
								PSObject psobject2 = PSObject.AsPSObject(fileSystemInfo);
								PSMemberInfo psmemberInfo = psobject2.Properties[text];
								if (psmemberInfo != null)
								{
									object value = psmemberInfo.Value;
									if (psobject == null)
									{
										psobject = new PSObject();
									}
									psobject.Properties.Add(new PSNoteProperty(text, value));
								}
								else
								{
									string message2 = StringUtil.Format(FileSystemProviderStrings.PropertyNotFound, text);
									Exception exception2 = new IOException(message2);
									base.WriteError(new ErrorRecord(exception2, "GetValueError", ErrorCategory.ReadError, text));
								}
							}
							catch (GetValueException exception3)
							{
								base.WriteError(new ErrorRecord(exception3, "GetValueError", ErrorCategory.ReadError, text));
							}
						}
					}
				}
			}
			catch (ArgumentException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "GetPropertyArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception5)
			{
				base.WriteError(new ErrorRecord(exception5, "GetPropertyIOError", ErrorCategory.ReadError, path));
			}
			catch (UnauthorizedAccessException exception6)
			{
				base.WriteError(new ErrorRecord(exception6, "GetPropertyUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
			if (psobject != null)
			{
				base.WritePropertyObject(psobject, path);
			}
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x001933EC File Offset: 0x001915EC
		public object GetPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList)
		{
			return null;
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x001933F0 File Offset: 0x001915F0
		public void SetProperty(string path, PSObject propertyToSet)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (propertyToSet == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyToSet");
			}
			path = FileSystemProvider.NormalizePath(path);
			PSObject psobject = new PSObject();
			PSObject psobject2 = null;
			bool flag = false;
			bool flag3;
			Exception ex;
			bool flag2 = Utils.NativeItemExists(path, out flag3, out ex);
			if (ex != null)
			{
				throw ex;
			}
			if (flag2)
			{
				if (flag3)
				{
					flag = true;
					psobject2 = PSObject.AsPSObject(new DirectoryInfo(path));
				}
				else
				{
					psobject2 = PSObject.AsPSObject(new FileInfo(path));
				}
			}
			if (psobject2 != null)
			{
				bool flag4 = false;
				foreach (PSMemberInfo psmemberInfo in propertyToSet.Properties)
				{
					object value = psmemberInfo.Value;
					string action = null;
					if (flag)
					{
						action = FileSystemProviderStrings.SetPropertyActionDirectory;
					}
					else
					{
						action = FileSystemProviderStrings.SetPropertyActionFile;
					}
					string setPropertyResourceTemplate = FileSystemProviderStrings.SetPropertyResourceTemplate;
					string text = value.ToString();
					try
					{
						PSObject psobject3 = PSObject.AsPSObject(value);
						text = psobject3.ToString();
					}
					catch (Exception)
					{
						throw;
					}
					string target = string.Format(base.Host.CurrentCulture, setPropertyResourceTemplate, new object[]
					{
						path,
						psmemberInfo.Name,
						text
					});
					if (base.ShouldProcess(target, action))
					{
						PSObject psobject4 = PSObject.AsPSObject(psobject2);
						PSMemberInfo psmemberInfo2 = psobject4.Properties[psmemberInfo.Name];
						if (psmemberInfo2 != null)
						{
							if (string.Compare(psmemberInfo.Name, "attributes", StringComparison.OrdinalIgnoreCase) == 0)
							{
								FileAttributes fileAttributes;
								if (value is FileAttributes)
								{
									fileAttributes = (FileAttributes)value;
								}
								else
								{
									fileAttributes = (FileAttributes)Enum.Parse(typeof(FileAttributes), text, true);
								}
								if ((fileAttributes & ~(FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Archive | FileAttributes.Normal)) != (FileAttributes)0)
								{
									string message = StringUtil.Format(FileSystemProviderStrings.AttributesNotSupported, psmemberInfo);
									Exception exception = new IOException(message);
									base.WriteError(new ErrorRecord(exception, "SetPropertyError", ErrorCategory.ReadError, psmemberInfo));
									continue;
								}
							}
							psmemberInfo2.Value = value;
							psobject.Properties.Add(new PSNoteProperty(psmemberInfo.Name, value));
							flag4 = true;
						}
						else
						{
							string message2 = StringUtil.Format(FileSystemProviderStrings.PropertyNotFound, psmemberInfo);
							Exception exception2 = new IOException(message2);
							base.WriteError(new ErrorRecord(exception2, "SetPropertyError", ErrorCategory.ReadError, psmemberInfo));
						}
					}
				}
				if (flag4)
				{
					base.WritePropertyObject(psobject, path);
					return;
				}
			}
			else
			{
				string message3 = StringUtil.Format(FileSystemProviderStrings.ItemDoesNotExist, path);
				Exception exception3 = new IOException(message3);
				base.WriteError(new ErrorRecord(exception3, "ItemDoesNotExist", ErrorCategory.ObjectNotFound, path));
			}
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x0019368C File Offset: 0x0019188C
		public object SetPropertyDynamicParameters(string path, PSObject propertyValue)
		{
			return null;
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x00193690 File Offset: 0x00191890
		public void ClearProperty(string path, Collection<string> propertiesToClear)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			if (propertiesToClear == null || propertiesToClear.Count == 0)
			{
				throw PSTraceSource.NewArgumentNullException("propertiesToClear");
			}
			if (propertiesToClear.Count > 1 || base.Host.CurrentCulture.CompareInfo.Compare("Attributes", propertiesToClear[0], CompareOptions.IgnoreCase) != 0)
			{
				throw PSTraceSource.NewArgumentException("propertiesToClear", FileSystemProviderStrings.CannotClearProperty, new object[0]);
			}
			try
			{
				bool flag = this.IsItemContainer(path);
				string action;
				FileSystemInfo fileSystemInfo;
				if (flag)
				{
					action = FileSystemProviderStrings.ClearPropertyActionDirectory;
					fileSystemInfo = new DirectoryInfo(path);
				}
				else
				{
					action = FileSystemProviderStrings.ClearPropertyActionFile;
					fileSystemInfo = new FileInfo(path);
				}
				string clearPropertyResourceTemplate = FileSystemProviderStrings.ClearPropertyResourceTemplate;
				string target = string.Format(base.Host.CurrentCulture, clearPropertyResourceTemplate, new object[]
				{
					fileSystemInfo.FullName,
					propertiesToClear[0]
				});
				if (base.ShouldProcess(target, action))
				{
					fileSystemInfo.Attributes = FileAttributes.Normal;
					base.WritePropertyObject(new PSObject
					{
						Properties = 
						{
							new PSNoteProperty(propertiesToClear[0], fileSystemInfo.Attributes)
						}
					}, path);
				}
			}
			catch (UnauthorizedAccessException exception)
			{
				base.WriteError(new ErrorRecord(exception, "ClearPropertyUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
			catch (ArgumentException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "ClearPropertyArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "ClearPropertyIOError", ErrorCategory.WriteError, path));
			}
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x00193834 File Offset: 0x00191A34
		public object ClearPropertyDynamicParameters(string path, Collection<string> propertiesToClear)
		{
			return null;
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x00193838 File Offset: 0x00191A38
		public IContentReader GetContentReader(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			string delimiter = "\n";
			Encoding encoding = ClrFacade.GetDefaultEncoding();
			bool waitForChanges = false;
			bool flag = false;
			bool flag2 = false;
			bool isRawStream = false;
			string streamName = null;
			if (base.DynamicParameters != null)
			{
				FileSystemContentReaderDynamicParameters fileSystemContentReaderDynamicParameters = base.DynamicParameters as FileSystemContentReaderDynamicParameters;
				if (fileSystemContentReaderDynamicParameters != null)
				{
					this.ValidateParameters(fileSystemContentReaderDynamicParameters.Raw);
					isRawStream = fileSystemContentReaderDynamicParameters.Raw;
					flag2 = fileSystemContentReaderDynamicParameters.DelimiterSpecified;
					if (flag2)
					{
						delimiter = fileSystemContentReaderDynamicParameters.Delimiter;
					}
					flag = fileSystemContentReaderDynamicParameters.UsingByteEncoding;
					bool wasStreamTypeSpecified = fileSystemContentReaderDynamicParameters.WasStreamTypeSpecified;
					if (wasStreamTypeSpecified)
					{
						encoding = fileSystemContentReaderDynamicParameters.EncodingType;
					}
					waitForChanges = fileSystemContentReaderDynamicParameters.Wait;
					streamName = fileSystemContentReaderDynamicParameters.Stream;
				}
			}
			int num = path.IndexOf(':');
			int num2 = path.IndexOf(':', num + 1);
			if (num2 > 0)
			{
				streamName = path.Substring(num2 + 1);
				path = path.Remove(num2);
			}
			FileSystemContentReaderWriter result = null;
			try
			{
				if (flag2)
				{
					if (flag)
					{
						Exception exception = new ArgumentException(FileSystemProviderStrings.DelimiterError, "delimiter");
						base.WriteError(new ErrorRecord(exception, "GetContentReaderArgumentError", ErrorCategory.InvalidArgument, path));
					}
					else
					{
						result = new FileSystemContentReaderWriter(path, streamName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, delimiter, encoding, waitForChanges, this, isRawStream);
					}
				}
				else
				{
					result = new FileSystemContentReaderWriter(path, streamName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, encoding, flag, waitForChanges, this, isRawStream);
				}
			}
			catch (PathTooLongException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "GetContentReaderPathTooLongError", ErrorCategory.InvalidArgument, path));
			}
			catch (FileNotFoundException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "GetContentReaderFileNotFoundError", ErrorCategory.ObjectNotFound, path));
			}
			catch (DirectoryNotFoundException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "GetContentReaderDirectoryNotFoundError", ErrorCategory.ObjectNotFound, path));
			}
			catch (ArgumentException exception5)
			{
				base.WriteError(new ErrorRecord(exception5, "GetContentReaderArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception6)
			{
				base.WriteError(new ErrorRecord(exception6, "GetContentReaderIOError", ErrorCategory.ReadError, path));
			}
			catch (SecurityException exception7)
			{
				base.WriteError(new ErrorRecord(exception7, "GetContentReaderSecurityError", ErrorCategory.PermissionDenied, path));
			}
			catch (UnauthorizedAccessException exception8)
			{
				base.WriteError(new ErrorRecord(exception8, "GetContentReaderUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
			return result;
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x00193A9C File Offset: 0x00191C9C
		public object GetContentReaderDynamicParameters(string path)
		{
			return new FileSystemContentReaderDynamicParameters();
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x00193AA4 File Offset: 0x00191CA4
		public IContentWriter GetContentWriter(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			bool usingByteEncoding = false;
			Encoding encoding = ClrFacade.GetDefaultEncoding();
			FileMode mode = FileMode.OpenOrCreate;
			string streamName = null;
			bool suppressNewline = false;
			if (base.DynamicParameters != null)
			{
				FileSystemContentWriterDynamicParameters fileSystemContentWriterDynamicParameters = base.DynamicParameters as FileSystemContentWriterDynamicParameters;
				if (fileSystemContentWriterDynamicParameters != null)
				{
					usingByteEncoding = fileSystemContentWriterDynamicParameters.UsingByteEncoding;
					bool wasStreamTypeSpecified = fileSystemContentWriterDynamicParameters.WasStreamTypeSpecified;
					if (wasStreamTypeSpecified)
					{
						encoding = fileSystemContentWriterDynamicParameters.EncodingType;
					}
					streamName = fileSystemContentWriterDynamicParameters.Stream;
					suppressNewline = fileSystemContentWriterDynamicParameters.NoNewline.IsPresent;
				}
			}
			int num = path.IndexOf(':');
			int num2 = path.IndexOf(':', num + 1);
			if (num2 > 0)
			{
				streamName = path.Substring(num2 + 1);
				path = path.Remove(num2);
			}
			FileSystemContentReaderWriter result = null;
			try
			{
				result = new FileSystemContentReaderWriter(path, streamName, mode, FileAccess.Write, FileShare.Write, encoding, usingByteEncoding, false, this, false, suppressNewline);
			}
			catch (PathTooLongException exception)
			{
				base.WriteError(new ErrorRecord(exception, "GetContentWriterPathTooLongError", ErrorCategory.InvalidArgument, path));
			}
			catch (FileNotFoundException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "GetContentWriterFileNotFoundError", ErrorCategory.ObjectNotFound, path));
			}
			catch (DirectoryNotFoundException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "GetContentWriterDirectoryNotFoundError", ErrorCategory.ObjectNotFound, path));
			}
			catch (ArgumentException exception4)
			{
				base.WriteError(new ErrorRecord(exception4, "GetContentWriterArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception5)
			{
				base.WriteError(new ErrorRecord(exception5, "GetContentWriterIOError", ErrorCategory.WriteError, path));
			}
			catch (SecurityException exception6)
			{
				base.WriteError(new ErrorRecord(exception6, "GetContentWriterSecurityError", ErrorCategory.PermissionDenied, path));
			}
			catch (UnauthorizedAccessException exception7)
			{
				base.WriteError(new ErrorRecord(exception7, "GetContentWriterUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
			}
			return result;
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x00193C88 File Offset: 0x00191E88
		public object GetContentWriterDynamicParameters(string path)
		{
			return new FileSystemContentWriterDynamicParameters();
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x00193C90 File Offset: 0x00191E90
		public void ClearContent(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			try
			{
				bool flag = false;
				FileSystemClearContentDynamicParameters fileSystemClearContentDynamicParameters = null;
				string text = null;
				if (base.DynamicParameters != null)
				{
					fileSystemClearContentDynamicParameters = (base.DynamicParameters as FileSystemClearContentDynamicParameters);
					FileSystemContentWriterDynamicParameters fileSystemContentWriterDynamicParameters = base.DynamicParameters as FileSystemContentWriterDynamicParameters;
					if (fileSystemClearContentDynamicParameters != null)
					{
						if (fileSystemClearContentDynamicParameters.Stream != null && fileSystemClearContentDynamicParameters.Stream.Length > 0)
						{
							flag = true;
						}
						text = fileSystemClearContentDynamicParameters.Stream;
					}
					else if (fileSystemContentWriterDynamicParameters != null)
					{
						if (fileSystemContentWriterDynamicParameters.Stream != null && fileSystemContentWriterDynamicParameters.Stream.Length > 0)
						{
							flag = true;
						}
						text = fileSystemContentWriterDynamicParameters.Stream;
					}
					if (string.IsNullOrEmpty(text))
					{
						int num = path.IndexOf(':');
						int num2 = path.IndexOf(':', num + 1);
						if (num2 > 0)
						{
							text = path.Substring(num2 + 1);
							path = path.Remove(num2);
							flag = true;
						}
					}
				}
				if (string.Equals(":$DATA", text, StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
				}
				if (flag)
				{
					string target = string.Format(CultureInfo.InvariantCulture, FileSystemProviderStrings.StreamAction, new object[]
					{
						text,
						path
					});
					if (base.ShouldProcess(target))
					{
						FileStream fileStream;
						if (fileSystemClearContentDynamicParameters != null)
						{
							fileStream = AlternateDataStreamUtilities.CreateFileStream(path, text, FileMode.Open, FileAccess.Write, FileShare.Write);
							fileStream.Dispose();
						}
						fileStream = AlternateDataStreamUtilities.CreateFileStream(path, text, FileMode.Create, FileAccess.Write, FileShare.Write);
						fileStream.Dispose();
					}
				}
				else
				{
					string clearContentActionFile = FileSystemProviderStrings.ClearContentActionFile;
					string target2 = StringUtil.Format(FileSystemProviderStrings.ClearContentesourceTemplate, path);
					if (!base.ShouldProcess(target2, clearContentActionFile))
					{
						return;
					}
					FileStream fileStream2 = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
					fileStream2.Dispose();
				}
				base.WriteItemObject("", path, false);
			}
			catch (ArgumentException exception)
			{
				base.WriteError(new ErrorRecord(exception, "ClearContentArgumentError", ErrorCategory.InvalidArgument, path));
			}
			catch (IOException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "ClearContentIOError", ErrorCategory.WriteError, path));
			}
			catch (UnauthorizedAccessException exception3)
			{
				if (base.Force)
				{
					FileAttributes attributes = File.GetAttributes(path);
					try
					{
						try
						{
							File.SetAttributes(path, File.GetAttributes(path) & ~(FileAttributes.ReadOnly | FileAttributes.Hidden));
							FileStream fileStream3 = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
							fileStream3.Dispose();
							base.WriteItemObject("", path, false);
						}
						catch (UnauthorizedAccessException exception4)
						{
							base.WriteError(new ErrorRecord(exception4, "RemoveFileSystemItemUnAuthorizedAccess", ErrorCategory.PermissionDenied, path));
						}
						goto IL_238;
					}
					finally
					{
						File.SetAttributes(path, attributes);
					}
				}
				base.WriteError(new ErrorRecord(exception3, "ClearContentUnauthorizedAccessError", ErrorCategory.PermissionDenied, path));
				IL_238:;
			}
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x00193F54 File Offset: 0x00192154
		public object ClearContentDynamicParameters(string path)
		{
			return new FileSystemClearContentDynamicParameters();
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x00193F5C File Offset: 0x0019215C
		internal static int SafeGetFileAttributes(string path)
		{
			int fileAttributes = Utils.NativeMethods.GetFileAttributes(path);
			if (fileAttributes == -1)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 5)
				{
					Win32Exception ex = new Win32Exception(lastWin32Error);
					throw new UnauthorizedAccessException(ex.Message, ex);
				}
			}
			return fileAttributes;
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x00193F94 File Offset: 0x00192194
		private void ValidateParameters(bool isRawSpecified)
		{
			if (isRawSpecified)
			{
				if (base.Context.MyInvocation.BoundParameters.ContainsKey("TotalCount"))
				{
					string message = StringUtil.Format(FileSystemProviderStrings.NoFirstLastWaitForRaw, "Raw", "TotalCount");
					throw new PSInvalidOperationException(message);
				}
				if (base.Context.MyInvocation.BoundParameters.ContainsKey("Tail"))
				{
					string message2 = StringUtil.Format(FileSystemProviderStrings.NoFirstLastWaitForRaw, "Raw", "Tail");
					throw new PSInvalidOperationException(message2);
				}
				if (base.Context.MyInvocation.BoundParameters.ContainsKey("Wait"))
				{
					string message3 = StringUtil.Format(FileSystemProviderStrings.NoFirstLastWaitForRaw, "Raw", "Wait");
					throw new PSInvalidOperationException(message3);
				}
				if (base.Context.MyInvocation.BoundParameters.ContainsKey("Delimiter"))
				{
					string message4 = StringUtil.Format(FileSystemProviderStrings.NoFirstLastWaitForRaw, "Raw", "Delimiter");
					throw new PSInvalidOperationException(message4);
				}
			}
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x00194088 File Offset: 0x00192288
		public void GetSecurityDescriptor(string path, AccessControlSections sections)
		{
			ObjectSecurity securityDescriptor = null;
			path = FileSystemProvider.NormalizePath(path);
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if ((sections & ~(AccessControlSections.Audit | AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group)) != AccessControlSections.None)
			{
				throw PSTraceSource.NewArgumentException("sections");
			}
			PlatformInvokes.TOKEN_PRIVILEGE token_PRIVILEGE = default(PlatformInvokes.TOKEN_PRIVILEGE);
			try
			{
				PlatformInvokes.EnableTokenPrivilege("SeBackupPrivilege", ref token_PRIVILEGE);
				if (Directory.Exists(path))
				{
					securityDescriptor = new DirectorySecurity(path, sections);
				}
				else
				{
					securityDescriptor = new FileSecurity(path, sections);
				}
			}
			catch (SecurityException ex)
			{
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.PermissionDenied, path));
			}
			finally
			{
				PlatformInvokes.RestoreTokenPrivilege("SeBackupPrivilege", ref token_PRIVILEGE);
			}
			base.WriteSecurityDescriptorObject(securityDescriptor, path);
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00194148 File Offset: 0x00192348
		public void SetSecurityDescriptor(string path, ObjectSecurity securityDescriptor)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			path = FileSystemProvider.NormalizePath(path);
			if (securityDescriptor == null)
			{
				throw PSTraceSource.NewArgumentNullException("securityDescriptor");
			}
			if (!File.Exists(path) && !Directory.Exists(path))
			{
				base.ThrowTerminatingError(FileSystemProvider.CreateErrorRecord(path, "SetSecurityDescriptor_FileNotFound"));
			}
			FileSystemSecurity fileSystemSecurity = securityDescriptor as FileSystemSecurity;
			if (fileSystemSecurity == null)
			{
				throw PSTraceSource.NewArgumentException("securityDescriptor");
			}
			try
			{
				this.SetSecurityDescriptor(path, fileSystemSecurity, AccessControlSections.All);
			}
			catch (PrivilegeNotHeldException)
			{
				ObjectSecurity accessControl = new FileInfo(path).GetAccessControl();
				Type typeFromHandle = typeof(NTAccount);
				AccessControlSections accessControlSections = AccessControlSections.All;
				if (fileSystemSecurity.GetAuditRules(true, true, typeFromHandle).Count == 0 && fileSystemSecurity.AreAuditRulesProtected == accessControl.AreAccessRulesProtected)
				{
					accessControlSections &= ~AccessControlSections.Audit;
				}
				if (fileSystemSecurity.GetOwner(typeFromHandle) == accessControl.GetOwner(typeFromHandle))
				{
					accessControlSections &= ~AccessControlSections.Owner;
				}
				if (fileSystemSecurity.GetGroup(typeFromHandle) == accessControl.GetGroup(typeFromHandle))
				{
					accessControlSections &= ~AccessControlSections.Group;
				}
				this.SetSecurityDescriptor(path, fileSystemSecurity, accessControlSections);
			}
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x00194254 File Offset: 0x00192454
		private void SetSecurityDescriptor(string path, ObjectSecurity sd, AccessControlSections sections)
		{
			PlatformInvokes.TOKEN_PRIVILEGE token_PRIVILEGE = default(PlatformInvokes.TOKEN_PRIVILEGE);
			byte[] binaryForm = null;
			try
			{
				PlatformInvokes.EnableTokenPrivilege("SeBackupPrivilege", ref token_PRIVILEGE);
				binaryForm = sd.GetSecurityDescriptorBinaryForm();
			}
			finally
			{
				PlatformInvokes.RestoreTokenPrivilege("SeBackupPrivilege", ref token_PRIVILEGE);
			}
			try
			{
				PlatformInvokes.EnableTokenPrivilege("SeRestorePrivilege", ref token_PRIVILEGE);
				if (Directory.Exists(path))
				{
					DirectorySecurity directorySecurity = new DirectorySecurity();
					directorySecurity.SetSecurityDescriptorBinaryForm(binaryForm, sections);
					new DirectoryInfo(path).SetAccessControl(directorySecurity);
					base.WriteSecurityDescriptorObject(directorySecurity, path);
				}
				else
				{
					FileSecurity fileSecurity = new FileSecurity();
					fileSecurity.SetSecurityDescriptorBinaryForm(binaryForm, sections);
					new FileInfo(path).SetAccessControl(fileSecurity);
					base.WriteSecurityDescriptorObject(fileSecurity, path);
				}
			}
			finally
			{
				PlatformInvokes.RestoreTokenPrivilege("SeRestorePrivilege", ref token_PRIVILEGE);
			}
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x00194318 File Offset: 0x00192518
		public ObjectSecurity NewSecurityDescriptorFromPath(string path, AccessControlSections sections)
		{
			FileSystemProvider.ItemType itemType;
			if (this.IsItemContainer(path))
			{
				itemType = FileSystemProvider.ItemType.Directory;
			}
			else
			{
				itemType = FileSystemProvider.ItemType.File;
			}
			return FileSystemProvider.NewSecurityDescriptor(itemType);
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x0019433C File Offset: 0x0019253C
		public ObjectSecurity NewSecurityDescriptorOfType(string type, AccessControlSections sections)
		{
			FileSystemProvider.ItemType itemType = FileSystemProvider.GetItemType(type);
			return FileSystemProvider.NewSecurityDescriptor(itemType);
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x00194358 File Offset: 0x00192558
		private static ObjectSecurity NewSecurityDescriptor(FileSystemProvider.ItemType itemType)
		{
			ObjectSecurity result = null;
			switch (itemType)
			{
			case FileSystemProvider.ItemType.File:
				result = new FileSecurity();
				break;
			case FileSystemProvider.ItemType.Directory:
				result = new DirectorySecurity();
				break;
			}
			return result;
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x0019438C File Offset: 0x0019258C
		private static ErrorRecord CreateErrorRecord(string path, string errorId)
		{
			string message = StringUtil.Format(FileSystemProviderStrings.FileNotFound, path);
			return new ErrorRecord(new FileNotFoundException(message), errorId, ErrorCategory.ObjectNotFound, null);
		}

		// Token: 0x040024AA RID: 9386
		private const int FILETRANSFERSIZE = 4194304;

		// Token: 0x040024AB RID: 9387
		public const string ProviderName = "FileSystem";

		// Token: 0x040024AC RID: 9388
		[TraceSource("FileSystemProvider", "The namespace navigation provider for the file system")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("FileSystemProvider", "The namespace navigation provider for the file system");

		// Token: 0x0200076B RID: 1899
		private enum ItemType
		{
			// Token: 0x040024AE RID: 9390
			Unknown,
			// Token: 0x040024AF RID: 9391
			File,
			// Token: 0x040024B0 RID: 9392
			Directory,
			// Token: 0x040024B1 RID: 9393
			SymbolicLink,
			// Token: 0x040024B2 RID: 9394
			Junction,
			// Token: 0x040024B3 RID: 9395
			HardLink
		}

		// Token: 0x0200076C RID: 1900
		private static class NativeMethods
		{
			// Token: 0x06004C3E RID: 19518
			[DllImport("mpr.dll", CharSet = CharSet.Unicode)]
			internal static extern int WNetAddConnection2(ref FileSystemProvider.NetResource netResource, byte[] password, string username, int flags);

			// Token: 0x06004C3F RID: 19519
			[DllImport("mpr.dll", CharSet = CharSet.Unicode)]
			internal static extern int WNetCancelConnection2(string driveName, int flags, bool force);

			// Token: 0x06004C40 RID: 19520
			[DllImport("mpr.dll", CharSet = CharSet.Unicode)]
			internal static extern int WNetGetConnection(string localName, StringBuilder remoteName, ref int remoteNameLength);

			// Token: 0x06004C41 RID: 19521
			[DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool PathIsNetworkPath(string path);

			// Token: 0x06004C42 RID: 19522
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern int QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

			// Token: 0x06004C43 RID: 19523
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern int CreateSymbolicLink(string name, string destination, int destinationType);

			// Token: 0x06004C44 RID: 19524
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern bool CreateHardLink(string name, string existingFileName, IntPtr SecurityAttributes);

			// Token: 0x0200076D RID: 1901
			[Flags]
			internal enum FileAttributes
			{
				// Token: 0x040024B5 RID: 9397
				Hidden = 2,
				// Token: 0x040024B6 RID: 9398
				Directory = 16
			}
		}

		// Token: 0x0200076E RID: 1902
		private struct NetResource
		{
			// Token: 0x040024B7 RID: 9399
			public int Scope;

			// Token: 0x040024B8 RID: 9400
			public int Type;

			// Token: 0x040024B9 RID: 9401
			public int DisplayType;

			// Token: 0x040024BA RID: 9402
			public int Usage;

			// Token: 0x040024BB RID: 9403
			[MarshalAs(UnmanagedType.LPWStr)]
			public string LocalName;

			// Token: 0x040024BC RID: 9404
			[MarshalAs(UnmanagedType.LPWStr)]
			public string RemoteName;

			// Token: 0x040024BD RID: 9405
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Comment;

			// Token: 0x040024BE RID: 9406
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Provider;
		}
	}
}
