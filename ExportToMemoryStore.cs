[SecurityCritical]
		internal static SafeCertStoreHandle ExportToMemoryStore(X509Certificate2Collection collection, X509Certificate2Collection collection2 = null)
		{
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AllFlags);
			storePermission.Assert();
			SafeCertStoreHandle safeCertStoreHandle = CAPI.CertOpenStore(new IntPtr(2L), 65537U, IntPtr.Zero, 8704U, null);
			if (safeCertStoreHandle == null || safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			X509Utils.AddToStore(safeCertStoreHandle, collection);
			if (collection2 != null)
			{
				X509Utils.AddToStore(safeCertStoreHandle, collection2);
			}
			return safeCertStoreHandle;
		}
