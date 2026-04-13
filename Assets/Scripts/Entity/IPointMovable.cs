using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointMovable
{
    float MoveSpeed { get; set; }
    void MoveToPoint(Vector3 pos);
}
