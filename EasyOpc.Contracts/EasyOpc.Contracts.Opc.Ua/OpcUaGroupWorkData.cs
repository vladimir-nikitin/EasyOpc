using EasyOpc.Common.Types;
using System;

namespace EasyOpc.Contracts.Opc.Ua
{
    /// <summary>
    /// OPC.UA group work
    /// </summary>
    public class OpcUaGroupWorkData : IIdentifiable
    {
        /// <summary>
        /// Work id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// OPC.UA group id
        /// </summary>
        public Guid OpcUaGroupId { get; set; }

        /// <summary>
        /// Work name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Work type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Activity flag
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Launch group
        /// </summary>
        public string LaunchGroup { get; set; }

        /// <summary>
        /// Work settings in json format
        /// </summary>
        public string JsonSettings { get; set; }
    }
}
