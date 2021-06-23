using System.Collections.Generic;

namespace EasyOpc.Contracts.Opc.Ua
{
    public class AddOpcUaGroupRequest : OpcUaGroupData
    {
        public IEnumerable<OpcUaItemData> OpcUaItems { get; set; }
    }
}
