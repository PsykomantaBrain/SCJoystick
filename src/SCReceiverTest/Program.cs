using System;
using System.Net;
using System.Net.Sockets;

using vJoyInterfaceWrap;

namespace SCReceiverTest
{
    class Program
    {

        static int _axisRange = 16384;
        static vJoy joystick;
        static SCState deviceState;
        static uint id = 1;

        static void Main(string[] args)
        {
            Console.WriteLine("Setting up virtual device...");

            joystick = new vJoy();
                        

            // Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    Console.WriteLine("vJoy Device {0} is already owned by this feeder\n", id);
                    break;
                case VjdStat.VJD_STAT_FREE:
                    Console.WriteLine("vJoy Device {0} is free\n", id);
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    Console.WriteLine("vJoy Device {0} is already owned by another feeder\nCannot continue\n", id);
                    return;
                    joystick.ResetVJD(id);
                case VjdStat.VJD_STAT_MISS:
                    Console.WriteLine("vJoy Device {0} is not installed or disabled\nCannot continue\n", id);
                    return;
                default:
                    Console.WriteLine("vJoy Device {0} general error\nCannot continue\n", id);
                    return;
            }

            if (!(joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X) &&
                  joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y) &&
                  joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX) &&
                  joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RY) &&
                  joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Z) &&
                  joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ) &&
                  joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_SL0) &&
                  joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_SL1)
                    ))
            {
                Console.WriteLine("VJoy Device " + id + " does not support enough axes for SkyController. Make sure device has at least 8 axes");
                return;
            }

            // Test if DLL matches the driver
            UInt32 DllVer = 0, DrvVer = 0;
            bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (match)
                Console.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", DllVer);
            else
                Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);


            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
            {
                Console.WriteLine("Failed to acquire vJoy device number {0}.\n", id);
                return;
            }
            else
                Console.WriteLine("Acquired: vJoy device number {0}.\n", id);


            deviceState = new SCState();


            Console.WriteLine("Starting net receiver...");
            UdpClient listener = new UdpClient(8899);
            IPEndPoint ep = null;
            byte[] data = new byte[128];

            listener.JoinMulticastGroup(IPAddress.Parse("230.0.0.1"));
            Console.WriteLine("Waiting for data on 230.0.0.1:8899...");

            data = listener.Receive(ref ep);
            Console.WriteLine("Receiving from " + ep.Address.ToString() + ":" + ep.Port.ToString() + "...");

            joystick.ResetVJD(id);

            int CursorRow = Console.CursorTop;
            uint pId = 0;

            long axisMax = 0;
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref axisMax);

            while (true)
            {
                //System.Threading.Thread.Sleep(10);

                data = listener.Receive(ref ep);
                pId = BitConverter.ToUInt32(data, 0);

                deviceState.Deserialize(data, 4);

                Console.CursorLeft = 0;
                Console.CursorTop = CursorRow;

                Console.WriteLine("Received " + data.Length + " bytes. Packet ID: " + pId);
                //Console.WriteLine("Yaw: " + deviceState._axis0.ToString("0.00"));
                //Console.WriteLine("Gaz: " + deviceState._axis1.ToString("0.00"));
                //Console.WriteLine("Roll: " + deviceState._axis13.ToString("0.00"));
                //Console.WriteLine("Pitch: " + deviceState._axis2.ToString("0.00"));
                //Console.WriteLine("LThumb X: " + deviceState._axis14.ToString("0.00"));
                //Console.WriteLine("LThumb Y: " + deviceState._axis15.ToString("0.00"));
                //Console.WriteLine("RThumb X: " + deviceState._axis8.ToString("0.00"));
                //Console.WriteLine("RThumb Y: " + deviceState._axis9.ToString("0.00"));
                Console.WriteLine("Battery: " + (deviceState._axis10 * 100f).ToString("0.00") + "%");
                                

                joystick.SetAxis((int)(deviceState._axis0 * _axisRange + _axisRange), id, HID_USAGES.HID_USAGE_X);
                joystick.SetAxis((int)(-deviceState._axis1 * _axisRange + _axisRange), id, HID_USAGES.HID_USAGE_Y);
                joystick.SetAxis((int)(deviceState._axis13 * _axisRange + _axisRange), id, HID_USAGES.HID_USAGE_RX);
                joystick.SetAxis((int)(deviceState._axis2 * _axisRange + _axisRange), id, HID_USAGES.HID_USAGE_RY);
                joystick.SetAxis((int)(deviceState._axis14 * _axisRange + _axisRange), id, HID_USAGES.HID_USAGE_Z);
                joystick.SetAxis((int)(deviceState._axis15 * _axisRange + _axisRange), id, HID_USAGES.HID_USAGE_RZ);
                joystick.SetAxis((int)(deviceState._axis8 * _axisRange + _axisRange), id, HID_USAGES.HID_USAGE_SL0);
                joystick.SetAxis((int)(deviceState._axis9 * _axisRange + _axisRange), id, HID_USAGES.HID_USAGE_SL1);

                
            }
        }
    }
}
