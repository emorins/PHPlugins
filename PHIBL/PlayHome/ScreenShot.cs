using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace PHIBL
{
    class ScreenShot:Singleton<ScreenShot>
    {
        class Screen
        {
            internal static int width;
            internal static int height;
        }
        void OnEnable()
        {
            StartCoroutine(Shoot());
        }

        IEnumerator Shoot()
        {
            while (true)
            {
                yield return null;
                if (Capture)
                {
                    Capture = false;
                    float mul;
                    switch (PHIBL.screenShotSize)
                    {
                        default:
                        case ScreenShotSize.Original:
                            mul = 1f;
                            break;
                        case ScreenShotSize.FourThirds:
                            mul = 1.33333f;
                            break;
                        case ScreenShotSize.ThreeHalves:
                            mul = 1.5f;
                            break;
                        case ScreenShotSize.Double:
                            mul = 2f;
                            break;
                    }
                    Screen.width = (int)Math.Round(mul * UnityEngine.Screen.width);
                    Screen.height = (int)Math.Round(mul * UnityEngine.Screen.height);
                    StringBuilder stringBuilder = new StringBuilder(256);
                    stringBuilder.Append(UserData.Create("cap"));
                    stringBuilder.Append(DateTime.Now.Year.ToString("0000"));
                    stringBuilder.Append(DateTime.Now.Month.ToString("00"));
                    stringBuilder.Append(DateTime.Now.Day.ToString("00"));
                    stringBuilder.Append(DateTime.Now.Hour.ToString("00"));
                    stringBuilder.Append(DateTime.Now.Minute.ToString("00"));
                    stringBuilder.Append(DateTime.Now.Second.ToString("00"));
                    stringBuilder.Append(DateTime.Now.Millisecond.ToString("000"));
                    stringBuilder.Append(".png");
                    SaveScreenshot(CaptureMethod.RenderToTex_Asynch, stringBuilder.ToString());
                    //File.WriteAllBytes(stringBuilder.ToString(), result);
                }
            }
        }

        public static bool Capture = false;

        public void SaveScreenshot(CaptureMethod method, string filePath)
        {
            if (method == CaptureMethod.AppCapture_Asynch)
            {
                Application.CaptureScreenshot(filePath);
            }
            else if (method == CaptureMethod.AppCapture_Synch)
            {
                Texture2D texture = GetScreenshot(CaptureMethod.AppCapture_Synch);
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(filePath, bytes);
            }
            else if (method == CaptureMethod.ReadPixels_Asynch)
            {
                StartCoroutine(SaveScreenshot_ReadPixelsAsynch(filePath));
            }
            else if (method == CaptureMethod.ReadPixels_Synch)
            {
                Texture2D texture = GetScreenshot(CaptureMethod.ReadPixels_Synch);

                byte[] bytes = texture.EncodeToPNG();

                //Save our test image (could also upload to WWW)
                File.WriteAllBytes(filePath, bytes);

                //Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
                DestroyObject(texture);
            }
            else if (method == CaptureMethod.RenderToTex_Asynch)
            {
                StartCoroutine(SaveScreenshot_RenderToTexAsynch(filePath));
            }
            else
            {
                Texture2D screenShot = GetScreenshot(CaptureMethod.RenderToTex_Synch);
                byte[] bytes = screenShot.EncodeToPNG();
                File.WriteAllBytes(filePath, bytes);
            }
        }

        private IEnumerator SaveScreenshot_ReadPixelsAsynch(string filePath)
        {
            //Wait for graphics to render
            yield return new WaitForEndOfFrame();

            //Create a texture to pass to encoding
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            //Put buffer into texture
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            //Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
            yield return 0;

            byte[] bytes = texture.EncodeToPNG();

            //Save our test image (could also upload to WWW)
            File.WriteAllBytes(filePath, bytes);

            //Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            DestroyObject(texture);
        }

        private IEnumerator SaveScreenshot_RenderToTexAsynch(string filePath)
        {
            //Wait for graphics to render
            yield return new WaitForEndOfFrame();

            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            Camera.main.targetTexture = rt;
            Camera.main.Render();

            //Render from all!
            //foreach (Camera cam in Camera.allCameras)
            //{
            //    cam.targetTexture = rt;
            //    cam.Render();
            //    cam.targetTexture = null;
            //}

            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null; //Added to avoid errors
            Destroy(rt);

            //Split the process up
            yield return 0;

            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }

        private static int tempFileCount = 0;
        ///<summary>Must use a Synch capture type to work.</summary>
        public Texture2D GetScreenshot(CaptureMethod method)
        {
            if (method == CaptureMethod.AppCapture_Synch)
            {
                string tempFilePath = Environment.GetEnvironmentVariable("TEMP") + "/screenshotBuffer" + tempFileCount + ".png";
                tempFileCount++;
                Application.CaptureScreenshot(tempFilePath);
                WWW www = new WWW("file://" + tempFilePath.Replace(Path.DirectorySeparatorChar.ToString(), "/"));

                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                while (!www.isDone) { }
                www.LoadImageIntoTexture((Texture2D)texture);
                File.Delete(tempFilePath); //Can delete now

                return texture;
            }
            else if (method == CaptureMethod.ReadPixels_Synch)
            {
                //Create a texture to pass to encoding
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

                //Put buffer into texture
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0); //Unity complains about this line's call being made "while not inside drawing frame", but it works just fine.*

                return texture;
            }
            else if (method == CaptureMethod.RenderToTex_Synch)
            {
                RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
                Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

                Camera.main.targetTexture = rt;
                Camera.main.Render();

                //Render from all!
                //foreach (Camera cam in Camera.allCameras)
                //{
                //    cam.targetTexture = rt;
                //    cam.Render();
                //    cam.targetTexture = null;
                //}

                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                Camera.main.targetTexture = null;
                RenderTexture.active = null; //Added to avoid errors
                Destroy(rt);

                return screenShot;
            }
            else
                return null;
        }
    }
    public enum CaptureMethod
    {
        AppCapture_Asynch,
        AppCapture_Synch,
        ReadPixels_Asynch,
        ReadPixels_Synch,
        RenderToTex_Asynch,
        RenderToTex_Synch
    }

}
