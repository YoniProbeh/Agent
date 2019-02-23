using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.AspNetCore.Http;
using Server.Resources.Shared.DTOs;

namespace Server.Resources.Shared.Models
{
    public class Image : BaseModel<Image>
    {
        public string FileName { get; set; }

    }
}