import sys

from algo import Algorithm


def main():
    algorithm = Algorithm()

    while True:
        try:
            header = sys.stdin.buffer.read(8)
            assert len(header) == 8

            data_length = int.from_bytes(header[:4], byteorder="little")

            data = sys.stdin.buffer.read(data_length)
            assert len(data) == data_length

            reply_data = algorithm.run(data)

            reply_header = bytearray([0x00] * 8)
            reply_header[:4] = len(reply_data).to_bytes(length=4, byteorder="little")

            sys.stdout.buffer.write(reply_header)
            sys.stdout.flush()
            sys.stdout.buffer.write(reply_data)
            sys.stdout.flush()
        except Exception as ex:
            error_data = str(ex).encode("utf-8")
            error_header = bytearray([0x00] * 8)
            error_header[:4] = len(error_data).to_bytes(length=4, byteorder="little")

            sys.stderr.buffer.write(error_header)
            sys.stderr.flush()
            sys.stderr.buffer.write(error_data)
            sys.stderr.flush()

            reply_header = bytearray([0x00] * 8)
            reply_header[:4] = (4).to_bytes(length=4, byteorder="little")
            reply_data = bytes([0xCD, 0xBC, 0xD0, 0xAC])

            sys.stdout.buffer.write(reply_header)
            sys.stdout.flush()
            sys.stdout.buffer.write(reply_data)
            sys.stdout.flush()


if __name__ == "__main__":
    main()
