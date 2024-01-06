using System.Collections.Generic;

public class ModData
{
    public string name { get; set; }
    public string id { get; set; }
    public string version { get; set; }
    public string supported_game_version { get; set; }
    public string short_description { get; set; }
    public List<string> tags { get; set; }
    public List<object> relationships { get; set; }
    public GameCustomData game_custom_data { get; set; }

    public ModData()
    {
        tags = new List<string>();
        relationships = new List<object>();
        game_custom_data = new GameCustomData();
    }
}

public class GameCustomData
{
    public bool multiplayer_synchronized { get; set; }
}
