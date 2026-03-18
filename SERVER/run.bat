@echo off
if not exist venv (
    echo Virtual environment not found. Running install.bat first...
    call install.bat
)

echo Starting Kampai Server...
call venv\Scripts\activate.bat
python kampai_server.py
pause
