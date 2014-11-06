using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT
{
    namespace Codekit
    {
        namespace PAYMENT
        {
            public class ownershipEvent
            {
                public string networkOperatorId
                { get; set; }

                public string ownerIdentifier
                { get; set; }

                public string purchaseDate
                { get; set; }

                public string productIdentifier
                { get; set; }

                public string purchaseActivityIdentifier
                { get; set; }

                public string instanceIdentifier
                { get; set; }

                public string minIdentifier
                { get; set; }

                public string reasonCode
                { get; set; }

                public string reasonMessage
                { get; set; }

                public string vendorPurchaseIdentifier
                { get; set; }
            }
        }
    }
}