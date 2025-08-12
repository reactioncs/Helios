import hashlib
import json
import time


class Algorithm:
    def __init__(self) -> None:
        time.sleep(1)  # loading simulation

    def run(self, data: bytes) -> bytes:
        if len(data) > 1500:
            raise Exception(f"Data too long: {len(data)} > 1500\nSecond line.")

        input = json.loads(data)

        data_md5 = hashlib.md5(data)

        city_measures: dict[str, list[float]] = {}
        for weather_measure in input["weather_measures"]:
            city = weather_measure["city"]
            temperature = float(weather_measure["temperature"])

            if city not in city_measures:
                city_measures[city] = []

            city_measures[city].append(temperature)

        summaries = []
        for city, measures in city_measures.items():
            summaries.append(
                {
                    "city": city,
                    "average_temperature": sum(measures) / len(measures),
                }
            )

        reply_json = {
            "md5": data_md5.hexdigest(),
            "summaries": summaries,
        }

        return json.dumps(reply_json).encode("utf-8")
