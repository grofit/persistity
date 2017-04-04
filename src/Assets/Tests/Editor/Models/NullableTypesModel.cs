using Persistity.Attributes;
using UnityEngine;

namespace Tests.Editor.Models
{
    [Persist]
    public class NullableTypesModel
    {
        [PersistData]
        public int? NullableInt { get; set; }

        [PersistData]
        public float? NullableFloat { get; set; }

        [PersistData]
        public Vector3? NullableVector3 { get; set; }
    }
}