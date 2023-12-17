using MediiDeProgramare_Mesesan_Daria_lab7.Models;
using Plugin.LocalNotification;

namespace MediiDeProgramare_Mesesan_Daria_lab7;
public partial class ShopPage : ContentPage
{

    public ShopPage()
    {
        InitializeComponent();

    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat" };
        var location = locations?.FirstOrDefault();
        // var myLocation = await Geolocation.GetLocationAsync();
        var myLocation = new Location(46.7731796289, 23.6213886738);
        var distance = myLocation.CalculateDistance(location,
       DistanceUnits.Kilometers);
        if (distance < 4)
        {
            var request = new Plugin.LocalNotification.NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new Plugin.LocalNotification.NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            await LocalNotificationCenter.Current.Show(request).ConfigureAwait(false);
        }
        await Map.OpenAsync(location, options);
    }
}
