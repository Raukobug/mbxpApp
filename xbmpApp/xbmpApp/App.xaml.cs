using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace xbmpApp
{
    public partial class App : Application
    {
        static GiftcardItemDatabase database;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new CardList());
        }

        public static GiftcardItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new GiftcardItemDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("GiftcardSQLite.db3"));
                }
                return database;
            }
        }

        public int ResumeAtGiftcardId { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
