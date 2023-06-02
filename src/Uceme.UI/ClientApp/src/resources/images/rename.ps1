Get-ChildItem -File -Recurse | ForEach-Object { Rename-Item -Path $_.PSPath -NewName $_.Name.replace(".*.webp",".webp")}
Get-ChildItem | ForEach-Object { move-item -literal $_ $_.name.replace(".jpg.webp",".webp") }
Get-ChildItem | ForEach-Object { move-item -literal $_ $_.name.replace(".png.webp",".webp") }
Get-ChildItem | ForEach-Object { move-item -literal $_ $_.name.replace(".JPG.webp",".webp") }
