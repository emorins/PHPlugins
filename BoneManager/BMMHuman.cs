using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;
using System.Text;
namespace BoneModHarmony
{
    public class Bone
    {
        public Transform transform;
        public int ID;
        public Bone(Transform t)
        {
            transform = t;
            ID = Animator.StringToHash(t.name);
        }
    }
    public class BMMHuman : MonoBehaviour
    {
        internal Human human;
        public string Name;
        internal List<Bone> targetBones = new List<Bone>();

        internal Dictionary<int, BoneModifier> boneModifiers;
        Dictionary<int, BoneModifier> originalBones = new Dictionary<int, BoneModifier>();
        //internal bool shapeApplied = true;
        public bool reset = false;

        void Awake()
        {
            human = GetComponent<Human>();            
        }
        void Start()
        {
            _UpdateCache();
            OnShapeApplied();
            //shapeApplied = true;
        }
        void OnEnable()
        {
            BoneManager.ModedCharas.Add(this);            
            //StopAllCoroutines();
            //StartCoroutine(BoneOverride());
        }
        void OnDisable()
        {
            BoneManager.ModedCharas.Remove(this);
        }
        void Update()
        {
            if (reset)
            {
                ResetBoneModifiers();
                Console.WriteLine("Reset Bones on: " + Name);
                _UpdateCache();
                reset = false;
                OnShapeApplied();                    
            }
        }
        public void SaveProfile()
        {            
            if(Prefs.SaveFormat == SaveFormat.Compressed)
                File.WriteAllBytes(Prefs.GetCharaPathMsgPack(Name, human.sex), LZ4MessagePackSerializer.Serialize(boneModifiers));
            else
                File.WriteAllBytes(Prefs.GetCharaPathMsgPack(Name, human.sex), MessagePackSerializer.Serialize(boneModifiers));
        }

        //IEnumerator BoneOverride()
        //{
        //    while (true)
        //    {
        //        yield return null;
        //        if (shapeApplied)
        //        {
        //            OnShapeApplied();
        //            shapeApplied = false;
        //        }                
        //    }
        //}

        public void OnShapeApplied()
        {
            foreach (var Bone in targetBones)
            {
                ApplyBoneModifier(Bone);
            }            
        }

        public void RemoveBoneModifier(Bone bone)
        {
            var boneModifier = boneModifiers[bone.ID];
            var originalBone = originalBones[bone.ID];
            bone.transform.localScale = originalBone.Scale;
            bone.transform.localEulerAngles = originalBone.Rotation;
            bone.transform.localPosition = originalBone.Position;
            boneModifiers.Remove(bone.ID);
            originalBones.Remove(bone.ID);
            targetBones.Remove(bone);
        }
        public void AddBoneModifier(string bonename)
        {
            if (boneModifiers.ContainsKey(Animator.StringToHash(bonename)))
                return;
            var t = Transform_Utility.FindTransform(human.body.AnimatedBoneRoot,bonename);
            if (t != null)
            {
                var bone = new Bone(t);
                targetBones.Add(bone);
                BoneModifier boneModifer = new BoneModifier(bonename)
                {
                    Scale = bone.transform.localScale,
                    Rotation = bone.transform.localEulerAngles,
                    Position = bone.transform.localPosition
                };
                originalBones.Add(bone.ID, boneModifer);                
                boneModifiers.Add(bone.ID, new BoneModifier(bonename));
                Console.WriteLine(Name + " has a new bone to modify: " + bonename);
            }
        }
        public void ApplyBoneModifier(Bone bone)
        {
            var boneModifier = boneModifiers[bone.ID];
            var originalBone = originalBones[bone.ID];
            if (boneModifier.isScale)
                bone.transform.localScale = Vector3.Scale(originalBone.Scale, boneModifier.Scale);
            else
                bone.transform.localScale = originalBone.Scale;
            if (boneModifier.isRotate)
                bone.transform.localEulerAngles = originalBone.Rotation + boneModifier.Rotation;
            else
                bone.transform.localEulerAngles = originalBone.Rotation;
            if (boneModifier.isPosition)
                bone.transform.localPosition = originalBone.Position + boneModifier.Position;
            else
                bone.transform.localPosition = originalBone.Position;
        }

        public void ResetBoneModifiers(bool isRotation = true, bool isPosition = true, bool isScale = true)
        {
            foreach (var Bone in targetBones)
            {
                if (isScale && boneModifiers[Bone.ID].isScale)
                    Bone.transform.localScale = originalBones[Bone.ID].Scale;
                if (isRotation && boneModifiers[Bone.ID].isRotate)
                    Bone.transform.localEulerAngles = originalBones[Bone.ID].Rotation;
                if (isPosition && boneModifiers[Bone.ID].isPosition)
                    Bone.transform.localPosition = originalBones[Bone.ID].Position;
            }
        }

        private void _UpdateCache()
        {
            LoadBoneModifers();
            targetBones.Clear();
            originalBones.Clear();
            var transforms = human.body.AnimatedBoneRoot.GetComponentsInChildren<Transform>();
            foreach (var transform in transforms)
            {
                if (transform.name != null && boneModifiers.ContainsKey(Animator.StringToHash(transform.name)))
                {
                    var Bone = new Bone(transform);
                    targetBones.Add(Bone);
                    BoneModifier boneModifer = new BoneModifier(transform.name)
                    {
                        Scale = Bone.transform.localScale,
                        Rotation = Bone.transform.localEulerAngles,
                        Position = Bone.transform.localPosition
                    };
                    originalBones.Add(Bone.ID, boneModifer);
                    Console.WriteLine(Name + "'s Bone to modify: " + transform.name);
                }
            }
        }

        private void LoadBoneModifers()
        {
            if (File.Exists(Prefs.GetCharaPathMsgPack(Name, human.sex)))
            {
                var bin = File.ReadAllBytes(Prefs.GetCharaPathMsgPack(Name, human.sex));
                boneModifiers = LZ4MessagePackSerializer.Deserialize<Dictionary<int, BoneModifier>>(bin);
                Console.WriteLine("Use bonemod msgpack profile");
            }
            else
            {
                boneModifiers = Prefs.LoadBoneModifiers(Name, human.sex);
                Console.WriteLine("Use bmm txt profile");
            }
        }
    }
}
