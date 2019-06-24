using System;
using System.Xml;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utility.Xml
{
    public class Data
    {
        public Data(string elementName) => this.elementName = elementName;
        public FieldInfo[] FieldInfos => GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static);
        internal virtual void Init()
        {
        }
        internal void Read(string rootName, XmlDocument xml)
        {
            string str = rootName + "/" + elementName + "/";
            foreach (FieldInfo fieldInfo in FieldInfos)
            {
                XmlNodeList xmlNodeList = xml.SelectNodes(str + fieldInfo.Name);
                if (xmlNodeList != null)
                {
                    if (xmlNodeList.Item(0) is XmlElement xmlElement)
                    {
                        fieldInfo.SetValue(this, Cast(xmlElement.InnerText, fieldInfo.FieldType));
                    }
                }
            }
        }

        internal void Write(XmlWriter writer)
        {
            writer.WriteStartElement(elementName);
            foreach (FieldInfo fieldInfo in FieldInfos)
            {
                writer.WriteStartElement(fieldInfo.Name);
                writer.WriteValue(ConvertString(fieldInfo.GetValue(this)));
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        object Cast(string str, Type type)
        {
            if (type == typeof(Color))
            {
                string[] array = str.Split(new char[]
                {
                    ','
                });
                if (array.Length == 4)
                {
                    int num = 0;
                    return new Color(float.Parse(array[num++]), float.Parse(array[num++]), float.Parse(array[num++]), float.Parse(array[num++]));
                }
                return Color.white;
            }
            else
            {
                if (type.IsArray)
                {
                    string[] splitted_Strings = str.Split(new char[]
                    {
                        ','
                    });
                    Type elementType = type.GetElementType();
                    Array array3 = Array.CreateInstance(elementType, splitted_Strings.Length);
                    foreach (var __AnonType in splitted_Strings.Select((string v, int i) => new
                    {
                        v,
                        i
                    }))
                    {
                        array3.SetValue(Convert.ChangeType(__AnonType.v, elementType), __AnonType.i);
                    }
                    return array3;
                }
                return Convert.ChangeType(str, type);
            }
        }

        string ConvertString(object o)
        {
            if (o is Color color)
            {
                return string.Format("{0},{1},{2},{3}", new object[]
                {
                    color.r,
                    color.g,
                    color.b,
                    color.a
                });
            }
            if (o.GetType().IsArray)
            {
                Array array = (Array)o;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array.GetValue(i));
                    if (i + 1 < array.Length)
                    {
                        stringBuilder.Append(",");
                    }
                }
                return stringBuilder.ToString();
            }
            return o.ToString();
        }

        protected readonly string elementName;
    }
}
