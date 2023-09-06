﻿using System.Globalization;

namespace BetterCoding.Strapi.SDK.Core.Services
{
    public interface IDataDecoder
    {
        object Decode(object data, IServiceHub serviceHub);
        T Grab<T>(IDictionary<string, object> dictionary, string path, Func<object, T> converter);
    }

    public class DataDecode : IDataDecoder
    {
        public object Decode(object data, IServiceHub serviceHub) => data switch
        {
            null => default,
            IDictionary<string, object> { } dictionary => dictionary.ToDictionary(pair => pair.Key, pair => Decode(pair.Value, serviceHub)),
            IList<object> { } list => list.Select(item => Decode(item, serviceHub)).ToList(),
            _ => data
        };

        internal static string[] DateFormatStrings { get; } =
        {
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f'Z'",
        };

        public static DateTime ParseDate(string input) => DateTime.ParseExact(input, DateFormatStrings, CultureInfo.InvariantCulture, DateTimeStyles.None);

        public T Grab<T>(IDictionary<string, object> dictionary, string path, Func<object, T> converter)
        {
            var subPaths = path.Split('.');
            var rootPath = subPaths.First();
            if (dictionary.TryGetValue(rootPath, out var value))
            {
                if (subPaths.Length == 1) return converter(value);
                else
                {
                    var subDictionary = value as Dictionary<string, object>;
                    var restOfPath = string.Join('.', subPaths.Skip(1).ToList());
                    return Grab<T>(subDictionary, restOfPath, converter);
                }
            }
            throw new KeyNotFoundException($"not key matches {path} in dictionary");
        }
    }
}
