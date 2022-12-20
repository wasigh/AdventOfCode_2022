namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static bool printWell = true;

        static void Main(string[] args)
        {
            var sample = File.ReadAllText("example.in");
            var input = File.ReadAllText("data.in");

            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            Part1(ParseInput(sample), 2022);
            Console.WriteLine($"Part 1(sample): {sw.Elapsed}");
            sw.Reset();

            sw.Start();
            Part1(ParseInput(input), 2022);
            Console.WriteLine($"Part 1(input): {sw.Elapsed}");
            sw.Reset();

            sw.Start();
            Part2(sample);
            Console.WriteLine($"Part 2(sample): {sw.Elapsed}");
            sw.Reset();

            sw.Start();
            Part2(input);
            Console.WriteLine($"Part 2(input): {sw.Elapsed}");
            sw.Reset();
        }

        static void Part1(int[] moves, int numRocks)
        {
            var well = new Dictionary<int, HashSet<int>>();

            int rock = 0, round = 0;
            var shape = ShapeToDictionary(Shapes[rock % Shapes.Length]);
            shape = StartShape(well.Count, shape);

            // PrintWell(well, shape, "Start");

            while (rock < numRocks)
            {
                // PrintWell(well, $"Start of round {round}");

                var move = moves[round % moves.Length];
                round++;

                shape = MoveShape(well, shape, move);
                // PrintWell(well, shape, $"Moved {((move < 0) ? '<' : '>')}");

                shape = DropShape(well, shape, out var dropped);
                // PrintWell(well, shape, dropped ? "Dropped." : "Could not drop.");

                if (!dropped)
                {
                    // Merge shape will well.
                    foreach (var row in shape)
                    {
                        if (well.TryGetValue(row.Key, out var columns))
                        {
                            columns.UnionWith(row.Value);
                        }
                        else
                        {
                            well.Add(row.Key, row.Value);
                        }
                    }

                    rock++;

                    // PrintWell(well, $"After {rock} rocks stopped.");
                    if (rock >= numRocks)
                    {
                        // PrintWell(well, new Dictionary<int, HashSet<int>>(), $"rock = {rock}");
                        Console.WriteLine($"After {rock} rocks. Well is {well.Count} rows deep.");
                        break;
                    }

                    // Console.WriteLine($"After {rocks} rocks. Well is {well.Count} rows deep.");
                    shape = ShapeToDictionary(Shapes[rock % Shapes.Length]);
                    shape = StartShape(well.Count, shape);
                }

                // PrintWell(well, shape, "After dropping.");
            }
        }

        static void Part2(string data)
        {
        }

        static Dictionary<int, HashSet<int>> MoveShape(Dictionary<int, HashSet<int>> well, Dictionary<int, HashSet<int>> shape, int move)
        {
            // Move the shape
            var movedShape = shape.ToDictionary(
                row => row.Key,
                row => row.Value.Select(col => col + move).ToHashSet());

            if (movedShape.Values.Any(row => row.Contains(-1) || row.Contains(7)))
            {
                return shape;
            }

            // check for conflicts.
            var blocked = movedShape
                .Where(shapeRow => well.ContainsKey(shapeRow.Key))
                .Where(shapeRow => shapeRow.Value.Intersect(well[shapeRow.Key]).Any())
                .Any();

            return blocked ? shape : movedShape;
        }

        static Dictionary<int, HashSet<int>> DropShape(Dictionary<int, HashSet<int>> well, Dictionary<int, HashSet<int>> shape, out bool dropped)
        {
            dropped = false;

            // Move the shape
            var movedShape = shape.ToDictionary(
                row => row.Key - 1,
                row => row.Value);

            if (movedShape.Keys.Any(row => row < 0))
            {
                return shape;
            }

            // check for conflicts.
            var blocked = movedShape
                .Where(shapeRow => well.ContainsKey(shapeRow.Key))
                .Where(shapeRow => shapeRow.Value.Intersect(well[shapeRow.Key]).Any())
                .Any();

            dropped = !blocked;
            return blocked ? shape : movedShape;
        }

        static void PrintWell(Dictionary<int, HashSet<int>> well, Dictionary<int, HashSet<int>> shape, string text)
        {
            if (printWell)
            {
                Console.WriteLine(text);
                Console.WriteLine(" 0123456 ");
                var maxRow = Math.Max(
                    (shape.Keys.Count == 0) ? 0 : shape.Keys.Max(),
                    (well.Count == 0) ? 0 : well.Keys.Max());
                for (int row = maxRow; row >= Math.Max(maxRow - 20, 0); row--)
                {
                    Console.Write("|");
                    for (int col = 0; col < 7; col++)
                    {
                        var character = (shape.TryGetValue(row, out var r) && r.Contains(col))
                            ? "@"
                            : (well.TryGetValue(row, out var wr) && wr.Contains(col))
                            ? "#"
                            : ".";
                        Console.Write(character);
                    }

                    Console.WriteLine($"| - {row}");
                }

                Console.WriteLine("+-------+");
                Console.WriteLine();
            }

            System.Threading.Thread.Sleep(100);
        }

        static Dictionary<int, HashSet<int>> StartShape(int wellHeight, Dictionary<int, HashSet<int>> shape)
        {
            return shape.ToDictionary(
                kvp => kvp.Key + wellHeight + 3,
                kvp => kvp.Value.Select(col => col + 2).ToHashSet());
        }

        static int[] ParseInput(string data)
        {
            List<int> moves = new List<int>();
            foreach (char move in data)
            {
                switch (move)
                {
                    case '<':
                        moves.Add(-1);
                        break;

                    case '>':
                        moves.Add(1);
                        break;

                    default:
                        break; // WHAT?
                }
            }

            return moves.ToArray();
        }

        static Dictionary<int, HashSet<int>> ShapeToDictionary(string[] shape)
        {
            var dict = new Dictionary<int, HashSet<int>>();
            int row = 0;
            for (int i = shape.Length - 1; i >= 0; i--)
            {
                var hash = new HashSet<int>();
                for (int j = 0; j < shape[i].Length; j++)
                {
                    if (shape[i][j] == '#')
                    {
                        hash.Add(j);
                    }
                }

                if (hash.Count > 0)
                {
                    dict.Add(row, hash);
                }

                row++;
            }

            return dict;
        }

        static readonly string[][] Shapes = new string[][]
        {
            new string[]
            {
                "####"
            },
            new string[]{
                ".#.",
                "###",
                ".#.",
            },
            new string[]{
                "..#",
                "..#",
                "###",
            },
            new string[]{
                "#",
                "#",
                "#",
                "#",
            },
            new string[]{
                "##",
                "##",
            }
        };
    }
}