Panda File System

#Description
Main purpose of application is to store, access and download files online. 
So the system is for everyone who wish to have a quick and simple file system to store their files. Main platform is computer but the page is scalable and will fit to smaller devices such as phones as well.


#Usage
For users to use all these features they need to register first. 

After that they can upload his files and manage them.
To support that they have access to file and folder management and navigation as well.
They can upload files, renames them, delete and download them. Files can also be shared and that can be disabled even when sharing link was already given out. Shared files can be imported via link.
Application supports group actions on files as well so user does not have to move, delete or download them separately. Whole folder can be downloaded as well.

Application will also log user login location to lead a book keeping of his past logins.
Server will also update user about it current status, so the user can decide not to upload some bigger files if connection is weak.

#Compatibility issues
At the moment application works with Google Chrome, Mozilla Firefox and Opera. 
Internet Explorer/Edge has problems with popup windows.

#Special features
##Efficient popup system
A very easy to call popup system that will show a popup on the screen with close button. 
Before opening, the JavaScript will close any open popup window. It is used for most of the actions on the site as it is very handy.

##File and folder navigation system
A must have thing yet deep enables user to manage his filse and folders such as creating, deleting, moving, renaming and all others.
Client gets (at the moment it is hard coded) JSON structure of folders and files and then JavaScript will parse the data into folder list and file list. These generated files/folders are then clickable and navigatable. 

#To-Do
At the moment the site presents (mostly) only front end. It needs a working back end to use implemented actions. Most of current actions are preset templates for backend.
Further note is that front end only has basic validation, but most of that should be done in the back end anyway.

