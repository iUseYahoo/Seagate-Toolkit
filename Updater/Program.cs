using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using CommonUtils;
using DeviceManage;
using Microsoft.Win32;
using ToastManager.Shell;

namespace Upgrader
{
	// Token: 0x02000004 RID: 4
	internal static class Program
	{
		// Token: 0x06000016 RID: 22 RVA: 0x0000294C File Offset: 0x00000B4C
		[STAThread]
		private static void Main()
		{
			PathManager.Init();
			LogConfigure.SetLogFolder(PathManager.AppDataLog);
			LogConfigure.Init(PathManager.AppDataLog, LogOutputTypes.FileAndDebugView, LogLevels.I);
			Program._log = new Log("upgrader", true, new Log4netWrapper("Upgrader"));
			Program._log.I("Upgrader Start!", new object[0]);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			string empty = string.Empty;
			if (commandLineArgs == null)
			{
				return;
			}
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				string arg = commandLineArgs[i];
				Program._log.I(string.Format("upgrade start args: {0}--{1}", i, arg), new object[0]);
			}
			if (commandLineArgs.Count<string>() == 2 && commandLineArgs[1] == "-uninstall")
			{
				QuickAndFavoritePath quickAndFavoritePath = QuickAndFavoritePath.Load();
				if (quickAndFavoritePath != null)
				{
					foreach (string text in quickAndFavoritePath.Paths)
					{
						Utility.RemoveFolderFromQuickAccess(text);
						Utility.RemovePathFromFavorites(text);
					}
				}
			}
			if (commandLineArgs.Count<string>() < 4)
			{
				return;
			}
			if (commandLineArgs.Count<string>() == 5)
			{
				string text2 = string.Concat(new string[]
				{
					commandLineArgs[1],
					" ",
					commandLineArgs[2],
					" ",
					commandLineArgs[3],
					" ",
					commandLineArgs[4]
				});
				Program._log.I("installer first launch toolkit with command line: " + text2, new object[0]);
				Program.RegisterAppForNotificationSupport();
				MainForm mainForm = new MainForm(Program._log, text2);
				mainForm.Hide();
				Application.Run(mainForm);
				return;
			}
			string text3 = commandLineArgs[1];
			string text4 = commandLineArgs[2];
			string text5 = commandLineArgs[3];
			string text6 = "0";
			string text7 = "0";
			string text8 = "0";
			string text9 = string.Empty;
			if (commandLineArgs.Count<string>() >= 6)
			{
				text6 = commandLineArgs[4];
				text7 = commandLineArgs[5];
			}
			Program._log.I(string.Format("params count: {0}", commandLineArgs.Count<string>()), new object[0]);
			if (commandLineArgs.Count<string>() == 7)
			{
				text8 = commandLineArgs[6];
			}
			if (commandLineArgs.Count<string>() == 8)
			{
				text8 = commandLineArgs[6];
				text9 = commandLineArgs[7];
				Program._log.I(string.Concat(new string[]
				{
					"relunch toolkit, params: ",
					text5,
					", ",
					text6,
					", ",
					text7,
					", ",
					text8,
					", ",
					text9
				}), new object[0]);
				MainForm mainForm2 = new MainForm(Program._log, text3, text4, text5, text6, text7, text8, text9);
				mainForm2.Hide();
				Application.Run(mainForm2);
				return;
			}
			Program._log.I("params: {0}, from: {1}, to: {2}, exe: {3}, left: {4}, top: {5}, startTime: {6}", new object[]
			{
				commandLineArgs[0],
				text3,
				text4,
				text5,
				text6,
				text7,
				text8
			});
			MainForm mainForm3 = new MainForm(Program._log, text3, text4, text5, text6, text7, text8);
			mainForm3.Hide();
			Application.Run(mainForm3);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002C48 File Offset: 0x00000E48
		private static void ThunderboltDevice()
		{
			Device.Init();
			Device.EnumDeviceTree();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002C58 File Offset: 0x00000E58
		private static void RegisterAppForNotificationSupport()
		{
			Program._log.I("RegisterAppForNotificationSupport", new object[0]);
			string text = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Toolkit.lnk";
			if (Program.IsUserAdministrator())
			{
				Program._log.I("recreate shortcut", new object[0]);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				string exePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "Toolkit.exe");
				Program.InstallShortcut(text, exePath);
				Program.RegisterComServer(exePath);
				Program._log.I("recreate shortcut finish", new object[0]);
			}
			Program._log.I("end RegisterAppForNotificationSupport", new object[0]);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002D0C File Offset: 0x00000F0C
		public static bool IsUserAdministrator()
		{
			WindowsIdentity windowsIdentity = null;
			bool result;
			try
			{
				windowsIdentity = WindowsIdentity.GetCurrent();
				result = new WindowsPrincipal(windowsIdentity).IsInRole(WindowsBuiltInRole.Administrator);
			}
			catch (UnauthorizedAccessException)
			{
				result = false;
			}
			catch (Exception)
			{
				result = false;
			}
			finally
			{
				if (windowsIdentity != null)
				{
					windowsIdentity.Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002D74 File Offset: 0x00000F74
		private static void RegisterComServer(string exePath)
		{
			string subkey = string.Format("SOFTWARE\\Classes\\CLSID\\{{{0}}}\\LocalServer32", typeof(NotificationActivator).GUID);
			Registry.CurrentUser.CreateSubKey(subkey).SetValue(null, exePath);
			Program._log.I("end RegisterComServer", new object[0]);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002DC8 File Offset: 0x00000FC8
		private static void InstallShortcut(string shortcutPath, string exePath)
		{
			try
			{
				IShellLinkW shellLinkW = (IShellLinkW)new CShellLink();
				shellLinkW.SetPath(exePath);
				shellLinkW.SetWorkingDirectory(Path.GetDirectoryName(exePath));
				IPropertyStore propertyStore = (IPropertyStore)shellLinkW;
				PropVariantHelper propVariantHelper = new PropVariantHelper();
				propVariantHelper.SetValue("Toolkit");
				PROPERTYKEY propertykey = PROPERTYKEY.AppUserModel_ID;
				PROPVARIANT propvariant = propVariantHelper.Propvariant;
				propertyStore.SetValue(ref propertykey, ref propvariant);
				PropVariantHelper propVariantHelper2 = new PropVariantHelper();
				propVariantHelper2.VarType = VarEnum.VT_CLSID;
				propVariantHelper2.SetValue(typeof(NotificationActivator).GUID);
				propertykey = PROPERTYKEY.AppUserModel_ToastActivatorCLSID;
				propvariant = propVariantHelper2.Propvariant;
				propertyStore.SetValue(ref propertykey, ref propvariant);
				((IPersistFile)shellLinkW).Save(shortcutPath, true);
				Program._log.I("end InstallShortcut", new object[0]);
			}
			catch (Exception ex)
			{
				Program._log.E("InstallShortcut" + shortcutPath + "-->" + exePath, new object[]
				{
					ex
				});
			}
		}

		// Token: 0x04000012 RID: 18
		private static Log _log;
	}
}
