using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xbmpApp.model;

namespace xbmpApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardList : ContentPage
    {
        public ObservableCollection<GiftcardItem> Giftcards = new ObservableCollection<GiftcardItem>();
        private GiftcardItem oldCard;

        public CardList()
        {
            InitializeComponent();
            AddCard.Clicked += GotoNewCardPage;
            CreateList();
            ListView.IsPullToRefreshEnabled = true;
            ListView.ItemSelected += OnSelection;
            ListView.Refreshing += OnRefresh;
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            var card = (GiftcardItem)e.SelectedItem;
            HideOrShow(card);
            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }

        public void HideOrShow(GiftcardItem card)
        {
            if(oldCard == card)
            {
                card.IsVisible = !card.IsVisible;
                UpdateCardList(card);
            }
            else
            {
                if(oldCard != null)
                {
                    oldCard.IsVisible = false;
                    UpdateCardList(oldCard);
                }
                card.IsVisible = true;
                UpdateCardList(card);
            }
            oldCard = card;
        }

        public void UpdateCardList(GiftcardItem card)
        {
            int index = Giftcards.IndexOf(card);
            Giftcards.Remove(card);
            Giftcards.Insert(index, card);
        }

        private async void CreateList()
        {
            var list = await App.Database.GetItemsAsync();
            Giftcards.Clear();
            oldCard = null;
            foreach(var l in list)
            {
                l.Image = byteArrayToImage(l.ImageInBytes).Source;
                Giftcards.Add(l);
            }
            if(Giftcards.Count < 1)
            {
                NoCards.Text = "Ingen kort tilføjet klik på '+' for at tilføje dit første kort.";
            }
            else
            {
                NoCards.Text = "";
            }

            ListView.ItemsSource = Giftcards;
        }

        void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView) sender;
            CreateList();
            //make sure to end the refresh state
            list.IsRefreshing = false;
        }

        private void GotoNewCardPage(object sender, EventArgs e)
        {
            var page = new NavigationPage(new NewCardPage());
            Navigation.PushAsync(page);
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            Image image = new Image();
            image.Source = ImageSource.FromStream(() => new MemoryStream(byteArrayIn));
            return image;
        }
    }
}

