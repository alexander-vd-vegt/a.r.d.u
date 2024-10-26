For particular pi4 running hostname ardu-mk1
change in /boot/firmware/config.txt the following

camera_auto_detect=0
# enable V2 camera (IMX219)
dtoverlay=imx500

this forces to boot with the imx500 overlay and then the camera will be detected