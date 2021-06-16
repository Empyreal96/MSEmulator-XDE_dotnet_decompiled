using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200003B RID: 59
	internal class BidirectionalDictionary<TFirst, TSecond>
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x00010914 File Offset: 0x0000EB14
		public BidirectionalDictionary() : this(EqualityComparer<TFirst>.Default, EqualityComparer<TSecond>.Default)
		{
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00010926 File Offset: 0x0000EB26
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer) : this(firstEqualityComparer, secondEqualityComparer, "Duplicate item already exists for '{0}'.", "Duplicate item already exists for '{0}'.")
		{
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001093A File Offset: 0x0000EB3A
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer, string duplicateFirstErrorMessage, string duplicateSecondErrorMessage)
		{
			this._firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
			this._secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
			this._duplicateFirstErrorMessage = duplicateFirstErrorMessage;
			this._duplicateSecondErrorMessage = duplicateSecondErrorMessage;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001096C File Offset: 0x0000EB6C
		public void Set(TFirst first, TSecond second)
		{
			TSecond tsecond;
			if (this._firstToSecond.TryGetValue(first, out tsecond) && !tsecond.Equals(second))
			{
				throw new ArgumentException(this._duplicateFirstErrorMessage.FormatWith(CultureInfo.InvariantCulture, first));
			}
			TFirst tfirst;
			if (this._secondToFirst.TryGetValue(second, out tfirst) && !tfirst.Equals(first))
			{
				throw new ArgumentException(this._duplicateSecondErrorMessage.FormatWith(CultureInfo.InvariantCulture, second));
			}
			this._firstToSecond.Add(first, second);
			this._secondToFirst.Add(second, first);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00010A15 File Offset: 0x0000EC15
		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return this._firstToSecond.TryGetValue(first, out second);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00010A24 File Offset: 0x0000EC24
		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return this._secondToFirst.TryGetValue(second, out first);
		}

		// Token: 0x0400014F RID: 335
		private readonly IDictionary<TFirst, TSecond> _firstToSecond;

		// Token: 0x04000150 RID: 336
		private readonly IDictionary<TSecond, TFirst> _secondToFirst;

		// Token: 0x04000151 RID: 337
		private readonly string _duplicateFirstErrorMessage;

		// Token: 0x04000152 RID: 338
		private readonly string _duplicateSecondErrorMessage;
	}
}
