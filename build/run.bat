@cd /d %~dp0
adb\adb.exe forward tcp:5566 tcp:5566
adb\adb.exe shell "su -c chmod 777 /data/local/tmp/*"
adb\adb.exe shell "su -c /data/local/tmp/injector com.tencent.tmgp.ztj libmono.so libtolua.so"
