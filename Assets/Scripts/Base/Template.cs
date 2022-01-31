using System;

[Serializable]
public class Template
{
    public Layer[] layers;
}
[Serializable]
public class Layer
{
    public string type;
    public string path;
    public Placement[] placement;
    public Operation[] operations;
}
[Serializable]
public class Placement
{
    public Position position;
}
[Serializable]
public class Position
{
    public int x;
    public int y;
    public int width;
    public int height;
}
[Serializable]
public class Operation
{
    public string name;
    public string argument;
}

