using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;
using Microsoft.Management.Infrastructure.Serialization;
using Microsoft.PowerShell.Commands;

namespace Microsoft.PowerShell.DesiredStateConfiguration.Internal
{
	// Token: 0x020009FD RID: 2557
	public static class DscClassCache
	{
		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06005DA3 RID: 23971 RVA: 0x001FF2A5 File Offset: 0x001FD4A5
		private static Dictionary<string, CimClass> ClassCache
		{
			get
			{
				if (DscClassCache._classCache == null)
				{
					DscClassCache._classCache = new Dictionary<string, CimClass>(StringComparer.OrdinalIgnoreCase);
				}
				return DscClassCache._classCache;
			}
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06005DA4 RID: 23972 RVA: 0x001FF2C2 File Offset: 0x001FD4C2
		private static Dictionary<string, Tuple<string, Version>> ByClassModuleCache
		{
			get
			{
				if (DscClassCache._byClassModuleCache == null)
				{
					DscClassCache._byClassModuleCache = new Dictionary<string, Tuple<string, Version>>(StringComparer.OrdinalIgnoreCase);
				}
				return DscClassCache._byClassModuleCache;
			}
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06005DA5 RID: 23973 RVA: 0x001FF2DF File Offset: 0x001FD4DF
		private static Dictionary<string, List<CimClass>> ByFileClassCache
		{
			get
			{
				if (DscClassCache._byFileClassCache == null)
				{
					DscClassCache._byFileClassCache = new Dictionary<string, List<CimClass>>(StringComparer.OrdinalIgnoreCase);
				}
				return DscClassCache._byFileClassCache;
			}
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06005DA6 RID: 23974 RVA: 0x001FF2FC File Offset: 0x001FD4FC
		private static HashSet<string> ScriptKeywordFileCache
		{
			get
			{
				if (DscClassCache._scriptKeywordFileCache == null)
				{
					DscClassCache._scriptKeywordFileCache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				}
				return DscClassCache._scriptKeywordFileCache;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06005DA7 RID: 23975 RVA: 0x001FF319 File Offset: 0x001FD519
		// (set) Token: 0x06005DA8 RID: 23976 RVA: 0x001FF320 File Offset: 0x001FD520
		private static bool CacheResourcesFromMultipleModuleVersions
		{
			get
			{
				return DscClassCache._cacheResourcesFromMultipleModuleVersions;
			}
			set
			{
				DscClassCache._cacheResourcesFromMultipleModuleVersions = value;
			}
		}

		// Token: 0x06005DA9 RID: 23977 RVA: 0x001FF328 File Offset: 0x001FD528
		public static void Initialize()
		{
			DscClassCache.Initialize(null, null);
		}

		// Token: 0x06005DAA RID: 23978 RVA: 0x001FF340 File Offset: 0x001FD540
		public static void Initialize(Collection<Exception> errors, List<string> modulePathList)
		{
			DscClassCache.tracer.WriteLine("Initializing DSC class cache force={0}", new object[0]);
			string text = Path.Combine(Environment.SystemDirectory, "Configuration");
			string environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles");
			string text2 = Path.Combine(environmentVariable, "WindowsPowerShell\\Configuration");
			string[] array = new string[]
			{
				text,
				text2
			};
			DscClassCache.ClearCache();
			string path = Path.Combine(text, "BaseRegistration\\BaseResource.Schema.mof");
			DscClassCache.ImportClasses(path, DscClassCache.DefaultModuleInfoForResource, errors);
			string path2 = Path.Combine(text, "BaseRegistration\\MSFT_DSCMetaConfiguration.mof");
			DscClassCache.ImportClasses(path2, DscClassCache.DefaultModuleInfoForResource, errors);
			string path3 = Path.Combine(text, "BaseRegistration\\MSFT_MetaConfigurationExtensionClasses.Schema.mof");
			DscClassCache.ImportClasses(path3, DscClassCache.DefaultModuleInfoForMetaConfigResource, errors);
			foreach (string path4 in array)
			{
				string path5 = Path.Combine(path4, "Schema");
				if (Directory.Exists(path5))
				{
					foreach (string path6 in Directory.EnumerateDirectories(path5).SelectMany((string d) => Directory.EnumerateFiles(d, "*.schema.mof")))
					{
						DscClassCache.ImportClasses(path6, DscClassCache.DefaultModuleInfoForResource, errors);
					}
				}
			}
			bool isInboxResource = false;
			List<string> list = new List<string>();
			if (modulePathList == null || modulePathList.Count == 0)
			{
				list.Add(Path.Combine(Environment.SystemDirectory, "WindowsPowershell\\v1.0\\Modules\\PsDesiredStateConfiguration"));
				isInboxResource = true;
			}
			else
			{
				foreach (string path7 in modulePathList)
				{
					if (Directory.Exists(path7))
					{
						foreach (string item in Directory.EnumerateDirectories(path7))
						{
							list.Add(item);
						}
					}
				}
			}
			DscClassCache.LoadDSCResourceIntoCache(errors, list, isInboxResource);
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x001FF560 File Offset: 0x001FD760
		private static void LoadDSCResourceIntoCache(Collection<Exception> errors, List<string> modulePathList, bool isInboxResource)
		{
			foreach (string text in modulePathList)
			{
				if (Directory.Exists(text))
				{
					string path = Path.Combine(text, "DSCResources");
					if (Directory.Exists(path))
					{
						foreach (string path2 in Directory.EnumerateDirectories(path))
						{
							IEnumerable<string> enumerable = Directory.EnumerateFiles(path2, "*.schema.mof");
							if (enumerable.Any<string>())
							{
								Tuple<string, Version> moduleInfoHelper = DscClassCache.GetModuleInfoHelper(text, isInboxResource, false);
								if (moduleInfoHelper != null)
								{
									foreach (string path3 in enumerable)
									{
										DscClassCache.ImportClasses(path3, moduleInfoHelper, errors);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06005DAC RID: 23980 RVA: 0x001FF670 File Offset: 0x001FD870
		private static Tuple<string, Version> GetModuleInfoHelper(string moduleFolderPath, bool isInboxResource, bool isPsProviderModule)
		{
			string text = "PsDesiredStateConfiguration";
			if (!isInboxResource)
			{
				text = Path.GetFileName(moduleFolderPath);
			}
			string text2 = Path.Combine(moduleFolderPath, text + ".psd1");
			DscClassCache.tracer.WriteLine("DSC GetModuleVersion: Try retrieving module version information from file: {0}.", new object[]
			{
				text2
			});
			if (File.Exists(text2))
			{
				try
				{
					Hashtable moduleManifestProperties = PsUtils.GetModuleManifestProperties(text2, PsUtils.ManifestModuleVersionPropertyName);
					object obj = moduleManifestProperties["ModuleVersion"];
					if (obj != null)
					{
						Version item;
						if (LanguagePrimitives.TryConvertTo<Version>(obj, out item))
						{
							return new Tuple<string, Version>(text, item);
						}
						DscClassCache.tracer.WriteLine("DSC GetModuleVersion: ModuleVersion value '{0}' cannot be converted to System.Version. Skip the module '{1}'.", new object[]
						{
							obj,
							text
						});
					}
					else
					{
						DscClassCache.tracer.WriteLine("DSC GetModuleVersion: Manifest file '{0}' does not contain ModuleVersion. Skip the module '{1}'.", new object[]
						{
							text2,
							text
						});
					}
				}
				catch (PSInvalidOperationException ex)
				{
					DscClassCache.tracer.WriteLine("DSC GetModuleVersion: Error evaluating module manifest file '{0}', with error '{1}'. Skip the module '{2}'.", new object[]
					{
						text2,
						ex,
						text
					});
				}
				return null;
			}
			if (isPsProviderModule)
			{
				return new Tuple<string, Version>(text, new Version("1.0"));
			}
			DscClassCache.tracer.WriteLine("DSC GetModuleVersion: Manifest file '{0}' not exist.", new object[]
			{
				text2
			});
			return null;
		}

		// Token: 0x06005DAD RID: 23981 RVA: 0x001FF7B8 File Offset: 0x001FD9B8
		private static CimClass MyClassCallback(string serverName, string namespaceName, string className)
		{
			foreach (KeyValuePair<string, CimClass> keyValuePair in DscClassCache.ClassCache)
			{
				string strA = keyValuePair.Key.Split(new char[]
				{
					'\\'
				})[2];
				if (string.Compare(strA, className, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06005DAE RID: 23982 RVA: 0x001FF83C File Offset: 0x001FDA3C
		public static List<CimClass> ImportClasses(string path, Tuple<string, Version> moduleInfo, Collection<Exception> errors)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			DscClassCache.tracer.WriteLine("DSC ClassCache: importing file: {0}", new object[]
			{
				path
			});
			CimDSCParser cimDSCParser = new CimDSCParser(new CimMofDeserializer.OnClassNeeded(DscClassCache.MyClassCallback));
			List<CimClass> list = null;
			try
			{
				list = cimDSCParser.ParseSchemaMof(path);
			}
			catch (PSInvalidOperationException ex)
			{
				DscClassCache.tracer.WriteLine("DSC ClassCache: Error importing file '{0}', with error '{1}'.  Skipping file.", new object[]
				{
					path,
					ex
				});
				if (errors != null)
				{
					errors.Add(ex);
				}
			}
			if (list != null)
			{
				foreach (CimClass cimClass in list)
				{
					string className = cimClass.CimSystemProperties.ClassName;
					string moduleQualifiedResourceName = DscClassCache.GetModuleQualifiedResourceName(moduleInfo.Item1, moduleInfo.Item2.ToString(), className);
					if (DscClassCache.ClassCache.ContainsKey(moduleQualifiedResourceName) && !DscClassCache.IsSameNestedObject(DscClassCache.ClassCache[moduleQualifiedResourceName], cimClass))
					{
						string text = string.Join(",", DscClassCache.GetFileDefiningClass(className));
						PSInvalidOperationException ex2 = PSTraceSource.NewInvalidOperationException(ParserStrings.DuplicateCimClassDefinition, new object[]
						{
							className,
							path,
							text
						});
						ex2.SetErrorId("DuplicateCimClassDefinition");
						if (errors != null)
						{
							errors.Add(ex2);
						}
					}
					if (!DscClassCache.hiddenResourceCache.Contains(className))
					{
						if (!DscClassCache.CacheResourcesFromMultipleModuleVersions)
						{
							List<KeyValuePair<string, CimClass>> list2 = DscClassCache.FindResourceInCache(moduleInfo.Item1, className);
							if (list2.Count > 0 && !string.IsNullOrEmpty(list2.First<KeyValuePair<string, CimClass>>().Key))
							{
								DscClassCache.ClassCache.Remove(list2.First<KeyValuePair<string, CimClass>>().Key);
							}
						}
						DscClassCache.ClassCache[moduleQualifiedResourceName] = cimClass;
						DscClassCache.ByClassModuleCache[className] = moduleInfo;
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (CimClass cimClass2 in list)
				{
					stringBuilder.Append(cimClass2.CimSystemProperties.ClassName);
					stringBuilder.Append(",");
				}
				DscClassCache.tracer.WriteLine("DSC ClassCache: loading file '{0}' added the following classes to the cache: {1}", new object[]
				{
					path,
					stringBuilder.ToString()
				});
			}
			else
			{
				DscClassCache.tracer.WriteLine("DSC ClassCache: loading file '{0}' added no classes to the cache.", new object[0]);
			}
			DscClassCache.ByFileClassCache[path] = list;
			return list;
		}

		// Token: 0x06005DAF RID: 23983 RVA: 0x001FFB00 File Offset: 0x001FDD00
		public static string GetStringFromSecureString(SecureString value)
		{
			string result = string.Empty;
			if (value != null)
			{
				IntPtr intPtr = Marshal.SecureStringToCoTaskMemUnicode(value);
				result = Marshal.PtrToStringUni(intPtr);
				Marshal.ZeroFreeCoTaskMemUnicode(intPtr);
			}
			return result;
		}

		// Token: 0x06005DB0 RID: 23984 RVA: 0x001FFB2C File Offset: 0x001FDD2C
		public static void ClearCache()
		{
			DscClassCache.tracer.WriteLine("DSC class: clearing the cache and associated keywords.", new object[0]);
			DscClassCache.ClassCache.Clear();
			DscClassCache.ByClassModuleCache.Clear();
			DscClassCache.ByFileClassCache.Clear();
			DscClassCache.ScriptKeywordFileCache.Clear();
			DscClassCache.CacheResourcesFromMultipleModuleVersions = false;
		}

		// Token: 0x06005DB1 RID: 23985 RVA: 0x001FFB7C File Offset: 0x001FDD7C
		private static string GetModuleQualifiedResourceName(string moduleName, string moduleVersion, string className)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}\\{1}\\{2}", new object[]
			{
				moduleName,
				moduleVersion,
				className
			});
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x001FFF98 File Offset: 0x001FE198
		private static List<KeyValuePair<string, CimClass>> FindResourceInCache(string moduleName, string className)
		{
			return (from cacheEntry in DscClassCache.ClassCache
			let splittedName = cacheEntry.Key.Split(new char[]
			{
				'\\'
			})
			let cachedClassName = splittedName[2]
			let cachedModuleName = splittedName[0]
			where string.Compare(cachedClassName, className, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(cachedModuleName, moduleName, StringComparison.OrdinalIgnoreCase) == 0
			select cacheEntry).ToList<KeyValuePair<string, CimClass>>();
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x0020005C File Offset: 0x001FE25C
		public static List<CimClass> GetCachedClasses()
		{
			return DscClassCache.ClassCache.Values.ToList<CimClass>();
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x00200090 File Offset: 0x001FE290
		public static List<string> GetFileDefiningClass(string className)
		{
			List<string> list = new List<string>();
			foreach (string text in DscClassCache.ByFileClassCache.Keys.ToArray<string>())
			{
				List<CimClass> list2 = DscClassCache.ByFileClassCache[text];
				if (list2 != null)
				{
					if (DscClassCache.ByFileClassCache[text].FirstOrDefault((CimClass c) => string.Equals(c.CimSystemProperties.ClassName, className, StringComparison.OrdinalIgnoreCase)) != null)
					{
						list.Add(text);
					}
				}
			}
			return list;
		}

		// Token: 0x06005DB5 RID: 23989 RVA: 0x00200119 File Offset: 0x001FE319
		public static string[] GetLoadedFiles()
		{
			return DscClassCache.ByFileClassCache.Keys.ToArray<string>();
		}

		// Token: 0x06005DB6 RID: 23990 RVA: 0x0020012A File Offset: 0x001FE32A
		public static List<CimClass> GetCachedClassByFileName(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw PSTraceSource.NewArgumentNullException("fileName");
			}
			if (!DscClassCache.ByFileClassCache.ContainsKey(fileName))
			{
				return null;
			}
			return DscClassCache.ByFileClassCache[fileName];
		}

		// Token: 0x06005DB7 RID: 23991 RVA: 0x00200180 File Offset: 0x001FE380
		public static List<CimClass> GetCachedClassByModuleName(string moduleName)
		{
			if (string.IsNullOrWhiteSpace(moduleName))
			{
				throw PSTraceSource.NewArgumentNullException("moduleName");
			}
			string moduleFileName = moduleName + ".schema.mof";
			return (from filename in DscClassCache.ByFileClassCache.Keys
			where string.Equals(Path.GetFileName(filename), moduleFileName, StringComparison.OrdinalIgnoreCase)
			select DscClassCache.GetCachedClassByFileName(filename)).FirstOrDefault<List<CimClass>>();
		}

		// Token: 0x06005DB8 RID: 23992 RVA: 0x002001FC File Offset: 0x001FE3FC
		public static List<CimInstance> ImportInstances(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CimDSCParser cimDSCParser = new CimDSCParser(new CimMofDeserializer.OnClassNeeded(DscClassCache.MyClassCallback));
			return cimDSCParser.ParseInstanceMof(path);
		}

		// Token: 0x06005DB9 RID: 23993 RVA: 0x00200238 File Offset: 0x001FE438
		public static List<CimInstance> ImportInstances(string path, int schemaValidationOption)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (schemaValidationOption < 0 || schemaValidationOption > 4)
			{
				throw new IndexOutOfRangeException("schemaValidationOption");
			}
			CimDSCParser cimDSCParser = new CimDSCParser(new CimMofDeserializer.OnClassNeeded(DscClassCache.MyClassCallback), (MofDeserializerSchemaValidationOption)schemaValidationOption);
			return cimDSCParser.ParseInstanceMof(path);
		}

		// Token: 0x06005DBA RID: 23994 RVA: 0x00200288 File Offset: 0x001FE488
		public static void ValidateInstanceText(string instanceText)
		{
			if (string.IsNullOrEmpty(instanceText))
			{
				throw PSTraceSource.NewArgumentNullException("instanceText");
			}
			CimDSCParser cimDSCParser = new CimDSCParser(new CimMofDeserializer.OnClassNeeded(DscClassCache.MyClassCallback));
			cimDSCParser.ValidateInstanceText(instanceText);
		}

		// Token: 0x06005DBB RID: 23995 RVA: 0x002002C1 File Offset: 0x001FE4C1
		private static bool IsMagicProperty(string propertyName)
		{
			return Regex.Match(propertyName, "^(ResourceId|SourceInfo|ModuleName|ModuleVersion|ConfigurationName)$", RegexOptions.IgnoreCase).Success;
		}

		// Token: 0x06005DBC RID: 23996 RVA: 0x002002D4 File Offset: 0x001FE4D4
		private static string GetFriendlyName(CimClass cimClass)
		{
			try
			{
				CimQualifier cimQualifier = cimClass.CimClassQualifiers["FriendlyName"];
				if (cimQualifier != null)
				{
					return cimQualifier.Value as string;
				}
			}
			catch (CimException)
			{
			}
			return null;
		}

		// Token: 0x06005DBD RID: 23997 RVA: 0x0020031C File Offset: 0x001FE51C
		public static Collection<DynamicKeyword> GetCachedKeywords()
		{
			Collection<DynamicKeyword> collection = new Collection<DynamicKeyword>();
			foreach (KeyValuePair<string, CimClass> keyValuePair in DscClassCache.ClassCache)
			{
				string[] array = keyValuePair.Key.Split(new char[]
				{
					'\\'
				});
				string moduleName = array[0];
				string input = array[1];
				DynamicKeyword dynamicKeyword = DscClassCache.CreateKeywordFromCimClass(moduleName, Version.Parse(input), keyValuePair.Value, null, DSCResourceRunAsCredential.Default);
				if (dynamicKeyword != null)
				{
					collection.Add(dynamicKeyword);
				}
			}
			return collection;
		}

		// Token: 0x06005DBE RID: 23998 RVA: 0x002003BC File Offset: 0x001FE5BC
		private static void CreateAndRegisterKeywordFromCimClass(string moduleName, Version moduleVersion, CimClass cimClass, Dictionary<string, ScriptBlock> functionsToDefine, DSCResourceRunAsCredential runAsBehavior)
		{
			DynamicKeyword dynamicKeyword = DscClassCache.CreateKeywordFromCimClass(moduleName, moduleVersion, cimClass, functionsToDefine, runAsBehavior);
			if (dynamicKeyword == null)
			{
				return;
			}
			if (!DscClassCache.CacheResourcesFromMultipleModuleVersions && DynamicKeyword.ContainsKeyword(dynamicKeyword.Keyword))
			{
				DynamicKeyword keyword = DynamicKeyword.GetKeyword(dynamicKeyword.Keyword);
				if (!keyword.ImplementingModule.Equals(moduleName, StringComparison.OrdinalIgnoreCase) || keyword.ImplementingModuleVersion != moduleVersion)
				{
					PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.DuplicateKeywordDefinition, new object[]
					{
						dynamicKeyword.Keyword
					});
					ex.SetErrorId("DuplicateKeywordDefinition");
					throw ex;
				}
			}
			DynamicKeyword.AddKeyword(dynamicKeyword);
			if (functionsToDefine != null)
			{
				functionsToDefine[moduleName + "\\" + dynamicKeyword.Keyword] = DscClassCache.CimKeywordImlementationFunction;
			}
		}

		// Token: 0x06005DBF RID: 23999 RVA: 0x00200464 File Offset: 0x001FE664
		private static DynamicKeyword CreateKeywordFromCimClass(string moduleName, Version moduleVersion, CimClass cimClass, Dictionary<string, ScriptBlock> functionsToDefine, DSCResourceRunAsCredential runAsBehavior)
		{
			string className = cimClass.CimSystemProperties.ClassName;
			string friendlyName = DscClassCache.GetFriendlyName(cimClass);
			string text = string.IsNullOrEmpty(friendlyName) ? className : friendlyName;
			if (Regex.Match(text, "^OMI_Base|^OMI_.*Registration", RegexOptions.IgnoreCase).Success)
			{
				return null;
			}
			DynamicKeyword dynamicKeyword = new DynamicKeyword
			{
				BodyMode = DynamicKeywordBodyMode.Hashtable,
				Keyword = text,
				ResourceName = className,
				ImplementingModule = moduleName,
				ImplementingModuleVersion = moduleVersion
			};
			if (Regex.Match(text, "^(Synchronization|Certificate|IIS|SQL)$", RegexOptions.IgnoreCase).Success)
			{
				dynamicKeyword.IsReservedKeyword = true;
			}
			bool flag = false;
			CimClass cimClass2 = cimClass;
			while (!string.IsNullOrEmpty(cimClass2.CimSuperClassName))
			{
				if (string.Equals("OMI_BaseResource", cimClass2.CimSuperClassName, StringComparison.OrdinalIgnoreCase) || string.Equals("OMI_MetaConfigurationResource", cimClass2.CimSuperClassName, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
				cimClass2 = cimClass2.CimSuperClass;
			}
			dynamicKeyword.NameMode = (flag ? DynamicKeywordNameMode.NameRequired : DynamicKeywordNameMode.NoName);
			foreach (CimPropertyDeclaration cimPropertyDeclaration in cimClass.CimClassProperties)
			{
				if ((cimPropertyDeclaration.Flags & CimFlags.ReadOnly) != CimFlags.ReadOnly)
				{
					try
					{
						if (cimPropertyDeclaration.Qualifiers["Read"] != null)
						{
							continue;
						}
					}
					catch (CimException)
					{
					}
					if (!DscClassCache.IsMagicProperty(cimPropertyDeclaration.Name) && (runAsBehavior != DSCResourceRunAsCredential.NotSupported || !string.Equals(cimPropertyDeclaration.Name, "PsDscRunAsCredential", StringComparison.OrdinalIgnoreCase)))
					{
						if (Regex.Match(cimPropertyDeclaration.Name, "^(Require|Trigger|Notify|Before|After|Subscribe)$", RegexOptions.IgnoreCase).Success)
						{
							dynamicKeyword.HasReservedProperties = true;
						}
						else
						{
							DynamicKeywordProperty dynamicKeywordProperty = new DynamicKeywordProperty();
							dynamicKeywordProperty.Name = cimPropertyDeclaration.Name;
							if ((cimPropertyDeclaration.Flags & CimFlags.Key) == CimFlags.Key)
							{
								dynamicKeywordProperty.Mandatory = true;
								dynamicKeywordProperty.IsKey = true;
							}
							if (cimPropertyDeclaration.CimType == CimType.Instance && !string.IsNullOrEmpty(cimPropertyDeclaration.ReferenceClassName))
							{
								dynamicKeywordProperty.TypeConstraint = cimPropertyDeclaration.ReferenceClassName;
							}
							else
							{
								dynamicKeywordProperty.TypeConstraint = cimPropertyDeclaration.CimType.ToString();
							}
							string[] array = null;
							foreach (CimQualifier cimQualifier in cimPropertyDeclaration.Qualifiers)
							{
								if (string.Equals(cimQualifier.Name, "Values", StringComparison.OrdinalIgnoreCase) && cimQualifier.CimType == CimType.StringArray)
								{
									dynamicKeywordProperty.Values.AddRange((string[])cimQualifier.Value);
								}
								if (string.Equals(cimQualifier.Name, "ValueMap", StringComparison.OrdinalIgnoreCase) && cimQualifier.CimType == CimType.StringArray)
								{
									array = (string[])cimQualifier.Value;
								}
								if (string.Equals(cimQualifier.Name, "Required", StringComparison.OrdinalIgnoreCase) && cimQualifier.CimType == CimType.Boolean && (bool)cimQualifier.Value)
								{
									dynamicKeywordProperty.Mandatory = true;
								}
								if (runAsBehavior == DSCResourceRunAsCredential.Mandatory && string.Equals(cimPropertyDeclaration.Name, "PsDscRunAsCredential", StringComparison.OrdinalIgnoreCase))
								{
									dynamicKeywordProperty.Mandatory = true;
								}
							}
							if (array != null && dynamicKeywordProperty.Values.Count > 0)
							{
								if (array.Length != dynamicKeywordProperty.Values.Count)
								{
									DscClassCache.tracer.WriteLine("DSC CreateDynamicKeywordFromClass: the count of values for qualifier 'Values' and 'ValueMap' doesn't match. count of 'Values': {0}, count of 'ValueMap': {1}. Skip the keyword '{2}'.", new object[]
									{
										dynamicKeywordProperty.Values.Count,
										array.Length,
										dynamicKeyword.Keyword
									});
									return null;
								}
								for (int i = 0; i < array.Length; i++)
								{
									string text2 = dynamicKeywordProperty.Values[i];
									string value = array[i];
									if (dynamicKeywordProperty.ValueMap.ContainsKey(text2))
									{
										DscClassCache.tracer.WriteLine("DSC CreateDynamicKeywordFromClass: same string value '{0}' appears more than once in qualifier 'Values'. Skip the keyword '{1}'.", new object[]
										{
											text2,
											dynamicKeyword.Keyword
										});
										return null;
									}
									dynamicKeywordProperty.ValueMap.Add(text2, value);
								}
							}
							dynamicKeyword.Properties.Add(cimPropertyDeclaration.Name, dynamicKeywordProperty);
						}
					}
				}
			}
			DscClassCache.UpdateKnownRestriction(dynamicKeyword);
			return dynamicKeyword;
		}

		// Token: 0x06005DC0 RID: 24000 RVA: 0x002008D4 File Offset: 0x001FEAD4
		private static void UpdateKnownRestriction(DynamicKeyword keyword)
		{
			if (string.Compare(keyword.ResourceName, "MSFT_DSCMetaConfigurationV2", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(keyword.ResourceName, "MSFT_DSCMetaConfiguration", StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (keyword.Properties["RefreshFrequencyMins"] != null)
				{
					keyword.Properties["RefreshFrequencyMins"].Range = new Tuple<int, int>(30, 44640);
				}
				if (keyword.Properties["ConfigurationModeFrequencyMins"] != null)
				{
					keyword.Properties["ConfigurationModeFrequencyMins"].Range = new Tuple<int, int>(15, 44640);
				}
				if (keyword.Properties["DebugMode"] != null)
				{
					keyword.Properties["DebugMode"].Values.Remove("ResourceScriptBreakAll");
					keyword.Properties["DebugMode"].ValueMap.Remove("ResourceScriptBreakAll");
				}
			}
		}

		// Token: 0x06005DC1 RID: 24001 RVA: 0x002009C2 File Offset: 0x001FEBC2
		public static void LoadDefaultCimKeywords()
		{
			DscClassCache.LoadDefaultCimKeywords(null, null, null, false);
		}

		// Token: 0x06005DC2 RID: 24002 RVA: 0x002009CD File Offset: 0x001FEBCD
		public static void LoadDefaultCimKeywords(List<string> modulePathList)
		{
			DscClassCache.LoadDefaultCimKeywords(null, null, modulePathList, false);
		}

		// Token: 0x06005DC3 RID: 24003 RVA: 0x002009D8 File Offset: 0x001FEBD8
		public static void LoadDefaultCimKeywords(Collection<Exception> errors)
		{
			DscClassCache.LoadDefaultCimKeywords(null, errors, null, false);
		}

		// Token: 0x06005DC4 RID: 24004 RVA: 0x002009E3 File Offset: 0x001FEBE3
		public static void LoadDefaultCimKeywords(Dictionary<string, ScriptBlock> functionsToDefine)
		{
			DscClassCache.LoadDefaultCimKeywords(functionsToDefine, null, null, false);
		}

		// Token: 0x06005DC5 RID: 24005 RVA: 0x002009EE File Offset: 0x001FEBEE
		public static void LoadDefaultCimKeywords(Collection<Exception> errors, bool cacheResourcesFromMultipleModuleVersions)
		{
			DscClassCache.LoadDefaultCimKeywords(null, errors, null, cacheResourcesFromMultipleModuleVersions);
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x002009FC File Offset: 0x001FEBFC
		private static void LoadDefaultCimKeywords(Dictionary<string, ScriptBlock> functionsToDefine, Collection<Exception> errors, List<string> modulePathList, bool cacheResourcesFromMultipleModuleVersions)
		{
			DynamicKeyword.Reset();
			DscClassCache.Initialize(errors, modulePathList);
			DscClassCache.CacheResourcesFromMultipleModuleVersions = cacheResourcesFromMultipleModuleVersions;
			foreach (CimClass cimClass in DscClassCache.GetCachedClasses())
			{
				string className = cimClass.CimSystemProperties.ClassName;
				Tuple<string, Version> tuple = DscClassCache.ByClassModuleCache[className];
				DscClassCache.CreateAndRegisterKeywordFromCimClass(tuple.Item1, tuple.Item2, cimClass, functionsToDefine, DSCResourceRunAsCredential.Default);
			}
			if (!DynamicKeyword.ContainsKeyword("Node"))
			{
				DynamicKeyword keywordToAdd = new DynamicKeyword
				{
					BodyMode = DynamicKeywordBodyMode.ScriptBlock,
					ImplementingModule = DscClassCache.DefaultModuleInfoForResource.Item1,
					ImplementingModuleVersion = DscClassCache.DefaultModuleInfoForResource.Item2,
					NameMode = DynamicKeywordNameMode.NameRequired,
					Keyword = "Node"
				};
				DynamicKeyword.AddKeyword(keywordToAdd);
			}
			if (!DynamicKeyword.ContainsKeyword("Import-DscResource"))
			{
				DynamicKeyword keywordToAdd2 = new DynamicKeyword
				{
					BodyMode = DynamicKeywordBodyMode.Command,
					ImplementingModule = DscClassCache.DefaultModuleInfoForResource.Item1,
					ImplementingModuleVersion = DscClassCache.DefaultModuleInfoForResource.Item2,
					NameMode = DynamicKeywordNameMode.NoName,
					Keyword = "Import-DscResource",
					MetaStatement = true,
					PostParse = new Func<DynamicKeywordStatementAst, ParseError[]>(DscClassCache.ImportResourceCheckSemantics)
				};
				DynamicKeyword.AddKeyword(keywordToAdd2);
			}
		}

		// Token: 0x06005DC7 RID: 24007 RVA: 0x00200B58 File Offset: 0x001FED58
		private static ParseError[] ImportResourceCheckSemantics(DynamicKeywordStatementAst kwAst)
		{
			CommandElementAst[] commandElements = Ast.CopyElements<CommandElementAst>(kwAst.CommandElements);
			CommandAst commandAst = new CommandAst(kwAst.Extent, commandElements, TokenKind.Unknown, null);
			StaticBindingResult staticBindingResult = StaticParameterBinder.BindCommand(commandAst, false);
			List<ParseError> list = new List<ParseError>();
			foreach (StaticBindingError staticBindingError in staticBindingResult.BindingExceptions.Values)
			{
				list.Add(new ParseError(staticBindingError.CommandElement.Extent, "ParameterBindingException", staticBindingError.BindingException.Message));
			}
			ParameterBindingResult parameterBindingResult = null;
			ParameterBindingResult parameterBindingResult2 = null;
			ParameterBindingResult parameterBindingResult3 = null;
			foreach (KeyValuePair<string, ParameterBindingResult> keyValuePair in staticBindingResult.BoundParameters)
			{
				string key = keyValuePair.Key;
				ParameterBindingResult value = keyValuePair.Value;
				if (key.All(new Func<char, bool>(char.IsDigit)))
				{
					list.Add(new ParseError(value.Value.Extent, "ImportDscResourcePositionalParamsNotSupported", string.Format(CultureInfo.CurrentCulture, ParserStrings.ImportDscResourcePositionalParamsNotSupported, new object[0])));
				}
				else if ("Name".StartsWith(key, StringComparison.OrdinalIgnoreCase))
				{
					parameterBindingResult2 = value;
				}
				else if ("ModuleName".StartsWith(key, StringComparison.OrdinalIgnoreCase))
				{
					parameterBindingResult = value;
				}
				else if ("ModuleVersion".StartsWith(key, StringComparison.OrdinalIgnoreCase))
				{
					parameterBindingResult3 = value;
				}
				else
				{
					list.Add(new ParseError(value.Value.Extent, "ImportDscResourceNeedParams", string.Format(CultureInfo.CurrentCulture, ParserStrings.ImportDscResourceNeedParams, new object[0])));
				}
			}
			if (list.Count == 0 && parameterBindingResult == null && parameterBindingResult2 == null)
			{
				list.Add(new ParseError(kwAst.Extent, "ImportDscResourceNeedParams", string.Format(CultureInfo.CurrentCulture, ParserStrings.ImportDscResourceNeedParams, new object[0])));
			}
			if (parameterBindingResult3 != null && parameterBindingResult == null && parameterBindingResult2 != null)
			{
				list.Add(new ParseError(kwAst.Extent, "ImportDscResourceNeedModuleNameWithModuleVersion", string.Format(CultureInfo.CurrentCulture, ParserStrings.ImportDscResourceNeedParams, new object[0])));
			}
			string[] array = null;
			if (parameterBindingResult2 != null)
			{
				object valueToConvert = null;
				if (!IsConstantValueVisitor.IsConstant(parameterBindingResult2.Value, out valueToConvert, true, true) || !LanguagePrimitives.TryConvertTo<string[]>(valueToConvert, out array))
				{
					list.Add(new ParseError(parameterBindingResult2.Value.Extent, "RequiresInvalidStringArgument", string.Format(CultureInfo.CurrentCulture, ParserStrings.RequiresInvalidStringArgument, new object[]
					{
						"Name"
					})));
				}
			}
			Version version = null;
			if (parameterBindingResult3 != null)
			{
				object obj = null;
				if (!IsConstantValueVisitor.IsConstant(parameterBindingResult3.Value, out obj, true, true))
				{
					list.Add(new ParseError(parameterBindingResult3.Value.Extent, "RequiresArgumentMustBeConstant", ParserStrings.RequiresArgumentMustBeConstant));
				}
				if (obj is double)
				{
					obj = parameterBindingResult3.Value.Extent.Text;
				}
				if (!LanguagePrimitives.TryConvertTo<Version>(obj, out version))
				{
					list.Add(new ParseError(parameterBindingResult3.Value.Extent, "RequiresVersionInvalid", ParserStrings.RequiresVersionInvalid));
				}
			}
			ModuleSpecification[] array2 = null;
			if (parameterBindingResult != null)
			{
				object valueToConvert2 = null;
				if (!IsConstantValueVisitor.IsConstant(parameterBindingResult.Value, out valueToConvert2, true, true))
				{
					list.Add(new ParseError(parameterBindingResult.Value.Extent, "RequiresArgumentMustBeConstant", ParserStrings.RequiresArgumentMustBeConstant));
				}
				if (LanguagePrimitives.TryConvertTo<ModuleSpecification[]>(valueToConvert2, out array2))
				{
					if (array2 != null && array2.Length > 1 && array != null)
					{
						list.Add(new ParseError(parameterBindingResult.Value.Extent, "ImportDscResourceMultipleModulesNotSupportedWithName", string.Format(CultureInfo.CurrentCulture, ParserStrings.ImportDscResourceMultipleModulesNotSupportedWithName, new object[0])));
					}
					if (array2 != null && array2.Length > 1 && version != null)
					{
						list.Add(new ParseError(parameterBindingResult.Value.Extent, "ImportDscResourceMultipleModulesNotSupportedWithVersion", string.Format(CultureInfo.CurrentCulture, ParserStrings.ImportDscResourceNeedParams, new object[0])));
					}
					if (array2 != null && (array2[0].Version != null || array2[0].MaximumVersion != null) && version != null)
					{
						list.Add(new ParseError(parameterBindingResult.Value.Extent, "ImportDscResourceMultipleModuleVersionsNotSupported", string.Format(CultureInfo.CurrentCulture, ParserStrings.ImportDscResourceNeedParams, new object[0])));
					}
					if (array2 != null && array2[0].Version == null && array2[0].MaximumVersion == null && version != null)
					{
						array2[0].Version = version;
					}
				}
				else
				{
					list.Add(new ParseError(parameterBindingResult.Value.Extent, "RequiresInvalidStringArgument", string.Format(CultureInfo.CurrentCulture, ParserStrings.RequiresInvalidStringArgument, new object[]
					{
						"ModuleName"
					})));
				}
			}
			if (list.Count == 0)
			{
				DscClassCache.LoadResourcesFromModule(kwAst.Extent, array2, array, list);
			}
			return list.ToArray();
		}

		// Token: 0x06005DC8 RID: 24008 RVA: 0x00201030 File Offset: 0x001FF230
		public static void LoadResourcesFromModule(IScriptExtent scriptExtent, ModuleSpecification[] moduleSpecifications, string[] resourceNames, List<ParseError> errorList)
		{
			Collection<PSModuleInfo> collection = new Collection<PSModuleInfo>();
			if (moduleSpecifications != null)
			{
				int i = 0;
				while (i < moduleSpecifications.Length)
				{
					ModuleSpecification moduleSpecification = moduleSpecifications[i];
					bool flag = false;
					Collection<PSModuleInfo> moduleIfAvailable = ModuleCmdletBase.GetModuleIfAvailable(moduleSpecification, null);
					if (moduleIfAvailable.Count >= 1 && (moduleSpecification.Version != null || moduleSpecification.Guid != null))
					{
						using (IEnumerator<PSModuleInfo> enumerator = moduleIfAvailable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								PSModuleInfo psmoduleInfo = enumerator.Current;
								if ((moduleSpecification.Guid != null && moduleSpecification.Guid.Equals(psmoduleInfo.Guid)) || (moduleSpecification.Version != null && moduleSpecification.Version.Equals(psmoduleInfo.Version)))
								{
									collection.Add(psmoduleInfo);
									flag = true;
									break;
								}
							}
							goto IL_F9;
						}
						goto IL_E1;
					}
					goto IL_E1;
					IL_F9:
					if (!flag)
					{
						if (moduleIfAvailable.Count > 1)
						{
							errorList.Add(new ParseError(scriptExtent, "MultipleModuleEntriesFoundDuringParse", string.Format(CultureInfo.CurrentCulture, ParserStrings.MultipleModuleEntriesFoundDuringParse, new object[]
							{
								moduleSpecification.Name
							})));
						}
						else
						{
							string text = (moduleSpecification.Version == null) ? moduleSpecification.Name : string.Format(CultureInfo.CurrentCulture, "<{0}, {1}>", new object[]
							{
								moduleSpecification.Name,
								moduleSpecification.Version
							});
							errorList.Add(new ParseError(scriptExtent, "ModuleNotFoundDuringParse", string.Format(CultureInfo.CurrentCulture, ParserStrings.ModuleNotFoundDuringParse, new object[]
							{
								text
							})));
						}
						return;
					}
					i++;
					continue;
					IL_E1:
					if (moduleIfAvailable.Count == 1)
					{
						collection.Add(moduleIfAvailable[0]);
						flag = true;
						goto IL_F9;
					}
					goto IL_F9;
				}
			}
			else if (resourceNames != null)
			{
				using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
				{
					powerShell.AddCommand("Get-Module");
					powerShell.AddParameter("ListAvailable");
					collection = powerShell.Invoke<PSModuleInfo>();
				}
			}
			List<string> list = new List<string>();
			if (resourceNames == null || resourceNames.Length == 0)
			{
				list.Add("*");
			}
			else
			{
				list.AddRange(resourceNames);
			}
			foreach (PSModuleInfo psmoduleInfo2 in collection)
			{
				string path = Path.Combine(psmoduleInfo2.ModuleBase, "DscResources");
				List<string> list2 = new List<string>();
				DscClassCache.LoadPowerShellClassResourcesFromModule(psmoduleInfo2, psmoduleInfo2, list, list2, errorList, null, true, scriptExtent);
				if (Directory.Exists(path))
				{
					foreach (string text2 in list)
					{
						bool flag2 = false;
						foreach (string path2 in Directory.EnumerateDirectories(path, text2))
						{
							string fileName = Path.GetFileName(path2);
							bool flag3 = false;
							bool flag4 = false;
							string empty = string.Empty;
							try
							{
								flag3 = DscClassCache.ImportCimKeywordsFromModule(psmoduleInfo2, fileName, out empty);
							}
							catch (FileNotFoundException)
							{
								errorList.Add(new ParseError(scriptExtent, "SchemaFileNotFound", string.Format(CultureInfo.CurrentCulture, ParserStrings.SchemaFileNotFound, new object[]
								{
									empty
								})));
							}
							catch (PSInvalidOperationException ex)
							{
								errorList.Add(new ParseError(scriptExtent, ex.ErrorRecord.FullyQualifiedErrorId, ex.Message));
							}
							catch (Exception ex2)
							{
								CommandProcessorBase.CheckForSevereException(ex2);
								errorList.Add(new ParseError(scriptExtent, "ExceptionParsingMOFFile", string.Format(CultureInfo.CurrentCulture, ParserStrings.ExceptionParsingMOFFile, new object[]
								{
									empty,
									ex2.Message
								})));
							}
							string empty2 = string.Empty;
							try
							{
								flag4 = DscClassCache.ImportScriptKeywordsFromModule(psmoduleInfo2, fileName, out empty2);
							}
							catch (FileNotFoundException)
							{
								errorList.Add(new ParseError(scriptExtent, "SchemaFileNotFound", string.Format(CultureInfo.CurrentCulture, ParserStrings.SchemaFileNotFound, new object[]
								{
									empty2
								})));
							}
							catch (Exception ex3)
							{
								CommandProcessorBase.CheckForSevereException(ex3);
								errorList.Add(new ParseError(scriptExtent, "UnexpectedParseError", string.Format(CultureInfo.CurrentCulture, ex3.ToString(), new object[0])));
							}
							if (flag3 || flag4)
							{
								flag2 = true;
							}
						}
						if (!flag2)
						{
							try
							{
								string text3;
								flag2 = DscClassCache.ImportCimKeywordsFromModule(psmoduleInfo2, text2, out text3);
							}
							catch (Exception)
							{
							}
						}
						if (!text2.Contains("*") && flag2)
						{
							list2.Add(text2);
						}
					}
				}
				foreach (string item in list2)
				{
					list.Remove(item);
				}
				if (list.Count == 0)
				{
					break;
				}
			}
			if (list.Count > 0)
			{
				foreach (string text4 in list)
				{
					if (!text4.Contains("*"))
					{
						errorList.Add(new ParseError(scriptExtent, "DscResourcesNotFoundDuringParsing", string.Format(CultureInfo.CurrentCulture, ParserStrings.DscResourcesNotFoundDuringParsing, new object[]
						{
							text4
						})));
					}
				}
			}
		}

		// Token: 0x06005DC9 RID: 24009 RVA: 0x002016C4 File Offset: 0x001FF8C4
		private static void LoadPowerShellClassResourcesFromModule(PSModuleInfo primaryModuleInfo, PSModuleInfo moduleInfo, ICollection<string> resourcesToImport, ICollection<string> resourcesFound, List<ParseError> errorList, Dictionary<string, ScriptBlock> functionsToDefine = null, bool recurse = true, IScriptExtent extent = null)
		{
			if (primaryModuleInfo._declaredDscResourceExports == null || primaryModuleInfo._declaredDscResourceExports.Count == 0)
			{
				return;
			}
			if (moduleInfo.ModuleType == ModuleType.Binary)
			{
				ResolveEventHandler value = (object sender, ResolveEventArgs args) => DscClassCache.CurrentDomain_ReflectionOnlyAssemblyResolve(sender, args, moduleInfo);
				try
				{
					AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += value;
					Assembly assembly = moduleInfo.ImplementingAssembly;
					if (assembly == null && moduleInfo.Path != null)
					{
						try
						{
							string assemblyFile = moduleInfo.Path;
							if (moduleInfo.RootModule != null && !Path.GetExtension(moduleInfo.Path).Equals(".dll", StringComparison.OrdinalIgnoreCase))
							{
								assemblyFile = moduleInfo.ModuleBase + "\\" + moduleInfo.RootModule;
							}
							assembly = Assembly.ReflectionOnlyLoadFrom(assemblyFile);
						}
						catch
						{
						}
					}
					if (assembly != null)
					{
						DscClassCache.ImportKeywordsFromAssembly(moduleInfo, resourcesToImport, resourcesFound, functionsToDefine, assembly);
					}
					goto IL_17D;
				}
				finally
				{
					AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= value;
				}
			}
			if (moduleInfo.RootModule != null)
			{
				string fileName = Path.Combine(moduleInfo.ModuleBase, moduleInfo.RootModule);
				DscClassCache.ImportKeywordsFromScriptFile(fileName, moduleInfo, resourcesToImport, resourcesFound, functionsToDefine, errorList, extent);
			}
			else if (moduleInfo.Path != null)
			{
				DscClassCache.ImportKeywordsFromScriptFile(moduleInfo.Path, primaryModuleInfo, resourcesToImport, resourcesFound, functionsToDefine, errorList, extent);
			}
			IL_17D:
			if (moduleInfo.NestedModules != null && recurse)
			{
				foreach (PSModuleInfo moduleInfo2 in moduleInfo.NestedModules)
				{
					DscClassCache.LoadPowerShellClassResourcesFromModule(primaryModuleInfo, moduleInfo2, resourcesToImport, resourcesFound, errorList, functionsToDefine, false, extent);
				}
			}
		}

		// Token: 0x06005DCA RID: 24010 RVA: 0x002018D0 File Offset: 0x001FFAD0
		private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args, PSModuleInfo moduleInfo)
		{
			AssemblyName assemblyName = new AssemblyName(args.Name);
			if (moduleInfo != null && !string.IsNullOrEmpty(moduleInfo.Path))
			{
				string text = Path.GetDirectoryName(moduleInfo.Path) + "\\" + assemblyName.Name + ".dll";
				if (File.Exists(text))
				{
					return Assembly.ReflectionOnlyLoadFrom(text);
				}
				text = Path.GetDirectoryName(moduleInfo.Path) + "\\" + assemblyName.Name + ".exe";
				if (File.Exists(text))
				{
					return Assembly.ReflectionOnlyLoadFrom(text);
				}
			}
			return Assembly.ReflectionOnlyLoad(args.Name);
		}

		// Token: 0x06005DCB RID: 24011 RVA: 0x00201964 File Offset: 0x001FFB64
		public static List<string> ImportClassResourcesFromModule(PSModuleInfo moduleInfo, ICollection<string> resourcesToImport, Dictionary<string, ScriptBlock> functionsToDefine)
		{
			List<string> list = new List<string>();
			DscClassCache.LoadPowerShellClassResourcesFromModule(moduleInfo, moduleInfo, resourcesToImport, list, null, functionsToDefine, true, null);
			return list;
		}

		// Token: 0x06005DCC RID: 24012 RVA: 0x00201988 File Offset: 0x001FFB88
		internal static string GenerateMofForAst(TypeDefinitionAst typeAst)
		{
			List<object> embeddedInstanceTypes = new List<object>();
			StringBuilder stringBuilder = new StringBuilder();
			DscClassCache.GenerateMofForAst(typeAst, stringBuilder, embeddedInstanceTypes);
			DscClassCache.ProcessEmbeddedInstanceTypes(embeddedInstanceTypes, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06005DCD RID: 24013 RVA: 0x002019C0 File Offset: 0x001FFBC0
		internal static string MapTypeNameToMofType(ITypeName typeName, string memberName, string className, out bool isArrayType, out string embeddedInstanceType, List<object> embeddedInstanceTypes, ref string[] enumNames)
		{
			ArrayTypeName arrayTypeName = typeName as ArrayTypeName;
			TypeName typeName2;
			if (arrayTypeName != null)
			{
				isArrayType = true;
				typeName2 = (arrayTypeName.ElementType as TypeName);
			}
			else
			{
				isArrayType = false;
				typeName2 = (typeName as TypeName);
			}
			if (typeName2 == null || typeName2._typeDefinitionAst == null)
			{
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, ParserStrings.UnsupportedPropertyTypeOfDSCResourceClass, new object[]
				{
					memberName,
					typeName.FullName,
					typeName
				}));
			}
			if (typeName2._typeDefinitionAst.IsEnum)
			{
				enumNames = (from m in typeName2._typeDefinitionAst.Members
				select m.Name).ToArray<string>();
				isArrayType = false;
				embeddedInstanceType = null;
				return "string";
			}
			if (!embeddedInstanceTypes.Contains(typeName2._typeDefinitionAst))
			{
				embeddedInstanceTypes.Add(typeName2._typeDefinitionAst);
			}
			embeddedInstanceType = typeName.FullName.Replace('.', '_');
			return "string";
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x00201ACC File Offset: 0x001FFCCC
		private static void GenerateMofForAst(TypeDefinitionAst typeAst, StringBuilder sb, List<object> embeddedInstanceTypes)
		{
			string name = typeAst.Name;
			sb.AppendFormat(CultureInfo.InvariantCulture, "[ClassVersion(\"1.0.0\")]\nclass {0}", new object[]
			{
				name
			});
			if (typeAst.Attributes.Any((AttributeAst a) => a.TypeName.GetReflectionAttributeType() == typeof(DscResourceAttribute)))
			{
				sb.Append(" : OMI_BaseResource");
			}
			sb.Append("\n{\n");
			DscClassCache.ProcessMembers(sb, embeddedInstanceTypes, typeAst, name);
			Queue<object> queue = new Queue<object>();
			using (IEnumerator<TypeConstraintAst> enumerator = typeAst.BaseTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TypeConstraintAst item = enumerator.Current;
					queue.Enqueue(item);
				}
				goto IL_17C;
			}
			IL_B0:
			object obj = queue.Dequeue();
			TypeConstraintAst typeConstraintAst = obj as TypeConstraintAst;
			if (typeConstraintAst != null)
			{
				obj = typeConstraintAst.TypeName.GetReflectionType();
				if (obj == null)
				{
					TypeName typeName = typeConstraintAst.TypeName as TypeName;
					if (typeName == null || typeName._typeDefinitionAst == null)
					{
						goto IL_17C;
					}
					DscClassCache.ProcessMembers(sb, embeddedInstanceTypes, typeName._typeDefinitionAst, name);
					using (IEnumerator<TypeConstraintAst> enumerator2 = typeName._typeDefinitionAst.BaseTypes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							TypeConstraintAst item2 = enumerator2.Current;
							queue.Enqueue(item2);
						}
						goto IL_17C;
					}
				}
			}
			Type type = obj as Type;
			if (type != null)
			{
				DscClassCache.ProcessMembers(type, sb, embeddedInstanceTypes, name);
				Type baseType = type.GetTypeInfo().BaseType;
				if (baseType != null)
				{
					queue.Enqueue(baseType);
				}
			}
			IL_17C:
			if (queue.Count <= 0)
			{
				sb.Append("};");
				return;
			}
			goto IL_B0;
		}

		// Token: 0x06005DCF RID: 24015 RVA: 0x00201CA8 File Offset: 0x001FFEA8
		private static void ProcessMembers(StringBuilder sb, List<object> embeddedInstanceTypes, TypeDefinitionAst typeDefinitionAst, string className)
		{
			foreach (MemberAst memberAst in typeDefinitionAst.Members)
			{
				PropertyMemberAst propertyMemberAst = memberAst as PropertyMemberAst;
				if (propertyMemberAst != null && !propertyMemberAst.IsStatic)
				{
					if (!propertyMemberAst.Attributes.All((AttributeAst a) => a.TypeName.GetReflectionAttributeType() != typeof(DscPropertyAttribute)))
					{
						Type type = (propertyMemberAst.PropertyType == null) ? typeof(object) : propertyMemberAst.PropertyType.TypeName.GetReflectionType();
						List<object> list = new List<object>();
						for (int i = 0; i < propertyMemberAst.Attributes.Count; i++)
						{
							list.Add(propertyMemberAst.Attributes[i].GetAttribute());
						}
						string[] enumNames = null;
						bool flag;
						string embeddedInstanceType;
						string text;
						if (type != null)
						{
							text = DscClassCache.MapTypeToMofType(type, memberAst.Name, className, out flag, out embeddedInstanceType, embeddedInstanceTypes);
							if (type.GetTypeInfo().IsEnum)
							{
								enumNames = Enum.GetNames(type);
							}
						}
						else
						{
							text = DscClassCache.MapTypeNameToMofType(propertyMemberAst.PropertyType.TypeName, memberAst.Name, className, out flag, out embeddedInstanceType, embeddedInstanceTypes, ref enumNames);
						}
						string text2 = flag ? "[]" : string.Empty;
						sb.AppendFormat(CultureInfo.InvariantCulture, "    {0}{1} {2}{3};\n", new object[]
						{
							DscClassCache.MapAttributesToMof(enumNames, list, embeddedInstanceType),
							text,
							memberAst.Name,
							text2
						});
					}
				}
			}
		}

		// Token: 0x06005DD0 RID: 24016 RVA: 0x00201EAC File Offset: 0x002000AC
		private static bool ImportKeywordsFromScriptFile(string fileName, PSModuleInfo module, ICollection<string> resourcesToImport, ICollection<string> resourcesFound, Dictionary<string, ScriptBlock> functionsToDefine, List<ParseError> errorList, IScriptExtent extent)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return false;
			}
			if (!".psm1".Equals(Path.GetExtension(fileName), StringComparison.OrdinalIgnoreCase) && !".ps1".Equals(Path.GetExtension(fileName), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (!File.Exists(fileName) || DscClassCache.ScriptKeywordFileCache.Contains(fileName))
			{
				return false;
			}
			Token[] array;
			ParseError[] array2;
			ScriptBlockAst scriptBlockAst = Parser.ParseFile(fileName, out array, out array2);
			if (array2 != null && array2.Length > 0)
			{
				if (errorList != null && extent != null)
				{
					List<string> list = new List<string>();
					foreach (ParseError parseError in array2)
					{
						list.Add(parseError.ToString());
					}
					errorList.Add(new ParseError(extent, "FailToParseModuleScriptFile", string.Format(CultureInfo.CurrentCulture, ParserStrings.FailToParseModuleScriptFile, new object[]
					{
						fileName,
						string.Join(Environment.NewLine, list)
					})));
				}
				return false;
			}
			IEnumerable<Ast> enumerable = scriptBlockAst.FindAll(delegate(Ast n)
			{
				TypeDefinitionAst typeDefinitionAst2 = n as TypeDefinitionAst;
				if (typeDefinitionAst2 != null)
				{
					for (int j = 0; j < typeDefinitionAst2.Attributes.Count; j++)
					{
						AttributeAst attributeAst2 = typeDefinitionAst2.Attributes[j];
						if (attributeAst2.TypeName.GetReflectionAttributeType() == typeof(DscResourceAttribute))
						{
							return true;
						}
					}
				}
				return false;
			}, false);
			bool result = false;
			CimDSCParser parser = new CimDSCParser(new CimMofDeserializer.OnClassNeeded(DscClassCache.MyClassCallback));
			IEnumerable<WildcardPattern> patterns = SessionStateUtilities.CreateWildcardsFromStrings(module._declaredDscResourceExports, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			foreach (Ast ast in enumerable)
			{
				result = true;
				TypeDefinitionAst typeDefinitionAst = (TypeDefinitionAst)ast;
				if (SessionStateUtilities.MatchesAnyWildcardPattern(typeDefinitionAst.Name, patterns, true))
				{
					bool flag = true;
					foreach (string pattern in resourcesToImport)
					{
						if (new WildcardPattern(pattern).IsMatch(typeDefinitionAst.Name))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						DSCResourceRunAsCredential runAsBehavior = DSCResourceRunAsCredential.Default;
						foreach (AttributeAst attributeAst in typeDefinitionAst.Attributes)
						{
							if (attributeAst.TypeName.GetReflectionAttributeType() == typeof(DscResourceAttribute))
							{
								foreach (NamedAttributeArgumentAst namedAttributeArgumentAst in attributeAst.NamedArguments)
								{
									if (namedAttributeArgumentAst.ArgumentName.Equals("RunAsCredential", StringComparison.OrdinalIgnoreCase))
									{
										DscResourceAttribute dscResourceAttribute = attributeAst.GetAttribute() as DscResourceAttribute;
										if (dscResourceAttribute != null)
										{
											runAsBehavior = dscResourceAttribute.RunAsCredential;
										}
									}
								}
							}
						}
						string mof = DscClassCache.GenerateMofForAst(typeDefinitionAst);
						DscClassCache.ProcessMofForDynamicKeywords(module, resourcesFound, functionsToDefine, parser, mof, runAsBehavior);
					}
				}
			}
			return result;
		}

		// Token: 0x06005DD1 RID: 24017 RVA: 0x002021B0 File Offset: 0x002003B0
		private static bool AreQualifiersSame(CimReadOnlyKeyedCollection<CimQualifier> oldQualifier, CimReadOnlyKeyedCollection<CimQualifier> newQualifiers)
		{
			if (oldQualifier.Count != newQualifiers.Count)
			{
				return false;
			}
			foreach (CimQualifier cimQualifier in oldQualifier)
			{
				CimQualifier cimQualifier2 = newQualifiers[cimQualifier.Name];
				if (cimQualifier2 == null)
				{
					return false;
				}
				if (cimQualifier.CimType != cimQualifier2.CimType || cimQualifier.Flags != cimQualifier2.Flags)
				{
					return false;
				}
				if ((cimQualifier.Value == null && cimQualifier2.Value != null) || (cimQualifier.Value != null && cimQualifier2.Value == null) || (cimQualifier.Value != null && cimQualifier2.Value != null && !string.Equals(cimQualifier.Value.ToString(), cimQualifier2.Value.ToString(), StringComparison.OrdinalIgnoreCase)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005DD2 RID: 24018 RVA: 0x00202294 File Offset: 0x00200494
		private static bool ArePropertiesSame(CimReadOnlyKeyedCollection<CimPropertyDeclaration> oldProperties, CimReadOnlyKeyedCollection<CimPropertyDeclaration> newProperties)
		{
			if (oldProperties.Count != newProperties.Count)
			{
				return false;
			}
			foreach (CimPropertyDeclaration cimPropertyDeclaration in oldProperties)
			{
				CimPropertyDeclaration cimPropertyDeclaration2 = newProperties[cimPropertyDeclaration.Name];
				if (cimPropertyDeclaration2 == null)
				{
					return false;
				}
				if (cimPropertyDeclaration.CimType != cimPropertyDeclaration2.CimType || cimPropertyDeclaration.Flags != cimPropertyDeclaration2.Flags)
				{
					return false;
				}
				if (!DscClassCache.AreQualifiersSame(cimPropertyDeclaration.Qualifiers, cimPropertyDeclaration2.Qualifiers))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005DD3 RID: 24019 RVA: 0x00202334 File Offset: 0x00200534
		private static bool IsSameNestedObject(CimClass oldClass, CimClass newClass)
		{
			return (oldClass.CimSuperClassName == null || !string.Equals("OMI_BaseResource", oldClass.CimSuperClassName, StringComparison.OrdinalIgnoreCase)) && (newClass.CimSuperClassName == null || !string.Equals("OMI_BaseResource", newClass.CimSuperClassName, StringComparison.OrdinalIgnoreCase)) && DscClassCache.AreQualifiersSame(oldClass.CimClassQualifiers, newClass.CimClassQualifiers) && DscClassCache.ArePropertiesSame(oldClass.CimClassProperties, newClass.CimClassProperties);
		}

		// Token: 0x06005DD4 RID: 24020 RVA: 0x002023A4 File Offset: 0x002005A4
		internal static string MapTypeToMofType(Type type, string memberName, string className, out bool isArrayType, out string embeddedInstanceType, List<object> embeddedInstanceTypes)
		{
			isArrayType = false;
			if (type.GetTypeInfo().IsValueType)
			{
				type = (Nullable.GetUnderlyingType(type) ?? type);
			}
			if (type.GetTypeInfo().IsEnum)
			{
				embeddedInstanceType = null;
				return "string";
			}
			if (type == typeof(Hashtable))
			{
				isArrayType = true;
				embeddedInstanceType = "MSFT_KeyValuePair";
				return "string";
			}
			if (type == typeof(PSCredential))
			{
				embeddedInstanceType = "MSFT_Credential";
				return "string";
			}
			string result;
			if (type.IsArray)
			{
				isArrayType = true;
				Type elementType = type.GetElementType();
				if (!elementType.IsArray)
				{
					bool flag;
					return DscClassCache.MapTypeToMofType(type.GetElementType(), memberName, className, out flag, out embeddedInstanceType, embeddedInstanceTypes);
				}
			}
			else if (DscClassCache.mapPrimitiveDotNetTypeToMof.TryGetValue(type, out result))
			{
				embeddedInstanceType = null;
				return result;
			}
			bool flag2 = false;
			if (type.GetTypeInfo().IsValueType)
			{
				flag2 = true;
			}
			else if (!type.GetTypeInfo().IsAbstract && type.GetConstructor(PSTypeExtensions.EmptyTypes) != null && type.GetTypeInfo().BaseType == typeof(object) && (type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length > 0 || type.GetFields(BindingFlags.Instance | BindingFlags.Public).Length > 0))
			{
				flag2 = true;
			}
			if (flag2)
			{
				if (!embeddedInstanceTypes.Contains(type))
				{
					embeddedInstanceTypes.Add(type);
				}
				embeddedInstanceType = type.FullName.Replace('.', '_');
				return "string";
			}
			throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, ParserStrings.UnsupportedPropertyTypeOfDSCResourceClass, new object[]
			{
				memberName,
				type.Name,
				className
			}));
		}

		// Token: 0x06005DD5 RID: 24021 RVA: 0x00202538 File Offset: 0x00200738
		private static string MapAttributesToMof(string[] enumNames, IEnumerable<object> customAttributes, string embeddedInstanceType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[");
			bool flag = false;
			foreach (object obj in customAttributes)
			{
				DscPropertyAttribute dscPropertyAttribute = obj as DscPropertyAttribute;
				if (dscPropertyAttribute != null)
				{
					if (dscPropertyAttribute.Key)
					{
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}key", new object[]
						{
							flag ? ", " : ""
						});
						flag = true;
					}
					if (dscPropertyAttribute.Mandatory)
					{
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}required", new object[]
						{
							flag ? ", " : ""
						});
						flag = true;
					}
					if (dscPropertyAttribute.NotConfigurable)
					{
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}read", new object[]
						{
							flag ? ", " : ""
						});
						flag = true;
					}
				}
				else
				{
					ValidateSetAttribute validateSetAttribute = obj as ValidateSetAttribute;
					if (validateSetAttribute != null)
					{
						bool flag2 = false;
						StringBuilder stringBuilder2 = new StringBuilder(", Values{");
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}ValueMap{{", new object[]
						{
							flag ? ", " : ""
						});
						flag = true;
						foreach (string text in validateSetAttribute.ValidValues)
						{
							stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}\"{1}\"", new object[]
							{
								flag2 ? ", " : "",
								text
							});
							stringBuilder2.AppendFormat(CultureInfo.InvariantCulture, "{0}\"{1}\"", new object[]
							{
								flag2 ? ", " : "",
								text
							});
							flag2 = true;
						}
						stringBuilder.Append("}");
						stringBuilder.Append(stringBuilder2);
						stringBuilder.Append("}");
					}
				}
			}
			if (stringBuilder.Length == 1)
			{
				stringBuilder.Append("write");
				flag = true;
			}
			if (enumNames != null)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}ValueMap{{", new object[]
				{
					flag ? ", " : ""
				});
				flag = false;
				foreach (string text2 in enumNames)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}\"{1}\"", new object[]
					{
						flag ? ", " : "",
						text2
					});
					flag = true;
				}
				stringBuilder.Append("}, Values{");
				flag = false;
				foreach (string text3 in enumNames)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}\"{1}\"", new object[]
					{
						flag ? ", " : "",
						text3
					});
					flag = true;
				}
				stringBuilder.Append("}");
			}
			else if (embeddedInstanceType != null)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}EmbeddedInstance(\"{1}\")", new object[]
				{
					flag ? ", " : "",
					embeddedInstanceType
				});
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x002028E0 File Offset: 0x00200AE0
		public static string GenerateMofForType(Type type)
		{
			List<object> embeddedInstanceTypes = new List<object>();
			StringBuilder stringBuilder = new StringBuilder();
			DscClassCache.GenerateMofForType(type, stringBuilder, embeddedInstanceTypes);
			DscClassCache.ProcessEmbeddedInstanceTypes(embeddedInstanceTypes, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06005DD7 RID: 24023 RVA: 0x00202910 File Offset: 0x00200B10
		private static void ProcessEmbeddedInstanceTypes(List<object> embeddedInstanceTypes, StringBuilder sb)
		{
			StringBuilder stringBuilder = null;
			while (embeddedInstanceTypes.Count > 0)
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder();
				}
				else
				{
					stringBuilder.Clear();
				}
				object[] array = embeddedInstanceTypes.ToArray();
				embeddedInstanceTypes.Clear();
				for (int i = array.Length - 1; i >= 0; i--)
				{
					Type type = array[i] as Type;
					if (type != null)
					{
						DscClassCache.GenerateMofForType(type, stringBuilder, embeddedInstanceTypes);
					}
					else
					{
						DscClassCache.GenerateMofForAst((TypeDefinitionAst)array[i], stringBuilder, embeddedInstanceTypes);
					}
					stringBuilder.Append('\n');
				}
				sb.Insert(0, stringBuilder.ToString());
			}
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x0020299C File Offset: 0x00200B9C
		private static void GenerateMofForType(Type type, StringBuilder sb, List<object> embeddedInstanceTypes)
		{
			string name = type.Name;
			sb.AppendFormat(CultureInfo.InvariantCulture, "[ClassVersion(\"1.0.0\")]\nclass {0}", new object[]
			{
				name
			});
			if (type.GetTypeInfo().GetCustomAttributes<DscResourceAttribute>().Any<DscResourceAttribute>())
			{
				sb.Append(" : OMI_BaseResource");
			}
			sb.Append("\n{\n");
			DscClassCache.ProcessMembers(type, sb, embeddedInstanceTypes, name);
			sb.Append("};");
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x00202A38 File Offset: 0x00200C38
		private static void ProcessMembers(Type type, StringBuilder sb, List<object> embeddedInstanceTypes, string className)
		{
			foreach (MemberInfo memberInfo in from m in type.GetMembers(BindingFlags.Instance | BindingFlags.Public)
			where m is PropertyInfo || m is FieldInfo
			select m)
			{
				if (!memberInfo.CustomAttributes.All((CustomAttributeData cad) => cad.AttributeType != typeof(DscPropertyAttribute)))
				{
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					Type type2;
					if (propertyInfo == null)
					{
						FieldInfo fieldInfo = (FieldInfo)memberInfo;
						type2 = fieldInfo.FieldType;
					}
					else
					{
						if (propertyInfo.GetSetMethod() == null)
						{
							continue;
						}
						type2 = propertyInfo.PropertyType;
					}
					bool flag;
					string embeddedInstanceType;
					string text = DscClassCache.MapTypeToMofType(type2, memberInfo.Name, className, out flag, out embeddedInstanceType, embeddedInstanceTypes);
					string text2 = flag ? "[]" : string.Empty;
					string[] enumNames = type2.GetTypeInfo().IsEnum ? Enum.GetNames(type2) : null;
					sb.AppendFormat(CultureInfo.InvariantCulture, "    {0}{1} {2}{3};\n", new object[]
					{
						DscClassCache.MapAttributesToMof(enumNames, memberInfo.GetCustomAttributes(true), embeddedInstanceType),
						text,
						memberInfo.Name,
						text2
					});
				}
			}
		}

		// Token: 0x06005DDA RID: 24026 RVA: 0x00202BB8 File Offset: 0x00200DB8
		private static bool ImportKeywordsFromAssembly(PSModuleInfo module, ICollection<string> resourcesToImport, ICollection<string> resourcesFound, Dictionary<string, ScriptBlock> functionsToDefine, Assembly assembly)
		{
			bool result = false;
			CimDSCParser parser = new CimDSCParser(new CimMofDeserializer.OnClassNeeded(DscClassCache.MyClassCallback));
			IEnumerable<Type> enumerable = from t in assembly.GetTypes()
			where t.GetTypeInfo().GetCustomAttributes<DscResourceAttribute>().Any<DscResourceAttribute>()
			select t;
			foreach (Type type in enumerable)
			{
				result = true;
				bool flag = true;
				foreach (string pattern in resourcesToImport)
				{
					if (new WildcardPattern(pattern).IsMatch(type.Name))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					string mof = DscClassCache.GenerateMofForType(type);
					DscClassCache.ProcessMofForDynamicKeywords(module, resourcesFound, functionsToDefine, parser, mof, DSCResourceRunAsCredential.Default);
				}
			}
			return result;
		}

		// Token: 0x06005DDB RID: 24027 RVA: 0x00202CAC File Offset: 0x00200EAC
		private static void ProcessMofForDynamicKeywords(PSModuleInfo module, ICollection<string> resourcesFound, Dictionary<string, ScriptBlock> functionsToDefine, CimDSCParser parser, string mof, DSCResourceRunAsCredential runAsBehavior)
		{
			foreach (CimClass cimClass in parser.ParseSchemaMofFileBuffer(mof))
			{
				string className = cimClass.CimSystemProperties.ClassName;
				if (!DscClassCache.CacheResourcesFromMultipleModuleVersions)
				{
					List<KeyValuePair<string, CimClass>> list = DscClassCache.FindResourceInCache(module.Name, className);
					if (list.Count > 0 && !string.IsNullOrEmpty(list[0].Key))
					{
						DscClassCache.ClassCache.Remove(list[0].Key);
					}
				}
				string moduleQualifiedResourceName = DscClassCache.GetModuleQualifiedResourceName(module.Name, module.Version.ToString(), className);
				DscClassCache.ClassCache[moduleQualifiedResourceName] = cimClass;
				DscClassCache.ByClassModuleCache[className] = new Tuple<string, Version>(module.Name, module.Version);
				resourcesFound.Add(className);
				DscClassCache.CreateAndRegisterKeywordFromCimClass(module.Name, module.Version, cimClass, functionsToDefine, runAsBehavior);
			}
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x00202DB8 File Offset: 0x00200FB8
		public static bool ImportCimKeywordsFromModule(PSModuleInfo module, string resourceName, out string schemaFilePath)
		{
			return DscClassCache.ImportCimKeywordsFromModule(module, resourceName, out schemaFilePath, null);
		}

		// Token: 0x06005DDD RID: 24029 RVA: 0x00202DC3 File Offset: 0x00200FC3
		public static bool ImportCimKeywordsFromModule(PSModuleInfo module, string resourceName, out string schemaFilePath, Dictionary<string, ScriptBlock> functionsToDefine)
		{
			return DscClassCache.ImportCimKeywordsFromModule(module, resourceName, out schemaFilePath, functionsToDefine, null);
		}

		// Token: 0x06005DDE RID: 24030 RVA: 0x00202DD0 File Offset: 0x00200FD0
		public static bool ImportCimKeywordsFromModule(PSModuleInfo module, string resourceName, out string schemaFilePath, Dictionary<string, ScriptBlock> functionsToDefine, Collection<Exception> errors)
		{
			if (module == null)
			{
				throw PSTraceSource.NewArgumentNullException("module");
			}
			if (resourceName == null)
			{
				throw PSTraceSource.NewArgumentNullException("resourceName");
			}
			string text = Path.Combine(module.ModuleBase, "DSCResources");
			schemaFilePath = Path.Combine(Path.Combine(text, resourceName), resourceName + ".Schema.mof");
			if (File.Exists(schemaFilePath))
			{
				List<CimClass> list = DscClassCache.GetCachedClassByFileName(schemaFilePath) ?? DscClassCache.ImportClasses(schemaFilePath, new Tuple<string, Version>(module.Name, module.Version), errors);
				if (list != null)
				{
					foreach (CimClass cimClass in list)
					{
						DscClassCache.CreateAndRegisterKeywordFromCimClass(module.Name, module.Version, cimClass, functionsToDefine, DSCResourceRunAsCredential.Default);
					}
				}
				return true;
			}
			if (Directory.Exists(text))
			{
				try
				{
					string[] directories = Directory.GetDirectories(text);
					foreach (string path in directories)
					{
						string[] files = Directory.GetFiles(path, "*.Schema.mof", SearchOption.TopDirectoryOnly);
						if (files.Length > 0)
						{
							string text2 = files[0];
							List<CimClass> list2 = DscClassCache.GetCachedClassByFileName(text2) ?? DscClassCache.ImportClasses(text2, new Tuple<string, Version>(module.Name, module.Version), errors);
							if (list2 != null)
							{
								foreach (CimClass cimClass2 in list2)
								{
									string friendlyName = DscClassCache.GetFriendlyName(cimClass2);
									if (string.Equals(friendlyName, resourceName, StringComparison.OrdinalIgnoreCase))
									{
										DscClassCache.CreateAndRegisterKeywordFromCimClass(module.Name, module.Version, cimClass2, functionsToDefine, DSCResourceRunAsCredential.Default);
										return true;
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
				return false;
			}
			return false;
		}

		// Token: 0x06005DDF RID: 24031 RVA: 0x00202FA4 File Offset: 0x002011A4
		public static bool ImportScriptKeywordsFromModule(PSModuleInfo module, string resourceName, out string schemaFilePath)
		{
			return DscClassCache.ImportScriptKeywordsFromModule(module, resourceName, out schemaFilePath, null);
		}

		// Token: 0x06005DE0 RID: 24032 RVA: 0x00202FB0 File Offset: 0x002011B0
		public static bool ImportScriptKeywordsFromModule(PSModuleInfo module, string resourceName, out string schemaFilePath, Dictionary<string, ScriptBlock> functionsToDefine)
		{
			if (module == null)
			{
				throw PSTraceSource.NewArgumentNullException("module");
			}
			if (resourceName == null)
			{
				throw PSTraceSource.NewArgumentNullException("resourceName");
			}
			schemaFilePath = Path.Combine(Path.Combine(Path.Combine(module.ModuleBase, "DSCResources"), resourceName), resourceName + ".Schema.psm1");
			if (File.Exists(schemaFilePath) && !DscClassCache.CurrentImportingScriptFiles.Contains(schemaFilePath))
			{
				if (!DscClassCache.ScriptKeywordFileCache.Contains(schemaFilePath))
				{
					DscClassCache.CurrentImportingScriptFiles.Add(schemaFilePath);
					Token[] array;
					ParseError[] array2;
					Parser.ParseFile(schemaFilePath, out array, out array2);
					DscClassCache.CurrentImportingScriptFiles.Remove(schemaFilePath);
					DscClassCache.ScriptKeywordFileCache.Add(schemaFilePath);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x0020305C File Offset: 0x0020125C
		public static ErrorRecord GetBadlyFormedRequiredResourceIdErrorRecord(string badDependsOnReference, string definingResource)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.GetBadlyFormedRequiredResourceId, new object[]
			{
				badDependsOnReference,
				definingResource
			});
			ex.SetErrorId("GetBadlyFormedRequiredResourceId");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x00203098 File Offset: 0x00201298
		public static ErrorRecord GetBadlyFormedExclusiveResourceIdErrorRecord(string badExclusiveResourcereference, string definingResource)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.GetBadlyFormedExclusiveResourceId, new object[]
			{
				badExclusiveResourcereference,
				definingResource
			});
			ex.SetErrorId("GetBadlyFormedExclusiveResourceId");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x002030D4 File Offset: 0x002012D4
		public static ErrorRecord GetPullModeNeedConfigurationSource(string resourceId)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.GetPullModeNeedConfigurationSource, new object[]
			{
				resourceId
			});
			ex.SetErrorId("GetPullModeNeedConfigurationSource");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x0020310C File Offset: 0x0020130C
		public static ErrorRecord DisabledRefreshModeNotValidForPartialConfig(string resourceId)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.DisabledRefreshModeNotValidForPartialConfig, new object[]
			{
				resourceId
			});
			ex.SetErrorId("DisabledRefreshModeNotValidForPartialConfig");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DE5 RID: 24037 RVA: 0x00203144 File Offset: 0x00201344
		public static ErrorRecord DuplicateResourceIdInNodeStatementErrorRecord(string duplicateResourceId, string nodeName)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.DuplicateResourceIdInNodeStatement, new object[]
			{
				duplicateResourceId,
				nodeName
			});
			ex.SetErrorId("DuplicateResourceIdInNodeStatement");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DE6 RID: 24038 RVA: 0x00203180 File Offset: 0x00201380
		public static ErrorRecord InvalidConfigurationNameErrorRecord(string configurationName)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.InvalidConfigurationName, new object[]
			{
				configurationName
			});
			ex.SetErrorId("InvalidConfigurationName");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DE7 RID: 24039 RVA: 0x002031B8 File Offset: 0x002013B8
		public static ErrorRecord InvalidValueForPropertyErrorRecord(string propertyName, string value, string keywordName, string validValues)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.InvalidValueForProperty, new object[]
			{
				value,
				propertyName,
				keywordName,
				validValues
			});
			ex.SetErrorId("InvalidValueForProperty");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x002031FC File Offset: 0x002013FC
		public static ErrorRecord InvalidLocalConfigurationManagerPropertyErrorRecord(string propertyName, string validProperties)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.InvalidLocalConfigurationManagerProperty, new object[]
			{
				propertyName,
				validProperties
			});
			ex.SetErrorId("InvalidLocalConfigurationManagerProperty");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DE9 RID: 24041 RVA: 0x00203238 File Offset: 0x00201438
		public static ErrorRecord UnsupportedValueForPropertyErrorRecord(string propertyName, string value, string keywordName, string validValues)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.UnsupportedValueForProperty, new object[]
			{
				value,
				propertyName,
				keywordName,
				validValues
			});
			ex.SetErrorId("UnsupportedValueForProperty");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DEA RID: 24042 RVA: 0x0020327C File Offset: 0x0020147C
		public static ErrorRecord MissingValueForMandatoryPropertyErrorRecord(string keywordName, string typeName, string propertyName)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.MissingValueForMandatoryProperty, new object[]
			{
				keywordName,
				typeName,
				propertyName
			});
			ex.SetErrorId("MissingValueForMandatoryProperty");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DEB RID: 24043 RVA: 0x002032BC File Offset: 0x002014BC
		public static ErrorRecord DebugModeShouldHaveOneValue()
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.DebugModeShouldHaveOneValue, new object[0]);
			ex.SetErrorId("DebugModeShouldHaveOneValue");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DEC RID: 24044 RVA: 0x002032EC File Offset: 0x002014EC
		public static ErrorRecord ValueNotInRangeErrorRecord(string property, string name, int providedValue, int lower, int upper)
		{
			PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.ValueNotInRange, new object[]
			{
				property,
				name,
				providedValue,
				lower,
				upper
			});
			ex.SetErrorId("ValueNotInRange");
			return ex.ErrorRecord;
		}

		// Token: 0x06005DED RID: 24045 RVA: 0x0020334C File Offset: 0x0020154C
		public static string GetDSCResourceUsageString(DynamicKeyword keyword)
		{
			StringBuilder stringBuilder;
			switch (keyword.NameMode)
			{
			case DynamicKeywordNameMode.SimpleNameRequired:
				stringBuilder = new StringBuilder(keyword.Keyword + " [string] # Resource Name");
				break;
			case DynamicKeywordNameMode.NameRequired:
				stringBuilder = new StringBuilder(keyword.Keyword + " [string[]] # Name List");
				break;
			case DynamicKeywordNameMode.SimpleOptionalName:
				stringBuilder = new StringBuilder(keyword.Keyword + " [ [string] ] # Optional Name");
				break;
			case DynamicKeywordNameMode.OptionalName:
				stringBuilder = new StringBuilder(keyword.Keyword + " [ [string[]] ] # Optional NameList");
				break;
			default:
				stringBuilder = new StringBuilder(keyword.Keyword);
				break;
			}
			stringBuilder.Append("\n{\n");
			bool flag = true;
			for (;;)
			{
				foreach (KeyValuePair<string, DynamicKeywordProperty> keyValuePair in from ob in keyword.Properties
				orderby ob.Key
				select ob)
				{
					if (!string.Equals(keyValuePair.Key, "ResourceId", StringComparison.OrdinalIgnoreCase))
					{
						DynamicKeywordProperty value = keyValuePair.Value;
						if ((flag && value.IsKey) || (!flag && !value.IsKey))
						{
							stringBuilder.Append(value.Mandatory ? "    " : "    [ ");
							stringBuilder.Append(keyValuePair.Key);
							stringBuilder.Append(" = ");
							stringBuilder.Append(DscClassCache.FormatCimPropertyType(value, !value.Mandatory));
						}
					}
				}
				if (!flag)
				{
					break;
				}
				flag = false;
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x002034FC File Offset: 0x002016FC
		private static StringBuilder FormatCimPropertyType(DynamicKeywordProperty prop, bool isOptionalProperty)
		{
			string typeConstraint = prop.TypeConstraint;
			StringBuilder stringBuilder = new StringBuilder();
			if (string.Equals(typeConstraint, "MSFT_Credential", StringComparison.OrdinalIgnoreCase))
			{
				stringBuilder.Append("[PSCredential]");
			}
			else if (string.Equals(typeConstraint, "MSFT_KeyValuePair", StringComparison.OrdinalIgnoreCase) || string.Equals(typeConstraint, "MSFT_KeyValuePair[]", StringComparison.OrdinalIgnoreCase))
			{
				stringBuilder.Append("[Hashtable]");
			}
			else
			{
				string text = LanguagePrimitives.ConvertTypeNameToPSTypeName(typeConstraint);
				if (!string.Equals(text, "[]", StringComparison.OrdinalIgnoreCase))
				{
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.Append("[" + typeConstraint + "]");
				}
			}
			if (prop.ValueMap != null && prop.ValueMap.Count > 0)
			{
				stringBuilder.Append(" { " + string.Join(" | ", from x in prop.ValueMap.Keys
				orderby x
				select x) + " }");
			}
			if (isOptionalProperty)
			{
				stringBuilder.Append("]");
			}
			stringBuilder.Append("\n");
			return stringBuilder;
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06005DEF RID: 24047 RVA: 0x00203611 File Offset: 0x00201811
		private static ScriptBlock CimKeywordImlementationFunction
		{
			get
			{
				if (DscClassCache._cimKeywordImlementationFunction == null)
				{
					DscClassCache._cimKeywordImlementationFunction = ScriptBlock.Create("\r\n    param (\r\n        [Parameter(Mandatory)]\r\n            $KeywordData,\r\n        [Parameter(Mandatory)]\r\n            $Name,\r\n        [Parameter(Mandatory)]\r\n        [Hashtable]\r\n            $Value,\r\n        [Parameter(Mandatory)]\r\n            $SourceMetadata\r\n    )\r\n\r\n# walk the call stack to get at all of the enclosing configuration resource IDs\r\n    $stackedConfigs = @(Get-PSCallStack |\r\n        where { ($_.InvocationInfo.MyCommand -ne $null) -and ($_.InvocationInfo.MyCommand.CommandType -eq 'Configuration') })\r\n# keep all but the top-most\r\n    $stackedConfigs = $stackedConfigs[0..(@($stackedConfigs).Length - 2)]\r\n# and build the complex resource ID suffix.\r\n    $complexResourceQualifier = ( $stackedConfigs | foreach { '[' + $_.Command + ']' + $_.InvocationInfo.BoundParameters['InstanceName'] } ) -join '::'\r\n\r\n#\r\n# Utility function used to validate that the DependsOn arguments are well-formed.\r\n# The function also adds them to the define nodes resource collection.\r\n# in the case of resources generated inside a script resource, this routine\r\n# will also fix up the DependsOn references to '[Type]Instance::[OuterType]::OuterInstance\r\n#\r\n    function Test-DependsOn\r\n    {\r\n\r\n# make sure the references are well-formed\r\n        $updatedDependsOn = foreach ($DependsOnVar in $value['DependsOn']) {\r\n            if ($DependsOnVar -notmatch '^\\[[a-z]\\w*\\][a-z][a-z_0-9\\p{Zs}\\.\\\\-]*$')\r\n            {\r\n                Update-ConfigurationErrorCount\r\n                Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::GetBadlyFormedRequiredResourceIdErrorRecord($DependsOnVar, $resourceId))\r\n            }\r\n\r\n# Fix up DependsOn for nested names\r\n            if ($MyTypeName -and $typeName -ne $MyTypeName -and $InstanceName)\r\n            {\r\n                \"$DependsOnVar::$complexResourceQualifier\"\r\n            }\r\n            else\r\n            {\r\n                $DependsOnVar\r\n            }\r\n        }\r\n        $value['DependsOn']= $updatedDependsOn\r\n\r\n        if($DependsOn -ne $null)\r\n        {\r\n            #\r\n            # Combine DependsOn with dependson from outer composite resource\r\n            # which is set as local variable $DependsOn at the composite resource context\r\n            #\r\n            $value['DependsOn']= @($value['DependsOn']) + $DependsOn\r\n        }\r\n\r\n# Save the resource id in a per-node dictionary to do cross validation at the end\r\n        Set-NodeResources $resourceId @( $value['DependsOn'])\r\n\r\n# Remove depends on because it need to be fixed up for composite resources\r\n# We do it in ValidateNodeResource and Update-Depends on in configuration/Node function\r\n        $value.Remove('DependsOn')\r\n    }\r\n\r\n# A copy of the value object with correctly-cased property names\r\n    $canonicalizedValue = @{}\r\n\r\n    $typeName = $keywordData.ResourceName # CIM type\r\n    $keywordName = $keywordData.Keyword   # user-friendly alias that is used in scripts\r\n    $keyValues = ''\r\n    $debugPrefix = \"   ${TypeName}:\" # set up a debug prefix string that makes it easier to track what's happening.\r\n\r\n    Write-Debug \"${debugPrefix} RESOURCE PROCESSING STARTED [KeywordName='$keywordName'] Function='$($myinvocation.Invocationname)']\"\r\n\r\n# Check whether it's an old style metaconfig\r\n    $OldMetaConfig = $false\r\n    if ((-not $IsMetaConfig) -and ($keywordName -ieq 'LocalConfigurationManager')) {\r\n        $OldMetaConfig = $true\r\n    }\r\n\r\n# Check to see if it's a resource keyword. If so add the meta-properties to the canonical property collection.\r\n    $resourceId = $null\r\n# todo: need to include configuration managers and partial configuration\r\n    if (($keywordData.Properties.Keys -contains 'DependsOn') -or (($KeywordData.ImplementingModule -ieq 'PSDesiredStateConfigurationEngine') -and ($KeywordData.NameMode -eq [System.Management.Automation.Language.DynamicKeywordNameMode]::NameRequired)))\r\n    {\r\n\r\n        $resourceId = \"[$keywordName]$name\"\r\n        if ($MyTypeName -and $keywordName -ne $MyTypeName -and $InstanceName)\r\n        {\r\n            $resourceId += \"::$complexResourceQualifier\"\r\n        }\r\n\r\n        Write-Debug \"${debugPrefix} ResourceID = $resourceId\"\r\n\r\n# copy the meta-properties\r\n        $canonicalizedValue['ResourceID'] = $resourceId\r\n        $canonicalizedValue['SourceInfo'] = $SourceMetadata\r\n        if(-not $IsMetaConfig)\r\n        {\r\n            $canonicalizedValue['ModuleName'] = $keywordData.ImplementingModule\r\n            $canonicalizedValue['ModuleVersion'] = $keywordData.ImplementingModuleVersion -as [string]\r\n        }\r\n\r\n# see if there is already a resource with this ID.\r\n        if (Test-NodeResources $resourceId)\r\n        {\r\n            Update-ConfigurationErrorCount\r\n            Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::DuplicateResourceIdInNodeStatementErrorRecord($resourceId, (Get-PSCurrentConfigurationNode)))\r\n        }\r\n        else\r\n        {\r\n# If there are prerequsite resources, validate that the references are well-formed strings\r\n# This routine also adds the resource to the global node resources table.\r\n            Test-DependsOn\r\n\r\n            # Save the resource id in a per-node dictionary to do cross validation at the end\r\n            if($keywordData.ImplementingModule -ieq \"PSDesiredStateConfigurationEngine\")\r\n            {\r\n                #$keywordName is PartialConfiguration \r\n                if($keywordName -eq 'PartialConfiguration')\r\n                {\r\n                    # RefreshMode is 'Pull' and .ConfigurationSource is empty\r\n                    if($value['RefreshMode'] -eq 'Pull' -and -not $value['ConfigurationSource'])\r\n                    {\r\n                        Update-ConfigurationErrorCount\r\n                        Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::GetPullModeNeedConfigurationSource($resourceId))\r\n                    }\r\n                \r\n                    # Verify that RefreshMode is not Disabled for Partial configuration\r\n                    if($value['RefreshMode'] -eq 'Disabled')\r\n                    {\r\n                        Update-ConfigurationErrorCount\r\n                        Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::DisabledRefreshModeNotValidForPartialConfig($resourceId))\r\n                    }\r\n\r\n                    if($value['ConfigurationSource'] -ne $null)\r\n                    {\r\n                        Set-NodeManager $resourceId $value['ConfigurationSource']\r\n                    }\r\n\r\n                    if($value['ResourceModuleSource'] -ne $null)\r\n                    {\r\n                        Set-NodeResourceSource $resourceId $value['ResourceModuleSource']\r\n                    }\r\n                }\r\n\r\n\r\n                if($value['ExclusiveResources'] -ne $null)\r\n                {\r\n# make sure the references are well-formed\r\n                    foreach ($ExclusiveResource in $value['ExclusiveResources']) {\r\n                        if (($ExclusiveResource -notmatch '^[a-z][a-z_0-9]*\\\\[a-z][a-z_0-9]*$') -and ($ExclusiveResource -notmatch '^[a-z][a-z_0-9]*$') -and ($ExclusiveResource -notmatch '^[a-z][a-z_0-9]*\\\\\\*$'))\r\n                        {\r\n                            Update-ConfigurationErrorCount\r\n                            Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::GetBadlyFormedExclusiveResourceIdErrorRecord($ExclusiveResource, $resourceId))\r\n                        }\r\n                    }\r\n\r\n                    # Save the resource id in a per-node dictionary to do cross validation at the end\r\n                    # Validate resource exist\r\n                    # Also update the resource reference from module\\friendlyname to module\\name\r\n                    $value['ExclusiveResources'] = @(Set-NodeExclusiveResources $resourceId @( $value['ExclusiveResources'] ))\r\n                }\r\n            }\r\n        }\r\n    }\r\n    else\r\n    {\r\n        Write-Debug \"${debugPrefix} TYPE IS NOT AS DSC RESOURCE\"\r\n    }\r\n\r\n#\r\n# Copy the user-supplied values into a new collection with canonicalized property names\r\n#\r\n    foreach ($key in $keywordData.Properties.Keys)\r\n    {\r\n        Write-Debug \"${debugPrefix} Processing property '$key' [\"\r\n\r\n        if ($value.Contains($key))\r\n        {\r\n            if ($OldMetaConfig -and (-not ($V1MetaConfigPropertyList -contains $key)))\r\n            {\r\n                Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::InvalidLocalConfigurationManagerPropertyErrorRecord($key, ($V1MetaConfigPropertyList -join ', ')))\r\n                Update-ConfigurationErrorCount\r\n            }\r\n# see if there is a list of allowed values for this property (similar to an enum)\r\n            $allowedValues = $keywordData.Properties[$key].Values\r\n# If there is and user-provided value is not in that list, write an error.\r\n            if ($allowedValues)\r\n            {\r\n                if(($value[$key] -eq $null) -and ($allowedValues -notcontains $value[$key]))\r\n                {\r\n                    Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::InvalidValueForPropertyErrorRecord($key, \"$($value[$key])\", $keywordData.Keyword, ($allowedValues -join ', ')))\r\n                    Update-ConfigurationErrorCount\r\n                }\r\n                else\r\n                {\r\n                    $notAllowedValue=$null\r\n                    foreach($v in $value[$key])\r\n                    {\r\n                        if($allowedValues -notcontains $v)\r\n                        {\r\n                            $notAllowedValue +=$v.ToString() + ', '\r\n                        }\r\n                    }\r\n\r\n                    if($notAllowedValue)\r\n                    {\r\n                        $notAllowedValue = $notAllowedValue.Substring(0, $notAllowedValue.Length -2)\r\n                        Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::UnsupportedValueForPropertyErrorRecord($key, $notAllowedValue, $keywordData.Keyword, ($allowedValues -join ', ')))\r\n                        Update-ConfigurationErrorCount\r\n                    }\r\n                }\r\n            }\r\n\r\n# see if a value range is defined for this property\r\n            $allowedRange = $keywordData.Properties[$key].Range\r\n            if($allowedRange)\r\n            {\r\n                $castedValue = $value[$key] -as [int]\r\n                if((($castedValue -is [int]) -and (($castedValue -lt  $keywordData.Properties[$key].Range.Item1) -or ($castedValue -gt $keywordData.Properties[$key].Range.Item2))) -or ($castedValue -eq $null))\r\n                {\r\n                    Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::ValueNotInRangeErrorRecord($key, $keywordName, $value[$key],  $keywordData.Properties[$key].Range.Item1,  $keywordData.Properties[$key].Range.Item2))\r\n                    Update-ConfigurationErrorCount\r\n                }\r\n            }\r\n\r\n            Write-Debug \"${debugPrefix}        Canonicalized property '$key' = '$($value[$key])'\"\r\n\r\n            if ($keywordData.Properties[$key].IsKey)\r\n            {\r\n                if($value[$key] -eq $null)\r\n                {\r\n                    $keyValues += \"::__NULL__\"\r\n                }\r\n                else\r\n                {\r\n                    $keyValues += \"::\" + $value[$key]\r\n                }\r\n            }\r\n\r\n            # see if ValueMap is also defined for this property (actual values)\r\n            $allowedValueMap = $keywordData.Properties[$key].ValueMap\r\n#if it is and the ValueMap contains the user-provided value as a key, use the actual value\r\n            if ($allowedValueMap -and $allowedValueMap.ContainsKey($value[$key]))\r\n            {\r\n                $canonicalizedValue[$key] = $allowedValueMap[$value[$key]]\r\n            }\r\n            else\r\n            {\r\n                $canonicalizedValue[$key] = $value[$key]\r\n            }\r\n        }\r\n        elseif ($keywordData.Properties[$key].Mandatory)\r\n        {\r\n# If the property was mandatory but the user didn't provide a value, write and error.\r\n            Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::MissingValueForMandatoryPropertyErrorRecord($keywordData.Keyword, $keywordData.Properties[$key].TypeConstraint, $Key))\r\n            Update-ConfigurationErrorCount\r\n        }\r\n\r\n        Write-Debug \"${debugPrefix}    Processing completed '$key' ]\"\r\n    }\r\n\r\n    if($keyValues)\r\n    {\r\n        $keyValues = $keyValues.Substring(2) # Remove the leading '::'\r\n        Add-NodeKeys $keyValues $keywordName\r\n        Test-ConflictingResources $keywordName $canonicalizedValue $keywordData\r\n    }\r\n\r\n# update OMI_ConfigurationDocument\r\n    if($IsMetaConfig)\r\n    {\r\n        if($keywordData.ResourceName -eq 'OMI_ConfigurationDocument')\r\n        {\r\n            if($(Get-PSMetaConfigurationProcessed))\r\n            {\r\n                $PSMetaConfigDocumentInstVersionInfo = Get-PSMetaConfigDocumentInstVersionInfo\r\n                $canonicalizedValue['MinimumCompatibleVersion']=$PSMetaConfigDocumentInstVersionInfo['MinimumCompatibleVersion']\r\n            }\r\n            else\r\n            {\r\n                Set-PSMetaConfigDocInsProcessedBeforeMeta\r\n                $canonicalizedValue['MinimumCompatibleVersion']='1.0.0'\r\n            }\r\n        }\r\n\r\n        if(($keywordData.ResourceName -eq 'MSFT_WebDownloadManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_FileDownloadManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_WebResourceManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_FileResourceManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_WebReportManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_PartialConfiguration'))\r\n        {\r\n            Set-PSMetaConfigVersionInfoV2\r\n        }\r\n    }\r\n    elseif($keywordData.ResourceName -eq 'OMI_ConfigurationDocument')\r\n    {\r\n        $canonicalizedValue['MinimumCompatibleVersion']='1.0.0'\r\n        $canonicalizedValue['CompatibleVersionAdditionalProperties']=@('Omi_BaseResource:ConfigurationName')\r\n    }\r\n\r\n    if(($keywordData.ResourceName -eq 'MSFT_DSCMetaConfiguration') -or ($keywordData.ResourceName -eq 'MSFT_DSCMetaConfigurationV2'))\r\n    {\r\n        if($canonicalizedValue['DebugMode'] -and @($canonicalizedValue['DebugMode']).Length -gt 1)\r\n        {\r\n            # we only allow one value for debug mode now.\r\n            Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::DebugModeShouldHaveOneValue())\r\n            Update-ConfigurationErrorCount\r\n        }\r\n    }\r\n        \r\n\r\n    # Generate the MOF text for this resource instance.\r\n    # when generate mof text for OMI_ConfigurationDocument we handle below two cases:\r\n    # 1. we will add versioning related property based on meta configuration instance already process\r\n    # 2. we update the existing OMI_ConfigurationDocument instance if it already exists when process meta configuration instance\r\n    $aliasId = ConvertTo-MOFInstance $keywordName $canonicalizedValue\r\n\r\n# If a OMI_ConfigurationDocument is executed outside of a node statement, it becomes the default\r\n# for all nodes that don't have an explicit OMI_ConfigurationDocument declaration\r\n    if ($keywordData.ResourceName -eq 'OMI_ConfigurationDocument' -and -not (Get-PSCurrentConfigurationNode))\r\n    {\r\n        $data = Get-MoFInstanceText $aliasId\r\n        Write-Debug \"${debugPrefix} DEFINING DEFAULT CONFIGURATION DOCUMENT: $data\"\r\n        Set-PSDefaultConfigurationDocument $data\r\n    }\r\n\r\n    Write-Debug \"${debugPrefix} MOF alias for this resource is '$aliasId'\"\r\n\r\n# always return the aliasId so the generated file will be well-formed if not valid\r\n    $aliasId\r\n\r\n    Write-Debug \"${debugPrefix} RESOURCE PROCESSING COMPLETED. TOTAL ERROR COUNT: $(Get-ConfigurationErrorCount)\"\r\n\r\n    ");
				}
				return DscClassCache._cimKeywordImlementationFunction;
			}
		}

		// Token: 0x040031CF RID: 12751
		private const string InboxDscResourceModulePath = "WindowsPowershell\\v1.0\\Modules\\PsDesiredStateConfiguration";

		// Token: 0x040031D0 RID: 12752
		private const string reservedDynamicKeywords = "^(Synchronization|Certificate|IIS|SQL)$";

		// Token: 0x040031D1 RID: 12753
		private const string reservedProperties = "^(Require|Trigger|Notify|Before|After|Subscribe)$";

		// Token: 0x040031D2 RID: 12754
		private const int IndexModuleName = 0;

		// Token: 0x040031D3 RID: 12755
		private const int IndexModuleVersion = 1;

		// Token: 0x040031D4 RID: 12756
		private const int IndexClassName = 2;

		// Token: 0x040031D5 RID: 12757
		private const string CimKeywordImlementationFunctionText = "\r\n    param (\r\n        [Parameter(Mandatory)]\r\n            $KeywordData,\r\n        [Parameter(Mandatory)]\r\n            $Name,\r\n        [Parameter(Mandatory)]\r\n        [Hashtable]\r\n            $Value,\r\n        [Parameter(Mandatory)]\r\n            $SourceMetadata\r\n    )\r\n\r\n# walk the call stack to get at all of the enclosing configuration resource IDs\r\n    $stackedConfigs = @(Get-PSCallStack |\r\n        where { ($_.InvocationInfo.MyCommand -ne $null) -and ($_.InvocationInfo.MyCommand.CommandType -eq 'Configuration') })\r\n# keep all but the top-most\r\n    $stackedConfigs = $stackedConfigs[0..(@($stackedConfigs).Length - 2)]\r\n# and build the complex resource ID suffix.\r\n    $complexResourceQualifier = ( $stackedConfigs | foreach { '[' + $_.Command + ']' + $_.InvocationInfo.BoundParameters['InstanceName'] } ) -join '::'\r\n\r\n#\r\n# Utility function used to validate that the DependsOn arguments are well-formed.\r\n# The function also adds them to the define nodes resource collection.\r\n# in the case of resources generated inside a script resource, this routine\r\n# will also fix up the DependsOn references to '[Type]Instance::[OuterType]::OuterInstance\r\n#\r\n    function Test-DependsOn\r\n    {\r\n\r\n# make sure the references are well-formed\r\n        $updatedDependsOn = foreach ($DependsOnVar in $value['DependsOn']) {\r\n            if ($DependsOnVar -notmatch '^\\[[a-z]\\w*\\][a-z][a-z_0-9\\p{Zs}\\.\\\\-]*$')\r\n            {\r\n                Update-ConfigurationErrorCount\r\n                Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::GetBadlyFormedRequiredResourceIdErrorRecord($DependsOnVar, $resourceId))\r\n            }\r\n\r\n# Fix up DependsOn for nested names\r\n            if ($MyTypeName -and $typeName -ne $MyTypeName -and $InstanceName)\r\n            {\r\n                \"$DependsOnVar::$complexResourceQualifier\"\r\n            }\r\n            else\r\n            {\r\n                $DependsOnVar\r\n            }\r\n        }\r\n        $value['DependsOn']= $updatedDependsOn\r\n\r\n        if($DependsOn -ne $null)\r\n        {\r\n            #\r\n            # Combine DependsOn with dependson from outer composite resource\r\n            # which is set as local variable $DependsOn at the composite resource context\r\n            #\r\n            $value['DependsOn']= @($value['DependsOn']) + $DependsOn\r\n        }\r\n\r\n# Save the resource id in a per-node dictionary to do cross validation at the end\r\n        Set-NodeResources $resourceId @( $value['DependsOn'])\r\n\r\n# Remove depends on because it need to be fixed up for composite resources\r\n# We do it in ValidateNodeResource and Update-Depends on in configuration/Node function\r\n        $value.Remove('DependsOn')\r\n    }\r\n\r\n# A copy of the value object with correctly-cased property names\r\n    $canonicalizedValue = @{}\r\n\r\n    $typeName = $keywordData.ResourceName # CIM type\r\n    $keywordName = $keywordData.Keyword   # user-friendly alias that is used in scripts\r\n    $keyValues = ''\r\n    $debugPrefix = \"   ${TypeName}:\" # set up a debug prefix string that makes it easier to track what's happening.\r\n\r\n    Write-Debug \"${debugPrefix} RESOURCE PROCESSING STARTED [KeywordName='$keywordName'] Function='$($myinvocation.Invocationname)']\"\r\n\r\n# Check whether it's an old style metaconfig\r\n    $OldMetaConfig = $false\r\n    if ((-not $IsMetaConfig) -and ($keywordName -ieq 'LocalConfigurationManager')) {\r\n        $OldMetaConfig = $true\r\n    }\r\n\r\n# Check to see if it's a resource keyword. If so add the meta-properties to the canonical property collection.\r\n    $resourceId = $null\r\n# todo: need to include configuration managers and partial configuration\r\n    if (($keywordData.Properties.Keys -contains 'DependsOn') -or (($KeywordData.ImplementingModule -ieq 'PSDesiredStateConfigurationEngine') -and ($KeywordData.NameMode -eq [System.Management.Automation.Language.DynamicKeywordNameMode]::NameRequired)))\r\n    {\r\n\r\n        $resourceId = \"[$keywordName]$name\"\r\n        if ($MyTypeName -and $keywordName -ne $MyTypeName -and $InstanceName)\r\n        {\r\n            $resourceId += \"::$complexResourceQualifier\"\r\n        }\r\n\r\n        Write-Debug \"${debugPrefix} ResourceID = $resourceId\"\r\n\r\n# copy the meta-properties\r\n        $canonicalizedValue['ResourceID'] = $resourceId\r\n        $canonicalizedValue['SourceInfo'] = $SourceMetadata\r\n        if(-not $IsMetaConfig)\r\n        {\r\n            $canonicalizedValue['ModuleName'] = $keywordData.ImplementingModule\r\n            $canonicalizedValue['ModuleVersion'] = $keywordData.ImplementingModuleVersion -as [string]\r\n        }\r\n\r\n# see if there is already a resource with this ID.\r\n        if (Test-NodeResources $resourceId)\r\n        {\r\n            Update-ConfigurationErrorCount\r\n            Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::DuplicateResourceIdInNodeStatementErrorRecord($resourceId, (Get-PSCurrentConfigurationNode)))\r\n        }\r\n        else\r\n        {\r\n# If there are prerequsite resources, validate that the references are well-formed strings\r\n# This routine also adds the resource to the global node resources table.\r\n            Test-DependsOn\r\n\r\n            # Save the resource id in a per-node dictionary to do cross validation at the end\r\n            if($keywordData.ImplementingModule -ieq \"PSDesiredStateConfigurationEngine\")\r\n            {\r\n                #$keywordName is PartialConfiguration \r\n                if($keywordName -eq 'PartialConfiguration')\r\n                {\r\n                    # RefreshMode is 'Pull' and .ConfigurationSource is empty\r\n                    if($value['RefreshMode'] -eq 'Pull' -and -not $value['ConfigurationSource'])\r\n                    {\r\n                        Update-ConfigurationErrorCount\r\n                        Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::GetPullModeNeedConfigurationSource($resourceId))\r\n                    }\r\n                \r\n                    # Verify that RefreshMode is not Disabled for Partial configuration\r\n                    if($value['RefreshMode'] -eq 'Disabled')\r\n                    {\r\n                        Update-ConfigurationErrorCount\r\n                        Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::DisabledRefreshModeNotValidForPartialConfig($resourceId))\r\n                    }\r\n\r\n                    if($value['ConfigurationSource'] -ne $null)\r\n                    {\r\n                        Set-NodeManager $resourceId $value['ConfigurationSource']\r\n                    }\r\n\r\n                    if($value['ResourceModuleSource'] -ne $null)\r\n                    {\r\n                        Set-NodeResourceSource $resourceId $value['ResourceModuleSource']\r\n                    }\r\n                }\r\n\r\n\r\n                if($value['ExclusiveResources'] -ne $null)\r\n                {\r\n# make sure the references are well-formed\r\n                    foreach ($ExclusiveResource in $value['ExclusiveResources']) {\r\n                        if (($ExclusiveResource -notmatch '^[a-z][a-z_0-9]*\\\\[a-z][a-z_0-9]*$') -and ($ExclusiveResource -notmatch '^[a-z][a-z_0-9]*$') -and ($ExclusiveResource -notmatch '^[a-z][a-z_0-9]*\\\\\\*$'))\r\n                        {\r\n                            Update-ConfigurationErrorCount\r\n                            Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::GetBadlyFormedExclusiveResourceIdErrorRecord($ExclusiveResource, $resourceId))\r\n                        }\r\n                    }\r\n\r\n                    # Save the resource id in a per-node dictionary to do cross validation at the end\r\n                    # Validate resource exist\r\n                    # Also update the resource reference from module\\friendlyname to module\\name\r\n                    $value['ExclusiveResources'] = @(Set-NodeExclusiveResources $resourceId @( $value['ExclusiveResources'] ))\r\n                }\r\n            }\r\n        }\r\n    }\r\n    else\r\n    {\r\n        Write-Debug \"${debugPrefix} TYPE IS NOT AS DSC RESOURCE\"\r\n    }\r\n\r\n#\r\n# Copy the user-supplied values into a new collection with canonicalized property names\r\n#\r\n    foreach ($key in $keywordData.Properties.Keys)\r\n    {\r\n        Write-Debug \"${debugPrefix} Processing property '$key' [\"\r\n\r\n        if ($value.Contains($key))\r\n        {\r\n            if ($OldMetaConfig -and (-not ($V1MetaConfigPropertyList -contains $key)))\r\n            {\r\n                Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::InvalidLocalConfigurationManagerPropertyErrorRecord($key, ($V1MetaConfigPropertyList -join ', ')))\r\n                Update-ConfigurationErrorCount\r\n            }\r\n# see if there is a list of allowed values for this property (similar to an enum)\r\n            $allowedValues = $keywordData.Properties[$key].Values\r\n# If there is and user-provided value is not in that list, write an error.\r\n            if ($allowedValues)\r\n            {\r\n                if(($value[$key] -eq $null) -and ($allowedValues -notcontains $value[$key]))\r\n                {\r\n                    Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::InvalidValueForPropertyErrorRecord($key, \"$($value[$key])\", $keywordData.Keyword, ($allowedValues -join ', ')))\r\n                    Update-ConfigurationErrorCount\r\n                }\r\n                else\r\n                {\r\n                    $notAllowedValue=$null\r\n                    foreach($v in $value[$key])\r\n                    {\r\n                        if($allowedValues -notcontains $v)\r\n                        {\r\n                            $notAllowedValue +=$v.ToString() + ', '\r\n                        }\r\n                    }\r\n\r\n                    if($notAllowedValue)\r\n                    {\r\n                        $notAllowedValue = $notAllowedValue.Substring(0, $notAllowedValue.Length -2)\r\n                        Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::UnsupportedValueForPropertyErrorRecord($key, $notAllowedValue, $keywordData.Keyword, ($allowedValues -join ', ')))\r\n                        Update-ConfigurationErrorCount\r\n                    }\r\n                }\r\n            }\r\n\r\n# see if a value range is defined for this property\r\n            $allowedRange = $keywordData.Properties[$key].Range\r\n            if($allowedRange)\r\n            {\r\n                $castedValue = $value[$key] -as [int]\r\n                if((($castedValue -is [int]) -and (($castedValue -lt  $keywordData.Properties[$key].Range.Item1) -or ($castedValue -gt $keywordData.Properties[$key].Range.Item2))) -or ($castedValue -eq $null))\r\n                {\r\n                    Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::ValueNotInRangeErrorRecord($key, $keywordName, $value[$key],  $keywordData.Properties[$key].Range.Item1,  $keywordData.Properties[$key].Range.Item2))\r\n                    Update-ConfigurationErrorCount\r\n                }\r\n            }\r\n\r\n            Write-Debug \"${debugPrefix}        Canonicalized property '$key' = '$($value[$key])'\"\r\n\r\n            if ($keywordData.Properties[$key].IsKey)\r\n            {\r\n                if($value[$key] -eq $null)\r\n                {\r\n                    $keyValues += \"::__NULL__\"\r\n                }\r\n                else\r\n                {\r\n                    $keyValues += \"::\" + $value[$key]\r\n                }\r\n            }\r\n\r\n            # see if ValueMap is also defined for this property (actual values)\r\n            $allowedValueMap = $keywordData.Properties[$key].ValueMap\r\n#if it is and the ValueMap contains the user-provided value as a key, use the actual value\r\n            if ($allowedValueMap -and $allowedValueMap.ContainsKey($value[$key]))\r\n            {\r\n                $canonicalizedValue[$key] = $allowedValueMap[$value[$key]]\r\n            }\r\n            else\r\n            {\r\n                $canonicalizedValue[$key] = $value[$key]\r\n            }\r\n        }\r\n        elseif ($keywordData.Properties[$key].Mandatory)\r\n        {\r\n# If the property was mandatory but the user didn't provide a value, write and error.\r\n            Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::MissingValueForMandatoryPropertyErrorRecord($keywordData.Keyword, $keywordData.Properties[$key].TypeConstraint, $Key))\r\n            Update-ConfigurationErrorCount\r\n        }\r\n\r\n        Write-Debug \"${debugPrefix}    Processing completed '$key' ]\"\r\n    }\r\n\r\n    if($keyValues)\r\n    {\r\n        $keyValues = $keyValues.Substring(2) # Remove the leading '::'\r\n        Add-NodeKeys $keyValues $keywordName\r\n        Test-ConflictingResources $keywordName $canonicalizedValue $keywordData\r\n    }\r\n\r\n# update OMI_ConfigurationDocument\r\n    if($IsMetaConfig)\r\n    {\r\n        if($keywordData.ResourceName -eq 'OMI_ConfigurationDocument')\r\n        {\r\n            if($(Get-PSMetaConfigurationProcessed))\r\n            {\r\n                $PSMetaConfigDocumentInstVersionInfo = Get-PSMetaConfigDocumentInstVersionInfo\r\n                $canonicalizedValue['MinimumCompatibleVersion']=$PSMetaConfigDocumentInstVersionInfo['MinimumCompatibleVersion']\r\n            }\r\n            else\r\n            {\r\n                Set-PSMetaConfigDocInsProcessedBeforeMeta\r\n                $canonicalizedValue['MinimumCompatibleVersion']='1.0.0'\r\n            }\r\n        }\r\n\r\n        if(($keywordData.ResourceName -eq 'MSFT_WebDownloadManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_FileDownloadManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_WebResourceManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_FileResourceManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_WebReportManager') `\r\n            -or ($keywordData.ResourceName -eq 'MSFT_PartialConfiguration'))\r\n        {\r\n            Set-PSMetaConfigVersionInfoV2\r\n        }\r\n    }\r\n    elseif($keywordData.ResourceName -eq 'OMI_ConfigurationDocument')\r\n    {\r\n        $canonicalizedValue['MinimumCompatibleVersion']='1.0.0'\r\n        $canonicalizedValue['CompatibleVersionAdditionalProperties']=@('Omi_BaseResource:ConfigurationName')\r\n    }\r\n\r\n    if(($keywordData.ResourceName -eq 'MSFT_DSCMetaConfiguration') -or ($keywordData.ResourceName -eq 'MSFT_DSCMetaConfigurationV2'))\r\n    {\r\n        if($canonicalizedValue['DebugMode'] -and @($canonicalizedValue['DebugMode']).Length -gt 1)\r\n        {\r\n            # we only allow one value for debug mode now.\r\n            Write-Error -ErrorRecord ([Microsoft.PowerShell.DesiredStateConfiguration.Internal.DscClassCache]::DebugModeShouldHaveOneValue())\r\n            Update-ConfigurationErrorCount\r\n        }\r\n    }\r\n        \r\n\r\n    # Generate the MOF text for this resource instance.\r\n    # when generate mof text for OMI_ConfigurationDocument we handle below two cases:\r\n    # 1. we will add versioning related property based on meta configuration instance already process\r\n    # 2. we update the existing OMI_ConfigurationDocument instance if it already exists when process meta configuration instance\r\n    $aliasId = ConvertTo-MOFInstance $keywordName $canonicalizedValue\r\n\r\n# If a OMI_ConfigurationDocument is executed outside of a node statement, it becomes the default\r\n# for all nodes that don't have an explicit OMI_ConfigurationDocument declaration\r\n    if ($keywordData.ResourceName -eq 'OMI_ConfigurationDocument' -and -not (Get-PSCurrentConfigurationNode))\r\n    {\r\n        $data = Get-MoFInstanceText $aliasId\r\n        Write-Debug \"${debugPrefix} DEFINING DEFAULT CONFIGURATION DOCUMENT: $data\"\r\n        Set-PSDefaultConfigurationDocument $data\r\n    }\r\n\r\n    Write-Debug \"${debugPrefix} MOF alias for this resource is '$aliasId'\"\r\n\r\n# always return the aliasId so the generated file will be well-formed if not valid\r\n    $aliasId\r\n\r\n    Write-Debug \"${debugPrefix} RESOURCE PROCESSING COMPLETED. TOTAL ERROR COUNT: $(Get-ConfigurationErrorCount)\"\r\n\r\n    ";

		// Token: 0x040031D6 RID: 12758
		private static PSTraceSource tracer = PSTraceSource.GetTracer("DSC", "DSC Class Cache");

		// Token: 0x040031D7 RID: 12759
		private static readonly string[] hiddenResourceList = new string[]
		{
			"MSFT_BaseConfigurationProviderRegistration",
			"MSFT_CimConfigurationProviderRegistration",
			"MSFT_PSConfigurationProviderRegistration"
		};

		// Token: 0x040031D8 RID: 12760
		private static readonly HashSet<string> hiddenResourceCache = new HashSet<string>(DscClassCache.hiddenResourceList, StringComparer.OrdinalIgnoreCase);

		// Token: 0x040031D9 RID: 12761
		private static readonly HashSet<string> CurrentImportingScriptFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040031DA RID: 12762
		[ThreadStatic]
		private static Dictionary<string, CimClass> _classCache;

		// Token: 0x040031DB RID: 12763
		[ThreadStatic]
		private static Dictionary<string, Tuple<string, Version>> _byClassModuleCache;

		// Token: 0x040031DC RID: 12764
		[ThreadStatic]
		private static Dictionary<string, List<CimClass>> _byFileClassCache;

		// Token: 0x040031DD RID: 12765
		[ThreadStatic]
		private static HashSet<string> _scriptKeywordFileCache;

		// Token: 0x040031DE RID: 12766
		private static readonly Tuple<string, Version> DefaultModuleInfoForResource = new Tuple<string, Version>("PSDesiredStateConfiguration", new Version("1.1"));

		// Token: 0x040031DF RID: 12767
		internal static readonly Tuple<string, Version> DefaultModuleInfoForMetaConfigResource = new Tuple<string, Version>("PSDesiredStateConfigurationEngine", new Version("2.0"));

		// Token: 0x040031E0 RID: 12768
		internal static readonly HashSet<string> SystemResourceNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"Node",
			"OMI_ConfigurationDocument"
		};

		// Token: 0x040031E1 RID: 12769
		[ThreadStatic]
		private static bool _cacheResourcesFromMultipleModuleVersions;

		// Token: 0x040031E2 RID: 12770
		private static readonly Dictionary<Type, string> mapPrimitiveDotNetTypeToMof = new Dictionary<Type, string>
		{
			{
				typeof(sbyte),
				"sint8"
			},
			{
				typeof(byte),
				"uint8"
			},
			{
				typeof(short),
				"sint16"
			},
			{
				typeof(ushort),
				"uint16"
			},
			{
				typeof(int),
				"sint32"
			},
			{
				typeof(uint),
				"uint32"
			},
			{
				typeof(long),
				"sint64"
			},
			{
				typeof(ulong),
				"uint64"
			},
			{
				typeof(float),
				"real32"
			},
			{
				typeof(double),
				"real64"
			},
			{
				typeof(bool),
				"boolean"
			},
			{
				typeof(string),
				"string"
			},
			{
				typeof(DateTime),
				"datetime"
			},
			{
				typeof(PSCredential),
				"string"
			},
			{
				typeof(char),
				"char16"
			}
		};

		// Token: 0x040031E3 RID: 12771
		private static ScriptBlock _cimKeywordImlementationFunction;
	}
}
