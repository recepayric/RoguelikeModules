using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MVC.Base.Runtime.Utility
{
    public static class SaveSystem<T>
    {
        public static void Save(T data,string filename)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + filename +".mvc";
            FileStream stream = new FileStream(path,FileMode.OpenOrCreate);
            formatter.Serialize(stream,data);
            stream.Close();
        }

        public static T Load(string filename)
        {
            string path = Application.persistentDataPath + "/" + filename +".mvc";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path,FileMode.Open);
                T data = (T)formatter.Deserialize(stream);
                stream.Close();
                return data;
            }
            else
            {
                Debug.Log("SaveFileDidntFound");
                return default(T);
            }

        }
    }
}