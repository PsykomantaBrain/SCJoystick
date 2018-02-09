using System;
using System.Collections.Generic;
using System.ComponentModel;
using FreePIE.Core.Contracts;
using FreePIE.Core.Plugins.Globals;
using FreePIE.Core.Plugins.Strategies;
using System.Net;
using System.Net.Sockets;

namespace FreePIE.Core.Plugins
{

	[GlobalType(Type = typeof(SkyControllerPlugin), IsIndexed = false)]
	public class SkyControllerPlugin : Plugin
	{
		public override string FriendlyName
		{
			get
			{
				return "Parrot SkyController";
			}
		}

		public override object CreateGlobal()
		{
			return new SkyControllerGlobal(this);
		}
	}

	public class SkyControllerGlobal : UpdateblePluginGlobal<SkyControllerPlugin>
	{

		private UdpClient listener = new UdpClient(8899);
		private IPEndPoint ep = null;

		private byte[] data = new byte[128];
		private uint pId = 0;



		protected SCState deviceState;
		public SkyControllerGlobal(SkyControllerPlugin plugin) : base(plugin)
		{
			Console.WriteLine("Starting net receiver...");

			listener.JoinMulticastGroup(IPAddress.Parse("230.0.0.1"));
			Console.WriteLine("Waiting for data on 230.0.0.1:8899...");

			data = listener.Receive(ref ep);
			Console.WriteLine("Receiving from " + ep.Address.ToString() + ":" + ep.Port.ToString() + "...");

			deviceState = new SCState();
		}

		internal void Update()
		{
			data = listener.Receive(ref ep);
			pId = BitConverter.ToUInt32(data, 0);
			deviceState.Deserialize(data, 4);
		}

		public double LeftStickX
		{
			get { return deviceState._axis0; }
		}

		public double LeftStickY
		{
			get { return deviceState._axis1; }
		}
		public double RightStickX
		{
			get { return deviceState._axis13; }
		}
		public double RightStickY
		{
			get { return deviceState._axis2; }
		}

		public double LThumbX
		{
			get { return deviceState._axis14; }
		}
		public double LThumbY
		{
			get { return deviceState._axis15; }
		}
		public double RThumbX
		{
			get { return deviceState._axis8; }
		}
		public double RThumbY
		{
			get { return deviceState._axis7; }
		}


		//Console.WriteLine("Yaw: " + deviceState._axis0.ToString("0.00"));
		//Console.WriteLine("Gaz: " + deviceState._axis1.ToString("0.00"));
		//Console.WriteLine("Roll: " + deviceState._axis13.ToString("0.00"));
		//Console.WriteLine("Pitch: " + deviceState._axis2.ToString("0.00"));
		//Console.WriteLine("LThumb X: " + deviceState._axis14.ToString("0.00"));
		//Console.WriteLine("LThumb Y: " + deviceState._axis15.ToString("0.00"));
		//Console.WriteLine("RThumb X: " + deviceState._axis8.ToString("0.00"));
		//Console.WriteLine("RThumb Y: " + deviceState._axis9.ToString("0.00"));
	}
}