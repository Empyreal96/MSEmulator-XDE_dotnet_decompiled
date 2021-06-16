using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x020008B8 RID: 2232
	internal static class Verbs
	{
		// Token: 0x060054D0 RID: 21712 RVA: 0x001BF630 File Offset: 0x001BD830
		static Verbs()
		{
			Type[] array = new Type[]
			{
				typeof(VerbsCommon),
				typeof(VerbsCommunications),
				typeof(VerbsData),
				typeof(VerbsDiagnostic),
				typeof(VerbsLifecycle),
				typeof(VerbsOther),
				typeof(VerbsSecurity)
			};
			foreach (Type type in array)
			{
				foreach (FieldInfo fieldInfo in type.GetFields())
				{
					if (fieldInfo.IsLiteral)
					{
						Verbs.validVerbs.Add((string)fieldInfo.GetValue(null), true);
					}
				}
			}
			Verbs.recommendedAlternateVerbs.Add("accept", new string[]
			{
				"Receive"
			});
			Verbs.recommendedAlternateVerbs.Add("acquire", new string[]
			{
				"Get",
				"Read"
			});
			Verbs.recommendedAlternateVerbs.Add("allocate", new string[]
			{
				"New"
			});
			Verbs.recommendedAlternateVerbs.Add("allow", new string[]
			{
				"Enable",
				"Grant",
				"Unblock"
			});
			Verbs.recommendedAlternateVerbs.Add("amend", new string[]
			{
				"Edit"
			});
			Verbs.recommendedAlternateVerbs.Add("analyze", new string[]
			{
				"Measure",
				"Test"
			});
			Verbs.recommendedAlternateVerbs.Add("append", new string[]
			{
				"Add"
			});
			Verbs.recommendedAlternateVerbs.Add("assign", new string[]
			{
				"Set"
			});
			Verbs.recommendedAlternateVerbs.Add("associate", new string[]
			{
				"Join",
				"Merge"
			});
			Verbs.recommendedAlternateVerbs.Add("attach", new string[]
			{
				"Add",
				"Debug"
			});
			Verbs.recommendedAlternateVerbs.Add("bc", new string[]
			{
				"Compare"
			});
			Verbs.recommendedAlternateVerbs.Add("boot", new string[]
			{
				"Start"
			});
			Verbs.recommendedAlternateVerbs.Add("break", new string[]
			{
				"Disconnect"
			});
			Verbs.recommendedAlternateVerbs.Add("broadcast", new string[]
			{
				"Send"
			});
			Verbs.recommendedAlternateVerbs.Add("build", new string[]
			{
				"New"
			});
			Verbs.recommendedAlternateVerbs.Add("burn", new string[]
			{
				"Backup"
			});
			Verbs.recommendedAlternateVerbs.Add("calculate", new string[]
			{
				"Measure"
			});
			Verbs.recommendedAlternateVerbs.Add("cancel", new string[]
			{
				"Stop"
			});
			Verbs.recommendedAlternateVerbs.Add("cat", new string[]
			{
				"Get"
			});
			Verbs.recommendedAlternateVerbs.Add("change", new string[]
			{
				"Convert",
				"Edit",
				"Rename"
			});
			Verbs.recommendedAlternateVerbs.Add("clean", new string[]
			{
				"Uninstall"
			});
			Verbs.recommendedAlternateVerbs.Add("clone", new string[]
			{
				"Copy"
			});
			Verbs.recommendedAlternateVerbs.Add("combine", new string[]
			{
				"Join",
				"Merge"
			});
			Verbs.recommendedAlternateVerbs.Add("compact", new string[]
			{
				"Compress"
			});
			Verbs.recommendedAlternateVerbs.Add("concatenate", new string[]
			{
				"Add"
			});
			Verbs.recommendedAlternateVerbs.Add("configure", new string[]
			{
				"Set"
			});
			Verbs.recommendedAlternateVerbs.Add("create", new string[]
			{
				"New"
			});
			Verbs.recommendedAlternateVerbs.Add("cut", new string[]
			{
				"Remove"
			});
			Verbs.recommendedAlternateVerbs.Add("delete", new string[]
			{
				"Remove"
			});
			Verbs.recommendedAlternateVerbs.Add("deploy", new string[]
			{
				"Install",
				"Publish"
			});
			Verbs.recommendedAlternateVerbs.Add("detach", new string[]
			{
				"Dismount",
				"Remove"
			});
			Verbs.recommendedAlternateVerbs.Add("determine", new string[]
			{
				"Measure",
				"Resolve"
			});
			Verbs.recommendedAlternateVerbs.Add("diagnose", new string[]
			{
				"Debug",
				"Test"
			});
			Verbs.recommendedAlternateVerbs.Add("diff", new string[]
			{
				"Checkpoint",
				"Compare"
			});
			Verbs.recommendedAlternateVerbs.Add("difference", new string[]
			{
				"Checkpoint",
				"Compare"
			});
			Verbs.recommendedAlternateVerbs.Add("dig", new string[]
			{
				"Trace"
			});
			Verbs.recommendedAlternateVerbs.Add("dir", new string[]
			{
				"Get"
			});
			Verbs.recommendedAlternateVerbs.Add("discard", new string[]
			{
				"Remove"
			});
			Verbs.recommendedAlternateVerbs.Add("display", new string[]
			{
				"Show",
				"Write"
			});
			Verbs.recommendedAlternateVerbs.Add("dispose", new string[]
			{
				"Remove"
			});
			Verbs.recommendedAlternateVerbs.Add("divide", new string[]
			{
				"Split"
			});
			Verbs.recommendedAlternateVerbs.Add("dump", new string[]
			{
				"Get"
			});
			Verbs.recommendedAlternateVerbs.Add("duplicate", new string[]
			{
				"Copy"
			});
			Verbs.recommendedAlternateVerbs.Add("empty", new string[]
			{
				"Clear"
			});
			Verbs.recommendedAlternateVerbs.Add("end", new string[]
			{
				"Stop"
			});
			Verbs.recommendedAlternateVerbs.Add("erase", new string[]
			{
				"Clear",
				"Remove"
			});
			Verbs.recommendedAlternateVerbs.Add("examine", new string[]
			{
				"Get"
			});
			Verbs.recommendedAlternateVerbs.Add("execute", new string[]
			{
				"Invoke"
			});
			Verbs.recommendedAlternateVerbs.Add("explode", new string[]
			{
				"Expand"
			});
			Verbs.recommendedAlternateVerbs.Add("extract", new string[]
			{
				"Export"
			});
			Verbs.recommendedAlternateVerbs.Add("fix", new string[]
			{
				"Repair",
				"Restore"
			});
			Verbs.recommendedAlternateVerbs.Add("flush", new string[]
			{
				"Clear"
			});
			Verbs.recommendedAlternateVerbs.Add("follow", new string[]
			{
				"Trace"
			});
			Verbs.recommendedAlternateVerbs.Add("generate", new string[]
			{
				"New"
			});
			Verbs.recommendedAlternateVerbs.Add("halt", new string[]
			{
				"Disable"
			});
			Verbs.recommendedAlternateVerbs.Add("in", new string[]
			{
				"ConvertTo"
			});
			Verbs.recommendedAlternateVerbs.Add("index", new string[]
			{
				"Update"
			});
			Verbs.recommendedAlternateVerbs.Add("initiate", new string[]
			{
				"Start"
			});
			Verbs.recommendedAlternateVerbs.Add("input", new string[]
			{
				"ConvertTo",
				"Unregister"
			});
			Verbs.recommendedAlternateVerbs.Add("insert", new string[]
			{
				"Add",
				"Unregister"
			});
			Verbs.recommendedAlternateVerbs.Add("inspect", new string[]
			{
				"Trace"
			});
			Verbs.recommendedAlternateVerbs.Add("kill", new string[]
			{
				"Stop"
			});
			Verbs.recommendedAlternateVerbs.Add("launch", new string[]
			{
				"Start"
			});
			Verbs.recommendedAlternateVerbs.Add("load", new string[]
			{
				"Import"
			});
			Verbs.recommendedAlternateVerbs.Add("locate", new string[]
			{
				"Search",
				"Select"
			});
			Verbs.recommendedAlternateVerbs.Add("logoff", new string[]
			{
				"Disconnect"
			});
			Verbs.recommendedAlternateVerbs.Add("mail", new string[]
			{
				"Send"
			});
			Verbs.recommendedAlternateVerbs.Add("make", new string[]
			{
				"New"
			});
			Verbs.recommendedAlternateVerbs.Add("match", new string[]
			{
				"Select"
			});
			Verbs.recommendedAlternateVerbs.Add("migrate", new string[]
			{
				"Move"
			});
			Verbs.recommendedAlternateVerbs.Add("modify", new string[]
			{
				"Edit"
			});
			Verbs.recommendedAlternateVerbs.Add("name", new string[]
			{
				"Move"
			});
			Verbs.recommendedAlternateVerbs.Add("nullify", new string[]
			{
				"Clear"
			});
			Verbs.recommendedAlternateVerbs.Add("obtain", new string[]
			{
				"Get"
			});
			Verbs.recommendedAlternateVerbs.Add("output", new string[]
			{
				"ConvertFrom"
			});
			Verbs.recommendedAlternateVerbs.Add("pause", new string[]
			{
				"Suspend",
				"Wait"
			});
			Verbs.recommendedAlternateVerbs.Add("peek", new string[]
			{
				"Receive"
			});
			Verbs.recommendedAlternateVerbs.Add("permit", new string[]
			{
				"Enable"
			});
			Verbs.recommendedAlternateVerbs.Add("purge", new string[]
			{
				"Clear",
				"Remove"
			});
			Verbs.recommendedAlternateVerbs.Add("pick", new string[]
			{
				"Select"
			});
			Verbs.recommendedAlternateVerbs.Add("prevent", new string[]
			{
				"Block"
			});
			Verbs.recommendedAlternateVerbs.Add("print", new string[]
			{
				"Write"
			});
			Verbs.recommendedAlternateVerbs.Add("prompt", new string[]
			{
				"Read"
			});
			Verbs.recommendedAlternateVerbs.Add("put", new string[]
			{
				"Send",
				"Write"
			});
			Verbs.recommendedAlternateVerbs.Add("puts", new string[]
			{
				"Write"
			});
			Verbs.recommendedAlternateVerbs.Add("quota", new string[]
			{
				"Limit"
			});
			Verbs.recommendedAlternateVerbs.Add("quote", new string[]
			{
				"Limit"
			});
			Verbs.recommendedAlternateVerbs.Add("rebuild", new string[]
			{
				"Initialize"
			});
			Verbs.recommendedAlternateVerbs.Add("recycle", new string[]
			{
				"Restart"
			});
			Verbs.recommendedAlternateVerbs.Add("refresh", new string[]
			{
				"Update"
			});
			Verbs.recommendedAlternateVerbs.Add("reinitialize", new string[]
			{
				"Initialize"
			});
			Verbs.recommendedAlternateVerbs.Add("release", new string[]
			{
				"Clear",
				"Install",
				"Publish",
				"Unlock"
			});
			Verbs.recommendedAlternateVerbs.Add("reload", new string[]
			{
				"Update"
			});
			Verbs.recommendedAlternateVerbs.Add("renew", new string[]
			{
				"Initialize",
				"Update"
			});
			Verbs.recommendedAlternateVerbs.Add("replicate", new string[]
			{
				"Copy"
			});
			Verbs.recommendedAlternateVerbs.Add("resample", new string[]
			{
				"Convert"
			});
			Verbs.recommendedAlternateVerbs.Add("restrict", new string[]
			{
				"Lock"
			});
			Verbs.recommendedAlternateVerbs.Add("return", new string[]
			{
				"Repair",
				"Restore"
			});
			Verbs.recommendedAlternateVerbs.Add("revert", new string[]
			{
				"Unpublish"
			});
			Verbs.recommendedAlternateVerbs.Add("revise", new string[]
			{
				"Edit"
			});
			Verbs.recommendedAlternateVerbs.Add("run", new string[]
			{
				"Invoke",
				"Start"
			});
			Verbs.recommendedAlternateVerbs.Add("salvage", new string[]
			{
				"Test"
			});
			Verbs.recommendedAlternateVerbs.Add("secure", new string[]
			{
				"Lock"
			});
			Verbs.recommendedAlternateVerbs.Add("separate", new string[]
			{
				"Split"
			});
			Verbs.recommendedAlternateVerbs.Add("setup", new string[]
			{
				"Initialize",
				"Install"
			});
			Verbs.recommendedAlternateVerbs.Add("sleep", new string[]
			{
				"Suspend",
				"Wait"
			});
			Verbs.recommendedAlternateVerbs.Add("starttransaction", new string[]
			{
				"Checkpoint"
			});
			Verbs.recommendedAlternateVerbs.Add("telnet", new string[]
			{
				"Connect"
			});
			Verbs.recommendedAlternateVerbs.Add("terminate", new string[]
			{
				"Stop"
			});
			Verbs.recommendedAlternateVerbs.Add("track", new string[]
			{
				"Trace"
			});
			Verbs.recommendedAlternateVerbs.Add("transfer", new string[]
			{
				"Move"
			});
			Verbs.recommendedAlternateVerbs.Add("type", new string[]
			{
				"Get"
			});
			Verbs.recommendedAlternateVerbs.Add("unite", new string[]
			{
				"Join",
				"Merge"
			});
			Verbs.recommendedAlternateVerbs.Add("unlink", new string[]
			{
				"Dismount"
			});
			Verbs.recommendedAlternateVerbs.Add("unmark", new string[]
			{
				"Clear"
			});
			Verbs.recommendedAlternateVerbs.Add("unrestrict", new string[]
			{
				"Unlock"
			});
			Verbs.recommendedAlternateVerbs.Add("unsecure", new string[]
			{
				"Unlock"
			});
			Verbs.recommendedAlternateVerbs.Add("unset", new string[]
			{
				"Clear"
			});
			Verbs.recommendedAlternateVerbs.Add("verify", new string[]
			{
				"Test"
			});
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x001C0812 File Offset: 0x001BEA12
		internal static bool IsStandard(string verb)
		{
			return Verbs.validVerbs.ContainsKey(verb);
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x001C0820 File Offset: 0x001BEA20
		internal static string[] SuggestedAlternates(string verb)
		{
			string[] result = null;
			Verbs.recommendedAlternateVerbs.TryGetValue(verb, out result);
			return result;
		}

		// Token: 0x04002BFC RID: 11260
		private static Dictionary<string, bool> validVerbs = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002BFD RID: 11261
		private static Dictionary<string, string[]> recommendedAlternateVerbs = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
	}
}
