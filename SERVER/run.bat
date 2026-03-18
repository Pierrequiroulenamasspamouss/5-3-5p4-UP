@echo off
setlocal
cd /d "%~dp0"

:: Check if venv is valid
if not exist venv\Scripts\activate.bat (
    echo [WARN] Virtual environment not found or broken.
    echo Running install.bat to set up the environment...
    call install.bat
    if not exist venv\Scripts\activate.bat (
        echo [ERROR] Setup failed. Cannot start server.
        pause
        exit /b 1
    )
)

echo Starting Kampai Server...
call venv\Scripts\activate.bat
python kampai_server.py
if %errorlevel% neq 0 (
    echo [ERROR] Server crashed or failed to start.
    pause
)
