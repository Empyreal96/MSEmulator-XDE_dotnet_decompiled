using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;

namespace System.Management.Automation
{
	// Token: 0x02000837 RID: 2103
	public class PSVariable : IHasSessionStateEntryVisibility
	{
		// Token: 0x060050DD RID: 20701 RVA: 0x001AFB15 File Offset: 0x001ADD15
		public PSVariable(string name) : this(name, null, ScopedItemOptions.None, null)
		{
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x001AFB21 File Offset: 0x001ADD21
		public PSVariable(string name, object value) : this(name, value, ScopedItemOptions.None, null)
		{
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x001AFB2D File Offset: 0x001ADD2D
		public PSVariable(string name, object value, ScopedItemOptions options) : this(name, value, options, null)
		{
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x001AFB39 File Offset: 0x001ADD39
		internal PSVariable(string name, object value, ScopedItemOptions options, string description) : this(name, value, options, null)
		{
			this.description = description;
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x001AFB4D File Offset: 0x001ADD4D
		internal PSVariable(string name, object value, ScopedItemOptions options, Collection<Attribute> attributes, string description) : this(name, value, options, attributes)
		{
			this.description = description;
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x001AFB64 File Offset: 0x001ADD64
		public PSVariable(string name, object value, ScopedItemOptions options, Collection<Attribute> attributes)
		{
			this.name = string.Empty;
			this.description = string.Empty;
			base..ctor();
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			this.attributes = new PSVariableAttributeCollection(this);
			this.SetValueRawImpl(value, true);
			if (attributes != null)
			{
				foreach (Attribute item in attributes)
				{
					this.attributes.Add(item);
				}
			}
			this.options = options;
			if (this.IsAllScope)
			{
				VariableAnalysis.NoteAllScopeVariable(name);
			}
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x001AFC18 File Offset: 0x001ADE18
		internal PSVariable(string name, bool dummy)
		{
			this.name = string.Empty;
			this.description = string.Empty;
			base..ctor();
			this.name = name;
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x060050E4 RID: 20708 RVA: 0x001AFC3D File Offset: 0x001ADE3D
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x060050E5 RID: 20709 RVA: 0x001AFC45 File Offset: 0x001ADE45
		// (set) Token: 0x060050E6 RID: 20710 RVA: 0x001AFC4D File Offset: 0x001ADE4D
		public virtual string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x001AFC58 File Offset: 0x001ADE58
		internal void DebuggerCheckVariableRead()
		{
			ExecutionContext executionContext = (this._sessionState != null) ? this._sessionState.ExecutionContext : LocalPipeline.GetExecutionContextFromTLS();
			if (executionContext != null && executionContext._debuggingMode > 0)
			{
				executionContext.Debugger.CheckVariableRead(this.Name);
			}
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x001AFCA0 File Offset: 0x001ADEA0
		internal void DebuggerCheckVariableWrite()
		{
			ExecutionContext executionContext = (this._sessionState != null) ? this._sessionState.ExecutionContext : LocalPipeline.GetExecutionContextFromTLS();
			if (executionContext != null && executionContext._debuggingMode > 0)
			{
				executionContext.Debugger.CheckVariableWrite(this.Name);
			}
		}

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x060050E9 RID: 20713 RVA: 0x001AFCE5 File Offset: 0x001ADEE5
		// (set) Token: 0x060050EA RID: 20714 RVA: 0x001AFCF3 File Offset: 0x001ADEF3
		public virtual object Value
		{
			get
			{
				this.DebuggerCheckVariableRead();
				return this._value;
			}
			set
			{
				this.SetValue(value);
			}
		}

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x060050EB RID: 20715 RVA: 0x001AFCFC File Offset: 0x001ADEFC
		// (set) Token: 0x060050EC RID: 20716 RVA: 0x001AFD04 File Offset: 0x001ADF04
		public SessionStateEntryVisibility Visibility
		{
			get
			{
				return this._visibility;
			}
			set
			{
				this._visibility = value;
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x060050ED RID: 20717 RVA: 0x001AFD0D File Offset: 0x001ADF0D
		public PSModuleInfo Module
		{
			get
			{
				return this._module;
			}
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x001AFD15 File Offset: 0x001ADF15
		internal void SetModule(PSModuleInfo module)
		{
			this._module = module;
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x060050EF RID: 20719 RVA: 0x001AFD1E File Offset: 0x001ADF1E
		public string ModuleName
		{
			get
			{
				if (this._module != null)
				{
					return this._module.Name;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x060050F0 RID: 20720 RVA: 0x001AFD39 File Offset: 0x001ADF39
		// (set) Token: 0x060050F1 RID: 20721 RVA: 0x001AFD41 File Offset: 0x001ADF41
		public virtual ScopedItemOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.SetOptions(value, false);
			}
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x001AFD4C File Offset: 0x001ADF4C
		internal void SetOptions(ScopedItemOptions newOptions, bool force)
		{
			if (this.IsConstant || (!force && this.IsReadOnly))
			{
				SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(this.name, SessionStateCategory.Variable, "VariableNotWritable", SessionStateStrings.VariableNotWritable);
				throw ex;
			}
			if ((newOptions & ScopedItemOptions.Constant) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(this.name, SessionStateCategory.Variable, "VariableCannotBeMadeConstant", SessionStateStrings.VariableCannotBeMadeConstant);
				throw ex2;
			}
			if (this.IsAllScope && (newOptions & ScopedItemOptions.AllScope) == ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex3 = new SessionStateUnauthorizedAccessException(this.name, SessionStateCategory.Variable, "VariableAllScopeOptionCannotBeRemoved", SessionStateStrings.VariableAllScopeOptionCannotBeRemoved);
				throw ex3;
			}
			this.options = newOptions;
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x060050F3 RID: 20723 RVA: 0x001AFDD0 File Offset: 0x001ADFD0
		public Collection<Attribute> Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new PSVariableAttributeCollection(this);
				}
				return this.attributes;
			}
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x001AFDEC File Offset: 0x001ADFEC
		public virtual bool IsValidValue(object value)
		{
			return PSVariable.IsValidValue(this.attributes, value);
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x001AFDFC File Offset: 0x001ADFFC
		internal static bool IsValidValue(IEnumerable<Attribute> attributes, object value)
		{
			if (attributes != null)
			{
				foreach (Attribute attribute in attributes)
				{
					if (!PSVariable.IsValidValue(value, attribute))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x001AFE50 File Offset: 0x001AE050
		internal static bool IsValidValue(object value, Attribute attribute)
		{
			bool result = true;
			ValidateArgumentsAttribute validateArgumentsAttribute = attribute as ValidateArgumentsAttribute;
			if (validateArgumentsAttribute != null)
			{
				try
				{
					ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
					EngineIntrinsics engineIntrinsics = null;
					if (executionContextFromTLS != null)
					{
						engineIntrinsics = executionContextFromTLS.EngineIntrinsics;
					}
					validateArgumentsAttribute.InternalValidate(value, engineIntrinsics);
				}
				catch (ValidationMetadataException)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x001AFE9C File Offset: 0x001AE09C
		internal static object TransformValue(IEnumerable<Attribute> attributes, object value)
		{
			object obj = value;
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			EngineIntrinsics engineIntrinsics = null;
			if (executionContextFromTLS != null)
			{
				engineIntrinsics = executionContextFromTLS.EngineIntrinsics;
			}
			foreach (Attribute attribute in attributes)
			{
				ArgumentTransformationAttribute argumentTransformationAttribute = attribute as ArgumentTransformationAttribute;
				if (argumentTransformationAttribute != null)
				{
					obj = argumentTransformationAttribute.Transform(engineIntrinsics, obj);
				}
			}
			return obj;
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x001AFF10 File Offset: 0x001AE110
		internal void AddParameterAttributesNoChecks(Collection<Attribute> attributes)
		{
			foreach (Attribute item in attributes)
			{
				this.attributes.AddAttributeNoCheck(item);
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x060050F9 RID: 20729 RVA: 0x001AFF60 File Offset: 0x001AE160
		internal bool IsConstant
		{
			get
			{
				return (this.options & ScopedItemOptions.Constant) != ScopedItemOptions.None;
			}
		}

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x060050FA RID: 20730 RVA: 0x001AFF70 File Offset: 0x001AE170
		internal bool IsReadOnly
		{
			get
			{
				return (this.options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None;
			}
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x060050FB RID: 20731 RVA: 0x001AFF80 File Offset: 0x001AE180
		internal bool IsPrivate
		{
			get
			{
				return (this.options & ScopedItemOptions.Private) != ScopedItemOptions.None;
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x060050FC RID: 20732 RVA: 0x001AFF90 File Offset: 0x001AE190
		internal bool IsAllScope
		{
			get
			{
				return (this.options & ScopedItemOptions.AllScope) != ScopedItemOptions.None;
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x060050FD RID: 20733 RVA: 0x001AFFA0 File Offset: 0x001AE1A0
		// (set) Token: 0x060050FE RID: 20734 RVA: 0x001AFFA8 File Offset: 0x001AE1A8
		internal bool WasRemoved
		{
			get
			{
				return this._wasRemoved;
			}
			set
			{
				this._wasRemoved = value;
				if (value)
				{
					this.options = ScopedItemOptions.None;
					this._value = null;
					this._wasRemoved = true;
					this.attributes = null;
				}
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x060050FF RID: 20735 RVA: 0x001AFFD0 File Offset: 0x001AE1D0
		// (set) Token: 0x06005100 RID: 20736 RVA: 0x001AFFD8 File Offset: 0x001AE1D8
		internal SessionStateInternal SessionState
		{
			get
			{
				return this._sessionState;
			}
			set
			{
				this._sessionState = value;
			}
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x001AFFE4 File Offset: 0x001AE1E4
		private void SetValue(object value)
		{
			if ((this.options & (ScopedItemOptions.ReadOnly | ScopedItemOptions.Constant)) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(this.name, SessionStateCategory.Variable, "VariableNotWritable", SessionStateStrings.VariableNotWritable);
				throw ex;
			}
			object obj = value;
			if (this.attributes != null && this.attributes.Count > 0)
			{
				obj = PSVariable.TransformValue(this.attributes, value);
				if (!this.IsValidValue(obj))
				{
					ValidationMetadataException ex2 = new ValidationMetadataException("ValidateSetFailure", null, Metadata.InvalidValueFailure, new object[]
					{
						this.name,
						(obj != null) ? obj.ToString() : "$null"
					});
					throw ex2;
				}
			}
			if (obj != null)
			{
				obj = PSVariable.CopyMutableValues(obj);
			}
			this._value = obj;
			this.DebuggerCheckVariableWrite();
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x001B0090 File Offset: 0x001AE290
		private void SetValueRawImpl(object newValue, bool preserveValueTypeSemantics)
		{
			if (preserveValueTypeSemantics)
			{
				newValue = PSVariable.CopyMutableValues(newValue);
			}
			this._value = newValue;
		}

		// Token: 0x06005103 RID: 20739 RVA: 0x001B00A4 File Offset: 0x001AE2A4
		internal virtual void SetValueRaw(object newValue, bool preserveValueTypeSemantics)
		{
			this.SetValueRawImpl(newValue, preserveValueTypeSemantics);
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x001B00AE File Offset: 0x001AE2AE
		internal static object CopyMutableValues(object o)
		{
			return PSVariable._copyMutableValueSite.Target(PSVariable._copyMutableValueSite, o);
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x001B00C5 File Offset: 0x001AE2C5
		internal void WrapValue()
		{
			if (!this.IsConstant && this._value != null)
			{
				this._value = PSObject.AsPSObject(this._value);
			}
		}

		// Token: 0x04002963 RID: 10595
		private string name;

		// Token: 0x04002964 RID: 10596
		private string description;

		// Token: 0x04002965 RID: 10597
		private object _value;

		// Token: 0x04002966 RID: 10598
		private SessionStateEntryVisibility _visibility;

		// Token: 0x04002967 RID: 10599
		private PSModuleInfo _module;

		// Token: 0x04002968 RID: 10600
		private ScopedItemOptions options;

		// Token: 0x04002969 RID: 10601
		private PSVariableAttributeCollection attributes;

		// Token: 0x0400296A RID: 10602
		private bool _wasRemoved;

		// Token: 0x0400296B RID: 10603
		private SessionStateInternal _sessionState;

		// Token: 0x0400296C RID: 10604
		private static readonly CallSite<Func<CallSite, object, object>> _copyMutableValueSite = CallSite<Func<CallSite, object, object>>.Create(PSVariableAssignmentBinder.Get());
	}
}
