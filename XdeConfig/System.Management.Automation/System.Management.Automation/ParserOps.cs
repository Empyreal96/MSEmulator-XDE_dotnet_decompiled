using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x02000488 RID: 1160
	internal static class ParserOps
	{
		// Token: 0x0600335C RID: 13148 RVA: 0x00118318 File Offset: 0x00116518
		static ParserOps()
		{
			for (int i = 0; i < 1100; i++)
			{
				ParserOps._integerCache[i] = i + -100;
			}
			for (char c = '\0'; c < 'ÿ'; c += '\u0001')
			{
				ParserOps._chars[(int)c] = new string(c, 1);
			}
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x001183A2 File Offset: 0x001165A2
		internal static string CharToString(char ch)
		{
			if (ch < 'ÿ')
			{
				return ParserOps._chars[(int)ch];
			}
			return new string(ch, 1);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x001183BB File Offset: 0x001165BB
		internal static object BoolToObject(bool value)
		{
			if (!value)
			{
				return ParserOps._FalseObject;
			}
			return ParserOps._TrueObject;
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x001183CB File Offset: 0x001165CB
		internal static object IntToObject(int value)
		{
			if (value < 1000 && value >= -100)
			{
				return ParserOps._integerCache[value - -100];
			}
			return value;
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x001183EC File Offset: 0x001165EC
		internal static PSObject WrappedNumber(object data, string text)
		{
			return new PSObject(data)
			{
				TokenText = text
			};
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x00118408 File Offset: 0x00116608
		internal static int FixNum(object obj, IScriptExtent errorPosition)
		{
			obj = PSObject.Base(obj);
			if (obj == null)
			{
				return 0;
			}
			if (obj is int)
			{
				return (int)obj;
			}
			return ParserOps.ConvertTo<int>(obj, errorPosition);
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x0011843C File Offset: 0x0011663C
		internal static T ConvertTo<T>(object obj, IScriptExtent errorPosition)
		{
			T result;
			try
			{
				result = (T)((object)LanguagePrimitives.ConvertTo(obj, typeof(T), CultureInfo.InvariantCulture));
			}
			catch (PSInvalidCastException ex)
			{
				RuntimeException ex2 = new RuntimeException(ex.Message, ex);
				ex2.ErrorRecord.SetInvocationInfo(new InvocationInfo(null, errorPosition));
				throw ex2;
			}
			return result;
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x0011849C File Offset: 0x0011669C
		internal static object ImplicitOp(object lval, object rval, string op, IScriptExtent errorPosition, string errorOp)
		{
			lval = PSObject.Base(lval);
			rval = PSObject.Base(rval);
			Type type = (lval != null) ? lval.GetType() : null;
			Type type2 = (rval != null) ? rval.GetType() : null;
			Type type3;
			if (type == null || type.GetTypeInfo().IsPrimitive)
			{
				type3 = ((type2 == null || type2.GetTypeInfo().IsPrimitive) ? null : type2);
			}
			else
			{
				type3 = type;
			}
			if (type3 == null)
			{
				throw InterpreterError.NewInterpreterException(lval, typeof(RuntimeException), errorPosition, "NotADefinedOperationForType", ParserStrings.NotADefinedOperationForType, new object[]
				{
					(type == null) ? "$null" : type.FullName,
					errorOp,
					(type2 == null) ? "$null" : type2.FullName
				});
			}
			return ParserOps.CallMethod(errorPosition, type3, op, null, new object[]
			{
				lval,
				rval
			}, true, AutomationNull.Value);
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x00118590 File Offset: 0x00116790
		private static object[] unfoldTuple(ExecutionContext context, IScriptExtent errorPosition, object tuple)
		{
			List<object> list = new List<object>();
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(tuple);
			if (enumerator != null)
			{
				while (ParserOps.MoveNext(context, errorPosition, enumerator))
				{
					object item = ParserOps.Current(errorPosition, enumerator);
					list.Add(item);
				}
			}
			else
			{
				list.Add(tuple);
			}
			return list.ToArray();
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x00118750 File Offset: 0x00116950
		private static IEnumerable<string> enumerateContent(ExecutionContext context, IScriptExtent errorPosition, ParserOps.SplitImplOptions implOptions, object tuple)
		{
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(tuple);
			if (enumerator == null)
			{
				enumerator = new object[]
				{
					tuple
				}.GetEnumerator();
			}
			while (ParserOps.MoveNext(context, errorPosition, enumerator))
			{
				string strValue = PSObject.ToStringParser(context, enumerator.Current);
				if ((implOptions & ParserOps.SplitImplOptions.TrimContent) != ParserOps.SplitImplOptions.None)
				{
					strValue = strValue.Trim();
				}
				yield return strValue;
			}
			yield break;
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x00118784 File Offset: 0x00116984
		private static RegexOptions parseRegexOptions(SplitOptions options)
		{
			int[][] array = new int[][]
			{
				new int[]
				{
					4,
					512
				},
				new int[]
				{
					8,
					32
				},
				new int[]
				{
					16,
					2
				},
				new int[]
				{
					32,
					16
				},
				new int[]
				{
					64,
					1
				},
				new int[]
				{
					128,
					4
				}
			};
			RegexOptions regexOptions = RegexOptions.None;
			foreach (int[] array3 in array)
			{
				if ((options & (SplitOptions)array3[0]) != (SplitOptions)0)
				{
					regexOptions |= (RegexOptions)array3[1];
				}
			}
			return regexOptions;
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x00118860 File Offset: 0x00116A60
		internal static object UnarySplitOperator(ExecutionContext context, IScriptExtent errorPosition, object lval)
		{
			return ParserOps.SplitOperatorImpl(context, errorPosition, lval, new object[]
			{
				"\\s+"
			}, ParserOps.SplitImplOptions.TrimContent, false);
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x00118887 File Offset: 0x00116A87
		internal static object SplitOperator(ExecutionContext context, IScriptExtent errorPosition, object lval, object rval, bool ignoreCase)
		{
			return ParserOps.SplitOperatorImpl(context, errorPosition, lval, rval, ParserOps.SplitImplOptions.None, ignoreCase);
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x00118898 File Offset: 0x00116A98
		private static void ExtendList<T>(IList<T> list, IList<T> items)
		{
			foreach (T item in items)
			{
				list.Add(item);
			}
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x001188E0 File Offset: 0x00116AE0
		private static object SplitOperatorImpl(ExecutionContext context, IScriptExtent errorPosition, object lval, object rval, ParserOps.SplitImplOptions implOptions, bool ignoreCase)
		{
			IEnumerable<string> content = ParserOps.enumerateContent(context, errorPosition, implOptions, lval);
			string separatorPattern = null;
			int limit = 0;
			SplitOptions splitOptions = (SplitOptions)0;
			object[] array = ParserOps.unfoldTuple(context, errorPosition, rval);
			if (array.Length < 1)
			{
				throw InterpreterError.NewInterpreterException(rval, typeof(RuntimeException), errorPosition, "BadOperatorArgument", ParserStrings.BadOperatorArgument, new object[]
				{
					"-split",
					rval
				});
			}
			ScriptBlock scriptBlock = array[0] as ScriptBlock;
			if (scriptBlock == null)
			{
				separatorPattern = PSObject.ToStringParser(context, array[0]);
			}
			if (array.Length >= 2)
			{
				limit = ParserOps.FixNum(array[1], errorPosition);
			}
			if (array.Length >= 3 && array[2] != null)
			{
				string text = array[2] as string;
				if (text == null || !string.IsNullOrEmpty(text))
				{
					splitOptions = ParserOps.ConvertTo<SplitOptions>(array[2], errorPosition);
					if (scriptBlock != null)
					{
						throw InterpreterError.NewInterpreterException(null, typeof(ParseException), errorPosition, "InvalidSplitOptionWithPredicate", ParserStrings.InvalidSplitOptionWithPredicate, new object[0]);
					}
					if (ignoreCase && (splitOptions & SplitOptions.IgnoreCase) == (SplitOptions)0)
					{
						splitOptions |= SplitOptions.IgnoreCase;
					}
				}
			}
			else if (ignoreCase)
			{
				splitOptions |= SplitOptions.IgnoreCase;
			}
			if (scriptBlock != null)
			{
				return ParserOps.SplitWithPredicate(context, errorPosition, content, scriptBlock, limit);
			}
			return ParserOps.SplitWithPattern(context, errorPosition, content, separatorPattern, limit, splitOptions);
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x00118A04 File Offset: 0x00116C04
		private static object SplitWithPredicate(ExecutionContext context, IScriptExtent errorPosition, IEnumerable<string> content, ScriptBlock predicate, int limit)
		{
			List<string> list = new List<string>();
			foreach (string text in content)
			{
				List<string> list2 = new List<string>();
				if (limit == 1)
				{
					list.Add(text);
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < text.Length; i++)
					{
						object obj = predicate.DoInvokeReturnAsIs(true, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, ParserOps.CharToString(text[i]), AutomationNull.Value, AutomationNull.Value, new object[]
						{
							text,
							i
						});
						if (LanguagePrimitives.IsTrue(obj))
						{
							list2.Add(stringBuilder.ToString());
							stringBuilder = new StringBuilder();
							if (limit > 0 && list2.Count >= limit - 1)
							{
								if (i + 1 < text.Length)
								{
									list2.Add(text.Substring(i + 1));
									break;
								}
								list2.Add("");
								break;
							}
							else if (i == text.Length - 1)
							{
								list2.Add("");
							}
						}
						else
						{
							stringBuilder.Append(text[i]);
						}
					}
					if (stringBuilder.Length > 0 && (limit <= 0 || list2.Count < limit))
					{
						list2.Add(stringBuilder.ToString());
					}
					ParserOps.ExtendList<string>(list, list2);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x00118B88 File Offset: 0x00116D88
		private static object SplitWithPattern(ExecutionContext context, IScriptExtent errorPosition, IEnumerable<string> content, string separatorPattern, int limit, SplitOptions options)
		{
			if ((options & SplitOptions.SimpleMatch) == (SplitOptions)0 && (options & SplitOptions.RegexMatch) == (SplitOptions)0)
			{
				options |= SplitOptions.RegexMatch;
			}
			if ((options & SplitOptions.SimpleMatch) != (SplitOptions)0 && (options & ~(SplitOptions.SimpleMatch | SplitOptions.IgnoreCase)) != (SplitOptions)0)
			{
				throw InterpreterError.NewInterpreterException(null, typeof(ParseException), errorPosition, "InvalidSplitOptionCombination", ParserStrings.InvalidSplitOptionCombination, new object[0]);
			}
			if ((options & (SplitOptions.Multiline | SplitOptions.Singleline)) == (SplitOptions.Multiline | SplitOptions.Singleline))
			{
				throw InterpreterError.NewInterpreterException(null, typeof(ParseException), errorPosition, "InvalidSplitOptionCombination", ParserStrings.InvalidSplitOptionCombination, new object[0]);
			}
			if ((options & SplitOptions.SimpleMatch) != (SplitOptions)0)
			{
				separatorPattern = Regex.Escape(separatorPattern);
			}
			if (limit < 0)
			{
				limit = 0;
			}
			RegexOptions options2 = ParserOps.parseRegexOptions(options);
			Regex regex = ParserOps.NewRegex(separatorPattern, options2);
			List<string> list = new List<string>();
			foreach (string input in content)
			{
				string[] items = regex.Split(input, limit, 0);
				ParserOps.ExtendList<string>(list, items);
			}
			return list.ToArray();
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x00118C80 File Offset: 0x00116E80
		internal static object UnaryJoinOperator(ExecutionContext context, IScriptExtent errorPosition, object lval)
		{
			return ParserOps.JoinOperator(context, errorPosition, lval, "");
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x00118C90 File Offset: 0x00116E90
		internal static object JoinOperator(ExecutionContext context, IScriptExtent errorPosition, object lval, object rval)
		{
			string separator = PSObject.ToStringParser(context, rval);
			IEnumerable enumerable = LanguagePrimitives.GetEnumerable(lval);
			if (enumerable != null)
			{
				return PSObject.ToStringEnumerable(context, enumerable, separator, null, null);
			}
			return PSObject.ToStringParser(context, lval);
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x00118CC4 File Offset: 0x00116EC4
		internal static object ReplaceOperator(ExecutionContext context, IScriptExtent errorPosition, object lval, object rval, bool ignoreCase)
		{
			string replacement = "";
			object obj = "";
			rval = PSObject.Base(rval);
			IList list = rval as IList;
			if (list != null)
			{
				if (list.Count > 2)
				{
					throw InterpreterError.NewInterpreterException(rval, typeof(RuntimeException), errorPosition, "BadReplaceArgument", ParserStrings.BadReplaceArgument, new object[]
					{
						ignoreCase ? "-ireplace" : "-replace",
						list.Count
					});
				}
				if (list.Count > 0)
				{
					obj = list[0];
					if (list.Count > 1)
					{
						replacement = PSObject.ToStringParser(context, list[1]);
					}
				}
			}
			else
			{
				obj = rval;
			}
			RegexOptions options = RegexOptions.None;
			if (ignoreCase)
			{
				options = RegexOptions.IgnoreCase;
			}
			Regex regex = obj as Regex;
			if (regex == null)
			{
				try
				{
					regex = ParserOps.NewRegex(PSObject.ToStringParser(context, obj), options);
				}
				catch (ArgumentException innerException)
				{
					throw InterpreterError.NewInterpreterExceptionWithInnerException(obj, typeof(RuntimeException), null, "InvalidRegularExpression", ParserStrings.InvalidRegularExpression, innerException, new object[]
					{
						obj
					});
				}
			}
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(lval);
			if (enumerator == null)
			{
				string input = ((lval == null) ? string.Empty : lval).ToString();
				return regex.Replace(input, replacement);
			}
			List<object> list2 = new List<object>();
			while (ParserOps.MoveNext(context, errorPosition, enumerator))
			{
				string input2 = PSObject.ToStringParser(context, ParserOps.Current(errorPosition, enumerator));
				list2.Add(regex.Replace(input2, replacement));
			}
			return list2.ToArray();
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x00118E38 File Offset: 0x00117038
		internal static object IsOperator(ExecutionContext context, IScriptExtent errorPosition, object left, object right)
		{
			object obj = PSObject.Base(left);
			object obj2 = PSObject.Base(right);
			Type type = obj2 as Type;
			if (type == null)
			{
				type = ParserOps.ConvertTo<Type>(obj2, errorPosition);
				if (type == null)
				{
					throw InterpreterError.NewInterpreterException(obj2, typeof(RuntimeException), errorPosition, "IsOperatorRequiresType", ParserStrings.IsOperatorRequiresType, new object[0]);
				}
			}
			if (type == typeof(PSCustomObject) && obj is PSObject)
			{
				return ParserOps._TrueObject;
			}
			if (type.Equals(typeof(PSObject)) && left is PSObject)
			{
				return ParserOps._TrueObject;
			}
			return ParserOps.BoolToObject(type.IsInstanceOfType(obj));
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x00118EE4 File Offset: 0x001170E4
		internal static object IsNotOperator(ExecutionContext context, IScriptExtent errorPosition, object left, object right)
		{
			object obj = PSObject.Base(left);
			object obj2 = PSObject.Base(right);
			Type type = obj2 as Type;
			if (type == null)
			{
				type = ParserOps.ConvertTo<Type>(obj2, errorPosition);
				if (type == null)
				{
					throw InterpreterError.NewInterpreterException(obj2, typeof(RuntimeException), errorPosition, "IsOperatorRequiresType", ParserStrings.IsOperatorRequiresType, new object[0]);
				}
			}
			if (type == typeof(PSCustomObject) && obj is PSObject)
			{
				return ParserOps._FalseObject;
			}
			if (type.Equals(typeof(PSObject)) && left is PSObject)
			{
				return ParserOps._FalseObject;
			}
			return ParserOps.BoolToObject(!type.IsInstanceOfType(obj));
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x00118F94 File Offset: 0x00117194
		internal static object LikeOperator(ExecutionContext context, IScriptExtent errorPosition, object lval, object rval, bool notLike, bool ignoreCase)
		{
			WildcardPattern wildcardPattern = new WildcardPattern(PSObject.ToStringParser(context, rval), ignoreCase ? WildcardOptions.IgnoreCase : WildcardOptions.None);
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(lval);
			if (enumerator == null)
			{
				string input = (lval == null) ? string.Empty : PSObject.ToStringParser(context, lval);
				return ParserOps.BoolToObject(wildcardPattern.IsMatch(input) ^ notLike);
			}
			List<object> list = new List<object>();
			while (ParserOps.MoveNext(context, errorPosition, enumerator))
			{
				object obj = ParserOps.Current(errorPosition, enumerator);
				string text = (obj == null) ? string.Empty : PSObject.ToStringParser(context, obj);
				if (wildcardPattern.IsMatch(text) ^ notLike)
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x00119030 File Offset: 0x00117230
		internal static object MatchOperator(ExecutionContext context, IScriptExtent errorPosition, object lval, object rval, bool notMatch, bool ignoreCase)
		{
			RegexOptions options = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
			Regex regex = PSObject.Base(rval) as Regex;
			if (regex == null)
			{
				regex = ParserOps.NewRegex(PSObject.ToStringParser(context, rval), options);
			}
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(lval);
			if (enumerator == null)
			{
				string input = (lval == null) ? string.Empty : PSObject.ToStringParser(context, lval);
				Match match = regex.Match(input);
				if (match.Success)
				{
					GroupCollection groups = match.Groups;
					if (groups.Count > 0)
					{
						Hashtable hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
						foreach (string text in regex.GetGroupNames())
						{
							Group group = groups[text];
							if (group.Success)
							{
								int num;
								if (int.TryParse(text, out num))
								{
									hashtable.Add(num, group.ToString());
								}
								else
								{
									hashtable.Add(text, group.ToString());
								}
							}
						}
						context.SetVariable(SpecialVariables.MatchesVarPath, hashtable);
					}
				}
				return ParserOps.BoolToObject(match.Success ^ notMatch);
			}
			List<object> list = new List<object>();
			int num2 = 0;
			object result;
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					string input2 = (obj == null) ? string.Empty : PSObject.ToStringParser(context, obj);
					Match match2 = regex.Match(input2);
					if (match2.Success ^ notMatch)
					{
						list.Add(obj);
					}
					if (num2++ > 1000)
					{
						if (context != null && context.CurrentPipelineStopping)
						{
							throw new PipelineStoppedException();
						}
						num2 = 0;
					}
				}
				result = list.ToArray();
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
				throw InterpreterError.NewInterpreterExceptionWithInnerException(enumerator, typeof(RuntimeException), errorPosition, "BadEnumeration", ParserStrings.BadEnumeration, ex, new object[]
				{
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x00119238 File Offset: 0x00117438
		internal static bool ContainsOperatorCompiled(ExecutionContext context, CallSite<Func<CallSite, object, IEnumerator>> getEnumeratorSite, CallSite<Func<CallSite, object, object, object>> comparerSite, object left, object right)
		{
			IEnumerator enumerator = getEnumeratorSite.Target(getEnumeratorSite, left);
			if (enumerator == null || enumerator is EnumerableOps.NonEnumerableObjectEnumerator)
			{
				return (bool)comparerSite.Target(comparerSite, left, right);
			}
			while (EnumerableOps.MoveNext(context, enumerator))
			{
				object arg = EnumerableOps.Current(enumerator);
				if ((bool)comparerSite.Target(comparerSite, arg, right))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x0011929C File Offset: 0x0011749C
		internal static object ContainsOperator(ExecutionContext context, IScriptExtent errorPosition, object left, object right, bool contains, bool ignoreCase)
		{
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(left);
			if (enumerator == null)
			{
				return ParserOps.BoolToObject(contains == LanguagePrimitives.Equals(left, right, ignoreCase, CultureInfo.InvariantCulture));
			}
			while (ParserOps.MoveNext(context, errorPosition, enumerator))
			{
				object first = ParserOps.Current(errorPosition, enumerator);
				if (LanguagePrimitives.Equals(first, right, ignoreCase, CultureInfo.InvariantCulture))
				{
					return ParserOps.BoolToObject(contains);
				}
			}
			return ParserOps.BoolToObject(!contains);
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x00119300 File Offset: 0x00117500
		internal static object CompareOperators(ExecutionContext context, IScriptExtent errorPosition, object left, object right, ParserOps.CompareDelegate compareDelegate, bool ignoreCase)
		{
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(left);
			if (enumerator == null)
			{
				return ParserOps.BoolToObject(compareDelegate(left, right, ignoreCase));
			}
			List<object> list = new List<object>();
			while (ParserOps.MoveNext(context, errorPosition, enumerator))
			{
				object obj = ParserOps.Current(errorPosition, enumerator);
				if (compareDelegate(obj, right, ignoreCase))
				{
					list.Add(obj);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x0011935C File Offset: 0x0011755C
		internal static Regex NewRegex(string patternString, RegexOptions options)
		{
			if (options != RegexOptions.IgnoreCase)
			{
				return new Regex(patternString, options);
			}
			Regex result;
			lock (ParserOps._regexCache)
			{
				if (ParserOps._regexCache.ContainsKey(patternString))
				{
					result = ParserOps._regexCache[patternString];
				}
				else
				{
					if (ParserOps._regexCache.Count > 1000)
					{
						ParserOps._regexCache.Clear();
					}
					Regex regex = new Regex(patternString, RegexOptions.IgnoreCase);
					ParserOps._regexCache.Add(patternString, regex);
					result = regex;
				}
			}
			return result;
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x001193F0 File Offset: 0x001175F0
		internal static bool MoveNext(ExecutionContext context, IScriptExtent errorPosition, IEnumerator enumerator)
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
				throw InterpreterError.NewInterpreterExceptionWithInnerException(enumerator, typeof(RuntimeException), errorPosition, "BadEnumeration", ParserStrings.BadEnumeration, ex, new object[]
				{
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x0011948C File Offset: 0x0011768C
		internal static object Current(IScriptExtent errorPosition, IEnumerator enumerator)
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
				throw InterpreterError.NewInterpreterExceptionWithInnerException(enumerator, typeof(RuntimeException), errorPosition, "BadEnumeration", ParserStrings.BadEnumeration, ex, new object[]
				{
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x00119518 File Offset: 0x00117718
		internal static string GetTypeFullName(object obj)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			PSObject psobject = obj as PSObject;
			if (psobject == null)
			{
				return obj.GetType().FullName;
			}
			if (psobject.InternalTypeNames.Count == 0)
			{
				return typeof(PSObject).FullName;
			}
			return psobject.InternalTypeNames[0];
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x00119570 File Offset: 0x00117770
		internal static object CallMethod(IScriptExtent errorPosition, object target, string methodName, PSMethodInvocationConstraints invocationConstraints, object[] paramArray, bool callStatic, object valueToSet)
		{
			if (LanguagePrimitives.IsNull(target))
			{
				throw InterpreterError.NewInterpreterException(methodName, typeof(RuntimeException), errorPosition, "InvokeMethodOnNull", ParserStrings.InvokeMethodOnNull, new object[0]);
			}
			object obj = PSObject.Base(target);
			PSObject psobject = PSObject.AsPSObject(target);
			Type type;
			if (callStatic)
			{
				type = (Type)obj;
			}
			else
			{
				type = obj.GetType();
			}
			PSMethodInfo psmethodInfo;
			if (callStatic)
			{
				psmethodInfo = (PSObject.GetStaticCLRMember(target, methodName) as PSMethod);
			}
			else
			{
				psmethodInfo = (psobject.Members[methodName] as PSMethodInfo);
			}
			if (psmethodInfo != null)
			{
				object result;
				try
				{
					if (valueToSet != AutomationNull.Value)
					{
						PSParameterizedProperty psparameterizedProperty = psmethodInfo as PSParameterizedProperty;
						if (psparameterizedProperty == null)
						{
							throw InterpreterError.NewInterpreterException(methodName, typeof(RuntimeException), errorPosition, "ParameterizedPropertyAssignmentFailed", ParserStrings.ParameterizedPropertyAssignmentFailed, new object[]
							{
								ParserOps.GetTypeFullName(target),
								methodName
							});
						}
						psparameterizedProperty.InvokeSet(valueToSet, paramArray);
						result = valueToSet;
					}
					else
					{
						PSMethod psmethod = psmethodInfo as PSMethod;
						if (psmethod != null)
						{
							result = psmethod.Invoke(invocationConstraints, paramArray);
						}
						else
						{
							result = psmethodInfo.Invoke(paramArray);
						}
					}
				}
				catch (MethodInvocationException ex)
				{
					if (ex.ErrorRecord.InvocationInfo == null)
					{
						ex.ErrorRecord.SetInvocationInfo(new InvocationInfo(null, errorPosition));
					}
					throw;
				}
				catch (RuntimeException ex2)
				{
					if (ex2.ErrorRecord.InvocationInfo == null)
					{
						ex2.ErrorRecord.SetInvocationInfo(new InvocationInfo(null, errorPosition));
					}
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
				catch (Exception ex3)
				{
					CommandProcessorBase.CheckForSevereException(ex3);
					throw InterpreterError.NewInterpreterExceptionByMessage(typeof(RuntimeException), errorPosition, ex3.Message, "MethodInvocationException", ex3);
				}
				return result;
			}
			string text;
			if (callStatic)
			{
				text = type.FullName;
			}
			else
			{
				text = ParserOps.GetTypeFullName(target);
			}
			if (valueToSet == AutomationNull.Value)
			{
				throw InterpreterError.NewInterpreterException(methodName, typeof(RuntimeException), errorPosition, "MethodNotFound", ParserStrings.MethodNotFound, new object[]
				{
					text,
					methodName
				});
			}
			throw InterpreterError.NewInterpreterException(methodName, typeof(RuntimeException), errorPosition, "ParameterizedPropertyAssignmentFailed", ParserStrings.ParameterizedPropertyAssignmentFailed, new object[]
			{
				text,
				methodName
			});
		}

		// Token: 0x04001AA8 RID: 6824
		internal const string MethodNotFoundErrorId = "MethodNotFound";

		// Token: 0x04001AA9 RID: 6825
		private const int _MinCache = -100;

		// Token: 0x04001AAA RID: 6826
		private const int _MaxCache = 1000;

		// Token: 0x04001AAB RID: 6827
		private const int MaxRegexCache = 1000;

		// Token: 0x04001AAC RID: 6828
		private static readonly object[] _integerCache = new object[1100];

		// Token: 0x04001AAD RID: 6829
		private static readonly string[] _chars = new string[255];

		// Token: 0x04001AAE RID: 6830
		internal static readonly object _TrueObject = true;

		// Token: 0x04001AAF RID: 6831
		internal static readonly object _FalseObject = false;

		// Token: 0x04001AB0 RID: 6832
		private static Dictionary<string, Regex> _regexCache = new Dictionary<string, Regex>();

		// Token: 0x02000489 RID: 1161
		[Flags]
		private enum SplitImplOptions
		{
			// Token: 0x04001AB2 RID: 6834
			None = 0,
			// Token: 0x04001AB3 RID: 6835
			TrimContent = 1
		}

		// Token: 0x0200048A RID: 1162
		// (Invoke) Token: 0x0600337D RID: 13181
		internal delegate bool CompareDelegate(object lhs, object rhs, bool ignoreCase);
	}
}
