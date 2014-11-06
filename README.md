# AT&T Codekit

## Description

The AT&T C# Codekit allows you to create C# apps that can interact with APIs provided by AT&T.

## Requirements

Microsoft Visual Studio is requirement including a  .Net C# Web site project.
An ASP.NET page is typically divided into two files:
    1. A markup file with a .aspx extension.
    2. A code file, with a .aspx.cs extension.

This code defines a partial class named default (You can replace the class name can be replaced with the name of your Web page).
  
For more information about parameters for AT&T APIs, refer to the online documentation at https://developer.att.com.

## Usage

Your solution should consist of two projects:

	1. Web site project
	2. Class library project

Installation:

	1. Open a empty website in your Visual Studio.
	2. Add a default page.
	3. Copy the Default.aspx.cs to your Default.aspx.cs file.
	4. Add a new project to your current solution (by right click solution and select add/new project)
	5. Add all of the .cs files for the API that you wish to include in your project. the .cs files for each API are located in sub-folder under the codekit directory. 
			(https://github.com/attdevsupport/codekit-csharp/)
	6. Build the project and solution.

