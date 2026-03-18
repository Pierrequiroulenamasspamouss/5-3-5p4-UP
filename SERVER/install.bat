@echo off
setlocal
echo Setting up Kampai Server...

:: Check for python
python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] Python not found! Please install Python 3.x from python.org
    pause
    exit /b 1
)

:: Force recreate venv if it doesn't have activate.bat
if exist venv (
    if not exist venv\Scripts\activate.bat (
        echo [WARN] Existing venv is incomplete or broken. Removing...
        rd /s /q venv
    )
)

:: Create virtual environment
if not exist venv (
    echo Creating virtual environment...
    python -m venv venv
    if %errorlevel% neq 0 (
        echo [ERROR] Failed to create virtual environment.
        pause
        exit /b 1
    )
)

:: Activate venv and install requirements
echo Installing dependencies...
if not exist venv\Scripts\activate.bat (
    echo [ERROR] venv\Scripts\activate.bat is still missing. 
    echo Please try running: python -m venv venv --clear
    pause
    exit /b 1
)

call venv\Scripts\activate.bat
python -m pip install --upgrade pip
pip install -r requirements.txt

if %errorlevel% neq 0 (
    echo [ERROR] Failed to install dependencies.
    pause
    exit /b 1
)

echo.
echo Setup complete!
echo To run the server, use: run.bat
echo.
pause
