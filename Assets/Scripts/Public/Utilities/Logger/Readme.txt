A simple custom logger class for logging to unity console.

Log Levels-

Debug -->   To Debug info, use to debug the gameplay logic and basic info
Info  -->   Use to highlight all the major events
Warn  -->   Exceptions, Bugs
Error -->   Severe errors causing crash/abort

Assert-->   Basic Assert, Use this to avoid unnecessary null checker or other conditionals


To use the logger,

Syntax-

YCLogger.Debug(this, "Debug Message"); //Preferred way
this.Debug("Debug Message");
YCLogger.Debug("className", "Debug Message"); //For static or singleton

**One also need to #define YCLOGGER in individual classes, but already done at project level.