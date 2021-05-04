# Covid-19 Vaccine Tracker (India)

This is a small console application, written in C#, to track availability of Covid-19 vaccines in India.  
It uses the Cowin Open APIs, Spec available [at this link](https://apisetu.gov.in/public/marketplace/api/cowin).  


## Getting started from Source Code

**Pre-requisities:**  
You need to install dotnet core SDK in your machine. For details on how to install and use dotnet core, [visit this link](https://dotnet.microsoft.com/download).  
Please install dotnet core version 3.1 as this is the LTS version.

1. Clone the repository
1. `cd src/VaccineTrackers`
1. `dotnet restore`
1. `dotnet build`
1. `dotnet run -- --help` to view the commands available.

To create [self-contained exe files], run these commands (from `./src/VaccineTrackers`):
1. Windows 64-bit compatible: `dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true`  
1. Linux 64-bit compatible: `dotnet publish -r linux-x64 -p:PublishSingleFile=true --self-contained true`  

### Samples

**Check available commands**  
1. `dotnet run -- --help`
   - show details of all available commands
1. `dotnet run -- checkvaccineslots byDistrict --help`
   - show help on `checkvaccineslots byDistrict` command
1. `dotnet run -- getdistrictid byState --help`  
   - show help on `getdistrictid` command
1. `dotnet run -- checkvaccineslots byPin --help`
   - show help on `checkvaccineslots byPin` command

<details><summary>Sample output: Available Commands</summary>
<p>

```ps
❯ dotnet run -- --help
Usage: Cowin.VaccineTrackers <Command>

Commands:
  checkvaccineslots byPin         Track Vaccine availability by District
  checkvaccineslots byDistrict    Track Vaccine availability by District
  getdistrictid byState           Get District IDs in a State
  help                            Display help.
  version                         Display version.
```  


</p>
</details>  

</br>

**Check vaccine availability in your district:**  
1. `dotnet run -- checkvaccineslots byDistrict -s karnataka -d bbmp -a 18 -w 4`
   - here we are checking vaccine availability in BBMP district of Karnataka, for ages 18 and above, in next 4 weeks.
1. `dotnet run -- checkvaccineslots byDistrict -s karnataka -d bbmp -a 18 -w 4 -c`
   - same as above, but here the command will be *continuously* checking (until user presses `ctrl+c`).


**Check vaccine availability by PIN Code:**  
1. `dotnet run -- checkvaccineslots byPin -p 560001 -a 45 -w 4`
   - here we are checking vaccine availability in Pin Code `560001`, for ages 18 and above, in next 4 weeks.
   - add `-c` to keep the checks running continuously (until user presses `ctrl+c`).


**Get names and IDs of all the districts in a state:**  
1. `dotnet run -- getdistrictid bystate -s "West Bengal"`


## Getting started from pre-built exe file

I have provided pre-built executable files for Windows and Linux (64-bit) versions [in this directory](./exe) as zipped files.
Unzip the files, and start using them as shown below.

### Samples

**Check available commands**  
1. `.\CowinVaccineTrackers.exe  --help`
   - show details of all available commands
1. `.\CowinVaccineTrackers.exe checkvaccineslots byDistrict --help`
   - show help on `checkvaccineslots byDistrict` command
1. `.\CowinVaccineTrackers.exe getdistrictid byState --help`  
   - show help on `getdistrictid` command
1. `.\CowinVaccineTrackers.exe checkvaccineslots byPin --help`
   - show help on `checkvaccineslots byPin` command

**Check vaccine availability in your district:**  
1. `.\CowinVaccineTrackers.exe checkvaccineslots byDistrict -s karnataka -d bbmp -a 18 -w 4`
   - here we are checking vaccine availability in BBMP district of Karnataka, for ages 18 and above, in next 4 weeks.
1. `.\CowinVaccineTrackers.exe checkvaccineslots byDistrict -s karnataka -d bbmp -a 18 -w 4 -c`
   - same as above, but here the command will be *continuously* checking (until user presses `ctrl+c`).

**Check vaccine availability by PIN Code:**  
1. `.\CowinVaccineTrackers.exe checkvaccineslots byPin -p 560001 -a 45 -w 4`
   - here we are checking vaccine availability in Pin Code `560001`, for ages 18 and above, in next 4 weeks.
   - add `-c` to keep the checks running continuously (until user presses `ctrl+c`).


**Get names and IDs of all the districts in a state:**  
1. `.\CowinVaccineTrackers.exe getdistrictid bystate -s karnataka`