using System.Collections;
using System.Globalization;
using System.Resources;
using System.Text;
using CompileTech.Common.Extensions;

namespace CompileTech.Common.Helpers
{
    public static class ResxHelper
    {
        public static Dictionary<string, string> GenerateResources<T>(string cultureName)
        {
            return CultureToDictionary<T>(CultureInfo.GetCultureInfo(cultureName));
        }

        public static string GenerateResourceKeyTsFile<T>()
        {
            var sb = new StringBuilder();
            var resourceKeys = CultureToDictionary<T>(CultureInfo.CurrentCulture).Select(r => $"'{r.Key}'").ToList();
            resourceKeys.Insert(0, "''");

            sb.AppendLine($"/* Generated File ({DateTime.Now:MM/dd/yyyy hh:mm:ss tt 'UTC'z}) */");
            sb.AppendLine("export type { ResourceKey };");
            sb.AppendLine($"declare type ResourceKey = {string.Join(" | ", resourceKeys)};");

            return sb.ToString();
        }

        private static Dictionary<string, string> CultureToDictionary<T>(CultureInfo culture)
        {
            var resourceManager = new ResourceManager(typeof(T));
            using var resourceSet = resourceManager.GetResourceSet(culture, true, true);
            return resourceSet.Cast<DictionaryEntry>().ToDictionary(x => x.Key.ToString().ToCamelCase(), x => resourceManager.GetObject((string)x.Key, culture)!.ToString());
        }
    }
}