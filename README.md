## SupercellProxy

A proxy en/decrypting..
* ..Clash of Clans
* ..Clash Royale
* ..Boom Beach

traffic. Useful for people developing private servers, bots and/or APIs.
The proxy has its own Libsodium port and manages to work without heavy external libs.

## Installation

The proxy installation takes ~5 minutes.
Info: This proxy only runs on Windows operating systems. Do not attempt to use mono, it won't work properly.
Open SupercellProxy.sln with Visual Studio and open App.config. Choose your preferred Supercell game by changing "host".
Run the proxy and wait until it finished loading. 
You need a modded APK file in order to capture and decrypt packets, visit our wiki and follow the instructions to mod an APK file.

* Bluestacks users:
Rightclick the modded APK > Open with > Bluestacks. DO NOT RUN THE MODDED GAME ONCE IT'S INSTALLED!
Mod the hosts file using Hosts Editor or a terminal application:

```
su
echo "insertyourlocalipaddress insertyourdesiredhost" > /etc/hosts
exit
```

Example:
```
su
echo "192.168.178.31 gamea.clashofclans.com" > /etc/hosts
exit
```

Open the modded game once you modded /etc/hosts and take a look at the proxy window.
You'll find the decrypted traffic at /SupercellProxy/bin/Debug/Packets.
Enjoy!

* Android smartphone/tablet users:
Tranfer the modded APK file to your device using adb or the file explorer and install it. 
I hope you enabled the "Trust unknown sources" option ;)!
Mod the hosts file, look at the steps under the Bluestacks section, there's no difference.
Run the modded game and you're good to go.

## Contact

Unhandled exceptions? Questions? Bugs? 
Either..
* add me on Discord, username: expl0itr,
* talk to me on a german IRC server: irc.belwue.de #ircsupport,
* or create a GitHub issue.

## License

Copyright 2016 expl0itr 

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
Further information: license.txt