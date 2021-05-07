using System;

namespace EasyOpc.Contracts.Works
{
    public class GetByTypeAndExternalIdRequest
    {
       public string Type { get; set; }

       public Guid ExternalId { get; set; }
    }
}
