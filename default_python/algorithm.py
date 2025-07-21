import hashlib
import csv
import random


def run(data: bytes):
    if len(data) > 10000:
        raise Exception(f"Data too long: {len(data)} > 10000")

    data_md5 = hashlib.md5(data)

    staffs = []
    with open("default_python/dummy_data.csv", "r", encoding="utf-8") as csvfile:
        reader = csv.DictReader(csvfile, delimiter=",")
        for row in random.sample(list(reader), 5):
            staffs.append(
                {
                    "id": int(row["id"]),
                    "first_name": row["first_name"],
                    "last_name": row["last_name"],
                    "email": row["email"],
                }
            )

    return {
        "data_length": len(data),
        "md5": data_md5.hexdigest(),
        "staffs": staffs,
    }
