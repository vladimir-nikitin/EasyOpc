using System.Collections.Generic;

namespace EasyOpc.Contracts.Opc.Da
{
    public class AddOpcDaGroupRequest : OpcDaGroupData
    {
        public IEnumerable<OpcDaItemData> OpcDaItems { get; set; }
    }
}
