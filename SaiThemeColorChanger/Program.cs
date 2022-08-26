using SaiDarkTheme;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SaiThemeColorChanger.JSON_Templates;

namespace SaiThemeColorChanger
{
    //Hex strat derived from https://social.msdn.microsoft.com/Forums/vstudio/en-US/a0b2133f-ae23-4c0b-b136-dd531952f3c7/find-amp-replace-hex-values-in-a-dll-file-using-c?forum=csharpgeneral
    public class Program
    {
        private static readonly string defaultThemesDirectory = $"{Environment.CurrentDirectory}\\Themes";
        private static readonly string configPath = $"{Environment.CurrentDirectory}\\config.json";
        private static Config cfg = new Config();
        public static void Main(string[] args)
        {
            string inputPath = "";
            if (args.Length > 0)
                inputPath = args[0];
            
            // Load the configuration file
            if (!File.Exists(Path.GetFileName(configPath)))
            {
                Debug.WriteLine(configPath);
                Console.Error.WriteLine("Config file (config.json) does not exist. Please re-download the application.");
                Console.ReadKey();
                return;
            }
            cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
            
            if (inputPath.Length == 0)
            {
                Console.WriteLine("Drag 'sai2.exe' in this window or write the full path with the executable (MUST NOT INCLUDE DOUBLE QUOTES):");
                // Using verbatim string, this will ignore special characters like spaces in a path.
                inputPath = @$"{Console.ReadLine()}";
                while (!Directory.Exists(Path.GetDirectoryName(inputPath)))
                {
                    Console.WriteLine("Not a valid path: " + inputPath);
                    inputPath = Console.ReadLine();
                }
            }
            
            if (!Directory.Exists(Path.GetDirectoryName(inputPath)))
            {
                Console.WriteLine("Not a valid path: " + inputPath);
                Console.ReadKey();
                return;
            }

            string outputPath = inputPath;   //Needs to be the same as the original or Sai throws a weird error with moonrunes 
            Dictionary<int, string> filesList = null;
            if (!Directory.Exists(@$"{Environment.CurrentDirectory}\\Themes"))
            {
                Console.WriteLine("Directory 'Themes' was not found! Please re-download the program.");
                Console.ReadKey();
                return;
            }

            string previousThemeUsed = null;
            string selectedThemePath = null;
            string selectedThemeName = null;
            if (Directory.GetFiles(defaultThemesDirectory).Any())
            {
                Console.WriteLine("Found themes in 'Themes' folder:");
                filesList = new Dictionary<int, string>();
                int index = 0;
                foreach (var themeFile in Directory.GetFiles(defaultThemesDirectory))
                {
                    Console.WriteLine($"[{index}] {Path.GetFileNameWithoutExtension(themeFile).Replace(".theme", "")}");
                    filesList.Add(index, themeFile);
                    index++;
                }
                previousThemeUsed = filesList.FirstOrDefault(t => Path.GetFileNameWithoutExtension(t.Value) == cfg.CurrentUsedTheme).Value;
                if (string.IsNullOrEmpty(previousThemeUsed))
                {
                    Console.Error.WriteLine("Cannot find last used theme in 'Themes' Directory. Program cannot continue.");
                    Console.ReadKey();
                    return;
                }
                bool selectedValidFile = false;

                while (!selectedValidFile)
                {
                    Console.WriteLine("Which theme would you like to use?");
                    try
                    {
                        int themeFileInput = int.Parse(Console.ReadLine() ?? string.Empty);
                        if (themeFileInput >= 0 || themeFileInput <= index)
                        {
                            selectedThemePath = filesList[themeFileInput];
                            selectedThemeName = $"{Path.GetFileNameWithoutExtension(selectedThemePath).Replace(".theme", "")}";
                            if (cfg != null && selectedThemeName == cfg.CurrentUsedTheme)
                            {
                                Console.Error.WriteLine("This theme is already used in SAI, please use another theme.");
                                continue;
                            }
                            Console.WriteLine($"Selected theme '{selectedThemeName}'");
                            selectedValidFile = true;
                            break;
                        }
                    }
                    catch
                    {
                       Console.WriteLine("Invalid Input.");
                    }
                    
                }
            }
            List<ReplacerHelper> toReplace = new List<ReplacerHelper>();

            ColorTheme oldTheme, newTheme;
            try
            {
                oldTheme = JsonConvert.DeserializeObject<ColorTheme>(File.ReadAllText(previousThemeUsed));
                newTheme = JsonConvert.DeserializeObject<ColorTheme>(File.ReadAllText(selectedThemePath));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            //Hex color code -> replacement (won't work with pure white and pure black, but everything else seems fine!)
            //Basically this replaces left hex with the right hex.
            //You can swap out the values to get other colors, I haven't noticed any issues using a version with these values modified
            
            toReplace.Add(new ReplacerHelper(oldTheme.main_panel, newTheme.main_panel)); //Main panel color
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_background, newTheme.canvas_background)); //Canvas background color
            toReplace.Add(new ReplacerHelper(oldTheme.scrollbar_insides, newTheme.scrollbar_insides)); //Scrollbar insides
            toReplace.Add(new ReplacerHelper(oldTheme.scrollbars, newTheme.scrollbars)); //Scrollbars
            toReplace.Add(new ReplacerHelper(oldTheme.tools_background, newTheme.tools_background)); //Tools background
            toReplace.Add(new ReplacerHelper(oldTheme.inactive_scrollbar, newTheme.inactive_scrollbar)); //Inactive scrollbar 
            toReplace.Add(new ReplacerHelper(oldTheme.active_canvas_background, newTheme.active_canvas_background)); //Active canvas background
            toReplace.Add(new ReplacerHelper(oldTheme.tools_panel_background, newTheme.tools_panel_background)); //Tools panel background
            toReplace.Add(new ReplacerHelper(oldTheme.panel_borders.borders_1, newTheme.panel_borders.borders_1)); //Panel borders 1 
            toReplace.Add(new ReplacerHelper(oldTheme.panel_borders.borders_2, newTheme.panel_borders.borders_2)); //Panel borders 2 
            toReplace.Add(new ReplacerHelper(oldTheme.panel_borders.borders_3, newTheme.panel_borders.borders_3)); //Panel borders 3 
            toReplace.Add(new ReplacerHelper(oldTheme.panel_borders.borders_4, newTheme.panel_borders.borders_4)); //Panel borders 4 
            toReplace.Add(new ReplacerHelper(oldTheme.panel_borders.borders_5, newTheme.panel_borders.borders_5)); //Panel borders 5 
            toReplace.Add(new ReplacerHelper(oldTheme.panel_borders.borders_6, newTheme.panel_borders.borders_6)); //Panel borders 6  
            toReplace.Add(new ReplacerHelper(oldTheme.panel_seperator_btn, newTheme.panel_seperator_btn)); //Panel separator button 1
            toReplace.Add(new ReplacerHelper(oldTheme.corners.corners_1, newTheme.corners.corners_1)); //Corners 1
            toReplace.Add(new ReplacerHelper(oldTheme.corners.corners_2, newTheme.corners.corners_2)); //Corners 2
            toReplace.Add(new ReplacerHelper(oldTheme.corners.corners_3, newTheme.corners.corners_3)); //Corners 3
            toReplace.Add(new ReplacerHelper(oldTheme.corners.corners_4, newTheme.corners.corners_4)); //Corners 4
            toReplace.Add(new ReplacerHelper(oldTheme.corners.corners_5, newTheme.corners.corners_5)); //Corners 5
            toReplace.Add(new ReplacerHelper(oldTheme.corners.corners_6, newTheme.corners.corners_6)); //Corners 6
            toReplace.Add(new ReplacerHelper(oldTheme.corners.corners_7, newTheme.corners.corners_7)); //Corners 7
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_1, newTheme.brown.brown_1)); //Brown 1, DON'T change the 20 at the start of this HEX!
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_2, newTheme.brown.brown_2)); //Brown 2
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_3, newTheme.brown.brown_3)); //Brown 3
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_4, newTheme.brown.brown_4)); //Brown 4
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_5, newTheme.brown.brown_5)); //Brown 5
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_6, newTheme.brown.brown_6)); //Brown 6
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_7, newTheme.brown.brown_7)); //Brown 7
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_8, newTheme.brown.brown_8)); //Brown 8
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_9, newTheme.brown.brown_9)); //Brown 9
            toReplace.Add(new ReplacerHelper(oldTheme.brown.brown_10, newTheme.brown.brown_10)); //Brown 10
            toReplace.Add(new ReplacerHelper(oldTheme.blue.blue_1, newTheme.blue.blue_1)); //Blue 1
            toReplace.Add(new ReplacerHelper(oldTheme.blue.blue_2, newTheme.blue.blue_2)); //Blue 2
            toReplace.Add(new ReplacerHelper(oldTheme.blue.blue_3, newTheme.blue.blue_3)); //Blue 3
            toReplace.Add(new ReplacerHelper(oldTheme.blue.blue_corner_1, newTheme.blue.blue_corner_1)); //Blue corner 1
            toReplace.Add(new ReplacerHelper(oldTheme.blue.blue_corner_2, newTheme.blue.blue_corner_2)); //Blue corner 2
            toReplace.Add(new ReplacerHelper(oldTheme.blue.blue_corner_3, newTheme.blue.blue_corner_3)); //Blue corner 3
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_select_border.border_1, newTheme.canvas_select_border.border_1)); //Canvas select border 1
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_select_border.border_2, newTheme.canvas_select_border.border_2)); //Canvas select border 2
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_select_border.border_3, newTheme.canvas_select_border.border_3)); //Canvas select border 3
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_select_border.border_4, newTheme.canvas_select_border.border_4)); //Canvas select border 4
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_border_line.border_line_1, newTheme.canvas_border_line.border_line_1)); //Canvas border line
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_border_line.border_line_2, newTheme.canvas_border_line.border_line_2)); //Canvas border line
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_select_border.selection_border_1, newTheme.canvas_select_border.selection_border_1)); //Canvas selection border
            toReplace.Add(new ReplacerHelper(oldTheme.canvas_select_border.selection_border_2, newTheme.canvas_select_border.selection_border_2)); //Canvas selection border
            toReplace.Add(new ReplacerHelper(oldTheme.Random, newTheme.Random)); //Random
            toReplace.Add(new ReplacerHelper(oldTheme.window_border, newTheme.window_border)); //Window border
            
            // TODO: This need to be fixed. Someone else do this please :(
            // toReplace.AddRange(doColorWheelFix(true, newTheme, oldTheme));

            makeBackup(inputPath);
            Console.WriteLine("Replacing stuff in: " + inputPath);
            replaceHex(inputPath, outputPath, toReplace);
            Console.WriteLine("Replaced file saved to: " + outputPath);
            if (cfg != null)
            {
                Console.WriteLine("Updating config");
                cfg.CurrentUsedTheme = $"{selectedThemeName}.theme";
                File.WriteAllText(configPath, JsonConvert.SerializeObject(cfg));
            }

            Console.WriteLine("Finished");
            
            Console.ReadKey();
        }

        private static List<ReplacerHelper> doColorWheelFix(bool NoItterVals, ColorTheme newTheme, ColorTheme oldTheme)
        {
            List<ReplacerHelper> colorWheelFixes = new List<ReplacerHelper>();
            if (!NoItterVals)
            {
                for (int i = 162; i <= 254; i++)
                {
                    colorWheelFixes.Add(new ReplacerHelper("" + i.ToString("X2") + i.ToString("X2") + i.ToString("X2"), "212121")); //Color wheel fix (1 in file)
                }
            }

            try
            {
                if (!newTheme.color_wheel_fixes.revert_fix)
                {
                    colorWheelFixes.Add(new ReplacerHelper(oldTheme.color_wheel_fixes.fix_2,
                        newTheme.color_wheel_fixes.fix_2)); //Color wheel fix 1 (2 in file)
                    colorWheelFixes.Add(new ReplacerHelper(oldTheme.color_wheel_fixes.fix_3,
                        newTheme.color_wheel_fixes.fix_3)); //Color wheel fix 2 (3 in file)
                    colorWheelFixes.Add(new ReplacerHelper(oldTheme.color_wheel_fixes.fix_4,
                        newTheme.color_wheel_fixes.fix_4)); //Color wheel fix 3 (4 in file)
                }

            }
            catch { }
            if (!NoItterVals)
            {
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        for (int k = 1; k <= 8; k++)
                        {
                            if (i != j || i != k)
                            {
                                colorWheelFixes.Add(new ReplacerHelper("f" + i + "f" + j + "f" + k, "212121")); //Color wheel 
                            }
                        }
                    }
                }
            }

            return colorWheelFixes;
        }
        
        //Fuggin fug fug
        //Cut the hex string -> byte array
        public static byte[] GetByteArray(string str)
        {
            //https://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
            return Enumerable.Range(0, str.Length)
                                .Where(x => x % 2 == 0)
                                .Select(x => System.Convert.ToByte(str.Substring(x, 2), 16))
                                .ToArray();
        }

        public static void makeBackup(string path)
        {
            // To the person who previously wrote this: Parsing the extension of the file name with Path is redundant, since we only open an .exe file. 
            string targetPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}_backup_{DateTime.Now.ToString("hhmmss")}.exe";
            if (!File.Exists(targetPath))
            {
                File.Copy(path, targetPath);
                Console.WriteLine("Backup copy generated in " + targetPath);
            }
            else
            {
                Console.WriteLine("Backup copy already exists in " + targetPath);
            }
        }

        public static bool findHex(byte[] sequence, int position, byte[] seeker)
        {
            if (position + seeker.Length > sequence.Length) return false;

            for (int i = 0; i < seeker.Length; i++)
            {
                if (seeker[i] != sequence[position + i]) return false;
            }

            return true;
        }

        public static void replaceHex(string targetFile, string resultFile, string searchString, string replacementString)
        {

            var targetDirectory = Path.GetDirectoryName(resultFile);
            if (targetDirectory == null) return;
            Directory.CreateDirectory(targetDirectory);

            byte[] fileContent = File.ReadAllBytes(targetFile);

            byte[] seeker = GetByteArray(searchString);
            byte[] hider = GetByteArray(replacementString);

            for (int i = 0; i < fileContent.Length; i++)
            {
                if (!findHex(fileContent, i, seeker)) continue;
                for (int j = 0; j < seeker.Length; j++)
                {
                    fileContent[i + j] = hider[j];
                }
            }

            File.WriteAllBytes(resultFile, fileContent);
        }

        public static void replaceHex(string targetFile, string resultFile, List<ReplacerHelper> toReplace)
        {

            var targetDirectory = Path.GetDirectoryName(resultFile);
            if (targetDirectory == null) return;
            Directory.CreateDirectory(targetDirectory);

            byte[] fileContent = File.ReadAllBytes(targetFile);

            foreach (ReplacerHelper replacerHelper in toReplace)
            {
                byte[] seeker = GetByteArray(replacerHelper.Search);
                byte[] hider = GetByteArray(replacerHelper.Replace);

                bool ringFlag = false;

                for (int r = 162; r <= 254; r++)
                {
                    if (replacerHelper.Search == ("" + r.ToString("X2") + r.ToString("X2") + r.ToString("X2")))
                    {
                        ringFlag = true;
                    }
                }

                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        for (int k = 1; k <= 8; k++)
                        {
                            if (i != j || i != k)
                            {
                                if (replacerHelper.Search == ("f" + i + "f" + j + "f" + k))
                                {
                                    ringFlag = true;
                                }
                            }
                        }
                    }
                }

                if (ringFlag == false)
                {
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (!findHex(fileContent, i, seeker)) continue;

                        for (int j = 0; j < seeker.Length; j++)
                        {
                            fileContent[i + j] = hider[j];
                        }
                    }
                }
                else
                {
                    for (int i = 3160329; i < 3243232; i++)
                    {
                        if (!findHex(fileContent, i, seeker)) continue;

                        for (int j = 0; j < seeker.Length; j++)
                        {
                            fileContent[i + j] = hider[j];
                        }
                    }
                }
            }
            File.WriteAllBytes(resultFile, fileContent);
        }

    }
}