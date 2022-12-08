using System.Linq;

var sLines = File.ReadAllLines("data.in");


var matrixW = sLines[0].Trim().Length;
var matrixH = sLines.Length;

var rowW = matrixW + 2;
var rowH = matrixH + 2;

// create matrix with extra zero padding
var cellCount = rowH * rowW;
var heightMatrix = new int[cellCount];

for (var n = 0; n < heightMatrix.Length; n++)
{
    heightMatrix[n] = -1;
}

var visibilityMatrix = new bool[cellCount];

for (var r = 0; r < sLines.Length; r++)
{
    var sLine = sLines[r].Trim();
    for (var c = 0; c < sLine.Length; c++)
    {
        var nIndex = calcIndex(r, c);
        var nVal = int.Parse(sLine[c].ToString());

        heightMatrix[nIndex] = nVal;
    }
}


renderMatrix();

var nVisible = 0;
// calc visibility
for (var r = 0; r < matrixH; r++)
{
    for (var c = 0; c < matrixW; c++)
    {
        var rowHeights = calcRowHeight(r, c);
        var columnHeight = calcColumnHeight(r, c);

        var nIndex = calcIndex(r, c);
        var nVal = heightMatrix[nIndex];

        var visibility = rowHeights.Any((a) => a < nVal) || columnHeight.Any((a) => a < nVal);
        visibilityMatrix[nIndex] = visibility;
        if (visibility) nVisible++;
    }
}
renderVisibleMatrix();
Console.Out.WriteLine(nVisible);

var nMaxSize = 0;
// calc visibility
for (var r = 0; r < matrixH; r++)
{
    for (var c = 0; c < matrixW; c++)
    {   
        if (r == 3 && c == 2)
        {
            var no =1;
        }

        var treeIndex = calcIndex(r,c);
        var treeHeight = heightMatrix[treeIndex];

        var nRowBefore = 0;
        for (var rb = r - 1; rb >=0; rb--)
        {
            nRowBefore++;
            var nIndex = calcIndex(rb, c);
            var val = heightMatrix[nIndex];
            if (val >= treeHeight)
                break;
        }

        var nRowAfter = 0;
        for (var rb = r + 1; rb < matrixW; rb++)
        {
            nRowAfter++;
            var nIndex = calcIndex(rb, c);
            var val = heightMatrix[nIndex];
            if (val >= treeHeight)
                break;
        }


        var nColBefore = 0;
        for (var cb = c - 1; cb >=0; cb--)
        {
            nColBefore++;
            var nIndex = calcIndex(r, cb);
            var val = heightMatrix[nIndex];
            if (val >= treeHeight)
                break;
        }

        var nColAfter = 0;
        for (var cb = c + 1 ; cb < matrixH; cb++)
        {
            nColAfter++;
            var nIndex = calcIndex(r, cb);
            var val = heightMatrix[nIndex];
            if (val >= treeHeight)
                break;
        }

        
        var nTotal = nRowBefore * nRowAfter * nColBefore * nColAfter;
        nMaxSize = Math.Max(nMaxSize, nTotal);
    }
}

Console.Out.WriteLine(nMaxSize);



List<int> calcColumnHeight(int r, int c)
{
    var before = -1;
    var after = -1;

    for (var n = 0; n < matrixW; n++)
    {
        if (n == r)
        {
            continue;
        }

        var nIndex = calcIndex(n, c);
        var val = heightMatrix[nIndex];

        if (n < r)
            before = Math.Max(before, val);
        if (n > r)
            after = Math.Max(after, val);
    }
    return new List<int>() { before, after };
}

List<int> calcRowHeight(int r, int c)
{
    var before = -1;
    var after = -1;
    for (var n = 0; n < matrixW; n++)
    {
        if (n == c)
        {
            continue;
        }

        var nIndex = calcIndex(r, n);
        var val = heightMatrix[nIndex];

        if (n < c)
            before = Math.Max(before, val);
        if (n > c)
            after = Math.Max(after, val);
    }

    return new List<int>() { before, after };
}




void renderMatrix()
{
    for (var n = 0; n < heightMatrix.Length; n++)
    {
        if (n > 0 && n % rowW == 0)
        {
            Console.Out.WriteLine();
        }
        Console.Out.Write(heightMatrix[n]);
    }

    Console.Out.WriteLine();
    Console.Out.Flush();
}

void renderVisibleMatrix()
{
    for (var n = 0; n < visibilityMatrix.Length; n++)
    {
        if (n > 0 && n % rowW == 0)
        {
            Console.Out.WriteLine();
        }
        Console.Out.Write(visibilityMatrix[n] ? "1" : "-");
    }

    Console.Out.WriteLine();
    Console.Out.Flush();
}


int calcIndex(int row, int col)
{
    var nRow = row + 1;
    var nCol = col + 1;

    var index = nRow * rowW;
    index += nCol;

    return index;
}

