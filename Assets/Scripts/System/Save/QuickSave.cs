using System;
using System.IO;
using Service_Locator;
using UnityEditor;
using UnityEngine;

namespace Save
{
    public interface IData{}
    
    public interface ISerializer
    {
        public void Serialize<T>(T data, string fileName, bool overwrite) where T : IData;
        public T Deserialize<T>(string fileName) where T : IData;
    }

    public interface IDataServices
    {
        public void Save<T>(T data, string fileName, bool overwrite = true) where T : IData;
        public T Load<T>(string fileName) where T : IData;
        public void Delete(string fileName);
        public void DeleteAll();
    }
    
    public class QuickSave : MonoBehaviour, ISerializer, IDataServices
    {
        private string _savePath;

        private void Awake()
        {
            ServiceLocator.Global.Register<QuickSave>(this);
            _savePath = Application.persistentDataPath + Path.DirectorySeparatorChar;
        }

        public void Serialize<T>(T data, string fileName, bool overwrite) where T : IData
        {
            string filePath = CombineWithPath(fileName);

            if (!overwrite &&  File.Exists(filePath))
            {
                Debug.LogWarning($"File {fileName} already exists and cannot be overwritten");
            }

            var content = JsonUtility.ToJson(data, true);
            try
            {
                File.WriteAllText(filePath, content);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save file {fileName} to {_savePath}");
            }
        }

        public T Deserialize<T>(string fileName) where T : IData
        {
            string filePath = CombineWithPath(fileName);
            
            if (!File.Exists(filePath))
            {
                Debug.Log($"{fileName} does not exist at path {_savePath}");
                throw new FileNotFoundException($"File {fileName} does not exist at path {_savePath}");
            }

            var json = File.ReadAllText(filePath);
            try
            {
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete file {fileName} from {_savePath}");
                throw;
            }
        }

        public void Save<T>(T data, string fileName, bool overwrite = true) where T : IData
        {
            Serialize(data, fileName, overwrite);
        }

        public T Load<T>(string fileName) where T : IData
        {
            return Deserialize<T>(fileName);
        }

        public void Delete(string fileName)
        {
            var filePath = CombineWithPath(fileName);
            if (!File.Exists(filePath))
                Debug.Log($"{fileName} does not exist at path {_savePath}");

            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete file {fileName} from {_savePath}");
            }
        }

        public void DeleteAll()
        {
            var files = Directory.GetFiles(_savePath, "*.json");
            if (files.Length == 0)
                Debug.Log("Does not contain any json files");
            foreach (var file in files)
                File.Delete(file);
        }
        
        /// <summary>
        /// Combine file name with Application.persistentDataPath 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CombineWithPath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be empty or null");

            string extension = Path.GetExtension(fileName).ToLower();
            if (!string.IsNullOrEmpty(extension))
                fileName = Path.GetFileNameWithoutExtension(fileName);
            return Path.Combine(_savePath, fileName + ".json");
        }
    }
}