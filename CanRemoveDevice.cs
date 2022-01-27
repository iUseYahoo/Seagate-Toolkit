private bool CanRemoveDrive(PSDriveInfo drive, CmdletProviderContext context)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			SessionStateInternal.tracer.WriteLine("Drive name = {0}", new object[]
			{
				drive.Name
			});
			context.Drive = drive;
			DriveCmdletProvider driveProviderInstance = this.GetDriveProviderInstance(drive.Provider);
			bool flag = false;
			PSDriveInfo psdriveInfo = null;
			try
			{
				psdriveInfo = driveProviderInstance.RemoveDrive(drive, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RemoveDriveProviderException", SessionStateStrings.RemoveDriveProviderException, driveProviderInstance.ProviderInfo, null, e);
			}
			if (psdriveInfo != null && string.Compare(psdriveInfo.Name, drive.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
			{
				flag = true;
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}
