using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XmlFileEngine
{
    public class Commands
    {
        public string BasePath { get;  }
        private Methods methods { get; }

        private string[] DirectoryHeadAttributes { get; }
        private string[] FileHeadAttributes { get; }
        private string[] FileSubAttributes { get; }

        public Commands(string filePath){
            BasePath = filePath;

            DirectoryHeadAttributes = new string[]{"name", "modifdate", "size"};
            FileHeadAttributes = new string[]{"name"};
            FileSubAttributes = new string[]{"extension", "text", "modifdate", "size"};

            methods = new Methods(BasePath, DirectoryHeadAttributes, FileHeadAttributes, FileSubAttributes);
        }


        public void CreateDirectory(string fullPath)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(Path.GetDirectoryName(fullPath), 0);

            string[] headAttributes = new string[] {
                Path.GetFileName(fullPath),
                DateTime.Now.ToFileTime().ToString(),
                "0"
            };

            methods.CreateDirectory(xmlPath, headAttributes);

            methods.SaveDocument();
        }
        public void CreateFile(string fullPath)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(Path.GetDirectoryName(fullPath), 0);

            string[] headAttributes = new string[] {
                Path.GetFileName(fullPath)
            };

            string[] subAttributes = new string[] {
                methods.GetExtension(Path.GetFileName(fullPath)),
                "",
                DateTime.Now.ToFileTime().ToString(),
                "0"
            };

            methods.CreateFile(xmlPath, headAttributes, subAttributes);

            methods.SaveDocument();
        }

        public string GetDirectoryAttribute(string path, string attribute)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 0);

            return methods.GetDirectoryAttribute(xmlPath, attribute);
        }
        public string GetFileAttribute(string path, string attribute)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 1);

            return methods.GetFileAttribute(xmlPath, attribute);
        }

        public void DeleteDirectory(string path)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 0);
            methods.DeleteElement(xmlPath);

            methods.SaveDocument();
        }
        public void DeleteFile(string path)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 1);
            methods.DeleteElement(xmlPath);

            methods.SaveDocument();
        }

        public void ChangeDirectoryAttribute(string path, string attribute, string value)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 0);

            methods.ChangeDirectoryAttribute(xmlPath, attribute, value);

            methods.SaveDocument();
        }
        public void ChangeFileAttribute(string path, string attribute, string value)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 1);

            methods.ChangeFileAttribute(xmlPath, attribute, value);

            methods.SaveDocument();
        }

        public string[] GetAllDirectories(string path)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 0);
            string[] directories = methods.GetAllElements(xmlPath, 0);

            for (int i = 0; i < directories.Length; i++) {
                directories[i] = Path.Combine(path, directories[i]);
            }

            return directories;
        }
        public string[] GetAllFiles(string path)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 0);
            string[] files = methods.GetAllElements(xmlPath, 1);

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.Combine(path, files[i]);
            }

            return files;
        }

        public bool DirectoryExists(string path)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 0);

            return methods.ElementExists(xmlPath);
        }
        public bool FileExists(string path)
        {
            methods.LoadDocument();

            string xmlPath = methods.PathToXml(path, 1);

            return methods.ElementExists(xmlPath);
        }

        public void MoveDirectory(string directoryPath, string newParentPath)
        {
            methods.LoadDocument();

            string xmlDirectoryPath = methods.PathToXml(directoryPath, 0);
            string xmlNewParentPath = methods.PathToXml(newParentPath, 0);

            methods.MoveElement(xmlDirectoryPath, xmlNewParentPath);

            methods.SaveDocument();
        }
        public void MoveFile(string filePath, string newParentPath)
        {
            methods.LoadDocument();

            string xmlFilePath = methods.PathToXml(filePath, 1);
            string xmlNewParentPath = methods.PathToXml(newParentPath, 0);

            methods.MoveElement(xmlFilePath, xmlNewParentPath);

            methods.SaveDocument();
        }

        public void CloneDirectory(string directoryPath, string newParentPath, string newName)
        {
            methods.LoadDocument();

            string xmlDirectoryPath = methods.PathToXml(directoryPath, 0);
            string xmlNewParentPath = methods.PathToXml(newParentPath, 0);

            methods.CloneElement(xmlDirectoryPath, xmlNewParentPath, newName);

            methods.SaveDocument();
        }
        public void CloneFile(string filePath, string newParentPath, string newName)
        {
            methods.LoadDocument();

            string xmlFilePath = methods.PathToXml(filePath, 1);
            string xmlNewParentPath = methods.PathToXml(newParentPath, 0);

            methods.CloneElement(xmlFilePath, xmlNewParentPath, newName);

            methods.SaveDocument();
        }
    }
}
