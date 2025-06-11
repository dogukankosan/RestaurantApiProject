namespace RestaurantAPI.Helpers
{
    internal static class ValidatorMethod
    {
        private const int MaxImageSize = 5 * 1024 * 1024;
        internal static bool BeValidImage(byte[] image)
        {
            if (image == null || image.Length == 0 || image.Length > MaxImageSize)
                return false;
            byte[] header = image.Take(8).ToArray();
            if (header.Take(3).SequenceEqual(new byte[] { 0xFF, 0xD8, 0xFF })) return true;
            if (header.Take(4).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47 })) return true;
            if (header.Take(3).SequenceEqual(new byte[] { 0x47, 0x49, 0x46 })) return true;
            return false;
        }
        internal static bool BeValidName(string name)
        {
            string[] parts = name?.Trim().Split(' ');
            return parts != null && parts.Length >= 2 && parts.All(p => p.Length >= 2);
        }
        internal static bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var u)
                   && (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps);
        }
        internal static bool BeValidHttpUrl(string url) =>
            Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}