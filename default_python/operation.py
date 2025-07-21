import sys
import json
import time

import algorithm


def main():
    time.sleep(1)  # loading simulation

    while True:
        try:
            header = sys.stdin.buffer.read(8)
            assert len(header) == 8

            byte_count = int.from_bytes(header[:4], byteorder="little")

            data = sys.stdin.buffer.read(byte_count)
            assert len(data) == byte_count

            reply = algorithm.run(data)

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
