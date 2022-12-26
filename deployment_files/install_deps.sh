#!/bin/bash
sudo apt update
sudo apt full-upgrade -y
sudo apt install docker.io docker-compose pulseaudio apulse git -y
sudo usermod -aG docker pi
