import hashlib
import json
import csv
import random
import time


class Algorithm:
    def __init__(self) -> None:
        time.sleep(1)  # loading simulation

    def run(self, data: bytes) -> bytes:
        if len(data) > 1500:
            raise Exception(f"Data too long: {len(data)} > 1500\nSecond line.")

        data_md5 = hashlib.md5(data)

        staffs = []
        with open("default_python/dummy_data.csv", "r", encoding="utf-8") as csv_file:
            reader = csv.DictReader(csv_file, delimiter=",")
            for row in random.sample(list(reader), 5):
                staffs.append(
                    {
                        "id": int(row["id"]),
                        "first_name": row["first_name"],
                        "last_name": row["last_name"],
                        "email": row["email"],
                    }
                )

        reply_json = {
            "data_length": len(data),
            "md5": data_md5.hexdigest(),
            "staffs": staffs,
        }

        return json.dumps(reply_json).encode("utf-8")
