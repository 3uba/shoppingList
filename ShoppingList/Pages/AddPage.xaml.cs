using System.Text;
using Newtonsoft.Json;
using ShoppingList.Models;
using ShoppingList.Views;

namespace ShoppingList.Pages;

public partial class AddPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    
    public AddPage()
    {
        InitializeComponent();
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        var name = this.name.Text;
        var quantity = this.qunatity.Text;
        var type = this.type.Text;

        var newItem = new Item(name, quantity, type);

        try
        {
            var json = JsonConvert.SerializeObject(newItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:3000/api/list", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Item added successfully!", "OK");
                this.name.Text = "";
                this.qunatity.Text = "";
                this.type.Text = "";
                ItemsList.LoadData();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", errorMessage, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}