using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x0200098A RID: 2442
	internal class CompletionExecutionHelper
	{
		// Token: 0x06005A40 RID: 23104 RVA: 0x001E598A File Offset: 0x001E3B8A
		internal CompletionExecutionHelper(PowerShell powershell)
		{
			if (powershell == null)
			{
				throw PSTraceSource.NewArgumentNullException("powershell");
			}
			this.CurrentPowerShell = powershell;
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06005A41 RID: 23105 RVA: 0x001E59A7 File Offset: 0x001E3BA7
		// (set) Token: 0x06005A42 RID: 23106 RVA: 0x001E59AF File Offset: 0x001E3BAF
		internal bool CancelTabCompletion { get; set; }

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06005A43 RID: 23107 RVA: 0x001E59B8 File Offset: 0x001E3BB8
		// (set) Token: 0x06005A44 RID: 23108 RVA: 0x001E59C0 File Offset: 0x001E3BC0
		internal PowerShell CurrentPowerShell { get; set; }

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06005A45 RID: 23109 RVA: 0x001E59C9 File Offset: 0x001E3BC9
		internal bool IsRunning
		{
			get
			{
				return this.CurrentPowerShell.InvocationStateInfo.State == PSInvocationState.Running;
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06005A46 RID: 23110 RVA: 0x001E59DE File Offset: 0x001E3BDE
		internal bool IsStopped
		{
			get
			{
				return this.CurrentPowerShell.InvocationStateInfo.State == PSInvocationState.Stopped;
			}
		}

		// Token: 0x06005A47 RID: 23111 RVA: 0x001E59F4 File Offset: 0x001E3BF4
		internal Collection<PSObject> ExecuteCommand(string command)
		{
			Exception ex;
			return this.ExecuteCommand(command, true, out ex, null);
		}

		// Token: 0x06005A48 RID: 23112 RVA: 0x001E5A0C File Offset: 0x001E3C0C
		internal bool ExecuteCommandAndGetResultAsBool()
		{
			Exception ex;
			Collection<PSObject> collection = this.ExecuteCurrentPowerShell(out ex, null);
			return ex == null && collection != null && collection.Count != 0 && (collection.Count > 1 || LanguagePrimitives.IsTrue(collection[0]));
		}

		// Token: 0x06005A49 RID: 23113 RVA: 0x001E5A4C File Offset: 0x001E3C4C
		internal string ExecuteCommandAndGetResultAsString()
		{
			Exception ex;
			Collection<PSObject> collection = this.ExecuteCurrentPowerShell(out ex, null);
			if (ex != null || collection == null || collection.Count == 0)
			{
				return null;
			}
			if (collection[0] == null)
			{
				return string.Empty;
			}
			return CompletionExecutionHelper.SafeToString(collection[0]);
		}

		// Token: 0x06005A4A RID: 23114 RVA: 0x001E5A90 File Offset: 0x001E3C90
		internal Collection<PSObject> ExecuteCommand(string command, bool isScript, out Exception exceptionThrown, Hashtable args)
		{
			exceptionThrown = null;
			if (this.CancelTabCompletion)
			{
				return new Collection<PSObject>();
			}
			this.CurrentPowerShell.AddCommand(command);
			Command command2 = new Command(command, isScript);
			if (args != null)
			{
				foreach (object obj in args)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					command2.Parameters.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
			Collection<PSObject> result = null;
			try
			{
				if (this.IsStopped)
				{
					result = new Collection<PSObject>();
					this.CancelTabCompletion = true;
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				exceptionThrown = ex;
			}
			return result;
		}

		// Token: 0x06005A4B RID: 23115 RVA: 0x001E5B60 File Offset: 0x001E3D60
		internal Collection<PSObject> ExecuteCurrentPowerShell(out Exception exceptionThrown, IEnumerable input = null)
		{
			exceptionThrown = null;
			if (this.CancelTabCompletion)
			{
				return new Collection<PSObject>();
			}
			Collection<PSObject> result = null;
			try
			{
				result = this.CurrentPowerShell.Invoke(input);
				if (this.IsStopped)
				{
					result = new Collection<PSObject>();
					this.CancelTabCompletion = true;
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				exceptionThrown = ex;
			}
			finally
			{
				this.CurrentPowerShell.Commands.Clear();
			}
			return result;
		}

		// Token: 0x06005A4C RID: 23116 RVA: 0x001E5BE0 File Offset: 0x001E3DE0
		internal static string SafeToString(object obj)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			string result;
			try
			{
				PSObject psobject = obj as PSObject;
				string text;
				if (psobject != null)
				{
					object baseObject = psobject.BaseObject;
					if (baseObject != null && !(baseObject is PSCustomObject))
					{
						text = baseObject.ToString();
					}
					else
					{
						text = psobject.ToString();
					}
				}
				else
				{
					text = obj.ToString();
				}
				result = text;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06005A4D RID: 23117 RVA: 0x001E5C54 File Offset: 0x001E3E54
		internal static void SafeAddToStringList(List<string> list, object obj)
		{
			if (list == null)
			{
				return;
			}
			string text = CompletionExecutionHelper.SafeToString(obj);
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(text);
			}
		}
	}
}
