import json.tool
from pydantic_settings import BaseSettings, SettingsConfigDict
import requests
import json

class HassClient:
    def __init__(self) -> None:
        self.config = HassSettings()
        self.headers = {
        'Content-Type': 'application/json',
        'Authorization': f'Bearer {self.config.token}'
        }

    def call_service(self, service, data):
        url = self.config.url+"services/"+service
        response = requests.post(url, headers=self.headers ,data= json.dumps(data))
        if response.status_code < 200 & response.status_cod >= 300:
            return f'Error: {response.status_code}'

class HassSettings(BaseSettings):
    url: str
    token: str

    model_config = SettingsConfigDict(env_file="settings.env")