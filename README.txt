Skype .NET
-----------------------------------------------
Created: Sept 2012 
Author: Sverrir Sigmundarson
Where: https://github.com/sverrirs/SkypeNet


"Don't worry about people stealing your ideas. 
If your ideas are any good, you'll have to ram 
them down people's throats."
                                  --Howard Aiken


What is this:
-----------------------------------------------
Skype Desktop API wrapper written for .NET developers (.NET v4.0+). 
http://developer.skype.com/public-api-reference-index

Provides a native C# implementation to communicate 
with a running instance of the Skype application.
Uses p/Invoke though and depends on user32.dll.

Based off previous work done by Gabriel Szabo and 
published on CodeProject in 2006 including a bug fix 
proposed by user Vlad0 to that same project on 19 Sep 2007
http://www.codeproject.com/Articles/13081/Controlling-Skype-with-C

This project was started as an open-source alternative 
to the ActiveX control Skype4COM that Skype already offers 
for developers. If you want you can download Skype4COM from
the official Skype developer website: 
http://developer.skype.com/accessories/skype4com


License or other restrictions:
-----------------------------------------------
Take it and do with it what you like. I'd honestly 
be delighted if anyone would be interested in using 
this code. 

I only ask that if you do indeed use it 
please consider contributing any enhancements, ideas, 
design, problems encountered or other thoughts back 
to this project so other people might also benefit 
from them.

Just as a reminder though, in order to use the Skype 
Desktop API (which this library helps you do) you must 
first register as a developer at http://developer.skype.com.

Please also remember that if you use this library to help 
you develop applications that interact with Skype you are 
responsible for making sure that the application you develop 
conforms to the Terms of Use that are laid out by Skype here:
http://developer.skype.com/certify-market/legal/desktop-api/terms-of-use

Please play nice OK?

How to use:
-----------------------------------------------
There are two main classes:
	SkypeNet.cs
		Provides low level communication with a Skype instance
		only via the windows messaging functions. No understanding
		of the data that is passed to or received from Skype.

		Few points of failures, best suited for debugging or 
		monitoring situations.

	SkypeNetClient.cs
		High-level access to a Skype instance. This class understands
		the messages being passed and is aware of protocol support etc

		This is the class is best suted for applications that want to
		interact with Skype (e.g. initiate or receive calls, chats, sms etc)

See the SkypeNet.App for examples on how to use these classes.
		