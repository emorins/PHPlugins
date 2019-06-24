using System;
using System.IO;
using UnityEngine;

namespace PHIBL
{
    public class FileData
    {
        public FileData(string rootName = "")
        {
            this.rootName = rootName;
        }

        public virtual string Create(string name)
        {
            string text = Path + name;
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            return text + '/';
        }

        public string Path
        {
            get
            {
                string text;
                if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    text = Application.dataPath + "/../";
                }
                else
                {
                    text = Application.persistentDataPath + "/";
                }
                if (rootName != string.Empty)
                {
                    text = text + rootName + '/';
                }
                return text;
            }
        }

        protected string rootName = string.Empty;
    }
}