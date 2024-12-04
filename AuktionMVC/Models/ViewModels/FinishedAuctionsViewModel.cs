// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using AuktionMVC.Core.Models;

// namespace AuktionMVC.Models.ViewModels
// {
//     public class FinishedAuctionsViewModel
//     {
//         public List<Sale> BoughtItems { get; set; } = new();
//         public List<Sale> SoldItems { get; set; } = new();

        

//         public static FinishedAuctionsViewModel Create(List<Sale> boughtItems, List<Sale> soldItems)
//         {
//             var model = new FinishedAuctionsViewModel();

//             model.BoughtItems = boughtItems;
//             model.SoldItems = soldItems;

//             return model;
//         }
//     }
// }