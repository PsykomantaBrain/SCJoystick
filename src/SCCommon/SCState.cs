
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
    }
    

    public void Deserialize(BinaryReader br)
    {
        _axis0 = br.ReadSingle();
        _axis1 = br.ReadSingle();
        _axis2 = br.ReadSingle();
        _axis3 = br.ReadSingle();
        _axis4 = br.ReadSingle();
        _axis5 = br.ReadSingle();
        _axis6 = br.ReadSingle();
        _axis7 = br.ReadSingle();
        _axis8 = br.ReadSingle();
        _axis9 = br.ReadSingle();
        _axis10 = br.ReadSingle();
        _axis11 = br.ReadSingle();
        _axis12 = br.ReadSingle();
        _axis13 = br.ReadSingle();
        _axis14 = br.ReadSingle();
        _axis15 = br.ReadSingle();
    }
}