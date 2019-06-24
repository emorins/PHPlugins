using System;
using System.Collections;
using UnityEngine;

namespace PlayHomeMirrorHelper
{

    [ExecuteInEditMode]
    public class MirrorReflectionPlus : MirrorReflection
    {

        public override void OnWillRenderObject()
        {
            Renderer component = GetComponent<Renderer>();
            if (!enabled || !component || !component.sharedMaterial || !component.enabled)
            {
                return;
            }
            Camera current = Camera.current;
            if (!current)
            {
                return;
            }
            if (s_InsideRendering)
            {
                return;
            }
            s_InsideRendering = true;
            CreateMirrorObjects(current, out Camera camera);
            Vector3 position = transform.position;
            Vector3 up = transform.up;

            UpdateCameraModes(current, camera);  
            //camera.renderingPath = current.actualRenderingPath;

            float w = -Vector3.Dot(up, position) - m_ClipPlaneOffset;
            Vector4 plane = new Vector4(up.x, up.y, up.z, w);
            Matrix4x4 zero = Matrix4x4.zero;
            CalculateReflectionMatrix(ref zero, plane);
            Vector3 position2 = current.transform.position;
            Vector3 position3 = zero.MultiplyPoint(position2);
            camera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
            Vector4 clipPlane = CameraSpacePlane(camera, position, up, 1f);
            Matrix4x4 projectionMatrix = current.CalculateObliqueMatrix(clipPlane);
            camera.projectionMatrix = projectionMatrix;
            camera.cullingMask = (-17 & m_ReflectLayers.value);
            camera.targetTexture = m_ReflectionTexture;
            GL.invertCulling = true;
            camera.transform.position = position3;
            Vector3 eulerAngles = current.transform.eulerAngles;
            camera.transform.eulerAngles = new Vector3(0f, eulerAngles.y, eulerAngles.z);
            camera.Render();
            camera.transform.position = position2;
            GL.invertCulling = false;
            Material[] sharedMaterials = component.sharedMaterials;
            foreach (Material material in sharedMaterials)
            {
                if (material.HasProperty("_ReflectionTex"))
                {
                    material.SetTexture("_ReflectionTex", m_ReflectionTexture);
                }
            }
            s_InsideRendering = false;
        }

        public override void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera)
        {
            reflectionCamera = null;
            m_DisablePixelLights = false;
            if (!m_ReflectionTexture || m_OldReflectionTextureSize != m_TextureSize)
            {
                if (m_ReflectionTexture)
                {
                    DestroyImmediate(m_ReflectionTexture);
                }
                m_ReflectionTexture = new RenderTexture(m_TextureSize, m_TextureSize, 32, RenderTextureFormat.ARGBHalf)
                {
                    name = "__MirrorReflection" + GetInstanceID(),
                    isPowerOfTwo = true,
                    hideFlags = HideFlags.DontSave
                };
                m_OldReflectionTextureSize = m_TextureSize;
            }
            reflectionCamera = (m_ReflectionCameras[currentCamera] as Camera);
            if (!reflectionCamera)
            {
                GameObject gameObject;
                if (cameraOriginal == null)
                {
                    gameObject = new GameObject(string.Concat(new object[]
                    {
                    "Mirror Refl Camera id",
                    GetInstanceID(),
                    " for ",
                    currentCamera.GetInstanceID()
                    }), new Type[]
                    {
                    typeof(Camera),
                    typeof(Skybox)
                    });
                    gameObject.AddComponent<FlareLayer>();
                }
                else
                {
                    gameObject = Instantiate(cameraOriginal.gameObject);
                }
                reflectionCamera = gameObject.GetComponent<Camera>();
                reflectionCamera.hdr = true;
                reflectionCamera.enabled = false;
                reflectionCamera.transform.position = transform.position;
                reflectionCamera.transform.rotation = transform.rotation;
                gameObject.hideFlags = HideFlags.HideAndDontSave;
                m_ReflectionCameras[currentCamera] = reflectionCamera;
            }
        }

        private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
        {
            reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
            reflectionMat.m01 = -2f * plane[0] * plane[1];
            reflectionMat.m02 = -2f * plane[0] * plane[2];
            reflectionMat.m03 = -2f * plane[3] * plane[0];
            reflectionMat.m10 = -2f * plane[1] * plane[0];
            reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
            reflectionMat.m12 = -2f * plane[1] * plane[2];
            reflectionMat.m13 = -2f * plane[3] * plane[1];
            reflectionMat.m20 = -2f * plane[2] * plane[0];
            reflectionMat.m21 = -2f * plane[2] * plane[1];
            reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
            reflectionMat.m23 = -2f * plane[3] * plane[2];
            reflectionMat.m30 = 0f;
            reflectionMat.m31 = 0f;
            reflectionMat.m32 = 0f;
            reflectionMat.m33 = 1f;
        }
    }
}
