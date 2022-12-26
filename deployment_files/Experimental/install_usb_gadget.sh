#!/bin/bash
sudo apt-get update
sudo apt-get -y install dnsmasq
sudo apt-get clean
sudo cp dnsmasq_usb0 /etc/dnsmasq.d/usb0
sudo cp network_interface_usb0 /etc/network/interfaces.d/usb0
sudo cp usb-gadget.sh /usr/local/sbin/usb-gadget.sh
sudo chmod +x /usr/local/sbin/usb-gadget.sh
sudo cp usbgadget.service /lib/systemd/system/usbgadget.service
sudo systemctl enable usbgadget.service
sudo echo dtoverlay=dwc2 >> /boot/config.txt
sudo sed -i 's/$/ modules-load=dwc2,g_ether/' /boot/cmdline.txt
sudo echo libcomposite >> /etc/modules
sudo echo denyinterfaces usb0 >> /etc/dhcpcd.conf
sudo systemctl enable getty@ttyGS0.service
