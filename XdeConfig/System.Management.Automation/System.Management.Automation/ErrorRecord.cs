using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public class ErrorRecord : ISerializable
	{
		// Token: 0x06000180 RID: 384 RVA: 0x000074B4 File Offset: 0x000056B4
		private ErrorRecord()
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000074D0 File Offset: 0x000056D0
		public ErrorRecord(Exception exception, string errorId, ErrorCategory errorCategory, object targetObject)
		{
			if (exception == null)
			{
				throw PSTraceSource.NewArgumentNullException("exception");
			}
			if (errorId == null)
			{
				errorId = "";
			}
			this._error = exception;
			this._errorId = errorId;
			this._category = errorCategory;
			this._target = targetObject;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000752C File Offset: 0x0000572C
		protected ErrorRecord(SerializationInfo info, StreamingContext context)
		{
			PSObject serializedErrorRecord = PSObject.ConstructPSObjectFromSerializationInfo(info, context);
			this.ConstructFromPSObjectForRemoting(serializedErrorRecord);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007560 File Offset: 0x00005760
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info != null)
			{
				PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
				this.ToPSObjectForRemoting(psobject, true);
				psobject.GetObjectData(info, context);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00007586 File Offset: 0x00005786
		internal bool IsSerialized
		{
			get
			{
				return this._isSerialized;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007590 File Offset: 0x00005790
		internal ErrorRecord(Exception exception, object targetObject, string fullyQualifiedErrorId, ErrorCategory errorCategory, string errorCategory_Activity, string errorCategory_Reason, string errorCategory_TargetName, string errorCategory_TargetType, string errorCategory_Message, string errorDetails_Message, string errorDetails_RecommendedAction)
		{
			this.PopulateProperties(exception, targetObject, fullyQualifiedErrorId, errorCategory, errorCategory_Activity, errorCategory_Reason, errorCategory_TargetName, errorCategory_TargetType, errorDetails_Message, errorDetails_Message, errorDetails_RecommendedAction, null);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000075D0 File Offset: 0x000057D0
		private void PopulateProperties(Exception exception, object targetObject, string fullyQualifiedErrorId, ErrorCategory errorCategory, string errorCategory_Activity, string errorCategory_Reason, string errorCategory_TargetName, string errorCategory_TargetType, string errorCategory_Message, string errorDetails_Message, string errorDetails_RecommendedAction, string errorDetails_ScriptStackTrace)
		{
			if (exception == null)
			{
				throw PSTraceSource.NewArgumentNullException("exception");
			}
			if (fullyQualifiedErrorId == null)
			{
				throw PSTraceSource.NewArgumentNullException("fullyQualifiedErrorId");
			}
			this._isSerialized = true;
			this._error = exception;
			this._target = targetObject;
			this._serializedFullyQualifiedErrorId = fullyQualifiedErrorId;
			this._category = errorCategory;
			this._activityOverride = errorCategory_Activity;
			this._reasonOverride = errorCategory_Reason;
			this._targetNameOverride = errorCategory_TargetName;
			this._targetTypeOverride = errorCategory_TargetType;
			this._serializedErrorCategoryMessageOverride = errorCategory_Message;
			if (errorDetails_Message != null)
			{
				this._errorDetails = new ErrorDetails(errorDetails_Message);
				if (errorDetails_RecommendedAction != null)
				{
					this._errorDetails.RecommendedAction = errorDetails_RecommendedAction;
				}
			}
			this._scriptStackTrace = errorDetails_ScriptStackTrace;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000766F File Offset: 0x0000586F
		internal void ToPSObjectForRemoting(PSObject dest)
		{
			this.ToPSObjectForRemoting(dest, this.SerializeExtendedInfo);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007724 File Offset: 0x00005924
		private void ToPSObjectForRemoting(PSObject dest, bool serializeExtInfo)
		{
			RemotingEncoder.AddNoteProperty<Exception>(dest, "Exception", () => this.Exception);
			RemotingEncoder.AddNoteProperty<object>(dest, "TargetObject", () => this.TargetObject);
			RemotingEncoder.AddNoteProperty<string>(dest, "FullyQualifiedErrorId", () => this.FullyQualifiedErrorId);
			RemotingEncoder.AddNoteProperty<InvocationInfo>(dest, "InvocationInfo", () => this.InvocationInfo);
			RemotingEncoder.AddNoteProperty<int>(dest, "ErrorCategory_Category", () => (int)this.CategoryInfo.Category);
			RemotingEncoder.AddNoteProperty<string>(dest, "ErrorCategory_Activity", () => this.CategoryInfo.Activity);
			RemotingEncoder.AddNoteProperty<string>(dest, "ErrorCategory_Reason", () => this.CategoryInfo.Reason);
			RemotingEncoder.AddNoteProperty<string>(dest, "ErrorCategory_TargetName", () => this.CategoryInfo.TargetName);
			RemotingEncoder.AddNoteProperty<string>(dest, "ErrorCategory_TargetType", () => this.CategoryInfo.TargetType);
			RemotingEncoder.AddNoteProperty<string>(dest, "ErrorCategory_Message", () => this.CategoryInfo.GetMessage(CultureInfo.CurrentCulture));
			if (this.ErrorDetails != null)
			{
				RemotingEncoder.AddNoteProperty<string>(dest, "ErrorDetails_Message", () => this.ErrorDetails.Message);
				RemotingEncoder.AddNoteProperty<string>(dest, "ErrorDetails_RecommendedAction", () => this.ErrorDetails.RecommendedAction);
			}
			if (!serializeExtInfo || this.InvocationInfo == null)
			{
				RemotingEncoder.AddNoteProperty<bool>(dest, "SerializeExtendedInfo", () => false);
			}
			else
			{
				RemotingEncoder.AddNoteProperty<bool>(dest, "SerializeExtendedInfo", () => true);
				this.InvocationInfo.ToPSObjectForRemoting(dest);
				RemotingEncoder.AddNoteProperty<object>(dest, "PipelineIterationInfo", () => this.PipelineIterationInfo);
			}
			if (!string.IsNullOrEmpty(this.ScriptStackTrace))
			{
				RemotingEncoder.AddNoteProperty<string>(dest, "ErrorDetails_ScriptStackTrace", () => this.ScriptStackTrace);
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00007910 File Offset: 0x00005B10
		private static object GetNoteValue(PSObject mshObject, string note)
		{
			PSNoteProperty psnoteProperty = mshObject.Properties[note] as PSNoteProperty;
			if (psnoteProperty != null)
			{
				return psnoteProperty.Value;
			}
			return null;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000793C File Offset: 0x00005B3C
		internal static ErrorRecord FromPSObjectForRemoting(PSObject serializedErrorRecord)
		{
			ErrorRecord errorRecord = new ErrorRecord();
			errorRecord.ConstructFromPSObjectForRemoting(serializedErrorRecord);
			return errorRecord;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007958 File Offset: 0x00005B58
		private void ConstructFromPSObjectForRemoting(PSObject serializedErrorRecord)
		{
			if (serializedErrorRecord == null)
			{
				throw PSTraceSource.NewArgumentNullException("serializedErrorRecord");
			}
			PSObject propertyValue = RemotingDecoder.GetPropertyValue<PSObject>(serializedErrorRecord, "Exception");
			object propertyValue2 = RemotingDecoder.GetPropertyValue<object>(serializedErrorRecord, "TargetObject");
			PSObject propertyValue3 = RemotingDecoder.GetPropertyValue<PSObject>(serializedErrorRecord, "InvocationInfo");
			string text = null;
			if (propertyValue != null)
			{
				PSPropertyInfo pspropertyInfo = propertyValue.Properties["Message"];
				if (pspropertyInfo != null)
				{
					text = (pspropertyInfo.Value as string);
				}
			}
			string text2 = RemotingDecoder.GetPropertyValue<string>(serializedErrorRecord, "FullyQualifiedErrorId");
			if (text2 == null)
			{
				text2 = "fullyQualifiedErrorId";
			}
			ErrorCategory propertyValue4 = RemotingDecoder.GetPropertyValue<ErrorCategory>(serializedErrorRecord, "errorCategory_Category");
			string propertyValue5 = RemotingDecoder.GetPropertyValue<string>(serializedErrorRecord, "ErrorCategory_Activity");
			string propertyValue6 = RemotingDecoder.GetPropertyValue<string>(serializedErrorRecord, "ErrorCategory_Reason");
			string propertyValue7 = RemotingDecoder.GetPropertyValue<string>(serializedErrorRecord, "ErrorCategory_TargetName");
			string propertyValue8 = RemotingDecoder.GetPropertyValue<string>(serializedErrorRecord, "ErrorCategory_TargetType");
			string propertyValue9 = RemotingDecoder.GetPropertyValue<string>(serializedErrorRecord, "ErrorCategory_Message");
			string errorDetails_Message = ErrorRecord.GetNoteValue(serializedErrorRecord, "ErrorDetails_Message") as string;
			string errorDetails_RecommendedAction = ErrorRecord.GetNoteValue(serializedErrorRecord, "ErrorDetails_RecommendedAction") as string;
			string errorDetails_ScriptStackTrace = ErrorRecord.GetNoteValue(serializedErrorRecord, "ErrorDetails_ScriptStackTrace") as string;
			RemoteException ex = new RemoteException((!string.IsNullOrWhiteSpace(text)) ? text : propertyValue9, propertyValue, propertyValue3);
			this.PopulateProperties(ex, propertyValue2, text2, propertyValue4, propertyValue5, propertyValue6, propertyValue7, propertyValue8, propertyValue9, errorDetails_Message, errorDetails_RecommendedAction, errorDetails_ScriptStackTrace);
			ex.SetRemoteErrorRecord(this);
			this.serializeExtendedInfo = RemotingDecoder.GetPropertyValue<bool>(serializedErrorRecord, "SerializeExtendedInfo");
			if (this.serializeExtendedInfo)
			{
				this._invocationInfo = new InvocationInfo(serializedErrorRecord);
				ArrayList propertyValue10 = RemotingDecoder.GetPropertyValue<ArrayList>(serializedErrorRecord, "PipelineIterationInfo");
				if (propertyValue10 != null)
				{
					this.pipelineIterationInfo = new ReadOnlyCollection<int>((int[])propertyValue10.ToArray(typeof(int)));
					return;
				}
			}
			else
			{
				this._invocationInfo = null;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007AF8 File Offset: 0x00005CF8
		public ErrorRecord(ErrorRecord errorRecord, Exception replaceParentContainsErrorRecordException)
		{
			if (errorRecord == null)
			{
				throw new PSArgumentNullException("errorRecord");
			}
			if (replaceParentContainsErrorRecordException != null && errorRecord.Exception is ParentContainsErrorRecordException)
			{
				this._error = replaceParentContainsErrorRecordException;
			}
			else
			{
				this._error = errorRecord.Exception;
			}
			this._target = errorRecord.TargetObject;
			this._errorId = errorRecord._errorId;
			this._category = errorRecord._category;
			this._activityOverride = errorRecord._activityOverride;
			this._reasonOverride = errorRecord._reasonOverride;
			this._targetNameOverride = errorRecord._targetNameOverride;
			this._targetTypeOverride = errorRecord._targetTypeOverride;
			if (errorRecord.ErrorDetails != null)
			{
				this._errorDetails = new ErrorDetails(errorRecord.ErrorDetails);
			}
			this.SetInvocationInfo(errorRecord._invocationInfo);
			this._scriptStackTrace = errorRecord._scriptStackTrace;
			this._serializedFullyQualifiedErrorId = errorRecord._serializedFullyQualifiedErrorId;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007BE0 File Offset: 0x00005DE0
		internal virtual ErrorRecord WrapException(Exception replaceParentContainsErrorRecordException)
		{
			return new ErrorRecord(this, replaceParentContainsErrorRecordException);
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00007BE9 File Offset: 0x00005DE9
		public Exception Exception
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00007BF1 File Offset: 0x00005DF1
		public object TargetObject
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007BF9 File Offset: 0x00005DF9
		internal void SetTargetObject(object target)
		{
			this._target = target;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00007C02 File Offset: 0x00005E02
		public ErrorCategoryInfo CategoryInfo
		{
			get
			{
				if (this._categoryInfo == null)
				{
					this._categoryInfo = new ErrorCategoryInfo(this);
				}
				return this._categoryInfo;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00007C20 File Offset: 0x00005E20
		public string FullyQualifiedErrorId
		{
			get
			{
				if (this._serializedFullyQualifiedErrorId != null)
				{
					return this._serializedFullyQualifiedErrorId;
				}
				string invocationTypeName = this.GetInvocationTypeName();
				string str = (string.IsNullOrEmpty(invocationTypeName) || string.IsNullOrEmpty(this._errorId)) ? "" : ",";
				return ErrorRecord.NotNull(this._errorId) + str + ErrorRecord.NotNull(invocationTypeName);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00007C7C File Offset: 0x00005E7C
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00007C84 File Offset: 0x00005E84
		public ErrorDetails ErrorDetails
		{
			get
			{
				return this._errorDetails;
			}
			set
			{
				this._errorDetails = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007C8D File Offset: 0x00005E8D
		public InvocationInfo InvocationInfo
		{
			get
			{
				return this._invocationInfo;
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00007C98 File Offset: 0x00005E98
		internal void SetInvocationInfo(InvocationInfo invocationInfo)
		{
			IScriptExtent scriptExtent = null;
			if (this._invocationInfo != null)
			{
				scriptExtent = this._invocationInfo.DisplayScriptPosition;
			}
			if (invocationInfo != null)
			{
				this._invocationInfo = new InvocationInfo(invocationInfo.MyCommand, invocationInfo.ScriptPosition);
				this._invocationInfo.InvocationName = invocationInfo.InvocationName;
				if (invocationInfo.MyCommand == null)
				{
					this._invocationInfo.HistoryId = invocationInfo.HistoryId;
				}
			}
			if (scriptExtent != null)
			{
				this._invocationInfo.DisplayScriptPosition = scriptExtent;
			}
			this.LockScriptStackTrace();
			if (invocationInfo != null && invocationInfo.PipelineIterationInfo != null)
			{
				int[] list = (int[])invocationInfo.PipelineIterationInfo.Clone();
				this.pipelineIterationInfo = new ReadOnlyCollection<int>(list);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00007D3C File Offset: 0x00005F3C
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00007D44 File Offset: 0x00005F44
		internal bool PreserveInvocationInfoOnce
		{
			get
			{
				return this.preserveInvocationInfoOnce;
			}
			set
			{
				this.preserveInvocationInfoOnce = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00007D4D File Offset: 0x00005F4D
		public string ScriptStackTrace
		{
			get
			{
				return this._scriptStackTrace;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007D58 File Offset: 0x00005F58
		internal void LockScriptStackTrace()
		{
			if (this._scriptStackTrace != null)
			{
				return;
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				IEnumerable<CallStackFrame> callStack = executionContextFromTLS.Debugger.GetCallStack();
				bool flag = true;
				foreach (CallStackFrame callStackFrame in callStack)
				{
					if (!flag)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					flag = false;
					stringBuilder.Append(callStackFrame.ToString());
				}
				this._scriptStackTrace = stringBuilder.ToString();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00007DF0 File Offset: 0x00005FF0
		public ReadOnlyCollection<int> PipelineIterationInfo
		{
			get
			{
				return this.pipelineIterationInfo;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00007DF8 File Offset: 0x00005FF8
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00007E00 File Offset: 0x00006000
		internal bool SerializeExtendedInfo
		{
			get
			{
				return this.serializeExtendedInfo;
			}
			set
			{
				this.serializeExtendedInfo = value;
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007E09 File Offset: 0x00006009
		internal static string NotNull(string s)
		{
			if (s == null)
			{
				return "";
			}
			return s;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007E18 File Offset: 0x00006018
		private string GetInvocationTypeName()
		{
			InvocationInfo invocationInfo = this.InvocationInfo;
			if (invocationInfo == null)
			{
				return "";
			}
			CommandInfo myCommand = invocationInfo.MyCommand;
			if (myCommand == null)
			{
				return "";
			}
			IScriptCommandInfo scriptCommandInfo = myCommand as IScriptCommandInfo;
			if (scriptCommandInfo != null)
			{
				return myCommand.Name;
			}
			CmdletInfo cmdletInfo = myCommand as CmdletInfo;
			if (cmdletInfo == null)
			{
				return "";
			}
			return cmdletInfo.ImplementingType.FullName;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007E74 File Offset: 0x00006074
		public override string ToString()
		{
			if (this.ErrorDetails != null && !string.IsNullOrEmpty(this.ErrorDetails.Message))
			{
				return this.ErrorDetails.Message;
			}
			if (this.Exception == null)
			{
				return base.ToString();
			}
			if (!string.IsNullOrEmpty(this.Exception.Message))
			{
				return this.Exception.Message;
			}
			return this.Exception.ToString();
		}

		// Token: 0x04000096 RID: 150
		private bool _isSerialized;

		// Token: 0x04000097 RID: 151
		private string _serializedFullyQualifiedErrorId;

		// Token: 0x04000098 RID: 152
		internal string _serializedErrorCategoryMessageOverride;

		// Token: 0x04000099 RID: 153
		private Exception _error;

		// Token: 0x0400009A RID: 154
		private object _target;

		// Token: 0x0400009B RID: 155
		private ErrorCategoryInfo _categoryInfo;

		// Token: 0x0400009C RID: 156
		private ErrorDetails _errorDetails;

		// Token: 0x0400009D RID: 157
		private InvocationInfo _invocationInfo;

		// Token: 0x0400009E RID: 158
		private bool preserveInvocationInfoOnce;

		// Token: 0x0400009F RID: 159
		private string _scriptStackTrace;

		// Token: 0x040000A0 RID: 160
		private ReadOnlyCollection<int> pipelineIterationInfo = new ReadOnlyCollection<int>(new int[0]);

		// Token: 0x040000A1 RID: 161
		private bool serializeExtendedInfo;

		// Token: 0x040000A2 RID: 162
		private string _errorId;

		// Token: 0x040000A3 RID: 163
		internal ErrorCategory _category;

		// Token: 0x040000A4 RID: 164
		internal string _activityOverride;

		// Token: 0x040000A5 RID: 165
		internal string _reasonOverride;

		// Token: 0x040000A6 RID: 166
		internal string _targetNameOverride;

		// Token: 0x040000A7 RID: 167
		internal string _targetTypeOverride;
	}
}
