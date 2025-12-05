using SkiaSharp;

namespace Backend.Application.Avatars
{
    public class AvatarGenerator
    {
        public byte[] GenerateAvatar(string? username, int size = 128)
        {
            string initials = GetInitials(username);

            using var bitmap = new SKBitmap(size, size);
            using var canvas = new SKCanvas(bitmap);

            var paint = new SKPaint
            {
                Color = SKColors.Transparent,
                IsAntialias = true
            };
            canvas.DrawCircle(size / 2, size / 2, size / 2, paint);

            // Text
            var textPaint = new SKPaint
            {
                Color = SKColor.Parse("#3F51B5"),
                TextAlign = SKTextAlign.Center,
                TextSize = size * 0.5f,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial")
            };

            canvas.DrawText(initials, size / 2, (size / 2) + (textPaint.TextSize / 3), textPaint);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return data.ToArray();
        }

        private string GetInitials(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return "?";
            }

            var parts = username.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                return parts[0].Substring(0, 1).ToUpper();
            }

            return (parts[0][0].ToString() + parts[1][0]).ToUpper();
        }
    }
}
