using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation.Internal;
using System.Reflection;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200084D RID: 2125
	internal class RunspaceConfigForSingleShell : RunspaceConfiguration
	{
		// Token: 0x060051C3 RID: 20931 RVA: 0x001B3C9C File Offset: 0x001B1E9C
		internal new static RunspaceConfigForSingleShell Create(string consoleFile, out PSConsoleLoadException warning)
		{
			PSConsoleLoadException ex = null;
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Creating MshConsoleInfo. consoleFile={0}", new object[]
			{
				consoleFile
			});
			MshConsoleInfo mshConsoleInfo = MshConsoleInfo.CreateFromConsoleFile(consoleFile, out ex);
			if (ex != null)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("There was a warning while creating MshConsoleInfo: {0}", new object[]
				{
					ex.Message
				});
			}
			if (mshConsoleInfo != null)
			{
				RunspaceConfigForSingleShell runspaceConfigForSingleShell = new RunspaceConfigForSingleShell(mshConsoleInfo);
				PSConsoleLoadException ex2 = null;
				runspaceConfigForSingleShell.LoadConsole(out ex2);
				if (ex2 != null)
				{
					RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("There was a warning while loading console: {0}", new object[]
					{
						ex2.Message
					});
				}
				warning = RunspaceConfigForSingleShell.CombinePSConsoleLoadException(ex, ex2);
				return runspaceConfigForSingleShell;
			}
			warning = null;
			return null;
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x001B3D44 File Offset: 0x001B1F44
		private static PSConsoleLoadException CombinePSConsoleLoadException(PSConsoleLoadException e1, PSConsoleLoadException e2)
		{
			if ((e1 == null || e1.PSSnapInExceptions.Count == 0) && (e2 == null || e2.PSSnapInExceptions.Count == 0))
			{
				return null;
			}
			if (e1 == null || e1.PSSnapInExceptions.Count == 0)
			{
				return e2;
			}
			if (e2 == null || e2.PSSnapInExceptions.Count == 0)
			{
				return e1;
			}
			foreach (PSSnapInException item in e2.PSSnapInExceptions)
			{
				e1.PSSnapInExceptions.Add(item);
			}
			return e1;
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x001B3DE0 File Offset: 0x001B1FE0
		internal static RunspaceConfigForSingleShell CreateDefaultConfiguration()
		{
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Creating default runspace configuration.", new object[0]);
			MshConsoleInfo mshConsoleInfo = MshConsoleInfo.CreateDefaultConfiguration();
			if (mshConsoleInfo != null)
			{
				RunspaceConfigForSingleShell runspaceConfigForSingleShell = new RunspaceConfigForSingleShell(mshConsoleInfo);
				PSConsoleLoadException ex = null;
				runspaceConfigForSingleShell.LoadConsole(out ex);
				if (ex != null)
				{
					RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("There was a warning while loading console: {0}", new object[]
					{
						ex.Message
					});
				}
				return runspaceConfigForSingleShell;
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Default runspace configuration created.", new object[0]);
			return null;
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x001B3E57 File Offset: 0x001B2057
		private RunspaceConfigForSingleShell(MshConsoleInfo consoleInfo)
		{
			this._consoleInfo = consoleInfo;
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x060051C7 RID: 20935 RVA: 0x001B3E7C File Offset: 0x001B207C
		internal MshConsoleInfo ConsoleInfo
		{
			get
			{
				return this._consoleInfo;
			}
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x001B3E84 File Offset: 0x001B2084
		internal void SaveConsoleFile()
		{
			if (this._consoleInfo == null)
			{
				return;
			}
			this._consoleInfo.Save();
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x001B3E9A File Offset: 0x001B209A
		internal void SaveAsConsoleFile(string filename)
		{
			if (this._consoleInfo == null)
			{
				return;
			}
			this._consoleInfo.SaveAsConsoleFile(filename);
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x001B3EB4 File Offset: 0x001B20B4
		internal override PSSnapInInfo DoAddPSSnapIn(string name, out PSSnapInException warning)
		{
			warning = null;
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Adding mshsnapin {0}", new object[]
			{
				name
			});
			if (this._consoleInfo == null)
			{
				return null;
			}
			PSSnapInInfo pssnapInInfo = null;
			try
			{
				pssnapInInfo = this._consoleInfo.AddPSSnapIn(name);
			}
			catch (PSArgumentException ex)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceError(ex.Message, new object[0]);
				RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Adding mshsnapin {0} failed.", new object[]
				{
					name
				});
				throw;
			}
			catch (PSArgumentNullException ex2)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceError(ex2.Message, new object[0]);
				RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Adding mshsnapin {0} failed.", new object[]
				{
					name
				});
				throw;
			}
			if (pssnapInInfo == null)
			{
				return null;
			}
			this.LoadPSSnapIn(pssnapInInfo, out warning);
			if (warning != null)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("There was a warning when loading mshsnapin {0}: {1}", new object[]
				{
					name,
					warning.Message
				});
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("MshSnapin {0} added", new object[]
			{
				name
			});
			return pssnapInInfo;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x001B3FE0 File Offset: 0x001B21E0
		internal override PSSnapInInfo DoRemovePSSnapIn(string name, out PSSnapInException warning)
		{
			warning = null;
			if (this._consoleInfo == null)
			{
				return null;
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Removing mshsnapin {0}", new object[]
			{
				name
			});
			PSSnapInInfo pssnapInInfo = this._consoleInfo.RemovePSSnapIn(name);
			this.UnloadPSSnapIn(pssnapInInfo, out warning);
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("MshSnapin {0} removed", new object[]
			{
				name
			});
			return pssnapInInfo;
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x001B4048 File Offset: 0x001B2248
		internal void UpdateAll()
		{
			string text = "";
			this.UpdateAll(out text);
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x001B4064 File Offset: 0x001B2264
		internal void UpdateAll(out string errors)
		{
			errors = "";
			this.Cmdlets.Update();
			this.Providers.Update();
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Updating types and formats", new object[0]);
			try
			{
				this.Types.Update();
			}
			catch (RuntimeException ex)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("There was a warning updating types: {0}", new object[]
				{
					ex.Message
				});
				errors = errors + ex.Message + "\n";
			}
			try
			{
				this.Formats.Update();
			}
			catch (RuntimeException ex2)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("There was a warning updating formats: {0}", new object[]
				{
					ex2.Message
				});
				errors = errors + ex2.Message + "\n";
			}
			try
			{
				this.Assemblies.Update();
			}
			catch (RuntimeException ex3)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("There was a warning updating assemblies: {0}", new object[]
				{
					ex3.Message
				});
				errors = errors + ex3.Message + "\n";
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Types and formats updated successfully", new object[0]);
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x001B41B4 File Offset: 0x001B23B4
		private void LoadConsole(out PSConsoleLoadException warning)
		{
			if (this._consoleInfo == null)
			{
				warning = null;
				return;
			}
			this.LoadPSSnapIns(this._consoleInfo.PSSnapIns, out warning);
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x001B41D4 File Offset: 0x001B23D4
		private void LoadPSSnapIn(PSSnapInInfo mshsnapinInfo, out PSSnapInException warning)
		{
			warning = null;
			try
			{
				this.LoadPSSnapIn(mshsnapinInfo);
			}
			catch (PSSnapInException)
			{
				if (!mshsnapinInfo.IsDefault)
				{
					this._consoleInfo.RemovePSSnapIn(mshsnapinInfo.Name);
				}
				throw;
			}
			string text;
			this.UpdateAll(out text);
			if (!string.IsNullOrEmpty(text))
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("There was a warning while loading mshsnapin {0}:{1}", new object[]
				{
					mshsnapinInfo.Name,
					text
				});
				warning = new PSSnapInException(mshsnapinInfo.Name, text, true);
			}
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x001B4260 File Offset: 0x001B2460
		private void LoadPSSnapIns(Collection<PSSnapInInfo> mshsnapinInfos, out PSConsoleLoadException warning)
		{
			warning = null;
			Collection<PSSnapInException> collection = new Collection<PSSnapInException>();
			bool flag = false;
			foreach (PSSnapInInfo pssnapInInfo in mshsnapinInfos)
			{
				try
				{
					this.LoadPSSnapIn(pssnapInInfo);
					flag = true;
				}
				catch (PSSnapInException item)
				{
					if (pssnapInInfo.IsDefault)
					{
						throw;
					}
					this._consoleInfo.RemovePSSnapIn(pssnapInInfo.Name);
					collection.Add(item);
				}
			}
			if (flag)
			{
				string text;
				this.UpdateAll(out text);
				if (!string.IsNullOrEmpty(text))
				{
					RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning(text, new object[0]);
					collection.Add(new PSSnapInException(null, text, true));
				}
			}
			if (collection.Count > 0)
			{
				warning = new PSConsoleLoadException(this._consoleInfo, collection);
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning(warning.Message, new object[0]);
			}
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x001B4354 File Offset: 0x001B2554
		private void LoadPSSnapIn(PSSnapInInfo mshsnapinInfo)
		{
			if (mshsnapinInfo == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(mshsnapinInfo.CustomPSSnapInType))
			{
				this.LoadCustomPSSnapIn(mshsnapinInfo);
				return;
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Loading assembly for mshsnapin {0}", new object[]
			{
				mshsnapinInfo.Name
			});
			Assembly assembly = this.LoadMshSnapinAssembly(mshsnapinInfo);
			if (assembly == null)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceError("Loading assembly for mshsnapin {0} failed", new object[]
				{
					mshsnapinInfo.Name
				});
				return;
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Loading assembly for mshsnapin {0} succeeded", new object[]
			{
				mshsnapinInfo.Name
			});
			Dictionary<string, SessionStateCmdletEntry> dictionary = null;
			Dictionary<string, List<SessionStateAliasEntry>> dictionary2 = null;
			Dictionary<string, SessionStateProviderEntry> dictionary3 = null;
			string text = null;
			PSSnapInHelpers.AnalyzePSSnapInAssembly(assembly, ClrFacade.GetAssemblyLocation(assembly), mshsnapinInfo, null, false, out dictionary, out dictionary2, out dictionary3, out text);
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, SessionStateCmdletEntry> keyValuePair in dictionary)
				{
					CmdletConfigurationEntry item = new CmdletConfigurationEntry(keyValuePair.Key, keyValuePair.Value.ImplementingType, keyValuePair.Value.HelpFileName, mshsnapinInfo);
					this._cmdlets.AddBuiltInItem(item);
				}
			}
			if (dictionary3 != null)
			{
				foreach (KeyValuePair<string, SessionStateProviderEntry> keyValuePair2 in dictionary3)
				{
					ProviderConfigurationEntry item2 = new ProviderConfigurationEntry(keyValuePair2.Key, keyValuePair2.Value.ImplementingType, keyValuePair2.Value.HelpFileName, mshsnapinInfo);
					this._providers.AddBuiltInItem(item2);
				}
			}
			foreach (string path in mshsnapinInfo.Types)
			{
				string text2 = Path.Combine(mshsnapinInfo.ApplicationBase, path);
				TypeConfigurationEntry item3 = new TypeConfigurationEntry(text2, text2, mshsnapinInfo);
				this.Types.AddBuiltInItem(item3);
			}
			foreach (string path2 in mshsnapinInfo.Formats)
			{
				string text3 = Path.Combine(mshsnapinInfo.ApplicationBase, path2);
				FormatConfigurationEntry item4 = new FormatConfigurationEntry(text3, text3, mshsnapinInfo);
				this.Formats.AddBuiltInItem(item4);
			}
			AssemblyConfigurationEntry item5 = new AssemblyConfigurationEntry(mshsnapinInfo.AssemblyName, mshsnapinInfo.AbsoluteModulePath, mshsnapinInfo);
			this.Assemblies.AddBuiltInItem(item5);
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x001B45E0 File Offset: 0x001B27E0
		private void LoadCustomPSSnapIn(PSSnapInInfo mshsnapinInfo)
		{
			if (mshsnapinInfo == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(mshsnapinInfo.CustomPSSnapInType))
			{
				return;
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Loading assembly for mshsnapin {0}", new object[]
			{
				mshsnapinInfo.Name
			});
			Assembly assembly = this.LoadMshSnapinAssembly(mshsnapinInfo);
			if (assembly == null)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceError("Loading assembly for mshsnapin {0} failed", new object[]
				{
					mshsnapinInfo.Name
				});
				return;
			}
			CustomPSSnapIn customPSSnapIn = null;
			try
			{
				Type type = assembly.GetType(mshsnapinInfo.CustomPSSnapInType, true, false);
				if (type != null)
				{
					customPSSnapIn = (CustomPSSnapIn)Activator.CreateInstance(type);
				}
				RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Loading assembly for mshsnapin {0} succeeded", new object[]
				{
					mshsnapinInfo.Name
				});
			}
			catch (TypeLoadException ex)
			{
				throw new PSSnapInException(mshsnapinInfo.Name, ex.Message);
			}
			catch (ArgumentException ex2)
			{
				throw new PSSnapInException(mshsnapinInfo.Name, ex2.Message);
			}
			catch (MissingMethodException ex3)
			{
				throw new PSSnapInException(mshsnapinInfo.Name, ex3.Message);
			}
			catch (InvalidCastException ex4)
			{
				throw new PSSnapInException(mshsnapinInfo.Name, ex4.Message);
			}
			catch (TargetInvocationException ex5)
			{
				if (ex5.InnerException != null)
				{
					throw new PSSnapInException(mshsnapinInfo.Name, ex5.InnerException.Message);
				}
				throw new PSSnapInException(mshsnapinInfo.Name, ex5.Message);
			}
			this.MergeCustomPSSnapIn(mshsnapinInfo, customPSSnapIn);
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x001B4778 File Offset: 0x001B2978
		private void MergeCustomPSSnapIn(PSSnapInInfo mshsnapinInfo, CustomPSSnapIn customPSSnapIn)
		{
			if (mshsnapinInfo == null || customPSSnapIn == null)
			{
				return;
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Merging configuration from custom mshsnapin {0}", new object[]
			{
				mshsnapinInfo.Name
			});
			if (customPSSnapIn.Cmdlets != null)
			{
				foreach (CmdletConfigurationEntry cmdletConfigurationEntry in customPSSnapIn.Cmdlets)
				{
					CmdletConfigurationEntry item = new CmdletConfigurationEntry(cmdletConfigurationEntry.Name, cmdletConfigurationEntry.ImplementingType, cmdletConfigurationEntry.HelpFileName, mshsnapinInfo);
					this._cmdlets.AddBuiltInItem(item);
				}
			}
			if (customPSSnapIn.Providers != null)
			{
				foreach (ProviderConfigurationEntry providerConfigurationEntry in customPSSnapIn.Providers)
				{
					ProviderConfigurationEntry item2 = new ProviderConfigurationEntry(providerConfigurationEntry.Name, providerConfigurationEntry.ImplementingType, providerConfigurationEntry.HelpFileName, mshsnapinInfo);
					this._providers.AddBuiltInItem(item2);
				}
			}
			if (customPSSnapIn.Types != null)
			{
				foreach (TypeConfigurationEntry typeConfigurationEntry in customPSSnapIn.Types)
				{
					string fileName = Path.Combine(mshsnapinInfo.ApplicationBase, typeConfigurationEntry.FileName);
					TypeConfigurationEntry item3 = new TypeConfigurationEntry(typeConfigurationEntry.Name, fileName, mshsnapinInfo);
					this._types.AddBuiltInItem(item3);
				}
			}
			if (customPSSnapIn.Formats != null)
			{
				foreach (FormatConfigurationEntry formatConfigurationEntry in customPSSnapIn.Formats)
				{
					string fileName2 = Path.Combine(mshsnapinInfo.ApplicationBase, formatConfigurationEntry.FileName);
					FormatConfigurationEntry item4 = new FormatConfigurationEntry(formatConfigurationEntry.Name, fileName2, mshsnapinInfo);
					this._formats.AddBuiltInItem(item4);
				}
			}
			AssemblyConfigurationEntry item5 = new AssemblyConfigurationEntry(mshsnapinInfo.AssemblyName, mshsnapinInfo.AbsoluteModulePath, mshsnapinInfo);
			this.Assemblies.AddBuiltInItem(item5);
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Configuration from custom mshsnapin {0} merged", new object[]
			{
				mshsnapinInfo.Name
			});
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x001B49B0 File Offset: 0x001B2BB0
		private void UnloadPSSnapIn(PSSnapInInfo mshsnapinInfo, out PSSnapInException warning)
		{
			warning = null;
			if (mshsnapinInfo != null)
			{
				this.Cmdlets.RemovePSSnapIn(mshsnapinInfo.Name);
				this.Providers.RemovePSSnapIn(mshsnapinInfo.Name);
				this.Assemblies.RemovePSSnapIn(mshsnapinInfo.Name);
				this.Types.RemovePSSnapIn(mshsnapinInfo.Name);
				this.Formats.RemovePSSnapIn(mshsnapinInfo.Name);
				string text;
				this.UpdateAll(out text);
				if (!string.IsNullOrEmpty(text))
				{
					RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning(text, new object[0]);
					warning = new PSSnapInException(mshsnapinInfo.Name, text, true);
				}
			}
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x001B4A4C File Offset: 0x001B2C4C
		private Assembly LoadMshSnapinAssembly(PSSnapInInfo mshsnapinInfo)
		{
			Assembly assembly = null;
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Loading assembly from GAC. Assembly Name: {0}", new object[]
			{
				mshsnapinInfo.AssemblyName
			});
			try
			{
				assembly = Assembly.Load(new AssemblyName(mshsnapinInfo.AssemblyName));
			}
			catch (FileLoadException ex)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
				{
					mshsnapinInfo.AssemblyName,
					ex.Message
				});
			}
			catch (BadImageFormatException ex2)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
				{
					mshsnapinInfo.AssemblyName,
					ex2.Message
				});
			}
			catch (FileNotFoundException ex3)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
				{
					mshsnapinInfo.AssemblyName,
					ex3.Message
				});
			}
			if (assembly != null)
			{
				return assembly;
			}
			RunspaceConfigForSingleShell._mshsnapinTracer.WriteLine("Loading assembly from path: {0}", new object[]
			{
				mshsnapinInfo.AssemblyName
			});
			try
			{
				AssemblyName assemblyName = ClrFacade.GetAssemblyName(mshsnapinInfo.AbsoluteModulePath);
				if (string.Compare(assemblyName.FullName, mshsnapinInfo.AssemblyName, StringComparison.OrdinalIgnoreCase) != 0)
				{
					string text = StringUtil.Format(ConsoleInfoErrorStrings.PSSnapInAssemblyNameMismatch, mshsnapinInfo.AbsoluteModulePath, mshsnapinInfo.AssemblyName);
					RunspaceConfigForSingleShell._mshsnapinTracer.TraceError(text, new object[0]);
					throw new PSSnapInException(mshsnapinInfo.Name, text);
				}
				assembly = ClrFacade.LoadFrom(mshsnapinInfo.AbsoluteModulePath);
			}
			catch (FileLoadException ex4)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceError("Not able to load assembly {0}: {1}", new object[]
				{
					mshsnapinInfo.AssemblyName,
					ex4.Message
				});
				throw new PSSnapInException(mshsnapinInfo.Name, ex4.Message);
			}
			catch (BadImageFormatException ex5)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceError("Not able to load assembly {0}: {1}", new object[]
				{
					mshsnapinInfo.AssemblyName,
					ex5.Message
				});
				throw new PSSnapInException(mshsnapinInfo.Name, ex5.Message);
			}
			catch (FileNotFoundException ex6)
			{
				RunspaceConfigForSingleShell._mshsnapinTracer.TraceError("Not able to load assembly {0}: {1}", new object[]
				{
					mshsnapinInfo.AssemblyName,
					ex6.Message
				});
				throw new PSSnapInException(mshsnapinInfo.Name, ex6.Message);
			}
			return assembly;
		}

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x060051D6 RID: 20950 RVA: 0x001B4CD4 File Offset: 0x001B2ED4
		public override string ShellId
		{
			get
			{
				return Utils.DefaultPowerShellShellID;
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x060051D7 RID: 20951 RVA: 0x001B4CDB File Offset: 0x001B2EDB
		public override RunspaceConfigurationEntryCollection<CmdletConfigurationEntry> Cmdlets
		{
			get
			{
				return this._cmdlets;
			}
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x060051D8 RID: 20952 RVA: 0x001B4CE3 File Offset: 0x001B2EE3
		public override RunspaceConfigurationEntryCollection<ProviderConfigurationEntry> Providers
		{
			get
			{
				return this._providers;
			}
		}

		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x001B4CEB File Offset: 0x001B2EEB
		public override RunspaceConfigurationEntryCollection<TypeConfigurationEntry> Types
		{
			get
			{
				if (this._types == null)
				{
					this._types = new RunspaceConfigurationEntryCollection<TypeConfigurationEntry>();
				}
				return this._types;
			}
		}

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060051DA RID: 20954 RVA: 0x001B4D06 File Offset: 0x001B2F06
		public override RunspaceConfigurationEntryCollection<FormatConfigurationEntry> Formats
		{
			get
			{
				if (this._formats == null)
				{
					this._formats = new RunspaceConfigurationEntryCollection<FormatConfigurationEntry>();
				}
				return this._formats;
			}
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060051DB RID: 20955 RVA: 0x001B4D21 File Offset: 0x001B2F21
		public override RunspaceConfigurationEntryCollection<ScriptConfigurationEntry> InitializationScripts
		{
			get
			{
				if (this._initializationScripts == null)
				{
					this._initializationScripts = new RunspaceConfigurationEntryCollection<ScriptConfigurationEntry>();
				}
				return this._initializationScripts;
			}
		}

		// Token: 0x04002A0E RID: 10766
		private MshConsoleInfo _consoleInfo;

		// Token: 0x04002A0F RID: 10767
		private RunspaceConfigurationEntryCollection<CmdletConfigurationEntry> _cmdlets = new RunspaceConfigurationEntryCollection<CmdletConfigurationEntry>();

		// Token: 0x04002A10 RID: 10768
		private RunspaceConfigurationEntryCollection<ProviderConfigurationEntry> _providers = new RunspaceConfigurationEntryCollection<ProviderConfigurationEntry>();

		// Token: 0x04002A11 RID: 10769
		private RunspaceConfigurationEntryCollection<TypeConfigurationEntry> _types;

		// Token: 0x04002A12 RID: 10770
		private RunspaceConfigurationEntryCollection<FormatConfigurationEntry> _formats;

		// Token: 0x04002A13 RID: 10771
		private RunspaceConfigurationEntryCollection<ScriptConfigurationEntry> _initializationScripts;

		// Token: 0x04002A14 RID: 10772
		private static PSTraceSource _mshsnapinTracer = PSTraceSource.GetTracer("MshSnapinLoadUnload", "Loading and unloading mshsnapins", false);
	}
}
