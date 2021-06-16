using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Automation
{
	// Token: 0x02000075 RID: 117
	[DebuggerDisplay("Command = {command}")]
	internal abstract class ParameterBinderBase
	{
		// Token: 0x0600062A RID: 1578 RVA: 0x0001D108 File Offset: 0x0001B308
		internal ParameterBinderBase(object target, InvocationInfo invocationInfo, ExecutionContext context, InternalCommand command)
		{
			ParameterBinderBase.bindingTracer.ShowHeaders = false;
			this.command = command;
			this.target = target;
			this.invocationInfo = invocationInfo;
			this.context = context;
			this.engine = context.EngineIntrinsics;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001D156 File Offset: 0x0001B356
		internal ParameterBinderBase(InvocationInfo invocationInfo, ExecutionContext context, InternalCommand command)
		{
			ParameterBinderBase.bindingTracer.ShowHeaders = false;
			this.command = command;
			this.invocationInfo = invocationInfo;
			this.context = context;
			this.engine = context.EngineIntrinsics;
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001D191 File Offset: 0x0001B391
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x0001D199 File Offset: 0x0001B399
		internal object Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0001D1AB File Offset: 0x0001B3AB
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x0001D1A2 File Offset: 0x0001B3A2
		internal CommandLineParameters CommandLineParameters
		{
			get
			{
				if (this.commandLineParameters == null)
				{
					this.commandLineParameters = new CommandLineParameters();
				}
				return this.commandLineParameters;
			}
			set
			{
				this.commandLineParameters = value;
			}
		}

		// Token: 0x06000630 RID: 1584
		internal abstract object GetDefaultParameterValue(string name);

		// Token: 0x06000631 RID: 1585
		internal abstract void BindParameter(string name, object value);

		// Token: 0x06000632 RID: 1586 RVA: 0x0001D1C8 File Offset: 0x0001B3C8
		private void ValidatePSTypeName(CommandParameterInternal parameter, CompiledCommandParameter parameterMetadata, bool retryOtherBindingAfterFailure, object parameterValue)
		{
			if (parameterValue == null)
			{
				return;
			}
			IEnumerable<string> internalTypeNames = PSObject.AsPSObject(parameterValue).InternalTypeNames;
			string pstypeName = parameterMetadata.PSTypeName;
			if (!internalTypeNames.Contains(pstypeName, StringComparer.OrdinalIgnoreCase))
			{
				PSInvalidCastException innerException = new PSInvalidCastException(ErrorCategory.InvalidArgument.ToString(), null, ParameterBinderStrings.MismatchedPSTypeName, new object[]
				{
					(this.invocationInfo != null && this.invocationInfo.MyCommand != null) ? this.invocationInfo.MyCommand.Name : string.Empty,
					parameterMetadata.Name,
					parameterMetadata.Type,
					parameterValue.GetType(),
					0,
					0,
					pstypeName
				});
				ParameterBindingException ex;
				if (!retryOtherBindingAfterFailure)
				{
					ex = new ParameterBindingArgumentTransformationException(innerException, ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(parameter), parameterMetadata.Name, parameterMetadata.Type, parameterValue.GetType(), ParameterBinderStrings.MismatchedPSTypeName, "MismatchedPSTypeName", new object[]
					{
						pstypeName
					});
				}
				else
				{
					ex = new ParameterBindingException(innerException, ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(parameter), parameterMetadata.Name, parameterMetadata.Type, parameterValue.GetType(), ParameterBinderStrings.MismatchedPSTypeName, "MismatchedPSTypeName", new object[]
					{
						pstypeName
					});
				}
				throw ex;
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001D314 File Offset: 0x0001B514
		internal virtual bool BindParameter(CommandParameterInternal parameter, CompiledCommandParameter parameterMetadata, ParameterBindingFlags flags)
		{
			bool flag = false;
			bool flag2 = (flags & ParameterBindingFlags.ShouldCoerceType) != ParameterBindingFlags.None;
			bool flag3 = (flags & ParameterBindingFlags.IsDefaultValue) != ParameterBindingFlags.None;
			if (parameter == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameter");
			}
			if (parameterMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterMetadata");
			}
			bool result;
			using (ParameterBinderBase.bindingTracer.TraceScope("BIND arg [{0}] to parameter [{1}]", new object[]
			{
				parameter.ArgumentValue,
				parameterMetadata.Name
			}))
			{
				parameter.ParameterName = parameterMetadata.Name;
				object obj = parameter.ArgumentValue;
				ScriptParameterBinder scriptParameterBinder = this as ScriptParameterBinder;
				bool flag4 = false;
				if (scriptParameterBinder != null)
				{
					flag4 = scriptParameterBinder.Script.UsesCmdletBinding;
				}
				foreach (ArgumentTransformationAttribute argumentTransformationAttribute in parameterMetadata.ArgumentTransformationAttributes)
				{
					using (ParameterBinderBase.bindingTracer.TraceScope("Executing DATA GENERATION metadata: [{0}]", new object[]
					{
						argumentTransformationAttribute.GetType()
					}))
					{
						try
						{
							ArgumentTypeConverterAttribute argumentTypeConverterAttribute = argumentTransformationAttribute as ArgumentTypeConverterAttribute;
							if (argumentTypeConverterAttribute != null)
							{
								if (flag2)
								{
									obj = argumentTypeConverterAttribute.Transform(this.engine, obj, true, flag4);
								}
							}
							else if (obj != null || (!flag3 && (ParameterBinderBase.IsParameterMandatory(parameterMetadata) || ParameterBinderBase.ParameterCannotBeNull(parameterMetadata) || argumentTransformationAttribute.TransformNullOptionalParameters)))
							{
								obj = argumentTransformationAttribute.Transform(this.engine, obj);
							}
							ParameterBinderBase.bindingTracer.WriteLine("result returned from DATA GENERATION: {0}", new object[]
							{
								obj
							});
						}
						catch (Exception ex)
						{
							CommandProcessorBase.CheckForSevereException(ex);
							ParameterBinderBase.bindingTracer.WriteLine("ERROR: DATA GENERATION: {0}", new object[]
							{
								ex.Message
							});
							ParameterBindingException ex2 = new ParameterBindingArgumentTransformationException(ex, ErrorCategory.InvalidData, this.InvocationInfo, this.GetErrorExtent(parameter), parameterMetadata.Name, parameterMetadata.Type, (obj == null) ? null : obj.GetType(), ParameterBinderStrings.ParameterArgumentTransformationError, "ParameterArgumentTransformationError", new object[]
							{
								ex.Message
							});
							throw ex2;
						}
					}
				}
				if (flag2)
				{
					obj = this.CoerceTypeAsNeeded(parameter, parameterMetadata.Name, parameterMetadata.Type, parameterMetadata.CollectionTypeInformation, obj);
				}
				else if (!this.ShouldContinueUncoercedBind(parameter, parameterMetadata, flags, ref obj))
				{
					goto IL_4A9;
				}
				if (parameterMetadata.PSTypeName != null && obj != null)
				{
					IEnumerable enumerable = LanguagePrimitives.GetEnumerable(obj);
					if (enumerable != null)
					{
						using (IEnumerator enumerator2 = enumerable.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object parameterValue = enumerator2.Current;
								this.ValidatePSTypeName(parameter, parameterMetadata, !flag2, parameterValue);
							}
							goto IL_280;
						}
					}
					this.ValidatePSTypeName(parameter, parameterMetadata, !flag2, obj);
				}
				IL_280:
				if (!flag3)
				{
					foreach (ValidateArgumentsAttribute validateArgumentsAttribute in parameterMetadata.ValidationAttributes)
					{
						using (ParameterBinderBase.bindingTracer.TraceScope("Executing VALIDATION metadata: [{0}]", new object[]
						{
							validateArgumentsAttribute.GetType()
						}))
						{
							try
							{
								validateArgumentsAttribute.InternalValidate(obj, this.engine);
							}
							catch (Exception ex3)
							{
								CommandProcessorBase.CheckForSevereException(ex3);
								ParameterBinderBase.bindingTracer.WriteLine("ERROR: VALIDATION FAILED: {0}", new object[]
								{
									ex3.Message
								});
								ParameterBindingValidationException ex4 = new ParameterBindingValidationException(ex3, ErrorCategory.InvalidData, this.InvocationInfo, this.GetErrorExtent(parameter), parameterMetadata.Name, parameterMetadata.Type, (obj == null) ? null : obj.GetType(), ParameterBinderStrings.ParameterArgumentValidationError, "ParameterArgumentValidationError", new object[]
								{
									ex3.Message
								});
								throw ex4;
							}
							ParameterBinderBase.tracer.WriteLine("Validation attribute on {0} returned {1}.", new object[]
							{
								parameterMetadata.Name,
								flag
							});
						}
					}
					if (ParameterBinderBase.IsParameterMandatory(parameterMetadata))
					{
						this.ValidateNullOrEmptyArgument(parameter, parameterMetadata, parameterMetadata.Type, obj, true);
					}
				}
				if (parameterMetadata.ObsoleteAttribute != null && !flag3 && scriptParameterBinder != null && !flag4)
				{
					string message = string.Format(CultureInfo.InvariantCulture, ParameterBinderStrings.UseOfDeprecatedParameterWarning, new object[]
					{
						parameterMetadata.Name,
						parameterMetadata.ObsoleteAttribute.Message
					});
					MshCommandRuntime mshCommandRuntime = this.Command.commandRuntime as MshCommandRuntime;
					if (mshCommandRuntime != null)
					{
						mshCommandRuntime.WriteWarning(new WarningRecord("ParameterObsolete", message), false);
					}
				}
				Exception ex5 = null;
				try
				{
					this.BindParameter(parameter.ParameterName, obj);
					flag = true;
				}
				catch (SetValueException ex6)
				{
					ex5 = ex6;
				}
				if (ex5 != null)
				{
					Type typeSpecified = (obj == null) ? null : obj.GetType();
					ParameterBindingException ex7 = new ParameterBindingException(ex5, ErrorCategory.WriteError, this.InvocationInfo, this.GetErrorExtent(parameter), parameterMetadata.Name, parameterMetadata.Type, typeSpecified, ParameterBinderStrings.ParameterBindingFailed, "ParameterBindingFailed", new object[]
					{
						ex5.Message
					});
					throw ex7;
				}
				IL_4A9:
				ParameterBinderBase.bindingTracer.WriteLine("BIND arg [{0}] to param [{1}] {2}", new object[]
				{
					obj,
					parameter.ParameterName,
					flag ? "SUCCESSFUL" : "SKIPPED"
				});
				if (flag)
				{
					if (this.RecordBoundParameters)
					{
						this.CommandLineParameters.Add(parameter.ParameterName, obj);
					}
					MshCommandRuntime mshCommandRuntime2 = this.Command.commandRuntime as MshCommandRuntime;
					if (mshCommandRuntime2 != null && (mshCommandRuntime2.LogPipelineExecutionDetail || this.Context.EngineHostInterface.UI.IsTranscribing) && mshCommandRuntime2.PipelineProcessor != null)
					{
						IEnumerable enumerable2 = LanguagePrimitives.GetEnumerable(obj);
						if (enumerable2 != null)
						{
							string parameterValue2 = string.Join(", ", enumerable2.Cast<object>().ToArray<object>());
							mshCommandRuntime2.PipelineProcessor.LogExecutionParameterBinding(this.InvocationInfo, parameter.ParameterName, parameterValue2);
						}
						else
						{
							string parameterValue3 = "";
							if (obj != null)
							{
								try
								{
									parameterValue3 = obj.ToString();
								}
								catch (Exception e)
								{
									CommandProcessorBase.CheckForSevereException(e);
								}
							}
							mshCommandRuntime2.PipelineProcessor.LogExecutionParameterBinding(this.InvocationInfo, parameter.ParameterName, parameterValue3);
						}
					}
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001D9EC File Offset: 0x0001BBEC
		private void ValidateNullOrEmptyArgument(CommandParameterInternal parameter, CompiledCommandParameter parameterMetadata, Type argumentType, object parameterValue, bool recurseIntoCollections)
		{
			if (parameterValue == null && argumentType != typeof(bool?))
			{
				if (!parameterMetadata.AllowsNullArgument)
				{
					ParameterBinderBase.bindingTracer.WriteLine("ERROR: Argument cannot be null", new object[0]);
					ParameterBindingValidationException ex = new ParameterBindingValidationException(ErrorCategory.InvalidData, this.InvocationInfo, this.GetErrorExtent(parameter), parameterMetadata.Name, argumentType, null, ParameterBinderStrings.ParameterArgumentValidationErrorNullNotAllowed, "ParameterArgumentValidationErrorNullNotAllowed", new object[0]);
					throw ex;
				}
				return;
			}
			else if (argumentType == typeof(string))
			{
				string text = parameterValue as string;
				if (text.Length == 0 && !parameterMetadata.AllowsEmptyStringArgument)
				{
					ParameterBinderBase.bindingTracer.WriteLine("ERROR: Argument cannot be an empty string", new object[0]);
					ParameterBindingValidationException ex2 = new ParameterBindingValidationException(ErrorCategory.InvalidData, this.InvocationInfo, this.GetErrorExtent(parameter), parameterMetadata.Name, parameterMetadata.Type, (parameterValue == null) ? null : parameterValue.GetType(), ParameterBinderStrings.ParameterArgumentValidationErrorEmptyStringNotAllowed, "ParameterArgumentValidationErrorEmptyStringNotAllowed", new object[0]);
					throw ex2;
				}
				return;
			}
			else
			{
				if (!recurseIntoCollections)
				{
					return;
				}
				switch (parameterMetadata.CollectionTypeInformation.ParameterCollectionType)
				{
				case ParameterCollectionType.IList:
				case ParameterCollectionType.Array:
				case ParameterCollectionType.ICollectionGeneric:
				{
					IEnumerator enumerator = LanguagePrimitives.GetEnumerator(parameterValue);
					bool flag = true;
					while (ParserOps.MoveNext(null, null, enumerator))
					{
						object parameterValue2 = ParserOps.Current(null, enumerator);
						flag = false;
						this.ValidateNullOrEmptyArgument(parameter, parameterMetadata, parameterMetadata.CollectionTypeInformation.ElementType, parameterValue2, false);
					}
					if (flag && !parameterMetadata.AllowsEmptyCollectionArgument)
					{
						ParameterBinderBase.bindingTracer.WriteLine("ERROR: Argument cannot be an empty collection", new object[0]);
						string errorId;
						string resourceString;
						if (parameterMetadata.CollectionTypeInformation.ParameterCollectionType == ParameterCollectionType.Array)
						{
							errorId = "ParameterArgumentValidationErrorEmptyArrayNotAllowed";
							resourceString = ParameterBinderStrings.ParameterArgumentValidationErrorEmptyArrayNotAllowed;
						}
						else
						{
							errorId = "ParameterArgumentValidationErrorEmptyCollectionNotAllowed";
							resourceString = ParameterBinderStrings.ParameterArgumentValidationErrorEmptyCollectionNotAllowed;
						}
						ParameterBindingValidationException ex3 = new ParameterBindingValidationException(ErrorCategory.InvalidData, this.InvocationInfo, this.GetErrorExtent(parameter), parameterMetadata.Name, parameterMetadata.Type, (parameterValue == null) ? null : parameterValue.GetType(), resourceString, errorId, new object[0]);
						throw ex3;
					}
					return;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001DBC8 File Offset: 0x0001BDC8
		private static bool IsParameterMandatory(CompiledCommandParameter parameterMetadata)
		{
			bool flag = false;
			foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in parameterMetadata.ParameterSetData.Values)
			{
				if (parameterSetSpecificMetadata.IsMandatory)
				{
					flag = true;
					break;
				}
			}
			ParameterBinderBase.tracer.WriteLine("isMandatory = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001DC48 File Offset: 0x0001BE48
		private static bool ParameterCannotBeNull(CompiledCommandParameter parameterMetadata)
		{
			bool result = false;
			foreach (ValidateArgumentsAttribute validateArgumentsAttribute in parameterMetadata.ValidationAttributes)
			{
				if (validateArgumentsAttribute is ValidateNotNullAttribute || validateArgumentsAttribute is ValidateNotNullOrEmptyAttribute)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
		private bool ShouldContinueUncoercedBind(CommandParameterInternal parameter, CompiledCommandParameter parameterMetadata, ParameterBindingFlags flags, ref object parameterValue)
		{
			bool result = false;
			bool flag = (flags & ParameterBindingFlags.IsDefaultValue) != ParameterBindingFlags.None;
			Type type = parameterMetadata.Type;
			if (parameterValue == null)
			{
				result = (type == null || flag || (!type.GetTypeInfo().IsValueType && type != typeof(string)));
			}
			else
			{
				Type type2 = parameterValue.GetType();
				if (type2 == type || type2.IsSubclassOf(type) || type.IsAssignableFrom(type2))
				{
					result = true;
				}
				else
				{
					PSObject psobject = parameterValue as PSObject;
					if (psobject != null && !psobject.immediateBaseObjectIsEmpty)
					{
						parameterValue = psobject.BaseObject;
						type2 = parameterValue.GetType();
						if (type2 == type || type2.IsSubclassOf(type) || type.IsAssignableFrom(type2))
						{
							return true;
						}
					}
					if (parameterMetadata.CollectionTypeInformation.ParameterCollectionType != ParameterCollectionType.NotCollection)
					{
						bool flag2 = false;
						object obj = this.EncodeCollection(parameter, parameterMetadata.Name, parameterMetadata.CollectionTypeInformation, type, parameterValue, false, out flag2);
						if (obj != null && !flag2)
						{
							parameterValue = obj;
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001DDAE File Offset: 0x0001BFAE
		internal InvocationInfo InvocationInfo
		{
			get
			{
				return this.invocationInfo;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001DDB6 File Offset: 0x0001BFB6
		internal ExecutionContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0001DDBE File Offset: 0x0001BFBE
		internal InternalCommand Command
		{
			get
			{
				return this.command;
			}
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001DDC8 File Offset: 0x0001BFC8
		private object CoerceTypeAsNeeded(CommandParameterInternal argument, string parameterName, Type toType, ParameterCollectionTypeInformation collectionTypeInfo, object currentValue)
		{
			if (argument == null)
			{
				throw PSTraceSource.NewArgumentNullException("argument");
			}
			if (toType == null)
			{
				throw PSTraceSource.NewArgumentNullException("toType");
			}
			if (collectionTypeInfo == null)
			{
				collectionTypeInfo = new ParameterCollectionTypeInformation(toType);
			}
			object obj = currentValue;
			using (ParameterBinderBase.bindingTracer.TraceScope("COERCE arg to [{0}]", new object[]
			{
				toType
			}))
			{
				Type type = null;
				try
				{
					if (ParameterBinderBase.IsNullParameterValue(currentValue))
					{
						obj = this.HandleNullParameterForSpecialTypes(argument, parameterName, toType, currentValue);
					}
					else
					{
						type = currentValue.GetType();
						if (toType.IsAssignableFrom(type))
						{
							ParameterBinderBase.bindingTracer.WriteLine("Parameter and arg types the same, no coercion is needed.", new object[0]);
							obj = currentValue;
						}
						else
						{
							ParameterBinderBase.bindingTracer.WriteLine("Trying to convert argument value from {0} to {1}", new object[]
							{
								type,
								toType
							});
							if (toType == typeof(PSObject))
							{
								if (this.command != null && currentValue == this.command.CurrentPipelineObject.BaseObject)
								{
									currentValue = this.command.CurrentPipelineObject;
								}
								ParameterBinderBase.bindingTracer.WriteLine("The parameter is of type [{0}] and the argument is an PSObject, so the parameter value is the argument value wrapped into an PSObject.", new object[]
								{
									toType
								});
								obj = LanguagePrimitives.AsPSObjectOrNull(currentValue);
							}
							else
							{
								if (toType == typeof(string) && type == typeof(PSObject))
								{
									PSObject psobject = (PSObject)currentValue;
									if (psobject == AutomationNull.Value)
									{
										ParameterBinderBase.bindingTracer.WriteLine("CONVERT a null PSObject to a null string.", new object[0]);
										obj = null;
										goto IL_59A;
									}
								}
								if (toType == typeof(bool) || toType == typeof(SwitchParameter) || toType == typeof(bool?))
								{
									Type type2;
									if (type == typeof(PSObject))
									{
										PSObject psobject2 = (PSObject)currentValue;
										currentValue = psobject2.BaseObject;
										if (currentValue is SwitchParameter)
										{
											currentValue = ((SwitchParameter)currentValue).IsPresent;
										}
										type2 = currentValue.GetType();
									}
									else
									{
										type2 = type;
									}
									if (type2 == typeof(bool))
									{
										if (LanguagePrimitives.IsBooleanType(toType))
										{
											obj = ParserOps.BoolToObject((bool)currentValue);
										}
										else
										{
											obj = new SwitchParameter((bool)currentValue);
										}
									}
									else if (type2 == typeof(int))
									{
										if ((int)LanguagePrimitives.ConvertTo(currentValue, typeof(int), CultureInfo.InvariantCulture) != 0)
										{
											if (LanguagePrimitives.IsBooleanType(toType))
											{
												obj = ParserOps.BoolToObject(true);
											}
											else
											{
												obj = new SwitchParameter(true);
											}
										}
										else if (LanguagePrimitives.IsBooleanType(toType))
										{
											obj = ParserOps.BoolToObject(false);
										}
										else
										{
											obj = new SwitchParameter(false);
										}
									}
									else
									{
										if (!LanguagePrimitives.IsNumeric(type2.GetTypeCode()))
										{
											ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(argument), parameterName, toType, type, ParameterBinderStrings.CannotConvertArgument, "CannotConvertArgument", new object[]
											{
												type2,
												""
											});
											throw ex;
										}
										double num = (double)LanguagePrimitives.ConvertTo(currentValue, typeof(double), CultureInfo.InvariantCulture);
										if (num != 0.0)
										{
											if (LanguagePrimitives.IsBooleanType(toType))
											{
												obj = ParserOps.BoolToObject(true);
											}
											else
											{
												obj = new SwitchParameter(true);
											}
										}
										else if (LanguagePrimitives.IsBooleanType(toType))
										{
											obj = ParserOps.BoolToObject(false);
										}
										else
										{
											obj = new SwitchParameter(false);
										}
									}
								}
								else
								{
									if (collectionTypeInfo.ParameterCollectionType == ParameterCollectionType.ICollectionGeneric || collectionTypeInfo.ParameterCollectionType == ParameterCollectionType.IList)
									{
										object obj2 = PSObject.Base(currentValue);
										if (obj2 != null)
										{
											ConversionRank conversionRank = LanguagePrimitives.GetConversionRank(obj2.GetType(), toType);
											if ((conversionRank == ConversionRank.Constructor || conversionRank == ConversionRank.ImplicitCast || conversionRank == ConversionRank.ExplicitCast) && LanguagePrimitives.TryConvertTo(currentValue, toType, CultureInfo.CurrentCulture, out obj))
											{
												goto IL_59A;
											}
										}
									}
									if (collectionTypeInfo.ParameterCollectionType != ParameterCollectionType.NotCollection)
									{
										ParameterBinderBase.bindingTracer.WriteLine("ENCODING arg into collection", new object[0]);
										bool flag = false;
										obj = this.EncodeCollection(argument, parameterName, collectionTypeInfo, toType, currentValue, collectionTypeInfo.ElementType != null, out flag);
									}
									else
									{
										TypeInfo typeInfo = toType.GetTypeInfo();
										if (ParameterBinderBase.GetIList(currentValue) != null && toType != typeof(object) && toType != typeof(PSObject) && toType != typeof(PSListModifier) && (!typeInfo.IsGenericType || typeInfo.GetGenericTypeDefinition() != typeof(PSListModifier<>)) && (!typeInfo.IsGenericType || typeInfo.GetGenericTypeDefinition() != typeof(FlagsExpression<>)) && !typeInfo.IsEnum)
										{
											throw new NotSupportedException();
										}
										ParameterBinderBase.bindingTracer.WriteLine("CONVERT arg type to param type using LanguagePrimitives.ConvertTo", new object[0]);
										bool flag2 = false;
										if (this.context.LanguageMode == PSLanguageMode.ConstrainedLanguage)
										{
											object obj3 = PSObject.Base(currentValue);
											bool flag3 = obj3 is PSObject;
											bool flag4 = obj3 != null && typeof(IDictionary).IsAssignableFrom(obj3.GetType());
											flag2 = (this.Command.CommandInfo.DefiningLanguageMode == PSLanguageMode.FullLanguage && !flag3 && !flag4);
										}
										try
										{
											if (flag2)
											{
												this.context.LanguageMode = PSLanguageMode.FullLanguage;
											}
											obj = LanguagePrimitives.ConvertTo(currentValue, toType, CultureInfo.CurrentCulture);
										}
										finally
										{
											if (flag2)
											{
												this.context.LanguageMode = PSLanguageMode.ConstrainedLanguage;
											}
										}
										ParameterBinderBase.bindingTracer.WriteLine("CONVERT SUCCESSFUL using LanguagePrimitives.ConvertTo: [{0}]", new object[]
										{
											(obj == null) ? "null" : obj.ToString()
										});
									}
								}
							}
						}
					}
					IL_59A:;
				}
				catch (NotSupportedException ex2)
				{
					ParameterBinderBase.bindingTracer.TraceError("ERROR: COERCE FAILED: arg [{0}] could not be converted to the parameter type [{1}]", new object[]
					{
						(obj == null) ? "null" : obj,
						toType
					});
					ParameterBindingException ex3 = new ParameterBindingException(ex2, ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(argument), parameterName, toType, type, ParameterBinderStrings.CannotConvertArgument, "CannotConvertArgument", new object[]
					{
						(obj == null) ? "null" : obj,
						ex2.Message
					});
					throw ex3;
				}
				catch (PSInvalidCastException ex4)
				{
					ParameterBinderBase.bindingTracer.TraceError("ERROR: COERCE FAILED: arg [{0}] could not be converted to the parameter type [{1}]", new object[]
					{
						obj ?? "null",
						toType
					});
					ParameterBindingException ex5 = new ParameterBindingException(ex4, ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(argument), parameterName, toType, type, ParameterBinderStrings.CannotConvertArgumentNoMessage, "CannotConvertArgumentNoMessage", new object[]
					{
						ex4.Message
					});
					throw ex5;
				}
			}
			return obj;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001E4CC File Offset: 0x0001C6CC
		private static bool IsNullParameterValue(object currentValue)
		{
			bool result = false;
			if (currentValue == null || currentValue == AutomationNull.Value || currentValue == UnboundParameter.Value)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001E4F4 File Offset: 0x0001C6F4
		private object HandleNullParameterForSpecialTypes(CommandParameterInternal argument, string parameterName, Type toType, object currentValue)
		{
			if (toType == typeof(bool))
			{
				ParameterBinderBase.bindingTracer.WriteLine("ERROR: No argument is specified for parameter and parameter type is BOOL", new object[0]);
				ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(argument), parameterName, toType, null, ParameterBinderStrings.ParameterArgumentValidationErrorNullNotAllowed, "ParameterArgumentValidationErrorNullNotAllowed", new object[]
				{
					""
				});
				throw ex;
			}
			object result;
			if (toType == typeof(SwitchParameter))
			{
				ParameterBinderBase.bindingTracer.WriteLine("Arg is null or not present, parameter type is SWITCHPARAMTER, value is true.", new object[0]);
				result = SwitchParameter.Present;
			}
			else
			{
				if (currentValue == UnboundParameter.Value)
				{
					ParameterBinderBase.bindingTracer.TraceError("ERROR: No argument was specified for the parameter and the parameter is not of type bool", new object[0]);
					ParameterBindingException ex2 = new ParameterBindingException(ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetParameterErrorExtent(argument), parameterName, toType, null, ParameterBinderStrings.MissingArgument, "MissingArgument", new object[0]);
					throw ex2;
				}
				ParameterBinderBase.bindingTracer.WriteLine("Arg is null, parameter type not bool or SwitchParameter, value is null.", new object[0]);
				result = null;
			}
			return result;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001E5F0 File Offset: 0x0001C7F0
		private object EncodeCollection(CommandParameterInternal argument, string parameterName, ParameterCollectionTypeInformation collectionTypeInformation, Type toType, object currentValue, bool coerceElementTypeIfNeeded, out bool coercionRequired)
		{
			object result = null;
			coercionRequired = false;
			ParameterBinderBase.bindingTracer.WriteLine("Binding collection parameter {0}: argument type [{1}], parameter type [{2}], collection type {3}, element type [{4}], {5}", new object[]
			{
				parameterName,
				(currentValue == null) ? "null" : currentValue.GetType().Name,
				toType,
				collectionTypeInformation.ParameterCollectionType,
				collectionTypeInformation.ElementType,
				coerceElementTypeIfNeeded ? "coerceElementType" : "no coerceElementType"
			});
			if (currentValue != null)
			{
				int num = 1;
				Type type = collectionTypeInformation.ElementType;
				IList ilist = ParameterBinderBase.GetIList(currentValue);
				if (ilist != null)
				{
					num = ilist.Count;
					ParameterBinderBase.tracer.WriteLine("current value is an IList with {0} elements", new object[]
					{
						num
					});
					ParameterBinderBase.bindingTracer.WriteLine("Arg is IList with {0} elements", new object[]
					{
						num
					});
				}
				object obj = null;
				IList list = null;
				MethodInfo methodInfo = null;
				bool flag = toType == typeof(Array);
				if (collectionTypeInformation.ParameterCollectionType == ParameterCollectionType.Array || flag)
				{
					if (flag)
					{
						type = typeof(object);
					}
					ParameterBinderBase.bindingTracer.WriteLine("Creating array with element type [{0}] and {1} elements", new object[]
					{
						type,
						num
					});
					list = (obj = Array.CreateInstance(type, num));
				}
				else
				{
					if (collectionTypeInformation.ParameterCollectionType != ParameterCollectionType.IList && collectionTypeInformation.ParameterCollectionType != ParameterCollectionType.ICollectionGeneric)
					{
						return result;
					}
					ParameterBinderBase.bindingTracer.WriteLine("Creating collection [{0}]", new object[]
					{
						toType
					});
					bool flag2 = false;
					Exception ex = null;
					try
					{
						obj = Activator.CreateInstance(toType, BindingFlags.Default, null, new object[0], CultureInfo.InvariantCulture);
						if (collectionTypeInformation.ParameterCollectionType == ParameterCollectionType.IList)
						{
							list = (IList)obj;
						}
						else
						{
							Type elementType = collectionTypeInformation.ElementType;
							Exception ex2 = null;
							try
							{
								methodInfo = toType.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
								{
									elementType
								}, null);
							}
							catch (AmbiguousMatchException ex3)
							{
								ParameterBinderBase.bindingTracer.WriteLine("Ambiguous match to Add(T) for type " + toType.FullName + ": " + ex3.Message, new object[0]);
								ex2 = ex3;
							}
							catch (ArgumentException ex4)
							{
								ParameterBinderBase.bindingTracer.WriteLine("ArgumentException matching Add(T) for type " + toType.FullName + ": " + ex4.Message, new object[0]);
								ex2 = ex4;
							}
							if (null == methodInfo)
							{
								ParameterBindingException ex5 = new ParameterBindingException(ex2, ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(argument), parameterName, toType, currentValue.GetType(), ParameterBinderStrings.CannotExtractAddMethod, "CannotExtractAddMethod", new object[]
								{
									(ex2 == null) ? "" : ex2.Message
								});
								throw ex5;
							}
						}
					}
					catch (ArgumentException ex6)
					{
						flag2 = true;
						ex = ex6;
					}
					catch (NotSupportedException ex7)
					{
						flag2 = true;
						ex = ex7;
					}
					catch (TargetInvocationException ex8)
					{
						flag2 = true;
						ex = ex8;
					}
					catch (MethodAccessException ex9)
					{
						flag2 = true;
						ex = ex9;
					}
					catch (MemberAccessException ex10)
					{
						flag2 = true;
						ex = ex10;
					}
					catch (InvalidComObjectException ex11)
					{
						flag2 = true;
						ex = ex11;
					}
					catch (COMException ex12)
					{
						flag2 = true;
						ex = ex12;
					}
					catch (TypeLoadException ex13)
					{
						flag2 = true;
						ex = ex13;
					}
					if (flag2)
					{
						ParameterBindingException ex14 = new ParameterBindingException(ex, ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(argument), parameterName, toType, currentValue.GetType(), ParameterBinderStrings.CannotConvertArgument, "CannotConvertArgument", new object[]
						{
							"null",
							ex.Message
						});
						throw ex14;
					}
				}
				if (ilist != null)
				{
					int num2 = 0;
					ParameterBinderBase.bindingTracer.WriteLine("Argument type {0} is IList", new object[]
					{
						currentValue.GetType()
					});
					using (IEnumerator enumerator = ilist.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							object obj3 = PSObject.Base(obj2);
							if (coerceElementTypeIfNeeded)
							{
								ParameterBinderBase.bindingTracer.WriteLine("COERCE collection element from type {0} to type {1}", new object[]
								{
									(obj2 == null) ? "null" : obj2.GetType().Name,
									type
								});
								obj3 = this.CoerceTypeAsNeeded(argument, parameterName, type, null, obj2);
							}
							else if (null != type && obj3 != null)
							{
								Type type2 = obj3.GetType();
								Type type3 = type;
								if (type2 != type3 && !type2.IsSubclassOf(type3))
								{
									ParameterBinderBase.bindingTracer.WriteLine("COERCION REQUIRED: Did not attempt to coerce collection element from type {0} to type {1}", new object[]
									{
										(obj2 == null) ? "null" : obj2.GetType().Name,
										type
									});
									coercionRequired = true;
									break;
								}
							}
							try
							{
								if (collectionTypeInformation.ParameterCollectionType == ParameterCollectionType.Array || flag)
								{
									ParameterBinderBase.bindingTracer.WriteLine("Adding element of type {0} to array position {1}", new object[]
									{
										(obj3 == null) ? "null" : obj3.GetType().Name,
										num2
									});
									list[num2++] = obj3;
								}
								else if (collectionTypeInformation.ParameterCollectionType == ParameterCollectionType.IList)
								{
									ParameterBinderBase.bindingTracer.WriteLine("Adding element of type {0} via IList.Add", new object[]
									{
										(obj3 == null) ? "null" : obj3.GetType().Name
									});
									list.Add(obj3);
								}
								else
								{
									ParameterBinderBase.bindingTracer.WriteLine("Adding element of type {0} via ICollection<T>::Add()", new object[]
									{
										(obj3 == null) ? "null" : obj3.GetType().Name
									});
									methodInfo.Invoke(obj, new object[]
									{
										obj3
									});
								}
							}
							catch (Exception innerException)
							{
								CommandProcessorBase.CheckForSevereException(innerException);
								if (innerException is TargetInvocationException && innerException.InnerException != null)
								{
									innerException = innerException.InnerException;
								}
								ParameterBindingException ex15 = new ParameterBindingException(innerException, ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(argument), parameterName, toType, (obj3 == null) ? null : obj3.GetType(), ParameterBinderStrings.CannotConvertArgument, "CannotConvertArgument", new object[]
								{
									obj3 ?? "null",
									innerException.Message
								});
								throw ex15;
							}
						}
						goto IL_811;
					}
				}
				ParameterBinderBase.bindingTracer.WriteLine("Argument type {0} is not IList, treating this as scalar", new object[]
				{
					currentValue.GetType().Name
				});
				if (type != null)
				{
					if (coerceElementTypeIfNeeded)
					{
						ParameterBinderBase.bindingTracer.WriteLine("Coercing scalar arg value to type {1}", new object[]
						{
							type
						});
						currentValue = this.CoerceTypeAsNeeded(argument, parameterName, type, null, currentValue);
					}
					else
					{
						Type type4 = currentValue.GetType();
						Type type5 = type;
						if (type4 != type5 && !type4.IsSubclassOf(type5))
						{
							ParameterBinderBase.bindingTracer.WriteLine("COERCION REQUIRED: Did not coerce scalar arg value to type {1}", new object[]
							{
								type
							});
							coercionRequired = true;
							return result;
						}
					}
				}
				try
				{
					if (collectionTypeInformation.ParameterCollectionType == ParameterCollectionType.Array || flag)
					{
						ParameterBinderBase.bindingTracer.WriteLine("Adding scalar element of type {0} to array position {1}", new object[]
						{
							(currentValue == null) ? "null" : currentValue.GetType().Name,
							0
						});
						list[0] = currentValue;
					}
					else if (collectionTypeInformation.ParameterCollectionType == ParameterCollectionType.IList)
					{
						ParameterBinderBase.bindingTracer.WriteLine("Adding scalar element of type {0} via IList.Add", new object[]
						{
							(currentValue == null) ? "null" : currentValue.GetType().Name
						});
						list.Add(currentValue);
					}
					else
					{
						ParameterBinderBase.bindingTracer.WriteLine("Adding scalar element of type {0} via ICollection<T>::Add()", new object[]
						{
							(currentValue == null) ? "null" : currentValue.GetType().Name
						});
						methodInfo.Invoke(obj, new object[]
						{
							currentValue
						});
					}
				}
				catch (Exception innerException2)
				{
					CommandProcessorBase.CheckForSevereException(innerException2);
					if (innerException2 is TargetInvocationException && innerException2.InnerException != null)
					{
						innerException2 = innerException2.InnerException;
					}
					ParameterBindingException ex16 = new ParameterBindingException(innerException2, ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetErrorExtent(argument), parameterName, toType, (currentValue == null) ? null : currentValue.GetType(), ParameterBinderStrings.CannotConvertArgument, "CannotConvertArgument", new object[]
					{
						currentValue ?? "null",
						innerException2.Message
					});
					throw ex16;
				}
				IL_811:
				if (!coercionRequired)
				{
					result = obj;
				}
			}
			return result;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001EF54 File Offset: 0x0001D154
		internal static IList GetIList(object value)
		{
			object obj = PSObject.Base(value);
			IList list = obj as IList;
			if (list != null)
			{
				ParameterBinderBase.tracer.WriteLine((obj == value) ? "argument is IList" : "argument is PSObject with BaseObject as IList", new object[0]);
			}
			return list;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001EF94 File Offset: 0x0001D194
		protected IScriptExtent GetErrorExtent(CommandParameterInternal cpi)
		{
			IScriptExtent scriptExtent = cpi.ErrorExtent;
			if (scriptExtent == PositionUtilities.EmptyExtent)
			{
				scriptExtent = this.InvocationInfo.ScriptPosition;
			}
			return scriptExtent;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001EFC0 File Offset: 0x0001D1C0
		protected IScriptExtent GetParameterErrorExtent(CommandParameterInternal cpi)
		{
			IScriptExtent scriptExtent = cpi.ParameterExtent;
			if (scriptExtent == PositionUtilities.EmptyExtent)
			{
				scriptExtent = this.InvocationInfo.ScriptPosition;
			}
			return scriptExtent;
		}

		// Token: 0x04000285 RID: 645
		internal const string FQIDParameterObsolete = "ParameterObsolete";

		// Token: 0x04000286 RID: 646
		[TraceSource("ParameterBinderBase", "A abstract helper class for the CommandProcessor that binds parameters to the specified object.")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("ParameterBinderBase", "A abstract helper class for the CommandProcessor that binds parameters to the specified object.");

		// Token: 0x04000287 RID: 647
		[TraceSource("ParameterBinding", "Traces the process of binding the arguments to the parameters of cmdlets, scripts, and applications.")]
		internal static PSTraceSource bindingTracer = PSTraceSource.GetTracer("ParameterBinding", "Traces the process of binding the arguments to the parameters of cmdlets, scripts, and applications.", false);

		// Token: 0x04000288 RID: 648
		private object target;

		// Token: 0x04000289 RID: 649
		private CommandLineParameters commandLineParameters;

		// Token: 0x0400028A RID: 650
		internal bool RecordBoundParameters = true;

		// Token: 0x0400028B RID: 651
		private InvocationInfo invocationInfo;

		// Token: 0x0400028C RID: 652
		private ExecutionContext context;

		// Token: 0x0400028D RID: 653
		private InternalCommand command;

		// Token: 0x0400028E RID: 654
		private EngineIntrinsics engine;
	}
}
