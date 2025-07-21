import sys
import json
import random
import time

dummy_staffs = [
    (1, "Dasya", "Bondesen", "dbondesen0@sbwire.com"),
    (2, "Meredeth", "Holhouse", "mholhouse1@ustream.tv"),
    (3, "Adelle", "Malser", "amalser2@google.cn"),
    (4, "Nikki", "Sidebotham", "nsidebotham3@mail.ru"),
    (5, "Ellsworth", "Thistleton", "ethistleton4@facebook.com"),
    (6, "Sabrina", "Pallant", "spallant5@shinystat.com"),
    (7, "Melany", "Shrigley", "mshrigley6@1und1.de"),
    (8, "Maynord", "Snarr", "msnarr7@prlog.org"),
    (9, "Laetitia", "Billyard", "lbillyard8@over-blog.com"),
    (10, "Greg", "Wherton", "gwherton9@blogspot.com"),
]


def process_image(data: bytes):
    if len(data) > 10000:
        raise Exception(f"Data too long: {len(data)} > 10000")

    staffs = []
    for id, first_name, last_name, email in dummy_staffs:
        staffs.append(
            {
                "id": id,
                "first_name": first_name,
                "last_name": last_name,
                "email": email,
            }
        )

    return {
        "data_length": len(data),
        "staffs": staffs,
    }


def main():
    time.sleep(2)  # loading simulation

    while True:
        try:
            header = sys.stdin.buffer.read(8)
            assert len(header) == 8

            byte_count = int.from_bytes(header[:4], byteorder="little")

            data = sys.stdin.buffer.read(byte_count)
            assert len(data) == byte_count

            reply = process_image(data)

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
