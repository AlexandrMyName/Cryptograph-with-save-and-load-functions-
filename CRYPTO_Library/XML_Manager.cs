using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace CRYPTO_Library
{
    /// <summary>
    /// To use XML_Manager you need create KeyGen (XML_Manager.CreateKeyGen)
    /// </summary>
    public class XML_Manager
    {
        private  string _path;
        private XML_Manager(string path) => _path = path;
         
        private string SerializeObjectToSTR<T>(T objectForDesirialization)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlText = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xmlSerializer.Serialize(xmlText, objectForDesirialization);
            memoryStream = (MemoryStream) xmlText.BaseStream;
            return GetStr(memoryStream.ToArray());
        }
        private object DesirializeObject<T>(string str)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(GetBytes(str));
            return xmlSerializer.Deserialize(memoryStream);
        }
        private string LoadStr()
        {
            using (StreamReader reader = File.OpenText(_path))
            {
                string str = reader.ReadToEnd();
                return Cryptograph.Decrypt(str);
            }
        }
        private void SaveStr(string str)
        {
            using(StreamWriter writer = File.CreateText(_path))
            {
                writer.Write(Cryptograph.Encrypt(str));
            }
        }
        private bool HasFile => File.Exists(_path);
        private byte[] GetBytes(string str) => UTF8Encoding.UTF8.GetBytes(str);

        private string GetStr(byte[] bytes) => UTF8Encoding.UTF8.GetString(bytes);

        /// <summary>
        /// Key is 32 bit string
        /// </summary>
        /// <param name="key"></param>
        public static XML_Manager CreateKeyGen(string key, string savePath)
        {
            Cryptograph.Key = key;
            return new XML_Manager(key);
        }
        public void Save<T>(T objectForSerialization)
        {
            if(_path == null) throw new Exception("You should create KeyGen | Path not found..");
            string str = SerializeObjectToSTR<T>(objectForSerialization);
            SaveStr(str);
        }
        public T Load<T>(T objectForDesirialization)
        {
            if (HasFile){

                string str = LoadStr();
                return (T) DesirializeObject<T>(str);
            }
            else throw new Exception("File doesn't exist | I can load..");
        }
    }
}
