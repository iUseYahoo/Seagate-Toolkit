using System;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace eject.UsbEject
{
	// Token: 0x0200000B RID: 11
	public class EjectDevice
	{
		// Token: 0x06000028 RID: 40
		[DllImport("CfgMgr32.dll")]
		internal static extern int CM_Locate_DevNode(out int devHandle, string deviceId, int ulFlags);

		// Token: 0x06000029 RID: 41
		[DllImport("CfgMgr32.dll")]
		internal static extern int CM_MapCrToWin32Err(int returnCode, uint defaulterror);

		// Token: 0x0600002A RID: 42
		[DllImport("setupapi.dll")]
		internal static extern int CM_Request_Device_Eject(int dnDevInst, out PNP_VETO_TYPE pVetoType, StringBuilder pszVetoName, int ulNameLength, int ulFlags);

		// Token: 0x0600002B RID: 43 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public static bool EjectByPNPDeviceID(string PNPDeviceID)
		{
			if (string.IsNullOrEmpty(PNPDeviceID))
			{
				return false;
			}
			int devHandle;
			if (EjectDevice.CM_Locate_DevNode(out devHandle, PNPDeviceID, 0) == 0)
			{
				Device device = new DiskDeviceClass().Devices.FirstOrDefault((Device o) => o.InstanceHandle == devHandle);
				if (device != null)
				{
					return device.Eject(false) != null;
				}
			}
			return false;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B30 File Offset: 0x00000D30
		public static bool EjectMemoryCardByPNPDeviceID(string PNPDeviceID)
		{
			if (string.IsNullOrEmpty(PNPDeviceID))
			{
				return false;
			}
			try
			{
				string drivePhysicalNameByPNPDeviceID = EjectDevice.GetDrivePhysicalNameByPNPDeviceID(PNPDeviceID);
				if (!string.IsNullOrEmpty(drivePhysicalNameByPNPDeviceID))
				{
					return EjectDevice.Eject(EjectDevice.GetDriveHandleByPhysicalName(drivePhysicalNameByPNPDeviceID));
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B80 File Offset: 0x00000D80
		public static bool EjectMemoryCardByDriveLetter(string driveLetter)
		{
			if (string.IsNullOrEmpty(driveLetter))
			{
				return false;
			}
			try
			{
				return EjectDevice.Eject(EjectDevice.GetDriveHandleByDriveLetter(driveLetter));
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002BBC File Offset: 0x00000DBC
		public static string GetDrivePhysicalNameByPNPDeviceID(string PNPDeviceID)
		{
			if (string.IsNullOrEmpty(PNPDeviceID))
			{
				return null;
			}
			try
			{
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("select * from Win32_DiskDrive").Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					if (!(PNPDeviceID.ToLower() != Convert.ToString(managementObject.Properties["PNPDeviceID"].Value).ToLower()))
					{
						return managementObject.Properties["DeviceId"].Value.ToString();
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x0600002F RID: 47
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr SecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x06000030 RID: 48
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		private static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

		// Token: 0x06000031 RID: 49
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		private static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, byte[] lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

		// Token: 0x06000032 RID: 50
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x06000033 RID: 51 RVA: 0x00002C78 File Offset: 0x00000E78
		public static IntPtr GetDriveHandleByDriveLetter(string driveLetter)
		{
			return EjectDevice.CreateFile("\\\\.\\" + driveLetter[0].ToString() + ":", 3221225472U, 3U, IntPtr.Zero, 3U, 0U, IntPtr.Zero);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002CBA File Offset: 0x00000EBA
		public static IntPtr GetDriveHandleByPhysicalName(string physicalName)
		{
			return EjectDevice.CreateFile(physicalName, 3221225472U, 3U, IntPtr.Zero, 3U, 0U, IntPtr.Zero);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002CD4 File Offset: 0x00000ED4
		public static bool Eject(IntPtr handle)
		{
			bool result = false;
			if (EjectDevice.DismountVolume(handle))
			{
				EjectDevice.PreventRemovalOfVolume(handle, false);
				result = EjectDevice.AutoEjectVolume(handle);
			}
			EjectDevice.CloseHandle(handle);
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002D04 File Offset: 0x00000F04
		private static bool LockVolume(IntPtr handle)
		{
			for (int i = 0; i < 10; i++)
			{
				uint num;
				if (EjectDevice.DeviceIoControl(handle, 589848U, IntPtr.Zero, 0U, IntPtr.Zero, 0U, out num, IntPtr.Zero))
				{
					return true;
				}
				Marshal.GetLastWin32Error();
				Thread.Sleep(500);
			}
			return false;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D54 File Offset: 0x00000F54
		private static bool PreventRemovalOfVolume(IntPtr handle, bool prevent)
		{
			uint num;
			return EjectDevice.DeviceIoControl(handle, 2967556U, new byte[]
			{
				prevent ? 1 : 0
			}, 1U, IntPtr.Zero, 0U, out num, IntPtr.Zero);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D8C File Offset: 0x00000F8C
		private static bool DismountVolume(IntPtr handle)
		{
			uint num;
			return EjectDevice.DeviceIoControl(handle, 589856U, IntPtr.Zero, 0U, IntPtr.Zero, 0U, out num, IntPtr.Zero);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002DB8 File Offset: 0x00000FB8
		private static bool AutoEjectVolume(IntPtr handle)
		{
			uint num;
			return EjectDevice.DeviceIoControl(handle, 2967560U, IntPtr.Zero, 0U, IntPtr.Zero, 0U, out num, IntPtr.Zero);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002DE3 File Offset: 0x00000FE3
		private static bool CloseVolume(IntPtr handle)
		{
			return EjectDevice.CloseHandle(handle);
		}

		// Token: 0x04000033 RID: 51
		private IntPtr handle = IntPtr.Zero;

		// Token: 0x04000034 RID: 52
		private const uint GENERIC_READ = 2147483648U;

		// Token: 0x04000035 RID: 53
		private const uint GENERIC_WRITE = 1073741824U;

		// Token: 0x04000036 RID: 54
		private const int FILE_SHARE_READ = 1;

		// Token: 0x04000037 RID: 55
		private const int FILE_SHARE_WRITE = 2;

		// Token: 0x04000038 RID: 56
		private const int FSCTL_LOCK_VOLUME = 589848;

		// Token: 0x04000039 RID: 57
		private const int FSCTL_DISMOUNT_VOLUME = 589856;

		// Token: 0x0400003A RID: 58
		private const int IOCTL_STORAGE_EJECT_MEDIA = 2967560;

		// Token: 0x0400003B RID: 59
		private const int IOCTL_STORAGE_MEDIA_REMOVAL = 2967556;

		// Token: 0x0400003C RID: 60
		private const int OPEN_EXISTING = 3;
	}
}
