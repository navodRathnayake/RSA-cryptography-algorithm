# RSA-cryptography-algorithm
The following program has developed for the software security cw.

[*]THERE ARE TWO METHODS TO RUN THIS APPLICATION
	[1] RUN THE APPLICATION USING CMD
	[2] RUN THE APPLICATION USING VISUAL STUDIO

[METHOD-1] 
	[1]- find the RSA.exe (CW Program\RSA\RSA\bin\Debug\netcoreapp3.1)
	[2]- open cmd from above file path
	[3]- run the command (RSA.exe public.pem your_source.txt your_cipher.txt)

[METHOD-2]
	[1]- open the project from visual studio
	[2]- add parameters (Properties->Debug->application arguments)
	[3]- run the project

[NOTE]
------------------------------------------------------------------------------------
[*]- video link 
https://drive.google.com/drive/folders/1y24faxg0nm2VgbXEJEHrP1XYCyWjgYkf?usp=sharing

[*]- Language : C#
------------------------------------------------------------------------------------

[Course Work Details]

For this assignment you should implement a pair of programs to encrypt and decrypt text files using the
RSA public-key cryptography algorithm. The purpose of the assignment is to gain some experience
working with files and programming, and to gain an appreciation for some of the technical issues
involved in information security and privacy.
This write up gives a fairly complete specification of what you need to do. The actual programming will
not require much coding, although it may take some time to analyze the problem first. You should work
independently on this assignment (no partner) although, of course, you should feel free to discuss
algorithms and general ideas with your colleagues.
As always, your code will be evaluated both for how well it meets the requirements and how well it is
written - structure and comments/layout/clarity.
Requirements
You should create two separate programs:
• Program Encrypt should read a key file and a character file, encrypt the character data using the
keys, and write the results to a separate output file.
• Program Decrypt should read a key file and a file of encrypted data, decrypt the data using the
keys, and write the resulting character data to a separate output file.
Both programs should be main methods in appropriately named classes (Encrypt and Decrypt). We will
provide some classes that you should find useful [1], and you can define additional classes if you need to.
Both programs should get the file names to be used from the String array argument in method main. The
arguments, in order, are
• args[0] - The name of the file containing the keys
• args[1] - The name of the source file
• args[2] - The name of the destination file
