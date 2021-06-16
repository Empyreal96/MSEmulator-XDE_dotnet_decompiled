using System;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x020000C5 RID: 197
	internal class AutomationEngine
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x000401AC File Offset: 0x0003E3AC
		internal ExecutionContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000401B4 File Offset: 0x0003E3B4
		internal CommandDiscovery CommandDiscovery
		{
			get
			{
				return this.commandDiscovery;
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x000401BC File Offset: 0x0003E3BC
		internal AutomationEngine(PSHost hostInterface, RunspaceConfiguration runspaceConfiguration, InitialSessionState iss)
		{
			string text = Environment.GetEnvironmentVariable("PathEXT");
			text = (text ?? string.Empty);
			bool flag = false;
			if (text != string.Empty)
			{
				string[] array = text.Split(new char[]
				{
					';'
				});
				foreach (string text2 in array)
				{
					string text3 = text2.Trim();
					if (text3.Equals(".CPL", StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				text = ((text == string.Empty) ? ".CPL" : (text.EndsWith(";", StringComparison.OrdinalIgnoreCase) ? (text + ".CPL") : (text + ";.CPL")));
				Environment.SetEnvironmentVariable("PathEXT", text);
			}
			if (runspaceConfiguration != null)
			{
				this._context = new ExecutionContext(this, hostInterface, runspaceConfiguration);
			}
			else
			{
				this._context = new ExecutionContext(this, hostInterface, iss);
			}
			this.EngineParser = new Parser();
			this.commandDiscovery = new CommandDiscovery(this._context);
			if (runspaceConfiguration != null)
			{
				runspaceConfiguration.Bind(this._context);
			}
			else
			{
				iss.Bind(this._context, false);
			}
			InitialSessionState.SetSessionStateDrive(this._context, true);
			InitialSessionState.CreateQuestionVariable(this._context);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00040300 File Offset: 0x0003E500
		internal string Expand(string s)
		{
			ExpressionAst expressionAst = Parser.ScanString(s);
			return (Compiler.GetExpressionValue(expressionAst, true, this.Context, this.Context.EngineSessionState, null) as string) ?? "";
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0004033B File Offset: 0x0003E53B
		internal ScriptBlock ParseScriptBlock(string script, bool interactiveCommand)
		{
			return this.ParseScriptBlock(script, null, interactiveCommand);
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00040348 File Offset: 0x0003E548
		internal ScriptBlock ParseScriptBlock(string script, string fileName, bool interactiveCommand)
		{
			ParseError[] array;
			ScriptBlockAst ast = this.EngineParser.Parse(fileName, script, null, out array);
			if (interactiveCommand)
			{
				this.EngineParser.SetPreviousFirstLastToken(this._context);
			}
			if (!array.Any<ParseError>())
			{
				return new ScriptBlock(ast, false);
			}
			if (array[0].IncompleteInput)
			{
				throw new IncompleteParseException(array[0].Message, array[0].ErrorId);
			}
			throw new ParseException(array);
		}

		// Token: 0x040004C0 RID: 1216
		internal Parser EngineParser;

		// Token: 0x040004C1 RID: 1217
		private ExecutionContext _context;

		// Token: 0x040004C2 RID: 1218
		private CommandDiscovery commandDiscovery;
	}
}
