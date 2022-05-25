# DROMsM
DROMsM is a Windows application written in C# WinForms that exposes several helpful operations that assist you in maintaining your romset collections.
### Features

- Open and view DAT files in a spreadsheet-like environment with column reordering, filtering and searching.
- Export filtered DAT files to CSV.
- Move similarly named roms out of the collection and only keep the best one.
- Move similarly named roms into their own directory.
- Move all ROMS from their subdirectories to the root directory.
- Remove empty root directories.
- Combine multiple bin files into a single bin file.
- Given a list containing rom filenames, remove all the matching files from the romset.
- Show or hide entire LaunchBox platforms

#### Main Operations Window

![Main View](https://i.imgur.com/1k91L9r.png)

#### Viewing or modifying a DAT File

![DAT File Viewer](https://user-images.githubusercontent.com/501697/170152398-8bcc2b30-13e9-48f0-9ccd-c196d96fed47.png)


#### Showing/Hiding LaunchBox Platforms
From `Tools -> LaunchBox -> Manage Platforms`, you can access a window which allows you to toggle the visibility of entire LaunchBox platforms.  This allows you to show or hide platforms in LaunchBox without removing them.

The way this works is by modifying the file extenstions of the files that LaunchBox uses to handle your platforms.

![image](https://user-images.githubusercontent.com/501697/170151502-d4668813-07b9-4f0b-b048-860e3cc609bc.png)

