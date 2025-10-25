using System;
using System.Text.Json;

namespace ServerApp
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
