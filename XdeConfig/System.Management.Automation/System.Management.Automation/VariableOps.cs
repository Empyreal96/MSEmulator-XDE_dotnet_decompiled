using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x02000641 RID: 1601
	internal static class VariableOps
	{
		// Token: 0x0600455C RID: 17756 RVA: 0x0017389C File Offset: 0x00171A9C
		private static object SetVariableValue(VariablePath variablePath, object value, ExecutionContext executionContext, AttributeBaseAst[] attributeAsts)
		{
			SessionStateInternal engineSessionState = executionContext.EngineSessionState;
			CommandOrigin scopeOrigin = engineSessionState.CurrentScope.ScopeOrigin;
			if (!variablePath.IsVariable)
			{
				engineSessionState.SetVariable(variablePath, value, true, scopeOrigin);
				return value;
			}
			if (executionContext.PSDebugTraceLevel > 1)
			{
				executionContext.Debugger.TraceVariableSet(variablePath.UnqualifiedPath, value);
			}
			if (variablePath.IsUnscopedVariable)
			{
				variablePath = variablePath.CloneAndSetLocal();
			}
			SessionStateScope sessionStateScope;
			PSVariable psvariable = engineSessionState.GetVariableItem(variablePath, out sessionStateScope, scopeOrigin);
			if (psvariable == null)
			{
				Collection<Attribute> attributes = (attributeAsts == null) ? new Collection<Attribute>() : VariableOps.GetAttributeCollection(attributeAsts);
				psvariable = new PSVariable(variablePath.UnqualifiedPath, value, ScopedItemOptions.None, attributes);
				engineSessionState.SetVariable(variablePath, psvariable, false, scopeOrigin);
				if (executionContext._debuggingMode > 0)
				{
					executionContext.Debugger.CheckVariableWrite(variablePath.UnqualifiedPath);
				}
			}
			else if (attributeAsts != null)
			{
				psvariable.Attributes.Clear();
				Collection<Attribute> attributeCollection = VariableOps.GetAttributeCollection(attributeAsts);
				value = PSVariable.TransformValue(attributeCollection, value);
				if (!PSVariable.IsValidValue(attributeCollection, value))
				{
					ValidationMetadataException ex = new ValidationMetadataException("ValidateSetFailure", null, Metadata.InvalidValueFailure, new object[]
					{
						psvariable.Name,
						(value != null) ? value.ToString() : "$null"
					});
					throw ex;
				}
				psvariable.SetValueRaw(value, true);
				psvariable.AddParameterAttributesNoChecks(attributeCollection);
				if (executionContext._debuggingMode > 0)
				{
					executionContext.Debugger.CheckVariableWrite(variablePath.UnqualifiedPath);
				}
			}
			else
			{
				psvariable.Value = value;
			}
			return value;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x001739F8 File Offset: 0x00171BF8
		private static bool ThrowStrictModeUndefinedVariable(ExecutionContext executionContext, VariableExpressionAst varAst)
		{
			if (varAst == null)
			{
				return false;
			}
			if (executionContext.IsStrictVersion(2))
			{
				return true;
			}
			if (executionContext.IsStrictVersion(1))
			{
				for (Ast parent = varAst.Parent; parent != null; parent = parent.Parent)
				{
					if (parent is ExpandableStringExpressionAst)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x00173A40 File Offset: 0x00171C40
		private static object GetAutomaticVariableValue(int tupleIndex, ExecutionContext executionContext, VariableExpressionAst varAst)
		{
			if (executionContext._debuggingMode > 0)
			{
				executionContext.Debugger.CheckVariableRead(SpecialVariables.AutomaticVariables[tupleIndex]);
			}
			object obj = executionContext.EngineSessionState.GetAutomaticVariableValue((AutomaticVariable)tupleIndex);
			if (obj == AutomationNull.Value)
			{
				if (VariableOps.ThrowStrictModeUndefinedVariable(executionContext, varAst))
				{
					throw InterpreterError.NewInterpreterException(SpecialVariables.AutomaticVariables[tupleIndex], typeof(RuntimeException), varAst.Extent, "VariableIsUndefined", ParserStrings.VariableIsUndefined, new object[]
					{
						SpecialVariables.AutomaticVariables[tupleIndex]
					});
				}
				obj = null;
			}
			return obj;
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x00173AC4 File Offset: 0x00171CC4
		internal static object GetVariableValue(VariablePath variablePath, ExecutionContext executionContext, VariableExpressionAst varAst)
		{
			if (!variablePath.IsVariable)
			{
				SessionStateInternal engineSessionState = executionContext.EngineSessionState;
				CmdletProviderContext cmdletProviderContext;
				SessionStateScope sessionStateScope;
				return engineSessionState.GetVariableValueFromProvider(variablePath, out cmdletProviderContext, out sessionStateScope, engineSessionState.CurrentScope.ScopeOrigin);
			}
			SessionStateInternal engineSessionState2 = executionContext.EngineSessionState;
			CommandOrigin scopeOrigin = engineSessionState2.CurrentScope.ScopeOrigin;
			SessionStateScope sessionStateScope2;
			PSVariable variableItem = engineSessionState2.GetVariableItem(variablePath, out sessionStateScope2, scopeOrigin);
			if (variableItem != null)
			{
				return variableItem.Value;
			}
			if (engineSessionState2.ExecutionContext._debuggingMode > 0)
			{
				engineSessionState2.ExecutionContext.Debugger.CheckVariableRead(variablePath.UnqualifiedPath);
			}
			if (VariableOps.ThrowStrictModeUndefinedVariable(executionContext, varAst))
			{
				throw InterpreterError.NewInterpreterException(variablePath.UserPath, typeof(RuntimeException), varAst.Extent, "VariableIsUndefined", ParserStrings.VariableIsUndefined, new object[]
				{
					variablePath.UserPath
				});
			}
			return null;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x00173B8C File Offset: 0x00171D8C
		private static PSReference GetVariableAsRef(VariablePath variablePath, ExecutionContext executionContext, Type staticType)
		{
			SessionStateInternal engineSessionState = executionContext.EngineSessionState;
			CommandOrigin scopeOrigin = engineSessionState.CurrentScope.ScopeOrigin;
			SessionStateScope sessionStateScope;
			PSVariable variableItem = engineSessionState.GetVariableItem(variablePath, out sessionStateScope, scopeOrigin);
			if (variableItem == null)
			{
				throw InterpreterError.NewInterpreterException(variablePath, typeof(RuntimeException), null, "NonExistingVariableReference", ParserStrings.NonExistingVariableReference, new object[0]);
			}
			object obj = variableItem.Value;
			if (staticType == null && obj != null)
			{
				obj = PSObject.Base(obj);
				if (obj != null)
				{
					staticType = obj.GetType();
				}
			}
			if (staticType == null)
			{
				ArgumentTypeConverterAttribute argumentTypeConverterAttribute = variableItem.Attributes.OfType<ArgumentTypeConverterAttribute>().FirstOrDefault<ArgumentTypeConverterAttribute>();
				staticType = ((argumentTypeConverterAttribute != null) ? argumentTypeConverterAttribute.TargetType : typeof(LanguagePrimitives.Null));
			}
			return PSReference.CreateInstance(variableItem, staticType);
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x00173C44 File Offset: 0x00171E44
		private static Collection<Attribute> GetAttributeCollection(AttributeBaseAst[] attributeAsts)
		{
			Collection<Attribute> collection = new Collection<Attribute>();
			foreach (AttributeBaseAst attributeBaseAst in attributeAsts)
			{
				collection.Add(attributeBaseAst.GetAttribute());
			}
			return collection;
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x00173C78 File Offset: 0x00171E78
		private static VariableOps.UsingResult GetUsingValueFromTuple(MutableTuple tuple, string usingExpressionKey, int index)
		{
			PSBoundParametersDictionary psboundParametersDictionary = tuple.GetAutomaticVariable(AutomaticVariable.PSBoundParameters) as PSBoundParametersDictionary;
			if (psboundParametersDictionary != null)
			{
				IDictionary implicitUsingParameters = psboundParametersDictionary.ImplicitUsingParameters;
				if (implicitUsingParameters != null)
				{
					if (implicitUsingParameters.Contains(usingExpressionKey))
					{
						return new VariableOps.UsingResult
						{
							Value = implicitUsingParameters[usingExpressionKey]
						};
					}
					if (implicitUsingParameters.Contains(index))
					{
						return new VariableOps.UsingResult
						{
							Value = implicitUsingParameters[index]
						};
					}
				}
			}
			return null;
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x00173CE8 File Offset: 0x00171EE8
		private static object GetUsingValue(MutableTuple tuple, string usingExpressionKey, int index, ExecutionContext context)
		{
			VariableOps.UsingResult usingValueFromTuple = VariableOps.GetUsingValueFromTuple(tuple, usingExpressionKey, index);
			if (usingValueFromTuple != null)
			{
				return usingValueFromTuple.Value;
			}
			for (SessionStateScope sessionStateScope = context.EngineSessionState.CurrentScope; sessionStateScope != null; sessionStateScope = sessionStateScope.Parent)
			{
				usingValueFromTuple = VariableOps.GetUsingValueFromTuple(sessionStateScope.LocalsTuple, usingExpressionKey, index);
				if (usingValueFromTuple != null)
				{
					return usingValueFromTuple.Value;
				}
				foreach (MutableTuple tuple2 in sessionStateScope.DottedScopes)
				{
					usingValueFromTuple = VariableOps.GetUsingValueFromTuple(tuple2, usingExpressionKey, index);
					if (usingValueFromTuple != null)
					{
						return usingValueFromTuple.Value;
					}
				}
			}
			throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "UsingWithoutInvokeCommand", ParserStrings.UsingWithoutInvokeCommand, new object[0]);
		}

		// Token: 0x02000642 RID: 1602
		private class UsingResult
		{
			// Token: 0x17000EB1 RID: 3761
			// (get) Token: 0x06004564 RID: 17764 RVA: 0x00173DB0 File Offset: 0x00171FB0
			// (set) Token: 0x06004565 RID: 17765 RVA: 0x00173DB8 File Offset: 0x00171FB8
			public object Value { get; set; }
		}
	}
}
