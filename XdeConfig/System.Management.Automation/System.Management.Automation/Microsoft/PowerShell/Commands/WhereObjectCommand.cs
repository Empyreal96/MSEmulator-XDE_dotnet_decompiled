using System;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000494 RID: 1172
	[Cmdlet("Where", "Object", DefaultParameterSetName = "EqualSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113423", RemotingCapability = RemotingCapability.None)]
	public sealed class WhereObjectCommand : PSCmdlet
	{
		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600345F RID: 13407 RVA: 0x0011D970 File Offset: 0x0011BB70
		// (set) Token: 0x0600345E RID: 13406 RVA: 0x0011D967 File Offset: 0x0011BB67
		[Parameter(ValueFromPipeline = true)]
		public PSObject InputObject
		{
			get
			{
				return this._inputObject;
			}
			set
			{
				this._inputObject = value;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06003461 RID: 13409 RVA: 0x0011D981 File Offset: 0x0011BB81
		// (set) Token: 0x06003460 RID: 13408 RVA: 0x0011D978 File Offset: 0x0011BB78
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "ScriptBlockSet")]
		public ScriptBlock FilterScript
		{
			get
			{
				return this.script;
			}
			set
			{
				this.script = value;
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06003463 RID: 13411 RVA: 0x0011D992 File Offset: 0x0011BB92
		// (set) Token: 0x06003462 RID: 13410 RVA: 0x0011D989 File Offset: 0x0011BB89
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveInSet")]
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "IsNotSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "NotInSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveNotInSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "LessOrEqualSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveLessOrEqualSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "InSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "EqualSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveEqualSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "NotEqualSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveNotEqualSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "GreaterThanSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveGreaterThanSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "LessThanSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveLessThanSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "GreaterOrEqualSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveGreaterOrEqualSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "IsSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "LikeSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveLikeSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "NotLikeSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveNotLikeSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "MatchSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveMatchSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "NotMatchSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveNotMatchSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "ContainsSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveContainsSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "NotContainsSet")]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "CaseSensitiveNotContainsSet")]
		public string Property
		{
			get
			{
				return this._property;
			}
			set
			{
				this._property = value;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06003465 RID: 13413 RVA: 0x0011D9AA File Offset: 0x0011BBAA
		// (set) Token: 0x06003464 RID: 13412 RVA: 0x0011D99A File Offset: 0x0011BB9A
		[Parameter(Position = 1, ParameterSetName = "IsNotSet")]
		[Parameter(Position = 1, ParameterSetName = "MatchSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveLessThanSet")]
		[Parameter(Position = 1, ParameterSetName = "EqualSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveNotEqualSet")]
		[Parameter(Position = 1, ParameterSetName = "GreaterOrEqualSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveGreaterOrEqualSet")]
		[Parameter(Position = 1, ParameterSetName = "LessOrEqualSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveLessOrEqualSet")]
		[Parameter(Position = 1, ParameterSetName = "LikeSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveLikeSet")]
		[Parameter(Position = 1, ParameterSetName = "NotLikeSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveNotMatchSet")]
		[Parameter(Position = 1, ParameterSetName = "ContainsSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveContainsSet")]
		[Parameter(Position = 1, ParameterSetName = "NotContainsSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveNotLikeSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveNotContainsSet")]
		[Parameter(Position = 1, ParameterSetName = "InSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveInSet")]
		[Parameter(Position = 1, ParameterSetName = "NotInSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveNotInSet")]
		[Parameter(Position = 1, ParameterSetName = "IsSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveMatchSet")]
		[Parameter(Position = 1, ParameterSetName = "NotMatchSet")]
		[Parameter(Position = 1, ParameterSetName = "NotEqualSet")]
		[Parameter(Position = 1, ParameterSetName = "GreaterThanSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveGreaterThanSet")]
		[Parameter(Position = 1, ParameterSetName = "LessThanSet")]
		[Parameter(Position = 1, ParameterSetName = "CaseSensitiveEqualSet")]
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				this._valueNotSpecified = false;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x0011D9C3 File Offset: 0x0011BBC3
		// (set) Token: 0x06003466 RID: 13414 RVA: 0x0011D9B2 File Offset: 0x0011BBB2
		[Parameter(ParameterSetName = "EqualSet")]
		[Alias(new string[]
		{
			"IEQ"
		})]
		public SwitchParameter EQ
		{
			get
			{
				return this._binaryOperator == TokenKind.Ieq;
			}
			set
			{
				this._binaryOperator = TokenKind.Ieq;
				this._forceBooleanEvaluation = false;
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06003469 RID: 13417 RVA: 0x0011D9DE File Offset: 0x0011BBDE
		// (set) Token: 0x06003468 RID: 13416 RVA: 0x0011D9D4 File Offset: 0x0011BBD4
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveEqualSet")]
		public SwitchParameter CEQ
		{
			get
			{
				return this._binaryOperator == TokenKind.Ceq;
			}
			set
			{
				this._binaryOperator = TokenKind.Ceq;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x0600346B RID: 13419 RVA: 0x0011D9F9 File Offset: 0x0011BBF9
		// (set) Token: 0x0600346A RID: 13418 RVA: 0x0011D9EF File Offset: 0x0011BBEF
		[Alias(new string[]
		{
			"INE"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "NotEqualSet")]
		public SwitchParameter NE
		{
			get
			{
				return this._binaryOperator == TokenKind.Ine;
			}
			set
			{
				this._binaryOperator = TokenKind.Ine;
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x0600346D RID: 13421 RVA: 0x0011DA14 File Offset: 0x0011BC14
		// (set) Token: 0x0600346C RID: 13420 RVA: 0x0011DA0A File Offset: 0x0011BC0A
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveNotEqualSet")]
		public SwitchParameter CNE
		{
			get
			{
				return this._binaryOperator == TokenKind.Cne;
			}
			set
			{
				this._binaryOperator = TokenKind.Cne;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x0011DA2F File Offset: 0x0011BC2F
		// (set) Token: 0x0600346E RID: 13422 RVA: 0x0011DA25 File Offset: 0x0011BC25
		[Alias(new string[]
		{
			"IGT"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "GreaterThanSet")]
		public SwitchParameter GT
		{
			get
			{
				return this._binaryOperator == TokenKind.Igt;
			}
			set
			{
				this._binaryOperator = TokenKind.Igt;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x0011DA4A File Offset: 0x0011BC4A
		// (set) Token: 0x06003470 RID: 13424 RVA: 0x0011DA40 File Offset: 0x0011BC40
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveGreaterThanSet")]
		public SwitchParameter CGT
		{
			get
			{
				return this._binaryOperator == TokenKind.Cgt;
			}
			set
			{
				this._binaryOperator = TokenKind.Cgt;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06003473 RID: 13427 RVA: 0x0011DA7A File Offset: 0x0011BC7A
		// (set) Token: 0x06003472 RID: 13426 RVA: 0x0011DA5C File Offset: 0x0011BC5C
		[Parameter(Mandatory = true, ParameterSetName = "LessThanSet")]
		[Alias(new string[]
		{
			"ILT"
		})]
		public SwitchParameter LT
		{
			get
			{
				return this._binaryOperator == TokenKind.Ilt;
			}
			set
			{
				this._binaryOperator = (this._binaryOperator = TokenKind.Ilt);
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06003475 RID: 13429 RVA: 0x0011DA95 File Offset: 0x0011BC95
		// (set) Token: 0x06003474 RID: 13428 RVA: 0x0011DA8B File Offset: 0x0011BC8B
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveLessThanSet")]
		public SwitchParameter CLT
		{
			get
			{
				return this._binaryOperator == TokenKind.Clt;
			}
			set
			{
				this._binaryOperator = TokenKind.Clt;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x0011DAB0 File Offset: 0x0011BCB0
		// (set) Token: 0x06003476 RID: 13430 RVA: 0x0011DAA6 File Offset: 0x0011BCA6
		[Parameter(Mandatory = true, ParameterSetName = "GreaterOrEqualSet")]
		[Alias(new string[]
		{
			"IGE"
		})]
		public SwitchParameter GE
		{
			get
			{
				return this._binaryOperator == TokenKind.Ige;
			}
			set
			{
				this._binaryOperator = TokenKind.Ige;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x0011DACB File Offset: 0x0011BCCB
		// (set) Token: 0x06003478 RID: 13432 RVA: 0x0011DAC1 File Offset: 0x0011BCC1
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveGreaterOrEqualSet")]
		public SwitchParameter CGE
		{
			get
			{
				return this._binaryOperator == TokenKind.Cge;
			}
			set
			{
				this._binaryOperator = TokenKind.Cge;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x0011DAE6 File Offset: 0x0011BCE6
		// (set) Token: 0x0600347A RID: 13434 RVA: 0x0011DADC File Offset: 0x0011BCDC
		[Alias(new string[]
		{
			"ILE"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "LessOrEqualSet")]
		public SwitchParameter LE
		{
			get
			{
				return this._binaryOperator == TokenKind.Ile;
			}
			set
			{
				this._binaryOperator = TokenKind.Ile;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x0011DB01 File Offset: 0x0011BD01
		// (set) Token: 0x0600347C RID: 13436 RVA: 0x0011DAF7 File Offset: 0x0011BCF7
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveLessOrEqualSet")]
		public SwitchParameter CLE
		{
			get
			{
				return this._binaryOperator == TokenKind.Cle;
			}
			set
			{
				this._binaryOperator = TokenKind.Cle;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x0011DB1C File Offset: 0x0011BD1C
		// (set) Token: 0x0600347E RID: 13438 RVA: 0x0011DB12 File Offset: 0x0011BD12
		[Alias(new string[]
		{
			"ILike"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "LikeSet")]
		public SwitchParameter Like
		{
			get
			{
				return this._binaryOperator == TokenKind.Ilike;
			}
			set
			{
				this._binaryOperator = TokenKind.Ilike;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x0011DB37 File Offset: 0x0011BD37
		// (set) Token: 0x06003480 RID: 13440 RVA: 0x0011DB2D File Offset: 0x0011BD2D
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveLikeSet")]
		public SwitchParameter CLike
		{
			get
			{
				return this._binaryOperator == TokenKind.Clike;
			}
			set
			{
				this._binaryOperator = TokenKind.Clike;
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06003483 RID: 13443 RVA: 0x0011DB52 File Offset: 0x0011BD52
		// (set) Token: 0x06003482 RID: 13442 RVA: 0x0011DB48 File Offset: 0x0011BD48
		[Parameter(Mandatory = true, ParameterSetName = "NotLikeSet")]
		[Alias(new string[]
		{
			"INotLike"
		})]
		public SwitchParameter NotLike
		{
			get
			{
				return false;
			}
			set
			{
				this._binaryOperator = TokenKind.Inotlike;
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x0011DB64 File Offset: 0x0011BD64
		// (set) Token: 0x06003484 RID: 13444 RVA: 0x0011DB5A File Offset: 0x0011BD5A
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveNotLikeSet")]
		public SwitchParameter CNotLike
		{
			get
			{
				return this._binaryOperator == TokenKind.Cnotlike;
			}
			set
			{
				this._binaryOperator = TokenKind.Cnotlike;
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x0011DB7F File Offset: 0x0011BD7F
		// (set) Token: 0x06003486 RID: 13446 RVA: 0x0011DB75 File Offset: 0x0011BD75
		[Parameter(Mandatory = true, ParameterSetName = "MatchSet")]
		[Alias(new string[]
		{
			"IMatch"
		})]
		public SwitchParameter Match
		{
			get
			{
				return this._binaryOperator == TokenKind.Imatch;
			}
			set
			{
				this._binaryOperator = TokenKind.Imatch;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06003489 RID: 13449 RVA: 0x0011DB9A File Offset: 0x0011BD9A
		// (set) Token: 0x06003488 RID: 13448 RVA: 0x0011DB90 File Offset: 0x0011BD90
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveMatchSet")]
		public SwitchParameter CMatch
		{
			get
			{
				return this._binaryOperator == TokenKind.Cmatch;
			}
			set
			{
				this._binaryOperator = TokenKind.Cmatch;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x0600348B RID: 13451 RVA: 0x0011DBB5 File Offset: 0x0011BDB5
		// (set) Token: 0x0600348A RID: 13450 RVA: 0x0011DBAB File Offset: 0x0011BDAB
		[Alias(new string[]
		{
			"INotMatch"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "NotMatchSet")]
		public SwitchParameter NotMatch
		{
			get
			{
				return this._binaryOperator == TokenKind.Inotmatch;
			}
			set
			{
				this._binaryOperator = TokenKind.Inotmatch;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x0600348D RID: 13453 RVA: 0x0011DBD0 File Offset: 0x0011BDD0
		// (set) Token: 0x0600348C RID: 13452 RVA: 0x0011DBC6 File Offset: 0x0011BDC6
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveNotMatchSet")]
		public SwitchParameter CNotMatch
		{
			get
			{
				return this._binaryOperator == TokenKind.Cnotmatch;
			}
			set
			{
				this._binaryOperator = TokenKind.Cnotmatch;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x0011DBEB File Offset: 0x0011BDEB
		// (set) Token: 0x0600348E RID: 13454 RVA: 0x0011DBE1 File Offset: 0x0011BDE1
		[Alias(new string[]
		{
			"IContains"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "ContainsSet")]
		public SwitchParameter Contains
		{
			get
			{
				return this._binaryOperator == TokenKind.Icontains;
			}
			set
			{
				this._binaryOperator = TokenKind.Icontains;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06003491 RID: 13457 RVA: 0x0011DC06 File Offset: 0x0011BE06
		// (set) Token: 0x06003490 RID: 13456 RVA: 0x0011DBFC File Offset: 0x0011BDFC
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveContainsSet")]
		public SwitchParameter CContains
		{
			get
			{
				return this._binaryOperator == TokenKind.Ccontains;
			}
			set
			{
				this._binaryOperator = TokenKind.Ccontains;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06003493 RID: 13459 RVA: 0x0011DC21 File Offset: 0x0011BE21
		// (set) Token: 0x06003492 RID: 13458 RVA: 0x0011DC17 File Offset: 0x0011BE17
		[Alias(new string[]
		{
			"INotContains"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "NotContainsSet")]
		public SwitchParameter NotContains
		{
			get
			{
				return this._binaryOperator == TokenKind.Inotcontains;
			}
			set
			{
				this._binaryOperator = TokenKind.Inotcontains;
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06003495 RID: 13461 RVA: 0x0011DC3C File Offset: 0x0011BE3C
		// (set) Token: 0x06003494 RID: 13460 RVA: 0x0011DC32 File Offset: 0x0011BE32
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveNotContainsSet")]
		public SwitchParameter CNotContains
		{
			get
			{
				return this._binaryOperator == TokenKind.Cnotcontains;
			}
			set
			{
				this._binaryOperator = TokenKind.Cnotcontains;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06003497 RID: 13463 RVA: 0x0011DC5A File Offset: 0x0011BE5A
		// (set) Token: 0x06003496 RID: 13462 RVA: 0x0011DC4D File Offset: 0x0011BE4D
		[Parameter(Mandatory = true, ParameterSetName = "InSet")]
		[Alias(new string[]
		{
			"IIn"
		})]
		public SwitchParameter In
		{
			get
			{
				return this._binaryOperator == TokenKind.In;
			}
			set
			{
				this._binaryOperator = TokenKind.In;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06003499 RID: 13465 RVA: 0x0011DC78 File Offset: 0x0011BE78
		// (set) Token: 0x06003498 RID: 13464 RVA: 0x0011DC6E File Offset: 0x0011BE6E
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveInSet")]
		public SwitchParameter CIn
		{
			get
			{
				return this._binaryOperator == TokenKind.Cin;
			}
			set
			{
				this._binaryOperator = TokenKind.Cin;
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600349B RID: 13467 RVA: 0x0011DC93 File Offset: 0x0011BE93
		// (set) Token: 0x0600349A RID: 13466 RVA: 0x0011DC89 File Offset: 0x0011BE89
		[Alias(new string[]
		{
			"INotIn"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "NotInSet")]
		public SwitchParameter NotIn
		{
			get
			{
				return this._binaryOperator == TokenKind.Inotin;
			}
			set
			{
				this._binaryOperator = TokenKind.Inotin;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x0600349D RID: 13469 RVA: 0x0011DCAE File Offset: 0x0011BEAE
		// (set) Token: 0x0600349C RID: 13468 RVA: 0x0011DCA4 File Offset: 0x0011BEA4
		[Parameter(Mandatory = true, ParameterSetName = "CaseSensitiveNotInSet")]
		public SwitchParameter CNotIn
		{
			get
			{
				return this._binaryOperator == TokenKind.Cnotin;
			}
			set
			{
				this._binaryOperator = TokenKind.Cnotin;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x0011DCC9 File Offset: 0x0011BEC9
		// (set) Token: 0x0600349E RID: 13470 RVA: 0x0011DCBF File Offset: 0x0011BEBF
		[Parameter(Mandatory = true, ParameterSetName = "IsSet")]
		public SwitchParameter Is
		{
			get
			{
				return this._binaryOperator == TokenKind.Is;
			}
			set
			{
				this._binaryOperator = TokenKind.Is;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x060034A1 RID: 13473 RVA: 0x0011DCE4 File Offset: 0x0011BEE4
		// (set) Token: 0x060034A0 RID: 13472 RVA: 0x0011DCDA File Offset: 0x0011BEDA
		[Parameter(Mandatory = true, ParameterSetName = "IsNotSet")]
		public SwitchParameter IsNot
		{
			get
			{
				return this._binaryOperator == TokenKind.IsNot;
			}
			set
			{
				this._binaryOperator = TokenKind.IsNot;
			}
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x0011DD18 File Offset: 0x0011BF18
		private static Func<object, object, object> GetCallSiteDelegate(ExpressionType expressionType, bool ignoreCase)
		{
			CallSite<Func<CallSite, object, object, object>> site = CallSite<Func<CallSite, object, object, object>>.Create(PSBinaryOperationBinder.Get(expressionType, ignoreCase, false));
			return (object x, object y) => site.Target(site, x, y);
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x0011DD4C File Offset: 0x0011BF4C
		private static Tuple<CallSite<Func<CallSite, object, IEnumerator>>, CallSite<Func<CallSite, object, object, object>>> GetContainsCallSites(bool ignoreCase)
		{
			CallSite<Func<CallSite, object, IEnumerator>> item = CallSite<Func<CallSite, object, IEnumerator>>.Create(PSEnumerableBinder.Get());
			CallSite<Func<CallSite, object, object, object>> item2 = CallSite<Func<CallSite, object, object, object>>.Create(PSBinaryOperationBinder.Get(ExpressionType.Equal, ignoreCase, true));
			return Tuple.Create<CallSite<Func<CallSite, object, IEnumerator>>, CallSite<Func<CallSite, object, object, object>>>(item, item2);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x0011DD7C File Offset: 0x0011BF7C
		private void CheckLanguageMode()
		{
			if (base.Context.LanguageMode.Equals(PSLanguageMode.RestrictedLanguage))
			{
				string message = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.OperationNotAllowedInRestrictedLanguageMode, new object[]
				{
					this._binaryOperator
				});
				PSInvalidOperationException exception = new PSInvalidOperationException(message);
				base.ThrowTerminatingError(new ErrorRecord(exception, "OperationNotAllowedInRestrictedLanguageMode", ErrorCategory.InvalidOperation, null));
			}
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x0011E074 File Offset: 0x0011C274
		protected override void BeginProcessing()
		{
			if (this.script != null)
			{
				return;
			}
			TokenKind binaryOperator = this._binaryOperator;
			Tuple<CallSite<Func<CallSite, object, IEnumerator>>, CallSite<Func<CallSite, object, object, object>>> sites;
			switch (binaryOperator)
			{
			case TokenKind.Ieq:
			{
				if (!this._forceBooleanEvaluation)
				{
					this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.Equal, true);
					return;
				}
				CallSite<Func<CallSite, object, object, object>> site = CallSite<Func<CallSite, object, object, object>>.Create(PSBinaryOperationBinder.Get(ExpressionType.Equal, true, false));
				this.operationDelegate = ((object x, object y) => site.Target(site, y, x));
				return;
			}
			case TokenKind.Ine:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.NotEqual, true);
				return;
			case TokenKind.Ige:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.GreaterThanOrEqual, true);
				return;
			case TokenKind.Igt:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.GreaterThan, true);
				return;
			case TokenKind.Ilt:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.LessThan, true);
				return;
			case TokenKind.Ile:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.LessThanOrEqual, true);
				return;
			case TokenKind.Ilike:
				this.operationDelegate = ((object lval, object rval) => ParserOps.LikeOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval, false, true));
				return;
			case TokenKind.Inotlike:
				this.operationDelegate = ((object lval, object rval) => ParserOps.LikeOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval, true, true));
				return;
			case TokenKind.Imatch:
				this.CheckLanguageMode();
				this.operationDelegate = ((object lval, object rval) => ParserOps.MatchOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval, false, true));
				return;
			case TokenKind.Inotmatch:
				this.CheckLanguageMode();
				this.operationDelegate = ((object lval, object rval) => ParserOps.MatchOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval, true, true));
				return;
			case TokenKind.Ireplace:
			case TokenKind.Iin:
			case TokenKind.Isplit:
			case TokenKind.Creplace:
			case TokenKind.Csplit:
				return;
			case TokenKind.Icontains:
			case TokenKind.Inotcontains:
			case TokenKind.Inotin:
				break;
			case TokenKind.Ceq:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.Equal, false);
				return;
			case TokenKind.Cne:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.NotEqual, false);
				return;
			case TokenKind.Cge:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.GreaterThanOrEqual, false);
				return;
			case TokenKind.Cgt:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.GreaterThan, false);
				return;
			case TokenKind.Clt:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.LessThan, false);
				return;
			case TokenKind.Cle:
				this.operationDelegate = WhereObjectCommand.GetCallSiteDelegate(ExpressionType.LessThanOrEqual, false);
				return;
			case TokenKind.Clike:
				this.operationDelegate = ((object lval, object rval) => ParserOps.LikeOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval, false, false));
				return;
			case TokenKind.Cnotlike:
				this.operationDelegate = ((object lval, object rval) => ParserOps.LikeOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval, true, false));
				return;
			case TokenKind.Cmatch:
				this.CheckLanguageMode();
				this.operationDelegate = ((object lval, object rval) => ParserOps.MatchOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval, false, false));
				return;
			case TokenKind.Cnotmatch:
				this.CheckLanguageMode();
				this.operationDelegate = ((object lval, object rval) => ParserOps.MatchOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval, true, false));
				return;
			case TokenKind.Ccontains:
			case TokenKind.Cnotcontains:
			case TokenKind.Cin:
			case TokenKind.Cnotin:
			{
				Tuple<CallSite<Func<CallSite, object, IEnumerator>>, CallSite<Func<CallSite, object, object, object>>> sites = WhereObjectCommand.GetContainsCallSites(false);
				switch (this._binaryOperator)
				{
				case TokenKind.Ccontains:
					this.operationDelegate = ((object lval, object rval) => ParserOps.ContainsOperatorCompiled(this.Context, sites.Item1, sites.Item2, lval, rval));
					return;
				case TokenKind.Cnotcontains:
					this.operationDelegate = ((object lval, object rval) => !ParserOps.ContainsOperatorCompiled(this.Context, sites.Item1, sites.Item2, lval, rval));
					return;
				case TokenKind.Cin:
					this.operationDelegate = ((object lval, object rval) => ParserOps.ContainsOperatorCompiled(this.Context, sites.Item1, sites.Item2, rval, lval));
					return;
				case TokenKind.Cnotin:
					this.operationDelegate = ((object lval, object rval) => !ParserOps.ContainsOperatorCompiled(this.Context, sites.Item1, sites.Item2, rval, lval));
					return;
				default:
					return;
				}
				break;
			}
			case TokenKind.Is:
				this.operationDelegate = ((object lval, object rval) => ParserOps.IsOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval));
				return;
			case TokenKind.IsNot:
				this.operationDelegate = ((object lval, object rval) => ParserOps.IsNotOperator(base.Context, PositionUtilities.EmptyExtent, lval, rval));
				return;
			default:
				if (binaryOperator != TokenKind.In)
				{
					return;
				}
				break;
			}
			sites = WhereObjectCommand.GetContainsCallSites(true);
			TokenKind binaryOperator2 = this._binaryOperator;
			switch (binaryOperator2)
			{
			case TokenKind.Icontains:
				this.operationDelegate = ((object lval, object rval) => ParserOps.ContainsOperatorCompiled(this.Context, sites.Item1, sites.Item2, lval, rval));
				return;
			case TokenKind.Inotcontains:
				this.operationDelegate = ((object lval, object rval) => !ParserOps.ContainsOperatorCompiled(this.Context, sites.Item1, sites.Item2, lval, rval));
				return;
			case TokenKind.Iin:
				return;
			case TokenKind.Inotin:
				this.operationDelegate = ((object lval, object rval) => !ParserOps.ContainsOperatorCompiled(this.Context, sites.Item1, sites.Item2, rval, lval));
				return;
			default:
				if (binaryOperator2 != TokenKind.In)
				{
					return;
				}
				this.operationDelegate = ((object lval, object rval) => ParserOps.ContainsOperatorCompiled(this.Context, sites.Item1, sites.Item2, rval, lval));
				return;
			}
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x0011E4D0 File Offset: 0x0011C6D0
		protected override void ProcessRecord()
		{
			if (this._inputObject == AutomationNull.Value)
			{
				return;
			}
			if (this.script != null)
			{
				object[] input = new object[]
				{
					this._inputObject
				};
				object arg = this.script.DoInvokeReturnAsIs(false, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, this.InputObject, input, AutomationNull.Value, new object[0]);
				if (this.toBoolSite.Target(this.toBoolSite, arg))
				{
					base.WriteObject(this.InputObject);
					return;
				}
			}
			else
			{
				if (this._valueNotSpecified && (this._binaryOperator != TokenKind.Ieq || !this._forceBooleanEvaluation))
				{
					base.ThrowTerminatingError(ForEachObjectCommand.GenerateNameParameterError("Value", InternalCommandStrings.ValueNotSpecifiedForWhereObject, "ValueNotSpecifiedForWhereObject", null, new object[0]));
				}
				if (!this._valueNotSpecified && this._binaryOperator == TokenKind.Ieq && this._forceBooleanEvaluation)
				{
					base.ThrowTerminatingError(ForEachObjectCommand.GenerateNameParameterError("Operator", InternalCommandStrings.OperatorNotSpecified, "OperatorNotSpecified", null, new object[0]));
				}
				bool flag = false;
				object value = this.GetValue(ref flag);
				if (flag)
				{
					return;
				}
				try
				{
					if (this._binaryOperator == TokenKind.Is || this._binaryOperator == TokenKind.IsNot)
					{
						string text = this._value as string;
						if (text != null && text.StartsWith("[", StringComparison.CurrentCulture) && text.EndsWith("]", StringComparison.CurrentCulture))
						{
							this._value = text.Substring(1, text.Length - 2);
						}
					}
					object arg2 = this.operationDelegate(value, this._value);
					if (this.toBoolSite.Target(this.toBoolSite, arg2))
					{
						base.WriteObject(this.InputObject);
					}
				}
				catch (PipelineStoppedException)
				{
					throw;
				}
				catch (ArgumentException ex)
				{
					ErrorRecord errorRecord = new ErrorRecord(PSTraceSource.NewArgumentException("BinaryOperator", ParserStrings.BadOperatorArgument, new object[]
					{
						this._binaryOperator,
						ex.Message
					}), "BadOperatorArgument", ErrorCategory.InvalidArgument, this._inputObject);
					base.WriteError(errorRecord);
				}
				catch (Exception ex2)
				{
					CommandProcessorBase.CheckForSevereException(ex2);
					ErrorRecord errorRecord2 = new ErrorRecord(PSTraceSource.NewInvalidOperationException(ParserStrings.OperatorFailed, new object[]
					{
						this._binaryOperator,
						ex2.Message
					}), "OperatorFailed", ErrorCategory.InvalidOperation, this._inputObject);
					base.WriteError(errorRecord2);
				}
			}
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x0011E73C File Offset: 0x0011C93C
		private object GetValue(ref bool error)
		{
			if (LanguagePrimitives.IsNull(this.InputObject))
			{
				if (base.Context.IsStrictVersion(2))
				{
					base.WriteError(ForEachObjectCommand.GenerateNameParameterError("InputObject", InternalCommandStrings.InputObjectIsNull, "InputObjectIsNull", this._inputObject, new object[]
					{
						this._property
					}));
					error = true;
				}
				return null;
			}
			object obj = PSObject.Base(this._inputObject);
			IDictionary dictionary = obj as IDictionary;
			try
			{
				if (dictionary != null && dictionary.Contains(this._property))
				{
					return dictionary[this._property];
				}
			}
			catch (InvalidOperationException)
			{
			}
			ReadOnlyPSMemberInfoCollection<PSMemberInfo> matchMembers = this.GetMatchMembers();
			if (matchMembers.Count > 1)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (PSMemberInfo psmemberInfo in matchMembers)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", new object[]
					{
						psmemberInfo.Name
					});
				}
				base.WriteError(ForEachObjectCommand.GenerateNameParameterError("Property", InternalCommandStrings.AmbiguousPropertyOrMethodName, "AmbiguousPropertyName", this._inputObject, new object[]
				{
					this._property,
					stringBuilder
				}));
				error = true;
			}
			else if (matchMembers.Count == 0)
			{
				if (base.Context.IsStrictVersion(2))
				{
					base.WriteError(ForEachObjectCommand.GenerateNameParameterError("Property", InternalCommandStrings.PropertyNotFound, "PropertyNotFound", this._inputObject, new object[]
					{
						this._property
					}));
					error = true;
				}
			}
			else
			{
				try
				{
					return matchMembers[0].Value;
				}
				catch (TerminateException)
				{
					throw;
				}
				catch (MethodException)
				{
					throw;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					return null;
				}
			}
			return null;
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x0011E938 File Offset: 0x0011CB38
		private ReadOnlyPSMemberInfoCollection<PSMemberInfo> GetMatchMembers()
		{
			if (!WildcardPattern.ContainsWildcardCharacters(this._property))
			{
				PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<PSMemberInfo>();
				PSMemberInfo psmemberInfo = this._inputObject.Members[this._property];
				if (psmemberInfo != null)
				{
					psmemberInfoInternalCollection.Add(psmemberInfo);
				}
				return new ReadOnlyPSMemberInfoCollection<PSMemberInfo>(psmemberInfoInternalCollection);
			}
			return this._inputObject.Members.Match(this._property, PSMemberTypes.All);
		}

		// Token: 0x04001AEB RID: 6891
		private PSObject _inputObject = AutomationNull.Value;

		// Token: 0x04001AEC RID: 6892
		private ScriptBlock script;

		// Token: 0x04001AED RID: 6893
		private string _property;

		// Token: 0x04001AEE RID: 6894
		private object _value = true;

		// Token: 0x04001AEF RID: 6895
		private bool _valueNotSpecified = true;

		// Token: 0x04001AF0 RID: 6896
		private TokenKind _binaryOperator = TokenKind.Ieq;

		// Token: 0x04001AF1 RID: 6897
		private bool _forceBooleanEvaluation = true;

		// Token: 0x04001AF2 RID: 6898
		private readonly CallSite<Func<CallSite, object, bool>> toBoolSite = CallSite<Func<CallSite, object, bool>>.Create(PSConvertBinder.Get(typeof(bool)));

		// Token: 0x04001AF3 RID: 6899
		private Func<object, object, object> operationDelegate;
	}
}
