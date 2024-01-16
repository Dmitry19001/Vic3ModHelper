using System.Collections.Generic;
#pragma warning disable IDE1006 // Naming Styles (because of JSON)
// Output naming should be the same as in JSON file
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
        tags = [];
        relationships = [];
        game_custom_data = new();
    }
}

public class GameCustomData
{
    public bool multiplayer_synchronized { get; set; }
}
#pragma warning restore IDE1006 // Naming Styles