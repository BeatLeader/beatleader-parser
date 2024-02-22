// See https://aka.ms/new-console-template for more information
using beatleader_parser;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var result = BeatmapParser.LoadPath("C:\\SteamLibrary\\steamapps\\common\\Beat Saber\\Beat Saber_Data\\CustomWIPLevels\\Break Stasis Extended Mix");

Console.WriteLine(JsonConvert.SerializeObject(result));