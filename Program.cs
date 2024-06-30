using Newtonsoft.Json.Linq;

// Enable unicode encoding for emojis
Console.OutputEncoding = System.Text.Encoding.Unicode;

if (args.Length != 4)
{
    Console.WriteLine("Usage: jsontocsv -i input.json -o output.csv");
    return;
}

string? inputFilePath = null;
string? outputFilePath = null;

for (int i = 0; i < args.Length; i += 2)
{
    switch (args[i])
    {
        case "-i": inputFilePath = args[i + 1]; break;
        case "-o": outputFilePath = args[i + 1]; break;
        default:
            Console.WriteLine("Invalid argument");
            return;
    }
}

if (inputFilePath == null || outputFilePath == null)
{
    Console.WriteLine("Usage: jsontocsv -i input.json -o output.csv");
    return;
}

try
{
    string json = File.ReadAllText(inputFilePath);
    var csv = ConvertJsonToCsv(json);
    File.WriteAllText(outputFilePath, csv);
    Console.WriteLine("💠 CSV file created successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

/// Convert json to csv
static string ConvertJsonToCsv(string json)
{
    var csv = new List<string>();
    var jsonArray = JArray.Parse(json);
    var headers = jsonArray.Children<JObject>()
        .SelectMany(obj => obj.Properties().Select(p => p.Name))
        .Distinct()
        .ToList();

    csv.Add(string.Join(",", headers));

    foreach (var obj in jsonArray.Children<JObject>())
    {
        var row = headers.Select(header => obj[header]?.ToString() ?? string.Empty);
        csv.Add(string.Join(",", row));
    }

    return string.Join(Environment.NewLine, csv);
}