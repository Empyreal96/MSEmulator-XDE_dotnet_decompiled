using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000915 RID: 2325
	internal class MshExpression
	{
		// Token: 0x06005742 RID: 22338 RVA: 0x001C810A File Offset: 0x001C630A
		internal MshExpression(string s) : this(s, false)
		{
		}

		// Token: 0x06005743 RID: 22339 RVA: 0x001C8114 File Offset: 0x001C6314
		internal MshExpression(string s, bool isResolved)
		{
			if (string.IsNullOrEmpty(s))
			{
				throw PSTraceSource.NewArgumentNullException("s");
			}
			this._stringValue = s;
			this._isResolved = isResolved;
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x001C813D File Offset: 0x001C633D
		internal MshExpression(ScriptBlock scriptBlock)
		{
			if (scriptBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("scriptBlock");
			}
			this._script = scriptBlock;
		}

		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x06005745 RID: 22341 RVA: 0x001C815A File Offset: 0x001C635A
		public ScriptBlock Script
		{
			get
			{
				return this._script;
			}
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x001C8162 File Offset: 0x001C6362
		public override string ToString()
		{
			if (this._script != null)
			{
				return this._script.ToString();
			}
			return this._stringValue;
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x001C817E File Offset: 0x001C637E
		internal List<MshExpression> ResolveNames(PSObject target)
		{
			return this.ResolveNames(target, true);
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x06005748 RID: 22344 RVA: 0x001C8188 File Offset: 0x001C6388
		internal bool HasWildCardCharacters
		{
			get
			{
				return this._script == null && WildcardPattern.ContainsWildcardCharacters(this._stringValue);
			}
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x001C81A0 File Offset: 0x001C63A0
		internal List<MshExpression> ResolveNames(PSObject target, bool expand)
		{
			List<MshExpression> list = new List<MshExpression>();
			if (this._isResolved)
			{
				list.Add(this);
				return list;
			}
			if (this._script != null)
			{
				list.Add(new MshExpression(this._script)
				{
					_isResolved = true
				});
				return list;
			}
			IEnumerable<PSMemberInfo> enumerable;
			if (this.HasWildCardCharacters)
			{
				enumerable = target.Members.Match(this._stringValue, PSMemberTypes.AliasProperty | PSMemberTypes.CodeProperty | PSMemberTypes.Property | PSMemberTypes.NoteProperty | PSMemberTypes.ScriptProperty | PSMemberTypes.PropertySet);
			}
			else
			{
				PSMemberInfo psmemberInfo = target.Members[this._stringValue];
				List<PSMemberInfo> list2 = new List<PSMemberInfo>();
				if (psmemberInfo != null)
				{
					list2.Add(psmemberInfo);
				}
				enumerable = list2;
			}
			List<PSMemberInfo> list3 = new List<PSMemberInfo>();
			foreach (PSMemberInfo psmemberInfo2 in enumerable)
			{
				PSPropertySet pspropertySet = psmemberInfo2 as PSPropertySet;
				if (pspropertySet != null)
				{
					if (expand)
					{
						Collection<string> referencedPropertyNames = pspropertySet.ReferencedPropertyNames;
						for (int i = 0; i < referencedPropertyNames.Count; i++)
						{
							ReadOnlyPSMemberInfoCollection<PSPropertyInfo> readOnlyPSMemberInfoCollection = target.Properties.Match(referencedPropertyNames[i]);
							for (int j = 0; j < readOnlyPSMemberInfoCollection.Count; j++)
							{
								list3.Add(readOnlyPSMemberInfoCollection[j]);
							}
						}
					}
				}
				else if (psmemberInfo2 is PSPropertyInfo)
				{
					list3.Add(psmemberInfo2);
				}
			}
			Hashtable hashtable = new Hashtable();
			foreach (PSMemberInfo psmemberInfo3 in list3)
			{
				if (!hashtable.ContainsKey(psmemberInfo3.Name))
				{
					list.Add(new MshExpression(psmemberInfo3.Name)
					{
						_isResolved = true
					});
					hashtable.Add(psmemberInfo3.Name, null);
				}
			}
			return list;
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x001C8374 File Offset: 0x001C6574
		internal List<MshExpressionResult> GetValues(PSObject target)
		{
			return this.GetValues(target, true, true);
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x001C8380 File Offset: 0x001C6580
		internal List<MshExpressionResult> GetValues(PSObject target, bool expand, bool eatExceptions)
		{
			List<MshExpressionResult> list = new List<MshExpressionResult>();
			if (this._script != null)
			{
				MshExpression mshExpression = new MshExpression(this._script);
				MshExpressionResult value = mshExpression.GetValue(target, eatExceptions);
				list.Add(value);
				return list;
			}
			List<MshExpression> list2 = this.ResolveNames(target, expand);
			foreach (MshExpression mshExpression2 in list2)
			{
				MshExpressionResult value2 = mshExpression2.GetValue(target, eatExceptions);
				list.Add(value2);
			}
			return list;
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x001C8414 File Offset: 0x001C6614
		private MshExpressionResult GetValue(PSObject target, bool eatExceptions)
		{
			MshExpressionResult result;
			try
			{
				object res;
				if (this._script != null)
				{
					res = this._script.DoInvokeReturnAsIs(true, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, target, AutomationNull.Value, AutomationNull.Value, new object[0]);
				}
				else
				{
					PSMemberInfo psmemberInfo = target.Properties[this._stringValue];
					if (psmemberInfo == null)
					{
						return new MshExpressionResult(null, this, null);
					}
					res = psmemberInfo.Value;
				}
				result = new MshExpressionResult(res, this, null);
			}
			catch (RuntimeException e)
			{
				if (!eatExceptions)
				{
					throw;
				}
				result = new MshExpressionResult(null, this, e);
			}
			return result;
		}

		// Token: 0x04002E78 RID: 11896
		private string _stringValue;

		// Token: 0x04002E79 RID: 11897
		private ScriptBlock _script;

		// Token: 0x04002E7A RID: 11898
		private bool _isResolved;
	}
}
