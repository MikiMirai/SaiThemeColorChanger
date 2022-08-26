using Newtonsoft.Json;

namespace SaiThemeColorChanger.JSON_Templates
{
    public class Config
    {
        [JsonProperty("current_used_theme")]
        public string CurrentUsedTheme { get; set; }
    }
}