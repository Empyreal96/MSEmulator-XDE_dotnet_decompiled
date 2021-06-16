using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Resources;

namespace System.Management.Automation
{
	// Token: 0x0200048C RID: 1164
	internal static class InterpreterError
	{
		// Token: 0x06003387 RID: 13191 RVA: 0x0011986C File Offset: 0x00117A6C
		internal static RuntimeException NewInterpreterException(object targetObject, Type exceptionType, IScriptExtent errorPosition, string resourceIdAndErrorId, string resourceString, params object[] args)
		{
			return InterpreterError.NewInterpreterExceptionWithInnerException(targetObject, exceptionType, errorPosition, resourceIdAndErrorId, resourceString, null, args);
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x0011987C File Offset: 0x00117A7C
		internal static RuntimeException NewInterpreterExceptionWithInnerException(object targetObject, Type exceptionType, IScriptExtent errorPosition, string resourceIdAndErrorId, string resourceString, Exception innerException, params object[] args)
		{
			if (string.IsNullOrEmpty(resourceIdAndErrorId))
			{
				throw PSTraceSource.NewArgumentException("resourceIdAndErrorId");
			}
			RuntimeException ex = null;
			try
			{
				string text;
				if (args == null || args.Length == 0)
				{
					text = resourceString;
				}
				else
				{
					text = StringUtil.Format(resourceString, args);
				}
				if (string.IsNullOrEmpty(text))
				{
					ex = InterpreterError.NewBackupInterpreterException(exceptionType, errorPosition, resourceIdAndErrorId, null);
				}
				else
				{
					ex = InterpreterError.NewInterpreterExceptionByMessage(exceptionType, errorPosition, text, resourceIdAndErrorId, innerException);
				}
			}
			catch (InvalidOperationException innerException2)
			{
				ex = InterpreterError.NewBackupInterpreterException(exceptionType, errorPosition, resourceIdAndErrorId, innerException2);
			}
			catch (MissingManifestResourceException innerException3)
			{
				ex = InterpreterError.NewBackupInterpreterException(exceptionType, errorPosition, resourceIdAndErrorId, innerException3);
			}
			catch (FormatException innerException4)
			{
				ex = InterpreterError.NewBackupInterpreterException(exceptionType, errorPosition, resourceIdAndErrorId, innerException4);
			}
			ex.SetTargetObject(targetObject);
			return ex;
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x00119934 File Offset: 0x00117B34
		internal static RuntimeException NewInterpreterExceptionByMessage(Type exceptionType, IScriptExtent errorPosition, string message, string errorId, Exception innerException)
		{
			RuntimeException ex;
			if (exceptionType == typeof(ParseException))
			{
				ex = new ParseException(message, errorId, innerException);
			}
			else if (exceptionType == typeof(IncompleteParseException))
			{
				ex = new IncompleteParseException(message, errorId, innerException);
			}
			else
			{
				ex = new RuntimeException(message, innerException);
				ex.SetErrorId(errorId);
				ex.SetErrorCategory(ErrorCategory.InvalidOperation);
			}
			if (errorPosition != null)
			{
				ex.ErrorRecord.SetInvocationInfo(new InvocationInfo(null, errorPosition));
			}
			return ex;
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x001199AC File Offset: 0x00117BAC
		private static RuntimeException NewBackupInterpreterException(Type exceptionType, IScriptExtent errorPosition, string errorId, Exception innerException)
		{
			string message;
			if (innerException == null)
			{
				message = StringUtil.Format(ParserStrings.BackupParserMessage, errorId);
			}
			else
			{
				message = StringUtil.Format(ParserStrings.BackupParserMessageWithException, errorId, innerException.Message);
			}
			return InterpreterError.NewInterpreterExceptionByMessage(exceptionType, errorPosition, message, errorId, innerException);
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x001199E8 File Offset: 0x00117BE8
		internal static void UpdateExceptionErrorRecordPosition(Exception exception, IScriptExtent extent)
		{
			if (extent == null || extent == PositionUtilities.EmptyExtent)
			{
				return;
			}
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null)
			{
				ErrorRecord errorRecord = containsErrorRecord.ErrorRecord;
				InvocationInfo invocationInfo = errorRecord.InvocationInfo;
				if (invocationInfo == null)
				{
					errorRecord.SetInvocationInfo(new InvocationInfo(null, extent));
					return;
				}
				if (invocationInfo.ScriptPosition == null || invocationInfo.ScriptPosition == PositionUtilities.EmptyExtent)
				{
					invocationInfo.ScriptPosition = extent;
					errorRecord.LockScriptStackTrace();
				}
			}
		}
	}
}
