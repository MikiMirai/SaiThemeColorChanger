using Newtonsoft.Json;

namespace SaiThemeColorChanger.JSON_Templates
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<ColorTheme>(myJsonResponse);
    public class Blue
    {
        public string blue_1 { get; set; }
        public string blue_2 { get; set; }
        public string blue_3 { get; set; }
        public string blue_corner_1 { get; set; }
        public string blue_corner_2 { get; set; }
        public string blue_corner_3 { get; set; }
    }

    public class Brown
    {
        public string brown_1 { get; set; }
        public string brown_2 { get; set; }
        public string brown_3 { get; set; }
        public string brown_4 { get; set; }
        public string brown_5 { get; set; }
        public string brown_6 { get; set; }
        public string brown_7 { get; set; }
        public string brown_8 { get; set; }
        public string brown_9 { get; set; }
        public string brown_10 { get; set; }
    }

    public class ColorWheelFixes
    {
        public bool revert_fix { get; set; }
        public string range_162_to_254_fix { get; set; }
        public string fix_2 { get; set; }
        public string fix_3 { get; set; }
        public string fix_4 { get; set; }
        public string color_wheel { get; set; }
    }
    
    public class CanvasBorderLine
    {
        public string border_line_1 { get; set; }
        public string border_line_2 { get; set; }
    }

    public class CanvasSelectBorder
    {
        public string border_1 { get; set; }
        public string border_2 { get; set; }
        public string border_3 { get; set; }
        public string border_4 { get; set; }
        public string selection_border_1 { get; set; }
        public string selection_border_2 { get; set; }
    }

    public class Corners
    {
        public string corners_1 { get; set; }
        public string corners_2 { get; set; }
        public string corners_3 { get; set; }
        public string corners_4 { get; set; }
        public string corners_5 { get; set; }
        public string corners_6 { get; set; }
        public string corners_7 { get; set; }
    }

    public class PanelBorders
    {
        public string borders_1 { get; set; }
        public string borders_2 { get; set; }
        public string borders_3 { get; set; }
        public string borders_4 { get; set; }
        public string borders_5 { get; set; }
        public string borders_6 { get; set; }
    }

    public class ColorTheme
    {
        public string main_panel { get; set; }
        public string canvas_background { get; set; }
        public string scrollbar_insides { get; set; }
        public string scrollbars { get; set; }
        public string tools_background { get; set; }
        public string inactive_scrollbar { get; set; }
        public string active_canvas_background { get; set; }
        public string tools_panel_background { get; set; }
        public PanelBorders panel_borders { get; set; }
        public string panel_seperator_btn { get; set; }
        public Corners corners { get; set; }
        public Brown brown { get; set; }
        public Blue blue { get; set; }
        public CanvasSelectBorder canvas_select_border { get; set; }
        public CanvasBorderLine canvas_border_line { get; set; }
        public ColorWheelFixes color_wheel_fixes { get; set; }

        [JsonProperty("why_is_this_random?")]
        public string Random { get; set; }
        public string window_border { get; set; }
    }
}