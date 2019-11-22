echo off



:RESTART
cls
setlocal

:: DEFAULT SERVICE NAME
set SERVICENAME=ServiceIntegration

echo.
echo **************************************************************************
echo Install Wizard SERVICE INTEGRATION
echo **************************************************************************

echo Install or Unistall service ?
set /P yesorno=Install (y) or Unnistall (n) or Cancel(c): 


if "%yesorno%" == "" goto RESTART
if "%yesorno%" == "y" goto INSTALL
if "%yesorno%" == "n" goto UNINSTALL
if "%yesorno%" == "c" goto ENDBATCH
goto RESTART




:INSTALL
cls

set /P INSTANCENAME=Define instance name: 
echo.

set PATHBIN="%cd%\ServiceIntegration.exe"

IF NOT EXIST %PATHBIN% (
	ECHO. FILE NOT FOUND
	ECHO Please check file ServiceIntegration.exe
	goto ENDBATCH
)

sc create %SERVICENAME%$%INSTANCENAME%  DisplayName="Service (%INSTANCENAME%)" binPath=%PATHBIN%

echo.
echo Install success "%SERVICENAME%$%INSTANCENAME%"
echo Please setting "ServiceSettings.json"

goto ENDBATCH


:UNINSTALL
cls
set /P INSTANCENAME=Define instance name for unistall: 
echo.

:UNINSTALLAREYOUSURE
echo Are you sure?
set /P UNINSTALLYES=YES (y) or NO (n) : 


if "%UNINSTALLYES%" == "" goto UNINSTALLAREYOUSURE
if "%UNINSTALLYES%" == "y" goto UNINSTALLAREYOUSUREYES
if "%UNINSTALLYES%" == "n" goto RESTART

:UNINSTALLAREYOUSUREYES
set PATHBIN="%cd%\ServiceIntegration.exe"

IF NOT EXIST %PATHBIN% (
	ECHO. FILE NOT FOUND
	ECHO Please check file ServiceIntegration.exe
	goto ENDBATCH
)

sc delete %SERVICENAME%$%INSTANCENAME%
echo.

goto ENDBATCH

DEL %SERVICENAME%
DEL %INSTANCENAME%
DEL %UNINSTALLYES%
DEL %PATHBIN%


:ENDBATCH
echo.


REM echo param1 %1
REM echo param1 %2
REM echo param1 %3
REM echo param1 %4
REM echo param1 %5::set SERVICENAME=ServiceIntegration

:: SEMPRE DEVE TER VALOR NA INSTANCIA
REM set INSTANCENAME=DESE_TESTE
REM set PATHBIN="C:\Working\Pessoais\Service\ServiceIntegration\bin\Debug\netcoreapp2.2\publish\ServiceIntegration.exe"


REM sc create %SERVICENAME%$%INSTANCENAME%  DisplayName="Service (%INSTANCENAME%)" binPath=%PATHBIN%

