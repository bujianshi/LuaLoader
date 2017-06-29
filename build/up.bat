@cd /d %~dp0
adb\adb.exe push injector /data/local/tmp/injector
adb\adb.exe push libxgame.so /data/local/tmp/libxgame.so
adb\adb.exe push libsubstrate.so /data/local/tmp/libsubstrate.so
