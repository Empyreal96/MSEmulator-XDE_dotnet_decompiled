using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004A4 RID: 1188
	internal static class AssociationManager
	{
		// Token: 0x0600350B RID: 13579 RVA: 0x0011FD68 File Offset: 0x0011DF68
		internal static List<MshResolvedExpressionParameterAssociation> SetupActiveProperties(List<MshParameter> rawMshParameterList, PSObject target, MshExpressionFactory expressionFactory)
		{
			if (rawMshParameterList != null && rawMshParameterList.Count > 0)
			{
				return AssociationManager.ExpandParameters(rawMshParameterList, target);
			}
			List<MshResolvedExpressionParameterAssociation> list = AssociationManager.ExpandDefaultPropertySet(target, expressionFactory);
			if (list.Count > 0)
			{
				if (PSObjectHelper.ShouldShowComputerNameProperty(target))
				{
					list.Add(new MshResolvedExpressionParameterAssociation(null, new MshExpression(RemotingConstants.ComputerNameNoteProperty)));
				}
				return list;
			}
			list = AssociationManager.ExpandAll(target);
			AssociationManager.HandleComputerNameProperties(target, list);
			return list;
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x0011FDCC File Offset: 0x0011DFCC
		internal static List<MshResolvedExpressionParameterAssociation> ExpandTableParameters(List<MshParameter> parameters, PSObject target)
		{
			List<MshResolvedExpressionParameterAssociation> list = new List<MshResolvedExpressionParameterAssociation>();
			foreach (MshParameter mshParameter in parameters)
			{
				MshExpression mshExpression = mshParameter.GetEntry("expression") as MshExpression;
				List<MshExpression> list2 = mshExpression.ResolveNames(target);
				if (!mshExpression.HasWildCardCharacters && list2.Count == 0)
				{
					list.Add(new MshResolvedExpressionParameterAssociation(mshParameter, mshExpression));
				}
				foreach (MshExpression expression in list2)
				{
					list.Add(new MshResolvedExpressionParameterAssociation(mshParameter, expression));
				}
			}
			return list;
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x0011FE9C File Offset: 0x0011E09C
		internal static List<MshResolvedExpressionParameterAssociation> ExpandParameters(List<MshParameter> parameters, PSObject target)
		{
			List<MshResolvedExpressionParameterAssociation> list = new List<MshResolvedExpressionParameterAssociation>();
			foreach (MshParameter mshParameter in parameters)
			{
				MshExpression mshExpression = mshParameter.GetEntry("expression") as MshExpression;
				List<MshExpression> list2 = mshExpression.ResolveNames(target);
				foreach (MshExpression expression in list2)
				{
					list.Add(new MshResolvedExpressionParameterAssociation(mshParameter, expression));
				}
			}
			return list;
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x0011FF4C File Offset: 0x0011E14C
		internal static List<MshResolvedExpressionParameterAssociation> ExpandDefaultPropertySet(PSObject target, MshExpressionFactory expressionFactory)
		{
			List<MshResolvedExpressionParameterAssociation> list = new List<MshResolvedExpressionParameterAssociation>();
			List<MshExpression> defaultPropertySet = PSObjectHelper.GetDefaultPropertySet(target);
			foreach (MshExpression expression in defaultPropertySet)
			{
				list.Add(new MshResolvedExpressionParameterAssociation(null, expression));
			}
			return list;
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x0011FFB0 File Offset: 0x0011E1B0
		private static List<string> GetPropertyNamesFromView(PSObject source, PSMemberViewTypes viewType)
		{
			Collection<CollectionEntry<PSMemberInfo>> memberCollection = PSObject.GetMemberCollection(viewType);
			PSMemberInfoIntegratingCollection<PSMemberInfo> psmemberInfoIntegratingCollection = new PSMemberInfoIntegratingCollection<PSMemberInfo>(source, memberCollection);
			ReadOnlyPSMemberInfoCollection<PSMemberInfo> readOnlyPSMemberInfoCollection = psmemberInfoIntegratingCollection.Match("*", PSMemberTypes.Properties);
			List<string> list = new List<string>();
			foreach (PSMemberInfo psmemberInfo in readOnlyPSMemberInfoCollection)
			{
				list.Add(psmemberInfo.Name);
			}
			return list;
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x00120028 File Offset: 0x0011E228
		internal static List<MshResolvedExpressionParameterAssociation> ExpandAll(PSObject target)
		{
			List<string> propertyNamesFromView = AssociationManager.GetPropertyNamesFromView(target, PSMemberViewTypes.Adapted);
			List<string> propertyNamesFromView2 = AssociationManager.GetPropertyNamesFromView(target, PSMemberViewTypes.Base);
			List<string> propertyNamesFromView3 = AssociationManager.GetPropertyNamesFromView(target, PSMemberViewTypes.Extended);
			List<string> list = new List<string>();
			if (propertyNamesFromView.Count != 0)
			{
				list = propertyNamesFromView;
			}
			else
			{
				list = propertyNamesFromView2;
			}
			list.AddRange(propertyNamesFromView3);
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			List<MshResolvedExpressionParameterAssociation> list2 = new List<MshResolvedExpressionParameterAssociation>();
			foreach (string text in list)
			{
				if (!dictionary.ContainsKey(text))
				{
					dictionary.Add(text, null);
					MshExpression expression = new MshExpression(text, true);
					list2.Add(new MshResolvedExpressionParameterAssociation(null, expression));
				}
			}
			return list2;
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x001200E8 File Offset: 0x0011E2E8
		internal static void HandleComputerNameProperties(PSObject so, List<MshResolvedExpressionParameterAssociation> activeAssociationList)
		{
			if (so.Properties[RemotingConstants.ShowComputerNameNoteProperty] != null)
			{
				Collection<MshResolvedExpressionParameterAssociation> collection = new Collection<MshResolvedExpressionParameterAssociation>();
				foreach (MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation in activeAssociationList)
				{
					if (mshResolvedExpressionParameterAssociation.ResolvedExpression.ToString().Equals(RemotingConstants.ShowComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase))
					{
						collection.Add(mshResolvedExpressionParameterAssociation);
						break;
					}
				}
				if (so.Properties[RemotingConstants.ComputerNameNoteProperty] != null && !PSObjectHelper.ShouldShowComputerNameProperty(so))
				{
					foreach (MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation2 in activeAssociationList)
					{
						if (mshResolvedExpressionParameterAssociation2.ResolvedExpression.ToString().Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase))
						{
							collection.Add(mshResolvedExpressionParameterAssociation2);
							break;
						}
					}
				}
				if (collection.Count > 0)
				{
					foreach (MshResolvedExpressionParameterAssociation item in collection)
					{
						activeAssociationList.Remove(item);
					}
				}
			}
		}
	}
}
