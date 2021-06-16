using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000CF RID: 207
	internal class BooleanQueryExpression : QueryExpression
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00030718 File Offset: 0x0002E918
		// (set) Token: 0x06000BFA RID: 3066 RVA: 0x00030720 File Offset: 0x0002E920
		public object Left { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00030729 File Offset: 0x0002E929
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00030731 File Offset: 0x0002E931
		public object Right { get; set; }

		// Token: 0x06000BFD RID: 3069 RVA: 0x0003073C File Offset: 0x0002E93C
		private IEnumerable<JToken> GetResult(JToken root, JToken t, object o)
		{
			JToken jtoken;
			if ((jtoken = (o as JToken)) != null)
			{
				return new JToken[]
				{
					jtoken
				};
			}
			List<PathFilter> filters;
			if ((filters = (o as List<PathFilter>)) != null)
			{
				return JPath.Evaluate(filters, root, t, false);
			}
			return CollectionUtils.ArrayEmpty<JToken>();
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00030778 File Offset: 0x0002E978
		public override bool IsMatch(JToken root, JToken t)
		{
			if (base.Operator == QueryOperator.Exists)
			{
				return this.GetResult(root, t, this.Left).Any<JToken>();
			}
			using (IEnumerator<JToken> enumerator = this.GetResult(root, t, this.Left).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					IEnumerable<JToken> result = this.GetResult(root, t, this.Right);
					ICollection<JToken> collection = (result as ICollection<JToken>) ?? result.ToList<JToken>();
					do
					{
						JToken leftResult = enumerator.Current;
						foreach (JToken rightResult in collection)
						{
							if (this.MatchTokens(leftResult, rightResult))
							{
								return true;
							}
						}
					}
					while (enumerator.MoveNext());
				}
			}
			return false;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00030854 File Offset: 0x0002EA54
		private bool MatchTokens(JToken leftResult, JToken rightResult)
		{
			JValue jvalue;
			JValue jvalue2;
			if ((jvalue = (leftResult as JValue)) != null && (jvalue2 = (rightResult as JValue)) != null)
			{
				switch (base.Operator)
				{
				case QueryOperator.Equals:
					if (BooleanQueryExpression.EqualsWithStringCoercion(jvalue, jvalue2))
					{
						return true;
					}
					break;
				case QueryOperator.NotEquals:
					if (!BooleanQueryExpression.EqualsWithStringCoercion(jvalue, jvalue2))
					{
						return true;
					}
					break;
				case QueryOperator.Exists:
					return true;
				case QueryOperator.LessThan:
					if (jvalue.CompareTo(jvalue2) < 0)
					{
						return true;
					}
					break;
				case QueryOperator.LessThanOrEquals:
					if (jvalue.CompareTo(jvalue2) <= 0)
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThan:
					if (jvalue.CompareTo(jvalue2) > 0)
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThanOrEquals:
					if (jvalue.CompareTo(jvalue2) >= 0)
					{
						return true;
					}
					break;
				case QueryOperator.RegexEquals:
					if (BooleanQueryExpression.RegexEquals(jvalue, jvalue2))
					{
						return true;
					}
					break;
				case QueryOperator.StrictEquals:
					if (BooleanQueryExpression.EqualsWithStrictMatch(jvalue, jvalue2))
					{
						return true;
					}
					break;
				case QueryOperator.StrictNotEquals:
					if (!BooleanQueryExpression.EqualsWithStrictMatch(jvalue, jvalue2))
					{
						return true;
					}
					break;
				}
			}
			else
			{
				QueryOperator @operator = base.Operator;
				if (@operator - QueryOperator.NotEquals <= 1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00030938 File Offset: 0x0002EB38
		private static bool RegexEquals(JValue input, JValue pattern)
		{
			if (input.Type != JTokenType.String || pattern.Type != JTokenType.String)
			{
				return false;
			}
			string text = (string)pattern.Value;
			int num = text.LastIndexOf('/');
			string pattern2 = text.Substring(1, num - 1);
			string optionsText = text.Substring(num + 1);
			return Regex.IsMatch((string)input.Value, pattern2, MiscellaneousUtils.GetRegexOptions(optionsText));
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00030998 File Offset: 0x0002EB98
		internal static bool EqualsWithStringCoercion(JValue value, JValue queryValue)
		{
			if (value.Equals(queryValue))
			{
				return true;
			}
			if ((value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float) || (value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer))
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			if (queryValue.Type != JTokenType.String)
			{
				return false;
			}
			string b = (string)queryValue.Value;
			string a;
			switch (value.Type)
			{
			case JTokenType.Date:
				using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
				{
					object value2;
					if ((value2 = value.Value) is DateTimeOffset)
					{
						DateTimeOffset value3 = (DateTimeOffset)value2;
						DateTimeUtils.WriteDateTimeOffsetString(stringWriter, value3, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
					}
					else
					{
						DateTimeUtils.WriteDateTimeString(stringWriter, (DateTime)value.Value, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
					}
					a = stringWriter.ToString();
					goto IL_121;
				}
				break;
			case JTokenType.Raw:
				return false;
			case JTokenType.Bytes:
				break;
			case JTokenType.Guid:
			case JTokenType.TimeSpan:
				a = value.Value.ToString();
				goto IL_121;
			case JTokenType.Uri:
				a = ((Uri)value.Value).OriginalString;
				goto IL_121;
			default:
				return false;
			}
			a = Convert.ToBase64String((byte[])value.Value);
			IL_121:
			return string.Equals(a, b, StringComparison.Ordinal);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00030AE0 File Offset: 0x0002ECE0
		internal static bool EqualsWithStrictMatch(JValue value, JValue queryValue)
		{
			if ((value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float) || (value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer))
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			return value.Type == queryValue.Type && value.Equals(queryValue);
		}
	}
}
