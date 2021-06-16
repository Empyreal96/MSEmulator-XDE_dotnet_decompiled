using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation.Provider
{
	// Token: 0x0200046E RID: 1134
	public abstract class NavigationCmdletProvider : ContainerCmdletProvider
	{
		// Token: 0x06003283 RID: 12931 RVA: 0x001133AF File Offset: 0x001115AF
		internal string MakePath(string parent, string child, CmdletProviderContext context)
		{
			base.Context = context;
			return this.MakePath(parent, child);
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x001133C0 File Offset: 0x001115C0
		internal string GetParentPath(string path, string root, CmdletProviderContext context)
		{
			base.Context = context;
			return this.GetParentPath(path, root);
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x001133D1 File Offset: 0x001115D1
		internal string NormalizeRelativePath(string path, string basePath, CmdletProviderContext context)
		{
			base.Context = context;
			return this.NormalizeRelativePath(path, basePath);
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x001133E2 File Offset: 0x001115E2
		internal string GetChildName(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.GetChildName(path);
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x001133F2 File Offset: 0x001115F2
		internal bool IsItemContainer(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.IsItemContainer(path);
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x00113402 File Offset: 0x00111602
		internal void MoveItem(string path, string destination, CmdletProviderContext context)
		{
			base.Context = context;
			this.MoveItem(path, destination);
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x00113413 File Offset: 0x00111613
		internal object MoveItemDynamicParameters(string path, string destination, CmdletProviderContext context)
		{
			base.Context = context;
			return this.MoveItemDynamicParameters(path, destination);
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x00113424 File Offset: 0x00111624
		protected virtual string MakePath(string parent, string child)
		{
			return this.MakePath(parent, child, false);
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x00113430 File Offset: 0x00111630
		protected string MakePath(string parent, string child, bool childIsLeaf)
		{
			string result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (parent == null && child == null)
				{
					throw PSTraceSource.NewArgumentException("parent");
				}
				string text;
				if (string.IsNullOrEmpty(parent) && string.IsNullOrEmpty(child))
				{
					text = string.Empty;
				}
				else if (string.IsNullOrEmpty(parent) && !string.IsNullOrEmpty(child))
				{
					text = child.Replace('/', '\\');
				}
				else if (!string.IsNullOrEmpty(parent) && string.IsNullOrEmpty(child))
				{
					if (parent.EndsWith('\\'.ToString(), StringComparison.Ordinal))
					{
						text = parent;
					}
					else
					{
						text = parent + '\\';
					}
				}
				else
				{
					if (childIsLeaf)
					{
						parent = this.NormalizePath(parent);
					}
					else
					{
						parent = parent.Replace('/', '\\');
						child = child.Replace('/', '\\');
					}
					StringBuilder stringBuilder = new StringBuilder();
					if (parent.EndsWith('\\'.ToString(), StringComparison.Ordinal))
					{
						if (child.StartsWith('\\'.ToString(), StringComparison.Ordinal))
						{
							stringBuilder.Append(parent);
							stringBuilder.Append(child, 1, child.Length - 1);
						}
						else
						{
							stringBuilder.Append(parent);
							stringBuilder.Append(child);
						}
					}
					else if (child.StartsWith('\\'.ToString(), StringComparison.CurrentCulture))
					{
						stringBuilder.Append(parent);
						if (parent.Length == 0)
						{
							stringBuilder.Append(child, 1, child.Length - 1);
						}
						else
						{
							stringBuilder.Append(child);
						}
					}
					else
					{
						stringBuilder.Append(parent);
						if (parent.Length > 0 && child.Length > 0)
						{
							stringBuilder.Append('\\');
						}
						stringBuilder.Append(child);
					}
					text = stringBuilder.ToString();
				}
				CmdletProvider.providerBaseTracer.WriteLine("result={0}", new object[]
				{
					text
				});
				result = text;
			}
			return result;
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x0011360C File Offset: 0x0011180C
		protected virtual string GetParentPath(string path, string root)
		{
			string result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (string.IsNullOrEmpty(path))
				{
					throw PSTraceSource.NewArgumentException("path");
				}
				if (root == null && base.PSDriveInfo != null)
				{
					root = base.PSDriveInfo.Root;
				}
				path = this.NormalizePath(path);
				path = path.TrimEnd(new char[]
				{
					'\\'
				});
				string strB = string.Empty;
				if (root != null)
				{
					strB = this.NormalizePath(root);
				}
				string text;
				if (string.Compare(path, strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					text = string.Empty;
				}
				else
				{
					int num = path.LastIndexOf('\\');
					if (num != -1)
					{
						if (num == 0)
						{
							num++;
						}
						text = path.Substring(0, num);
					}
					else
					{
						text = string.Empty;
					}
				}
				result = text;
			}
			return result;
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x001136E0 File Offset: 0x001118E0
		protected virtual string NormalizeRelativePath(string path, string basePath)
		{
			string result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = this.ContractRelativePath(path, basePath, false, base.Context);
			}
			return result;
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x00113720 File Offset: 0x00111920
		internal string ContractRelativePath(string path, string basePath, bool allowNonExistingPaths, CmdletProviderContext context)
		{
			base.Context = context;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				return string.Empty;
			}
			if (basePath == null)
			{
				basePath = string.Empty;
			}
			CmdletProvider.providerBaseTracer.WriteLine("basePath = {0}", new object[]
			{
				basePath
			});
			string text = path;
			bool flag = false;
			string text2 = path;
			string text3 = basePath;
			if (!string.Equals(context.ProviderInstance.ProviderInfo.FullName, "Microsoft.ActiveDirectory.Management\\ActiveDirectory", StringComparison.OrdinalIgnoreCase))
			{
				text2 = this.NormalizePath(path);
				text3 = this.NormalizePath(basePath);
			}
			string text4 = path;
			if (path.EndsWith('\\'.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				path = path.TrimEnd(new char[]
				{
					'\\'
				});
				flag = true;
			}
			basePath = basePath.TrimEnd(new char[]
			{
				'\\'
			});
			if (string.Equals(text2, text3, StringComparison.OrdinalIgnoreCase) && !text4.EndsWith(string.Concat('\\'), StringComparison.OrdinalIgnoreCase))
			{
				string childName = this.GetChildName(path);
				text = this.MakePath("..", childName);
			}
			else if (!text2.StartsWith(text3, StringComparison.OrdinalIgnoreCase) && basePath.Length > 0)
			{
				text = string.Empty;
				string commonBase = this.GetCommonBase(text2, text3);
				Stack<string> stack = this.TokenizePathToStack(text3, commonBase);
				int num = stack.Count;
				if (string.IsNullOrEmpty(commonBase))
				{
					num--;
				}
				for (int i = 0; i < num; i++)
				{
					text = this.MakePath("..", text);
				}
				if (!string.IsNullOrEmpty(commonBase))
				{
					if (string.Equals(text2, commonBase, StringComparison.OrdinalIgnoreCase) && !text2.EndsWith(string.Concat('\\'), StringComparison.OrdinalIgnoreCase))
					{
						string childName2 = this.GetChildName(path);
						text = this.MakePath("..", text);
						text = this.MakePath(text, childName2);
					}
					else
					{
						string[] array = this.TokenizePathToStack(text2, commonBase).ToArray();
						for (int j = 0; j < array.Length; j++)
						{
							text = this.MakePath(text, array[j]);
						}
					}
				}
			}
			else
			{
				Stack<string> tokenizedPathStack = this.TokenizePathToStack(path, basePath);
				Stack<string> normalizedPathStack = new Stack<string>();
				try
				{
					normalizedPathStack = NavigationCmdletProvider.NormalizeThePath(tokenizedPathStack, path, basePath, allowNonExistingPaths);
				}
				catch (ArgumentException ex)
				{
					base.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, null));
					text = null;
					goto IL_23D;
				}
				text = this.CreateNormalizedRelativePathFromStack(normalizedPathStack);
			}
			IL_23D:
			if (flag)
			{
				text += '\\';
			}
			CmdletProvider.providerBaseTracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x001139AC File Offset: 0x00111BAC
		private string GetCommonBase(string path1, string path2)
		{
			while (!string.Equals(path1, path2, StringComparison.OrdinalIgnoreCase))
			{
				if (path2.Length > path1.Length)
				{
					path2 = this.GetParentPath(path2, null);
				}
				else
				{
					path1 = this.GetParentPath(path1, null);
				}
			}
			return path1;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x001139E0 File Offset: 0x00111BE0
		protected virtual string GetChildName(string path)
		{
			string result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (string.IsNullOrEmpty(path))
				{
					throw PSTraceSource.NewArgumentException("path");
				}
				path = this.NormalizePath(path);
				path = path.TrimEnd(new char[]
				{
					'\\'
				});
				int num = path.LastIndexOf('\\');
				string text;
				if (num == -1)
				{
					text = path;
				}
				else if (base.ItemExists(path, base.Context))
				{
					string parentPath = this.GetParentPath(path, null);
					if (string.IsNullOrEmpty(parentPath))
					{
						text = path;
					}
					else if (parentPath.IndexOf('\\') == parentPath.Length - 1)
					{
						num = path.IndexOf(parentPath, StringComparison.OrdinalIgnoreCase) + parentPath.Length;
						text = path.Substring(num);
					}
					else
					{
						num = path.IndexOf(parentPath, StringComparison.OrdinalIgnoreCase) + parentPath.Length;
						text = path.Substring(num + 1);
					}
				}
				else
				{
					text = path.Substring(num + 1);
				}
				CmdletProvider.providerBaseTracer.WriteLine("Result = {0}", new object[]
				{
					text
				});
				result = text;
			}
			return result;
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x00113AF0 File Offset: 0x00111CF0
		protected virtual bool IsItemContainer(string path)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00113B30 File Offset: 0x00111D30
		protected virtual void MoveItem(string path, string destination)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00113B70 File Offset: 0x00111D70
		protected virtual object MoveItemDynamicParameters(string path, string destination)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x00113BA4 File Offset: 0x00111DA4
		private string NormalizePath(string path)
		{
			bool flag = path.IndexOf('/') != -1;
			bool flag2 = path.IndexOf('\\') != -1;
			bool flag3 = flag && flag2;
			bool flag4 = true;
			string text = path.Replace('/', '\\');
			if (flag3 && this.IsAbsolutePath(path) && this.ItemExists(path) && (path.EndsWith('/'.ToString(), StringComparison.Ordinal) || !this.ItemExists(text)))
			{
				flag4 = false;
			}
			if (!flag4)
			{
				return path;
			}
			return text;
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x00113C28 File Offset: 0x00111E28
		private bool IsAbsolutePath(string path)
		{
			bool result = false;
			if (LocationGlobber.IsAbsolutePath(path))
			{
				result = true;
			}
			else if (base.PSDriveInfo != null && !string.IsNullOrEmpty(base.PSDriveInfo.Root) && path.StartsWith(base.PSDriveInfo.Root, StringComparison.OrdinalIgnoreCase))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00113C7C File Offset: 0x00111E7C
		private Stack<string> TokenizePathToStack(string path, string basePath)
		{
			Stack<string> stack = new Stack<string>();
			string text = path;
			string text2 = path;
			while (text.Length > basePath.Length)
			{
				string childName = this.GetChildName(text);
				if (string.IsNullOrEmpty(childName))
				{
					stack.Push(text);
					break;
				}
				CmdletProvider.providerBaseTracer.WriteLine("tokenizedPathStack.Push({0})", new object[]
				{
					childName
				});
				stack.Push(childName);
				text = this.GetParentPath(text, basePath);
				if (text.Length >= text2.Length)
				{
					break;
				}
				text2 = text;
			}
			return stack;
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x00113CFC File Offset: 0x00111EFC
		private static Stack<string> NormalizeThePath(Stack<string> tokenizedPathStack, string path, string basePath, bool allowNonExistingPaths)
		{
			Stack<string> stack = new Stack<string>();
			while (tokenizedPathStack.Count > 0)
			{
				string text = tokenizedPathStack.Pop();
				CmdletProvider.providerBaseTracer.WriteLine("childName = {0}", new object[]
				{
					text
				});
				if (!text.Equals(".", StringComparison.OrdinalIgnoreCase))
				{
					if (text.Equals("..", StringComparison.OrdinalIgnoreCase))
					{
						if (stack.Count > 0)
						{
							string text2 = stack.Pop();
							CmdletProvider.providerBaseTracer.WriteLine("normalizedPathStack.Pop() : {0}", new object[]
							{
								text2
							});
							continue;
						}
						if (!allowNonExistingPaths)
						{
							PSArgumentException ex = PSTraceSource.NewArgumentException("path", SessionStateStrings.NormalizeRelativePathOutsideBase, new object[]
							{
								path,
								basePath
							});
							throw ex;
						}
					}
					CmdletProvider.providerBaseTracer.WriteLine("normalizedPathStack.Push({0})", new object[]
					{
						text
					});
					stack.Push(text);
				}
			}
			return stack;
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x00113DE4 File Offset: 0x00111FE4
		private string CreateNormalizedRelativePathFromStack(Stack<string> normalizedPathStack)
		{
			string text = string.Empty;
			while (normalizedPathStack.Count > 0)
			{
				if (string.IsNullOrEmpty(text))
				{
					text = normalizedPathStack.Pop();
				}
				else
				{
					string parent = normalizedPathStack.Pop();
					text = this.MakePath(parent, text);
				}
			}
			CmdletProvider.providerBaseTracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}
	}
}
