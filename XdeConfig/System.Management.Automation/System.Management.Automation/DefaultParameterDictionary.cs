using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Language;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000052 RID: 82
	public sealed class DefaultParameterDictionary : Hashtable
	{
		// Token: 0x06000483 RID: 1155 RVA: 0x00013FB4 File Offset: 0x000121B4
		public bool ChangeSinceLastCheck()
		{
			bool isChanged = this._isChanged;
			this._isChanged = false;
			return isChanged;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00013FD0 File Offset: 0x000121D0
		public DefaultParameterDictionary() : base(StringComparer.OrdinalIgnoreCase)
		{
			this._isChanged = true;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00013FE4 File Offset: 0x000121E4
		public DefaultParameterDictionary(IDictionary dictionary) : this()
		{
			if (dictionary == null)
			{
				throw PSTraceSource.NewArgumentNullException("dictionary");
			}
			List<object> list = new List<object>();
			foreach (object obj in dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key as string;
				if (text != null)
				{
					string text2 = text.Trim();
					string text3 = null;
					string text4 = null;
					if (!DefaultParameterDictionary.CheckKeyIsValid(text2, ref text3, ref text4) && !text2.Equals("Disabled", StringComparison.OrdinalIgnoreCase))
					{
						list.Add(text);
					}
					else if (list.Count == 0 && !base.ContainsKey(text2))
					{
						base.Add(text2, dictionaryEntry.Value);
					}
				}
				else
				{
					list.Add(dictionaryEntry.Key);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj2 in list)
			{
				stringBuilder.Append(obj2.ToString() + ", ");
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
				string resourceString = (list.Count > 1) ? ParameterBinderStrings.MultipleKeysInBadFormat : ParameterBinderStrings.SingleKeyInBadFormat;
				throw PSTraceSource.NewInvalidOperationException(resourceString, new object[]
				{
					stringBuilder
				});
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00014170 File Offset: 0x00012370
		public override bool Contains(object key)
		{
			return this.ContainsKey(key);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001417C File Offset: 0x0001237C
		public override bool ContainsKey(object key)
		{
			if (key == null)
			{
				throw PSTraceSource.NewArgumentNullException("key");
			}
			string text = key as string;
			if (text == null)
			{
				return false;
			}
			string key2 = text.Trim();
			return base.ContainsKey(key2);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000141B1 File Offset: 0x000123B1
		public override void Add(object key, object value)
		{
			this.AddImpl(key, value, false);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000141BC File Offset: 0x000123BC
		private void AddImpl(object key, object value, bool isSelfIndexing)
		{
			if (key == null)
			{
				throw PSTraceSource.NewArgumentNullException("key");
			}
			string text = key as string;
			if (text == null)
			{
				throw PSTraceSource.NewArgumentException("key", ParameterBinderStrings.StringValueKeyExpected, new object[]
				{
					key,
					key.GetType().FullName
				});
			}
			string text2 = text.Trim();
			string text3 = null;
			string text4 = null;
			if (base.ContainsKey(text2))
			{
				if (isSelfIndexing)
				{
					this._isChanged = true;
					base[text2] = value;
					return;
				}
				throw PSTraceSource.NewArgumentException("key", ParameterBinderStrings.KeyAlreadyAdded, new object[]
				{
					key
				});
			}
			else
			{
				if (!DefaultParameterDictionary.CheckKeyIsValid(text2, ref text3, ref text4) && !text2.Equals("Disabled", StringComparison.OrdinalIgnoreCase))
				{
					throw PSTraceSource.NewInvalidOperationException(ParameterBinderStrings.SingleKeyInBadFormat, new object[]
					{
						key
					});
				}
				this._isChanged = true;
				base.Add(text2, value);
				return;
			}
		}

		// Token: 0x1700012A RID: 298
		public override object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw PSTraceSource.NewArgumentNullException("key");
				}
				string text = key as string;
				if (text == null)
				{
					return null;
				}
				string key2 = text.Trim();
				return base[key2];
			}
			set
			{
				this.AddImpl(key, value, true);
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000142D8 File Offset: 0x000124D8
		public override void Remove(object key)
		{
			if (key == null)
			{
				throw PSTraceSource.NewArgumentNullException("key");
			}
			string text = key as string;
			if (text == null)
			{
				return;
			}
			string key2 = text.Trim();
			if (base.ContainsKey(key2))
			{
				base.Remove(key2);
				this._isChanged = true;
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001431C File Offset: 0x0001251C
		public override void Clear()
		{
			base.Clear();
			this._isChanged = true;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001432C File Offset: 0x0001252C
		internal static bool CheckKeyIsValid(string key, ref string cmdletName, ref string parameterName)
		{
			if (key == string.Empty)
			{
				return false;
			}
			int num = DefaultParameterDictionary.GetValueToken(0, key, ref cmdletName, true);
			if (num == -1)
			{
				return false;
			}
			num = DefaultParameterDictionary.SkipWhiteSpace(num, key);
			if (num == -1 || key[num] != ':')
			{
				return false;
			}
			num = DefaultParameterDictionary.SkipWhiteSpace(num + 1, key);
			if (num == -1)
			{
				return false;
			}
			num = DefaultParameterDictionary.GetValueToken(num, key, ref parameterName, false);
			return num != -1 && num == key.Length;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001439C File Offset: 0x0001259C
		private static int GetValueToken(int index, string key, ref string name, bool getCmdletName)
		{
			char c = '\0';
			if (key[index].IsSingleQuote() || key[index].IsDoubleQuote())
			{
				c = key[index];
				index++;
			}
			StringBuilder stringBuilder = new StringBuilder(string.Empty);
			while (index < key.Length)
			{
				if (c != '\0')
				{
					if ((c.IsSingleQuote() && key[index].IsSingleQuote()) || (c.IsDoubleQuote() && key[index].IsDoubleQuote()))
					{
						name = stringBuilder.ToString().Trim();
						if (name.Length != 0)
						{
							return index + 1;
						}
						return -1;
					}
					else
					{
						stringBuilder.Append(key[index]);
					}
				}
				else if (getCmdletName)
				{
					if (key[index] != ':')
					{
						stringBuilder.Append(key[index]);
					}
					else
					{
						name = stringBuilder.ToString().Trim();
						if (name.Length != 0)
						{
							return index;
						}
						return -1;
					}
				}
				else
				{
					stringBuilder.Append(key[index]);
				}
				index++;
			}
			if (!getCmdletName && c == '\0')
			{
				name = stringBuilder.ToString().Trim();
				return index;
			}
			return -1;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000144AC File Offset: 0x000126AC
		private static int SkipWhiteSpace(int index, string key)
		{
			while (index < key.Length)
			{
				if (!key[index].IsWhitespace() && key[index] != '\r' && key[index] != '\n')
				{
					return index;
				}
				index++;
			}
			return -1;
		}

		// Token: 0x040001AB RID: 427
		private bool _isChanged;
	}
}
