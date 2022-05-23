General:
In order to build the system, you need to make sure to run one of the build scripts in this dir first.
First, make sure to create the directory packages in this directory. All the built packages will be placed there.
build_packages.bat on Windows and build_packages.sh on linux should then be used.
This makes sure to build all the local nuget packages. Re-run it after changes have been made in the components inside the libs folder.
Currently, this system will not run on Windows, due to the need for an audio server. The pulseaudio server is beeing used, because it is the default on the Raspberry PI platform that is the target for this system.
Theoretically, it should be possible to configure the Windows version of Pulseaudio, might be investigated in the future if need be.
