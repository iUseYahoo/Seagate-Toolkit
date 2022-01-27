using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Upgrader
{
	// Token: 0x02000005 RID: 5
	internal static class UacUtil
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002EB8 File Offset: 0x000010B8
		private static bool RunAsDesktopUser(string fileName, string commandLine)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", "fileName");
			}
			IntPtr zero = IntPtr.Zero;
			try
			{
				if (!UacUtil.OpenProcessToken(UacUtil.GetCurrentProcess(), 32, ref zero))
				{
					return false;
				}
				UacUtil.TOKEN_PRIVILEGES token_PRIVILEGES = new UacUtil.TOKEN_PRIVILEGES
				{
					PrivilegeCount = 1U,
					Privileges = new UacUtil.LUID_AND_ATTRIBUTES[1]
				};
				if (!UacUtil.LookupPrivilegeValue(null, "SeIncreaseQuotaPrivilege", ref token_PRIVILEGES.Privileges[0].Luid))
				{
					return false;
				}
				token_PRIVILEGES.Privileges[0].Attributes = 2U;
				if (!UacUtil.AdjustTokenPrivileges(zero, false, ref token_PRIVILEGES, 0, IntPtr.Zero, IntPtr.Zero))
				{
					return false;
				}
			}
			finally
			{
				UacUtil.CloseHandle(zero);
			}
			IntPtr shellWindow = UacUtil.GetShellWindow();
			if (shellWindow == IntPtr.Zero)
			{
				return false;
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			IntPtr zero3 = IntPtr.Zero;
			try
			{
				uint processId;
				if (UacUtil.GetWindowThreadProcessId(shellWindow, out processId) == 0U)
				{
					return false;
				}
				intPtr = UacUtil.OpenProcess(UacUtil.ProcessAccessFlags.QueryInformation, false, processId);
				if (intPtr == IntPtr.Zero)
				{
					return false;
				}
				if (!UacUtil.OpenProcessToken(intPtr, 2, ref zero2))
				{
					return false;
				}
				uint dwDesiredAccess = 395U;
				if (!UacUtil.DuplicateTokenEx(zero2, dwDesiredAccess, IntPtr.Zero, UacUtil.SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, UacUtil.TOKEN_TYPE.TokenPrimary, out zero3))
				{
					return false;
				}
				UacUtil.STARTUPINFO startupinfo = default(UacUtil.STARTUPINFO);
				UacUtil.PROCESS_INFORMATION process_INFORMATION = default(UacUtil.PROCESS_INFORMATION);
				if (!UacUtil.CreateProcessWithTokenW(zero3, 0, fileName, commandLine, 0, IntPtr.Zero, Path.GetDirectoryName(fileName), ref startupinfo, out process_INFORMATION))
				{
					return false;
				}
			}
			finally
			{
				UacUtil.CloseHandle(zero2);
				UacUtil.CloseHandle(zero3);
				UacUtil.CloseHandle(intPtr);
			}
			return true;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003074 File Offset: 0x00001274
		internal static void TryRunAsDesktopUser(string fileName, string commandLine)
		{
			if (!UacUtil.RunAsDesktopUser(fileName, commandLine))
			{
				Process.Start(fileName);
			}
		}

		// Token: 0x0600001E RID: 30
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern IntPtr GetCurrentProcess();

		// Token: 0x0600001F RID: 31
		[DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

		// Token: 0x06000020 RID: 32
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LookupPrivilegeValue(string host, string name, ref UacUtil.LUID pluid);

		// Token: 0x06000021 RID: 33
		[DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref UacUtil.TOKEN_PRIVILEGES newst, int len, IntPtr prev, IntPtr relen);

		// Token: 0x06000022 RID: 34
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x06000023 RID: 35
		[DllImport("user32.dll")]
		private static extern IntPtr GetShellWindow();

		// Token: 0x06000024 RID: 36
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		// Token: 0x06000025 RID: 37
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr OpenProcess(UacUtil.ProcessAccessFlags processAccess, bool bInheritHandle, uint processId);

		// Token: 0x06000026 RID: 38
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DuplicateTokenEx(IntPtr hExistingToken, uint dwDesiredAccess, IntPtr lpTokenAttributes, UacUtil.SECURITY_IMPERSONATION_LEVEL impersonationLevel, UacUtil.TOKEN_TYPE tokenType, out IntPtr phNewToken);

		// Token: 0x06000027 RID: 39
		[DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool CreateProcessWithTokenW(IntPtr hToken, int dwLogonFlags, string lpApplicationName, string lpCommandLine, int dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref UacUtil.STARTUPINFO lpStartupInfo, out UacUtil.PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x0200000D RID: 13
		private struct TOKEN_PRIVILEGES
		{
			// Token: 0x0400003E RID: 62
			public uint PrivilegeCount;

			// Token: 0x0400003F RID: 63
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
			public UacUtil.LUID_AND_ATTRIBUTES[] Privileges;
		}

		// Token: 0x0200000E RID: 14
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct LUID_AND_ATTRIBUTES
		{
			// Token: 0x04000040 RID: 64
			public UacUtil.LUID Luid;

			// Token: 0x04000041 RID: 65
			public uint Attributes;
		}

		// Token: 0x0200000F RID: 15
		private struct LUID
		{
			// Token: 0x04000042 RID: 66
			public uint LowPart;

			// Token: 0x04000043 RID: 67
			public int HighPart;
		}

		// Token: 0x02000010 RID: 16
		[Flags]
		private enum ProcessAccessFlags : uint
		{
			// Token: 0x04000045 RID: 69
			All = 2035711U,
			// Token: 0x04000046 RID: 70
			Terminate = 1U,
			// Token: 0x04000047 RID: 71
			CreateThread = 2U,
			// Token: 0x04000048 RID: 72
			VirtualMemoryOperation = 8U,
			// Token: 0x04000049 RID: 73
			VirtualMemoryRead = 16U,
			// Token: 0x0400004A RID: 74
			VirtualMemoryWrite = 32U,
			// Token: 0x0400004B RID: 75
			DuplicateHandle = 64U,
			// Token: 0x0400004C RID: 76
			CreateProcess = 128U,
			// Token: 0x0400004D RID: 77
			SetQuota = 256U,
			// Token: 0x0400004E RID: 78
			SetInformation = 512U,
			// Token: 0x0400004F RID: 79
			QueryInformation = 1024U,
			// Token: 0x04000050 RID: 80
			QueryLimitedInformation = 4096U,
			// Token: 0x04000051 RID: 81
			Synchronize = 1048576U
		}

		// Token: 0x02000011 RID: 17
		private enum SECURITY_IMPERSONATION_LEVEL
		{
			// Token: 0x04000053 RID: 83
			SecurityAnonymous,
			// Token: 0x04000054 RID: 84
			SecurityIdentification,
			// Token: 0x04000055 RID: 85
			SecurityImpersonation,
			// Token: 0x04000056 RID: 86
			SecurityDelegation
		}

		// Token: 0x02000012 RID: 18
		private enum TOKEN_TYPE
		{
			// Token: 0x04000058 RID: 88
			TokenPrimary = 1,
			// Token: 0x04000059 RID: 89
			TokenImpersonation
		}

		// Token: 0x02000013 RID: 19
		private struct PROCESS_INFORMATION
		{
			// Token: 0x0400005A RID: 90
			public IntPtr hProcess;

			// Token: 0x0400005B RID: 91
			public IntPtr hThread;

			// Token: 0x0400005C RID: 92
			public int dwProcessId;

			// Token: 0x0400005D RID: 93
			public int dwThreadId;
		}

		// Token: 0x02000014 RID: 20
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct STARTUPINFO
		{
			// Token: 0x0400005E RID: 94
			public int cb;

			// Token: 0x0400005F RID: 95
			public string lpReserved;

			// Token: 0x04000060 RID: 96
			public string lpDesktop;

			// Token: 0x04000061 RID: 97
			public string lpTitle;

			// Token: 0x04000062 RID: 98
			public int dwX;

			// Token: 0x04000063 RID: 99
			public int dwY;

			// Token: 0x04000064 RID: 100
			public int dwXSize;

			// Token: 0x04000065 RID: 101
			public int dwYSize;

			// Token: 0x04000066 RID: 102
			public int dwXCountChars;

			// Token: 0x04000067 RID: 103
			public int dwYCountChars;

			// Token: 0x04000068 RID: 104
			public int dwFillAttribute;

			// Token: 0x04000069 RID: 105
			public int dwFlags;

			// Token: 0x0400006A RID: 106
			public short wShowWindow;

			// Token: 0x0400006B RID: 107
			public short cbReserved2;

			// Token: 0x0400006C RID: 108
			public IntPtr lpReserved2;

			// Token: 0x0400006D RID: 109
			public IntPtr hStdInput;

			// Token: 0x0400006E RID: 110
			public IntPtr hStdOutput;

			// Token: 0x0400006F RID: 111
			public IntPtr hStdError;
		}
	}
}
