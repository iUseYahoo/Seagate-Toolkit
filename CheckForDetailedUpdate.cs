		public UpdateCheckInfo CheckForDetailedUpdate(bool persistUpdateCheckResult)
		{
			new NamedPermissionSet("FullTrust").Demand();
			if (Interlocked.CompareExchange(ref this._guard, 2, 0) != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SingleOperation"));
			}
			this._cancellationPending = false;
			UpdateCheckInfo updateCheckInfo = null;
			try
			{
				DeploymentManager deploymentManager = this.CreateDeploymentManager();
				try
				{
					deploymentManager.Bind();
					updateCheckInfo = this.DetermineUpdateCheckResult(deploymentManager.ActivationDescription);
					if (updateCheckInfo.UpdateAvailable)
					{
						deploymentManager.DeterminePlatformRequirements();
						try
						{
							deploymentManager.DetermineTrust(new TrustParams
							{
								NoPrompt = true
							});
						}
						catch (TrustNotGrantedException)
						{
							if (!deploymentManager.ActivationDescription.IsUpdateInPKTGroup)
							{
								throw;
							}
						}
					}
					if (persistUpdateCheckResult)
					{
						this.ProcessUpdateCheckResult(updateCheckInfo, deploymentManager.ActivationDescription);
					}
				}
				finally
				{
					deploymentManager.Dispose();
				}
			}
			finally
			{
				Interlocked.Exchange(ref this._guard, 0);
			}
			return updateCheckInfo;
		}
