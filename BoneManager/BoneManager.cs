using System;
using System.Collections.Generic;
using System.Collections;
using IllusionPlugin;
using UnityEngine;
using static BoneModHarmony.UIUtils;

namespace BoneModHarmony
{
    class BoneManager : Singleton<BoneManager>
    {
        static internal List<BMMHuman> ModedCharas = new List<BMMHuman>();
        private bool mainWindow = false;
        internal bool windowdragflag;
        BMMHuman selectedChara;
        Bone selectedBone;
        BoneModifier clonedValue;
        private bool additionalBonePage = false;
        private float scaleMin;
        private float scaleMax;
        private float positionRange;
        private float rotationRange;
        const int windowID = 33901;
        IEnumerator ApplyBone()
        {
            while (true)
            {
                yield return null;
                if(selectedChara != null && selectedBone != null && mainWindow)
                {
                    if (selectedChara.targetBones.Contains(selectedBone))
                        selectedChara.ApplyBoneModifier(selectedBone);
                    else
                        selectedBone = null;
                }
            }            
        }
        void Update()
        {
            if (Input.GetKeyDown(Prefs.WindowToggle))
            {
                mainWindow = !mainWindow;
            }
            if (mainWindow)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    mainWindow = false;
                }
            }
            else
            {
                windowdragflag = false;
            }
        }
        void OnEnable()
        {
            scaleMin = ModPrefs.GetFloat("BoneManager", "Scale.min", 0f);
            scaleMax = ModPrefs.GetFloat("BoneManager", "Scale.max", 2f);
            positionRange = ModPrefs.GetFloat("BoneManager", "Position.range", 0.1f);
            rotationRange = ModPrefs.GetFloat("BoneManager", "Rotation.range", 60f);
            StartCoroutine(ApplyBone());
        }
        private void OnGUI()
        {
            if (!mainWindow)
            {
                return;
            }

            if (!styleInitialized)
            {
                InitStyle();
            }
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(x: scale.x * customscale, y: scale.y * customscale, z: 1f));
            if (Event.current.type == EventType.repaint)
            {
                LimitWindowRect();
            }
            windowRect = GUILayout.Window(windowID, windowRect, BoneManagerWindow, "", windowstyle);
            Vector2 pos = new Vector2
            {
                x = Input.mousePosition.x * UIUtils.Screen.width / UnityEngine.Screen.width,
                y = (UnityEngine.Screen.height - Input.mousePosition.x) * UIUtils.Screen.height / UnityEngine.Screen.height
            };

            if ((Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown) && !windowRect.Contains(pos * customscale))
            {
                windowdragflag = false;
            }
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }

        private void BoneManagerWindow(int id)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                GUI.FocusWindow(windowID);
                windowdragflag = true;
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                windowdragflag = false;
            }
            if (ModedCharas.Count==0)
            {
                EmptyPage(new GUIContent("No Characters. "));
                GUI.DragWindow();
                return;
            }

            GUILayout.BeginHorizontal();
            using (var verticalScope = new GUILayout.VerticalScope("box", GUILayout.Width(windowRect.width * 0.24f)))
            {                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Characters: ", labelstyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                scrollPosition[0] = GUILayout.BeginScrollView(scrollPosition[0]);
                foreach (var Chara in ModedCharas)
                {
                    ModedCharaList(Chara);
                }
                GUILayout.EndScrollView();
            }
            using (var verticalScope = new GUILayout.VerticalScope("box", GUILayout.Width(windowRect.width * 0.24f)))
            {
                ModedBoneList(selectedChara);                
            }
            GUILayout.BeginVertical();
            scrollPosition[2] = GUILayout.BeginScrollView(scrollPosition[2]);
            if (additionalBonePage)
            {
                AdditionalBonePage();
            }
            else
            {
                BoneModifierEditor(selectedBone);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(GUIStrings.Close_Window, buttonstyleNoStretch))
            {
                mainWindow = false;
            }
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }

        private void AdditionalBonePage()
        {
            GUILayout.BeginHorizontal();

            if (selectedChara.human.sex == Character.SEX.FEMALE)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(" Body bones: ", labelstyle);
                scrollPosition[3] = GUILayout.BeginScrollView(scrollPosition[3]);
                AdditionalBoneList<ShapeBodyInfoFemale.DstBoneName>();
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                GUILayout.Label(" Head bones: ", labelstyle);
                scrollPosition[4] = GUILayout.BeginScrollView(scrollPosition[4]);
                AdditionalBoneList<ShapeHeadInfoFemale.DstBoneName>();
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginVertical();
                GUILayout.Label(" Body bones: ", labelstyle);
                scrollPosition[3] = GUILayout.BeginScrollView(scrollPosition[3]);
                AdditionalBoneList<ShapeBodyInfoMale.DstBoneName>();
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                GUILayout.Label(" Head bones: ", labelstyle);
                scrollPosition[4] = GUILayout.BeginScrollView(scrollPosition[4]);
                AdditionalBoneList<ShapeHeadInfoMale.DstBoneName>();
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }            
            GUILayout.EndHorizontal();
        }
        void AdditionalBoneList<TEnum>()
        {            
            string[] selectionGUIContent = Enum.GetNames(typeof(TEnum));
            var index = GUILayout.SelectionGrid(-1, selectionGUIContent, 1, selectstyle);
            if (index == -1)
                return;
            selectedChara.AddBoneModifier(selectionGUIContent.GetValue(index).ToString());
        }
        void ModedCharaList(BMMHuman modedChara)
        {
            if (modedChara == null)
            {
                return;
            }
            GUILayout.BeginHorizontal();
            var selected = ToggleButton(ReferenceEquals(modedChara, selectedChara), new GUIContent(modedChara.Name), x => selectedBone = null);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(GUIStrings.Reset, buttonstyleNoStretch))
            {
                modedChara.reset = true;
                if(selected)
                    selectedBone = null;
            }
            if (GUILayout.Button(GUIStrings.Save, buttonstyleNoStretch))
                modedChara.SaveProfile();
            GUILayout.EndHorizontal();
            if (selected)
            {
                selectedChara = modedChara;
            }
        }
        void ModedBoneList(BMMHuman modedChara)
        {
            if (modedChara == null)
            {
                EmptyPage(new GUIContent("Select a character. "));
                selectedBone = null;
                additionalBonePage = false;
                return;
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Bones: ", labelstyle);
            GUILayout.FlexibleSpace();
            additionalBonePage = UIUtils.ToggleButton(additionalBonePage, new GUIContent(" + "), x => selectedBone = null);                 
            GUILayout.EndHorizontal();           

            if(modedChara.boneModifiers == null || modedChara.boneModifiers.Count == 0)
            {
                EmptyPage(new GUIContent("No modified bones. "));
                return;
            }
            scrollPosition[1] = GUILayout.BeginScrollView(scrollPosition[1]);
            foreach (var bone in modedChara.targetBones)
            {
                GUILayout.BeginHorizontal();
                if (UIUtils.ToggleButton(ReferenceEquals(bone, selectedBone), new GUIContent(bone.transform.name)))
                {
                    selectedBone = bone;
                    additionalBonePage = false;
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(" - ", buttonstyleNoStretch))
                {                    
                    modedChara.RemoveBoneModifier(bone);
                    if (bone == selectedBone)
                        selectedBone = null;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        void BoneModifierEditor(Bone bone)
        {
            if(bone == null)
            {
                EmptyPage(new GUIContent("Select a bone. "));
                return;
            }
            var boneModifier = selectedChara.boneModifiers[bone.ID];
            GUILayout.BeginHorizontal();
            GUILayout.Label(bone.transform.name, titlestyle2);
            if (GUILayout.Button(" Copy value ",buttonstyleNoStretch))
                clonedValue = boneModifier.Clone();
            if (clonedValue != null)
            {
                if (GUILayout.Button(" Paste value ", buttonstyleNoStretch))
                    boneModifier.PasteValue(clonedValue);
            }
            GUILayout.EndHorizontal();
            
            ToggleGUITitle(ref boneModifier.isScale, new GUIContent("Scale"));
            if (boneModifier.isScale)
            {
                SliderGUI(ref boneModifier.Scale.x, scaleMin, scaleMax, 1f, " x ", valuedecimals: "N5");
                SliderGUI(ref boneModifier.Scale.y, scaleMin, scaleMax, 1f, " y ", valuedecimals: "N5");
                SliderGUI(ref boneModifier.Scale.z, scaleMin, scaleMax, 1f, " z ", valuedecimals: "N5");
            }
            ToggleGUITitle(ref boneModifier.isPosition, new GUIContent("Position"));
            if (boneModifier.isPosition)
            {
                TweakGUI(ref boneModifier.Position.x, positionRange, " x ", valuedecimals: "N5");
                TweakGUI(ref boneModifier.Position.y, positionRange, " y ", valuedecimals: "N5");
                TweakGUI(ref boneModifier.Position.z, positionRange, " z ", valuedecimals: "N5");
            }
            ToggleGUITitle(ref boneModifier.isRotate, new GUIContent("Rotation"));
            if(boneModifier.isRotate)
            {
                TweakGUI(ref boneModifier.Rotation.x, rotationRange, " x ");
                TweakGUI(ref boneModifier.Rotation.y, rotationRange, " y ");
                TweakGUI(ref boneModifier.Rotation.z, rotationRange, " z ");
            }
        }        
    }
    public enum SaveFormat
    {
        Compressed,
        Uncompressed
    };
}
