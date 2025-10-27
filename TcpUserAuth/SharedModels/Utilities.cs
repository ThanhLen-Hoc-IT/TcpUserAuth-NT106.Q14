////using System;
////using System.Text.Json;

////namespace ShareModels
////{
////    public static class Utilities
////    {
////        // ✅ Chuyển object → JSON string
////        public static string ToJson(object obj)
////        {
////            try
////            {
////                return JsonSerializer.Serialize(obj);
////            }
////            catch (Exception ex)
////            {
////                System.Diagnostics.Debug.WriteLine("ToJson error: " + ex.Message);
////                return "{}";
////            }
////        }

////        // ✅ Chuyển JSON string → object kiểu T
////        public static T FromJson<T>(string json)
////        {
////            try
////            {
////                return JsonSerializer.Deserialize<T>(json);
////            }
////            catch (Exception ex)
////            {
////                System.Diagnostics.Debug.WriteLine("FromJson error: " + ex.Message);
////                return default;
////            }
////        }
////    }
////}

//using SharedModels;

//using System;
//using System.Text.Json;

//namespace SharedModels
//{
//    public static class Utilities
//    {
//        // ✅ Object → JSON
//        public static string ToJson(object obj)
//        {
//            try
//            {
//                return JsonSerializer.Serialize(obj);
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine("ToJson error: " + ex.Message);
//                return "{}";
//            }
//        }

//        // ✅ JSON → Object
//        public static T FromJson<T>(string json)
//        {
//            try
//            {
//                return JsonSerializer.Deserialize<T>(json);
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine("FromJson error: " + ex.Message);
//                return default;
//            }
//        }
//    }
//}

using System;
using System.Text.Json;

namespace SharedModels
{
    public static class Utilities
    {
        public static string ToJson(object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ToJson error: " + ex.Message);
                return "{}";
            }
        }

        public static T FromJson<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FromJson error: " + ex.Message);
                return default;
            }
        }
    }
}


