using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Server.Resources.Shared.Models
{
    public class BaseModel<TSource>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public ICollection<TSource> Parents { get; set; } = new Collection<TSource>();
        public ICollection<TSource> Children { get; set; } = new Collection<TSource>();

        public BaseModel()
        {
            this.ID = Guid.NewGuid();
            this.IsActive = true;
            this.Created = DateTime.Now;
        }
        public BaseModel(Guid iD, string name, bool isActive, DateTime created)
        {
            this.ID = iD;
            this.Name = name;
            this.IsActive = isActive;
            this.Created = created;
        }
    }
}