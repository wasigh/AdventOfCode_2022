
var blockCount = 0;

var lPlayingField = new List<int>();

// The tall, vertical chamber is exactly seven units wide. Each rock appears so that its left edge is two units away from the left wall 
// and its bottom edge is three units above the highest rock in the room (or the floor, if there isn't one).
var nStartX = 2;

var sWind = File.ReadAllText("data.in").Trim();
var nWindPos = 0;

var commonDenominator = (sWind.Length * 5);
var totalBlocks = 1000000000000l;
var blockCycles = totalBlocks / 5;
var windCycles = blockCycles / sWind.Length;

var periods = totalBlocks / commonDenominator;
var prev = 0;


var lDiffs = new List<int>();


while (blockCount < commonDenominator * 750)
{

    if (blockCount % commonDenominator == 0)
    {
        var diff = lPlayingField.Count - prev;
        Console.Out.WriteLine(lPlayingField.Count + " - diff: " + (diff) + " - BLocks: " + blockCount);
        prev = lPlayingField.Count;

        if (diff != 0)
            lDiffs.Add(diff);
    }

    var nStartY = lPlayingField.Count + 3;
    var blockNr = blockCount % 5;
    var currentBlock = new Block(blockNr, nStartX, nStartY);

    do
    {

        //Console.Out.WriteLine($"{blockCount} - X: {currentBlock.x} - Y: {currentBlock.y} - ({nWindPos})");

        nWindPos = nWindPos % sWind.Length;
            

        var windDirection = sWind[nWindPos];
        if (windDirection == '<')
            currentBlock.MoveLeft(lPlayingField);
        else if (windDirection == '>')
            currentBlock.MoveRight(lPlayingField);

        nWindPos++;

    } while (currentBlock.MoveDown(lPlayingField));

    currentBlock.ApplyMove(lPlayingField);

    //printPlayingField();

    blockCount++;

}

Console.Out.WriteLine(lPlayingField.Count);

var foundStepSize = 0;
for (var stepsize = 1; stepsize < lDiffs.Count / 2; stepsize++)
{
    var found = false;;
    var nStart = 1;
    var nVal = lDiffs[nStart];
    for (var step = nStart + stepsize; step < lDiffs.Count; step++)
    {
        
        var currVal = lDiffs[step];
        if (currVal == nVal)
        {
            found = true;

            for (var k = 0; k < stepsize; k++)
            {
                found &= lDiffs[nStart + k] == lDiffs[nStart + k + stepsize];
            }

            if (found)
            {
                break;
            }
        }
    }

    if (found)
    {
        foundStepSize = stepsize;
        break;
    }
}

var nTotalPerStep = lDiffs.Skip(1).Take(foundStepSize).Sum();
Console.Out.WriteLine("repeating per:" + foundStepSize + " Total: " + nTotalPerStep);

var lTotal = 0l;
var completeTours = (periods - 1) / (long) foundStepSize;
var extraTours = (int) ((periods - 1) % foundStepSize);


lTotal += lDiffs[0];
lTotal += (long)completeTours * (long) nTotalPerStep;
lTotal += lDiffs.Skip(1).Take(extraTours).Sum();

//1555113615633 to low

Console.Out.WriteLine("Total : " + lTotal);


//printPlayingField();

void printPlayingField()
{

    Console.Out.WriteLine("========================");
    for (var nRows = lPlayingField.Count - 1; nRows >= 0; nRows--)
    {
        var n = lPlayingField[nRows];

        for (var i = 0; i < 7; i++)
        {
            var mark = n >> i;
            if ((mark & 1) == 1)
                Console.Out.Write("#");
            else
                Console.Out.Write(".");
        }

        Console.Out.WriteLine();
    }

    Console.Out.Flush();
}

class Block
{
    public int x; // left right
    public int y; // down

    public List<Tuple<int, int>> coordinates = new List<Tuple<int, int>>();
    private int width;
    private int height;

    public Block(int nr, int x, int y)
    {
        if (nr == 0)
        {
            // ####
            coordinates.Add(new Tuple<int, int>(0, 0));
            coordinates.Add(new Tuple<int, int>(1, 0));
            coordinates.Add(new Tuple<int, int>(2, 0));
            coordinates.Add(new Tuple<int, int>(3, 0));
            this.width = 4;
            this.height = 1;
        }
        else if (nr == 1)
        {
            // .#.
            // ### 
            // .#.
            coordinates.Add(new Tuple<int, int>(0, 1));
            coordinates.Add(new Tuple<int, int>(1, 0));
            coordinates.Add(new Tuple<int, int>(1, 1));
            coordinates.Add(new Tuple<int, int>(1, 2));
            coordinates.Add(new Tuple<int, int>(2, 1));
            this.width = 3;
            this.height = 3;
        }
        else if (nr == 2)
        {
            // ..#
            // ..#
            // ###
            coordinates.Add(new Tuple<int, int>(0, 0));
            coordinates.Add(new Tuple<int, int>(1, 0));
            coordinates.Add(new Tuple<int, int>(2, 0));
            coordinates.Add(new Tuple<int, int>(2, 1));
            coordinates.Add(new Tuple<int, int>(2, 2));

            this.width = 3;
            this.height = 3;
        }
        else if (nr == 3)
        {
            // #
            // #
            // #
            // #
            coordinates.Add(new Tuple<int, int>(0, 0));
            coordinates.Add(new Tuple<int, int>(0, 1));
            coordinates.Add(new Tuple<int, int>(0, 2));
            coordinates.Add(new Tuple<int, int>(0, 3));

            this.width = 1;
            this.height = 4;
        }
        else if (nr == 4)
        {
            // ##
            // ##
            coordinates.Add(new Tuple<int, int>(0, 0));
            coordinates.Add(new Tuple<int, int>(1, 0));
            coordinates.Add(new Tuple<int, int>(0, 1));
            coordinates.Add(new Tuple<int, int>(1, 1));

            this.width = 2;
            this.height = 2;
        }

        this.x = x;
        this.y = y;
    }

    public void MoveLeft(List<int> lPlayingField)
    {
        var newX = this.x - 1;
        if (CheckMove(lPlayingField, newX, this.y))
        {
            this.x = newX;
        }
    }


    public void MoveRight(List<int> lPlayingField)
    {
        var newX = this.x + 1;
        if (CheckMove(lPlayingField, newX, this.y))
        {
            this.x = newX;
        }
    }

    public bool MoveDown(List<int> lPlayingField)
    {
        var newY = this.y - 1;
        if (CheckMove(lPlayingField, this.x, newY))
        {
            this.y = newY;
            return true;
        }
        return false;
    }

    public bool CheckMove(List<int> lPlayingField, int x, int y)
    {
        foreach (var coordinate in coordinates)
        {
            var newX = coordinate.Item1 + x;
            var newY = coordinate.Item2 + y;

            if (newX < 0 || newX > 6)
                return false;

            if (newY < 0)
                return false;


            if (lPlayingField.Count > newY)
            {
                var checkRow = lPlayingField[newY];
                var bOccupied = ((checkRow >> newX) & 1) == 1;

                if (bOccupied)
                    return false;
            }
        }

        return true;
    }

    public void ApplyMove(List<int> lPlayingField)
    {
        foreach (var coordinate in coordinates)
        {
            var newX = coordinate.Item1 + this.x;
            var newY = coordinate.Item2 + this.y;

            while (lPlayingField.Count <= newY)
            {
                lPlayingField.Add(0);
            }


            var checkRow = lPlayingField[newY];
            var rowVal = 1 << newX;

            lPlayingField[newY] = checkRow + rowVal;

        }
    }
}


