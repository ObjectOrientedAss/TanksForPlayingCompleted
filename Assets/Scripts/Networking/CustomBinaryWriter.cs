using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CustomBinaryWriter : BinaryWriter
{
    public CustomBinaryWriter(MemoryStream ms) : base(ms)
    {

    }

    public void Write(Vector3 vector)
    {
        Write(vector.x);
        Write(vector.y);
        Write(vector.z);
    }

    public void Write(Quaternion quaternion)
    {
        Write(quaternion.x);
        Write(quaternion.y);
        Write(quaternion.z);
        Write(quaternion.w);
    }
}
