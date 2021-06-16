using System;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000AF RID: 175
	public class ModuleSpecification
	{
		// Token: 0x060008F5 RID: 2293 RVA: 0x000368AF File Offset: 0x00034AAF
		public ModuleSpecification()
		{
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x000368B8 File Offset: 0x00034AB8
		public ModuleSpecification(string moduleName)
		{
			if (string.IsNullOrEmpty(moduleName))
			{
				throw new ArgumentNullException("moduleName");
			}
			this.Name = moduleName;
			this.Version = null;
			this.RequiredVersion = null;
			this.MaximumVersion = null;
			this.Guid = null;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0003690C File Offset: 0x00034B0C
		public ModuleSpecification(Hashtable moduleSpecification)
		{
			if (moduleSpecification == null)
			{
				throw new ArgumentNullException("moduleSpecification");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in moduleSpecification)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (dictionaryEntry.Key.ToString().Equals("ModuleName", StringComparison.OrdinalIgnoreCase))
				{
					this.Name = LanguagePrimitives.ConvertTo<string>(dictionaryEntry.Value);
				}
				else if (dictionaryEntry.Key.ToString().Equals("ModuleVersion", StringComparison.OrdinalIgnoreCase))
				{
					this.Version = LanguagePrimitives.ConvertTo<Version>(dictionaryEntry.Value);
				}
				else if (dictionaryEntry.Key.ToString().Equals("RequiredVersion", StringComparison.OrdinalIgnoreCase))
				{
					this.RequiredVersion = LanguagePrimitives.ConvertTo<Version>(dictionaryEntry.Value);
				}
				else if (dictionaryEntry.Key.ToString().Equals("MaximumVersion", StringComparison.OrdinalIgnoreCase))
				{
					this.MaximumVersion = LanguagePrimitives.ConvertTo<string>(dictionaryEntry.Value);
				}
				else if (dictionaryEntry.Key.ToString().Equals("GUID", StringComparison.OrdinalIgnoreCase))
				{
					this.Guid = LanguagePrimitives.ConvertTo<Guid?>(dictionaryEntry.Value);
				}
				else
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("'");
					stringBuilder.Append(dictionaryEntry.Key.ToString());
					stringBuilder.Append("'");
				}
			}
			if (stringBuilder.Length != 0)
			{
				string message = StringUtil.Format(Modules.InvalidModuleSpecificationMember, "ModuleName, ModuleVersion, RequiredVersion, GUID", stringBuilder);
				throw new ArgumentException(message);
			}
			if (string.IsNullOrEmpty(this.Name))
			{
				string message = StringUtil.Format(Modules.RequiredModuleMissingModuleName, new object[0]);
				throw new MissingMemberException(message);
			}
			if (this.RequiredVersion == null && this.Version == null && this.MaximumVersion == null)
			{
				string message = StringUtil.Format(Modules.RequiredModuleMissingModuleVersion, new object[0]);
				throw new MissingMemberException(message);
			}
			if (this.RequiredVersion != null && this.Version != null)
			{
				string message = StringUtil.Format(SessionStateStrings.GetContent_TailAndHeadCannotCoexist, "ModuleVersion", "RequiredVersion");
				throw new ArgumentException(message);
			}
			if (this.RequiredVersion != null && this.MaximumVersion != null)
			{
				string message = StringUtil.Format(SessionStateStrings.GetContent_TailAndHeadCannotCoexist, "MaxiumVersion", "RequiredVersion");
				throw new ArgumentException(message);
			}
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00036BA0 File Offset: 0x00034DA0
		internal ModuleSpecification(PSModuleInfo moduleInfo)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			this.Name = moduleInfo.Name;
			this.Version = moduleInfo.Version;
			this.Guid = new Guid?(moduleInfo.Guid);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00036BE0 File Offset: 0x00034DE0
		public override string ToString()
		{
			string text = string.Empty;
			if (this.Guid == null && this.Version == null && this.RequiredVersion == null && this.MaximumVersion == null)
			{
				text = this.Name;
			}
			else
			{
				text = "@{ ModuleName = '" + this.Name + "'";
				if (this.Guid != null)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"; Guid = '{",
						this.Guid,
						"}' "
					});
				}
				if (this.RequiredVersion != null)
				{
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						"; RequiredVersion = '",
						this.RequiredVersion,
						"'"
					});
				}
				else
				{
					if (this.Version != null)
					{
						object obj3 = text;
						text = string.Concat(new object[]
						{
							obj3,
							"; ModuleVersion = '",
							this.Version,
							"'"
						});
					}
					if (this.MaximumVersion != null)
					{
						text = text + "; MaximumVersion = '" + this.MaximumVersion + "'";
					}
				}
				text += " }";
			}
			return text;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00036D48 File Offset: 0x00034F48
		public static bool TryParse(string input, out ModuleSpecification result)
		{
			result = null;
			try
			{
				Hashtable moduleSpecification;
				if (Parser.TryParseAsConstantHashtable(input, out moduleSpecification))
				{
					result = new ModuleSpecification(moduleSpecification);
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x00036D88 File Offset: 0x00034F88
		// (set) Token: 0x060008FC RID: 2300 RVA: 0x00036D90 File Offset: 0x00034F90
		public string Name { get; internal set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00036D99 File Offset: 0x00034F99
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x00036DA1 File Offset: 0x00034FA1
		public Guid? Guid { get; internal set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00036DAA File Offset: 0x00034FAA
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x00036DB2 File Offset: 0x00034FB2
		public Version Version { get; internal set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00036DBB File Offset: 0x00034FBB
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x00036DC3 File Offset: 0x00034FC3
		public string MaximumVersion { get; internal set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00036DCC File Offset: 0x00034FCC
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x00036DD4 File Offset: 0x00034FD4
		public Version RequiredVersion { get; internal set; }
	}
}
