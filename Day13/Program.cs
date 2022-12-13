var sLines = File.ReadAllLines("data.in");

var lPairs = new List<Tuple<string, string>>();

var nTotal = 0;

for (var n = 0; n < sLines.Length; n += 3)
{
    var s1 = sLines[n];
    var s2 = sLines[n + 1];

    var pair = new Tuple<string, string>(s1, s2);

    lPairs.Add(pair);

    

    var n1Parts = new SignalString(s1);
    var n2Parts = new SignalString(s2);

    // how to signal true, false/ undecisive 1, 0, math.Max
    var nResult = n1Parts.Compare(n2Parts);
    var bCorrectOrder = nResult > 0;   


    Console.Out.WriteLine($"{s1} vs {s2}: {bCorrectOrder}");     
    if (bCorrectOrder)
    {
        nTotal += lPairs.Count;
    }
}

Console.Out.WriteLine($"Total: {nTotal}");

lPairs.Add(new Tuple<string, string>("[[2]]","[[6]]"));

var lAll = new List<SignalString>();
foreach(var pair in lPairs)
{
    lAll.Add(new SignalString(pair.Item1));
    lAll.Add(new SignalString(pair.Item2));
}

lAll.Sort();

var nPos = 1;
var nTotal2 = 1;
foreach(var s in lAll)
{
    Console.Out.WriteLine(s.sIn);

    if (s.sIn == "[[2]]" || s.sIn == "[[6]]")
        nTotal2 *= nPos;
    nPos ++;
}
Console.Out.WriteLine($"Total2: {nTotal2}");




class SignalString: IComparable<SignalString>
{
    public string sIn;
    
    public SignalString(string sIn)
    {
        this.sIn = sIn;
        if (this.sIn.Length > 0 && !this.sIn.StartsWith('['))
        {
            this.sIn = $"[{this.sIn}]";
        }


        var nLevel = 0;
        var nVal = "";
        sVals = new List<string>();

        foreach (var c in this.sIn)
        {
            switch (c)
            {
                case '[':
                    if (nLevel > 0)
                    {
                        nVal += c;
                    }
                    nLevel ++;
                    break;
                case ']':
                    nLevel--;
                    if (nLevel > 0)
                    {
                        nVal += c;
                    }
                    else
                    {
                       sVals.Add(nVal);
                    }
                    
                    break;
                case ',':
                    if (nLevel == 1)
                    {
                        sVals.Add(nVal);
                        nVal = "";
                    }
                    else
                    {
                        nVal += c;
                    }
                    break;
                default:
                    nVal += c;
                    break;
            }
        }
    }

    public List<string> sVals { get; private set; }

    public int CompareTo(SignalString? other)
    {
        var nCompare = this.Compare(other);
        if (nCompare == 0)
        {
            return 1;
        }
        else if (nCompare == 1)
        {
            return -1;
        }
        else{
            return 0;
        }
        
    }


    // how to signal true, false/ undecisive 1, 0, math.Max
    internal int Compare(SignalString right)
    {
        var nTrue = 1;
        var nFalse = 0;
        var nUndecisive = int.MaxValue;

        for (var n = 0; n < this.sVals.Count; n++)
        {
            if (n >= right.sVals.Count) // If the right list runs out of items first, the inputs are not in the right order
                return nFalse;

            var leftItem = this.sVals[n];
            var rightItem = right.sVals[n];

            // TODO : parse as int here

            var bLeftIsNumber = int.TryParse(leftItem, out var nLeft);
            var bRightIsNumber = int.TryParse(rightItem, out var nRight);

            if (bLeftIsNumber && bRightIsNumber) // should be numbers
            {
                if (nLeft < nRight)
                    return nTrue;
                if (nLeft > nRight)
                    return nFalse;
            }
            else
            {
                var lSignal = new SignalString(leftItem);
                var rSignal = new SignalString(rightItem);
                var nCompare = lSignal.Compare(rSignal);
                if (nCompare != nUndecisive)
                    return nCompare;
            }
        }


        if (this.sVals.Count < right.sVals.Count)
            return nTrue;

        return nUndecisive;
    }
}
