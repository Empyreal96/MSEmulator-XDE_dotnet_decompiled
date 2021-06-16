using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x020005E9 RID: 1513
	internal static class TypeAccelerators
	{
		// Token: 0x060040CA RID: 16586 RVA: 0x001582CC File Offset: 0x001564CC
		static TypeAccelerators()
		{
			foreach (KeyValuePair<Type, string[]> keyValuePair in CoreTypes.Items.Value)
			{
				if (keyValuePair.Value != null)
				{
					foreach (string key in keyValuePair.Value)
					{
						TypeAccelerators.builtinTypeAccelerators.Add(key, keyValuePair.Key);
					}
				}
			}
			TypeAccelerators.builtinTypeAccelerators.Add("scriptblock", typeof(ScriptBlock));
			TypeAccelerators.builtinTypeAccelerators.Add("psvariable", typeof(PSVariable));
			TypeAccelerators.builtinTypeAccelerators.Add("type", typeof(Type));
			TypeAccelerators.builtinTypeAccelerators.Add("psmoduleinfo", typeof(PSModuleInfo));
			TypeAccelerators.builtinTypeAccelerators.Add("powershell", typeof(PowerShell));
			TypeAccelerators.builtinTypeAccelerators.Add("runspacefactory", typeof(RunspaceFactory));
			TypeAccelerators.builtinTypeAccelerators.Add("runspace", typeof(Runspace));
			TypeAccelerators.builtinTypeAccelerators.Add("initialsessionstate", typeof(InitialSessionState));
			TypeAccelerators.builtinTypeAccelerators.Add("psscriptmethod", typeof(PSScriptMethod));
			TypeAccelerators.builtinTypeAccelerators.Add("psscriptproperty", typeof(PSScriptProperty));
			TypeAccelerators.builtinTypeAccelerators.Add("psnoteproperty", typeof(PSNoteProperty));
			TypeAccelerators.builtinTypeAccelerators.Add("psaliasproperty", typeof(PSAliasProperty));
			TypeAccelerators.builtinTypeAccelerators.Add("psvariableproperty", typeof(PSVariableProperty));
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x001584C0 File Offset: 0x001566C0
		internal static string FindBuiltinAccelerator(Type type)
		{
			foreach (KeyValuePair<string, Type> keyValuePair in TypeAccelerators.builtinTypeAccelerators)
			{
				if (keyValuePair.Value == type)
				{
					return keyValuePair.Key;
				}
			}
			return null;
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00158528 File Offset: 0x00156728
		public static void Add(string typeName, Type type)
		{
			TypeAccelerators.userTypeAccelerators[typeName] = type;
			if (TypeAccelerators.allTypeAccelerators != null)
			{
				TypeAccelerators.allTypeAccelerators[typeName] = type;
			}
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x00158549 File Offset: 0x00156749
		public static bool Remove(string typeName)
		{
			TypeAccelerators.userTypeAccelerators.Remove(typeName);
			if (TypeAccelerators.allTypeAccelerators != null)
			{
				TypeAccelerators.allTypeAccelerators.Remove(typeName);
			}
			return true;
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x060040CE RID: 16590 RVA: 0x0015856B File Offset: 0x0015676B
		public static Dictionary<string, Type> Get
		{
			get
			{
				if (TypeAccelerators.allTypeAccelerators == null)
				{
					TypeAccelerators.allTypeAccelerators = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
					TypeAccelerators.FillCache(TypeAccelerators.allTypeAccelerators);
				}
				return TypeAccelerators.allTypeAccelerators;
			}
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x00158594 File Offset: 0x00156794
		internal static void FillCache(Dictionary<string, Type> cache)
		{
			foreach (KeyValuePair<string, Type> keyValuePair in TypeAccelerators.builtinTypeAccelerators)
			{
				cache.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<string, Type> keyValuePair2 in TypeAccelerators.userTypeAccelerators)
			{
				cache.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
		}

		// Token: 0x0400208C RID: 8332
		internal static Dictionary<string, Type> builtinTypeAccelerators = new Dictionary<string, Type>(64, StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400208D RID: 8333
		internal static Dictionary<string, Type> userTypeAccelerators = new Dictionary<string, Type>(64, StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400208E RID: 8334
		private static Dictionary<string, Type> allTypeAccelerators;
	}
}
