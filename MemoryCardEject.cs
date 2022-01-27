		public void MemoryCardEject(string memorycard_type, string sn, int pid, int vid)
		{
			this._log.I("Memory Card Eject event", new object[0]);
			this.AddUploadTask(new PayloadMemoryCardEject(this.currentSessionId)
			{
				MemoryCardType = memorycard_type,
				SerialNumber = sn,
				PID = string.Format("0x{0:X4}", pid),
				VID = string.Format("0x{0:X4}", vid)
			});
			this.SetTaskEvent();
		}
