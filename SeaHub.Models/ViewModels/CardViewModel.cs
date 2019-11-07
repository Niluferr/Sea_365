using System;
using System.Collections.Generic;
using System.Text;

namespace SeaHub.Models.ViewModels
{
    public class CardViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public IList<Service> ServiceList { get; set; }

    }
}
