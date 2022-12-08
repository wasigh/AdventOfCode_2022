var sLines = File.ReadAllLines("data.in");

var currentNode = new FsItem() { Name = "/" };
var rootNode = currentNode;

foreach (var sLine in sLines)
{
    var lParts = sLine.Split(" ");

    var sCmd = lParts[0];

    switch (sCmd)
    {
        case "$":
            currentNode = currentNode.ExecuteCommand(lParts);
            break;
        case "dir":
            currentNode.AddDir(lParts[1]);
            break;
        default:
            var lSize = long.Parse(lParts[0]);
            var sName = lParts[1];
            currentNode.AddFile(sName, lSize);
            break;
    }
}


writeOutput(rootNode, 0);

var filteredDirs = FilterDirMaxSize(rootNode, 100000);
long lTotal = 0;

foreach(var fDir in filteredDirs)
{
    Console.Out.WriteLine(fDir.ToString());
    lTotal += fDir.GetSize();
}
Console.Out.WriteLine("Total= " + lTotal);



var lMaxDiskSize = 70000000;
var lMinDiskSize = 30000000;

var totalInUse = rootNode.GetSize();
var freeSpace = lMaxDiskSize - totalInUse;
Console.Out.WriteLine("FreeSpace: " + freeSpace);
var toDelete = lMinDiskSize - freeSpace;
Console.Out.WriteLine("To delete: " + toDelete);

var allDirs = FilterDirMaxSize(rootNode, lMaxDiskSize);

var sortedDirs = allDirs.ToList();
sortedDirs.Sort();
var delDir = sortedDirs.First(a => a.GetSize() >= toDelete);
Console.Out.WriteLine(delDir.ToString() + " -  " + delDir.GetSize());


IEnumerable<FsItem> FilterDirMaxSize(FsItem node, long size)
{
    if (node.IsDir && node.GetSize() <= size)
    {
        yield return node;
    }

    if (node.Children != null )
    {
        foreach(var childNode in node.Children)
        {
            var lItems = FilterDirMaxSize(childNode, size);
            foreach(var item in lItems)
            {
                yield return item;
            }
        }
    }

}

void writeOutput(FsItem node, int indent)
{
    var sIndent = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t".Substring(0, indent);
    Console.WriteLine (sIndent + node.ToString());

    if (node.Children != null )
    {
        node.Children.ForEach(n => writeOutput(n, indent + 1));
    }
}

class FsItem : IComparable<FsItem>
{
    public string Name { get; set; } = "";

    public List<FsItem> Children { get; set; } = new List<FsItem>();

    public FsItem ParentNode { get; set; } = null;

    protected long Size { get; set; } = -1;

    public bool IsDir {get;set;} = true;

    public FsItem AddDir(string sName)
    {
        var dir = new FsItem() { Name = sName };
        dir.ParentNode = this;
        this.Children.Add(dir);
        return dir;
    }

    public FsItem AddFile(string sName, long sSize)
    {
        var file = new FsFile() { Name = sName, Size = sSize };
        file.ParentNode = this;
        file.IsDir = false;
        this.Children.Add(file);
        return file;
    }

    public long GetSize()
    {
        if (Size < 0)
        {
            Size = Children.Sum(c => c.GetSize());
        }
        return Size;
    }

    public override string ToString()
    {
        return string.Format($"- {this.Name} (dir)");
    }

    public FsItem ExecuteCommand(string[] lParts)
    {
        var cmd = lParts[1];
        switch (cmd)
        {
            case "ls":
                return this;
            case "cd":
                var sDirName = lParts[2];
                if (sDirName == "..")
                {
                    return this.ParentNode;
                }
                else if (sDirName == "/")
                {
                    return this; // should be root...
                }
                else{
                    return this.Children.Where(c => c.Name == sDirName).First();
                }
        }

        return this;
    }

    int IComparable<FsItem>.CompareTo(FsItem? other)
    {
        return  this.GetSize().CompareTo(other?.GetSize());
    }
}

class FsFile : FsItem
{

    public new long GetSize()
    {
        return Size;
    }

    public override string ToString()
    {
        return string.Format($"- {this.Name} (file, size={this.Size})");
    }
}