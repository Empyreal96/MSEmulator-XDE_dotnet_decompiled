using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x0200048E RID: 1166
	[Serializable]
	public class ScriptBlock : ISerializable
	{
		// Token: 0x0600338E RID: 13198 RVA: 0x00119AD0 File Offset: 0x00117CD0
		internal static ScriptBlock Create(ExecutionContext context, string script)
		{
			ScriptBlock scriptBlock = ScriptBlock.Create(context.Engine.EngineParser, null, script);
			if (context.EngineSessionState != null && context.EngineSessionState.Module != null)
			{
				scriptBlock.SessionStateInternal = context.EngineSessionState;
			}
			return scriptBlock;
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x00119B12 File Offset: 0x00117D12
		public static ScriptBlock Create(string script)
		{
			return ScriptBlock.Create(new Parser(), null, script);
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x00119B20 File Offset: 0x00117D20
		internal static ScriptBlock CreateDelayParsedScriptBlock(string script)
		{
			return new ScriptBlock(new CompiledScriptBlockData(script));
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x00119B30 File Offset: 0x00117D30
		public ScriptBlock GetNewClosure()
		{
			PSModuleInfo psmoduleInfo = new PSModuleInfo(true);
			psmoduleInfo.CaptureLocals();
			return psmoduleInfo.NewBoundScriptBlock(this);
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x00119B54 File Offset: 0x00117D54
		public PowerShell GetPowerShell(params object[] args)
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			return this.GetPowerShellImpl(executionContextFromTLS, null, false, false, null, args);
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x00119B7C File Offset: 0x00117D7C
		public PowerShell GetPowerShell(bool isTrustedInput, params object[] args)
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			return this.GetPowerShellImpl(executionContextFromTLS, null, isTrustedInput, false, null, args);
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x00119BA4 File Offset: 0x00117DA4
		public PowerShell GetPowerShell(Dictionary<string, object> variables, params object[] args)
		{
			ExecutionContext context = LocalPipeline.GetExecutionContextFromTLS();
			Dictionary<string, object> variables2 = null;
			if (variables != null)
			{
				variables2 = new Dictionary<string, object>(variables, StringComparer.OrdinalIgnoreCase);
				context = null;
			}
			return this.GetPowerShellImpl(context, variables2, false, false, null, args);
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x00119BDE File Offset: 0x00117DDE
		public PowerShell GetPowerShell(Dictionary<string, object> variables, out Dictionary<string, object> usingVariables, params object[] args)
		{
			return this.GetPowerShell(variables, out usingVariables, false, args);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x00119BEC File Offset: 0x00117DEC
		public PowerShell GetPowerShell(Dictionary<string, object> variables, out Dictionary<string, object> usingVariables, bool isTrustedInput, params object[] args)
		{
			ExecutionContext context = LocalPipeline.GetExecutionContextFromTLS();
			Dictionary<string, object> dictionary = null;
			if (variables != null)
			{
				dictionary = new Dictionary<string, object>(variables, StringComparer.OrdinalIgnoreCase);
				context = null;
			}
			PowerShell powerShellImpl = this.GetPowerShellImpl(context, dictionary, isTrustedInput, true, null, args);
			usingVariables = dictionary;
			return powerShellImpl;
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x00119C2C File Offset: 0x00117E2C
		internal PowerShell GetPowerShell(ExecutionContext context, bool isTrustedInput, bool? useLocalScope, params object[] args)
		{
			return this.GetPowerShellImpl(context, null, isTrustedInput, false, useLocalScope, args);
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x00119C3B File Offset: 0x00117E3B
		public SteppablePipeline GetSteppablePipeline()
		{
			return this.GetSteppablePipelineImpl(CommandOrigin.Internal, null);
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x00119C45 File Offset: 0x00117E45
		public SteppablePipeline GetSteppablePipeline(CommandOrigin commandOrigin)
		{
			return this.GetSteppablePipelineImpl(commandOrigin, null);
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x00119C4F File Offset: 0x00117E4F
		public SteppablePipeline GetSteppablePipeline(CommandOrigin commandOrigin, object[] args)
		{
			return this.GetSteppablePipelineImpl(commandOrigin, args);
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x00119C59 File Offset: 0x00117E59
		public Collection<PSObject> Invoke(params object[] args)
		{
			return this.DoInvoke(AutomationNull.Value, AutomationNull.Value, args);
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x00119C6C File Offset: 0x00117E6C
		public Collection<PSObject> InvokeWithContext(IDictionary functionsToDefine, List<PSVariable> variablesToDefine, params object[] args)
		{
			Dictionary<string, ScriptBlock> dictionary = null;
			if (functionsToDefine != null)
			{
				dictionary = new Dictionary<string, ScriptBlock>();
				foreach (object obj in functionsToDefine)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text = dictionaryEntry.Key as string;
					if (string.IsNullOrWhiteSpace(text))
					{
						PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.EmptyFunctionNameInFunctionDefinitionDictionary, new object[0]);
						ex.SetErrorId("EmptyFunctionNameInFunctionDefinitionDictionary");
						throw ex;
					}
					ScriptBlock value = dictionaryEntry.Value as ScriptBlock;
					dictionary.Add(text, value);
				}
			}
			return this.InvokeWithContext(dictionary, variablesToDefine, args);
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x00119D60 File Offset: 0x00117F60
		public Collection<PSObject> InvokeWithContext(Dictionary<string, ScriptBlock> functionsToDefine, List<PSVariable> variablesToDefine, params object[] args)
		{
			object value = AutomationNull.Value;
			object value2 = AutomationNull.Value;
			object value3 = AutomationNull.Value;
			if (variablesToDefine != null)
			{
				PSVariable psvariable = variablesToDefine.FirstOrDefault((PSVariable v) => string.Equals(v.Name, "this", StringComparison.OrdinalIgnoreCase));
				if (psvariable != null)
				{
					value3 = psvariable.Value;
					variablesToDefine.Remove(psvariable);
				}
				psvariable = variablesToDefine.FirstOrDefault((PSVariable v) => string.Equals(v.Name, "_", StringComparison.OrdinalIgnoreCase));
				if (psvariable != null)
				{
					value2 = psvariable.Value;
					variablesToDefine.Remove(psvariable);
				}
				psvariable = variablesToDefine.FirstOrDefault((PSVariable v) => string.Equals(v.Name, "input", StringComparison.OrdinalIgnoreCase));
				if (psvariable != null)
				{
					value = psvariable.Value;
					variablesToDefine.Remove(psvariable);
				}
			}
			List<object> list = new List<object>();
			Pipe outputPipe = new Pipe(list);
			this.InvokeWithPipe(true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, value2, value, value3, outputPipe, null, false, variablesToDefine, functionsToDefine, args);
			return ScriptBlock.GetWrappedResult(list);
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x00119E58 File Offset: 0x00118058
		public object InvokeReturnAsIs(params object[] args)
		{
			return this.DoInvokeReturnAsIs(true, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, AutomationNull.Value, AutomationNull.Value, AutomationNull.Value, args);
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x00119E74 File Offset: 0x00118074
		internal T InvokeAsMemberFunctionT<T>(object instance, object[] args)
		{
			List<object> list = new List<object>();
			Pipe outputPipe = new Pipe(list);
			this.InvokeWithPipe(true, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, AutomationNull.Value, AutomationNull.Value, instance ?? AutomationNull.Value, outputPipe, null, true, null, null, args);
			return (T)((object)list[0]);
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x00119EBC File Offset: 0x001180BC
		internal void InvokeAsMemberFunction(object instance, object[] args)
		{
			List<object> resultList = new List<object>();
			Pipe outputPipe = new Pipe(resultList);
			this.InvokeWithPipe(true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, AutomationNull.Value, instance ?? AutomationNull.Value, outputPipe, null, true, null, null, args);
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x00119EF8 File Offset: 0x001180F8
		public List<Attribute> Attributes
		{
			get
			{
				return this.GetAttributes();
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x00119F00 File Offset: 0x00118100
		public string File
		{
			get
			{
				return this.GetFileName();
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x060033A3 RID: 13219 RVA: 0x00119F08 File Offset: 0x00118108
		// (set) Token: 0x060033A4 RID: 13220 RVA: 0x00119F10 File Offset: 0x00118110
		public bool IsFilter
		{
			get
			{
				return this.GetIsFilter();
			}
			set
			{
				this.SetIsFilter(value);
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x060033A5 RID: 13221 RVA: 0x00119F19 File Offset: 0x00118119
		// (set) Token: 0x060033A6 RID: 13222 RVA: 0x00119F21 File Offset: 0x00118121
		public bool IsConfiguration
		{
			get
			{
				return this.GetIsConfiguration();
			}
			set
			{
				this.SetIsConfiguration(value);
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x060033A7 RID: 13223 RVA: 0x00119F2A File Offset: 0x0011812A
		public PSModuleInfo Module
		{
			get
			{
				if (this.SessionStateInternal == null)
				{
					return null;
				}
				return this.SessionStateInternal.Module;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x060033A8 RID: 13224 RVA: 0x00119F41 File Offset: 0x00118141
		public PSToken StartPosition
		{
			get
			{
				return this.GetStartPosition();
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x060033A9 RID: 13225 RVA: 0x00119F49 File Offset: 0x00118149
		// (set) Token: 0x060033AA RID: 13226 RVA: 0x00119F51 File Offset: 0x00118151
		internal PSLanguageMode? LanguageMode
		{
			get
			{
				return this.languageMode;
			}
			set
			{
				this.languageMode = value;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x060033AB RID: 13227 RVA: 0x00119F5C File Offset: 0x0011815C
		internal ReadOnlyCollection<PSTypeName> OutputType
		{
			get
			{
				List<PSTypeName> list = new List<PSTypeName>();
				foreach (Attribute attribute in this.Attributes)
				{
					OutputTypeAttribute outputTypeAttribute = attribute as OutputTypeAttribute;
					if (outputTypeAttribute != null)
					{
						list.AddRange(outputTypeAttribute.Type);
					}
				}
				return new ReadOnlyCollection<PSTypeName>(list);
			}
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x00119FCC File Offset: 0x001181CC
		internal static object GetRawResult(List<object> result)
		{
			if (result.Count == 0)
			{
				return AutomationNull.Value;
			}
			if (result.Count == 1)
			{
				return LanguagePrimitives.AsPSObjectOrNull(result[0]);
			}
			return LanguagePrimitives.AsPSObjectOrNull(result.ToArray());
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x0011A000 File Offset: 0x00118200
		internal void InvokeUsingCmdlet(Cmdlet contextCmdlet, bool useLocalScope, ScriptBlock.ErrorHandlingBehavior errorHandlingBehavior, object dollarUnder, object input, object scriptThis, params object[] args)
		{
			Pipe outputPipe = ((MshCommandRuntime)contextCmdlet.CommandRuntime).OutputPipe;
			ExecutionContext contextFromTLS = this.GetContextFromTLS();
			InvocationInfo invocationInfo = (InvocationInfo)contextFromTLS.GetVariableValue(SpecialVariables.MyInvocationVarPath);
			this.InvokeWithPipe(useLocalScope, errorHandlingBehavior, dollarUnder, input, scriptThis, outputPipe, invocationInfo, false, null, null, args);
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x060033AE RID: 13230 RVA: 0x0011A04B File Offset: 0x0011824B
		// (set) Token: 0x060033AF RID: 13231 RVA: 0x0011A053 File Offset: 0x00118253
		internal SessionStateInternal SessionStateInternal { get; set; }

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060033B0 RID: 13232 RVA: 0x0011A05C File Offset: 0x0011825C
		// (set) Token: 0x060033B1 RID: 13233 RVA: 0x0011A0A5 File Offset: 0x001182A5
		internal SessionState SessionState
		{
			get
			{
				if (this.SessionStateInternal == null)
				{
					ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
					if (executionContextFromTLS != null)
					{
						this.SessionStateInternal = executionContextFromTLS.EngineSessionState.PublicSessionState.Internal;
					}
				}
				if (this.SessionStateInternal == null)
				{
					return null;
				}
				return this.SessionStateInternal.PublicSessionState;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				this.SessionStateInternal = value.Internal;
			}
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x0011A0C4 File Offset: 0x001182C4
		internal Delegate GetDelegate(Type delegateType)
		{
			ConcurrentDictionary<Type, Delegate> orCreateValue = ScriptBlock.delegateTable.GetOrCreateValue(this);
			return orCreateValue.GetOrAdd(delegateType, new Func<Type, Delegate>(this.CreateDelegate));
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x0011A104 File Offset: 0x00118304
		internal Delegate CreateDelegate(Type delegateType)
		{
			MethodInfo method = delegateType.GetMethod("Invoke");
			ParameterInfo[] parameters = method.GetParameters();
			if (method.ContainsGenericParameters)
			{
				throw new ScriptBlockToPowerShellNotSupportedException("CantConvertScriptBlockToOpenGenericType", null, "AutomationExceptions", new object[]
				{
					"CantConvertScriptBlockToOpenGenericType",
					delegateType
				});
			}
			List<ParameterExpression> list = new List<ParameterExpression>();
			foreach (ParameterInfo parameterInfo in parameters)
			{
				list.Add(Expression.Parameter(parameterInfo.ParameterType));
			}
			bool flag = !method.ReturnType.Equals(typeof(void));
			Expression arg;
			Expression arg2;
			if (parameters.Length == 2 && !flag)
			{
				arg = list[1].Cast(typeof(object));
				arg2 = list[0].Cast(typeof(object));
			}
			else
			{
				arg = ExpressionCache.AutomationNullConstant;
				arg2 = ExpressionCache.AutomationNullConstant;
			}
			Expression expression = Expression.Call(Expression.Constant(this), CachedReflectionInfo.ScriptBlock_InvokeAsDelegateHelper, arg, arg2, Expression.NewArrayInit(typeof(object), from p in list
			select p.Cast(typeof(object))));
			if (flag)
			{
				expression = DynamicExpression.Dynamic(PSConvertBinder.Get(method.ReturnType), method.ReturnType, expression);
			}
			return Expression.Lambda(delegateType, expression, list).Compile();
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x0011A260 File Offset: 0x00118460
		internal object InvokeAsDelegateHelper(object dollarUnder, object dollarThis, params object[] args)
		{
			ExecutionContext contextFromTLS = this.GetContextFromTLS();
			RunspaceBase runspaceBase = (RunspaceBase)contextFromTLS.CurrentRunspace;
			List<object> list = new List<object>();
			Pipe outputPipe = new Pipe(list);
			this.InvokeWithPipe(true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, dollarUnder, null, dollarThis, outputPipe, null, false, null, null, args);
			return ScriptBlock.GetRawResult(list);
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x0011A2A4 File Offset: 0x001184A4
		internal ExecutionContext GetContextFromTLS()
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				string text = this.ToString();
				text = ErrorCategoryInfo.Ellipsize(CultureInfo.CurrentUICulture, text);
				PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.ScriptBlockDelegateInvokedFromWrongThread, new object[]
				{
					text
				});
				ex.SetErrorId("ScriptBlockDelegateInvokedFromWrongThread");
				throw ex;
			}
			return executionContextFromTLS;
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x0011A2F4 File Offset: 0x001184F4
		internal Collection<PSObject> DoInvoke(object dollarUnder, object input, params object[] args)
		{
			List<object> list = new List<object>();
			Pipe outputPipe = new Pipe(list);
			this.InvokeWithPipe(true, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, dollarUnder, input, AutomationNull.Value, outputPipe, null, false, null, null, args);
			return ScriptBlock.GetWrappedResult(list);
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x0011A32C File Offset: 0x0011852C
		private static Collection<PSObject> GetWrappedResult(List<object> result)
		{
			if (result == null || result.Count == 0)
			{
				return new Collection<PSObject>();
			}
			Collection<PSObject> collection = new Collection<PSObject>();
			for (int i = 0; i < result.Count; i++)
			{
				collection.Add(LanguagePrimitives.AsPSObjectOrNull(result[i]));
			}
			return collection;
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x0011A374 File Offset: 0x00118574
		internal object DoInvokeReturnAsIs(bool useLocalScope, ScriptBlock.ErrorHandlingBehavior errorHandlingBehavior, object dollarUnder, object input, object scriptThis, params object[] args)
		{
			List<object> list = new List<object>();
			Pipe outputPipe = new Pipe(list);
			this.InvokeWithPipe(useLocalScope, errorHandlingBehavior, dollarUnder, input, scriptThis, outputPipe, null, false, null, null, args);
			return ScriptBlock.GetRawResult(list);
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x0011A404 File Offset: 0x00118604
		internal void InvokeWithPipe(bool useLocalScope, ScriptBlock.ErrorHandlingBehavior errorHandlingBehavior, object dollarUnder, object input, object scriptThis, Pipe outputPipe, InvocationInfo invocationInfo, bool propagateAllExceptionsToTop = false, List<PSVariable> variablesToDefine = null, Dictionary<string, ScriptBlock> functionsToDefine = null, object[] args = null)
		{
			bool flag = false;
			bool propagateExceptionsToEnclosingStatementBlock = false;
			ExecutionContext executionContext = LocalPipeline.GetExecutionContextFromTLS();
			if (this.SessionStateInternal != null && this.SessionStateInternal.ExecutionContext != executionContext)
			{
				executionContext = this.SessionStateInternal.ExecutionContext;
				flag = true;
			}
			else if (executionContext == null)
			{
				this.GetContextFromTLS();
			}
			else
			{
				if (propagateAllExceptionsToTop)
				{
					propagateExceptionsToEnclosingStatementBlock = executionContext.PropagateExceptionsToEnclosingStatementBlock;
					executionContext.PropagateExceptionsToEnclosingStatementBlock = true;
				}
				try
				{
					RunspaceBase runspaceBase = (RunspaceBase)executionContext.CurrentRunspace;
					flag = !runspaceBase.RunActionIfNoRunningPipelinesWithThreadCheck(delegate
					{
						this.InvokeWithPipeImpl(useLocalScope, functionsToDefine, variablesToDefine, errorHandlingBehavior, dollarUnder, input, scriptThis, outputPipe, invocationInfo, args);
					});
				}
				finally
				{
					if (propagateAllExceptionsToTop)
					{
						executionContext.PropagateExceptionsToEnclosingStatementBlock = propagateExceptionsToEnclosingStatementBlock;
					}
				}
			}
			if (flag)
			{
				executionContext.Events.SubscribeEvent(null, "PowerShell.OnScriptBlockInvoke", "PowerShell.OnScriptBlockInvoke", null, new PSEventReceivedEventHandler(ScriptBlock.OnScriptBlockInvokeEventHandler), true, false, true, 1);
				ScriptBlockInvocationEventArgs scriptBlockInvocationEventArgs = new ScriptBlockInvocationEventArgs(this, useLocalScope, errorHandlingBehavior, dollarUnder, input, scriptThis, outputPipe, invocationInfo, args);
				executionContext.Events.GenerateEvent("PowerShell.OnScriptBlockInvoke", null, new object[]
				{
					scriptBlockInvocationEventArgs
				}, null, true, true);
				if (scriptBlockInvocationEventArgs.Exception != null)
				{
					throw scriptBlockInvocationEventArgs.Exception;
				}
			}
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x0011A5B8 File Offset: 0x001187B8
		private static void OnScriptBlockInvokeEventHandler(object sender, PSEventArgs args)
		{
			ScriptBlockInvocationEventArgs scriptBlockInvocationEventArgs = args.SourceEventArgs as ScriptBlockInvocationEventArgs;
			try
			{
				ScriptBlock scriptBlock = scriptBlockInvocationEventArgs.ScriptBlock;
				scriptBlock.InvokeWithPipeImpl(scriptBlockInvocationEventArgs.UseLocalScope, null, null, scriptBlockInvocationEventArgs.ErrorHandlingBehavior, scriptBlockInvocationEventArgs.DollarUnder, scriptBlockInvocationEventArgs.Input, scriptBlockInvocationEventArgs.ScriptThis, scriptBlockInvocationEventArgs.OutputPipe, scriptBlockInvocationEventArgs.InvocationInfo, scriptBlockInvocationEventArgs.Args);
			}
			catch (Exception exception)
			{
				scriptBlockInvocationEventArgs.Exception = exception;
			}
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x0011A62C File Offset: 0x0011882C
		internal void SetPSScriptRootAndPSCommandPath(MutableTuple locals, ExecutionContext context)
		{
			string value = string.Empty;
			string value2 = string.Empty;
			if (!string.IsNullOrEmpty(this.File))
			{
				value = Path.GetDirectoryName(this.File);
				value2 = this.File;
			}
			locals.SetAutomaticVariable(AutomaticVariable.PSScriptRoot, value, context);
			locals.SetAutomaticVariable(AutomaticVariable.PSCommandPath, value2, context);
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x0011A677 File Offset: 0x00118877
		internal ScriptBlock(IParameterMetadataProvider ast, bool isFilter) : this(ast, isFilter, new CompiledScriptBlockData(ast))
		{
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x0011A687 File Offset: 0x00118887
		private ScriptBlock(CompiledScriptBlockData scriptBlockData)
		{
			this.languageMode = null;
			base..ctor();
			this._scriptBlockData = scriptBlockData;
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x0011A6A4 File Offset: 0x001188A4
		private ScriptBlock(IParameterMetadataProvider ast, bool isFilter, CompiledScriptBlockData scriptBlockData)
		{
			this.languageMode = null;
			base..ctor();
			this._scriptBlockData = scriptBlockData;
			this._isFilter = isFilter;
			ScriptBlockAst scriptBlockAst = ast as ScriptBlockAst;
			if (scriptBlockAst != null)
			{
				this._isConfiguration = scriptBlockAst.IsConfiguration;
			}
			this.SetLanguageModeFromContext();
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x0011A6F0 File Offset: 0x001188F0
		private void SetLanguageModeFromContext()
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS != null)
			{
				this.LanguageMode = new PSLanguageMode?(executionContextFromTLS.LanguageMode);
			}
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x0011A717 File Offset: 0x00118917
		protected ScriptBlock(SerializationInfo info, StreamingContext context)
		{
			this.languageMode = null;
			base..ctor();
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x0011A72C File Offset: 0x0011892C
		internal static ScriptBlock TryGetCachedScriptBlock(string fileName, string fileContents)
		{
			if (InternalTestHooks.IgnoreScriptBlockCache)
			{
				return null;
			}
			Tuple<string, string> key = Tuple.Create<string, string>(fileName, fileContents);
			ScriptBlock scriptBlock;
			if (ScriptBlock._cachedScripts.TryGetValue(key, out scriptBlock))
			{
				return scriptBlock.Clone(false);
			}
			return null;
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x0011A764 File Offset: 0x00118964
		private static bool IsDynamicKeyword(Ast ast)
		{
			CommandAst commandAst = ast as CommandAst;
			return commandAst != null && commandAst.DefiningKeyword != null;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x0011A78C File Offset: 0x0011898C
		private static bool IsUsingTypes(Ast ast)
		{
			UsingStatementAst usingStatementAst = ast as UsingStatementAst;
			return usingStatementAst != null && usingStatementAst.IsUsingModuleOrAssembly();
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x0011A7BC File Offset: 0x001189BC
		internal static void CacheScriptBlock(ScriptBlock scriptBlock, string fileName, string fileContents)
		{
			if (InternalTestHooks.IgnoreScriptBlockCache)
			{
				return;
			}
			if (scriptBlock.Ast.Find((Ast ast) => ScriptBlock.IsUsingTypes(ast), false) == null)
			{
				if (scriptBlock.Ast.Find((Ast ast) => ScriptBlock.IsDynamicKeyword(ast), true) == null)
				{
					if (ScriptBlock._cachedScripts.Count > 1024)
					{
						ScriptBlock._cachedScripts.Clear();
					}
					Tuple<string, string> key = Tuple.Create<string, string>(fileName, fileContents);
					ScriptBlock._cachedScripts.TryAdd(key, scriptBlock);
					return;
				}
			}
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x0011A858 File Offset: 0x00118A58
		internal static void ClearScriptBlockCache()
		{
			ScriptBlock._cachedScripts.Clear();
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x0011A864 File Offset: 0x00118A64
		internal static ScriptBlock Create(Parser parser, string fileName, string fileContents)
		{
			ScriptBlock scriptBlock = ScriptBlock.TryGetCachedScriptBlock(fileName, fileContents);
			if (scriptBlock != null)
			{
				return scriptBlock;
			}
			ParseError[] array;
			ScriptBlockAst ast = parser.Parse(fileName, fileContents, null, out array);
			if (array.Length != 0)
			{
				throw new ParseException(array);
			}
			ScriptBlock scriptBlock2 = new ScriptBlock(ast, false);
			ScriptBlock.CacheScriptBlock(scriptBlock2, fileName, fileContents);
			return scriptBlock2.Clone(false);
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x0011A8AD File Offset: 0x00118AAD
		internal ScriptBlock Clone(bool cloneHelpInfo = false)
		{
			return new ScriptBlock(this.AstInternal, this._isFilter, this._scriptBlockData);
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x0011A8C6 File Offset: 0x00118AC6
		public override string ToString()
		{
			return this._scriptBlockData.ToString();
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x0011A8D4 File Offset: 0x00118AD4
		internal string ToStringWithDollarUsingHandling(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple)
		{
			FunctionDefinitionAst functionDefinitionAst = null;
			ScriptBlockAst scriptBlockAst = this.Ast as ScriptBlockAst;
			if (scriptBlockAst == null)
			{
				functionDefinitionAst = (FunctionDefinitionAst)this.Ast;
				scriptBlockAst = functionDefinitionAst.Body;
			}
			string text = scriptBlockAst.ToStringForSerialization(usingVariablesTuple, scriptBlockAst.Extent.StartOffset, scriptBlockAst.Extent.EndOffset);
			if (scriptBlockAst.ParamBlock != null)
			{
				return text;
			}
			string item = usingVariablesTuple.Item2;
			string str;
			if (functionDefinitionAst == null || functionDefinitionAst.Parameters == null)
			{
				str = "param(" + item + ")" + Environment.NewLine;
			}
			else
			{
				str = functionDefinitionAst.GetParamTextFromParameterList(usingVariablesTuple);
			}
			return str + text;
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x0011A96C File Offset: 0x00118B6C
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			string value = this.ToString();
			info.AddValue("ScriptText", value);
			info.SetType(typeof(ScriptBlockSerializationHelper));
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x0011A9AA File Offset: 0x00118BAA
		internal PowerShell GetPowerShellImpl(ExecutionContext context, Dictionary<string, object> variables, bool isTrustedInput, bool filterNonUsingVariables, bool? createLocalScope, params object[] args)
		{
			return this.AstInternal.GetPowerShell(context, variables, isTrustedInput, filterNonUsingVariables, createLocalScope, args);
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x0011A9D0 File Offset: 0x00118BD0
		internal SteppablePipeline GetSteppablePipelineImpl(CommandOrigin commandOrigin, object[] args)
		{
			PipelineAst simplePipeline = this.GetSimplePipeline(delegate(string resourceString)
			{
				throw PSTraceSource.NewInvalidOperationException(resourceString, new object[0]);
			});
			if (!(simplePipeline.PipelineElements[0] is CommandAst))
			{
				throw PSTraceSource.NewInvalidOperationException(AutomationExceptions.CantConvertEmptyPipeline, new object[0]);
			}
			return PipelineOps.GetSteppablePipeline(simplePipeline, commandOrigin, this, args);
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x0011AA34 File Offset: 0x00118C34
		private PipelineAst GetSimplePipeline(Func<string, PipelineAst> errorHandler)
		{
			Func<string, PipelineAst> func;
			if ((func = errorHandler) == null)
			{
				func = ((string _) => null);
			}
			errorHandler = func;
			if (this.HasBeginBlock || this.HasProcessBlock)
			{
				return errorHandler(AutomationExceptions.CanConvertOneClauseOnly);
			}
			IParameterMetadataProvider astInternal = this.AstInternal;
			ReadOnlyCollection<StatementAst> statements = astInternal.Body.EndBlock.Statements;
			if (!statements.Any<StatementAst>())
			{
				return errorHandler(AutomationExceptions.CantConvertEmptyPipeline);
			}
			if (statements.Count > 1)
			{
				return errorHandler(AutomationExceptions.CanOnlyConvertOnePipeline);
			}
			if (astInternal.Body.EndBlock.Traps != null && astInternal.Body.EndBlock.Traps.Any<TrapStatementAst>())
			{
				return errorHandler(AutomationExceptions.CantConvertScriptBlockWithTrap);
			}
			PipelineAst pipelineAst = statements[0] as PipelineAst;
			if (pipelineAst == null)
			{
				return errorHandler(AutomationExceptions.CanOnlyConvertOnePipeline);
			}
			return pipelineAst;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x0011AB14 File Offset: 0x00118D14
		internal List<Attribute> GetAttributes()
		{
			return this._scriptBlockData.GetAttributes();
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x0011AB21 File Offset: 0x00118D21
		internal string GetFileName()
		{
			return this.AstInternal.Body.Extent.File;
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x0011AB38 File Offset: 0x00118D38
		internal bool GetIsFilter()
		{
			return this._isFilter;
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x0011AB40 File Offset: 0x00118D40
		internal void SetIsFilter(bool value)
		{
			throw new PSInvalidOperationException();
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x0011AB47 File Offset: 0x00118D47
		internal bool GetIsConfiguration()
		{
			return this._isConfiguration;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x0011AB4F File Offset: 0x00118D4F
		internal bool IsMetaConfiguration()
		{
			return this.GetAttributes().OfType<DscLocalConfigurationManagerAttribute>().Any<DscLocalConfigurationManagerAttribute>();
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x0011AB61 File Offset: 0x00118D61
		internal void SetIsConfiguration(bool value)
		{
			throw new PSInvalidOperationException();
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x0011AB68 File Offset: 0x00118D68
		internal void SetIsMetaConfiguration(bool value)
		{
			throw new PSInvalidOperationException();
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x0011AB6F File Offset: 0x00118D6F
		internal PSToken GetStartPosition()
		{
			return new PSToken(this.Ast.Extent);
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060033D7 RID: 13271 RVA: 0x0011AB81 File Offset: 0x00118D81
		internal MergedCommandParameterMetadata ParameterMetadata
		{
			get
			{
				return this._scriptBlockData.GetParameterMetadata(this);
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060033D8 RID: 13272 RVA: 0x0011AB8F File Offset: 0x00118D8F
		internal bool UsesCmdletBinding
		{
			get
			{
				return this._scriptBlockData.UsesCmdletBinding;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x0011AB9C File Offset: 0x00118D9C
		internal bool HasDynamicParameters
		{
			get
			{
				return this.AstInternal.Body.DynamicParamBlock != null;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060033DA RID: 13274 RVA: 0x0011ABB4 File Offset: 0x00118DB4
		// (set) Token: 0x060033DB RID: 13275 RVA: 0x0011ABC1 File Offset: 0x00118DC1
		public bool DebuggerHidden
		{
			get
			{
				return this._scriptBlockData.DebuggerHidden;
			}
			set
			{
				this._scriptBlockData.DebuggerHidden = value;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060033DC RID: 13276 RVA: 0x0011ABCF File Offset: 0x00118DCF
		public Guid Id
		{
			get
			{
				return this._scriptBlockData.Id;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x0011ABDC File Offset: 0x00118DDC
		// (set) Token: 0x060033DE RID: 13278 RVA: 0x0011ABE9 File Offset: 0x00118DE9
		internal bool DebuggerStepThrough
		{
			get
			{
				return this._scriptBlockData.DebuggerStepThrough;
			}
			set
			{
				this._scriptBlockData.DebuggerStepThrough = value;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060033DF RID: 13279 RVA: 0x0011ABF7 File Offset: 0x00118DF7
		internal RuntimeDefinedParameterDictionary RuntimeDefinedParameters
		{
			get
			{
				return this._scriptBlockData.RuntimeDefinedParameters;
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x060033E0 RID: 13280 RVA: 0x0011AC04 File Offset: 0x00118E04
		// (set) Token: 0x060033E1 RID: 13281 RVA: 0x0011AC11 File Offset: 0x00118E11
		internal bool HasLogged
		{
			get
			{
				return this._scriptBlockData.HasLogged;
			}
			set
			{
				this._scriptBlockData.HasLogged = value;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x060033E3 RID: 13283 RVA: 0x0011AC28 File Offset: 0x00118E28
		// (set) Token: 0x060033E2 RID: 13282 RVA: 0x0011AC1F File Offset: 0x00118E1F
		internal Assembly AssemblyDefiningPSTypes { get; set; }

		// Token: 0x060033E4 RID: 13284 RVA: 0x0011AC30 File Offset: 0x00118E30
		internal HelpInfo GetHelpInfo(ExecutionContext context, CommandInfo commandInfo, bool dontSearchOnRemoteComputer, Dictionary<Ast, Token[]> scriptBlockTokenCache, out string helpFile, out string helpUriFromDotLink)
		{
			helpUriFromDotLink = null;
			Tuple<List<Token>, List<string>> helpCommentTokens = HelpCommentsParser.GetHelpCommentTokens(this.AstInternal, scriptBlockTokenCache);
			if (helpCommentTokens != null)
			{
				return HelpCommentsParser.CreateFromComments(context, commandInfo, helpCommentTokens.Item1, helpCommentTokens.Item2, dontSearchOnRemoteComputer, out helpFile, out helpUriFromDotLink);
			}
			helpFile = null;
			return null;
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x0011AC70 File Offset: 0x00118E70
		public void CheckRestrictedLanguage(IEnumerable<string> allowedCommands, IEnumerable<string> allowedVariables, bool allowEnvironmentVariables)
		{
			Parser parser = new Parser();
			IParameterMetadataProvider astInternal = this.AstInternal;
			if (this.HasBeginBlock || this.HasProcessBlock || astInternal.Body.ParamBlock != null)
			{
				NamedBlockAst namedBlockAst;
				if ((namedBlockAst = astInternal.Body.BeginBlock) == null)
				{
					namedBlockAst = (astInternal.Body.ProcessBlock ?? astInternal.Body.ParamBlock);
				}
				Ast ast = namedBlockAst;
				parser.ReportError(ast.Extent, () => ParserStrings.InvalidScriptBlockInDataSection);
			}
			if (this.HasEndBlock)
			{
				RestrictedLanguageChecker visitor = new RestrictedLanguageChecker(parser, allowedCommands, allowedVariables, allowEnvironmentVariables);
				NamedBlockAst endBlock = astInternal.Body.EndBlock;
				StatementBlockAst.InternalVisit(visitor, endBlock.Traps, endBlock.Statements, AstVisitAction.Continue);
			}
			if (parser.ErrorList.Any<ParseError>())
			{
				throw new ParseException(parser.ErrorList.ToArray());
			}
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x0011AD50 File Offset: 0x00118F50
		internal string GetWithInputHandlingForInvokeCommand()
		{
			return this.AstInternal.GetWithInputHandlingForInvokeCommand();
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x0011AD60 File Offset: 0x00118F60
		internal string GetWithInputHandlingForInvokeCommandWithUsingExpression(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple)
		{
			Tuple<string, string> withInputHandlingForInvokeCommandWithUsingExpression = this.AstInternal.GetWithInputHandlingForInvokeCommandWithUsingExpression(usingVariablesTuple);
			if (withInputHandlingForInvokeCommandWithUsingExpression.Item1 == null)
			{
				return withInputHandlingForInvokeCommandWithUsingExpression.Item2;
			}
			return withInputHandlingForInvokeCommandWithUsingExpression.Item1 + withInputHandlingForInvokeCommandWithUsingExpression.Item2;
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0011AD9A File Offset: 0x00118F9A
		internal bool IsUsingDollarInput()
		{
			return AstSearcher.IsUsingDollarInput(this.Ast);
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x0011ADA8 File Offset: 0x00118FA8
		internal void InvokeWithPipeImpl(bool createLocalScope, Dictionary<string, ScriptBlock> functionsToDefine, List<PSVariable> variablesToDefine, ScriptBlock.ErrorHandlingBehavior errorHandlingBehavior, object dollarUnder, object input, object scriptThis, Pipe outputPipe, InvocationInfo invocationInfo, params object[] args)
		{
			this.InvokeWithPipeImpl(ScriptBlockClauseToInvoke.ProcessBlockOnly, createLocalScope, functionsToDefine, variablesToDefine, errorHandlingBehavior, dollarUnder, input, scriptThis, outputPipe, invocationInfo, args);
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x0011ADD0 File Offset: 0x00118FD0
		internal void InvokeWithPipeImpl(ScriptBlockClauseToInvoke clauseToInvoke, bool createLocalScope, Dictionary<string, ScriptBlock> functionsToDefine, List<PSVariable> variablesToDefine, ScriptBlock.ErrorHandlingBehavior errorHandlingBehavior, object dollarUnder, object input, object scriptThis, Pipe outputPipe, InvocationInfo invocationInfo, params object[] args)
		{
			if (clauseToInvoke == ScriptBlockClauseToInvoke.Begin && !this.HasBeginBlock)
			{
				return;
			}
			if (clauseToInvoke == ScriptBlockClauseToInvoke.Process && !this.HasProcessBlock)
			{
				return;
			}
			if (clauseToInvoke == ScriptBlockClauseToInvoke.End && !this.HasEndBlock)
			{
				return;
			}
			ExecutionContext contextFromTLS = this.GetContextFromTLS();
			if (contextFromTLS.CurrentPipelineStopping)
			{
				throw new PipelineStoppedException();
			}
			if (args == null)
			{
				args = new object[0];
			}
			bool createLocalScope2 = contextFromTLS._debuggingMode <= 0 && createLocalScope;
			Action<FunctionContext> codeToInvoke = this.GetCodeToInvoke(ref createLocalScope2, clauseToInvoke);
			if (codeToInvoke == null)
			{
				return;
			}
			if (outputPipe == null)
			{
				outputPipe = new Pipe
				{
					NullPipe = true
				};
			}
			MutableTuple mutableTuple = this.MakeLocalsTuple(createLocalScope2);
			if (dollarUnder != AutomationNull.Value)
			{
				mutableTuple.SetAutomaticVariable(AutomaticVariable.Underbar, dollarUnder, contextFromTLS);
			}
			if (input != AutomationNull.Value)
			{
				mutableTuple.SetAutomaticVariable(AutomaticVariable.Input, input, contextFromTLS);
			}
			if (scriptThis != AutomationNull.Value)
			{
				mutableTuple.SetAutomaticVariable(AutomaticVariable.This, scriptThis, contextFromTLS);
			}
			this.SetPSScriptRootAndPSCommandPath(mutableTuple, contextFromTLS);
			Pipe shellFunctionErrorOutputPipe = contextFromTLS.ShellFunctionErrorOutputPipe;
			PipelineWriter externalErrorOutput = contextFromTLS.ExternalErrorOutput;
			CommandOrigin scopeOrigin = contextFromTLS.EngineSessionState.CurrentScope.ScopeOrigin;
			SessionStateInternal engineSessionState = contextFromTLS.EngineSessionState;
			PSLanguageMode? pslanguageMode = null;
			PSLanguageMode? pslanguageMode2 = null;
			if (this.LanguageMode != null && this.LanguageMode != contextFromTLS.LanguageMode)
			{
				pslanguageMode = new PSLanguageMode?(contextFromTLS.LanguageMode);
				pslanguageMode2 = this.LanguageMode;
			}
			Dictionary<string, PSVariable> dictionary = null;
			try
			{
				InvocationInfo invocationInfo2 = invocationInfo;
				if (invocationInfo2 == null)
				{
					CallStackFrame callStackFrame = contextFromTLS.Debugger.GetCallStack().LastOrDefault<CallStackFrame>();
					IScriptExtent scriptPosition = (callStackFrame != null) ? callStackFrame.FunctionContext.CurrentPosition : this.Ast.Extent;
					invocationInfo2 = new InvocationInfo(null, scriptPosition, contextFromTLS);
				}
				mutableTuple.SetAutomaticVariable(AutomaticVariable.MyInvocation, invocationInfo2, contextFromTLS);
				if (this.SessionStateInternal != null)
				{
					contextFromTLS.EngineSessionState = this.SessionStateInternal;
				}
				switch (errorHandlingBehavior)
				{
				case ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe:
					contextFromTLS.ShellFunctionErrorOutputPipe = null;
					break;
				case ScriptBlock.ErrorHandlingBehavior.SwallowErrors:
					contextFromTLS.ShellFunctionErrorOutputPipe = null;
					contextFromTLS.ExternalErrorOutput = new DiscardingPipelineWriter();
					break;
				}
				if (createLocalScope)
				{
					SessionStateScope sessionStateScope = contextFromTLS.EngineSessionState.NewScope(false);
					contextFromTLS.EngineSessionState.CurrentScope = sessionStateScope;
					sessionStateScope.LocalsTuple = mutableTuple;
					if (functionsToDefine != null)
					{
						foreach (KeyValuePair<string, ScriptBlock> keyValuePair in functionsToDefine)
						{
							if (string.IsNullOrWhiteSpace(keyValuePair.Key))
							{
								PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.EmptyFunctionNameInFunctionDefinitionDictionary, new object[0]);
								ex.SetErrorId("EmptyFunctionNameInFunctionDefinitionDictionary");
								throw ex;
							}
							if (keyValuePair.Value == null)
							{
								PSInvalidOperationException ex2 = PSTraceSource.NewInvalidOperationException(ParserStrings.NullFunctionBodyInFunctionDefinitionDictionary, new object[]
								{
									keyValuePair.Key
								});
								ex2.SetErrorId("NullFunctionBodyInFunctionDefinitionDictionary");
								throw ex2;
							}
							sessionStateScope.FunctionTable.Add(keyValuePair.Key, new FunctionInfo(keyValuePair.Key, keyValuePair.Value, contextFromTLS));
						}
					}
					if (variablesToDefine == null)
					{
						goto IL_3AA;
					}
					int num = 0;
					using (List<PSVariable>.Enumerator enumerator2 = variablesToDefine.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							PSVariable psvariable = enumerator2.Current;
							if (psvariable == null)
							{
								PSInvalidOperationException ex3 = PSTraceSource.NewInvalidOperationException(ParserStrings.NullEntryInVariablesDefinitionList, new object[]
								{
									num
								});
								ex3.SetErrorId("NullEntryInVariablesDefinitionList");
								throw ex3;
							}
							string name = psvariable.Name;
							num++;
							sessionStateScope.Variables.Add(name, psvariable);
						}
						goto IL_3AA;
					}
				}
				if (contextFromTLS.EngineSessionState.CurrentScope.LocalsTuple == null)
				{
					contextFromTLS.EngineSessionState.CurrentScope.LocalsTuple = mutableTuple;
				}
				else
				{
					contextFromTLS.EngineSessionState.CurrentScope.DottedScopes.Push(mutableTuple);
					dictionary = new Dictionary<string, PSVariable>();
				}
				IL_3AA:
				if (pslanguageMode2 != null)
				{
					contextFromTLS.LanguageMode = pslanguageMode2.Value;
				}
				args = ScriptBlock.BindArgumentsForScripblockInvoke((RuntimeDefinedParameter[])this.RuntimeDefinedParameters.Data, args, contextFromTLS, !createLocalScope, dictionary, mutableTuple);
				mutableTuple.SetAutomaticVariable(AutomaticVariable.Args, args, contextFromTLS);
				contextFromTLS.EngineSessionState.CurrentScope.ScopeOrigin = CommandOrigin.Internal;
				FunctionContext obj = new FunctionContext
				{
					_executionContext = contextFromTLS,
					_outputPipe = outputPipe,
					_localsTuple = mutableTuple,
					_scriptBlock = this,
					_file = this.File,
					_debuggerHidden = this.DebuggerHidden,
					_debuggerStepThrough = this.DebuggerStepThrough,
					_sequencePoints = this.SequencePoints
				};
				ScriptBlock.LogScriptBlockStart(this, contextFromTLS.CurrentRunspace.InstanceId);
				try
				{
					codeToInvoke(obj);
				}
				finally
				{
					ScriptBlock.LogScriptBlockEnd(this, contextFromTLS.CurrentRunspace.InstanceId);
				}
			}
			catch (TargetInvocationException ex4)
			{
				throw ex4.InnerException;
			}
			finally
			{
				if (pslanguageMode != null)
				{
					contextFromTLS.LanguageMode = pslanguageMode.Value;
				}
				contextFromTLS.ShellFunctionErrorOutputPipe = shellFunctionErrorOutputPipe;
				contextFromTLS.ExternalErrorOutput = externalErrorOutput;
				contextFromTLS.EngineSessionState.CurrentScope.ScopeOrigin = scopeOrigin;
				if (createLocalScope)
				{
					contextFromTLS.EngineSessionState.RemoveScope(contextFromTLS.EngineSessionState.CurrentScope);
				}
				else if (dictionary != null)
				{
					contextFromTLS.EngineSessionState.CurrentScope.DottedScopes.Pop();
					foreach (KeyValuePair<string, PSVariable> keyValuePair2 in dictionary)
					{
						if (keyValuePair2.Value != null)
						{
							contextFromTLS.EngineSessionState.SetVariable(keyValuePair2.Value, false, CommandOrigin.Internal);
						}
						else
						{
							contextFromTLS.EngineSessionState.RemoveVariable(keyValuePair2.Key);
						}
					}
				}
				contextFromTLS.EngineSessionState = engineSessionState;
			}
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x0011B3E4 File Offset: 0x001195E4
		internal MutableTuple MakeLocalsTuple(bool createLocalScope)
		{
			MutableTuple result;
			if (createLocalScope)
			{
				result = MutableTuple.MakeTuple(this._scriptBlockData.LocalsMutableTupleType, this._scriptBlockData.NameToIndexMap);
			}
			else
			{
				result = (this.UsesCmdletBinding ? MutableTuple.MakeTuple(this._scriptBlockData.UnoptimizedLocalsMutableTupleType, Compiler.DottedScriptCmdletLocalsNameIndexMap) : MutableTuple.MakeTuple(this._scriptBlockData.UnoptimizedLocalsMutableTupleType, Compiler.DottedLocalsNameIndexMap));
			}
			return result;
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x0011B448 File Offset: 0x00119648
		internal static object[] BindArgumentsForScripblockInvoke(RuntimeDefinedParameter[] parameters, object[] args, ExecutionContext context, bool dotting, Dictionary<string, PSVariable> backupWhenDotting, MutableTuple locals)
		{
			CommandLineParameters commandLineParameters = new CommandLineParameters();
			if (parameters.Length == 0)
			{
				return args;
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				RuntimeDefinedParameter runtimeDefinedParameter = parameters[i];
				bool flag = false;
				object obj;
				if (i >= args.Length)
				{
					obj = runtimeDefinedParameter.Value;
					if (obj is Compiler.DefaultValueExpressionWrapper)
					{
						obj = ((Compiler.DefaultValueExpressionWrapper)obj).GetValue(context, null, null);
					}
					flag = true;
				}
				else
				{
					obj = args[i];
				}
				bool flag2 = false;
				if (dotting && backupWhenDotting != null)
				{
					backupWhenDotting[runtimeDefinedParameter.Name] = context.EngineSessionState.GetVariableAtScope(runtimeDefinedParameter.Name, "local");
				}
				else
				{
					flag2 = locals.TrySetParameter(runtimeDefinedParameter.Name, obj);
				}
				if (!flag2)
				{
					PSVariable variable = new PSVariable(runtimeDefinedParameter.Name, obj, ScopedItemOptions.None, runtimeDefinedParameter.Attributes);
					context.EngineSessionState.SetVariable(variable, false, CommandOrigin.Internal);
				}
				if (!flag)
				{
					commandLineParameters.Add(runtimeDefinedParameter.Name, obj);
					commandLineParameters.MarkAsBoundPositionally(runtimeDefinedParameter.Name);
				}
			}
			locals.SetAutomaticVariable(AutomaticVariable.PSBoundParameters, commandLineParameters.GetValueToBindToPSBoundParameters(), context);
			int num = args.Length - parameters.Length;
			if (num <= 0)
			{
				return ScriptBlock.EmptyArray;
			}
			object[] array = new object[num];
			Array.Copy(args, parameters.Length, array, 0, array.Length);
			return array;
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x0011B56D File Offset: 0x0011976D
		internal static void SetAutomaticVariable(AutomaticVariable variable, object value, MutableTuple locals)
		{
			locals.SetValue((int)variable, value);
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x0011B578 File Offset: 0x00119778
		private Action<FunctionContext> GetCodeToInvoke(ref bool optimized, ScriptBlockClauseToInvoke clauseToInvoke)
		{
			if (clauseToInvoke == ScriptBlockClauseToInvoke.ProcessBlockOnly && (this.HasBeginBlock || (this.HasEndBlock && this.HasProcessBlock)))
			{
				throw PSTraceSource.NewInvalidOperationException(AutomationExceptions.ScriptBlockInvokeOnOneClauseOnly, new object[0]);
			}
			optimized = this._scriptBlockData.Compile(optimized);
			if (optimized)
			{
				switch (clauseToInvoke)
				{
				case ScriptBlockClauseToInvoke.Begin:
					return this._scriptBlockData.BeginBlock;
				case ScriptBlockClauseToInvoke.Process:
					return this._scriptBlockData.ProcessBlock;
				case ScriptBlockClauseToInvoke.End:
					return this._scriptBlockData.EndBlock;
				default:
					if (!this.HasProcessBlock)
					{
						return this._scriptBlockData.EndBlock;
					}
					return this._scriptBlockData.ProcessBlock;
				}
			}
			else
			{
				switch (clauseToInvoke)
				{
				case ScriptBlockClauseToInvoke.Begin:
					return this._scriptBlockData.UnoptimizedBeginBlock;
				case ScriptBlockClauseToInvoke.Process:
					return this._scriptBlockData.UnoptimizedProcessBlock;
				case ScriptBlockClauseToInvoke.End:
					return this._scriptBlockData.UnoptimizedEndBlock;
				default:
					if (!this.HasProcessBlock)
					{
						return this._scriptBlockData.UnoptimizedEndBlock;
					}
					return this._scriptBlockData.UnoptimizedProcessBlock;
				}
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x0011B678 File Offset: 0x00119878
		internal CmdletBindingAttribute CmdletBindingAttribute
		{
			get
			{
				return this._scriptBlockData.CmdletBindingAttribute;
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060033F0 RID: 13296 RVA: 0x0011B685 File Offset: 0x00119885
		internal AliasAttribute AliasAttribute
		{
			get
			{
				return this._scriptBlockData.AliasAttribute;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x060033F1 RID: 13297 RVA: 0x0011B692 File Offset: 0x00119892
		internal ObsoleteAttribute ObsoleteAttribute
		{
			get
			{
				return this._scriptBlockData.ObsoleteAttribute;
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x0011B6A0 File Offset: 0x001198A0
		internal bool IsSingleFunctionDefinition(string functionName)
		{
			IParameterMetadataProvider astInternal = this.AstInternal;
			if (this.HasEndBlock && !this.HasDynamicParameters && !this.HasBeginBlock && !this.HasProcessBlock && astInternal.Body.EndBlock.Traps == null && astInternal.Body.ParamBlock == null && astInternal.Body.EndBlock.Statements.Count == 1)
			{
				FunctionDefinitionAst functionDefinitionAst = astInternal.Body.EndBlock.Statements[0] as FunctionDefinitionAst;
				if (functionDefinitionAst != null)
				{
					return functionDefinitionAst.Name.Equals(functionName, StringComparison.OrdinalIgnoreCase);
				}
			}
			return false;
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x0011B739 File Offset: 0x00119939
		internal bool Compile(bool optimized)
		{
			return this._scriptBlockData.Compile(optimized);
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x0011B748 File Offset: 0x00119948
		internal static void LogScriptBlockCreation(ScriptBlock scriptBlock, bool force)
		{
			if ((force || ScriptBlock.ShouldLogScriptBlockActivity("EnableScriptBlockLogging")) && (!scriptBlock.HasLogged || InternalTestHooks.ForceScriptBlockLogging))
			{
				if (ScriptBlock.ScriptBlockLoggingExplicitlyDisabled() || SecuritySupport.IsProductBinary(scriptBlock.File))
				{
					return;
				}
				string text = scriptBlock.Ast.Extent.Text;
				bool flag = false;
				if (text.Length < 20000)
				{
					flag = ScriptBlock.WriteScriptBlockToLog(scriptBlock, 0, 1, scriptBlock.Ast.Extent.Text);
				}
				else
				{
					int num = 10000 + new Random().Next(10000);
					int num2 = (int)Math.Floor((double)(text.Length / num)) + 1;
					for (int i = 0; i < num2; i++)
					{
						int num3 = i * num;
						int length = Math.Min(num, text.Length - num3);
						string textToLog = text.Substring(num3, length);
						flag = ScriptBlock.WriteScriptBlockToLog(scriptBlock, i, num2, textToLog);
					}
				}
				if (flag)
				{
					scriptBlock.HasLogged = true;
				}
			}
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x0011B840 File Offset: 0x00119A40
		private static bool WriteScriptBlockToLog(ScriptBlock scriptBlock, int segment, int segments, string textToLog)
		{
			Dictionary<string, object> groupPolicySetting = Utils.GetGroupPolicySetting("Software\\Policies\\Microsoft\\Windows\\EventLog", "ProtectedEventLogging", new RegistryKey[]
			{
				Registry.LocalMachine
			});
			if (groupPolicySetting != null)
			{
				lock (ScriptBlock.syncObject)
				{
					if (!ScriptBlock.GetAndValidateEncryptionRecipients(scriptBlock))
					{
						return false;
					}
					if (ScriptBlock.encryptionRecipients != null)
					{
						ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
						ErrorRecord errorRecord = null;
						byte[] bytes = Encoding.UTF8.GetBytes(textToLog);
						string text = CmsUtils.Encrypt(bytes, ScriptBlock.encryptionRecipients, executionContextFromTLS.SessionState, out errorRecord);
						if (errorRecord != null)
						{
							string text2 = StringUtil.Format(SecuritySupportStrings.CouldNotEncryptContent, textToLog, errorRecord.ToString());
							PSEtwLog.LogOperationalError(PSEventId.ScriptBlock_Compile_Detail, PSOpcode.Create, PSTask.ExecuteCommand, PSKeyword.UseAlwaysAnalytic, new object[]
							{
								0,
								0,
								text2,
								scriptBlock.Id.ToString(),
								scriptBlock.File ?? string.Empty
							});
						}
						else
						{
							textToLog = text;
						}
					}
				}
			}
			if (scriptBlock._scriptBlockData.HasSuspiciousContent)
			{
				PSEtwLog.LogOperationalWarning(PSEventId.ScriptBlock_Compile_Detail, PSOpcode.Create, PSTask.ExecuteCommand, PSKeyword.UseAlwaysAnalytic, new object[]
				{
					segment + 1,
					segments,
					textToLog,
					scriptBlock.Id.ToString(),
					scriptBlock.File ?? string.Empty
				});
			}
			else
			{
				PSEtwLog.LogOperationalVerbose(PSEventId.ScriptBlock_Compile_Detail, PSOpcode.Create, PSTask.ExecuteCommand, PSKeyword.UseAlwaysAnalytic, new object[]
				{
					segment + 1,
					segments,
					textToLog,
					scriptBlock.Id.ToString(),
					scriptBlock.File ?? string.Empty
				});
			}
			return true;
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x0011BA50 File Offset: 0x00119C50
		private static bool GetAndValidateEncryptionRecipients(ScriptBlock scriptBlock)
		{
			Dictionary<string, object> groupPolicySetting = Utils.GetGroupPolicySetting("Software\\Policies\\Microsoft\\Windows\\EventLog", "ProtectedEventLogging", new RegistryKey[]
			{
				Registry.LocalMachine
			});
			object obj = null;
			if (groupPolicySetting.TryGetValue("EnableProtectedEventLogging", out obj) && string.Equals("1", obj.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				object obj2 = null;
				if (groupPolicySetting.TryGetValue("EncryptionCertificate", out obj2))
				{
					ErrorRecord errorRecord = null;
					ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
					SessionState sessionState = null;
					if (executionContextFromTLS != null)
					{
						sessionState = executionContextFromTLS.SessionState;
					}
					if (sessionState == null)
					{
						return false;
					}
					string[] array = obj2 as string[];
					string text;
					if (array != null)
					{
						text = string.Join(Environment.NewLine, array);
					}
					else
					{
						text = (obj2 as string);
					}
					ScriptBlock.ResetCertificateCacheIfNeeded(text);
					if (ScriptBlock.encryptionRecipients != null)
					{
						return true;
					}
					if (ScriptBlock.hasProcessedCertificate)
					{
						return true;
					}
					CmsMessageRecipient cmsMessageRecipient = new CmsMessageRecipient(text);
					cmsMessageRecipient.Resolve(sessionState, ResolutionPurpose.Encryption, out errorRecord);
					ScriptBlock.hasProcessedCertificate = true;
					if (errorRecord != null)
					{
						string text2 = StringUtil.Format(SecuritySupportStrings.CouldNotUseCertificate, errorRecord.ToString());
						PSEtwLog.LogOperationalError(PSEventId.ScriptBlock_Compile_Detail, PSOpcode.Create, PSTask.ExecuteCommand, PSKeyword.UseAlwaysAnalytic, new object[]
						{
							0,
							0,
							text2,
							scriptBlock.Id.ToString(),
							scriptBlock.File ?? string.Empty
						});
						return true;
					}
					ScriptBlock.encryptionRecipients = new CmsMessageRecipient[]
					{
						cmsMessageRecipient
					};
					foreach (X509Certificate2 x509Certificate in cmsMessageRecipient.Certificates)
					{
						if (x509Certificate.HasPrivateKey)
						{
							string o = text;
							if (array != null && array.Length > 1)
							{
								o = array[1];
							}
							string text3 = StringUtil.Format(SecuritySupportStrings.CertificateContainsPrivateKey, o);
							PSEtwLog.LogOperationalError(PSEventId.ScriptBlock_Compile_Detail, PSOpcode.Create, PSTask.ExecuteCommand, PSKeyword.UseAlwaysAnalytic, new object[]
							{
								0,
								0,
								text3,
								scriptBlock.Id.ToString(),
								scriptBlock.File ?? string.Empty
							});
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x0011BC88 File Offset: 0x00119E88
		private static void ResetCertificateCacheIfNeeded(string certificate)
		{
			if (!string.Equals(ScriptBlock.lastSeenCertificate, certificate, StringComparison.Ordinal))
			{
				ScriptBlock.hasProcessedCertificate = false;
				ScriptBlock.lastSeenCertificate = certificate;
				ScriptBlock.encryptionRecipients = null;
			}
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x0011BCAC File Offset: 0x00119EAC
		private static bool ShouldLogScriptBlockActivity(string activity)
		{
			Dictionary<string, object> groupPolicySetting = Utils.GetGroupPolicySetting("ScriptBlockLogging", new RegistryKey[]
			{
				Registry.LocalMachine,
				Registry.CurrentUser
			});
			if (groupPolicySetting != null)
			{
				object obj = null;
				if (groupPolicySetting.TryGetValue(activity, out obj) && string.Equals("1", obj.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x0011BD48 File Offset: 0x00119F48
		internal static string CheckSuspiciousContent(Ast scriptBlockAst)
		{
			string text = scriptBlockAst.Extent.Text;
			IEnumerable<string> source = ScriptBlock.TokenizeWordElements(text);
			ParallelOptions parallelOptions = new ParallelOptions();
			string foundSignature = null;
			Parallel.ForEach<string>(source, parallelOptions, delegate(string element, ParallelLoopState loopState)
			{
				if (foundSignature == null && ScriptBlock.signatures.Contains(element))
				{
					foundSignature = element;
					loopState.Break();
				}
			});
			if (!string.IsNullOrEmpty(foundSignature))
			{
				return foundSignature;
			}
			if (!scriptBlockAst.HasSuspiciousContent)
			{
				return null;
			}
			Ast ast2 = scriptBlockAst.Find((Ast ast) => !ast.HasSuspiciousContent && ast.Parent.HasSuspiciousContent, true);
			if (ast2 != null)
			{
				return ast2.Parent.Extent.Text;
			}
			return scriptBlockAst.Extent.Text;
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x0011C058 File Offset: 0x0011A258
		private static IEnumerable<string> TokenizeWordElements(string scriptBlockText)
		{
			StringBuilder currentElement = new StringBuilder(100);
			foreach (char character in scriptBlockText)
			{
				if (character >= 'a' && character <= 'z')
				{
					currentElement.Append(character);
				}
				else if (character >= 'A' && character <= 'Z')
				{
					currentElement.Append(character);
				}
				else if (character == '-')
				{
					currentElement.Append(character);
				}
				else
				{
					if (currentElement.Length >= 4)
					{
						yield return currentElement.ToString();
					}
					currentElement.Clear();
				}
			}
			if (currentElement.Length > 0)
			{
				yield return currentElement.ToString();
			}
			yield break;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x0011C078 File Offset: 0x0011A278
		internal static bool ScriptBlockLoggingExplicitlyDisabled()
		{
			Dictionary<string, object> groupPolicySetting = Utils.GetGroupPolicySetting("ScriptBlockLogging", new RegistryKey[]
			{
				Registry.LocalMachine,
				Registry.CurrentUser
			});
			object obj;
			return groupPolicySetting != null && groupPolicySetting.TryGetValue("EnableScriptBlockLogging", out obj) && string.Equals("0", obj.ToString(), StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x0011C0D0 File Offset: 0x0011A2D0
		internal static void LogScriptBlockStart(ScriptBlock scriptBlock, Guid runspaceId)
		{
			bool force = false;
			if (scriptBlock._scriptBlockData.HasSuspiciousContent)
			{
				force = true;
			}
			ScriptBlock.LogScriptBlockCreation(scriptBlock, force);
			if (ScriptBlock.ShouldLogScriptBlockActivity("EnableScriptBlockInvocationLogging"))
			{
				PSEtwLog.LogOperationalVerbose(PSEventId.ScriptBlock_Invoke_Start_Detail, PSOpcode.Create, PSTask.CommandStart, PSKeyword.UseAlwaysAnalytic, new object[]
				{
					scriptBlock.Id.ToString(),
					runspaceId.ToString()
				});
			}
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x0011C148 File Offset: 0x0011A348
		internal static void LogScriptBlockEnd(ScriptBlock scriptBlock, Guid runspaceId)
		{
			if (ScriptBlock.ShouldLogScriptBlockActivity("EnableScriptBlockInvocationLogging"))
			{
				PSEtwLog.LogOperationalVerbose(PSEventId.ScriptBlock_Invoke_Complete_Detail, PSOpcode.Create, PSTask.CommandStop, PSKeyword.UseAlwaysAnalytic, new object[]
				{
					scriptBlock.Id.ToString(),
					runspaceId.ToString()
				});
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060033FE RID: 13310 RVA: 0x0011C1A7 File Offset: 0x0011A3A7
		internal CompiledScriptBlockData ScriptBlockData
		{
			get
			{
				return this._scriptBlockData;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060033FF RID: 13311 RVA: 0x0011C1AF File Offset: 0x0011A3AF
		public Ast Ast
		{
			get
			{
				return (Ast)this._scriptBlockData.Ast;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06003400 RID: 13312 RVA: 0x0011C1C1 File Offset: 0x0011A3C1
		internal IParameterMetadataProvider AstInternal
		{
			get
			{
				return this._scriptBlockData.Ast;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06003401 RID: 13313 RVA: 0x0011C1CE File Offset: 0x0011A3CE
		internal IScriptExtent[] SequencePoints
		{
			get
			{
				return this._scriptBlockData.SequencePoints;
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06003402 RID: 13314 RVA: 0x0011C1DB File Offset: 0x0011A3DB
		internal Action<FunctionContext> DynamicParamBlock
		{
			get
			{
				return this._scriptBlockData.DynamicParamBlock;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x0011C1E8 File Offset: 0x0011A3E8
		internal Action<FunctionContext> UnoptimizedDynamicParamBlock
		{
			get
			{
				return this._scriptBlockData.UnoptimizedDynamicParamBlock;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06003404 RID: 13316 RVA: 0x0011C1F5 File Offset: 0x0011A3F5
		internal Action<FunctionContext> BeginBlock
		{
			get
			{
				return this._scriptBlockData.BeginBlock;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x0011C202 File Offset: 0x0011A402
		internal Action<FunctionContext> UnoptimizedBeginBlock
		{
			get
			{
				return this._scriptBlockData.UnoptimizedBeginBlock;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06003406 RID: 13318 RVA: 0x0011C20F File Offset: 0x0011A40F
		internal Action<FunctionContext> ProcessBlock
		{
			get
			{
				return this._scriptBlockData.ProcessBlock;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06003407 RID: 13319 RVA: 0x0011C21C File Offset: 0x0011A41C
		internal Action<FunctionContext> UnoptimizedProcessBlock
		{
			get
			{
				return this._scriptBlockData.UnoptimizedProcessBlock;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06003408 RID: 13320 RVA: 0x0011C229 File Offset: 0x0011A429
		internal Action<FunctionContext> EndBlock
		{
			get
			{
				return this._scriptBlockData.EndBlock;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06003409 RID: 13321 RVA: 0x0011C236 File Offset: 0x0011A436
		internal Action<FunctionContext> UnoptimizedEndBlock
		{
			get
			{
				return this._scriptBlockData.UnoptimizedEndBlock;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x0600340A RID: 13322 RVA: 0x0011C243 File Offset: 0x0011A443
		internal Expression<Action<FunctionContext>> UnoptimizedDynamicParamBlockTree
		{
			get
			{
				return this._scriptBlockData.UnoptimizedDynamicParamBlockTree;
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x0600340B RID: 13323 RVA: 0x0011C250 File Offset: 0x0011A450
		internal Expression<Action<FunctionContext>> DynamicParamBlockTree
		{
			get
			{
				return this._scriptBlockData.DynamicParamBlockTree;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600340C RID: 13324 RVA: 0x0011C25D File Offset: 0x0011A45D
		internal Expression<Action<FunctionContext>> BeginBlockTree
		{
			get
			{
				return this._scriptBlockData.BeginBlockTree;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x0011C26A File Offset: 0x0011A46A
		internal Expression<Action<FunctionContext>> UnoptimizedBeginBlockTree
		{
			get
			{
				return this._scriptBlockData.UnoptimizedBeginBlockTree;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600340E RID: 13326 RVA: 0x0011C277 File Offset: 0x0011A477
		internal Expression<Action<FunctionContext>> ProcessBlockTree
		{
			get
			{
				return this._scriptBlockData.ProcessBlockTree;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x0011C284 File Offset: 0x0011A484
		internal Expression<Action<FunctionContext>> UnoptimizedProcessBlockTree
		{
			get
			{
				return this._scriptBlockData.UnoptimizedProcessBlockTree;
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06003410 RID: 13328 RVA: 0x0011C291 File Offset: 0x0011A491
		internal Expression<Action<FunctionContext>> EndBlockTree
		{
			get
			{
				return this._scriptBlockData.EndBlockTree;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06003411 RID: 13329 RVA: 0x0011C29E File Offset: 0x0011A49E
		internal Expression<Action<FunctionContext>> UnoptimizedEndBlockTree
		{
			get
			{
				return this._scriptBlockData.UnoptimizedEndBlockTree;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06003412 RID: 13330 RVA: 0x0011C2AB File Offset: 0x0011A4AB
		internal bool HasBeginBlock
		{
			get
			{
				return this.AstInternal.Body.BeginBlock != null;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06003413 RID: 13331 RVA: 0x0011C2C3 File Offset: 0x0011A4C3
		internal bool HasProcessBlock
		{
			get
			{
				return this.AstInternal.Body.ProcessBlock != null;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06003414 RID: 13332 RVA: 0x0011C2DB File Offset: 0x0011A4DB
		internal bool HasEndBlock
		{
			get
			{
				return this.AstInternal.Body.EndBlock != null;
			}
		}

		// Token: 0x04001AB9 RID: 6841
		private PSLanguageMode? languageMode;

		// Token: 0x04001ABA RID: 6842
		private static readonly ConditionalWeakTable<ScriptBlock, ConcurrentDictionary<Type, Delegate>> delegateTable = new ConditionalWeakTable<ScriptBlock, ConcurrentDictionary<Type, Delegate>>();

		// Token: 0x04001ABB RID: 6843
		private readonly bool _isFilter;

		// Token: 0x04001ABC RID: 6844
		private readonly CompiledScriptBlockData _scriptBlockData;

		// Token: 0x04001ABD RID: 6845
		private readonly bool _isConfiguration;

		// Token: 0x04001ABE RID: 6846
		private static readonly ConcurrentDictionary<Tuple<string, string>, ScriptBlock> _cachedScripts = new ConcurrentDictionary<Tuple<string, string>, ScriptBlock>();

		// Token: 0x04001ABF RID: 6847
		internal static readonly object[] EmptyArray = new object[0];

		// Token: 0x04001AC0 RID: 6848
		private static object syncObject = new object();

		// Token: 0x04001AC1 RID: 6849
		private static string lastSeenCertificate = string.Empty;

		// Token: 0x04001AC2 RID: 6850
		private static bool hasProcessedCertificate = false;

		// Token: 0x04001AC3 RID: 6851
		private static CmsMessageRecipient[] encryptionRecipients = null;

		// Token: 0x04001AC4 RID: 6852
		private static HashSet<string> signatures = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"Add-Type",
			"DllImport",
			"DefineDynamicAssembly",
			"DefineDynamicModule",
			"DefineType",
			"DefineConstructor",
			"CreateType",
			"DefineLiteral",
			"DefineEnum",
			"DefineField",
			"ILGenerator",
			"Emit",
			"UnverifiableCodeAttribute",
			"DefinePInvokeMethod",
			"GetTypes",
			"GetAssemblies",
			"Methods",
			"Properties",
			"GetConstructor",
			"GetConstructors",
			"GetDefaultMembers",
			"GetEvent",
			"GetEvents",
			"GetField",
			"GetFields",
			"GetInterface",
			"GetInterfaceMap",
			"GetInterfaces",
			"GetMember",
			"GetMembers",
			"GetMethod",
			"GetMethods",
			"GetNestedType",
			"GetNestedTypes",
			"GetProperties",
			"GetProperty",
			"InvokeMember",
			"MakeArrayType",
			"MakeByRefType",
			"MakeGenericType",
			"MakePointerType",
			"DeclaringMethod",
			"DeclaringType",
			"ReflectedType",
			"TypeHandle",
			"TypeInitializer",
			"UnderlyingSystemType",
			"InteropServices",
			"Marshal",
			"AllocHGlobal",
			"PtrToStructure",
			"StructureToPtr",
			"FreeHGlobal",
			"IntPtr",
			"MemoryStream",
			"DeflateStream",
			"FromBase64String",
			"EncodedCommand",
			"Bypass",
			"ToBase64String",
			"ExpandString",
			"GetPowerShell",
			"OpenProcess",
			"VirtualAlloc",
			"VirtualFree",
			"WriteProcessMemory",
			"CreateUserThread",
			"CloseHandle",
			"GetDelegateForFunctionPointer",
			"kernel32",
			"CreateThread",
			"memcpy",
			"LoadLibrary",
			"GetModuleHandle",
			"GetProcAddress",
			"VirtualProtect",
			"FreeLibrary",
			"ReadProcessMemory",
			"CreateRemoteThread",
			"AdjustTokenPrivileges",
			"WriteByte",
			"WriteInt32",
			"OpenThreadToken",
			"PtrToString",
			"FreeHGlobal",
			"ZeroFreeGlobalAllocUnicode",
			"OpenProcessToken",
			"GetTokenInformation",
			"SetThreadToken",
			"ImpersonateLoggedOnUser",
			"RevertToSelf",
			"GetLogonSessionData",
			"CreateProcessWithToken",
			"DuplicateTokenEx",
			"OpenWindowStation",
			"OpenDesktop",
			"MiniDumpWriteDump",
			"AddSecurityPackage",
			"EnumerateSecurityPackages",
			"GetProcessHandle",
			"DangerousGetHandle",
			"CryptoServiceProvider",
			"Cryptography",
			"RijndaelManaged",
			"SHA1Managed",
			"CryptoStream",
			"CreateEncryptor",
			"CreateDecryptor",
			"TransformFinalBlock",
			"DeviceIoControl",
			"SetInformationProcess",
			"PasswordDeriveBytes",
			"GetAsyncKeyState",
			"GetKeyboardState",
			"GetForegroundWindow",
			"BindingFlags",
			"NonPublic",
			"ScriptBlockLogging",
			"LogPipelineExecutionDetails",
			"ProtectedEventLogging"
		};

		// Token: 0x0200048F RID: 1167
		internal enum ErrorHandlingBehavior
		{
			// Token: 0x04001AD1 RID: 6865
			WriteToCurrentErrorPipe = 1,
			// Token: 0x04001AD2 RID: 6866
			WriteToExternalErrorPipe,
			// Token: 0x04001AD3 RID: 6867
			SwallowErrors
		}
	}
}
