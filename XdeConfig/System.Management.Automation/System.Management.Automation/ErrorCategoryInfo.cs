using System;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x02000025 RID: 37
	public class ErrorCategoryInfo
	{
		// Token: 0x06000161 RID: 353 RVA: 0x00006D39 File Offset: 0x00004F39
		internal ErrorCategoryInfo(ErrorRecord errorRecord)
		{
			if (errorRecord == null)
			{
				throw new ArgumentNullException("errorRecord");
			}
			this._errorRecord = errorRecord;
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00006D56 File Offset: 0x00004F56
		public ErrorCategory Category
		{
			get
			{
				return this._errorRecord._category;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006D64 File Offset: 0x00004F64
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00006E01 File Offset: 0x00005001
		public string Activity
		{
			get
			{
				if (!string.IsNullOrEmpty(this._errorRecord._activityOverride))
				{
					return this._errorRecord._activityOverride;
				}
				if (this._errorRecord.InvocationInfo != null && (this._errorRecord.InvocationInfo.MyCommand is CmdletInfo || this._errorRecord.InvocationInfo.MyCommand is IScriptCommandInfo) && !string.IsNullOrEmpty(this._errorRecord.InvocationInfo.MyCommand.Name))
				{
					return this._errorRecord.InvocationInfo.MyCommand.Name;
				}
				return "";
			}
			set
			{
				this._errorRecord._activityOverride = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00006E10 File Offset: 0x00005010
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00006E71 File Offset: 0x00005071
		public string Reason
		{
			get
			{
				this._reasonIsExceptionType = false;
				if (!string.IsNullOrEmpty(this._errorRecord._reasonOverride))
				{
					return this._errorRecord._reasonOverride;
				}
				if (this._errorRecord.Exception != null)
				{
					this._reasonIsExceptionType = true;
					return this._errorRecord.Exception.GetType().Name;
				}
				return "";
			}
			set
			{
				this._errorRecord._reasonOverride = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00006E80 File Offset: 0x00005080
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00006EF4 File Offset: 0x000050F4
		public string TargetName
		{
			get
			{
				if (!string.IsNullOrEmpty(this._errorRecord._targetNameOverride))
				{
					return this._errorRecord._targetNameOverride;
				}
				if (this._errorRecord.TargetObject != null)
				{
					string s;
					try
					{
						s = this._errorRecord.TargetObject.ToString();
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
						s = null;
					}
					return ErrorRecord.NotNull(s);
				}
				return "";
			}
			set
			{
				this._errorRecord._targetNameOverride = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006F04 File Offset: 0x00005104
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00006F57 File Offset: 0x00005157
		public string TargetType
		{
			get
			{
				if (!string.IsNullOrEmpty(this._errorRecord._targetTypeOverride))
				{
					return this._errorRecord._targetTypeOverride;
				}
				if (this._errorRecord.TargetObject != null)
				{
					return this._errorRecord.TargetObject.GetType().Name;
				}
				return "";
			}
			set
			{
				this._errorRecord._targetTypeOverride = value;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006F65 File Offset: 0x00005165
		public string GetMessage()
		{
			return this.GetMessage(CultureInfo.CurrentUICulture);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006F74 File Offset: 0x00005174
		public string GetMessage(CultureInfo uiCultureInfo)
		{
			string text = this.Category.ToString();
			if (string.IsNullOrEmpty(text))
			{
				text = ErrorCategory.NotSpecified.ToString();
			}
			string text2 = ErrorCategoryStrings.ResourceManager.GetString(text, uiCultureInfo);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = ErrorCategoryStrings.NotSpecified;
			}
			string text3 = ErrorCategoryInfo.Ellipsize(uiCultureInfo, this.Activity);
			string text4 = ErrorCategoryInfo.Ellipsize(uiCultureInfo, this.TargetName);
			string text5 = ErrorCategoryInfo.Ellipsize(uiCultureInfo, this.TargetType);
			string text6 = this.Reason;
			text6 = (this._reasonIsExceptionType ? text6 : ErrorCategoryInfo.Ellipsize(uiCultureInfo, text6));
			string result;
			try
			{
				result = string.Format(uiCultureInfo, text2, new object[]
				{
					text3,
					text4,
					text5,
					text6,
					text
				});
			}
			catch (FormatException)
			{
				text2 = ErrorCategoryStrings.InvalidErrorCategory;
				result = string.Format(uiCultureInfo, text2, new object[]
				{
					text3,
					text4,
					text5,
					text6,
					text
				});
			}
			return result;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007080 File Offset: 0x00005280
		public override string ToString()
		{
			return this.GetMessage(CultureInfo.CurrentUICulture);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007090 File Offset: 0x00005290
		internal static string Ellipsize(CultureInfo uiCultureInfo, string original)
		{
			if (40 >= original.Length)
			{
				return original;
			}
			string text = original.Substring(0, 15);
			string text2 = original.Substring(original.Length - 15, 15);
			return string.Format(uiCultureInfo, ErrorPackage.Ellipsize, new object[]
			{
				text,
				text2
			});
		}

		// Token: 0x04000091 RID: 145
		private bool _reasonIsExceptionType;

		// Token: 0x04000092 RID: 146
		private ErrorRecord _errorRecord;
	}
}
