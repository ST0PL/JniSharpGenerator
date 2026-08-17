@echo off
:: 1. Publish the generator tool (NativeAOT)
dotnet publish ..\JniSharpGenerator.CLI -c Release -r win-x64

:: 2. Compile the Java source code
javac -d . java\JavaSample.java

:: 3. Package the Java class into a JAR file
jar cvf JavaSample.jar JniSharpGenerator
rd /s /q JniSharpGenerator

:: 4. Generate C# JNI wrappers using the built generator
..\JniSharpGenerator.CLI\bin\x64\Release\JniSharpGenerator.exe -a -s -d . -cp JavaSample.jar JniSharpGenerator.Sample.JavaSample

:: 5. Publish the Sample project as a native binary (NativeAOT)
dotnet publish -c Release -r win-x64

:: 6. Run the Java application with the C# NativeAOT agent attached
java -agentpath:bin\x64\Release\JniSharpGenerator.Sample.dll -cp JavaSample.jar JniSharpGenerator.Sample.JavaSample

:: 7. Cleanup temporary JAR
del JavaSample.jar