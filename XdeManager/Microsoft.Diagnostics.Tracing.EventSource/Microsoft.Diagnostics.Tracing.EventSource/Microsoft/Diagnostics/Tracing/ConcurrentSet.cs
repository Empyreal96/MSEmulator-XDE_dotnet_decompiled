using System;
using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000019 RID: 25
	internal struct ConcurrentSet<KeyType, ItemType> where ItemType : ConcurrentSetItem<KeyType, ItemType>
	{
		// Token: 0x06000102 RID: 258 RVA: 0x0000970C File Offset: 0x0000790C
		public ItemType TryGet(KeyType key)
		{
			ItemType[] array = this.items;
			checked
			{
				ItemType result;
				if (array != null)
				{
					int num = 0;
					int num2 = array.Length;
					do
					{
						int num3 = (num + num2) / 2;
						result = array[num3];
						int num4 = result.Compare(key);
						if (num4 == 0)
						{
							return result;
						}
						if (num4 < 0)
						{
							num = num3 + 1;
						}
						else
						{
							num2 = num3;
						}
					}
					while (num != num2);
				}
				result = default(ItemType);
				return result;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000976C File Offset: 0x0000796C
		public ItemType GetOrAdd(ItemType newItem)
		{
			ItemType[] array = this.items;
			checked
			{
				ItemType result;
				for (;;)
				{
					ItemType[] array2;
					if (array == null)
					{
						array2 = new ItemType[]
						{
							newItem
						};
					}
					else
					{
						int num = 0;
						int num2 = array.Length;
						do
						{
							int num3 = (num + num2) / 2;
							result = array[num3];
							int num4 = result.Compare(newItem);
							if (num4 == 0)
							{
								return result;
							}
							if (num4 < 0)
							{
								num = num3 + 1;
							}
							else
							{
								num2 = num3;
							}
						}
						while (num != num2);
						int num5 = array.Length;
						array2 = new ItemType[num5 + 1];
						Array.Copy(array, 0, array2, 0, num);
						array2[num] = newItem;
						Array.Copy(array, num, array2, num + 1, num5 - num);
					}
					array2 = Interlocked.CompareExchange<ItemType[]>(ref this.items, array2, array);
					if (array == array2)
					{
						break;
					}
					array = array2;
				}
				result = newItem;
				return result;
			}
		}

		// Token: 0x04000088 RID: 136
		private ItemType[] items;
	}
}
