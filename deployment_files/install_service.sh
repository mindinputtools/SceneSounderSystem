#!/bin/bash
sudo cp SceneSounder.service /etc/systemd/user/
systemctl enable SceneSounder --user
