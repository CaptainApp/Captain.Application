# Captain.Application
![version: 0.6](https://img.shields.io/badge/version-0.6-blue.svg)
![license: BSD 2-Clause](https://img.shields.io/badge/license-BSD_2--Clause-brightgreen.svg)
> Native GUI application that implements most [Captain](https://github.com/CaptainApp) logic

## What's this?
It's the _actual_ code for [Captain](https://github.com/CaptainApp), an extensible screen capturing application.

## Compatibility
The application is designed to work with Windows Vista SP2 onwards. Windows XP support is not present nor planned, as
we depend upon the .NET Framework 4.5, which is not compatible with this platform.

## Building
This project depends upon certain libraries which are not included in this repository.
Refer to the [CaptainApp/Captain](https://github.com/Captain) repository for build instructions.

## Project structure
- `Resources/` - Contains images, icons and other assets used by the application.
- `Source/` - Contains application logic and utilities not directly related with the user interface.
- `UI/` - Contains user interface logic, Windows Forms generated files and utility classes used by the desktop UI.

## Localization
Captain uses .NET resource files (`*.resx`) which include strings and other assets that may be localized. For
translating the UI, you may use the built-in Windows Forms designer features in Visual Studio to modify strings and
other properties. You may want to look to the `Resources/Resources.resx` file, which contains other strings that
are used to display notifications, dialogs and other strings not directly bound to a Windows Forms control.

## Open-source code
This software depends upon awesome open-source software without which it could not be possible.

### Third-party
> Some directories contain a `README.md` file which briefly explains the contents and its original authority.
Additionally, some code snippets, lines and methods have been appropriately annotated to include their original sources
in most cases.

These open-source projects are also being used (albeit with no actual source code of these being included):
- **Multiple [SharpDX](http://sharpdx.org/) libraries**, as NuGet package dependencies.  
  `Copyright (c) 2010-2015 SharpDX - Alexandre Mutel`
- **[Newtonsoft.Json](https://www.newtonsoft.com/json)**, as a
  [NuGet package dependency](https://www.nuget.org/packages/newtonsoft.json/).  
  `Copyright (c) 2007 James Newton-King`
- **[Ookii.Dialogs](http://www.ookii.org/software/dialogs/)**, as a
  [NuGet package dependency](https://www.nuget.org/packages/Ookii.Dialogs.Wpf/).  
  `Copyright © Sven Groot (Ookii.org) 2009`
- **[dot-net-transitions](https://github.com/UweKeim/dot-net-transitions)**, as a
  [NuGet package dependency](https://www.nuget.org/packages/dot-net-transitions/).  
  `Copyright (c) 2009 Richard S. Shepherd`
- **[`LinkLabel2.cs`](https://github.com/CaptainApp/Captain.Application/tree/master/UI/Common/LinkLabel2)**  
  `Copyright (c) 2017, wyDay`
- **[DragDropLib](https://github.com/CaptainApp/Captain.Application/tree/master/UI/Helper)**  
  Copyrighted by Microsoft