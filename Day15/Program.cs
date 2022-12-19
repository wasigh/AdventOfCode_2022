// var sLines = File.ReadAllLines("example.in");
// var nMax = 20;
var sLines = File.ReadAllLines("data.in");
var nMax = 4000000;

var lSensors = new List<SensorAndBeacon>();

foreach (var sLine in sLines)
{
    var sPrepped = sLine.Replace("Sensor at ", "")
                        .Replace("x=", "")
                        .Replace(": closest beacon is at ", ";")
                        .Replace(", y=", ";");



    var nParts = sPrepped.Split(";").Select(a => int.Parse(a)).ToList();


    Console.Out.WriteLine(string.Join(",", nParts));

    lSensors.Add(new SensorAndBeacon(nParts[0], nParts[1], nParts[2], nParts[3]));
}

// 1879233318 - toLow


//var nSensors = new List<SensorAndBeacon>() {lSensors[6]};

//Console.Out.WriteLine("Check Sensors = " + nSensors.Count());



var nMinY = 0;
var nMaxY = nMax;

// var nMinX = nSensors.Aggregate(0, (acc, x) => Math.Min(acc, x.minX)) - 3;
// var nMaxX = nSensors.Aggregate(0, (acc, x) => Math.Max(acc, x.maxX)) + 3;
var nMinX = 0;
var nMaxX = nMax;

var toCheck = new int[nMax];
var nCount = 0;
var bFound = false;
for (int r = 0; r < toCheck.Length; r++)
{

    if (bFound)
        break;

    if (r % 400000 == 0)
    {
        Console.Out.Write(".");
        Console.Out.Flush();
    }

    var c = toCheck[r];

    if (c > nMaxX)
        continue;

    do
    {
        var sensor = lSensors.FirstOrDefault(s => s.intersectsPoint(r, c));
        if (sensor == null)
        {
            Console.Out.WriteLine($"{r} x {c}= {(long)r * 4000000l + (long)c}");
            bFound = true;
            break;
        }
        else
        {
            sensor.UpdateSkips(r, c, toCheck, nMax);
        }

        c = toCheck[r];

    } while (c < nMaxX);

}

// 1879233318 - toLow


// Console.Out.WriteLine($"MinX: {nMinX}, MaxX: {nMaxX}");
// var nCount = 0;
// for (var j = nMinY; j <= nMaxY; j++)
// {
//     var nSensors = lSensors.Where(s => s.intersectsLine(j));

//     for (var i = nMinX; i <= nMaxX; i++)
//     {

//         if (i % 400000 == 0)
//             Console.Out.Write(".");

//         if (lSensors.Any(s => s.isBeaconPos(i, j)))
//         {
//             //Console.Out.Write("B");
//             continue;
//         }

//         if (lSensors.Any(s => s.isSensorPos(i, j)))
//         {
//             //Console.Out.Write("S");
//             continue;
//         }

//         if (nSensors.Any(s => s.intersectsPoint(i, j)))
//         {
//             //Console.Out.Write("#");
//             //Console.Out.Write(".");
//             nCount++;
//         }
//         else
//         {
//             Console.Out.WriteLine($"{i} x {j}= {i * 4000000 + j}");
//             break;
//         }
//     }
//     Console.Out.WriteLine();
// }
//  4861076
// 5649994
// 5099318 to high
Console.Out.WriteLine($"Count: {nCount}");


class SensorAndBeacon
{
    private int sX;
    private int sY;
    private int bX;
    private int bY;
    private int manhattanDist;

    public SensorAndBeacon(int sX, int sY, int bX, int bY)
    {
        this.sX = sX;
        this.sY = sY;
        this.bX = bX;
        this.bY = bY;

        this.manhattanDist = Math.Abs(this.sX - this.bX) + Math.Abs(this.sY - this.bY);
    }

    public int minX => this.sX - this.manhattanDist;
    public int maxX => this.sX + this.manhattanDist;

    public bool intersectsLine(int y)
    {
        var xDist = Math.Abs(this.sY - y);
        return xDist <= this.manhattanDist;
    }

    public bool isBeaconPos(int x, int y)
    {
        return (this.bX == x && this.bY == y);
    }

    public bool isSensorPos(int x, int y)
    {
        return (this.sX == x && this.sY == y);
    }


    public bool intersectsPoint(int x, int y)
    {
        var dist = Math.Abs(this.sX - x) + Math.Abs(this.sY - y);
        return dist <= this.manhattanDist; ;
    }

    internal void UpdateSkips(int x, int y, int[] toCheck, int nMax)
    {
        // find the first post this is outside the range

        for (var xPos = x; xPos <= maxX + 1; xPos++)
        {
            if (xPos >= nMax)
                break;

            if (xPos == 14){
                var nop = true;    
            }

            var yPos = toCheck[xPos];
            if (!this.intersectsPoint(xPos, yPos))
                continue;

            var dist = Math.Abs(this.sX - xPos);
            var outSideY = this.sY + (this.manhattanDist - dist) + 1;

            if (xPos < nMax)
            {
                toCheck[xPos] = Math.Max(toCheck[xPos], outSideY);
            }
        }

    }
}