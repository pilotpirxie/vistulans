using System;
using System.Collections.Generic;

[Serializable]
public class LevelConfig
{
    public List<Level> levels;
}

[Serializable]
public class Level
{
    public string title;
    public int background;
    public List<VertexProxy> vertexProxies;
    public List<Connection> connections;
}

[Serializable]
public class Connection
{
    public int a;
    public int b;
}

[Serializable]
public class VertexProxy
{
    public int id, type, x, y, level, owner, power;
}