using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace System.Management.Automation
{
	// Token: 0x02000636 RID: 1590
	internal static class EnumerableOps
	{
		// Token: 0x060044D7 RID: 17623 RVA: 0x00171150 File Offset: 0x0016F350
		internal static object Where(IEnumerator enumerator, ScriptBlock expressionSB, WhereOperatorSelectionMode selectionMode, int numberToReturn)
		{
			if (numberToReturn < 0)
			{
				throw new ArgumentOutOfRangeException("numberToReturn", numberToReturn, ParserStrings.NumberToReturnMustBeGreaterThanZero);
			}
			ExecutionContext executionContext = Runspace.DefaultRunspace.ExecutionContext;
			if (expressionSB == null)
			{
				if (selectionMode == WhereOperatorSelectionMode.Default)
				{
					throw new InvalidOperationException(ParserStrings.EmptyExpressionRequiresANonDefaultMode);
				}
				List<object> list = new List<object>();
				object obj = null;
				int i = 0;
				if (numberToReturn == 0)
				{
					numberToReturn = 1;
				}
				if (selectionMode == WhereOperatorSelectionMode.SkipUntil)
				{
					while (i < numberToReturn)
					{
						if (!EnumerableOps.MoveNext(null, enumerator))
						{
							break;
						}
						i++;
					}
					while (EnumerableOps.MoveNext(executionContext, enumerator))
					{
						list.Add(EnumerableOps.Current(enumerator));
					}
					return list.ToArray();
				}
				if (selectionMode == WhereOperatorSelectionMode.Last)
				{
					while (EnumerableOps.MoveNext(executionContext, enumerator))
					{
						obj = EnumerableOps.Current(enumerator);
						if (numberToReturn > 1)
						{
							list.Add(obj);
							if (list.Count > numberToReturn)
							{
								list.RemoveAt(0);
							}
						}
					}
					if (numberToReturn == 1)
					{
						return new object[]
						{
							obj
						};
					}
					return list.ToArray();
				}
				else
				{
					object[] array = new object[numberToReturn];
					while (EnumerableOps.MoveNext(executionContext, enumerator))
					{
						obj = EnumerableOps.Current(enumerator);
						array[i++] = obj;
						if (i >= numberToReturn)
						{
							if (selectionMode == WhereOperatorSelectionMode.First || selectionMode == WhereOperatorSelectionMode.Until)
							{
								return array;
							}
							break;
						}
					}
					if (selectionMode == WhereOperatorSelectionMode.Split)
					{
						while (EnumerableOps.MoveNext(executionContext, enumerator))
						{
							object item = EnumerableOps.Current(enumerator);
							list.Add(item);
						}
						return new object[]
						{
							array,
							list.ToArray()
						};
					}
					return array;
				}
			}
			else
			{
				Collection<PSObject> collection = new Collection<PSObject>();
				Collection<PSObject> collection2 = null;
				if (selectionMode == WhereOperatorSelectionMode.Split)
				{
					collection2 = new Collection<PSObject>();
				}
				List<object> list2 = new List<object>();
				Pipe outputPipe = new Pipe(list2);
				bool flag = false;
				while (EnumerableOps.MoveNext(executionContext, enumerator))
				{
					object obj2 = EnumerableOps.Current(enumerator);
					if (flag)
					{
						collection.Add((obj2 == null) ? null : PSObject.AsPSObject(obj2));
						if (numberToReturn > 0 && collection.Count >= numberToReturn)
						{
							break;
						}
					}
					else
					{
						list2.Clear();
						expressionSB.InvokeWithPipeImpl(false, null, null, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, obj2, AutomationNull.Value, AutomationNull.Value, outputPipe, null, new object[0]);
						bool flag2 = LanguagePrimitives.IsTrue(list2);
						if (flag2)
						{
							if (selectionMode == WhereOperatorSelectionMode.Until)
							{
								break;
							}
							if (selectionMode == WhereOperatorSelectionMode.Last)
							{
								if (numberToReturn == 0)
								{
									numberToReturn = 1;
								}
								if (collection.Count < numberToReturn)
								{
									collection.Add((obj2 == null) ? null : PSObject.AsPSObject(obj2));
								}
								else if (numberToReturn == 1)
								{
									collection[0] = ((obj2 == null) ? null : PSObject.AsPSObject(obj2));
								}
								else
								{
									collection.RemoveAt(0);
									collection.Add((obj2 == null) ? null : PSObject.AsPSObject(obj2));
								}
							}
							else if (selectionMode == WhereOperatorSelectionMode.SkipUntil)
							{
								collection.Add((obj2 == null) ? null : PSObject.AsPSObject(obj2));
								flag = true;
							}
							else
							{
								collection.Add((obj2 == null) ? null : PSObject.AsPSObject(obj2));
							}
							if (selectionMode != WhereOperatorSelectionMode.Last)
							{
								if (numberToReturn == 0 && selectionMode == WhereOperatorSelectionMode.First)
								{
									break;
								}
								if (numberToReturn != 0 && numberToReturn == collection.Count)
								{
									break;
								}
							}
						}
						else if (selectionMode == WhereOperatorSelectionMode.Until)
						{
							collection.Add((obj2 == null) ? null : PSObject.AsPSObject(obj2));
							if (numberToReturn > 0 && collection.Count >= numberToReturn)
							{
								break;
							}
						}
						else if (selectionMode == WhereOperatorSelectionMode.Split)
						{
							collection2.Add((obj2 == null) ? null : PSObject.AsPSObject(obj2));
						}
					}
				}
				if (selectionMode == WhereOperatorSelectionMode.Split)
				{
					while (EnumerableOps.MoveNext(executionContext, enumerator))
					{
						object obj3 = EnumerableOps.Current(enumerator);
						collection2.Add((obj3 == null) ? null : PSObject.AsPSObject(obj3));
					}
					return new object[]
					{
						collection,
						collection2
					};
				}
				return collection;
			}
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x00171480 File Offset: 0x0016F680
		internal static object ForEach(IEnumerator enumerator, object expression, object[] arguments)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			ExecutionContext executionContext = Runspace.DefaultRunspace.ExecutionContext;
			Type type = expression as Type;
			if (!(type != null))
			{
				Collection<PSObject> collection = new Collection<PSObject>();
				ScriptBlock scriptBlock = expression as ScriptBlock;
				if (scriptBlock != null)
				{
					Pipe outputPipe = new Pipe(collection);
					if (scriptBlock.HasBeginBlock)
					{
						scriptBlock.InvokeWithPipeImpl(ScriptBlockClauseToInvoke.Begin, false, null, null, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, AutomationNull.Value, AutomationNull.Value, outputPipe, null, arguments);
					}
					ScriptBlockClauseToInvoke scriptBlockClauseToInvoke = scriptBlock.HasProcessBlock ? ScriptBlockClauseToInvoke.Process : ScriptBlockClauseToInvoke.End;
					object obj = null;
					while (EnumerableOps.MoveNext(executionContext, enumerator))
					{
						obj = EnumerableOps.Current(enumerator);
						if (obj != AutomationNull.Value)
						{
							scriptBlock.InvokeWithPipeImpl(scriptBlockClauseToInvoke, false, null, null, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, obj, AutomationNull.Value, AutomationNull.Value, outputPipe, null, arguments);
						}
					}
					if (scriptBlockClauseToInvoke == ScriptBlockClauseToInvoke.Process && scriptBlock.HasEndBlock)
					{
						scriptBlock.InvokeWithPipeImpl(ScriptBlockClauseToInvoke.End, false, null, null, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, obj, AutomationNull.Value, AutomationNull.Value, outputPipe, null, arguments);
					}
				}
				else
				{
					string text = ParserOps.ConvertTo<string>(expression, null);
					int num = arguments.Length;
					PSLanguageMode languageMode = executionContext.LanguageMode;
					while (EnumerableOps.MoveNext(executionContext, enumerator))
					{
						object obj2 = EnumerableOps.Current(enumerator);
						object obj3 = PSObject.Base(obj2);
						Hashtable hashtable = obj3 as Hashtable;
						if (hashtable != null)
						{
							switch (num)
							{
							case 0:
							{
								object obj4 = hashtable[text];
								collection.Add((obj4 != null) ? PSObject.AsPSObject(obj4) : null);
								break;
							}
							case 1:
								hashtable[text] = arguments[0];
								break;
							default:
								hashtable[text] = arguments;
								break;
							}
						}
						else if (obj2 == null)
						{
							if (arguments.Length != 0)
							{
								NullReferenceException ex = new NullReferenceException();
								throw new MethodInvocationException(ex.GetType().Name, ex, ExtendedTypeSystem.MethodInvocationException, new object[]
								{
									text,
									arguments.Length,
									ex.Message
								});
							}
							collection.Add(null);
						}
						else
						{
							PSObject psobject = PSObject.AsPSObject(obj2);
							if (psobject != AutomationNull.Value)
							{
								PSMemberInfo psmemberInfo = psobject.Members[text];
								if (psmemberInfo == null)
								{
									if (executionContext.IsStrictVersion(2))
									{
										throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "PropertyNotFoundStrict", ParserStrings.PropertyNotFoundStrict, new object[]
										{
											text
										});
									}
									if (num != 0)
									{
										throw InterpreterError.NewInterpreterException(psobject, typeof(NullReferenceException), null, "ForEachNonexistentMemberReference", ParserStrings.ForEachNonexistentMemberReference, new object[]
										{
											text
										});
									}
									collection.Add(null);
								}
								else
								{
									PSMethodInfo psmethodInfo = psmemberInfo as PSMethodInfo;
									if (psmethodInfo != null)
									{
										if (languageMode == PSLanguageMode.RestrictedLanguage)
										{
											throw InterpreterError.NewInterpreterException(obj2, typeof(PSInvalidOperationException), null, "NoMethodInvocationInRestrictedLanguageMode", InternalCommandStrings.NoMethodInvocationInRestrictedLanguageMode, new object[0]);
										}
										if (languageMode == PSLanguageMode.ConstrainedLanguage && !CoreTypes.Contains(obj3.GetType()))
										{
											throw InterpreterError.NewInterpreterException(obj2, typeof(PSInvalidOperationException), null, "MethodInvocationNotSupportedInConstrainedLanguage", ParserStrings.InvokeMethodConstrainedLanguage, new object[0]);
										}
										collection.Add(PSObject.AsPSObject(psmethodInfo.Invoke(arguments)));
									}
									else
									{
										PSPropertyInfo pspropertyInfo = psmemberInfo as PSPropertyInfo;
										switch (num)
										{
										case 0:
											collection.Add(PSObject.AsPSObject(pspropertyInfo.Value));
											break;
										case 1:
											pspropertyInfo.Value = arguments[0];
											break;
										default:
											pspropertyInfo.Value = arguments;
											break;
										}
									}
								}
							}
						}
					}
				}
				return collection;
			}
			object obj5 = null;
			if (type.GetInterface("System.Collections.ICollection") != null)
			{
				if (type.IsArray)
				{
					List<object> list = new List<object>();
					while (EnumerableOps.MoveNext(null, enumerator))
					{
						object item = EnumerableOps.Current(enumerator);
						list.Add(item);
					}
					return LanguagePrimitives.ConvertTo(list, type, CultureInfo.InvariantCulture);
				}
				if (type.GetTypeInfo().IsGenericType)
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (genericArguments.Length != 1)
					{
						throw InterpreterError.NewInterpreterException(expression, typeof(RuntimeException), null, "ForEachBadGenericConversionTypeSpecified", ParserStrings.ForEachBadGenericConversionTypeSpecified, new object[]
						{
							ParserOps.ConvertTo<string>(type, null)
						});
					}
					obj5 = PSObject.AsPSObject(Activator.CreateInstance(type));
					while (EnumerableOps.MoveNext(executionContext, enumerator))
					{
						object arg = EnumerableOps.Current(enumerator);
						if (EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site1 == null)
						{
							EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site1 = CallSite<Action<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(EnumerableOps), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site1.Target(EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site1, obj5, arg);
					}
				}
			}
			else
			{
				Type type2 = typeof(Collection<>).MakeGenericType(new Type[]
				{
					type
				});
				obj5 = PSObject.AsPSObject(Activator.CreateInstance(type2));
				while (EnumerableOps.MoveNext(executionContext, enumerator))
				{
					object arg2 = EnumerableOps.Current(enumerator);
					if (EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site2 == null)
					{
						EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site2 = CallSite<Action<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(EnumerableOps), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site2.Target(EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site2, obj5, arg2);
				}
			}
			if (EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site3 == null)
			{
				EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site3 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(EnumerableOps), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target = EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site3.Target;
			CallSite <>p__Site = EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site3;
			if (EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site4 == null)
			{
				EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site4 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(EnumerableOps), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			if (target(<>p__Site, EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site4.Target(EnumerableOps.<ForEach>o__SiteContainer0.<>p__Site4, obj5, null)))
			{
				throw InterpreterError.NewInterpreterException(expression, typeof(RuntimeException), null, "ForEachTypeConversionFailed", ParserStrings.ForEachTypeConversionFailed, new object[]
				{
					ParserOps.ConvertTo<string>(type, null)
				});
			}
			return obj5;
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x00171A84 File Offset: 0x0016FC84
		internal static object SlicingIndex(object target, IEnumerator indexes, Func<object, object, object> indexer)
		{
			EnumerableOps.NonEnumerableObjectEnumerator nonEnumerableObjectEnumerator = indexes as EnumerableOps.NonEnumerableObjectEnumerator;
			if (nonEnumerableObjectEnumerator != null)
			{
				return indexer(target, nonEnumerableObjectEnumerator.GetNonEnumerableObject());
			}
			List<object> list = new List<object>();
			while (EnumerableOps.MoveNext(null, indexes))
			{
				object obj = indexer(target, EnumerableOps.Current(indexes));
				if (obj != AutomationNull.Value)
				{
					list.Add(obj);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x00171AE0 File Offset: 0x0016FCE0
		private static void FlattenResults(object o, List<object> result)
		{
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(o);
			if (enumerator != null)
			{
				while (enumerator.MoveNext())
				{
					o = enumerator.Current;
					if (o != AutomationNull.Value)
					{
						result.Add(o);
					}
				}
				return;
			}
			result.Add(o);
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x00171B20 File Offset: 0x0016FD20
		private static void PropertyGetterWorker(CallSite<Func<CallSite, object, object>> getMemberBinderSite, IEnumerator enumerator, ExecutionContext context, List<object> result)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			while (EnumerableOps.MoveNext(context, enumerator))
			{
				object obj = EnumerableOps.Current(enumerator);
				object obj2 = getMemberBinderSite.Target(getMemberBinderSite, obj);
				if (obj2 != AutomationNull.Value)
				{
					EnumerableOps.FlattenResults(obj2, result);
				}
				else
				{
					IEnumerator enumerator2 = LanguagePrimitives.GetEnumerator(obj);
					if (enumerator2 != null)
					{
						EnumerableOps.PropertyGetterWorker(getMemberBinderSite, enumerator2, context, result);
					}
				}
			}
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x00171B78 File Offset: 0x0016FD78
		internal static object PropertyGetter(PSGetMemberBinder binder, IEnumerator enumerator)
		{
			CallSite<Func<CallSite, object, object>> getMemberBinderSite = CallSite<Func<CallSite, object, object>>.Create(binder);
			List<object> list = new List<object>();
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			EnumerableOps.PropertyGetterWorker(getMemberBinderSite, enumerator, executionContextFromTLS, list);
			if (list.Count == 1)
			{
				return list[0];
			}
			if (list.Count != 0)
			{
				return list.ToArray();
			}
			if (executionContextFromTLS.IsStrictVersion(2))
			{
				throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "PropertyNotFoundStrict", ParserStrings.PropertyNotFoundStrict, new object[]
				{
					binder.Name
				});
			}
			return null;
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x00171BF8 File Offset: 0x0016FDF8
		private static void MethodInvokerWorker(CallSite invokeMemberSite, IEnumerator enumerator, object[] args, ExecutionContext context, List<object> result, ref bool foundMethod)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			while (EnumerableOps.MoveNext(context, enumerator))
			{
				object obj = EnumerableOps.Current(enumerator);
				try
				{
					if (EnumerableOps.<MethodInvokerWorker>o__SiteContainer5.<>p__Site6 == null)
					{
						EnumerableOps.<MethodInvokerWorker>o__SiteContainer5.<>p__Site6 = CallSite<Func<CallSite, object, object[], object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "DynamicInvoke", null, typeof(EnumerableOps), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Func<CallSite, object, object[], object> target = EnumerableOps.<MethodInvokerWorker>o__SiteContainer5.<>p__Site6.Target;
					CallSite <>p__Site = EnumerableOps.<MethodInvokerWorker>o__SiteContainer5.<>p__Site6;
					if (EnumerableOps.<MethodInvokerWorker>o__SiteContainer5.<>p__Site7 == null)
					{
						EnumerableOps.<MethodInvokerWorker>o__SiteContainer5.<>p__Site7 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Target", typeof(EnumerableOps), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj2 = target(<>p__Site, EnumerableOps.<MethodInvokerWorker>o__SiteContainer5.<>p__Site7.Target(EnumerableOps.<MethodInvokerWorker>o__SiteContainer5.<>p__Site7, invokeMemberSite), args.Prepend(obj).Prepend(invokeMemberSite).ToArray<object>());
					foundMethod = true;
					if (obj2 != AutomationNull.Value)
					{
						EnumerableOps.FlattenResults(obj2, result);
					}
				}
				catch (TargetInvocationException ex)
				{
					RuntimeException ex2 = ex.InnerException as RuntimeException;
					if (ex2 != null && ex2.ErrorRecord.FullyQualifiedErrorId.Equals("MethodNotFound", StringComparison.Ordinal))
					{
						IEnumerator enumerator2 = LanguagePrimitives.GetEnumerator(obj);
						if (enumerator2 != null)
						{
							EnumerableOps.MethodInvokerWorker(invokeMemberSite, enumerator2, args, context, result, ref foundMethod);
							continue;
						}
					}
					throw ex.InnerException;
				}
			}
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x00171D58 File Offset: 0x0016FF58
		internal static object MethodInvoker(PSInvokeMemberBinder binder, Type delegateType, IEnumerator enumerator, object[] args, Type typeForMessage)
		{
			CallSite invokeMemberSite = CallSite.Create(delegateType, binder);
			List<object> list = new List<object>();
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			bool flag = false;
			EnumerableOps.MethodInvokerWorker(invokeMemberSite, enumerator, args, executionContextFromTLS, list, ref flag);
			if (list.Count == 1)
			{
				return list[0];
			}
			if (!flag)
			{
				throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "MethodNotFound", ParserStrings.MethodNotFound, new object[]
				{
					typeForMessage.FullName,
					binder.Name
				});
			}
			if (list.Count == 0)
			{
				return AutomationNull.Value;
			}
			return list.ToArray();
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x00171DEC File Offset: 0x0016FFEC
		internal static object Multiply(IEnumerator enumerator, uint times)
		{
			EnumerableOps.NonEnumerableObjectEnumerator nonEnumerableObjectEnumerator = enumerator as EnumerableOps.NonEnumerableObjectEnumerator;
			if (nonEnumerableObjectEnumerator != null)
			{
				return ParserOps.ImplicitOp(nonEnumerableObjectEnumerator.GetNonEnumerableObject(), times, "op_Multiply", null, "*");
			}
			List<object> list = new List<object>();
			while (EnumerableOps.MoveNext(null, enumerator))
			{
				list.Add(EnumerableOps.Current(enumerator));
			}
			if (list.Count == 0)
			{
				return new object[0];
			}
			return ArrayOps.Multiply<object>(list.ToArray(), times);
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x00171E58 File Offset: 0x00170058
		internal static IEnumerator GetEnumerator(IEnumerable enumerable)
		{
			IEnumerator enumerator;
			try
			{
				enumerator = enumerable.GetEnumerator();
			}
			catch (RuntimeException)
			{
				throw;
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("ExceptionInGetEnumerator", ex, ExtendedTypeSystem.EnumerationException, new object[]
				{
					ex.Message
				});
			}
			return enumerator;
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x00171EB8 File Offset: 0x001700B8
		internal static IEnumerator GetCOMEnumerator(object obj)
		{
			try
			{
				IEnumerable enumerable = obj as IEnumerable;
				if (enumerable != null)
				{
					IEnumerator enumerator = enumerable.GetEnumerator();
					if (enumerator != null)
					{
						return enumerator;
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			return (obj as IEnumerator) ?? EnumerableOps.NonEnumerableObjectEnumerator.Create(obj);
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x00171F0C File Offset: 0x0017010C
		internal static IEnumerator GetGenericEnumerator<T>(IEnumerable<T> enumerable)
		{
			IEnumerator enumerator;
			try
			{
				enumerator = enumerable.GetEnumerator();
			}
			catch (RuntimeException)
			{
				throw;
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("ExceptionInGetEnumerator", ex, ExtendedTypeSystem.EnumerationException, new object[]
				{
					ex.Message
				});
			}
			return enumerator;
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x00171F6C File Offset: 0x0017016C
		internal static bool MoveNext(ExecutionContext context, IEnumerator enumerator)
		{
			bool result;
			try
			{
				if (context != null && context.CurrentPipelineStopping)
				{
					throw new PipelineStoppedException();
				}
				result = enumerator.MoveNext();
			}
			catch (RuntimeException)
			{
				throw;
			}
			catch (FlowControlException)
			{
				throw;
			}
			catch (ScriptCallDepthException)
			{
				throw;
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw InterpreterError.NewInterpreterExceptionWithInnerException(enumerator, typeof(RuntimeException), null, "BadEnumeration", ParserStrings.BadEnumeration, ex, new object[]
				{
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x00172008 File Offset: 0x00170208
		internal static object Current(IEnumerator enumerator)
		{
			object result;
			try
			{
				result = enumerator.Current;
			}
			catch (RuntimeException)
			{
				throw;
			}
			catch (ScriptCallDepthException)
			{
				throw;
			}
			catch (FlowControlException)
			{
				throw;
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw InterpreterError.NewInterpreterExceptionWithInnerException(enumerator, typeof(RuntimeException), null, "BadEnumeration", ParserStrings.BadEnumeration, ex, new object[]
				{
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x00172094 File Offset: 0x00170294
		internal static object AddFakeEnumerable(EnumerableOps.NonEnumerableObjectEnumerator fakeEnumerator, object rhs)
		{
			EnumerableOps.NonEnumerableObjectEnumerator nonEnumerableObjectEnumerator = rhs as EnumerableOps.NonEnumerableObjectEnumerator;
			return ParserOps.ImplicitOp(fakeEnumerator.GetNonEnumerableObject(), (nonEnumerableObjectEnumerator != null) ? nonEnumerableObjectEnumerator.GetNonEnumerableObject() : rhs, "op_Addition", null, "+");
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x001720CC File Offset: 0x001702CC
		internal static object AddEnumerable(ExecutionContext context, IEnumerator lhs, IEnumerator rhs)
		{
			EnumerableOps.NonEnumerableObjectEnumerator nonEnumerableObjectEnumerator = lhs as EnumerableOps.NonEnumerableObjectEnumerator;
			if (nonEnumerableObjectEnumerator != null)
			{
				return EnumerableOps.AddFakeEnumerable(nonEnumerableObjectEnumerator, rhs);
			}
			List<object> list = new List<object>();
			while (EnumerableOps.MoveNext(context, lhs))
			{
				list.Add(EnumerableOps.Current(lhs));
			}
			while (EnumerableOps.MoveNext(context, rhs))
			{
				list.Add(EnumerableOps.Current(rhs));
			}
			return list.ToArray();
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x00172128 File Offset: 0x00170328
		internal static object AddObject(ExecutionContext context, IEnumerator lhs, object rhs)
		{
			EnumerableOps.NonEnumerableObjectEnumerator nonEnumerableObjectEnumerator = lhs as EnumerableOps.NonEnumerableObjectEnumerator;
			if (nonEnumerableObjectEnumerator != null)
			{
				return EnumerableOps.AddFakeEnumerable(nonEnumerableObjectEnumerator, rhs);
			}
			List<object> list = new List<object>();
			while (EnumerableOps.MoveNext(context, lhs))
			{
				list.Add(EnumerableOps.Current(lhs));
			}
			list.Add(rhs);
			return list.ToArray();
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x00172174 File Offset: 0x00170374
		internal static object Compare(IEnumerator enumerator, object valueToCompareTo, Func<object, object, bool> compareDelegate)
		{
			EnumerableOps.NonEnumerableObjectEnumerator nonEnumerableObjectEnumerator = enumerator as EnumerableOps.NonEnumerableObjectEnumerator;
			if (nonEnumerableObjectEnumerator == null)
			{
				List<object> list = new List<object>();
				while (EnumerableOps.MoveNext(null, enumerator))
				{
					object obj = EnumerableOps.Current(enumerator);
					if (compareDelegate(obj, valueToCompareTo))
					{
						list.Add(obj);
					}
				}
				return list.ToArray();
			}
			if (!compareDelegate(nonEnumerableObjectEnumerator.GetNonEnumerableObject(), valueToCompareTo))
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x001721D8 File Offset: 0x001703D8
		internal static void WriteEnumerableToPipe(IEnumerator enumerator, Pipe pipe, ExecutionContext context, bool dispose)
		{
			try
			{
				while (EnumerableOps.MoveNext(context, enumerator))
				{
					pipe.Add(EnumerableOps.Current(enumerator));
				}
			}
			finally
			{
				if (dispose)
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x00172224 File Offset: 0x00170424
		internal static object[] ToArray(IEnumerator enumerator)
		{
			List<object> list = new List<object>();
			while (EnumerableOps.MoveNext(null, enumerator))
			{
				list.Add(EnumerableOps.Current(enumerator));
			}
			return list.ToArray();
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x00172254 File Offset: 0x00170454
		internal static object[] GetSlice(IList list, int startIndex)
		{
			int num = list.Count - startIndex;
			object[] array = new object[num];
			int num2 = startIndex;
			int i = 0;
			while (i < num)
			{
				array[i++] = list[num2++];
			}
			return array;
		}

		// Token: 0x02000637 RID: 1591
		internal class NonEnumerableObjectEnumerator : IEnumerator
		{
			// Token: 0x060044EC RID: 17644 RVA: 0x00172290 File Offset: 0x00170490
			internal static IEnumerator Create(object obj)
			{
				return new EnumerableOps.NonEnumerableObjectEnumerator
				{
					obj = obj,
					realEnumerator = new object[]
					{
						obj
					}.GetEnumerator()
				};
			}

			// Token: 0x060044ED RID: 17645 RVA: 0x001722C2 File Offset: 0x001704C2
			bool IEnumerator.MoveNext()
			{
				return this.realEnumerator.MoveNext();
			}

			// Token: 0x060044EE RID: 17646 RVA: 0x001722CF File Offset: 0x001704CF
			void IEnumerator.Reset()
			{
				this.realEnumerator.Reset();
			}

			// Token: 0x17000EB0 RID: 3760
			// (get) Token: 0x060044EF RID: 17647 RVA: 0x001722DC File Offset: 0x001704DC
			object IEnumerator.Current
			{
				get
				{
					return this.realEnumerator.Current;
				}
			}

			// Token: 0x060044F0 RID: 17648 RVA: 0x001722E9 File Offset: 0x001704E9
			internal object GetNonEnumerableObject()
			{
				return this.obj;
			}

			// Token: 0x04002227 RID: 8743
			private object obj;

			// Token: 0x04002228 RID: 8744
			private IEnumerator realEnumerator;
		}

		// Token: 0x02000BB1 RID: 2993
		[CompilerGenerated]
		private static class <ForEach>o__SiteContainer0
		{
			// Token: 0x04003B99 RID: 15257
			public static CallSite<Action<CallSite, object, object>> <>p__Site1;

			// Token: 0x04003B9A RID: 15258
			public static CallSite<Action<CallSite, object, object>> <>p__Site2;

			// Token: 0x04003B9B RID: 15259
			public static CallSite<Func<CallSite, object, bool>> <>p__Site3;

			// Token: 0x04003B9C RID: 15260
			public static CallSite<Func<CallSite, object, object, object>> <>p__Site4;
		}

		// Token: 0x02000BB2 RID: 2994
		[CompilerGenerated]
		private static class <MethodInvokerWorker>o__SiteContainer5
		{
			// Token: 0x04003B9D RID: 15261
			public static CallSite<Func<CallSite, object, object[], object>> <>p__Site6;

			// Token: 0x04003B9E RID: 15262
			public static CallSite<Func<CallSite, object, object>> <>p__Site7;
		}
	}
}
