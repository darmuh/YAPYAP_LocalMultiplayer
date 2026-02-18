# Changelog

## 0.1.3
 - Added new config item ``Spoof Player ID``
	- This will replace the playerId value with GUIDs that are created on game start.
	- The playerId value normally holds unique steam ids and without this spoof is not unique between players.
	- The default value for this config item is false, you will need to enable it in order to begin using the GUID based IDs.
 - Added relevant patch to replace the playerId with the GUID.

## 0.1.2
 - Updated for latest version of YAPYAP, Feb 10th update.
	- Thanks Omniscye for her work in [PR#2](https://github.com/darmuh/YAPYAP_LocalMultiplayer/pull/2)
 - Added Omniscye to credits section of README
 - Mentioned Omniscye's linux-based launcher tool in README

## 0.1.1
 - Added keybind for toggling the UINetwork debug ui per request - https://github.com/darmuh/YAPYAP_LocalMultiplayer/issues/1
	- Keybind is in config section ``Settings`` under ``Toggle NetworkDebug UI``

## 0.1.0
 - Initial release.
