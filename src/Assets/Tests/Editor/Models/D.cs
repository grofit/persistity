using System;
using Persistity.Attributes;
using UnityEngine;

namespace Tests.Editor.Models
{
    public class D
    {
        [PersistData] public byte ByteValue { get; set; }
        [PersistData] public short ShortValue { get; set; }
        [PersistData] public int IntValue { get; set; }
        [PersistData] public long LongValue { get; set; }
        [PersistData] public Guid GuidValue { get; set; }
        [PersistData] public DateTime DateTimeValue { get; set; }
        [PersistData] public Vector2 Vector2Value { get; set; }
        [PersistData] public Vector3 Vector3Value { get; set; }
        [PersistData] public Vector4 Vector4Value { get; set; }
        [PersistData] public Quaternion QuaternionValue { get; set; }
        [PersistData] public SomeTypes SomeType { get; set; }
    }
}