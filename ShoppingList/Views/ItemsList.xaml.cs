using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class ItemsList : ContentView
{
    public static ObservableCollection<Item> shoppingList = new ObservableCollection<Item>();
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
    
    public static async void LoadData()
    {
        shoppingList.Clear();
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
        int id = (int)button.CommandParameter;

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