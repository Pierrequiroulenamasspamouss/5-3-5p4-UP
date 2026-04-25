using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program {
    static void Main() {
        string json = File.ReadAllText(@"C:\Unity\LATEST\Minionsparadise\Assets\Resources\definitions.json");
        var imageRegex = new Regex(@""image"\s*:\s*"([^"]+)"", RegexOptions.IgnoreCase);
        var maskRegex = new Regex(@""mask"\s*:\s*"([^"]+)"", RegexOptions.IgnoreCase);
        
        var images = new List<string>();
        foreach (Match m in imageRegex.Matches(json)) images.Add(m.Groups[1].Value);
        
        var masks = new List<string>();
        foreach (Match m in maskRegex.Matches(json)) masks.Add(m.Groups[1].Value);
        
        var hashes = new Dictionary<int, string>();
        for (int i = 0; i < Math.Min(images.Count, masks.Count); i++) {
            string str = string.Format("{0}{1}TrueFalseFalse1", images[i], masks[i]);
            int hash = str.GetHashCode();
            if (hashes.ContainsKey(hash) && hashes[hash] != str) {
                Console.WriteLine("COLLISION DETECTED: " + hashes[hash] + " and " + str);
            } else {
                hashes[hash] = str;
            }
        }
        Console.WriteLine("Checked " + hashes.Count + " combinations.");
    }
}
