var sLines = File.ReadAllLines("data.in");


var lMonkeys = new List<Monkey>();
Monkey currentMonkey = null;

foreach (var inLine in sLines)
{

    var sLine = inLine.Trim();
    if (sLine.StartsWith("Monkey"))
    {
        currentMonkey = new Monkey();
        currentMonkey.lessWorry = 1;
        lMonkeys.Add(currentMonkey);
    }
    else if (sLine.StartsWith("Starting items:"))
    {
        var sParsed = sLine.Substring("Starting items:".Length);
        var sItems = sParsed.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var sItem in sItems)
        {
            var nItem = int.Parse(sItem);
            currentMonkey.items.Add(nItem);
        }
    }
    else if (sLine.StartsWith("Operation: new = old"))
    {
        var sParsed = sLine.Substring("Operation: new = old".Length);
        var sItems = sParsed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        currentMonkey.sOperator = sItems[0];
        currentMonkey.sOperand = sItems[1];
    }
    else if (sLine.StartsWith("Test: divisible by "))
    {
        var sParsed = sLine.Substring("Test: divisible by ".Length);
        currentMonkey.TestBy = int.Parse(sParsed);
    }
    else if (sLine.StartsWith("If true: throw to monkey"))
    {
        var sParsed = sLine.Substring("If true: throw to monkey".Length);
        currentMonkey.ifTrue = int.Parse(sParsed);
    }
    else if (sLine.StartsWith("If false: throw to monkey"))
    {
        var sParsed = sLine.Substring("If false: throw to monkey".Length);
        currentMonkey.ifFalse = int.Parse(sParsed);
    }
    else
    {
        if (sLine.Length > 0)
            throw new Exception($"Invalid input: {sLine}");
    }
}

var nCheck = 1;
foreach (var m in lMonkeys)
{
    nCheck *= m.TestBy;
}

RenderMonkeys();


for (var n = 0; n < 10000; n++)
{

    foreach (var m in lMonkeys)
    {
        m.lessWorry = nCheck;
        m.ProcessNext(lMonkeys);
    }

    if (n % 1000 == 0)
    {
        Console.Out.Write($"Round {n}");
        RenderMonkeys();
    }
}

RenderMonkeys();

void RenderMonkeys()
{
    var nCount = 1;
    foreach (var m in lMonkeys)
    {
        Console.Out.WriteLine($"Monkey {nCount}: inspected: {m.nInspected} -  {m.ToString()}");
        nCount++;
    }
}


class Monkey
{

    // Starting items: 64, 89, 65, 95
    // Operation: new = old * 7
    // Test: divisible by 3
    // If true: throw to monkey 4
    // If false: throw to monkey 1

    public List<long> items = new List<long>();

    public int TestBy = 0;

    public string sOperator;
    public string sOperand;

    public int ifTrue = -1;
    public int ifFalse = -1;

    public int nInspected = 0;

    public int lessWorry = 3;

    public void ProcessNext(List<Monkey> allMonkeys)
    {
        var nResults = new List<int>();

        foreach (var nItem in items)
        {
            nInspected++;
            var nNew = callVal(nItem);
            if (nNew % TestBy == 0)
            {
                allMonkeys[ifTrue].items.Add(nNew);
            }
            else
            {
                allMonkeys[ifFalse].items.Add(nNew);
            }
        }
        items = new List<long>();
    }

    public override string ToString()
    {
        return string.Join(",", this.items);
    }


    private long callVal(long nOld)
    {

        var nNew = nOld;
        var nOperand = -1l;
        if (sOperand == "old")
            nOperand = nOld;
        else
            nOperand = int.Parse(sOperand);


        switch (sOperator)
        {
            case "+":
                nNew += nOperand;
                break;
            case "*":
                nNew *= nOperand;
                break;
            default:
                throw new Exception($"Oparator {sOperator} not allowed");
        }

        nNew = nNew % lessWorry;
        return nNew;
    }
}