# <img  src="icon.ico"  alt="logo"  height="30"  />&nbsp;&nbsp;AMEML&nbsp;&nbsp;-&nbsp;&nbsp;Amazon Music Export My Library

> A performat single exe programm to export your Amazon Music Songs from "My Library" since this library is not a playlist so you normaly can't export it<br />
> Works without active Amazon Music subscription<br />
> [FreeYourMusic](https://freeyourmusic.com) friendly
<br />

## Requirements
You need to have Amazon Music installed
<br />
You can get the installer [here](https://amazon.de/Amazon-Music-für-PC-Download/dp/B00CTTEKJW)
<br />
<br />
The programm also needs admin rights so be sure to run this on a PC with admin rights or alternaitvely in a sandbox with internet connection
<br />
<br />

## Instalation

Head to the [latest release](https://github.com/deadlinecode/AMEML/releases/latest), grab the file for your PC and download it
<br />
<br />

## Usage

Just start the programm by double clicking the downloaded file
<br />
A console window will pop up telling you what you need to do
<br />
<br />

`If a window pops up telling you to install a certificate or let a programm through the firewall click accept`<br />
`The programm will use the certificate only to decrypt the https traffic of the `
<br />
<br />

After the programm has gathered all necessary informations you can choose between 3 formats
| Format | Explanation |
| ------ | ----------- |
| CSV    | All data in CSV Format with headers |
| JSON   | All data in JSON (array containing objects) |
| CSV (FreeYourMusic friendly)&nbsp;&nbsp;&nbsp;&nbsp; | CSV with less data and formated so FreeYourMusic recognizes it&nbsp;&nbsp;&nbsp;&nbsp;<br />(`name`, `artist`, `album`) |
> [Scroll down](#data-infos) to see all informations you get
<br />

After the programm is done the file will be saved in the current folder where the programm is located
The file name will be the current date and time in the following format `11-07-2023T22-25.csv`
<br />
<br />

## Performance
Here are some benchmarks of the programm.
<br />

`Specs`<br />
Windows 11 Home<br />
Dell XPS 15 9500<br />
CPU i7-10750H 2.60GHz<br />
RAM 32GB<br />
2352 Songs<br />
<br />

| Action                                    | Average time |
| ----------------------------------------- | ------------ |
| Parse CSV                                 | 216ms        |
| To JSON                                   | 48ms         |
| To CSV                                    | 2ms          |
| Convert CSV<br />(FreeYourMusic Friendly) | 5ms          |
> Since the data comes in as csv we need to parse it and then convert it to JSON or FreeYourMusic Friendly CSV<br />
> For normal CSV the file is directly written without any further processing
<br />
<br />

## Data infos
You will get following data:
- objectId
- fileName
- fileExtension
- fileSize
- creationDate
- lastUpdatedDate
- orderId
- asin
- purchaseDate
- localFilePath
- md5
- status
- purchased
- uploaded
- title
- sortTitle
- rating
- marketplace
- physicalOrderId
- assetType
- artistName
- artistAsin
- contributors
- trackNum
- discNum
- primaryGenre
- duration
- bitrate
- composer
- songWriter
- performer
- lyricist
- publisher
- errorCode
- instantImport
- primeStatus
- isMusicSubscription
- albumName
- albumAsin
- albumArtistName
- albumArtistAsin
- albumContributors
- albumRating
- albumPrimaryGenre
- albumReleaseDate
- sortArtistName
- sortAlbumName
- sortAlbumArtistName
- audioUpgradeDate
- parentalControls
- assetEligibility
- eligibility
- internalTags
<br />

In case of the normal CSV file you will get all these informations with the first line being the headers with exactly the same name.<br />
For the JSON file you will get the same but a single array containing objects with all the key value pairs of the infos above.<br />
For the FreeYourMusic friendly CSV format you will get only `name` (title), `artist` (artistName) and `album` (albumName) which will also decrease the file size.

<br />
<br />

### Disclaimer
AMEML and the creator deadlinecode are not affiliated with Amazon Music and assumes no legal responsibility for the use of this programm.
<br />
Use at your own risk.
