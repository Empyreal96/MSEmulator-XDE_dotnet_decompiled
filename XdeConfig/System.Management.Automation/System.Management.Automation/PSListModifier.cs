using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000210 RID: 528
	public class PSListModifier
	{
		// Token: 0x060018C0 RID: 6336 RVA: 0x00096A80 File Offset: 0x00094C80
		public PSListModifier()
		{
			this._itemsToAdd = new Collection<object>();
			this._itemsToRemove = new Collection<object>();
			this._replacementItems = new Collection<object>();
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x00096AA9 File Offset: 0x00094CA9
		public PSListModifier(Collection<object> removeItems, Collection<object> addItems)
		{
			this._itemsToAdd = ((addItems != null) ? addItems : new Collection<object>());
			this._itemsToRemove = ((removeItems != null) ? removeItems : new Collection<object>());
			this._replacementItems = new Collection<object>();
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x00096AE0 File Offset: 0x00094CE0
		public PSListModifier(object replacementItems)
		{
			this._itemsToAdd = new Collection<object>();
			this._itemsToRemove = new Collection<object>();
			if (replacementItems == null)
			{
				this._replacementItems = new Collection<object>();
				return;
			}
			if (replacementItems is Collection<object>)
			{
				this._replacementItems = (Collection<object>)replacementItems;
				return;
			}
			if (replacementItems is IList<object>)
			{
				this._replacementItems = new Collection<object>((IList<object>)replacementItems);
				return;
			}
			if (replacementItems is IList)
			{
				this._replacementItems = new Collection<object>();
				using (IEnumerator enumerator = ((IList)replacementItems).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object item = enumerator.Current;
						this._replacementItems.Add(item);
					}
					return;
				}
			}
			this._replacementItems = new Collection<object>();
			this._replacementItems.Add(replacementItems);
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x00096BC0 File Offset: 0x00094DC0
		public PSListModifier(Hashtable hash)
		{
			if (hash == null)
			{
				throw PSTraceSource.NewArgumentNullException("hash");
			}
			this._itemsToAdd = new Collection<object>();
			this._itemsToRemove = new Collection<object>();
			this._replacementItems = new Collection<object>();
			foreach (object obj in hash)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (!(dictionaryEntry.Key is string))
				{
					throw PSTraceSource.NewArgumentException("hash", PSListModifierStrings.ListModifierDisallowedKey, new object[]
					{
						dictionaryEntry.Key
					});
				}
				string text = dictionaryEntry.Key as string;
				bool flag = text.Equals("Add", StringComparison.OrdinalIgnoreCase);
				bool flag2 = text.Equals("Remove", StringComparison.OrdinalIgnoreCase);
				bool flag3 = text.Equals("Replace", StringComparison.OrdinalIgnoreCase);
				if (!flag && !flag2 && !flag3)
				{
					throw PSTraceSource.NewArgumentException("hash", PSListModifierStrings.ListModifierDisallowedKey, new object[]
					{
						text
					});
				}
				Collection<object> collection;
				if (flag2)
				{
					collection = this._itemsToRemove;
				}
				else if (flag)
				{
					collection = this._itemsToAdd;
				}
				else
				{
					collection = this._replacementItems;
				}
				IEnumerable enumerable = LanguagePrimitives.GetEnumerable(dictionaryEntry.Value);
				if (enumerable != null)
				{
					using (IEnumerator enumerator2 = enumerable.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object item = enumerator2.Current;
							collection.Add(item);
						}
						continue;
					}
				}
				collection.Add(dictionaryEntry.Value);
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x00096D88 File Offset: 0x00094F88
		public Collection<object> Add
		{
			get
			{
				return this._itemsToAdd;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x00096D90 File Offset: 0x00094F90
		public Collection<object> Remove
		{
			get
			{
				return this._itemsToRemove;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x00096D98 File Offset: 0x00094F98
		public Collection<object> Replace
		{
			get
			{
				return this._replacementItems;
			}
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x00096DA0 File Offset: 0x00094FA0
		public void ApplyTo(IList collectionToUpdate)
		{
			if (collectionToUpdate == null)
			{
				throw PSTraceSource.NewArgumentNullException("collectionToUpdate");
			}
			if (this._replacementItems.Count > 0)
			{
				collectionToUpdate.Clear();
				using (IEnumerator<object> enumerator = this._replacementItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						collectionToUpdate.Add(PSObject.Base(obj));
					}
					return;
				}
			}
			foreach (object obj2 in this._itemsToRemove)
			{
				collectionToUpdate.Remove(PSObject.Base(obj2));
			}
			foreach (object obj3 in this._itemsToAdd)
			{
				collectionToUpdate.Add(PSObject.Base(obj3));
			}
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x00096EA4 File Offset: 0x000950A4
		public void ApplyTo(object collectionToUpdate)
		{
			if (collectionToUpdate == null)
			{
				throw new ArgumentNullException("collectionToUpdate");
			}
			collectionToUpdate = PSObject.Base(collectionToUpdate);
			IList list = collectionToUpdate as IList;
			if (list == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PSListModifierStrings.UpdateFailed, new object[0]);
			}
			this.ApplyTo(list);
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x00096EEC File Offset: 0x000950EC
		internal Hashtable ToHashtable()
		{
			Hashtable hashtable = new Hashtable(2);
			if (this._itemsToAdd.Count > 0)
			{
				hashtable.Add("Add", this._itemsToAdd);
			}
			if (this._itemsToRemove.Count > 0)
			{
				hashtable.Add("Remove", this._itemsToRemove);
			}
			if (this._replacementItems.Count > 0)
			{
				hashtable.Add("Replace", this._replacementItems);
			}
			return hashtable;
		}

		// Token: 0x04000A3B RID: 2619
		internal const string AddKey = "Add";

		// Token: 0x04000A3C RID: 2620
		internal const string RemoveKey = "Remove";

		// Token: 0x04000A3D RID: 2621
		internal const string ReplaceKey = "Replace";

		// Token: 0x04000A3E RID: 2622
		private Collection<object> _itemsToAdd;

		// Token: 0x04000A3F RID: 2623
		private Collection<object> _itemsToRemove;

		// Token: 0x04000A40 RID: 2624
		private Collection<object> _replacementItems;
	}
}
