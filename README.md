# Brave Browser - Set Default Profile
A Windows service that periodically updates the Windows registry to ensure that the Brave Browser will open links in a profile of your choosing.

The only entry that is checked and updated by this service is at: ```Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Classes\BraveHTML\shell\open\command```

The service updates the existing entry by adding ```-profile-directory="{{PROFILE}}"``` into the middle of the path, where "{{PROFILE}}" is your choosen Brave profile name.

# Installation
- Download the latest installer from the releases section.
- Double click the exe and follow the install steps.

# Configuration
By default, the service will use "Profile 1" by default and it will check the registry entry every 60 seconds. You will want to change the profile name to your preferred Brave profile.

## Edit the service settings

- Navigate to ``` "C:\Program Files (x86)\BoruSoft\BoruSoft - Brave Browser Set Default Profile" ```
- Open the ```appsettings.json``` file in a text editor.

## Set the default brave profile

- Change the "ProfileName" property to your desired Brave profile name.
    - To find your Brave profile name:
        - Open your default/main profile and type ```brave://version``` into the address bar.
        - Look at the "Profile Path" property. The last part of that URL should be your profile directory. (e.g "Profile 1")
- Save the changes to the file.
- Restart the service.

## Increase/decrease the registry check interval

You may want to increase or decrease this depending on how much you need to garuntee that links will open in your choosen profile. Realistically this entry should only change after Brave does an update, which usually runs once a day. 

- Change the "IntervalSeconds" property to your desired interval.
- Save the changes to the file.
- Restart the service.