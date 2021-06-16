using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace System.Management.Automation
{
	// Token: 0x0200005A RID: 90
	internal class CommandPathSearch : IEnumerable<string>, IEnumerable, IEnumerator<string>, IDisposable, IEnumerator
	{
		// Token: 0x060004E2 RID: 1250 RVA: 0x00016AA1 File Offset: 0x00014CA1
		internal CommandPathSearch(IEnumerable<string> patterns, IEnumerable<string> lookupPaths, ExecutionContext context) : this(patterns, lookupPaths, null, true, context)
		{
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00016AB0 File Offset: 0x00014CB0
		internal CommandPathSearch(IEnumerable<string> patterns, IEnumerable<string> lookupPaths, HashSet<string> allowedExtensions, bool allowAnyExtension, ExecutionContext context)
		{
			if (patterns == null)
			{
				throw PSTraceSource.NewArgumentNullException("patterns");
			}
			if (lookupPaths == null)
			{
				throw PSTraceSource.NewArgumentNullException("lookupPaths");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			this._context = context;
			this.patterns = patterns;
			this.lookupPaths = new LookupPathCollection(lookupPaths);
			this.ResolveCurrentDirectoryInLookupPaths();
			this.Reset();
			this.allowAnyExtension = allowAnyExtension;
			this.allowedExtensions = allowedExtensions;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00016B24 File Offset: 0x00014D24
		private void ResolveCurrentDirectoryInLookupPaths()
		{
			SortedDictionary<int, int> sortedDictionary = new SortedDictionary<int, int>();
			int num = 0;
			string fileSystem = this._context.ProviderNames.FileSystem;
			SessionStateInternal engineSessionState = this._context.EngineSessionState;
			bool flag = engineSessionState.CurrentDrive != null && engineSessionState.CurrentDrive.Provider.NameEquals(fileSystem) && engineSessionState.IsProviderLoaded(fileSystem);
			string currentDirectory = Directory.GetCurrentDirectory();
			LocationGlobber locationGlobber = this._context.LocationGlobber;
			foreach (int num2 in this.lookupPaths.IndexOfRelativePath())
			{
				string text = null;
				string text2 = null;
				CommandDiscovery.discoveryTracer.WriteLine("Lookup directory \"{0}\" appears to be a relative path. Attempting resolution...", new object[]
				{
					this.lookupPaths[num2]
				});
				if (flag)
				{
					ProviderInfo providerInfo = null;
					try
					{
						text2 = locationGlobber.GetProviderPath(this.lookupPaths[num2], out providerInfo);
					}
					catch (ProviderInvocationException ex)
					{
						CommandDiscovery.discoveryTracer.WriteLine("The relative path '{0}', could not be resolved because the provider threw an exception: '{1}'", new object[]
						{
							this.lookupPaths[num2],
							ex.Message
						});
					}
					catch (InvalidOperationException)
					{
						CommandDiscovery.discoveryTracer.WriteLine("The relative path '{0}', could not resolve a home directory for the provider", new object[]
						{
							this.lookupPaths[num2]
						});
					}
					if (!string.IsNullOrEmpty(text2))
					{
						CommandDiscovery.discoveryTracer.TraceError("The relative path resolved to: {0}", new object[]
						{
							text2
						});
						text = text2;
					}
					else
					{
						CommandDiscovery.discoveryTracer.WriteLine("The relative path was not a file system path. {0}", new object[]
						{
							this.lookupPaths[num2]
						});
					}
				}
				else
				{
					CommandDiscovery.discoveryTracer.TraceWarning("The current drive is not set, using the process current directory: {0}", new object[]
					{
						currentDirectory
					});
					text = currentDirectory;
				}
				if (text != null)
				{
					int num3 = this.lookupPaths.IndexOf(text);
					if (num3 != -1)
					{
						if (num3 > num2)
						{
							sortedDictionary.Add(num++, num3);
							this.lookupPaths[num2] = text;
						}
						else
						{
							sortedDictionary.Add(num++, num2);
						}
					}
					else
					{
						this.lookupPaths[num2] = text;
					}
				}
				else
				{
					sortedDictionary.Add(num++, num2);
				}
			}
			for (int i = sortedDictionary.Count; i > 0; i--)
			{
				int index = sortedDictionary[i - 1];
				this.lookupPaths.RemoveAt(index);
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00016DEC File Offset: 0x00014FEC
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00016DEF File Offset: 0x00014FEF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00016DF4 File Offset: 0x00014FF4
		public bool MoveNext()
		{
			bool flag = false;
			if (this.justReset)
			{
				this.justReset = false;
				if (!this.patternEnumerator.MoveNext())
				{
					CommandPathSearch.tracer.TraceError("No patterns were specified", new object[0]);
					return false;
				}
				if (!this.lookupPathsEnumerator.MoveNext())
				{
					CommandPathSearch.tracer.TraceError("No lookup paths were specified", new object[0]);
					return false;
				}
				this.GetNewDirectoryResults(this.patternEnumerator.Current, this.lookupPathsEnumerator.Current);
			}
			for (;;)
			{
				if (!this.currentDirectoryResultsEnumerator.MoveNext())
				{
					CommandPathSearch.tracer.WriteLine("Current directory results are invalid", new object[0]);
					if (!this.patternEnumerator.MoveNext())
					{
						CommandPathSearch.tracer.WriteLine("Current patterns exhausted in current directory: {0}", new object[]
						{
							this.lookupPathsEnumerator.Current
						});
					}
					else
					{
						this.GetNewDirectoryResults(this.patternEnumerator.Current, this.lookupPathsEnumerator.Current);
						if (!flag)
						{
							continue;
						}
					}
				}
				else
				{
					CommandPathSearch.tracer.WriteLine("Next path found: {0}", new object[]
					{
						this.currentDirectoryResultsEnumerator.Current
					});
					flag = true;
				}
				if (flag)
				{
					goto IL_196;
				}
				if (!this.lookupPathsEnumerator.MoveNext())
				{
					break;
				}
				this.patternEnumerator = this.patterns.GetEnumerator();
				if (!this.patternEnumerator.MoveNext())
				{
					goto Block_8;
				}
				this.GetNewDirectoryResults(this.patternEnumerator.Current, this.lookupPathsEnumerator.Current);
				if (flag)
				{
					goto IL_196;
				}
			}
			CommandPathSearch.tracer.WriteLine("All lookup paths exhausted, no more matches can be found", new object[0]);
			goto IL_196;
			Block_8:
			CommandPathSearch.tracer.WriteLine("All patterns exhausted, no more matches can be found", new object[0]);
			IL_196:
			CommandPathSearch.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00016FB8 File Offset: 0x000151B8
		public void Reset()
		{
			this.lookupPathsEnumerator = this.lookupPaths.GetEnumerator();
			this.patternEnumerator = this.patterns.GetEnumerator();
			this.currentDirectoryResults = new Collection<string>();
			this.currentDirectoryResultsEnumerator = this.currentDirectoryResults.GetEnumerator();
			this.justReset = true;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0001700A File Offset: 0x0001520A
		string IEnumerator<string>.Current
		{
			get
			{
				if (this.currentDirectoryResults == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				return this.currentDirectoryResultsEnumerator.Current;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00017025 File Offset: 0x00015225
		object IEnumerator.Current
		{
			get
			{
				return ((IEnumerator<string>)this).Current;
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001702D File Offset: 0x0001522D
		public void Dispose()
		{
			this.Reset();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001703B File Offset: 0x0001523B
		private void GetNewDirectoryResults(string pattern, string directory)
		{
			this.currentDirectoryResults = CommandPathSearch.GetMatchingPathsInDirectory(pattern, directory, this.allowAnyExtension, this.allowedExtensions);
			this.currentDirectoryResultsEnumerator = this.currentDirectoryResults.GetEnumerator();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00017068 File Offset: 0x00015268
		private static Collection<string> GetMatchingPathsInDirectory(string pattern, string directory, bool allowAnyExtension, HashSet<string> allowedExtensions)
		{
			Collection<string> result = new Collection<string>();
			try
			{
				CommandDiscovery.discoveryTracer.WriteLine("Looking for {0} in {1}", new object[]
				{
					pattern,
					directory
				});
				if (Directory.Exists(directory))
				{
					IEnumerable<string> enumerable = null;
					if (!".".Equals(pattern, StringComparison.OrdinalIgnoreCase))
					{
						enumerable = Directory.EnumerateFiles(directory, pattern);
					}
					List<string> list = null;
					if (enumerable != null)
					{
						list = new List<string>();
						if (allowAnyExtension)
						{
							list.AddRange(enumerable);
						}
						else
						{
							foreach (string text in enumerable)
							{
								string extension = Path.GetExtension(text);
								if (!string.IsNullOrEmpty(extension) && allowedExtensions.Contains(extension))
								{
									list.Add(text);
								}
							}
						}
					}
					result = SessionStateUtilities.ConvertListToCollection<string>(list);
				}
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (NotSupportedException)
			{
			}
			return result;
		}

		// Token: 0x040001D4 RID: 468
		[TraceSource("CommandSearch", "CommandSearch")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("CommandSearch", "CommandSearch");

		// Token: 0x040001D5 RID: 469
		private LookupPathCollection lookupPaths;

		// Token: 0x040001D6 RID: 470
		private IEnumerator<string> lookupPathsEnumerator;

		// Token: 0x040001D7 RID: 471
		private Collection<string> currentDirectoryResults;

		// Token: 0x040001D8 RID: 472
		private IEnumerator<string> currentDirectoryResultsEnumerator;

		// Token: 0x040001D9 RID: 473
		private IEnumerable<string> patterns;

		// Token: 0x040001DA RID: 474
		private IEnumerator<string> patternEnumerator;

		// Token: 0x040001DB RID: 475
		private ExecutionContext _context;

		// Token: 0x040001DC RID: 476
		private bool justReset;

		// Token: 0x040001DD RID: 477
		private bool allowAnyExtension;

		// Token: 0x040001DE RID: 478
		private HashSet<string> allowedExtensions;
	}
}
