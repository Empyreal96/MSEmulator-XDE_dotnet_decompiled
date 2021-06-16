using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Provider;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000835 RID: 2101
	internal static class PSSnapInHelpers
	{
		// Token: 0x060050CF RID: 20687 RVA: 0x001AEE14 File Offset: 0x001AD014
		internal static Assembly LoadPSSnapInAssembly(PSSnapInInfo psSnapInInfo, out Dictionary<string, SessionStateCmdletEntry> cmdlets, out Dictionary<string, SessionStateProviderEntry> providers)
		{
			Assembly assembly = null;
			cmdlets = null;
			providers = null;
			PSSnapInHelpers._PSSnapInTracer.WriteLine("Loading assembly from GAC. Assembly Name: {0}", new object[]
			{
				psSnapInInfo.AssemblyName
			});
			try
			{
				assembly = Assembly.Load(new AssemblyName(psSnapInInfo.AssemblyName));
			}
			catch (BadImageFormatException ex)
			{
				PSSnapInHelpers._PSSnapInTracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
				{
					psSnapInInfo.AssemblyName,
					ex.Message
				});
			}
			catch (FileNotFoundException ex2)
			{
				PSSnapInHelpers._PSSnapInTracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
				{
					psSnapInInfo.AssemblyName,
					ex2.Message
				});
			}
			catch (FileLoadException ex3)
			{
				PSSnapInHelpers._PSSnapInTracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
				{
					psSnapInInfo.AssemblyName,
					ex3.Message
				});
			}
			if (assembly != null)
			{
				return assembly;
			}
			PSSnapInHelpers._PSSnapInTracer.WriteLine("Loading assembly from path: {0}", new object[]
			{
				psSnapInInfo.AssemblyName
			});
			try
			{
				AssemblyName assemblyName = ClrFacade.GetAssemblyName(psSnapInInfo.AbsoluteModulePath);
				if (!string.Equals(assemblyName.FullName, psSnapInInfo.AssemblyName, StringComparison.OrdinalIgnoreCase))
				{
					string text = StringUtil.Format(ConsoleInfoErrorStrings.PSSnapInAssemblyNameMismatch, psSnapInInfo.AbsoluteModulePath, psSnapInInfo.AssemblyName);
					PSSnapInHelpers._PSSnapInTracer.TraceError(text, new object[0]);
					throw new PSSnapInException(psSnapInInfo.Name, text);
				}
				assembly = ClrFacade.LoadFrom(psSnapInInfo.AbsoluteModulePath);
			}
			catch (FileLoadException ex4)
			{
				PSSnapInHelpers._PSSnapInTracer.TraceError("Not able to load assembly {0}: {1}", new object[]
				{
					psSnapInInfo.AssemblyName,
					ex4.Message
				});
				throw new PSSnapInException(psSnapInInfo.Name, ex4.Message);
			}
			catch (BadImageFormatException ex5)
			{
				PSSnapInHelpers._PSSnapInTracer.TraceError("Not able to load assembly {0}: {1}", new object[]
				{
					psSnapInInfo.AssemblyName,
					ex5.Message
				});
				throw new PSSnapInException(psSnapInInfo.Name, ex5.Message);
			}
			catch (FileNotFoundException ex6)
			{
				PSSnapInHelpers._PSSnapInTracer.TraceError("Not able to load assembly {0}: {1}", new object[]
				{
					psSnapInInfo.AssemblyName,
					ex6.Message
				});
				throw new PSSnapInException(psSnapInInfo.Name, ex6.Message);
			}
			return assembly;
		}

		// Token: 0x060050D0 RID: 20688 RVA: 0x001AF0A4 File Offset: 0x001AD2A4
		private static T GetCustomAttribute<T>(TypeInfo decoratedType) where T : Attribute
		{
			IEnumerable<T> customAttributes = decoratedType.GetCustomAttributes(false);
			T[] array = customAttributes.ToArray<T>();
			if (array.Length != 0)
			{
				return array[0];
			}
			return default(T);
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x001AF0E0 File Offset: 0x001AD2E0
		internal static void AnalyzePSSnapInAssembly(Assembly assembly, string name, PSSnapInInfo psSnapInInfo, PSModuleInfo moduleInfo, bool isModuleLoad, out Dictionary<string, SessionStateCmdletEntry> cmdlets, out Dictionary<string, List<SessionStateAliasEntry>> aliases, out Dictionary<string, SessionStateProviderEntry> providers, out string helpFile)
		{
			helpFile = null;
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			cmdlets = null;
			aliases = null;
			providers = null;
			if (PSSnapInHelpers._cmdletCache.Value.ContainsKey(assembly))
			{
				cmdlets = new Dictionary<string, SessionStateCmdletEntry>(PSSnapInHelpers._cmdletCache.Value.Count, StringComparer.OrdinalIgnoreCase);
				aliases = new Dictionary<string, List<SessionStateAliasEntry>>(StringComparer.OrdinalIgnoreCase);
				Dictionary<string, Tuple<SessionStateCmdletEntry, List<SessionStateAliasEntry>>> dictionary = PSSnapInHelpers._cmdletCache.Value[assembly];
				foreach (string key in dictionary.Keys)
				{
					Tuple<SessionStateCmdletEntry, List<SessionStateAliasEntry>> tuple = dictionary[key];
					if (tuple.Item1.PSSnapIn == null && psSnapInInfo != null)
					{
						tuple.Item1.SetPSSnapIn(psSnapInInfo);
					}
					SessionStateCmdletEntry sessionStateCmdletEntry = (SessionStateCmdletEntry)tuple.Item1.Clone();
					if (sessionStateCmdletEntry.PSSnapIn != null && psSnapInInfo == null)
					{
						sessionStateCmdletEntry.SetPSSnapIn(null);
					}
					cmdlets[key] = sessionStateCmdletEntry;
					if (tuple.Item2 != null)
					{
						List<SessionStateAliasEntry> list = new List<SessionStateAliasEntry>();
						foreach (SessionStateAliasEntry sessionStateAliasEntry in tuple.Item2)
						{
							if (sessionStateAliasEntry.PSSnapIn == null && psSnapInInfo != null)
							{
								sessionStateAliasEntry.SetPSSnapIn(psSnapInInfo);
							}
							SessionStateAliasEntry sessionStateAliasEntry2 = (SessionStateAliasEntry)sessionStateAliasEntry.Clone();
							if (sessionStateAliasEntry2.PSSnapIn != null && psSnapInInfo == null)
							{
								sessionStateAliasEntry2.SetPSSnapIn(null);
							}
							list.Add(sessionStateAliasEntry2);
						}
						aliases[key] = list;
					}
				}
			}
			if (PSSnapInHelpers._providerCache.Value.ContainsKey(assembly))
			{
				providers = new Dictionary<string, SessionStateProviderEntry>(PSSnapInHelpers._providerCache.Value.Count, StringComparer.OrdinalIgnoreCase);
				Dictionary<string, SessionStateProviderEntry> dictionary2 = PSSnapInHelpers._providerCache.Value[assembly];
				foreach (string key2 in dictionary2.Keys)
				{
					SessionStateProviderEntry sessionStateProviderEntry = dictionary2[key2];
					if (sessionStateProviderEntry.PSSnapIn == null && psSnapInInfo != null)
					{
						sessionStateProviderEntry.SetPSSnapIn(psSnapInInfo);
					}
					SessionStateProviderEntry sessionStateProviderEntry2 = (SessionStateProviderEntry)sessionStateProviderEntry.Clone();
					if (sessionStateProviderEntry2.PSSnapIn != null && psSnapInInfo == null)
					{
						sessionStateProviderEntry2.SetPSSnapIn(null);
					}
					providers[key2] = sessionStateProviderEntry2;
				}
			}
			string assemblyLocation = ClrFacade.GetAssemblyLocation(assembly);
			Type[] assemblyTypes;
			if (cmdlets == null && providers == null)
			{
				PSSnapInHelpers._PSSnapInTracer.WriteLine("Analyzing assembly {0} for cmdlet and providers", new object[]
				{
					assemblyLocation
				});
				helpFile = PSSnapInHelpers.GetHelpFile(assemblyLocation);
				assemblyTypes = PSSnapInHelpers.GetAssemblyTypes(assembly, name);
				PSSnapInHelpers.ExecuteModuleInitializer(assembly, assemblyTypes, isModuleLoad);
				Type type = null;
				Type type2 = null;
				foreach (Type type3 in assemblyTypes)
				{
					TypeInfo typeInfo = type3.GetTypeInfo();
					if ((typeInfo.IsPublic || typeInfo.IsNestedPublic) && !typeInfo.IsAbstract)
					{
						if (PSSnapInHelpers.IsCmdletClass(type3) && PSSnapInHelpers.HasDefaultConstructor(type3))
						{
							type = type3;
							CmdletAttribute customAttribute = PSSnapInHelpers.GetCustomAttribute<CmdletAttribute>(typeInfo);
							if (customAttribute != null)
							{
								string cmdletName = PSSnapInHelpers.GetCmdletName(customAttribute);
								if (!string.IsNullOrEmpty(cmdletName))
								{
									if (cmdlets != null && cmdlets.ContainsKey(cmdletName))
									{
										string text = StringUtil.Format(ConsoleInfoErrorStrings.PSSnapInDuplicateCmdlets, cmdletName, name);
										PSSnapInHelpers._PSSnapInTracer.TraceError(text, new object[0]);
										throw new PSSnapInException(name, text);
									}
									SessionStateCmdletEntry sessionStateCmdletEntry2 = new SessionStateCmdletEntry(cmdletName, type3, helpFile);
									if (psSnapInInfo != null)
									{
										sessionStateCmdletEntry2.SetPSSnapIn(psSnapInInfo);
									}
									if (cmdlets == null)
									{
										cmdlets = new Dictionary<string, SessionStateCmdletEntry>(StringComparer.OrdinalIgnoreCase);
									}
									cmdlets.Add(cmdletName, sessionStateCmdletEntry2);
									AliasAttribute customAttribute2 = PSSnapInHelpers.GetCustomAttribute<AliasAttribute>(typeInfo);
									if (customAttribute2 != null)
									{
										if (aliases == null)
										{
											aliases = new Dictionary<string, List<SessionStateAliasEntry>>(StringComparer.OrdinalIgnoreCase);
										}
										List<SessionStateAliasEntry> list2 = new List<SessionStateAliasEntry>();
										foreach (string name2 in customAttribute2.AliasNames)
										{
											SessionStateAliasEntry sessionStateAliasEntry3 = new SessionStateAliasEntry(name2, cmdletName, "", ScopedItemOptions.None);
											if (psSnapInInfo != null)
											{
												sessionStateAliasEntry3.SetPSSnapIn(psSnapInInfo);
											}
											list2.Add(sessionStateAliasEntry3);
										}
										aliases.Add(cmdletName, list2);
									}
									PSSnapInHelpers._PSSnapInTracer.WriteLine("{0} from type {1} is added as a cmdlet. ", new object[]
									{
										cmdletName,
										type3.FullName
									});
								}
							}
						}
						else if (PSSnapInHelpers.IsProviderClass(type3) && PSSnapInHelpers.HasDefaultConstructor(type3))
						{
							type2 = type3;
							CmdletProviderAttribute customAttribute3 = PSSnapInHelpers.GetCustomAttribute<CmdletProviderAttribute>(typeInfo);
							if (customAttribute3 != null)
							{
								string providerName = PSSnapInHelpers.GetProviderName(customAttribute3);
								if (!string.IsNullOrEmpty(providerName))
								{
									if (providers != null && providers.ContainsKey(providerName))
									{
										string text2 = StringUtil.Format(ConsoleInfoErrorStrings.PSSnapInDuplicateProviders, providerName, psSnapInInfo.Name);
										PSSnapInHelpers._PSSnapInTracer.TraceError(text2, new object[0]);
										throw new PSSnapInException(psSnapInInfo.Name, text2);
									}
									SessionStateProviderEntry sessionStateProviderEntry3 = new SessionStateProviderEntry(providerName, type3, helpFile);
									sessionStateProviderEntry3.SetPSSnapIn(psSnapInInfo);
									if (moduleInfo != null)
									{
										sessionStateProviderEntry3.SetModule(moduleInfo);
									}
									if (providers == null)
									{
										providers = new Dictionary<string, SessionStateProviderEntry>(StringComparer.OrdinalIgnoreCase);
									}
									providers.Add(providerName, sessionStateProviderEntry3);
									PSSnapInHelpers._PSSnapInTracer.WriteLine("{0} from type {1} is added as a provider. ", new object[]
									{
										providerName,
										type3.FullName
									});
								}
							}
						}
					}
				}
				if (providers == null || providers.Count == 0)
				{
					if (cmdlets != null)
					{
						if (cmdlets.Count != 0)
						{
							goto IL_628;
						}
					}
					try
					{
						if (type != null)
						{
							ConstructorInfo constructor = type.GetConstructor(PSTypeExtensions.EmptyTypes);
							if (constructor != null)
							{
								constructor.Invoke(null);
							}
						}
						if (type2 != null)
						{
							ConstructorInfo constructor2 = type2.GetConstructor(PSTypeExtensions.EmptyTypes);
							if (constructor2 != null)
							{
								constructor2.Invoke(null);
							}
						}
					}
					catch (TargetInvocationException ex)
					{
						throw ex.InnerException;
					}
				}
				IL_628:
				if (cmdlets != null)
				{
					Dictionary<string, Tuple<SessionStateCmdletEntry, List<SessionStateAliasEntry>>> dictionary3 = new Dictionary<string, Tuple<SessionStateCmdletEntry, List<SessionStateAliasEntry>>>(cmdlets.Count, StringComparer.OrdinalIgnoreCase);
					List<SessionStateAliasEntry> item = null;
					foreach (KeyValuePair<string, SessionStateCmdletEntry> keyValuePair in cmdlets)
					{
						if (aliases != null && aliases.ContainsKey(keyValuePair.Key))
						{
							item = (from aliasEntry in aliases[keyValuePair.Key]
							select aliasEntry.Clone()).Cast<SessionStateAliasEntry>().ToList<SessionStateAliasEntry>();
						}
						dictionary3[keyValuePair.Key] = new Tuple<SessionStateCmdletEntry, List<SessionStateAliasEntry>>((SessionStateCmdletEntry)keyValuePair.Value.Clone(), item);
					}
					PSSnapInHelpers._cmdletCache.Value[assembly] = dictionary3;
				}
				if (providers != null)
				{
					Dictionary<string, SessionStateProviderEntry> dictionary4 = new Dictionary<string, SessionStateProviderEntry>(providers.Count, StringComparer.OrdinalIgnoreCase);
					foreach (KeyValuePair<string, SessionStateProviderEntry> keyValuePair2 in providers)
					{
						dictionary4[keyValuePair2.Key] = (SessionStateProviderEntry)keyValuePair2.Value.Clone();
					}
					PSSnapInHelpers._providerCache.Value[assembly] = providers;
				}
				return;
			}
			if (!PSSnapInHelpers._assembliesWithModuleInitializerCache.Value.ContainsKey(assembly))
			{
				PSSnapInHelpers._PSSnapInTracer.WriteLine("Returning cached cmdlet and provider entries for {0}", new object[]
				{
					assemblyLocation
				});
				return;
			}
			PSSnapInHelpers._PSSnapInTracer.WriteLine("Executing IModuleAssemblyInitializer.Import for {0}", new object[]
			{
				assemblyLocation
			});
			assemblyTypes = PSSnapInHelpers.GetAssemblyTypes(assembly, name);
			PSSnapInHelpers.ExecuteModuleInitializer(assembly, assemblyTypes, isModuleLoad);
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x001AF8C4 File Offset: 0x001ADAC4
		private static void ExecuteModuleInitializer(Assembly assembly, Type[] assemblyTypes, bool isModuleLoad)
		{
			foreach (Type type in assemblyTypes)
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				if ((typeInfo.IsPublic || typeInfo.IsNestedPublic) && !typeInfo.IsAbstract && isModuleLoad && typeof(IModuleAssemblyInitializer).IsAssignableFrom(type) && type != typeof(IModuleAssemblyInitializer))
				{
					PSSnapInHelpers._assembliesWithModuleInitializerCache.Value[assembly] = true;
					IModuleAssemblyInitializer moduleAssemblyInitializer = (IModuleAssemblyInitializer)PSSnapInHelpers.CreateModuleInitializerInstance.Target(PSSnapInHelpers.CreateModuleInitializerInstance, type);
					moduleAssemblyInitializer.OnImport();
				}
			}
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x001AF964 File Offset: 0x001ADB64
		internal static Type[] GetAssemblyTypes(Assembly assembly, string name)
		{
			Type[] result = null;
			try
			{
				IEnumerable<Type> exportedTypes = assembly.ExportedTypes;
				result = ((exportedTypes as Type[]) ?? exportedTypes.ToArray<Type>());
			}
			catch (ReflectionTypeLoadException ex)
			{
				string text = ex.Message;
				text += "\nLoader Exceptions: \n";
				if (ex.LoaderExceptions != null)
				{
					foreach (Exception ex2 in ex.LoaderExceptions)
					{
						text = text + "\n" + ex2.Message;
					}
				}
				PSSnapInHelpers._PSSnapInTracer.TraceError(text, new object[0]);
				throw new PSSnapInException(name, text);
			}
			return result;
		}

		// Token: 0x060050D4 RID: 20692 RVA: 0x001AFA08 File Offset: 0x001ADC08
		private static string GetCmdletName(CmdletAttribute cmdletAttribute)
		{
			string verbName = cmdletAttribute.VerbName;
			string nounName = cmdletAttribute.NounName;
			return verbName + "-" + nounName;
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x001AFA2F File Offset: 0x001ADC2F
		private static string GetProviderName(CmdletProviderAttribute providerAttribute)
		{
			return providerAttribute.ProviderName;
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x001AFA37 File Offset: 0x001ADC37
		private static bool IsCmdletClass(Type type)
		{
			return !(type == null) && type.IsSubclassOf(typeof(Cmdlet));
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x001AFA54 File Offset: 0x001ADC54
		private static bool IsProviderClass(Type type)
		{
			return !(type == null) && type.IsSubclassOf(typeof(CmdletProvider));
		}

		// Token: 0x060050D8 RID: 20696 RVA: 0x001AFA71 File Offset: 0x001ADC71
		internal static bool IsModuleAssemblyInitializerClass(Type type)
		{
			return !(type == null) && type.IsSubclassOf(typeof(IModuleAssemblyInitializer));
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x001AFA8E File Offset: 0x001ADC8E
		private static bool HasDefaultConstructor(Type type)
		{
			return !(type.GetConstructor(PSTypeExtensions.EmptyTypes) == null);
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x001AFAA4 File Offset: 0x001ADCA4
		private static string GetHelpFile(string assemblyPath)
		{
			return Path.GetFileName(assemblyPath) + "-Help.xml";
		}

		// Token: 0x0400295A RID: 10586
		internal static readonly CallSite<Func<CallSite, object, object>> CreateModuleInitializerInstance = CallSite<Func<CallSite, object, object>>.Create(PSCreateInstanceBinder.Get(new CallInfo(0, new string[0]), null, false));

		// Token: 0x0400295B RID: 10587
		private static Lazy<ConcurrentDictionary<Assembly, Dictionary<string, Tuple<SessionStateCmdletEntry, List<SessionStateAliasEntry>>>>> _cmdletCache = new Lazy<ConcurrentDictionary<Assembly, Dictionary<string, Tuple<SessionStateCmdletEntry, List<SessionStateAliasEntry>>>>>();

		// Token: 0x0400295C RID: 10588
		private static Lazy<ConcurrentDictionary<Assembly, Dictionary<string, SessionStateProviderEntry>>> _providerCache = new Lazy<ConcurrentDictionary<Assembly, Dictionary<string, SessionStateProviderEntry>>>();

		// Token: 0x0400295D RID: 10589
		private static Lazy<ConcurrentDictionary<Assembly, bool>> _assembliesWithModuleInitializerCache = new Lazy<ConcurrentDictionary<Assembly, bool>>();

		// Token: 0x0400295E RID: 10590
		private static PSTraceSource _PSSnapInTracer = PSTraceSource.GetTracer("PSSnapInLoadUnload", "Loading and unloading mshsnapins", false);
	}
}
