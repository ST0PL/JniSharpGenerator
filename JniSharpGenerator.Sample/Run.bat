@echo off
:: 1. Build the generator tool
dotnet build ..\JniSharpGenerator -c Release

:: 2. Compile the Java source code
javac -d . Java\Sample.java

:: 3. Package the Java class into a JAR file
jar cvf Sample.jar JniSharpGenerator
rd /s /q JniSharpGenerator

:: 4. Generate C# JNI wrappers using the built generator
..\JniSharpGenerator\bin\x64\Release\JniSharpGenerator.exe -a -s -d . -cp Sample.jar JniSharpGenerator.Sample.Sample

:: 5. Publish the Sample project as a native binary (NativeAOT)
dotnet publish -c Release -r win-x64

:: 6. Run the Java application with the C# NativeAOT agent attached
java -agentpath:bin\x64\Release\JniSharpGenerator.Sample.dll -cp Sample.jar JniSharpGenerator.Sample.Sample

:: 7. Cleanup temporary JAR
del Sample.jar