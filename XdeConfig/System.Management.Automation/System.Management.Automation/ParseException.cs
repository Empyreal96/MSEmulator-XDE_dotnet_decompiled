using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000898 RID: 2200
	[Serializable]
	public class ParseException : RuntimeException
	{
		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x0600544D RID: 21581 RVA: 0x001BD502 File Offset: 0x001BB702
		public ParseError[] Errors
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x0600544E RID: 21582 RVA: 0x001BD50A File Offset: 0x001BB70A
		protected ParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errors = (ParseError[])info.GetValue("Errors", typeof(ParseError[]));
		}

		// Token: 0x0600544F RID: 21583 RVA: 0x001BD534 File Offset: 0x001BB734
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("Errors", this._errors);
		}

		// Token: 0x06005450 RID: 21584 RVA: 0x001BD55D File Offset: 0x001BB75D
		public ParseException()
		{
			base.SetErrorId("Parse");
			base.SetErrorCategory(ErrorCategory.ParserError);
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x001BD578 File Offset: 0x001BB778
		public ParseException(string message) : base(message)
		{
			base.SetErrorId("Parse");
			base.SetErrorCategory(ErrorCategory.ParserError);
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x001BD594 File Offset: 0x001BB794
		internal ParseException(string message, string errorId) : base(message)
		{
			base.SetErrorId(errorId);
			base.SetErrorCategory(ErrorCategory.ParserError);
		}

		// Token: 0x06005453 RID: 21587 RVA: 0x001BD5AC File Offset: 0x001BB7AC
		internal ParseException(string message, string errorId, Exception innerException) : base(message, innerException)
		{
			base.SetErrorId(errorId);
			base.SetErrorCategory(ErrorCategory.ParserError);
		}

		// Token: 0x06005454 RID: 21588 RVA: 0x001BD5C5 File Offset: 0x001BB7C5
		public ParseException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorId("Parse");
			base.SetErrorCategory(ErrorCategory.ParserError);
		}

		// Token: 0x06005455 RID: 21589 RVA: 0x001BD5E4 File Offset: 0x001BB7E4
		public ParseException(ParseError[] errors)
		{
			if (errors == null || errors.Length == 0)
			{
				throw new ArgumentNullException("errors");
			}
			this._errors = errors;
			base.SetErrorId(this._errors[0].ErrorId);
			base.SetErrorCategory(ErrorCategory.ParserError);
			if (errors[0].Extent != null)
			{
				this.ErrorRecord.SetInvocationInfo(new InvocationInfo(null, errors[0].Extent));
			}
		}

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x06005456 RID: 21590 RVA: 0x001BD660 File Offset: 0x001BB860
		public override string Message
		{
			get
			{
				if (this._errors == null)
				{
					return base.Message;
				}
				IEnumerable<string> enumerable;
				if (this._errors.Length <= 10)
				{
					enumerable = from e in this._errors
					select e.ToString();
				}
				else
				{
					enumerable = (from e in this._errors.Take(10)
					select e.ToString()).Append(ParserStrings.TooManyErrors);
				}
				IEnumerable<string> values = enumerable;
				return string.Join(Environment.NewLine + Environment.NewLine, values);
			}
		}

		// Token: 0x04002B32 RID: 11058
		private const string errorIdString = "Parse";

		// Token: 0x04002B33 RID: 11059
		private ParseError[] _errors;
	}
}
