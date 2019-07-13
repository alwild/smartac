from urllib import request
import random
import datetime
import json
import time


ACCESS_KEY = 'b251fbcc-22b3-402f-820e-a60708c3c5af'
API_URL = 'http://localhost:5000/api/reading'

def random_decimal(start, end):
    return random.randint(start * 100, end * 100)/100

def random_date():
    dateRange = 60 * 24 * 400  #  400 days
    return datetime.datetime.utcnow() - datetime.timedelta(minutes=random.randint(10, dateRange))

def random_choice(choices):
    return choices[random.randint(0, len(choices)-1)]

def mock_readings(access_key):
    for i in range(40):
        readings = [
               {
                   "Temperature": random_decimal(20, 30),
                   "COLevel": random_decimal(0, 10),
                   "HealthStatus": random_choice(["needs_service", "needs_filter", "gas_leak", "online"]),
                   "Humidity": random_decimal(0, 100),
                   "ReadingDateTime": random_date().isoformat()
               } for x in range(random.randint(5, 10))
           ]
           
        params = json.dumps(readings).encode('utf8')
        req = request.Request(API_URL, data=params,
                                 headers={'content-type': 'application/json',
                                          'x-access-key': access_key
                                          })
        response = request.urlopen(req)
        print(response.read().decode('utf8'))
   

if __name__ == '__main__':
    mock_readings(ACCESS_KEY)