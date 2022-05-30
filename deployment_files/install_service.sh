#!/bin/bash
mkdir -p /home/pi/SceneSounder
cp docker-compose.yml /home/pi/SceneSounder
sudo cp SceneSounder.service /etc/systemd/user/
systemctl enable SceneSounder --user
