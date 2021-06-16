using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200001B RID: 27
	public sealed class FlagsExpression<T> where T : struct, IConvertible
	{
		// Token: 0x06000135 RID: 309 RVA: 0x0000609C File Offset: 0x0000429C
		public FlagsExpression(string expression)
		{
			if (!typeof(T).GetTypeInfo().IsEnum)
			{
				throw InterpreterError.NewInterpreterException(expression, typeof(RuntimeException), null, "InvalidGenericType", EnumExpressionEvaluatorStrings.InvalidGenericType, new object[0]);
			}
			this._underType = Enum.GetUnderlyingType(typeof(T));
			if (string.IsNullOrWhiteSpace(expression))
			{
				throw InterpreterError.NewInterpreterException(expression, typeof(RuntimeException), null, "EmptyInputString", EnumExpressionEvaluatorStrings.EmptyInputString, new object[0]);
			}
			List<FlagsExpression<T>.Token> list = this.TokenizeInput(expression);
			list.Add(new FlagsExpression<T>.Token(FlagsExpression<T>.TokenKind.Or));
			this.CheckSyntaxError(list);
			this._root = this.ConstructExpressionTree(list);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006150 File Offset: 0x00004350
		public FlagsExpression(object[] expression)
		{
			if (!typeof(T).GetTypeInfo().IsEnum)
			{
				throw InterpreterError.NewInterpreterException(expression, typeof(RuntimeException), null, "InvalidGenericType", EnumExpressionEvaluatorStrings.InvalidGenericType, new object[0]);
			}
			this._underType = Enum.GetUnderlyingType(typeof(T));
			if (expression == null)
			{
				throw InterpreterError.NewInterpreterException(null, typeof(ArgumentNullException), null, "EmptyInputString", EnumExpressionEvaluatorStrings.EmptyInputString, new object[0]);
			}
			foreach (string value in expression)
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw InterpreterError.NewInterpreterException(expression, typeof(RuntimeException), null, "EmptyInputString", EnumExpressionEvaluatorStrings.EmptyInputString, new object[0]);
				}
			}
			List<FlagsExpression<T>.Token> list = new List<FlagsExpression<T>.Token>();
			foreach (string input in expression)
			{
				list.AddRange(this.TokenizeInput(input));
				list.Add(new FlagsExpression<T>.Token(FlagsExpression<T>.TokenKind.Or));
			}
			this.CheckSyntaxError(list);
			this._root = this.ConstructExpressionTree(list);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00006271 File Offset: 0x00004471
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00006279 File Offset: 0x00004479
		internal FlagsExpression<T>.Node Root
		{
			get
			{
				return this._root;
			}
			set
			{
				this._root = value;
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006284 File Offset: 0x00004484
		public bool Evaluate(T value)
		{
			object val = LanguagePrimitives.ConvertTo(value, this._underType, CultureInfo.InvariantCulture);
			return this._root.Eval(val);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000062B4 File Offset: 0x000044B4
		internal bool ExistsInExpression(T flagName)
		{
			object enumVal = LanguagePrimitives.ConvertTo(flagName, this._underType, CultureInfo.InvariantCulture);
			return this._root.ExistEnum(enumVal);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000062E8 File Offset: 0x000044E8
		private List<FlagsExpression<T>.Token> TokenizeInput(string input)
		{
			List<FlagsExpression<T>.Token> list = new List<FlagsExpression<T>.Token>();
			int i = 0;
			while (i < input.Length)
			{
				this.FindNextToken(input, ref i);
				if (i < input.Length)
				{
					list.Add(this.GetNextToken(input, ref i));
				}
			}
			return list;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000632C File Offset: 0x0000452C
		private void FindNextToken(string input, ref int _offset)
		{
			while (_offset < input.Length)
			{
				char c = input[_offset++];
				if (!char.IsWhiteSpace(c))
				{
					_offset--;
					return;
				}
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006364 File Offset: 0x00004564
		private FlagsExpression<T>.Token GetNextToken(string input, ref int _offset)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			while (_offset < input.Length)
			{
				char c = input[_offset++];
				if (c == ',' || c == '+' || c == '!')
				{
					if (!flag)
					{
						stringBuilder.Append(c);
						break;
					}
					_offset--;
					break;
				}
				else
				{
					stringBuilder.Append(c);
					flag = true;
				}
			}
			string text = stringBuilder.ToString().Trim();
			if (text.Length >= 2 && ((text[0] == '\'' && text[text.Length - 1] == '\'') || (text[0] == '"' && text[text.Length - 1] == '"')))
			{
				text = text.Substring(1, text.Length - 2);
			}
			text = text.Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				throw InterpreterError.NewInterpreterException(input, typeof(RuntimeException), null, "EmptyTokenString", EnumExpressionEvaluatorStrings.EmptyTokenString, new object[]
				{
					EnumMinimumDisambiguation.EnumAllValues(typeof(T))
				});
			}
			if (text[0] == '(')
			{
				int num = input.IndexOf(')', _offset);
				if (text[text.Length - 1] == ')' || num >= 0)
				{
					throw InterpreterError.NewInterpreterException(input, typeof(RuntimeException), null, "NoIdentifierGroupingAllowed", EnumExpressionEvaluatorStrings.NoIdentifierGroupingAllowed, new object[0]);
				}
			}
			if (text.Equals(","))
			{
				return new FlagsExpression<T>.Token(FlagsExpression<T>.TokenKind.Or);
			}
			if (text.Equals("+"))
			{
				return new FlagsExpression<T>.Token(FlagsExpression<T>.TokenKind.And);
			}
			if (text.Equals("!"))
			{
				return new FlagsExpression<T>.Token(FlagsExpression<T>.TokenKind.Not);
			}
			return new FlagsExpression<T>.Token(text);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006500 File Offset: 0x00004700
		private void CheckSyntaxError(List<FlagsExpression<T>.Token> tokenList)
		{
			FlagsExpression<T>.TokenKind tokenKind = FlagsExpression<T>.TokenKind.Or;
			for (int i = 0; i < tokenList.Count; i++)
			{
				FlagsExpression<T>.Token token = tokenList[i];
				if (tokenKind == FlagsExpression<T>.TokenKind.Or || tokenKind == FlagsExpression<T>.TokenKind.And)
				{
					if (token.Kind == FlagsExpression<T>.TokenKind.Or || token.Kind == FlagsExpression<T>.TokenKind.And)
					{
						throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "SyntaxErrorUnexpectedBinaryOperator", EnumExpressionEvaluatorStrings.SyntaxErrorUnexpectedBinaryOperator, new object[0]);
					}
				}
				else if (tokenKind == FlagsExpression<T>.TokenKind.Not)
				{
					if (token.Kind != FlagsExpression<T>.TokenKind.Identifier)
					{
						throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "SyntaxErrorIdentifierExpected", EnumExpressionEvaluatorStrings.SyntaxErrorIdentifierExpected, new object[0]);
					}
				}
				else if (tokenKind == FlagsExpression<T>.TokenKind.Identifier && (token.Kind == FlagsExpression<T>.TokenKind.Identifier || token.Kind == FlagsExpression<T>.TokenKind.Not))
				{
					throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "SyntaxErrorBinaryOperatorExpected", EnumExpressionEvaluatorStrings.SyntaxErrorBinaryOperatorExpected, new object[0]);
				}
				if (token.Kind == FlagsExpression<T>.TokenKind.Identifier)
				{
					string text = token.Text;
					token.Text = EnumMinimumDisambiguation.EnumDisambiguate(text, typeof(T));
				}
				tokenKind = token.Kind;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006600 File Offset: 0x00004800
		private FlagsExpression<T>.Node ConstructExpressionTree(List<FlagsExpression<T>.Token> tokenList)
		{
			bool flag = false;
			Queue<FlagsExpression<T>.Node> queue = new Queue<FlagsExpression<T>.Node>();
			Queue<FlagsExpression<T>.Node> queue2 = new Queue<FlagsExpression<T>.Node>();
			for (int i = 0; i < tokenList.Count; i++)
			{
				FlagsExpression<T>.Token token = tokenList[i];
				FlagsExpression<T>.TokenKind kind = token.Kind;
				if (kind == FlagsExpression<T>.TokenKind.Identifier)
				{
					FlagsExpression<T>.Node node = new FlagsExpression<T>.OperandNode(token.Text);
					if (flag)
					{
						FlagsExpression<T>.Node node2 = new FlagsExpression<T>.NotNode();
						node2.Operand1 = node;
						flag = false;
						queue.Enqueue(node2);
					}
					else
					{
						queue.Enqueue(node);
					}
				}
				else if (kind == FlagsExpression<T>.TokenKind.Not)
				{
					flag = true;
				}
				else if (kind != FlagsExpression<T>.TokenKind.And && kind == FlagsExpression<T>.TokenKind.Or)
				{
					FlagsExpression<T>.Node node3 = queue.Dequeue();
					while (queue.Count > 0)
					{
						node3 = new FlagsExpression<T>.AndNode(node3)
						{
							Operand1 = queue.Dequeue()
						};
					}
					queue2.Enqueue(node3);
				}
			}
			FlagsExpression<T>.Node node4 = queue2.Dequeue();
			while (queue2.Count > 0)
			{
				node4 = new FlagsExpression<T>.OrNode(node4)
				{
					Operand1 = queue2.Dequeue()
				};
			}
			return node4;
		}

		// Token: 0x04000062 RID: 98
		private Type _underType;

		// Token: 0x04000063 RID: 99
		private FlagsExpression<T>.Node _root;

		// Token: 0x0200001C RID: 28
		internal enum TokenKind
		{
			// Token: 0x04000065 RID: 101
			Identifier,
			// Token: 0x04000066 RID: 102
			And,
			// Token: 0x04000067 RID: 103
			Or,
			// Token: 0x04000068 RID: 104
			Not
		}

		// Token: 0x0200001D RID: 29
		internal class Token
		{
			// Token: 0x1700004F RID: 79
			// (get) Token: 0x06000140 RID: 320 RVA: 0x000066F6 File Offset: 0x000048F6
			// (set) Token: 0x06000141 RID: 321 RVA: 0x000066FE File Offset: 0x000048FE
			public string Text
			{
				get
				{
					return this._text;
				}
				set
				{
					this._text = value;
				}
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x06000142 RID: 322 RVA: 0x00006707 File Offset: 0x00004907
			// (set) Token: 0x06000143 RID: 323 RVA: 0x0000670F File Offset: 0x0000490F
			public FlagsExpression<T>.TokenKind Kind
			{
				get
				{
					return this._kind;
				}
				set
				{
					this._kind = value;
				}
			}

			// Token: 0x06000144 RID: 324 RVA: 0x00006718 File Offset: 0x00004918
			internal Token(FlagsExpression<T>.TokenKind kind)
			{
				this._kind = kind;
				switch (kind)
				{
				case FlagsExpression<T>.TokenKind.And:
					this._text = "AND";
					return;
				case FlagsExpression<T>.TokenKind.Or:
					this._text = "OR";
					return;
				case FlagsExpression<T>.TokenKind.Not:
					this._text = "NOT";
					return;
				default:
					return;
				}
			}

			// Token: 0x06000145 RID: 325 RVA: 0x0000676C File Offset: 0x0000496C
			internal Token(string identifier)
			{
				this.Kind = FlagsExpression<T>.TokenKind.Identifier;
				this.Text = identifier;
			}

			// Token: 0x04000069 RID: 105
			private string _text;

			// Token: 0x0400006A RID: 106
			private FlagsExpression<T>.TokenKind _kind;
		}

		// Token: 0x0200001E RID: 30
		internal abstract class Node
		{
			// Token: 0x17000051 RID: 81
			// (get) Token: 0x06000146 RID: 326 RVA: 0x00006782 File Offset: 0x00004982
			// (set) Token: 0x06000147 RID: 327 RVA: 0x0000678A File Offset: 0x0000498A
			public FlagsExpression<T>.Node Operand1
			{
				get
				{
					return this._operand1;
				}
				set
				{
					this._operand1 = value;
				}
			}

			// Token: 0x06000148 RID: 328
			internal abstract bool Eval(object val);

			// Token: 0x06000149 RID: 329
			internal abstract bool ExistEnum(object enumVal);

			// Token: 0x0400006B RID: 107
			private FlagsExpression<T>.Node _operand1;
		}

		// Token: 0x0200001F RID: 31
		internal class OrNode : FlagsExpression<T>.Node
		{
			// Token: 0x17000052 RID: 82
			// (get) Token: 0x0600014B RID: 331 RVA: 0x0000679B File Offset: 0x0000499B
			// (set) Token: 0x0600014C RID: 332 RVA: 0x000067A3 File Offset: 0x000049A3
			public FlagsExpression<T>.Node Operand2
			{
				get
				{
					return this._operand2;
				}
				set
				{
					this._operand2 = value;
				}
			}

			// Token: 0x0600014D RID: 333 RVA: 0x000067AC File Offset: 0x000049AC
			public OrNode(FlagsExpression<T>.Node n)
			{
				this._operand2 = n;
			}

			// Token: 0x0600014E RID: 334 RVA: 0x000067BC File Offset: 0x000049BC
			internal override bool Eval(object val)
			{
				return base.Operand1.Eval(val) || this.Operand2.Eval(val);
			}

			// Token: 0x0600014F RID: 335 RVA: 0x000067E8 File Offset: 0x000049E8
			internal override bool ExistEnum(object enumVal)
			{
				return base.Operand1.ExistEnum(enumVal) || this.Operand2.ExistEnum(enumVal);
			}

			// Token: 0x0400006C RID: 108
			private FlagsExpression<T>.Node _operand2;
		}

		// Token: 0x02000020 RID: 32
		internal class AndNode : FlagsExpression<T>.Node
		{
			// Token: 0x17000053 RID: 83
			// (get) Token: 0x06000150 RID: 336 RVA: 0x00006814 File Offset: 0x00004A14
			// (set) Token: 0x06000151 RID: 337 RVA: 0x0000681C File Offset: 0x00004A1C
			public FlagsExpression<T>.Node Operand2
			{
				get
				{
					return this._operand2;
				}
				set
				{
					this._operand2 = value;
				}
			}

			// Token: 0x06000152 RID: 338 RVA: 0x00006825 File Offset: 0x00004A25
			public AndNode(FlagsExpression<T>.Node n)
			{
				this._operand2 = n;
			}

			// Token: 0x06000153 RID: 339 RVA: 0x00006834 File Offset: 0x00004A34
			internal override bool Eval(object val)
			{
				return base.Operand1.Eval(val) && this.Operand2.Eval(val);
			}

			// Token: 0x06000154 RID: 340 RVA: 0x00006860 File Offset: 0x00004A60
			internal override bool ExistEnum(object enumVal)
			{
				return base.Operand1.ExistEnum(enumVal) || this.Operand2.ExistEnum(enumVal);
			}

			// Token: 0x0400006D RID: 109
			private FlagsExpression<T>.Node _operand2;
		}

		// Token: 0x02000021 RID: 33
		internal class NotNode : FlagsExpression<T>.Node
		{
			// Token: 0x06000155 RID: 341 RVA: 0x0000688C File Offset: 0x00004A8C
			internal override bool Eval(object val)
			{
				return !base.Operand1.Eval(val);
			}

			// Token: 0x06000156 RID: 342 RVA: 0x000068AC File Offset: 0x00004AAC
			internal override bool ExistEnum(object enumVal)
			{
				return base.Operand1.ExistEnum(enumVal);
			}
		}

		// Token: 0x02000022 RID: 34
		internal class OperandNode : FlagsExpression<T>.Node
		{
			// Token: 0x17000054 RID: 84
			// (get) Token: 0x06000158 RID: 344 RVA: 0x000068CF File Offset: 0x00004ACF
			// (set) Token: 0x06000159 RID: 345 RVA: 0x000068D7 File Offset: 0x00004AD7
			public object OperandValue
			{
				get
				{
					return this._operandValue;
				}
				set
				{
					this._operandValue = value;
				}
			}

			// Token: 0x0600015A RID: 346 RVA: 0x000068E0 File Offset: 0x00004AE0
			internal OperandNode(string enumString)
			{
				Type typeFromHandle = typeof(T);
				Type underlyingType = Enum.GetUnderlyingType(typeFromHandle);
				FieldInfo field = typeFromHandle.GetField(enumString);
				this._operandValue = LanguagePrimitives.ConvertTo(field.GetValue(typeFromHandle), underlyingType, CultureInfo.InvariantCulture);
			}

			// Token: 0x0600015B RID: 347 RVA: 0x00006928 File Offset: 0x00004B28
			internal override bool Eval(object val)
			{
				Type underlyingType = Enum.GetUnderlyingType(typeof(T));
				bool result;
				if (this.isUnsigned(underlyingType))
				{
					ulong num = (ulong)LanguagePrimitives.ConvertTo(val, typeof(ulong), CultureInfo.InvariantCulture);
					ulong num2 = (ulong)LanguagePrimitives.ConvertTo(this._operandValue, typeof(ulong), CultureInfo.InvariantCulture);
					result = (num2 == (num & num2));
				}
				else
				{
					long num3 = (long)LanguagePrimitives.ConvertTo(val, typeof(long), CultureInfo.InvariantCulture);
					long num4 = (long)LanguagePrimitives.ConvertTo(this._operandValue, typeof(long), CultureInfo.InvariantCulture);
					result = (num4 == (num3 & num4));
				}
				return result;
			}

			// Token: 0x0600015C RID: 348 RVA: 0x000069DC File Offset: 0x00004BDC
			internal override bool ExistEnum(object enumVal)
			{
				Type underlyingType = Enum.GetUnderlyingType(typeof(T));
				bool result;
				if (this.isUnsigned(underlyingType))
				{
					ulong num = (ulong)LanguagePrimitives.ConvertTo(enumVal, typeof(ulong), CultureInfo.InvariantCulture);
					ulong num2 = (ulong)LanguagePrimitives.ConvertTo(this._operandValue, typeof(ulong), CultureInfo.InvariantCulture);
					result = (num == (num & num2));
				}
				else
				{
					long num3 = (long)LanguagePrimitives.ConvertTo(enumVal, typeof(long), CultureInfo.InvariantCulture);
					long num4 = (long)LanguagePrimitives.ConvertTo(this._operandValue, typeof(long), CultureInfo.InvariantCulture);
					result = (num3 == (num3 & num4));
				}
				return result;
			}

			// Token: 0x0600015D RID: 349 RVA: 0x00006A90 File Offset: 0x00004C90
			private bool isUnsigned(Type type)
			{
				return type == typeof(ulong) || type == typeof(uint) || type == typeof(ushort) || type == typeof(byte);
			}

			// Token: 0x0400006E RID: 110
			internal object _operandValue;
		}
	}
}
