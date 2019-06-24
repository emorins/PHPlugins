using UnityEngine;
using System;

namespace NormalRecalculate
{
    internal class RuntimeNormalRecalculate : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                renderers = FindObjectsOfType<SkinnedMeshRenderer>();
                foreach(SkinnedMeshRenderer render in renderers)
                {
                    //Console.WriteLine("SkinnedMeshRenderer: " + render.name);
                    if (render.name.ToLower().Contains("cf_o_hair"))
                    {
                        NormalSolver.RecalculateNormals(render.sharedMesh, 60f);
                        TangentSolver.Solve(render.sharedMesh);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.F8))
            {
                renderers = FindObjectsOfType<SkinnedMeshRenderer>();
                foreach (SkinnedMeshRenderer render in renderers)
                {
                    //Console.WriteLine("SkinnedMeshRenderer: " + render.name);
                    if (render.name.ToLower().Contains("cf_o_hair"))
                    {
                        render.sharedMesh.RecalculateNormals();
                        TangentSolver.Solve(render.sharedMesh);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                renderers = FindObjectsOfType<SkinnedMeshRenderer>();
                foreach (SkinnedMeshRenderer render in renderers)
                {
                    if (render.name.ToLower().Contains("cf_o_hair"))
                    {
                        TangentSolver.Solve(render.sharedMesh);
                    }
                }
            }

        }

        SkinnedMeshRenderer[] renderers;
    }
}