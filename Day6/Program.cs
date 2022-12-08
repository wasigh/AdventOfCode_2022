var sLines = File.ReadAllLines("example.in");


foreach(var sLine in sLines)
{
    var nStart = 14;
    var nEnd = sLine.Length;
    
    for (var i = nStart; i< nEnd; i++)
    {
        var sPart = sLine.Substring(i - nStart, 14);
        
        var bFound = false;

        for (var c = 0; c < sPart.Length - 1; c++)
        {
            var nChar = sPart[c];
            if (sPart.IndexOf(nChar, c + 1) > 0 )
            {
                bFound = true;
                break;
            }
        }

        if (!bFound)
        {
            Console.Out.WriteLine(sPart + ":" +  i);
            break;
        }
    }

}
