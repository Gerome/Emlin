
FOR /L %%A IN (1,1,11) DO (del Emlin\keys.env
	xcopy /i "..\Documents\Consent forms\%%Akeys.env" Emlin\keys.env*
	msbuild.exe /p:Configuration=Release Emlin.sln
	cd Emlin\bin
	"D:\Program Files\7-Zip\7z.exe" a -t7z Emlin%%A.zip -r Release
	cd ..\..
)
