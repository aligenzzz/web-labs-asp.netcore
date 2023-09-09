using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web_153505_Bybko.Models
{
    public class HomeViewModel
    {
        public HomeViewModel() 
        {
            var list = new List<ListDemo>
            {
                new(1, "Item 1"),
                new(2, "Item 2"),
                new(3, "Item 3")
            };

            SelectList = new SelectList(list, "Id", "Name");
        }
        public SelectList? SelectList { get; set; }
    }
}
