import sys
import os

# Add SERVER directory to path
sys.path.append(os.path.abspath('.'))

from kampai_server import create_app

app = create_app(44733)
client = app.test_client()

url = '/rest/dlc/manifests/d7c606f6-44bb-42ab-a655-96d03f1467e0.json'
print(f"Testing URL: {url}")
response = client.get(url)
print(f"Status Code: {response.status_code}")
print(f"Response Data: {response.data.decode('utf-8')[:200]}")
