


var sLines = File.ReadAllLines("data.in");
var nCount = 0;

foreach(var sLine in sLines)
{
    var lineParts = sLine.Trim().Split(",");

    var p1 = new Linepart(lineParts[0]);
    var p2 = new Linepart(lineParts[1]);

    if (p1.OverLaps(p2) || p2.OverLaps(p1) )
    {
        Console.Out.WriteLine(sLine);
        nCount ++;
    }

}


Console.Out.WriteLine(nCount);



class Linepart
{

    int nMin = 0;
    int nMax = 0;
     

    public Linepart(string sIn)
    {
        var sParts = sIn.Split("-");
        this.nMin = int.Parse(sParts[0]);
        this.nMax = int.Parse(sParts[1]);
    }

    public bool Contains(Linepart other)
    {
        return (other.nMin >= this.nMin && other.nMax <= this.nMax);
    }

    public bool OverLaps(Linepart other)
    {
        if (other.nMin >= this.nMin && other.nMin <= this.nMax)
            return true;
        if (other.nMax >= this.nMin && other.nMax <= this.nMax)
            return true;
        return false;

    }
}
