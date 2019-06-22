using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockAlerts.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockAlerts.App.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var content = this.Content;
            this.Content = null;
            this.Content = content;

            var vm = BindingContext as LoginPageViewModel;
            if (vm != null)
            {
            }
        }
    }
}