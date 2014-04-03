IF NOT EXIST .\Binaries GOTO NOBINDIR

:CONTINUE1
IF NOT EXIST .\Binaries\Ffsti.Library GOTO NOFFSTIDIR

:CONTINUE2
IF NOT EXIST .\Binaries\Ffsti.Library.Database GOTO NODBDIR

:CONTINUE3
copy .\Ffsti.Library\bin\Release\*.dll .\Binaries\Ffsti.Library
copy .\Ffsti.Library\bin\Release\*.xml .\Binaries\Ffsti.Library
copy .\Ffsti.Library\bin\Release\NLog.Config .\Binaries\Ffsti.Library

copy .\Ffsti.Library.Database\bin\Release\*.dll .\Binaries\Ffsti.Library.Database
copy .\Ffsti.Library.Database\bin\Release\*.xml .\Binaries\Ffsti.Library.Database

pause
exit

:NOBINDIR
md .\Binaries
GOTO CONTINUE1

:NOFFSTIDIR
md .\Binaries\Ffsti.Library
GOTO CONTINUE2

:NODBDIR
md .\Binaries\Ffsti.Library.Database
GOTO CONTINUE3