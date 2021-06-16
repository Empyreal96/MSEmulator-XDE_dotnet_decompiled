using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000CE RID: 206
	internal class CompositeExpression : QueryExpression
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00030636 File Offset: 0x0002E836
		// (set) Token: 0x06000BF6 RID: 3062 RVA: 0x0003063E File Offset: 0x0002E83E
		public List<QueryExpression> Expressions { get; set; }

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00030647 File Offset: 0x0002E847
		public CompositeExpression()
		{
			this.Expressions = new List<QueryExpression>();
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0003065C File Offset: 0x0002E85C
		public override bool IsMatch(JToken root, JToken t)
		{
			QueryOperator @operator = base.Operator;
			if (@operator == QueryOperator.And)
			{
				using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsMatch(root, t))
						{
							return false;
						}
					}
				}
				return true;
			}
			if (@operator != QueryOperator.Or)
			{
				throw new ArgumentOutOfRangeException();
			}
			using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMatch(root, t))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
