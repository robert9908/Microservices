using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedMessages
{
   public record OrderPlaced(Guid OrderId, int Quantity);
   public record InventoryReserved(Guid OrderId);
   public record PaymentCompleted(Guid OrderId); 
}
