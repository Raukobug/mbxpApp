using SQLite;
using Xamarin.Forms;

namespace xbmpApp.model
{
    public class GiftcardItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Price { get; set; }
        public string ExpirationDate { get; set; }
        public byte[] ImageInBytes { get; set; }
        [Ignore]
        public bool IsVisible { get; set; }
        [Ignore]
        public ImageSource Image {get; set;}
    }
}
