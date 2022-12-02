//var sTestFile = "example.in";
var sTestFile = "test.in";
var sLines = File.ReadAllLines(sTestFile);


var nTotal = 0;
var nTotal2 = 0;
foreach (var sLine in sLines)
{
    switch(sLine.Trim())
    {
        case "A X":
            nTotal += 1 + 3;
            break;
        case "A Y":
            nTotal += 2 + 6;
            break;
        case "A Z":
            nTotal += 3 + 0;
            break;

        case "B X":
            nTotal += 1 + 0;
            break;
        case "B Y":
            nTotal += 2 + 3;
            break;
        case "B Z":
            nTotal += 3 + 6;
            break;

        case "C X":
            nTotal += 1 + 6;
            break;
        case "C Y":
            nTotal += 2 + 0;
            break;
        case "C Z":
            nTotal += 3 + 3;
            break;
    }

    var oppPlay = sLine[0];
    var selfPlay = ' ';
    var result = sLine[2];

    var cWin = 'Z';
    var cDraw = 'Y';
    var cLose = 'X';

    if (result == cWin)
    {
        nTotal2 += 6;
        if (oppPlay == 'A')
            selfPlay = 'B';
        if (oppPlay == 'B')
            selfPlay = 'C';
        if (oppPlay == 'C')
            selfPlay = 'A';
    }
    else if (result == cDraw)
    {
        nTotal2 += 3;
        selfPlay = oppPlay;
    }
    else if (result == cLose)
    {
        if (oppPlay == 'A')
            selfPlay = 'C';
        if (oppPlay == 'B')
            selfPlay = 'A';
        if (oppPlay == 'C')
            selfPlay = 'B';
    }

    nTotal2 += (selfPlay - 'A') + 1;   
}

Console.WriteLine("Total: " + nTotal);
Console.WriteLine("Total2: " + nTotal2);

