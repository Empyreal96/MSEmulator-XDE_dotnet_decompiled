using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Provider;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using Microsoft.PowerShell.Commands.Internal;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000474 RID: 1140
	[OutputType(new Type[]
	{
		typeof(RegistryKey)
	}, ProviderCmdlet = "New-Item")]
	[OutputType(new Type[]
	{
		typeof(RegistryKey),
		typeof(string)
	}, ProviderCmdlet = "Get-ChildItem")]
	[OutputType(new Type[]
	{
		typeof(RegistryKey)
	}, ProviderCmdlet = "Get-Item")]
	[OutputType(new Type[]
	{
		typeof(RegistryKey),
		typeof(string),
		typeof(int),
		typeof(long)
	}, ProviderCmdlet = "Get-ItemProperty")]
	[OutputType(new Type[]
	{
		typeof(string)
	}, ProviderCmdlet = "Move-ItemProperty")]
	[OutputType(new Type[]
	{
		typeof(RegistrySecurity)
	}, ProviderCmdlet = "Get-Acl")]
	[OutputType(new Type[]
	{
		typeof(RegistryKey)
	}, ProviderCmdlet = "Get-Item")]
	[CmdletProvider("Registry", ProviderCapabilities.ShouldProcess | ProviderCapabilities.Transactions)]
	[OutputType(new Type[]
	{
		typeof(RegistryKey)
	}, ProviderCmdlet = "Get-ChildItem")]
	public sealed class RegistryProvider : NavigationCmdletProvider, IDynamicPropertyCmdletProvider, IPropertyCmdletProvider, ISecurityDescriptorCmdletProvider
	{
		// Token: 0x060032A7 RID: 12967 RVA: 0x001140A8 File Offset: 0x001122A8
		protected override PSDriveInfo NewDrive(PSDriveInfo drive)
		{
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			if (!this.ItemExists(drive.Root))
			{
				Exception ex = new ArgumentException(RegistryProviderStrings.NewDriveRootDoesNotExist);
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, drive.Root));
			}
			return drive;
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x00114104 File Offset: 0x00112304
		protected override Collection<PSDriveInfo> InitializeDefaultDrives()
		{
			return new Collection<PSDriveInfo>
			{
				new PSDriveInfo("HKLM", base.ProviderInfo, "HKEY_LOCAL_MACHINE", RegistryProviderStrings.HKLMDriveDescription, null),
				new PSDriveInfo("HKCU", base.ProviderInfo, "HKEY_CURRENT_USER", RegistryProviderStrings.HKCUDriveDescription, null)
			};
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x0011415C File Offset: 0x0011235C
		protected override bool IsValidPath(string path)
		{
			bool flag = true;
			string text = this.NormalizePath(path);
			text = text.TrimStart(new char[]
			{
				'\\'
			});
			text = text.TrimEnd(new char[]
			{
				'\\'
			});
			int num = text.IndexOf('\\');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			if (string.IsNullOrEmpty(text))
			{
				flag = true;
			}
			else if (this.GetHiveRoot(text) == null)
			{
				flag = false;
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x001141F0 File Offset: 0x001123F0
		protected override void GetItem(string path)
		{
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, false);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			this.WriteRegistryItemObject(regkeyForPathWriteIfError, path);
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x00114214 File Offset: 0x00112414
		protected override void SetItem(string path, object value)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			string setItemAction = RegistryProviderStrings.SetItemAction;
			string setItemResourceTemplate = RegistryProviderStrings.SetItemResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, setItemResourceTemplate, new object[]
			{
				path,
				value
			});
			if (base.ShouldProcess(target, setItemAction))
			{
				string text = null;
				IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, true);
				if (regkeyForPathWriteIfError == null)
				{
					return;
				}
				bool flag = false;
				if (base.DynamicParameters != null)
				{
					RegistryProviderSetItemDynamicParameter registryProviderSetItemDynamicParameter = base.DynamicParameters as RegistryProviderSetItemDynamicParameter;
					if (registryProviderSetItemDynamicParameter != null)
					{
						try
						{
							RegistryValueKind type = registryProviderSetItemDynamicParameter.Type;
							regkeyForPathWriteIfError.SetValue(text, value, type);
							flag = true;
						}
						catch (ArgumentException ex)
						{
							base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, text));
							regkeyForPathWriteIfError.Close();
							return;
						}
						catch (IOException ex2)
						{
							base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.WriteError, path));
							regkeyForPathWriteIfError.Close();
							return;
						}
						catch (SecurityException ex3)
						{
							base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
							regkeyForPathWriteIfError.Close();
							return;
						}
						catch (UnauthorizedAccessException ex4)
						{
							base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.PermissionDenied, path));
							regkeyForPathWriteIfError.Close();
							return;
						}
					}
				}
				if (!flag)
				{
					try
					{
						regkeyForPathWriteIfError.SetValue(text, value);
					}
					catch (IOException ex5)
					{
						base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.WriteError, path));
						regkeyForPathWriteIfError.Close();
						return;
					}
					catch (SecurityException ex6)
					{
						base.WriteError(new ErrorRecord(ex6, ex6.GetType().FullName, ErrorCategory.PermissionDenied, path));
						regkeyForPathWriteIfError.Close();
						return;
					}
					catch (UnauthorizedAccessException ex7)
					{
						base.WriteError(new ErrorRecord(ex7, ex7.GetType().FullName, ErrorCategory.PermissionDenied, path));
						regkeyForPathWriteIfError.Close();
						return;
					}
				}
				object item = RegistryProvider.ReadExistingKeyValue(regkeyForPathWriteIfError, text);
				regkeyForPathWriteIfError.Close();
				base.WriteItemObject(item, path, false);
			}
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x00114468 File Offset: 0x00112668
		protected override object SetItemDynamicParameters(string path, object value)
		{
			return new RegistryProviderSetItemDynamicParameter();
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00114470 File Offset: 0x00112670
		protected override void ClearItem(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			string clearItemAction = RegistryProviderStrings.ClearItemAction;
			string clearItemResourceTemplate = RegistryProviderStrings.ClearItemResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, clearItemResourceTemplate, new object[]
			{
				path
			});
			if (base.ShouldProcess(target, clearItemAction))
			{
				IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, true);
				if (regkeyForPathWriteIfError == null)
				{
					return;
				}
				string[] array = new string[0];
				try
				{
					array = regkeyForPathWriteIfError.GetValueNames();
				}
				catch (IOException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.ReadError, path));
					return;
				}
				catch (SecurityException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
					return;
				}
				catch (UnauthorizedAccessException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
					return;
				}
				for (int i = 0; i < array.Length; i++)
				{
					try
					{
						regkeyForPathWriteIfError.DeleteValue(array[i]);
					}
					catch (IOException ex4)
					{
						base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.WriteError, path));
					}
					catch (SecurityException ex5)
					{
						base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.PermissionDenied, path));
					}
					catch (UnauthorizedAccessException ex6)
					{
						base.WriteError(new ErrorRecord(ex6, ex6.GetType().FullName, ErrorCategory.PermissionDenied, path));
					}
				}
				this.WriteRegistryItemObject(regkeyForPathWriteIfError, path);
			}
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x00114628 File Offset: 0x00112828
		protected override void GetChildItems(string path, bool recurse, uint depth)
		{
			RegistryProvider.tracer.WriteLine("recurse = {0}, depth = {1}", new object[]
			{
				recurse,
				depth
			});
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (this.IsHiveContainer(path))
			{
				foreach (string path2 in RegistryProvider.hiveNames)
				{
					if (base.Stopping)
					{
						return;
					}
					this.GetItem(path2);
				}
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, false);
			if (regkeyForPathWriteIfError != null)
			{
				try
				{
					string[] subKeyNames = regkeyForPathWriteIfError.GetSubKeyNames();
					regkeyForPathWriteIfError.Close();
					if (subKeyNames != null)
					{
						foreach (string text in subKeyNames)
						{
							if (base.Stopping)
							{
								return;
							}
							if (!string.IsNullOrEmpty(text))
							{
								string text2 = path;
								try
								{
									text2 = base.MakePath(path, text, true);
									if (!string.IsNullOrEmpty(text2))
									{
										IRegistryWrapper regkeyForPath = this.GetRegkeyForPath(text2, false);
										if (regkeyForPath != null)
										{
											this.WriteRegistryItemObject(regkeyForPath, text2);
										}
										if (recurse && depth > 0U)
										{
											this.GetChildItems(text2, recurse, depth - 1U);
										}
									}
								}
								catch (IOException ex)
								{
									base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.ReadError, text2));
								}
								catch (SecurityException ex2)
								{
									base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, text2));
								}
								catch (UnauthorizedAccessException ex3)
								{
									base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, text2));
								}
							}
						}
					}
				}
				catch (IOException ex4)
				{
					base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.ReadError, path));
				}
				catch (SecurityException ex5)
				{
					base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (UnauthorizedAccessException ex6)
				{
					base.WriteError(new ErrorRecord(ex6, ex6.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				return;
			}
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x00114858 File Offset: 0x00112A58
		protected override void GetChildNames(string path, ReturnContainers returnContainers)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				foreach (string text in RegistryProvider.hiveNames)
				{
					if (base.Stopping)
					{
						return;
					}
					base.WriteItemObject(text, text, true);
				}
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, false);
			if (regkeyForPathWriteIfError != null)
			{
				try
				{
					string[] subKeyNames = regkeyForPathWriteIfError.GetSubKeyNames();
					regkeyForPathWriteIfError.Close();
					for (int j = 0; j < subKeyNames.Length; j++)
					{
						if (base.Stopping)
						{
							return;
						}
						string text2 = RegistryProvider.EscapeChildName(subKeyNames[j]);
						string path2 = base.MakePath(path, text2, true);
						base.WriteItemObject(text2, path2, true);
					}
				}
				catch (IOException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.ReadError, path));
				}
				catch (SecurityException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (UnauthorizedAccessException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				return;
			}
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x0011498C File Offset: 0x00112B8C
		private static string EscapeSpecialChars(string path)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(path);
			while (textElementEnumerator.MoveNext())
			{
				string textElement = textElementEnumerator.GetTextElement();
				if (textElement.Contains(".*?[]:"))
				{
					stringBuilder.Append("`");
				}
				stringBuilder.Append(textElement);
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				stringBuilder
			});
			return stringBuilder.ToString();
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x001149FC File Offset: 0x00112BFC
		private static string EscapeChildName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(name);
			while (textElementEnumerator.MoveNext())
			{
				string textElement = textElementEnumerator.GetTextElement();
				if (textElement.Contains(".*?[]:"))
				{
					stringBuilder.Append("`");
				}
				stringBuilder.Append(textElement);
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				stringBuilder
			});
			return stringBuilder.ToString();
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x00114A6C File Offset: 0x00112C6C
		protected override void RenameItem(string path, string newName)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (string.IsNullOrEmpty(newName))
			{
				throw PSTraceSource.NewArgumentException("newName");
			}
			RegistryProvider.tracer.WriteLine("newName = {0}", new object[]
			{
				newName
			});
			string parentPath = this.GetParentPath(path, null);
			string text = this.MakePath(parentPath, newName);
			bool flag = this.ItemExists(text);
			if (flag)
			{
				Exception ex = new ArgumentException(RegistryProviderStrings.RenameItemAlreadyExists);
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, text));
				return;
			}
			string renameItemAction = RegistryProviderStrings.RenameItemAction;
			string renameItemResourceTemplate = RegistryProviderStrings.RenameItemResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, renameItemResourceTemplate, new object[]
			{
				path,
				text
			});
			if (base.ShouldProcess(target, renameItemAction))
			{
				this.MoveRegistryItem(path, text);
			}
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x00114B4C File Offset: 0x00112D4C
		protected override void NewItem(string path, string type, object newItem)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			string newItemAction = RegistryProviderStrings.NewItemAction;
			string newItemResourceTemplate = RegistryProviderStrings.NewItemResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, newItemResourceTemplate, new object[]
			{
				path
			});
			if (base.ShouldProcess(target, newItemAction))
			{
				IRegistryWrapper regkeyForPath = this.GetRegkeyForPath(path, false);
				if (regkeyForPath != null)
				{
					if (!base.Force)
					{
						Exception ex = new IOException(RegistryProviderStrings.KeyAlreadyExists);
						base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.ResourceExists, regkeyForPath));
						regkeyForPath.Close();
						return;
					}
					regkeyForPath.Close();
					this.RemoveItem(path, false);
				}
				if (base.Force && !this.CreateIntermediateKeys(path))
				{
					return;
				}
				string parentPath = this.GetParentPath(path, null);
				string childName = this.GetChildName(path);
				IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(parentPath, true);
				if (regkeyForPathWriteIfError == null)
				{
					return;
				}
				try
				{
					IRegistryWrapper registryWrapper = regkeyForPathWriteIfError.CreateSubKey(childName);
					regkeyForPathWriteIfError.Close();
					try
					{
						if (newItem != null)
						{
							RegistryValueKind kind;
							if (!this.ParseKind(type, out kind))
							{
								return;
							}
							this.SetRegistryValue(registryWrapper, string.Empty, newItem, kind, path, false);
						}
					}
					catch (Exception ex2)
					{
						if (!(ex2 is ArgumentException) && !(ex2 is InvalidCastException) && !(ex2 is IOException) && !(ex2 is SecurityException) && !(ex2 is UnauthorizedAccessException) && !(ex2 is NotSupportedException))
						{
							throw;
						}
						base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.WriteError, registryWrapper)
						{
							ErrorDetails = new ErrorDetails(StringUtil.Format(RegistryProviderStrings.KeyCreatedValueFailed, childName))
						});
					}
					this.WriteRegistryItemObject(registryWrapper, path);
				}
				catch (IOException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.WriteError, path));
				}
				catch (SecurityException ex4)
				{
					base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (UnauthorizedAccessException ex5)
				{
					base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (ArgumentException ex6)
				{
					base.WriteError(new ErrorRecord(ex6, ex6.GetType().FullName, ErrorCategory.InvalidArgument, path));
				}
				catch (NotSupportedException ex7)
				{
					base.WriteError(new ErrorRecord(ex7, ex7.GetType().FullName, ErrorCategory.InvalidOperation, path));
				}
			}
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x00114DE0 File Offset: 0x00112FE0
		protected override void RemoveItem(string path, bool recurse)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			RegistryProvider.tracer.WriteLine("recurse = {0}", new object[]
			{
				recurse
			});
			string parentPath = this.GetParentPath(path, null);
			string childName = this.GetChildName(path);
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(parentPath, true);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			string removeKeyAction = RegistryProviderStrings.RemoveKeyAction;
			string removeKeyResourceTemplate = RegistryProviderStrings.RemoveKeyResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, removeKeyResourceTemplate, new object[]
			{
				path
			});
			if (base.ShouldProcess(target, removeKeyAction))
			{
				try
				{
					regkeyForPathWriteIfError.DeleteSubKeyTree(childName);
				}
				catch (ArgumentException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, path));
				}
				catch (IOException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.WriteError, path));
				}
				catch (SecurityException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (UnauthorizedAccessException ex4)
				{
					base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (NotSupportedException ex5)
				{
					base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.InvalidOperation, path));
				}
			}
			regkeyForPathWriteIfError.Close();
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x00114F70 File Offset: 0x00113170
		protected override bool ItemExists(string path)
		{
			bool flag = false;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			try
			{
				if (this.IsHiveContainer(path))
				{
					flag = true;
				}
				else
				{
					IRegistryWrapper regkeyForPath = this.GetRegkeyForPath(path, false);
					if (regkeyForPath != null)
					{
						flag = true;
						regkeyForPath.Close();
					}
				}
			}
			catch (IOException)
			{
			}
			catch (SecurityException)
			{
				flag = true;
			}
			catch (UnauthorizedAccessException)
			{
				flag = true;
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x00115008 File Offset: 0x00113208
		protected override bool HasChildItems(string path)
		{
			bool flag = false;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			try
			{
				if (this.IsHiveContainer(path))
				{
					flag = (RegistryProvider.hiveNames.Length > 0);
				}
				else
				{
					IRegistryWrapper regkeyForPath = this.GetRegkeyForPath(path, false);
					if (regkeyForPath != null)
					{
						flag = (regkeyForPath.SubKeyCount > 0);
						regkeyForPath.Close();
					}
				}
			}
			catch (IOException)
			{
				flag = false;
			}
			catch (SecurityException)
			{
				flag = false;
			}
			catch (UnauthorizedAccessException)
			{
				flag = false;
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x001150B4 File Offset: 0x001132B4
		protected override void CopyItem(string path, string destination, bool recurse)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (string.IsNullOrEmpty(destination))
			{
				throw PSTraceSource.NewArgumentException("destination");
			}
			RegistryProvider.tracer.WriteLine("destination = {0}", new object[]
			{
				destination
			});
			RegistryProvider.tracer.WriteLine("recurse = {0}", new object[]
			{
				recurse
			});
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, false);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			try
			{
				this.CopyRegistryKey(regkeyForPathWriteIfError, path, destination, recurse, true, false);
			}
			catch (IOException ex)
			{
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, path));
			}
			catch (SecurityException ex2)
			{
				base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
			}
			catch (UnauthorizedAccessException ex3)
			{
				base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
			}
			regkeyForPathWriteIfError.Close();
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x001151CC File Offset: 0x001133CC
		private bool CopyRegistryKey(IRegistryWrapper key, string path, string destination, bool recurse, bool streamResult, bool streamFirstOnly)
		{
			bool result = true;
			if (recurse && this.ErrorIfDestinationIsSourceOrChildOfSource(path, destination))
			{
				return false;
			}
			RegistryProvider.tracer.WriteLine("destination = {0}", new object[]
			{
				destination
			});
			IRegistryWrapper registryWrapper = this.GetRegkeyForPath(destination, true);
			string childName = this.GetChildName(path);
			string text = destination;
			if (registryWrapper == null)
			{
				text = this.GetParentPath(destination, null);
				childName = this.GetChildName(destination);
				registryWrapper = this.GetRegkeyForPathWriteIfError(text, true);
			}
			if (registryWrapper == null)
			{
				return false;
			}
			string text2 = this.MakePath(text, childName);
			string copyKeyAction = RegistryProviderStrings.CopyKeyAction;
			string copyKeyResourceTemplate = RegistryProviderStrings.CopyKeyResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, copyKeyResourceTemplate, new object[]
			{
				path,
				destination
			});
			if (base.ShouldProcess(target, copyKeyAction))
			{
				IRegistryWrapper registryWrapper2 = null;
				try
				{
					registryWrapper2 = registryWrapper.CreateSubKey(childName);
				}
				catch (NotSupportedException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidOperation, childName));
				}
				if (registryWrapper2 != null)
				{
					string[] valueNames = key.GetValueNames();
					for (int i = 0; i < valueNames.Length; i++)
					{
						if (base.Stopping)
						{
							registryWrapper.Close();
							registryWrapper2.Close();
							return false;
						}
						registryWrapper2.SetValue(valueNames[i], key.GetValue(valueNames[i], null, RegistryValueOptions.DoNotExpandEnvironmentNames), key.GetValueKind(valueNames[i]));
					}
					if (streamResult)
					{
						this.WriteRegistryItemObject(registryWrapper2, text2);
						if (streamFirstOnly)
						{
							streamResult = false;
						}
					}
				}
			}
			registryWrapper.Close();
			if (recurse)
			{
				string[] subKeyNames = key.GetSubKeyNames();
				for (int j = 0; j < subKeyNames.Length; j++)
				{
					if (base.Stopping)
					{
						return false;
					}
					string path2 = this.MakePath(path, subKeyNames[j]);
					string destination2 = this.MakePath(text2, subKeyNames[j]);
					IRegistryWrapper regkeyForPath = this.GetRegkeyForPath(path2, false);
					bool flag = this.CopyRegistryKey(regkeyForPath, path2, destination2, recurse, streamResult, streamFirstOnly);
					regkeyForPath.Close();
					if (!flag)
					{
						result = flag;
					}
				}
			}
			return result;
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x001153B4 File Offset: 0x001135B4
		private bool ErrorIfDestinationIsSourceOrChildOfSource(string sourcePath, string destinationPath)
		{
			RegistryProvider.tracer.WriteLine("destinationPath = {0}", new object[]
			{
				destinationPath
			});
			bool flag = false;
			while (string.Compare(sourcePath, destinationPath, StringComparison.OrdinalIgnoreCase) != 0)
			{
				string parentPath = this.GetParentPath(destinationPath, null);
				if (string.IsNullOrEmpty(parentPath) || string.Compare(parentPath, destinationPath, StringComparison.OrdinalIgnoreCase) == 0)
				{
					IL_4B:
					if (flag)
					{
						Exception ex = new ArgumentException(RegistryProviderStrings.DestinationChildOfSource);
						base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, destinationPath));
					}
					RegistryProvider.tracer.WriteLine("result = {0}", new object[]
					{
						flag
					});
					return flag;
				}
				destinationPath = parentPath;
			}
			flag = true;
			goto IL_4B;
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x00115458 File Offset: 0x00113658
		protected override bool IsItemContainer(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			bool flag = false;
			if (this.IsHiveContainer(path))
			{
				flag = true;
			}
			else
			{
				try
				{
					IRegistryWrapper regkeyForPath = this.GetRegkeyForPath(path, false);
					if (regkeyForPath != null)
					{
						regkeyForPath.Close();
						flag = true;
					}
				}
				catch (IOException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.ReadError, path));
				}
				catch (SecurityException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (UnauthorizedAccessException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x00115540 File Offset: 0x00113740
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
			RegistryProvider.tracer.WriteLine("destination = {0}", new object[]
			{
				destination
			});
			string moveItemAction = RegistryProviderStrings.MoveItemAction;
			string moveItemResourceTemplate = RegistryProviderStrings.MoveItemResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, moveItemResourceTemplate, new object[]
			{
				path,
				destination
			});
			if (base.ShouldProcess(target, moveItemAction))
			{
				this.MoveRegistryItem(path, destination);
			}
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x001155D4 File Offset: 0x001137D4
		private void MoveRegistryItem(string path, string destination)
		{
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, false);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			bool flag = false;
			try
			{
				flag = this.CopyRegistryKey(regkeyForPathWriteIfError, path, destination, true, true, true);
			}
			catch (IOException ex)
			{
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, path));
				regkeyForPathWriteIfError.Close();
				return;
			}
			catch (SecurityException ex2)
			{
				base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
				regkeyForPathWriteIfError.Close();
				return;
			}
			catch (UnauthorizedAccessException ex3)
			{
				base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
				regkeyForPathWriteIfError.Close();
				return;
			}
			regkeyForPathWriteIfError.Close();
			string parentPath = this.GetParentPath(path, null);
			if (string.Equals(parentPath, destination, StringComparison.OrdinalIgnoreCase))
			{
				flag = false;
			}
			if (flag)
			{
				try
				{
					this.RemoveItem(path, true);
				}
				catch (IOException ex4)
				{
					base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.WriteError, path));
				}
				catch (SecurityException ex5)
				{
					base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (UnauthorizedAccessException ex6)
				{
					base.WriteError(new ErrorRecord(ex6, ex6.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
			}
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x0011574C File Offset: 0x0011394C
		public void GetProperty(string path, Collection<string> providerSpecificPickList)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!this.CheckOperationNotAllowedOnHiveContainer(path))
			{
				return;
			}
			IRegistryWrapper registryWrapper;
			Collection<string> collection;
			this.GetFilteredRegistryKeyProperties(path, providerSpecificPickList, true, false, out registryWrapper, out collection);
			if (registryWrapper == null)
			{
				return;
			}
			bool flag = false;
			PSObject psobject = new PSObject();
			foreach (string text in collection)
			{
				string name = text;
				if (string.IsNullOrEmpty(text))
				{
					name = this.GetLocalizedDefaultToken();
				}
				psobject.Properties.Add(new PSNoteProperty(name, registryWrapper.GetValue(text)));
				flag = true;
			}
			registryWrapper.Close();
			if (flag)
			{
				base.WritePropertyObject(psobject, path);
			}
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x00115808 File Offset: 0x00113A08
		public void SetProperty(string path, PSObject propertyValue)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!this.CheckOperationNotAllowedOnHiveContainer(path))
			{
				return;
			}
			if (propertyValue == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyValue");
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, true);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			RegistryValueKind kind = RegistryValueKind.Unknown;
			if (base.DynamicParameters != null)
			{
				RegistryProviderSetItemDynamicParameter registryProviderSetItemDynamicParameter = base.DynamicParameters as RegistryProviderSetItemDynamicParameter;
				if (registryProviderSetItemDynamicParameter != null)
				{
					kind = registryProviderSetItemDynamicParameter.Type;
				}
			}
			string setPropertyAction = RegistryProviderStrings.SetPropertyAction;
			string setPropertyResourceTemplate = RegistryProviderStrings.SetPropertyResourceTemplate;
			foreach (PSMemberInfo psmemberInfo in propertyValue.Properties)
			{
				object value = psmemberInfo.Value;
				string target = string.Format(base.Host.CurrentCulture, setPropertyResourceTemplate, new object[]
				{
					path,
					psmemberInfo.Name
				});
				if (base.ShouldProcess(target, setPropertyAction))
				{
					try
					{
						this.SetRegistryValue(regkeyForPathWriteIfError, psmemberInfo.Name, value, kind, path);
					}
					catch (InvalidCastException ex)
					{
						base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, path));
					}
					catch (IOException ex2)
					{
						base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.WriteError, psmemberInfo.Name));
					}
					catch (SecurityException ex3)
					{
						base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, psmemberInfo.Name));
					}
					catch (UnauthorizedAccessException ex4)
					{
						base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.PermissionDenied, psmemberInfo.Name));
					}
				}
			}
			regkeyForPathWriteIfError.Close();
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x00115A1C File Offset: 0x00113C1C
		public object SetPropertyDynamicParameters(string path, PSObject propertyValue)
		{
			return new RegistryProviderSetItemDynamicParameter();
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x00115A24 File Offset: 0x00113C24
		public void ClearProperty(string path, Collection<string> propertyToClear)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!this.CheckOperationNotAllowedOnHiveContainer(path))
			{
				return;
			}
			IRegistryWrapper registryWrapper;
			Collection<string> collection;
			this.GetFilteredRegistryKeyProperties(path, propertyToClear, false, true, out registryWrapper, out collection);
			if (registryWrapper == null)
			{
				return;
			}
			string clearPropertyAction = RegistryProviderStrings.ClearPropertyAction;
			string clearPropertyResourceTemplate = RegistryProviderStrings.ClearPropertyResourceTemplate;
			bool flag = false;
			PSObject psobject = new PSObject();
			foreach (string text in collection)
			{
				string target = string.Format(base.Host.CurrentCulture, clearPropertyResourceTemplate, new object[]
				{
					path,
					text
				});
				if (base.ShouldProcess(target, clearPropertyAction))
				{
					object value = this.ResetRegistryKeyValue(registryWrapper, text);
					string name = text;
					if (string.IsNullOrEmpty(text))
					{
						name = this.GetLocalizedDefaultToken();
					}
					psobject.Properties.Add(new PSNoteProperty(name, value));
					flag = true;
				}
			}
			registryWrapper.Close();
			if (flag)
			{
				base.WritePropertyObject(psobject, path);
			}
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x00115B2C File Offset: 0x00113D2C
		public object GetPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList)
		{
			return null;
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x00115B2F File Offset: 0x00113D2F
		public object ClearPropertyDynamicParameters(string path, Collection<string> propertyToClear)
		{
			return null;
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x00115B34 File Offset: 0x00113D34
		public void NewProperty(string path, string propertyName, string type, object value)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!this.CheckOperationNotAllowedOnHiveContainer(path))
			{
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, true);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			string newPropertyAction = RegistryProviderStrings.NewPropertyAction;
			string newPropertyResourceTemplate = RegistryProviderStrings.NewPropertyResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, newPropertyResourceTemplate, new object[]
			{
				path,
				propertyName
			});
			if (base.ShouldProcess(target, newPropertyAction))
			{
				RegistryValueKind kind;
				if (!this.ParseKind(type, out kind))
				{
					regkeyForPathWriteIfError.Close();
					return;
				}
				try
				{
					if (!base.Force && regkeyForPathWriteIfError.GetValue(propertyName) != null)
					{
						IOException ex = new IOException(RegistryProviderStrings.PropertyAlreadyExists);
						base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.ResourceExists, path));
						regkeyForPathWriteIfError.Close();
						return;
					}
					this.SetRegistryValue(regkeyForPathWriteIfError, propertyName, value, kind, path);
				}
				catch (ArgumentException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.WriteError, path));
				}
				catch (InvalidCastException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.WriteError, path));
				}
				catch (IOException ex4)
				{
					base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.WriteError, path));
				}
				catch (SecurityException ex5)
				{
					base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (UnauthorizedAccessException ex6)
				{
					base.WriteError(new ErrorRecord(ex6, ex6.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
			}
			regkeyForPathWriteIfError.Close();
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x00115D00 File Offset: 0x00113F00
		public void RemoveProperty(string path, string propertyName)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!this.CheckOperationNotAllowedOnHiveContainer(path))
			{
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, true);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			WildcardPattern wildcardPattern = new WildcardPattern(propertyName, WildcardOptions.IgnoreCase);
			bool hadAMatch = false;
			foreach (string text in regkeyForPathWriteIfError.GetValueNames())
			{
				if ((base.Context.SuppressWildcardExpansion || wildcardPattern.IsMatch(text)) && (!base.Context.SuppressWildcardExpansion || string.Equals(text, propertyName, StringComparison.OrdinalIgnoreCase)))
				{
					hadAMatch = true;
					string removePropertyAction = RegistryProviderStrings.RemovePropertyAction;
					string removePropertyResourceTemplate = RegistryProviderStrings.RemovePropertyResourceTemplate;
					string target = string.Format(base.Host.CurrentCulture, removePropertyResourceTemplate, new object[]
					{
						path,
						text
					});
					if (base.ShouldProcess(target, removePropertyAction))
					{
						string propertyName2 = this.GetPropertyName(text);
						try
						{
							regkeyForPathWriteIfError.DeleteValue(propertyName2);
						}
						catch (IOException ex)
						{
							base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, propertyName2));
						}
						catch (SecurityException ex2)
						{
							base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, propertyName2));
						}
						catch (UnauthorizedAccessException ex3)
						{
							base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, propertyName2));
						}
					}
				}
			}
			regkeyForPathWriteIfError.Close();
			this.WriteErrorIfPerfectMatchNotFound(hadAMatch, path, propertyName);
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x00115E88 File Offset: 0x00114088
		public void RenameProperty(string path, string sourceProperty, string destinationProperty)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!this.CheckOperationNotAllowedOnHiveContainer(path))
			{
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, true);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			string renamePropertyAction = RegistryProviderStrings.RenamePropertyAction;
			string renamePropertyResourceTemplate = RegistryProviderStrings.RenamePropertyResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, renamePropertyResourceTemplate, new object[]
			{
				path,
				sourceProperty,
				destinationProperty
			});
			if (base.ShouldProcess(target, renamePropertyAction))
			{
				try
				{
					this.MoveProperty(regkeyForPathWriteIfError, regkeyForPathWriteIfError, sourceProperty, destinationProperty);
				}
				catch (IOException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, path));
				}
				catch (SecurityException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
				catch (UnauthorizedAccessException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
				}
			}
			regkeyForPathWriteIfError.Close();
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x00115F98 File Offset: 0x00114198
		public void CopyProperty(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty)
		{
			if (sourcePath == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourcePath");
			}
			if (destinationPath == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationPath");
			}
			if (!this.CheckOperationNotAllowedOnHiveContainer(sourcePath, destinationPath))
			{
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(sourcePath, false);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError2 = this.GetRegkeyForPathWriteIfError(destinationPath, true);
			if (regkeyForPathWriteIfError2 == null)
			{
				return;
			}
			string copyPropertyAction = RegistryProviderStrings.CopyPropertyAction;
			string copyPropertyResourceTemplate = RegistryProviderStrings.CopyPropertyResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, copyPropertyResourceTemplate, new object[]
			{
				sourcePath,
				sourceProperty,
				destinationPath,
				destinationProperty
			});
			if (base.ShouldProcess(target, copyPropertyAction))
			{
				try
				{
					this.CopyProperty(regkeyForPathWriteIfError, regkeyForPathWriteIfError2, sourceProperty, destinationProperty, true);
				}
				catch (IOException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, sourcePath));
				}
				catch (SecurityException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, sourcePath));
				}
				catch (UnauthorizedAccessException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, sourcePath));
				}
			}
			regkeyForPathWriteIfError.Close();
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x001160CC File Offset: 0x001142CC
		public void MoveProperty(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty)
		{
			if (sourcePath == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourcePath");
			}
			if (destinationPath == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationPath");
			}
			if (!this.CheckOperationNotAllowedOnHiveContainer(sourcePath, destinationPath))
			{
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(sourcePath, true);
			if (regkeyForPathWriteIfError == null)
			{
				return;
			}
			IRegistryWrapper regkeyForPathWriteIfError2 = this.GetRegkeyForPathWriteIfError(destinationPath, true);
			if (regkeyForPathWriteIfError2 == null)
			{
				return;
			}
			string movePropertyAction = RegistryProviderStrings.MovePropertyAction;
			string movePropertyResourceTemplate = RegistryProviderStrings.MovePropertyResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, movePropertyResourceTemplate, new object[]
			{
				sourcePath,
				sourceProperty,
				destinationPath,
				destinationProperty
			});
			if (base.ShouldProcess(target, movePropertyAction))
			{
				try
				{
					this.MoveProperty(regkeyForPathWriteIfError, regkeyForPathWriteIfError2, sourceProperty, destinationProperty);
				}
				catch (IOException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, sourcePath));
				}
				catch (SecurityException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, sourcePath));
				}
				catch (UnauthorizedAccessException ex3)
				{
					base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, sourcePath));
				}
			}
			regkeyForPathWriteIfError.Close();
			regkeyForPathWriteIfError2.Close();
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x00116204 File Offset: 0x00114404
		protected override string GetParentPath(string path, string root)
		{
			string parentPath = base.GetParentPath(path, root);
			if (!string.Equals(parentPath, root, StringComparison.OrdinalIgnoreCase))
			{
				bool flag = this.ItemExists(path);
				bool flag2 = false;
				if (!flag)
				{
					flag2 = this.ItemExists(this.MakePath(root, path));
				}
				if (!string.IsNullOrEmpty(parentPath) && (flag || flag2))
				{
					do
					{
						string path2 = parentPath;
						if (flag2)
						{
							path2 = this.MakePath(root, parentPath);
						}
						if (this.ItemExists(path2))
						{
							break;
						}
						parentPath = base.GetParentPath(parentPath, root);
					}
					while (!string.IsNullOrEmpty(parentPath));
				}
			}
			return RegistryProvider.EnsureDriveIsRooted(parentPath);
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x00116280 File Offset: 0x00114480
		protected override string GetChildName(string path)
		{
			string childName = base.GetChildName(path);
			return childName.Replace('\\', '/');
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x001162A0 File Offset: 0x001144A0
		private static string EnsureDriveIsRooted(string path)
		{
			string text = path;
			int num = path.IndexOf(':');
			if (num != -1 && num + 1 == path.Length)
			{
				text = path + '\\';
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x001162F1 File Offset: 0x001144F1
		public object NewPropertyDynamicParameters(string path, string propertyName, string type, object value)
		{
			return null;
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x001162F4 File Offset: 0x001144F4
		public object RemovePropertyDynamicParameters(string path, string propertyName)
		{
			return null;
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x001162F7 File Offset: 0x001144F7
		public object RenamePropertyDynamicParameters(string path, string sourceProperty, string destinationProperty)
		{
			return null;
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x001162FA File Offset: 0x001144FA
		public object CopyPropertyDynamicParameters(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty)
		{
			return null;
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x001162FD File Offset: 0x001144FD
		public object MovePropertyDynamicParameters(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty)
		{
			return null;
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x00116300 File Offset: 0x00114500
		private void CopyProperty(IRegistryWrapper sourceKey, IRegistryWrapper destinationKey, string sourceProperty, string destinationProperty, bool writeOnSuccess)
		{
			string propertyName = this.GetPropertyName(sourceProperty);
			this.GetPropertyName(destinationProperty);
			object value = sourceKey.GetValue(sourceProperty);
			RegistryValueKind valueKind = sourceKey.GetValueKind(sourceProperty);
			destinationKey.SetValue(destinationProperty, value, valueKind);
			if (writeOnSuccess)
			{
				this.WriteWrappedPropertyObject(value, propertyName, sourceKey.Name);
			}
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x0011634C File Offset: 0x0011454C
		private void MoveProperty(IRegistryWrapper sourceKey, IRegistryWrapper destinationKey, string sourceProperty, string destinationProperty)
		{
			string propertyName = this.GetPropertyName(sourceProperty);
			string propertyName2 = this.GetPropertyName(destinationProperty);
			try
			{
				bool flag = true;
				if (string.Equals(sourceKey.Name, destinationKey.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(propertyName, propertyName2, StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
				}
				this.CopyProperty(sourceKey, destinationKey, propertyName, propertyName2, false);
				if (flag)
				{
					sourceKey.DeleteValue(propertyName);
				}
				object value = destinationKey.GetValue(propertyName2);
				this.WriteWrappedPropertyObject(value, destinationProperty, destinationKey.Name);
			}
			catch (IOException ex)
			{
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, sourceKey.Name));
			}
			catch (SecurityException ex2)
			{
				base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, sourceKey.Name));
			}
			catch (UnauthorizedAccessException ex3)
			{
				base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, sourceKey.Name));
			}
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x00116454 File Offset: 0x00114654
		private string NormalizePath(string path)
		{
			string text = path;
			if (!string.IsNullOrEmpty(path))
			{
				text = path.Replace('/', '\\');
				if (this.HasRelativePathTokens(path))
				{
					text = this.NormalizeRelativePath(text, null);
				}
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x001164A4 File Offset: 0x001146A4
		private bool HasRelativePathTokens(string path)
		{
			return path.IndexOf("\\", StringComparison.OrdinalIgnoreCase) == 0 || path.Contains("\\.\\") || path.Contains("\\..\\") || path.EndsWith("\\..", StringComparison.OrdinalIgnoreCase) || path.EndsWith("\\.", StringComparison.OrdinalIgnoreCase) || path.StartsWith("..\\", StringComparison.OrdinalIgnoreCase) || path.StartsWith(".\\", StringComparison.OrdinalIgnoreCase) || path.StartsWith("~", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x00116520 File Offset: 0x00114720
		private void GetFilteredRegistryKeyProperties(string path, Collection<string> propertyNames, bool getAll, bool writeAccess, out IRegistryWrapper key, out Collection<string> filteredCollection)
		{
			bool flag = false;
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			filteredCollection = new Collection<string>();
			key = this.GetRegkeyForPathWriteIfError(path, writeAccess);
			if (key == null)
			{
				return;
			}
			if (propertyNames == null)
			{
				propertyNames = new Collection<string>();
			}
			if (propertyNames.Count == 0 && getAll)
			{
				propertyNames.Add("*");
				flag = true;
			}
			string[] array = new string[0];
			try
			{
				array = key.GetValueNames();
			}
			catch (IOException ex)
			{
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.ReadError, path));
				return;
			}
			catch (SecurityException ex2)
			{
				base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
				return;
			}
			catch (UnauthorizedAccessException ex3)
			{
				base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
				return;
			}
			foreach (string text in propertyNames)
			{
				WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
				bool hadAMatch = false;
				foreach (string text2 in array)
				{
					string text3 = text2;
					if (string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text))
					{
						text3 = this.GetLocalizedDefaultToken();
					}
					if (flag || (!base.Context.SuppressWildcardExpansion && wildcardPattern.IsMatch(text3)) || (base.Context.SuppressWildcardExpansion && string.Equals(text3, text, StringComparison.OrdinalIgnoreCase)))
					{
						if (string.IsNullOrEmpty(text3))
						{
							text3 = this.GetLocalizedDefaultToken();
						}
						hadAMatch = true;
						filteredCollection.Add(text2);
					}
				}
				this.WriteErrorIfPerfectMatchNotFound(hadAMatch, path, text);
			}
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x00116700 File Offset: 0x00114900
		private void WriteErrorIfPerfectMatchNotFound(bool hadAMatch, string path, string requestedValueName)
		{
			if (!hadAMatch && !WildcardPattern.ContainsWildcardCharacters(requestedValueName))
			{
				string propertyNotAtPath = RegistryProviderStrings.PropertyNotAtPath;
				Exception ex = new PSArgumentException(string.Format(CultureInfo.CurrentCulture, propertyNotAtPath, new object[]
				{
					requestedValueName,
					path
				}), null);
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, requestedValueName));
			}
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x0011675C File Offset: 0x0011495C
		private object ResetRegistryKeyValue(IRegistryWrapper key, string valueName)
		{
			RegistryValueKind valueKind = key.GetValueKind(valueName);
			object obj = null;
			switch (valueKind)
			{
			case RegistryValueKind.Unknown:
			case RegistryValueKind.Binary:
				obj = new byte[0];
				break;
			case RegistryValueKind.String:
			case RegistryValueKind.ExpandString:
				obj = "";
				break;
			case RegistryValueKind.DWord:
				obj = 0;
				break;
			case RegistryValueKind.MultiString:
				obj = new string[0];
				break;
			case RegistryValueKind.QWord:
				obj = 0L;
				break;
			}
			try
			{
				key.SetValue(valueName, obj, valueKind);
			}
			catch (IOException ex)
			{
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.WriteError, valueName));
			}
			catch (SecurityException ex2)
			{
				base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, valueName));
			}
			catch (UnauthorizedAccessException ex3)
			{
				base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, valueName));
			}
			return obj;
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x00116868 File Offset: 0x00114A68
		private bool IsHiveContainer(string path)
		{
			bool flag = false;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (string.IsNullOrEmpty(path) || string.Compare(path, "\\", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(path, "/", StringComparison.OrdinalIgnoreCase) == 0)
			{
				flag = true;
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x001168CC File Offset: 0x00114ACC
		private bool CheckOperationNotAllowedOnHiveContainer(string path)
		{
			if (this.IsHiveContainer(path))
			{
				string containerInvalidOperationTemplate = RegistryProviderStrings.ContainerInvalidOperationTemplate;
				InvalidOperationException exception = new InvalidOperationException(containerInvalidOperationTemplate);
				base.WriteError(new ErrorRecord(exception, "InvalidContainer", ErrorCategory.InvalidArgument, path));
				return false;
			}
			return true;
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x00116908 File Offset: 0x00114B08
		private bool CheckOperationNotAllowedOnHiveContainer(string sourcePath, string destinationPath)
		{
			if (this.IsHiveContainer(sourcePath))
			{
				string sourceContainerInvalidOperationTemplate = RegistryProviderStrings.SourceContainerInvalidOperationTemplate;
				InvalidOperationException exception = new InvalidOperationException(sourceContainerInvalidOperationTemplate);
				base.WriteError(new ErrorRecord(exception, "InvalidContainer", ErrorCategory.InvalidArgument, sourcePath));
				return false;
			}
			if (this.IsHiveContainer(destinationPath))
			{
				string destinationContainerInvalidOperationTemplate = RegistryProviderStrings.DestinationContainerInvalidOperationTemplate;
				InvalidOperationException exception2 = new InvalidOperationException(destinationContainerInvalidOperationTemplate);
				base.WriteError(new ErrorRecord(exception2, "InvalidContainer", ErrorCategory.InvalidArgument, destinationPath));
				return false;
			}
			return true;
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x0011696C File Offset: 0x00114B6C
		private IRegistryWrapper GetHiveRoot(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (base.TransactionAvailable())
			{
				for (int i = 0; i < RegistryProvider.wellKnownHivesTx.Length; i++)
				{
					if (string.Equals(path, RegistryProvider.hiveNames[i], StringComparison.OrdinalIgnoreCase) || string.Equals(path, RegistryProvider.hiveShortNames[i], StringComparison.OrdinalIgnoreCase))
					{
						using (base.CurrentPSTransaction)
						{
							return new TransactedRegistryWrapper(RegistryProvider.wellKnownHivesTx[i], this);
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < RegistryProvider.wellKnownHives.Length; j++)
				{
					if (string.Equals(path, RegistryProvider.hiveNames[j], StringComparison.OrdinalIgnoreCase) || string.Equals(path, RegistryProvider.hiveShortNames[j], StringComparison.OrdinalIgnoreCase))
					{
						return new RegistryWrapper(RegistryProvider.wellKnownHives[j]);
					}
				}
			}
			return null;
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x00116A3C File Offset: 0x00114C3C
		private bool CreateIntermediateKeys(string path)
		{
			bool result = false;
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			try
			{
				path = this.NormalizePath(path);
				int num = path.IndexOf("\\", StringComparison.Ordinal);
				if (num == 0)
				{
					path = path.Substring(1);
					num = path.IndexOf("\\", StringComparison.Ordinal);
				}
				if (num == -1)
				{
					return true;
				}
				string path2 = path.Substring(0, num);
				string text = path.Substring(num + 1);
				IRegistryWrapper hiveRoot = this.GetHiveRoot(path2);
				if (text.Length == 0 || hiveRoot == null)
				{
					throw PSTraceSource.NewArgumentException("path");
				}
				IRegistryWrapper registryWrapper = hiveRoot.CreateSubKey(text);
				if (registryWrapper == null)
				{
					throw PSTraceSource.NewArgumentException("path");
				}
				registryWrapper.Close();
				result = true;
			}
			catch (ArgumentException ex)
			{
				base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.OpenError, path));
				return result;
			}
			catch (IOException ex2)
			{
				base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.OpenError, path));
				return result;
			}
			catch (SecurityException ex3)
			{
				base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.PermissionDenied, path));
				return result;
			}
			catch (UnauthorizedAccessException ex4)
			{
				base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.PermissionDenied, path));
				return result;
			}
			catch (NotSupportedException ex5)
			{
				base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.InvalidOperation, path));
			}
			return result;
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x00116BE8 File Offset: 0x00114DE8
		private IRegistryWrapper GetRegkeyForPathWriteIfError(string path, bool writeAccess)
		{
			IRegistryWrapper registryWrapper = null;
			try
			{
				registryWrapper = this.GetRegkeyForPath(path, writeAccess);
				if (registryWrapper == null)
				{
					ArgumentException ex = new ArgumentException(RegistryProviderStrings.KeyDoesNotExist);
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, path));
					return registryWrapper;
				}
			}
			catch (ArgumentException ex2)
			{
				base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.OpenError, path));
				return registryWrapper;
			}
			catch (IOException ex3)
			{
				base.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.OpenError, path));
				return registryWrapper;
			}
			catch (SecurityException ex4)
			{
				base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.PermissionDenied, path));
				return registryWrapper;
			}
			catch (UnauthorizedAccessException ex5)
			{
				base.WriteError(new ErrorRecord(ex5, ex5.GetType().FullName, ErrorCategory.PermissionDenied, path));
				return registryWrapper;
			}
			return registryWrapper;
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x00116CF0 File Offset: 0x00114EF0
		private IRegistryWrapper GetRegkeyForPath(string path, bool writeAccess)
		{
			if (string.IsNullOrEmpty(path))
			{
				ArgumentException ex = new ArgumentException(RegistryProviderStrings.KeyDoesNotExist);
				throw ex;
			}
			if (base.Stopping)
			{
				return null;
			}
			RegistryProvider.tracer.WriteLine("writeAccess = {0}", new object[]
			{
				writeAccess
			});
			IRegistryWrapper registryWrapper = null;
			int num = path.IndexOf("\\", StringComparison.Ordinal);
			if (num == 0)
			{
				path = path.Substring(1);
				num = path.IndexOf("\\", StringComparison.Ordinal);
			}
			if (num == -1)
			{
				registryWrapper = this.GetHiveRoot(path);
			}
			else
			{
				string path2 = path.Substring(0, num);
				string text = path.Substring(num + 1);
				IRegistryWrapper hiveRoot = this.GetHiveRoot(path2);
				if (text.Length == 0 || hiveRoot == null)
				{
					registryWrapper = hiveRoot;
				}
				else
				{
					try
					{
						registryWrapper = hiveRoot.OpenSubKey(text, writeAccess);
					}
					catch (NotSupportedException ex2)
					{
						base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.InvalidOperation, path));
					}
					if (registryWrapper == null)
					{
						IRegistryWrapper registryWrapper2 = hiveRoot;
						while (!string.IsNullOrEmpty(text))
						{
							bool flag = false;
							string[] subKeyNames = registryWrapper2.GetSubKeyNames();
							int i = 0;
							while (i < subKeyNames.Length)
							{
								string text2 = subKeyNames[i];
								string text3 = text2;
								if (!text.Equals(text2, StringComparison.OrdinalIgnoreCase) && !text.StartsWith(text2 + '\\', StringComparison.OrdinalIgnoreCase))
								{
									text3 = this.NormalizePath(text2);
									if (!text.Equals(text3, StringComparison.OrdinalIgnoreCase) && !text.StartsWith(text3 + '\\', StringComparison.OrdinalIgnoreCase))
									{
										i++;
										continue;
									}
								}
								IRegistryWrapper registryWrapper3 = registryWrapper2.OpenSubKey(text2, writeAccess);
								registryWrapper2.Close();
								registryWrapper2 = registryWrapper3;
								flag = true;
								text = (text.Equals(text3, StringComparison.OrdinalIgnoreCase) ? "" : text.Substring((text3 + '\\').Length));
								break;
							}
							if (!flag)
							{
								return null;
							}
						}
						return registryWrapper2;
					}
				}
			}
			return registryWrapper;
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x00116EDC File Offset: 0x001150DC
		private void SetRegistryValue(IRegistryWrapper key, string propertyName, object value, RegistryValueKind kind, string path)
		{
			this.SetRegistryValue(key, propertyName, value, kind, path, true);
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x00116EEC File Offset: 0x001150EC
		private void SetRegistryValue(IRegistryWrapper key, string propertyName, object value, RegistryValueKind kind, string path, bool writeResult)
		{
			string propertyName2 = this.GetPropertyName(propertyName);
			RegistryValueKind registryValueKind = RegistryValueKind.Unknown;
			if (kind == RegistryValueKind.Unknown)
			{
				registryValueKind = RegistryProvider.GetValueKindForProperty(key, propertyName2);
			}
			if (registryValueKind != RegistryValueKind.Unknown)
			{
				try
				{
					value = RegistryProvider.ConvertValueToKind(value, registryValueKind);
					kind = registryValueKind;
				}
				catch (InvalidCastException)
				{
					registryValueKind = RegistryValueKind.Unknown;
				}
			}
			if (registryValueKind == RegistryValueKind.Unknown)
			{
				if (kind == RegistryValueKind.Unknown)
				{
					if (value != null)
					{
						kind = RegistryProvider.GetValueKindFromObject(value);
					}
					else
					{
						kind = RegistryValueKind.String;
					}
				}
				value = RegistryProvider.ConvertValueToKind(value, kind);
			}
			key.SetValue(propertyName2, value, kind);
			if (writeResult)
			{
				object value2 = key.GetValue(propertyName2);
				this.WriteWrappedPropertyObject(value2, propertyName, path);
			}
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x00116F78 File Offset: 0x00115178
		private void WriteWrappedPropertyObject(object value, string propertyName, string path)
		{
			PSObject psobject = new PSObject();
			string name = propertyName;
			if (string.IsNullOrEmpty(propertyName))
			{
				name = this.GetLocalizedDefaultToken();
			}
			psobject.Properties.Add(new PSNoteProperty(name, value));
			base.WritePropertyObject(psobject, path);
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x00116FB8 File Offset: 0x001151B8
		private static object ConvertValueToKind(object value, RegistryValueKind kind)
		{
			switch (kind)
			{
			case RegistryValueKind.String:
				value = ((value != null) ? ((string)LanguagePrimitives.ConvertTo(value, typeof(string), CultureInfo.CurrentCulture)) : "");
				break;
			case RegistryValueKind.ExpandString:
				value = ((value != null) ? ((string)LanguagePrimitives.ConvertTo(value, typeof(string), CultureInfo.CurrentCulture)) : "");
				break;
			case RegistryValueKind.Binary:
				value = ((value != null) ? ((byte[])LanguagePrimitives.ConvertTo(value, typeof(byte[]), CultureInfo.CurrentCulture)) : new byte[0]);
				break;
			case RegistryValueKind.DWord:
				if (value != null)
				{
					try
					{
						value = (int)LanguagePrimitives.ConvertTo(value, typeof(int), CultureInfo.CurrentCulture);
						break;
					}
					catch (PSInvalidCastException)
					{
						value = (uint)LanguagePrimitives.ConvertTo(value, typeof(uint), CultureInfo.CurrentCulture);
						break;
					}
				}
				value = 0;
				break;
			case RegistryValueKind.MultiString:
				value = ((value != null) ? ((string[])LanguagePrimitives.ConvertTo(value, typeof(string[]), CultureInfo.CurrentCulture)) : new string[0]);
				break;
			case RegistryValueKind.QWord:
				if (value != null)
				{
					try
					{
						value = (long)LanguagePrimitives.ConvertTo(value, typeof(long), CultureInfo.CurrentCulture);
						break;
					}
					catch (PSInvalidCastException)
					{
						value = (ulong)LanguagePrimitives.ConvertTo(value, typeof(ulong), CultureInfo.CurrentCulture);
						break;
					}
				}
				value = 0;
				break;
			}
			return value;
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x00117174 File Offset: 0x00115374
		private static RegistryValueKind GetValueKindFromObject(object value)
		{
			if (value == null)
			{
				throw PSTraceSource.NewArgumentNullException("value");
			}
			RegistryValueKind result = RegistryValueKind.Unknown;
			Type type = value.GetType();
			if (type == typeof(byte[]))
			{
				result = RegistryValueKind.Binary;
			}
			else if (type == typeof(int))
			{
				result = RegistryValueKind.DWord;
			}
			if (type == typeof(string))
			{
				result = RegistryValueKind.String;
			}
			if (type == typeof(string[]))
			{
				result = RegistryValueKind.MultiString;
			}
			if (type == typeof(long))
			{
				result = RegistryValueKind.QWord;
			}
			return result;
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x00117200 File Offset: 0x00115400
		private static RegistryValueKind GetValueKindForProperty(IRegistryWrapper key, string valueName)
		{
			try
			{
				return key.GetValueKind(valueName);
			}
			catch (ArgumentException)
			{
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
			return RegistryValueKind.Unknown;
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x0011725C File Offset: 0x0011545C
		private static object ReadExistingKeyValue(IRegistryWrapper key, string valueName)
		{
			try
			{
				return key.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
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
			return null;
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x001172AC File Offset: 0x001154AC
		private void WriteRegistryItemObject(IRegistryWrapper key, string path)
		{
			if (key == null)
			{
				return;
			}
			path = RegistryProvider.EscapeSpecialChars(path);
			PSObject psobject = PSObject.AsPSObject(key.RegistryKey);
			string[] valueNames = key.GetValueNames();
			for (int i = 0; i < valueNames.Length; i++)
			{
				if (string.IsNullOrEmpty(valueNames[i]))
				{
					valueNames[i] = this.GetLocalizedDefaultToken();
					break;
				}
			}
			psobject.AddOrSetProperty("Property", valueNames);
			base.WriteItemObject(psobject, path, true);
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x00117310 File Offset: 0x00115510
		private bool ParseKind(string type, out RegistryValueKind kind)
		{
			kind = RegistryValueKind.Unknown;
			if (string.IsNullOrEmpty(type))
			{
				return true;
			}
			bool result = true;
			Exception ex = null;
			try
			{
				kind = (RegistryValueKind)Enum.Parse(typeof(RegistryValueKind), type, true);
			}
			catch (InvalidCastException ex2)
			{
				ex = ex2;
			}
			catch (ArgumentException ex3)
			{
				ex = ex3;
			}
			if (ex != null)
			{
				result = false;
				string typeParameterBindingFailure = RegistryProviderStrings.TypeParameterBindingFailure;
				Exception ex4 = new ArgumentException(string.Format(CultureInfo.CurrentCulture, typeParameterBindingFailure, new object[]
				{
					type,
					typeof(RegistryValueKind).FullName
				}), ex);
				base.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.InvalidArgument, type));
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				kind
			});
			return result;
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x001173F0 File Offset: 0x001155F0
		private string GetLocalizedDefaultToken()
		{
			return "(default)";
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x00117404 File Offset: 0x00115604
		private string GetPropertyName(string userEnteredPropertyName)
		{
			string text = userEnteredPropertyName;
			if (!string.IsNullOrEmpty(userEnteredPropertyName))
			{
				CompareInfo compareInfo = base.Host.CurrentCulture.CompareInfo;
				if (compareInfo.Compare(userEnteredPropertyName, this.GetLocalizedDefaultToken(), CompareOptions.IgnoreCase) == 0)
				{
					text = null;
				}
			}
			RegistryProvider.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x0011745C File Offset: 0x0011565C
		public void GetSecurityDescriptor(string path, AccessControlSections sections)
		{
			ObjectSecurity securityDescriptor = null;
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if ((sections & ~(AccessControlSections.Audit | AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group)) != AccessControlSections.None)
			{
				throw PSTraceSource.NewArgumentException("sections");
			}
			path = this.NormalizePath(path);
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, false);
			if (regkeyForPathWriteIfError != null)
			{
				try
				{
					securityDescriptor = regkeyForPathWriteIfError.GetAccessControl(sections);
				}
				catch (SecurityException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.PermissionDenied, path));
					return;
				}
				base.WriteSecurityDescriptorObject(securityDescriptor, path);
			}
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x001174E8 File Offset: 0x001156E8
		public void SetSecurityDescriptor(string path, ObjectSecurity securityDescriptor)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (securityDescriptor == null)
			{
				throw PSTraceSource.NewArgumentNullException("securityDescriptor");
			}
			path = this.NormalizePath(path);
			ObjectSecurity objectSecurity;
			if (base.TransactionAvailable())
			{
				objectSecurity = (securityDescriptor as TransactedRegistrySecurity);
				if (objectSecurity == null)
				{
					throw PSTraceSource.NewArgumentException("securityDescriptor");
				}
			}
			else
			{
				objectSecurity = (securityDescriptor as RegistrySecurity);
				if (objectSecurity == null)
				{
					throw PSTraceSource.NewArgumentException("securityDescriptor");
				}
			}
			IRegistryWrapper regkeyForPathWriteIfError = this.GetRegkeyForPathWriteIfError(path, true);
			if (regkeyForPathWriteIfError != null)
			{
				try
				{
					regkeyForPathWriteIfError.SetAccessControl(objectSecurity);
				}
				catch (SecurityException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.PermissionDenied, path));
					return;
				}
				catch (UnauthorizedAccessException ex2)
				{
					base.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
					return;
				}
				base.WriteSecurityDescriptorObject(objectSecurity, path);
			}
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x001175C8 File Offset: 0x001157C8
		public ObjectSecurity NewSecurityDescriptorFromPath(string path, AccessControlSections sections)
		{
			if (base.TransactionAvailable())
			{
				return new TransactedRegistrySecurity();
			}
			return new RegistrySecurity();
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x001175DD File Offset: 0x001157DD
		public ObjectSecurity NewSecurityDescriptorOfType(string type, AccessControlSections sections)
		{
			if (base.TransactionAvailable())
			{
				return new TransactedRegistrySecurity();
			}
			return new RegistrySecurity();
		}

		// Token: 0x04001A86 RID: 6790
		public const string ProviderName = "Registry";

		// Token: 0x04001A87 RID: 6791
		private const string charactersThatNeedEscaping = ".*?[]:";

		// Token: 0x04001A88 RID: 6792
		[TraceSource("RegistryProvider", "The namespace navigation provider for the Windows Registry")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("RegistryProvider", "The namespace navigation provider for the Windows Registry");

		// Token: 0x04001A89 RID: 6793
		private static readonly string[] hiveNames = new string[]
		{
			"HKEY_LOCAL_MACHINE",
			"HKEY_CURRENT_USER",
			"HKEY_CLASSES_ROOT",
			"HKEY_CURRENT_CONFIG",
			"HKEY_USERS",
			"HKEY_PERFORMANCE_DATA"
		};

		// Token: 0x04001A8A RID: 6794
		private static readonly string[] hiveShortNames = new string[]
		{
			"HKLM",
			"HKCU",
			"HKCR",
			"HKCC",
			"HKU",
			"HKPD"
		};

		// Token: 0x04001A8B RID: 6795
		private static readonly RegistryKey[] wellKnownHives = new RegistryKey[]
		{
			Registry.LocalMachine,
			Registry.CurrentUser,
			Registry.ClassesRoot,
			Registry.CurrentConfig,
			Registry.Users,
			Registry.PerformanceData
		};

		// Token: 0x04001A8C RID: 6796
		private static readonly TransactedRegistryKey[] wellKnownHivesTx = new TransactedRegistryKey[]
		{
			TransactedRegistry.LocalMachine,
			TransactedRegistry.CurrentUser,
			TransactedRegistry.ClassesRoot,
			TransactedRegistry.CurrentConfig,
			TransactedRegistry.Users
		};
	}
}
