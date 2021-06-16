using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000167 RID: 359
	internal sealed class Types_Ps1Xml
	{
		// Token: 0x0600126A RID: 4714 RVA: 0x00066DF0 File Offset: 0x00064FF0
		private static MethodInfo GetMethodInfo(string typeName, string method)
		{
			Type type = LanguagePrimitives.ConvertTo<Type>(typeName);
			return Types_Ps1Xml.GetMethodInfo(type, method);
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00066E0B File Offset: 0x0006500B
		private static MethodInfo GetMethodInfo(Type type, string method)
		{
			return type.GetMethod(method, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00066E18 File Offset: 0x00065018
		private static ScriptBlock GetScriptBlock(string s)
		{
			ScriptBlock scriptBlock = ScriptBlock.CreateDelayParsedScriptBlock(s);
			scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			return scriptBlock;
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00073508 File Offset: 0x00071708
		public static IEnumerable<TypeData> Get()
		{
			yield return new TypeData("System.Array", true)
			{
				Members = 
				{
					{
						"Count",
						new AliasPropertyData("Count", "Length")
					}
				}
			};
			yield return new TypeData("System.Xml.XmlNode", true)
			{
				Members = 
				{
					{
						"ToString",
						new CodeMethodData("ToString", Types_Ps1Xml.GetMethodInfo(typeof(ToStringCodeMethods), "XmlNode"))
					}
				}
			};
			yield return new TypeData("System.Xml.XmlNodeList", true)
			{
				Members = 
				{
					{
						"ToString",
						new CodeMethodData("ToString", Types_Ps1Xml.GetMethodInfo(typeof(ToStringCodeMethods), "XmlNodeList"))
					}
				}
			};
			yield return new TypeData("System.Management.Automation.PSDriveInfo", true)
			{
				Members = 
				{
					{
						"Used",
						new ScriptPropertyData("Used", Types_Ps1Xml.GetScriptBlock("## Ensure that this is a FileSystem drive\r\n          if($this.Provider.ImplementingType -eq\r\n          [Microsoft.PowerShell.Commands.FileSystemProvider])\r\n          {\r\n          $driveRoot = ([System.IO.DirectoryInfo] $this.Root).Name.Replace('\\','')\r\n          $drive = Get-WmiObject Win32_LogicalDisk -Filter \"DeviceId='$driveRoot'\"\r\n          $drive.Size - $drive.FreeSpace\r\n          }"), null)
					},
					{
						"Free",
						new ScriptPropertyData("Free", Types_Ps1Xml.GetScriptBlock("## Ensure that this is a FileSystem drive\r\n          if($this.Provider.ImplementingType -eq\r\n          [Microsoft.PowerShell.Commands.FileSystemProvider])\r\n          {\r\n          $driveRoot = ([System.IO.DirectoryInfo] $this.Root).Root.Name.Replace('\\','')\r\n          $drive = Get-WmiObject Win32_LogicalDisk -Filter \"DeviceId='$driveRoot'\"\r\n          $drive.FreeSpace\r\n          }"), null)
					}
				}
			};
			yield return new TypeData("System.DirectoryServices.PropertyValueCollection", true)
			{
				Members = 
				{
					{
						"ToString",
						new CodeMethodData("ToString", Types_Ps1Xml.GetMethodInfo(typeof(ToStringCodeMethods), "PropertyValueCollection"))
					}
				}
			};
			yield return new TypeData("System.Drawing.Printing.PrintDocument", true)
			{
				Members = 
				{
					{
						"Name",
						new ScriptPropertyData("Name", Types_Ps1Xml.GetScriptBlock("$this.PrinterSettings.PrinterName"), null)
					},
					{
						"Color",
						new ScriptPropertyData("Color", Types_Ps1Xml.GetScriptBlock("$this.PrinterSettings.SupportsColor"), null)
					},
					{
						"Duplex",
						new ScriptPropertyData("Duplex", Types_Ps1Xml.GetScriptBlock("$this.PrinterSettings.Duplex"), null)
					}
				}
			};
			yield return new TypeData("System.Management.Automation.ApplicationInfo", true)
			{
				Members = 
				{
					{
						"FileVersionInfo",
						new ScriptPropertyData("FileVersionInfo", Types_Ps1Xml.GetScriptBlock("[System.Diagnostics.FileVersionInfo]::getversioninfo( $this.Path )"), null)
					}
				}
			};
			yield return new TypeData("System.DateTime", true)
			{
				Members = 
				{
					{
						"DateTime",
						new ScriptPropertyData("DateTime", Types_Ps1Xml.GetScriptBlock("if ((& { Set-StrictMode -Version 1; $this.DisplayHint }) -ieq  \"Date\")\r\n          {\r\n          \"{0}\" -f $this.ToLongDateString()\r\n          }\r\n          elseif ((& { Set-StrictMode -Version 1; $this.DisplayHint }) -ieq \"Time\")\r\n          {\r\n          \"{0}\" -f  $this.ToLongTimeString()\r\n          }\r\n          else\r\n          {\r\n          \"{0} {1}\" -f $this.ToLongDateString(), $this.ToLongTimeString()\r\n          }"), null)
					}
				}
			};
			yield return new TypeData("System.Net.IPAddress", true)
			{
				Members = 
				{
					{
						"IPAddressToString",
						new ScriptPropertyData("IPAddressToString", Types_Ps1Xml.GetScriptBlock("$this.Tostring()"), null)
					}
				},
				DefaultDisplayProperty = "IPAddressToString",
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Net.IPAddress", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Diagnostics.ProcessModule", true)
			{
				Members = 
				{
					{
						"Size",
						new ScriptPropertyData("Size", Types_Ps1Xml.GetScriptBlock("$this.ModuleMemorySize / 1024"), null)
					},
					{
						"Company",
						new ScriptPropertyData("Company", Types_Ps1Xml.GetScriptBlock("$this.FileVersionInfo.CompanyName"), null)
					},
					{
						"FileVersion",
						new ScriptPropertyData("FileVersion", Types_Ps1Xml.GetScriptBlock("$this.FileVersionInfo.FileVersion"), null)
					},
					{
						"ProductVersion",
						new ScriptPropertyData("ProductVersion", Types_Ps1Xml.GetScriptBlock("$this.FileVersionInfo.ProductVersion"), null)
					},
					{
						"Description",
						new ScriptPropertyData("Description", Types_Ps1Xml.GetScriptBlock("$this.FileVersionInfo.FileDescription"), null)
					},
					{
						"Product",
						new ScriptPropertyData("Product", Types_Ps1Xml.GetScriptBlock("$this.FileVersionInfo.ProductName"), null)
					}
				}
			};
			yield return new TypeData("System.Collections.DictionaryEntry", true)
			{
				Members = 
				{
					{
						"Name",
						new AliasPropertyData("Name", "Key")
					}
				}
			};
			yield return new TypeData("System.Management.Automation.PSModuleInfo", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Name",
					"Path",
					"Description",
					"Guid",
					"Version",
					"ModuleBase",
					"ModuleType",
					"PrivateData",
					"AccessMode",
					"ExportedAliases",
					"ExportedCmdlets",
					"ExportedFunctions",
					"ExportedVariables",
					"NestedModules"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.ServiceProcess.ServiceController", true)
			{
				Members = 
				{
					{
						"Name",
						new AliasPropertyData("Name", "ServiceName")
					},
					{
						"RequiredServices",
						new AliasPropertyData("RequiredServices", "ServicesDependedOn")
					},
					{
						"ToString",
						new ScriptMethodData("ToString", Types_Ps1Xml.GetScriptBlock("$this.ServiceName"))
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Status",
					"Name",
					"DisplayName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Deserialized.System.ServiceProcess.ServiceController", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Status",
					"Name",
					"DisplayName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.Automation.CmdletInfo", true)
			{
				Members = 
				{
					{
						"DLL",
						new ScriptPropertyData("DLL", Types_Ps1Xml.GetScriptBlock("$this.ImplementingType.Assembly.Location"), null)
					}
				}
			};
			yield return new TypeData("System.Management.Automation.AliasInfo", true)
			{
				Members = 
				{
					{
						"ResolvedCommandName",
						new ScriptPropertyData("ResolvedCommandName", Types_Ps1Xml.GetScriptBlock("$this.ResolvedCommand.Name"), null)
					},
					{
						"DisplayName",
						new ScriptPropertyData("DisplayName", Types_Ps1Xml.GetScriptBlock("if ($this.Name.IndexOf('-') -lt 0)\r\n          {\r\n          if ($this.ResolvedCommand -ne $null)\r\n          {\r\n          $this.Name + \" -> \" + $this.ResolvedCommand.Name\r\n          }\r\n          else\r\n          {\r\n          $this.Name + \" -> \" + $this.Definition\r\n          }\r\n          }\r\n          else\r\n          {\r\n          $this.Name\r\n          }"), null)
					}
				}
			};
			yield return new TypeData("System.DirectoryServices.DirectoryEntry", true)
			{
				Members = 
				{
					{
						"ConvertLargeIntegerToInt64",
						new CodeMethodData("ConvertLargeIntegerToInt64", Types_Ps1Xml.GetMethodInfo(typeof(AdapterCodeMethods), "ConvertLargeIntegerToInt64"))
					},
					{
						"ConvertDNWithBinaryToString",
						new CodeMethodData("ConvertDNWithBinaryToString", Types_Ps1Xml.GetMethodInfo(typeof(AdapterCodeMethods), "ConvertDNWithBinaryToString"))
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"distinguishedName",
					"Path"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.IO.DirectoryInfo", true)
			{
				Members = 
				{
					{
						"Mode",
						new CodePropertyData("Mode", Types_Ps1Xml.GetMethodInfo(typeof(FileSystemProvider), "Mode"), null)
					},
					{
						"BaseName",
						new ScriptPropertyData("BaseName", Types_Ps1Xml.GetScriptBlock("$this.Name"), null)
					},
					{
						"Target",
						new CodePropertyData("Target", Types_Ps1Xml.GetMethodInfo(typeof(InternalSymbolicLinkLinkCodeMethods), "GetTarget"), null)
					},
					{
						"LinkType",
						new CodePropertyData("LinkType", Types_Ps1Xml.GetMethodInfo(typeof(InternalSymbolicLinkLinkCodeMethods), "GetLinkType"), null)
					}
				},
				DefaultDisplayProperty = "Name"
			};
			yield return new TypeData("System.IO.FileInfo", true)
			{
				Members = 
				{
					{
						"Mode",
						new CodePropertyData("Mode", Types_Ps1Xml.GetMethodInfo(typeof(FileSystemProvider), "Mode"), null)
					},
					{
						"VersionInfo",
						new ScriptPropertyData("VersionInfo", Types_Ps1Xml.GetScriptBlock("[System.Diagnostics.FileVersionInfo]::GetVersionInfo($this.FullName)"), null)
					},
					{
						"BaseName",
						new ScriptPropertyData("BaseName", Types_Ps1Xml.GetScriptBlock("if ($this.Extension.Length -gt 0){$this.Name.Remove($this.Name.Length - $this.Extension.Length)}else{$this.Name}"), null)
					},
					{
						"Target",
						new CodePropertyData("Target", Types_Ps1Xml.GetMethodInfo(typeof(InternalSymbolicLinkLinkCodeMethods), "GetTarget"), null)
					},
					{
						"LinkType",
						new CodePropertyData("LinkType", Types_Ps1Xml.GetMethodInfo(typeof(InternalSymbolicLinkLinkCodeMethods), "GetLinkType"), null)
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"LastWriteTime",
					"Length",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Diagnostics.FileVersionInfo", true)
			{
				Members = 
				{
					{
						"FileVersionRaw",
						new ScriptPropertyData("FileVersionRaw", Types_Ps1Xml.GetScriptBlock("New-Object System.Version -ArgumentList @(\r\n            $this.FileMajorPart\r\n            $this.FileMinorPart\r\n            $this.FileBuildPart\r\n            $this.FilePrivatePart)"), null)
					},
					{
						"ProductVersionRaw",
						new ScriptPropertyData("ProductVersionRaw", Types_Ps1Xml.GetScriptBlock("New-Object System.Version -ArgumentList @(\r\n            $this.ProductMajorPart\r\n            $this.ProductMinorPart\r\n            $this.ProductBuildPart\r\n            $this.ProductPrivatePart)"), null)
					}
				}
			};
			yield return new TypeData("System.Diagnostics.EventLogEntry", true)
			{
				Members = 
				{
					{
						"EventID",
						new ScriptPropertyData("EventID", Types_Ps1Xml.GetScriptBlock("$this.get_EventID() -band 0xFFFF"), null)
					}
				}
			};
			yield return new TypeData("System.Management.ManagementBaseObject", true)
			{
				Members = 
				{
					{
						"PSComputerName",
						new AliasPropertyData("PSComputerName", "__SERVER")
					}
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PingStatus", true)
			{
				Members = 
				{
					{
						"IPV4Address",
						new ScriptPropertyData("IPV4Address", Types_Ps1Xml.GetScriptBlock("$iphost = [System.Net.Dns]::GetHostEntry($this.address)\r\n          $iphost.AddressList | ?{ $_.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetwork } | select -first 1"), null)
					},
					{
						"IPV6Address",
						new ScriptPropertyData("IPV6Address", Types_Ps1Xml.GetScriptBlock("$iphost = [System.Net.Dns]::GetHostEntry($this.address)\r\n          $iphost.AddressList | ?{ $_.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetworkV6 } | select -first 1"), null)
					}
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Process", true)
			{
				Members = 
				{
					{
						"ProcessName",
						new AliasPropertyData("ProcessName", "Name")
					},
					{
						"Handles",
						new AliasPropertyData("Handles", "Handlecount")
					},
					{
						"VM",
						new AliasPropertyData("VM", "VirtualSize")
					},
					{
						"WS",
						new AliasPropertyData("WS", "WorkingSetSize")
					},
					{
						"Path",
						new ScriptPropertyData("Path", Types_Ps1Xml.GetScriptBlock("$this.ExecutablePath"), null)
					}
				}
			};
			yield return new TypeData("System.Diagnostics.Process", true)
			{
				Members = 
				{
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"Name",
							"Id",
							"PriorityClass",
							"FileVersion"
						})
						{
							Name = "PSConfiguration"
						}
					},
					{
						"PSResources",
						new PropertySetData(new string[]
						{
							"Name",
							"Id",
							"Handlecount",
							"WorkingSet",
							"NonPagedMemorySize",
							"PagedMemorySize",
							"PrivateMemorySize",
							"VirtualMemorySize",
							"Threads.Count",
							"TotalProcessorTime"
						})
						{
							Name = "PSResources"
						}
					},
					{
						"Name",
						new AliasPropertyData("Name", "ProcessName")
					},
					{
						"SI",
						new AliasPropertyData("SI", "SessionId")
					},
					{
						"Handles",
						new AliasPropertyData("Handles", "Handlecount")
					},
					{
						"VM",
						new AliasPropertyData("VM", "VirtualMemorySize64")
					},
					{
						"WS",
						new AliasPropertyData("WS", "WorkingSet64")
					},
					{
						"PM",
						new AliasPropertyData("PM", "PagedMemorySize64")
					},
					{
						"NPM",
						new AliasPropertyData("NPM", "NonpagedSystemMemorySize64")
					},
					{
						"Path",
						new ScriptPropertyData("Path", Types_Ps1Xml.GetScriptBlock("$this.Mainmodule.FileName"), null)
					},
					{
						"Company",
						new ScriptPropertyData("Company", Types_Ps1Xml.GetScriptBlock("$this.Mainmodule.FileVersionInfo.CompanyName"), null)
					},
					{
						"CPU",
						new ScriptPropertyData("CPU", Types_Ps1Xml.GetScriptBlock("$this.TotalProcessorTime.TotalSeconds"), null)
					},
					{
						"FileVersion",
						new ScriptPropertyData("FileVersion", Types_Ps1Xml.GetScriptBlock("$this.Mainmodule.FileVersionInfo.FileVersion"), null)
					},
					{
						"ProductVersion",
						new ScriptPropertyData("ProductVersion", Types_Ps1Xml.GetScriptBlock("$this.Mainmodule.FileVersionInfo.ProductVersion"), null)
					},
					{
						"Description",
						new ScriptPropertyData("Description", Types_Ps1Xml.GetScriptBlock("$this.Mainmodule.FileVersionInfo.FileDescription"), null)
					},
					{
						"Product",
						new ScriptPropertyData("Product", Types_Ps1Xml.GetScriptBlock("$this.Mainmodule.FileVersionInfo.ProductName"), null)
					},
					{
						"__NounName",
						new NotePropertyData("__NounName", "Process")
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Id",
					"Handles",
					"CPU",
					"SI",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Deserialized.System.Diagnostics.Process", true)
			{
				Members = 
				{
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"Name",
							"Id",
							"PriorityClass",
							"FileVersion"
						})
						{
							Name = "PSConfiguration"
						}
					},
					{
						"PSResources",
						new PropertySetData(new string[]
						{
							"Name",
							"Id",
							"Handlecount",
							"WorkingSet",
							"NonPagedMemorySize",
							"PagedMemorySize",
							"PrivateMemorySize",
							"VirtualMemorySize",
							"Threads.Count",
							"TotalProcessorTime"
						})
						{
							Name = "PSResources"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Id",
					"Handles",
					"CPU",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cli\\Msft_CliAlias", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"FriendlyName",
					"PWhere",
					"Target"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_BaseBoard", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"PoweredOn"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Manufacturer",
					"Model",
					"Name",
					"SerialNumber",
					"SKU",
					"Product"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_BIOS", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"Caption",
							"SMBIOSPresent"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"SMBIOSBIOSVersion",
					"Manufacturer",
					"Name",
					"SerialNumber",
					"Version"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_BootConfiguration", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"SettingID",
							"ConfigurationPath"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"BootDirectory",
					"Name",
					"SettingID",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_CDROMDrive", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Availability",
							"Drive",
							"ErrorCleared",
							"MediaLoaded",
							"NeedsCleaning",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Drive",
					"Manufacturer",
					"VolumeName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_ComputerSystem", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"AdminPasswordStatus",
							"BootupState",
							"ChassisBootupState",
							"KeyboardPasswordStatus",
							"PowerOnPasswordStatus",
							"PowerSupplyState",
							"PowerState",
							"FrontPanelResetStatus",
							"ThermalState",
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"POWER",
						new PropertySetData(new string[]
						{
							"Name",
							"PowerManagementCapabilities",
							"PowerManagementSupported",
							"PowerOnPasswordStatus",
							"PowerState",
							"PowerSupplyState"
						})
						{
							Name = "POWER"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Domain",
					"Manufacturer",
					"Model",
					"Name",
					"PrimaryOwnerName",
					"TotalPhysicalMemory"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\WIN32_PROCESSOR", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Availability",
							"CpuStatus",
							"CurrentVoltage",
							"DeviceID",
							"ErrorCleared",
							"ErrorDescription",
							"LastErrorCode",
							"LoadPercentage",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"AddressWidth",
							"DataWidth",
							"DeviceID",
							"ExtClock",
							"L2CacheSize",
							"L2CacheSpeed",
							"MaxClockSpeed",
							"PowerManagementSupported",
							"ProcessorType",
							"Revision",
							"SocketDesignation",
							"Version",
							"VoltageCaps"
						})
						{
							Name = "PSConfiguration"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"DeviceID",
					"Manufacturer",
					"MaxClockSpeed",
					"Name",
					"SocketDesignation"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_ComputerSystemProduct", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Version"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"IdentifyingNumber",
					"Name",
					"Vendor",
					"Version",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\CIM_DataFile", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Compressed",
					"Encrypted",
					"Size",
					"Hidden",
					"Name",
					"Readable",
					"System",
					"Version",
					"Writeable"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\WIN32_DCOMApplication", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Status"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"AppID",
					"InstallDate",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\WIN32_DESKTOP", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"ScreenSaverActive"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Name",
					"ScreenSaverActive",
					"ScreenSaverSecure",
					"ScreenSaverTimeout",
					"SettingID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\WIN32_DESKTOPMONITOR", true)
			{
				Members = 
				{
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"DeviceID",
							"Name",
							"PixelsPerXLogicalInch",
							"PixelsPerYLogicalInch",
							"ScreenHeight",
							"ScreenWidth"
						})
						{
							Name = "PSConfiguration"
						}
					},
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"DeviceID",
							"IsLocked",
							"LastErrorCode",
							"Name",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceID",
					"DisplayType",
					"MonitorManufacturer",
					"Name",
					"ScreenHeight",
					"ScreenWidth"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_DeviceMemoryAddress", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"MemoryType"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"MemoryType",
					"Name",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_DiskDrive", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"ConfigManagerErrorCode",
							"LastErrorCode",
							"NeedsCleaning",
							"Status",
							"DeviceID",
							"StatusInfo",
							"Partitions"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"BytesPerSector",
							"ConfigManagerUserConfig",
							"DefaultBlockSize",
							"DeviceID",
							"Index",
							"InstallDate",
							"InterfaceType",
							"MaxBlockSize",
							"MaxMediaSize",
							"MinBlockSize",
							"NumberOfMediaSupported",
							"Partitions",
							"SectorsPerTrack",
							"Size",
							"TotalCylinders",
							"TotalHeads",
							"TotalSectors",
							"TotalTracks",
							"TracksPerCylinder"
						})
						{
							Name = "PSConfiguration"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Partitions",
					"DeviceID",
					"Model",
					"Size",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_DiskQuota", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"__PATH",
							"Status"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DiskSpaceUsed",
					"Limit",
					"QuotaVolume",
					"User"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_DMAChannel", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"AddressSize",
					"DMAChannel",
					"MaxTransferSize",
					"Name",
					"Port"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Environment", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"SystemVariable"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"VariableValue",
					"Name",
					"UserName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Directory", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Compressed",
							"Encrypted",
							"Name",
							"Readable",
							"Writeable"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Hidden",
					"Archive",
					"EightDotThreeFileName",
					"FileSize",
					"Name",
					"Compressed",
					"Encrypted",
					"Readable"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Group", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Domain",
					"Name",
					"SID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_IDEController", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Manufacturer",
					"Name",
					"ProtocolSupported",
					"Status",
					"StatusInfo"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_IRQResource", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Caption",
							"Availability"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Hardware",
					"IRQNumber",
					"Name",
					"Shareable",
					"TriggerLevel",
					"TriggerType"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_ScheduledJob", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"JobId",
							"JobStatus",
							"ElapsedTime",
							"StartTime",
							"Owner"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"JobId",
					"Name",
					"Owner",
					"Priority",
					"Command"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_LoadOrderGroup", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"GroupOrder",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_LogicalDisk", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Availability",
							"DeviceID",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceID",
					"DriveType",
					"ProviderName",
					"FreeSpace",
					"Size",
					"VolumeName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_LogonSession", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"__PATH",
							"Status"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"AuthenticationPackage",
					"LogonId",
					"LogonType",
					"Name",
					"StartTime",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\WIN32_CACHEMEMORY", true)
			{
				Members = 
				{
					{
						"ERROR",
						new PropertySetData(new string[]
						{
							"DeviceID",
							"ErrorCorrectType"
						})
						{
							Name = "ERROR"
						}
					},
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Availability",
							"DeviceID",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"BlockSize",
							"CacheSpeed",
							"CacheType",
							"DeviceID",
							"InstalledSize",
							"Level",
							"MaxCacheSize",
							"NumberOfBlocks",
							"Status",
							"WritePolicy"
						})
						{
							Name = "PSConfiguration"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"BlockSize",
					"CacheSpeed",
					"CacheType",
					"DeviceID",
					"InstalledSize",
					"Level",
					"MaxCacheSize",
					"NumberOfBlocks",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_LogicalMemoryConfiguration", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"AvailableVirtualMemory",
							"Name",
							"TotalVirtualMemory"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Name",
					"TotalVirtualMemory",
					"TotalPhysicalMemory",
					"TotalPageFileSpace"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PhysicalMemoryArray", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"Replaceable",
							"Location"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Model",
					"Name",
					"MaxCapacity",
					"MemoryDevices"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\WIN32_NetworkClient", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Status"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"InstallDate",
					"Manufacturer",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_NetworkLoginProfile", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Privileges",
					"Profile",
					"UserId",
					"UserType",
					"Workstations"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_NetworkProtocol", true)
			{
				Members = 
				{
					{
						"FULLXXX",
						new PropertySetData(new string[]
						{
							"ConnectionlessService",
							"Description",
							"GuaranteesDelivery",
							"GuaranteesSequencing",
							"InstallDate",
							"MaximumAddressSize",
							"MaximumMessageSize",
							"MessageOriented",
							"MinimumAddressSize",
							"Name",
							"PseudoStreamOriented",
							"Status",
							"SupportsBroadcasting",
							"SupportsConnectData",
							"SupportsDisconnectData",
							"SupportsEncryption",
							"SupportsExpeditedData",
							"SupportsFragmentation",
							"SupportsGracefulClosing",
							"SupportsGuaranteedBandwidth",
							"SupportsMulticasting",
							"SupportsQualityofService"
						})
						{
							Name = "FULLXXX"
						}
					},
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Status",
							"SupportsBroadcasting",
							"SupportsConnectData",
							"SupportsDisconnectData",
							"SupportsEncryption",
							"SupportsExpeditedData",
							"SupportsFragmentation",
							"SupportsGracefulClosing",
							"SupportsGuaranteedBandwidth",
							"SupportsMulticasting",
							"SupportsQualityofService"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"GuaranteesDelivery",
					"GuaranteesSequencing",
					"ConnectionlessService",
					"Status",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_NetworkConnection", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"ConnectionState",
							"Persistent",
							"LocalName",
							"RemoteName"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"LocalName",
					"RemoteName",
					"ConnectionState",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_NetworkAdapter", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Availability",
							"Name",
							"Status",
							"StatusInfo",
							"DeviceID"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ServiceName",
					"MACAddress",
					"AdapterType",
					"DeviceID",
					"Name",
					"NetworkAddresses",
					"Speed"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_NetworkAdapterConfiguration", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"DHCPLeaseExpires",
							"Index",
							"Description"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"DHCP",
						new PropertySetData(new string[]
						{
							"Description",
							"DHCPEnabled",
							"DHCPLeaseExpires",
							"DHCPLeaseObtained",
							"DHCPServer",
							"Index"
						})
						{
							Name = "DHCP"
						}
					},
					{
						"DNS",
						new PropertySetData(new string[]
						{
							"Description",
							"DNSDomain",
							"DNSDomainSuffixSearchOrder",
							"DNSEnabledForWINSResolution",
							"DNSHostName",
							"DNSServerSearchOrder",
							"DomainDNSRegistrationEnabled",
							"FullDNSRegistrationEnabled",
							"Index"
						})
						{
							Name = "DNS"
						}
					},
					{
						"IP",
						new PropertySetData(new string[]
						{
							"Description",
							"Index",
							"IPAddress",
							"IPConnectionMetric",
							"IPEnabled",
							"IPFilterSecurityEnabled"
						})
						{
							Name = "IP"
						}
					},
					{
						"WINS",
						new PropertySetData(new string[]
						{
							"Description",
							"Index",
							"WINSEnableLMHostsLookup",
							"WINSHostLookupFile",
							"WINSPrimaryServer",
							"WINSScopeID",
							"WINSSecondaryServer"
						})
						{
							Name = "WINS"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DHCPEnabled",
					"IPAddress",
					"DefaultIPGateway",
					"DNSDomain",
					"ServiceName",
					"Description",
					"Index"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_NTDomain", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"DomainName"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"GUID",
						new PropertySetData(new string[]
						{
							"DomainName",
							"DomainGuid"
						})
						{
							Name = "GUID"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ClientSiteName",
					"DcSiteName",
					"Description",
					"DnsForestName",
					"DomainControllerAddress",
					"DomainControllerName",
					"DomainName",
					"Roles",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_NTLogEvent", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Category",
					"CategoryString",
					"EventCode",
					"EventIdentifier",
					"TypeEvent",
					"InsertionStrings",
					"LogFile",
					"Message",
					"RecordNumber",
					"SourceName",
					"TimeGenerated",
					"TimeWritten",
					"Type",
					"UserName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_NTEventlogFile", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"LogfileName",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"FileSize",
					"LogfileName",
					"Name",
					"NumberOfRecords"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_OnBoardDevice", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Description"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceType",
					"SerialNumber",
					"Enabled",
					"Description"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_OperatingSystem", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"FREE",
						new PropertySetData(new string[]
						{
							"FreePhysicalMemory",
							"FreeSpaceInPagingFiles",
							"FreeVirtualMemory",
							"Name"
						})
						{
							Name = "FREE"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"SystemDirectory",
					"Organization",
					"BuildNumber",
					"RegisteredUser",
					"SerialNumber",
					"Version"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PageFileUsage", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"CurrentUsage"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Name",
					"PeakUsage"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PageFileSetting", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"MaximumSize",
					"Name",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_DiskPartition", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Index",
							"Status",
							"StatusInfo",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"NumberOfBlocks",
					"BootPartition",
					"Name",
					"PrimaryPartition",
					"Size",
					"Index"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PortResource", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"NetConnectionStatus",
							"Status",
							"Name",
							"StartingAddress",
							"EndingAddress"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Name",
					"Alias"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PortConnector", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"ExternalReferenceDesignator"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Tag",
					"ConnectorType",
					"SerialNumber",
					"ExternalReferenceDesignator",
					"PortType"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Printer", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Location",
					"Name",
					"PrinterState",
					"PrinterStatus",
					"ShareName",
					"SystemName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PrinterConfiguration", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"DriverVersion",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"PrintQuality",
					"DriverVersion",
					"Name",
					"PaperSize",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PrintJob", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Document",
							"JobId",
							"JobStatus",
							"Name",
							"PagesPrinted",
							"Status",
							"JobIdCopy",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Document",
					"JobId",
					"JobStatus",
					"Owner",
					"Priority",
					"Size",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_ProcessXXX", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"ProcessId"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"MEMORY",
						new PropertySetData(new string[]
						{
							"Handle",
							"MaximumWorkingSetSize",
							"MinimumWorkingSetSize",
							"Name",
							"PageFaults",
							"PageFileUsage",
							"PeakPageFileUsage",
							"PeakVirtualSize",
							"PeakWorkingSetSize",
							"PrivatePageCount",
							"QuotaNonPagedPoolUsage",
							"QuotaPagedPoolUsage",
							"QuotaPeakNonPagedPoolUsage",
							"QuotaPeakPagedPoolUsage",
							"VirtualSize",
							"WorkingSetSize"
						})
						{
							Name = "MEMORY"
						}
					},
					{
						"IO",
						new PropertySetData(new string[]
						{
							"Name",
							"ProcessId",
							"ReadOperationCount",
							"ReadTransferCount",
							"WriteOperationCount",
							"WriteTransferCount"
						})
						{
							Name = "IO"
						}
					},
					{
						"STATISTICS",
						new PropertySetData(new string[]
						{
							"HandleCount",
							"Name",
							"KernelModeTime",
							"MaximumWorkingSetSize",
							"MinimumWorkingSetSize",
							"OtherOperationCount",
							"OtherTransferCount",
							"PageFaults",
							"PageFileUsage",
							"PeakPageFileUsage",
							"PeakVirtualSize",
							"PeakWorkingSetSize",
							"PrivatePageCount",
							"ProcessId",
							"QuotaNonPagedPoolUsage",
							"QuotaPagedPoolUsage",
							"QuotaPeakNonPagedPoolUsage",
							"QuotaPeakPagedPoolUsage",
							"ReadOperationCount",
							"ReadTransferCount",
							"ThreadCount",
							"UserModeTime",
							"VirtualSize",
							"WorkingSetSize",
							"WriteOperationCount",
							"WriteTransferCount"
						})
						{
							Name = "STATISTICS"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ThreadCount",
					"HandleCount",
					"Name",
					"Priority",
					"ProcessId",
					"WorkingSetSize"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Product", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Version",
							"InstallState"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"IdentifyingNumber",
					"Name",
					"Vendor",
					"Version",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_QuickFixEngineering", true)
			{
				Members = 
				{
					{
						"InstalledOn",
						new ScriptPropertyData("InstalledOn", Types_Ps1Xml.GetScriptBlock("if ([environment]::osversion.version.build -ge 7000)\r\n          {\r\n          # WMI team fixed the formatting issue related to InstalledOn\r\n          # property in Windows7 (to return string)..so returning the WMI's\r\n          # version directly\r\n          [DateTime]::Parse($this.psBase.properties[\"InstalledOn\"].Value)\r\n          }\r\n          else\r\n          {\r\n          $orig = $this.psBase.properties[\"InstalledOn\"].Value\r\n          $date = [datetime]::FromFileTimeUTC($(\"0x\" + $orig))\r\n          if ($date -lt \"1/1/1980\")\r\n          {\r\n          if ($orig -match \"([0-9]{4})([01][0-9])([012][0-9])\")\r\n          {\r\n          new-object datetime @([int]$matches[1], [int]$matches[2], [int]$matches[3])\r\n          }\r\n          }\r\n          else\r\n          {\r\n          $date\r\n          }\r\n          }"), null)
					},
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"__PATH",
							"Status"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Description",
					"FixComments",
					"HotFixID",
					"InstallDate",
					"InstalledBy",
					"InstalledOn",
					"Name",
					"ServicePackInEffect",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_QuotaSetting", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"State",
							"VolumePath",
							"Caption"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"DefaultLimit",
					"SettingID",
					"State",
					"VolumePath",
					"DefaultWarningLimit"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_OSRecoveryConfiguration", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DebugFilePath",
					"Name",
					"SettingID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Registry", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"CurrentSize",
							"MaximumSize",
							"ProposedSize"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"CurrentSize",
					"MaximumSize",
					"Name",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_SCSIController", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DriverName",
					"Manufacturer",
					"Name",
					"ProtocolSupported",
					"Status",
					"StatusInfo"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_PerfRawData_PerfNet_Server", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"LogonPerSec",
					"LogonTotal",
					"Name",
					"ServerSessions",
					"WorkItemShortages"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Service", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Status",
							"ExitCode"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"DesktopInteract",
							"ErrorControl",
							"Name",
							"PathName",
							"ServiceType",
							"StartMode"
						})
						{
							Name = "PSConfiguration"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ExitCode",
					"Name",
					"ProcessId",
					"StartMode",
					"State",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_Share", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Type",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Name",
					"Path",
					"Description"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_SoftwareElement", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"SoftwareElementState",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Name",
					"Path",
					"SerialNumber",
					"SoftwareElementID",
					"Version"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_SoftwareFeature", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"InstallState",
							"LastUse"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"IdentifyingNumber",
					"ProductName",
					"Vendor",
					"Version"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\WIN32_SoundDevice", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"ConfigManagerUserConfig",
							"Name",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Manufacturer",
					"Name",
					"Status",
					"StatusInfo"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_StartupCommand", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Command",
					"User",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_SystemAccount", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"SIDType",
							"Name",
							"Domain"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Domain",
					"Name",
					"SID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_SystemDriver", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"State",
							"ExitCode",
							"Started",
							"ServiceSpecificExitCode"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DisplayName",
					"Name",
					"State",
					"Status",
					"Started"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_SystemEnclosure", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Tag",
							"Status",
							"Name",
							"SecurityStatus"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Manufacturer",
					"Model",
					"LockPresent",
					"SerialNumber",
					"SMBIOSAssetTag",
					"SecurityStatus"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_SystemSlot", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"SlotDesignation"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"SlotDesignation",
					"Tag",
					"SupportsHotPlug",
					"Status",
					"Shared",
					"PMESignal",
					"MaxDataWidth"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_TapeDrive", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Availability",
							"DeviceID",
							"NeedsCleaning",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceID",
					"Id",
					"Manufacturer",
					"Name",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_TemperatureProbe", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"CurrentReading",
							"DeviceID",
							"Name",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"CurrentReading",
					"Name",
					"Description",
					"MinReadable",
					"MaxReadable",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_TimeZone", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Bias",
					"SettingID",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_UninterruptiblePowerSupply", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"DeviceID",
							"EstimatedChargeRemaining",
							"EstimatedRunTime",
							"Name",
							"StatusInfo",
							"TimeOnBackup"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceID",
					"EstimatedRunTime",
					"Name",
					"TimeOnBackup",
					"UPSPort",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_UserAccount", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Caption",
							"PasswordExpires"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"AccountType",
					"Caption",
					"Domain",
					"SID",
					"FullName",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_VoltageProbe", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"DeviceID",
							"Name",
							"NominalReading",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Status",
					"Description",
					"CurrentReading",
					"MaxReadable",
					"MinReadable"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_VolumeQuotaSetting", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Element",
					"Setting"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject#root\\cimv2\\Win32_WMISetting", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"BuildVersion",
					"Caption",
					"DatabaseDirectory",
					"EnableEvents",
					"LoggingLevel",
					"SettingID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementObject", true)
			{
				Members = 
				{
					{
						"ConvertToDateTime",
						new ScriptMethodData("ConvertToDateTime", Types_Ps1Xml.GetScriptBlock("[System.Management.ManagementDateTimeConverter]::ToDateTime($args[0])"))
					},
					{
						"ConvertFromDateTime",
						new ScriptMethodData("ConvertFromDateTime", Types_Ps1Xml.GetScriptBlock("[System.Management.ManagementDateTimeConverter]::ToDmtfDateTime($args[0])"))
					}
				}
			};
			yield return new TypeData("System.Security.AccessControl.ObjectSecurity", true)
			{
				Members = 
				{
					{
						"Path",
						new CodePropertyData("Path", Types_Ps1Xml.GetMethodInfo("Microsoft.PowerShell.Commands.SecurityDescriptorCommandsBase", "GetPath"), null)
					},
					{
						"Owner",
						new CodePropertyData("Owner", Types_Ps1Xml.GetMethodInfo("Microsoft.PowerShell.Commands.SecurityDescriptorCommandsBase", "GetOwner"), null)
					},
					{
						"Group",
						new CodePropertyData("Group", Types_Ps1Xml.GetMethodInfo("Microsoft.PowerShell.Commands.SecurityDescriptorCommandsBase", "GetGroup"), null)
					},
					{
						"Access",
						new CodePropertyData("Access", Types_Ps1Xml.GetMethodInfo("Microsoft.PowerShell.Commands.SecurityDescriptorCommandsBase", "GetAccess"), null)
					},
					{
						"Sddl",
						new CodePropertyData("Sddl", Types_Ps1Xml.GetMethodInfo("Microsoft.PowerShell.Commands.SecurityDescriptorCommandsBase", "GetSddl"), null)
					},
					{
						"AccessToString",
						new ScriptPropertyData("AccessToString", Types_Ps1Xml.GetScriptBlock("$toString = \"\";\r\n          $first = $true;\r\n          if ( ! $this.Access ) { return \"\" }\r\n          foreach($ace in $this.Access)\r\n          {\r\n          if($first)\r\n          {\r\n          $first = $false;\r\n          }\r\n          else\r\n          {\r\n          $tostring += \"`n\";\r\n          }\r\n          $toString += $ace.IdentityReference.ToString();\r\n          $toString += \" \";\r\n          $toString += $ace.AccessControlType.ToString();\r\n          $toString += \"  \";\r\n          if($ace -is [System.Security.AccessControl.FileSystemAccessRule])\r\n          {\r\n          $toString += $ace.FileSystemRights.ToString();\r\n          }\r\n          elseif($ace -is  [System.Security.AccessControl.RegistryAccessRule])\r\n          {\r\n          $toString += $ace.RegistryRights.ToString();\r\n          }\r\n          }\r\n          return $toString;"), null)
					},
					{
						"AuditToString",
						new ScriptPropertyData("AuditToString", Types_Ps1Xml.GetScriptBlock("$toString = \"\";\r\n          $first = $true;\r\n          if ( ! (& { Set-StrictMode -Version 1; $this.audit }) ) { return \"\" }\r\n          foreach($ace in (& { Set-StrictMode -Version 1; $this.audit }))\r\n          {\r\n          if($first)\r\n          {\r\n          $first = $false;\r\n          }\r\n          else\r\n          {\r\n          $tostring += \"`n\";\r\n          }\r\n          $toString += $ace.IdentityReference.ToString();\r\n          $toString += \" \";\r\n          $toString += $ace.AuditFlags.ToString();\r\n          $toString += \"  \";\r\n          if($ace -is [System.Security.AccessControl.FileSystemAuditRule])\r\n          {\r\n          $toString += $ace.FileSystemRights.ToString();\r\n          }\r\n          elseif($ace -is [System.Security.AccessControl.RegistryAuditRule])\r\n          {\r\n          $toString += $ace.RegistryRights.ToString();\r\n          }\r\n          }\r\n          return $toString;"), null)
					}
				}
			};
			yield return new TypeData("Microsoft.PowerShell.Commands.HistoryInfo", true)
			{
				DefaultKeyPropertySet = new PropertySetData(new string[]
				{
					"Id"
				})
				{
					Name = "DefaultKeyPropertySet"
				}
			};
			yield return new TypeData("System.Management.ManagementClass", true)
			{
				Members = 
				{
					{
						"Name",
						new AliasPropertyData("Name", "__Class")
					}
				}
			};
			yield return new TypeData("System.Management.Automation.Runspaces.PSSession", true)
			{
				Members = 
				{
					{
						"State",
						new ScriptPropertyData("State", Types_Ps1Xml.GetScriptBlock("$this.Runspace.RunspaceStateInfo.State"), null)
					},
					{
						"IdleTimeout",
						new ScriptPropertyData("IdleTimeout", Types_Ps1Xml.GetScriptBlock("$this.Runspace.ConnectionInfo.IdleTimeout"), null)
					},
					{
						"OutputBufferingMode",
						new ScriptPropertyData("OutputBufferingMode", Types_Ps1Xml.GetScriptBlock("$this.Runspace.ConnectionInfo.OutputBufferingMode"), null)
					},
					{
						"DisconnectedOn",
						new ScriptPropertyData("DisconnectedOn", Types_Ps1Xml.GetScriptBlock("$this.Runspace.DisconnectedOn"), null)
					},
					{
						"ExpiresOn",
						new ScriptPropertyData("ExpiresOn", Types_Ps1Xml.GetScriptBlock("$this.Runspace.ExpiresOn"), null)
					}
				}
			};
			yield return new TypeData("System.Guid", true)
			{
				Members = 
				{
					{
						"Guid",
						new ScriptPropertyData("Guid", Types_Ps1Xml.GetScriptBlock("$this.ToString()"), null)
					}
				}
			};
			yield return new TypeData("System.Management.Automation.Signature", true)
			{
				SerializationDepth = 2U
			};
			yield return new TypeData("System.Management.Automation.Job", true)
			{
				Members = 
				{
					{
						"State",
						new ScriptPropertyData("State", Types_Ps1Xml.GetScriptBlock("$this.JobStateInfo.State.ToString()"), null)
					}
				},
				SerializationMethod = "SpecificProperties",
				SerializationDepth = 2U,
				PropertySerializationSet = new PropertySetData(new string[]
				{
					"HasMoreData",
					"StatusMessage",
					"Location",
					"Command",
					"JobStateInfo",
					"InstanceId",
					"Id",
					"Name",
					"State",
					"ChildJobs",
					"PSJobTypeName",
					"PSBeginTime",
					"PSEndTime"
				})
				{
					Name = "PropertySerializationSet"
				}
			};
			yield return new TypeData("System.Management.Automation.JobStateInfo", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.JobStateInfo", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("Microsoft.PowerShell.DeserializingTypeConverter", true)
			{
				TypeConverter = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Net.Mail.MailAddress", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Net.Mail.MailAddress", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Globalization.CultureInfo", true)
			{
				SerializationMethod = "SpecificProperties",
				SerializationDepth = 1U,
				PropertySerializationSet = new PropertySetData(new string[]
				{
					"LCID",
					"Name",
					"DisplayName",
					"IetfLanguageTag",
					"ThreeLetterISOLanguageName",
					"ThreeLetterWindowsLanguageName",
					"TwoLetterISOLanguageName"
				})
				{
					Name = "PropertySerializationSet"
				}
			};
			yield return new TypeData("Deserialized.System.Globalization.CultureInfo", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.PSCredential", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.PSCredential", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.PSPrimitiveDictionary", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.PSPrimitiveDictionary", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.SwitchParameter", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.SwitchParameter", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.PSListModifier", true)
			{
				SerializationDepth = 2U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.PSListModifier", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Security.Cryptography.X509Certificates.X509Certificate2", true)
			{
				SerializationMethod = "SpecificProperties",
				SerializationDepth = 1U,
				PropertySerializationSet = new PropertySetData(new string[]
				{
					"RawData"
				})
				{
					Name = "PropertySerializationSet"
				}
			};
			yield return new TypeData("Deserialized.System.Security.Cryptography.X509Certificates.X509Certificate2", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Security.Cryptography.X509Certificates.X500DistinguishedName", true)
			{
				SerializationMethod = "SpecificProperties",
				SerializationDepth = 1U,
				PropertySerializationSet = new PropertySetData(new string[]
				{
					"RawData"
				})
				{
					Name = "PropertySerializationSet"
				}
			};
			yield return new TypeData("Deserialized.System.Security.Cryptography.X509Certificates.X500DistinguishedName", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Security.AccessControl.RegistrySecurity", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Security.AccessControl.RegistrySecurity", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Security.AccessControl.FileSystemSecurity", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Security.AccessControl.FileSystemSecurity", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("HelpInfo", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("System.Management.Automation.PSTypeName", true)
			{
				SerializationMethod = "String",
				StringSerializationSource = "Name"
			};
			yield return new TypeData("System.Management.Automation.ParameterMetadata", true)
			{
				SerializationMethod = "SpecificProperties",
				PropertySerializationSet = new PropertySetData(new string[]
				{
					"Name",
					"ParameterType",
					"Aliases",
					"IsDynamic",
					"SwitchParameter"
				})
				{
					Name = "PropertySerializationSet"
				}
			};
			yield return new TypeData("System.Management.Automation.CommandInfo", true)
			{
				Members = 
				{
					{
						"Namespace",
						new AliasPropertyData("Namespace", "ModuleName")
						{
							IsHidden = true
						}
					},
					{
						"HelpUri",
						new ScriptPropertyData("HelpUri", Types_Ps1Xml.GetScriptBlock("$oldProgressPreference = $ProgressPreference\r\n          $ProgressPreference = 'SilentlyContinue'\r\n          try\r\n          {\r\n          if ($psversiontable.psversion.Major -lt 3)\r\n          {\r\n          # ok to cast CommandTypes enum to HelpCategory because string/indentifier for\r\n          # cmdlet,function,filter,alias,externalscript is identical.\r\n          # it is ok to fail for other enum values (i.e. for Application)\r\n          $commandName = $this.Name\r\n          if ($this.ModuleName)\r\n          {\r\n          $commandName = \"{0}\\{1}\" -f $this.ModuleName,$commandName\r\n          }\r\n\r\n          $helpObject = get-help -Name $commandName -Category ([string]($this.CommandType)) -ErrorAction SilentlyContinue\r\n\r\n          # return first non-null uri (and try not to hit any strict mode things)\r\n          if ($helpObject -eq $null) { return $null }\r\n          if ($helpObject.psobject.properties['relatedLinks'] -eq $null) { return $null }\r\n          if ($helpObject.relatedLinks.psobject.properties['navigationLink'] -eq $null) { return $null }\r\n          $helpUri = [string]$( $helpObject.relatedLinks.navigationLink | %{ if ($_.psobject.properties['uri'] -ne $null) { $_.uri } } | ?{ $_ } | select -first 1 )\r\n          return $helpUri\r\n          }\r\n          else\r\n          {\r\n          [Microsoft.PowerShell.Commands.GetHelpCodeMethods]::GetHelpUri($this)\r\n          }\r\n          }\r\n          catch {}\r\n          finally\r\n          {\r\n          $ProgressPreference = $oldProgressPreference\r\n          }"), null)
					}
				}
			};
			yield return new TypeData("System.Management.Automation.ParameterSetMetadata", true)
			{
				Members = 
				{
					{
						"Flags",
						new CodePropertyData("Flags", Types_Ps1Xml.GetMethodInfo(typeof(DeserializingTypeConverter), "GetParameterSetMetadataFlags"), null)
						{
							IsHidden = true
						}
					}
				},
				SerializationMethod = "SpecificProperties",
				PropertySerializationSet = new PropertySetData(new string[]
				{
					"Position",
					"Flags",
					"HelpMessage"
				})
				{
					Name = "PropertySerializationSet"
				}
			};
			yield return new TypeData("Deserialized.System.Management.Automation.ParameterSetMetadata", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("Deserialized.System.Management.Automation.ExtendedTypeDefinition", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.ExtendedTypeDefinition", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.FormatViewDefinition", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.FormatViewDefinition", true)
			{
				Members = 
				{
					{
						"InstanceId",
						new CodePropertyData("InstanceId", Types_Ps1Xml.GetMethodInfo(typeof(DeserializingTypeConverter), "GetFormatViewDefinitionInstanceId"), null)
						{
							IsHidden = true
						}
					}
				},
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.PSControl", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.PSControl", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.DisplayEntry", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.DisplayEntry", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.TableControlColumnHeader", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.TableControlColumnHeader", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.TableControlRow", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.TableControlRow", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.TableControlColumn", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.TableControlColumn", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.ListControlEntry", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.ListControlEntry", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.ListControlEntryItem", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.ListControlEntryItem", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.WideControlEntryItem", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.WideControlEntryItem", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("System.Web.Services.Protocols.SoapException", true)
			{
				Members = 
				{
					{
						"PSMessageDetails",
						new ScriptPropertyData("PSMessageDetails", Types_Ps1Xml.GetScriptBlock("$this.Detail.\"#text\""), null)
					}
				}
			};
			yield return new TypeData("System.Management.Automation.ErrorRecord", true)
			{
				Members = 
				{
					{
						"PSMessageDetails",
						new ScriptPropertyData("PSMessageDetails", Types_Ps1Xml.GetScriptBlock("& { Set-StrictMode -Version 1; $this.Exception.InnerException.PSMessageDetails }"), null)
					}
				}
			};
			yield return new TypeData("Deserialized.System.Enum", true)
			{
				Members = 
				{
					{
						"Value",
						new ScriptPropertyData("Value", Types_Ps1Xml.GetScriptBlock("$this.ToString()"), null)
					}
				}
			};
			yield return new TypeData("Microsoft.PowerShell.Commands.Internal.Format.FormatInfoData", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.Microsoft.PowerShell.Commands.Internal.Format.FormatInfoData", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("System.Management.ManagementEventArgs", true)
			{
				SerializationDepth = 2U
			};
			yield return new TypeData("Deserialized.System.Management.ManagementEventArgs", true)
			{
				SerializationDepth = 2U
			};
			yield return new TypeData("System.Management.Automation.CallStackFrame", true)
			{
				Members = 
				{
					{
						"Command",
						new ScriptPropertyData("Command", Types_Ps1Xml.GetScriptBlock("if ($this.InvocationInfo -eq $null) { return $this.FunctionName }\r\n          $commandInfo = $this.InvocationInfo.MyCommand\r\n          if ($commandInfo -eq $null) { return $this.InvocationInfo.InvocationName }\r\n          if ($commandInfo.Name -ne \"\") { return $commandInfo.Name }\r\n          return $this.FunctionName"), null)
					},
					{
						"Location",
						new ScriptPropertyData("Location", Types_Ps1Xml.GetScriptBlock("$this.GetScriptLocation()"), null)
					},
					{
						"Arguments",
						new ScriptPropertyData("Arguments", Types_Ps1Xml.GetScriptBlock("$argumentsBuilder = new-object System.Text.StringBuilder\r\n\r\n          $null = $(\r\n          $argumentsBuilder.Append(\"{\")\r\n          foreach ($entry in $this.InvocationInfo.BoundParameters.GetEnumerator())\r\n          {\r\n          if ($argumentsBuilder.Length -gt 1)\r\n          {\r\n          $argumentsBuilder.Append(\", \");\r\n          }\r\n\r\n          $argumentsBuilder.Append($entry.Key).Append(\"=\")\r\n\r\n          if ($entry.Value)\r\n          {\r\n          $argumentsBuilder.Append([string]$entry.Value)\r\n          }\r\n          }\r\n\r\n          foreach ($arg in $this.InvocationInfo.UnboundArguments.GetEnumerator())\r\n          {\r\n          if ($argumentsBuilder.Length -gt 1)\r\n          {\r\n          $argumentsBuilder.Append(\", \")\r\n          }\r\n          if ($arg)\r\n          {\r\n          $argumentsBuilder.Append([string]$arg)\r\n          }\r\n          else\r\n          {\r\n          $argumentsBuilder.Append('$null')\r\n          }\r\n          }\r\n\r\n          $argumentsBuilder.Append('}');\r\n          )\r\n\r\n          return $argumentsBuilder.ToString();"), null)
					}
				}
			};
			yield return new TypeData("Microsoft.PowerShell.Commands.PSSessionConfigurationCommands#PSSessionConfiguration", true)
			{
				Members = 
				{
					{
						"Permission",
						new ScriptPropertyData("Permission", Types_Ps1Xml.GetScriptBlock("trap { continue; }\r\n          $private:sd = $null\r\n          $private:sd = new-object System.Security.AccessControl.CommonSecurityDescriptor $false,$false,$this.SecurityDescriptorSddl\r\n          if ($private:sd)\r\n          {\r\n          # reset trap\r\n          trap { }\r\n          $private:dacls = \"\";\r\n          $private:first = $true\r\n          $private:sd.DiscretionaryAcl | % {\r\n          trap { }\r\n          if ($private:first)\r\n          {\r\n          $private:first = $false;\r\n          }\r\n          else\r\n          {\r\n          $private:dacls += \", \"\r\n          }\r\n          $private:dacls += $_.SecurityIdentifier.Translate([System.Security.Principal.NTAccount]).ToString() + \" \" + $_.AceType\r\n          } # end of foreach\r\n\r\n          return $private:dacls\r\n          }"), null)
					}
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PingStatus", true)
			{
				Members = 
				{
					{
						"IPV4Address",
						new ScriptPropertyData("IPV4Address", Types_Ps1Xml.GetScriptBlock("$iphost = [System.Net.Dns]::GetHostEntry($this.address)\r\n          $iphost.AddressList | ?{ $_.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetwork } | select -first 1"), null)
					},
					{
						"IPV6Address",
						new ScriptPropertyData("IPV6Address", Types_Ps1Xml.GetScriptBlock("$iphost = [System.Net.Dns]::GetHostEntry($this.address)\r\n          $iphost.AddressList | ?{ $_.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetworkV6 } | select -first 1"), null)
					}
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Process", true)
			{
				Members = 
				{
					{
						"ProcessName",
						new AliasPropertyData("ProcessName", "Name")
					},
					{
						"Handles",
						new AliasPropertyData("Handles", "Handlecount")
					},
					{
						"VM",
						new AliasPropertyData("VM", "VirtualSize")
					},
					{
						"WS",
						new AliasPropertyData("WS", "WorkingSetSize")
					},
					{
						"Path",
						new ScriptPropertyData("Path", Types_Ps1Xml.GetScriptBlock("$this.ExecutablePath"), null)
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ProcessId",
					"Name",
					"HandleCount",
					"WorkingSetSize",
					"VirtualSize"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Msft_CliAlias", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"FriendlyName",
					"PWhere",
					"Target"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_BaseBoard", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"PoweredOn"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Manufacturer",
					"Model",
					"Name",
					"SerialNumber",
					"SKU",
					"Product"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_BIOS", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"Caption",
							"SMBIOSPresent"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"SMBIOSBIOSVersion",
					"Manufacturer",
					"Name",
					"SerialNumber",
					"Version"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_BootConfiguration", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"SettingID",
							"ConfigurationPath"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"BootDirectory",
					"Name",
					"SettingID",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_CDROMDrive", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Availability",
							"Drive",
							"ErrorCleared",
							"MediaLoaded",
							"NeedsCleaning",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Drive",
					"Manufacturer",
					"VolumeName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_ComputerSystem", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"AdminPasswordStatus",
							"BootupState",
							"ChassisBootupState",
							"KeyboardPasswordStatus",
							"PowerOnPasswordStatus",
							"PowerSupplyState",
							"PowerState",
							"FrontPanelResetStatus",
							"ThermalState",
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"POWER",
						new PropertySetData(new string[]
						{
							"Name",
							"PowerManagementCapabilities",
							"PowerManagementSupported",
							"PowerOnPasswordStatus",
							"PowerState",
							"PowerSupplyState"
						})
						{
							Name = "POWER"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Domain",
					"Manufacturer",
					"Model",
					"Name",
					"PrimaryOwnerName",
					"TotalPhysicalMemory"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/WIN32_PROCESSOR", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Availability",
							"CpuStatus",
							"CurrentVoltage",
							"DeviceID",
							"ErrorCleared",
							"ErrorDescription",
							"LastErrorCode",
							"LoadPercentage",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"AddressWidth",
							"DataWidth",
							"DeviceID",
							"ExtClock",
							"L2CacheSize",
							"L2CacheSpeed",
							"MaxClockSpeed",
							"PowerManagementSupported",
							"ProcessorType",
							"Revision",
							"SocketDesignation",
							"Version",
							"VoltageCaps"
						})
						{
							Name = "PSConfiguration"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"DeviceID",
					"Manufacturer",
					"MaxClockSpeed",
					"Name",
					"SocketDesignation"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_ComputerSystemProduct", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Version"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"IdentifyingNumber",
					"Name",
					"Vendor",
					"Version",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/CIM_DataFile", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Compressed",
					"Encrypted",
					"Size",
					"Hidden",
					"Name",
					"Readable",
					"System",
					"Version",
					"Writeable"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/WIN32_DCOMApplication", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Status"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"AppID",
					"InstallDate",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/WIN32_DESKTOP", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"ScreenSaverActive"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Name",
					"ScreenSaverActive",
					"ScreenSaverSecure",
					"ScreenSaverTimeout",
					"SettingID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/WIN32_DESKTOPMONITOR", true)
			{
				Members = 
				{
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"DeviceID",
							"Name",
							"PixelsPerXLogicalInch",
							"PixelsPerYLogicalInch",
							"ScreenHeight",
							"ScreenWidth"
						})
						{
							Name = "PSConfiguration"
						}
					},
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"DeviceID",
							"IsLocked",
							"LastErrorCode",
							"Name",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceID",
					"DisplayType",
					"MonitorManufacturer",
					"Name",
					"ScreenHeight",
					"ScreenWidth"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_DeviceMemoryAddress", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"MemoryType"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"MemoryType",
					"Name",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_DiskDrive", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"ConfigManagerErrorCode",
							"LastErrorCode",
							"NeedsCleaning",
							"Status",
							"DeviceID",
							"StatusInfo",
							"Partitions"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"BytesPerSector",
							"ConfigManagerUserConfig",
							"DefaultBlockSize",
							"DeviceID",
							"Index",
							"InstallDate",
							"InterfaceType",
							"MaxBlockSize",
							"MaxMediaSize",
							"MinBlockSize",
							"NumberOfMediaSupported",
							"Partitions",
							"SectorsPerTrack",
							"Size",
							"TotalCylinders",
							"TotalHeads",
							"TotalSectors",
							"TotalTracks",
							"TracksPerCylinder"
						})
						{
							Name = "PSConfiguration"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Partitions",
					"DeviceID",
					"Model",
					"Size",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_DiskQuota", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DiskSpaceUsed",
					"Limit",
					"QuotaVolume",
					"User"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_DMAChannel", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"AddressSize",
					"DMAChannel",
					"MaxTransferSize",
					"Name",
					"Port"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Environment", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"SystemVariable"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"VariableValue",
					"Name",
					"UserName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Directory", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Compressed",
							"Encrypted",
							"Name",
							"Readable",
							"Writeable"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Hidden",
					"Archive",
					"EightDotThreeFileName",
					"FileSize",
					"Name",
					"Compressed",
					"Encrypted",
					"Readable"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Group", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Domain",
					"Name",
					"SID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_IDEController", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Manufacturer",
					"Name",
					"ProtocolSupported",
					"Status",
					"StatusInfo"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_IRQResource", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Caption",
							"Availability"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Hardware",
					"IRQNumber",
					"Name",
					"Shareable",
					"TriggerLevel",
					"TriggerType"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_ScheduledJob", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"JobId",
							"JobStatus",
							"ElapsedTime",
							"StartTime",
							"Owner"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"JobId",
					"Name",
					"Owner",
					"Priority",
					"Command"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_LoadOrderGroup", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"GroupOrder",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_LogicalDisk", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Availability",
							"DeviceID",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceID",
					"DriveType",
					"ProviderName",
					"FreeSpace",
					"Size",
					"VolumeName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_LogonSession", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"AuthenticationPackage",
					"LogonId",
					"LogonType",
					"Name",
					"StartTime",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/WIN32_CACHEMEMORY", true)
			{
				Members = 
				{
					{
						"ERROR",
						new PropertySetData(new string[]
						{
							"DeviceID",
							"ErrorCorrectType"
						})
						{
							Name = "ERROR"
						}
					},
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Availability",
							"DeviceID",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"BlockSize",
							"CacheSpeed",
							"CacheType",
							"DeviceID",
							"InstalledSize",
							"Level",
							"MaxCacheSize",
							"NumberOfBlocks",
							"Status",
							"WritePolicy"
						})
						{
							Name = "PSConfiguration"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"BlockSize",
					"CacheSpeed",
					"CacheType",
					"DeviceID",
					"InstalledSize",
					"Level",
					"MaxCacheSize",
					"NumberOfBlocks",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_LogicalMemoryConfiguration", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"AvailableVirtualMemory",
							"Name",
							"TotalVirtualMemory"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Name",
					"TotalVirtualMemory",
					"TotalPhysicalMemory",
					"TotalPageFileSpace"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PhysicalMemoryArray", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"Replaceable",
							"Location"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Model",
					"Name",
					"MaxCapacity",
					"MemoryDevices"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/WIN32_NetworkClient", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Status"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"InstallDate",
					"Manufacturer",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_NetworkLoginProfile", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Privileges",
					"Profile",
					"UserId",
					"UserType",
					"Workstations"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_NetworkProtocol", true)
			{
				Members = 
				{
					{
						"FULLXXX",
						new PropertySetData(new string[]
						{
							"ConnectionlessService",
							"Description",
							"GuaranteesDelivery",
							"GuaranteesSequencing",
							"InstallDate",
							"MaximumAddressSize",
							"MaximumMessageSize",
							"MessageOriented",
							"MinimumAddressSize",
							"Name",
							"PseudoStreamOriented",
							"Status",
							"SupportsBroadcasting",
							"SupportsConnectData",
							"SupportsDisconnectData",
							"SupportsEncryption",
							"SupportsExpeditedData",
							"SupportsFragmentation",
							"SupportsGracefulClosing",
							"SupportsGuaranteedBandwidth",
							"SupportsMulticasting",
							"SupportsQualityofService"
						})
						{
							Name = "FULLXXX"
						}
					},
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Status",
							"SupportsBroadcasting",
							"SupportsConnectData",
							"SupportsDisconnectData",
							"SupportsEncryption",
							"SupportsExpeditedData",
							"SupportsFragmentation",
							"SupportsGracefulClosing",
							"SupportsGuaranteedBandwidth",
							"SupportsMulticasting",
							"SupportsQualityofService"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"GuaranteesDelivery",
					"GuaranteesSequencing",
					"ConnectionlessService",
					"Status",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_NetworkConnection", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"ConnectionState",
							"Persistent",
							"LocalName",
							"RemoteName"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"LocalName",
					"RemoteName",
					"ConnectionState",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_NetworkAdapter", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Availability",
							"Name",
							"Status",
							"StatusInfo",
							"DeviceID"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ServiceName",
					"MACAddress",
					"AdapterType",
					"DeviceID",
					"Name",
					"NetworkAddresses",
					"Speed"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_NetworkAdapterConfiguration", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"DHCPLeaseExpires",
							"Index",
							"Description"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"DHCP",
						new PropertySetData(new string[]
						{
							"Description",
							"DHCPEnabled",
							"DHCPLeaseExpires",
							"DHCPLeaseObtained",
							"DHCPServer",
							"Index"
						})
						{
							Name = "DHCP"
						}
					},
					{
						"DNS",
						new PropertySetData(new string[]
						{
							"Description",
							"DNSDomain",
							"DNSDomainSuffixSearchOrder",
							"DNSEnabledForWINSResolution",
							"DNSHostName",
							"DNSServerSearchOrder",
							"DomainDNSRegistrationEnabled",
							"FullDNSRegistrationEnabled",
							"Index"
						})
						{
							Name = "DNS"
						}
					},
					{
						"IP",
						new PropertySetData(new string[]
						{
							"Description",
							"Index",
							"IPAddress",
							"IPConnectionMetric",
							"IPEnabled",
							"IPFilterSecurityEnabled"
						})
						{
							Name = "IP"
						}
					},
					{
						"WINS",
						new PropertySetData(new string[]
						{
							"Description",
							"Index",
							"WINSEnableLMHostsLookup",
							"WINSHostLookupFile",
							"WINSPrimaryServer",
							"WINSScopeID",
							"WINSSecondaryServer"
						})
						{
							Name = "WINS"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DHCPEnabled",
					"IPAddress",
					"DefaultIPGateway",
					"DNSDomain",
					"ServiceName",
					"Description",
					"Index"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_NTDomain", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"DomainName"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"GUID",
						new PropertySetData(new string[]
						{
							"DomainName",
							"DomainGuid"
						})
						{
							Name = "GUID"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ClientSiteName",
					"DcSiteName",
					"Description",
					"DnsForestName",
					"DomainControllerAddress",
					"DomainControllerName",
					"DomainName",
					"Roles",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_NTLogEvent", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Category",
					"CategoryString",
					"EventCode",
					"EventIdentifier",
					"TypeEvent",
					"InsertionStrings",
					"LogFile",
					"Message",
					"RecordNumber",
					"SourceName",
					"TimeGenerated",
					"TimeWritten",
					"Type",
					"UserName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_NTEventlogFile", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"LogfileName",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"FileSize",
					"LogfileName",
					"Name",
					"NumberOfRecords"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_OnBoardDevice", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Description"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceType",
					"SerialNumber",
					"Enabled",
					"Description"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_OperatingSystem", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"FREE",
						new PropertySetData(new string[]
						{
							"FreePhysicalMemory",
							"FreeSpaceInPagingFiles",
							"FreeVirtualMemory",
							"Name"
						})
						{
							Name = "FREE"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"SystemDirectory",
					"Organization",
					"BuildNumber",
					"RegisteredUser",
					"SerialNumber",
					"Version"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PageFileUsage", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"CurrentUsage"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Name",
					"PeakUsage"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PageFileSetting", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"MaximumSize",
					"Name",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_DiskPartition", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Index",
							"Status",
							"StatusInfo",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"NumberOfBlocks",
					"BootPartition",
					"Name",
					"PrimaryPartition",
					"Size",
					"Index"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PortResource", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"NetConnectionStatus",
							"Status",
							"Name",
							"StartingAddress",
							"EndingAddress"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Name",
					"Alias"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PortConnector", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"ExternalReferenceDesignator"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Tag",
					"ConnectorType",
					"SerialNumber",
					"ExternalReferenceDesignator",
					"PortType"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Printer", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Location",
					"Name",
					"PrinterState",
					"PrinterStatus",
					"ShareName",
					"SystemName"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PrinterConfiguration", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"DriverVersion",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"PrintQuality",
					"DriverVersion",
					"Name",
					"PaperSize",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PrintJob", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Document",
							"JobId",
							"JobStatus",
							"Name",
							"PagesPrinted",
							"Status",
							"JobIdCopy",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Document",
					"JobId",
					"JobStatus",
					"Owner",
					"Priority",
					"Size",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_ProcessXXX", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"ProcessId"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"MEMORY",
						new PropertySetData(new string[]
						{
							"Handle",
							"MaximumWorkingSetSize",
							"MinimumWorkingSetSize",
							"Name",
							"PageFaults",
							"PageFileUsage",
							"PeakPageFileUsage",
							"PeakVirtualSize",
							"PeakWorkingSetSize",
							"PrivatePageCount",
							"QuotaNonPagedPoolUsage",
							"QuotaPagedPoolUsage",
							"QuotaPeakNonPagedPoolUsage",
							"QuotaPeakPagedPoolUsage",
							"VirtualSize",
							"WorkingSetSize"
						})
						{
							Name = "MEMORY"
						}
					},
					{
						"IO",
						new PropertySetData(new string[]
						{
							"Name",
							"ProcessId",
							"ReadOperationCount",
							"ReadTransferCount",
							"WriteOperationCount",
							"WriteTransferCount"
						})
						{
							Name = "IO"
						}
					},
					{
						"STATISTICS",
						new PropertySetData(new string[]
						{
							"HandleCount",
							"Name",
							"KernelModeTime",
							"MaximumWorkingSetSize",
							"MinimumWorkingSetSize",
							"OtherOperationCount",
							"OtherTransferCount",
							"PageFaults",
							"PageFileUsage",
							"PeakPageFileUsage",
							"PeakVirtualSize",
							"PeakWorkingSetSize",
							"PrivatePageCount",
							"ProcessId",
							"QuotaNonPagedPoolUsage",
							"QuotaPagedPoolUsage",
							"QuotaPeakNonPagedPoolUsage",
							"QuotaPeakPagedPoolUsage",
							"ReadOperationCount",
							"ReadTransferCount",
							"ThreadCount",
							"UserModeTime",
							"VirtualSize",
							"WorkingSetSize",
							"WriteOperationCount",
							"WriteTransferCount"
						})
						{
							Name = "STATISTICS"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ThreadCount",
					"HandleCount",
					"Name",
					"Priority",
					"ProcessId",
					"WorkingSetSize"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Product", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Version",
							"InstallState"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"IdentifyingNumber",
					"Name",
					"Vendor",
					"Version",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_QuickFixEngineering", true)
			{
				Members = 
				{
					{
						"InstalledOn",
						new ScriptPropertyData("InstalledOn", Types_Ps1Xml.GetScriptBlock("if ([environment]::osversion.version.build -ge 7000)\r\n          {\r\n          # WMI team fixed the formatting issue related to InstalledOn\r\n          # property in Windows7 (to return string)..so returning the WMI's\r\n          # version directly\r\n          [DateTime]::Parse($this.psBase.CimInstanceProperties[\"InstalledOn\"].Value)\r\n          }\r\n          else\r\n          {\r\n          $orig = $this.psBase.CimInstanceProperties[\"InstalledOn\"].Value\r\n          $date = [datetime]::FromFileTimeUTC($(\"0x\" + $orig))\r\n          if ($date -lt \"1/1/1980\")\r\n          {\r\n          if ($orig -match \"([0-9]{4})([01][0-9])([012][0-9])\")\r\n          {\r\n          new-object datetime @([int]$matches[1], [int]$matches[2], [int]$matches[3])\r\n          }\r\n          }\r\n          else\r\n          {\r\n          $date\r\n          }\r\n          }"), null)
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Description",
					"FixComments",
					"HotFixID",
					"InstallDate",
					"InstalledBy",
					"InstalledOn",
					"Name",
					"ServicePackInEffect",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_QuotaSetting", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"State",
							"VolumePath",
							"Caption"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"DefaultLimit",
					"SettingID",
					"State",
					"VolumePath",
					"DefaultWarningLimit"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_OSRecoveryConfiguration", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DebugFilePath",
					"Name",
					"SettingID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Registry", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"CurrentSize",
							"MaximumSize",
							"ProposedSize"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"CurrentSize",
					"MaximumSize",
					"Name",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_SCSIController", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DriverName",
					"Manufacturer",
					"Name",
					"ProtocolSupported",
					"Status",
					"StatusInfo"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_PerfRawData_PerfNet_Server", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"LogonPerSec",
					"LogonTotal",
					"Name",
					"ServerSessions",
					"WorkItemShortages"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Service", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Name",
							"Status",
							"ExitCode"
						})
						{
							Name = "PSStatus"
						}
					},
					{
						"PSConfiguration",
						new PropertySetData(new string[]
						{
							"DesktopInteract",
							"ErrorControl",
							"Name",
							"PathName",
							"ServiceType",
							"StartMode"
						})
						{
							Name = "PSConfiguration"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"ExitCode",
					"Name",
					"ProcessId",
					"StartMode",
					"State",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_Share", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Type",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Name",
					"Path",
					"Description"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_SoftwareElement", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"SoftwareElementState",
							"Name"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Name",
					"Path",
					"SerialNumber",
					"SoftwareElementID",
					"Version"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_SoftwareFeature", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"InstallState",
							"LastUse"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"IdentifyingNumber",
					"ProductName",
					"Vendor",
					"Version"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/WIN32_SoundDevice", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"ConfigManagerUserConfig",
							"Name",
							"Status",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Manufacturer",
					"Name",
					"Status",
					"StatusInfo"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_StartupCommand", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Command",
					"User",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_SystemAccount", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"SIDType",
							"Name",
							"Domain"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Caption",
					"Domain",
					"Name",
					"SID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_SystemDriver", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Name",
							"State",
							"ExitCode",
							"Started",
							"ServiceSpecificExitCode"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DisplayName",
					"Name",
					"State",
					"Status",
					"Started"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_SystemEnclosure", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Tag",
							"Status",
							"Name",
							"SecurityStatus"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Manufacturer",
					"Model",
					"LockPresent",
					"SerialNumber",
					"SMBIOSAssetTag",
					"SecurityStatus"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_SystemSlot", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"SlotDesignation"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"SlotDesignation",
					"Tag",
					"SupportsHotPlug",
					"Status",
					"Shared",
					"PMESignal",
					"MaxDataWidth"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_TapeDrive", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Availability",
							"DeviceID",
							"NeedsCleaning",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceID",
					"Id",
					"Manufacturer",
					"Name",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_TemperatureProbe", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"CurrentReading",
							"DeviceID",
							"Name",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"CurrentReading",
					"Name",
					"Description",
					"MinReadable",
					"MaxReadable",
					"Status"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_TimeZone", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Bias",
					"SettingID",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_UninterruptiblePowerSupply", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"DeviceID",
							"EstimatedChargeRemaining",
							"EstimatedRunTime",
							"Name",
							"StatusInfo",
							"TimeOnBackup"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"DeviceID",
					"EstimatedRunTime",
					"Name",
					"TimeOnBackup",
					"UPSPort",
					"Caption"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_UserAccount", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"Caption",
							"PasswordExpires"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"AccountType",
					"Caption",
					"Domain",
					"SID",
					"FullName",
					"Name"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_VoltageProbe", true)
			{
				Members = 
				{
					{
						"PSStatus",
						new PropertySetData(new string[]
						{
							"Status",
							"DeviceID",
							"Name",
							"NominalReading",
							"StatusInfo"
						})
						{
							Name = "PSStatus"
						}
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Status",
					"Description",
					"CurrentReading",
					"MaxReadable",
					"MinReadable"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_VolumeQuotaSetting", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Element",
					"Setting"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimInstance#root/cimv2/Win32_WMISetting", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"BuildVersion",
					"Caption",
					"DatabaseDirectory",
					"EnableEvents",
					"LoggingLevel",
					"SettingID"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimClass", true)
			{
				Members = 
				{
					{
						"CimClassName",
						new ScriptPropertyData("CimClassName", Types_Ps1Xml.GetScriptBlock("[OutputType([string])]\r\n          param()\r\n          $this.PSBase.CimSystemProperties.ClassName"), null)
					}
				}
			};
			yield return new TypeData("Microsoft.Management.Infrastructure.CimCmdlets.CimIndicationEventInstanceEventArgs", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("System.Management.Automation.Breakpoint", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.Breakpoint", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.BreakpointUpdatedEventArgs", true)
			{
				SerializationDepth = 2U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.BreakpointUpdatedEventArgs", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.DebuggerCommand", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.DebuggerCommand", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.DebuggerCommandResults", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.DebuggerCommandResults", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield break;
		}
	}
}
