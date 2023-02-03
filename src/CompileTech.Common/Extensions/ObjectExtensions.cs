using System.Dynamic;

namespace CompileTech.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToStringCheckForNull(this object? str)
        {
            if (str != null) return str.ToString();
            return string.Empty;
        }

        public static int ToInt(this object? i)
        {
            if (i != null && i.ToStringCheckForNull() != string.Empty)
                return Convert.ToInt32(i);
            return 0;
        }

        public static int? ToIntOrNull(this object? i)
        {
            if (i != null && i.ToStringCheckForNull() != string.Empty)
                return Convert.ToInt32(i);
            return null;
        }

        public static dynamic CreateExpandoFromObject(this object source)
        {
            var result = new ExpandoObject();
            IDictionary<string, object> dictionary = result;
            foreach (var property in source
                         .GetType()
                         .GetProperties()
                         .Where(p => p.CanRead && p.GetMethod.IsPublic))
                dictionary[property.Name] = property.GetValue(source, null);
            return result;
        }

        public static IDictionary<string, object> CreateDictionaryFromObject(this object source)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (var property in source
                         .GetType()
                         .GetProperties()
                         .Where(p => p.CanRead && p.GetMethod.IsPublic))
                result[property.Name] = property.GetValue(source, null);
            return result;
        }
    }
}