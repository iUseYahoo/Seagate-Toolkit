private List<StoragePoolDevice> FindPhysicalDiskSerialNumbers(string sn)
		{
			List<StoragePoolDevice> list = new List<StoragePoolDevice>();
			try
			{
				Log log = this._log;
				if (log != null)
				{
					log.I("Start to FindPhysicalDiskSerialNumbers, sn = " + sn, new object[0]);
				}
				List<string> list2 = new List<string>();
				List<string> list3 = new List<string>();
				if (string.IsNullOrEmpty(sn))
				{
					Log log2 = this._log;
					if (log2 != null)
					{
						log2.I("Finished FindPhysicalDiskSerialNumbers: The SerialNumber is empty;", new object[0]);
					}
					return list;
				}
				ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\Microsoft\\Windows\\Storage");
				try
				{
					Log log3 = this._log;
					if (log3 != null)
					{
						log3.I("Step_1: Start to find 'VirtualDisk->UniqueId' from 'Disk->sn = " + sn + "'", new object[0]);
					}
					ObjectQuery query = new ObjectQuery("SELECT * FROM MSFT_VirtualDiskToDisk");
					ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher(scope, query).Get();
					Log log4 = this._log;
					if (log4 != null)
					{
						log4.I(string.Format("Find MSFT_VirtualDiskToDisk.Count = {0}", (managementObjectCollection != null) ? new int?(managementObjectCollection.Count) : null), new object[0]);
					}
					foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						try
						{
							ManagementObject managementObject2 = new ManagementObject(managementObject["Disk"].ToString());
							ManagementObject managementObject3 = new ManagementObject(managementObject["VirtualDisk"].ToString());
							if (sn.Equals(managementObject2["SerialNumber"].ToString(), StringComparison.CurrentCultureIgnoreCase))
							{
								string text = managementObject3["UniqueId"].ToString();
								Log log5 = this._log;
								if (log5 != null)
								{
									log5.I("Find UniqueId for Virtual Disk List, uId = " + text, new object[0]);
								}
								if (!string.IsNullOrEmpty(text))
								{
									list2.Add(text);
									Log log6 = this._log;
									if (log6 != null)
									{
										log6.I("Add UniqueId to Virtual Disk List, uId = " + text, new object[0]);
									}
								}
							}
						}
						catch (Exception ex)
						{
							Log log7 = this._log;
							if (log7 != null)
							{
								log7.E("Exception on Step_1(Find 1 item on MSFT_VirtualDiskToDisk) ex: {0}", new object[]
								{
									ex.ToString()
								});
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Log log8 = this._log;
					if (log8 != null)
					{
						log8.E("Exception on Step_1(Find MSFT_VirtualDiskToDisk) ex: {0}", new object[]
						{
							ex2.ToString()
						});
					}
				}
				if (list2.Count <= 0)
				{
					Log log9 = this._log;
					if (log9 != null)
					{
						log9.I("Finished FindPhysicalDiskSerialNumbers: The listVirtualDiskUniqueIds.Count <= 0;", new object[0]);
					}
					return list;
				}
				try
				{
					Log log10 = this._log;
					if (log10 != null)
					{
						log10.I(string.Format("Step_2: using 'VirtualDisk->UniqueId' to find 'StoragePool->UniqueId'; listVirtualDiskUniqueIds.Count = {0}", list2.Count), new object[0]);
					}
					ObjectQuery query2 = new ObjectQuery("SELECT * FROM MSFT_StoragePoolToVirtualDisk");
					ManagementObjectCollection managementObjectCollection2 = new ManagementObjectSearcher(scope, query2).Get();
					Log log11 = this._log;
					if (log11 != null)
					{
						log11.I(string.Format("Find MSFT_StoragePoolToVirtualDisk.Count = {0}", (managementObjectCollection2 != null) ? new int?(managementObjectCollection2.Count) : null), new object[0]);
					}
					foreach (ManagementBaseObject managementBaseObject2 in managementObjectCollection2)
					{
						ManagementObject managementObject4 = (ManagementObject)managementBaseObject2;
						ManagementObject managementObject5 = new ManagementObject(managementObject4["StoragePool"].ToString());
						ManagementObject managementObject6 = new ManagementObject(managementObject4["VirtualDisk"].ToString());
						foreach (string text2 in list2)
						{
							try
							{
								if (text2.Equals(managementObject6["UniqueId"].ToString(), StringComparison.CurrentCultureIgnoreCase))
								{
									string text3 = managementObject5["UniqueId"].ToString();
									Log log12 = this._log;
									if (log12 != null)
									{
										log12.I("Find UniqueId for Storage Pool List, uId = " + text3, new object[0]);
									}
									if (!string.IsNullOrEmpty(text3))
									{
										list3.Add(text3);
										Log log13 = this._log;
										if (log13 != null)
										{
											log13.I("Add UniqueId to Storage Pool List, uId = " + text3, new object[0]);
										}
									}
								}
							}
							catch (Exception ex3)
							{
								Log log14 = this._log;
								if (log14 != null)
								{
									log14.E("Exception on Step_2(Find 1 item on MSFT_StoragePoolToVirtualDisk) ex: {0}", new object[]
									{
										ex3.ToString()
									});
								}
							}
						}
					}
				}
				catch (Exception ex4)
				{
					Log log15 = this._log;
					if (log15 != null)
					{
						log15.E("Exception on Step_2(Find MSFT_StoragePoolToVirtualDisk) ex: {0}", new object[]
						{
							ex4.ToString()
						});
					}
				}
				if (list3.Count <= 0)
				{
					Log log16 = this._log;
					if (log16 != null)
					{
						log16.I("Finished FindPhysicalDiskSerialNumbers: The listStoragePoolUniqueIds.Count <= 0;", new object[0]);
					}
					return list;
				}
				try
				{
					Log log17 = this._log;
					if (log17 != null)
					{
						log17.I(string.Format("Step_3: finally using 'StoragePool->UniqueId' to find all 'PhysicalDisk->SerialNumber'; listStoragePoolUniqueIds.Count = {0}", list3.Count), new object[0]);
					}
					ObjectQuery query3 = new ObjectQuery("SELECT * FROM MSFT_StoragePoolToPhysicalDisk");
					ManagementObjectCollection managementObjectCollection3 = new ManagementObjectSearcher(scope, query3).Get();
					Log log18 = this._log;
					if (log18 != null)
					{
						log18.I(string.Format("Find MSFT_StoragePoolToPhysicalDisk.Count = {0}", (managementObjectCollection3 != null) ? new int?(managementObjectCollection3.Count) : null), new object[0]);
					}
					foreach (ManagementBaseObject managementBaseObject3 in managementObjectCollection3)
					{
						ManagementObject managementObject7 = (ManagementObject)managementBaseObject3;
						ManagementObject managementObject8 = new ManagementObject(managementObject7["StoragePool"].ToString());
						ManagementObject managementObject9 = new ManagementObject(managementObject7["PhysicalDisk"].ToString());
						if (!(bool)managementObject8["IsPrimordial"])
						{
							foreach (string text4 in list3)
							{
								try
								{
									if (text4.Equals(managementObject8["UniqueId"].ToString(), StringComparison.CurrentCultureIgnoreCase))
									{
										string text5 = managementObject9["SerialNumber"].ToString();
										long size = 0L;
										long.TryParse(managementObject9["Size"].ToString(), out size);
										Log log19 = this._log;
										if (log19 != null)
										{
											log19.I("Add To Detected Serial Numbers: {0}", new object[]
											{
												text5
											});
										}
										list.Add(new StoragePoolDevice
										{
											Size = size,
											SerialNumber = text5
										});
									}
								}
								catch (Exception ex5)
								{
									Log log20 = this._log;
									if (log20 != null)
									{
										log20.E("Exception on Step_3(Find 1 item on MSFT_StoragePoolToPhysicalDisk) ex: {0}", new object[]
										{
											ex5.ToString()
										});
									}
								}
							}
						}
					}
				}
				catch (Exception ex6)
				{
					Log log21 = this._log;
					if (log21 != null)
					{
						log21.E("Exception on Step_3(Find MSFT_StoragePoolToPhysicalDisk) ex: {0}", new object[]
						{
							ex6.ToString()
						});
					}
				}
			}
			catch (Exception ex7)
			{
				Log log22 = this._log;
				if (log22 != null)
				{
					log22.E("Exception on MSStorageSpaceDetector->FindPhysicalDiskSerialNumbers; ex: {0}", new object[]
					{
						ex7.ToString()
					});
				}
			}
			return list;
		}
