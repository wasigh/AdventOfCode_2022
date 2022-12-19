
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        //var sFileName = "example.in";
        var sFileName = "data.in";
        var sImageFileName = new FileInfo(sFileName).Name + ".png";

        int[,] data = new int[300,1000];

        var sLines = File.ReadAllLines(sFileName);

        var maxRows = 0;

        foreach(var sLine in sLines)
        {
            var sLineParts = sLine.Split(" -> ");
                
            for (var i = 0; i < sLineParts.Length - 1; i++)
            {
                var sStartPoint = sLineParts[i];
                var sEndPoint = sLineParts[i + 1];

                Console.Out.WriteLine($"Drawing line from {sStartPoint} to {sEndPoint}");
                var maxR = drawLine(data, sStartPoint, sEndPoint);

                maxRows = Math.Max(maxRows, maxR);
            }
        }

        // drawFloor
        drawLine(data, $"0,{maxRows+2}", $"999,{maxRows+2}");

        Console.Out.WriteLine($"MaxRows: {maxRows}");

        while(data[0, 500] == 0)
        {
            var r = -1;
            var c = 500;

            while(true)
            {
                // check down
                if (data[r + 1, c] == 0)
                {
                    r+= 1;
                }
                // check left
                else if (data[r + 1, c - 1] == 0)
                {
                    r += 1;
                    c -= 1;
                }
                else if (data[r + 1, c + 1] == 0)
                {
                    r += 1;
                    c += 1;
                }
                else
                {
                    data[r, c] = 2;
                    break;
                }
            }
        }    


        // var rnd = new Random();
        // int w = 500;
        // int h = 100;
        // for (var i = 0; i < 5000; i++)
        // {
        //     var x = rnd.Next(w);
        //     var y = rnd.Next(h);

        //     data[x,y] = rnd.Next(4);
        // }
        
        saveBitMap(data, sImageFileName);
    }

    private static int drawLine(int[,] data, string sStartPoint, string sEndPoint)
    {
        var nStart = sStartPoint.Trim().Split(",").Select(a => int.Parse(a)).ToList();
        var nEnd = sEndPoint.Trim().Split(",").Select(a => int.Parse(a)).ToList();

        var startC = nStart[0];
        var startR = nStart[1];

        var endC = nEnd[0];
        var endR = nEnd[1];

        var rMin = Math.Min(startR, endR);
        var rMax = Math.Max(startR, endR);

        var cMin = Math.Min(startC, endC);
        var cMax = Math.Max(startC, endC);

        for (var r = rMin; r <= rMax; r++ )
        {
            for (var c = cMin; c <= cMax; c++ )
            {
                data[r, c] = 1;
            }   
        }

        return rMax;
    }

    public static void saveBitMap(int[,] data, string sFileName){

        var nCount = 0;

        int w = data.GetLength(1);
        int h = data.GetLength(0);

        var bm = new Bitmap(w, h);
        
        var colors = new List<Color> (){Color.Black, Color.Red, Color.Green, Color.Blue};

        for (var r = 0; r < h; r++)       
        {
            for (var c = 0; c < w; c++)
            {
                var nColorIndex = data[r,c];
                if (nColorIndex == 2)
                {
                    nCount ++;
                }

                var color = colors[nColorIndex];
                bm.SetPixel(c,r,color);
            }
        }

        bm.Save(sFileName);

        Console.Out.WriteLine(nCount);
    }
}