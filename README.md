# SaiThemeColorChanger
Recolors Sai2.exe to have a faux-dark theme

Drag the sai2.exe file into the executable to change the UI colors.  Be sure to run it from somewhere with the appropriate permissions.  If you want to make your own custom colors, just edit the hex list in the source code.

ALSO MAKE SURE YOU BACK UP YOUR SAI FOLDER JUST TO BE SAFE LOL.  I haven't noticed any issues using a modified version for a couple months now, but still.

Fork:
Abused pixel hex value inspection to find even more areas to be changed into a proper darkmode, not just pale gray

Text blacks and button whites are #000000 and #ffffff respectively, which cannot be changed in the binary without completely breaking the program.
Address filtering could be applied to filter which #0s and #fs are changed, but the locations themselves are discovered manually through brute force.
That takes time; I do not have that much of it.