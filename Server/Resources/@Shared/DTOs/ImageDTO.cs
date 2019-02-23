using System;
using Microsoft.AspNetCore.Http;

namespace Server.Resources.Shared.DTOs
{
    public class ImageDTO : BaseDTO
    {
        public IFormFile File { get; set; }
        public string ProfileID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public ImageDTO() {}
        public ImageDTO(string name, bool isActive) : base(name, isActive) {}
        public ImageDTO(string id, string name, bool isActive) : base(id, name, isActive) {}
        public ImageDTO(IFormFile file, string profileID, string fileName, string filePath, string id, string name, bool isActive) : this(id, name, isActive)
        {
            this.File = file;
            this.ProfileID = profileID;
            this.FileName = fileName;
            this.FilePath = filePath;
        }
    }
    public class ImageResultDTO : BaseResponseDTO<ImageResultDTO>
    {
        public Guid ProfileID { get; set; }

        public ImageResultDTO() {}
        public ImageResultDTO(Guid id, DateTime created, string name, bool isActive) : base(id, created, name, isActive) {}
    }
}