using UnityEngine;
using MessagePack;

namespace BoneModHarmony
{
    [MessagePackObject(true)]
    public class BoneModifier
    {
        public BoneModifier(string boneName = "")
        {
            this.boneName = boneName;
        }

        [SerializationConstructor]
        public BoneModifier(string boneName, Vector3 Scale, Vector3 Rotation, Vector3 Position, bool isScale, bool isRotate, bool isPosition)
        {
            this.boneName = boneName;
            this.Scale = Scale;
            this.Rotation = Rotation;
            this.Position = Position;
            this.isScale = isScale;
            this.isRotate = isRotate;
            this.isPosition = isPosition;
        }

        public void PasteValue(BoneModifier modifier)
        {
            Scale = modifier.Scale;
            Rotation = modifier.Rotation;
            Position = modifier.Position;
            isScale = modifier.isScale;
            isRotate = modifier.isRotate;
            isPosition = modifier.isPosition;
        }
        public BoneModifier Clone()
        {
            return (BoneModifier)MemberwiseClone();
        }
        public string boneName;
        public Vector3 Scale = Vector3.one;
        public Vector3 Rotation = Vector3.zero;
        public Vector3 Position = Vector3.zero;
        public bool isScale = false;
        public bool isRotate = false;
        public bool isPosition = false;
    }
}
