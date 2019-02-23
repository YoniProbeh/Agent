using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Server.Resources.Shared.DTOs
{
    public class BaseDTO
    {
        [Required]
        public string Name { get; set; }
        public string ID { get; set; }
        public bool IsActive { get; set; }

        public BaseDTO() {}
        public BaseDTO(string name, bool isActive)
        {
            this.Name = name;
            this.IsActive = isActive;
        }
        public BaseDTO(string id, string name, bool isActive) : this(name, isActive)
        {
            this.ID = id;
        }
    }
    public class BaseResponseDTO<TSource> : BaseDTO
    {
        public new Guid ID { get; set; }
        public DateTime Created { get; set; }
        public ICollection<TSource> Parents { get; set; } = new Collection<TSource>();
        public ICollection<TSource> Children { get; set; } = new Collection<TSource>();
        
        public BaseResponseDTO() {}
        public BaseResponseDTO(Guid id, DateTime created, string name, bool isActive) : base(name, isActive)
        {
            this.ID = id;
            this.Created = created;
        }
    }
}