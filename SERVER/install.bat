@echo off
echo Setting up Kampai Server...

:: Check for python
python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Python not found! Please install Python from python.org
    pause
    exit /b 1
)

:: Create virtual environment
if not exist venv (
    echo Creating virtual environment...
    python -m venv venv
)

:: Activate venv and install requirements
echo Installing dependencies...
call venv\Scripts\activate.bat
pip install -r requirements.txt

echo.
echo Setup complete!
echo To run the server, use: run.bat
echo.
pause
