# Smart AC Prototype

Create a proof of concept for Smart AC Devices.  These devices have sensors for Temperature, CO Levels, Humidity and a General Health Status.  The devices need to send these readings, either one at a time or as part of a batch, to a central locations.  This Prototype exposes an HTTP API for these smart devices and provides an admin dashboard for users to view the devices and their stats.

# Completed Requirements
- API:  Register Devices.  Records 1 or more Readings. The complete api usage can be found at [https://smartacfe2.azurewebsites.net/api](https://smartacfe2.azurewebsites.net/api)
- Web App  [https://smartacfe2.azurewebsites.net/AdminPanel](https://smartacfe2.azurewebsites.net/AdminPanel/)
	- Login / Out  [https://smartacfe2.azurewebsites.net/Account/Login](https://smartacfe2.azurewebsites.net/Account/Login)
	- List All Devices 
	- Search by Serial Number
	- View Device Details.  Show's the latest reading as well as trend graphs for the Temperature, Humidity and CO Levels
	- Notifications.  The Web App will ping the back end every 10 seconds and display any uncleared notifications at the top of the current page.  Notifications must be cleared or they will keep showing up.

# Missing Features
- Recovering/Resetting Passwords
- Inviting other users.  (There is a registration feature though)
- List users, enable/disable option

# Data Definition
### User
Field Name|Type|Required|Description
-------------|------|--------|---------
ID|Int32|Yes|Auto Increment, Primary Key
Username|String|Yes|Should be an email address
FirstName|String|No|
LastName|String|No|

### ACDevice
Field Name|Type|Required|Description
-------------|------|--------|---
ID|Int32|Yes|Auto Increment-PrimaryKey
SerialNumber|String|Yes|Unique serial number of the device
FirmwareVersion|String|Yes|Firmware Version of the device
RegistrationDate|DateTime|Yes|Autopopulated with the current date when a device registers
AccessKey|String|No|Generated access key that clients should use when sending readings for this device

### ACDeviceReading
Field Name|Type|Required|Description
-------------|------|--------|---
ID|Int32|Yes|Auto Increment - Primary Key
ACDeviceID|Int32|Yes|Foreign Key to ACDevice
Temperature|Decimal|No|Temperature reading in Celsius
COLevel|Decimal|No|Carbon Monoxide level in parts per million
Humidity|Decimal|No|Humidity as a percentage
HealthStatus|String|No|Current status of the device  *should be an enum*
ReadingDateTime|DateTime|Yes|Client supplied timestamp for the readings
LoggedDateTime|DateTime|Yes|System populated timestamp represents when the reading was sent to the system

### ACDeviceAlert
Field Name|Type|Required|Description
-------------|------|--------|---
ID|Int32|Yes|Auto Increment - Primary Key
ACDeviceID|Int32|Yes|Foreign Key to ACDevice
DeviceAlertType|Int32|Yes|Enum of HEALTH_STATUS=0, CO_LEVEL=1
ReadingDateTime|DateTime|Yes|Timestamp of the reading that caused the alert
AlertDateTime|DateTime|Yes|System populated timestamp representing when the reading was sent that triggered the alert
Cleared|Boolean|No|Boolean indicating that the alert has been cleared

## ERD
```mermaid
graph LR
A[ACDevice Reading] -- FK --> B[AC Device]
C[ACDevice Alert] -- FK --> B[AC Device]
U((User))
```
