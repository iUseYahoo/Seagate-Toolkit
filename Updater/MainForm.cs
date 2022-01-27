using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CommonUtils;
using Microsoft.Win32;

namespace Upgrader
{
	// Token: 0x02000003 RID: 3
	public class MainForm : Form
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002058 File Offset: 0x00000258
		public MainForm(Log log, string from, string to, string exe, string left, string top, string startTime)
		{
			this.InitializeComponent();
			this._fromPath = from;
			this._toPath = to;
			this._exe = exe;
			this._leftPosition = left;
			this._topPosition = top;
			this._startTime = startTime;
			this._log = log;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000020F4 File Offset: 0x000002F4
		public MainForm(Log log, string from, string to, string exe, string left, string top, string startTime, string actionType)
		{
			this.InitializeComponent();
			this._log = log;
			this._log.I("InitializeComponent", new object[0]);
			this._fromPath = from;
			this._toPath = to;
			this._exe = exe;
			this._leftPosition = left;
			this._topPosition = top;
			this._startTime = startTime;
			if (actionType == "Relunch")
			{
				this._log.I("Relunch", new object[0]);
				this._isJustRelunch = true;
				return;
			}
			if (actionType == "UpgradeAndRelunch")
			{
				this._log.I("UpgradeAndRelunch", new object[0]);
				this._isHiddenUpgradeRelunch = true;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021FC File Offset: 0x000003FC
		public MainForm(Log log, string commandLine)
		{
			this.InitializeComponent();
			this._log = log;
			this._installerCommandLine = commandLine;
			this._toPath = Application.StartupPath;
			this._exe = "Toolkit.exe";
			this._isLaunchFromInstaller = true;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002290 File Offset: 0x00000490
		private void MainForm_Load(object sender, EventArgs e)
		{
			base.Left = -1;
			base.Top = -1;
			base.Width = 1;
			base.Height = 1;
			base.IsAccessible = false;
			this._log.I("MainForm_Load start", new object[0]);
			this._log.I("TerminateProcess start", new object[0]);
			this.TerminateProcess();
			Thread.Sleep(1000);
			if (!this._isJustRelunch)
			{
				this._log.I("Copy start from " + this._fromPath + " to " + this._toPath, new object[0]);
				this.CopyToPath(this._fromPath, this._toPath);
				this._log.I("Copy finish from " + this._fromPath + " to " + this._toPath, new object[0]);
			}
			string fileName = Path.Combine(this._toPath, this._exe);
			this.UpdateAppVersion(fileName);
			if (this._isLaunchFromInstaller)
			{
				UacUtil.TryRunAsDesktopUser(Path.Combine(this._toPath, this._exe), this._installerCommandLine);
				base.Close();
				return;
			}
			string text = (this._isJustRelunch || this._isHiddenUpgradeRelunch) ? "relunch" : "upgrade";
			string text2 = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", new object[]
			{
				string.Empty,
				this._topPosition,
				this._leftPosition,
				this._startTime,
				text
			});
			this._log.I(string.Format("Start exe: {0}, {1}, {2}", this._toPath, this._exe, text2), new object[0]);
			UacUtil.TryRunAsDesktopUser(Path.Combine(this._toPath, this._exe), text2);
			this._log.I("SyncHelper exit", new object[0]);
			base.Close();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002464 File Offset: 0x00000664
		private void UpdateAppVersion(string fileName)
		{
			try
			{
				if (File.Exists(fileName))
				{
					string productVersion = FileVersionInfo.GetVersionInfo(fileName).ProductVersion;
					RegistryKey localMachine = Registry.LocalMachine;
					RegistryKey registryKey = localMachine.OpenSubKey(string.Format("SOFTWARE\\Microsoft\\WINDOWS\\CURRENTVERSION\\UNINSTALL\\{0}", "Toolkit"), true);
					if (registryKey != null)
					{
						registryKey.SetValue("DisplayVersion", productVersion);
						registryKey.Close();
						localMachine.Close();
					}
				}
			}
			catch (Exception ex)
			{
				this._log.I(string.Format("UpdateAppVersion ex: {0}", ex.ToString()), new object[0]);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024F8 File Offset: 0x000006F8
		private bool TerminateProcess()
		{
			this._log.I("_exe: " + this._exe, new object[0]);
			if (this._exe.Length <= 4)
			{
				return false;
			}
			int num = 0;
			for (;;)
			{
				try
				{
					string text = this._exe.Substring(0, this._exe.Length - 4);
					this._log.I("exeName: " + text, new object[0]);
					Process[] processesByName = Process.GetProcessesByName(text);
					this._log.I(string.Format("processes count of {0}, {1}, tryCount = {2}", text, processesByName.Count<Process>(), num++), new object[0]);
					if (processesByName == null || processesByName.Count<Process>() <= 0)
					{
						break;
					}
					foreach (Process process in processesByName)
					{
						this._log.I(process.MainModule.FileName ?? "", new object[0]);
						if (Path.Combine(this._toPath, this._exe) == process.MainModule.FileName)
						{
							this._log.I("kill process: " + process.MainModule.FileName, new object[0]);
							this._log.I(string.Format("exit, {0}", process.MainModule.FileName), new object[0]);
							process.Kill();
							process.Close();
						}
					}
				}
				catch (Exception ex)
				{
					this._log.E(string.Format("ex {0}", ex.ToString()), new object[0]);
				}
				Thread.Sleep(1000);
			}
			return false;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000026D4 File Offset: 0x000008D4
		private void CopyToPath(string fromFolder, string toFolder)
		{
			try
			{
				this._log.I(string.Format("Copy from {0} to {1}", fromFolder, toFolder), new object[0]);
				if (!Directory.Exists(fromFolder))
				{
					this._log.I(string.Format("from folder not exist", new object[0]), new object[0]);
				}
				else
				{
					FileFolderHelper.CreateFolder(toFolder);
					string[] fileSystemEntries = Directory.GetFileSystemEntries(fromFolder);
					int i = 0;
					while (i < fileSystemEntries.Length)
					{
						string text = fileSystemEntries[i];
						FileInfo fileInfo = new FileInfo(text);
						string text2 = Path.Combine(toFolder, fileInfo.Name);
						if (File.Exists(text))
						{
							this._log.I(string.Format("copy \"{0}\" to \"{1}\"", text, text2), new object[0]);
							try
							{
								File.Copy(text, text2, true);
								goto IL_D6;
							}
							catch (Exception ex)
							{
								this._log.E(string.Format("copy error: {0}", ex.ToString()), new object[0]);
								goto IL_D6;
							}
							goto IL_CD;
						}
						goto IL_CD;
						IL_D6:
						i++;
						continue;
						IL_CD:
						this.CopyToPath(text, text2);
						goto IL_D6;
					}
				}
			}
			catch (Exception ex2)
			{
				this._log.E(string.Format("copy error: {0}", ex2.ToString()), new object[0]);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002808 File Offset: 0x00000A08
		private static void DeleteFolder(string dir, bool bDeleteDir)
		{
			if (Directory.Exists(dir))
			{
				if (dir.Length <= 3)
				{
					return;
				}
				foreach (string text in Directory.GetFileSystemEntries(dir))
				{
					if (File.Exists(text))
					{
						FileInfo fileInfo = new FileInfo(text);
						if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}
						FileFolderHelper.DeleteFile(text);
					}
					else
					{
						MainForm.DeleteFolder(text, true);
					}
				}
				if (bDeleteDir)
				{
					Directory.Delete(dir);
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002894 File Offset: 0x00000A94
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000028B4 File Offset: 0x00000AB4
		private void InitializeComponent()
		{
			this.components = new Container();
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(300, 300);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "MainForm";
			base.Opacity = 0.01;
			base.ShowInTaskbar = false;
			this.Text = "SeagateOmniUpgrader";
			base.Load += this.MainForm_Load;
			base.ResumeLayout(false);
		}

		// Token: 0x04000006 RID: 6
		private string _fromPath = string.Empty;

		// Token: 0x04000007 RID: 7
		private string _toPath = string.Empty;

		// Token: 0x04000008 RID: 8
		private string _exe = string.Empty;

		// Token: 0x04000009 RID: 9
		private string _leftPosition = string.Empty;

		// Token: 0x0400000A RID: 10
		private string _topPosition = string.Empty;

		// Token: 0x0400000B RID: 11
		private string _startTime = "0";

		// Token: 0x0400000C RID: 12
		private bool _isJustRelunch;

		// Token: 0x0400000D RID: 13
		private bool _isHiddenUpgradeRelunch;

		// Token: 0x0400000E RID: 14
		private bool _isLaunchFromInstaller;

		// Token: 0x0400000F RID: 15
		private string _installerCommandLine = string.Empty;

		// Token: 0x04000010 RID: 16
		private Log _log;

		// Token: 0x04000011 RID: 17
		private IContainer components;
	}
}
