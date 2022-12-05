
var sLines = File.ReadAllLines("data.in");
var nTotal = 0;

// foreach (var sLine in sLines)
// {
//     //Console.Out.WriteLine(sLine);
//     var sIn = sLine.Trim();
//     var nLength = sIn.Length;
//     var nHalfLength = nLength / 2;
//     var sDict = new Dictionary<char, int>();

//     var sFirstPart = sIn.Substring(0, nHalfLength);

//     for (var i = nHalfLength; i < nLength; i++)
//     {
//         var c1 = sIn[i];
//         if (sFirstPart.Contains(c1))
//         {
//             Console.Out.WriteLine(c1);

//             if ((c1 - 'A') <= 26)
//                 nTotal += (c1 - 'A') + 27;

//             else
//                 nTotal += (c1 - 'a') + 1;

//             //Console.Out.WriteLine(nTotal);  
//             break;
//         }

//     }
// }



// part2
for (var n = 0; n < sLines.Length; n += 3)
{
    var sL1 = sLines[n];
    var sL2 = sLines[n + 1];
    var sL3 = sLines[n + 2];

    foreach (var c in sL1)
    {
        if (sL2.Contains(c) && sL3.Contains(c))
        {
            Console.WriteLine("Found:" + c);
            if ((c - 'A') <= 26)
                nTotal += (c - 'A') + 27;

            else
                nTotal += (c - 'a') + 1;

            break;
        }
    }

}

Console.Out.WriteLine(nTotal);




