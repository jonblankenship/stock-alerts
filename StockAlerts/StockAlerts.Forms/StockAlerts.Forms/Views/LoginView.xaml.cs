using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockAlerts.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockAlerts.Forms.Views
{
    public partial class LoginView : ContentPage
    {
        private bool _animate;

        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var content = this.Content;
            this.Content = null;
            this.Content = content;

            var vm = BindingContext as LoginViewModel;
            if (vm != null)
            {
            }
        }
    }
}