var sLines = File.ReadAllLines("data.in");

Node nodeStart = null;
Node nodeEnd = null;

var lNodes = new List<Node>();
var lToVisit = new List<Node>();

var height = sLines.Count();
var width = sLines[0].Trim().Count();

var row = 0;
var col = 0;

foreach(var sLine in sLines)
{
   
    foreach(var c in sLine.Trim())
    {
        var n = new Node(row, col);    
        n.CellValue = c;
        lNodes.Add(n);


        if (c == 'S' || c == 'a')
        {
            //nodeStart = n;
            n.minDistance = 0;
            lToVisit.Add(n);

        }
        else if  (c == 'E')
        {
            nodeEnd = n;
        }

        col ++;
    } 
    row ++;
    col = 0;
}

var bFound = false;

while (!bFound && lToVisit.Count > 0)
{
    lToVisit.Sort();

    
    var compareNode = lToVisit[0];
    lToVisit.RemoveAt(0);

    var nIndex = (compareNode.row * width) + compareNode.col;
    var neighbours = new List<Node>();

    // up
    if (nIndex >= width)
    {
        var nUpIndex = nIndex - width;
        var upNode = lNodes[nUpIndex];
        neighbours.Add(upNode);        
    }
    // down
    if (nIndex + width < lNodes.Count)
    {
        var nDownIndex = nIndex + width;
        var downNode = lNodes[nDownIndex];
        neighbours.Add(downNode);        
    }

    // left 
    if (compareNode.col > 0)
    {
        var nLeftIndex = nIndex - 1;
        var leftNode = lNodes[nLeftIndex];
        neighbours.Add(leftNode);        
    }

    // right 
    if (compareNode.col < (width - 1))
    {
        var nRightIndex = nIndex + 1;
        var rightNode = lNodes[nRightIndex];
        neighbours.Add(rightNode);        
    }


    // check if neigbour is visitable
    foreach (var n in neighbours)
    {

        if (n == nodeEnd )
        {
            if (compareNode.CellValue == 'z' && compareNode.minDistance + 1 < n.minDistance)
            {
                nodeEnd.minDistance = compareNode.minDistance + 1;
                nodeEnd.prevNode = compareNode;
            }
            continue;
            //bFound = true;
            //break;
        }
        else if (n == nodeStart || n.CellValue == 'a')
        {
            continue;
        }
        else{
            var currentChar = compareNode.CellValue;
            if (currentChar == 'S')
                currentChar = 'a';

            if (currentChar == 'E')
                currentChar = 'z';

            var targetChar = n.CellValue;

            Console.Out.WriteLine($"TO: {targetChar} - FROM: {currentChar} = {targetChar - currentChar}");   

            if (targetChar - currentChar <= 1)
            {
                if (compareNode.minDistance + 1 < n.minDistance)
                {
                    Console.Out.WriteLine($"{compareNode.ToString()} - >> -  {n.ToString()}");   

                    n.minDistance = compareNode.minDistance + 1;
                    n.prevNode = compareNode;
                    lToVisit.Add(n);
                }
            }
        }
    }
}


var visitNode = nodeEnd;
Console.Out.WriteLine($"Distance: {nodeEnd.minDistance}" );
var nCount = 0;
while (visitNode != null)
{
    nCount++;
    visitNode = visitNode.prevNode;
}

Console.Out.WriteLine($"Visited: {nCount}" );




class Node : IComparable<Node>{
    public int minDistance = int.MaxValue;

    public Node prevNode = null;

    public char CellValue = '.';
    public int row;
    public int col;

    public Node(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public int CompareTo(Node? other)
    {
        if (other == null)
            return -1;

        return -1 * this.minDistance - other.minDistance;
    }

    public override string ToString()
    {
        return $"Row: {this.row}; Col: {this.col}; {this.CellValue}";
    }
}