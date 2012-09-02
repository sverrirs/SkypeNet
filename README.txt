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
A .NET 4 implementation of the Skype Desktop API. 
Provides a native C# way of communicating with a 
running instance of the Skype application.

Based off previous work done by Gabriel Szabo and 
published on CodeProject in 2006 including a bug fix 
proposed by user Vlad0 to that same project on 19 Sep 2007
http://www.codeproject.com/Articles/13081/Controlling-Skype-with-C

License or other restrictions:
-----------------------------------------------
None, absolutely none.

Take it and do with it what you like. I'd honestly 
be delighted if anyone would be interested in using 
this code. I only ask that if you do indeed use it 
please consider contributing any enhancements you 
make to it code, design, ideas or thoughts.


How to use:
-----------------------------------------------
There are two main classes:
	SkypeNet.cs
		Provides low level communication with a Skype instance
		only via the windows messaging functions. No understanding
		of the data that is passed to or received from Skype.

		Few points of failures, best suited for debugging or 
		monitoring situations

	SkypeNetClient.cs
		High-level access to a Skype instance. This class understands
		the messages being passed and is aware of protocol support etc

		This is the class most applications that require to interact
		with Skype (e.g. initiate or receive calls, chats, sms etc)

See the SkypeNet.App for examples on how to use these classes.
		