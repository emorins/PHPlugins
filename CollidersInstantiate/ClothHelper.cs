using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ClothHelper
{
    public enum UseDynamicBoneColliders
    {
        Disable,
        Enable,
        Auto
    }

    [RequireComponent(typeof(Cloth))]
    public class PlayhomeClothHelper : MonoBehaviour
    {
     
        public UseDynamicBoneColliders useDynamicBoneColliders;
        Cloth cloth;
        Human human;

        void Awake()
        {
            cloth = GetComponent<Cloth>();
            if (Application.isEditor)
            {                
                print("We are running this from inside of the editor!");
                enabled = false;
            }
        }

        void Start()
        {
            human = GetComponentInParent<Human>();
            if (human == null)
            {
                Console.WriteLine("Cloth is not attach to a character!");
                enabled = false;
                return;
            }
            SetupClothColliders();
            cloth.ClearTransformMotion();
        }
        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.T))
        //    {
        //        cloth.enabled = !cloth.enabled;
        //    }
        //}



        void SetupClothColliders()
        {
            var root = human.body.AnimatedBoneRoot;
            Console.WriteLine("Found animated bone root: " + root.name);
            
            if (cloth.capsuleColliders.Length == 0 && cloth.sphereColliders.Length == 0 && useDynamicBoneColliders == UseDynamicBoneColliders.Auto ||
                useDynamicBoneColliders == UseDynamicBoneColliders.Enable)
            {
                cloth.capsuleColliders = ConvertDBColliders(root).ToArray();
                cloth.sphereColliders = null;
            }
            else
            {
                SetupColliders(root, cloth, out List<CapsuleCollider> newCapsules, out List<ClothSphereColliderPair> newSpheres);
                cloth.capsuleColliders = newCapsules.ToArray();
                cloth.sphereColliders = newSpheres.ToArray();
            }

        }

        internal static void SetupColliders(Transform root, Cloth cloth, out List<CapsuleCollider> newCapsules, out List<ClothSphereColliderPair> newSpheres)
        {
            newCapsules = new List<CapsuleCollider>();
            newSpheres = new List<ClothSphereColliderPair>();
            foreach (var cc in cloth.capsuleColliders)
            {
                var newTransform = Transform_Utility.FindTransform(root, cc.transform.name);
                if (newTransform == null)
                {
                    if (cc.transform.parent == null)
                        continue;
                    newTransform = Transform_Utility.FindTransform(root, cc.transform.parent.name);
                    if (newTransform == null)
                        continue;
                }
                var newcc = Instantiate(cc, newTransform, false);
                Destroy(cc);
                newCapsules.Add(newcc);
                
                //cc.enabled = false;
                Console.WriteLine("Add new Capsule collider on: " + newcc.name);

            }
            foreach (var sc in cloth.sphereColliders)
            {
                if (sc.first != null)
                {
                    Console.WriteLine("1st sphereColliders: " + sc.first.transform.name);
                    var newTransform1 = Transform_Utility.FindTransform(root, sc.first.transform.name);
                    var newscp = new ClothSphereColliderPair();
                    if (newTransform1 == null)
                    {
                        if (sc.first.transform.parent == null)
                            continue;
                        newTransform1 = Transform_Utility.FindTransform(root, sc.first.transform.parent.name);
                        if (newTransform1 == null)
                            continue;
                    }
                    newscp.first = Instantiate(sc.first, newTransform1, false);
                    Destroy(sc.first);
                    Console.WriteLine("Add new Sphere collider on: " + newscp.first.name);

                    if (sc.second != null)
                    {
                        Console.WriteLine("2nd sphereColliders: " + sc.second.transform.name);
                        var newTransform2 = Transform_Utility.FindTransform(root, sc.second.transform.name);
                        if (newTransform2 == null)
                        {
                            if (sc.second.transform.parent == null)
                                goto addnew;
                            newTransform2 = Transform_Utility.FindTransform(root, sc.second.transform.parent.name);
                            if (newTransform2 == null)
                                goto addnew;
                        }
                        newscp.second = Instantiate(sc.second, newTransform2, false);
                        Destroy(sc.second);
                        Console.WriteLine("Add new Sphere collider on: " + newscp.second.name);
                    }
                    addnew:
                    newSpheres.Add(newscp);
                }
            }
        }

        internal static List<CapsuleCollider> ConvertDBColliders(Transform root)
        {
            var dynamicBoneColliders = root.GetComponentsInChildren<DynamicBoneCollider>(true);
            var Capsules = new List<CapsuleCollider>();
            foreach (var item in dynamicBoneColliders)
            {
                var capsule = item.GetOrAddComponent<CapsuleCollider>();
                capsule.height = item.m_Height;
                capsule.radius = item.m_Radius;
                capsule.direction = (int)item.m_Direction;
                capsule.center = item.m_Center;
                Capsules.Add(capsule);
            }
            return Capsules;
        }
    }
}
