using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MapsDemo.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        bool hasLocationPermission;
        public MapPage()
        {
            InitializeComponent();
            GetPermission();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var geoLocator = CrossGeolocator.Current;
            geoLocator.PositionChanged += GeoLocator_PositionChanged;

        }

        private void GeoLocator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            GetUserlocation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
        private async void GetPermission()
        {
            //It will check permission status
            var Status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            if (Status !=PermissionStatus.Granted) {
                // it will show a dialog to allow or denied
                await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse);
            }
            var result = CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);

            if (result.Result.ContainsKey(Permission.LocationWhenInUse))
            {
                Status = result.Result[Permission.LocationWhenInUse];
            }
            if (Status == PermissionStatus.Granted)
            {
                locationMap.IsShowingUser = true;
                hasLocationPermission = true;
            }
            else {
                hasLocationPermission = false;
                await DisplayAlert("Need Location Permission","With Out this Permission can't display your current or latest Location on Map","Ok");
            }

        }

       private void GetUserlocation() {
            if (hasLocationPermission)
            {
                var geoLocator = CrossGeolocator.Current;
                var postion = geoLocator.GetPositionAsync().Result;
                MoveMap(postion);
            }
        }

        private void MoveMap(Plugin.Geolocator.Abstractions.Position position) {

            var center = new Position(position.Latitude, position.Latitude);
            var span = new MapSpan(center, 1, 1);
            
            locationMap.MoveToRegion(span);


        }
    }
}