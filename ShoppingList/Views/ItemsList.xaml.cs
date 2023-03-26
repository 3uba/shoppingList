using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SQLite;
using Newtonsoft.Json;

namespace ShoppingList.Views;

public partial class ItemsList : ContentView
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public bool Mark { get; set; }
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

    public ObservableCollection<Item> shoppingList = new ObservableCollection<Item>();
    public ObservableCollection<Item> ShoppingList
    {
        get { return shoppingList; }
    }
    public ItemsList()
    {
        InitializeComponent();
        LoadData();
        ShoppingListView.ItemsSource = shoppingList;
    }

    
    public async void LoadData()
    {
        try
        {
            string apiUrl = "http://localhost:3000/api/list";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(responseData);

                foreach (Item item in items)
                {
                    shoppingList.Add(item);
                }
            }
            else
            {
                throw new Exception("Failed to fetch data from API.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    
    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        Item item = button.BindingContext as Item;
        int id = (int)button.CommandParameter;

        using (HttpClient httpClient = new HttpClient())
        {
            string url = $"http://localhost:3000/api/list/{id}";
            HttpResponseMessage response = await httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                ShoppingList.Remove(item);
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
            }
        }
    }
    
    public async void ToggleButton_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        Item item = button.BindingContext as Item;
        int id = item.Id;

        using (HttpClient httpClient = new HttpClient())
        {
            string url = $"http://localhost:3000/api/list/{id}";
        
            var data = new { 
                name = item.Name,
                quantity = item.Quantity,
                type = item.Type,
                mark = !item.Mark
            };
        
            string jsonData = JsonConvert.SerializeObject(data);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                item.Mark = !item.Mark;
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error: " + errorMessage);
            }
        }
    }

}