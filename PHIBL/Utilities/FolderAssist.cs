using System;
using System.Collections.Generic;
using System.IO;

namespace PHIBL
{

    public class FolderAssist
    {
        public int GetFileCount()
        {
            return lstFile.Count;
        }

        public bool CreateFolderInfo(string folder, string searchPattern, bool getFiles = true, bool clear = true)
        {
            if (clear)
            {
                lstFile.Clear();
            }
            if (!Directory.Exists(folder))
            {
                return false;
            }
            string[] array;
            if (getFiles)
            {
                array = Directory.GetFiles(folder, searchPattern);
            }
            else
            {
                array = Directory.GetDirectories(folder);
            }
            if (array.Length == 0)
            {
                return false;
            }
            foreach (string text in array)
            {
                FileInfo fileInfo = new FileInfo();
                fileInfo.FullPath = text;
                if (getFiles)
                {
                    fileInfo.FileName = Path.GetFileNameWithoutExtension(text);
                }
                fileInfo.time = File.GetLastWriteTime(text);
                lstFile.Add(fileInfo);
            }
            return true;
        }

        public bool CreateFolderInfoEx(string folder, string[] searchPattern, bool clear = true)
        {
            if (clear)
            {
                lstFile.Clear();
            }
            if (!Directory.Exists(folder))
            {
                return false;
            }
            List<string> list = new List<string>();
            foreach (string searchPattern2 in searchPattern)
            {
                list.AddRange(Directory.GetFiles(folder, searchPattern2));
            }
            string[] array = list.ToArray();
            if (array.Length == 0)
            {
                return false;
            }
            foreach (string text in array)
            {
                FileInfo fileInfo = new FileInfo();
                fileInfo.FullPath = text;
                fileInfo.FileName = Path.GetFileNameWithoutExtension(text);
                fileInfo.time = File.GetLastWriteTime(text);
                lstFile.Add(fileInfo);
            }
            return true;
        }

        public int GetIndexFromFileName(string filename)
        {
            int num = 0;
            foreach (FileInfo fileInfo in lstFile)
            {
                if (fileInfo.FileName == filename)
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        public void SortName(bool ascend = true)
        {
            if (lstFile.Count == 0)
            {
                return;
            }
            if (ascend)
            {
                lstFile.Sort((FileInfo a, FileInfo b) => a.FileName.CompareTo(b.FileName));
            }
            else
            {
                lstFile.Sort((FileInfo a, FileInfo b) => b.FileName.CompareTo(a.FileName));
            }
        }

        public void SortDate(bool ascend = true)
        {
            if (lstFile.Count == 0)
            {
                return;
            }
            if (ascend)
            {
                lstFile.Sort((FileInfo a, FileInfo b) => a.time.CompareTo(b.time));
            }
            else
            {
                lstFile.Sort((FileInfo a, FileInfo b) => b.time.CompareTo(a.time));
            }
        }

        public List<FileInfo> lstFile = new List<FileInfo>();

        public class FileInfo
        {
            public FileInfo()
            {
            }

            public FileInfo(string fullpath)
            {
                FullPath = fullpath;                
                FileName = Path.GetFileNameWithoutExtension(fullpath);                
                time = File.GetLastWriteTime(fullpath);
            }

            public FileInfo(FileInfo src)
            {
                FullPath = src.FullPath;
                FileName = src.FileName;
                time = src.time;
            }

            public void Copy(FileInfo src)
            {
                FullPath = src.FullPath;
                FileName = src.FileName;
                time = src.time;
            }

            public string FullPath = string.Empty;
            public string FileName = string.Empty;
            public DateTime time;
        }
        public string[] GetFilenames()
        {
            var FileNames = new string[GetFileCount()];
            int count = 0;
            foreach (FileInfo fileInfo in lstFile)
            {
                FileNames[count] = fileInfo.FileName;
                count++;
            }
            return FileNames;
        }
    }
}