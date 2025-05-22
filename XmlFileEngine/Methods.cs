using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace XmlFileEngine
{
    class Methods
    {
        private string BasePath { get; }

        private string[] directoryHeadAttributes;
        private string[] fileHeadAttributes;
        private string[] fileSubAttributes;

        private XmlDocument document;

        public Methods(string filePath, string[] directoryHeadAttributes, string[] fileHeadAttributes, string[] fileSubAttributes)
        {
            BasePath = filePath;

            this.directoryHeadAttributes = directoryHeadAttributes;
            this.fileHeadAttributes = fileHeadAttributes;
            this.fileSubAttributes = fileSubAttributes;

            document = new XmlDocument();
        }

        public void LoadDocument() {
            document.Load(BasePath);
        }
        public void SaveDocument() {
            document.Save(BasePath);
        }

        public string PathToXml(string path, int type)
        {
            string xmlPath = "/root";

            if (path.Length > 0)
            {
                string[] nodes = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
                for (int i = 0; i < nodes.Length - 1; i++)
                {
                    xmlPath += "/directory[@name='" + StringToHex(nodes[i]) + "']";
                }

                if (type == 0)
                {
                    xmlPath += "/directory[@name='" + StringToHex(nodes[nodes.Length - 1]) + "']";
                }
                else if (type == 1)
                {
                    xmlPath += "/file[@name='" + StringToHex(nodes[nodes.Length - 1]) + "']";
                }
            }

            return xmlPath;
        }
        public string StringToHex(string text)
        {
            string newText = "";

            byte[] ba = Encoding.Default.GetBytes(text);
            newText = BitConverter.ToString(ba);

            return newText;
        }
        public string HexToString(string text)
        {
            string newText = "";

            text = text.Replace("-", "");
            byte[] raw = new byte[text.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(text.Substring(i * 2, 2), 16);
            }

            newText = Encoding.ASCII.GetString(raw);

            return newText;
        }
        public string GetExtension(string name)
        {
            string extension = "";
            string[] array = name.Split('.');

            if (array.Length > 1)
            {
                extension = array[1].ToUpper();
            }

            return extension;
        }

        public int TypeOfFileAttribute(string attribute) {
            int value = -1;

            // Check head attribute
            for (int i = 0; i < fileHeadAttributes.Length; i++) {
                if (fileHeadAttributes[i].Equals(attribute)) {
                    value = 0;
                }
            }

            // Check sub attribute
            for (int i = 0; i < fileSubAttributes.Length; i++){
                if (fileSubAttributes[i].Equals(attribute))
                {
                    value = 1;
                }
            }

            return value;
        }

        //Commands
        public void CreateDirectory(string parentPath, string[] headAttributes) {
            XmlElement directoryElement = document.CreateElement("directory");

            // Load head attributes
            for (int i = 0; i < directoryHeadAttributes.Length; i++) {
                directoryElement.SetAttribute(directoryHeadAttributes[i], StringToHex(headAttributes[i]));
            }

            document.SelectSingleNode(parentPath).AppendChild(directoryElement);
        }
        public void CreateFile(string parentPath, string[] headAttributes, string[] subAttributes){
            XmlElement fileElement = document.CreateElement("file");

            // Load head attributes
            for (int i = 0; i < fileHeadAttributes.Length; i++) {
                fileElement.SetAttribute(fileHeadAttributes[i], StringToHex(headAttributes[i]));
            }

            // Load head attributes
            for (int i = 0; i < fileSubAttributes.Length; i++){
                XmlElement element  = document.CreateElement(fileSubAttributes[i]);
                element.InnerText = subAttributes[i];
            
                fileElement.AppendChild(element);
            }

            document.SelectSingleNode(parentPath).AppendChild(fileElement);
        }

        public string GetDirectoryAttribute(string path, string attribute) {
            return HexToString(document.SelectSingleNode(path).Attributes[attribute].Value.ToString());
        }
        public string GetFileAttribute(string path, string attribute) {
            int typeOfAttribute = TypeOfFileAttribute(attribute);

            if (typeOfAttribute == 0)
            {
                return HexToString(document.SelectSingleNode(path).Attributes[attribute].Value.ToString());
            }
            else if (typeOfAttribute == 1)
            {
                return document.SelectSingleNode(path + "/" + attribute).InnerText;
            }
            else {
                return "";
            }
        }

        public void DeleteElement(string path) {
            document.SelectSingleNode(path).ParentNode.RemoveChild(document.SelectSingleNode(path));
        }

        public void ChangeDirectoryAttribute(string path, string attribute, string value) {
            document.SelectSingleNode(path).Attributes[attribute].Value = StringToHex(value);
        }
        public void ChangeFileAttribute(string path, string attribute, string value){
            int type = TypeOfFileAttribute(attribute);

            if (type == 0) {
                document.SelectSingleNode(path).Attributes[attribute].Value = StringToHex(value);
            } else if (type == 1) {
                document.SelectSingleNode(path + "/" + attribute).InnerText = value;
            }
        }

        public string[] GetAllElements(string path, int type) {
            XmlNodeList elementNodes = document.SelectNodes(path + (type == 0 ? "/directory" : "/file"));
            string[] elements = new string[elementNodes.Count];
            for (int i = 0; i < elements.Length; i++){
                elements[i] = HexToString(elementNodes[i].Attributes["name"].Value);
            }

            return elements;
        }

        public bool ElementExists(string path) {
            if (document.SelectSingleNode(path) == null){
                return false;
            }
            else{
                return true;
            }
        }

        public void MoveElement(string elementPath, string newParentPath) {
            XmlNode elementNode = document.SelectSingleNode(elementPath).CloneNode(true);
            document.SelectSingleNode(newParentPath).AppendChild(elementNode);

            DeleteElement(elementPath);
        }

        public void CloneElement(string elementPath, string newParentPath, string newName) {
            XmlNode elementNode = document.SelectSingleNode(elementPath).CloneNode(true);
            elementNode.Attributes["name"].Value = StringToHex(newName);

            document.SelectSingleNode(newParentPath).AppendChild(elementNode);
        }
    }
}