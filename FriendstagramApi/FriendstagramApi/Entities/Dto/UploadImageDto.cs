namespace FriendstagramApi.Entities.Dto
{
    public class UploadImageDto
    {
        public UploadImageDto(string filePath)
        {
            Success = true;
            Message = filePath;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
