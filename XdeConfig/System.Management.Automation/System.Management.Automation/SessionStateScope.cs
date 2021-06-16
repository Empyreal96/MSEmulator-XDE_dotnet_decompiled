using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x0200083C RID: 2108
	internal sealed class SessionStateScope
	{
		// Token: 0x06005114 RID: 20756 RVA: 0x001B02F8 File Offset: 0x001AE4F8
		internal SessionStateScope(SessionStateScope parentScope)
		{
			this.ScopeOrigin = CommandOrigin.Internal;
			this.Parent = parentScope;
			if (parentScope != null)
			{
				this._scriptScope = parentScope.ScriptScope;
				return;
			}
			this._scriptScope = this;
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x001B036C File Offset: 0x001AE56C
		private SessionStateCapacityVariable CreateCapacityVariable(string variableName, int defaultCapacity, int maxCapacity, int minCapacity, string descriptionResourceString)
		{
			SessionStateCapacityVariable sessionStateCapacityVariable = null;
			if (this.Parent != null)
			{
				sessionStateCapacityVariable = (this.Parent.GetVariable(variableName) as SessionStateCapacityVariable);
			}
			if (sessionStateCapacityVariable == null)
			{
				sessionStateCapacityVariable = new SessionStateCapacityVariable(variableName, defaultCapacity, maxCapacity, minCapacity, ScopedItemOptions.None);
			}
			else
			{
				sessionStateCapacityVariable = new SessionStateCapacityVariable(variableName, sessionStateCapacityVariable, ScopedItemOptions.None);
			}
			if (string.IsNullOrEmpty(sessionStateCapacityVariable.Description))
			{
				sessionStateCapacityVariable.Description = descriptionResourceString;
			}
			return sessionStateCapacityVariable;
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06005116 RID: 20758 RVA: 0x001B03C5 File Offset: 0x001AE5C5
		// (set) Token: 0x06005117 RID: 20759 RVA: 0x001B03CD File Offset: 0x001AE5CD
		internal SessionStateScope Parent { get; set; }

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x06005118 RID: 20760 RVA: 0x001B03D6 File Offset: 0x001AE5D6
		// (set) Token: 0x06005119 RID: 20761 RVA: 0x001B03DE File Offset: 0x001AE5DE
		internal CommandOrigin ScopeOrigin { get; set; }

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x0600511A RID: 20762 RVA: 0x001B03E7 File Offset: 0x001AE5E7
		// (set) Token: 0x0600511B RID: 20763 RVA: 0x001B03EF File Offset: 0x001AE5EF
		internal SessionStateScope ScriptScope
		{
			get
			{
				return this._scriptScope;
			}
			set
			{
				this._scriptScope = value;
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x0600511C RID: 20764 RVA: 0x001B03F8 File Offset: 0x001AE5F8
		// (set) Token: 0x0600511D RID: 20765 RVA: 0x001B0400 File Offset: 0x001AE600
		internal Version StrictModeVersion { get; set; }

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x0600511E RID: 20766 RVA: 0x001B0409 File Offset: 0x001AE609
		// (set) Token: 0x0600511F RID: 20767 RVA: 0x001B0411 File Offset: 0x001AE611
		internal MutableTuple LocalsTuple { get; set; }

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06005120 RID: 20768 RVA: 0x001B041A File Offset: 0x001AE61A
		internal Stack<MutableTuple> DottedScopes
		{
			get
			{
				return this._dottedScopes;
			}
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x001B0424 File Offset: 0x001AE624
		internal void NewDrive(PSDriveInfo newDrive)
		{
			if (newDrive == null)
			{
				throw PSTraceSource.NewArgumentNullException("newDrive");
			}
			if (this.GetDrives().ContainsKey(newDrive.Name))
			{
				SessionStateException ex = new SessionStateException(newDrive.Name, SessionStateCategory.Drive, "DriveAlreadyExists", SessionStateStrings.DriveAlreadyExists, ErrorCategory.ResourceExists, new object[0]);
				throw ex;
			}
			if (!newDrive.IsAutoMounted && this.GetDrives().Count > this.DriveCapacity.FastValue - 1)
			{
				SessionStateOverflowException ex2 = new SessionStateOverflowException(newDrive.Name, SessionStateCategory.Drive, "DriveOverflow", SessionStateStrings.DriveOverflow, new object[]
				{
					this.DriveCapacity.FastValue
				});
				throw ex2;
			}
			if (!newDrive.IsAutoMounted)
			{
				this.GetDrives().Add(newDrive.Name, newDrive);
				return;
			}
			if (!this.GetAutomountedDrives().ContainsKey(newDrive.Name))
			{
				this.GetAutomountedDrives().Add(newDrive.Name, newDrive);
			}
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x001B0510 File Offset: 0x001AE710
		internal void RemoveDrive(PSDriveInfo drive)
		{
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			if (this._drives == null)
			{
				return;
			}
			if (this.GetDrives().ContainsKey(drive.Name))
			{
				this.GetDrives().Remove(drive.Name);
				return;
			}
			if (this.GetAutomountedDrives().ContainsKey(drive.Name))
			{
				this.GetAutomountedDrives()[drive.Name].IsAutoMountedManuallyRemoved = true;
				if (drive.IsNetworkDrive)
				{
					this.GetAutomountedDrives().Remove(drive.Name);
				}
			}
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x001B05A4 File Offset: 0x001AE7A4
		internal void RemoveAllDrives()
		{
			this.GetDrives().Clear();
			this.GetAutomountedDrives().Clear();
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x001B05BC File Offset: 0x001AE7BC
		internal PSDriveInfo GetDrive(string name)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			PSDriveInfo result = null;
			if (this.GetDrives().ContainsKey(name))
			{
				result = this.GetDrives()[name];
			}
			else if (this.GetAutomountedDrives().ContainsKey(name))
			{
				result = this.GetAutomountedDrives()[name];
			}
			return result;
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06005125 RID: 20773 RVA: 0x001B0614 File Offset: 0x001AE814
		internal IEnumerable<PSDriveInfo> Drives
		{
			get
			{
				Collection<PSDriveInfo> collection = new Collection<PSDriveInfo>();
				foreach (PSDriveInfo item in this.GetDrives().Values)
				{
					collection.Add(item);
				}
				foreach (PSDriveInfo psdriveInfo in this.GetAutomountedDrives().Values)
				{
					if (!psdriveInfo.IsAutoMountedManuallyRemoved)
					{
						collection.Add(psdriveInfo);
					}
				}
				return collection;
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06005126 RID: 20774 RVA: 0x001B06C4 File Offset: 0x001AE8C4
		internal IDictionary<string, PSVariable> Variables
		{
			get
			{
				return this.GetPrivateVariables();
			}
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x001B06CC File Offset: 0x001AE8CC
		internal PSVariable GetVariable(string name, CommandOrigin origin)
		{
			PSVariable result;
			this.TryGetVariable(name, origin, false, out result);
			return result;
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x001B06E6 File Offset: 0x001AE8E6
		internal PSVariable GetVariable(string name)
		{
			return this.GetVariable(name, this.ScopeOrigin);
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x001B06F5 File Offset: 0x001AE8F5
		internal bool TryGetVariable(string name, CommandOrigin origin, bool fromNewOrSet, out PSVariable variable)
		{
			if (this.TryGetLocalVariableFromTuple(name, fromNewOrSet, out variable))
			{
				SessionState.ThrowIfNotVisible(origin, variable);
				return true;
			}
			if (this.GetPrivateVariables().TryGetValue(name, out variable))
			{
				SessionState.ThrowIfNotVisible(origin, variable);
				return true;
			}
			return false;
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x001B072C File Offset: 0x001AE92C
		internal object GetAutomaticVariableValue(AutomaticVariable variable)
		{
			foreach (MutableTuple mutableTuple in this._dottedScopes)
			{
				if (mutableTuple.IsValueSet((int)variable))
				{
					return mutableTuple.GetValue((int)variable);
				}
			}
			if (this.LocalsTuple != null && this.LocalsTuple.IsValueSet((int)variable))
			{
				return this.LocalsTuple.GetValue((int)variable);
			}
			return AutomationNull.Value;
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x001B07B8 File Offset: 0x001AE9B8
		internal PSVariable SetVariable(string name, object value, bool asValue, bool force, SessionStateInternal sessionState, CommandOrigin origin = CommandOrigin.Internal, bool fastPath = false)
		{
			PSVariable psvariable = value as PSVariable;
			if (fastPath)
			{
				if (this.Parent != null)
				{
					throw new NotImplementedException("fastPath");
				}
				PSVariable psvariable2 = new PSVariable(name, psvariable.Value, psvariable.Options, psvariable.Attributes)
				{
					Description = psvariable.Description
				};
				this.GetPrivateVariables()[name] = psvariable2;
				return psvariable2;
			}
			else
			{
				PSVariable psvariable2;
				bool flag = this.TryGetVariable(name, origin, true, out psvariable2);
				if (this._variables == null)
				{
					this.GetPrivateVariables();
				}
				if (!asValue && psvariable != null)
				{
					if (flag)
					{
						if (psvariable2 == null || psvariable2.IsConstant || (!force && psvariable2.IsReadOnly))
						{
							SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Variable, "VariableNotWritable", SessionStateStrings.VariableNotWritable);
							throw ex;
						}
						if (psvariable2 is LocalVariable && (psvariable.Attributes.Any<Attribute>() || psvariable.Options != psvariable2.Options))
						{
							SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Variable, "VariableNotWritableRare", SessionStateStrings.VariableNotWritableRare);
							throw ex2;
						}
						if (psvariable2.IsReadOnly && force)
						{
							this._variables.Remove(name);
							flag = false;
							psvariable2 = new PSVariable(name, psvariable.Value, psvariable.Options, psvariable.Attributes)
							{
								Description = psvariable.Description
							};
							goto IL_1D1;
						}
						psvariable2.Attributes.Clear();
						psvariable2.Value = psvariable.Value;
						psvariable2.Options = psvariable.Options;
						psvariable2.Description = psvariable.Description;
						using (IEnumerator<Attribute> enumerator = psvariable.Attributes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Attribute item = enumerator.Current;
								psvariable2.Attributes.Add(item);
							}
							goto IL_1D1;
						}
					}
					psvariable2 = psvariable;
				}
				else if (psvariable2 != null)
				{
					psvariable2.Value = value;
				}
				else
				{
					psvariable2 = (((this.LocalsTuple != null) ? this.LocalsTuple.TrySetVariable(name, value) : null) ?? new PSVariable(name, value));
				}
				IL_1D1:
				if (!flag && this._variables.Count > this.VariableCapacity.FastValue - 1)
				{
					SessionStateOverflowException ex3 = new SessionStateOverflowException(name, SessionStateCategory.Variable, "VariableOverflow", SessionStateStrings.VariableOverflow, new object[]
					{
						this.VariableCapacity.FastValue
					});
					throw ex3;
				}
				if (ExecutionContext.HasEverUsedConstrainedLanguage)
				{
					ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
					if (executionContextFromTLS != null && executionContextFromTLS.LanguageMode == PSLanguageMode.ConstrainedLanguage && (psvariable2.Options & ScopedItemOptions.AllScope) == ScopedItemOptions.AllScope)
					{
						throw new PSNotSupportedException();
					}
				}
				this._variables[name] = psvariable2;
				psvariable2.SessionState = sessionState;
				return psvariable2;
			}
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x001B0A3C File Offset: 0x001AEC3C
		internal PSVariable NewVariable(PSVariable newVariable, bool force, SessionStateInternal sessionState)
		{
			PSVariable psvariable;
			bool flag = this.TryGetVariable(newVariable.Name, this.ScopeOrigin, true, out psvariable);
			if (flag)
			{
				if (psvariable == null || psvariable.IsConstant || (!force && psvariable.IsReadOnly))
				{
					SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(newVariable.Name, SessionStateCategory.Variable, "VariableNotWritable", SessionStateStrings.VariableNotWritable);
					throw ex;
				}
				if (psvariable is LocalVariable)
				{
					SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(newVariable.Name, SessionStateCategory.Variable, "VariableNotWritableRare", SessionStateStrings.VariableNotWritableRare);
					throw ex2;
				}
				if (!object.ReferenceEquals(newVariable, psvariable))
				{
					psvariable.WasRemoved = true;
					psvariable = newVariable;
				}
			}
			else
			{
				psvariable = newVariable;
			}
			if (!flag && this._variables.Count > this.VariableCapacity.FastValue - 1)
			{
				SessionStateOverflowException ex3 = new SessionStateOverflowException(newVariable.Name, SessionStateCategory.Variable, "VariableOverflow", SessionStateStrings.VariableOverflow, new object[]
				{
					this.VariableCapacity.FastValue
				});
				throw ex3;
			}
			if (ExecutionContext.HasEverUsedConstrainedLanguage)
			{
				ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
				if (executionContextFromTLS != null && executionContextFromTLS.LanguageMode == PSLanguageMode.ConstrainedLanguage && (psvariable.Options & ScopedItemOptions.AllScope) == ScopedItemOptions.AllScope)
				{
					throw new PSNotSupportedException();
				}
			}
			this._variables[psvariable.Name] = psvariable;
			psvariable.SessionState = sessionState;
			return psvariable;
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x001B0B68 File Offset: 0x001AED68
		internal void RemoveVariable(string name, bool force)
		{
			PSVariable variable = this.GetVariable(name);
			if (variable.IsConstant || (variable.IsReadOnly && !force))
			{
				SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Variable, "VariableNotRemovable", SessionStateStrings.VariableNotRemovable);
				throw ex;
			}
			if (variable is SessionStateCapacityVariable)
			{
				SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Variable, "VariableNotRemovableSystem", SessionStateStrings.VariableNotRemovableSystem);
				throw ex2;
			}
			if (variable is LocalVariable)
			{
				SessionStateUnauthorizedAccessException ex3 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Variable, "VariableNotRemovableRare", SessionStateStrings.VariableNotRemovableRare);
				throw ex3;
			}
			this._variables.Remove(name);
			variable.WasRemoved = true;
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x001B0BF0 File Offset: 0x001AEDF0
		internal bool TrySetLocalParameterValue(string name, object value)
		{
			foreach (MutableTuple mutableTuple in this._dottedScopes)
			{
				if (mutableTuple.TrySetParameter(name, value))
				{
					return true;
				}
			}
			return this.LocalsTuple != null && this.LocalsTuple.TrySetParameter(name, value);
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x001B0C64 File Offset: 0x001AEE64
		internal bool TryGetLocalVariableFromTuple(string name, bool fromNewOrSet, out PSVariable result)
		{
			foreach (MutableTuple mutableTuple in this._dottedScopes)
			{
				if (mutableTuple.TryGetLocalVariable(name, fromNewOrSet, out result))
				{
					return true;
				}
			}
			result = null;
			return this.LocalsTuple != null && this.LocalsTuple.TryGetLocalVariable(name, fromNewOrSet, out result);
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06005130 RID: 20784 RVA: 0x001B0CDC File Offset: 0x001AEEDC
		internal IEnumerable<AliasInfo> AliasTable
		{
			get
			{
				return this.GetAliases().Values;
			}
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x001B0CEC File Offset: 0x001AEEEC
		internal AliasInfo GetAlias(string name)
		{
			AliasInfo result;
			this.GetAliases().TryGetValue(name, out result);
			return result;
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x001B0D0C File Offset: 0x001AEF0C
		internal AliasInfo SetAliasValue(string name, string value, ExecutionContext context, bool force, CommandOrigin origin)
		{
			if (!this.GetAliases().ContainsKey(name))
			{
				if (this.GetAliases().Count > this.AliasCapacity.FastValue - 1)
				{
					SessionStateOverflowException ex = new SessionStateOverflowException(name, SessionStateCategory.Alias, "AliasOverflow", SessionStateStrings.AliasOverflow, new object[]
					{
						this.AliasCapacity.FastValue
					});
					throw ex;
				}
				this.GetAliases()[name] = new AliasInfo(name, value, context);
			}
			else
			{
				AliasInfo aliasInfo = this.GetAliases()[name];
				if ((aliasInfo.Options & ScopedItemOptions.Constant) != ScopedItemOptions.None || (!force && (aliasInfo.Options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None))
				{
					SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Alias, "AliasNotWritable", SessionStateStrings.AliasNotWritable);
					throw ex2;
				}
				SessionState.ThrowIfNotVisible(origin, aliasInfo);
				this.RemoveAliasFromCache(aliasInfo.Name, aliasInfo.Definition);
				if (force)
				{
					this.GetAliases().Remove(name);
					aliasInfo = new AliasInfo(name, value, context);
					this.GetAliases()[name] = aliasInfo;
				}
				else
				{
					aliasInfo.SetDefinition(value, false);
				}
			}
			this.AddAliasToCache(name, value);
			return this.GetAliases()[name];
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x001B0E24 File Offset: 0x001AF024
		internal AliasInfo SetAliasValue(string name, string value, ScopedItemOptions options, ExecutionContext context, bool force, CommandOrigin origin)
		{
			if (!this.GetAliases().ContainsKey(name))
			{
				if (this.GetAliases().Count > this.AliasCapacity.FastValue - 1)
				{
					SessionStateOverflowException ex = new SessionStateOverflowException(name, SessionStateCategory.Alias, "AliasOverflow", SessionStateStrings.AliasOverflow, new object[]
					{
						this.AliasCapacity.FastValue
					});
					throw ex;
				}
				AliasInfo value2 = new AliasInfo(name, value, context, options);
				this.GetAliases()[name] = value2;
			}
			else
			{
				AliasInfo aliasInfo = this.GetAliases()[name];
				if ((aliasInfo.Options & ScopedItemOptions.Constant) != ScopedItemOptions.None || (!force && (aliasInfo.Options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None))
				{
					SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Alias, "AliasNotWritable", SessionStateStrings.AliasNotWritable);
					throw ex2;
				}
				if ((options & ScopedItemOptions.Constant) != ScopedItemOptions.None)
				{
					SessionStateUnauthorizedAccessException ex3 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Alias, "AliasCannotBeMadeConstant", SessionStateStrings.AliasCannotBeMadeConstant);
					throw ex3;
				}
				if ((options & ScopedItemOptions.AllScope) == ScopedItemOptions.None && (aliasInfo.Options & ScopedItemOptions.AllScope) != ScopedItemOptions.None)
				{
					SessionStateUnauthorizedAccessException ex4 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Alias, "AliasAllScopeOptionCannotBeRemoved", SessionStateStrings.AliasAllScopeOptionCannotBeRemoved);
					throw ex4;
				}
				SessionState.ThrowIfNotVisible(origin, aliasInfo);
				this.RemoveAliasFromCache(aliasInfo.Name, aliasInfo.Definition);
				if (force)
				{
					this.GetAliases().Remove(name);
					aliasInfo = new AliasInfo(name, value, context, options);
					this.GetAliases()[name] = aliasInfo;
				}
				else
				{
					aliasInfo.Options = options;
					aliasInfo.SetDefinition(value, false);
				}
			}
			this.AddAliasToCache(name, value);
			return this.GetAliases()[name];
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x001B0F8C File Offset: 0x001AF18C
		internal AliasInfo SetAliasItem(AliasInfo aliasToSet, bool force, CommandOrigin origin = CommandOrigin.Internal)
		{
			if (!this.GetAliases().ContainsKey(aliasToSet.Name))
			{
				if (this.GetAliases().Count > this.AliasCapacity.FastValue - 1)
				{
					SessionStateOverflowException ex = new SessionStateOverflowException(aliasToSet.Name, SessionStateCategory.Alias, "AliasOverflow", SessionStateStrings.AliasOverflow, new object[]
					{
						this.AliasCapacity.FastValue
					});
					throw ex;
				}
				this.GetAliases()[aliasToSet.Name] = aliasToSet;
			}
			else
			{
				AliasInfo aliasInfo = this.GetAliases()[aliasToSet.Name];
				SessionState.ThrowIfNotVisible(origin, aliasInfo);
				if ((aliasInfo.Options & ScopedItemOptions.Constant) != ScopedItemOptions.None || ((aliasInfo.Options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None && !force))
				{
					SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(aliasToSet.Name, SessionStateCategory.Alias, "AliasNotWritable", SessionStateStrings.AliasNotWritable);
					throw ex2;
				}
				if ((aliasToSet.Options & ScopedItemOptions.AllScope) == ScopedItemOptions.None && (aliasInfo.Options & ScopedItemOptions.AllScope) != ScopedItemOptions.None)
				{
					SessionStateUnauthorizedAccessException ex3 = new SessionStateUnauthorizedAccessException(aliasToSet.Name, SessionStateCategory.Alias, "AliasAllScopeOptionCannotBeRemoved", SessionStateStrings.AliasAllScopeOptionCannotBeRemoved);
					throw ex3;
				}
				this.RemoveAliasFromCache(aliasInfo.Name, aliasInfo.Definition);
				this.GetAliases()[aliasToSet.Name] = aliasToSet;
			}
			this.AddAliasToCache(aliasToSet.Name, aliasToSet.Definition);
			return this.GetAliases()[aliasToSet.Name];
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001B10D4 File Offset: 0x001AF2D4
		internal void RemoveAlias(string name, bool force)
		{
			if (this.GetAliases().ContainsKey(name))
			{
				AliasInfo aliasInfo = this.GetAliases()[name];
				if ((aliasInfo.Options & ScopedItemOptions.Constant) != ScopedItemOptions.None || (!force && (aliasInfo.Options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None))
				{
					SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Alias, "AliasNotRemovable", SessionStateStrings.AliasNotRemovable);
					throw ex;
				}
				this.RemoveAliasFromCache(aliasInfo.Name, aliasInfo.Definition);
			}
			this.GetAliases().Remove(name);
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x06005136 RID: 20790 RVA: 0x001B1146 File Offset: 0x001AF346
		internal Dictionary<string, FunctionInfo> FunctionTable
		{
			get
			{
				return this.GetFunctions();
			}
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x001B1150 File Offset: 0x001AF350
		internal FunctionInfo GetFunction(string name)
		{
			FunctionInfo result;
			this.GetFunctions().TryGetValue(name, out result);
			return result;
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x001B116D File Offset: 0x001AF36D
		internal FunctionInfo SetFunction(string name, ScriptBlock function, bool force, CommandOrigin origin, ExecutionContext context)
		{
			return this.SetFunction(name, function, null, ScopedItemOptions.Unspecified, force, origin, context);
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x001B117F File Offset: 0x001AF37F
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, bool force, CommandOrigin origin, ExecutionContext context)
		{
			return this.SetFunction(name, function, originalFunction, ScopedItemOptions.Unspecified, force, origin, context);
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x001B1194 File Offset: 0x001AF394
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, ScopedItemOptions options, bool force, CommandOrigin origin, ExecutionContext context)
		{
			return this.SetFunction(name, function, originalFunction, options, force, origin, context, null);
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x001B11B4 File Offset: 0x001AF3B4
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, ScopedItemOptions options, bool force, CommandOrigin origin, ExecutionContext context, string helpFile)
		{
			return this.SetFunction(name, function, originalFunction, options, force, origin, context, helpFile, new Func<string, ScriptBlock, FunctionInfo, ScopedItemOptions, ExecutionContext, string, FunctionInfo>(SessionStateScope.CreateFunction));
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x001B11E0 File Offset: 0x001AF3E0
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, ScopedItemOptions options, bool force, CommandOrigin origin, ExecutionContext context, string helpFile, Func<string, ScriptBlock, FunctionInfo, ScopedItemOptions, ExecutionContext, string, FunctionInfo> functionFactory)
		{
			if (!this.GetFunctions().ContainsKey(name))
			{
				if (this.GetFunctions().Count > this.FunctionCapacity.FastValue - 1)
				{
					SessionStateOverflowException ex = new SessionStateOverflowException(name, SessionStateCategory.Function, "FunctionOverflow", SessionStateStrings.FunctionOverflow, new object[]
					{
						this.FunctionCapacity.FastValue
					});
					throw ex;
				}
				FunctionInfo functionInfo = functionFactory(name, function, originalFunction, options, context, helpFile);
				this.GetFunctions()[name] = functionInfo;
				if (SessionStateScope.IsFunctionOptionSet(functionInfo, ScopedItemOptions.AllScope))
				{
					this.GetAllScopeFunctions()[name] = functionInfo;
				}
			}
			else
			{
				FunctionInfo functionInfo2 = this.GetFunctions()[name];
				SessionState.ThrowIfNotVisible(origin, functionInfo2);
				if (SessionStateScope.IsFunctionOptionSet(functionInfo2, ScopedItemOptions.Constant) || (!force && SessionStateScope.IsFunctionOptionSet(functionInfo2, ScopedItemOptions.ReadOnly)))
				{
					SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Function, "FunctionNotWritable", SessionStateStrings.FunctionNotWritable);
					throw ex2;
				}
				if ((options & ScopedItemOptions.Constant) != ScopedItemOptions.None)
				{
					SessionStateUnauthorizedAccessException ex3 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Function, "FunctionCannotBeMadeConstant", SessionStateStrings.FunctionCannotBeMadeConstant);
					throw ex3;
				}
				if ((options & ScopedItemOptions.AllScope) == ScopedItemOptions.None && SessionStateScope.IsFunctionOptionSet(functionInfo2, ScopedItemOptions.AllScope))
				{
					SessionStateUnauthorizedAccessException ex4 = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Function, "FunctionAllScopeOptionCannotBeRemoved", SessionStateStrings.FunctionAllScopeOptionCannotBeRemoved);
					throw ex4;
				}
				FunctionInfo functionInfo3 = functionInfo2;
				if (functionInfo3 != null)
				{
					FunctionInfo functionInfo4 = functionFactory(name, function, originalFunction, options, context, helpFile);
					if (!functionInfo3.GetType().Equals(functionInfo4.GetType()) || ((functionInfo3.Options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None && force))
					{
						this.GetFunctions()[name] = functionInfo4;
					}
					else
					{
						bool force2 = force || (options & ScopedItemOptions.ReadOnly) == ScopedItemOptions.None;
						functionInfo3.Update(functionInfo4, force2, options, helpFile);
					}
				}
			}
			return this.GetFunctions()[name];
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x001B1388 File Offset: 0x001AF588
		internal void RemoveFunction(string name, bool force)
		{
			if (this.GetFunctions().ContainsKey(name))
			{
				FunctionInfo function = this.GetFunctions()[name];
				if (SessionStateScope.IsFunctionOptionSet(function, ScopedItemOptions.Constant) || (!force && SessionStateScope.IsFunctionOptionSet(function, ScopedItemOptions.ReadOnly)))
				{
					SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(name, SessionStateCategory.Function, "FunctionNotRemovable", SessionStateStrings.FunctionNotRemovable);
					throw ex;
				}
				if (SessionStateScope.IsFunctionOptionSet(function, ScopedItemOptions.AllScope))
				{
					this.GetAllScopeFunctions().Remove(name);
				}
			}
			this.GetFunctions().Remove(name);
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x001B13FC File Offset: 0x001AF5FC
		internal Dictionary<string, List<CmdletInfo>> CmdletTable
		{
			get
			{
				return this.GetCmdlets();
			}
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x001B1404 File Offset: 0x001AF604
		internal CmdletInfo GetCmdlet(string name)
		{
			CmdletInfo result = null;
			List<CmdletInfo> list;
			if (this.GetCmdlets().TryGetValue(name, out list) && list != null && list.Count > 0)
			{
				result = list[0];
			}
			return result;
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x001B1438 File Offset: 0x001AF638
		internal CmdletInfo AddCmdletToCache(string name, CmdletInfo cmdlet, CommandOrigin origin, ExecutionContext context)
		{
			bool flag = false;
			try
			{
				List<CmdletInfo> list;
				if (!this.GetCmdlets().TryGetValue(name, out list))
				{
					list = new List<CmdletInfo>();
					list.Add(cmdlet);
					this.GetCmdlets().Add(name, list);
					if ((cmdlet.Options & ScopedItemOptions.AllScope) != ScopedItemOptions.None)
					{
						this.GetAllScopeCmdlets()[name].Insert(0, cmdlet);
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(cmdlet.ModuleName))
					{
						using (List<CmdletInfo>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								CmdletInfo cmdletInfo = enumerator.Current;
								if (string.Equals(cmdlet.FullName, cmdletInfo.FullName, StringComparison.OrdinalIgnoreCase))
								{
									if (cmdlet.ImplementingType == cmdletInfo.ImplementingType)
									{
										return null;
									}
									flag = true;
									break;
								}
							}
							goto IL_103;
						}
					}
					using (List<CmdletInfo>.Enumerator enumerator2 = list.GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							CmdletInfo cmdletInfo2 = enumerator2.Current;
							if (cmdlet.ImplementingType == cmdletInfo2.ImplementingType)
							{
								return null;
							}
							flag = true;
						}
					}
					IL_103:
					if (!flag)
					{
						list.Insert(0, cmdlet);
					}
				}
			}
			catch (ArgumentException)
			{
				flag = true;
			}
			if (flag)
			{
				PSNotSupportedException ex = PSTraceSource.NewNotSupportedException(DiscoveryExceptions.DuplicateCmdletName, new object[]
				{
					cmdlet.Name
				});
				throw ex;
			}
			return this.GetCmdlets()[name][0];
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x001B15E4 File Offset: 0x001AF7E4
		internal void RemoveCmdlet(string name, int index, bool force)
		{
			List<CmdletInfo> list;
			if (this.GetCmdlets().TryGetValue(name, out list))
			{
				CmdletInfo cmdletInfo = list[index];
				if ((cmdletInfo.Options & ScopedItemOptions.AllScope) != ScopedItemOptions.None)
				{
					this.GetAllScopeCmdlets()[name].RemoveAt(index);
				}
				list.RemoveAt(index);
				if (list.Count == 0)
				{
					this.GetCmdlets().Remove(name);
				}
			}
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x001B1641 File Offset: 0x001AF841
		internal void RemoveCmdletEntry(string name, bool force)
		{
			if (this.GetCmdlets().ContainsKey(name))
			{
				this.GetCmdlets().Remove(name);
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06005143 RID: 20803 RVA: 0x001B165E File Offset: 0x001AF85E
		// (set) Token: 0x06005144 RID: 20804 RVA: 0x001B1666 File Offset: 0x001AF866
		internal TypeResolutionState TypeResolutionState { get; set; }

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x06005145 RID: 20805 RVA: 0x001B166F File Offset: 0x001AF86F
		// (set) Token: 0x06005146 RID: 20806 RVA: 0x001B1677 File Offset: 0x001AF877
		internal IDictionary<string, Type> TypeTable { get; private set; }

		// Token: 0x06005147 RID: 20807 RVA: 0x001B1680 File Offset: 0x001AF880
		internal void AddType(string name, Type type)
		{
			if (this.TypeTable == null)
			{
				this.TypeTable = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
			}
			this.TypeTable[name] = type;
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x001B16A8 File Offset: 0x001AF8A8
		internal Type LookupType(string name)
		{
			if (this.TypeTable == null)
			{
				return null;
			}
			Type result;
			this.TypeTable.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x001B16CF File Offset: 0x001AF8CF
		private static bool IsFunctionOptionSet(FunctionInfo function, ScopedItemOptions options)
		{
			return (function.Options & options) != ScopedItemOptions.None;
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x001B16E0 File Offset: 0x001AF8E0
		private static FunctionInfo CreateFunction(string name, ScriptBlock function, FunctionInfo originalFunction, ScopedItemOptions options, ExecutionContext context, string helpFile)
		{
			if (options == ScopedItemOptions.Unspecified)
			{
				options = ScopedItemOptions.None;
			}
			FunctionInfo result;
			if (originalFunction is FilterInfo)
			{
				result = new FilterInfo(name, (FilterInfo)originalFunction);
			}
			else if (originalFunction is WorkflowInfo)
			{
				result = new WorkflowInfo(name, (WorkflowInfo)originalFunction);
			}
			else if (originalFunction is ConfigurationInfo)
			{
				result = new ConfigurationInfo(name, (ConfigurationInfo)originalFunction);
			}
			else if (originalFunction != null)
			{
				result = new FunctionInfo(name, originalFunction);
			}
			else if (function.IsFilter)
			{
				result = new FilterInfo(name, function, options, context, helpFile);
			}
			else if (function.IsConfiguration)
			{
				result = new ConfigurationInfo(name, function, options, context, helpFile, function.IsMetaConfiguration());
			}
			else
			{
				result = new FunctionInfo(name, function, options, context, helpFile);
			}
			return result;
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x001B178B File Offset: 0x001AF98B
		private Dictionary<string, PSDriveInfo> GetDrives()
		{
			if (this._drives == null)
			{
				this._drives = new Dictionary<string, PSDriveInfo>(StringComparer.OrdinalIgnoreCase);
			}
			return this._drives;
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x001B17AB File Offset: 0x001AF9AB
		private Dictionary<string, PSDriveInfo> GetAutomountedDrives()
		{
			if (this._automountedDrives == null)
			{
				this._automountedDrives = new Dictionary<string, PSDriveInfo>(StringComparer.OrdinalIgnoreCase);
			}
			return this._automountedDrives;
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x001B17CB File Offset: 0x001AF9CB
		private Dictionary<string, PSVariable> GetPrivateVariables()
		{
			if (this._variables == null)
			{
				this._variables = new Dictionary<string, PSVariable>(StringComparer.OrdinalIgnoreCase);
				this.AddSessionStateScopeDefaultVariables();
			}
			return this._variables;
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x001B17F4 File Offset: 0x001AF9F4
		internal void AddSessionStateScopeDefaultVariables()
		{
			if (this.Parent == null)
			{
				this._variables.Add(SessionStateScope._nullVar.Name, SessionStateScope._nullVar);
				this._variables.Add(SessionStateScope._falseVar.Name, SessionStateScope._falseVar);
				this._variables.Add(SessionStateScope._trueVar.Name, SessionStateScope._trueVar);
			}
			else
			{
				foreach (PSVariable psvariable in this.Parent.GetPrivateVariables().Values)
				{
					if (psvariable.IsAllScope)
					{
						this._variables.Add(psvariable.Name, psvariable);
					}
				}
			}
			string text = "MaximumErrorCount";
			this._errorCapacity = this.CreateCapacityVariable(text, 256, 32768, 256, SessionStateStrings.MaxErrorCountDescription);
			this._variables.Add(text, this._errorCapacity);
			text = "MaximumVariableCount";
			this._variableCapacity = this.CreateCapacityVariable(text, 4096, 32768, 1024, SessionStateStrings.MaxVariableCountDescription);
			this._variables.Add(text, this._variableCapacity);
			text = "MaximumFunctionCount";
			this._functionCapacity = this.CreateCapacityVariable(text, 4096, 32768, 1024, SessionStateStrings.MaxFunctionCountDescription);
			this._variables.Add(text, this._functionCapacity);
			text = "MaximumAliasCount";
			this._aliasCapacity = this.CreateCapacityVariable(text, 4096, 32768, 1024, SessionStateStrings.MaxAliasCountDescription);
			this._variables.Add(text, this._aliasCapacity);
			text = "MaximumDriveCount";
			this._driveCapacity = this.CreateCapacityVariable(text, 4096, 32768, 1024, SessionStateStrings.MaxDriveCountDescription);
			this._variables.Add(text, this._driveCapacity);
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001B19DC File Offset: 0x001AFBDC
		private Dictionary<string, AliasInfo> GetAliases()
		{
			if (this._alias == null)
			{
				this._alias = new Dictionary<string, AliasInfo>(StringComparer.OrdinalIgnoreCase);
				if (this.Parent != null)
				{
					foreach (AliasInfo aliasInfo in this.Parent.GetAliases().Values)
					{
						if ((aliasInfo.Options & ScopedItemOptions.AllScope) != ScopedItemOptions.None)
						{
							this._alias.Add(aliasInfo.Name, aliasInfo);
						}
					}
				}
			}
			return this._alias;
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x001B1A74 File Offset: 0x001AFC74
		private Dictionary<string, FunctionInfo> GetFunctions()
		{
			if (this._functions == null)
			{
				this._functions = new Dictionary<string, FunctionInfo>(StringComparer.OrdinalIgnoreCase);
				if (this.Parent != null && this.Parent._allScopeFunctions != null)
				{
					foreach (FunctionInfo functionInfo in this.Parent._allScopeFunctions.Values)
					{
						this._functions.Add(functionInfo.Name, functionInfo);
					}
				}
			}
			return this._functions;
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x001B1B10 File Offset: 0x001AFD10
		private Dictionary<string, FunctionInfo> GetAllScopeFunctions()
		{
			if (this._allScopeFunctions == null)
			{
				if (this.Parent != null && this.Parent._allScopeFunctions != null)
				{
					return this.Parent._allScopeFunctions;
				}
				this._allScopeFunctions = new Dictionary<string, FunctionInfo>(StringComparer.OrdinalIgnoreCase);
			}
			return this._allScopeFunctions;
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x001B1B5C File Offset: 0x001AFD5C
		private Dictionary<string, List<CmdletInfo>> GetCmdlets()
		{
			lock (this.CmdletCache)
			{
				if (this.CmdletCache == null)
				{
					this._cmdlets = new Dictionary<string, List<CmdletInfo>>(StringComparer.OrdinalIgnoreCase);
					if (this.Parent != null && this.Parent.AllScopeCmdletCache != null)
					{
						foreach (KeyValuePair<string, List<CmdletInfo>> keyValuePair in this.Parent.AllScopeCmdletCache)
						{
							this._cmdlets.Add(keyValuePair.Key, keyValuePair.Value);
						}
					}
				}
			}
			return this.CmdletCache;
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x001B1C24 File Offset: 0x001AFE24
		internal Dictionary<string, List<CmdletInfo>> CmdletCache
		{
			get
			{
				return this._cmdlets;
			}
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x001B1C2C File Offset: 0x001AFE2C
		private Dictionary<string, List<CmdletInfo>> GetAllScopeCmdlets()
		{
			lock (this.AllScopeCmdletCache)
			{
				if (this.AllScopeCmdletCache == null)
				{
					if (this.Parent != null && this.Parent.AllScopeCmdletCache != null)
					{
						return this.Parent.AllScopeCmdletCache;
					}
					this._allScopeCmdlets = new Dictionary<string, List<CmdletInfo>>(StringComparer.OrdinalIgnoreCase);
				}
			}
			return this.AllScopeCmdletCache;
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x06005155 RID: 20821 RVA: 0x001B1CAC File Offset: 0x001AFEAC
		internal Dictionary<string, List<CmdletInfo>> AllScopeCmdletCache
		{
			get
			{
				return this._allScopeCmdlets;
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x06005156 RID: 20822 RVA: 0x001B1CB4 File Offset: 0x001AFEB4
		internal SessionStateCapacityVariable ErrorCapacity
		{
			get
			{
				this.GetPrivateVariables();
				return this._errorCapacity;
			}
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x06005157 RID: 20823 RVA: 0x001B1CC3 File Offset: 0x001AFEC3
		internal SessionStateCapacityVariable VariableCapacity
		{
			get
			{
				this.GetPrivateVariables();
				return this._variableCapacity;
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06005158 RID: 20824 RVA: 0x001B1CD2 File Offset: 0x001AFED2
		private SessionStateCapacityVariable FunctionCapacity
		{
			get
			{
				this.GetPrivateVariables();
				return this._functionCapacity;
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06005159 RID: 20825 RVA: 0x001B1CE1 File Offset: 0x001AFEE1
		private SessionStateCapacityVariable AliasCapacity
		{
			get
			{
				this.GetPrivateVariables();
				return this._aliasCapacity;
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x0600515A RID: 20826 RVA: 0x001B1CF0 File Offset: 0x001AFEF0
		private SessionStateCapacityVariable DriveCapacity
		{
			get
			{
				this.GetPrivateVariables();
				return this._driveCapacity;
			}
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x001B1EAC File Offset: 0x001B00AC
		internal IEnumerable<string> GetAliasesByCommandName(string command)
		{
			List<string> commandsToAliases;
			if (this.commandsToAliasesCache.TryGetValue(command, out commandsToAliases))
			{
				foreach (string str in commandsToAliases)
				{
					yield return str;
				}
			}
			yield break;
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x001B1ED0 File Offset: 0x001B00D0
		private void AddAliasToCache(string alias, string value)
		{
			List<string> list;
			if (!this.commandsToAliasesCache.TryGetValue(value, out list))
			{
				List<string> list2 = new List<string>();
				list2.Add(alias);
				this.commandsToAliasesCache.Add(value, list2);
				return;
			}
			if (!list.Contains(alias, StringComparer.OrdinalIgnoreCase))
			{
				list.Add(alias);
			}
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x001B1F34 File Offset: 0x001B0134
		private void RemoveAliasFromCache(string alias, string value)
		{
			List<string> list;
			if (!this.commandsToAliasesCache.TryGetValue(value, out list))
			{
				return;
			}
			if (list.Count <= 1)
			{
				this.commandsToAliasesCache.Remove(value);
				return;
			}
			string text = list.FirstOrDefault((string item) => item.Equals(alias, StringComparison.OrdinalIgnoreCase));
			if (text != null)
			{
				list.Remove(text);
			}
		}

		// Token: 0x04002972 RID: 10610
		private SessionStateScope _scriptScope;

		// Token: 0x04002973 RID: 10611
		private readonly Stack<MutableTuple> _dottedScopes = new Stack<MutableTuple>();

		// Token: 0x04002974 RID: 10612
		private Dictionary<string, PSDriveInfo> _drives;

		// Token: 0x04002975 RID: 10613
		private Dictionary<string, PSDriveInfo> _automountedDrives;

		// Token: 0x04002976 RID: 10614
		private Dictionary<string, PSVariable> _variables;

		// Token: 0x04002977 RID: 10615
		private Dictionary<string, AliasInfo> _alias;

		// Token: 0x04002978 RID: 10616
		private Dictionary<string, FunctionInfo> _functions;

		// Token: 0x04002979 RID: 10617
		private Dictionary<string, FunctionInfo> _allScopeFunctions;

		// Token: 0x0400297A RID: 10618
		private Dictionary<string, List<CmdletInfo>> _cmdlets = new Dictionary<string, List<CmdletInfo>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400297B RID: 10619
		private Dictionary<string, List<CmdletInfo>> _allScopeCmdlets = new Dictionary<string, List<CmdletInfo>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400297C RID: 10620
		private SessionStateCapacityVariable _errorCapacity;

		// Token: 0x0400297D RID: 10621
		private SessionStateCapacityVariable _variableCapacity;

		// Token: 0x0400297E RID: 10622
		private SessionStateCapacityVariable _functionCapacity;

		// Token: 0x0400297F RID: 10623
		private SessionStateCapacityVariable _aliasCapacity;

		// Token: 0x04002980 RID: 10624
		private SessionStateCapacityVariable _driveCapacity;

		// Token: 0x04002981 RID: 10625
		private static readonly PSVariable _trueVar = new PSVariable("true", true, ScopedItemOptions.Constant | ScopedItemOptions.AllScope, "Boolean True");

		// Token: 0x04002982 RID: 10626
		private static readonly PSVariable _falseVar = new PSVariable("false", false, ScopedItemOptions.Constant | ScopedItemOptions.AllScope, "Boolean False");

		// Token: 0x04002983 RID: 10627
		private static readonly NullVariable _nullVar = new NullVariable();

		// Token: 0x04002984 RID: 10628
		private Dictionary<string, List<string>> commandsToAliasesCache = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
	}
}
