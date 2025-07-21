import sys
import json
import time
import csv
import random


def process_data(data: bytes):
    if len(data) > 10000:
        raise Exception(f"Data too long: {len(data)} > 10000")

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
        "staffs": staffs,
    }


def main():
    time.sleep(1)  # loading simulation

    while True:
        try:
            header = sys.stdin.buffer.read(8)
            assert len(header) == 8

            byte_count = int.from_bytes(header[:4], byteorder="little")

            data = sys.stdin.buffer.read(byte_count)
            assert len(data) == byte_count

            reply = process_data(data)

            json.dump(reply, sys.stdout)
            sys.stdout.write("\n")
            sys.stdout.flush()
        except Exception as ex:
            sys.stderr.write(f"{ex}")
            sys.stderr.write("\n")
            sys.stderr.flush()
            sys.stdout.write("Error")
            sys.stdout.write("\n")
            sys.stdout.flush()


main()
