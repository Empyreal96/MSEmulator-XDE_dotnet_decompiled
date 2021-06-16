using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Text;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A2 RID: 1186
	internal sealed class ParameterProcessor
	{
		// Token: 0x060034F7 RID: 13559 RVA: 0x0011F704 File Offset: 0x0011D904
		internal static void ThrowParameterBindingException(TerminatingErrorContext invocationContext, string errorId, string msg)
		{
			invocationContext.ThrowTerminatingError(new ErrorRecord(new NotSupportedException(), errorId, ErrorCategory.InvalidArgument, null)
			{
				ErrorDetails = new ErrorDetails(msg)
			});
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x0011F732 File Offset: 0x0011D932
		internal ParameterProcessor(CommandParameterDefinition p)
		{
			this.paramDef = p;
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x0011F744 File Offset: 0x0011D944
		internal List<MshParameter> ProcessParameters(object[] p, TerminatingErrorContext invocationContext)
		{
			if (p == null || p.Length == 0)
			{
				return null;
			}
			List<MshParameter> list = new List<MshParameter>();
			bool originalParameterWasHashTable = false;
			for (int i = 0; i < p.Length; i++)
			{
				MshParameter mshParameter = this.paramDef.CreateInstance();
				object obj = PSObject.Base(p[i]);
				if (obj is IDictionary)
				{
					originalParameterWasHashTable = true;
					mshParameter.hash = this.VerifyHashTable((IDictionary)obj, invocationContext);
				}
				else if (obj != null && ParameterProcessor.MatchesAllowedTypes(obj.GetType(), this.paramDef.hashEntries[0].AllowedTypes))
				{
					mshParameter.hash = this.paramDef.hashEntries[0].CreateHashtableFromSingleType(obj);
				}
				else
				{
					ParameterProcessor.ProcessUnknownParameterType(invocationContext, obj, this.paramDef.hashEntries[0].AllowedTypes);
				}
				this.VerifyAndNormalizeParameter(mshParameter, invocationContext, originalParameterWasHashTable);
				list.Add(mshParameter);
			}
			return list;
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x0011F828 File Offset: 0x0011DA28
		private static bool MatchesAllowedTypes(Type t, Type[] allowedTypes)
		{
			for (int i = 0; i < allowedTypes.Length; i++)
			{
				if (allowedTypes[i].IsAssignableFrom(t))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x0011F854 File Offset: 0x0011DA54
		private Hashtable VerifyHashTable(IDictionary hash, TerminatingErrorContext invocationContext)
		{
			Hashtable hashtable = new Hashtable();
			foreach (object obj in hash)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (dictionaryEntry.Key == null)
				{
					ParameterProcessor.ProcessNullHashTableKey(invocationContext);
				}
				string text = dictionaryEntry.Key as string;
				if (text == null)
				{
					ParameterProcessor.ProcessNonStringHashTableKey(invocationContext, dictionaryEntry.Key);
				}
				HashtableEntryDefinition hashtableEntryDefinition = this.paramDef.MatchEntry(text, invocationContext);
				if (hashtable.Contains(hashtableEntryDefinition.KeyName))
				{
					ParameterProcessor.ProcessDuplicateHashTableKey(invocationContext, text, hashtableEntryDefinition.KeyName);
				}
				bool flag = false;
				if (hashtableEntryDefinition.AllowedTypes == null || hashtableEntryDefinition.AllowedTypes.Length == 0)
				{
					flag = true;
				}
				else
				{
					for (int i = 0; i < hashtableEntryDefinition.AllowedTypes.Length; i++)
					{
						if (dictionaryEntry.Value == null)
						{
							ParameterProcessor.ProcessMissingKeyValue(invocationContext, text);
						}
						if (hashtableEntryDefinition.AllowedTypes[i].IsAssignableFrom(dictionaryEntry.Value.GetType()))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					ParameterProcessor.ProcessIllegalHashTableKeyValue(invocationContext, text, dictionaryEntry.Value.GetType(), hashtableEntryDefinition.AllowedTypes);
				}
				hashtable.Add(hashtableEntryDefinition.KeyName, dictionaryEntry.Value);
			}
			return hashtable;
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x0011F9A8 File Offset: 0x0011DBA8
		private void VerifyAndNormalizeParameter(MshParameter parameter, TerminatingErrorContext invocationContext, bool originalParameterWasHashTable)
		{
			for (int i = 0; i < this.paramDef.hashEntries.Count; i++)
			{
				if (parameter.hash.ContainsKey(this.paramDef.hashEntries[i].KeyName))
				{
					object val = parameter.hash[this.paramDef.hashEntries[i].KeyName];
					object obj = this.paramDef.hashEntries[i].Verify(val, invocationContext, originalParameterWasHashTable);
					if (obj != null)
					{
						parameter.hash[this.paramDef.hashEntries[i].KeyName] = obj;
					}
				}
				else
				{
					object obj2 = this.paramDef.hashEntries[i].ComputeDefaultValue();
					if (obj2 != AutomationNull.Value)
					{
						parameter.hash[this.paramDef.hashEntries[i].KeyName] = obj2;
					}
					else if (this.paramDef.hashEntries[i].Mandatory)
					{
						ParameterProcessor.ProcessMissingMandatoryKey(invocationContext, this.paramDef.hashEntries[i].KeyName);
					}
				}
			}
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x0011FAD8 File Offset: 0x0011DCD8
		private static void ProcessUnknownParameterType(TerminatingErrorContext invocationContext, object actualObject, Type[] allowedTypes)
		{
			string text = ParameterProcessor.CatenateTypeArray(allowedTypes);
			string msg;
			if (actualObject != null)
			{
				msg = StringUtil.Format(FormatAndOut_MshParameter.UnknownParameterTypeError, actualObject.GetType().FullName, text);
			}
			else
			{
				msg = StringUtil.Format(FormatAndOut_MshParameter.NullParameterTypeError, text);
			}
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "DictionaryKeyUnknownType", msg);
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x0011FB20 File Offset: 0x0011DD20
		private static void ProcessDuplicateHashTableKey(TerminatingErrorContext invocationContext, string duplicateKey, string existingKey)
		{
			string msg = StringUtil.Format(FormatAndOut_MshParameter.DuplicateKeyError, duplicateKey, existingKey);
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "DictionaryKeyDuplicate", msg);
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x0011FB48 File Offset: 0x0011DD48
		private static void ProcessNullHashTableKey(TerminatingErrorContext invocationContext)
		{
			string msg = StringUtil.Format(FormatAndOut_MshParameter.DictionaryKeyNullError, new object[0]);
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "DictionaryKeyNull", msg);
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x0011FB74 File Offset: 0x0011DD74
		private static void ProcessNonStringHashTableKey(TerminatingErrorContext invocationContext, object key)
		{
			string msg = StringUtil.Format(FormatAndOut_MshParameter.DictionaryKeyNonStringError, key.GetType().Name);
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "DictionaryKeyNonString", msg);
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x0011FBA4 File Offset: 0x0011DDA4
		private static void ProcessIllegalHashTableKeyValue(TerminatingErrorContext invocationContext, string key, Type actualType, Type[] allowedTypes)
		{
			string msg;
			string errorId;
			if (allowedTypes.Length > 1)
			{
				string text = ParameterProcessor.CatenateTypeArray(allowedTypes);
				msg = StringUtil.Format(FormatAndOut_MshParameter.IllegalTypeMultiError, new object[]
				{
					key,
					actualType.FullName,
					text
				});
				errorId = "DictionaryKeyIllegalValue1";
			}
			else
			{
				msg = StringUtil.Format(FormatAndOut_MshParameter.IllegalTypeSingleError, new object[]
				{
					key,
					actualType.FullName,
					allowedTypes[0]
				});
				errorId = "DictionaryKeyIllegalValue2";
			}
			ParameterProcessor.ThrowParameterBindingException(invocationContext, errorId, msg);
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x0011FC24 File Offset: 0x0011DE24
		private static void ProcessMissingKeyValue(TerminatingErrorContext invocationContext, string keyName)
		{
			string msg = StringUtil.Format(FormatAndOut_MshParameter.MissingKeyValueError, keyName);
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "DictionaryKeyMissingValue", msg);
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x0011FC4C File Offset: 0x0011DE4C
		private static void ProcessMissingMandatoryKey(TerminatingErrorContext invocationContext, string keyName)
		{
			string msg = StringUtil.Format(FormatAndOut_MshParameter.MissingKeyMandatoryEntryError, keyName);
			ParameterProcessor.ThrowParameterBindingException(invocationContext, "DictionaryKeyMandatoryEntry", msg);
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x0011FC74 File Offset: 0x0011DE74
		private static string CatenateTypeArray(Type[] arr)
		{
			string[] array = new string[arr.Length];
			for (int i = 0; i < arr.Length; i++)
			{
				array[i] = arr[i].FullName;
			}
			return ParameterProcessor.CatenateStringArray(array);
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x0011FCAC File Offset: 0x0011DEAC
		internal static string CatenateStringArray(string[] arr)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			for (int i = 0; i < arr.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(arr[i]);
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x04001B1E RID: 6942
		[TraceSource("ParameterProcessor", "ParameterProcessor")]
		internal static PSTraceSource tracer = PSTraceSource.GetTracer("ParameterProcessor", "ParameterProcessor");

		// Token: 0x04001B1F RID: 6943
		private CommandParameterDefinition paramDef;
	}
}
