using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BD
{
    public static class SystemCustom
    {
       public static char[] chars { get; private set; } = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890".ToCharArray();
        public static string Generation(int sizen)
        {
            Random random = new Random();
            string txt = "";
            for (int i = 0;i < sizen;i++)
            {
                txt += Convert.ToString(chars[random.Next(0,(chars.Length-1))]);
            }
            return txt;
        }
        public static byte[] WriteXml<T>(T save)// Запись xml в байты 
        {
            byte[] bytes;
            using (MemoryStream MS = new MemoryStream())
            {
                new XmlSerializer(typeof(T)).Serialize(MS, save);
                bytes = MS.ToArray();
            }
            return bytes;
        }
        public static T ReadXml<T>(byte[] bytes)
        {
            T t;
            using (MemoryStream mem_stream = new MemoryStream(File.ReadAllBytes(Name)))
            {
                t = (T)new XmlSerializer(typeof(T)).Deserialize(mem_stream);
            }
            return t;
        }

    }
}
