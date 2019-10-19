@echo none 
for /f "delims=" %%a in ('dir "*.jpg" /b /s /a-d') do (
echo processing "%%a"
jpegtran.exe -optimize -progressive -copy none "%%a" "%%a.tmp"
move /Y "%%a.tmp" "%%a" >nul
)

for /f "delims=" %%a in ('dir "*.png" /b /s /a-d') DO (
echo processing "%%a"
optipng.exe -force -v -o2 "%%a"
)

pause