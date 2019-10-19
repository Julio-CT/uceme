@echo none 
for /f "delims=" %%a in ('dir "*.jpg" /b /s /a-d') do (
echo processing "%%a"
"jpegtran.exe" -optimize -progressive -copy none "%%a" "%%a.tmp"
move /Y "%%a.tmp" "%%a" >nul
)
pause