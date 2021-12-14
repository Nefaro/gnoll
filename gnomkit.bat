@echo off
SET py=py

py --version 2>NUL 1>NUL
IF ERRORLEVEL 1 (
    GOTO :checkPython
) ELSE (
    GOTO :pyOk
)
:checkPython
python --version 2>NUL 1>NUL
IF ERRORLEVEL 1 (
    ECHO Could not find python. Install it from here: https://www.python.org/downloads/
    EXIT /B
) ELSE (
    SET py=python
    GOTO :pyOk
)

:pyOk
%py% gnomodkit.py %*