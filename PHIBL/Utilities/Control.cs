using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Utility.Xml
{
	public class Control
	{
		public Control(string savePath, string saveName, string rootName, List<Data> dataList)
		{
			this.savePath = savePath;
			this.saveName = saveName;
			this.rootName = rootName;
			this.dataList = dataList;
            Init();
		}
		public List<Data> DataList
		{
			get
			{
				return dataList;
			}
		}

		public Data this[int index]
		{
			get
			{
				return dataList[index];
			}
		}

		public virtual void Init()
		{
			foreach (Data data in dataList)
			{
				data.Init();
			}
		}

		public virtual void Write()
		{
			string file = PHIBL.UserData.Create(savePath) + saveName;
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8
            };
            XmlWriter xmlWriter = null;
			try
			{
				xmlWriter = XmlWriter.Create(file, xmlWriterSettings);
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement(rootName);
				foreach (Data data in dataList)
				{
					data.Write(xmlWriter);
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();
			}
			finally
			{
				if (xmlWriter != null)
				{
					xmlWriter.Close();
				}
			}
		}

		public virtual void Read()
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				string text = string.Concat(new object[]
				{
                    PHIBL.UserData.Path,
                    savePath,
					'/',
                    saveName
                });
				if (System.IO.File.Exists(text))
				{
					xmlDocument.Load(text);
					foreach (Data data in dataList)
					{
						data.Read(rootName, xmlDocument);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
		}

		protected readonly string savePath;
		protected readonly string saveName;
		protected readonly string rootName;
		protected List<Data> dataList = new List<Data>();
	}
}
