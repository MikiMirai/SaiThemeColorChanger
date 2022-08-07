INSTRUCTIONS:
!!!!(It is recommended to back up your sai folder first just in case anything goes wrong, just save it to your desktop or something. Just copy it and paste it somewhere else to do this.)!!!!

1. Open "SaiThemeColorChanger.exe".
2. Go into SAI's folder and drag the file "sai.exe" into the window you just opened (Be sure to run it from somewhere with the appropriate permissions) 
3. Press enter.

Install https://dotnet.microsoft.com/en-us/download/dotnet/5.0 > under **.NET Desktop Runtime 5** click **x86** if it doesn't work!

If you want to make your own custom colors, just edit the hex list in the source code given.

(CHANGES)
- NotBoogie's original project had very light colors, so Nyamhk and BusteanHAN changed the UI hex codes into others similar to Clip Studio's dark mode.
- BusteanHAN wrote a for loop to fix the light gray pixels around the color wheel. This only works in 100% interface scaling though.
- Miki and Nyamhk changed the remaining hex codes
- Miki and Nyamhk fixed a bug in which the layer blending mode panel and the resize canvas panel would not open and cause a bunch of errors.

(NOTES)
- Because this works on the compiled program, every hex code has been found manually and any other changes require brute forcing of memory addresses that would make this color changer incompatible with potential newer versions of the program.
- Trying to change pure black (#000000) and pure white (#ffffff) breaks the program.
- All hex codes that start with 20 can only be changed with other hex codes that start with 20 or the blending modes break due to the memory address issue mentioned earlier.
- Some hex codes get flipped by the program for some reason (#204080 becomes #804020), for some reason.
