using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Upgrader
{
	// Token: 0x02000002 RID: 2
	public class HIDImports
	{
		// Token: 0x06000001 RID: 1
		[DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern void HidD_GetHidGuid(out Guid gHid);

		// Token: 0x06000002 RID: 2
		[DllImport("hid.dll")]
		internal static extern bool HidD_GetAttributes(IntPtr HidDeviceObject, ref HIDImports.HIDD_ATTRIBUTES Attributes);

		// Token: 0x06000003 RID: 3
		[DllImport("hid.dll")]
		internal static extern bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);

		// Token: 0x06000004 RID: 4
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, [MarshalAs(UnmanagedType.LPTStr)] string Enumerator, IntPtr hwndParent, uint Flags);

		// Token: 0x06000005 RID: 5
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInvo, ref Guid interfaceClassGuid, uint memberIndex, ref HIDImports.SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

		// Token: 0x06000006 RID: 6
		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref HIDImports.SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, IntPtr deviceInfoData);

		// Token: 0x06000007 RID: 7
		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref HIDImports.SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, IntPtr deviceInfoData);

		// Token: 0x06000008 RID: 8
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern ushort SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

		// Token: 0x06000009 RID: 9
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess, [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] HIDImports.EFileAttributes flags, IntPtr template);

		// Token: 0x0600000A RID: 10
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x04000001 RID: 1
		internal const int DIGCF_DEFAULT = 1;

		// Token: 0x04000002 RID: 2
		internal const int DIGCF_PRESENT = 2;

		// Token: 0x04000003 RID: 3
		internal const int DIGCF_ALLCLASSES = 4;

		// Token: 0x04000004 RID: 4
		internal const int DIGCF_PROFILE = 8;

		// Token: 0x04000005 RID: 5
		internal const int DIGCF_DEVICEINTERFACE = 16;

		// Token: 0x02000008 RID: 8
		[Flags]
		internal enum EFileAttributes : uint
		{
			// Token: 0x04000017 RID: 23
			Readonly = 1U,
			// Token: 0x04000018 RID: 24
			Hidden = 2U,
			// Token: 0x04000019 RID: 25
			System = 4U,
			// Token: 0x0400001A RID: 26
			Directory = 16U,
			// Token: 0x0400001B RID: 27
			Archive = 32U,
			// Token: 0x0400001C RID: 28
			Device = 64U,
			// Token: 0x0400001D RID: 29
			Normal = 128U,
			// Token: 0x0400001E RID: 30
			Temporary = 256U,
			// Token: 0x0400001F RID: 31
			SparseFile = 512U,
			// Token: 0x04000020 RID: 32
			ReparsePoint = 1024U,
			// Token: 0x04000021 RID: 33
			Compressed = 2048U,
			// Token: 0x04000022 RID: 34
			Offline = 4096U,
			// Token: 0x04000023 RID: 35
			NotContentIndexed = 8192U,
			// Token: 0x04000024 RID: 36
			Encrypted = 16384U,
			// Token: 0x04000025 RID: 37
			Write_Through = 2147483648U,
			// Token: 0x04000026 RID: 38
			Overlapped = 1073741824U,
			// Token: 0x04000027 RID: 39
			NoBuffering = 536870912U,
			// Token: 0x04000028 RID: 40
			RandomAccess = 268435456U,
			// Token: 0x04000029 RID: 41
			SequentialScan = 134217728U,
			// Token: 0x0400002A RID: 42
			DeleteOnClose = 67108864U,
			// Token: 0x0400002B RID: 43
			BackupSemantics = 33554432U,
			// Token: 0x0400002C RID: 44
			PosixSemantics = 16777216U,
			// Token: 0x0400002D RID: 45
			OpenReparsePoint = 2097152U,
			// Token: 0x0400002E RID: 46
			OpenNoRecall = 1048576U,
			// Token: 0x0400002F RID: 47
			FirstPipeInstance = 524288U
		}

		// Token: 0x02000009 RID: 9
		internal struct SP_DEVINFO_DATA
		{
			// Token: 0x04000030 RID: 48
			public uint cbSize;

			// Token: 0x04000031 RID: 49
			public Guid ClassGuid;

			// Token: 0x04000032 RID: 50
			public uint DevInst;

			// Token: 0x04000033 RID: 51
			public IntPtr Reserved;
		}

		// Token: 0x0200000A RID: 10
		internal struct SP_DEVICE_INTERFACE_DATA
		{
			// Token: 0x04000034 RID: 52
			public int cbSize;

			// Token: 0x04000035 RID: 53
			public Guid InterfaceClassGuid;

			// Token: 0x04000036 RID: 54
			public int Flags;

			// Token: 0x04000037 RID: 55
			public IntPtr RESERVED;
		}

		// Token: 0x0200000B RID: 11
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			// Token: 0x04000038 RID: 56
			public uint cbSize;

			// Token: 0x04000039 RID: 57
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string DevicePath;
		}

		// Token: 0x0200000C RID: 12
		public struct HIDD_ATTRIBUTES
		{
			// Token: 0x0400003A RID: 58
			public uint Size;

			// Token: 0x0400003B RID: 59
			public ushort VendorID;

			// Token: 0x0400003C RID: 60
			public ushort ProductID;

			// Token: 0x0400003D RID: 61
			public ushort VersionNumber;
		}
	}
}
