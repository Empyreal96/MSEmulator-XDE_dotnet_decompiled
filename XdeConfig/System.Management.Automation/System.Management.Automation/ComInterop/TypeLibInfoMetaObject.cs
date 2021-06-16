using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A8C RID: 2700
	internal sealed class TypeLibInfoMetaObject : DynamicMetaObject
	{
		// Token: 0x06006B34 RID: 27444 RVA: 0x0021A0BB File Offset: 0x002182BB
		internal TypeLibInfoMetaObject(Expression expression, ComTypeLibInfo info) : base(expression, BindingRestrictions.Empty, info)
		{
			this._info = info;
		}

		// Token: 0x06006B35 RID: 27445 RVA: 0x0021A0D4 File Offset: 0x002182D4
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			string text = binder.Name;
			if (text == this._info.Name)
			{
				text = "TypeLibDesc";
			}
			else if (text != "Guid" && text != "Name" && text != "VersionMajor" && text != "VersionMinor")
			{
				return binder.FallbackGetMember(this);
			}
			return new DynamicMetaObject(Expression.Convert(Expression.Property(Utils.Convert(base.Expression, typeof(ComTypeLibInfo)), typeof(ComTypeLibInfo).GetProperty(text)), typeof(object)), this.ComTypeLibInfoRestrictions(new DynamicMetaObject[]
			{
				this
			}));
		}

		// Token: 0x06006B36 RID: 27446 RVA: 0x0021A18F File Offset: 0x0021838F
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._info.GetMemberNames();
		}

		// Token: 0x06006B37 RID: 27447 RVA: 0x0021A19C File Offset: 0x0021839C
		private BindingRestrictions ComTypeLibInfoRestrictions(params DynamicMetaObject[] args)
		{
			return BindingRestrictions.Combine(args).Merge(BindingRestrictions.GetTypeRestriction(base.Expression, typeof(ComTypeLibInfo)));
		}

		// Token: 0x0400333B RID: 13115
		private readonly ComTypeLibInfo _info;
	}
}
