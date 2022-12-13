var sLines = File.ReadAllLines("data.in");


IEnumerable<int> ExecuteLines(string[] sLines)
{
    var nVal = 1;

    foreach(var sIn in sLines)
    {
        if (sIn == "noop")
            yield return nVal;
        else if (sIn.StartsWith("addx"))
        {
            var sOpVal =  sIn.Substring("addx".Length);
            var nOpVal = int.Parse(sOpVal);

            yield return nVal;
            yield return nVal;
            nVal += nOpVal;
        }
    }
}

var nCount = 0;

foreach(var nVal in ExecuteLines(sLines))
{
    
    if (nCount % 40 == 0)
    {
        Console.Out.WriteLine();
    }

    var nPos = nCount % 40;
    
    if (Math.Abs(nPos - nVal) <= 1)
    {
        Console.Out.Write("#");
    } 
    else{
        Console.Out.Write(".");
    }

      nCount++;

}
Console.Out.WriteLine();
Console.Out.Flush();
