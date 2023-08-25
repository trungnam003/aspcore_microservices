using Shared.SeedWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Inventory
{
    public class GetInventoryPagingQuery : PagingRequestParameters
    {
        public string ItemNo() => _itemNo;

        private string _itemNo;

        public void SetItemNo(string itemNo)
        {
            _itemNo = itemNo;
        }  

        public string? SearchTerm { get; set; }
    }
}
