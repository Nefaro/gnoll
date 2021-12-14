@echo off
SET py=py

py --version 2>NUL 1>NUL
IF ERRORLEVEL 1 (
    GOTO :checkPy
) ELSE (
    GOTO :pyOk
)
:checkPy
python --version 2>NUL 1>NUL
IF ERRORLEVEL 1 (
    ECHO Could not find python. Install it from here: https://www.python.org/downloads/
    EXIT /B
) ELSE (
    SET py=py
    GOTO :pyOk
)

:pyOk
%py% gnomodkit.py %*