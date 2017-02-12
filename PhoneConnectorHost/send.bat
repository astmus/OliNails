@echo off
cd /d c:\Program Files (x86)\Android\android-sdk\platform-tools\
echo adb shell am start -a android.intent.action.SENDTO -d sms:%1 --es sms_body '%2' --ez exit_on_sent true
adb shell am start -a android.intent.action.SENDTO -d sms:%1 --es sms_body '%2' --ez exit_on_sent true
timeout 1 > NUL
adb shell input keyevent 66
adb shell input text olinails.com
adb shell input keyevent 22
adb shell input keyevent 66
timeout 1 > NUL
echo completed