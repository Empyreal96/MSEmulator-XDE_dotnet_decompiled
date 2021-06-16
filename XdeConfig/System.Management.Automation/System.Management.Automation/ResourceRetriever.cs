using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace System.Management.Automation
{
	// Token: 0x02000853 RID: 2131
	internal class ResourceRetriever : MarshalByRefObject
	{
		// Token: 0x06005212 RID: 21010 RVA: 0x001B6244 File Offset: 0x001B4444
		internal string GetStringResource(string assemblyName, string modulePath, string baseName, string resourceID)
		{
			string result = null;
			Assembly assembly = ResourceRetriever.LoadAssembly(assemblyName, modulePath);
			if (!(assembly == null))
			{
				CultureInfo cultureInfo = CultureInfo.CurrentUICulture;
				Stream manifestResourceStream;
				for (;;)
				{
					string text = baseName;
					if (!string.IsNullOrEmpty(cultureInfo.Name))
					{
						text = text + "." + cultureInfo.Name;
					}
					text += ".resources";
					manifestResourceStream = assembly.GetManifestResourceStream(text);
					if (manifestResourceStream != null || string.IsNullOrEmpty(cultureInfo.Name))
					{
						break;
					}
					cultureInfo = cultureInfo.Parent;
				}
				if (manifestResourceStream != null)
				{
					result = ResourceRetriever.GetString(manifestResourceStream, resourceID);
				}
			}
			return result;
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x001B62D0 File Offset: 0x001B44D0
		private static Assembly LoadAssembly(string assemblyName, string modulePath)
		{
			AssemblyName assemblyName2 = new AssemblyName(assemblyName);
			string directoryName = Path.GetDirectoryName(modulePath);
			string fileName = Path.GetFileName(modulePath);
			CultureInfo cultureInfo = CultureInfo.CurrentUICulture;
			Assembly assembly;
			for (;;)
			{
				assembly = ResourceRetriever.LoadAssemblyForCulture(cultureInfo, assemblyName2, directoryName, fileName);
				if (assembly != null || string.IsNullOrEmpty(cultureInfo.Name))
				{
					break;
				}
				cultureInfo = cultureInfo.Parent;
			}
			return assembly;
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x001B632C File Offset: 0x001B452C
		private static Assembly LoadAssemblyForCulture(CultureInfo culture, AssemblyName assemblyName, string moduleBase, string moduleFile)
		{
			Assembly assembly = null;
			assemblyName.CultureInfo = culture;
			try
			{
				assembly = Assembly.ReflectionOnlyLoad(assemblyName.FullName);
			}
			catch (FileLoadException)
			{
			}
			catch (BadImageFormatException)
			{
			}
			catch (FileNotFoundException)
			{
			}
			if (assembly != null)
			{
				return assembly;
			}
			string name = assemblyName.Name;
			try
			{
				assemblyName.Name = name + ".resources";
				assembly = Assembly.ReflectionOnlyLoad(assemblyName.FullName);
			}
			catch (FileLoadException)
			{
			}
			catch (BadImageFormatException)
			{
			}
			catch (FileNotFoundException)
			{
			}
			if (assembly != null)
			{
				return assembly;
			}
			assemblyName.Name = name;
			string text = Path.Combine(moduleBase, culture.Name);
			text = Path.Combine(text, moduleFile);
			if (File.Exists(text))
			{
				try
				{
					assembly = Assembly.ReflectionOnlyLoadFrom(text);
				}
				catch (FileLoadException)
				{
				}
				catch (BadImageFormatException)
				{
				}
				catch (FileNotFoundException)
				{
				}
			}
			return assembly;
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x001B6448 File Offset: 0x001B4648
		private static string GetString(Stream stream, string resourceID)
		{
			string result = null;
			ResourceReader resourceReader = new ResourceReader(stream);
			foreach (object obj in resourceReader)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (string.Equals(resourceID, (string)dictionaryEntry.Key, StringComparison.OrdinalIgnoreCase))
				{
					result = (string)dictionaryEntry.Value;
					break;
				}
			}
			return result;
		}
	}
}
