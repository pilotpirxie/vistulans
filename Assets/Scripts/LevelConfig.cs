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
    public List<VertexConfig> verticies;
    public List<EdgeConfig> edges;
}

[Serializable]
public class EdgeConfig
{
    public int a;
    public int b;
}

[Serializable]
public class VertexConfig
{
    public int id, type, x, y, level, owner, power;
}