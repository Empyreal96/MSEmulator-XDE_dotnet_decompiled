using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A9B RID: 2715
	internal class VariantBuilder
	{
		// Token: 0x17001DE5 RID: 7653
		// (get) Token: 0x06006BAF RID: 27567 RVA: 0x0021C5ED File Offset: 0x0021A7ED
		// (set) Token: 0x06006BB0 RID: 27568 RVA: 0x0021C5F5 File Offset: 0x0021A7F5
		internal ParameterExpression TempVariable { get; private set; }

		// Token: 0x06006BB1 RID: 27569 RVA: 0x0021C5FE File Offset: 0x0021A7FE
		internal VariantBuilder(VarEnum targetComType, ArgBuilder builder)
		{
			this._targetComType = targetComType;
			this._argBuilder = builder;
		}

		// Token: 0x17001DE6 RID: 7654
		// (get) Token: 0x06006BB2 RID: 27570 RVA: 0x0021C614 File Offset: 0x0021A814
		internal bool IsByRef
		{
			get
			{
				return (this._targetComType & VarEnum.VT_BYREF) != VarEnum.VT_EMPTY;
			}
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x0021C628 File Offset: 0x0021A828
		internal Expression InitializeArgumentVariant(MemberExpression variant, Expression parameter)
		{
			this._variant = variant;
			if (this.IsByRef)
			{
				Expression expression = this._argBuilder.MarshalToRef(parameter);
				this.TempVariable = Expression.Variable(expression.Type, null);
				return Expression.Block(Expression.Assign(this.TempVariable, expression), Expression.Call(variant, Variant.GetByrefSetter(this._targetComType & (VarEnum)(-16385)), new Expression[]
				{
					this.TempVariable
				}));
			}
			Expression expression2 = this._argBuilder.Marshal(parameter);
			if (this._argBuilder is ConvertibleArgBuilder)
			{
				return Expression.Call(variant, typeof(Variant).GetMethod("SetAsIConvertible"), new Expression[]
				{
					expression2
				});
			}
			if (Variant.IsPrimitiveType(this._targetComType) || this._targetComType == VarEnum.VT_DISPATCH || this._targetComType == VarEnum.VT_UNKNOWN || this._targetComType == VarEnum.VT_VARIANT || this._targetComType == VarEnum.VT_RECORD || this._targetComType == VarEnum.VT_ARRAY)
			{
				return Expression.Assign(Expression.Property(variant, Variant.GetAccessor(this._targetComType)), expression2);
			}
			switch (this._targetComType)
			{
			case VarEnum.VT_EMPTY:
				return null;
			case VarEnum.VT_NULL:
				return Expression.Call(variant, typeof(Variant).GetMethod("SetAsNull"));
			default:
				return null;
			}
		}

		// Token: 0x06006BB4 RID: 27572 RVA: 0x0021C76F File Offset: 0x0021A96F
		private static Expression Release(Expression pUnk)
		{
			return Expression.Call(typeof(UnsafeMethods).GetMethod("IUnknownReleaseNotZero"), pUnk);
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x0021C78C File Offset: 0x0021A98C
		internal Expression Clear()
		{
			if (!this.IsByRef)
			{
				VarEnum targetComType = this._targetComType;
				if (targetComType <= VarEnum.VT_UNKNOWN)
				{
					switch (targetComType)
					{
					case VarEnum.VT_EMPTY:
					case VarEnum.VT_NULL:
						return null;
					default:
						switch (targetComType)
						{
						case VarEnum.VT_BSTR:
						case VarEnum.VT_DISPATCH:
						case VarEnum.VT_VARIANT:
						case VarEnum.VT_UNKNOWN:
							break;
						case VarEnum.VT_ERROR:
						case VarEnum.VT_BOOL:
							goto IL_106;
						default:
							goto IL_106;
						}
						break;
					}
				}
				else if (targetComType != VarEnum.VT_RECORD && targetComType != VarEnum.VT_ARRAY)
				{
					goto IL_106;
				}
				return Expression.Call(this._variant, typeof(Variant).GetMethod("Clear"));
				IL_106:
				return null;
			}
			if (this._argBuilder is StringArgBuilder)
			{
				return Expression.Call(typeof(Marshal).GetMethod("FreeBSTR"), this.TempVariable);
			}
			if (this._argBuilder is DispatchArgBuilder)
			{
				return VariantBuilder.Release(this.TempVariable);
			}
			if (this._argBuilder is UnknownArgBuilder)
			{
				return VariantBuilder.Release(this.TempVariable);
			}
			if (this._argBuilder is VariantArgBuilder)
			{
				return Expression.Call(this.TempVariable, typeof(Variant).GetMethod("Clear"));
			}
			return null;
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x0021C8A0 File Offset: 0x0021AAA0
		internal Expression UpdateFromReturn(Expression parameter)
		{
			if (this.TempVariable == null)
			{
				return null;
			}
			return Expression.Assign(parameter, Helpers.Convert(this._argBuilder.UnmarshalFromRef(this.TempVariable), parameter.Type));
		}

		// Token: 0x04003373 RID: 13171
		private MemberExpression _variant;

		// Token: 0x04003374 RID: 13172
		private readonly ArgBuilder _argBuilder;

		// Token: 0x04003375 RID: 13173
		private readonly VarEnum _targetComType;
	}
}
