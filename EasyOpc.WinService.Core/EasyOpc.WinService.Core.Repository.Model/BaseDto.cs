using EasyOpc.Common.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasyOpc.WinService.Core.Repository.Model
{
    /// <summary>
    /// Object for storage in the database
    /// </summary>
    public abstract class BaseDto : IIdentifiable
    {
        /// <summary>
        /// Object identifier
        /// </summary>
        [Key]
        public Guid Id { get; set; }
    }
}
