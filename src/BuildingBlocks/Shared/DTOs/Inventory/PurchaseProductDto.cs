using Shared.Enums.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Shared.DTOs.Inventory
{
    public class PurchaseProductDto
    {
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public string DocumentNo { get; set; }
        public string ExternalDocumentNo { get; set; }
        public EDocumentType DocumentType { get; set; } = EDocumentType.Purchase;
    }
}
