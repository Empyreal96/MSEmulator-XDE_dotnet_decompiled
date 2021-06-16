using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Language;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200049A RID: 1178
	public sealed class PSParser
	{
		// Token: 0x060034CC RID: 13516 RVA: 0x0011ED6A File Offset: 0x0011CF6A
		private PSParser()
		{
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x0011ED80 File Offset: 0x0011CF80
		private void Parse(string script)
		{
			try
			{
				Parser parser = new Parser
				{
					ProduceV2Tokens = true
				};
				parser.Parse(null, script, this.tokenList, out this.errors);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060034CE RID: 13518 RVA: 0x0011EDCC File Offset: 0x0011CFCC
		private Collection<PSToken> Tokens
		{
			get
			{
				Collection<PSToken> collection = new Collection<PSToken>();
				for (int i = 0; i < this.tokenList.Count - 1; i++)
				{
					Token token = this.tokenList[i];
					collection.Add(new PSToken(token));
				}
				return collection;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060034CF RID: 13519 RVA: 0x0011EE14 File Offset: 0x0011D014
		private Collection<PSParseError> Errors
		{
			get
			{
				Collection<PSParseError> collection = new Collection<PSParseError>();
				foreach (ParseError error in this.errors)
				{
					collection.Add(new PSParseError(error));
				}
				return collection;
			}
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x0011EE50 File Offset: 0x0011D050
		public static Collection<PSToken> Tokenize(string script, out Collection<PSParseError> errors)
		{
			if (script == null)
			{
				throw PSTraceSource.NewArgumentNullException("script");
			}
			PSParser psparser = new PSParser();
			psparser.Parse(script);
			errors = psparser.Errors;
			return psparser.Tokens;
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x0011EE88 File Offset: 0x0011D088
		public static Collection<PSToken> Tokenize(object[] script, out Collection<PSParseError> errors)
		{
			if (script == null)
			{
				throw PSTraceSource.NewArgumentNullException("script");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in script)
			{
				if (obj != null)
				{
					stringBuilder.AppendLine(obj.ToString());
				}
			}
			return PSParser.Tokenize(stringBuilder.ToString(), out errors);
		}

		// Token: 0x04001AFA RID: 6906
		private readonly List<Token> tokenList = new List<Token>();

		// Token: 0x04001AFB RID: 6907
		private ParseError[] errors;
	}
}
