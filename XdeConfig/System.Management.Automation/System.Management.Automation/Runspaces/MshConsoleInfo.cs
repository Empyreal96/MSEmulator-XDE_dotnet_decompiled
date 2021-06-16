using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Security;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200084F RID: 2127
	internal class MshConsoleInfo
	{
		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x060051E3 RID: 20963 RVA: 0x001B52A2 File Offset: 0x001B34A2
		internal Version PSVersion
		{
			get
			{
				return this.psVersion;
			}
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x060051E4 RID: 20964 RVA: 0x001B52AC File Offset: 0x001B34AC
		internal string MajorVersion
		{
			get
			{
				return this.psVersion.Major.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x060051E5 RID: 20965 RVA: 0x001B52D1 File Offset: 0x001B34D1
		internal Collection<PSSnapInInfo> PSSnapIns
		{
			get
			{
				return this.MergeDefaultExternalMshSnapins();
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x060051E6 RID: 20966 RVA: 0x001B52D9 File Offset: 0x001B34D9
		internal Collection<PSSnapInInfo> ExternalPSSnapIns
		{
			get
			{
				return this.externalPSSnapIns;
			}
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x060051E7 RID: 20967 RVA: 0x001B52E1 File Offset: 0x001B34E1
		internal bool IsDirty
		{
			get
			{
				return this.isDirty;
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x060051E8 RID: 20968 RVA: 0x001B52E9 File Offset: 0x001B34E9
		internal string Filename
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x001B52F1 File Offset: 0x001B34F1
		private MshConsoleInfo(Version version)
		{
			this.psVersion = version;
			this.isDirty = false;
			this.fileName = null;
			this.defaultPSSnapIns = new Collection<PSSnapInInfo>();
			this.externalPSSnapIns = new Collection<PSSnapInInfo>();
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x001B5324 File Offset: 0x001B3524
		internal static MshConsoleInfo CreateDefaultConfiguration()
		{
			MshConsoleInfo mshConsoleInfo = new MshConsoleInfo(PSVersionInfo.PSVersion);
			try
			{
				mshConsoleInfo.defaultPSSnapIns = PSSnapInReader.ReadEnginePSSnapIns();
			}
			catch (PSArgumentException innerException)
			{
				string cannotLoadDefaults = ConsoleInfoErrorStrings.CannotLoadDefaults;
				MshConsoleInfo._mshsnapinTracer.TraceError(cannotLoadDefaults, new object[0]);
				throw new PSSnapInException(cannotLoadDefaults, innerException);
			}
			catch (SecurityException innerException2)
			{
				string cannotLoadDefaults2 = ConsoleInfoErrorStrings.CannotLoadDefaults;
				MshConsoleInfo._mshsnapinTracer.TraceError(cannotLoadDefaults2, new object[0]);
				throw new PSSnapInException(cannotLoadDefaults2, innerException2);
			}
			return mshConsoleInfo;
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x001B53AC File Offset: 0x001B35AC
		internal static MshConsoleInfo CreateFromConsoleFile(string fileName, out PSConsoleLoadException cle)
		{
			MshConsoleInfo._mshsnapinTracer.WriteLine("Creating console info from file {0}", new object[]
			{
				fileName
			});
			MshConsoleInfo mshConsoleInfo = MshConsoleInfo.CreateDefaultConfiguration();
			string fullPath = Path.GetFullPath(fileName);
			mshConsoleInfo.fileName = fullPath;
			mshConsoleInfo.Load(fullPath, out cle);
			MshConsoleInfo._mshsnapinTracer.WriteLine("Console info created successfully", new object[0]);
			return mshConsoleInfo;
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x001B5408 File Offset: 0x001B3608
		internal void SaveAsConsoleFile(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			string text = path;
			if (!Path.IsPathRooted(text))
			{
				text = Path.GetFullPath(this.fileName);
			}
			if (!text.EndsWith(".psc1", StringComparison.OrdinalIgnoreCase))
			{
				MshConsoleInfo._mshsnapinTracer.TraceError("Console file {0} doesn't have the right extension {1}.", new object[]
				{
					path,
					".psc1"
				});
				throw PSTraceSource.NewArgumentException("absolutePath", ConsoleInfoErrorStrings.BadConsoleExtension, new object[0]);
			}
			PSConsoleFileElement.WriteToFile(text, this.PSVersion, this.ExternalPSSnapIns);
			this.fileName = text;
			this.isDirty = false;
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x001B54A0 File Offset: 0x001B36A0
		internal void Save()
		{
			if (this.fileName == null)
			{
				throw PSTraceSource.NewInvalidOperationException(ConsoleInfoErrorStrings.SaveDefaultError, new object[0]);
			}
			PSConsoleFileElement.WriteToFile(this.fileName, this.PSVersion, this.ExternalPSSnapIns);
			this.isDirty = false;
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x001B54DC File Offset: 0x001B36DC
		internal PSSnapInInfo AddPSSnapIn(string mshSnapInID)
		{
			if (string.IsNullOrEmpty(mshSnapInID))
			{
				PSTraceSource.NewArgumentNullException("mshSnapInID");
			}
			if (MshConsoleInfo.IsDefaultPSSnapIn(mshSnapInID, this.defaultPSSnapIns))
			{
				MshConsoleInfo._mshsnapinTracer.TraceError("MshSnapin {0} can't be added since it is a default mshsnapin", new object[]
				{
					mshSnapInID
				});
				throw PSTraceSource.NewArgumentException("mshSnapInID", ConsoleInfoErrorStrings.CannotLoadDefault, new object[0]);
			}
			if (this.IsActiveExternalPSSnapIn(mshSnapInID))
			{
				MshConsoleInfo._mshsnapinTracer.TraceError("MshSnapin {0} is already loaded.", new object[]
				{
					mshSnapInID
				});
				throw PSTraceSource.NewArgumentException("mshSnapInID", ConsoleInfoErrorStrings.PSSnapInAlreadyExists, new object[]
				{
					mshSnapInID
				});
			}
			PSSnapInInfo pssnapInInfo = PSSnapInReader.Read(this.MajorVersion, mshSnapInID);
			if (!Utils.IsPSVersionSupported(pssnapInInfo.PSVersion.ToString()))
			{
				MshConsoleInfo._mshsnapinTracer.TraceError("MshSnapin {0} and current monad engine's versions don't match.", new object[]
				{
					mshSnapInID
				});
				throw PSTraceSource.NewArgumentException("mshSnapInID", ConsoleInfoErrorStrings.AddPSSnapInBadMonadVersion, new object[]
				{
					pssnapInInfo.PSVersion.ToString(),
					this.psVersion.ToString()
				});
			}
			this.externalPSSnapIns.Add(pssnapInInfo);
			MshConsoleInfo._mshsnapinTracer.WriteLine("MshSnapin {0} successfully added to consoleinfo list.", new object[]
			{
				mshSnapInID
			});
			this.isDirty = true;
			return pssnapInInfo;
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x001B5624 File Offset: 0x001B3824
		internal PSSnapInInfo RemovePSSnapIn(string mshSnapInID)
		{
			if (string.IsNullOrEmpty(mshSnapInID))
			{
				PSTraceSource.NewArgumentNullException("mshSnapInID");
			}
			PSSnapInInfo.VerifyPSSnapInFormatThrowIfError(mshSnapInID);
			PSSnapInInfo pssnapInInfo = null;
			foreach (PSSnapInInfo pssnapInInfo2 in this.externalPSSnapIns)
			{
				if (string.Equals(mshSnapInID, pssnapInInfo2.Name, StringComparison.OrdinalIgnoreCase))
				{
					pssnapInInfo = pssnapInInfo2;
					this.externalPSSnapIns.Remove(pssnapInInfo2);
					this.isDirty = true;
					break;
				}
			}
			if (pssnapInInfo != null)
			{
				return pssnapInInfo;
			}
			if (MshConsoleInfo.IsDefaultPSSnapIn(mshSnapInID, this.defaultPSSnapIns))
			{
				MshConsoleInfo._mshsnapinTracer.WriteLine("MshSnapin {0} can't be removed since it is a default mshsnapin.", new object[]
				{
					mshSnapInID
				});
				throw PSTraceSource.NewArgumentException("mshSnapInID", ConsoleInfoErrorStrings.CannotRemoveDefault, new object[]
				{
					mshSnapInID
				});
			}
			throw PSTraceSource.NewArgumentException("mshSnapInID", ConsoleInfoErrorStrings.CannotRemovePSSnapIn, new object[]
			{
				mshSnapInID
			});
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x001B5718 File Offset: 0x001B3918
		internal Collection<PSSnapInInfo> GetPSSnapIn(string pattern, bool searchRegistry)
		{
			bool flag = WildcardPattern.ContainsWildcardCharacters(pattern);
			if (!flag)
			{
				PSSnapInInfo.VerifyPSSnapInFormatThrowIfError(pattern);
			}
			Collection<PSSnapInInfo> collection = searchRegistry ? PSSnapInReader.ReadAll() : this.PSSnapIns;
			Collection<PSSnapInInfo> collection2 = new Collection<PSSnapInInfo>();
			if (collection == null)
			{
				return collection2;
			}
			if (!flag)
			{
				using (IEnumerator<PSSnapInInfo> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PSSnapInInfo pssnapInInfo = enumerator.Current;
						if (string.Equals(pssnapInInfo.Name, pattern, StringComparison.OrdinalIgnoreCase))
						{
							collection2.Add(pssnapInInfo);
						}
					}
					return collection2;
				}
			}
			WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
			foreach (PSSnapInInfo pssnapInInfo2 in collection)
			{
				if (wildcardPattern.IsMatch(pssnapInInfo2.Name))
				{
					collection2.Add(pssnapInInfo2);
				}
			}
			return collection2;
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x001B57FC File Offset: 0x001B39FC
		private Collection<PSSnapInInfo> Load(string path, out PSConsoleLoadException cle)
		{
			cle = null;
			MshConsoleInfo._mshsnapinTracer.WriteLine("Load mshsnapins from console file {0}", new object[]
			{
				path
			});
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!Path.IsPathRooted(path))
			{
				MshConsoleInfo._mshsnapinTracer.TraceError("Console file {0} needs to be a absolute path.", new object[]
				{
					path
				});
				throw PSTraceSource.NewArgumentException("path", ConsoleInfoErrorStrings.PathNotAbsolute, new object[]
				{
					path
				});
			}
			if (!path.EndsWith(".psc1", StringComparison.OrdinalIgnoreCase))
			{
				MshConsoleInfo._mshsnapinTracer.TraceError("Console file {0} needs to have {1} extension.", new object[]
				{
					path,
					".psc1"
				});
				throw PSTraceSource.NewArgumentException("path", ConsoleInfoErrorStrings.BadConsoleExtension, new object[0]);
			}
			PSConsoleFileElement psconsoleFileElement = PSConsoleFileElement.CreateFromFile(path);
			if (!Utils.IsPSVersionSupported(psconsoleFileElement.MonadVersion))
			{
				MshConsoleInfo._mshsnapinTracer.TraceError("Console version {0} is not supported in current monad session.", new object[]
				{
					psconsoleFileElement.MonadVersion
				});
				throw PSTraceSource.NewArgumentException("PSVersion", ConsoleInfoErrorStrings.BadMonadVersion, new object[]
				{
					psconsoleFileElement.MonadVersion,
					this.psVersion.ToString()
				});
			}
			Collection<PSSnapInException> collection = new Collection<PSSnapInException>();
			foreach (string text in psconsoleFileElement.PSSnapIns)
			{
				try
				{
					this.AddPSSnapIn(text);
				}
				catch (PSArgumentException ex)
				{
					PSSnapInException item = new PSSnapInException(text, ex.Message, ex);
					collection.Add(item);
				}
				catch (SecurityException exception)
				{
					string pssnapInReadError = ConsoleInfoErrorStrings.PSSnapInReadError;
					PSSnapInException item2 = new PSSnapInException(text, pssnapInReadError, exception);
					collection.Add(item2);
				}
			}
			if (collection.Count > 0)
			{
				cle = new PSConsoleLoadException(this, collection);
			}
			this.isDirty = false;
			return this.externalPSSnapIns;
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x001B59F8 File Offset: 0x001B3BF8
		internal static bool IsDefaultPSSnapIn(string mshSnapInID, IEnumerable<PSSnapInInfo> defaultSnapins)
		{
			foreach (PSSnapInInfo pssnapInInfo in defaultSnapins)
			{
				if (string.Equals(mshSnapInID, pssnapInInfo.Name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x001B5A50 File Offset: 0x001B3C50
		private bool IsActiveExternalPSSnapIn(string mshSnapInID)
		{
			foreach (PSSnapInInfo pssnapInInfo in this.externalPSSnapIns)
			{
				if (string.Equals(mshSnapInID, pssnapInInfo.Name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x001B5AAC File Offset: 0x001B3CAC
		private Collection<PSSnapInInfo> MergeDefaultExternalMshSnapins()
		{
			Collection<PSSnapInInfo> collection = new Collection<PSSnapInInfo>();
			foreach (PSSnapInInfo item in this.defaultPSSnapIns)
			{
				collection.Add(item);
			}
			foreach (PSSnapInInfo item2 in this.externalPSSnapIns)
			{
				collection.Add(item2);
			}
			return collection;
		}

		// Token: 0x04002A1F RID: 10783
		private readonly Version psVersion;

		// Token: 0x04002A20 RID: 10784
		private readonly Collection<PSSnapInInfo> externalPSSnapIns;

		// Token: 0x04002A21 RID: 10785
		private Collection<PSSnapInInfo> defaultPSSnapIns;

		// Token: 0x04002A22 RID: 10786
		private bool isDirty;

		// Token: 0x04002A23 RID: 10787
		private string fileName;

		// Token: 0x04002A24 RID: 10788
		private static readonly PSTraceSource _mshsnapinTracer = PSTraceSource.GetTracer("MshSnapinLoadUnload", "Loading and unloading mshsnapins", false);
	}
}
