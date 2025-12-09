namespace Backend.Controller
{
    public static class HttpExtensions
    {
        public static string[] GetUserLanguages(this HttpRequest request)
        {
            return request.GetTypedHeaders()
                .AcceptLanguage
                ?.OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .ToArray() ?? Array.Empty<string>();
        }

        public static string GetBestMatchingUserLanguage(this HttpRequest request)
        {
            string[] supportedLanguages = ["de", "en"];

            string[] languages = request.GetUserLanguages();

            foreach (string language in languages)
            {
                if (supportedLanguages.Contains(language))
                {
                    return language;
                }

                var neutral = language.Split('-')[0];

                if (supportedLanguages.Contains(neutral))
                {
                    return neutral;
                }
            }

            return "en";
        }
    }
}
