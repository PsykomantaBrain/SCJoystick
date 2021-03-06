
using System;
using System.IO;

public class SCState
{

	public float _axis0;
	public float _axis1;
	public float _axis2;
	public float _axis3;
	public float _axis4;
	public float _axis5;
	public float _axis6;
	public float _axis7;
	public float _axis8;
	public float _axis9;
	public float _axis10;
	public float _axis11;
	public float _axis12;
	public float _axis13;
	public float _axis14;
	public float _axis15;

	public bool _buttonHome;
	public bool _buttonSettings;
	public bool _buttonRec;
	public bool _buttonTakeOff;
	public bool _buttonRTH;
	public bool _buttonPhoto;
	public bool _buttonThumbL;
	public bool _buttonThumbR;


	private byte buttons;

	public SCState()
	{

	}


	public void Serialize(BinaryWriter bw)
	{
		bw.Write(_axis0);
		bw.Write(_axis1);
		bw.Write(_axis2);
		bw.Write(_axis3);
		bw.Write(_axis4);
		bw.Write(_axis5);
		bw.Write(_axis6);
		bw.Write(_axis7);
		bw.Write(_axis8);
		bw.Write(_axis9);
		bw.Write(_axis10);
		bw.Write(_axis11);
		bw.Write(_axis12);
		bw.Write(_axis13);
		bw.Write(_axis14);
		bw.Write(_axis15);

		// put buttons in a single flag
		buttons = 0;
		buttons |= (byte)((_buttonHome ? 1 : 0) << 0);
		buttons |= (byte)((_buttonSettings ? 1 : 0) << 1);
		buttons |= (byte)((_buttonRec ? 1 : 0) << 2);
		buttons |= (byte)((_buttonTakeOff ? 1 : 0) << 3);
		buttons |= (byte)((_buttonRTH ? 1 : 0) << 4);
		buttons |= (byte)((_buttonPhoto ? 1 : 0) << 5);
		buttons |= (byte)((_buttonThumbL ? 1 : 0) << 6);
		buttons |= (byte)((_buttonThumbR ? 1 : 0) << 7);
		bw.Write(buttons);
	}

	public void Deserialize(byte[] data, int offset = 0)
	{
		_axis0 = BitConverter.ToSingle(data, offset);
		_axis1 = BitConverter.ToSingle(data, 4 + offset);
		_axis2 = BitConverter.ToSingle(data, 4 * 2 + offset);
		_axis3 = BitConverter.ToSingle(data, 4 * 3 + offset);
		_axis4 = BitConverter.ToSingle(data, 4 * 4 + offset);
		_axis5 = BitConverter.ToSingle(data, 4 * 5 + offset);
		_axis6 = BitConverter.ToSingle(data, 4 * 6 + offset);
		_axis7 = BitConverter.ToSingle(data, 4 * 7 + offset);
		_axis8 = BitConverter.ToSingle(data, 4 * 8 + offset);
		_axis9 = BitConverter.ToSingle(data, 4 * 9 + offset);
		_axis10 = BitConverter.ToSingle(data, 4 * 10 + offset);
		_axis11 = BitConverter.ToSingle(data, 4 * 11 + offset);
		_axis12 = BitConverter.ToSingle(data, 4 * 12 + offset);
		_axis13 = BitConverter.ToSingle(data, 4 * 13 + offset);
		_axis14 = BitConverter.ToSingle(data, 4 * 14 + offset);
		_axis15 = BitConverter.ToSingle(data, 4 * 15 + offset);

		buttons = data[data.Length - 1];
		_buttonHome = (buttons & 1 << 0) == 1;
		_buttonSettings = (buttons & 1 << 1) == 1;
		_buttonRec = (buttons & 1 << 2) == 1;
		_buttonTakeOff = (buttons & 1 << 3) == 1;
		_buttonRTH = (buttons & 1 << 4) == 1;
		_buttonPhoto = (buttons & 1 << 5) == 1;
		_buttonThumbL = (buttons & 1 << 6) == 1;
		_buttonThumbR = (buttons & 1 << 7) == 1;

	}
}