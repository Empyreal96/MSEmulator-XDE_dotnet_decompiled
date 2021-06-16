using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xde.Base.Properties;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Base
{
	// Token: 0x02000007 RID: 7
	[Export(typeof(IXdeSku))]
	public class Sku : IXdeSku, INotifyPropertyChanged, IDisposable
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00003C0C File Offset: 0x00001E0C
		private Sku(SkuData data, string fileName, params object[] addtionalCompositionObjects)
		{
			this.SkuDirectory = Path.GetDirectoryName(fileName);
			AggregateCatalog aggregateCatalog = new AggregateCatalog();
			Sku.SkuFilteredCatalog catalog = new Sku.SkuFilteredCatalog(data, aggregateCatalog);
			AssemblyCatalog item = new AssemblyCatalog(Assembly.GetExecutingAssembly());
			aggregateCatalog.Catalogs.Add(item);
			string directoryName = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
			string path = Path.Combine(directoryName, "plugins");
			if (Directory.Exists(path))
			{
				DirectoryCatalog item2 = new DirectoryCatalog(path);
				aggregateCatalog.Catalogs.Add(item2);
			}
			string path2 = Path.Combine(this.SkuDirectory, "..\\..\\plugins");
			if (Directory.Exists(path2))
			{
				DirectoryCatalog item3 = new DirectoryCatalog(path2);
				aggregateCatalog.Catalogs.Add(item3);
			}
			DirectoryCatalog item4 = new DirectoryCatalog(directoryName, "*plugin.dll");
			aggregateCatalog.Catalogs.Add(item4);
			List<object> list = new List<object>();
			list.Add(this);
			foreach (object item5 in addtionalCompositionObjects)
			{
				if (list.IndexOf(item5) == -1)
				{
					list.Add(item5);
				}
			}
			object[] attributedParts = list.ToArray();
			this.container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection, Array.Empty<ExportProvider>());
			this.container.ComposeParts(attributedParts);
			this.LoadOptions(data);
			this.LoadBranding(data);
			this.LoadFeatures(data);
			this.LoadTabs(data);
			this.LoadToolbarItems(data);
			this.LoadKeys(data);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600006C RID: 108 RVA: 0x00003DA0 File Offset: 0x00001FA0
		// (remove) Token: 0x0600006D RID: 109 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003E0D File Offset: 0x0000200D
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003E15 File Offset: 0x00002015
		[ImportMany]
		public IEnumerable<Lazy<IXdeFeature, Sku.IPluginMetadata>> PotentialFeatures { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003E1E File Offset: 0x0000201E
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003E26 File Offset: 0x00002026
		[ImportMany]
		public IEnumerable<Lazy<IXdeTab, Sku.IPluginMetadata>> PotentialTabs { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003E2F File Offset: 0x0000202F
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00003E37 File Offset: 0x00002037
		[ImportMany]
		public IEnumerable<Lazy<IXdeButton, Sku.IPluginMetadata>> PotentialButtons { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003E40 File Offset: 0x00002040
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003E48 File Offset: 0x00002048
		[ImportMany]
		public IEnumerable<Lazy<IXdeToolbarItem, Sku.IPluginMetadata>> PotentialToolbarItems { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003E51 File Offset: 0x00002051
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00003E59 File Offset: 0x00002059
		[ImportMany]
		public IEnumerable<Lazy<IXdeGuestDisplay, Sku.IPluginMetadata>> PotentialGuestDisplays { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003E62 File Offset: 0x00002062
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00003E6A File Offset: 0x0000206A
		[ImportMany]
		public IEnumerable<Lazy<IXdeSkuBranding, Sku.IPluginMetadata>> PotentialBranding { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003E73 File Offset: 0x00002073
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003E7B File Offset: 0x0000207B
		public string SkuDirectory { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003E84 File Offset: 0x00002084
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003E8C File Offset: 0x0000208C
		public IXdeSkuBranding Branding { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003E95 File Offset: 0x00002095
		IEnumerable<IXdeFeature> IXdeSku.Features
		{
			get
			{
				return this.features;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003E9D File Offset: 0x0000209D
		public IEnumerable<IXdeConnectionController> ConnectionControllers
		{
			get
			{
				foreach (IXdeFeature feature in this.features)
				{
					if (feature.Connection != null)
					{
						yield return feature.Connection;
					}
					IXdeFeature2 xdeFeature = feature as IXdeFeature2;
					if (xdeFeature != null && xdeFeature.Connections != null)
					{
						foreach (IXdeConnectionController xdeConnectionController in xdeFeature.Connections)
						{
							yield return xdeConnectionController;
						}
						IEnumerator<IXdeConnectionController> enumerator2 = null;
					}
					feature = null;
				}
				List<IXdeFeature>.Enumerator enumerator = default(List<IXdeFeature>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003EAD File Offset: 0x000020AD
		IEnumerable<IXdeTab> IXdeSku.Tabs
		{
			get
			{
				return this.tabs;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003EB5 File Offset: 0x000020B5
		IEnumerable<IXdeToolbarItem> IXdeSku.ToolbarItems
		{
			get
			{
				return this.toolbarItems;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003EBD File Offset: 0x000020BD
		IXdeSkuOptions IXdeSku.Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003EC5 File Offset: 0x000020C5
		SkuRegKey[] IXdeSku.Keys
		{
			get
			{
				return this.keys;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003ECD File Offset: 0x000020CD
		public static Sku LoadSkuFromName(string skuFileName, params object[] addtionalCompositionObjects)
		{
			return new Sku(SkuData.LoadSkuInformation(skuFileName), skuFileName, addtionalCompositionObjects);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003EDC File Offset: 0x000020DC
		public void Dispose()
		{
			foreach (IXdeFeature xdeFeature in this.features)
			{
				xdeFeature.Dispose();
			}
			this.features.Clear();
			Sku.SkuOptionsImpl skuOptionsImpl = this.options;
			if (skuOptionsImpl != null)
			{
				skuOptionsImpl.Dispose();
			}
			this.container.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003F5C File Offset: 0x0000215C
		private void LoadFeatures(SkuData data)
		{
			FeatureData[] requiredFeatures = data.RequiredFeatures;
			for (int i = 0; i < requiredFeatures.Length; i++)
			{
				FeatureData featureData = requiredFeatures[i];
				Lazy<IXdeFeature, Sku.IPluginMetadata> lazy = (from t in this.PotentialFeatures
				where t.Metadata.Name == featureData.Name
				select t).FirstOrDefault<Lazy<IXdeFeature, Sku.IPluginMetadata>>();
				if (lazy == null)
				{
					throw new Exception(StringUtilities.CurrentCultureFormat(Strings.SkuLoader_FeatureNotFoundFormat, new object[]
					{
						featureData.Name
					}));
				}
				this.features.Add(lazy.Value);
			}
			this.OnPropertyChanged("Features");
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003FF0 File Offset: 0x000021F0
		private void LoadTabs(SkuData data)
		{
			TabData[] requiredTabs = data.RequiredTabs;
			for (int i = 0; i < requiredTabs.Length; i++)
			{
				TabData tabData = requiredTabs[i];
				Lazy<IXdeTab, Sku.IPluginMetadata> lazy = (from t in this.PotentialTabs
				where t.Metadata.Name == tabData.Name
				select t).FirstOrDefault<Lazy<IXdeTab, Sku.IPluginMetadata>>();
				if (lazy == null)
				{
					throw new Exception(StringUtilities.CurrentCultureFormat(Strings.SkuLoader_TabNotFoundFormat, new object[]
					{
						tabData.Name
					}));
				}
				this.tabs.Add(lazy.Value);
			}
			this.OnPropertyChanged("Tabs");
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004084 File Offset: 0x00002284
		private void LoadToolbarItems(SkuData data)
		{
			ButtonData[] toolbar = data.Toolbar;
			for (int i = 0; i < toolbar.Length; i++)
			{
				ButtonData buttonData = toolbar[i];
				Lazy<IXdeButton, Sku.IPluginMetadata> lazy = (from b in this.PotentialButtons
				where b.Metadata.Name == buttonData.Name
				select b).FirstOrDefault<Lazy<IXdeButton, Sku.IPluginMetadata>>();
				IXdeToolbarItem value;
				if (lazy != null)
				{
					value = lazy.Value;
				}
				else
				{
					Lazy<IXdeToolbarItem, Sku.IPluginMetadata> lazy2 = (from b in this.PotentialToolbarItems
					where b.Metadata.Name == buttonData.Name
					select b).FirstOrDefault<Lazy<IXdeToolbarItem, Sku.IPluginMetadata>>();
					if (lazy2 == null)
					{
						throw new Exception(StringUtilities.CurrentCultureFormat(Strings.SkuLoader_ButtonNotFoundFormat, new object[]
						{
							buttonData.Name
						}));
					}
					value = lazy2.Value;
				}
				this.toolbarItems.Add(value);
			}
			this.OnPropertyChanged("Toolbar");
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004150 File Offset: 0x00002350
		private void LoadBranding(SkuData data)
		{
			Lazy<IXdeSkuBranding, Sku.IPluginMetadata> lazy = (from b in this.PotentialBranding
			where b.Metadata.Name == data.BrandingName
			select b).FirstOrDefault<Lazy<IXdeSkuBranding, Sku.IPluginMetadata>>();
			if (lazy == null)
			{
				throw new Exception(StringUtilities.CurrentCultureFormat(Strings.SkuLoader_BrandingNotFoundFormat, new object[]
				{
					data.BrandingName
				}));
			}
			this.Branding = lazy.Value;
			this.OnPropertyChanged("Branding");
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000041C8 File Offset: 0x000023C8
		private void LoadOptions(SkuData data)
		{
			if (data.Options != null)
			{
				this.options = new Sku.SkuOptionsImpl(data.Options);
			}
			Lazy<IXdeGuestDisplay, Sku.IPluginMetadata> lazy = this.PotentialGuestDisplays.FirstOrDefault((Lazy<IXdeGuestDisplay, Sku.IPluginMetadata> f) => string.Equals(f.Metadata.Name, data.Options.GuestDisplayProvider, StringComparison.Ordinal));
			if (lazy == null)
			{
				throw new Exception(StringUtilities.CurrentCultureFormat(Strings.SkuLoader_GuestDisplayFactoryNotFoundFormat, new object[]
				{
					data.Options.GuestDisplayProvider
				}));
			}
			this.options.GuestDisplay = lazy.Value;
			this.OnPropertyChanged("Options");
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004265 File Offset: 0x00002465
		private void LoadKeys(SkuData data)
		{
			if (data.Registry != null && data.Registry.Length != 0)
			{
				this.keys = data.Registry;
				this.OnPropertyChanged("Keys");
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000428F File Offset: 0x0000248F
		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x04000030 RID: 48
		private List<IXdeFeature> features = new List<IXdeFeature>();

		// Token: 0x04000031 RID: 49
		private List<IXdeTab> tabs = new List<IXdeTab>();

		// Token: 0x04000032 RID: 50
		private List<IXdeToolbarItem> toolbarItems = new List<IXdeToolbarItem>();

		// Token: 0x04000033 RID: 51
		private Sku.SkuOptionsImpl options;

		// Token: 0x04000034 RID: 52
		private SkuRegKey[] keys = new SkuRegKey[0];

		// Token: 0x04000035 RID: 53
		private CompositionContainer container = new CompositionContainer();

		// Token: 0x02000013 RID: 19
		public interface IPluginMetadata
		{
			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000137 RID: 311
			string Name { get; }
		}

		// Token: 0x02000014 RID: 20
		private class SkuFilteredCatalog : ComposablePartCatalog
		{
			// Token: 0x06000138 RID: 312 RVA: 0x000060F0 File Offset: 0x000042F0
			public SkuFilteredCatalog(SkuData data, ComposablePartCatalog sourceCatalog)
			{
				this.data = data;
				this.sourceCatalog = sourceCatalog;
				this.inclusiveFilter = new Func<ComposablePartDefinition, bool>(this.CheckPart);
				foreach (FeatureData featureData in data.RequiredFeatures)
				{
					this.reqFeatures.Add(featureData.Name);
				}
				foreach (TabData tabData in data.RequiredTabs)
				{
					this.reqTabs.Add(tabData.Name);
				}
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000139 RID: 313 RVA: 0x00006194 File Offset: 0x00004394
			public override IQueryable<ComposablePartDefinition> Parts
			{
				get
				{
					return from p in this.sourceCatalog.Parts
					let keepPart = this.inclusiveFilter(p)
					where keepPart == true
					select p;
				}
			}

			// Token: 0x0600013A RID: 314 RVA: 0x00006300 File Offset: 0x00004500
			private bool CheckPart(ComposablePartDefinition part)
			{
				foreach (ExportDefinition exportDefinition in part.ExportDefinitions)
				{
					string contractName = exportDefinition.ContractName;
					if (contractName == "Microsoft.Xde.Common.IXdeFeature" || contractName == "Microsoft.Xde.Common.IXdeFeature2")
					{
						string item = (from m in exportDefinition.Metadata
						where m.Key == "Name"
						select m).First<KeyValuePair<string, object>>().Value.ToString();
						return this.reqFeatures.Contains(item);
					}
					if (contractName == "Microsoft.Xde.Common.IXdeTab")
					{
						string item2 = (from m in exportDefinition.Metadata
						where m.Key == "Name"
						select m).First<KeyValuePair<string, object>>().Value.ToString();
						return this.reqTabs.Contains(item2);
					}
				}
				return true;
			}

			// Token: 0x04000078 RID: 120
			private SkuData data;

			// Token: 0x04000079 RID: 121
			private ComposablePartCatalog sourceCatalog;

			// Token: 0x0400007A RID: 122
			private Func<ComposablePartDefinition, bool> inclusiveFilter;

			// Token: 0x0400007B RID: 123
			private HashSet<string> reqFeatures = new HashSet<string>();

			// Token: 0x0400007C RID: 124
			private HashSet<string> reqTabs = new HashSet<string>();
		}

		// Token: 0x02000015 RID: 21
		private class SkuOptionsImpl : IXdeSkuOptions, IDisposable
		{
			// Token: 0x0600013B RID: 315 RVA: 0x00006424 File Offset: 0x00004624
			public SkuOptionsImpl(SkuOptions options)
			{
				this.options = options;
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x0600013C RID: 316 RVA: 0x00006433 File Offset: 0x00004633
			public bool NATDisabled
			{
				get
				{
					return this.options.NATDisabled;
				}
			}

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x0600013D RID: 317 RVA: 0x00006440 File Offset: 0x00004640
			public bool WindowsKeyEnabled
			{
				get
				{
					return this.options.WindowsKeyEnabled;
				}
			}

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x0600013E RID: 318 RVA: 0x0000644D File Offset: 0x0000464D
			public bool WindowsKeyCombinationsEnabled
			{
				get
				{
					return this.options.WindowsKeyCombinationsEnabled;
				}
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x0600013F RID: 319 RVA: 0x0000645A File Offset: 0x0000465A
			public int DefaultMemSize
			{
				get
				{
					return this.options.DefaultMemSize;
				}
			}

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x06000140 RID: 320 RVA: 0x00006467 File Offset: 0x00004667
			public int ProcessorCount
			{
				get
				{
					return this.options.ProcessorCount;
				}
			}

			// Token: 0x17000063 RID: 99
			// (get) Token: 0x06000141 RID: 321 RVA: 0x00006474 File Offset: 0x00004674
			public bool WriteVhdBootSettingsDisabled
			{
				get
				{
					return this.options.WriteVhdBootSettingsDisabled;
				}
			}

			// Token: 0x17000064 RID: 100
			// (get) Token: 0x06000142 RID: 322 RVA: 0x00006481 File Offset: 0x00004681
			public string ValidSensors
			{
				get
				{
					return this.options.ValidSensors;
				}
			}

			// Token: 0x17000065 RID: 101
			// (get) Token: 0x06000143 RID: 323 RVA: 0x0000648E File Offset: 0x0000468E
			// (set) Token: 0x06000144 RID: 324 RVA: 0x00006496 File Offset: 0x00004696
			public IXdeGuestDisplay GuestDisplay { get; set; }

			// Token: 0x17000066 RID: 102
			// (get) Token: 0x06000145 RID: 325 RVA: 0x0000649F File Offset: 0x0000469F
			public bool HostCursorDisabledInMouseMode
			{
				get
				{
					return this.options.HostCursorDisabledInMouseMode;
				}
			}

			// Token: 0x17000067 RID: 103
			// (get) Token: 0x06000146 RID: 326 RVA: 0x000064AC File Offset: 0x000046AC
			public TouchMode InputMode
			{
				get
				{
					return this.options.InputMode;
				}
			}

			// Token: 0x17000068 RID: 104
			// (get) Token: 0x06000147 RID: 327 RVA: 0x000064B9 File Offset: 0x000046B9
			public bool UseHCSIfAvailable
			{
				get
				{
					return this.options.UseHCSIfAvailable;
				}
			}

			// Token: 0x17000069 RID: 105
			// (get) Token: 0x06000148 RID: 328 RVA: 0x000064C6 File Offset: 0x000046C6
			public bool ShowGuestDisplayASAP
			{
				get
				{
					return this.options.ShowGuestDisplayASAP;
				}
			}

			// Token: 0x1700006A RID: 106
			// (get) Token: 0x06000149 RID: 329 RVA: 0x000064D3 File Offset: 0x000046D3
			public GpuAssignmentMode GpuAssignmentMode
			{
				get
				{
					return this.options.GpuAssignmentMode;
				}
			}

			// Token: 0x0600014A RID: 330 RVA: 0x000064E0 File Offset: 0x000046E0
			public void Dispose()
			{
				IXdeGuestDisplay guestDisplay = this.GuestDisplay;
				if (guestDisplay == null)
				{
					return;
				}
				guestDisplay.Dispose();
			}

			// Token: 0x0400007D RID: 125
			private SkuOptions options;
		}
	}
}
