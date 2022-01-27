using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200045A RID: 1114
	internal sealed class SafeCertStoreHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600296D RID: 10605 RVA: 0x000BC1C8 File Offset: 0x000BA3C8
		private SafeCertStoreHandle() : base(true)
		{
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000BC1D1 File Offset: 0x000BA3D1
		internal SafeCertStoreHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x000BC1E4 File Offset: 0x000BA3E4
		internal static SafeCertStoreHandle InvalidHandle
		{
			get
			{
				SafeCertStoreHandle safeCertStoreHandle = new SafeCertStoreHandle(IntPtr.Zero);
				GC.SuppressFinalize(safeCertStoreHandle);
				return safeCertStoreHandle;
			}
		}

		// Token: 0x06002970 RID: 10608
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CertCloseStore(IntPtr hCertStore, uint dwFlags);

		// Token: 0x06002971 RID: 10609 RVA: 0x000BC203 File Offset: 0x000BA403
		protected override bool ReleaseHandle()
		{
			return SafeCertStoreHandle.CertCloseStore(this.handle, 0U);
		}
	}
}
