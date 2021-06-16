using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000073 RID: 115
	public static class VirtualMachineHelper
	{
		// Token: 0x060002BC RID: 700 RVA: 0x0000771C File Offset: 0x0000591C
		public static void DeleteVirtualMachine(string machineName)
		{
			IXdeVirtualServices xdeVirtualServices = (IXdeVirtualServices)Activator.CreateInstance("Microsoft.Xde.Wmi", "Microsoft.Xde.Wmi.XdeWmiVirtualServices").Unwrap();
			IXdeVirtualServices xdeVirtualServices2 = (IXdeVirtualServices)Activator.CreateInstance("Microsoft.Xde.Hcs", "Microsoft.Xde.Hcs.XdeHcsVirtualServices").Unwrap();
			IXdeVirtualServices[] array = new IXdeVirtualServices[]
			{
				xdeVirtualServices,
				xdeVirtualServices2
			};
			for (int i = 0; i < array.Length; i++)
			{
				IXdeVirtualMachine virtualMachine = array[i].GetVirtualMachine(machineName);
				if (virtualMachine != null)
				{
					virtualMachine.DeleteVirtualMachine();
				}
			}
		}
	}
}
