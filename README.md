# Intro
I enjoyed modding games long before I realized I could actually make brand new ones. Probably for that reason making modable games interest me a lot. This is my take on a modable developer console.<br/>
<br/>
This developer console uses Lua for scripting and MoonSharp as interpreter.<br/>
Refer to https://www.moonsharp.org/getting_started.html. <br/>

# How it works
 - DeveloperConsole.cs loads the Lua script located at StreamingAssets and then exposes desired classes/functions.
 - DeveloperConsole.cs parses the given string by the player and calls the given function name
 - If the Lua script has a function with given name, function gets evaluated with given params
