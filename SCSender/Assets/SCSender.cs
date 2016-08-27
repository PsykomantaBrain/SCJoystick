using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;

public class SCSender : MonoBehaviour
{

    public Text deviceInfo;
    public Text netInfo;

    public SCState deviceState = new SCState();



    private UdpClient sender;
    private BinaryWriter br;
    private MemoryStream dataStream;

    private uint packetID = 0;
    private void Start()
    {
        sender = new UdpClient("230.0.0.1", 8899);


        dataStream = new MemoryStream();
        br = new BinaryWriter(dataStream);
    }
    private void Update()
    {
        if (deviceState != null)
        {
            deviceState._axis1 = Input.GetAxis("axis0");
            deviceState._axis0 = Input.GetAxis("axis1");
            deviceState._axis3 = Input.GetAxis("axis2");
            deviceState._axis2 = Input.GetAxis("axis3");
            deviceState._axis4 = Input.GetAxis("axis4");
            deviceState._axis5 = Input.GetAxis("axis5");
            deviceState._axis6 = Input.GetAxis("axis6");
            deviceState._axis7 = Input.GetAxis("axis7");
            deviceState._axis8 = Input.GetAxis("axis8");
            deviceState._axis9 = Input.GetAxis("axis9");
            deviceState._axis10 = Input.GetAxis("axis10");
            deviceState._axis11 = Input.GetAxis("axis11");
            deviceState._axis12 = Input.GetAxis("axis12");
            deviceState._axis13 = Input.GetAxis("axis13");
            deviceState._axis14 = Input.GetAxis("axis14");
            deviceState._axis15 = Input.GetAxis("axis15");


            if (deviceInfo != null)
            {
                deviceInfo.text = string.Format("Device Info:\n\n" +
                     "Yaw: {0:0.00}\nGaz: {1:0.00}\nPitch: {2:0.00}\nAxis 3: {3:0.00}\n" +
                     "Horizontal: {4:0.00}\nVertical: {5:0.00}\nAxis 6: {6:0.00}\nAxis 7: {7:0.00}\n" +
                     "ThumbR X: {8:0.00}\nThumbR Y: {9:0.00}\nBattery: {10:0.00}\nAxis 11: {11:0.00}\n" +
                     "Axis 12: {12:0.00}\nRoll: {13:0.00}\nAxis 14: {14:0.00}\nAxis 15: {15:0.00}",
                     deviceState._axis1, deviceState._axis0, deviceState._axis3, deviceState._axis2,
                     deviceState._axis4, deviceState._axis5, deviceState._axis6, deviceState._axis7,
                     deviceState._axis8, deviceState._axis9, deviceState._axis10, deviceState._axis11,
                     deviceState._axis12, deviceState._axis13, deviceState._axis14, deviceState._axis15
                     );
            }


            if (sender != null)
            {
                dataStream.Position = 0;
                br.Write(packetID);

                deviceState.Serialize(br);

                packetID = (packetID + 1);// % 0xFFFFFFFF;

                if (netInfo != null)
                {
                    netInfo.text = "Connection Info:\n\n" +
                                    "Buffer Size: " + dataStream.Length + "bytes\n" +
                                    "Packet ID: " + packetID;

                }

                sender.Send(dataStream.GetBuffer(), (int)dataStream.Length);
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


    }
}
