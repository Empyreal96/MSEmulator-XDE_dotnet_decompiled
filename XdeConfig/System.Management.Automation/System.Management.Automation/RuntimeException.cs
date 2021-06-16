using System;
using System.Management.Automation.Language;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000063 RID: 99
	[Serializable]
	public class RuntimeException : SystemException, IContainsErrorRecord
	{
		// Token: 0x0600055E RID: 1374 RVA: 0x000199CF File Offset: 0x00017BCF
		public RuntimeException()
		{
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x000199E2 File Offset: 0x00017BE2
		protected RuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorId = info.GetString("ErrorId");
			this._errorCategory = (ErrorCategory)info.GetInt32("ErrorCategory");
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00019A19 File Offset: 0x00017C19
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ErrorId", this._errorId);
			info.AddValue("ErrorCategory", (int)this._errorCategory);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00019A53 File Offset: 0x00017C53
		public RuntimeException(string message) : base(message)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00019A67 File Offset: 0x00017C67
		public RuntimeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00019A7C File Offset: 0x00017C7C
		public RuntimeException(string message, Exception innerException, ErrorRecord errorRecord) : base(message, innerException)
		{
			this._errorRecord = errorRecord;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00019A98 File Offset: 0x00017C98
		internal RuntimeException(ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string errorIdAndResourceId, string message, Exception innerException) : base(message, innerException)
		{
			this.SetErrorCategory(errorCategory);
			this.SetErrorId(errorIdAndResourceId);
			if (errorPosition == null && invocationInfo != null)
			{
				errorPosition = invocationInfo.ScriptPosition;
			}
			if (invocationInfo == null)
			{
				return;
			}
			this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._errorId, this._errorCategory, this._targetObject);
			this._errorRecord.SetInvocationInfo(new InvocationInfo(invocationInfo.MyCommand, errorPosition));
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00019B15 File Offset: 0x00017D15
		public virtual ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._errorId, this._errorCategory, this._targetObject);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00019B48 File Offset: 0x00017D48
		internal void SetErrorId(string errorId)
		{
			if (this._errorId != errorId)
			{
				this._errorId = errorId;
				this._errorRecord = null;
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00019B66 File Offset: 0x00017D66
		internal void SetErrorCategory(ErrorCategory errorCategory)
		{
			if (this._errorCategory != errorCategory)
			{
				this._errorCategory = errorCategory;
				this._errorRecord = null;
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00019B7F File Offset: 0x00017D7F
		internal void SetTargetObject(object targetObject)
		{
			this._targetObject = targetObject;
			if (this._errorRecord != null)
			{
				this._errorRecord.SetTargetObject(targetObject);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x00019B9C File Offset: 0x00017D9C
		public override string StackTrace
		{
			get
			{
				if (!string.IsNullOrEmpty(this._overrideStackTrace))
				{
					return this._overrideStackTrace;
				}
				return base.StackTrace;
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00019BB8 File Offset: 0x00017DB8
		internal static void LockStackTrace(Exception e)
		{
			RuntimeException ex = e as RuntimeException;
			if (ex != null && string.IsNullOrEmpty(ex._overrideStackTrace))
			{
				string stackTrace = ex.StackTrace;
				if (!string.IsNullOrEmpty(stackTrace))
				{
					ex._overrideStackTrace = stackTrace;
				}
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00019BF4 File Offset: 0x00017DF4
		internal static string RetrieveMessage(ErrorRecord errorRecord)
		{
			if (errorRecord == null)
			{
				return "";
			}
			if (errorRecord.ErrorDetails != null && !string.IsNullOrEmpty(errorRecord.ErrorDetails.Message))
			{
				return errorRecord.ErrorDetails.Message;
			}
			if (errorRecord.Exception == null)
			{
				return "";
			}
			return errorRecord.Exception.Message;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00019C4C File Offset: 0x00017E4C
		internal static string RetrieveMessage(Exception e)
		{
			if (e == null)
			{
				return "";
			}
			IContainsErrorRecord containsErrorRecord = e as IContainsErrorRecord;
			if (containsErrorRecord == null)
			{
				return e.Message;
			}
			ErrorRecord errorRecord = containsErrorRecord.ErrorRecord;
			if (errorRecord == null)
			{
				return e.Message;
			}
			ErrorDetails errorDetails = errorRecord.ErrorDetails;
			if (errorDetails == null)
			{
				return e.Message;
			}
			string message = errorDetails.Message;
			if (!string.IsNullOrEmpty(message))
			{
				return message;
			}
			return e.Message;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00019CAC File Offset: 0x00017EAC
		internal static Exception RetrieveException(ErrorRecord errorRecord)
		{
			if (errorRecord == null)
			{
				return null;
			}
			return errorRecord.Exception;
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00019CB9 File Offset: 0x00017EB9
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x00019CC4 File Offset: 0x00017EC4
		public bool WasThrownFromThrowStatement
		{
			get
			{
				return this.thrownByThrowStatement;
			}
			set
			{
				this.thrownByThrowStatement = value;
				if (this._errorRecord != null)
				{
					RuntimeException ex = this._errorRecord.Exception as RuntimeException;
					if (ex != null)
					{
						ex.WasThrownFromThrowStatement = value;
					}
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00019CFB File Offset: 0x00017EFB
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x00019D03 File Offset: 0x00017F03
		internal bool SuppressPromptInInterpreter
		{
			get
			{
				return this.suppressPromptInInterpreter;
			}
			set
			{
				this.suppressPromptInInterpreter = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00019D0C File Offset: 0x00017F0C
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x00019D14 File Offset: 0x00017F14
		internal Token ErrorToken
		{
			get
			{
				return this._errorToken;
			}
			set
			{
				this._errorToken = value;
			}
		}

		// Token: 0x0400022A RID: 554
		private ErrorRecord _errorRecord;

		// Token: 0x0400022B RID: 555
		private string _errorId = "RuntimeException";

		// Token: 0x0400022C RID: 556
		private ErrorCategory _errorCategory;

		// Token: 0x0400022D RID: 557
		private object _targetObject;

		// Token: 0x0400022E RID: 558
		private string _overrideStackTrace;

		// Token: 0x0400022F RID: 559
		private bool thrownByThrowStatement;

		// Token: 0x04000230 RID: 560
		private bool suppressPromptInInterpreter;

		// Token: 0x04000231 RID: 561
		private Token _errorToken;
	}
}
