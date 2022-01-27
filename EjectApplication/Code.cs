using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using CommonUtils;
using eject.UsbEject;
using IModule;

namespace eject
{

	public class MainForm : Form
	{

		public MainForm(string id, string ejectType, string pnpDeivceID)
		{
			this.InitializeComponent();
			this._guid = id;
			this._ejectType = ejectType;
			this._pnpDeivceID = pnpDeivceID;
		}
    
		private void MainForm_Load(object sender, EventArgs e)
		{
			base.Left = -1;
			base.Top = -1;
			base.Width = 1;
			base.Height = 1;
			base.IsAccessible = false;
			if (this._ejectType == "MemoryCard")
			{
				try
				{
					bool result = EjectDevice.EjectMemoryCardByPNPDeviceID(this._pnpDeivceID);
					string data = JsonHelper.Serialize(new EjectResult
					{
						id = this._guid,
						result = result
					});
					using (WebClient webClient = new WebClient())
					{
						webClient.UploadString("http://127.0.0.1:19999/eject", "POST", data);
					}
				}
				catch (Exception)
				{
				}
			}
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(800, 450);
			base.Name = "MainForm";
			this.Text = "MainForm";
			base.Load += this.MainForm_Load;
			base.ResumeLayout(false);
		}
		public const string EJECT_MEMORYCARD = "MemoryCard";
		public const string EJECT_DEVICE = "Device";
		private string _guid;
		private string _ejectType;
		private string _pnpDeivceID;
		private System.ComponentModel.IContainer components;
	}
}
