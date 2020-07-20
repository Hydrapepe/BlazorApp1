using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.PeerToPeer.Collaboration;

namespace BlazorApp1.Data
{
    public class WeatherForecastService
    {
        public Task<WeatherForecast[]> GetForecastAsync()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string relPath = @"..\..\..\Memory\IPBase.txt";
            string resPath = Path.Combine(path, relPath);
            resPath = Path.GetFullPath(resPath);
            int count = File.ReadAllLines(resPath).Length;
            string[] counte = File.ReadAllLines(resPath);

            int row_count = count;
            int col_count = 4;
            string[,] str_array = new string[row_count, col_count];

            string one = string.Join("\\", counte);
            string[] words = one.Split(new char[] { '\\' });

            for (int i = 0, j = 0, t = 0; i < count*3; i += 3, t++)
            {
                str_array[t, j] = words[i];
                str_array[t, j + 1] = words[i + 1];
                str_array[t, j + 2] = words[i + 2];
            }


            return Task.FromResult(Enumerable.Range(1, count).Select(index => new WeatherForecast
            {
                IP = str_array[index - 1, 0],
                HostName = str_array[index - 1, 1],
                CheckSum = str_array[index - 1, 2]
            }).ToArray()) ;
        }
    }
}