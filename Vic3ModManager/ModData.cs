using System.Collections.Generic;

public class ModData
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string Version { get; set; }
    public string SupportedGameVersion { get; set; }
    public string ShortDescription { get; set; }
    public List<string> Tags { get; set; }
    public List<object> Relationships { get; set; }
    public GameCustomData GameCustomData { get; set; }

    public ModData()
    {
        Tags = new List<string>();
        Relationships = new List<object>();
        GameCustomData = new GameCustomData();
    }
}

public class GameCustomData
{
    public bool MultiplayerSynchronized { get; set; }
}
