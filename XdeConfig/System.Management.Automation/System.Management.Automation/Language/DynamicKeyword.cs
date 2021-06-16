using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x020005DD RID: 1501
	public class DynamicKeyword
	{
		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06003FFD RID: 16381 RVA: 0x00151D54 File Offset: 0x0014FF54
		private static Dictionary<string, DynamicKeyword> DynamicKeywords
		{
			get
			{
				Dictionary<string, DynamicKeyword> result;
				if ((result = DynamicKeyword._dynamicKeywords) == null)
				{
					result = (DynamicKeyword._dynamicKeywords = new Dictionary<string, DynamicKeyword>(StringComparer.OrdinalIgnoreCase));
				}
				return result;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06003FFE RID: 16382 RVA: 0x00151D6F File Offset: 0x0014FF6F
		private static Stack<Dictionary<string, DynamicKeyword>> DynamicKeywordsStack
		{
			get
			{
				Stack<Dictionary<string, DynamicKeyword>> result;
				if ((result = DynamicKeyword._dynamicKeywordsStack) == null)
				{
					result = (DynamicKeyword._dynamicKeywordsStack = new Stack<Dictionary<string, DynamicKeyword>>());
				}
				return result;
			}
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x00151D85 File Offset: 0x0014FF85
		public static void Reset()
		{
			DynamicKeyword._dynamicKeywords = new Dictionary<string, DynamicKeyword>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x00151D96 File Offset: 0x0014FF96
		public static void Push()
		{
			DynamicKeyword.DynamicKeywordsStack.Push(DynamicKeyword._dynamicKeywords);
			DynamicKeyword.Reset();
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x00151DAC File Offset: 0x0014FFAC
		public static void Pop()
		{
			DynamicKeyword._dynamicKeywords = DynamicKeyword.DynamicKeywordsStack.Pop();
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x00151DC0 File Offset: 0x0014FFC0
		public static DynamicKeyword GetKeyword(string name)
		{
			DynamicKeyword result;
			DynamicKeyword.DynamicKeywords.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x00151DDC File Offset: 0x0014FFDC
		public static List<DynamicKeyword> GetKeyword()
		{
			return new List<DynamicKeyword>(DynamicKeyword.DynamicKeywords.Values);
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x00151DF0 File Offset: 0x0014FFF0
		public static bool ContainsKeyword(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				PSArgumentNullException ex = PSTraceSource.NewArgumentNullException("name");
				throw ex;
			}
			return DynamicKeyword.DynamicKeywords.ContainsKey(name);
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x00151E20 File Offset: 0x00150020
		public static void AddKeyword(DynamicKeyword keywordToAdd)
		{
			if (keywordToAdd == null)
			{
				PSArgumentNullException ex = PSTraceSource.NewArgumentNullException("keywordToAdd");
				throw ex;
			}
			string keyword = keywordToAdd.Keyword;
			if (string.IsNullOrEmpty(keyword))
			{
				throw PSTraceSource.NewArgumentNullException("keywordToAdd.Keyword");
			}
			if (DynamicKeyword.DynamicKeywords.ContainsKey(keyword))
			{
				DynamicKeyword.DynamicKeywords.Remove(keyword);
			}
			DynamicKeyword.DynamicKeywords.Add(keyword, keywordToAdd);
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x00151E7C File Offset: 0x0015007C
		public static void RemoveKeyword(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				PSArgumentNullException ex = PSTraceSource.NewArgumentNullException("name");
				throw ex;
			}
			if (DynamicKeyword.DynamicKeywords.ContainsKey(name))
			{
				DynamicKeyword.DynamicKeywords.Remove(name);
			}
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x00151EB8 File Offset: 0x001500B8
		internal static bool IsHiddenKeyword(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				PSArgumentNullException ex = PSTraceSource.NewArgumentNullException("name");
				throw ex;
			}
			return DynamicKeyword.HiddenDynamicKeywords.Contains(name);
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x00151EE8 File Offset: 0x001500E8
		public DynamicKeyword Copy()
		{
			DynamicKeyword dynamicKeyword = new DynamicKeyword
			{
				ImplementingModule = this.ImplementingModule,
				ImplementingModuleVersion = this.ImplementingModuleVersion,
				Keyword = this.Keyword,
				ResourceName = this.ResourceName,
				BodyMode = this.BodyMode,
				DirectCall = this.DirectCall,
				NameMode = this.NameMode,
				MetaStatement = this.MetaStatement,
				IsReservedKeyword = this.IsReservedKeyword,
				HasReservedProperties = this.HasReservedProperties,
				PreParse = this.PreParse,
				PostParse = this.PostParse,
				SemanticCheck = this.SemanticCheck
			};
			foreach (KeyValuePair<string, DynamicKeywordProperty> keyValuePair in this.Properties)
			{
				dynamicKeyword.Properties.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<string, DynamicKeywordParameter> keyValuePair2 in this.Parameters)
			{
				dynamicKeyword.Parameters.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
			return dynamicKeyword;
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06004009 RID: 16393 RVA: 0x00152048 File Offset: 0x00150248
		// (set) Token: 0x0600400A RID: 16394 RVA: 0x00152050 File Offset: 0x00150250
		public string ImplementingModule { get; set; }

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x0600400B RID: 16395 RVA: 0x00152059 File Offset: 0x00150259
		// (set) Token: 0x0600400C RID: 16396 RVA: 0x00152061 File Offset: 0x00150261
		public Version ImplementingModuleVersion { get; set; }

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x0600400D RID: 16397 RVA: 0x0015206A File Offset: 0x0015026A
		// (set) Token: 0x0600400E RID: 16398 RVA: 0x00152072 File Offset: 0x00150272
		public string Keyword { get; set; }

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x0600400F RID: 16399 RVA: 0x0015207B File Offset: 0x0015027B
		// (set) Token: 0x06004010 RID: 16400 RVA: 0x00152083 File Offset: 0x00150283
		public string ResourceName { get; set; }

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06004011 RID: 16401 RVA: 0x0015208C File Offset: 0x0015028C
		// (set) Token: 0x06004012 RID: 16402 RVA: 0x00152094 File Offset: 0x00150294
		public DynamicKeywordBodyMode BodyMode { get; set; }

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06004013 RID: 16403 RVA: 0x0015209D File Offset: 0x0015029D
		// (set) Token: 0x06004014 RID: 16404 RVA: 0x001520A5 File Offset: 0x001502A5
		public bool DirectCall { get; set; }

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06004015 RID: 16405 RVA: 0x001520AE File Offset: 0x001502AE
		// (set) Token: 0x06004016 RID: 16406 RVA: 0x001520B6 File Offset: 0x001502B6
		public DynamicKeywordNameMode NameMode { get; set; }

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06004017 RID: 16407 RVA: 0x001520BF File Offset: 0x001502BF
		// (set) Token: 0x06004018 RID: 16408 RVA: 0x001520C7 File Offset: 0x001502C7
		public bool MetaStatement { get; set; }

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06004019 RID: 16409 RVA: 0x001520D0 File Offset: 0x001502D0
		// (set) Token: 0x0600401A RID: 16410 RVA: 0x001520D8 File Offset: 0x001502D8
		public bool IsReservedKeyword { get; set; }

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x0600401B RID: 16411 RVA: 0x001520E1 File Offset: 0x001502E1
		// (set) Token: 0x0600401C RID: 16412 RVA: 0x001520E9 File Offset: 0x001502E9
		public bool HasReservedProperties { get; set; }

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x0600401D RID: 16413 RVA: 0x001520F4 File Offset: 0x001502F4
		public Dictionary<string, DynamicKeywordProperty> Properties
		{
			get
			{
				Dictionary<string, DynamicKeywordProperty> result;
				if ((result = this._properties) == null)
				{
					result = (this._properties = new Dictionary<string, DynamicKeywordProperty>(StringComparer.OrdinalIgnoreCase));
				}
				return result;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x0600401E RID: 16414 RVA: 0x00152120 File Offset: 0x00150320
		public Dictionary<string, DynamicKeywordParameter> Parameters
		{
			get
			{
				Dictionary<string, DynamicKeywordParameter> result;
				if ((result = this._parameters) == null)
				{
					result = (this._parameters = new Dictionary<string, DynamicKeywordParameter>(StringComparer.OrdinalIgnoreCase));
				}
				return result;
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x0600401F RID: 16415 RVA: 0x0015214A File Offset: 0x0015034A
		// (set) Token: 0x06004020 RID: 16416 RVA: 0x00152152 File Offset: 0x00150352
		public Func<DynamicKeyword, ParseError[]> PreParse { get; set; }

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x0015215B File Offset: 0x0015035B
		// (set) Token: 0x06004022 RID: 16418 RVA: 0x00152163 File Offset: 0x00150363
		public Func<DynamicKeywordStatementAst, ParseError[]> PostParse { get; set; }

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06004023 RID: 16419 RVA: 0x0015216C File Offset: 0x0015036C
		// (set) Token: 0x06004024 RID: 16420 RVA: 0x00152174 File Offset: 0x00150374
		public Func<DynamicKeywordStatementAst, ParseError[]> SemanticCheck { get; set; }

		// Token: 0x0400203B RID: 8251
		[ThreadStatic]
		private static Dictionary<string, DynamicKeyword> _dynamicKeywords;

		// Token: 0x0400203C RID: 8252
		[ThreadStatic]
		private static Stack<Dictionary<string, DynamicKeyword>> _dynamicKeywordsStack;

		// Token: 0x0400203D RID: 8253
		private static readonly HashSet<string> HiddenDynamicKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"MSFT_Credential"
		};

		// Token: 0x0400203E RID: 8254
		private Dictionary<string, DynamicKeywordProperty> _properties;

		// Token: 0x0400203F RID: 8255
		private Dictionary<string, DynamicKeywordParameter> _parameters;
	}
}
