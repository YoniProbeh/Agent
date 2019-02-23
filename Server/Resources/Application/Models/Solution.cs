using System;
using Server.Resources.Shared.Models;

namespace Server.Resources.Application.Models
{
    public class Solution : BaseModel<Solution>
    {
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}