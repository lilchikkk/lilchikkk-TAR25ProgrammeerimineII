namespace TARge25Shop.Models.RealEstate
{
    public class FileToDatabaseViewModel
    {
        public Guid ImageId { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }
        public Guid? RealEstateId { get; set; }
        public string Image => ImageData == null
            ? null
            : $"data:image/jpeg;base64,{Convert.ToBase64String(ImageData)}";
    }
}
