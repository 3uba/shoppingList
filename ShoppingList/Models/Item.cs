using Newtonsoft.Json;

namespace ShoppingList.Models;

public class Item
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("quantity")]
    public string Quantity { get; set; }
    
    [JsonProperty("mark")]
    public bool Mark { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }

    public Item () {}
    public Item(string name, string quantity, string type = "Other", bool mark = false)
    {
        Name = name;
        Quantity = quantity;
        Type = type;
        Mark = mark;
    }
}