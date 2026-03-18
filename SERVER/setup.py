from setuptools import setup, find_packages

setup(
    name="kampai-server",
    version="1.0.0",
    packages=find_packages(),
    install_requires=[
        "flask",
    ],
    entry_points={
        "console_scripts": [
            "kampai-server=kampai_server:run_server",
        ],
    },
)
