var sLines  = File.ReadAllLines("data.in");

var lPos = new List<string>();
var lOperations = new List<string>();

var isParsingPos = true;

foreach (var sLine in sLines)
{
    if (sLine.Trim().Length == 0)
        isParsingPos = false;
    else if (isParsingPos)
        lPos.Add(sLine);
    else 
        lOperations.Add(sLine);
}



// build stacks for positions

var nCount = lPos[lPos.Count -1].Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries).Length;
var stacks = new List<Stack<char>>(nCount);
for (int n = 0; n < nCount; n++)
{
    stacks.Add(new Stack<char>());
}

// parse input
var numLines = lPos.Count - 2;
for (int n = numLines; n >= 0; n--)
{
    var currentlint = lPos[n];
    for (int cPos = 0; cPos < nCount; cPos++)
    {
        var nIndex = (cPos * 4) + 1;
        var c = currentlint[nIndex];

        if (c >= 'A')
            stacks[cPos].Push(c);
     }
}


var sMove = "move";
var sFrom = "from";
var sTo = "to";

// move items
foreach(var sOp in lOperations)
{
    var sParsed = sOp.Replace(sMove, ";")
                    .Replace(sFrom, ";")
                    .Replace(sTo, ";");

    var sParts = sParsed.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);

    var nAmount = int.Parse(sParts[0]);
    var nFrom  = int.Parse(sParts[1]);
    var nTo  = int.Parse(sParts[2]);

    var stFrom = stacks[nFrom - 1];
    var stTo = stacks[nTo - 1];

    Move(nAmount, stFrom, stTo);
}


void Move(int count, Stack<char> from, Stack<char> to)
{

    var nChars = new List<char>();
    for (var n = 0; n < count; n++)
    {
        nChars.Add(from.Pop());
    }

    nChars.Reverse();

    foreach(var c in nChars) { to.Push(c);}

    // for (var n = 0; n < count; n++)
    // {
    //     var c = from.Pop();
    //     to.Push(c);
    // }
}

var sOut = "";

for(var s = 0; s < stacks.Count; s++)
{
    sOut += stacks[s].Peek();
}


Console.Out.WriteLine(sOut);


