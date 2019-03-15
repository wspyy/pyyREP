using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Tools
{
    public class SerializerHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] Serializer<T>(T t)
        {
            MemoryStream mStream = new MemoryStream();
            BinaryFormatter bformatter = new BinaryFormatter();  //二进制序列化类
            bformatter.Serialize(mStream, t); //将消息类转换为内存流
            mStream.Flush();

            mStream.Position = 0;  //将流的当前位置重新归0，否则Read方法将读取不到任何数据
            long dataLength = mStream.Length;

            byte[] buffer = new byte[dataLength];
            mStream.Read(buffer, 0, (int)dataLength);

            return buffer;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] buffer)
        {

            MemoryStream mStream = new MemoryStream();
            mStream.Write(buffer, 0, buffer.Length); //将接收到的数据写入内存流
            mStream.Flush();
            mStream.Position = 0;

            BinaryFormatter bFormatter = new BinaryFormatter();
            //if (mStream.Capacity > 0)
            {
                T msg = (T)bFormatter.Deserialize(mStream);//将接收到的内存流反序列化为对象
                return msg;
            }

        }

    }
    /// <summary>
    /// JSON序列化和反序列化辅助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializerToString<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            //替换Json的Date字符串
            string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }
        /// <summary>
        /// JSON序列化
        /// </summary>
        public static byte[] JsonSerializerToArray<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            //替换Json的Date字符串
            string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            //转成数组           
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonString);
            return byteArray;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(Byte[] jsonArray)
        {
            string jsonString = System.Text.Encoding.UTF8.GetString(jsonArray);

            return JsonDeserialize<T>(jsonString);
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(MemoryStream mStream)
        {
            byte[] jsonArray = new byte[mStream.Length]; ;
            mStream.Read(jsonArray, 0, jsonArray.Length);


            return JsonDeserialize<T>(jsonArray);

        }
        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 将时间字符串转为Json时间
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
    }
}