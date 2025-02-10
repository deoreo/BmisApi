namespace BmisApi.Services
{
    public class PictureService
    {
        private readonly string _uploadPath;
        private readonly FileValidationOptions _validationOptions;

        public PictureService(IConfiguration configuration)
        {
            _uploadPath = configuration["Storage:UploadPath"] ?? "uploads";

            _validationOptions = new FileValidationOptions();
            configuration.GetSection("FileValidation").Bind(_validationOptions);
        }



        public async Task<string> SavePictureFileAsync(IFormFile picture, string relativePath)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(picture.FileName)}";
            var relativePicturePath = Path.Combine(relativePath, fileName);
            var fullPath = Path.Combine(_uploadPath, relativePicturePath);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }

            return relativePicturePath;
        }

        public void DeletePictureFile(string relativePath)
        {
            var fullPath = Path.Combine(_uploadPath, relativePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public string CreatePicturePath(string folderName)
        {
            var relativePath = Path.Combine(folderName, DateTime.UtcNow.ToString("yyyy-MM"));
            var fullPath = Path.Combine(_uploadPath, relativePath);
            Directory.CreateDirectory(fullPath);
            return relativePath;
        }

        public class FileValidationException : Exception
        {
            public FileValidationException(string message) : base(message) { }
        }

        public class FileValidationOptions
        {
            public long MaxFileSizeInMb { get; set; } = 10; // Default 10MB
            public string[] AllowedExtensions { get; set; } = new[]
            { 
                // Images
                ".jpg", ".jpeg", ".png", ".gif" 
                //// Documents
                //".pdf", ".doc", ".docx", 
                //// Other
                //".txt"
            };
        }

        public void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileValidationException("File is empty");

            // Check file size
            var fileSizeInMb = file.Length / 1024f / 1024f;
            if (fileSizeInMb > _validationOptions.MaxFileSizeInMb)
                throw new FileValidationException($"File size exceeds maximum allowed size of {_validationOptions.MaxFileSizeInMb}MB");

            // Check file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_validationOptions.AllowedExtensions.Contains(extension))
                throw new FileValidationException($"File type {extension} is not allowed");

            // Validate file content
            if (!IsValidFileContent(file))
                throw new FileValidationException("File content is not valid");
        }

        private bool IsValidFileContent(IFormFile file)
        {
            try
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                using var reader = new BinaryReader(file.OpenReadStream());
                var signatures = GetFileSignatures(extension);
                var headerBytes = reader.ReadBytes(signatures.Max(s => s.Length));

                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
            catch
            {
                return false;
            }
        }

        private byte[][] GetFileSignatures(string extension) => extension switch
        {
            ".jpg" or ".jpeg" => new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
            ".png" => new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
            ".gif" => new[] { new byte[] { 0x47, 0x49, 0x46, 0x38 } },
            ".pdf" => new[] { new byte[] { 0x25, 0x50, 0x44, 0x46 } },
            // Add more signatures as needed
            _ => new[] { Array.Empty<byte>() }
        };

    }
}
