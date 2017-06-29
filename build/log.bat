@cd /d %~dp0
adb\adb.exe shell "logcat -c"
adb\adb.exe shell "logcat|grep XMONO"