		private IntPtr _hDevInfo;

    private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);
    
    private struct SP_DEVINFO_DATA
		{
			// Token: 0x0400029E RID: 670
			public int cbSize;

			// Token: 0x0400029F RID: 671
			public Guid ClassGuid;

			// Token: 0x040002A0 RID: 672
			public uint DevInst;

			// Token: 0x040002A1 RID: 673
			public IntPtr Reserved;
		}

		public void Dispose()
		{
			if (this._hDevInfo != IntPtr.Zero)
			{
				Device.SetupDiDestroyDeviceInfoList(this._hDevInfo);
				this._hDevInfo = IntPtr.Zero;
			}
		}
