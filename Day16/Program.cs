var sLines = File.ReadAllLines("data.in");


var dTunnels = new Dictionary<string, Tunnel>();

foreach (var sLine in sLines)
{
    var sIn = sLine.Replace("Valve ", "")
                    .Replace(" has flow rate=", ";")
                    .Replace(" tunnel leads to valve ", "")
                    .Replace(" tunnels lead to valves ", "");

    Console.Out.WriteLine(sIn);

    var sParts = sIn.Split(";");
    var sCode = sParts[0];
    var nPressure = int.Parse(sParts[1]);
    var lNeighbours = sParts[2].Split(",").Select(s => s.Trim()).ToList(); ;

    var t = new Tunnel(sCode, nPressure, lNeighbours);
    dTunnels.Add(sCode, t);

}



foreach (var tunnel in dTunnels.Values)
{
    tunnel.fillDistanceMatrix(dTunnels);
    Console.Out.WriteLine(tunnel);
}

var steps = 31;
var startNode = dTunnels["AA"];

//var lVisited = new List<String>();
//var score = startNode.BestChoice(steps,0,lVisited, dTunnels);

// Hier moet een soort aStar worden uitgerekend over alle opties, met de visited route in een string
// en de totale score + als heruistic en de resterende 


var lVisiting = new List<TunnelVisits>();
var tv = new TunnelVisits()
{
    StepsHuman = 26,
    StepsElephant = 26,
    Score = 0,
    TotalPath = "AA",
    HumanCode = "AA",
    ElephantCode = "AA"
};

lVisiting.Add(tv);

TunnelVisits largest = null;


List<TunnelVisits> lFinal = new List<TunnelVisits>();

var nCount = 0;


while (lVisiting.Count > 0)
{

    nCount++;
    if (nCount % 1000 == 0)
        Console.Out.WriteLine(lVisiting.Count);

    //lVisiting.Sort();
    var currentTv = lVisiting.First();
    lVisiting.RemoveAt(0);

    var sHumanCode = currentTv.HumanCode;
    var humanTunnel = dTunnels[sHumanCode];
    
    // var sElephantCode = currentTv.ElephantCode;
    // var elephantTunnel = dTunnels[sElephantCode];
    
    var StepsHuman = currentTv.StepsHuman;
    var StepsElephant = currentTv.StepsElephant;

    // AA;DD;BB;JJ;HH;EE;CC
    if (currentTv.TotalPath.StartsWith("AA;DD;BB;"))
    {
        var b = false;
    }

    

    var nVistiableNeighbours = 0;

    foreach (var n in humanTunnel.dTunnels.Keys)
    {
        if (currentTv.TotalPath.Contains(";" + n))
            continue;

        var dist = humanTunnel.dTunnels[n];
        if (dist >= StepsHuman)
        {
            //lFinal.Add(currentTv);    
            //Console.Out.WriteLine(currentTv.ToString());
            continue;
        }

        var nPressure = dTunnels[n].nPressure;
        if (nPressure == 0)
            continue;


        var remainingHuman = StepsHuman - dist - 1;
        var maxPresRelease = remainingHuman * nPressure;
        var sTotalPath = currentTv.TotalPath + ";" + n;

        var newTv = new TunnelVisits()
            {
                StepsHuman = remainingHuman,
                //StepsElephant = remainingElephant,
                TotalPath = sTotalPath,
                Score = currentTv.Score + maxPresRelease,
                HumanCode = n,
                //ElephantCode = e
            };
            lVisiting.Add(newTv);
            nVistiableNeighbours++;            

            
            if (largest == null)
                largest = newTv;

            if (largest.Score < newTv.Score)
            {
                largest = newTv;
                Console.Out.WriteLine("New largest: " + newTv.ToString());
            }
       
    }

    // if (nVistiableNeighbours == 0)
    // {
        lFinal.Add(currentTv);  
    // }

}


lFinal.Sort();
Console.Out.WriteLine(lFinal.First().ToString());

var nLargest = 0;

for (var i = 0; i < lFinal.Count; i++)
{
    var first = lFinal[i];
    
    for (var j = i + 1; j < lFinal.Count; j++)
    {
        var second = lFinal[j];
        if (first.Score + second.Score < nLargest)
            break;

        var score = first.Add(second);
        if (score > nLargest)
        {
            nLargest = score;
            Console.Out.WriteLine($"New largest- {first.TotalPath} && {second.TotalPath} = {score}");
        }
    }
}



// Console.Out.WriteLine("Score: " + score);
// Console.Out.WriteLine(string.Join("; ", lVisited));


class TunnelVisits : IComparable<TunnelVisits>
{
    public string TotalPath { get; set; }
    //public int RemainingSteps {get;set;}

    public int StepsHuman { get; set; }
    public int StepsElephant { get; set; }

    public int Heueristic
    {
        get
        {
            return this.Score;// * (this.StepsHuman + this.StepsElephant);
        }
    }

    public int Score { get; set; }

    public string HumanCode { get; set; }

    public string ElephantCode { get; set; }

    int IComparable<TunnelVisits>.CompareTo(TunnelVisits? other)
    {
        return -1 * this.Heueristic.CompareTo(other.Heueristic);
    }

    public override string ToString()
    {
        return $"{this.TotalPath} - {this.Score} - {this.StepsHuman +  this.StepsElephant}";
    }

    public int Add(TunnelVisits other)
    {
        var parts = other.TotalPath.Split(";");
        foreach (var part in parts)
        {
            if (part == "AA")
                continue;
            if (this.TotalPath.Contains(part))
                return -1;
        }

        return this.Score + other.Score;
    }
}


class Tunnel
{
    public string sCode;
    public int nPressure;
    private List<string> neighbours;

    public new Dictionary<string, int> dTunnels = new Dictionary<string, int>();

    public Tunnel(string sCode, int nPressure, List<string> neighbours)
    {
        this.sCode = sCode;
        this.nPressure = nPressure;
        this.neighbours = neighbours;
    }

    public override string ToString()
    {
        return $"{this.sCode}, p:{this.nPressure} - n: {string.Join(",", this.neighbours)}";
    }


    public void fillDistanceMatrix(Dictionary<string, Tunnel> allTunnels)
    {

        foreach (var k in allTunnels.Keys)
        {
            if (k != this.sCode)
                dTunnels[k] = int.MaxValue;
            else
                dTunnels[k] = 0;
        }


        foreach (var n in this.neighbours)
        {
            // hoe gaan ik om met die 30, niet...
            var nNeigntbourVal = dTunnels[n];
            var distance = 1;
            if (nNeigntbourVal > distance)
            {
                dTunnels[n] = distance;
                var nTunnel = allTunnels[n];
                nTunnel.SetNeighbours(distance, dTunnels, allTunnels);
            }
        }
    }

    private void SetNeighbours(int distance, Dictionary<string, int> dTunnels, Dictionary<string, Tunnel> allTunnels)
    {
        foreach (var n in this.neighbours)
        {
            var nNeigntbourVal = dTunnels[n];
            var d = distance + 1;
            if (nNeigntbourVal > d)
            {
                dTunnels[n] = d;
                var nTunnel = allTunnels[n];
                nTunnel.SetNeighbours(d, dTunnels, allTunnels);
            }
        }
    }


    public int BestChoice(int remainingSteps, int score, List<string> visited, Dictionary<string, Tunnel> allTunnels)
    {
        if (remainingSteps <= 1)
            return score;

        if (remainingSteps == 2)
        {
            if (!visited.Contains(this.sCode))
            {
                visited.Add(this.sCode);
                score += this.nPressure;
                return score;
            }
        }

        var dScores = new Dictionary<string, int>();

        var sNextStop = this.sCode;
        var highestScore = this.nPressure * (remainingSteps - 1);

        foreach (var sName in this.dTunnels.Keys)
        {
            var dist = this.dTunnels[sName];
            if (sName == this.sCode || dist == 0)
                continue;

            if (dist < remainingSteps && !visited.Contains(sName))
            {
                var tunnel = allTunnels[sName];
                var copyList = string.Join(".", visited).Split(".").ToList();
                var tunnelScore = tunnel.BestChoice(remainingSteps - dist, score, copyList, allTunnels);

                if (tunnelScore > highestScore)
                {
                    highestScore = tunnelScore;
                    sNextStop = sName;
                }
            }
        }

        visited.Add(sNextStop);
        score += highestScore;
        return score;



    }


}
