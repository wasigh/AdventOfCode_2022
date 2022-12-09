var sLines = File.ReadAllLines("data.in");

static Tuple<int, int> UpdateKnotPosition(Tuple<int, int> headPos, Tuple<int, int> tailPos)
{
    // update tailPos
    var xDiff = headPos.Item1 - tailPos.Item1;
    var yDiff = headPos.Item2 - tailPos.Item2;

    var xDiffAbs = Math.Abs(xDiff);
    var yDiffAbs = Math.Abs(yDiff);

    if (xDiffAbs >= 1 && yDiffAbs >= 1 && (xDiffAbs >= 2 || yDiffAbs >= 2))
    {
        // normalize to -1 or  1
        var xStep = xDiff / xDiffAbs;
        var yStep = yDiff / yDiffAbs;
        tailPos = new Tuple<int, int>(tailPos.Item1 + xStep, tailPos.Item2 + yStep);
    }
    else if (xDiffAbs > 1)
    {
        var xStep = xDiff / xDiffAbs;
        tailPos = new Tuple<int, int>(tailPos.Item1 + xStep, tailPos.Item2);
    }
    else if (yDiffAbs > 1)
    {
        var yStep = yDiff / yDiffAbs;
        tailPos = new Tuple<int, int>(tailPos.Item1, tailPos.Item2 + yStep);
    }

    return tailPos;
}

var headPos = new Tuple<int, int>(0, 0);
//var tailPos = new Tuple<int, int>(0, 0);

var tails = new List<Tuple<int,int>>();
for (var i = 1; i <= 9 ; i++)
{
    var t = new Tuple<int, int>(0, 0);
    tails.Add(t);
}

var tailPosDict = new Dictionary<string, int>();

// loop trough all lines

foreach (var sLine in sLines)
{
    // parse input
    var sOp = sLine.Substring(0, 1);
    var sAmount = sLine.Substring(2);
    var nAmount = int.Parse(sAmount);

    // exeute step
    for (var n = 0; n < nAmount; n++)
    {
        switch (sOp)
        {
            case "R": // right
                headPos = new Tuple<int, int>(headPos.Item1, headPos.Item2 + 1);
                break;
            case "L": // Left
                headPos = new Tuple<int, int>(headPos.Item1, headPos.Item2 - 1);
                break;
            case "U": // up
                headPos = new Tuple<int, int>(headPos.Item1 + 1, headPos.Item2);
                break;
            case "D": // Down
                headPos = new Tuple<int, int>(headPos.Item1 - 1, headPos.Item2);
                break;
        }

        var prevKnot = headPos;

        for (var i = 0; i < tails.Count() ; i++)
        {
            var knot = tails[i];
            var knotPoss = UpdateKnotPosition(prevKnot, knot);
            tails[i] = knotPoss;
            prevKnot = knotPoss;
        }

        // Update TailDict
        var tailPos = tails.Last();
        var sTailKey = $"{tailPos.Item1}#{tailPos.Item2}";
        Console.Out.WriteLine($"{headPos.Item1}#{headPos.Item2} - {tailPos.Item1}#{tailPos.Item2}");

        if (tailPosDict.ContainsKey(sTailKey))
        {
            tailPosDict[sTailKey]++;
        }
        else
        {
            tailPosDict[sTailKey] = 1;
        }
    }
}

Console.Out.WriteLine(tailPosDict.Keys.Count);

