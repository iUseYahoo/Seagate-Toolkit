using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using CommonUtils;

namespace ServiceControl
{
	// Token: 0x02000003 RID: 3
	internal class ServiceManager
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002086 File Offset: 0x00000286
		private static string GetDestServicePath()
		{
			string appDataService = PathManager.AppDataService;
			FileFolderHelper.CreateFolder(appDataService);
			return Path.Combine(appDataService, "ToolkitService.exe");
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000209E File Offset: 0x0000029E
		private static string GetDestServiceVersion()
		{
			return Utility.GetFileVersion(ServiceManager.GetDestServicePath());
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020AA File Offset: 0x000002AA
		private static string GetSourceServicePath()
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ToolkitService.exe");
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C5 File Offset: 0x000002C5
		private static string GetSourceServiceVersion()
		{
			return Utility.GetFileVersion(ServiceManager.GetSourceServicePath());
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020D4 File Offset: 0x000002D4
		public static bool InstallAndStartService()
		{
			string sourceServicePath = ServiceManager.GetSourceServicePath();
			string destServicePath = ServiceManager.GetDestServicePath();
			bool result;
			try
			{
				ServiceManager.StopService(10000);
				for (int i = 0; i < 10; i++)
				{
					try
					{
						FileFolderHelper.DeleteFile(destServicePath);
						break;
					}
					catch
					{
						Thread.Sleep(1000);
					}
				}
				FileFolderHelper.Copy(sourceServicePath, destServicePath, true);
				Process process = Process.Start(new ProcessStartInfo
				{
					FileName = "sc.exe",
					WorkingDirectory = Environment.SystemDirectory,
					Arguments = "create \"Toolkit Service\" binpath=\"" + destServicePath + "\" start= auto",
					Verb = "RunAs",
					WindowStyle = ProcessWindowStyle.Hidden
				});
				if (process == null)
				{
					result = false;
				}
				else if (!process.WaitForExit(10000))
				{
					result = false;
				}
				else
				{
					result = ServiceManager.StartService(10000);
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021B8 File Offset: 0x000003B8
		public static bool IsServiceAvailable()
		{
			bool result;
			try
			{
				if (!ServiceManager.IsServiceInstalled())
				{
					result = false;
				}
				else if (new ServiceController("Toolkit Service").Status != ServiceControllerStatus.Running)
				{
					result = false;
				}
				else if (!FileFolderHelper.IsFileExist(ServiceManager.GetDestServicePath()))
				{
					result = false;
				}
				else if (ServiceManager.GetSourceServiceVersion() != ServiceManager.GetDestServiceVersion())
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002228 File Offset: 0x00000428
		private static bool StartService(int timeoutMs)
		{
			ServiceController serviceController = new ServiceController("Toolkit Service");
			bool result;
			try
			{
				serviceController.Start();
				serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds((double)timeoutMs));
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002270 File Offset: 0x00000470
		public static bool StopAndUninstallService()
		{
			string destServicePath = ServiceManager.GetDestServicePath();
			bool result;
			try
			{
				ServiceManager.StopService(10000);
				if (!File.Exists(destServicePath))
				{
					Console.WriteLine("StopAndUninstallService: {0} file does not exist", destServicePath);
					result = false;
				}
				else
				{
					Process process = Process.Start(new ProcessStartInfo
					{
						FileName = "sc.exe",
						WorkingDirectory = Environment.SystemDirectory,
						Arguments = "delete \"Toolkit Service\"",
						Verb = "RunAs",
						WindowStyle = ProcessWindowStyle.Hidden
					});
					if (process == null)
					{
						result = false;
					}
					else
					{
						process.WaitForExit(100000);
						for (int i = 0; i < 10; i++)
						{
							try
							{
								File.Delete(destServicePath);
								break;
							}
							catch
							{
								Thread.Sleep(1000);
							}
						}
						result = true;
					}
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002340 File Offset: 0x00000540
		private static bool StopService(int timeoutMs)
		{
			if (ServiceManager.IsServiceInstalled())
			{
				ServiceController serviceController = new ServiceController("Toolkit Service");
				try
				{
					serviceController.Stop();
					serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds((double)timeoutMs));
					return true;
				}
				catch
				{
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002390 File Offset: 0x00000590
		private static bool IsServiceInstalled()
		{
			try
			{
				return ServiceController.GetServices().FirstOrDefault((ServiceController p) => p.ServiceName == "Toolkit Service") != null;
			}
			catch (Exception)
			{
			}
			return false;
		}
	}
}
