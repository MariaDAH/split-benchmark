using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BlushingPenguin.JsonPath;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SplitBench
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Bench).Assembly);
        }
    }

    [MemoryDiagnoser]
    public class Bench
    {

        Stream JsonStream = new MemoryStream();
        string data = File.ReadAllText("PretendData.json");
        string split = "$.[*]";


        [IterationSetup]
        public void Setup()
        {
            JsonStream = new MemoryStream(Encoding.UTF8.GetBytes(data ?? ""));
        }

        [Benchmark(Baseline = true)]
        public void SplitJson()
        {
            var parsedContent = JToken.Parse(data);
            var splitContents = parsedContent.SelectTokens(split);
        }

        [Benchmark]
        public async Task SplitText()
        {
            var parseContent = await System.Text.Json.JsonDocument.ParseAsync(JsonStream);
            var splitContents = parseContent.RootElement.SelectTokens(split);
        }
    }
}
