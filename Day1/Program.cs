// See https://aka.ms/new-console-template for more information

var sTestFile = "test.in";
var sLines = File.ReadAllLines(sTestFile);

var nCurrent = 0;
var nMax = 0;
var lDwrfs = new List<int>();


foreach (var sLine in sLines)
{
    if (string.IsNullOrEmpty(sLine))
    {
        lDwrfs.Add(nCurrent);
        nCurrent= 0;
    }
    else
    {
        var nVal = int.Parse(sLine);
        nCurrent  += nVal;
        nMax = Math.Max(nCurrent, nMax);
    }
}

Console.WriteLine("max total: " + nMax);

// Sort 
lDwrfs.Sort();
lDwrfs.Reverse();
var nTotal = lDwrfs.Take(3).Sum();
Console.WriteLine("Top 3: " + nTotal);


