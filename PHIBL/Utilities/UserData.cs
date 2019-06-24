using System;

namespace PHIBL
{
    public abstract class UserData
    {
        public static string Create(string name)
        {
            return fileDat.Create(name);
        }

        public static string Path => fileDat.Path;

        private static FileData fileDat = new FileData("UserData");
    }
}