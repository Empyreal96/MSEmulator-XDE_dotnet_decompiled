using System;

namespace System.Management.Automation
{
	// Token: 0x02000118 RID: 280
	internal enum ConversionRank
	{
		// Token: 0x04000685 RID: 1669
		None,
		// Token: 0x04000686 RID: 1670
		UnrelatedArraysS2A = 7,
		// Token: 0x04000687 RID: 1671
		UnrelatedArrays = 15,
		// Token: 0x04000688 RID: 1672
		ToStringS2A = 23,
		// Token: 0x04000689 RID: 1673
		ToString = 31,
		// Token: 0x0400068A RID: 1674
		CustomS2A = 39,
		// Token: 0x0400068B RID: 1675
		Custom = 47,
		// Token: 0x0400068C RID: 1676
		IConvertibleS2A = 55,
		// Token: 0x0400068D RID: 1677
		IConvertible = 63,
		// Token: 0x0400068E RID: 1678
		ImplicitCastS2A = 71,
		// Token: 0x0400068F RID: 1679
		ImplicitCast = 79,
		// Token: 0x04000690 RID: 1680
		ExplicitCastS2A = 87,
		// Token: 0x04000691 RID: 1681
		ExplicitCast = 95,
		// Token: 0x04000692 RID: 1682
		ConstructorS2A = 103,
		// Token: 0x04000693 RID: 1683
		Constructor = 111,
		// Token: 0x04000694 RID: 1684
		Create = 115,
		// Token: 0x04000695 RID: 1685
		ParseS2A = 119,
		// Token: 0x04000696 RID: 1686
		Parse = 127,
		// Token: 0x04000697 RID: 1687
		PSObjectS2A = 135,
		// Token: 0x04000698 RID: 1688
		PSObject = 143,
		// Token: 0x04000699 RID: 1689
		LanguageS2A = 151,
		// Token: 0x0400069A RID: 1690
		Language = 159,
		// Token: 0x0400069B RID: 1691
		NullToValue = 175,
		// Token: 0x0400069C RID: 1692
		NullToRef = 191,
		// Token: 0x0400069D RID: 1693
		NumericExplicitS2A = 199,
		// Token: 0x0400069E RID: 1694
		NumericExplicit = 207,
		// Token: 0x0400069F RID: 1695
		NumericExplicit1S2A = 215,
		// Token: 0x040006A0 RID: 1696
		NumericExplicit1 = 223,
		// Token: 0x040006A1 RID: 1697
		NumericStringS2A = 231,
		// Token: 0x040006A2 RID: 1698
		NumericString = 239,
		// Token: 0x040006A3 RID: 1699
		NumericImplicitS2A = 247,
		// Token: 0x040006A4 RID: 1700
		NumericImplicit = 255,
		// Token: 0x040006A5 RID: 1701
		AssignableS2A = 263,
		// Token: 0x040006A6 RID: 1702
		Assignable = 271,
		// Token: 0x040006A7 RID: 1703
		IdentityS2A = 279,
		// Token: 0x040006A8 RID: 1704
		StringToCharArray = 282,
		// Token: 0x040006A9 RID: 1705
		Identity = 287,
		// Token: 0x040006AA RID: 1706
		ValueDependent = 65527
	}
}
